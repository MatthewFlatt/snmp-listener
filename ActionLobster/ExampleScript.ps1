param (
    [Parameter(Mandatory=$false)]
    [String]
    $SqlServerConnectionString,
    [Parameter(Mandatory=$false)]
    [String]
    $ObjectName,
	[Parameter(Mandatory=$false)]
    [String]
	$AlertId,
	[Parameter(Mandatory=$false)]
    [String]
    $AlertType,
	[Parameter(Mandatory=$false)]
    [String]
    $AlertDescription,
	[Parameter(Mandatory=$false)]
    [String]
    $EventTime,
	[Parameter(Mandatory=$false)]
    [String]
    $CurrentSeverity,
	[Parameter(Mandatory=$false)]
    [String]
    $TargetObject,
	[Parameter(Mandatory=$false)]
    [String]
    $DetailsUrl,
	[Parameter(Mandatory=$false)]
    [String]
    $StatusChangeType,
	[Parameter(Mandatory=$false)]
    [String]
    $PreviousWorstSeverity,
	[Parameter(Mandatory=$false)]
    [String]
    $MachineName,
	[Parameter(Mandatory=$false)]
    [String]
    $ClusterName,
	[Parameter(Mandatory=$false)]
    [String]
    $GroupName
)

Write-Host "Alert ID : $AlertId"
Write-Host "Sql Server connection string : $SqlServerConnectionString"
Write-Host "Object name : $ObjectName"