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


namespace DotNetNuke.Modules.Reports.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Components;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Skins;
    using DotNetNuke.UI.Skins.Controls;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The SqlDataSourceCommonSettingsControl class manages common settings for
    ///     Sql-based Data Sources
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class SqlDataSourceCommonSettingsControl : SettingsFormControl
    {
        public override string LocalResourceFile => this.ResolveUrl(
            "App_LocalResources/SqlDataSourceCommonSettingsControl.ascx");

        private PortalModuleBase ParentModule
        {
            get
                {
                    var current = this.Parent;
                    while (current != null && !(current is PortalModuleBase))
                    {
                        current = current.Parent;
                    }
                    return current as PortalModuleBase;
                }
        }

        protected override void OnLoad(EventArgs e)
        {
            AJAX.RegisterPostBackControl(this.QueryUploadButton);
            base.OnLoad(e);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ReferenceEquals(this.QueryUploadControl.PostedFile, null))
            {
                Skin.AddModuleMessage(this.ParentModule, Localization.GetString("UnableToUploadFile.Text"),
                                      ModuleMessage.ModuleMessageType.RedError);
                return;
            }
            using (var reader = new StreamReader(this.QueryUploadControl.PostedFile.InputStream))
            {
                this.QueryTextBox.Text = reader.ReadToEnd();
            }
        }

        public override void LoadSettings(Dictionary<string, string> Settings)
        {
            this.QueryTextBox.Text =
                Convert.ToString(
                    SettingsUtil.GetDictionarySetting(Settings, ReportsConstants.SETTING_Query, Null.NullString));
        }

        public override void SaveSettings(Dictionary<string, string> Settings)
        {
            Settings[ReportsConstants.SETTING_Query] = this.QueryTextBox.Text;
        }
    }
}