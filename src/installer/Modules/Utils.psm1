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
#Defines module variables
if(!$PSScriptRoot){ $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent }
$PSScriptParentRoot=Resolve-Path("$PSScriptRoot\..")


function Expand-Package
{
	param(
		$filePath,
		$destDir,
		$extractMode='x' #e can be useful also: https://sevenzip.osdn.jp/chm/cmdline/commands/extract.htm
	)

	Write-Host "Location: " $(Get-Location)
		
	
	$7z=$(Resolve-Path("$PSScriptRoot\..\7za.exe"))
	Write-Host $7z $command
	& "$7z" $extractMode "$filePath" -y -o"$DestDir"  | Write-Host

}

function Resolve-RelativePath
{
	param(
		$path
	)
	
	$newPath=$path
	# if relative path we will make an absolute one
	if([System.IO.Path]::IsPathRooted($path) -eq $false)
	{
		$newPath=Join-Path $PSScriptParentRoot $path;
		$newPath=$ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($newPath);
	}
	return $newPath
}


#
# Helper functions, may not be used, but could be useful as they can be called from the silent-install script configuration
#




#
# copy Files using robocopy
#
function Copy-Files {
	param(
	$source,
	$destination ,
	$files = ''
	)
	
	$options = @('/NJH', '/NJS')
	
	$CmdLine = @($source, $destination, $files) 
	& 'robocopy.exe' $CmdLine # + $options

}

#
# Load a powershell module only if not loaded
#
function FindandLoadModule()
{
    # This function looks for a module and loads it if
    # it is not already laoded and is available.
    # Returns true if end result is module loaded, false otherwise
    #
    param([string]$moduleName) 
    
    write-host "Will look for module [$moduleName] and load it, if appropriate ..."    
    if(-not(Get-Module -name $moduleName))
    {
        if(Get-Module -ListAvailable | Where-Object { $_.name -eq $moduleName })
        {
            Import-Module -Name $moduleName
            return $true
        } 
        else 
        { 
            write-host "Module [$moduleName] is not available on this machine ..."
            return $false 
        }
    }

    write-host "Module [$moduleName] is already loaded ... " 
    return $true 
}

#
# To create a scheduled task that will run at startup
#
Function CreateStartupTask
{
	Param(
	[string]$name,
	[string]$description,
	[string]$scriptName,
	[string]$WorkingDirectory
	)

	$arg="-NoProfile -WindowStyle Hidden -command ""$WorkingDirectory\$scriptName"""
	$action = New-ScheduledTaskAction -Execute 'Powershell.exe' -Argument $arg -WorkingDirectory $WorkingDirectory 

	  # if encounter an issue here: mofcomp C:\Windows\System32\wbem\SchedProv.mof
	  # this will recompile the module
	  # source: https://powershell.org/forums/topic/method-newtriggerbyonce-not-found/
	$trigger =  New-ScheduledTaskTrigger -AtLogOn
	
	Register-ScheduledTask  -TaskName $name `
							-Action $action `
							-Description $description `
							-Trigger $trigger `
							-RunLevel Highest
			
}	   


#
# Removes a Scheduled task
#
Function RemoveTask
{
	Param ([string] $name)

	$taskExists = Get-ScheduledTask | Where-Object {$_.TaskName -like $name }

	if($taskExists) {
		Unregister-ScheduledTask -TaskName $name -Confirm:$false
		Add-Log "Task $name was removed"
	}
	else {
		Add-Log  "Task $name did not exist so it was not needed to remove it."
	}


}
