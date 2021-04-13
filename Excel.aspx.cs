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

using System;
using System.Data;
using System.Web.UI;

namespace DotNetNuke.Modules.Reports
{
	public partial class Excel : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["report_export"] != null)
			{
                grd.DataSource = (DataTable) Session["report_export"];
                grd.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";

                Response.AddHeader("Content-Disposition", "attachment;filename=Export.xls");
                EnableViewState = false;

				System.IO.StringWriter sw = new System.IO.StringWriter();
				HtmlTextWriter hw = new HtmlTextWriter(sw);

                grd.RenderControl(hw);
                Response.Write(sw.ToString());
                Response.End();
			}
		}

		public override void VerifyRenderingInServerForm(Control control)
		{
			/* Confirms that an HtmlForm control is rendered for the specified ASP.NET
		  server control at run time. */
		}
	}
}