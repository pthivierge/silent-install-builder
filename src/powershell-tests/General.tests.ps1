
# Sets the right location for the script
$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Set-Location "$PSScriptRoot\\..\\installer"


Import-Module .\Modules\Utils.psm1

# This test extracts a zip file in temp folder.  Not exactly a repeatable test on all machines
# 
#Describe "can unzip a setup kit" {
#	Context "Exists" {
#		It "Runs" {
#			$curdir=Get-Location
#			Expand-Package -filePath "$PSScriptRoot\Resources\PI-Analysis-Service_2015_.exe" -destDir 'c:\Temp\PIanalysisServiceTest'
#		}
#	}
#}