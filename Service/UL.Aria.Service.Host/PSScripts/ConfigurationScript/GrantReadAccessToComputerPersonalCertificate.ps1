<#
.SYNOPSIS
	Grant permission to a certificate in the computer's certificate store
	
.DESCRIPTION
	Provide access permission to an account( or NTAUTORITY\NETWORK SERVICE) to be able to access the certificate by
	updating the ACL on the encrypted Private Key file corresponding to this certificate. 
	If an account running a Windows NT Services or an IIS Application Pool needs access to a certifcate 
	in the personal computer certificate store, you will need to grant that account read permission to that certificate.
#>
param
(
[Parameter(Mandatory=$false)][String]$FriendlyName,
[Parameter(Mandatory=$false)][String]$Subject,  
[Parameter(Mandatory=$true)][String]$AccountName
)

#Get the thumbprint a certificate
function GetCertificateThumbprint([Parameter(Mandatory=$false)][String]$FriendlyName, [Parameter(Mandatory=$false)][String]$Subject)
{
    if (![string]::IsNullOrEmpty($FriendlyName))
    {
	    $Cert = Get-ChildItem -Recurse Cert: | Where-Object {$_.FriendlyName -eq $FriendlyName}
    }
    else
    {
        $Cert = Get-ChildItem -Recurse Cert: | Where-Object {$_.Subject -eq $Subject}
    }

    return $Cert.ThumbPrint
}

function GrantReadAccessToComputerPersonalCertificate([Parameter(Mandatory=$true)][String]$ThumbPrint, [Parameter(Mandatory=$true)][String]$AccountName)
{

	#Find the physical certificate file in the certificate store using the certificate's thumbprint.
	#All the files for the certificates are located here.
	$keyName=(((Get-ChildItem Cert:\LocalMachine\My | Where-Object {$_.Thumbprint -like $ThumbPrint}).PrivateKey).CspKeyContainerInfo).UniqueKeyContainerName
	$keyPath = "C:\ProgramData\Microsoft\Crypto\RSA\MachineKeys\"
	$fullPath=$keyPath+$keyName

	#Update the ACL on the encrypted private key file.
	#Give the account readonly access.
	$acl=Get-Acl -Path $fullPath
	$permission=$AccountName,"Read","Allow"
	$accessRule=new-object System.Security.AccessControl.FileSystemAccessRule $permission
	$acl.AddAccessRule($accessRule)
	Set-Acl $fullPath $acl
}

function Main
(
[Parameter(Mandatory=$false)][String]$FriendlyName,
[Parameter(Mandatory=$false)][String]$Subject,  
[Parameter(Mandatory=$true)][String]$AccountName
)
{
    $ThumbPrint = GetCertificateThumbprint -FriendlyName $FriendlyName -Subject $Subject
    if($ThumbPrint -eq $null)
    {
        Write-Host "Can't locate Certificate."
        exit -1
    }

	GrantReadAccessToComputerPersonalCertificate -ThumbPrint $ThumbPrint -AccountName $AccountName
}

Main -FriendlyName $FriendlyName -Subject $Subject -AccountName $AccountName
