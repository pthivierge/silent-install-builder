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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SilentPackagesBuilder
{
    public class PackageSearchModel
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(PackageSearchModel));

        public string SearchPath { get; set; }
        public string SearchFilter { get; set; }

        public BindingList<PackageFileInfo> Files { get; private set; } = new BindingList<PackageFileInfo>();

        private List<PackageFileInfo> _foundfiles = new List<PackageFileInfo>();

        private List<PackageFileInfo> _filterdFiles = new List<PackageFileInfo>();


        public int FilesCount { get; set; }

        public Action SearchCompleted;
        public Action SearchProgress;


        BackgroundWorker _worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        public PackageSearchModel()
        {
            _worker.DoWork += BackgroundSearch;
            _worker.ProgressChanged += _worker_ProgressChanged;

        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var files = (List<PackageFileInfo>)e.UserState;
            files.ForEach(f =>
            {
                _foundfiles.Add(f);
                Files.Add(f);
            });

            SearchProgress?.Invoke();
        }

        private void BackgroundSearch(object sender, DoWorkEventArgs args)
        {

            try
            {
           

                foreach (var fileInfo in GetFilesFrom(SearchPath, new string[] {"*.exe","*.zip"}, true))
                {

                        PackageFileInfo result = new PackageFileInfo()
                        {
                            FilePath = fileInfo.FullName,
                            FileVersion = FileVersionInfo.GetVersionInfo(fileInfo.FullName).FileVersion,
                            FileName = fileInfo.Name,
                            FileBytes = fileInfo.Length
                        };

                        _worker.ReportProgress(0, new List<PackageFileInfo>() { result });

                        if (_worker.CancellationPending)
                            break;
                    }

                




            }

            catch (Exception e)
            {
                _logger.Error(e);

            }
            finally
            {
                SearchCompleted?.Invoke();
            }


        }


        public void Search()
        {


            if (_worker.IsBusy)
                return;

            if (string.IsNullOrEmpty(SearchPath) || !Directory.Exists(SearchPath))
            {
                if (string.IsNullOrEmpty(SearchPath) || !Directory.Exists(SearchPath))
                {

                    throw new ApplicationException("Search directory is empty or does not exit.");
                }
            }

            Files.Clear();
            _foundfiles = new List<PackageFileInfo>();

            _worker.RunWorkerAsync();

        }

        public void CancelSearch()
        {
            _worker?.CancelAsync();
        }

        public void ApplyFilter()
        {
            if (string.IsNullOrEmpty(SearchFilter))
            {
                return;
            }

            _filterdFiles = _foundfiles.Where(f => f.FileName.ToLowerInvariant().Contains(SearchFilter.ToLowerInvariant())).ToList();
            Files = new BindingList<PackageFileInfo>(_filterdFiles);
        }


        /// <summary>
        /// A safe way to get all the files in a directory and sub directory without crashing on UnauthorizedException or PathTooLongException
        /// </summary>
        /// <param name="rootPath">Starting directory</param>
        /// <param name="patternMatch">Filename pattern match</param>
        /// <param name="searchOption">Search subdirectories or only top level directory for files</param>
        /// <returns>List of files</returns>
        public IEnumerable<FileInfo> GetFilesFrom(string dir, string[] search_pattern, bool recursive)
        {
            

            List<FileInfo> temp_files = new List<FileInfo>();

            try
            {
                var directory=new DirectoryInfo(dir);
                temp_files=search_pattern.SelectMany(s => directory.GetFiles(s, SearchOption.TopDirectoryOnly)).ToList();
            }
            catch { }

            foreach (var fileInfo in temp_files)
            {
                yield return fileInfo;
            }

            if (recursive)
            {
                DirectoryInfo[] temp_dirs = null;

                try
                {
                    var directory = new DirectoryInfo(dir);
                    temp_dirs = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
                }
                catch { }

                if (temp_dirs == null) yield break;
                for (int i = 0; i < temp_dirs.Length; i++)
                {
                    temp_files = GetFilesFrom(temp_dirs[i].FullName, search_pattern, recursive).ToList();
                    foreach (var fileInfo in temp_files)
                    {
                        yield return fileInfo;
                    }
                }
            }

        }



    }
}
