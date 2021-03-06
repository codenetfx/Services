<#
.SYOPSIS
	Copies installation scripts and hotfixes to servers

.PARAMETER EnvironmentName
	The name of the deployment environment specified in the environments.xml
	
.PARAMETER DeploymentTier
	The name of the tier to deploy to.
	
.PARAMETER InputFile
	The name and path of the environments.xml file that contains the configuration information.
#>
param
(
	[Parameter(Mandatory=$false)]$EnvironmentName,
	[Parameter(Mandatory=$false)]$DeploymentTier = "AllAzure",
    [Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName=$null
)

Set-StrictMode -Version 2

#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

#Copies installation binaries from the staging directory to the destination server.
function CopyInstallationBinariesToServers([string]$SrcPath, [string]$DestinationPath, [string[]]$servers)
{
    foreach ($server in $servers)
	{   
		if($server.Length -eq 0)
		{
			continue
		}
		
        $UncRootDestPath = $DestinationPath.ToLower().Replace("c:\", "\\$server\c$\") #converting path to a UNC Path
        #Write-Host("Copying installation files from $SrcPath to $UncRootDestPath ")
		Write-Host("$SrcPath to $UncRootDestPath")
        $subdirectories = Get-Item $SrcPath
        foreach ($subdirectory in $subdirectories)
        {
            Copy-Item $subdirectory -Destination $UncRootDestPath -Recurse -Force
        }
    }
}


function Main(
[Parameter(Mandatory=$false)][String]$DeploymentTier,
[Parameter(Mandatory=$false)][String]$EnvironmentName,
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

    #$DeploymentTier = Ensure-DeploymentTier -DeploymentTier $DeploymentTier -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration

    Write-Host "DeploymentTier:  $DeploymentTier"

    if ($EnvironmentConfiguration -eq $null)
    {
        Write-Host "Environment Name $EnvironmentName not found in $InputFile"
        exit 4
    }

    $RemotePowerShellUser = $EnvironmentConfiguration.RemotePowerShellUser # User name used for remote PowerShell Sessions on each server
    $RemotePowerShellPwd = $EnvironmentConfiguration.RemotePowerShellPwd   # Password for remote PowerShell Sessions on each server

    #Servers that installation binaries are copied to & Servers used in Remote PowerShell Sessions
    $PortalServers = $EnvironmentConfiguration.PortalServers
    $MiddleTierServers = $EnvironmentConfiguration.MiddleTierServers
    $SharePointServer = $EnvironmentConfiguration.SharePointServer
	$SharePointAPPs = $EnvironmentConfiguration.SharePointAPPs
	$SharePointWFEs = $EnvironmentConfiguration.SharePointWFEs
	$SqlServers = $EnvironmentConfiguration.SqlServers
	
	$UseUnifiedPortalMiddleTier = $EnvironmentConfiguration.UseUnifiedPortalMiddleTier
	
    #$StagingDirectory= $rootSrcPath + "\Staging"
	$ConfigurationScriptsDirectory = $GlobalConfiguration.ConfigurationScriptsDirectory
	$SoftwareDirectory = $GlobalConfiguration.SoftwareDirectory
    $XMLDirectory = $GlobalConfiguration.GlobalConfigurationDirectoryPath
    $CertificatesDirectory = join-path (split-path $ConfigurationScriptsDirectory -Parent) "Certificates"
    #ValidateCommandLineParameters $RootSrcPath $buildNum $InputFile $DeploymentTier
	
	if (($DeploymentTier -eq "Portal") -or ($DeploymentTier -eq "AllAzure"))
	{
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server - PortalServers")
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $PortalServers
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $PortalServers
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $PortalServers        
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $PortalServers

	}
	
	if ((($DeploymentTier -eq "Services") -or ($DeploymentTier -eq "AllAzure")) -and ($UseUnifiedPortalMiddleTier -eq $false))
	{
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server - MiddleTierServers")
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $MiddleTierServers
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $MiddleTierServers
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $MiddleTierServers
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $MiddleTierServers
	}
	
	if (($DeploymentTier -eq "SPAPP") -or ($DeploymentTier -eq "AllAzure"))
	{	
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server - SharePointServer")
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $SharePointServer
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $SharePointServer
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $SharePointServer
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $SharePointServer
	}
	
	if (($DeploymentTier -eq "SPAPP") -or ($DeploymentTier -eq "AllAzure"))
	{	
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server - SharePointAPPs")
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $SharePointAPPs
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $SharePointAPPs
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $SharePointAPPs
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $SharePointAPPs
	}
	
	if (($DeploymentTier -eq "SPWFE") -or ($DeploymentTier -eq "AllAzure"))
	{	
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server - SharePointWFEs")
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $SharePointWFEs
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $SharePointWFEs
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $SharePointWFEs
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $SharePointWFEs
	}
	
	if (($DeploymentTier -eq "SQL") -or ($DeploymentTier -eq "AllAzure"))
	{	
		Write-Host("Copying installation files from $ConfigurationScriptsDirectory to each server- SqlServer")
        $SqlServerArray = $SqlServers  | Select -ExpandProperty Name 
		CopyInstallationBinariesToServers -SrcPath $ConfigurationScriptsDirectory -DestinationPath $ConfigurationScriptsDirectory -Servers $SqlServerArray
		CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $SqlServerArray
        CopyInstallationBinariesToServers -SrcPath $XMLDirectory -DestinationPath $XMLDirectory -Servers $SqlServerArray
        CopyInstallationBinariesToServers -SrcPath $CertificatesDirectory -DestinationPath $CertificatesDirectory -Servers $SqlServerArray
	}
	
	if (($DeploymentTier -eq "AD") -or ($DeploymentTier -eq "AllAzure"))
	{	
		#TODO - Don't try and copy software to yourself
		#Write-Host("Copying installation files from $StagingDirectory to each server- ActiveDirectories")
		#CopyInstallationBinariesToServers -SrcPath $StagingDirectory -DestinationPath $DestPath -Servers $ActiveDirectories
		#CopyInstallationBinariesToServers -SrcPath $SoftwareDirectory -DestinationPath $SoftwareDirectory -Servers $ActiveDirectories
	}
	
}

Main -DeploymentTier $DeploymentTier -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName
