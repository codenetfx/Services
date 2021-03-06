<# 
.SYNOPSIS
Creates all of the IIS Web Sites for a particular ServerRole on a single server.

.PARAMETER ServerRole
	Type type of server the script is running on.
	
.DESCRIPTION
Includes: Directories, IIS Sites & App Pools
#>
param(
[Parameter(Mandatory=$false)][bool]$CreateProbeSite=$false,
[Parameter(Mandatory=$false)][string]$ServerRole,
[Parameter(Mandatory=$false)][string]$EnvironmentName,
[Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName
)

Set-StrictMode -Version 2

#Region External Functions
		. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

#region Internal_Functions

function ContinuePrompt($caption,$message)
{
    $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes",""
    $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No",""
    $choices = [System.Management.Automation.Host.ChoiceDescription[]]($yes,$no)

    $result = $Host.UI.PromptForChoice($caption,$message,$choices,0)
    
    return $result -eq 0
}


function CreateAppPool 
{
	param(
	[Parameter(Mandatory=$true)][string]$Site,
	[Parameter(Mandatory=$true)][string]$SvcAcctDomain,
	[Parameter(Mandatory=$true)][string]$svcAcctName,
	[Parameter(Mandatory=$true)][string]$svcAcctPassword,
	[Parameter(Mandatory=$true)][string]$DotNetRunTimeVersion
	 )
    
	# Loading IIS Modules
	Import-Module WebAdministration
				
	$poolName   = "$Site"

    $createAppPool = $false
    if ((test-path IIS:\AppPools\$poolName)) 
    {
        $createAppPool = ContinuePrompt -caption "Confirm App Pool Creation" -message "The $poolName already exists.  Do you want to create it again?"
    }
    else
    {
        $createAppPool = $true
    }

	#If the IIS App Pool doesn't exist, create it.
	if ($createAppPool) 
	{
        Write-Host "Creating $poolName app pool"
        
		New-WebAppPool -name $poolName -Force
		Set-ItemProperty IIS:\AppPools\$poolName -name managedPipelineMode  -value 0 ### 0=Integrated, 1=Classic 
		Set-ItemProperty IIS:\AppPools\$poolName -name processModel -value @{userName="$SvcAcctDomain\$svcAcctName";password="$svcAcctPassword";identitytype=3}
		Set-ItemProperty IIS:\AppPools\$poolName -name managedRuntimeVersion -value $DotNetRunTimeVersion
	}
    else
    {
        Write-Host "Skipping creation of $poolName app pool"

    }

}

# Creates an IIS Web Site using Basic and Windows Authentication.
# Uses an App Pool with the same name as the site.
function CreateWebsite 
{
	param(
	[Parameter(Mandatory=$true)][int]$Port,
	[Parameter(Mandatory=$true)][String]$Site,
	[Parameter(Mandatory=$true)][String]$sitePath,
	[Parameter(Mandatory=$true)][String]$IISLogDirectory
	)
		  
	# Loading IIS Modules
	Import-Module WebAdministration	


    $createWebSite = $false
    if ((test-path IIS:\Sites\$Site)) 
    {
        $createWebSite = ContinuePrompt -caption "Confirm Web Site Creation" -message "The $Site already exists.  Do you want to create it again?"
    }
    else
    {
        $createWebSite = $true
    }

    if($createWebSite)
    {
        Write-Host "Creating $Site web site"

	    [int]$portSvc = $Port
	    $poolName   = "$Site"
	
	    $logPath	= "$IISLogDirectory\$Site"
	    New-Item -Path $logPath -type directory -Force -ErrorAction SilentlyContinue
	
	
	    $websitePath	= "$sitePath\$Site\"
	    New-Item -Path $websitePath -type directory -Force -ErrorAction SilentlyContinue
	
	    $portSvcBinding="$portSvc"+":*"   # powershell eats the colon during variable substitution if used inline
	    write-host($portSvcBinding)
	
	    New-Website -name $Site -PhysicalPath $sitePath -port $Port -id $Port -force
	    New-ItemProperty IIS:\Sites\$Site -name bindings -value @{protocol='net.tcp';bindingInformation= $portSvcBinding} -force
	    Set-ItemProperty IIS:\Sites\$Site -name logFile  -value @{directory="$logPath"}
	    Set-ItemProperty IIS:\Sites\$Site -name applicationPool -value $poolName 
	    Set-ItemProperty IIS:\Sites\$Site -name serverAutoStart -value $true -Force
	
	    Set-WebConfigurationProperty -filter "/system.applicationHost/sites/site[@name='$Site' and @id='$Port']/application[@path='/']" -name enabledProtocols -value 'http,net.tcp' -pspath IIS:\
	    Set-WebConfigurationProperty -filter //basicAuthentication -PSPath IIS:\ -name enabled -Value false -Location $Site
	    Set-WebConfigurationProperty -filter //windowsAuthentication -PSPath IIS:\ -name enabled -Value false -Location $Site
    }
    else
    {
        Write-Host "Skipping creation of $Site web site"
    }

}

#Creates the Probe web site that's used with the Azure Load Balancer probes.
# Each port bound to this site corresponds to a probe port in Azure which in turn is tied to an internal load balanced port.
function CreateProbeWebSite([int[]]$ProbePorts, [string]$Site)
{
    
    CreateWebSite -Port $ProbePorts[0] -Site "Probe" -SitePath "C:\Inetpub\wwwroot\Probe" -IISLogDirectory "C:\inetpub\logs\LogFiles\Probe"

    CD "IIS:\Sites\$Site"

    $i=1
  
    While ($i -lt $ProbePorts.Count)
    {
        New-WebBinding -Protocol http -Port $ProbePorts[$i]
        $i++
    }

    CD C:
}


#endregion Internal_Functions


function Main(
    [Parameter(Mandatory=$true)][bool]$CreateProbeSite,
	[Parameter(Mandatory=$false)][string]$ServerRole,
	[Parameter(Mandatory=$false)][string]$EnvironmentName,
    [Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName
)
{

    
	$EnvironmentConfigurationFileName=$null
	$ServerName = $env:computername

    $GlobalConfigurationFileName = Ensure-EnvironmentFile $GlobalConfigurationFileName
    $GlobalConfiguration = Get-GlobalConfiguration -InputFile $GlobalConfigurationFileName
    Write-Debug "Calling Ensure-EnvironmentName"
    $EnvironmentName = Ensure-EnvironmentName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles
    Write-Debug "Calling Ensure-EnvironmentConfigurationFileName"
    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles
    Write-Debug "Calling Get-EnvironmentConfiguration"
    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles

    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName

    $ServerRole = Ensure-DeploymentTier -DeploymentTier $ServerRole -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration

	[string]$SvcAcctDomain = Get-WindowsNtDomain
	[string]$svcAcctName = $EnvironmentConfiguration.IisSiteUserName
	[string]$svcAcctPassword = $EnvironmentConfiguration.IisSiteUserPwd
	[string]$DotNetRunTimeVersion = $GlobalConfiguration.DotNetRunTimeVersion
	[string]$IISLogDirectory = $GlobalConfiguration.IISLogDirectory
	[string]$IISWebSiteRootDirectory = $GlobalConfiguration.IISWebSiteRootDirectory

	#Validate Parameter
	if (!($ServerRole -in @("Portal", "Services", "OnPrem", "SPAPP", "SPAPP")))
	{
		Write-Host "Invalid ServerRole $ServerRole specified."
		exit -1
	}
	
	# Register ASP.Net 4 mappings in IIS on current server
	#$mappingScriptPath = Join-Path -Path (Split-Path $MyInvocation.MyCommand.Path -Parent) -ChildPath "Common\InstallAspDotNet4IisMappings.ps1"
    $mappingScriptPath ="$PSScriptRoot\Common\InstallAspDotNet4IisMappings.ps1"
	Invoke-Expression $mappingScriptPath

	if ($ServerRole -eq "Portal")
	{
		$WebSitesArray = $EnvironmentConfiguration.PortalWebSites
	}

	if ($ServerRole -eq "Services")
	{
		$WebSitesArray = $EnvironmentConfiguration.AzureServicesWebSites
	}
	
	if ($ServerRole -eq "OnPrem")
	{
		$WebSitesArray = $EnvironmentConfiguration.OnPremServicesWebSites
	}

	New-Item  -Path $IISWebSiteRootDirectory -type directory -Force -ErrorAction SilentlyContinue
	New-Item  -Path $IISLogDirectory -type directory -Force -ErrorAction SilentlyContinue

    if ($CreateProbeSite)
    {
    
        $Site = "Probe"

        #Creates the Probe IIS site for the Azure Load Balancer.
        #This site is created on each of the Portal/Services servers.
        Write-Output "Creating IIS Probe Site."
        CreateProbeWebSite -Site $Site -ProbePorts @(8880, 8443, 8802, 8803, 8804, 8805)
    }
    else
    {
	foreach ($WebSite in $WebSitesArray)
	{
        if ([string]::IsNullOrEmpty($WebSite.Name))
        {
            continue;
        }

		CreateAppPool -Site $WebSite.Name -SvcAcctDomain $SvcAcctDomain -svcAcctName $svcAcctName -svcAcctPassword $svcAcctPassword -DotNetRunTimeVersion $DotNetRunTimeVersion
		CreateWebsite -Port $WebSite.Port -Site $WebSite.Name -sitePath $IISWebSiteRootDirectory -IISLogDirectory $IISLogDirectory
	}
    }


}

Main -ServerRole $ServerRole -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName -CreateProbeSite $CreateProbeSite
