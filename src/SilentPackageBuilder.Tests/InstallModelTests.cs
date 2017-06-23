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
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentPackagesBuilder;
using SilentPackagesBuilder.Core;
using SilentPackagesBuilder.Core.Models;

namespace SilentPackageBuilder.Tests
{
    [TestClass]
    public class InstallModelTests
    {
     
        private InstallModel _mainModel = null;

        [TestInitialize]
        public void TestInitialization()
        {
            _mainModel = ModelUtils.GetTestModel();
        }

        [TestMethod]
        public void TestCanAddNewInstallPackages()
        {
            var model = new InstallModel();
            var package1 = new PIDataArchive();
            var package2 = new Executable();
            var package3 = new OSIAutoExtractSetupPackage();
            var package4 = new PowerShellCodeBlock();

            model.Add(package1);
            model.Add(package2);
            model.Add(package3);
            model.Add(package4);

            Assert.IsTrue(model.Items.Count == 4);

        }

        [TestMethod]
        public void TestCanRemovePackage()
        {
            var model = new InstallModel();
            var package1 = new PIDataArchive() {DisplayName = "1"};
            var package2 = new PowerShellCodeBlock() {DisplayName = "2" };

            model.Add(package1);
            model.Add(package2);

            model.Remove(package1);

            Assert.IsTrue(model.Items[0].Id == package2.Id,"remaining item is package 2");
            Assert.IsTrue(model.Items.Count==1,"only one item in the list");
        }

        [TestMethod]
        public void TestCanInsertPackage()
        {
            var model = new InstallModel();
            var package1 = new PIDataArchive() { DisplayName = "1" };
            var package2 = new PowerShellCodeBlock() { DisplayName = "2" };
            var package3 = new PowerShellCodeBlock() { DisplayName = "3" };

            model.Add(package1);
            model.Add(package2);

            Assert.IsTrue(model.Items[0].Id == package1.Id, "package 1 is first");
            Assert.IsTrue(model.Items[1].Id == package2.Id, "package 2 is second");

            
            model.Add(package3, package2);

            Assert.IsTrue(model.Items[0].Id == package1.Id, "package 1 is first");
            Assert.IsTrue(model.Items[1].Id == package3.Id, "package 3 is second");
            Assert.IsTrue(model.Items[2].Id == package2.Id, "package 2 is third");



            model.Remove(package1);


        }

        [TestMethod]
        public void TestCanISaveModel()
        {
            var path= Path.Combine(Environment.GetEnvironmentVariable("TEMP"),"InstallModelTest.xml");
            _mainModel.Save(path);   

        }

        [TestMethod]
        public void canLoadModelFromDisk()
        {
            var path = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "InstallModelTest.xml");
            var model=InstallModel.Deserialize(path);
            Assert.AreEqual(model.Items.Count,4);
        }
    }
}
