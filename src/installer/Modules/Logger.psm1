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
# Logger.psm1
#
#Defines module variables
if(!$PSScriptRoot){ $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent }
$PSScriptParentRoot=Resolve-Path("$PSScriptRoot\..")


Function InitModule()
{
<#
.SYNOPSIS
Logger Module initialization

.DESCRIPTION 
Initializes logging by setting the target log file

#>
	param(
		[string]$logFileName='SilentInstallationLogs.txt',
		[string]$logFileDir=''
	)
	
	#sets the log file directory
	$scriptDir=$PSScriptParentRoot
	if($logFileDir -ne '')
	{
		$scriptDir=$logFileDir
	}
	
	# defines global variable
	$script:LogFile = "$scriptDir\$logFileName"
}


Function Add-Log
{
   Param ([string]$logstring)

   $caller=GCI $MyInvocation.PSCommandPath | Select -Expand Name
   $date=get-date -format HH:mm:ss:fff
   $logEntry="$($date) - $($caller) - $($logstring)"
   
   Add-content $Logfile -value $logEntry
   Write-Host $logEntry
}

Function Add-ErrorLog
{
   Param ([string]$logstring)

   $caller=GCI $MyInvocation.PSCommandPath | Select -Expand Name
   $date=get-date -format HH:mm:ss:fff
   $logEntry="$($date) - $($caller) - $("ERROR: $logstring")"
   
   Add-content $Logfile -value $logEntry
   Write-Error $logEntry
}


function Add-LogExecTime()
{
    param([System.Diagnostics.Stopwatch]$execTime)	
	$format='{0:c}';

   $logstring="Module installation completed in: $([String]::Format($format,$execTime.Elapsed))"

   # this code is duplicated on purpose (not using the Add-Log function), because the $caller is related the the function that called that specific function
   # so if calling Add-Log here we'll lose the context of the real caller
   $caller=GCI $MyInvocation.PSCommandPath | Select -Expand Name
   $date=get-date -format HH:mm:ss:fff
   $logEntry="$($date) - $($caller) - $($logstring)"
   
   Add-content $Logfile -value $logEntry
   Write-Host $logEntry


    
	
}
