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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Reports.Extensions;


namespace DotNetNuke.Modules.Reports.DataSources
{
    using System.Collections.Generic;
    using System.Data;


    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The IReportsDataSource class provides an interface to a Reports Module Data Source
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         IMPORTANT: Data Sources may be reused in a single request context so they should not
    ///         store any information regarding a particular report in between calls to
    ///         <see cref="IDataSource.ExecuteReport" />.
    ///     </para>
    ///     <para>
    ///         Data Sources are components which provide data to Reports Module Visualizers.
    ///     </para>
    /// </remarks>
    /// <history>
    ///     [anurse]     08/02/2007    Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public interface IDataSource : IReportsExtension
    {
        /// <summary>
        ///     Gets a boolean indicating if this data source can process converters
        /// </summary>
        /// <returns>
        ///     True if this data source processes all the converters passed to it.
        ///     False if it does not.
        /// </returns>
        /// <remarks>
        ///     Converters are pieces of code that are run on each of the values in a
        ///     particular field. By processing them at the data source, performance may be improved.
        ///     However, not all data sources support this functionality. See
        ///     <see cref="ReportsController.ApplyConverter" /> for more details on applying
        ///     a converter to a particular field. Also, see <see cref="Converters.ConvertingDataReader" />
        ///     for a system that allows any data source which uses an
        ///     <see cref="IDataReader" /> to handle converters automatically.
        /// </remarks>
        bool CanProcessConverters { get; }

        /// <summary>
        ///     Executes the specified report, returning a <see cref="System.Data.DataView" /> containing
        ///     the results
        /// </summary>
        /// <param name="report">The settings for the report to execute</param>
        /// <param name="hostModule">The module that is hosting the Data Source. MAY BE A NULL REFERENCE (Nothing in VB.Net)</param>
        /// <param name="inputParameters">
        ///     The parameters to pass to the data source. May be a null reference (Nothing in VB.net),
        ///     in which case it should be treated as an empty dictionary
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        ///     The specified report does not use this data source
        /// </exception>
        /// <exception cref="Exceptions.DataSourceException">
        ///     An exception occurred when executing the report
        /// </exception>
        /// <exception cref="Exceptions.DataSourceNotConfiguredException">
        ///     The data source is not properly configured
        /// </exception>
        DataView ExecuteReport(ReportInfo report,
                               PortalModuleBase hostModule,
                               IDictionary<string, object> inputParameters);
    }
}