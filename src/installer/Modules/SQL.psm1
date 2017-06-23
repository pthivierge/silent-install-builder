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

#
# Test if the connection to SQL can be made
# 
#
function Test-SqlConnection()
{
	param(
		$server,
		$user='',
		$password=''
	)

	$connectionString=''

	if($user -ne '')
	{
		#standard
		#Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;Connection Timeout=3;
		 $connectionString='Server={0};Database=master;User Id={1};Password={2};Connection Timeout=3;' -f $server, $user, $password
	}
	else
	{
		#trusted
		#Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;Connection Timeout=3;
		$connectionString='Server={0};Database=master;Trusted_Connection=True;Connection Timeout=3;' -f $server, $user, $password
	}
	   
    $sqlConn=New-Object("Data.SqlClient.SqlConnection") $connectionString

    try{
        $sqlConn.Open()
    }
    catch {
        throw "Unable to open SQL connection.  Please make sure the SQL instance exists and the running user has access to the specified SQL Server Instance"
		return $false
    }

    write-host "Confirmed that SQL Server ($SQLInstanceName) exists ..."
	return $true
}
