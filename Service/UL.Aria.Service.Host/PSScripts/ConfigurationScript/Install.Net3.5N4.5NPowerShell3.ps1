<#
.SYNOPSIS 
	Installs .Net Framework version xyz and other prequisite software

.PARAMETER SoftwareDirectory
	Specifies directory to check for install files.

.DESCRIPTION
	Installs Powershell 3.0
	Installs hotfixes.

	Checks to see if the software is installed prior to attempting to install.
	Installs software based on the version of Windows Server
	If the install files needed aren't in the SoftwareDirectory, then it downloads them.
	This allows you to stage the hotfixes ahead time instead of having to download them 
	from microsoft.com everytime, a new environment is built.
	
	If you are running Windows Server 2012, then you don't need the hotfixes and software
	that this script will install.
#>
param
(
	[Parameter(Mandatory=$false)][String]$ServerRole,
    [Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName,
	[Parameter(Mandatory=$false)][String]$SoftwareDirectory	
)

Set-StrictMode -Version 2

#Region External Functions
	#Since, this is PowerShell v2, we can't use $PSScriptRoot
	$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
	. (Join-Path -Path $path -ChildPath "Common\ConfigurationScriptFunctions.ps1")
	#$InputFile = (Join-Path -Path $path -ChildPath "Restart-Functions.ps1")
	#$restartRequired = $false;
#EndRegion External Functions


#region 0.Install.Net 3.5\4.5\PowerShell 3
#Description of Windows Management Framework 3.0 for Windows 7 SP1 and Windows Server 2008 R2 SP1
#http://support.microsoft.com/kb/2506143

# Description of Windows Management Framework 3.0 for Windows Server 2008 SP2
# http://support.microsoft.com/kb/2506146/en-us

# An application that uses DirectWrite does not start in a restricted security context in Windows 7 or in Windows Server 2008 R2
# http://support.microsoft.com/kb/2592525/en-us
Function Install-OSCNetPSKB ([Parameter(Mandatory=$true)][String]$SoftwareDirectory)
{
	$KBURLs  = 	"http://download.microsoft.com/download/5/E/A/5EA7504B-E2B3-43A1-8279-892E007D50A0/Windows6.1-KB2592525-x64.msu",`
				"http://download.microsoft.com/download/E/7/6/E76850B8-DA6E-4FF5-8CCE-A24FC513FD16/Windows6.1-KB2506143-x64.msu",`
				"http://download.microsoft.com/download/E/7/6/E76850B8-DA6E-4FF5-8CCE-A24FC513FD16/Windows6.0-KB2506146-x86.msu",`
				"http://download.microsoft.com/download/E/7/6/E76850B8-DA6E-4FF5-8CCE-A24FC513FD16/Windows6.0-KB2506146-x64.msu"
	$KBFiles = 	"Windows6.1-KB2592525-x64.msu",`
				"Windows6.1-KB2506143-x64.msu",`
				"Windows6.0-KB2506146-x86.msu",`
				"Windows6.0-KB2506146-x64.msu"
	
	$KBDirectory = $SoftwareDirectory
	
	#Check the OS version
	switch(GetSystemInfo)
	{
		1 	
		{ 
			InstallNetFx $SoftwareDirectory
			#InstallKB $KBURLs[0] $KBPaths[0] "KB2592525"
			InstallKB $KBURLs[1] $KBDirectory $KBFiles[1] "KB2506143"
		}
		2 	
		{
			InstallNetFx $SoftwareDirectory
			#InstallKB $KBURLs[0] $KBPaths[0] "KB2592525"   
			InstallKB $KBURLs[2] $KBDirectory $KBFiles[2] "KB2506146"
		}
		3 
		{	
			InstallNetFx $SoftwareDirectory
			#InstallKB $KBURLs[0] $KBPaths[0] "KB2592525"   
			InstallKB $KBURLs[3] $KBDirectory $KBFiles[3] "KB2506146"
		}
		0 	
		{ 
			Write-Warning "Please run this script on Windows Server 2008 sp2 or 2008 R2"
		}
	}
      
}
Function GetSystemInfo
{	
	#This function is to get some information about the current OS
	$SystemInfo = Get-WmiObject -Class "win32_operatingsystem" | Select-Object -Property Caption,CSDVersion,OSArchitecture
	If($SystemInfo.Caption -match "Microsoft Windows Server 2008 R2")
	{
		#If the system version is Windows Server 2008 R2, it will return 1
		Return 1
	}
	Elseif($SystemInfo.Caption -match "Microsoft Windows Server 2008")
	{
		If( $SystemInfo.CSDVersion -match "Service Pack 2")
		{	
			If($SystemInfo.OSArchitecture -match "32-bit")
			{
				#If the system version is Windows Server 2008 SP2(32-bit), it will return 2
				Return 2
			}
			Else 
			{	
				#If the system version is Windows Server 2008 SP2(64-bit), it will return 3
				Return 3
			}
		}
		Else 
		{
			#If it is other version ,it will return 0
			Return 0
		}
	}
	Else
	{
		#If it is other version ,it will return 0
		Return 0
	}
}

Function InstallNetFx([Parameter(Mandatory=$true)][String]$SoftwareDirectory)
{	
    #The below two lines install .net35
	 Import-Module ServerManager
     $result = Add-WindowsFeature as-net-framework
	
	if ($result.RestartNeeded.ToString() -ne "No")
	{
		Write-Host "Restart needed. (as-net-framework)"
	}
	
	#This function is to download and install .Net framework 4.5 
	$NetFxURL = "http://download.microsoft.com/download/B/A/4/BA4A7E71-2906-4B2D-A0E1-80CF16844F5F/dotNetFx45_Full_setup.exe"
	$NetFxPath = $SoftwareDirectory + "\dotNetFx45_Full_setup.exe"
	#Verify if the .Net Framework 4.5 is installed
 	If((Get-WmiObject -Class "win32_product" | Where-Object {$_.name -match "Microsoft .NET Framework 4.5"}) -eq $null)
	{	
		try
		{
			#Verify if the file exists
			If(Test-Path -Path $NetFxPath)
			{	
				Write-Host "Installing Microsoft .NET Framework 4.5"
				#Install .Net Framework 4.5
				$process = (Start-Process -FilePath $NetFxPath -ArgumentList "/q /norestart" -Wait -Verb RunAs -PassThru)
			}
			Else 
			{	
				try
				{
					#Download .Net Framework 4.5
					Write-Host "Downloading Microsoft .NET Framework 4.5"
					$WebClient = New-Object System.Net.WebClient
					$WebClient.DownloadFile($NetFxURL,$NetFxPath)
					If(Test-Path -Path $NetFxPath)
					{	
						Write-Host "Installing Microsoft .NET Framework 4.5 "
						#Install .Net Framework 4.5
						$process = (Start-Process -FilePath $NetFxPath -ArgumentList "/q /norestart" -Wait -Verb RunAs -PassThru)
					}
					Else
					{
						Write-Warning "Failed to download Microsoft .NET Framework 4.5."
					}
				}
				catch
				{
					Write-Host "Failed to download Microsoft .NET Framework 4.5."
					write-Host ("Exception:  " + $Error[0].Exception)
				}
			}
			
			#Verify if .Net Framework 4.5 is installed successfully
			If(Get-WmiObject -Class "win32_product" | Where-Object {$_.name -match "Microsoft .NET Framework 4.5"})
			{
				Write-Host "Install  Microsoft .NET Framework 4.5 Successfully"
			}
			Else 
			{
				Write-Host "Failed to download Microsoft .NET Framework 4.5,you can find it in $$NetFxPath and install it manually."
			}
		}
		catch
		{
			Write-Host "Failed to download Microsoft .NET Framework 4.5."
			write-Host ("Exception:  " + $Error[0].Exception)
		}
	}
	Else
	{	
		Write-Host "Microsoft .NET Framework 4.5 has been installed."
	}

}

Function InstallKB([Parameter(Mandatory=$true)][String]$KBURl, [Parameter(Mandatory=$true)][String]$KBDirectory, [Parameter(Mandatory=$true)][String]$KBFile,[Parameter(Mandatory=$true)][String]$KBId)
{
	#This function is to download and install KB
	#Verify if the specified KB is installed 
	If((Get-HotFix | Where-Object {$_.HotFixId -eq $KBId}) -eq $null )
	{
		$KBPath = $KBDirectory + "\" + $KBFile
		#Verify if the file exists
    	If(Test-Path -Path $KBPath)
        {
			#Execute the KB
		    wusa.exe $KBPath /quiet /norestart 
			#Pause PowerShell when insatlling KB
			do
			{
				Start-Sleep -Seconds 10
			}while(Get-Process | Where-Object {$_.name -eq "wusa"})
        }
        Else 
        {
			try
			{
				#Download KB
				Write-Host "Downloading $KBId"
				$WebClient = New-Object System.Net.WebClient
				$WebClient.DownloadFile($KBURl,$KBPath)
				#Verify if the file exists
				If(Test-Path -Path $KBPath)
				{	
					Write-Host "Installing $KBId"
					wusa.exe $KBPath /quiet /norestart 
					do
					{
						Start-Sleep -Seconds 10
					}while(Get-Process | Where-Object {$_.name -eq "wusa"})
				}
				Else 
				{
					Write-Warning "Failed to download $KBId."
				}
				#Verify if KB is installed successfully
				If(Get-HotFix | Where-Object {$_.HotFixId -eq $KBId} )
				{
					Write-Host "Install $KBId successfully"
				}
				Else 
				{
					Write-Host "Failed to install $KBId,you can find it in $KBPath and install it manually. "
				}		
			}
			catch
			{
				Write-Host "Failed to install $KBId"
				Write-Host ("Exception:  " + $Error[0].Exception)
			}
        }
		
	}
	Else
	{
		Write-Host "$KBId has been installed"
	}			
}

function Main(
[Parameter(Mandatory=$false)][String]$ServerRole,
[Parameter(Mandatory=$false)][string]$EnvironmentName, 
[Parameter(Mandatory=$false)][String]$SoftwareDirectory,
[Parameter(Mandatory=$false)][String]$GlobalConfigurationFileName
)
{
	$EnvironmentConfigurationFileName=$null
	$ServerName = $env:computername

    $GlobalConfigurationFileName = Ensure-EnvironmentFile $GlobalConfigurationFileName
    $GlobalConfiguration = Get-GlobalConfiguration -InputFile $GlobalConfigurationFileName

    $EnvironmentName = Ensure-EnvironmentName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles

    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles
    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName

    $ServerRole = Ensure-DeploymentTier -DeploymentTier $ServerRole -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration

	if ([string]::IsNullOrEmpty($SoftwareDirectory))
	{
		$SoftwareDirectory = (join-path $GlobalConfiguration.SoftwareDirectory "HotFixes")
	}
	
	Install-OSCNetPSKB $SoftwareDirectory
}

Main -ServerRole $ServerRole -SoftwareDirectory $SoftwareDirectory -GlobalConfigurationFileName $GlobalConfigurationFileName
