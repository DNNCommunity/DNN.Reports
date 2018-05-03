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
    using DotNetNuke.Modules.Reports.Converters;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Stores information about a single report that can be displayed by the module
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	06/16/2006	Created
    ///     [anurse]    08/02/2007  Added DataSource-related properties and removed Query property
    /// </history>
    /// -----------------------------------------------------------------------------
    [Serializable]
    public class ReportInfo
    {
        #region  Public Constructor

        #endregion

        #region  Private Fields

        private int _ModuleID;
        private string _Title;
        private string _Description;
        private string _Parameters;
        private int _CacheDuration;
        private int _CreatedBy;
        private DateTime _CreatedOn;

        private readonly Dictionary<string, IList<ConverterInstanceInfo>> _Converters =
            new Dictionary<string, IList<ConverterInstanceInfo>>();

        private string _Visualizer;
        private Dictionary<string, string> _VisualizerSettings = new Dictionary<string, string>();
        private string _DataSource;
        private string _DataSourceClass;
        private Dictionary<string, string> _DataSourceSettings = new Dictionary<string, string>();
        private bool _ShowInfoPane;
        private bool _ShowControls;
        private bool _AutoRunReport;
        private bool _TokenReplace;

        #endregion

        #region  Public Properties

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the module ID of the report
        /// </summary>
        /// <returns>The module ID of the report</returns>
        /// <history>
        ///     [anurse]	06/19/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public int ModuleID
        {
            get { return this._ModuleID; }
            set { this._ModuleID = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the title of the report
        /// </summary>
        /// <returns>The title of the report</returns>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a short description of the report
        /// </summary>
        /// <returns>A short description of the report</returns>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string Description
        {
            get { return this._Description; }
            set { this._Description = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a comma separated list of URL parameters used by the report
        /// </summary>
        /// <returns>A comma separated list of URL parameters used by the report</returns>
        /// <history>
        ///     [anurse]	04/15/2009	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string Parameters
        {
            get { return this._Parameters; }
            set { this._Parameters = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the cache duration for the report
        /// </summary>
        /// <returns>The number of minutes to cache the report results</returns>
        /// <history>
        ///     [anurse]	01/15/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public int CacheDuration
        {
            get { return this._CacheDuration; }
            set { this._CacheDuration = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the user ID of the user who created this report
        /// </summary>
        /// <returns>The user ID of the user who created this report</returns>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public int CreatedBy
        {
            get { return this._CreatedBy; }
            set { this._CreatedBy = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the date that this report was created
        /// </summary>
        /// <returns>The date that this report was created</returns>
        /// <history>
        ///     [anurse]	06/19/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public DateTime CreatedOn
        {
            get { return this._CreatedOn; }
            set { this._CreatedOn = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets a collection of Converters keyed by field name
        /// </summary>
        /// <returns>A collection of Converters keyed by field name</returns>
        /// <history>
        ///     [anurse]	10/13/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public IDictionary<string, IList<ConverterInstanceInfo>> Converters => this._Converters;

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the data source for this report
        /// </summary>
        /// <returns>The name of the data source for this report</returns>
        /// <history>
        ///     [anurse]	08/02/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string DataSource
        {
            get { return this._DataSource; }
            set { this._DataSource = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the class containing the data source for this report
        /// </summary>
        /// <returns>The name of the class containing the data source for this report</returns>
        /// <history>
        ///     [anurse]	08/04/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string DataSourceClass
        {
            get { return this._DataSourceClass; }
            set { this._DataSourceClass = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a table of name-value pairs representing the settings
        ///     associated with the data source for this report
        /// </summary>
        /// <history>
        ///     [anurse]	08/02/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public Dictionary<string, string> DataSourceSettings
        {
            get { return this._DataSourceSettings; }
            set { this._DataSourceSettings = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the visualizer to use to display this
        ///     report
        /// </summary>
        /// <history>
        ///     [anurse]	01/07/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string Visualizer
        {
            get { return this._Visualizer; }
            set { this._Visualizer = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a table of name-value pairs representing the settings
        ///     associated with the visualizer for this report
        /// </summary>
        /// <history>
        ///     [anurse]	01/07/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public Dictionary<string, string> VisualizerSettings
        {
            get { return this._VisualizerSettings; }
            set { this._VisualizerSettings = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a boolean indicating if the info pane should be shown for this report
        /// </summary>
        /// <history>
        ///     [anurse]	10/17/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public bool ShowInfoPane
        {
            get { return this._ShowInfoPane; }
            set { this._ShowInfoPane = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a boolean indicating if the controls pane should be shown for this report
        /// </summary>
        /// <history>
        ///     [anurse]	10/17/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public bool ShowControls
        {
            get { return this._ShowControls; }
            set { this._ShowControls = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a boolean indicating if this report should run on demand, or
        ///     if it should be run automatically when the module is viewed
        /// </summary>
        /// <history>
        ///     [anurse]	10/17/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public bool AutoRunReport
        {
            get { return this._AutoRunReport; }
            set { this._AutoRunReport = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether token replace should take place.
        /// </summary>
        /// <value><c>true</c> if [token replace]; otherwise, <c>false</c>.</value>
        public bool TokenReplace
        {
            get { return this._TokenReplace; }
            set { this._TokenReplace = value; }
        }

        #endregion
    }
}