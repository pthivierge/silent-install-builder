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
using System.IO;
using System.Linq;
using System.Text;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Core.Models;

namespace SilentPackagesBuilder.Views
{
    public class PackageEditorViewModel : BaseViewModel
    {
        private IInstallationStep _model = null;

        public PackageEditorViewModel() { }

        public PackageEditorViewModel(IInstallationStep model)
        {
            _model = model;
        }

        public string DisplayName
        {
            get { return _model.DisplayName; }

            set
            {
                _model.DisplayName = value;
                this.OnPropertyChanged(nameof(DisplayName));
            }
        }

        public PackageType PackageType
        {
            get { return _model.Type; }

        }


        public string PackagePath
        {
            get
            {
                if (_model is IHasInstallFile)
                {
                    var m = ((IHasInstallFile) _model);
                    if(m.FileInfo!=null)
                    {
                        return m.FileInfo.FilePath;
                    }
                    else
                    {
                        return "";
                    }
                   
                }
                else
                {
                    return "Not Applicable";
                }

                

            }

        }






    }
}
