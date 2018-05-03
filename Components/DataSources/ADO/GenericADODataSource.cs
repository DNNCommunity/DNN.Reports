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


namespace DotNetNuke.Modules.Reports.DataSources.ADO
{
    using System;
    using System.Data.Common;
    using global::DotNetNuke.Modules.Reports.Exceptions;

    /// <summary>
    ///     A Data Source that provides data by querying a Generic ADO.Net Data Provider
    /// </summary>
    public class GenericADODataSource : ADODataSourceBase
    {
        private DbProviderFactory GetProviderFactory()
        {
            return DbProviderFactories.GetFactory(
                Convert.ToString(this.CurrentReport.DataSourceSettings[ReportsController.SETTING_ADO_ProviderName]));
        }

        protected override DbConnection CreateConnection()
        {
            this.ValidateRequiredReportSetting(ReportsController.SETTING_ADO_ProviderName);

            // Get the provider factory
            var factory = this.GetProviderFactory();

            // Create the connection, then populate the connection string
            var conn = factory.CreateConnection();
            conn.ConnectionString = this.CreateConnectionString();
            return conn;
        }

        protected override DataSourceException CreateDataSourceException(DbException dbException)
        {
            throw new DataSourceException(new LocalizedText("DSError.Text",
                                                            this.ExtensionContext.ResolveExtensionResourcesPath(
                                                                "DataSource.ascx.resx"),
                                                            dbException.ErrorCode.ToString(), dbException.Message),
                                          string.Format(
                                              "An error occurred while executing the data source (Code: {0}): {1}",
                                              dbException.ErrorCode, dbException.Message));
        }

        protected override DbParameter CreateParameter(string name, object value)
        {
            var factory = this.GetProviderFactory();
            var paramPrefix =
                Convert.ToString(SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                                   ReportsController.SETTING_ADO_ParamPrefix,
                                                                   string.Empty));
            var param = factory.CreateParameter();
            param.ParameterName = string.Concat(paramPrefix, name);
            param.Value = value;
            return param;
        }
    }
}