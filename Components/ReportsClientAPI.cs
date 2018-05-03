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


namespace DotNetNuke.Modules.Reports
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using DotNetNuke.UI.Utilities;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Manages the reports Client API Calls
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	08/24/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ReportsClientAPI
    {
        public const string ReportsScript = "~/DesktopModules/Reports/js/dnn.reports.js";

        public static bool IsSupported => ClientAPI.BrowserSupportsFunctionality(ClientAPI.ClientFunctionality.DHTML);

        public static void Import(Page page)
        {
            ClientAPI.RegisterClientReference(page, ClientAPI.ClientNamespaceReferences.dnn);
            ClientAPI.RegisterClientReference(page, ClientAPI.ClientNamespaceReferences.dnn_dom);

            //If IsSupported Then
            //    MSAJAX.RegisterClientScript(page, "dnn.reports.js", GetType(ReportsClientAPI).Assembly.FullName)
            //End If
            if (!page.ClientScript.IsClientScriptIncludeRegistered("dnn.reports"))
            {
                page.ClientScript.RegisterClientScriptInclude("dnn.reports",
                                                              page.ClientScript.GetWebResourceUrl(
                                                                  typeof(ReportsClientAPI),
                                                                  "dnn.reports.js"));
            }
        }

        public static void ShowHideByCheckBox(Page page, Control checkBox, Control target)
        {
            ShowHideByCheckBox(page, checkBox, target, true);
        }

        public static void ShowHideByCheckBox(Page page, Control checkBox, Control target, bool showWhenChecked)
        {
            var scpt = string.Format("dnn_reports_showHideByCheckbox('{0}', '{1}', {2}); ",
                                     checkBox.ClientID,
                                     target.ClientID,
                                     showWhenChecked.ToString().ToLower());
            ApplyScript((IAttributeAccessor) checkBox, "onclick", scpt);

            // Register the script on start as well to ensure the correct state is displayed
            page.ClientScript.RegisterStartupScript(typeof(ReportsClientAPI),
                                                    string.Concat("shcb",
                                                                  checkBox.ClientID,
                                                                  target.ClientID),
                                                    scpt,
                                                    true);
        }

        public static void RegisterBarColorModeSwitching(RadioButton radOneColor, RadioButton radColorPerBar,
                                                         HtmlTableRow rowOneColor, HtmlTableRow rowColorPerBar)
        {
            var updateScript = string.Format("dnn_reports_updateBarColorMode('{0}', '{1}', '{2}', '{3}');",
                                             radOneColor.ClientID,
                                             radColorPerBar.ClientID, rowOneColor.ClientID, rowColorPerBar.ClientID);
            ApplyScript(radOneColor, "onclick", updateScript);
            ApplyScript(radColorPerBar, "onclick", updateScript);
        }

        public static void RegisterColorPreview(Control txtColor, Control colorPreview)
        {
            ApplyScript((IAttributeAccessor) txtColor, "onchange",
                        string.Format("dnn_reports_updateColorPreview('{0}', '{1}')", txtColor.ClientID,
                                      colorPreview.ClientID));
        }

        public static void RegisterReportingServicesModeUpdate(RadioButton radLocal, RadioButton radServer,
                                                               Control rowFile, Control rowDataSource,
                                                               Control rowServerUrl, Control rowServerReportPath)
        {
            var sScript = string.Format("dnn_reports_updateRSMode('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                                        radLocal.ClientID,
                                        radServer.ClientID, rowFile.ClientID, rowDataSource.ClientID,
                                        rowServerUrl.ClientID, rowServerReportPath.ClientID);
            ApplyScript(radLocal, "onclick", sScript);
            ApplyScript(radServer, "onclick", sScript);
        }

        public static void ShowHideByRadioButtons(Page page, RadioButton radioButtonA, RadioButton radioButtonB,
                                                  Control ctrlA, Control ctrlB)
        {
            var scpt = string.Format("dnn_reports_showHideByRadioButtons('{0}', '{1}', '{2}', '{3}'); ",
                                     radioButtonA.ClientID,
                                     radioButtonB.ClientID,
                                     ctrlA.ClientID,
                                     ctrlB.ClientID);
            ApplyScript(radioButtonA, "onclick", scpt);
            ApplyScript(radioButtonB, "onclick", scpt);

            // Register the script on start as well to ensure the correct state is displayed
            page.ClientScript.RegisterStartupScript(typeof(ReportsClientAPI),
                                                    string.Concat("shrb",
                                                                  radioButtonA.ClientID,
                                                                  radioButtonB.ClientID,
                                                                  ctrlA.ClientID,
                                                                  ctrlB.ClientID),
                                                    scpt,
                                                    true);
        }

        /// <summary>
        ///     Hides the specified control on the client-side, while still rendering
        ///     it to HTML
        /// </summary>
        /// <param name="TargetControl">The control to hide</param>
        public static void ClientHide(Control TargetControl)
        {
            SetCssStyle(TargetControl, HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        ///     Shows a control hidden with <see cref="ClientHide" />
        /// </summary>
        /// <param name="TargetControl">The control to show</param>
        public static void ClientShow(Control TargetControl)
        {
            SetCssStyle(TargetControl, HtmlTextWriterStyle.Display, "");
        }

        private static void SetCssStyle(Control TargetControl, HtmlTextWriterStyle Field, string Value)
        {
            var webCtrl = TargetControl as WebControl;
            if (webCtrl != null)
            {
                webCtrl.Style[Field] = Value;
                return;
            }

            var htmlCtrl = TargetControl as HtmlControl;
            if (htmlCtrl != null)
            {
                htmlCtrl.Style[Field] = Value;
                return;
            }

            throw new ArgumentException("Control must be either a WebControl or an HtmlControl", "Control");
        }

        public static void ApplyScript(IAttributeAccessor control, string attr, string script)
        {
            var sCurValue = control.GetAttribute(attr);
            var sNewValue = string.Empty;
            if (!string.IsNullOrEmpty(sCurValue))
            {
                if (!sCurValue.EndsWith(";"))
                {
                    sNewValue = string.Format("{0}; {1}", sCurValue, script);
                }
                else
                {
                    sNewValue = string.Concat(sCurValue, script);
                }
            }
            else
            {
                sNewValue = script;
            }
            control.SetAttribute(attr, sNewValue);
        }
    }
}