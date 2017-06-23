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
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views
{
    public partial class PackageSearchControl : UserControl
    {
        PackageSearchViewModel _vm;
        BindingSource _bindingSource;

        public PackageSearchControl()
        {
            InitializeComponent();

            _vm = new PackageSearchViewModel();
            _bindingSource = new BindingSource(_vm, null);

            RestoreSettings();

            InitializeDataBindings();

            AttachEvents();

        }



        private void RestoreSettings()
        {
            // restores parameters from the last time the app was run
            _vm.SearchPath = UserSettings.Default.LastSelectedPackagesDirectory;
        }

        private void AttachEvents()
        {
            this.btnScanPackages.Click += _vm.Search;
            this.btnCancelSearch.Click += _vm.Cancel;
        }

        private void InitializeDataBindings()
        {

            MVVMUtils.AddDataBinding(txtPackagesDir, "Text", _bindingSource, nameof(_vm.SearchPath));
            MVVMUtils.AddDataBinding(txtfileNameFilter, "Text", _bindingSource, nameof(_vm.SearchFilter));
            MVVMUtils.AddDataBinding(statusFilesCount, "Text", _bindingSource, nameof(_vm.FilesCount));
            MVVMUtils.AddDataBinding(statusSearchInProgress, "Visible", _bindingSource, nameof(_vm.IsSearching));
            MVVMUtils.AddDataBinding(gridPackagesSearch, "DataSource", _bindingSource, nameof(_vm.Files));

            ConfigureGrid();
        }



        private void ConfigureGrid()
        {

            gridPackagesSearch.AutoGenerateColumns = false;

            //create the column programatically
            DataGridViewCell cell = new DataGridViewTextBoxCell();
            DataGridViewTextBoxColumn colFileName = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName" // Tell the column which property of FileName it should use
            };

            DataGridViewTextBoxColumn colVersion = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Version",
                HeaderText = "Version",
                DataPropertyName = "FileVersion" // Tell the column which property of FileName it should use
            };

            DataGridViewTextBoxColumn colPath = new DataGridViewTextBoxColumn()
            {
                CellTemplate = cell,
                Name = "Path",
                HeaderText = "Path to the file",
                DataPropertyName = "FilePath" // Tell the column which property of FileName it should use
            };

            gridPackagesSearch.Columns.Add(colFileName);
            gridPackagesSearch.Columns.Add(colVersion);
            gridPackagesSearch.Columns.Add(colPath);


        }

        private void btnOpenSearchDirDialog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPackagesDir.Text) && Directory.Exists(txtPackagesDir.Text))
            {
                folderBrowserDialog1.SelectedPath = txtPackagesDir.Text;

            }

            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                txtPackagesDir.Text = folderBrowserDialog1.SelectedPath;
                UserSettings.Default.LastSelectedPackagesDirectory = txtPackagesDir.Text;
            }
        }

        public DataGridView DataGridView { get { return gridPackagesSearch; } }


    }
}
