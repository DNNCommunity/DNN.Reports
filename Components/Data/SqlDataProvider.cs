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


namespace DotNetNuke.Modules.Reports.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Transactions;
    using Components;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Framework.Providers;
    using DotNetNuke.Modules.Reports.Visualizers.Xslt;
    using Microsoft.ApplicationBlocks.Data;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Implements the Reports Data Provider on Microsoft SQL Server
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	08/29/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {
        #region Constructors

        public SqlDataProvider()
        {
            this._providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ReportsConstants.ProviderType);


            // Read the configuration specific information for this provider
            var objProvider =
                (Provider) this._providerConfiguration.Providers[this._providerConfiguration.DefaultProvider];

            // Read the attributes for this provider

            //Get Connection string from web.config
            this._connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(this._connectionString))
            {
                // Use connection string specified in provider
                this._connectionString = objProvider.Attributes["connectionString"];
            }

            this._providerPath = objProvider.Attributes["providerPath"];

            this._objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(this._objectQualifier) && this._objectQualifier.EndsWith("_") == false)
            {
                this._objectQualifier += "_";
            }

            this._databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(this._databaseOwner) && this._databaseOwner.EndsWith(".") == false)
            {
                this._databaseOwner += ".";
            }
        }

        #endregion

        public override DbParameter CreateInputParameter(string name, object value)
        {
            return new SqlParameter(string.Concat("@", name), value);
        }

        public override IDataReader ExecuteSQL(string strScript, params DbParameter[] parameters)
        {
            // HACK: Copy-pasted from Core SqlDataProvider - core doesn't provide a system for parameterized dynamic sql

            // TODO: Switch to Regex and use IgnoreCase (punting this fix in 5.1 due to testing burden)
            strScript = strScript.Replace("{databaseOwner}", this._databaseOwner);
            strScript = strScript.Replace("{dO}", this._databaseOwner);
            strScript = strScript.Replace("{do}", this._databaseOwner);
            strScript = strScript.Replace("{Do}", this._databaseOwner);
            strScript = strScript.Replace("{objectQualifier}", this._objectQualifier);
            strScript = strScript.Replace("{oq}", this._objectQualifier);
            strScript = strScript.Replace("{oQ}", this._objectQualifier);
            strScript = strScript.Replace("{Oq}", this._objectQualifier);

            // Convert the db parameters to sql parameters
            var sqlParams = new SqlParameter[parameters.Length];
            for (var i = 0; i <= parameters.Length - 1; i++)
            {
                sqlParams[i] = (SqlParameter) parameters[i];
            }

            return SqlHelper.ExecuteReader(this._connectionString, CommandType.Text, strScript, sqlParams);
        }

        public override IDataReader GetXsltExtensionObjects(int tabModuleId)
        {
            return DotNetNuke.Data.DataProvider.Instance()
                             .ExecuteReader("reports_GetXsltExtensionObjects", tabModuleId);
        }

        public override void SetXsltExtensionObjects(int tabModuleId, IEnumerable<ExtensionObjectInfo> extensionObjects)
        {
            // Start a Transaction
            using (var transaction = new TransactionScope())
            {
                DotNetNuke.Data.DataProvider.Instance()
                          .ExecuteNonQuery("reports_ClearXsltExtensionObjects", tabModuleId);

                foreach (var extensionObject in extensionObjects)
                {
                    DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("reports_AddXsltExtensionObject",
                                                                            tabModuleId,
                                                                            extensionObject.XmlNamespace,
                                                                            extensionObject.ClrType);
                }

                transaction.Complete();
            }
        }

        #region Private Members

        private readonly ProviderConfiguration _providerConfiguration;

        private readonly string _connectionString;
        private string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion
    }
}