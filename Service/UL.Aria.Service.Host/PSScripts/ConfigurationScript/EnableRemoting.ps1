<#
.SYNOPSIS
	Configures Remote PowerShell based on server role

.PARAMETER ServerRole
	Type of server (Portal, Services SPWFE, SPAPP, SQL, AD)
	
.DESCRIPTION
	There's a base set of configuration that needs to happen on all servers that will
	participate in a remote powershell session.  Addtional configuration is needed when a server
	is going to participate in a session that requires credentials to be passed.
	This is handled by the server role types (AD, SPWFE, and SPAPP)
#>
param(
    [Parameter(Mandatory=$false)][string]$ServerRole,
    [Parameter(Mandatory=$false)][string]$EnvironmentName,
    [Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName=$null
)

Set-StrictMode -Version 2
#Region External Functions
	. "$PSScriptRoot\Common\ConfigurationScriptFunctions.ps1"
#EndRegion External Functions

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

	#All Server Roles
	#Enable-PSRemoting doesn't like 
	Set-StrictMode -Version 1
	Enable-PSRemoting -Force
	Set-StrictMode -Version 2
	winrm set winrm/config/client `@`{TrustedHosts=`"`*`"`}
	set-item wsman:localhost\client\trustedhosts -value * -Force

	#Certain server roles require additional configuration.
	#The server role where we are initiating the remote PowerShell commands.
	if ($ServerRole -eq "AD")
	{
		#On the client we are running the scripts from, we run:
		Enable-WSManCredSSP -Role Client -DelegateComputer * -Force
		
		#Allows us to start remote PowerShell sessions using CredSSP
		Set-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentials -Name WSMan -Value WSMAN/* 
		Set-ItemProperty HKLM:\SYSTEM\CurrentControlSet\Control\Lsa\Credssp\PolicyDefaults\AllowFreshCredentialsDomain -Name WSMan -Value WSMAN/*
	}

	if (($ServerRole -eq "SPWFE") -or ($ServerRole -eq "SPAPP"))
	{
		#CredSSP allows us to pass credentials from SharePoint to SQL
		Enable-WSManCredSSP -Role Server -Force 
	}
}

Main -ServerRole $ServerRole -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName