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
    using System.Collections.Generic;
    using System.Web.UI;
    using DotNetNuke.Modules.Reports.Extensions;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Settings class manages Grid Visualizer Settings
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Settings : ReportsSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ReportsClientAPI.IsSupported)
            {
                this.chkPageData.AutoPostBack = true;
                this.chkPageData.CheckedChanged += this.chkPageData_CheckedChanged;
            }
            else
            {
                ReportsClientAPI.Import(this.Page);
                ReportsClientAPI.ShowHideByCheckBox(this.Page, this.chkPageData, this.rowPageSize);
            }
        }

        public override void LoadSettings(Dictionary<string, string> VisualizerSettings)
        {
            this.chkPageData.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_EnablePaging,
                                                      false));
            this.chkSortData.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_EnableSorting,
                                                      false));
            this.txtPageSize.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_PageSize,
                                                      "10"));
            this.chkShowHeader.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_ShowHeader,
                                                      true));
            this.txtAdditionalCSS.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_AdditionalCSS,
                                                      ""));
            this.txtCSSClass.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsController.SETTING_Grid_CSSClass, ""));
            var gridLines = Convert.ToString(Utilities.GetGridLinesSetting(VisualizerSettings).ToString());
            DropDownUtils.TrySetValue(this.ddlGridLines, Convert.ToString(
                                          SettingsUtil.GetDictionarySetting(
                                              VisualizerSettings,
                                              ReportsController.SETTING_Grid_GridLines,
                                              ReportsController.DEFAULT_Grid_GridLines)),
                                      ReportsController.DEFAULT_Grid_GridLines);
            this.UpdatePageSizeText();
        }

        public override void SaveSettings(Dictionary<string, string> VisualizerSettings)
        {
            if (!this.typvalPageSize.IsValid)
            {
                this.txtPageSize.Text = "10";
            }

            VisualizerSettings[ReportsController.SETTING_Grid_EnablePaging] = this.chkPageData.Checked.ToString();
            VisualizerSettings[ReportsController.SETTING_Grid_EnableSorting] = this.chkSortData.Checked.ToString();
            VisualizerSettings[ReportsController.SETTING_Grid_PageSize] = this.txtPageSize.Text;
            VisualizerSettings[ReportsController.SETTING_Grid_ShowHeader] = this.chkShowHeader.Checked.ToString();
            VisualizerSettings[ReportsController.SETTING_Grid_GridLines] = this.ddlGridLines.SelectedValue;
            VisualizerSettings[ReportsController.SETTING_Grid_AdditionalCSS] = this.txtAdditionalCSS.Text;
            VisualizerSettings[ReportsController.SETTING_Grid_CSSClass] = this.txtCSSClass.Text;
        }

        private void UpdatePageSizeText()
        {
            if (this.chkPageData.Checked)
            {
                this.rowPageSize.Style[HtmlTextWriterStyle.Display] = string.Empty;
            }
            else
            {
                this.rowPageSize.Style[HtmlTextWriterStyle.Display] = "none";
            }
        }

        private void chkPageData_CheckedChanged(object sender, EventArgs args)
        {
            this.UpdatePageSizeText();
        }
    }
}