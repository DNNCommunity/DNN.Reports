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
    using System.Diagnostics;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Localization;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Abstract base class for reports module "sub-controls" (Visualizer
    ///     Controls, etc.)
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	01/15/2007	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public abstract class ReportsControlBase : UserControlBase, IReportsControl
    {
        private string _localResourceFile;
        private PortalModuleBase _parentModule;

        public ExtensionContext ExtensionContext { get; private set; }

        protected internal int ModuleId
        {
            get
                {
                    Debug.Assert(this._parentModule != null, "Cannot access ModuleId if ParentModule is not set");
                    return this._parentModule.ModuleId;
                }
        }

        protected internal int TabModuleId
        {
            get
                {
                    Debug.Assert(this._parentModule != null, "Cannot access ModuleId if ParentModule is not set");
                    return this._parentModule.TabModuleId;
                }
        }

        public string LocalResourceFile
        {
            get
                {
                    if (string.IsNullOrEmpty(this._localResourceFile))
                    {
                        this._localResourceFile = this.TemplateSourceDirectory + "/" +
                                                  Localization.LocalResourceDirectory + "/" + this.ASCXFileName;
                    }
                    return this._localResourceFile;
                }
            set { this._localResourceFile = value; }
        }

        protected abstract string ASCXFileName { get; }

        public PortalModuleBase ParentModule
        {
            get
                {
                    if (ReferenceEquals(this._parentModule, null))
                    {
                        this.FindParentModule();
                    }
                    return this._parentModule;
                }
            set { this._parentModule = value; }
        }

        public virtual void Initialize(ExtensionContext context)
        {
            this.ExtensionContext = context;
        }

        private void FindParentModule()
        {
            // Iterate up the parent tree to find the parent
            var ctrlCurrent = this.Parent;
            while (ctrlCurrent != null && !(ctrlCurrent is PortalModuleBase))
            {
                if (ctrlCurrent.Equals(ctrlCurrent.Parent))
                {
                    return;
                }

                ctrlCurrent = ctrlCurrent.Parent;
            }
            if (ctrlCurrent != null)
            {
                this._parentModule = ctrlCurrent as PortalModuleBase;
            }
        }
    }
}