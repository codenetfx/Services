﻿<#
.SYNOPSIS
    Give an account access to a certificate located in the local computer's personal certificate store
	 
.PARAMETER user
	The user or group to grant permissions to in the format domain\user.

.PARAMETER Subject
    The subject of the certificate.  Specify the subject in double quotes.  Example Subject:  CN=localhost, O=UL, OU=Aria, L=Northbrook, S=IL, C=US

.DESCRIPTION
    This can be used when a service (or the IIS app pool account) needs access to a certificate in the local computer's personal certificate store.
    If read permission is granted, then the user doesn't need to be an administrator to access the certificate.
#>
param (
[Parameter(Mandatory=$true)][string]$user,
)