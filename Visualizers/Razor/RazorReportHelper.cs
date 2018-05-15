using System.Data;
using System.Web;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Web.Razor.Helpers;


namespace DotNetNuke.Modules.Reports.Visualizers.Razor
{
	public static class RazorReportHelper
	{
		
		public static DataTable ReportResults(this DnnHelper Helper)
		{
			return ((DataTable) (HttpContext.Current.Items[ModuleController.CacheKey(Helper.Module.TabModuleID) + "_razor"]));
		}
	}
}

