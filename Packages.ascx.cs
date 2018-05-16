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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Components;
    using DNNtc;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Installer;
    using DotNetNuke.Services.Installer.Packages;
    using DotNetNuke.Services.Localization;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Packages class manages Module AddIns
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    [ModuleControlProperties("ManagePackages", "Add/Remove Extensions", ControlType.Edit, "", false, false)]
    public partial class Packages : PortalModuleBase
    {
        private static readonly Version BuiltInVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public void Page_Load(object sender, EventArgs args)
        {
            this.InstallVisualizerLink.NavigateUrl =
                Util.InstallURL(this.TabId, ReportsConstants.PACKAGETYPE_Visualizer);
            this.InstallDataSourceLink.NavigateUrl =
                Util.InstallURL(this.TabId, ReportsConstants.PACKAGETYPE_DataSource);

            Localization.LocalizeGridView(ref this.PackagesGrid, this.LocalResourceFile);

            if (!this.IsPostBack)
            {
                this.DataBind();
            }

            Localization.LocalizeGridView(ref this.PackagesGrid, this.LocalResourceFile);
        }

        public override void DataBind()
        {
            base.DataBind();

            var visualizers = PackageController
                .Instance.GetExtensionPackages(Null.NullInteger,
                                               arg => arg.PackageType == ReportsConstants.PACKAGETYPE_Visualizer)
                .ToList();
            var dataSources = PackageController
                .Instance.GetExtensionPackages(Null.NullInteger,
                                               arg => arg.PackageType == ReportsConstants.PACKAGETYPE_DataSource)
                .ToList();

            var allExtensions = new List<PackageInfo>(visualizers);
            allExtensions.AddRange(dataSources);

            // Add Built-in packages
            allExtensions.Add(this.CreateBuiltInPackage("Grid", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_Visualizer, "Grid"));
            allExtensions.Add(this.CreateBuiltInPackage("HTML", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_Visualizer, "HTML"));
            allExtensions.Add(this.CreateBuiltInPackage("XSLT", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_Visualizer, "XSLT"));
            allExtensions.Add(this.CreateBuiltInPackage("Razor", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_Visualizer, "Razor"));
            allExtensions.Add(this.CreateBuiltInPackage("Generic ADO.Net Provider", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_DataSource, "ADO"));
            allExtensions.Add(this.CreateBuiltInPackage("DotNetNuke", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_DataSource, "DNN"));
            allExtensions.Add(this.CreateBuiltInPackage("Microsoft SQL Server", BuiltInVersion,
                                                        ReportsConstants.PACKAGETYPE_DataSource, "SqlServer"));

            this.PackagesGrid.DataSource = allExtensions;
            this.PackagesGrid.DataBind();
        }

        private PackageInfo CreateBuiltInPackage(string Name, Version Version, string Type, string DescriptionKey)
        {
            var pkg = new PackageInfo();
            pkg.PackageID = -1;
            pkg.FriendlyName = Name;
            pkg.Version = Version;
            pkg.PackageType = Type;
            pkg.Description =
                Localization.GetString(string.Concat(DescriptionKey, ".Description"), this.LocalResourceFile);
            return pkg;
        }

        protected string GetImage(int id)
        {
            if (id < 0)
            {
                return this.ResolveUrl("images/BuiltInPackage.gif");
            }
            return this.ResolveUrl("~/images/delete.gif");
        }

        protected bool IsBuiltIn(int id)
        {
            return id < 0;
        }

        protected string StripPrefix(string value)
        {
            return value.Substring(ReportsConstants.PACKAGETYPE_Prefix.Length);
        }

        protected string InstallUrl(int id)
        {
            return Util.UnInstallURL(this.TabId, id);
        }
    }
}