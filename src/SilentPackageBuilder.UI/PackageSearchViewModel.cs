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
using System.ComponentModel;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views
{
    public class PackageSearchViewModel :BaseViewModel
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PackageSearchViewModel));
        private PackageSearchModel _model = new PackageSearchModel();
        private bool _isSearching;

        public PackageSearchViewModel()
        {
            // sets the notification for the view when the model has completed to search 
            _model.SearchCompleted+= () =>
            {
                this.OnPropertyChanged(nameof(Files));
                this.OnPropertyChanged(nameof(FilesCount));
                IsSearching = false;
            };

            _model.SearchProgress += () =>
            {
                this.OnPropertyChanged(nameof(FilesCount));
            };
        }

        public void Search(object sender, EventArgs eventArgs)
        {
            try
            {

           
            IsSearching = true;
            UserSettings.Default.LastSelectedPackagesDirectory = _model.SearchPath;
            
            _model.Search();

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}.  See logs for the full stack trace.", "An error occured with the search:",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                _logger.Error(e);
            }

        }

        public void Cancel(object sender, EventArgs eventArgs)
        {
            _model.CancelSearch();
        }

        public string SearchPath
        {
            get { return _model.SearchPath; }

            set
            {
                _model.SearchPath = value;
                this.OnPropertyChanged(nameof(SearchPath));
            }
        }

        public string SearchFilter
        {
            get { return _model.SearchFilter; }

            set
            {
                _model.SearchFilter = value;
                _model.ApplyFilter();
                this.OnPropertyChanged(nameof(SearchFilter));
                this.OnPropertyChanged(nameof(Files));
                this.OnPropertyChanged(nameof(FilesCount));
            }
        }

        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                _isSearching = value;
                this.OnPropertyChanged(nameof(IsSearching));
            }
        }


        public int FilesCount
        {
            get { return _model.Files.Count; }

            set
            {
                _model.FilesCount = value;
                this.OnPropertyChanged(nameof(FilesCount));
            }
        }



        public BindingList<PackageFileInfo> Files
        {
            get { return _model.Files; }
        }


      



    }
}
