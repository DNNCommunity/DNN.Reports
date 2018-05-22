#region Copyright

// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2018
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

#endregion


namespace DotNetNuke.Modules.Reports
{
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using Components;
    using DNNtc;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Modules.Reports.Exceptions;
    using DotNetNuke.Modules.Reports.Extensions;
    using DotNetNuke.Modules.Reports.Visualizers;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Skins;
    using DotNetNuke.UI.Skins.Controls;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The ViewReports class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    [ModuleDependencies(ModuleDependency.CoreVersion, "8.0.1")]
    [ModuleControlProperties("", "", ControlType.View, "", false, false)]
    public partial class ViewReports : PortalModuleBase, IActionable
    {
        private ReportInfo Report;

        #region  IActionable Members

        public ModuleActionCollection ModuleActions
        {
            get
                {
                    var actions = new ModuleActionCollection();
                    actions.Add(this.GetNextActionID(),
                                Localization.GetString("ManagePackages.Action", this.LocalResourceFile),
                                "", "", this.ResolveUrl("images/ManagePackages.gif"), this.EditUrl("ManagePackages"),
                                false, SecurityAccessLevel.Host, true, false);
                    return actions;
                }
        }

        #endregion

        #region  Error Handlers

        private void HandleVisualizerException(VisualizerException vex)
        {
            if (this.IsEditable)
            {
                Skin.AddModuleMessage(this,
                                      string.Format(Localization.GetString("VisualizerError.Message",
                                                                           this.LocalResourceFile),
                                                    vex.LocalizedMessage),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void HandleDataSourceException(DataSourceException ex)
        {
            if (this.UserInfo.IsSuperUser)
            {
                Skin.AddModuleMessage(this,
                                      string.Format(
                                          Localization.GetString("HostDSError.Message", this.LocalResourceFile),
                                          ex.LocalizedMessage),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
            else if (this.IsEditable)
            {
                Skin.AddModuleMessage(this, Localization.GetString("AdminDSError.Message", this.LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

        private void HandleMissingVisualizerError()
        {
            if (this.IsEditable)
            {
                Skin.AddModuleMessage(this,
                                      Localization.GetString("VisualizerDoesNotExist.Text",
                                                             this.LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void HandleVisualizerLoadError()
        {
            Skin.AddModuleMessage(this,
                                  Localization.GetString("VisualizerLoadError.Text",
                                                         this.LocalResourceFile),
                                  ModuleMessage.ModuleMessageType.RedError);
        }

        #endregion

        #region  Helper Methods

        private bool VisualizerFolderExists(string visualizerFolder)
        {
            return Directory.Exists(
                this.Server.MapPath(this.ResolveUrl(string.Format("Visualizers/{0}", visualizerFolder))));
        }

        private string GetVisualizerFolder()
        {
            var sVisualizerFolder = Convert.ToString(this.Settings[ReportsConstants.SETTING_Visualizer]);
            if (string.IsNullOrEmpty(sVisualizerFolder) || !this.VisualizerFolderExists(sVisualizerFolder))
            {
                if (this.VisualizerFolderExists("Grid"))
                {
                    // Default to grid if its installed, otherwise default to none selected
                    // This should cover most upgrades from pre-Visualizer versions as long as the
                    // user sticks with the default and installs Grid, if not, then they are advanced
                    // enough to know what to do.

                    sVisualizerFolder = "Grid";
                }
                else if (this.IsEditable)
                {
                    Skin.AddModuleMessage(this,
                                          Localization.GetString("VisualizerNotConfigured.Text",
                                                                 this.LocalResourceFile),
                                          ModuleMessage.ModuleMessageType.RedError);
                }
            }
            return sVisualizerFolder;
        }

        private VisualizerControlBase LoadVisualizerControl(string sVisualizerControl, string sVisualizerName)
        {
            VisualizerControlBase ctrlVisualizer = null;
            try
            {
                // Load the visualizer control
                ctrlVisualizer = this.LoadControl(sVisualizerControl) as VisualizerControlBase;
                ctrlVisualizer.Initialize(
                    new ExtensionContext(this.TemplateSourceDirectory, "Visualizer", sVisualizerName));
            }
            catch (VisualizerException vex)
            {
                this.HandleVisualizerException(vex);
                Services.Exceptions.Exceptions.LogException(vex);
            }
            catch (HttpCompileException ex)
            {
                Services.Exceptions.Exceptions.LogException(ex);
            }
            return ctrlVisualizer;
        }

        private bool AutoExecuteReport(VisualizerControlBase ctlVisualizer, ReportInfo report, ref DataTable results,
                                       bool fromCache)
        {
            try
            {
                results = ReportsController.ExecuteReport(
                    report, string.Concat(ReportsConstants.CACHEKEY_Reports, Convert.ToString(this.ModuleId)),
                    report.CacheDuration <= 0, this, ref fromCache);
            }
            catch (DataSourceException ex)
            {
                // Display the error message to host users only
                if (this.UserInfo.IsSuperUser)
                {
                    Skin.AddModuleMessage(this,
                                          string.Format(Localization.GetString("HostExecuteError.Message",
                                                                               this.LocalResourceFile),
                                                        ex.LocalizedMessage),
                                          ModuleMessage.ModuleMessageType.RedError);
                }
                else if (this.IsEditable)
                {
                    Skin.AddModuleMessage(this,
                                          Localization.GetString("AdminExecuteError.Message",
                                                                 this.LocalResourceFile),
                                          ModuleMessage.ModuleMessageType.RedError);
                }
                Services.Exceptions.Exceptions.LogException(ex);
                ctlVisualizer.Visible = false;
                return false;
            }
            return true;
        }

        private void RunReport()
        {
            // Check for a visualizer
            var sVisualizerFolder = this.GetVisualizerFolder();

            // If the visualizer folder is now non-empty
            if (!string.IsNullOrEmpty(sVisualizerFolder))
            {
                // Find the visualizer control
                var sVisualizerControl =
                    this.ResolveUrl(string.Format("Visualizers/{0}/{1}", sVisualizerFolder,
                                                  ReportsConstants.FILENAME_VisualizerASCX));
                if (!File.Exists(this.Server.MapPath(sVisualizerControl)))
                {
                    this.HandleMissingVisualizerError();
                }
                else
                {
                    var ctlVisualizer = this.LoadVisualizerControl(sVisualizerControl, sVisualizerFolder);
                    if (ctlVisualizer != null)
                    {
                        ctlVisualizer.ID = "Visualizer";
                        ctlVisualizer.ParentModule = this;
                        DataTable results = null;
                        var fromCache = false;
                        if (ctlVisualizer.AutoExecuteReport &&
                            !this.AutoExecuteReport(ctlVisualizer, this.Report, ref results, fromCache))
                        {
                            return;
                        }
                        ctlVisualizer.SetReport(this.Report, results, fromCache);
                        this.VisualizerSection.Controls.Clear();
                        this.VisualizerSection.Controls.Add(ctlVisualizer);
                    }
                    else if (this.IsEditable)
                    {
                        this.HandleVisualizerLoadError();
                    }
                }
            }
        }

        #endregion

        #region  Event Handlers

        protected void Page_Error(object sender, EventArgs e)
        {
            var ex = this.Server.GetLastError();
            if (ex is DataSourceException)
            {
                this.HandleDataSourceException((DataSourceException) ex);
            }
            else if (ex is VisualizerException)
            {
                this.HandleVisualizerException((VisualizerException) ex);
            }
            else
            {
                this.OnError(e);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UserInfo.IsSuperUser && Directory.Exists(this.Server.MapPath("App_Code/Reports")))
            {
                Skin.AddModuleMessage(this, Localization.GetString("CleanUpOldAppCode.Text", this.LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.YellowWarning);
            }

            this.Report = ReportsController.GetReport(this.ModuleConfiguration);
            this.InfoPane.Visible = this.Report.ShowInfoPane;
            this.ControlsPane.Visible = this.Report.ShowControls;

            if (this.Report.ShowInfoPane)
            {
                this.TitleLiteral.Text = this.Report.Title;
                this.DescriptionLiteral.Text = this.Report.Description;
            }

            if (this.Report.AutoRunReport)
            {
                this.RunReport();
            }
        }

        protected void RunReportButton_Click(object sender, EventArgs e)
        {
            this.RunReport();
            this.ClearReportButton.Visible = true;
        }

        protected void ClearReportButton_Click(object sender, EventArgs e)
        {
            this.VisualizerSection.Controls.Clear();
            this.ClearReportButton.Visible = false;
        }

        #endregion
    }
}