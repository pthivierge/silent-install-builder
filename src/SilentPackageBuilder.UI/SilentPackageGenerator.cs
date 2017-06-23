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
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Core.Models;
using DownloadProgressChangedEventArgs = System.Deployment.Application.DownloadProgressChangedEventArgs;

namespace SilentPackagesBuilder
{
    public static class SilentPackageGenerator
    {

        private static long TotalPercent = 0;
        private static long FileTransferedIndex = 0;
        private static long LastByteReceived = 0;
        private static BackgroundWorker _worker = new BackgroundWorker();

        public static void Create(InstallModel installModel, string outputFolder,bool configFileOnly=false)
        {
            TotalPercent = 0;
            FileTransferedIndex = 0;
            

            // set some variables that will be helpful
            var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Generate the powershell config files
            PowershellConfig.CreateInstallConfigFromObjects(installModel.Items, outputFolder);

            PowershellConfig.CreateVariablesConfigurationFile(installModel.UserVariablesModel.UserVariables, outputFolder);

            // move or copy the files in the directory

            // powershell scripts for the silent execution
            if (!Directory.Exists(Path.Combine(curDir, "powershell-template")))
            {
                throw new DirectoryNotFoundException("powershell-template directory could not be found.  This application expects this diretory to exist and contains the associated poweshell files to generate the silent package.");
            }

            var source = new DirectoryInfo(Path.Combine(curDir, "powershell-template"));
            var dest = new DirectoryInfo(Path.Combine(outputFolder));

            CopyAll(source, dest);

            if(configFileOnly)
                return;

            // setup packages


            var packagesDir = Path.Combine(outputFolder, "SetupPackages");
            Directory.CreateDirectory(packagesDir);


            // todo  - maybe refactor this as this is not very elegant!! 
            TotalPercent = installModel.Items.Where(p => p.Type != PackageType.PowershellCodeBlock).ToList().Count*100;
            foreach (dynamic package in installModel.Items)
            {
                // todo  - this is not very elegant!! 
                if (package.Type != PackageType.PowershellCodeBlock)
                {
                    FileTransferedIndex++;
                    var webclient = CopyFileWithProgress(package.FileInfo.FilePath, Path.Combine(packagesDir, package.FileInfo.FileName));

                    while (webclient.IsBusy)
                    {
                        Application.DoEvents();
                        Thread.Sleep(200);
                    }
                }
               

            }


            


        }



        // https://msdn.microsoft.com/en-us/library/system.io.directoryinfo.aspx
        //
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                // Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true); // will replace if already exist
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public delegate void ProgressDelegate(int percent);
        public static event ProgressDelegate FileCopyProgress;
        public static WebClient CopyFileWithProgress(string source, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += DownloadProgress;
            webClient.DownloadFileAsync(new Uri(source), destination);
            return webClient;
        }

        private static void DownloadProgress(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {

            double percent = ((FileTransferedIndex*100.0) + e.ProgressPercentage)/TotalPercent;
            int percentage = (int) (percent*100);
            FileCopyProgress?.Invoke(percentage > 100 ? 100 : percentage);
        }

    }
}
