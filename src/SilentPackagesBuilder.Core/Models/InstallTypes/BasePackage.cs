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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;
using PropertyChanged;
using SilentPackagesBuilder.Core.Models;

namespace SilentPackagesBuilder.Core
{
    /// <summary>
    /// This base class gives basic properties to any of the different Packages types
    /// </summary>
    [Serializable]
    public class BasePackage : IInstallationStep
    {

        /// <summary>
        /// Unique Id of the package
        /// </summary>
        public Guid Id { get; }=new Guid();

        /// <summary>
        /// Name that is displayed during the installation process in logs 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// It set to 1, installation step is run, if set to 0 it is not executed
        /// </summary>
        public int Install { get; set; } = 1;

        /// <summary>
        /// Type of the installation package
        /// </summary>
        public PackageType Type { get; set; }

        

        /// <summary>
        /// Time at which the information of the package was update in the application
        /// </summary>


        #region workaround

        // this class makes use of fody propertychanged assembly,  the assembly injects the code to make properties notifications at compile time.
        // but, because it was needed to have the INotifypropertyChange Interface on the IInstallationStep interface ( we are passing list if IInstallation step to a bindablecollection) then to compil we need to have 
        // this code below.  
        // see https://github.com/Fody/PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }

        #endregion workaround

    }



}
