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
using System.ComponentModel;
using System.IO;
using SilentPackagesBuilder.Core;


namespace SilentPackagesBuilder
{
    public interface IHasIniReplacements
    {
        BindingList<IniReplacement> IniReplacements { get; set; }
    }

    public interface IHasInstallFile
    {
        PackageFileInfo FileInfo { get; set; }
    }

    public interface IInstallationStep : INotifyPropertyChanged
    {
        /// <summary>
        /// Unique Id of the package
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Name that is displayed during the installation process in logs 
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// It set to 1, installation step is run, if set to 0 it is not executed
        /// </summary>
        int Install { get; set; }

        /// <summary>
        /// Type of the installation package
        /// </summary>
        PackageType Type { get; }


    }
}