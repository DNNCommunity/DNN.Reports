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

Imports System.Collections.Generic
Imports System.IO

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.UI
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Reports.Controls

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SqlDataSourceCommonSettingsControl class manages common settings for
    ''' Sql-based Data Sources
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Public Class SqlDataSourceCommonSettingsControl
        Inherits SettingsFormControl

        Public Overrides ReadOnly Property LocalResourceFile() As String
            Get
                Return ResolveUrl("App_LocalResources/SqlDataSourceCommonSettingsControl.ascx")
            End Get
        End Property

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Framework.AJAX.RegisterPostBackControl(QueryUploadButton)
            MyBase.OnLoad(e)
        End Sub

        Private ReadOnly Property ParentModule() As PortalModuleBase
            Get
                Dim current As Control = Me.Parent
                While current IsNot Nothing AndAlso Not TypeOf current Is PortalModuleBase
                    current = current.Parent
                End While
                Return TryCast(current, PortalModuleBase)
            End Get
        End Property

        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles QueryUploadButton.Click
            If QueryUploadControl.PostedFile Is Nothing Then
                Skins.Skin.AddModuleMessage(Me.ParentModule, Localization.GetString("UnableToUploadFile.Text"), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                Return
            End If
            Dim reader As New StreamReader(QueryUploadControl.PostedFile.InputStream)
            QueryTextBox.Text = reader.ReadToEnd()
        End Sub

        Public Overrides Sub LoadSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            QueryTextBox.Text = GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_Query, Null.NullString)
        End Sub

        Public Overrides Sub SaveSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            Settings(ReportsController.SETTING_Query) = QueryTextBox.Text
        End Sub

    End Class

End Namespace