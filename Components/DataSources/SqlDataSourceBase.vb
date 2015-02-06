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

Imports System.Data
Imports DotNetNuke.Entities.Modules
Imports System.Data.Common
Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Modules.Reports.Converters

Namespace DotNetNuke.Modules.Reports.DataSources

    ''' <summary>
    ''' Base class for SQL-based Data Sources
    ''' </summary>
    Public MustInherit Class SqlDataSourceBase
        Inherits DataSourceBase

#Region " Private Fields "

        Private _currentReport As ReportInfo
        Private _hostModule As PortalModuleBase
        Private _parameters As IDictionary(Of String, Object)
        Private _converters As IDictionary(Of String, IList(Of ConverterInstanceInfo))

#End Region

#Region " Protected Properties "

        Protected ReadOnly Property CurrentReport() As ReportInfo
            Get
                Return _currentReport
            End Get
        End Property

        Protected ReadOnly Property HostModule() As PortalModuleBase
            Get
                Return _hostModule
            End Get
        End Property

        Protected ReadOnly Property Parameters() As IDictionary(Of String, Object)
            Get
                Return _parameters
            End Get
        End Property

        Protected ReadOnly Property Converters() As IDictionary(Of String, IList(Of ConverterInstanceInfo))
            Get
                Return _converters
            End Get
        End Property

#End Region

        Public Overrides Function ExecuteReport(ByVal report As ReportInfo, _
                                                ByVal hostModule As PortalModuleBase, _
                                                ByVal inputParameters As IDictionary(Of String, Object)) As DataView
            MyBase.ExecuteReport(report, hostModule, inputParameters)

            ' Load up the properties
            Me._currentReport = report
            Me._hostModule = hostModule
            Me._parameters = inputParameters
            Me._converters = Converters

            ' Validate Query
            If Not report.DataSourceSettings.ContainsKey(QueryKey) OrElse _
               String.IsNullOrEmpty(report.DataSourceSettings(QueryKey)) Then
                If hostModule Is Nothing Then Return New DataView(New DataTable("QueryResults")) ' Not hosted in a module, so fail silently
                If hostModule.UserInfo IsNot Nothing Then
                    If hostModule.UserInfo.IsSuperUser Then
                        Throw New DataSourceNotConfiguredException(New LocalizedText("HostNoQuery.Error"), _
                                                                   String.Format("There is no data source configured for this module, please configure one in the Module Settings page"))
                    ElseIf hostModule.IsEditable Then
                        Throw New DataSourceNotConfiguredException(New LocalizedText("AdminNoQuery.Error"), _
                                                                   String.Format("There is no data source configured for this module, please contact your Host for assistance"))
                    End If
                Else
                    Throw New DataSourceNotConfiguredException(New LocalizedText("AnonNoQuery.Error"))
                End If
                End If

            ' Execute the Query
            Return ExecuteSQLReport()
        End Function

        ''' <summary>
        ''' When overriden, executes the SQL-based report
        ''' </summary>
        ''' <returns>A <see cref="DataTable"/> containing the results</returns>
        ''' <remarks>
        ''' The report, host module and input parameters can be accessed through
        ''' the <see cref="CurrentReport" />, <see cref="HostModule" />,
        ''' <see cref="Parameters" /> properties.
        ''' </remarks>
        Protected MustOverride Function ExecuteSQLReport() As DataView

        ''' <summary>
        ''' Gets the key used to find the SQL Query in the Data Source Settings
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable ReadOnly Property QueryKey() As String
            Get
                Return ReportsController.SETTING_Query
            End Get
        End Property


        ''' <summary>
        ''' Validates that the specified setting exists AND has a non-empty value
        ''' </summary>
        ''' <param name="setting">The name of the setting to validate</param>
        ''' <exception cref="RequiredSettingMissingException">
        ''' The setting was not found
        ''' </exception>
        Protected Sub ValidateRequiredReportSetting(ByVal setting As String)
            If SettingsUtil.FieldIsNotSet(Me.CurrentReport.DataSourceSettings, _
                                          setting) Then
                Throw New RequiredSettingMissingException(setting, Me.ExtensionContext)
            End If
        End Sub

    End Class

End Namespace
