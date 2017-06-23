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
using System.Windows.Forms;
using PropertyChanged;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views
{
    [ImplementPropertyChanged]
    public class PackagesListViewModel : BaseViewModel
    {
        private InstallModel _model;
        

        public PackagesListViewModel(InstallModel model)
        {
            _model = model;
            _model.Items.ListChanged += Items_ListChanged;
            this.Count = _model.Items.Count;
        }

        public InstallModel InstallModel
        {
            get { return _model; }

            set
            {
                _model = value;
                this.OnPropertyChanged(nameof(InstallModel));
               
            }
        }

        private void Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.Count = _model.Items.Count;
            this.OnPropertyChanged("Count");
        }

        public void DeletePackage(IInstallationStep step)
        {
            var message = $"Are you sure you want to delete {step.DisplayName}?";
            if (MessageBox.Show(null, message, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _model.Remove(step);
            }
        }

        public BindingList<IInstallationStep> PackagesList
        {
            get { return _model.Items; }



        }

        public int Count { get; set; }
    }
}
