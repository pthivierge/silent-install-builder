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


namespace SilentPackagesBuilder.Views.PackageEditor.Editors
{
    public partial class ExecutableParametersEditor : UserControl, IModelSettableControl
    {
        private Executable _model;
        private BindingSource _bindingsource = new BindingSource();

        public ExecutableParametersEditor()
        {
            InitializeComponent();

        }

        public void SetModel(object model)
        {
            _model=model as Executable;

            ConfigureDataBindings();
        }

        private void ConfigureDataBindings()
        {
            if (_model != null)
            {
                _bindingsource.DataSource = _model;
                MVVMUtils.AddDataBinding(txtArguments, "Text", _bindingsource, nameof(_model.Arguments));
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
