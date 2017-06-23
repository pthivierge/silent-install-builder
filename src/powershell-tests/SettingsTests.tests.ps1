# Sets the right location for the script
$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Set-Location "$PSScriptRoot\\..\\installer"

Import-Module .\Modules\Utils.psm1



Describe "Get Settings relative" {
	Context "Exists" {
		It "Runs" {
			#$curdir=Get-Location
			#$settings=Utils\GetSettings -baseDirectory ".\..\tests" -fileName "settings.psd1"
			$settings=Import-LocalizedData -BaseDirectory ".\..\powershell-tests" -FileName "settings.psd1";
			#$settings | Export-CliXml foo.xml
			$settings.extractedPackagesDir | should be 'ExtractedPackages'

		}
	}
}