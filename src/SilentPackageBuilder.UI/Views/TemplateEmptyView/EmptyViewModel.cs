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
using System.Linq;
using System.Text;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views.TemplateEmptyView
{
    public class EmptyViewmodel : BaseViewModel
    {
        private EmptyModel _model = null;

        public EmptyViewmodel(EmptyModel model)
        {
            _model = model;
        }

        public string HelloWorld
        {
            get { return _model.HelloWorld; }

            set
            {
                _model.HelloWorld = value;
                this.OnPropertyChanged(nameof(HelloWorld));
            }
        }



    }
}
