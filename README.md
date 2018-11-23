# Windows service to listen for SQL Monitor alerts via SNMP trap and run a PowerShell script controlled via rules.

Rules read from json files configured in AlertActioner.exe.config. Can have multiple files seperated by comma.

Example rule:

    {
      "AlertType": [
        "Backup overdue"
      ],
      "ActionFrom": "08:00",
      "ActionTo": "18:00",
      "IncludedServers": ["server1"],
      "IncludedGroups": ["group1],
      "MinimumSeverity": "Low",
      "Priority": 1,
      "PowerShellScriptFile": "ExampleScript.ps1"
    }
    
 Fields can be blank or removed to include all, only matching rules with the highest value in Priority will result in the PS script being run.
 
 Use InstallAlertActionerService to install as a Windows service, replacing the username
 
 PowerShell files to run must be in the root directory of where the app is running.
