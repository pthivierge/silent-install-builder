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

        public String FileInformation
        {
            get
            {
                if(!(_model is IHasInstallFile))
                { return "N/A"; }

                var _modelwInstallFile = (IHasInstallFile)_model;
                if(_modelwInstallFile.FileInfo!=null)
                {
                    var fi = _modelwInstallFile.FileInfo;
                    String fileInformation = String.Format("File Name: {0}{1}File Version: {2}{3}File Size: {4}", fi.FileName,Environment.NewLine, fi.FileVersion,Environment.NewLine, BytesToString(fi.FileBytes));
                    return fileInformation;
                }

                return "N/A";  // case else... normally should not get there.


            }

        }

        private String BytesToString(long byteCount)
        { // https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
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

            set
            {
                if (_model is IHasInstallFile)
                {
                    ((IHasInstallFile) _model).FileInfo.FilePath = value;
                    this.OnPropertyChanged(nameof(PackagePath));
                }

            }

        }






    }
}
