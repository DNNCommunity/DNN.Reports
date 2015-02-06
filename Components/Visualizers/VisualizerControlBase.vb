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
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports UISkin = DotNetNuke.UI.Skins.Skin
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.Visualizers

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The VisualizerControlBase class provides a base class for Visualizer Controls
    ''' </summary>
    ''' <remarks>
    ''' Visualizer Controls are the component of the visualizer that actually renders the report data.
    ''' </remarks>
    ''' <history>
    '''     [anurse]     08/31/2006    Documented
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class VisualizerControlBase
        Inherits ReportsControlBase
        Implements IVisualizerControl

#Region " Public Constants "

        Public Const FILENAME_VisualizerASCX As String = "Visualizer.ascx"

#End Region

#Region " Private Fields "

        Private _reportResults As DataTable
        Private _report As ReportInfo
        Private _fromCache As Boolean
        Private _isFirstRun As Boolean = False

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets the results of the report, or a null reference (Nothing in VB.Net)
        ''' if the report has not been executed
        ''' </summary>
        Public ReadOnly Property ReportResults() As DataTable
            Get
                Return _reportResults
            End Get
        End Property

        ''' <summary>
        ''' Gets the object representing the report settings
        ''' </summary>
        Public ReadOnly Property Report() As ReportInfo
            Get
                Return _report
            End Get
        End Property

        ''' <summary>
        ''' Gets a boolean indicating if the results are from the cache
        ''' </summary>
        Public ReadOnly Property FromCache() As Boolean
            Get
                Return _fromCache
            End Get
        End Property

        ''' <summary>
        ''' Gets a boolean indicating if the report should be executed automatically and
        ''' provided to the visualizer
        ''' </summary>
        Public Overridable ReadOnly Property AutoExecuteReport() As Boolean Implements IVisualizerControl.AutoExecuteReport
            Get
                Return True
            End Get
        End Property

        Protected Overrides ReadOnly Property ASCXFileName() As String
            Get
                Return FILENAME_VisualizerASCX
            End Get
        End Property

        ''' <summary>
        ''' Gets a boolean indicating if the current request is the first one
        ''' since the report was set
        ''' </summary>
        Protected ReadOnly Property IsFirstRun() As Boolean
            Get
                Return _isFirstRun
            End Get
        End Property

#End Region

#Region " Methods "

        Protected Sub LoadReport()
            Me._report = ReportsController.GetReport(Me.ModuleId, Me.TabModuleId)
        End Sub

        ''' <summary>
        ''' Executes the report, suppressing any exception thrown by the data source and
        ''' displaying any errors that occur
        ''' </summary>
        ''' <remarks>The <see cref="ReportResults"/> property contains the data table resulting from executing the report</remarks>
        Protected Sub ExecuteReport()
            ExecuteReport(False, True)
        End Sub

        ''' <summary>
        ''' Executes the report, displaying any errors that occur and
        ''' optionally suppressing any exception thrown by the data source.
        ''' </summary>
        ''' <param name="ThrowOnError">A boolean indicating if exceptions thrown by the data source should be suppressed</param>
        ''' <remarks>The <see cref="ReportResults"/> property contains the data table resulting from executing the report</remarks>
        Protected Sub ExecuteReport(ByVal ThrowOnError As Boolean)
            ExecuteReport(ThrowOnError, True)
        End Sub

        Protected Sub ExecuteReport(ByVal ThrowOnError As Boolean, ByVal ShowErrorMessage As Boolean)
            Try
                Me._reportResults = ReportsController.ExecuteReport(Report, String.Concat(ReportsController.CACHEKEY_Reports, Me.ModuleId), False, ParentModule, Me._fromCache)
            Catch exc As DataSourceException
                If ShowErrorMessage Then
                    If Me.ParentModule.UserInfo.IsSuperUser Then
                        UISkin.AddModuleMessage(Me.ParentModule, _
                        String.Format(Localization.GetString("HostDSError.Message", Me.ParentModule.LocalResourceFile), exc.Message), _
                            DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Else
                        UISkin.AddModuleMessage(Me.ParentModule, Localization.GetString("AdminDSError.Message", Me.ParentModule.LocalResourceFile), _
                            DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End If
                End If

                If ThrowOnError Then
                    Throw
                End If
            End Try
        End Sub

        Public Sub SetReport(ByVal Report As ReportInfo, ByVal Results As DataTable, ByVal FromCache As Boolean) Implements IVisualizerControl.SetReport
            Me._report = Report
            Me._reportResults = Results
            Me._fromCache = FromCache
            Me._isFirstRun = True
        End Sub

        Protected Function ValidateDataSource() As Boolean
            Return ValidateDataSource(True)
        End Function

        Protected Function ValidateDataSource(ByVal ShowMessage As Boolean) As Boolean
            If String.IsNullOrEmpty(Me.Report.DataSource) Then
                If ShowMessage Then
                    If Me.ParentModule.UserInfo.IsSuperUser Then
                        UISkin.AddModuleMessage(Me.ParentModule, Localization.GetString("HostNoReport.Message", Me.ParentModule.LocalResourceFile), _
                            DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    ElseIf Me.ParentModule.IsEditable Then
                        UISkin.AddModuleMessage(Me.ParentModule, Localization.GetString("AdminNoReport.Message", Me.ParentModule.LocalResourceFile), _
                            DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End If
                End If
                Return False
            Else
                Return True
            End If
        End Function

        Protected Function ValidateResults() As Boolean
            Return ValidateResults(True)
        End Function

        Protected Function ValidateResults(ByVal ShowMessage As Boolean) As Boolean
            If Me.ReportResults Is Nothing OrElse Me.ReportResults.Rows.Count = 0 Then
                If ShowMessage Then
                    If Me.ParentModule.IsEditable Then
                        UISkin.AddModuleMessage(Me.ParentModule, Localization.GetString("NoResults.Message", Me.ParentModule.LocalResourceFile), _
                            DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End If
                End If
                Return False
            Else
                Return True
            End If
        End Function

#End Region

        '#Region " Event Handlers "

        '        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '            Me.IsFirstRun = False
        '        End Sub

        '#End Region

    End Class

End Namespace
