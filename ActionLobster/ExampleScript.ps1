param (
    [Parameter(Mandatory=$false)]
    [String]
    $SqlServerConnectionString,
    [Parameter(Mandatory=$false)]
    [String]
    $ObjectName    
)

Write-Host "Sql Server connection string : $SqlServerConnectionString"
Write-Host "Object name : $ObjectName"