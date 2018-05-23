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


namespace DotNetNuke.Modules.Reports.Extensions
{
    using System.Web;
    using DotNetNuke.Services.Localization;

    /// <summary>
    ///     Contains complete path context for a Reports Module Extension
    /// </summary>
    public class ExtensionContext
    {
        #region  Private Fields

        #endregion

        #region  Public Properties

        /// <summary>
        ///     Gets the type of extension that this context was created for
        /// </summary>
        public string ExtensionType { get; }

        /// <summary>
        ///     Gets the web path to the module's root folder
        /// </summary>
        public string ModuleFolder { get; }

        /// <summary>
        ///     Gets the physical path to the module's root folder
        /// </summary>
        public string MappedModuleFolder
        {
            get
                {
                    if (ReferenceEquals(HttpContext.Current, null))
                    {
                        return string.Empty;
                    }

                    return HttpContext.Current.Server.MapPath(this.ModuleFolder);
                }
        }

        /// <summary>
        ///     Gets the web path to the module's App_LocalResources folder
        /// </summary>
        public string ModuleResourcesFolder { get; }

        /// <summary>
        ///     Gets the physical path to the module's App_LocalResources folder
        /// </summary>
        public string MappedModuleResourcesFolder
        {
            get
                {
                    if (ReferenceEquals(HttpContext.Current, null))
                    {
                        return string.Empty;
                    }

                    return HttpContext.Current.Server.MapPath(this.ModuleResourcesFolder);
                }
        }

        /// <summary>
        ///     Gets the web path to the folder containing all extensions of the same type
        ///     as the extension that this context was created for
        /// </summary>
        public string ExtensionsFolder { get; }

        /// <summary>
        ///     Gets the physical path to the folder containing all extensions of the same type
        ///     as the extension that this context was created for
        /// </summary>
        public string MappedExtensionsFolder
        {
            get
                {
                    if (ReferenceEquals(HttpContext.Current, null))
                    {
                        return string.Empty;
                    }

                    return HttpContext.Current.Server.MapPath(this.ExtensionsFolder);
                }
        }

        /// <summary>
        ///     Gets the web path to the folder containing the extension that this context was created for
        /// </summary>
        public string ExtensionFolder { get; }

        /// <summary>
        ///     Gets the physical path to the folder containing the extension that this context was created for
        /// </summary>
        public string MappedExtensionFolder
        {
            get
                {
                    if (ReferenceEquals(HttpContext.Current, null))
                    {
                        return string.Empty;
                    }

                    return HttpContext.Current.Server.MapPath(this.ExtensionFolder);
                }
        }

        /// <summary>
        ///     Gets the web path to the App_LocalResources folder for the extension that this context was created for
        /// </summary>
        public string ExtensionResourcesFolder { get; }

        /// <summary>
        ///     Gets the physical path to the App_LocalResources folder for the extension that this context was created for
        /// </summary>
        public string MappedExtensionResourcesFolder
        {
            get
                {
                    if (ReferenceEquals(HttpContext.Current, null))
                    {
                        return string.Empty;
                    }

                    return HttpContext.Current.Server.MapPath(this.ExtensionResourcesFolder);
                }
        }

        #endregion

        #region  Constructors

        public ExtensionContext(string moduleFolder, string extensionType, string extensionName) : this(
            moduleFolder, extensionType, string.Concat(extensionType, "s"), extensionName)
        { }

        public ExtensionContext(string moduleFolder, string extensionType, string extensionTypeFolder,
                                string extensionName) : this(extensionType, moduleFolder,
                                                             string.Format(
                                                                 "{0}/{1}", moduleFolder,
                                                                 Localization.LocalResourceDirectory),
                                                             string.Format(
                                                                 "{0}/{1}", moduleFolder, extensionTypeFolder),
                                                             string.Format(
                                                                 "{0}/{1}/{2}", moduleFolder, extensionTypeFolder,
                                                                 extensionName),
                                                             string.Format(
                                                                 "{0}/{1}/{2}/{3}", moduleFolder, extensionTypeFolder,
                                                                 extensionName, Localization.LocalResourceDirectory))
        { }

        public ExtensionContext(string extensionType, string moduleFolder, string moduleResourcesFolder,
                                string extensionsFolder, string extensionFolder, string extensionResourcesFolder)
        {
            this.ExtensionType = extensionType;
            this.ModuleFolder = moduleFolder;
            this.ModuleResourcesFolder = moduleResourcesFolder;
            this.ExtensionsFolder = extensionsFolder;
            this.ExtensionFolder = extensionFolder;
            this.ExtensionResourcesFolder = extensionResourcesFolder;
        }

        #endregion

        #region  Public Methods

        public string ResolveModulePath(string path)
        {
            return string.Format("{0}/{1}", this.ModuleFolder, path);
        }

        public string ResolveModuleResourcesPath(string path)
        {
            return string.Format("{0}/{1}", this.ModuleResourcesFolder, path);
        }

        public string ResolveExtensionsPath(string path)
        {
            return string.Format("{0}/{1}", this.ExtensionsFolder, path);
        }

        public string ResolveExtensionPath(string path)
        {
            return string.Format("{0}/{1}", this.ExtensionFolder, path);
        }

        public string ResolveExtensionResourcesPath(string path)
        {
            return string.Format("{0}/{1}", this.ExtensionResourcesFolder, path);
        }

        #endregion
    }
}