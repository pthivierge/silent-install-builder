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
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SilentPackagesBuilder.Components
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    public class BindableToolStripStatusLabel : ToolStripStatusLabel, IBindableComponent
    {
        private BindingContext _context = null;
        public BindingContext BindingContext
        {
            get
            {
                if (null == _context)
                {
                    _context = new BindingContext();
                }
                return _context;
            }
            set { _context = value; }
        }
        private ControlBindingsCollection _bindings;
        public ControlBindingsCollection DataBindings
        {
            get
            {
                if (null == _bindings)
                {
                    _bindings = new ControlBindingsCollection(this);
                }
                return _bindings;
            }
            set { _bindings = value; }
        }
    }
}
