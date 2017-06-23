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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SilentPackagesBuilder.Core;


namespace SilentPackagesBuilder
{
    /// <summary>
    /// This class is the main model class, it manages all the installation packages that the user configures
    /// </summary>
    public class InstallModel
    {


        private BindingList<IInstallationStep> _installationSteps = new BindingList<IInstallationStep>();

        public BindingList<IInstallationStep> Items
        {
            get { return _installationSteps; }
        }


        /// this list contains variables definition
        public VariablesDefinitionModel UserVariablesModel { get; set; }=new VariablesDefinitionModel();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="installationStep"></param>
        /// <param name="position">position of the install package</param>
        public void Add(IInstallationStep installationStep, IInstallationStep before = null)
        {
            int position = -1;
            if (before != null)
            {
                position = _installationSteps.ToList().FindIndex(x => x.Id == before.Id);
            }
            position = position == -1 ? _installationSteps.Count : position;
            _installationSteps.Insert(position, installationStep);

        }

        public void Remove(IInstallationStep installationStep)
        {
            _installationSteps.Remove(installationStep);

        }



        public void Update(IInstallationStep installationStep)
        {

            var installStep = _installationSteps.ToList().Find(x => x.Id == installationStep.Id);

            if (installStep == null)
                throw new ApplicationException("Cannot update the installation step, it does not exist in the current collection");

            installStep.DisplayName = installationStep.DisplayName;
            installStep.Install = installationStep.Install;


        }

        private static XmlSerializer GetSerializer()
        {
            Type[] dataTypes = { typeof(PowerShellCodeBlock), typeof(PIDataArchive), typeof(Executable), typeof(OSIAutoExtractSetupPackage),typeof(VariablesDefinitionModel) };
            XmlSerializer serializer = new XmlSerializer(typeof(List<object>), dataTypes);
            return serializer;
        }

        public void Save(string path)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly() ?? System.Reflection.Assembly.GetExecutingAssembly(); // getexecuting assembly is for unit testing
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            // Serialize 
            var serializer = GetSerializer();
            FileStream fs = new FileStream(path, FileMode.Create);

            var dataToSerialize = new List<object>();
            dataToSerialize.Add($"Generated with Silent Package Builder Version: {version}"); // adding the version in the file for information.  will not be deserializd... 
            dataToSerialize.AddRange(_installationSteps.Select(o => (object)o));
            dataToSerialize.Add((object)UserVariablesModel);
            serializer.Serialize(fs, dataToSerialize);
            fs.Close();

        }

        public static InstallModel Deserialize(string path)
        {
            // Deserialize
            List<object> data = null;
            var serializer = GetSerializer();
            using (var fs = new FileStream(path, FileMode.Open))
            {
                data = (List<object>)serializer.Deserialize(fs);
            }

            var model = new InstallModel();
            foreach (var o in data)
            {

                if (o is IInstallationStep)
                {
                    //var step = o as IInstallationStep;
                    model.Add((IInstallationStep)o);
                }

                if (o is VariablesDefinitionModel)
                {
                    model.UserVariablesModel = (VariablesDefinitionModel) o;
                }


            }



            return model;
        }


    }


}
