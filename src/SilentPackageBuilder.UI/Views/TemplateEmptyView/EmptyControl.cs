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

namespace SilentPackagesBuilder.Views.TemplateEmptyView
{
    public partial class EmptyControl : UserControl
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PackagesListControl));
        private BindingSource _bindingSource=new BindingSource();
        private object _dataSource;

        public Object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value; 
                InitializeDataBindings();
            }
        }

        public EmptyControl()
        {
            InitializeComponent();
            _logger.InfoFormat("{0} control initialized",this.GetType().Name);
            
            InitializeDataBindings();
        }
        
        private void InitializeDataBindings()
        {
            if (_dataSource == null)
            {
                _logger.InfoFormat("can't initialize data binding the datasource was null");
                return;
            }
                

            try
            {

                // assign the model to the binding source, binding source is the key component for the databinding to work
                _bindingSource.DataSource = _dataSource;

                // examples to bind the controls to the model
                // MVVMUtils.AddDataBinding(txtPackagesDir, "Text", _bindingSource, nameof(_vm.SearchPath));
                // MVVMUtils.AddDataBinding(gridPackagesSearch, "DataSource", _bindingSource, nameof(_vm.Files));


                _logger.InfoFormat("databinding initialized");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            
            
        }

       
    }
}
