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
#
# Executable.psm1
#
#Defines module variables
if(!$PSScriptRoot){ $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent }
$PSScriptParentRoot=Resolve-Path("$PSScriptRoot\..")

function Install-Executable 
{ 
	param(
		$packagePath,
		$argumentsAsArray
	)
	
	
    $fTime = [System.Diagnostics.Stopwatch]::StartNew()
	    
	Add-Log  "Installing $packagePath"
	$passedArgs=$argumentsAsArray -join ' '
	Add-Log "Arguments: $passedArgs"
	    	
    $rc = Start-Process -FilePath $packagePath -ArgumentList $argumentsAsArray -Wait -PassThru
    if (($rc.ExitCode -ne 0) -and ($rc.ExitCode -ne 3010))   # 3010 means ok, but need to reboot
    {
       throw "Installation process returned error code: $($rc.ExitCode)"
    }
    
    Add-LogExecTime($fTime)
}