<# 
.SYNOPSIS
	Downloads software for the specified server role from Azure Blob Storage

.PARAMETER ServerRole
	Type of server (Portal, Services SPWFE, SPAPP, SQL, AD)

.PARAMETER SoftwareDirectory
	The location to save the blob downloaded from Azure.

.COMPONENT
	Uses AzCopy from CodePlex. Requires .Net 4.0

.NOTES
	Each server role has a directory in Azure.
#>
param(
    [Parameter(Mandatory=$true)][string]$ServerRole,
    [Parameter(Mandatory=$true)][string]$EnvironmentName,
    [Parameter(Mandatory=$true)][string]$GlobalConfigurationFileName, 
    [Parameter(Mandatory=$false)][string]$nl = [Environment]::NewLine,
    [Parameter(Mandatory=$false)][String]$XMLDirectory
)

Set-StrictMode -Version 2
#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

function DownloadAzureBlob ([Parameter(Mandatory=$true)][String]$AzureBlobStorageUrl, [Parameter(Mandatory=$true)][String]$AzureBlobStorageSourceKey, [Parameter(Mandatory=$true)][String]$BlobDirectory, [Parameter(Mandatory=$true)][String]$Destination="C:\Software\",[Parameter(Mandatory=$false)][String]$Blob, [Parameter(Mandatory=$false)][String]$Location)
{
	$exeLocation = Get-AzureCopy -Location $Location
	$Source = $AzureBlobStorageUrl + $BlobDirectory
	Write-Host ($nl + "Copying software from source " + $Source + " to destination " + $Destination)
	#Recursively copy a set of blobs from a blob storage to a locally accessible directory in recursively (/S) using re-stable mode (/Z).
    if ($Blob -eq $null)
    {
	    & "$exeLocation" /source:$Source /dest:$Destination /sourcekey:$AzureBlobStorageSourceKey /S /Y /XO
    }
    else
    {
	    & "$exeLocation" /source:$Source /dest:$Destination /pattern:$Blob /sourcekey:$AzureBlobStorageSourceKey /S /Y /XO
    }
}

function Get-AzureCopy([Parameter(Mandatory=$false)][String]$Location)
{
    if (![string]::IsNullOrEmpty($Location))
    {
       return "$Location\Common\AzCopy\AzCopy.exe"
    }
    else
    {

    return "C:\Apps\Dist\Common\AzCopy\AzCopy.exe";
    }

}

function GetBlobDirectory ([Parameter(Mandatory=$true)][String]$ServerRole)
{

	#Not every tier has a blob in Azure that needs to be downloaded.
	#If there's no blob, the tier has "".
	$BlobDirectoryNameHashTable = @{
		"AD" ="admedia";
		"Portal"="";
		"Services" = "servicesmedia";
		"SPWFE"="";
		"SQL" = "media";
		}

	foreach ($Key in $BlobDirectoryNameHashTable.Keys)
	{
		if ($Key -eq $ServerRole)
		{
			$BlobDirectoryName=$BlobDirectoryNameHashTable.Item($Key)
			return $BlobDirectoryName
	    }
    }

}

function Main(
	[Parameter(Mandatory=$false)][String]$ServerRole,
	[Parameter(Mandatory=$false)][String]$EnvironmentName,
	[Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName,
	[Parameter(Mandatory=$false)][String]$XMLDirectory
)
{

	$EnvironmentConfigurationFileName=$null
	$ServerName = $env:computername

	$GlobalConfigurationFileName = Ensure-EnvironmentFile $GlobalConfigurationFileName
    $GlobalConfiguration = Get-GlobalConfiguration -InputFile $GlobalConfigurationFileName
    [String]$SoftwareDirectory = $GlobalConfiguration.SoftwareDirectory

    $EnvironmentName = Ensure-EnvironmentName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles

    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName $EnvironmentConfigurationFileName $EnvironmentName $ServerName $GlobalConfiguration.EnvironmentConfigurationFiles
    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName -XMLDirectory $XMLDirectory

    $ServerRole = Ensure-DeploymentTier -DeploymentTier $ServerRole -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration
	

 	
	$InstalledVersions = Get-DotNetFrameworkVersionsInstalled	
	if ( (!$InstalledVersions.Contains("4.0")) -or (!$InstalledVersions.Contains("4.0c")))
	{
		Write-Host ($nl + "AZCopy requires .Net 4.0")
		exit -2
	}
	
	#Azure Blob Storage - AzCopy
	$AzureBlobStorageUrl = $GlobalConfiguration.AzureBlobStorageUrl
	$AzureBlobStorageSourceKey = $GlobalConfiguration.AzureBlobStorageSourceKey
	
#	$BlobDirectory = GetBlobDirectory -ServerRole $ServerRole
#
#	if ($BlobDirectory -eq $null)
#	{
#		Write-Host ($nl + "Unable to find blob directory for $ServerRole server role")
#		exit -2
#	}
#	
#	DownloadAzureBlob -BlobDirectory $BlobDirectory -Destination $SoftwareDirectory

    if ($ServerRole -in @("Tool"))
    {
    	DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "toolsmedia" -Destination $SoftwareDirectory
	DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "tools" -Destination $SoftwareDirectory
    }
    
    if ($ServerRole -in @("AD"))
    {
		DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "toolsmedia" -Destination $SoftwareDirectory -Blob "sqlncli.msi"
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "toolsmedia" -Destination $SoftwareDirectory -Blob "SQLManagementStudio_x64_ENU.EXE"
	    DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "tools" -Destination $SoftwareDirectory -Blob "MSBuildTools.zip"
    }

	if ($ServerRole -in @("Portal", "Services"))
    {
	    DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "portalmedia" -Destination $SoftwareDirectory -Location $XMLDirectory
    }

    if ($ServerRole -in @("Portal", "Services"))
    {
	    DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "commonmedia" -Destination $SoftwareDirectory -Blob "EnterpriseLibraryLogging.zip" -Location $XMLDirectory
    }

    # These are the tools that should be placed on the tools server.
    if ($ServerRole -in @("SPAPP", "SPWFE", "Services"))
    {
	    DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "tools" -Destination $SoftwareDirectory -Blob "Tools.zip" -Location $XMLDirectory
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "toolsmedia" -Destination $SoftwareDirectory -Blob "sqlncli.msi" -Location $XMLDirectory
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "toolsmedia" -Destination $SoftwareDirectory -Blob "SQLManagementStudio_x64_ENU.EXE" -Location $XMLDirectory
    }

    if ($ServerRole -in @("SPAPP", "SPWFE"))
    {
        $SharePointDirectory = (Join-Path $GlobalConfiguration.ConfigurationScriptsDirectory "SPAPP\SP\2013\SharePoint") #SharePoint and prerequisites will be place in the AutoSPInstaller directory tree.
 	    $SharePointUpdatesDirectory = (Join-Path $GlobalConfiguration.ConfigurationScriptsDirectory "SPAPP\SP\2013\Updates") #Non-Slipstreamed SharePoint Updates will be place in the AutoSPInstaller directory tree.
        $SharePointSlipStreamedUpdatesDirectory = (Join-Path $GlobalConfiguration.ConfigurationScriptsDirectory "SPAPP\SP\2013\SharePoint\Updates") #Slipstreamed SharePoint Updates will be place in the AutoSPInstaller directory tree.

		Write-Host ($nl + "Downloading SP2013 RTM")
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepointmedia" -Destination $SoftwareDirectory -Blob "SharePoint2013_RTM.zip"
		Write-Host ($nl + "Downloading SP2013 Prequisites")
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepointmedia" -Destination $SoftwareDirectory -Blob "SP2013_Prerequisites.zip" #SharePoint 2013 Prequisites that apply to all OS.
        
        #Download SharePoint prerequisites that are operating system dependant
        if ((Get-WmiObject  Win32_OperatingSystem).Version -eq "6.1.7601") # Win2008 R2 SP1
            {
			Write-Host ($nl + "Downloading SP2013 Prequisites for Windows Server 2008 R2 SP1 into AutoSPInstaller directory")
            DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepointmedia" -Destination $SoftwareDirectory -Blob "SP2013_Prerequisites_Windows2008R2.zip"
            }

        $QueryOS = Get-WmiObject Win32_OperatingSystem
        If ($QueryOS.Version.Contains("6.2") -or $QueryOS.Version.Contains("6.3")) # Windows 2012 or 2012 R2
	        {
			Write-Host ($nl + "Downloading SP2013 Prequisites for Windows Server 2012 into AutoSPInstaller directory")
            DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepointmedia" -Destination $SoftwareDirectory -Blob "SP2013_Prerequisites_Windows2012.zip"
            }
        
		Write-Host ($nl + "Downloading SharePoint 2013 CUs into AutoSPInstaller directory")
	    #\SP\201x\SharePoint\Updates (optional, for slipstreaming Service Packs and Public/Cumulative Updates. NOTE not all updates support slipstreaming!)  
        #Any CUs downloaded from the sharepoint-cu Azure Blob Storage container will be installed

        #March 2013 CU - Slipstream
		DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepointmedia" -Destination $SharePointSlipStreamedUpdatesDirectory	-Blob "ubersrvsp2013-kb2767999-fullfile-x64-glb.exe"
        
        #Other SharePoint CUs after March 2013
		DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sharepoint-cu" -Destination $SharePointUpdatesDirectory
		
		Write-Host ($nl + "Downloading tools.")
		DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "tools" -Destination $SoftwareDirectory -Blob "SharePointUtils.zip"
    }

    if ($ServerRole -in @("SQL"))
    {
		Write-Host ($nl + "Downloading SQL Server 2012 SP1 and Cummulative Updates")
        DownloadAzureBlob -AzureBlobStorageUrl $AzureBlobStorageUrl -AzureBlobStorageSourceKey $AzureBlobStorageSourceKey -BlobDirectory "sqlmedia" -Destination $SoftwareDirectory
    }    
}

Main -ServerRole $ServerRole -GlobalConfigurationFileName $GlobalConfigurationFileName -EnvironmentName $EnvironmentName -XMLDirectory $XMLDirectory