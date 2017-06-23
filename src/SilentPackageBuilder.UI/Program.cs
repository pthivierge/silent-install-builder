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
using System.Windows.Forms;
using SilentPackagesBuilder;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Views;
using SilentPackagesBuilder.Views.DefineVariables;
using SilentPackagesBuilder.Views.PackageEditor.Editors;
using SilentPackagesBuilder.Views.PackagesList;

namespace SilentPackagesBuilderGUI
{
    public static class Program
    {

        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Program));
        private static Form _mainForm = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // *******************************
            // TESTS AND DEBUG
            // *******************************
            // Package search demo
            // Application.Run(new PackageSearchControlDemo());

            // PackageListControll Demo
            //Application.Run(new PackagesListDemo());

            // PackgeEditorDemo
            // Application.Run(new PackageEditorDemo());

            //Application.Run(new DefineVariablesDemo());


            // *******************************

            UserSettings.Default.PropertyChanged += SaveUserSettings;


            _mainForm = new frmMain();
            Application.Run(_mainForm);


        }



        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // here we just make sure any unhandled error goes to to the log file
            _logger.Error(e.Exception);

            // removed because it ended up throwing non-fatal exception - e.g. double click on grid header in the packageEditorDemo
            // throw (e.Exception);
        }

        public static void ShowPackageEditor(IInstallationStep step)
        {
            if (UserSettings.Default.UIOpenEditorOnAdd)
            {
                var f = new frmPackageEditor(step);
                f.WindowState = FormWindowState.Minimized;
                f.Show(_mainForm);
                f.WindowState = FormWindowState.Normal;
            }
        }

        private static void SaveUserSettings(object sender, PropertyChangedEventArgs e)
        {
            UserSettings.Default.Save();
        }

    }
}
