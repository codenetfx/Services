<# 
.SYNOPSIS
	Runs Environment Configuration Scripts.

.PARAMETER ServerRole
	Type of server (Portal, Services SPWFE, SPAPP, SQL, AD)
	
.DESCRIPTION
	There's a base set of configuration that needs to happen on all servers.  
	
#>
param(
    [Parameter(Mandatory=$false)][string]$ServerRole,
    [Parameter(Mandatory=$false)][string]$EnvironmentName,
    [Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName=$null
)

Set-StrictMode -Version 2

$ErrorActionPreference = "Inquire"

#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions


function ConfigureToolsServer([Parameter(Mandatory=$true)]$basePath)
{

    Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\OpenFirewall.ps1
       . $basePath\CreateSqlAliases.ps1
       . $basePath\Install_SSMS.ps1
}

function ConfigureADServers([Parameter(Mandatory=$true)]$basePath)
{
		Write-Output "Manual Steps:"
		Write-Output "Configure DNS Reverse Lookup & PTR Record"
		Write-Output "Enable the 'File and Printer Sharing (SMB-In) rule on each server."
		Write-Output "Run Set-Execution Policy "unrestricted" -Force on each server."
		pause
       . $basePath\AD\ConfigureDNS.ps1
        Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\AD\CreateADUsers.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\OpenFirewall.ps1
	   . $basePath\AD\CreateDnsRecords.ps1
	   . $basePath\AD\ConfigureDNS.ps1
       . $basePath\CreateSqlAliases.ps1
#       . $basePath\CopySoftwareToAllServers.ps1
	   . $basePath\Install_SSMS.ps1
       . $basePath\AD\CreateDNSRecords.ps1
}


function ConfigureSqlServer([Parameter(Mandatory=$true)]$basePath)
{
       Add-WindowsFeature NET-Framework-Features
	   Add-WindowsFeature Failover-Clustering
	   Add-WindowsFeature RSAT-Clustering-Mgmt
	   Add-WindowsFeature RSAT-Clustering-Powershell
	   Add-WindowsFeature RSAT-Clustering-CmdInterface
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\OpenFirewall.ps1
       . $basePath\NetworkDTC.ps1
       . $basePath\CreateSQLAliases.ps1
	   . $basePath\SQL\SetSQLDrives.ps1
	   . $basePath\SQL\InstallSql.ps1
	   . $basePath\SQL\SetSqlPort.ps1
	   . $basePath\SQL\CreateSqlUsers.ps1
}

function ConfigureCombinedPortalAndMiddleTierServers([Parameter(Mandatory=$true)]$basePath)
{
       Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1 -ServerRole "Services"
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1 -ServerRole "Services"
       . $basePath\UnzipSoftware.ps1 -ServerRole "Services"
       . $basePath\Addusers2requiredLocalGroups.ps1
       . $basePath\ImportCertificates.ps1 -ServerRole "Services"
       . $basePath\AddWindowsFeatures.ps1 -ServerRole "Services"
       . $basePath\Portal\AspNetStateService.ps1 -ServerRole "Portal" # Use this when we aren't using AppFabric.
       . $basePath\CreateWebSites.ps1 -ServerRole "Portal"
       . $basePath\CreateWebSites.ps1 -ServerRole "Services"
       . $basePath\CreateWebSites.ps1 -ServerRole "Services" -CreateProbeSite $true
       . $basePath\OpenFirewall.ps1
       . $basePath\NetworkDTC.ps1
       . $basePath\CreateSQLAliases.ps1
       . $basePath\SetupQueueDistributor.ps1
	   Write-Output "Manual Step:"
	   Write-Output "Install Certificates - See Faisal or Kyle"
	   Write-Output "Create DNS entry for new environment"

}

function ConfigureSharePointServers([Parameter(Mandatory=$true)]$basePath)
{

       Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\Addusers2requiredLocalGroups.ps1
       . $basePath\ImportCertificates.ps1
       . $basePath\OpenFirewall.ps1
       . $basePath\CreateSQLAliases.ps1
       . $basePath\CreateWebSites.ps1 -CreateProbeSite $true
}

function ConfigurePortalServers([Parameter(Mandatory=$true)]$basePath)
{
       Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\Addusers2requiredLocalGroups.ps1
       . $basePath\ImportCertificates.ps1
       . $basePath\AddWindowsFeatures.ps1
       . $basePath\Portal\AspNetStateService.ps1
       . $basePath\CreateWebSites.ps1
       . $basePath\CreateWebSites.ps1 -CreateProbeSite $true
       . $basePath\OpenFirewall.ps1
       . $basePath\NetworkDTC.ps1
       . $basePath\CreateSQLAliases.ps1
       . $basePath\SetupQueueDistributor.ps1
}


function ConfigureMiddleTierServers([Parameter(Mandatory=$true)]$basePath)
{

       Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\Addusers2requiredLocalGroups.ps1
       . $basePath\ImportCertificates.ps1
       . $basePath\AddWindowsFeatures.ps1
       . $basePath\AspNetStateService.ps1
       . $basePath\CreateWebSites.ps1
       . $basePath\CreateWebSites.ps1 -CreateProbeSite $true
       . $basePath\OpenFirewall.ps1
       . $basePath\NetworkDTC.ps1
       . $basePath\CreateSQLAliases.ps1
       . $basePath\SetupQueueDistributor.ps1
}


function ConfigureOnPremServer([Parameter(Mandatory=$true)]$basePath)
{

       Add-WindowsFeature NET-Framework-Features
       . $basePath\EnableRemoting.ps1
       . $basePath\CreateFolders.ps1
       . $basePath\UpdateServerSettings.ps1
       . $basePath\DownloadSoftwareFromAzureBlobStorage.ps1
       . $basePath\UnzipSoftware.ps1
       . $basePath\Addusers2requiredLocalGroups.ps1
       . $basePath\ImportCertificates.ps1
       . $basePath\AddWindowsFeatures.ps1
       . $basePath\AspNetStateService.ps1
       . $basePath\CreateWebSites.ps1
       . $basePath\OpenFirewall.ps1
       . $basePath\CreateSQLAliases.ps1
       . $basePath\SetupQueueDistributor.ps1
} 

function Main(
[Parameter(Mandatory=$false)][string]$ServerRole,
[Parameter(Mandatory=$false)][string]$EnvironmentName,
[Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName
)
{

	$EnvironmentConfigurationFileName=$null
	$ServerName = $env:computername

    $GlobalConfigurationFileName = Ensure-EnvironmentFile $GlobalConfigurationFileName
    $GlobalConfiguration = Get-GlobalConfiguration -InputFile $GlobalConfigurationFileName

    $EnvironmentName = Ensure-EnvironmentName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles

    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName $EnvironmentConfigurationFileName $EnvironmentName $ServerName $GlobalConfiguration.EnvironmentConfigurationFiles
    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName

    $ServerRole = Ensure-DeploymentTier -DeploymentTier $ServerRole -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration

	$basePath = $GlobalConfiguration.ConfigurationScriptsDirectory
	$UseUnifiedPortalMiddleTier =$EnvironmentConfiguration.UseUnifiedPortalMiddleTier
	#Certain server roles require additional configuration.
	#The server role where we are initiating the remote PowerShell commands.
	
	if ((($ServerRole -eq "Portal") -or ($ServerRole -eq "Services")) -and ($UseUnifiedPortalMiddleTier -eq $true))
	{
		Write-Output "Running Configuration Scripts for Unified Portal & Middle Tier"
	}
	else
	{
		Write-Output "Running Configuration Scripts for $ServerRole"
	}
	
	if ($ServerRole -eq "AD")
	{
		ConfigureADServers $basePath
	}

	if ((($ServerRole -eq "Portal") -or ($ServerRole -eq "Services")) -and ($UseUnifiedPortalMiddleTier -eq $true))
	{
		ConfigureCombinedPortalAndMiddleTierServers $basePath
	}

	if (($ServerRole -eq "Portal") -and ($UseUnifiedPortalMiddleTier -eq $false))
	{
		ConfigurePortalServers $basePath
	}
	
	if (($ServerRole -eq "Services") -and ($UseUnifiedPortalMiddleTier -eq $false))
	{
		ConfigureMiddleTierServers $basePath
	}
		
	if ($ServerRole -eq "SPAPP")
	{
		ConfigureSharePointServers $basePath
		
	   . $basePath\SPAPP\ConfigureAutoSPInstaller.ps1
		set-location $basePath\SPAPP\SP\AutoSPInstaller
		Write-Output "Manual Steps:"
		Write-Output "After SharePoint is installed, Run:"
		Write-Output "AddSharePointFarmAdmins.ps1"
		Write-Output "Update Web App Policy with domain\domain admins in Central Admin"
		pause
		.\AutoSPInstallerMain.ps1 .\AutoSPInstallerInput.xml
	}
	
	if ($ServerRole -eq "SPWFE")
	{
		ConfigureSharePointServers $basePath
		Write-Output "Manual Steps:"
		Write-Output "Run prequisiteInstaller.exe"
		Write-Output "Install Sharepoint 2013"
		Write-Output "Run SPAPP\AddServerToExistingFarm.ps1"
		Write-Output "Ensure IIS default site is off and Portal site is running"
	}
	
	if ($ServerRole -eq "SQL")
	{
		ConfigureSqlServer $basePath
	}
	
	if ($ServerRole -eq "OnPrem")
	{
		ConfigureOnPremServer $basePath
	}
}

Main -ServerRole $ServerRole -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName
