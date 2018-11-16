$location = Get-Location
$location = Join-Path $location "AlertActioner.exe"
New-Service -Name "AlertActioner" -BinaryPathName $location -DisplayName "Alert actioner for SQL Monitor" -Description "Alert actioner for SQL Monitor" -StartupType Automatic -Credential "domain\test"
