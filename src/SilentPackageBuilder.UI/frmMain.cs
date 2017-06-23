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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Core.Models;
using SilentPackagesBuilderGUI;

namespace SilentPackagesBuilder
{
    public partial class frmMain : Form
    {
        private InstallModel _model=null;
        private string _currentOpenedFilePath;

        public frmMain()
        {
            InitializeComponent();

            saveFileDialog1.FileOk += SaveFileDialog1_FileOk;
            openSibFileDialog.FileOk += OpenSibFileOK;
            
            NewProject_click(this,null);

            ConfigureEvents();





        }


        public void ConfigureEvents()
        {
            linkDefineVariables.LinkClicked += (sender, args) =>
            {
                try
                {
                    var form = new frmDefineVariables();
                    form.SetModel(_model.UserVariablesModel);
                    form.Show();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }

            };
        }

#region OpenSaveNewProject

        private void SaveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        
            UserSettings.Default.LastSavedProjectFilePath = saveFileDialog1.FileName;
            _currentOpenedFilePath = saveFileDialog1.FileName;
            _model.Save(saveFileDialog1.FileName);
            updateSaveTime();
        
    }




        /// <summary>
        /// event when File New is accessed
        /// </summary>
        private void NewProject_click(object sender, EventArgs e)
        {
            openSibFileDialog.FileName = UserSettings.Default.LastSavedProjectFilePath;

            var message = "Changes that you have not saved will be lost if you create another project file, do you want to continue?";
            var caption = "Create a new project?";

            if (_model!=null && _model.Items.Count>0)

            {
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _model=new InstallModel();
                }
            }
            else
            {
                _model=new InstallModel();
            }

            this.packagesListControl1.SetModel(_model);
        }



        private void SaveProjectAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();

        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentOpenedFilePath))
            {
                SaveProjectAs_Click(sender, e);
                return;
            }

            _model.Save(_currentOpenedFilePath);
            updateSaveTime();
        }

        private void updateSaveTime()
        {
            var time = DateTime.Now.ToString("hh:mm:ss");
            statusSavedAt.Text = $"Saved:{time}";
            statusSavedAt.Visible = true;
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {

            openSibFileDialog.FileName = UserSettings.Default.LastSavedProjectFilePath;

            var message =
                "Changes that you have not saved will be lost if you open another project file, do you want to continue?";
            var caption = "Open a new project?";
            if ( _model==null || _model.Items.Count == 0)
            {
                openSibFileDialog.ShowDialog();
            }
            else
            {
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    openSibFileDialog.ShowDialog();
            }



        }

        private void OpenSibFileOK(object sender, CancelEventArgs e)
        {



            try
            {
                _model = null;
                _model = InstallModel.Deserialize(openSibFileDialog.FileName);
                _currentOpenedFilePath = openSibFileDialog.FileName;
                this.packagesListControl1.SetModel(_model);

            }
            catch (Exception ex)
            {

                MessageBox.Show("There was an error when restoring the configuration.  " + ex);
            }

        }

#endregion


        private void btnNewPowershellScript_Click(object sender, EventArgs e)
        {
            var step = new PowerShellCodeBlock();

            _model.Add(step);

           Program.ShowPackageEditor(step);
        }

       

        private void btnNewPackage_Click(object sender, EventArgs e)
        {
            // safety check - should not happen
            if (_model != null)
            {
                var f=new frmNewPackage(_model);
                f.ShowDialog();
            }
        }



        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm=new frmOptions();
            frm.ShowDialog();
        }
    }
}
