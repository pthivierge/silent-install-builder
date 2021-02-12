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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views
{
    public partial class PackagesListControl : UserControl
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PackagesListControl));
        private BindingSource _bindingSource = new BindingSource();
        private PackagesListViewModel _viewModel = null;
        private InstallModel _model;
        private IInstallationStep _selectedItem;

        public DataGridView DataGridView { get { return dataGridView; } }

        public PackagesListControl()
        {
            InitializeComponent();
            _logger.InfoFormat("{0} control initialized", this.GetType().Name);

            ConfigureGrid();

            ConfigureEvents();

        }

        public void SetModel(InstallModel model)
        {
            _model = model;
            _viewModel = new PackagesListViewModel(model);
            InitializeDataBindings();
        }

        private void InitializeDataBindings()
        {
            if (_model == null)
            {
                _logger.InfoFormat("can't initialize data binding the datasource was null");
                return;
            }


            try
            {

                // assign the model to the binding source, binding source is the key component for the databinding to work
                _bindingSource.DataSource = _viewModel;

                // examples to bind the controls to the model
                // MVVMUtils.AddDataBinding(txtPackagesDir, "Text", _bindingSource, nameof(_vm.SearchPath));
                dataGridView.DataBindings.Clear();
                statusFilesCount.DataBindings.Clear();
                MVVMUtils.AddDataBinding(dataGridView, "DataSource", _bindingSource, nameof(_viewModel.PackagesList));
                MVVMUtils.AddDataBinding(statusFilesCount, "Text", _bindingSource, nameof(_viewModel.Count));


                _logger.InfoFormat("databinding initialized");

            }
            catch (Exception e)
            {
               _logger.Error(e);
                throw;
            }




        }

        private void ConfigureGrid()
        {
            dataGridView.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoGenerateColumns = false;

            //create the column programatically
            

            var  cols = new List<DataGridViewColumn>();

            cols.Add(new DataGridViewTextBoxColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                Name = "DisplayName",
                HeaderText = "Name of the package or script",
                DataPropertyName = "DisplayName",
                MinimumWidth = 150
            });

            cols.Add(new DataGridViewTextBoxColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                Name = "Type",
                HeaderText = "Type",
                DataPropertyName = "Type" ,
                FillWeight = 20,
                MinimumWidth = 150,
            });

            cols.Add(new DataGridViewButtonColumn()
            {
                CellTemplate = new DataGridViewButtonCell(),
                Name = "Edit",
                HeaderText = "Edit",
                Text = "Edit",
                FillWeight = 20,
                MinimumWidth = 75,
                UseColumnTextForButtonValue = true
            });

            cols.Add(new DataGridViewButtonColumn()
            {
               CellTemplate = new DataGridViewButtonCell() {Style = new DataGridViewCellStyle() {BackColor = Color.DarkRed} },
               Name = "Delete",
               HeaderText = "Delete",
               Text = "Delete",
               FillWeight=20,
               MinimumWidth = 75,
               UseColumnTextForButtonValue = true
            });





            dataGridView.Columns.AddRange(cols.ToArray());

            // configure button clicks
            dataGridView.CellClick += EditOrDeleteButtonClick;
            dataGridView.CellDoubleClick+=CellDoubleClick;
            dataGridView.KeyDown += DataGridView_KeyDown;


        }

        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                var grid = (DataGridView)sender;
                if(grid.SelectedRows.Count<=0)
                    return;

                IInstallationStep step = (IInstallationStep)grid.SelectedRows[0].DataBoundItem;
                EditInstallStep(step);
            }

 


        }

        private void ConfigureEvents()
        {
            linkLastExport.LinkClicked += (sender, args) =>
            {
                try
                {
                    Process.Start($"{UserSettings.Default.PackGenLastSuccessExportedPackageDir}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }

            };
        }

        private void EditInstallStep(IInstallationStep step)
        {
            var f = new frmPackageEditor(step);
            f.Show();
        }


        private void CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var grid = (DataGridView)sender;
            IInstallationStep step = (IInstallationStep)grid.Rows[e.RowIndex].DataBoundItem;

            EditInstallStep(step);
        }

        private void EditOrDeleteButtonClick(object sender, DataGridViewCellEventArgs e)
        {

            if(e.RowIndex<0)
                return;

            var grid = (DataGridView)sender;
            IInstallationStep step = (IInstallationStep)grid.Rows[e.RowIndex].DataBoundItem;

            if (e.ColumnIndex == dataGridView.Columns["Delete"].Index)
            {
                _viewModel.DeletePackage(step);
            }

            if (e.ColumnIndex == dataGridView.Columns["Edit"].Index)
            {
                EditInstallStep(step);
            }
        }

        private void btnGenerateSilentInstall_Click(object sender, EventArgs e)
        {

            if (_model.Items.Count == 0)
            {
                MessageBox.Show("There is nothing to generate, you must add packages first.");
                return;
            }
            
            folderBrowserDialog1.SelectedPath = UserSettings.Default.LastSelectedPackGenExportDirectory;

            var res = folderBrowserDialog1.ShowDialog();

            if (res == DialogResult.OK)
            {
                statusPackGenInProgress.Visible = true;
                toolStripProgressBar1.Visible = true;
                var targetDir = folderBrowserDialog1.SelectedPath;
                UserSettings.Default.LastSelectedPackGenExportDirectory = targetDir;
                SilentPackageGenerator.Create(_model, targetDir, chkConfigOnly.Checked);

                UserSettings.Default.PackGenLastSuccessExportedPackageDir = targetDir;
                statusPackGenInProgress.Visible = false;
                toolStripProgressBar1.Visible = false;
                toolStripProgressBar1.Value = 0;
            }




        }

        private void UpdatePackageGenerationProgress(int progressPercent)
        {
            toolStripProgressBar1.Value = progressPercent;
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                dataGridView.CurrentCell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // Can leave these here - doesn't hurt
                dataGridView.Rows[e.RowIndex].Selected = true;
                dataGridView.Focus();

                _selectedItem = (IInstallationStep)dataGridView.Rows[e.RowIndex].DataBoundItem;

                
                }
            catch (Exception)
            {

            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if(_selectedItem!=null)
            {
                _model.MoveUp(_selectedItem);
                dataGridView.Refresh();
            }
                

        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedItem != null)
            {
                _model.MoveDown(_selectedItem);
                dataGridView.Refresh();
            }
        }
    }


}

