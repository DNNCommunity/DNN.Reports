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
    using System.Data;
    using System.Data.Common;
    using Components;
    using global::DotNetNuke.Modules.Reports.Converters;
    using global::DotNetNuke.Modules.Reports.Exceptions;

    /// <summary>
    ///     Base class for ADO.Net-based Data Sources
    /// </summary>
    public abstract class ADODataSourceBase : SqlDataSourceBase
    {
        public override bool CanProcessConverters => true;

        protected override DataView ExecuteSQLReport()
        {
            try
            {
                IDataReader dr = null;

                // Create the connection
                var conn = this.CreateConnection();

                // Create and configure a command
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    Convert.ToString(this.CurrentReport.DataSourceSettings[ReportsConstants.SETTING_Query]);

                // Configure parameters
                foreach (var pair in this.Parameters)
                {
                    cmd.Parameters.Add(this.CreateParameter(Convert.ToString(pair.Key), pair.Value));
                }

                // Open the connection
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Execute the command
                dr = cmd.ExecuteReader();

                // Create the data table
                var dv = this.CreateDataView(new ConvertingDataReader(dr, this.CurrentReport.Converters));

                // Close the connection
                conn.Close();

                return dv;
            }
            catch (DbException ex)
            {
                throw this.CreateDataSourceException(ex);
            }
        }

        /// <summary>
        ///     Creates a new <see cref="DbConnection" /> and configures it with the correct connection string
        /// </summary>
        /// <returns>A configured <see cref="DbConnection" /></returns>
        /// <remarks>Do not open the connection before returning it</remarks>
        protected abstract DbConnection CreateConnection();

        /// <summary>
        ///     Creates a new <see cref="DbParameter" /> and configures it with the correct name and value
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>A configured <see cref="DbParameter" /></returns>
        /// <remarks>
        ///     IMPORTANT: The <paramref name="name" /> parameter does not include any prefixes
        ///     (such as '@') that are often used in Database Query Parameters. For example, in
        ///     a SQL Server Data Source, a parameter name such as 'Foo' will have to be converted
        ///     to '@Foo' in this method.
        /// </remarks>
        protected abstract DbParameter CreateParameter(string name, object value);

        /// <summary>
        ///     Creates a new <see cref="DataSourceException" /> configured with a message relevant and
        ///     exception recieved from the ADO.Net Database Provider
        /// </summary>
        /// <param name="dbException">The exception recieved from the provider</param>
        /// <returns>A new <see cref="DataSourceException" /></returns>
        protected abstract DataSourceException CreateDataSourceException(DbException dbException);

        /// <summary>
        ///     Helper function to build the connection string, checks the Connection String setting
        ///     called "ConnectionString"
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual string CreateConnectionString()
        {
            // Check the connection string setting
            if (this.CurrentReport.DataSourceSettings.ContainsKey(ReportsConstants.SETTING_ConnectionString))
            {
                return this.CurrentReport.DataSourceSettings[ReportsConstants.SETTING_ConnectionString];
            }
            return string.Empty;
        }
    }
}