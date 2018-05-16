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


namespace DotNetNuke.Modules.Reports.Visualizers.Razor
{
    using System;
    using System.Threading;
    using System.Web;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Web.Razor;

    public class Visualizer : VisualizerControlBase
    {
        private RazorReportHost RazorHost;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ParentModule.Actions.Add(this.ParentModule.ModuleContext.GetNextActionID(),
                                          Localization.GetString("EditContent.Action", this.LocalResourceFile),
                                          "AddContent.Action", "", "edit.gif",
                                          this.ParentModule.EditUrl("EditRazorScript"), false, SecurityAccessLevel.Host,
                                          true, false);
            this.RazorHost = new RazorReportHost();
            this.Controls.Add(this.RazorHost);
            this.RazorHost.ModuleContext.Configuration = this.ParentModule.ModuleConfiguration;
            this.DataBind();
        }

        public override void DataBind()
        {
            if (this.ValidateDataSource() && this.ValidateResults())
            {
                var strCacheKey = "TabModule:" + this.TabModuleId + ":" + Thread.CurrentThread.CurrentUICulture;
                HttpContext.Current.Items[strCacheKey + "_razor"] = this.ReportResults;
            }
        }

        private class RazorReportHost : RazorModuleBase
        {
            protected override string RazorScriptFile
            {
                get
                    {
                        var script = this.ModuleContext.Settings["ScriptFile"] as string;
                        if (!string.IsNullOrEmpty(script))
                        {
                            return string.Format("~/DesktopModules/RazorModules/RazorHost/Scripts/{0}", script);
                        }
                        return string.Empty;
                    }
            }
        }
    }
}