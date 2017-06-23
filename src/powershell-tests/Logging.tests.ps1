
# Sets the right location for the script
Write-Host $("PSRoot $PSScriptRoot")
$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Set-Location "$PSScriptRoot\\..\\installer"


Import-Module .\Modules\Logger.psm1

Describe "can create logs" {
	Context "Exists" {
		It "Runs" {

		    Write-Host $PSScriptRoot
			
			Logger\InitModule -logfileName 'PSTestLogs.txt'
			Add-Log 'test'
			Test-Path 'PSTestLogs.txt' | should be $true
		}
	}
}

Describe "can create execution time logs" {
	Context "Exists" {
		It "Runs" {
			
			Logger\InitModule -logfileName 'PSTestLogs.txt'
			$fTime = [System.Diagnostics.Stopwatch]::StartNew()
			Start-Sleep 0.1
			Logger\Add-LogExecTime $fTime
			Test-Path 'PSTestLogs.txt' | should be $true
		}
	}
}

