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
# OSISetup.psm1
#

#
# This function does the work of extracting the files prior the installation
# It makes usage of 7z to do so.
# Finally it calls RunOSISetupKit method to run the installation
function ExtractAndRunOSISetupKit($packagesDir,$extractedPackagesDir, $installConfig,$forceUnzip=$false,$extractMode='x') 
{ 
	# e.g. SetupPackages\PIDataArchive_2016_.exe
	$packagePath=Join-Path $packagesDir $installConfig.package
	$packageName=(Get-Item $packagePath).Basename
	$packageExtractedDir="$extractedPackagesDir\$packageName"

	#Test is the package is already present
	#Extracting the package if necessary
	if(![System.IO.Directory]::Exists("$packageExtractedDir") -or $forceUnzip)
	{
		Add-Log "Extracting package $packagesDir\$($installConfig.package)"
		Expand-Package -filePath $packagePath  -destDir $packageExtractedDir -extractMode $extractMode
	}	

	# getting the folder where the setup.exe is...
	$packageExtractedDir=FindExeDirectory -dir $packageExtractedDir

	#Generate the .ini file for the installation
	$silent="silent_install.ini"
	CreateNewIniFile $packageExtractedDir $silent $installConfig
 
	# start the installation
    $displayName=$installConfig.displayName					# name the software for display purposes
	$workingDir=$packageExtractedDir                        # setup kit
    $setup="Setup.exe"
	

    Add-Log "Ready to install..."
	RunOSISetupKit $displayName $setup $workingDir $silent



}

# generate a new silent.ini file
function CreateNewIniFile($sourceDir,$newIniFileName,$installConfig)
{
 
	$silentFilepath=Join-Path $sourceDir "silent.ini"
	Add-Log "Generating a new silent setup file from $silentFilepath"

	$silentConfig = Get-Content $silentFilepath
	foreach($replace in $installConfig.IniFileReplaces)
	{
		$silentConfig = $silentConfig.Replace($replace.default, $replace.newSetting)
	}
	
	#writes the content of the .ini file that will be used for the silent installation
    Set-Content -Path "$sourceDir\$newIniFileName" -Value $silentConfig -Force
}


function RunOSISetupKit($displayName,$setupExe,$workingDir,$silent)
{
    # Provide a general function to install software that has the same characteristics 
	# $setupExe - the .exe to be installed
	# $workingDir - where the files will be extracted to by the .exe
	# $silent - updates silent.ini file to copy into $workingDir before running the setup.exe from the $workingDir
	
	$fTime = [System.Diagnostics.Stopwatch]::StartNew()
    Add-Log "RunOSISetupKit $setupExe - $displayName ..."
	
    # Verify that the $setupExe file exists
    $path=Join-Path $workingDir $setupExe
    if (! (test-path -path $path -pathtype leaf))
    {
       throw "Installer program not found: $setupExe"
    }
	
    pushd $workingDir

    $process = Start-Process -FilePath ".\$setupExe" @("-f $silent") -PassThru -Wait
	
    if (($process.ExitCode -ne 0) -and ($process.ExitCode -ne 3010))
    {
       throw "Error installing, error code: $($process.ExitCode)"
       popd
    }
	
	Add-LogExecTime($fTime)

    # return to starting directory
    #
    popd
    
}

#
# This step is necessary because someinstall kits have packaged the installation within a folder
# this function will make sure to find the folder level at whish the setup.exe is located
function FindExeDirectory($dir)
{
	$file=Get-ChildItem -Path $dir -Filter 'Setup.exe' -Recurse;
	$fileDir=split-path $file.fullname;
	return $fileDir
}
