<#
.SYNOPSIS
	Installs SQL Server Management Studio (SSMS)

.PARAMETER SourceDirectory
	Directory that contains the zip file

.PARAMETER ZipFileName
	The name of the zip file that contains SSMS

.PARAMETER DestinationDirectory
	The directory to install SSMS
#>
param
(
[Parameter(Mandatory=$false)][String]$SourceDirectory="C:\Software",
[Parameter(Mandatory=$false)][string]$DestinationDirectory="C:\Software"
)

Set-StrictMode -Version 2
#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

function InstallSQLServerManagementStudio([string]$InstallDirectory)
{
    Write-Host "Installing SQL Server Management Studio.  (This takes a few minutes to run.)" -ForegroundColor Green
    #$SetupProgram = "$InstallDirectory\Setup.exe"
    #$SetupProgram /Q /Action=UnInstall /INDICATEPROGRESS /IACCEPTSQLSERVERLICENSETERMS /ENU /UpdateEnabled=1 /FEATURES=SSMS
	msiexec /i (Join-Path $InstallDirectory "sqlncli.msi") /q IACCEPTSQLNCLILICENSETERMS=YES
    #When this runs in quite mode, there's no visual queue its running.  It takes a couple of minutes to install this.
    & "$InstallDirectory\SQLManagementStudio_x64_ENU.exe" /QS /Action=Install /INDICATEPROGRESS /IACCEPTSQLSERVERLICENSETERMS /ENU /UpdateEnabled=1 /FEATURES=SSMS
    Write-Host "Installing SQL Server Management Studio - Finished." -ForegroundColor Green
}

function Main (
    [Parameter(Mandatory=$false)][String]$SourceDirectory="C:\Software",
    [Parameter(Mandatory=$false)][string]$DestinationDirectory="C:\Software"
)
{
    #$DriveLetter ="x:"
    #$ServerName = $env:userdomain + "SQL1"
    #$ShareName = "software"
    #MapNetworkDrive $DriveLetter $ServerName $ShareName

    #Unzip -SourceArchiveFileName (Join-Path $SourceDirectory $ZipFileName) -DestinationDirectoryName $DestinationDirectory

    InstallSQLServerManagementStudio ($DestinationDirectory)
}

Main -SourceDirectory $SourceDirectory -DestinationDirectory $DestinationDirectory