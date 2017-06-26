
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




# the only difference when installing PI DA
# is that we need to copy the license in the folder where we extracted the file
function InstallPIDataArchive($packagesDir, $extractedPackagesDir,$installConfig)
{
	
	$packagePath=Join-Path $packagesDir $installConfig.package
	$packageName=(Get-Item $packagePath).Basename
	$packageDir="$extractedPackagesDir\$packageName"

	Add-Log "Copying placing the license file for the PI DA installation from $PSScriptParentRoot\pilicense.dat into $packageDir "
	
	New-Item $packageDir -ItemType directory -Force
	Copy-Item -Path "$PSScriptParentRoot\pilicense.dat" -Destination $packageDir -Force
	
	ExtractAndRunOSISetupKit -packagesDir $packagesDir -extractedPackagesDir $extractedPackagesDir -installConfig $installConfig -forceUnzip $true -extractMode 'e'

}



Function New-PIDataArchiveCollective([String]$PICollectiveName,[string]$PIDescription,[string]$PISecondaryMember,[string]$PIPrimaryMember)
{
	$Connection=Connect-PIDataArchive -PIDataArchiveMachineName $PIPrimaryMember
	#$Status=New-PICollective -Name $PICollectiveName -Description $PIDescription -Secondaries $PISecondaryMember -Connection ($Connection)
	$Status=New-PICollective -Name $PICollectiveName -Description $PIDescription -Connection ($Connection)
	Add-Log "New-PICollective $PICollectiveName creation command completed"  
}



Function Add-PIDataArchiveTrust([String]$PITrustName,[string]$PIIdentity,[string]$PIDescription,[string]$NetworkPath,[string]$PIDataArchiveName)
{
	$Connection=Connect-PIDataArchive -PIDataArchiveMachineName $PIDataArchiveName
	$Status=add-PITrust -Name $PITrustName -Identity $PIIdentity -Description $PIDescription -NetworkPath $NetworkPath -connection ($Connection)
	Add-Log "add-PITrust PROCESSED"
}


Function Add-MemberToPICollective([String]$PICollectiveName,[string]$PISecondaryMember,[string]$PIPrimaryMember)
{
	$Connection=Connect-PIDataArchive -PIDataArchiveMachineName $PIPrimaryMember
	$Status=Add-PICollectiveMember -Name $PISecondaryMember -PICollective (Get-PICollective -Connection ($Connection))
	Add-Log "Add-MemberToPICollective:: Collective Name: $PICollectiveName, Member added: $PISecondaryMember" 
}

Function Add-PIMappingToPIDataArchive([string]$WindowsAccountName,[string]$PIIdentity,[string]$PIDataArchiveName)
{
	$SID=(New-Object System.Security.Principal.NTAccount($WindowsAccountName)).Translate([System.Security.Principal.SecurityIdentifier]).value
	$Connection=Connect-PIDataArchive -PIDataArchiveMachineName $PIDataArchiveName
	$Status=add-PIMapping -Name $WindowsAccountName -Identity $PIIdentity -PrincipalName $SID -connection ($Connection)
	Add-Log "Add-PIMappingToPIDataArchive: New mapping added on $PIDataArchiveName for $WindowsAccountName" 
}



function Add-SpareArchives()
{
	#
	# Create and add two archives to the PI Data Archive
	#
    $fTime = [System.Diagnostics.Stopwatch]::StartNew()
    write-host "Configuring additional Archives ..."

    $archives="$archiveDir\piarch.spare.00"
    $archivesize=256;

    try {
		if ($env:PIServer -ne $null) { 
			for ($i=1; $i -lt 3;$i++) {
				write-host "creating archive: $archives$i"
				$rc = Start-Process  ((Get-Item $env:PIServer).FullName + "\adm\piarcreate.exe") -ArgumentList "`"$archives$i`" $archivesize" -Wait -PassThru -NoNewWindow
				if ($rc.ExitCode -ne 0)
				{
					write-host "Archive creation returned error code: $($rc.ExitCode)"
				}
				$rc = Start-Process -FilePath ((Get-Item $env:PIServer).FullName + "\adm\piartool.exe") -ArgumentList "-ar $archives$i" -Wait -PassThru -NoNewWindow
				if ($rc.ExitCode -ne 0)
				{
					write-host "Archive registration returned error code: $($rc.ExitCode)"
				}
			}
		} else {
		write-host "Unable to locate PI software directory, please create additional archives manually"
		}
    } catch {
        write-host "Error creating archives, please create additional archives manually" 
    } 
    Add-LogExecTime($fTime)
}

function ScheduleBackup()
{
    Write-Host "Scheduling a PI Data Archive backup task..."
    
    $rc = Start-Process -FilePath ((Get-Item $env:PIServer).FullName + "\adm\pibackup.bat") -ArgumentList "$backupDir -install" -Wait -PassThru -NoNewWindow
    if ($rc.ExitCode -ne 0)
    {
		write-host "Scheduling a backup task returned error code: $($rc.ExitCode)"
	}

}

function StartPIDataArchive()
{
    write-host "Starting PI Data Archive ..."

    if ($env:piserver -ne $null)
    {
        # do it gracefully, if I know where the PI server is ...
        #
        pushd ((Get-Item $env:piserver).FullName + "\adm")
        .\pisrvstart.bat 2>&1 | out-null 
        popd
    } else
    {
        Write-Host "Unable to find environment variable %piserver%.  Cannot start the PI Data Archive."
    }       
}

function StopPIDataArchive()
{
    #write-host "Stopping PI Data Archive ..."
    
    $proc = "pinetmgr"

    if (Get-Process $proc -ErrorAction SilentlyContinue)
    {
        if ($env:piserver -ne $null)
        {
            # do it gracefully, if I know where the PI server is ...
            #
            pushd ((Get-Item $env:piserver).FullName + "\adm")
            .\pisrvstop.bat 2>&1 | out-null 
            popd
        } else
        {
            Stop-Process -processname $proc -force -ErrorAction SilentlyContinue
        }
    }
    
    # The pisrvstop should be sufficient to stop all PI
    # processes related to the PI Data Archive.  However,
    # we should sanity check to be sure.
    #
    if (Get-Process $proc -ErrorAction SilentlyContinue)
    {
        Start-Sleep 5   # give it a *little* time before giving up
        if (Get-Process $proc -ErrorAction SilentlyContinue)
        {
            popd
            throw "Cannot stop $proc, aborting ..."
        }
    }

    # There is a bug in the PI Server 2012, where the PI Log Service
    # (32-bit & 64-bit) will just hang in some sort of deadlock.
    # This will kill it if necessary.
    # PI Known Issue:  
    #   75659 - pilogsrv hangs and System event log shows timeout on transaction 
    #           response from the pilogsrv service
    #
    $proc = "pilogsrv"
    if (Get-Process $proc -ErrorAction SilentlyContinue)
    {
        Stop-Process -processname $proc -force
    }
}