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
    using System.IO;
    using System.Text;
    using System.Web.Compilation;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;
    using Components;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Visualizer class displays the XSLT Transform
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public partial class Visualizer : VisualizerControlBase
    {
        #region  Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsFirstRun)
            {
                this.DataBind();
            }
        }

        #endregion

        #region  Overrides

        public override void DataBind()
        {
            // Get the report for this module
            if (!this.ValidateDataSource() || !this.ValidateResults())
            {
                this.litContent.Visible = false;
            }
            else
            {
                this.litContent.Visible = true;

                // Get the extension objects
                IEnumerable<ExtensionObjectInfo> extensionObjects =
                    ReportsController.GetXsltExtensionObjects(this.TabModuleId);
                var argList = new XsltArgumentList();
                foreach (var extensionObject in extensionObjects)
                {
                    object obj = this.CreateExtensionObject(extensionObject.ClrType);
                    if (obj != null)
                    {
                        argList.AddExtensionObject(extensionObject.XmlNamespace, obj);
                    }
                }

                // Get the Xslt Url
                var sXsl = SettingsUtil.GetDictionarySetting(this.Report.VisualizerSettings,
                                                             ReportsConstants.SETTING_Xslt_TransformFile,
                                                             string.Empty);
                if (string.IsNullOrEmpty(sXsl))
                {
                    return;
                }
                if (sXsl.ToLower().StartsWith("fileid="))
                {
                    sXsl = Utilities.MapFileIdPath(this.ParentModule.PortalSettings, sXsl);
                }
                else
                {
                    sXsl = Path.Combine(this.ParentModule.PortalSettings.HomeDirectoryMapPath, sXsl.Replace("/", "\\"));
                }
                if (string.IsNullOrEmpty(sXsl))
                {
                    return;
                }

                // Serialize the results to Xml
                var sbSource = new StringBuilder();
                using (var srcWriter = new StringWriter(sbSource))
                {
                    this.ReportResults.WriteXml(srcWriter);
                }


                // Load the Transform and transform the Xml
                var sbDest = new StringBuilder();
                var xform = new XslCompiledTransform();
                using (var destWriter = new XmlTextWriter(new StringWriter(sbDest)))
                {
                    xform.Load(sXsl);
                    xform.Transform(new XPathDocument(new StringReader(sbSource.ToString())), argList, destWriter);
                }


                var objSec = new PortalSecurity();
                this.litContent.Text = objSec.InputFilter(sbDest.ToString(), PortalSecurity.FilterFlag.NoScripting);
            }
            base.DataBind();
        }

        private dynamic CreateExtensionObject(string typeName)
        {
            // Get the type from the build manager
            var type = BuildManager.GetType(typeName, false);
            if (ReferenceEquals(type, null))
            {
                return null;
            }

            // Construct an instance
            object instance = null;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return null;
            }

            // Check for the IXsltExtensionObject interface
            var extObj = instance as IXsltExtensionObject;
            if (extObj != null)
            {
                extObj.ParentModule = this.ParentModule;
                extObj.Report = this.Report;
                extObj.ReportResults = this.ReportResults;
            }

            return instance;
        }

        #endregion
    }
}