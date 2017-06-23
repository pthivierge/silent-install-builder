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
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace SilentPackagesBuilder.Core.Models
{
    public static class ModelUtils
    {
        public static InstallModel GetTestModel()
        {
            var model = new InstallModel();
            var package1 = new OSIAutoExtractSetupPackage() {
                  DisplayName = "package 1"
                 ,IniReplacements = new BindingList<IniReplacement>() { new IniReplacement() { DefaultValue = "16 = ...", ReplaceValue = "16 = abc" } }
                , FileInfo = new PackageFileInfo() { FilePath = "c:\\test.exe" }
            };
            var package2 = new PowerShellCodeBlock() { DisplayName = "package 2",   Code = "Restart-Computer", };
            var package3 = new Executable() { DisplayName = "Executable package", Arguments = "/silent"};
            var package4 = new PIDataArchive() { DisplayName = "PI DA package" , IniReplacements = new BindingList<IniReplacement>() {new IniReplacement() {DefaultValue = "16 = ...",ReplaceValue = "16 = abc"} }  };

            model.Add(package1);
            model.Add(package2);
            model.Add(package3);
            model.Add(package4);

            return model;
        }
    }
}
