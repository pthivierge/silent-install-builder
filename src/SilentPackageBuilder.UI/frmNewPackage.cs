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
using SilentPackagesBuilder.Core;
using SilentPackagesBuilderGUI;

namespace SilentPackagesBuilder
{
    public partial class frmNewPackage : Form
    {

        public IInstallationStep CurrentInstallationStep { get; set; }
        PackageFileInfo _currentFile;

        public frmNewPackage()
        {
            InitializeComponent();
        }

 

        private void frmNewPackage_Load(object sender, EventArgs e)
        {

            var values = Enum.GetValues(typeof(PackageType)).Cast<PackageType>()
            .Where(package => package != PackageType.PowershellCodeBlock)
            .ToList();

            this.listBox1.DataSource = values;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
           

            var packageType = (PackageType)listBox1.SelectedItem;
            IInstallationStep step = null;
            switch (packageType)
            {
                case PackageType.Executable:
                    step = new Executable();
                    break;
                case PackageType.PIDataArchiveLegacy:
                    step = new PIDataArchive();
                    break;
                
                case PackageType.OSIAutoExtractSetupPackageLegacy:
                    step = new OSIAutoExtractSetupPackage();
                    break;

            }

            step.DisplayName = "Package " + packageType.ToString();
                    
            this.CurrentInstallationStep = step;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            btnOk_Click(sender, e);
        }
    }
}
