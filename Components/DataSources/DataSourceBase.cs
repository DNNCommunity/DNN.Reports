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
    using global::DotNetNuke.Modules.Reports.Extensions;

    /// <summary>
    ///     Base class for Data Sources
    /// </summary>
    public abstract class DataSourceBase : ReportsExtensionBase, IDataSource
    {
        public virtual DataView ExecuteReport(ReportInfo report,
                                              PortalModuleBase hostModule,
                                              IDictionary<string, object> inputParameters)
        {
            // Verify Method Contract
            if (ReferenceEquals(report, null))
            {
                throw new ArgumentNullException("report");
            }
            if (ReferenceEquals(inputParameters, null))
            {
                inputParameters = new Dictionary<string, object>();
            }

            // Verify DataSourceClass property
            if (!report.DataSourceClass.Equals(this.GetType().FullName))
            {
                throw new InvalidOperationException("The specified report is not configured to use this data source");
            }

            return this.CreateDataView();
        }

        public virtual bool CanProcessConverters => false;

        /// <summary>
        ///     Constructs an empty data table configured for loading data for the reports module.
        /// </summary>
        /// <returns>A empty data table, configured for loading data for the reports module</returns>
        protected DataView CreateDataView()
        {
            return this.CreateDataView(null);
        }

        /// <summary>
        ///     Constructs a data table configured for loading data for the reports module. Optionally
        ///     reads in the contents of the specified reader
        /// </summary>
        /// <param name="reader">
        ///     The reader to load from, or a null reference (Nothing in VB.net)
        ///     to construct an empty table
        /// </param>
        /// <returns>A data table, optionally filled with data from the specified reader</returns>
        /// <remarks>
        ///     IMPORTANT: This method does NOT wrap the recieved reader in a
        ///     <see cref="Converters.ConvertingDataReader" />. In order to support converters,
        ///     implementors should wrap the data reader they recieve from their data source
        ///     in a <see cref="Converters.ConvertingDataReader" /> and then pass it to this method.
        ///     If that is done, the <see cref="CanProcessConverters" /> property should be overriden
        ///     to return true.
        /// </remarks>
        protected DataView CreateDataView(IDataReader reader)
        {
            var dt = new DataTable("QueryResults");
            if (reader != null)
            {
                dt.Load(reader);
            }
            return new DataView(dt);
        }
    }
}