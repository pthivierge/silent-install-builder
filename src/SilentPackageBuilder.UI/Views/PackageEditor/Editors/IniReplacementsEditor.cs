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


namespace SilentPackagesBuilder.Views.PackageEditor
{
    public partial class IniReplacementsEditor : UserControl, IModelSettableControl
    {
        private IHasIniReplacements _model;
        private BindingSource _bindingSource=new BindingSource();

        public IniReplacementsEditor()
        {
            InitializeComponent();

            ConfigureGrid();
        }

        public void SetModel(object model)
        {
            _model=model as IHasIniReplacements;
            
            ConfigureDataBindings();
        }


        private void ConfigureDataBindings()
        {
            if (_model != null)
            {
                _bindingSource.DataSource = _model;
                dataGridView.CellClick += DataGridView_CellClick;
                MVVMUtils.AddDataBinding(dataGridView, "DataSource", _bindingSource, nameof(_model.IniReplacements));
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            IniReplacement r = (IniReplacement)grid.Rows[e.RowIndex].DataBoundItem;

            if (e.ColumnIndex == dataGridView.Columns["Delete"].Index)
            {
                _model.IniReplacements.Remove(r);
            }


        }

        private void ConfigureGrid()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoGenerateColumns = false;
            

            //create the column programatically


            var cols = new List<DataGridViewColumn>();

            cols.Add(new DataGridViewTextBoxColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                Name = "DefaultValue",
                HeaderText = "Value to search",
                DataPropertyName = "DefaultValue",
                MinimumWidth = 200,
        });


         



            cols.Add(new DataGridViewTextBoxColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                Name = "ReplaceValue",
                HeaderText = "Replacement value",
                DataPropertyName = "ReplaceValue",
                MinimumWidth = 200
            });

            // set font on all columns
            cols.ForEach(c=>c.DefaultCellStyle.Font=new Font("Consolas",9));

            // set the multiline on all columns

            cols.ForEach(c => c.DefaultCellStyle.WrapMode = DataGridViewTriState.True);


            cols.Add(new DataGridViewButtonColumn()
            {
                CellTemplate = new DataGridViewButtonCell() { Style = new DataGridViewCellStyle() { BackColor = Color.DarkRed } },
                Name = "Delete",
                HeaderText = "Delete",
                Text = "Delete",
                FillWeight = 20,
                MinimumWidth = 75,
                UseColumnTextForButtonValue = true
            });





            dataGridView.Columns.AddRange(cols.ToArray());
            

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _model.IniReplacements.Add(new IniReplacement());
        }

  
    }
}
