#  This PowerShell Script is based on this post http://www.codeproject.com/Articles/223002/Reboot-and-Resume-PowerShell-Script from Lasse W

#Region "Set global variables"



$global:ScriptLocation = $myInvocation.MyCommand.Definition


#endregion

#Region "Reboot Functions"

# -------------------------------------
# Function Globals
# -------------------------------------
$global:started = $FALSE
$global:startingStep = $Step
$global:restartKey = "Aria-Machine-Setup"
$global:RegRunKey ="HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
$global:RegWinlogon = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
$global:powershell = (Join-Path $env:windir "system32\WindowsPowerShell\v1.0\powershell.exe")

#region Private_Functions
# -------------------------------------
# Collection of Utility functions.
# -------------------------------------
function private:Run-Step([string] $prospectStep) 
{
	if ($global:startingStep -eq $prospectStep -or $global:started) {
		$global:started = $TRUE
	}
	return $global:started
}

function private:Test-Key([string] $path, [string] $key)
{
    return ((Test-Path $path) -and ((Get-Key $path $key) -ne $null))   
}

function private:Remove-Key([string] $path, [string] $key)
{
	Remove-ItemProperty -path $path -name $key
}

function private:Set-Key([string] $path, [string] $key, [string] $value) 
{
	Set-ItemProperty -path $path -name $key -value $value
}

function private:Get-Key([string] $path, [string] $key) 
{
	return (Get-ItemProperty $path).$key
}

function private:Restart-And-Run([string] $key, [string] $run) 
{
	Set-Key $global:RegRunKey $key $run
	#Restart-Computer -Force
	#exit
}
 
#endregion Private_Functions

#Region Main_Calling_Function
function Clear-Any-Restart([string] $key=$global:restartKey) 
{
	if (Test-Key $global:RegRunKey $key) {
		Remove-Key $global:RegRunKey $key
	}
}

#Update Registry Run key to execute the specified PowerShell script with the specified command 
#line parameter when the computer restarts.
#The Parameter is -Step #.  Where # is the step # to run next.
function Restart-And-Resume([string]$Script, [string]$Parameter)
{
	Restart-And-Run -key $global:restartKey -run "$global:powershell -File ""$Script"" $Parameter"
}

#Enables Windows to autologon when restarted using the specified credentials.
#If no credentials are specified, the user is prompted to enter the 
#administrator credentials that the script will run on after it reboots
#in order to continue processing the subsequent steps in the script.
function SetAutoLogin([string]$user = "$env:USERNAME", [string]$password="") 
{   
    $MyPassword = $null
    #$env:USERDOMAIN\
    #Write-Host "user=$user;"
    #Write-Host "pass=$password;"

    if($password -eq "") {
        $credential = $host.ui.PromptForCredential("Dev Machine Automatic Install", "Enter Credentials for Automatic Restart Authentication:", $user, "NetBiosUserName")
        #use temp user so we can read the password
        $TempUser = New-Object System.Management.Automation.PSCredential("temp", $credential.Password)
        $MyPassword = $TempUser.GetNetworkCredential().Password
    }
    else {
        $MyPassword = $password
    }

    # Autologin so the runonce is run
    Set-ItemProperty -Path $global:RegWinlogon -Name DefaultUserName -Value $user
    Set-ItemProperty -Path $global:RegWinlogon -Name DefaultPassword -Value $MyPassword
    Set-ItemProperty -Path $global:RegWinlogon -Name AutoAdminLogon -Value "1"
    Set-ItemProperty -Path $global:RegWinlogon -Name ForceAutoLogon -Value "1"

    return $MyPassword
}

#Removes the Windows automatic logon configuration.
function RemoveAutoLogin()
{
    # Remove the runonce
    Remove-ItemProperty -Path $global:RegWinlogon DefaultUserName
    Remove-ItemProperty -Path $global:RegWinlogon DefaultPassword
    Set-ItemProperty -Path $global:RegWinlogon -Name AutoAdminLogon -Value "0"
    Set-ItemProperty -Path $global:RegWinlogon -Name ForceAutoLogon -Value "0"
} 
#endregion Main_Calling_Function


#region Custom_Functions
function Restart-Call($cutomOutput)
{
	Clear-Any-Restart
	if (Run-Step "PRE")
	{
		CustomPreRestartActions $cutomOutput			
		Restart-And-Resume $global:ScriptLocation "POST"
        exit
	}

	if (Run-Step "POST")
	{
		CustomRestartActions $cutomOutput
        RemoveAutoLogin
	}
}

function CustomPreRestartActions([string]$outputStr="Empty")
{
   Write-Host "  - Add your custom logic before the restart is performed: " + $outputStr -BackgroundColor Green
}

function CustomRestartActions([string]$outputStr="Empty")
{
   	Write-Host "  - Add your custom logic for the restart: " + $outputStr -BackgroundColor White -ForegroundColor Green
	Write-Host " PowerShell CustomRestartActions script is running, press any key to exit script..." -BackgroundColor White -foregroundcolor Green
	$key = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
#endregion Custom_Functions

