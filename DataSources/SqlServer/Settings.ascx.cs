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


namespace DotNetNuke.Modules.Reports.DataSources.SqlServer
{
    using System;
    using System.Collections.Generic;
    using global::DotNetNuke.Modules.Reports.Extensions;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Settings class manages settings for the Sql Data Source
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Settings : ReportsSettingsBase, IDataSourceSettingsControl
    {
        public override void LoadSettings(Dictionary<string, string> Settings)
        {
            var useConnectionString =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_UseConnectionString, false));
            this.AutomaticConnStrRadio.Checked = !useConnectionString;
            this.ManualConnStrRadio.Checked = useConnectionString;
            this.ConnStrTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_ConnectionString,
                                                      string.Empty));
            this.ServerTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_Server, string.Empty));
            this.DatabaseTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_Database, string.Empty));
            this.IntegratedSecurityCheckBox.Checked =
                Convert.ToBoolean(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_Sql_UseIntegratedSecurity,
                                                      false));
            if (!this.IntegratedSecurityCheckBox.Checked)
            {
                this.UserNameTextBox.Text =
                    Convert.ToString(
                        SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_UserName, string.Empty));
                this.PasswordTextBox.Text =
                    Convert.ToString(
                        SettingsUtil.GetDictionarySetting(Settings, ReportsController.SETTING_Password, string.Empty));
            }
            this.SqlDataSourceCommonSettingsControl.LoadSettings(Settings);

            this.UpdateControlVisibility();
        }

        public override void SaveSettings(Dictionary<string, string> Settings)
        {
            Settings.Clear();
            if (this.AutomaticConnStrRadio.Checked)
            {
                Settings.Add(ReportsController.SETTING_UseConnectionString, "False");
                Settings.Add(ReportsController.SETTING_Server, this.ServerTextBox.Text);
                Settings.Add(ReportsController.SETTING_Database, this.DatabaseTextBox.Text);
                Settings.Add(ReportsController.SETTING_Sql_UseIntegratedSecurity,
                             this.IntegratedSecurityCheckBox.Checked.ToString());
                if (!this.IntegratedSecurityCheckBox.Checked)
                {
                    Settings.Add(ReportsController.SETTING_UserName, this.UserNameTextBox.Text);
                    Settings.Add(ReportsController.SETTING_Password, this.PasswordTextBox.Text);
                }
            }
            else if (this.ManualConnStrRadio.Checked)
            {
                Settings.Add(ReportsController.SETTING_UseConnectionString, "True");
                Settings.Add(ReportsController.SETTING_ConnectionString, this.ConnStrTextBox.Text);
            }
            this.SqlDataSourceCommonSettingsControl.SaveSettings(Settings);
        }

        public string DataSourceClass => typeof(SqlServerDataSource).FullName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ReportsClientAPI.IsSupported)
            {
                this.AutomaticConnStrRadio.AutoPostBack = true;
                this.ManualConnStrRadio.AutoPostBack = true;
                this.IntegratedSecurityCheckBox.AutoPostBack = true;

                this.AutomaticConnStrRadio.CheckedChanged += this.ConnStrModeCheckedChanged;
                this.ManualConnStrRadio.CheckedChanged += this.ConnStrModeCheckedChanged;
                this.IntegratedSecurityCheckBox.CheckedChanged += this.IntegratedSecurityCheckBox_CheckedChanged;
            }
            else
            {
                ReportsClientAPI.Import(this.Page);
                ReportsClientAPI.ShowHideByRadioButtons(this.Page, this.AutomaticConnStrRadio, this.ManualConnStrRadio,
                                                        this.AutomaticConnStrConfig, this.ManualConnStrConfig);
                ReportsClientAPI.ShowHideByCheckBox(this.Page, this.IntegratedSecurityCheckBox, this.UserNameRow,
                                                    false);
                ReportsClientAPI.ShowHideByCheckBox(this.Page, this.IntegratedSecurityCheckBox, this.PasswordRow,
                                                    false);

                // Show the correct table
                this.UpdateControlVisibility();
            }
        }

        private void ConnStrModeCheckedChanged(object sender, EventArgs args)
        {
            this.UpdateControlVisibility();
        }

        private void IntegratedSecurityCheckBox_CheckedChanged(object sender, EventArgs args)
        {
            this.UpdateControlVisibility();
        }

        private void UpdateControlVisibility()
        {
            if (this.AutomaticConnStrRadio.Checked)
            {
                ReportsClientAPI.ClientShow(this.AutomaticConnStrConfig);
                ReportsClientAPI.ClientHide(this.ManualConnStrConfig);
            }
            else if (this.ManualConnStrRadio.Checked)
            {
                ReportsClientAPI.ClientShow(this.ManualConnStrConfig);
                ReportsClientAPI.ClientHide(this.AutomaticConnStrConfig);
            }

            if (this.IntegratedSecurityCheckBox.Checked)
            {
                ReportsClientAPI.ClientHide(this.UserNameRow);
                ReportsClientAPI.ClientHide(this.PasswordRow);
            }
            else
            {
                ReportsClientAPI.ClientShow(this.UserNameRow);
                ReportsClientAPI.ClientShow(this.PasswordRow);
            }
        }
    }
}