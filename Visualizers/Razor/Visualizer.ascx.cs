using System;
using System.Web;
using DotNetNuke.Web.Razor;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;


namespace DotNetNuke.Modules.Reports.Visualizers.Razor
{
	public partial class Visualizer : DotNetNuke.Modules.Reports.Visualizers.VisualizerControlBase
	{
		
		private class RazorReportHost : RazorModuleBase
		{
			
			protected override string RazorScriptFile
			{
				get
				{
					string script = this.ModuleContext.Settings["ScriptFile"] as string;
					if (!string.IsNullOrEmpty(script))
					{
						return string.Format("~/DesktopModules/RazorModules/RazorHost/Scripts/{0}", script);
					}
					return string.Empty;
				}
			}
			
		}
		
		private RazorReportHost RazorHost;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			
			ParentModule.Actions.Add(this.ParentModule.ModuleContext.GetNextActionID(), DotNetNuke.Services.Localization.Localization.GetString("EditContent.Action", this.LocalResourceFile), 
				"AddContent.Action", "", "edit.gif", this.ParentModule.EditUrl("EditRazorScript"), false, SecurityAccessLevel.Host, true, false);
			RazorHost = new RazorReportHost();
			this.Controls.Add(RazorHost);
			RazorHost.ModuleContext.Configuration = this.ParentModule.ModuleConfiguration;
			DataBind();
		}
		
		public override void DataBind()
		{
			if (this.ValidateDataSource() && this.ValidateResults())
			{
				HttpContext.Current.Items[ModuleController.CacheKey(TabModuleId) + "_razor"] = ReportResults;
			}
		}
	}
}
