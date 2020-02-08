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


using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DotNetNuke.Framework;
using DotNetNuke.Modules.Reports.Visualizers.Xslt;

namespace DotNetNuke.Modules.Reports.Data
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Provides an abstract base class for Reports Data Providers
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	08/29/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {
        /// <summary>
        ///     Creates an input parameter, given a name and value pair
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <remarks>
        ///     This method is used to construct the parameters that will be passed to any of
        ///     the data provider methods that accept <see cref="DbParameter" /> or arrays of that
        ///     type. If parameters are not supported by the data provider, simply return nothing
        ///     and ignore the parameters passed in to those methods.
        /// </remarks>
        /// <returns>A <see cref="DbParameter" /> containing the constructed parameter</returns>
        public abstract DbParameter CreateInputParameter(string name, object value);

        /// <summary>
        ///     Executes the specified SQL with the specified parameters
        /// </summary>
        /// <param name="strScript">The script to execute</param>
        /// <param name="parameters">The parameters to pass into the script</param>
        /// <history>
        ///     [anurse]     08/31/2006    documented
        /// </history>
        public abstract IDataReader ExecuteSQL(string strScript, params DbParameter[] parameters);

        public abstract IDataReader GetXsltExtensionObjects(int tabModuleId);

        public abstract void SetXsltExtensionObjects(int tabModuleId,
                                                     IEnumerable<ExtensionObjectInfo> extensionObjects);

        #region Shared/Static Methods

        // singleton reference to the instantiated object
        private static DataProvider objProvider;

        // constructor
        static DataProvider()
        {
            CreateProvider();
        }

        // dynamically create provider
        private static void CreateProvider()
        {
            objProvider =
                (DataProvider) Reflection.CreateObject("data", "DotNetNuke.Modules.Reports.Data", string.Empty);
        }

        // return the provider
        public static DataProvider Instance()
        {
            return objProvider;
        }

        #endregion
    }
}