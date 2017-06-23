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

# The functions in this file were not tested - variables needs to be added to function signatures as the variable used are not 
# available in this file, this code was re-factored in a module as it might be useful


function CreateAFDatabase($AFDatabase)
{

	# Create an AF database on the local AF Server
	
	$errorStatus = 0

	# if Database isn't specified, use the default name
	if ($AFDatabase -eq $null) {
		$AFDatabase="TestDatabase"
	}
	# See if we can load the assembly and create a PISystems object
	try {
		[System.Reflection.Assembly]::LoadWithPartialName("OSIsoft.AFSDK") | Out-Null
		$afServers = new-object OSIsoft.AF.PISystems
	} catch { 
		
		Add-ErrorLog "Create AF Database Error: Unable to Load AFSDK"
		$errorstatus=1
	}
	# Connect to the AF Server and if the database does not exist, create it
	if ($errorstatus -eq 0) {
	try { 
		$AFServerName=[System.Net.Dns]::GetHostByName((hostname)).HostName
		$afServer = $afServers[$AFServerName]
		$afServer.Databases.Refresh | Out-Null
		if ( $afServer.Databases[$AFDatabase] -eq $null) {
			$afServer.Databases.Add($AFDatabase) | Out-Null
			if ( $afServer -eq $null ) {
				$errorStatus=1
			} else {
				Write-Output "AF Database: $AFDatabase created!"
				$afServer=$null
			}
		} else {
			Write-Output "AF Database: $AFDatabase already exists, skipping database creation."
		}
	}
	catch { 
		$errorStatus = 1
	} 
	}
	if ($errorStatus -eq 1) { 
		Add-ErrorLog "Error: Unable to connect to AF Server: $AFServerName and create AF database: $AFDatabase"
		Add-ErrorLog "Check that the AF Server is installed and running"
		Add-ErrorLog "Alternatively Create an AF Database manually"
	}
}

function UpdateAFBackup()
{
    $fTime = [System.Diagnostics.Stopwatch]::StartNew()
    write-host "Updating AF backup script to specify the SQL Instance name: $SQLInstanceName ..."

    $afbackup = "AF\SQL\afbackup.bat"

	# See if we can get the path to the AF backup script
    try {
    if (test-Path "${env:PIHOME64}\$afbackup")
    { 
       $afbackuppath="${env:PIHOME64}\$afbackup"
    } 
    elseif (test-Path "${env:PIHOME}\$afbackup") { 
       $afbackuppath="${env:PIHOME}\$afbackup"
    }  
    }
    catch {
       # No action
    }
    try {
		if (($afbackuppath -ne $null) -and (Test-Path $afbackuppath))
		{
			# Remove the read only attribute from the file if set
			Set-ItemProperty $afbackuppath -Name IsReadonly -Value $false
			# load the file
			$content = Get-Content "$afbackuppath"
			# search for the configuration of the sql instance
			if ($content -match "^SET SQLINSTANCE=*")
			{
				# update the SQL instance to match that installed earlier
				write-host "Updating AF backup script: $afbackuppath to reference SQL Instance: $SQLInstanceName"
				write-host "Verify changes by 1) checking for $SQLInstanceName in $afbackuppath and 2) the results of running a backup" 
				$content -replace "^SET\s*SQLINSTANCE=.*$", "SET SQLINSTANCE=$SQLInstanceName" | Set-Content $afbackuppath
			}
			else
			{
				write-host "Update failed for AF backup script: $afbackuppath. Update AF backup file manually to reference SQL Instance: $SQLInstanceName"       
			}	
		}
		else
		{
		write-host "Cannot find AF backup script. Update AF backup file manually to reference SQL Instance: $SQLInstanceName"
		}
	}
    catch 
    {
        write-host "Update failed for AF backup script. Update AF backup file manually to reference SQL Instance: $SQLInstanceName"
    }
    Add-LogExecTime($fTime)
}