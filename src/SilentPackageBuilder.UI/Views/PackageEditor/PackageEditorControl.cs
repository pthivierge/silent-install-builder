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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Core.Models;
using SilentPackagesBuilder.Views.PackageEditor;
using SilentPackagesBuilder.Views.PackageEditor.Editors;

namespace SilentPackagesBuilder.Views
{
    public partial class PackageEditorControl : UserControl
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PackageEditorControl));
        private BindingSource _bindingSource = new BindingSource();
        private PackageEditorViewModel _viewModel = null;
        private IInstallationStep _model;


        public PackageEditorControl()
        {
            InitializeComponent();
            _logger.InfoFormat("{0} control initialized", this.GetType().Name);
            btnSelectPackage.Enabled = true;
        }


        public void SetModel(IInstallationStep installationStep)
        {
            _model = installationStep;
            _viewModel = new PackageEditorViewModel(installationStep);

            InitializeDataBindings();
            SetEditorType(installationStep);
        }

        private void InitializeDataBindings()
        {
            if (_viewModel == null)
            {
                _logger.InfoFormat("can't initialize data binding the view model was null");
                return;
            }


            try
            {
                _bindingSource=new BindingSource();
                // assign the model to the binding source, binding source is the key component for the databinding to work
                _bindingSource.DataSource = _viewModel;

                txtDisplayName.DataBindings.Clear();
                txtPackageType.DataBindings.Clear();
                txtPathtoFile.DataBindings.Clear();
                txtFileInfo.DataBindings.Clear();


                MVVMUtils.AddDataBinding(txtDisplayName, "Text", _bindingSource, nameof(_viewModel.DisplayName));
                MVVMUtils.AddDataBinding(txtPackageType, "Text", _bindingSource, nameof(_viewModel.PackageType));
                MVVMUtils.AddDataBinding(txtPathtoFile, "Text", _bindingSource, nameof(_viewModel.PackagePath));
                MVVMUtils.AddDataBinding(txtFileInfo, "Text", _bindingSource, nameof(_viewModel.FileInformation));


                _logger.InfoFormat("databinding initialized");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




        }

        private void SetEditorType(IInstallationStep model)
        {
            var type = model.Type;

            switch (type)
            {
                case PackageType.PowershellCodeBlock:
                    //AddControl(new IniReplacementsEditor());
                    AddControl(new ScriptEditor());
                    btnSelectPackage.Enabled = false;
                    break;
                case PackageType.Executable:
                    AddControl(new ExecutableParametersEditor());
                    break;
                case PackageType.PIDataArchiveLegacy:
                    AddControl(new IniReplacementsEditor());
                    break;
                case PackageType.OSIAutoExtractSetupPackageLegacy:
                    AddControl(new IniReplacementsEditor());
                    break;
            }
        }

        private void AddControl(IModelSettableControl ctrl)
        {
            var c = ctrl as UserControl;
            this.configurationContainer.Controls.Clear();
            this.configurationContainer.Controls.Add(c);
            c.Dock = DockStyle.Fill;
            ctrl.SetModel(_model);
        }

        private void PackageEditorControl_Load(object sender, EventArgs e)
        {

        }

        private void btnSelectPackage_Click(object sender, EventArgs e)
        {
            using (var f = new frmSelectPackage((IHasInstallFile)_model))
            {
                var res=f.ShowDialog();
                
                if (res == DialogResult.OK)
                {
                    _viewModel.PackagePath = ((IHasInstallFile)f.CurrentInstallationStep).FileInfo.FilePath;
                    
                }

               

            };
        }
    }
}

