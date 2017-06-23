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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilentPackagesBuilder.Views
{
    public partial class PackageSearchControlDemo : Form
    {
        public PackageSearchControlDemo()
        {
            InitializeComponent();


            this.packageSearchcontrol1.DataGridView.SelectionChanged += DataGridView_SelectionChanged;

        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var packageInfo = (PackageFileInfo) this.packageSearchcontrol1.DataGridView.SelectedRows[0].DataBoundItem;
            this.textBox1.Text = packageInfo.FileName;
        }
    }
}
