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
    using Components;
    using Extensions;

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
                chkPageData.AutoPostBack = true;
                chkPageData.CheckedChanged += chkPageData_CheckedChanged;
            }
            else
            {
                ReportsClientAPI.Import(Page);
                ReportsClientAPI.ShowHideByCheckBox(Page, chkPageData, rowPageSize);
            }
        }

        public override void LoadSettings(Dictionary<string, string> VisualizerSettings)
        {
            chkPageData.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_EnablePaging,
                                                      false));
            chkSortData.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_EnableSorting,
                                                      false));
            txtPageSize.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_PageSize,
                                                      "10"));
            chkShowHeader.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_ShowHeader,
                                                      true));
            txtAdditionalCSS.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_AdditionalCSS,
                                                      ""));
            txtCSSClass.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(VisualizerSettings, ReportsConstants.SETTING_Grid_CSSClass, ""));
            var gridLines = Convert.ToString(Utilities.GetGridLinesSetting(VisualizerSettings).ToString());
            DropDownUtils.TrySetValue(ddlGridLines, Convert.ToString(
                                          SettingsUtil.GetDictionarySetting(
                                              VisualizerSettings,
                                              ReportsConstants.SETTING_Grid_GridLines,
                                              ReportsConstants.DEFAULT_Grid_GridLines)),
                                      ReportsConstants.DEFAULT_Grid_GridLines);
            UpdatePageSizeText();
        }

        public override void SaveSettings(Dictionary<string, string> VisualizerSettings)
        {
            if (!typvalPageSize.IsValid)
            {
                txtPageSize.Text = "10";
            }

            VisualizerSettings[ReportsConstants.SETTING_Grid_EnablePaging] = chkPageData.Checked.ToString();
            VisualizerSettings[ReportsConstants.SETTING_Grid_EnableSorting] = chkSortData.Checked.ToString();
            VisualizerSettings[ReportsConstants.SETTING_Grid_PageSize] = txtPageSize.Text;
            VisualizerSettings[ReportsConstants.SETTING_Grid_ShowHeader] = chkShowHeader.Checked.ToString();
            VisualizerSettings[ReportsConstants.SETTING_Grid_GridLines] = ddlGridLines.SelectedValue;
            VisualizerSettings[ReportsConstants.SETTING_Grid_AdditionalCSS] = txtAdditionalCSS.Text;
            VisualizerSettings[ReportsConstants.SETTING_Grid_CSSClass] = txtCSSClass.Text;
        }

        private void UpdatePageSizeText()
        {
            if (chkPageData.Checked)
            {
                rowPageSize.Style[HtmlTextWriterStyle.Display] = string.Empty;
            }
            else
            {
                rowPageSize.Style[HtmlTextWriterStyle.Display] = "none";
            }
        }

        private void chkPageData_CheckedChanged(object sender, EventArgs args)
        {
            UpdatePageSizeText();
        }
    }
}