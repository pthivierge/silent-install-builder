# Copyright 2017 OSIsoft, LLC
# 
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# 
# <http://www.apache.org/licenses/LICENSE-2.0>
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
if(!$PSScriptRoot){ $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent }



# here we are importing the functions from different modules.
# functions can be called either from this file, or from a powershell code-block that comes from an install packages (a powershell code block).
Import-Module $PSScriptRoot\Modules\Logger
Import-Module $PSScriptRoot\Modules\Utils
Import-Module $PSScriptRoot\Modules\Executable
Import-Module $PSScriptRoot\Modules\OSISetup
Import-Module $PSScriptRoot\Modules\PIDataArchive
Import-Module $PSScriptRoot\Modules\AF
Import-Module $PSScriptRoot\Modules\Windows
Import-Module $PSScriptRoot\Modules\SQL

#Constants - do not change or you may need to change the UserSettings.settings in the SilentPackagesBuilderGUI
$CONFIG_FILE='InstallConfig.psd1'
$CONFIG_FILE_UPDATED='InstallConfig - updated.psd1'
$VARIABLES_FILE='SetupVariablesDefinitions.psd1'
$PROGRESS_FILE='silent-installation-progressConfig.xml'
$LOG_FILE='silent-installation-logs.txt'
$SCHD_TSK_NAME='Silent-Install-Resume'

# will wait a little before switching to the next package in case the last package asked for a restart.
# time is in seconds
$WAIT_TIME_S_BETWEEN_PACKAGES=7

#intitializes the logger module
Logger\InitModule -logFileName $LOG_FILE

# Stopwatch
$StartTime = Get-Date -f "MM-dd-yyyy HH:mm:ss"
Add-Log "Silent Installation Started at $StartTime"
$Time = [System.Diagnostics.Stopwatch]::StartNew()


function main() {


	try	{
		
		$settings=GetSettings;
		$settings.sourcePackagesDir=Resolve-RelativePath $settings.sourcePackagesDir
		$settings.extractedPackagesDir=Resolve-RelativePath $settings.extractedPackagesDir
		

		# wait before proceding in case there is already a package installed
		while((get-process | where -Property ProcessName -like 'msiexec*').count -gt 0 -or (get-process | where -Property ProcessName -like 'setup*').count -gt 0)
		{
			Write-Host "There is already an installation running, waiting until it completes to resume (checking every minute)... "
			sleep 60
		}
		
		CreateScheduledTaskToResumeInstall

		foreach($package in $settings.installPackages) {

			Add-Log "Setup will proceed with package $($package.displayName) - Package: $($package.package) - Type: $($package.type) - Install: $($package.install)"

			if($package.install -eq 1)
			{
				switch($package.type)
				{
					
					'OSIAutoExtractSetupPackage'
					{
						 OSISetup\ExtractAndRunOSISetupKit -packagesDir $settings.sourcePackagesDir -extractedPackagesDir $settings.extractedPackagesDir -installConfig $package
					}

					'PIDataArchive' 
					{

						 PIDataArchive\InstallPIDataArchive -packagesDir $settings.sourcePackagesDir -extractedPackagesDir $settings.extractedPackagesDir -installConfig $package
					}

					'Executable'
					{
						$path=Join-Path $settings.sourcePackagesDir $package.package  
						Executable\Install-Executable -PackagePath $path -argumentsAsArray $package.arguments
					}

					'PowershellCodeBlock'
					{
						# for a script we do save the progress before, we assume it will do things right... if it restarts the conputer, we do not want it to run again
						SaveInstallationProgress -package $package
						RunDynamicPSCode $package.PSCodeBlock

					}
				}
			}

			SaveInstallationProgress -package $package

			# here we will wait a little before switching to the next package in case the last package asked for a restart.
			Start-Sleep -s $WAIT_TIME_S_BETWEEN_PACKAGES
		
		}

		RemoveTask $SCHD_TSK_NAME

		InstallSummary

					
		# Implementation idea
		# to add common software this could be an idea - requires a network connection
		# e.g. to install chrome and notepad++, there is only two commands to run:
		#
		# Commands:
		# msiexec.exe /i http://go.just-install.it
		# just-install notepad++ chrome 

	}
	catch {
		Add-ErrorLog ($error[0] | out-string)
		# if debugging you may turn on this line so console stays visible
		# Note that if the script runs as a result of a reboot and poweshell is started by the scheduled task, this will keep the process running ( hanging) at the end.
		# 
		# PressKeyToContinue
		return

	}
	finally {
		# put here any action that should occur even if the script stops with an error
		
	}
}

function GetSettings
{
	# if the script already partly ran, we get the saved configuration file
	if(![System.IO.File]::Exists("$PSScriptRoot\$PROGRESS_FILE"))
	{
		Add-Log "Loading settings from $CONFIG_FILE"
		$settings=Import-LocalizedData -BaseDirectory $PSScriptRoot -FileName $CONFIG_FILE;
		$settings=ApplySetupVariables $settings
	}
	else
	{
		# in this case, there was a reboot, install is continuing.

		Add-Log "This script was ran already. Loading settings from $PROGRESS_FILE. Any remaining steps will be continued."
		$settings = Import-CliXml "$PSScriptRoot\\$PROGRESS_FILE";
		
	}
	return $settings;
}

# this function will replace variables 
function ApplySetupVariables($settings)
{

	Add-Log "trying to load defined variables from $VARIABLES_FILE. If any..."

	# checking if the file that defines the variables exist - if it does not exist we just return the setting as is.
	if([System.IO.File]::Exists("$PSScriptRoot\$VARIABLES_FILE"))
	{
		# load variables from the variable file
		Add-Log "Variable file found $VARIABLES_FILE"
		$variables=Import-LocalizedData -BaseDirectory $PSScriptRoot -FileName $VARIABLES_FILE;

		Add-Log "Replacing variable values"

		#get original config from the text file
		$config=[System.IO.File]::ReadAllText("$PSScriptRoot\$CONFIG_FILE")

		# do the actual replacement
		foreach ($var in $variables.GetEnumerator()) {
			$variableName=$var.Name.ToString();	
			$variableValue=$var.Value.ToString();	
			$config=$config.Replace("{$variableName}","$variableValue")
		}

		
		[System.IO.File]::WriteAllText("$PSScriptRoot\$CONFIG_FILE_UPDATED",$config)

		$replaced_settings=Import-LocalizedData -BaseDirectory $PSScriptRoot -FileName $CONFIG_FILE_UPDATED;
		
		return $replaced_settings;

	}

	else
	{
		#in the case there is no variable file, we just continue with the settings as is.  
		# it may mean there is no variables in the configuration.
		Add-Log "Variable file not found: $VARIABLES_FILE - continuing assuming there is no variables in the configuration"
		return $settings;
	}



}

function RunDynamicPSCode($CodeBlockArray)
{

	$finalscript = ""
	$CodeBlockArray |foreach {
		$finalscript += $_.ToString() + "`n"                        
	}
	$finalscript = $ExecutionContext.InvokeCommand.NewScriptBlock($finalscript)
	& $finalscript

}


function SaveInstallationProgress($package)
{
	$package.install=0;
	$settings | Export-CliXml "$PSScriptRoot\$PROGRESS_FILE";
}

function PressKeyToContinue
{
	# these 2 lines are handy for debugging the script, especially if you are calling the script with right click, run with powershell
	# for a lot of debugging you should consider calling the setup.ps1 from a .bat file, this should work better.
	Write-Host -NoNewLine 'Press any key to continue...';
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}

function InstallSummary()
{
	$elapsed = $Time.Elapsed.ToString()
	$EndTime = Get-Date -f "MM-dd-yyyy HH:mm:ss"
	Add-Log "Silent  Installation finished at $EndTime"
	Add-Log "Instalation duration: $elapsed"
	Add-Log "Installation completed."
}

function CreateScheduledTaskToResumeInstall()
{
	
	$taskExists = Get-ScheduledTask | Where-Object {$_.TaskName -like $SCHD_TSK_NAME }

	if($taskExists) {
		Add-Log "Task $SCHD_TSK_NAME already exist. No need to create it"
	}
	else {
		Add-Log  "A Schedule task with name: $SCHD_TSK_NAME, will be created to resume the installation after a reboot from an installation package."
		Add-Log  "The scheduled task will be removed after the installation completes.  Please make sure the task was removed after completion: check at the end of the logs."

			Utils\CreateStartupTask -name $SCHD_TSK_NAME `
							-description 'Resume the installation after a reboot from an installation package' `
							-scriptName 'setup.ps1' `
							-WorkingDirectory $PSScriptRoot `
	}


}


main