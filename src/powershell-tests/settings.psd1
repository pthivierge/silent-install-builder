# InstallConfig.psd1
# Generated on 2016-10-23 5:43:45 PM from the silent package builder v 1.0.0.0
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
            type='executable'
            displayName='SQL Server 2016 Express'
            package='en_sql_server_2016_express_x64_8762243.exe'
            arguments=
            @(
                '/QS',
                '/ACTION=Install',
                '/IACCEPTSQLSERVERLICENSETERMS=True',
                '/UpdateEnabled=False',
                '/FEATURES=SQLEngine,FullText',
                '/HIDECONSOLE',
                '/INSTANCENAME=SqlExpress',
                '/SQLCOLLATION=SQL_Latin1_General_CP1_CI_AS',
                '/SQLSYSADMINACCOUNTS=BUILTIN\ADMINISTRATORS',
                '/BROWSERSVCStartupType=2',
                '/NPENABLED=1',
                '/TCPENABLED=1',
                '/SQLSVCACCOUNT="NT AUTHORITY\SYSTEM"',
                '/AGTSVCACCOUNT="NT AUTHORITY\SYSTEM"'
            )
        },
        @{
            install='1'
            type='executable'
            displayName='SQL Server Management Studio'
            package='SSMS-Setup-ENU.exe'
            arguments=
            @(
                '/install',
                '/quiet',
                '/norestart'
            )
        },
        @{
            install='1'
            type='osi setup package'
            displayName='PI-AF-Server_2016'
            package='PI-AF-Server_2016_.exe'
            IniFileReplaces=
            @(
                @{
                    default='1 = /q'
                    newSetting='1 = /q /norestart'
                }
            )
        },
        @{
            install='1'
            type='osi setup package'
            displayName='PI-AF-Client_2016-SP2_.exe'
            package='PI-AF-Client_2016-SP2_.exe'
            IniFileReplaces=
            @(
                @{
                    default='ADDLOCAL=FD_AFSDK,FD_AFExplorer,FD_AFBuilder,FD_AFDocs'
                    newSetting='ADDLOCAL=FD_AFSDK,FD_AFExplorer,FD_AFDocs'
                }
            )
        },
        @{
            install='1'
            type='pi data archive setup package'
            displayName='PI Data Archive 2016'
            package='PIDataArchive_2016_.exe'
            IniFileReplaces=
            @(
                @{
                    default='12 = REBOOT=Suppress ALLUSERS=1 /qn SUPPRESS_PINS=1 PI_SERVER=localhost PI_ALIAS=localhost PI_TYPE=3'
                    newSetting='12 = REBOOT=Suppress ALLUSERS=1 /qn SUPPRESS_PINS=1 PI_SERVER=localhost PI_ALIAS=localhost PI_TYPE=3'
                },
                @{
                    default='16 = REBOOT=Suppress ALLUSERS=1 /qn'
                    newSetting='16 = REBOOT=Suppress ALLUSERS=1 /qn ARCHIVESIZE=128 NUMARCHIVES=1'
                }
            )
        }
    )
}
