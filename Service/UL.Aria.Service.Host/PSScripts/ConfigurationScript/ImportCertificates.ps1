<#
.SYNOPSIS
	Installs a certificate into the local machine's certificate store.

.PARAMETER ServerRole

.PARAMETER EnvironmentName

.PARAMETER GlobalConfigurationFileName

.DESCRIPTION
	This script installs the certifcate used by the SharePoint High Trust App that's used to allow 
	server-to-server (S2S/OAuth) 
#>
param (
[Parameter(Mandatory=$false)][string]$ServerRole,
[Parameter(Mandatory=$false)][string]$EnvironmentName,
[Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName
)

Set-StrictMode -Version 2
#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

#Name of PowerShell Script
$scriptName=$myInvocation.MyCommand.Name


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

	$EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles
    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName

    $ServerRole = Ensure-DeploymentTier -DeploymentTier $ServerRole -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration
	
	$Certificates = $EnvironmentConfiguration.Certificates.Certificate
	$certPath = $GlobalConfiguration.CertificatesDirectory
	Write-Host -ForeGround Cyan "[$scriptName] Importing Certificate from: " $certPath

	#Install PFX Certificates
	if ($ServerRole -in @("Services", "SPWFE", "SPAPP"))
		{
			foreach ($Certificate in $Certificates)
			{
				if ([string]::IsNullOrEmpty($Certificate.pfxFile))
				{
					continue;
				}
				
				#Root Certificate store:  "LocalMachine" or "CurrentUser". 
				#Default is CurrentUser, which is your personal user store. 
				#LocalMachine is the computer's certificate store. IIS site certificates go here.
				$certRootStore = $Certificate.certRootStore
					
				#Certificate Store.
				#If you go to the cert: drive, then change the directory to either LocalMachine or CurrentUser and run "dir" you will see a list of all the cert stores. 
				#My = Personal
				#root = Trusted Root Certificate Authorities, etc. 
				$pfxCertStore = $Certificate.pfxCertStore
				$certStore = $Certificate.certStore
				# Install PFX certificate in Certificates (Local Computer)\Personal\Certificates
				Write-Host -ForeGround Cyan "[$scriptName] Importing $($Certificate.pfxFile) to $certRootStore in $pfxCertStore"
				$ResolvedPath = Resolve-Path(($certPath +"\" + $Certificate.pfxFile))
				
				if (!(Test-Path $ResolvedPath))
				{
					Write-Host "Can't locate certificate $($Certificate.Name) from $ResolvedPath"
					exit -1
				}
				
				Import-PfxCertificate -certPath  $ResolvedPath -certRootStore $certRootStore  -certStore $pfxCertStore -pfxPass $Certificate.pfxPassword
				
				# If we have a thumbprint, verify that the certificate was actually imported
				if ([string]::IsNullOrEmpty($Certificate.thumbprint))
				{
					$cert = dir ("Cert:\" + $CertRootStore +"\" + $pfxCertStore) | ? {$_.Thumbprint -like ($Certificate.thumbprint +"*")}
					Test-Path ("Cert:\" + $CertRootStore +"\" + $pfxCertStore +"\" + $cert.Thumbprint)
				}
			} # foreach Certificate
		} #if ServerRole

	
	#Install CER Certificates	
	if ($ServerRole -in @("Services", "SPWFE", "SPAPP"))
		{	
			foreach ($Certificate in $Certificates)
			{
				if ([string]::IsNullOrEmpty($Certificate.cerFile))
				{
					continue;
				}
				#Root Certificate store:  "LocalMachine" or "CurrentUser". 
				#Default is CurrentUser, which is your personal user store. 
				#LocalMachine is the computer's certificate store. IIS site certificates go here.
				$certRootStore = $Certificate.certRootStore

				#Certificate Store.
				#If you go to the cert: drive, then change the directory to either LocalMachine or CurrentUser and run "dir" you will see a list of all the cert stores. 
				#My = Personal
				#root = Trusted Root Certificate Authorities, etc. 				
				$certStore = $Certificate.certStore
				
				#Install CER Certificate in Trusted Root Certificate Authorities\Certificates
				Write-Host -ForeGround Cyan "[$scriptName] Importing $($Certificate.cerFile) to $certRootStore in $certStore"
				$ResolvedPath = (Resolve-Path ($certPath +"\" + $Certificate.cerFile))
				
				if (!(Test-Path $ResolvedPath))
				{
					Write-Host "Can't locate certificate $($Certificate.Name) from $ResolvedPath"
					exit -1
				}
				
				Import-509Certificate -certPath $ResolvedPath -certRootStore $certRootStore -certStore $certStore
				
				if ([string]::IsNullOrEmpty($Certificate.thumbprint))
				{
					# If we have a thumbprint, verify that the certificate was actually imported		
					$cert = dir ("Cert:\" + $CertRootStore +"\" + $certStore) | ? {$_.Thumbprint -like ($Certificate.thumbprint +"*")}
					test-path ("Cert:\" + $CertRootStore +"\" + $certStore +"\" + $cert.Thumbprint)
				}
			} #foreach Certificate
		} # If ServerRole
}

Main -ServerRole $ServerRole -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName
