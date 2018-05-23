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


namespace DotNetNuke.Modules.Reports.DataSources.SqlServer
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Components;
    using global::DotNetNuke.Modules.Reports.Exceptions;

    /// <summary>
    ///     A Data Source that provides data by querying the Sql Data Provider
    /// </summary>
    public class SqlServerDataSource : ADODataSourceBase
    {
        protected override string CreateConnectionString()
        {
            var connStr = base.CreateConnectionString();
            if (!string.IsNullOrEmpty(connStr))
            {
                return connStr;
            }

            // No connection string setting, we need to build it

            // First validate the fields
            var server = Convert.ToString(
                SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                  ReportsConstants.SETTING_Server,
                                                  string.Empty));
            var database = Convert.ToString(
                SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                  ReportsConstants.SETTING_Database,
                                                  string.Empty));
            var useIntegratedSecurity = Convert.ToBoolean(
                SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                  ReportsConstants.SETTING_Sql_UseIntegratedSecurity,
                                                  false));
            var userName = Convert.ToString(
                SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                  ReportsConstants.SETTING_UserName,
                                                  string.Empty));
            var password = Convert.ToString(
                SettingsUtil.GetDictionarySetting(this.CurrentReport.DataSourceSettings,
                                                  ReportsConstants.SETTING_Password,
                                                  string.Empty));

            var csBuilder = new SqlConnectionStringBuilder();
            if (!string.IsNullOrEmpty(server))
            {
                csBuilder.Add("Server", server);
            }
            if (!string.IsNullOrEmpty(database))
            {
                csBuilder.Add("Database", database);
            }
            if (!useIntegratedSecurity)
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    csBuilder.Add("User ID", userName);
                }
                if (!string.IsNullOrEmpty(password))
                {
                    csBuilder.Add("Password", password);
                }
                csBuilder.Add("Trusted_Connection", "False");
            }
            else
            {
                csBuilder.Add("Trusted_Connection", "True");
            }

            return csBuilder.ConnectionString;
        }

        protected override DbConnection CreateConnection()
        {
            return new SqlConnection(this.CreateConnectionString());
        }

        protected override DataSourceException CreateDataSourceException(DbException dbException)
        {
            var ex = (SqlException) dbException;
            throw new DataSourceException(
                new LocalizedText("SqlError.Text",
                                  this.ExtensionContext.ResolveExtensionResourcesPath("DataSource.ascx.resx"),
                                  ex.LineNumber.ToString(), ex.Message),
                string.Format("There is an error in your SQL at line {0}: {1}", ex.LineNumber, ex.Message));
        }

        protected override DbParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(string.Concat("@", name), value);
        }
    }
}