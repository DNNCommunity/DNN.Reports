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


namespace DotNetNuke.Modules.Reports.DataSources.ADO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Web.UI.WebControls;
    using Components;
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
            this.ConnStrTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsConstants.SETTING_ConnectionString,
                                                      string.Empty));
            this.ParamPrefixTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsConstants.SETTING_ADO_ParamPrefix,
                                                      string.Empty));

            var provider =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsConstants.SETTING_ADO_ProviderName,
                                                      string.Empty));
            this.EnsureProviderDropDown();
            if (this.ProviderNameDropDown.Items.FindByValue(provider) != null)
            {
                this.ProviderNameDropDown.SelectedValue = provider;
            }

            this.SqlDataSourceCommonSettingsControl.LoadSettings(Settings);
        }

        public override void SaveSettings(Dictionary<string, string> Settings)
        {
            Settings.Clear();
            Settings.Add(ReportsConstants.SETTING_ConnectionString, this.ConnStrTextBox.Text);
            Settings.Add(ReportsConstants.SETTING_ADO_ProviderName, this.ProviderNameDropDown.SelectedValue);
            Settings.Add(ReportsConstants.SETTING_ADO_ParamPrefix, this.ParamPrefixTextBox.Text);
            this.SqlDataSourceCommonSettingsControl.SaveSettings(Settings);
        }

        public string DataSourceClass => typeof(GenericADODataSource).FullName;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.EnsureProviderDropDown();
        }

        private void EnsureProviderDropDown()
        {
            if (this.ProviderNameDropDown.Items.Count == 0)
            {
                this.LoadProviderDropDown();
            }
        }

        private void LoadProviderDropDown()
        {
            var providers = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow providerRow in providers.Rows)
            {
                this.ProviderNameDropDown.Items.Add(
                    new ListItem(Convert.ToString(providerRow["Name"].ToString()),
                                 Convert.ToString(providerRow["InvariantName"].ToString())));
            }
        }
    }
}