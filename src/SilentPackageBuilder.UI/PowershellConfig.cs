#region License
// Copyright 2017 OSIsoft, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// <http://www.apache.org/licenses/LICENSE-2.0>
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder
{
    public static class Indent
    {
        public static string Space(int level)
        {
            return new String(' ', level * 4);
        }
    }

    public class PowershellConfig
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PowershellConfig));

        public static string GetCurrentExeVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return version;
        }

        /// <summary>
        /// Creates the variable config file.
        /// </summary>
        public static void CreateVariablesConfigurationFile(IList<UserVariable> userVariables, string outputFolder)
        {

            if (userVariables==null || !userVariables.Any() )
            {
                _logger.Info("No user variable was found in the UsersVariable list.  No variables data file will be created.");
                return;
            }

            var version = GetCurrentExeVersion();
            // Generate headers
            var config = new List<string>();
            config.Add($"# {UserSettings.Default.PackGenUserVariablesFileName}");
            config.Add($"# Generated on {DateTime.Now} from the silent package builder v {version}");
            config.Add("# ");
            config.Add("# ");
            config.Add("# ");

            var rootObject = PsObject.GetPSObject();

            foreach (var userVariable in userVariables)
            {
                rootObject.AppendProperty(NewProp(userVariable.Name, userVariable.Value));
            }
            
            config.AddRange(rootObject.ToList(0));
            File.WriteAllLines(Path.Combine(outputFolder, UserSettings.Default.PackGenUserVariablesFileName), config);
            
        }

        public static void CreateInstallConfigFromObjects(IEnumerable<IInstallationStep> packages, string outputFolder)
        {


            var version = GetCurrentExeVersion();

            // Generate headers
            var config = new List<string>();
            config.Add($"# {UserSettings.Default.PackGenConfigFileName}");
            config.Add($"# Generated on {DateTime.Now} from the silent package builder v {version}");
            config.Add("# ");
            config.Add("# ");
            config.Add("# ");

            var rootObject = PsObject.GetPSObject();
            rootObject.AppendProperty(NewProp("sourcePackagesDir","SetupPackages"));
            rootObject.AppendProperty(NewProp("extractedPackagesDir", "ExtractedPackages"));
            var installPackagesArray = PsArray.GetArrayProperty("installPackages");
            // packages
            foreach (var package in packages)
            {

                if (package is PowerShellCodeBlock)
                {
                    installPackagesArray.AppendObject(CreateCodeBlock(0, (PowerShellCodeBlock)package));
                }
                

                if (package is PIDataArchive)
                {
                    installPackagesArray.AppendObject(CreateInstallObject(0, (PIDataArchive)package));
                }

                if (package is OSIAutoExtractSetupPackage)
                {
                    installPackagesArray.AppendObject(CreateInstallObject(0, (OSIAutoExtractSetupPackage)package));
                }

                if (package is Executable)
                {
                    installPackagesArray.AppendObject(CreateInstallObject(0, (Executable)package));
                }

            }

           rootObject.AppendItem(installPackagesArray.ToList(0)); 
           config.AddRange(rootObject.ToList(0));
           
            File.WriteAllLines(Path.Combine(outputFolder,UserSettings.Default.PackGenConfigFileName),config);

            // check space for packages in the target dir


        }

        private static List<string> CreateInstallObject(int indent, Executable package)
        {
            var obj = PsObject.GetPSObject();

            obj.AppendProperty(NewProp("install", package.Install.ToString()));
            obj.AppendProperty(NewProp("type", package.Type.ToString()));
            obj.AppendProperty(NewProp("displayName", package.DisplayName));
            obj.AppendProperty(NewProp("package", package.FileInfo.FileName));

            //arguments
            if (!string.IsNullOrEmpty(package.Arguments.Trim()))
            {
                var arguments = PsArray.GetArrayProperty("arguments");
                // goes over each line in the arguments configured
                using (StringReader sr = new StringReader(package.Arguments))
                {
                    string arg;
                    int argsCount = 0;
                    while ((arg = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(arg))
                        {
                            argsCount++;
                            arguments.AppendProperty($"'{arg}'");
                        }
                    }
                }

                obj.AppendItem(arguments.ToList(indent));
            }





            return obj.ToList(indent);
        }

        private static List<string> CreateInstallObject(int indent, OSIAutoExtractSetupPackage package)
        {
            var obj = PsObject.GetPSObject();

            obj.AppendProperty(NewProp("install", package.Install.ToString()));
            obj.AppendProperty(NewProp("type", package.Type.ToString()));
            obj.AppendProperty(NewProp("displayName", package.DisplayName));
            obj.AppendProperty(NewProp("package", package.FileInfo.FileName));

            // INI replacements
            if (package.IniReplacements.Count > 0)
            {
                var iniFileReplaces = PsArray.GetArrayProperty("IniFileReplaces");
                foreach (var iniReplacement in package.IniReplacements)
                {
                    var replaceObj = PsObject.GetPSObject();
                    replaceObj.AppendProperty(NewProp("default", iniReplacement.DefaultValue));
                    replaceObj.AppendProperty(NewProp("newSetting", iniReplacement.ReplaceValue));

                    iniFileReplaces.AppendObject(replaceObj.ToList(indent));
                }

                obj.AppendItem(iniFileReplaces.ToList(indent));
            }
            
            return obj.ToList(indent);
        }

        private static List<string> CreateInstallObject(int indent, PIDataArchive package)
        {
            var obj = PsObject.GetPSObject();

            obj.AppendProperty(NewProp("install", package.Install.ToString()));
            obj.AppendProperty(NewProp("type", package.Type.ToString()));
            obj.AppendProperty(NewProp("displayName", package.DisplayName));
            obj.AppendProperty(NewProp("package", package.FileInfo.FileName));

            // INI replacements
            if (package.IniReplacements.Count > 0)
            {
                var iniFileReplaces = PsArray.GetArrayProperty("IniFileReplaces");
                foreach (var iniReplacement in package.IniReplacements)
                {
                    var replaceObj = PsObject.GetPSObject();
                    replaceObj.AppendProperty(NewProp("default", iniReplacement.DefaultValue));
                    replaceObj.AppendProperty(NewProp("newSetting", iniReplacement.ReplaceValue));

                    iniFileReplaces.AppendObject(replaceObj.ToList(indent));
                }

                obj.AppendItem(iniFileReplaces.ToList(indent));
            }

            return obj.ToList(indent);
        }

        private static List<string> CreateCodeBlock(int indent, PowerShellCodeBlock package)
        {
            var obj = PsObject.GetPSObject();
            obj.AppendProperty(NewProp("install", package.Install.ToString()));
            obj.AppendProperty(NewProp("type", package.Type.ToString()));
            obj.AppendProperty(NewProp("displayName", package.DisplayName));

            if (!string.IsNullOrEmpty(package.Code.Trim()))
            {
                var arguments = PsArray.GetArrayProperty("PSCodeBlock");
                // goes over each line in the arguments configured
                using (StringReader sr = new StringReader(package.Code))
                {
                    string codeLine;
                    int lines = 0;
                    while ((codeLine = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(codeLine))
                        {
                            lines++;
                            arguments.AppendProperty($"'{codeLine}'");
                        }
                    }
                }

                obj.AppendItem(arguments.ToList(indent));
            }

            return obj.ToList(indent);

        }


   
        private static string NewProp(string name, string value)
        {
            return $"{name}='{value}'";
        }



    }

    public class PsArray
    {
        private string _objStart;
        private string _objEnd;
        private int count = 0;
        private readonly List<string> lines=new List<string>();
        private int _indent;
        private string _propertyName;
        

        public static PsArray GetArrayProperty(string propertyName)
        {
            return new PsArray(propertyName, "@(",")");
        }


        private PsArray(string property,string objStartString, string objEndString)
        {
            _propertyName = !string.IsNullOrEmpty(property) ? $"{property}=" : "";
            _objStart = objStartString;
            _objEnd = objEndString;
            
        }

        public void AppendObject(List<string> item)
        {
            if (lines.Count>0)
                lines[lines.Count - 1] += ",";

            foreach (var line in item)
            {
                lines.Add(line);
            }

        }
        public void AppendProperty(string property)
        {
            if (lines.Count > 0)
                lines[lines.Count - 1] += ",";
            lines.Add(property);
            
        }

        public List<string> ToList(int indent)
        {
            var array=new List<string>();
            array.Add(Indent.Space(indent) + _propertyName);
            array.Add(Indent.Space(indent)+_objStart);
            lines.ForEach(l=>array.Add(Indent.Space(indent+1)+l));
            array.Add(Indent.Space(indent) + _objEnd);

            return array;
        }

    }

    public class PsObject
    {
        private string _objStart;
        private string _objEnd;
        private int count = 0;
        private readonly List<string> lines = new List<string>();
        private int _indent;


        public static PsObject GetPSObject()
        {
            return new PsObject("@{", "}");
        }

        private PsObject(string objStartString, string objEndString)
        {
            _objStart = objStartString;
            _objEnd = objEndString;

        }

        public void AppendProperty(string property)
        {
            lines.Add(property);
        }

        public void AppendItem(List<string> item)
        {

            foreach (var line in item)
            {
                lines.Add(line);
            }

        }

        public List<string> ToList(int indent)
        {
            var array = new List<string>();
            array.Add(Indent.Space(indent) + _objStart);
            lines.ForEach(l => array.Add(Indent.Space(indent+1) + l));
            array.Add(Indent.Space(indent)+_objEnd);

            return array;
        }


    }
}
