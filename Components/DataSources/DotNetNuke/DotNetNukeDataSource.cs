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


namespace DotNetNuke.Modules.Reports.DataSources.DotNetNuke
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using global::DotNetNuke.Modules.Reports.Converters;
    using global::DotNetNuke.Modules.Reports.Data;
    using global::DotNetNuke.Modules.Reports.Exceptions;

    /// <summary>
    ///     A Data Source that provides data by querying the DotNetNuke Data Provider
    /// </summary>
    public class DotNetNukeDataSource : SqlDataSourceBase
    {
        public override bool CanProcessConverters => true;

        protected override DataView ExecuteSQLReport()
        {
            try
            {
                // Convert inputParameters to DB-specific Parameters
                var @params = new DbParameter[this.Parameters.Count + 1];
                var i = 0;
                foreach (var pair in this.Parameters)
                {
                    @params[i] = DataProvider.Instance().CreateInputParameter(Convert.ToString(pair.Key),
                                                                              pair.Value);
                    i++;
                }

                // Execute the query against the data source
                var dr = DataProvider.Instance().ExecuteSQL(
                    Convert.ToString(this.CurrentReport.DataSourceSettings[this.QueryKey]),
                    @params);
                return this.CreateDataView(new ConvertingDataReader(dr, this.CurrentReport.Converters));
            }
            catch (SqlException ex)
            {
                throw new DataSourceException(new LocalizedText("SqlError.Text",
                                                                this.ExtensionContext.ResolveExtensionResourcesPath(
                                                                    "DataSource.ascx.resx"),
                                                                ex.LineNumber.ToString(), ex.Message),
                                              string.Format("There is an error in your SQL at line {0}: {1}",
                                                            ex.LineNumber, ex.Message));
            }
        }
    }
}