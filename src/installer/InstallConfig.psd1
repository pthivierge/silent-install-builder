# InstallConfig.psd1
# Generated on 2016-10-24 1:26:40 AM from the silent package builder v 1.0.0.0
# 
# 
# 
@{
    sourcePackagesDir='SetupPackages'
    extractedPackagesDir='ExtractedPackages'
    installPackages=
    @(
        @{
            install='1'
            type='PSCodeBlock'
            displayName='PS Task'
            PSCodeBlock=
            @(
                'Write-Host configFile is: $CONFIG_FILE',
				'Add-Log "cooolll!!!!"',
				'Write-Host 3'
            )
        }
    )
}