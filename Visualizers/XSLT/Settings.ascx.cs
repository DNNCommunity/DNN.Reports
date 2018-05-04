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


namespace DotNetNuke.Modules.Reports.Visualizers.Xslt
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Components;
    using DotNetNuke.Modules.Reports.Extensions;
    using DotNetNuke.Services.Localization;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Settings class manages XSLT Transform Visualizer Settings
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Settings : ReportsSettingsBase
    {
        private IList<ExtensionObjectInfo> StoredExtensionObjects
        {
            get
                {
                    // Can't store a list in ViewState... have to convert to an array for now
                    var objArray = this.ViewState["StoredExtensionObjects"] as ExtensionObjectInfo[];
                    if (ReferenceEquals(objArray, null))
                    {
                        return new List<ExtensionObjectInfo>();
                    }
                    return new List<ExtensionObjectInfo>(objArray);
                }
            set
                {
                    if (value.Count == 0)
                    {
                        this.ViewState["StoredExtensionObjects"] = null;
                    }
                    else
                    {
                        var arr = new ExtensionObjectInfo[value.Count];
                        value.CopyTo(arr, 0);
                        this.ViewState["StoredExtensionObjects"] = arr;
                    }
                }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();

            if (this.ParentModule.UserInfo.IsSuperUser)
            {
                this.mvExtensionObjects.SetActiveView(this.vwAllowed);
            }
            else
            {
                this.mvExtensionObjects.SetActiveView(this.vwNotAllowed);
            }
        }

        public override void LoadSettings(Dictionary<string, string> VisualizerSettings)
        {
            this.ctlTransform.Url =
                Convert.ToString(SettingsUtil.GetDictionarySetting(VisualizerSettings,
                                                                   ReportsConstants.SETTING_Xslt_TransformFile,
                                                                   string.Empty));
            this.ctlTransform.DataBind();

            if (this.ParentModule.UserInfo.IsSuperUser)
            {
                this.StoredExtensionObjects = ReportsController.GetXsltExtensionObjects(this.TabModuleId);
            }
        }

        public override void SaveSettings(Dictionary<string, string> VisualizerSettings)
        {
            VisualizerSettings[ReportsConstants.SETTING_Xslt_TransformFile] = this.ctlTransform.Url;

            if (this.StoredExtensionObjects != null && this.ParentModule.UserInfo.IsSuperUser)
            {
                ReportsController.SetXsltExtensionObjects(this.TabModuleId, this.StoredExtensionObjects);
            }
        }

        public override void DataBind()
        {
            if (!this.IsPostBack)
            {
                foreach (DataControlField col in this.grdExtensionObjects.Columns)
                {
                    if (col is BoundField)
                    {
                        col.HeaderText = Localization.GetString(col.HeaderText, this.LocalResourceFile);
                    }
                }
            }

            this.grdExtensionObjects.DataSource = this.StoredExtensionObjects;
            this.grdExtensionObjects.DataBind();
        }

        protected void grdExtensionObjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var list = this.StoredExtensionObjects;
            list.RemoveAt(e.RowIndex);
            this.StoredExtensionObjects = list;
            this.DataBind();
        }

        protected void btnAddExtensionObject_Click(object sender, EventArgs e)
        {
            var newObj = new ExtensionObjectInfo
                             {
                                 XmlNamespace = this.txtXmlns.Text,
                                 ClrType = this.txtClrType.Text
                             };
            var list = this.StoredExtensionObjects;
            list.Add(newObj);
            this.StoredExtensionObjects = list;
            this.DataBind();
        }
    }
}