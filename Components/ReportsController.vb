'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports System
Imports System.Collections
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Configuration
Imports System.Configuration
Imports System.Data
Imports System.Data.Common
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Web
Imports System.Collections.Generic
Imports DotNetNuke.Modules.Reports.Data

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Search
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Modules.Reports.Converters
Imports DotNetNuke.Modules.Reports.Extensions
Imports DotNetNuke.Modules.Reports.DataSources
Imports DotNetNuke.Modules.Reports.Visualizers.Xslt

Imports System.Web.UI.WebControls.Expressions
Imports ReportsDataProvider = DotNetNuke.Modules.Reports.Data.DataProvider

Namespace DotNetNuke.Modules.Reports

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Controller class for Reports
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	06/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ReportsController
        Implements Entities.Modules.ISearchable
        Implements Entities.Modules.IPortable


#Region " Public Constants "

        ' App Settings
        Public Const APPSETTING_AllowCachingWithParameters As String = "Reports_AllowCachingAndParameters"

        ' Prefixes
        Public Const PREFIX_Reports As String = "dnn_Reports_"
        Public Const PREFIX_Visualizer As String = "dnn_ReportsVis_"
        Public Const PREFIX_DataSource As String = "dnn_ReportsDS_"

        ' Package Types
        Public Const PACKAGETYPE_Prefix As String = "DNN_Reports_"
        Public Const PACKAGETYPE_Visualizer As String = PACKAGETYPE_Prefix + "Visualizer"
        Public Const PACKAGETYPE_DataSource As String = PACKAGETYPE_Prefix + "DataSource"

        ' Reports Module Settings
        Public Const SETTING_ReportTitle As String = PREFIX_Reports + "Title"
        Public Const SETTING_ReportDescription As String = PREFIX_Reports + "Description"
        Public Const SETTING_ReportParameters As String = PREFIX_Reports + "Parameters"
        Public Const SETTING_ReportCreatedBy As String = PREFIX_Reports + "CreatedBy"
        Public Const SETTING_ReportCreatedOn As String = PREFIX_Reports + "CreatedOn"
        Public Const SETTING_CacheDuration As String = PREFIX_Reports + "CacheDuration"
        Public Const SETTING_DataSource As String = PREFIX_Reports + "DataSource"
        Public Const SETTING_DataSourceClass As String = PREFIX_Reports + "DataSourceClass"
        Public Const SETTING_Visualizer As String = PREFIX_Reports + "Visualizer"
        Public Const SETTING_Converters As String = PREFIX_Reports + "Converters"
        Public Const SETTING_ShowInfoPane As String = PREFIX_Reports + "ShowInfoPane"
        Public Const SETTING_ShowControls As String = PREFIX_Reports + "ShowControls"
        Public Const SETTING_AutoRunReport As String = PREFIX_Reports + "AutoRunReport"
        Public Const SETTING_TokenReplace As String = PREFIX_Reports + "TokenReplace"

        ' Data Source Settings

        ' Common
        Public Const SETTING_Query As String = "Query"
        Public Const SETTING_ConnectionString As String = "ConnectionString"
        Public Const SETTING_Server As String = "Server"
        Public Const SETTING_Database As String = "Database"
        Public Const SETTING_UserName As String = "UserName"
        Public Const SETTING_Password As String = "Password"
        Public Const SETTING_UseConnectionString As String = "UseConnectionString"

        ' Sql
        Public Const SETTING_Sql_UseIntegratedSecurity As String = "UseIntegratedSecurity"

        ' ADO
        Public Const SETTING_ADO_ProviderName As String = "ProviderName"
        Public Const SETTING_ADO_ParamPrefix As String = "ParamPrefix"

        ' Visualizer Settings

        ' Common
        Public Const SETTING_Height As String = "Height"
        Public Const SETTING_Width As String = "Width"

        ' Chart Visualizer
        Public Const SETTING_Chart_Type As String = "Type"
        Public Const SETTING_Chart_BarNameColumn As String = "BarNameColumn"
        Public Const SETTING_Chart_BarValueColumn As String = "BarValueColumn"
        Public Const SETTING_Chart_ColorMode As String = "ColorMode"
        Public Const SETTING_Chart_BarColorColumn As String = "BarColorColumn"
        Public Const SETTING_Chart_BarColor As String = "BarColor"
        Public Const SETTING_Chart_XAxisTitle As String = "XAxisTitle"
        Public Const SETTING_Chart_YAxisTitle As String = "YAxisTitle"

        ' Grid Visualizer
        Public Const SETTING_Grid_EnablePaging As String = "EnablePaging"
        Public Const SETTING_Grid_EnableSorting As String = "EnableSorting"
        Public Const SETTING_Grid_PageSize As String = "PageSize"
        Public Const SETTING_Grid_ShowHeader As String = "ShowHeader"
        Public Const SETTING_Grid_GridLines As String = "GridLines"
        Public Const SETTING_Grid_AdditionalCSS As String = "AdditionalCSS"
        Public Const SETTING_Grid_CSSClass As String = "CSSClass"
        Public Const DEFAULT_Grid_GridLines As String = "None"

        ' Reporting Services Visualizer
        Public Const SETTING_RS_Mode As String = "Mode"
        Public Const SETTING_RS_LocalReportFile As String = "LocalFile"
        Public Const SETTING_RS_ServerReportUrl As String = "ServerUrl"
        Public Const SETTING_RS_ServerReportPath As String = "ServerPath"
        Public Const SETTING_RS_DataSourceName As String = "DataSourceName"
        Public Const SETTING_RS_VisibleUIElements As String = "VisibleUI"
        Public Const SETTING_RS_EnableExternalImages As String = "EnExImg"
        Public Const SETTING_RS_EnableHyperlinks As String = "EnHyper"
        Public Const SETTING_RS_Domain As String = "Domain"
        Public Const DEFAULT_RS_VisibleUIElements As String = "DocumentMapButton,ExportControls,FindControls,NavigationControls,PrintButton,PromptAreaButton,RefreshButton,ToolBar,ZoomControl"

        ' HTML Visualizer
        Public Const SETTING_Html_TemplateFile As String = "TemplateFile"
        Public Const SETTING_Html_Repeat As String = "Repeat"
        Public Const SETTING_Html_Separator As String = "Separator"

        ' XSLT Visualizer
        Public Const SETTING_Xslt_TransformFile As String = "TransformFile"
        Public Const SETTING_Xslt_ExtensionObject As String = "Ext"

        Public Const CACHEKEY_Reports As String = "dnn_ReportCache"

#End Region

#Region " Public Methods "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the report associated with a reports module without retrieving
        ''' tab module specific settings (visualizers)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleId">The Id of the module to retrive the report for</param>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' 	[anurse]	01/15/2007	Updated to use GetReport(Integer, Integer) override
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetReport(ByVal ModuleId As Integer) As ReportInfo
            Return GetReport(ModuleId, Null.NullInteger)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the report associated with a reports module (including tab module
        ''' specific settings)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleId">The Id of the module</param>
        ''' <param name="TabModuleId">
        ''' The Id of the instance ("TabModule") of the module that the report is running on
        ''' </param>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created from GetReport(Integer)
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetReport(ByVal ModuleId As Integer, ByVal TabModuleId As Integer) As ReportInfo

            ' Check for a null Module Id
            If ModuleId = Null.NullInteger Then Return Nothing

            ' Extract the Title, Description and Query from the settings
            Dim objModuleController As New ModuleController
            Dim objSettings As Hashtable = _
                objModuleController.GetModuleSettings(ModuleId)
            ' Check that the settings hashtable was retrieved
            If objSettings Is Nothing Then Return Nothing

            ' Setup the Report
            Dim objReport As New ReportInfo()

            ' Build the Report from the Module Settings
            objReport.Title = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_ReportTitle, Null.NullString)
            objReport.Description = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_ReportDescription, Null.NullString)
            objReport.Parameters = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_ReportParameters, Null.NullString)
            objReport.CreatedOn = SettingsUtil.GetHashtableSetting(Of Date)(objSettings, ReportsController.SETTING_ReportCreatedOn, Null.NullDate)
            objReport.CreatedBy = SettingsUtil.GetHashtableSetting(Of Integer)(objSettings, ReportsController.SETTING_ReportCreatedBy, Null.NullInteger)
            objReport.DataSource = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_DataSource, Null.NullString)
            objReport.DataSourceClass = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_DataSourceClass, Null.NullString)
            objReport.ModuleID = ModuleId

            ' Load Filter Settings
            Dim converterString As String = SettingsUtil.GetHashtableSetting(Of String)(objSettings, ReportsController.SETTING_Converters, Null.NullString)
            If Not String.IsNullOrEmpty(converterString.Trim()) Then
                For Each converterItem As String In converterString.Split(";"c)
                    If Not String.IsNullOrEmpty(converterItem.Trim()) Then
                        Dim converterArray As String() = converterItem.Split("|"c)
                        If converterArray.Length >= 2 AndAlso converterArray.Length <= 3 Then
                            Dim newconverter As New ConverterInstanceInfo()
                            newconverter.FieldName = converterArray(0)
                            newconverter.ConverterName = converterArray(1)
                            If converterArray.Length = 3 Then
                                newconverter.Arguments = converterArray(2).Split(","c)
                            End If

                            ConverterUtils.AddConverter(objReport.Converters, newconverter)
                        End If
                    End If
                Next
            End If

            ' Load the tab module settings (visualizer settings) if we have a tab module Id
            Dim objTabModuleSettings As Hashtable = Nothing
            If TabModuleId <> Null.NullInteger Then

                objTabModuleSettings = objModuleController.GetTabModuleSettings(TabModuleId)

                objReport.ShowControls = SettingsUtil.GetHashtableSetting(Of Boolean)(objTabModuleSettings, ReportsController.SETTING_ShowControls, False)
                objReport.ShowInfoPane = SettingsUtil.GetHashtableSetting(Of Boolean)(objTabModuleSettings, ReportsController.SETTING_ShowInfoPane, False)
                objReport.AutoRunReport = SettingsUtil.GetHashtableSetting(Of Boolean)(objTabModuleSettings, ReportsController.SETTING_AutoRunReport, True)
                objReport.TokenReplace = SettingsUtil.GetHashtableSetting(Of Boolean)(objTabModuleSettings, ReportsController.SETTING_TokenReplace, False)

                ' Read the visualizer name and cache duration
                objReport.Visualizer = SettingsUtil.GetHashtableSetting(Of String)(objTabModuleSettings, ReportsController.SETTING_Visualizer, "Grid")
                objReport.CacheDuration = SettingsUtil.GetHashtableSetting(Of Integer)(objTabModuleSettings, ReportsController.SETTING_CacheDuration, Null.NullInteger)

            End If

            LoadExtensionSettings(objSettings, objTabModuleSettings, objReport)

            Return objReport

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Saves the report associated with a reports module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleId">The ModuleId to set the report for</param>
        ''' <param name="TabModuleId">
        ''' The Id of the instance ("TabModule") of the module to store the visualizer
        ''' settings with
        ''' </param>
        ''' <param name="objReport">The ReportInfo object to save</param>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' 	[anurse]	01/15/2007	Refactored, creating SaveReportDefinition and
        '''                             SaveReportViewSettings.
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub UpdateReport(ByVal ModuleId As Integer, ByVal TabModuleId As Integer, _
            ByVal objReport As ReportInfo)

            Dim objModuleController As New ModuleController
            UpdateReportDefinition(objModuleController, ModuleId, objReport)
            UpdateReportView(objModuleController, TabModuleId, objReport)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Saves the report definition (module settings) associated with a reports module
        ''' </summary>
        ''' <param name="ModuleId">The ModuleId to set the report for</param>
        ''' <param name="objReport">The ReportInfo object to save</param>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub UpdateReportDefinition(ByVal ModuleId As Integer, ByVal objReport As ReportInfo)
            Dim ctrl As New ModuleController()
            UpdateReportDefinition(ctrl, ModuleId, objReport)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Saves the report view (tab module settings) associated with a reports module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="TabModuleId">
        ''' The Id of the instance ("TabModule") of the module to store the view
        ''' settings for.
        ''' </param>
        ''' <param name="objReport">The ReportInfo object to save</param>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub UpdateReportView(ByVal TabModuleId As Integer, ByVal objReport As ReportInfo)
            Dim ctrl As New ModuleController()
            UpdateReportView(ctrl, TabModuleId, objReport)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Executes a report and returns the results
        ''' </summary>
        ''' <param name="objReport">The ReportInfo object</param>
        ''' <param name="cacheKey">The cache key for the module</param>
        ''' <param name="bypassCache">A boolean indicating that the cache should be bypassed</param>
        ''' <param name="hostModule">The module that is hosting the report</param>
        ''' <exception cref="System.ArgumentNullException">
        ''' The value of <paramref name="objReport"/> was null (Nothing in Visual Basic)
        ''' </exception>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function ExecuteReport(ByVal objReport As ReportInfo, ByVal cacheKey As String, ByVal bypassCache As Boolean, ByVal hostModule As PortalModuleBase) As DataTable
            Dim fromCache As Boolean = False
            Return ExecuteReport(objReport, cacheKey, bypassCache, hostModule, fromCache)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Executes a report and returns the results
        ''' </summary>
        ''' <param name="objReport">The ReportInfo object</param>
        ''' <param name="cacheKey">The cache key for the module</param>
        ''' <param name="bypassCache">A boolean indicating that the cache should be bypassed</param>
        ''' <param name="hostModule">The module that is hosting the report</param>
        ''' <param name="fromCache">A boolean indicating if the data was retrieved from the cache</param>
        ''' <exception cref="System.ArgumentNullException">
        ''' The value of <paramref name="objReport"/> was null (Nothing in Visual Basic)
        ''' </exception>
        ''' <history>
        ''' 	[anurse]	08/25/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function ExecuteReport(ByVal objReport As ReportInfo, ByVal cacheKey As String, ByVal bypassCache As Boolean, ByVal hostModule As PortalModuleBase, ByRef fromCache As Boolean) As DataTable
            Try
                fromCache = False
                If objReport Is Nothing Then Throw New ArgumentNullException("objReport")

                If String.IsNullOrEmpty(objReport.DataSource) Then
                    Return Nothing ' If there's no data source, there's no report
                Else
                    ' Check the cache
                    Dim results As DataTable = Nothing
                    If Not bypassCache Then
                        results = TryCast(DataCache.GetCache(cacheKey), DataTable)
                        If results IsNot Nothing Then
                            fromCache = True
                            Return results
                        End If
                    End If

                    ' Get an instance of the data source
                    Dim dataSource As IDataSource = GetDataSource(objReport.DataSourceClass)

                    ' Create an extension context
                    Dim moduleFolder As String = String.Empty
                    If hostModule IsNot Nothing Then
                        moduleFolder = hostModule.TemplateSourceDirectory
                    End If
                    Dim ctxt As New ExtensionContext(moduleFolder, "DataSource", objReport.DataSource)

                    ' Load Parameters
                    Dim parameters As IDictionary(Of String, Object) = ReportsController.BuildParameters(hostModule, objReport)
                    If objReport.CacheDuration <> 0 Then
                        ' Check the app setting
                        If Not "True".Equals(WebConfigurationManager.AppSettings(APPSETTING_AllowCachingWithParameters), StringComparison.InvariantCulture) Then
                            parameters.Clear()
                        End If
                    End If

                    ' Execute the report
                    dataSource.Initialize(ctxt)
                    results = dataSource.ExecuteReport(objReport, hostModule, parameters).ToTable()
                    results.TableName = "QueryResults"

                    ' Check if the converters were run
                    If Not dataSource.CanProcessConverters Then
                        ' If not, run them the slow way :(
                        For Each row As DataRow In results.Rows
                            For Each col As DataColumn In results.Columns
                                If Not objReport.Converters.ContainsKey(col.ColumnName) Then
                                    Continue For
                                End If
                                Dim list As IList(Of ConverterInstanceInfo) = objReport.Converters(col.ColumnName)
                                For Each converter As ConverterInstanceInfo In list
                                    row.Item(col) = ApplyConverter(row.Item(col), converter.ConverterName, converter.Arguments)
                                Next
                            Next
                        Next
                    End If

                    ' Do the token replace if specified
                    If objReport.TokenReplace Then

                        Dim localTokenReplacer As New Services.Tokens.TokenReplace
                        For Each col As DataColumn In results.Columns

                            ' Process each column of the resultset
                            If col.DataType = GetType(System.String) Then

                                ' We want to replace the data, we don't mind that it is marked as readonly
                                If col.ReadOnly Then
                                    col.ReadOnly = False
                                End If

                                Dim maxLength As Integer = col.MaxLength
                                Dim resultText As String

                                For Each row As DataRow In results.Rows
                                    resultText = localTokenReplacer.ReplaceEnvironmentTokens(row(col).ToString())

                                    ' Don't make it too long
                                    If resultText.Length > maxLength Then
                                        resultText = resultText.Substring(0, maxLength)
                                    End If

                                    row(col) = resultText
                                Next

                            End If

                        Next

                    End If


                    ' Cache it, if not asked to bypass the cache
                    If Not bypassCache AndAlso results IsNot Nothing Then
                        DataCache.SetCache(cacheKey, results)
                    End If

                    ' Return the results
                    Return results
                End If
            Catch dsx As DataSourceException
                Throw
                'Catch ex As Exception
                '    If hostModule IsNot Nothing Then
                '        Throw New DataSourceException("UnknownError.Text", hostModule.LocalResourceFile, ex, ex.Message)
                '    Else
                '        Throw
                '    End If
            End Try
        End Function

        '''-----------------------------------------------------------------------------
        ''' <summary>
        ''' Removes "Bad" SQL Commands from the specified string
        ''' </summary>
        ''' <param name="strSQL">This is the string to be filtered</param>
        ''' <returns>A filtered version of <paramref name="strSQL" /> with commands such as INSERT or DELETE removed</returns>
        ''' <history>
        '''     [anurse]        6/20/2006   Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        Public Shared Function FormatRemoveSQL(ByVal strSQL As String) As String

            Dim strCleanSQL As String = strSQL

            If strSQL <> Nothing Then

                ' each string in this array is one that must be removed from the SQL
                Dim BadSQL As String() = New String() {";", "--", "create ", "drop ", "insert ", "delete ", "update ", "sp_", "xp_"}

                ' strip any dangerous SQL commands
                Dim intCommand As Integer
                For intCommand = 0 To BadSQL.Length - 1
                    ' remove the current item in the "Bad SQL" list from the string by replacing it with a space
                    strCleanSQL = Regex.Replace(strCleanSQL, Convert.ToString(BadSQL.GetValue(intCommand)), " ", _
                        RegexOptions.IgnoreCase)
                Next
            End If

            ' return the clean SQL
            Return strCleanSQL

        End Function

        '''-----------------------------------------------------------------------------
        ''' <summary>
        ''' Clears the results that have been cached by the module.
        ''' </summary>
        ''' <param name="ModuleID">The ID of the module to clear cached results for</param>
        ''' <history>
        '''     [anurse]        6/20/2006   Created
        '''     [anurse]        6/21/2006   Documented
        ''' </history>
        '''-----------------------------------------------------------------------------
        Public Shared Sub ClearCachedResults(ByVal ModuleID As Integer)
            DataCache.RemoveCache(String.Concat(CACHEKEY_Reports, ModuleID))
        End Sub

        '''-----------------------------------------------------------------------------
        ''' <summary>
        ''' Executes the query and returns the results for test purposes
        ''' </summary>
        ''' <remarks>
        ''' Designed to allow the module to quickly run scripts without the 
        ''' overhead of builting a report to wrap it
        ''' </remarks>
        ''' <param name="Query">The query to test</param>
        ''' <param name="Parameters">The parameters to pass to the query</param>
        ''' <history>
        '''     [anurse]        1/15/2007   Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        <Obsolete()> _
        Public Shared Function TestQuery(ByVal Query As String, ByVal Parameters As DbParameter()) As DataTable
            Dim dt As New DataTable("QueryResults")
            If Not String.IsNullOrEmpty(Query) Then
                Dim dr As IDataReader = ReportsDataProvider.Instance().ExecuteSQL(Query, Parameters)
                dt.Load(dr)
            End If
            Return dt
        End Function

        Public Shared Function BuildParameters(ByVal SrcModule As PortalModuleBase, ByVal Report As ReportInfo) As Dictionary(Of String, Object)
            Dim dict As New Dictionary(Of String, Object)

            If SrcModule IsNot Nothing Then

                dict.Add("PortalID", SrcModule.PortalId)
                dict.Add("TabID", SrcModule.TabId)
                dict.Add("ModuleID", SrcModule.ModuleId)
                dict.Add("UserID", SrcModule.UserId)

                If Not String.IsNullOrEmpty(Report.Parameters.Trim()) Then

                    Dim params As String() = Report.Parameters.Split(","c)

                    For Each param As String In params
                        ' Get the value from the request
                        Dim value As String = SrcModule.Request.Params(param)

                        ' Add the parameter
                        dict.Add("url_" + param, value)
                    Next

                End If

            End If

            Return dict

        End Function

        ''' <summary>
        ''' Applies the specified converter to the specified input
        ''' </summary>
        ''' <param name="Input">The input data to convert</param>
        ''' <param name="ConverterName">The converter to apply</param>
        ''' <param name="Arguments">The arguments to pass to the converter</param>
        ''' <returns>The converted value</returns>
        Public Shared Function ApplyConverter(ByVal Input As Object, ByVal ConverterName As String, ByVal Arguments As String()) As Object
            Dim InputString As String = TryCast(Input, String)
            If InputString Is Nothing Then Return Input

            Select Case ConverterName.ToLower() ' Hardcoded for now
                Case "htmldecode"
                    Return HttpUtility.HtmlDecode(InputString)
                Case "htmlencode"
                    Return HttpUtility.HtmlEncode(InputString)
                Case Else
                    Return Input
            End Select
        End Function

        ''' <summary>
        ''' Sets the XSLT Visualizer Extension Objects for the specified tab module instance
        ''' </summary>
        Public Shared Sub SetXsltExtensionObjects(ByVal tabModuleId As Integer, ByVal extensionObjects As IEnumerable(Of ExtensionObjectInfo))
            ReportsDataProvider.Instance().SetXsltExtensionObjects(tabModuleId, extensionObjects)
        End Sub

        Public Shared Function GetXsltExtensionObjects(ByVal tabModuleId As Integer) As IList(Of ExtensionObjectInfo)
            Return CBO.FillCollection(Of ExtensionObjectInfo)(DataProvider.Instance().GetXsltExtensionObjects(tabModuleId))
        End Function


#End Region

#Region " Friend Methods "

        Friend Shared Sub LoadExtensionSettings(ByVal ModuleSettings As Hashtable, ByVal TabModuleSettings As Hashtable, ByVal Report As ReportInfo)
            ' Load Data Source Settings
            If Not String.IsNullOrEmpty(Report.DataSource) Then
                LoadPrefixedSettings(ModuleSettings, _
                                 String.Format("{0}{1}_", PREFIX_DataSource, Report.DataSource), _
                                 Report.DataSourceSettings)
            End If

            If TabModuleSettings IsNot Nothing Then
                ' Load Visualizer Settings
                LoadPrefixedSettings(TabModuleSettings, _
                                         String.Format("{0}{1}_", PREFIX_Visualizer, Report.Visualizer), _
                                         Report.VisualizerSettings)
            End If
        End Sub

#End Region

#Region " Private Methods "

        Private Shared Function GetDataSource(ByVal type As String) As IDataSource
            ' Use the context to cache data source instances because they can be shared
            ' among running modules, so in a single request there is no point in recreating
            ' them
            Dim sKey As String = String.Format("Reports_DataSource:{0}", type)
            If HttpContext.Current Is Nothing OrElse Not HttpContext.Current.Items.Contains(sKey) Then
                ' Load the Data Source
                Try
                    Dim dataSourceType As Type = System.Web.Compilation.BuildManager.GetType(type, True)
                    Dim dataSource As IDataSource = TryCast(Framework.Reflection.CreateInstance(dataSourceType),  _
                                             IDataSource)

                    ' Double check that HttpContext exists, because we might be here because it doesn exist
                    If HttpContext.Current IsNot Nothing AndAlso dataSource IsNot Nothing Then
                        HttpContext.Current.Items.Add(sKey, dataSource)
                    End If

                    Return dataSource
                Catch ex As HttpException
                    Throw New DataSourceException(New LocalizedText("DataSourceNotLoaded.Text"), _
                                                  "Data Source could not be loaded", ex)
                End Try
            Else
                Return DirectCast(HttpContext.Current.Items(sKey), IDataSource)
            End If
        End Function

        Private Shared Sub LoadPrefixedSettings(ByVal settingsTable As IDictionary, ByVal prefix As String, ByVal outputDictionary As Dictionary(Of String, String))
            outputDictionary.Clear()

            ' Load the module data source settings
            For Each key As Object In settingsTable.Keys
                Dim sKey As String = DirectCast(key, String)

                ' If the setting starts with the current visualizer's prefix
                If sKey.StartsWith(prefix) Then

                    ' Strip out the prefix and add it to the visualizer settings
                    outputDictionary.Add( _
                        sKey.Substring(prefix.Length), _
                        SettingsUtil.GetHashtableSetting(Of String)(settingsTable, sKey, Null.NullString))
                End If
            Next
        End Sub

        Private Shared Function GetSubDirectory(ByVal BaseDirectory As DirectoryInfo, ByVal Name As String) As DirectoryInfo
            Dim dirs As DirectoryInfo() = BaseDirectory.GetDirectories(Name)
            If dirs.Length < 1 Then
                Return Nothing
            End If
            Return dirs(0)
        End Function

        ' Internal version of SaveReportDefinition to allow SaveReport to use
        ' the same ModuleController instance for both method calls
        Private Shared Sub UpdateReportDefinition(ByVal ctrl As ModuleController, ByVal ModuleId As Integer, ByVal objReport As ReportInfo)
            ' Update the module settings with the data from the report
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_ReportTitle, objReport.Title)
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_ReportDescription, objReport.Description)
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_ReportParameters, objReport.Parameters)
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_DataSource, objReport.DataSource)
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_DataSourceClass, objReport.DataSourceClass)
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_ReportCreatedOn, objReport.CreatedOn.ToString())
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_ReportCreatedBy, objReport.CreatedBy.ToString())

            ' Update data source settings
            ' Can't do this in a common way because we must call a different method to
            ' update Visualizer Settings and Data Source Settings
            If Not String.IsNullOrEmpty(objReport.DataSource) Then

                Dim prefix As String = String.Format("{0}{1}_", PREFIX_DataSource, objReport.DataSource)
                For Each pair As KeyValuePair(Of String, String) In objReport.DataSourceSettings
                    ctrl.UpdateModuleSetting(ModuleId, _
                                             String.Concat(prefix, pair.Key), _
                                             pair.Value)
                Next

            End If

            ' Update Converter settigns
            Dim ConverterBuilder As New StringBuilder
            For Each list As IList(Of ConverterInstanceInfo) In objReport.Converters.Values
                For Each Converter As ConverterInstanceInfo In list
                    ConverterBuilder.Append(Converter.FieldName)
                    ConverterBuilder.Append("|")
                    ConverterBuilder.Append(Converter.ConverterName)
                    If Converter.Arguments IsNot Nothing AndAlso Converter.Arguments.Length > 0 Then
                        ConverterBuilder.Append("|")
                        ConverterBuilder.Append(String.Join(",", Converter.Arguments))
                    End If
                    ConverterBuilder.Append(";")
                Next
            Next
            ctrl.UpdateModuleSetting(ModuleId, ReportsController.SETTING_Converters, ConverterBuilder.ToString())
        End Sub

        ' Internal version of SaveReportViewSettings to allow SaveReport to use
        ' the same ModuleController instance for both method calls
        Private Shared Sub UpdateReportView(ByVal ctrl As ModuleController, ByVal TabModuleId As Integer, ByVal objReport As ReportInfo)
            ' Update the cache duration if it is specified
            If Not objReport.CacheDuration.Equals(Null.NullInteger) Then
                ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_CacheDuration, objReport.CacheDuration.ToString())
            End If

            ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_ShowInfoPane, objReport.ShowInfoPane.ToString())
            ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_ShowControls, objReport.ShowControls.ToString())
            ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_AutoRunReport, objReport.AutoRunReport.ToString())
            ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_TokenReplace, objReport.TokenReplace.ToString())

            ' Update the visualizer setting
            ctrl.UpdateTabModuleSetting(TabModuleId, ReportsController.SETTING_Visualizer, objReport.Visualizer)

            ' Update the Visualizer Settings
            ' Can't do this in a common way because we must call a different method to
            ' update Visualizer Settings and Data Source Settings
            If Not String.IsNullOrEmpty(objReport.Visualizer) Then

                ' Build the visualizer setting prefix (see GetReport)
                Dim strVisPrefix = String.Format("{0}{1}_", PREFIX_Visualizer, objReport.Visualizer)

                ' For each visualizer setting
                For Each pair As KeyValuePair(Of String, String) In objReport.VisualizerSettings

                    ' Prepend the visualizer setting prefix and save to tab module settings
                    ctrl.UpdateTabModuleSetting(TabModuleId, _
                        String.Concat(strVisPrefix, pair.Key), pair.Value)
                Next
            End If
        End Sub

        Private Shared Sub WriteSettingsDictionary(ByVal xmlBuilder As StringBuilder, ByVal dict As Dictionary(Of String, String))
            For Each pair As KeyValuePair(Of String, String) In dict
                xmlBuilder.AppendFormat("<setting key=""{0}""><![CDATA[{1}]]></setting>{2}", pair.Key, pair.Value, Environment.NewLine)
            Next
        End Sub

        Private Shared Function ReadSettingsDictionary(ByVal xmlElem As XmlElement) As Dictionary(Of String, String)
            Dim dict As New Dictionary(Of String, String)()
            For Each settingNode As XmlNode In xmlElem.SelectNodes("setting")
                If settingNode.NodeType <> XmlNodeType.Element Then Continue For
                Dim settingElem As XmlElement = DirectCast(settingNode, XmlElement)
                dict.Add(settingElem.GetAttribute("key"), settingElem.InnerText)
            Next
            Return dict
        End Function

#End Region

#Region " Optional Interfaces "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetSearchItems implements the ISearchable Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements DotNetNuke.Entities.Modules.ISearchable.GetSearchItems
            ' Get the report
            Dim objReport As ReportInfo = GetReport(ModInfo.ModuleID)

            ' Execute the report and serialize it to Xml
            Dim objTable As DataTable = ExecuteReport(objReport, String.Empty, True, Nothing)

            ' Each row will be a search item
            Dim objSearchItems As New SearchItemInfoCollection()
            For Each row As DataRow In objTable.Rows
                ' Build the content string
                Dim contentBuilder As New StringBuilder()
                For Each col As DataColumn In objTable.Columns
                    contentBuilder.Append(col.ColumnName)
                    contentBuilder.Append(": ")
                    contentBuilder.AppendLine(row(col).ToString())
                Next

                ' Get the title
                Dim title As String = objReport.Title
                If String.IsNullOrEmpty(title) Then
                    title = ModInfo.ModuleTitle
                End If

                ' Build a search item
                Dim objPortalSec As New PortalSecurity
                Dim objSearchItem As New SearchItemInfo(title, _
                    objPortalSec.InputFilter(objReport.Description, PortalSecurity.FilterFlag.NoMarkup), objReport.CreatedBy, _
                    objReport.CreatedOn, ModInfo.ModuleID, String.Empty, contentBuilder.ToString())

                ' Add it to the collection
                objSearchItems.Add(objSearchItem)
            Next

            Return objSearchItems
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ExportModule implements the IPortable ExportModule Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleID">The Id of the module to be exported</param>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim objReport As ReportInfo = GetReport(ModuleID)
            If objReport Is Nothing Then
                Return String.Empty
            End If

            Dim xmlBuilder As New StringBuilder
            xmlBuilder.AppendFormat("<title><![CDATA[{0}]]></title>{1}", objReport.Title, Environment.NewLine)
            xmlBuilder.AppendFormat("<description><![CDATA[{0}]]></description>{1}", objReport.Description, Environment.NewLine)
            xmlBuilder.AppendFormat("<converters>{0}", Environment.NewLine)
            For Each list As IList(Of ConverterInstanceInfo) In objReport.Converters.Values
                For Each converter As ConverterInstanceInfo In list
                    Dim args As String = String.Empty
                    If converter.Arguments IsNot Nothing Then
                        args = String.Join(",", converter.Arguments)
                    End If
                    xmlBuilder.AppendFormat("<converter name=""{0}"" field=""{1}""><![CDATA[{2}]]></converter>{3}", converter.ConverterName, converter.FieldName, args, Environment.NewLine)
                Next
            Next
            xmlBuilder.AppendFormat("</converters>{0}", Environment.NewLine)

            xmlBuilder.AppendFormat("<datasource type=""{0}"" class=""{2}"">{1}", objReport.DataSource, Environment.NewLine, objReport.DataSourceClass)
            WriteSettingsDictionary(xmlBuilder, objReport.DataSourceSettings)
            xmlBuilder.AppendFormat("</datasource>")

            ' Can't do this because Import/Export module does not have a TabModuleID
            'xmlBuilder.AppendFormat("<cacheDuration>{0}</cacheDuration>{1}", objReport.CacheDuration)
            'xmlBuilder.AppendFormat("<visualizer name=""{0}"">", objReport.Visualizer)
            'WriteSettingsDictionary(xmlBuilder, objReport.VisualizerSettings)
            'xmlBuilder.AppendFormat("</visualizer>")

            Return xmlBuilder.ToString()
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ImportModule implements the IPortable ImportModule Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleID">The Id of the module to be imported</param>
        ''' <param name="Content">The content to be imported</param>
        ''' <param name="Version">The version of the module to be imported</param>
        ''' <param name="UserId">The Id of the user performing the import</param>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule

            ' Check Access and Version
            Dim objUser As UserInfo = UserController.GetUser(-1, UserId, False)
            If objUser Is Nothing OrElse Not objUser.IsSuperUser Then Return

            Dim objNewReport As New ReportInfo
            Dim xmlDoc As New XmlDocument
            xmlDoc.LoadXml(String.Format("<content>{0}</content>", Content))
            Dim xmlRoot As XmlElement = xmlDoc.DocumentElement

            objNewReport.Title = XmlUtils.GetNodeValue(xmlRoot, "title", String.Empty)
            objNewReport.Description = XmlUtils.GetNodeValue(xmlRoot, "description", String.Empty)
            objNewReport.CreatedOn = DateTime.Now
            objNewReport.CreatedBy = UserId

            Dim ver As New Version(Version)
            If ver.Major = 4 Then
                Dim sQuery As String = XmlUtils.GetNodeValue(xmlRoot, "query", String.Empty)
                If ver.Minor < 4 Then
                    Dim queryBytes As Byte() = Convert.FromBase64String(sQuery)
                    sQuery = Encoding.Default.GetString(queryBytes)
                End If
                objNewReport.DataSource = "DotNetNuke.Modules.Reports.DataSources.DNNDataSource"
                objNewReport.DataSourceSettings.Add("Query", sQuery)
            Else
                ' Load converters
                For Each xmlElement As XmlElement In xmlRoot.SelectNodes("converters/converter")
                    Dim newConverter As New ConverterInstanceInfo
                    newConverter.ConverterName = xmlElement.GetAttribute("name")
                    newConverter.FieldName = xmlElement.GetAttribute("field")
                    newConverter.Arguments = xmlElement.InnerText.Split(","c)
                    ConverterUtils.AddConverter(objNewReport.Converters, newConverter)
                Next

                Dim dsElement As XmlElement = DirectCast(xmlRoot.SelectSingleNode("datasource"), XmlElement)
                objNewReport.DataSource = dsElement.GetAttribute("type")
                objNewReport.DataSourceClass = dsElement.GetAttribute("class")
                objNewReport.DataSourceSettings = ReadSettingsDictionary(dsElement)
            End If


            ' Can't do this because Import/Export module does not have a TabModuleID
            'Dim visElement As XmlElement = DirectCast(xmlRoot.SelectSingleNode("visualizer"), XmlElement)
            'objNewReport.CacheDuration = XmlUtils.GetNodeValue(xmlRoot, "cacheDuration", String.Empty)
            'objNewReport.Visualizer = visElement.GetAttribute("name")
            'objNewReport.VisualizerSettings = ReadSettingsDictionary(visElement)

            UpdateReportDefinition(ModuleID, objNewReport)
            ClearCachedResults(ModuleID)
        End Sub

#End Region
    End Class
End Namespace
