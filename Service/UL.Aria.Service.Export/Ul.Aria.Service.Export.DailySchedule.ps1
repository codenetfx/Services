
#read config, determine if folder should be created.
$outputPathXPath  = "/configuration/appSettings/add[@key='UL.Aria.Service.Export.Path']/@value"
$config = ([xml](Get-Content .\UL.Aria.Service.Export.exe.config))
$outputPath = (Select-Xml $outputPathXPath $config).Node.'#text'
$folder = Split-Path $outputPath -Parent
if ((Test-Path $folder) -eq $false)
    {
        mkdir $folder
    }

#create job
$trigger = New-JobTrigger -Daily -At "02:00"
try 
{
    $job = Get-ScheduledJob -Name ExportProjects
    Write-Host "ExportProjects: Job found, removing it then adding new."
    Unregister-ScheduledJob -InputObject $job
}
catch
{
    Write-Host "ExportProjects: Job not found, adding new."
}
Register-ScheduledJob -Name ExportProjects -Trigger $trigger -ScriptBlock `
     {
         .\UL.Aria.Service.Export.exe
     }

#verify
try 
{
    $job = Get-ScheduledJob -Name ExportProjects
    if ($job -ne $null)
    {
        Write-Host "ExportProjects: Job created successfully."    
    }
    else
    {
        throw "ExportProjects: Job was not created successfully, check errors above."
    }
}
catch
{
    throw "ExportProjects: Job was not created successfully, check errors above."
}


      