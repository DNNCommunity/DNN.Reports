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


namespace Components
{
    using System;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules.Settings;

    /// <summary>
    ///     The settings to be saved to the database. The datasource and visualzier settings are handled by the ReportInfo
    ///     class.
    /// </summary>
    [Serializable]
    public class ReportsModuleSettings
    {
        [ModuleSetting(Prefix = "dnn_Reports_")]
        public string Title { get; set; } = string.Empty;

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public string Description { get; set; } = string.Empty;

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public string Parameters { get; set; } = string.Empty;

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public int CreatedBy { get; set; }

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public DateTime CreatedOn { get; set; }

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public string DataSource { get; set; } = string.Empty;

        [ModuleSetting(Prefix = "dnn_Reports_")]
        public string DataSourceClass { get; set; } = string.Empty;

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public string Visualizers { get; set; } = string.Empty;

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public int CacheDuration { get; set; }

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public string Visualizer { get; set; } = string.Empty;

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public bool ShowInfoPane { get; set; }

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public bool ShowControls { get; set; }

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public bool AutoRunReport { get; set; } = true;

        [TabModuleSetting(Prefix = "dnn_Reports_")]
        public bool TokenReplace { get; set; }
    }

    /// <summary>
    ///     The <see cref="SettingsRepository{T}" /> used for storing and retrieving <see cref="ReportsModuleSettings" />
    /// </summary>
    public class ReportsModuleSettingsRepository : SettingsRepository<ReportsModuleSettings>
    { }
}