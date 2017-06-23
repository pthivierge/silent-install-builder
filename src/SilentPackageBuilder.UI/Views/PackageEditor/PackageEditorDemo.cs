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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilentPackagesBuilder.Core.Models;

namespace SilentPackagesBuilder.Views.PackageEditor.Editors
{
    public partial class PackageEditorDemo : Form
    {
        private InstallModel _model;

        public PackageEditorDemo()
        {
            InitializeComponent();

            _model = ModelUtils.GetTestModel();

            
            this.packagesListControl1.SetModel(_model);

            this.packagesListControl1.DataGridView.CellDoubleClick += DataGridView_CellDoubleClick;

        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var model = this.packagesListControl1.DataGridView.Rows[e.RowIndex].DataBoundItem as IInstallationStep;
            if(model!=null)
                this.packageEditorControl1.SetModel(model);
        }

        private void PackageEditorDemo_Load(object sender, EventArgs e)
        {

        }
    }
}
