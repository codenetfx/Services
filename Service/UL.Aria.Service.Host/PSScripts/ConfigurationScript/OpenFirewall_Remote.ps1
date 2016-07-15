<#.SYNOPSIS
   Configures the Windows Firewall across all machine in the environment.

.DESCRIPTION
    Configures the Windows Firewall across all tiers (Portal,
    Services, SharePoint, SQL, and Active Directory) or just a single tier.  

    This script works across different environments (i.e. DevInt, UAT, QA).
	This script requires Windows Server Server 2008 or Windows Server 2012.

.PARAMETER EnvironmentName
    The name of the environment configuration in the Environments.xml file.

.PARAMETER GlobalConfigurationFileName
    the environment xml file that describes the environment configuration information.

.PARAMETER DeploymentTier
    Determines which tier to deploy to.  The default is all Azure tiers (AllAzure).
    Choices are staging, AD, Portal, Services, SharePoint, SQL, AllAzure, OnPrem
	
#>
param(
[Parameter(Mandatory=$true)][string]$EnvironmentName,
[Parameter(Mandatory=$false)][string]$GlobalConfigurationFileName,
[Parameter(Mandatory=$false)]$DeploymentTier="AllAzure"
)

Set-StrictMode -Version 2

#Region External_Functions
    
    . "$PSSCriptRoot\Common\ConfigurationScriptFunctions.ps1"

#EndRegion

#region
function EnableInboundFirewallRule([Parameter(Mandatory=$true)][string]$RuleName, [Parameter(Mandatory=$true)][string]$Profile, [Parameter(Mandatory=$true)]$CimSession)
{
    Write-Host "Enabling Inbound Firewall Rule $RuleName on $Profile"
	New-NetFirewallRule -DisplayName $RuleName -Direction Inbound -Action "Allow" -Enabled True -Profile $Profile -CimSession $CimSession
}

function EnableOutboundFirewallRule([Parameter(Mandatory=$true)][string]$RuleName, [Parameter(Mandatory=$true)][string]$Profile, [Parameter(Mandatory=$true)]$CimSession)
{
    Write-Host "Enabling Outbound Firewall Rule $RuleName on $Profile"
	New-NetFirewallRule -DisplayName $RuleName -Direction Outbound -Action "Allow" -Enabled True -Profile $Profile -CimSession $CimSession
}

function CreateAllPortsRules([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
    $Profile = "Domain"

	Write-Host "Disabling the Windows Firewall Domain profile on ($CimSession.ComputerName)"
	Set-NetFirewallProfile -Name $Profile -Enabled True -CimSession $CimSession
    
	#Write-Debug "Creating All Ports rules - TCP"
	New-NetFirewallRule -DisplayName "All Ports TCP" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "any"  -Profile $Profile  -CimSession $CimSession
	New-NetFirewallRule -DisplayName "All Ports TCP" -Direction "Outbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "any" -Profile $Profile  -CimSession $CimSession
	
    #Write-Debug "Creating All Ports rules - UDP"
	New-NetFirewallRule -DisplayName "All Ports UDP" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "UDP" -LocalPort "any" -Profile $Profile  -CimSession $CimSession
	New-NetFirewallRule -DisplayName "All Ports UDP" -Direction "Outbound" -Action "Allow" -Enabled True -Protocol "UDP" -LocalPort "any" -Profile $Profile  -CimSession $CimSession
}

function EnableFirewallRulesForOtherSqlServices([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
	New-NetFirewallRule -DisplayName "Open Port 80" -Direction "Outbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "80" -Profile $Profile  -CimSession $CimSession
	
	#Write-Host "Enabling Conventional SQL Server Service Broker port 4022"
	New-NetFirewallRule -DisplayName "SQL Service Broker" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "4022" -Profile $Profile  -CimSession $CimSession

	#Write-Host "Enabling Transact SQL/RPC port 135"
	New-NetFirewallRule -DisplayName "SQL Debugger/RPC" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "135"  -Profile $Profile  -CimSession $CimSession

	#Write-Host "=========  Analysis Services Ports  =============="
	#Write-Host "Enabling SSAS Default Instance port 2383"
	New-NetFirewallRule -DisplayName "Analysis Services" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "2383" -Profile $Profile  -CimSession $CimSession

	#Write-Host "Enabling SQL Server Browser Service port 2382"
	New-NetFirewallRule -DisplayName "SQL Browser" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "2382" -Profile $Profile  -CimSession $CimSession

	#Write-Host "=========  Misc Applications  =============="
	#Write-Host "Enabling HTTP port 80"
	New-NetFirewallRule -DisplayName "Enabling HTTP port 80" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "HTTP" -LocalPort "80" -Profile $Profile  -CimSession $CimSession

	#Write-Host "Enabling SSL port 443"
	#netsh advfirewall firewall add rule name="SSL" dir=in action=allow protocol=TCP localport=443
	New-NetFirewallRule -DisplayName "Enabling HTTP port 80" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "SSL" -LocalPort "443" -Profile $Profile  -CimSession $CimSession
	
	#Write-Host "Enabling port for SQL Server Browser Service's 'Browse' Button"
	#netsh advfirewall firewall add rule name="SQL Browser" dir=in action=allow protocol=UDP localport=1434
	New-NetFirewallRule -DisplayName "SQL Browser" -Direction "Inbound" -Action "Allow" -Enabled True -Protocol "UDP" -LocalPort "1434" -Profile $Profile  -CimSession $CimSession

	#Write-Host "Allowing multicast broadcast response on UDP (Browser Service Enumerations OK)"
	#netsh firewall set multicastbroadcastresponse ENABLE
	Set-NetFirewallProfile -DefaultInboundAction Block -DefaultOutboundAction Allow -NotifyOnListen True -AllowUnicastResponseToMulticast True -Profile $Profile

}
#endregion

#region
function ConfigureFirewallAD ([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
    # Nothing to do.
    # Add Firewall rules here for that should run on AD servers.
}

function ConfigureFirewallPortal ([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
	#DTC Inbound Rules:CimSession
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (RPC)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (RPC-EPMAP)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (TCP-In)" -Profile $Profile -CimSession $CimSession
}

function ConfigureFirewallServices ([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
	#AppFabric / SharePoint Distributed Cache
	EnableInboundFirewallRule -RuleName "Remote Service Management (NP-In)" $Profile -CimSession $CimSession
	#AppFabric Caching Service (TCP-In) - Enabled by default
	#AppFabric Caching Service (TCP-Out) - Enabled by default
	
	#DTC Inbound Rules:
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (RPC)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (RPC-EPMAP)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule -RuleName "Distributed Transaction Coordinator (TCP-In)" -Profile $Profile -CimSession $CimSession
}

function ConfigureFirewallSharePoint ([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
	#AppFabric / SharePoint Distributed Cache
	EnableInboundFirewallRule -RuleName "Remote Service Management (NP-In)" -Profile $Profile -CimSession $CimSession
	#AppFabric Caching Service (TCP-In) - Enabled by default
	#AppFabric Caching Service (TCP-Out) - Enabled by default
}

function ConfigureFirewallSql ([Parameter(Mandatory=$true)]$CimSession, [Parameter(Mandatory=$true)][string]$Profile)
{
	EnableInboundFirewallRule "Remote Service Management (NP-In)" $Profile -CimSession $CimSession
	
	#DTC Inbound Rules:
	EnableInboundFirewallRule "Distributed Transaction Coordinator (RPC)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule "Distributed Transaction Coordinator (RPC-EPMAP)" -Profile $Profile -CimSession $CimSession
	EnableInboundFirewallRule "Distributed Transaction Coordinator (TCP-In)" -Profile $Profile -CimSession $CimSession
		
	Write-Host "=========  SQL Server Ports  ==================="
	Write-Host "Enabling SQLServer default instance port 1433"
	#netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433
	New-NetFirewallRule -DisplayName "SQL Server" -Direction Inbound -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "1433" -Profile $Profile -CimSession $CimSession
	Write-Host "Enabling Dedicated Admin Connection port 1434"
	#netsh advfirewall firewall add rule name="SQL Admin Connection" dir=in action=allow protocol=TCP localport=1434
	New-NetFirewallRule -DisplayName "SQL Admin Connection" -Direction Inbound -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "1434" -Profile $Profile -CimSession $CimSession
	#Write-Host "Enabling UL_Aria instance port 1435"
	#netsh advfirewall firewall add rule name="SQL Server Instance" dir=in action=allow protocol=TCP localport=1435
	New-NetFirewallRule -DisplayName "SQL Admin Connection" -Direction Inbound -Action "Allow" -Enabled True -Protocol "TCP" -LocalPort "1435" -Profile $Profile -CimSession $CimSession
}
#endregion

function Main()
{

    $EnvironmentConfigurationFileName=$null
    #$ServerName = $env:COMPUTERNAME
    $ServerName = "UAT8AD1"

    $GlobalConfigurationFileName = Ensure-EnvironmentFile $GlobalConfigurationFileName
    $GlobalConfiguration = Get-GlobalConfiguration -InputFile $GlobalConfigurationFileName

    $EnvironmentName = Ensure-EnvironmentName -EnvironmentName $EnvironmentName -ServerName $ServerName -EnvironmentConfigurationFiles $GlobalConfiguration.EnvironmentConfigurationFiles

    $EnvironmentConfigurationFileName = Ensure-EnvironmentConfigurationFileName $EnvironmentConfigurationFileName $EnvironmentName $ServerName $GlobalConfiguration.EnvironmentConfigurationFiles
    $EnvironmentConfiguration = Get-EnvironmentConfiguration -EnvironmentConfigurationFileName $EnvironmentConfigurationFileName -EnvironmentName $EnvironmentName 

    $DeploymentTier = Ensure-DeploymentTier -DeploymentTier $DeploymentTier -ServerName $ServerName -EnvironmentConfiguration $EnvironmentConfiguration

	$RemotePowerShellUser = $EnvironmentConfiguration.RemotePowerShellUser # User name used for remote PowerShell Sessions on each server
	$RemotePowerShellPwd = $EnvironmentConfiguration.RemotePowerShellPwd                               # Password for remote PowerShell Sessions on each server

	  
	#Global Configuration Information from Environment.xml
	$DeployLogsDir = $GlobalConfiguration.DeployLogsDir                       # Powershell Transcripts from each deploy live here
	
    # Servers used in Remote PowerShell Sessions
	#$ActiveDirectoryServers = $EnvironmentConfiguration.ActiveDirectoryServers
	$PortalServers = $EnvironmentConfiguration.PortalServers
	$MiddleTierServers = $EnvironmentConfiguration.MiddleTierServers
	$UseUnifiedPortalMiddleTier = $EnvironmentConfiguration.UseUnifiedPortalMiddleTier 
	$SharePointServer = $EnvironmentConfiguration.SharePointServer
    $SharePointWFEs = $EnvironmentConfiguration.SharePointWFEs
    $SharePointAPPs = $EnvironmentConfiguration.SharePointAPPs
    $ActiveDirectoryServers = $EnvironmentConfiguration.ActiveDirectoryServers


	$AllServers = @()
	$AllServers += $PortalServers
    
    if ($UseUnifiedPortalMiddleTier -eq $false)
    {
	    $AllServers += $MiddleTierServers
    }

	#$AllServers += $SharePointServer
    if (($SharePointWFEs.Count -gt 0) -and ($SharePointWFEs[0].Length -gt 0))
    {
        $AllServers += $SharePointWFEs
    }

    $AllServers += $SharePointAPPs
    $AllServers += $ActiveDirectoryServers

    $SQLServers = @()
	foreach ($SQLServer in $EnvironmentConfiguration.SQLServers)
	{
		$SQLServers += $SQLServer.Name
	}

	$AllServers += $SQLServers
	
	#Build transcript file name.
	$DateStamp = get-date -uformat "%Y-%m-%d@%H-%M-%S"
	[String] $TranscriptFile = $DeployLogsDir + "\Deploy_" + $EnvironmentConfiguration.Name + "_" + $dateStamp +".rtf"

    #endregion Configuration_Information

   
    # Cleanup any open remote PowerShell sessions and open transcript if the last script ended with a ctrl-c.
    Remove-PSSession * -EA 0
    try{Stop-Transcript}catch{} #There's no easy way to see if the transcript is running.

    if (!($Host.Name.Contains("ISE")))
    {
        Start-Transcript -path $TranscriptFile -Force
    }
    else
    {
        Write-Warning "The PowerShell ISE doesn't support transcripts."
    }

    Write-Host ("Starting Deployment: " + ([System.DateTime]::Now).ToString())
    Write-Host "ServerName:  $env:computername"

	Write-Host "Preparing to open the firewall."
	
	# Setup Remote PowerShell Sessions 
	# Creates a persistent connection (PSSession) to the specified computer. 
	#If you enter multiple computer names, New-PSSession creates multiple PSSessions, one for each computer. 
	#The default is the local computer.
	#Type the NetBIOS name, an IP address, or a fully qualified domain name of one or more remote computers. 
	#To specify the local computer, type the computer name, "localhost", or a dot (.). 
	#When the computer is in a different domain than the user, the fully qualified domain name is 
	$secpasswd = ConvertTo-SecureString $RemotePowerShellPwd -AsPlainText -Force
	$cred  = New-Object System.Management.Automation.PSCredential ($RemotePowerShellUser, $secpasswd)

    $CimSessionOption = New-CimSessionOption -SkipRevocationCheck

    $ADSsession = New-CimSession -Name "ServicesInstall"  -ComputerName $ActiveDirectoryServers -SessionOption $CimSessionOption
	$PortalSession   = New-CimSession -Name "PortalInstall"    -ComputerName $PortalServers -SessionOption $CimSessionOption
	$ServiceSsession = New-CimSession -Name "ServicesInstall"  -ComputerName $MiddleTierServers -SessionOption $CimSessionOption
	$SPServerSession    = New-CimSession -Name "SharPointInstall" -ComputerName $SharePointServer -SessionOption $CimSessionOption #-Authentication CredSSP -Credential $cred
    #$SPWFEsSession    = New-CimSession -Name "SharPointWFEInstall" -ComputerName $SharePointWFEs -SessionOption $CimSessionOption #-Authentication CredSSP -Credential $cred
    $SPAPPsSession    = New-CimSession -Name "SharPointAPPInstall" -ComputerName $SharePointAPPs -SessionOption $CimSessionOption #-Authentication CredSSP -Credential $cred
    $SqlSession = New-CimSession -Name "SqlInstall"  -ComputerName $SQLServers -SessionOption $CimSessionOption
	$AllServersSsession = New-CimSession -Name "AllServersInstall"  -ComputerName $SQLServers -SessionOption $CimSessionOption

	$Profile = "Domain"

	#All Servers
		CreateAllPortsRules -CimSession $AllServersSsession -Profile $Profile
		# Need for Ping and AppFabric / SharePoint Distributed Cache
		EnableInboundFirewallRule -RuleName "File and Printer Sharing (Echo Request - ICMPv4-In)" -Profile $Profile -CimSession $AllServersSsession
		# Open SMB-In to allow accessing UNC Shares
		EnableInboundFirewallRule -RuleName "File and Printer Sharing (SMB-In)" -Profile $Profile -CimSession $AllServersSsession

   if (($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "AD"))
    {
        ConfigureFirewallAD -CimSession $ADSsession -Profile $Profile
    }

    if ((($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "Portal")) -and ($UseUnifiedPortalMiddleTier -eq $false))
    {
		ConfigureFirewallServices -CimSession $PortalSession -Profile $Profile
    }

    if ((($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "Services")) -and ($UseUnifiedPortalMiddleTier -eq $false))
    {
		ConfigureFirewallServices -CimSession $ServicesSession -Profile $Profile
    }

    if ((($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "Portal") -or ($DeploymentTier -eq "Services")) -and ($UseUnifiedPortalMiddleTier -eq $true))
    {
		ConfigureFirewallServices -CimSession $ServicesSession -Profile $Profile
    }

    if (($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "SharePoint"))
    {	
		ConfigureFirewallSharePoint -CimSession $SPServerSession -Profile $Profile
        #ConfigureFirewallSharePoint -CimSession $SPWFEsSession -Profile $Profile
        ConfigureFirewallSharePoint -CimSession $SPAPPsSession -Profile $Profile
    }

	if (($DeploymentTier -eq "AllAzure") -or ($DeploymentTier -eq "SQL"))
    {
		ConfigureFirewallSql -CimSession $SqlSession -Profile $Profile
		#EnableFirewallRulesForOtherSqlServices -CimSession $SqlSession -Profile $Profile
    }
	
    if ($DeploymentTier -ne "OnPrem")
    {
        #Nothing to do.
	}
	
    if ($DeploymentTier -ne "OnPrem")
    {
        #Nothing to do.
	}
	
	#Close all open remote PowerShell sessions before ending.
	Get-CimSession | Remove-CimSession

    if (!($Host.Name.Contains("ISE")))
    {
        Stop-Transcript
    }
}

Main -DeploymentTier $DeploymentTier -EnvironmentName $EnvironmentName -GlobalConfigurationFileName $GlobalConfigurationFileName