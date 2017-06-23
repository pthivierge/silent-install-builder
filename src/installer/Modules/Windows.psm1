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



# The functions in this file were created to prepare to introduce the installation of PI Vision: the server features.


function InstalldotNetFeature
{

    $fTime = [System.Diagnostics.Stopwatch]::StartNew()
	write-output "Installing Prerequisites ..."

    # Step 1:  Install the .NET 3.5.1 Framework prerequisite
    # Note: Windows 2008 Server R2 requires that you install .NET 3.5 framework 
    # through the Server Manager.
    #
    $moduleName = "ServerManager"    
    if (FindAndLoadModule $moduleName)
    {
        #clear
        write-host "Adding as-net-framework via Server Manager ..."
        Add-WindowsFeature as-net-framework
        write-host ""
    }
		
	Add-LogExecTime($fTime)
}

# installs IIS with PI coresight requirements
# features references: https://www.iis.net/learn/install/installing-iis-85/installing-iis-85-on-windows-server-2012-r2
# command Get-WindowsFeature is really helful to see what is installed and what are the names of the features to install
function InstallIISfeatures()
{
	if(FindandLoadModule servermanager)
	{
		#import-module servermanager
		
		#application server
		$features=@("Application-Server") 
				
		#Windows Process Activation Service
		$features+=@("WAS-Process-Model","WAS-NET-Environment","WAS-Config-APIs")
		

		#Web Server Roles
		$features+=@("Web-Server","Web-WebServer")



		#Web Server features
		# COMMON HTTP FEATURES
		# Required:
		#  Static Content
		#  Default Document
		# Recommended:
		#  HTTP Errors
		#  HTTP Redirection
		$features+=@("Web-Static-Content","Web-Default-Doc","Web-Http-Errors","Web-Http-Redirect")

		#HEALTH AND DIAGNOSTICS
		#HTTP Logging,Logging Tools,Request monitor,Tracing 
		$features+=@("Web-Http-Logging","Web-Log-Libraries","Web-Request-Monitor","Web-Http-Tracing")

		#PERFORMANCE
		#Static Content Compression.Dynamic Content Compression
		$features+=@("Web-Stat-Compression","Web-Dyn-Compression")

		#SECURITY
		#Required: Windows Authentication, Request Filtering, URL Authorization
		#Recommended: IP and Domain Restrictions
		$features+=@("Web-Basic-Auth","Web-Windows-Auth","Web-Filtering","Web-Url-Auth","Web-IP-Security")


		#APPLICATION DEVELOPMENT
		# Required: .NET Extensibility 4.5, ASP.NET / ASP.NET 4.5
		#Recommended: ISAPI Extensions, ISAPI Filters
		$features+=@("Web-Net-Ext45","Web-Asp-Net45","Web-ISAPI-Ext","Web-ISAPI-Filter")

		#MANAGEMENT TOOLS
		#Required: None
		#Recommended: IIS Management Console,IIS Management Scripts and Tools,Management Service
		$features+=@("Web-Mgmt-Console","Web-Scripting-Tools","Web-Mgmt-Service")

		$features | Add-WindowsFeature

	}

}


function Update-Environment 
{
    $locations = 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
                 'HKCU:\Environment'

    $locations | ForEach-Object {
        $k = Get-Item $_
        $k.GetValueNames() | ForEach-Object {
            $name  = $_
            $value = $k.GetValue($_)

            if ($userLocation -and $name -ieq 'PATH') {
                Env:\Path += ";$value"
            } else {
                Set-Item -Path Env:\$name -Value $value
            }
        }

        $userLocation = $true
    }
}


Function Add-ComputerToLocalSecurityGroup([String]$Domain,[String]$LocalComputer,[String]$GroupName,[String]$ComputerNameToAdd)
{
	$Computer = [ADSI]"WinNT://$LocalComputer"
	$Groups = $Computer.psbase.Children | Where {$_.psbase.schemaClassName -eq "group"}

	ForEach ($Group In $Groups)
	{
		if($Group.Name -eq $GroupName) {
			$Group.add("WinNT://"+$domain+"/"+$ComputerNameToAdd+"$" )
			Break
			}
	}
	Add-Log "Computer $ComputerNameToAdd added to local security group of $LocalComputer"  -foregroundcolor "GREEN"
}


#------------------------------------------------------------------------------------------------
#
# Code imported from gallery.technet.microsoft.com
#
#------------------------------------------------------------------------------------------------



# Set-AutoLogon Method
# Copyright - VGSandz - 21/12/2016, 
# https://gallery.technet.microsoft.com/scriptcenter/Set-AutoLogon-and-execute-19ec3879
# See licence under licences\Autologon.Microsoft limited public license.txt
<#
.Synopsis
Here is the PowerShell CmdLet that would enable AutoLogon next time when the server reboots.We could trigger a specific Script to execute after the server is back online after Auto Logon.
The CmdLet has the follwing parameter(s) and function(s).
-DefaultUsername : Provide the username that the system would use to login.
-DefaultPassword : Provide the Password for the DefaultUser provided.
-AutoLogonCount : Sets the number of times the system would reboot without asking for credentials.Default is 1.
-Script : Provide Full path of the script for execution after server reboot. Example : c:\test\run.bat

Mandatory Parameters 
-DefaultUsername 
-DefaultPassword 


.Description
Here is the PowerShell CmdLet that would enable AutoLogon next time when the server reboots.We could trigger a specific Script to execute after the server is back online after Auto Logon.

.Example
Set-AutoLogon -DefaultUsername "win\admin" -DefaultPassword "password123"

.Example
Set-AutoLogon -DefaultUsername "win\admin" -DefaultPassword "password123" -AutoLogonCount "3"


.EXAMPLE
Set-AutoLogon -DefaultUsername "win\admin" -DefaultPassword "password123" -Script "c:\test.bat"

#>

Function Set-AutoLogon{

    [CmdletBinding()]
    Param(
        
        [Parameter(Mandatory=$True,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
        [String[]]$DefaultUsername,

        [Parameter(Mandatory=$True,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
        [String[]]$DefaultPassword,

        [Parameter(Mandatory=$False,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
        [AllowEmptyString()]
        [String[]]$AutoLogonCount,

        [Parameter(Mandatory=$False,ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
        [AllowEmptyString()]
        [String[]]$Script
                
    )

    Begin
    {
        #Registry path declaration
        $RegPath = "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon"
        $RegROPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce"
    
    }
    
    Process
    {

        try
        {
            #setting registry values
            Set-ItemProperty $RegPath "AutoAdminLogon" -Value "1" -type String  
            Set-ItemProperty $RegPath "DefaultUsername" -Value "$DefaultUsername" -type String  
            Set-ItemProperty $RegPath "DefaultPassword" -Value "$DefaultPassword" -type String
            if($AutoLogonCount)
            {
                
                Set-ItemProperty $RegPath "AutoLogonCount" -Value "$AutoLogonCount" -type DWord
            
            }
            else
            {

                Set-ItemProperty $RegPath "AutoLogonCount" -Value "1" -type DWord

            }
            if($Script)
            {
                
                Set-ItemProperty $RegROPath "(Default)" -Value "$Script" -type String
            
            }
            else
            {
            
                Set-ItemProperty $RegROPath "(Default)" -Value "" -type String
            
            }        
        }

        catch
        {

            Write-Output "An error had occured $Error"
            
        }
    }
    
    End
    {
        
        #End

    }

}



# Grant-UserRight Method
# Copyright - Tony MCP - 2017-02-16, 
# https://gallery.technet.microsoft.com/scriptcenter/Set-AutoLogon-and-execute-19ec3879
# See licence under licences\Autologon.Microsoft limited public license.txt
<#
VERSION   DATE          AUTHOR
1.0       2015-03-10    Tony Pombo
    - Initial Release

1.1       2015-03-11    Tony Pombo
    - Added enum Rights, and configured functions to use it
    - Fixed a couple typos in the help
#> # Source + Revision History

Set-StrictMode -Version 2.0

Add-Type @'
using System;
namespace PS_LSA
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Principal;
    using LSA_HANDLE = IntPtr;

    public enum Rights
    {
        SeTrustedCredManAccessPrivilege,      // Access Credential Manager as a trusted caller
        SeNetworkLogonRight,                  // Access this computer from the network
        SeTcbPrivilege,                       // Act as part of the operating system
        SeMachineAccountPrivilege,            // Add workstations to domain
        SeIncreaseQuotaPrivilege,             // Adjust memory quotas for a process
        SeInteractiveLogonRight,              // Allow log on locally
        SeRemoteInteractiveLogonRight,        // Allow log on through Remote Desktop Services
        SeBackupPrivilege,                    // Back up files and directories
        SeChangeNotifyPrivilege,              // Bypass traverse checking
        SeSystemtimePrivilege,                // Change the system time
        SeTimeZonePrivilege,                  // Change the time zone
        SeCreatePagefilePrivilege,            // Create a pagefile
        SeCreateTokenPrivilege,               // Create a token object
        SeCreateGlobalPrivilege,              // Create global objects
        SeCreatePermanentPrivilege,           // Create permanent shared objects
        SeCreateSymbolicLinkPrivilege,        // Create symbolic links
        SeDebugPrivilege,                     // Debug programs
        SeDenyNetworkLogonRight,              // Deny access this computer from the network
        SeDenyBatchLogonRight,                // Deny log on as a batch job
        SeDenyServiceLogonRight,              // Deny log on as a service
        SeDenyInteractiveLogonRight,          // Deny log on locally
        SeDenyRemoteInteractiveLogonRight,    // Deny log on through Remote Desktop Services
        SeEnableDelegationPrivilege,          // Enable computer and user accounts to be trusted for delegation
        SeRemoteShutdownPrivilege,            // Force shutdown from a remote system
        SeAuditPrivilege,                     // Generate security audits
        SeImpersonatePrivilege,               // Impersonate a client after authentication
        SeIncreaseWorkingSetPrivilege,        // Increase a process working set
        SeIncreaseBasePriorityPrivilege,      // Increase scheduling priority
        SeLoadDriverPrivilege,                // Load and unload device drivers
        SeLockMemoryPrivilege,                // Lock pages in memory
        SeBatchLogonRight,                    // Log on as a batch job
        SeServiceLogonRight,                  // Log on as a service
        SeSecurityPrivilege,                  // Manage auditing and security log
        SeRelabelPrivilege,                   // Modify an object label
        SeSystemEnvironmentPrivilege,         // Modify firmware environment values
        SeManageVolumePrivilege,              // Perform volume maintenance tasks
        SeProfileSingleProcessPrivilege,      // Profile single process
        SeSystemProfilePrivilege,             // Profile system performance
        SeUnsolicitedInputPrivilege,          // "Read unsolicited input from a terminal device"
        SeUndockPrivilege,                    // Remove computer from docking station
        SeAssignPrimaryTokenPrivilege,        // Replace a process level token
        SeRestorePrivilege,                   // Restore files and directories
        SeShutdownPrivilege,                  // Shut down the system
        SeSyncAgentPrivilege,                 // Synchronize directory service data
        SeTakeOwnershipPrivilege              // Take ownership of files or other objects
    }

    [StructLayout(LayoutKind.Sequential)]
    struct LSA_OBJECT_ATTRIBUTES
    {
        internal int Length;
        internal IntPtr RootDirectory;
        internal IntPtr ObjectName;
        internal int Attributes;
        internal IntPtr SecurityDescriptor;
        internal IntPtr SecurityQualityOfService;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct LSA_UNICODE_STRING
    {
        internal ushort Length;
        internal ushort MaximumLength;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string Buffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct LSA_ENUMERATION_INFORMATION
    {
        internal IntPtr PSid;
    }

    internal sealed class Win32Sec
    {
        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        internal static extern uint LsaOpenPolicy(
            LSA_UNICODE_STRING[] SystemName,
            ref LSA_OBJECT_ATTRIBUTES ObjectAttributes,
            int AccessMask,
            out IntPtr PolicyHandle
        );

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        internal static extern uint LsaAddAccountRights(
            LSA_HANDLE PolicyHandle,
            IntPtr pSID,
            LSA_UNICODE_STRING[] UserRights,
            int CountOfRights
        );

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        internal static extern uint LsaRemoveAccountRights(
            LSA_HANDLE PolicyHandle,
            IntPtr pSID,
            bool AllRights,
            LSA_UNICODE_STRING[] UserRights,
            int CountOfRights
        );

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        internal static extern uint LsaEnumerateAccountRights(
            LSA_HANDLE PolicyHandle,
            IntPtr pSID,
            out IntPtr /*LSA_UNICODE_STRING[]*/ UserRights,
            out ulong CountOfRights
        );

        [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        internal static extern uint LsaEnumerateAccountsWithUserRight(
            LSA_HANDLE PolicyHandle,
            LSA_UNICODE_STRING[] UserRights,
            out IntPtr EnumerationBuffer,
            out ulong CountReturned
        );

        [DllImport("advapi32")]
        internal static extern int LsaNtStatusToWinError(int NTSTATUS);

        [DllImport("advapi32")]
        internal static extern int LsaClose(IntPtr PolicyHandle);

        [DllImport("advapi32")]
        internal static extern int LsaFreeMemory(IntPtr Buffer);
    }

    internal sealed class Sid : IDisposable
    {
        public IntPtr pSid = IntPtr.Zero;
        public SecurityIdentifier sid = null;

        public Sid(string account)
        {
            sid = (SecurityIdentifier)(new NTAccount(account)).Translate(typeof(SecurityIdentifier));
            Byte[] buffer = new Byte[sid.BinaryLength];
            sid.GetBinaryForm(buffer, 0);

            pSid = Marshal.AllocHGlobal(sid.BinaryLength);
            Marshal.Copy(buffer, 0, pSid, sid.BinaryLength);
        }

        public void Dispose()
        {
            if (pSid != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pSid);
                pSid = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
        ~Sid() { Dispose(); }
    }

    public sealed class LsaWrapper : IDisposable
    {
        enum Access : int
        {
            POLICY_READ = 0x20006,
            POLICY_ALL_ACCESS = 0x00F0FFF,
            POLICY_EXECUTE = 0X20801,
            POLICY_WRITE = 0X207F8
        }
        const uint STATUS_ACCESS_DENIED = 0xc0000022;
        const uint STATUS_INSUFFICIENT_RESOURCES = 0xc000009a;
        const uint STATUS_NO_MEMORY = 0xc0000017;
        const uint STATUS_OBJECT_NAME_NOT_FOUND = 0xc0000034;
        const uint STATUS_NO_MORE_ENTRIES = 0x8000001a;

        IntPtr lsaHandle;

        public LsaWrapper() : this(null) { } // local system if systemName is null
        public LsaWrapper(string systemName)
        {
            LSA_OBJECT_ATTRIBUTES lsaAttr;
            lsaAttr.RootDirectory = IntPtr.Zero;
            lsaAttr.ObjectName = IntPtr.Zero;
            lsaAttr.Attributes = 0;
            lsaAttr.SecurityDescriptor = IntPtr.Zero;
            lsaAttr.SecurityQualityOfService = IntPtr.Zero;
            lsaAttr.Length = Marshal.SizeOf(typeof(LSA_OBJECT_ATTRIBUTES));
            lsaHandle = IntPtr.Zero;
            LSA_UNICODE_STRING[] system = null;
            if (systemName != null)
            {
                system = new LSA_UNICODE_STRING[1];
                system[0] = InitLsaString(systemName);
            }

            uint ret = Win32Sec.LsaOpenPolicy(system, ref lsaAttr, (int)Access.POLICY_ALL_ACCESS, out lsaHandle);
            if (ret == 0) return;
            if (ret == STATUS_ACCESS_DENIED) throw new UnauthorizedAccessException();
            if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY)) throw new OutOfMemoryException();
            throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
        }

        public void AddPrivilege(string account, Rights privilege)
        {
            uint ret = 0;
            using (Sid sid = new Sid(account))
            {
                LSA_UNICODE_STRING[] privileges = new LSA_UNICODE_STRING[1];
                privileges[0] = InitLsaString(privilege.ToString());
                ret = Win32Sec.LsaAddAccountRights(lsaHandle, sid.pSid, privileges, 1);
            }
            if (ret == 0) return;
            if (ret == STATUS_ACCESS_DENIED) throw new UnauthorizedAccessException();
            if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY)) throw new OutOfMemoryException();
            throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
        }

        public void RemovePrivilege(string account, Rights privilege)
        {
            uint ret = 0;
            using (Sid sid = new Sid(account))
            {
                LSA_UNICODE_STRING[] privileges = new LSA_UNICODE_STRING[1];
                privileges[0] = InitLsaString(privilege.ToString());
                ret = Win32Sec.LsaRemoveAccountRights(lsaHandle, sid.pSid, false, privileges, 1);
            }
            if (ret == 0) return;
            if (ret == STATUS_ACCESS_DENIED) throw new UnauthorizedAccessException();
            if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY)) throw new OutOfMemoryException();
            throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
        }

        public Rights[] EnumerateAccountPrivileges(string account)
        {
            uint ret = 0;
            ulong count = 0;
            IntPtr privileges = IntPtr.Zero;
            Rights[] rights = null;

            using (Sid sid = new Sid(account))
            {
                ret = Win32Sec.LsaEnumerateAccountRights(lsaHandle, sid.pSid, out privileges, out count);
            }
            if (ret == 0)
            {
                rights = new Rights[count];
                for (int i = 0; i < (int)count; i++)
                {
                    LSA_UNICODE_STRING str = (LSA_UNICODE_STRING)Marshal.PtrToStructure(
                        IntPtr.Add(privileges, i * Marshal.SizeOf(typeof(LSA_UNICODE_STRING))),
                        typeof(LSA_UNICODE_STRING));
                    rights[i] = (Rights)Enum.Parse(typeof(Rights), str.Buffer);
                }
                Win32Sec.LsaFreeMemory(privileges);
                return rights;
            }
            if (ret == STATUS_OBJECT_NAME_NOT_FOUND) return null;  // No privileges assigned
            if (ret == STATUS_ACCESS_DENIED) throw new UnauthorizedAccessException();
            if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY)) throw new OutOfMemoryException();
            throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
        }

        public string[] EnumerateAccountsWithUserRight(Rights privilege)
        {
            uint ret = 0;
            ulong count = 0;
            LSA_UNICODE_STRING[] rights = new LSA_UNICODE_STRING[1];
            rights[0] = InitLsaString(privilege.ToString());
            IntPtr buffer = IntPtr.Zero;
            string[] accounts = null;

            ret = Win32Sec.LsaEnumerateAccountsWithUserRight(lsaHandle, rights, out buffer, out count);
            if (ret == 0)
            {
                accounts = new string[count];
                for (int i = 0; i < (int)count; i++)
                {
                    LSA_ENUMERATION_INFORMATION LsaInfo = (LSA_ENUMERATION_INFORMATION)Marshal.PtrToStructure(
                        IntPtr.Add(buffer, i * Marshal.SizeOf(typeof(LSA_ENUMERATION_INFORMATION))),
                        typeof(LSA_ENUMERATION_INFORMATION));
                    accounts[i] = (new SecurityIdentifier(LsaInfo.PSid)).Translate(typeof(NTAccount)).ToString();
                }
                Win32Sec.LsaFreeMemory(buffer);
                return accounts;
            }
            if (ret == STATUS_NO_MORE_ENTRIES) return null;  // No accounts assigned
            if (ret == STATUS_ACCESS_DENIED) throw new UnauthorizedAccessException();
            if ((ret == STATUS_INSUFFICIENT_RESOURCES) || (ret == STATUS_NO_MEMORY)) throw new OutOfMemoryException();
            throw new Win32Exception(Win32Sec.LsaNtStatusToWinError((int)ret));
        }

        public void Dispose()
        {
            if (lsaHandle != IntPtr.Zero)
            {
                Win32Sec.LsaClose(lsaHandle);
                lsaHandle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }
        ~LsaWrapper() { Dispose(); }

        // helper functions:
        static LSA_UNICODE_STRING InitLsaString(string s)
        {
            // Unicode strings max. 32KB
            if (s.Length > 0x7ffe) throw new ArgumentException("String too long");
            LSA_UNICODE_STRING lus = new LSA_UNICODE_STRING();
            lus.Buffer = s;
            lus.Length = (ushort)(s.Length * sizeof(char));
            lus.MaximumLength = (ushort)(lus.Length + sizeof(char));
            return lus;
        }
    }
}
'@ # This type (PS_LSA) is used by Grant-UserRight, Revoke-UserRight, Get-UserRightsGrantedToAccount, Get-AccountsWithUserRight

function Grant-UserRight {
  <#
  .SYNOPSIS
  Assigns user rights to accounts
  .DESCRIPTION
  Assigns one or more user rights (privileges) to one or more accounts. If you specify privileges already granted to the account, they are ignored.
  .EXAMPLE
  Grant-UserRight "bilbo.baggins" SeServiceLogonRight

  Grants bilbo.baggins the "Logon as a service" right on the local computer.
  .EXAMPLE
  Grant-UserRight -Account "Edward","Karen" -Right SeServiceLogonRight,SeCreateTokenPrivilege -Computer TESTPC

  Grants both Edward and Karen, "Logon as a service" and "Create a token object" rights on the TESTPC system.
  .PARAMETER Account
  Logon name of the account. More than one account can be listed. If the account is not found on the computer, the default domain is searched. To specify a domain, you may use either "DOMAIN\username" or "username@domain.dns" formats.
  .PARAMETER Right
  Name of the right to grant. More than one right may be listed.

  Possible values: 
    SeTrustedCredManAccessPrivilege      Access Credential Manager as a trusted caller
    SeNetworkLogonRight                  Access this computer from the network
    SeTcbPrivilege                       Act as part of the operating system
    SeMachineAccountPrivilege            Add workstations to domain
    SeIncreaseQuotaPrivilege             Adjust memory quotas for a process
    SeInteractiveLogonRight              Allow log on locally
    SeRemoteInteractiveLogonRight        Allow log on through Remote Desktop Services
    SeBackupPrivilege                    Back up files and directories
    SeChangeNotifyPrivilege              Bypass traverse checking
    SeSystemtimePrivilege                Change the system time
    SeTimeZonePrivilege                  Change the time zone
    SeCreatePagefilePrivilege            Create a pagefile
    SeCreateTokenPrivilege               Create a token object
    SeCreateGlobalPrivilege              Create global objects
    SeCreatePermanentPrivilege           Create permanent shared objects
    SeCreateSymbolicLinkPrivilege        Create symbolic links
    SeDebugPrivilege                     Debug programs
    SeDenyNetworkLogonRight              Deny access this computer from the network
    SeDenyBatchLogonRight                Deny log on as a batch job
    SeDenyServiceLogonRight              Deny log on as a service
    SeDenyInteractiveLogonRight          Deny log on locally
    SeDenyRemoteInteractiveLogonRight    Deny log on through Remote Desktop Services
    SeEnableDelegationPrivilege          Enable computer and user accounts to be trusted for delegation
    SeRemoteShutdownPrivilege            Force shutdown from a remote system
    SeAuditPrivilege                     Generate security audits
    SeImpersonatePrivilege               Impersonate a client after authentication
    SeIncreaseWorkingSetPrivilege        Increase a process working set
    SeIncreaseBasePriorityPrivilege      Increase scheduling priority
    SeLoadDriverPrivilege                Load and unload device drivers
    SeLockMemoryPrivilege                Lock pages in memory
    SeBatchLogonRight                    Log on as a batch job
    SeServiceLogonRight                  Log on as a service
    SeSecurityPrivilege                  Manage auditing and security log
    SeRelabelPrivilege                   Modify an object label
    SeSystemEnvironmentPrivilege         Modify firmware environment values
    SeManageVolumePrivilege              Perform volume maintenance tasks
    SeProfileSingleProcessPrivilege      Profile single process
    SeSystemProfilePrivilege             Profile system performance
    SeUnsolicitedInputPrivilege          "Read unsolicited input from a terminal device"
    SeUndockPrivilege                    Remove computer from docking station
    SeAssignPrimaryTokenPrivilege        Replace a process level token
    SeRestorePrivilege                   Restore files and directories
    SeShutdownPrivilege                  Shut down the system
    SeSyncAgentPrivilege                 Synchronize directory service data
    SeTakeOwnershipPrivilege             Take ownership of files or other objects
  .PARAMETER Computer
  Specifies the name of the computer on which to run this cmdlet. If the input for this parameter is omitted, then the cmdlet runs on the local computer.
  .INPUTS
  String Account
  PS_LSA.Rights Right
  String Computer
  .OUTPUTS
  .LINK
  http://msdn.microsoft.com/en-us/library/ms721786.aspx
  http://msdn.microsoft.com/en-us/library/bb530716.aspx
  #>
    [CmdletBinding()]
    param (
        [Parameter(Position=0, Mandatory=$true, ValueFromPipelineByPropertyName=$true, ValueFromPipeline=$true)]
        [Alias('User','Username')][String[]] $Account,
        [Parameter(Position=1, Mandatory=$true, ValueFromPipelineByPropertyName=$true)]
        [Alias('Privilege')] [PS_LSA.Rights[]] $Right,
        [Parameter(ValueFromPipelineByPropertyName=$true, HelpMessage="Computer name")]
        [Alias('System','ComputerName','Host')][String] $Computer
    )
    process {
        $lsa = New-Object PS_LSA.LsaWrapper($Computer)
        foreach ($Acct in $Account) {
            foreach ($Priv in $Right) {
                $lsa.AddPrivilege($Acct,$Priv)
            }
        }
    }
} # Assigns user rights to accounts



