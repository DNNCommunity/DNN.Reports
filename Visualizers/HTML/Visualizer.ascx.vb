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
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports UISkin = DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.Security

Namespace DotNetNuke.Modules.Reports.Visualizers.Html

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Visualizer class displays the Html Template
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Visualizer
        Inherits VisualizerControlBase

#Region " Event Handlers "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If IsFirstRun Then
                DataBind()
            End If
        End Sub

#End Region

#Region " Overrides "

        Public Overrides Sub DataBind()
            ' Get the report for this module
            If Not Me.ValidateDataSource OrElse Not Me.ValidateResults() Then
                pnlContent.Visible = False
            Else
                pnlContent.Visible = True
                Dim sFileID As String = SettingsUtil.GetDictionarySetting(Of String)(Report.VisualizerSettings, ReportsController.SETTING_Html_TemplateFile, String.Empty)
                If Not String.IsNullOrEmpty(sFileID) Then
                    Dim sFile As String = Utilities.MapFileIdPath(Me.ParentModule.PortalSettings, sFileID)
                    If Not String.IsNullOrEmpty(sFile) Then
                        Dim sHtml As String = System.IO.File.ReadAllText(sFile)
                        If Not String.IsNullOrEmpty(sHtml) Then

                            ' Iterate over each row
                            For Each row As DataRow In Me.ReportResults.Rows
                                Dim rowHtml As String = sHtml
                                For Each dcol As DataColumn In Me.ReportResults.Columns
                                    rowHtml = rowHtml.Replace(String.Format("[{0}]", dcol.ColumnName), row(dcol).ToString())
                                Next

                                Dim objSec As New PortalSecurity
                                Dim divContent As New HtmlGenericControl("div")
                                divContent.Attributes("class") = "DNN_Reports_HTML_Item"
                                divContent.InnerHtml = objSec.InputFilter(rowHtml, PortalSecurity.FilterFlag.NoScripting)
                                pnlContent.Controls.Add(divContent)
                            Next

                        End If
                        MyBase.DataBind()
                    End If
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
