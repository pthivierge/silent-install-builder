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
using ScintillaNET;
using SilentPackagesBuilder.Core;

namespace SilentPackagesBuilder.Views.PackageEditor
{
    public partial class ScriptEditor : UserControl, IModelSettableControl
    {
        private PowerShellCodeBlock _model;
        private BindingSource _bindingsource=new BindingSource();

        public ScriptEditor()
        {
            InitializeComponent();

            ConfigureCodeEditor();
        }

        public void SetModel(object model)
        {
            _model=model as PowerShellCodeBlock;

            ConfigureDataBindings();
        }

        private void ConfigureDataBindings()
        {
            if (_model != null)
            {
                _bindingsource.DataSource = _model;
                MVVMUtils.AddDataBinding(scintilla1,"Text",_bindingsource,nameof(_model.Code));
            }
        }

        private void ConfigureCodeEditor()
        {
            // Configuring the default style with properties
            scintilla1.StyleResetDefault();
            scintilla1.Styles[Style.Default].Font = "Lucida Console";
            scintilla1.Styles[Style.Default].Size = 9;
            
            scintilla1.StyleClearAll();


            
            scintilla1.Styles[Style.PowerShell.Default].ForeColor = Color.Black;
            
           // scintilla1.Styles[Style.PowerShell.Identifier].ForeColor = Color.FromArgb(148, 43, 226);
            scintilla1.Styles[Style.PowerShell.Keyword].ForeColor = Color.FromArgb(0, 0, 139);
            scintilla1.Styles[Style.PowerShell.Operator].ForeColor = Color.FromArgb(169, 169, 169);
            scintilla1.Styles[Style.PowerShell.Variable].ForeColor = Color.FromArgb(255, 69, 0);
           // scintilla1.Styles[Style.PowerShell.User1].ForeColor = Color.FromArgb(0, 128, 128);

            scintilla1.Styles[Style.PowerShell.Function].ForeColor = Color.Blue;
            scintilla1.Styles[Style.PowerShell.Cmdlet].ForeColor = Color.Blue;
            scintilla1.Styles[Style.PowerShell.Alias].ForeColor = Color.Blue;

            scintilla1.Styles[Style.PowerShell.Comment].ForeColor = Color.FromArgb(0, 100, 0); // Green
            scintilla1.Styles[Style.PowerShell.CommentDocKeyword].ForeColor = Color.FromArgb(0, 100, 0); // Green
            scintilla1.Styles[Style.PowerShell.CommentStream].ForeColor = Color.FromArgb(0, 100, 0); // Green

            scintilla1.Styles[Style.PowerShell.Number].ForeColor = Color.FromArgb(128, 0, 128);
            scintilla1.Styles[Style.PowerShell.String].ForeColor = Color.FromArgb(139, 0, 0); // Red
            scintilla1.Styles[Style.PowerShell.Character].ForeColor = Color.FromArgb(139, 0, 0); // Red

            

            /*
            Set keyword indexes:

            0 Commands
            1 Cmdlets
            2 Aliases
            3 Functions
            4 User1
            5 DocComment
            */


            scintilla1.SetKeywords(0, "begin break catch continue data do dynamicparam else elseif end exit filter finally for foreach from function if in local param private process return switch throw trap try until where while");
            scintilla1.SetKeywords(1, "add-computer add-content add-history add-member add-pssnapin add-type checkpoint-computer clear-content clear-eventlog clear-history clear-item clear-itemproperty clear-variable compare-object complete-transaction connect-wsman convert-path convertfrom-csv convertfrom-securestring convertfrom-stringdata convertto-csv convertto-html convertto-securestring convertto-xml copy-item copy-itemproperty debug-process disable-computerrestore disable-psbreakpoint disable-pssessionconfiguration disable-wsmancredssp disconnect-wsman enable-computerrestore enable-psbreakpoint enable-psremoting enable-pssessionconfiguration enable-wsmancredssp enter-pssession exit-pssession export-alias export-clixml export-console export-counter export-csv export-formatdata export-modulemember export-pssession foreach-object format-custom format-list format-table format-wide get-acl get-alias get-authenticodesignature get-childitem get-command get-computerrestorepoint get-content get-counter get-credential get-culture get-date get-event get-eventlog get-eventsubscriber get-executionpolicy get-formatdata get-help get-history get-host get-hotfix get-item get-itemproperty get-job get-location get-member get-module get-psbreakpoint get-pscallstack get-psdrive get-psprovider get-pssession get-pssessionconfiguration get-pssnapin get-pfxcertificate get-process get-random get-service get-tracesource get-transaction get-uiculture get-unique get-variable get-wsmancredssp get-wsmaninstance get-winevent get-wmiobject group-object import-alias import-clixml import-counter import-csv import-localizeddata import-module import-pssession invoke-command invoke-expression invoke-history invoke-item invoke-wsmanaction invoke-wmimethod join-path limit-eventlog measure-command measure-object move-item move-itemproperty new-alias new-event new-eventlog new-item new-itemproperty new-module new-modulemanifest new-object new-psdrive new-pssession new-pssessionoption new-service new-timespan new-variable new-wsmaninstance new-wsmansessionoption new-webserviceproxy out-default out-file out-gridview out-host out-null out-printer out-string pop-location push-location read-host receive-job register-engineevent register-objectevent register-pssessionconfiguration register-wmievent remove-computer remove-event remove-eventlog remove-item remove-itemproperty remove-job remove-module remove-psbreakpoint remove-psdrive remove-pssession remove-pssnapin remove-variable remove-wsmaninstance remove-wmiobject rename-item rename-itemproperty reset-computermachinepassword resolve-path restart-computer restart-service restore-computer resume-service select-object select-string select-xml send-mailmessage set-acl set-alias set-authenticodesignature set-content set-date set-executionpolicy set-item set-itemproperty set-location set-psbreakpoint set-psdebug set-pssessionconfiguration set-service set-strictmode set-tracesource set-variable set-wsmaninstance set-wsmanquickconfig set-wmiinstance show-eventlog sort-object split-path start-job start-process start-service start-sleep start-transaction start-transcript stop-computer stop-job stop-process stop-service stop-transcript suspend-service tee-object test-computersecurechannel test-connection test-modulemanifest test-path test-wsman trace-command undo-transaction unregister-event unregister-pssessionconfiguration update-formatdata update-list update-typedata use-transaction wait-event wait-job wait-process where-object write-debug write-error write-eventlog write-host write-output write-progress write-verbose write-warning");
            scintilla1.SetKeywords(2, "ac asnp cat cd chdir clc clear clhy cli clp cls clv compare copy cp cpi cpp cvpa dbp del diff dir ebp echo epal epcsv epsn erase etsn exsn fc fl foreach ft fw gal gbp gc gci gcm gcs gdr ghy gi gjb gl gm gmo gp gps group gsn gsnp gsv gu gv gwmi h history icm iex ihy ii ipal ipcsv ipmo ipsn ise iwmi kill lp ls man md measure mi mount move mp mv nal ndr ni nmo nsn nv ogv oh popd ps pushd pwd r rbp rcjb rd rdr ren ri rjb rm rmdir rmo rni rnp rp rsn rsnp rv rvpa rwmi sajb sal saps sasv sbp sc select set si sl sleep sort sp spjb spps spsv start sv swmi tee type where wjb write");
            scintilla1.SetKeywords(3, "clear-host disable-psremoting enable-psremoting get-verb help importsystemmodules mkdir more prompt psedit tabexpansion");

            // most common types does not work ... need to find out why if 
            // scintilla1.SetKeywords(4, "system system.math System.Collections System.Data System.Drawing System.IO System.Text System.Threading System.Timers System.Web System.Web.Services System.Windows.Forms System.Xml System.Reflection System.Reflection.Assembly");

            scintilla1.SetKeywords(5, "#~");
            





        }


    }
}
