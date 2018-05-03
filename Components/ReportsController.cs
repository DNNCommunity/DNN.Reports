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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Compilation;
    using System.Web.Configuration;
    using System.Xml;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Framework;
    using DotNetNuke.Modules.Reports.Converters;
    using DotNetNuke.Modules.Reports.Data;
    using DotNetNuke.Modules.Reports.DataSources;
    using DotNetNuke.Modules.Reports.Exceptions;
    using DotNetNuke.Modules.Reports.Extensions;
    using DotNetNuke.Modules.Reports.Visualizers.Xslt;
    using DotNetNuke.Services.Search.Entities;
    using DotNetNuke.Services.Tokens;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The Controller class for Reports
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	06/16/2006	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ReportsController : ModuleSearchBase, IPortable
    {
        #region  Friend Methods

        internal static void LoadExtensionSettings(Hashtable ModuleSettings, Hashtable TabModuleSettings,
                                                   ReportInfo Report)
        {
            // Load Data Source Settings
            if (!string.IsNullOrEmpty(Report.DataSource))
            {
                LoadPrefixedSettings(ModuleSettings, string.Format("{0}{1}_", PREFIX_DataSource, Report.DataSource),
                                     Report.DataSourceSettings);
            }

            if (TabModuleSettings != null)
            {
                // Load Visualizer Settings
                LoadPrefixedSettings(TabModuleSettings, string.Format("{0}{1}_", PREFIX_Visualizer, Report.Visualizer),
                                     Report.VisualizerSettings);
            }
        }

        #endregion


        #region  Public Constants

        // App Settings
        public const string APPSETTING_AllowCachingWithParameters = "Reports_AllowCachingAndParameters";

        // Prefixes
        public const string PREFIX_Reports = "dnn_Reports_";

        public const string PREFIX_Visualizer = "dnn_ReportsVis_";
        public const string PREFIX_DataSource = "dnn_ReportsDS_";

        // Package Types
        public const string PACKAGETYPE_Prefix = "DNN_Reports_";

        public const string PACKAGETYPE_Visualizer = PACKAGETYPE_Prefix + "Visualizer";
        public const string PACKAGETYPE_DataSource = PACKAGETYPE_Prefix + "DataSource";

        // Reports Module Settings
        public const string SETTING_ReportTitle = PREFIX_Reports + "Title";

        public const string SETTING_ReportDescription = PREFIX_Reports + "Description";
        public const string SETTING_ReportParameters = PREFIX_Reports + "Parameters";
        public const string SETTING_ReportCreatedBy = PREFIX_Reports + "CreatedBy";
        public const string SETTING_ReportCreatedOn = PREFIX_Reports + "CreatedOn";
        public const string SETTING_CacheDuration = PREFIX_Reports + "CacheDuration";
        public const string SETTING_DataSource = PREFIX_Reports + "DataSource";
        public const string SETTING_DataSourceClass = PREFIX_Reports + "DataSourceClass";
        public const string SETTING_Visualizer = PREFIX_Reports + "Visualizer";
        public const string SETTING_Converters = PREFIX_Reports + "Converters";
        public const string SETTING_ShowInfoPane = PREFIX_Reports + "ShowInfoPane";
        public const string SETTING_ShowControls = PREFIX_Reports + "ShowControls";
        public const string SETTING_AutoRunReport = PREFIX_Reports + "AutoRunReport";
        public const string SETTING_TokenReplace = PREFIX_Reports + "TokenReplace";

        // Data Source Settings

        // Common
        public const string SETTING_Query = "Query";

        public const string SETTING_ConnectionString = "ConnectionString";
        public const string SETTING_Server = "Server";
        public const string SETTING_Database = "Database";
        public const string SETTING_UserName = "UserName";
        public const string SETTING_Password = "Password";
        public const string SETTING_UseConnectionString = "UseConnectionString";

        // Sql
        public const string SETTING_Sql_UseIntegratedSecurity = "UseIntegratedSecurity";

        // ADO
        public const string SETTING_ADO_ProviderName = "ProviderName";

        public const string SETTING_ADO_ParamPrefix = "ParamPrefix";

        // Visualizer Settings

        // Common
        public const string SETTING_Height = "Height";

        public const string SETTING_Width = "Width";

        // Chart Visualizer
        public const string SETTING_Chart_Type = "Type";

        public const string SETTING_Chart_BarNameColumn = "BarNameColumn";
        public const string SETTING_Chart_BarValueColumn = "BarValueColumn";
        public const string SETTING_Chart_ColorMode = "ColorMode";
        public const string SETTING_Chart_BarColorColumn = "BarColorColumn";
        public const string SETTING_Chart_BarColor = "BarColor";
        public const string SETTING_Chart_XAxisTitle = "XAxisTitle";
        public const string SETTING_Chart_YAxisTitle = "YAxisTitle";

        // Grid Visualizer
        public const string SETTING_Grid_EnablePaging = "EnablePaging";

        public const string SETTING_Grid_EnableSorting = "EnableSorting";
        public const string SETTING_Grid_PageSize = "PageSize";
        public const string SETTING_Grid_ShowHeader = "ShowHeader";
        public const string SETTING_Grid_GridLines = "GridLines";
        public const string SETTING_Grid_AdditionalCSS = "AdditionalCSS";
        public const string SETTING_Grid_CSSClass = "CSSClass";
        public const string DEFAULT_Grid_GridLines = "None";

        // Reporting Services Visualizer
        public const string SETTING_RS_Mode = "Mode";

        public const string SETTING_RS_LocalReportFile = "LocalFile";
        public const string SETTING_RS_ServerReportUrl = "ServerUrl";
        public const string SETTING_RS_ServerReportPath = "ServerPath";
        public const string SETTING_RS_DataSourceName = "DataSourceName";
        public const string SETTING_RS_VisibleUIElements = "VisibleUI";
        public const string SETTING_RS_EnableExternalImages = "EnExImg";
        public const string SETTING_RS_EnableHyperlinks = "EnHyper";
        public const string SETTING_RS_Domain = "Domain";

        public const string DEFAULT_RS_VisibleUIElements =
                "DocumentMapButton,ExportControls,FindControls,NavigationControls,PrintButton,PromptAreaButton,RefreshButton,ToolBar,ZoomControl"
            ;

        // HTML Visualizer
        public const string SETTING_Html_TemplateFile = "TemplateFile";

        public const string SETTING_Html_Repeat = "Repeat";
        public const string SETTING_Html_Separator = "Separator";

        // XSLT Visualizer
        public const string SETTING_Xslt_TransformFile = "TransformFile";

        public const string SETTING_Xslt_ExtensionObject = "Ext";

        public const string CACHEKEY_Reports = "dnn_ReportCache";

        #endregion

        #region  Public Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets the report associated with a reports module without retrieving
        ///     tab module specific settings (visualizers)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleId">The Id of the module to retrive the report for</param>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        ///     [anurse]	01/15/2007	Updated to use GetReport(Integer, Integer) override
        /// </history>
        /// -----------------------------------------------------------------------------
        public static ReportInfo GetReport(int ModuleId)
        {
           var info = new ModuleInfo
                           {
                               ModuleID = ModuleId,
                               TabModuleID = Null.NullInteger
                           };
            return GetReport(info);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets the report associated with a reports module (including tab module
        ///     specific settings)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name=" ModuleInfo">The module info</param> 
        /// <history>
        ///     [anurse]	01/15/2007	Created from GetReport(Integer)
        /// </history>
        /// -----------------------------------------------------------------------------
        public static ReportInfo GetReport(ModuleInfo ModuleInfo)
        {
            var ModuleId = ModuleInfo.ModuleID;
            var TabModuleId = ModuleInfo.TabModuleID;

            // Check for a null Module Id
            if (ModuleId == Null.NullInteger)
            {
                return null;
            }

            // Extract the Title, Description and Query from the settings
            var objModuleController = new ModuleController();
            
            var objSettings = ModuleInfo.ModuleSettings;
            // Check that the settings hashtable was retrieved
            if (ReferenceEquals(objSettings, null))
            {
                return null;
            }

            // Setup the Report
            var objReport = new ReportInfo();

            // Build the Report from the Module Settings
            objReport.Title =
                Convert.ToString(SettingsUtil.GetHashtableSetting(objSettings, SETTING_ReportTitle, Null.NullString));
            objReport.Description =
                Convert.ToString(
                    SettingsUtil.GetHashtableSetting(objSettings, SETTING_ReportDescription, Null.NullString));
            objReport.Parameters =
                Convert.ToString(
                    SettingsUtil.GetHashtableSetting(objSettings, SETTING_ReportParameters, Null.NullString));
            objReport.CreatedOn =
                Convert.ToDateTime(
                    SettingsUtil.GetHashtableSetting(objSettings, SETTING_ReportCreatedOn, Null.NullDate));
            objReport.CreatedBy =
                Convert.ToInt32(
                    SettingsUtil.GetHashtableSetting(objSettings, SETTING_ReportCreatedBy, Null.NullInteger));
            objReport.DataSource =
                Convert.ToString(SettingsUtil.GetHashtableSetting(objSettings, SETTING_DataSource, Null.NullString));
            objReport.DataSourceClass =
                Convert.ToString(
                    SettingsUtil.GetHashtableSetting(objSettings, SETTING_DataSourceClass, Null.NullString));
            objReport.ModuleID = ModuleId;

            // Load Filter Settings
            var converterString =
                Convert.ToString(SettingsUtil.GetHashtableSetting(objSettings, SETTING_Converters, Null.NullString));
            if (!string.IsNullOrEmpty(converterString.Trim()))
            {
                foreach (var converterItem in converterString.Split(';'))
                {
                    if (!string.IsNullOrEmpty(converterItem.Trim()))
                    {
                        var converterArray = converterItem.Split('|');
                        if (converterArray.Length >= 2 && converterArray.Length <= 3)
                        {
                            var newconverter = new ConverterInstanceInfo();
                            newconverter.FieldName = converterArray[0];
                            newconverter.ConverterName = converterArray[1];
                            if (converterArray.Length == 3)
                            {
                                newconverter.Arguments = converterArray[2].Split(',');
                            }

                            ConverterUtils.AddConverter(objReport.Converters, newconverter);
                        }
                    }
                }
            }

            // Load the tab module settings (visualizer settings) if we have a tab module Id
            Hashtable objTabModuleSettings = null;
            if (TabModuleId != Null.NullInteger)
            {
                objTabModuleSettings = ModuleInfo.TabModuleSettings;

                objReport.ShowControls =
                    Convert.ToBoolean(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_ShowControls, false));
                objReport.ShowInfoPane =
                    Convert.ToBoolean(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_ShowInfoPane, false));
                objReport.AutoRunReport =
                    Convert.ToBoolean(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_AutoRunReport, true));
                objReport.TokenReplace =
                    Convert.ToBoolean(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_TokenReplace, false));

                // Read the visualizer name and cache duration
                objReport.Visualizer =
                    Convert.ToString(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_Visualizer, "Grid"));
                objReport.CacheDuration =
                    Convert.ToInt32(
                        SettingsUtil.GetHashtableSetting(objTabModuleSettings, SETTING_CacheDuration,
                                                         Null.NullInteger));
            }

            LoadExtensionSettings(objSettings, objTabModuleSettings, objReport);

            return objReport;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Saves the report associated with a reports module
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleId">The ModuleId to set the report for</param>
        /// <param name="TabModuleId">
        ///     The Id of the instance ("TabModule") of the module to store the visualizer
        ///     settings with
        /// </param>
        /// <param name="objReport">The ReportInfo object to save</param>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        ///     [anurse]	01/15/2007	Refactored, creating SaveReportDefinition and
        ///     SaveReportViewSettings.
        /// </history>
        /// -----------------------------------------------------------------------------
        public static void UpdateReport(int ModuleId, int TabModuleId, ReportInfo objReport)
        {
            var objModuleController = new ModuleController();
            UpdateReportDefinition(objModuleController, ModuleId, objReport);
            UpdateReportView(objModuleController, TabModuleId, objReport);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Saves the report definition (module settings) associated with a reports module
        /// </summary>
        /// <param name="ModuleId">The ModuleId to set the report for</param>
        /// <param name="objReport">The ReportInfo object to save</param>
        /// <history>
        ///     [anurse]	01/15/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static void UpdateReportDefinition(int ModuleId, ReportInfo objReport)
        {
            var ctrl = new ModuleController();
            UpdateReportDefinition(ctrl, ModuleId, objReport);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Saves the report view (tab module settings) associated with a reports module
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="TabModuleId">
        ///     The Id of the instance ("TabModule") of the module to store the view
        ///     settings for.
        /// </param>
        /// <param name="objReport">The ReportInfo object to save</param>
        /// <history>
        ///     [anurse]	01/15/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static void UpdateReportView(int TabModuleId, ReportInfo objReport)
        {
            var ctrl = new ModuleController();
            UpdateReportView(ctrl, TabModuleId, objReport);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Executes a report and returns the results
        /// </summary>
        /// <param name="objReport">The ReportInfo object</param>
        /// <param name="cacheKey">The cache key for the module</param>
        /// <param name="bypassCache">A boolean indicating that the cache should be bypassed</param>
        /// <param name="hostModule">The module that is hosting the report</param>
        /// <exception cref="System.ArgumentNullException">
        ///     The value of <paramref name="objReport" /> was null (Nothing in Visual Basic)
        /// </exception>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static DataTable ExecuteReport(ReportInfo objReport, string cacheKey, bool bypassCache,
                                              PortalModuleBase hostModule)
        {
            var fromCache = false;
            return ExecuteReport(objReport, cacheKey, bypassCache, hostModule, ref fromCache);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Executes a report and returns the results
        /// </summary>
        /// <param name="objReport">The ReportInfo object</param>
        /// <param name="cacheKey">The cache key for the module</param>
        /// <param name="bypassCache">A boolean indicating that the cache should be bypassed</param>
        /// <param name="hostModule">The module that is hosting the report</param>
        /// <param name="fromCache">A boolean indicating if the data was retrieved from the cache</param>
        /// <exception cref="System.ArgumentNullException">
        ///     The value of <paramref name="objReport" /> was null (Nothing in Visual Basic)
        /// </exception>
        /// <history>
        ///     [anurse]	08/25/2007	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static DataTable ExecuteReport(ReportInfo objReport, string cacheKey, bool bypassCache,
                                              PortalModuleBase hostModule, ref bool fromCache)
        {
            try
            {
                fromCache = false;
                if (ReferenceEquals(objReport, null))
                {
                    throw new ArgumentNullException("objReport");
                }

                if (string.IsNullOrEmpty(objReport.DataSource))
                {
                    return null; // If there's no data source, there's no report
                }
                // Check the cache
                DataTable results = null;
                if (!bypassCache)
                {
                    results = DataCache.GetCache(cacheKey) as DataTable;
                    if (results != null)
                    {
                        fromCache = true;
                        return results;
                    }
                }

                // Get an instance of the data source
                //DotNetNuke.Modules.Reports.DataSources.IDataSource dataSource = GetDataSource(objReport.DataSourceClass) as DataSources.IDataSource;
                var dataSource = GetDataSource(objReport.DataSourceClass);

                // Create an extension context
                var moduleFolder = string.Empty;
                if (hostModule != null)
                {
                    moduleFolder = hostModule.TemplateSourceDirectory;
                }
                var ctxt = new ExtensionContext(moduleFolder, "DataSource", objReport.DataSource);

                // Load Parameters
                IDictionary<string, object> parameters = BuildParameters(hostModule, objReport);
                if (objReport.CacheDuration != 0)
                {
                    // Check the app setting
                    if (!"True".Equals(WebConfigurationManager.AppSettings[APPSETTING_AllowCachingWithParameters],
                                       StringComparison.InvariantCulture))
                    {
                        parameters.Clear();
                    }
                }

                // Execute the report
                dataSource.Initialize(ctxt);
                results = dataSource.ExecuteReport(objReport, hostModule, parameters).ToTable();
                results.TableName = "QueryResults";

                // Check if the converters were run
                if (!dataSource.CanProcessConverters)
                {
                    // If not, run them the slow way :(
                    foreach (DataRow row in results.Rows)
                    {
                        foreach (DataColumn col in results.Columns)
                        {
                            if (!objReport.Converters.ContainsKey(col.ColumnName))
                            {
                                continue;
                            }
                            var list = objReport.Converters[col.ColumnName];
                            foreach (var converter in list)
                            {
                                row[col] = ApplyConverter(row[col], converter.ConverterName, converter.Arguments);
                            }
                        }
                    }
                }

                // Do the token replace if specified
                if (objReport.TokenReplace)
                {
                    var localTokenReplacer = new TokenReplace();
                    foreach (DataColumn col in results.Columns)
                    {
                        // Process each column of the resultset
                        if (col.DataType == typeof(string))
                        {
                            // We want to replace the data, we don't mind that it is marked as readonly
                            if (col.ReadOnly)
                            {
                                col.ReadOnly = false;
                            }

                            var maxLength = col.MaxLength;
                            var resultText = "";

                            foreach (DataRow row in results.Rows)
                            {
                                resultText =
                                    localTokenReplacer.ReplaceEnvironmentTokens(Convert.ToString(row[col].ToString()));

                                // Don't make it too long
                                if (resultText.Length > maxLength)
                                {
                                    resultText = resultText.Substring(0, maxLength);
                                }

                                row[col] = resultText;
                            }
                        }
                    }
                }


                // Cache it, if not asked to bypass the cache
                if (!bypassCache && results != null)
                {
                    DataCache.SetCache(cacheKey, results);
                }

                // Return the results
                return results;
            }
            catch (DataSourceException)
            {
                throw;
                //Catch ex As Exception
                //    If hostModule IsNot Nothing Then
                //        Throw New DataSourceException("UnknownError.Text", hostModule.LocalResourceFile, ex, ex.Message)
                //    Else
                //        Throw
                //    End If
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Removes "Bad" SQL Commands from the specified string
        /// </summary>
        /// <param name="strSQL">This is the string to be filtered</param>
        /// <returns>A filtered version of <paramref name="strSQL" /> with commands such as INSERT or DELETE removed</returns>
        /// <history>
        ///     [anurse]        6/20/2006   Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string FormatRemoveSQL(string strSQL)
        {
            var strCleanSQL = strSQL;

            if (strSQL != null)
            {
                // each string in this array is one that must be removed from the SQL
                string[] BadSQL = {";", "--", "create ", "drop ", "insert ", "delete ", "update ", "sp_", "xp_"};

                // strip any dangerous SQL commands
                var intCommand = 0;
                for (intCommand = 0; intCommand <= BadSQL.Length - 1; intCommand++)
                {
                    // remove the current item in the "Bad SQL" list from the string by replacing it with a space
                    strCleanSQL = Regex.Replace(strCleanSQL, Convert.ToString(BadSQL.GetValue(intCommand)), " ",
                                                RegexOptions.IgnoreCase);
                }
            }

            // return the clean SQL
            return strCleanSQL;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Clears the results that have been cached by the module.
        /// </summary>
        /// <param name="ModuleID">The ID of the module to clear cached results for</param>
        /// <history>
        ///     [anurse]        6/20/2006   Created
        ///     [anurse]        6/21/2006   Documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static void ClearCachedResults(int ModuleID)
        {
            DataCache.RemoveCache(string.Concat(CACHEKEY_Reports, Convert.ToString(ModuleID)));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Executes the query and returns the results for test purposes
        /// </summary>
        /// <remarks>
        ///     Designed to allow the module to quickly run scripts without the
        ///     overhead of builting a report to wrap it
        /// </remarks>
        /// <param name="Query">The query to test</param>
        /// <param name="Parameters">The parameters to pass to the query</param>
        /// <history>
        ///     [anurse]        1/15/2007   Created
        /// </history>
        /// -----------------------------------------------------------------------------
        [Obsolete]
        public static DataTable TestQuery(string Query, DbParameter[] Parameters)
        {
            var dt = new DataTable("QueryResults");
            if (!string.IsNullOrEmpty(Query))
            {
                var dr = DataProvider.Instance().ExecuteSQL(Query, Parameters);
                dt.Load(dr);
            }
            return dt;
        }

        public static Dictionary<string, object> BuildParameters(PortalModuleBase SrcModule, ReportInfo Report)
        {
            var dict = new Dictionary<string, object>();

            if (SrcModule != null)
            {
                dict.Add("PortalID", SrcModule.PortalId);
                dict.Add("TabID", SrcModule.TabId);
                dict.Add("ModuleID", SrcModule.ModuleId);
                dict.Add("UserID", SrcModule.UserId);

                if (!string.IsNullOrEmpty(Report.Parameters.Trim()))
                {
                    var @params = Report.Parameters.Split(',');

                    foreach (var param in @params)
                    {
                        // Get the value from the request
                        var value = SrcModule.Request.Params[param];

                        // Add the parameter
                        dict.Add("url_" + param, value);
                    }
                }
            }

            return dict;
        }

        /// <summary>
        ///     Applies the specified converter to the specified input
        /// </summary>
        /// <param name="Input">The input data to convert</param>
        /// <param name="ConverterName">The converter to apply</param>
        /// <param name="Arguments">The arguments to pass to the converter</param>
        /// <returns>The converted value</returns>
        public static dynamic ApplyConverter(object Input, string ConverterName, string[] Arguments)
        {
            var InputString = Input as string;
            if (ReferenceEquals(InputString, null))
            {
                return Input;
            }

            switch (ConverterName.ToLower()) // Hardcoded for now
            {
                case "htmldecode":
                    return HttpUtility.HtmlDecode(InputString);
                case "htmlencode":
                    return HttpUtility.HtmlEncode(InputString);
                default:
                    return Input;
            }
        }

        /// <summary>
        ///     Sets the XSLT Visualizer Extension Objects for the specified tab module instance
        /// </summary>
        public static void SetXsltExtensionObjects(int tabModuleId, IEnumerable<ExtensionObjectInfo> extensionObjects)
        {
            DataProvider.Instance().SetXsltExtensionObjects(tabModuleId, extensionObjects);
        }

        public static IList<ExtensionObjectInfo> GetXsltExtensionObjects(int tabModuleId)
        {
            return CBO.FillCollection<ExtensionObjectInfo>(DataProvider
                                                               .Instance().GetXsltExtensionObjects(tabModuleId));
        }

        #endregion

        #region  Private Methods

        private static IDataSource GetDataSource(string type)
        {
            // Use the context to cache data source instances because they can be shared
            // among running modules, so in a single request there is no point in recreating
            // them
            var sKey = string.Format("Reports_DataSource:{0}", type);
            if (ReferenceEquals(HttpContext.Current, null) || !HttpContext.Current.Items.Contains(sKey))
            {
                // Load the Data Source
                try
                {
                    var dataSourceType = BuildManager.GetType(type, true);
                    var dataSource = Reflection.CreateInstance(dataSourceType) as IDataSource;

                    // Double check that HttpContext exists, because we might be here because it doesn exist
                    if (HttpContext.Current != null && dataSource != null)
                    {
                        HttpContext.Current.Items.Add(sKey, dataSource);
                    }

                    return dataSource;
                }
                catch (HttpException ex)
                {
                    throw new DataSourceException(new LocalizedText("DataSourceNotLoaded.Text"),
                                                  "Data Source could not be loaded", ex);
                }
            }
            return (IDataSource) HttpContext.Current.Items[sKey];
        }

        private static void LoadPrefixedSettings(IDictionary settingsTable, string prefix,
                                                 Dictionary<string, string> outputDictionary)
        {
            outputDictionary.Clear();

            // Load the module data source settings
            foreach (var key in settingsTable.Keys)
            {
                var sKey = (string) key;

                // If the setting starts with the current visualizer's prefix
                if (sKey.StartsWith(prefix))
                {
                    // Strip out the prefix and add it to the visualizer settings
                    outputDictionary.Add(sKey.Substring(prefix.Length),
                                         SettingsUtil.GetHashtableSetting(settingsTable, sKey, Null.NullString));
                }
            }
        }

        private static DirectoryInfo GetSubDirectory(DirectoryInfo BaseDirectory, string Name)
        {
            var dirs = BaseDirectory.GetDirectories(Name);
            if (dirs.Length < 1)
            {
                return null;
            }
            return dirs[0];
        }

        // Internal version of SaveReportDefinition to allow SaveReport to use
        // the same ModuleController instance for both method calls
        private static void UpdateReportDefinition(ModuleController ctrl, int ModuleId, ReportInfo objReport)
        {
            // Update the module settings with the data from the report
            ctrl.UpdateModuleSetting(ModuleId, SETTING_ReportTitle, objReport.Title);
            ctrl.UpdateModuleSetting(ModuleId, SETTING_ReportDescription, objReport.Description);
            ctrl.UpdateModuleSetting(ModuleId, SETTING_ReportParameters, objReport.Parameters);
            ctrl.UpdateModuleSetting(ModuleId, SETTING_DataSource, objReport.DataSource);
            ctrl.UpdateModuleSetting(ModuleId, SETTING_DataSourceClass, objReport.DataSourceClass);
            ctrl.UpdateModuleSetting(ModuleId, SETTING_ReportCreatedOn, objReport.CreatedOn.ToString());
            ctrl.UpdateModuleSetting(ModuleId, SETTING_ReportCreatedBy, objReport.CreatedBy.ToString());

            // Update data source settings
            // Can't do this in a common way because we must call a different method to
            // update Visualizer Settings and Data Source Settings
            if (!string.IsNullOrEmpty(objReport.DataSource))
            {
                var prefix = string.Format("{0}{1}_", PREFIX_DataSource, objReport.DataSource);
                foreach (var pair in objReport.DataSourceSettings)
                {
                    ctrl.UpdateModuleSetting(ModuleId, string.Concat(prefix, pair.Key), Convert.ToString(pair.Value));
                }
            }

            // Update Converter settigns
            var ConverterBuilder = new StringBuilder();
            foreach (var list in objReport.Converters.Values)
            {
                foreach (var Converter in list)
                {
                    ConverterBuilder.Append(Converter.FieldName);
                    ConverterBuilder.Append("|");
                    ConverterBuilder.Append(Converter.ConverterName);
                    if (Converter.Arguments != null && Converter.Arguments.Length > 0)
                    {
                        ConverterBuilder.Append("|");
                        ConverterBuilder.Append(string.Join(",", Converter.Arguments));
                    }
                    ConverterBuilder.Append(";");
                }
            }
            ctrl.UpdateModuleSetting(ModuleId, SETTING_Converters, ConverterBuilder.ToString());
        }

        // Internal version of SaveReportViewSettings to allow SaveReport to use
        // the same ModuleController instance for both method calls
        private static void UpdateReportView(ModuleController ctrl, int TabModuleId, ReportInfo objReport)
        {
            // Update the cache duration if it is specified
            if (!objReport.CacheDuration.Equals(Null.NullInteger))
            {
                ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_CacheDuration, objReport.CacheDuration.ToString());
            }

            ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_ShowInfoPane, objReport.ShowInfoPane.ToString());
            ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_ShowControls, objReport.ShowControls.ToString());
            ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_AutoRunReport, objReport.AutoRunReport.ToString());
            ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_TokenReplace, objReport.TokenReplace.ToString());

            // Update the visualizer setting
            ctrl.UpdateTabModuleSetting(TabModuleId, SETTING_Visualizer, objReport.Visualizer);

            // Update the Visualizer Settings
            // Can't do this in a common way because we must call a different method to
            // update Visualizer Settings and Data Source Settings
            if (!string.IsNullOrEmpty(objReport.Visualizer))
            {
                // Build the visualizer setting prefix (see GetReport)
                var strVisPrefix = string.Format("{0}{1}_", PREFIX_Visualizer, objReport.Visualizer);

                // For each visualizer setting
                foreach (var pair in objReport.VisualizerSettings)
                {
                    // Prepend the visualizer setting prefix and save to tab module settings
                    ctrl.UpdateTabModuleSetting(TabModuleId, string.Concat(strVisPrefix, pair.Key),
                                                Convert.ToString(pair.Value));
                }
            }
        }

        private static void WriteSettingsDictionary(StringBuilder xmlBuilder, Dictionary<string, string> dict)
        {
            foreach (var pair in dict)
            {
                xmlBuilder.AppendFormat("<setting key=\"{0}\"><![CDATA[{1}]]></setting>{2}", pair.Key, pair.Value,
                                        Environment.NewLine);
            }
        }

        private static Dictionary<string, string> ReadSettingsDictionary(XmlElement xmlElem)
        {
            var dict = new Dictionary<string, string>();
            foreach (XmlNode settingNode in xmlElem.SelectNodes("setting"))
            {
                if (settingNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                var settingElem = (XmlElement) settingNode;
                dict.Add(settingElem.GetAttribute("key"), settingElem.InnerText);
            }
            return dict;
        }

        #endregion

        #region  Optional Interfaces

        /// <summary>
        ///     Gets the modified search documents.
        /// </summary>
        /// <param name="moduleInfo">The module information.</param>
        /// <param name="beginDate">The begin date.</param>
        /// <returns></returns>
        public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo moduleInfo, DateTime beginDate)
        {
            // Get the report
            var objReport = GetReport(moduleInfo.ModuleID);

            var searchDocuments = new List<SearchDocument>();

            var searchDoc = new SearchDocument
                                {
                                    PortalId = moduleInfo.PortalID,
                                    AuthorUserId = objReport.CreatedBy,
                                    ModifiedTimeUtc = objReport.CreatedOn,
                                    ModuleId = moduleInfo.ModuleID,
                                    UniqueKey = string.Empty
                                };

            searchDocuments.Add(searchDoc);

            return searchDocuments;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {
            var objReport = GetReport(ModuleID);
            if (ReferenceEquals(objReport, null))
            {
                return string.Empty;
            }

            var xmlBuilder = new StringBuilder();
            xmlBuilder.AppendFormat("<title><![CDATA[{0}]]></title>{1}", objReport.Title, Environment.NewLine);
            xmlBuilder.AppendFormat("<description><![CDATA[{0}]]></description>{1}", objReport.Description,
                                    Environment.NewLine);
            xmlBuilder.AppendFormat("<converters>{0}", Environment.NewLine);
            foreach (var list in objReport.Converters.Values)
            {
                foreach (var converter in list)
                {
                    var args = string.Empty;
                    if (converter.Arguments != null)
                    {
                        args = string.Join(",", converter.Arguments);
                    }
                    xmlBuilder.AppendFormat("<converter name=\"{0}\" field=\"{1}\"><![CDATA[{2}]]></converter>{3}",
                                            converter.ConverterName, converter.FieldName, args, Environment.NewLine);
                }
            }
            xmlBuilder.AppendFormat("</converters>{0}", Environment.NewLine);

            xmlBuilder.AppendFormat("<datasource type=\"{0}\" class=\"{2}\">{1}", objReport.DataSource,
                                    Environment.NewLine, objReport.DataSourceClass);
            WriteSettingsDictionary(xmlBuilder, objReport.DataSourceSettings);
            xmlBuilder.AppendFormat("</datasource>");

            // Can't do this because Import/Export module does not have a TabModuleID
            //xmlBuilder.AppendFormat("<cacheDuration>{0}</cacheDuration>{1}", objReport.CacheDuration)
            //xmlBuilder.AppendFormat("<visualizer name=""{0}"">", objReport.Visualizer)
            //WriteSettingsDictionary(xmlBuilder, objReport.VisualizerSettings)
            //xmlBuilder.AppendFormat("</visualizer>")

            return xmlBuilder.ToString();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// <history>
        ///     [anurse]	06/16/2006	Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {
            // Check Access and Version
            var objUser = UserController.Instance.GetCurrentUserInfo();
            if (ReferenceEquals(objUser, null) || !objUser.IsSuperUser)
            {
                return;
            }

            var objNewReport = new ReportInfo();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(string.Format("<content>{0}</content>", Content));
            var xmlRoot = xmlDoc.DocumentElement;

            objNewReport.Title = XmlUtils.GetNodeValue(xmlRoot, "title", string.Empty);
            objNewReport.Description = XmlUtils.GetNodeValue(xmlRoot, "description", string.Empty);
            objNewReport.CreatedOn = DateTime.Now;
            objNewReport.CreatedBy = UserId;

            var ver = new Version(Version);
            if (ver.Major == 4)
            {
                var sQuery = XmlUtils.GetNodeValue(xmlRoot, "query", string.Empty);
                if (ver.Minor < 4)
                {
                    var queryBytes = Convert.FromBase64String(sQuery);
                    sQuery = Encoding.Default.GetString(queryBytes);
                }
                objNewReport.DataSource = "DotNetNuke.Modules.Reports.DataSources.DNNDataSource";
                objNewReport.DataSourceSettings.Add("Query", sQuery);
            }
            else
            {
                // Load converters
                foreach (XmlElement xmlElement in xmlRoot.SelectNodes("converters/converter"))
                {
                    var newConverter = new ConverterInstanceInfo();
                    newConverter.ConverterName = xmlElement.GetAttribute("name");
                    newConverter.FieldName = xmlElement.GetAttribute("field");
                    newConverter.Arguments = xmlElement.InnerText.Split(',');
                    ConverterUtils.AddConverter(objNewReport.Converters, newConverter);
                }

                var dsElement = (XmlElement) xmlRoot.SelectSingleNode("datasource");
                objNewReport.DataSource = dsElement.GetAttribute("type");
                objNewReport.DataSourceClass = dsElement.GetAttribute("class");
                objNewReport.DataSourceSettings = ReadSettingsDictionary(dsElement);
            }


            // Can't do this because Import/Export module does not have a TabModuleID
            //Dim visElement As XmlElement = DirectCast(xmlRoot.SelectSingleNode("visualizer"), XmlElement)
            //objNewReport.CacheDuration = XmlUtils.GetNodeValue(xmlRoot, "cacheDuration", String.Empty)
            //objNewReport.Visualizer = visElement.GetAttribute("name")
            //objNewReport.VisualizerSettings = ReadSettingsDictionary(visElement)

            UpdateReportDefinition(ModuleID, objNewReport);
            ClearCachedResults(ModuleID);
        }

        #endregion
    }
}