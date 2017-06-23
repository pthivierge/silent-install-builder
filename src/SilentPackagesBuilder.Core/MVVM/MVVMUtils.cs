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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilentPackagesBuilder.Core
{
    public static class MVVMUtils
    {
        public static void AddDataBinding(IBindableComponent component, string propertyToBind, BindingSource bindingSource, string modelPropertyName)
        {
            component.DataBindings.Add(propertyToBind, bindingSource, modelPropertyName, true,
                DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
