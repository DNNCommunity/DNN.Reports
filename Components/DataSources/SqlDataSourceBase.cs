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


namespace DotNetNuke.Modules.Reports.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using global::DotNetNuke.Entities.Modules;
    using global::DotNetNuke.Modules.Reports.Converters;
    using global::DotNetNuke.Modules.Reports.Exceptions;

    /// <summary>
    ///     Base class for SQL-based Data Sources
    /// </summary>
    public abstract class SqlDataSourceBase : DataSourceBase
    {
        /// <summary>
        ///     Gets the key used to find the SQL Query in the Data Source Settings
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual string QueryKey => ReportsController.SETTING_Query;

        public override DataView ExecuteReport(ReportInfo report,
                                               PortalModuleBase hostModule,
                                               IDictionary<string, object> inputParameters)
        {
            base.ExecuteReport(report, hostModule, inputParameters);

            // Load up the properties
            this.CurrentReport = report;
            this.HostModule = hostModule;
            this.Parameters = inputParameters;
            this.Converters = this.Converters;

            // Validate Query
            if (!report.DataSourceSettings.ContainsKey(this.QueryKey) ||
                string.IsNullOrEmpty(Convert.ToString(report.DataSourceSettings[this.QueryKey])))
            {
                if (ReferenceEquals(hostModule, null))
                {
                    return new DataView(new DataTable("QueryResults")); // Not hosted in a module, so fail silently
                }
                if (hostModule.UserInfo != null)
                {
                    if (hostModule.UserInfo.IsSuperUser)
                    {
                        throw new DataSourceNotConfiguredException(new LocalizedText("HostNoQuery.Error"),
                                                                   "There is no data source configured for this module, please configure one in the Module Settings page");
                    }
                    if (hostModule.IsEditable)
                    {
                        throw new DataSourceNotConfiguredException(new LocalizedText("AdminNoQuery.Error"),
                                                                   "There is no data source configured for this module, please contact your Host for assistance");
                    }
                }
                else
                {
                    throw new DataSourceNotConfiguredException(new LocalizedText("AnonNoQuery.Error"));
                }
            }

            // Execute the Query
            return this.ExecuteSQLReport();
        }

        /// <summary>
        ///     When overriden, executes the SQL-based report
        /// </summary>
        /// <returns>A <see cref="DataTable" /> containing the results</returns>
        /// <remarks>
        ///     The report, host module and input parameters can be accessed through
        ///     the <see cref="CurrentReport" />, <see cref="HostModule" />,
        ///     <see cref="Parameters" /> properties.
        /// </remarks>
        protected abstract DataView ExecuteSQLReport();


        /// <summary>
        ///     Validates that the specified setting exists AND has a non-empty value
        /// </summary>
        /// <param name="setting">The name of the setting to validate</param>
        /// <exception cref="RequiredSettingMissingException">
        ///     The setting was not found
        /// </exception>
        protected void ValidateRequiredReportSetting(string setting)
        {
            if (SettingsUtil.FieldIsNotSet(this.CurrentReport.DataSourceSettings, setting))
            {
                throw new RequiredSettingMissingException(setting, this.ExtensionContext);
            }
        }

        #region  Private Fields

        #endregion

        #region  Protected Properties

        protected ReportInfo CurrentReport { get; private set; }

        protected PortalModuleBase HostModule { get; private set; }

        protected IDictionary<string, object> Parameters { get; private set; }

        protected IDictionary<string, IList<ConverterInstanceInfo>> Converters { get; private set; }

        #endregion
    }
}