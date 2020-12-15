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

namespace DotNetNuke.Modules.Reports
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The ViewReports class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    
    [DNNtc.ModuleDependencies(DNNtc.ModuleDependency.CoreVersion, "9.4.0")]
    [ModuleControlProperties("", "Reports Container", ControlType.View, "https://github.com/DNNCommunity/DNN.Reports/wiki", true, false)]
    public partial class ViewReports : PortalModuleBase, IActionable
    {
        private ReportInfo Report;

        #region  IActionable Members

        public ModuleActionCollection ModuleActions
        {
            get
                {
                    var actions = new ModuleActionCollection();
                    actions.Add(GetNextActionID(),
                                Localization.GetString("ManagePackages.Action", LocalResourceFile),
                                "", "", ResolveUrl("images/ManagePackages.gif"), EditUrl("ManagePackages"),
                                false, SecurityAccessLevel.Host, true, false);
                    return actions;
                }
        }

        #endregion

        #region  Error Handlers

        private void HandleVisualizerException(VisualizerException vex)
        {
            if (IsEditable)
            {
                Skin.AddModuleMessage(this,
                    string.Format(Localization.GetString("VisualizerError.Message",
                            LocalResourceFile),
                        vex.LocalizedMessage),
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void HandleDataSourceException(DataSourceException ex)
        {
            if (UserInfo.IsSuperUser)
            {
                Skin.AddModuleMessage(this,
                                      string.Format(
                                          Localization.GetString("HostDSError.Message", LocalResourceFile),
                                          ex.LocalizedMessage),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
            else if (IsEditable)
            {
                Skin.AddModuleMessage(this, Localization.GetString("AdminDSError.Message", LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }

        private void HandleMissingVisualizerError()
        {
            if (IsEditable)
            {
                Skin.AddModuleMessage(this,
                                      Localization.GetString("VisualizerDoesNotExist.Text",
                                                             LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private void HandleVisualizerLoadError()
        {
            Skin.AddModuleMessage(this,
                                  Localization.GetString("VisualizerLoadError.Text",
                                                         LocalResourceFile),
                                  ModuleMessage.ModuleMessageType.RedError);
        }

        #endregion

        #region  Helper Methods

        private bool VisualizerFolderExists(string visualizerFolder)
        {
            return Directory.Exists(
                Server.MapPath(ResolveUrl(string.Format("Visualizers/{0}", visualizerFolder))));
        }

        private string GetVisualizerFolder()
        {
            var sVisualizerFolder = Convert.ToString(Settings[ReportsConstants.SETTING_Visualizer]);
            if (string.IsNullOrEmpty(sVisualizerFolder) || !VisualizerFolderExists(sVisualizerFolder))
            {
                if (VisualizerFolderExists("Grid"))
                {
                    // Default to grid if its installed, otherwise default to none selected
                    // This should cover most upgrades from pre-Visualizer versions as long as the
                    // user sticks with the default and installs Grid, if not, then they are advanced
                    // enough to know what to do.

                    sVisualizerFolder = "Grid";
                }
                else if (IsEditable)
                {
                    Skin.AddModuleMessage(this,
                                          Localization.GetString("VisualizerNotConfigured.Text",
                                                                 LocalResourceFile),
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
                ctrlVisualizer = LoadControl(sVisualizerControl) as VisualizerControlBase;
                ctrlVisualizer.Initialize(
                    new ExtensionContext(TemplateSourceDirectory, "Visualizer", sVisualizerName));
            }
            catch (VisualizerException vex)
            {
                HandleVisualizerException(vex);
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
                    report, string.Concat(ReportsConstants.CACHEKEY_Reports, Convert.ToString(ModuleId)),
                    report.CacheDuration <= 0, this, ref fromCache);
            }
            catch (DataSourceException ex)
            {
                // Display the error message to host users only
                if (UserInfo.IsSuperUser)
                {
                    Skin.AddModuleMessage(this,
                                          string.Format(Localization.GetString("HostExecuteError.Message",
                                                                               LocalResourceFile),
                                                        ex.LocalizedMessage),
                                          ModuleMessage.ModuleMessageType.RedError);
                }
                else if (IsEditable)
                {
                    Skin.AddModuleMessage(this,
                                          Localization.GetString("AdminExecuteError.Message",
                                                                 LocalResourceFile),
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
            var sVisualizerFolder = GetVisualizerFolder();

            // If the visualizer folder is now non-empty
            if (!string.IsNullOrEmpty(sVisualizerFolder))
            {
                // Find the visualizer control
                var sVisualizerControl =
                    ResolveUrl(string.Format("Visualizers/{0}/{1}", sVisualizerFolder,
                                                  ReportsConstants.FILENAME_VisualizerASCX));
                if (!File.Exists(Server.MapPath(sVisualizerControl)))
                {
                    HandleMissingVisualizerError();
                }
                else
                {
                    var ctlVisualizer = LoadVisualizerControl(sVisualizerControl, sVisualizerFolder);
                    if (ctlVisualizer != null)
                    {
                        ctlVisualizer.ID = "Visualizer";
                        ctlVisualizer.ParentModule = this;
                        DataTable results = null;
                        var fromCache = false;
                        if (ctlVisualizer.AutoExecuteReport &&
                            !AutoExecuteReport(ctlVisualizer, Report, ref results, fromCache))
                        {
                            return;
                        }
                        ctlVisualizer.SetReport(Report, results, fromCache);
                        VisualizerSection.Controls.Clear();
                        VisualizerSection.Controls.Add(ctlVisualizer);
                    }
                    else if (IsEditable)
                    {
                        HandleVisualizerLoadError();
                    }
                }
            }
        }

        #endregion

        #region  Event Handlers

        protected void Page_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex is DataSourceException)
            {
                HandleDataSourceException((DataSourceException) ex);
            }
            else if (ex is VisualizerException)
            {
                HandleVisualizerException((VisualizerException) ex);
            }
            else
            {
                OnError(e);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserInfo.IsSuperUser && Directory.Exists(Server.MapPath("App_Code/Reports")))
            {
                Skin.AddModuleMessage(this, Localization.GetString("CleanUpOldAppCode.Text", LocalResourceFile),
                                      ModuleMessage.ModuleMessageType.YellowWarning);
            }

            Report = ReportsController.GetReport(ModuleConfiguration);
            InfoPane.Visible = Report.ShowInfoPane;
            ControlsPane.Visible = Report.ShowControls;
			ExportExcelButton.Visible = Report.ExportExcel;


			if (Report.ShowInfoPane)
            {
                TitleLiteral.Text = Report.Title;
                DescriptionLiteral.Text = Report.Description;
            }

            if (Report.AutoRunReport)
            {
                RunReport();
            }
        }

        protected void RunReportButton_Click(object sender, EventArgs e)
        {
            RunReport();
            ClearReportButton.Visible = true;
        }

        protected void ClearReportButton_Click(object sender, EventArgs e)
        {
            VisualizerSection.Controls.Clear();
            ClearReportButton.Visible = false;
        }

		protected void ExportExcelButton_Click(object sender, EventArgs e)
		{
			DataTable results = null;
			results = ReportsController.ExecuteReport(Report, String.Concat(ReportsConstants.CACHEKEY_Reports, ModuleId), true, this);
			Session["report_export"] = results;
			Response.Redirect("~/desktopmodules/reports/excel.aspx");
		}

		#endregion
	}
}