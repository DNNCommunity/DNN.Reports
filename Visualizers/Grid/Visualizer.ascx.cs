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


namespace DotNetNuke.Modules.Reports.Visualizers.Grid
{
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using Components;
    using DotNetNuke.Common.Utilities;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Visualizer class displays the Grid
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Visualizer : VisualizerControlBase
    {
        #region  Overrides

        public override void DataBind()
        {
            // Get the report for this module
            if (!this.ValidateDataSource() || !this.ValidateResults())
            {
                this.grdResults.Visible = false;
            }
            else
            {
                this.grdResults.Visible = true;
                this.GenerateColumns(this.ReportResults);
                var view = new DataView(this.ReportResults);
                if (!string.IsNullOrEmpty(this.SortExpr))
                {
                    view.Sort = string.Format("{0} {1}", this.SortExpr, this.SortDir);
                }
                this.grdResults.DataSource = view;
                this.ConfigureGridFromSettings();
            }
            base.DataBind();
        }

        #endregion

        #region  Private Properties

        private string SortExpr
        {
            get
                {
                    var o = this.ViewState["SortExpr"];
                    if (o != null && o is string)
                    {
                        return o.ToString();
                    }
                    return Null.NullString;
                }
            set { this.ViewState["SortExpr"] = value; }
        }

        private string SortDir
        {
            get
                {
                    var o = this.ViewState["SortDir"];
                    if (o != null && o is string)
                    {
                        return o.ToString();
                    }
                    return "ASC";
                }
            set { this.ViewState["SortDir"] = value; }
        }

        #endregion

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

        protected void grdResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.grdResults.PageIndex = e.NewPageIndex;
            this.DataBind();
        }

        protected void grdResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.Equals(this.SortExpr)) // Toggle SortDir
            {
                if (this.SortDir == "ASC")
                {
                    this.SortDir = "DESC";
                }
                else
                {
                    this.SortDir = "ASC";
                }
            }
            else // Sort Ascending first when a new column selected for sort
            {
                this.SortDir = "ASC";
            }
            this.SortExpr = e.SortExpression;
            this.DataBind();
        }

        protected void grdResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // If the row is a header, add sort direction images
            if (e.Row != null && e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls())
                    {
                        var button = cell.Controls[0] as LinkButton;
                        if (button != null)
                        {
                            // Is this cell the one that we are sorting
                            if (this.SortExpr == button.CommandArgument)
                            {
                                var image = new Image();

                                // Use sort direction to determine image to display
                                if (this.SortDir == "ASC")
                                {
                                    image.ImageUrl = "~/images/sortascending.gif";
                                }
                                else
                                {
                                    image.ImageUrl = "~/images/sortdescending.gif";
                                }

                                cell.Controls.AddAt(0, image);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region  Private Methods

        private void ConfigureGridFromSettings()
        {
            // Load Paging and Sorting data from Visualizer Settings
            this.grdResults.AllowPaging =
                Convert.ToBoolean(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                    ReportsConstants.SETTING_Grid_EnablePaging,
                                                                    false));
            this.grdResults.AllowSorting =
                Convert.ToBoolean(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                    ReportsConstants.SETTING_Grid_EnableSorting,
                                                                    false));
            this.grdResults.PageSize =
                Convert.ToInt32(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                  ReportsConstants.SETTING_Grid_PageSize, 10));
            this.grdResults.ShowHeader =
                Convert.ToBoolean(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                    ReportsConstants.SETTING_Grid_ShowHeader, true));
            this.grdResults.CssClass +=
                Convert.ToString(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                   ReportsConstants.SETTING_Grid_CSSClass, ""));

            var styleString =
                Convert.ToString(SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                                   ReportsConstants.SETTING_Grid_AdditionalCSS, ""));
            foreach (var styleEntry in styleString.Split(';'))
            {
                var styleArray = styleEntry.Split(':');
                if (styleArray.Length != 2)
                {
                    continue;
                }
                this.grdResults.Style.Add(styleArray[0], styleArray[1]);
            }

            this.grdResults.GridLines = Utilities.GetGridLinesSetting(this.Report.VisualizerSettings);
        }

        // Generates columns on the GridView for the columns of the DataTable
        private void GenerateColumns(DataTable dataTable)
        {
            this.grdResults.Columns.Clear();
            foreach (DataColumn column in dataTable.Columns)
            {
                var field = new BoundField();
                field.DataField = column.ColumnName;
                field.SortExpression = column.ColumnName;
                field.HtmlEncode = false;
                field.HeaderText = column.ColumnName;
                this.grdResults.Columns.Add(field);
            }
        }

        #endregion
    }
}