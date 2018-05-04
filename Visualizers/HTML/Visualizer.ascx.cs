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

namespace DotNetNuke.Modules.Reports.Visualizers.Html
{
    using System;
    using System.Data;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using Components;
    using DotNetNuke.Security;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Visualizer class displays the Html Template
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Visualizer : VisualizerControlBase
    {
        #region  Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsFirstRun)
            {
                this.DataBind();
            }
        }

        #endregion

        #region  Overrides

        public override void DataBind()
        {
            // Get the report for this module
            if (!this.ValidateDataSource() || !this.ValidateResults())
            {
                this.pnlContent.Visible = false;
            }
            else
            {
                this.pnlContent.Visible = true;
                var sFileID =
                    Convert.ToString(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                       ReportsConstants.SETTING_Html_TemplateFile,
                                                                       string.Empty));
                if (!string.IsNullOrEmpty(sFileID))
                {
                    var sFile = Utilities.MapFileIdPath(this.ParentModule.PortalSettings, sFileID);
                    if (!string.IsNullOrEmpty(sFile))
                    {
                        var sHtml = File.ReadAllText(sFile);
                        if (!string.IsNullOrEmpty(sHtml))
                        {
                            // Iterate over each row
                            foreach (DataRow row in this.ReportResults.Rows)
                            {
                                var rowHtml = sHtml;
                                foreach (DataColumn dcol in this.ReportResults.Columns)
                                {
                                    rowHtml = rowHtml.Replace(string.Format("[{0}]", dcol.ColumnName),
                                                              Convert.ToString(row[dcol].ToString()));
                                }

                                var objSec = new PortalSecurity();
                                var divContent = new HtmlGenericControl("div");
                                divContent.Attributes["class"] = "DNN_Reports_HTML_Item";
                                divContent.InnerHtml =
                                    objSec.InputFilter(rowHtml, PortalSecurity.FilterFlag.NoScripting);
                                this.pnlContent.Controls.Add(divContent);
                            }
                        }
                        base.DataBind();
                    }
                }
            }
        }

        #endregion
    }
}