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


namespace DotNetNuke.Modules.Reports.Visualizers
{
    using System;
    using System.Data;
    using Components;
    using DotNetNuke.Modules.Reports.Exceptions;
    using DotNetNuke.Modules.Reports.Extensions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Skins;
    using DotNetNuke.UI.Skins.Controls;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The VisualizerControlBase class provides a base class for Visualizer Controls
    /// </summary>
    /// <remarks>
    ///     Visualizer Controls are the component of the visualizer that actually renders the report data.
    /// </remarks>
    /// <history>
    ///     [anurse]     08/31/2006    Documented
    /// </history>
    /// -----------------------------------------------------------------------------
    public class VisualizerControlBase : ReportsControlBase, IVisualizerControl
    {
        //#Region " Event Handlers "

        #region  Private Fields

        private bool _fromCache;

        #endregion

        #region  Properties

        /// <summary>
        ///     Gets the results of the report, or a null reference (Nothing in VB.Net)
        ///     if the report has not been executed
        /// </summary>
        public DataTable ReportResults { get; private set; }

        /// <summary>
        ///     Gets the object representing the report settings
        /// </summary>
        public ReportInfo Report { get; private set; }

        /// <summary>
        ///     Gets a boolean indicating if the results are from the cache
        /// </summary>
        public bool FromCache => this._fromCache;

        /// <summary>
        ///     Gets a boolean indicating if the report should be executed automatically and
        ///     provided to the visualizer
        /// </summary>
        public virtual bool AutoExecuteReport => true;

        protected override string ASCXFileName => ReportsConstants.FILENAME_VisualizerASCX;

        /// <summary>
        ///     Gets a boolean indicating if the current request is the first one
        ///     since the report was set
        /// </summary>
        protected bool IsFirstRun { get; private set; }

        #endregion

        #region  Methods

        protected void LoadReport()
        {
            this.Report = ReportsController.GetReport(this.ParentModule.ModuleConfiguration);
        }

        /// <summary>
        ///     Executes the report, suppressing any exception thrown by the data source and
        ///     displaying any errors that occur
        /// </summary>
        /// <remarks>The <see cref="ReportResults" /> property contains the data table resulting from executing the report</remarks>
        protected void ExecuteReport()
        {
            this.ExecuteReport(false, true);
        }

        /// <summary>
        ///     Executes the report, displaying any errors that occur and
        ///     optionally suppressing any exception thrown by the data source.
        /// </summary>
        /// <param name="ThrowOnError">A boolean indicating if exceptions thrown by the data source should be suppressed</param>
        /// <remarks>The <see cref="ReportResults" /> property contains the data table resulting from executing the report</remarks>
        protected void ExecuteReport(bool ThrowOnError)
        {
            this.ExecuteReport(ThrowOnError, true);
        }

        protected void ExecuteReport(bool ThrowOnError, bool ShowErrorMessage)
        {
            try
            {
                this.ReportResults =
                    ReportsController.ExecuteReport(this.Report,
                                                    string.Concat(ReportsConstants.CACHEKEY_Reports,
                                                                  Convert.ToString(this.ModuleId)), false,
                                                    this.ParentModule, ref this._fromCache);
            }
            catch (DataSourceException exc)
            {
                if (ShowErrorMessage)
                {
                    if (this.ParentModule.UserInfo.IsSuperUser)
                    {
                        Skin.AddModuleMessage(this.ParentModule,
                                              string.Format(
                                                  Localization.GetString(
                                                      "HostDSError.Message", this.ParentModule.LocalResourceFile),
                                                  exc.Message),
                                              ModuleMessage.ModuleMessageType.RedError);
                    }
                    else
                    {
                        Skin.AddModuleMessage(this.ParentModule,
                                              Localization.GetString("AdminDSError.Message",
                                                                     this.ParentModule.LocalResourceFile),
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }

                if (ThrowOnError)
                {
                    throw;
                }
            }
        }

        public void SetReport(ReportInfo Report, DataTable Results, bool FromCache)
        {
            this.Report = Report;
            this.ReportResults = Results;
            this._fromCache = FromCache;
            this.IsFirstRun = true;
        }

        protected bool ValidateDataSource()
        {
            return this.ValidateDataSource(true);
        }

        protected bool ValidateDataSource(bool ShowMessage)
        {
            if (string.IsNullOrEmpty(this.Report.DataSource))
            {
                if (ShowMessage)
                {
                    if (this.ParentModule.UserInfo.IsSuperUser)
                    {
                        Skin.AddModuleMessage(this.ParentModule,
                                              Localization.GetString("HostNoReport.Message",
                                                                     this.ParentModule.LocalResourceFile),
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                    else if (this.ParentModule.IsEditable)
                    {
                        Skin.AddModuleMessage(this.ParentModule,
                                              Localization.GetString("AdminNoReport.Message",
                                                                     this.ParentModule.LocalResourceFile),
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }
                return false;
            }
            return true;
        }

        protected bool ValidateResults()
        {
            return this.ValidateResults(true);
        }

        protected bool ValidateResults(bool ShowMessage)
        {
            if (ReferenceEquals(this.ReportResults, null) || this.ReportResults.Rows.Count == 0)
            {
                if (ShowMessage)
                {
                    if (this.ParentModule.IsEditable)
                    {
                        Skin.AddModuleMessage(this.ParentModule,
                                              Localization.GetString("NoResults.Message",
                                                                     this.ParentModule.LocalResourceFile),
                                              ModuleMessage.ModuleMessageType.YellowWarning);
                    }
                }
                return false;
            }
            return true;
        }

        #endregion

        //        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        //            Me.IsFirstRun = False
        //        End Sub

        //#End Region
    }
}