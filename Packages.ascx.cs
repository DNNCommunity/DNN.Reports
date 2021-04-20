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
    using DotNetNuke.Common;
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
    [DNNtc.ModuleControlProperties("ManagePackages", "Add/Remove Extensions", DNNtc.ControlType.Edit, "https://github.com/DNNCommunity/DNN.Reports/wiki", true, true)]
    public partial class Packages : PortalModuleBase
    {
        private static readonly Version BuiltInVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public void Page_Load(object sender, EventArgs args)
        {
            // Install plug-in packages does not work with the PB
            // InstallVisualizerLink.NavigateUrl = Util.InstallURL(TabId, ReportsConstants.PACKAGETYPE_Visualizer);
            // InstallDataSourceLink.NavigateUrl = Util.InstallURL(TabId, ReportsConstants.PACKAGETYPE_DataSource);

            Localization.LocalizeGridView(ref PackagesGrid, LocalResourceFile);

            if (!IsPostBack)
            {
                DataBind();
            }

            Localization.LocalizeGridView(ref PackagesGrid, LocalResourceFile);
        }

        public override void DataBind()
        {
            base.DataBind();

            var visualizers = PackageController
                .Instance.GetExtensionPackages(Null.NullInteger, arg => arg.PackageType == ReportsConstants.PACKAGETYPE_Visualizer)
                .ToList();
            var dataSources = PackageController
                .Instance.GetExtensionPackages(Null.NullInteger, arg => arg.PackageType == ReportsConstants.PACKAGETYPE_DataSource)
                .ToList();

            var allExtensions = new List<PackageInfo>(visualizers);
            allExtensions.AddRange(dataSources);

            // Add Built-in packages
            allExtensions.Add(CreateBuiltInPackage("Grid", BuiltInVersion, ReportsConstants.PACKAGETYPE_Visualizer, "Grid"));
            allExtensions.Add(CreateBuiltInPackage("HTML", BuiltInVersion, ReportsConstants.PACKAGETYPE_Visualizer, "HTML"));
            allExtensions.Add(CreateBuiltInPackage("XSLT", BuiltInVersion, ReportsConstants.PACKAGETYPE_Visualizer, "XSLT"));
            allExtensions.Add(CreateBuiltInPackage("Razor", BuiltInVersion, ReportsConstants.PACKAGETYPE_Visualizer, "Razor"));
            allExtensions.Add(CreateBuiltInPackage("Generic ADO.Net Provider", BuiltInVersion, ReportsConstants.PACKAGETYPE_DataSource, "ADO"));
            allExtensions.Add(CreateBuiltInPackage("DotNetNuke", BuiltInVersion, ReportsConstants.PACKAGETYPE_DataSource, "DNN"));
            allExtensions.Add(CreateBuiltInPackage("Microsoft SQL Server", BuiltInVersion, ReportsConstants.PACKAGETYPE_DataSource, "SqlServer"));

            PackagesGrid.DataSource = allExtensions;
            PackagesGrid.DataBind();
        }

        private PackageInfo CreateBuiltInPackage(string Name, Version Version, string Type, string DescriptionKey)
        {
            var pkg = new PackageInfo();
            pkg.PackageID = -1;
            pkg.FriendlyName = Name;
            pkg.Version = Version;
            pkg.PackageType = Type;
            pkg.Description =
                Localization.GetString(string.Concat(DescriptionKey, ".Description"), LocalResourceFile);
            return pkg;
        }

        protected string GetImage(int id)
        {
            if (id < 0)
            {
                return ResolveUrl("images/BuiltInPackage.gif");
            }
            return ResolveUrl("~/images/delete.gif");
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
            return Util.UnInstallURL(TabId, id);
        }

        protected void returnButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), false);
        }

    }
}