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
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.DataSources.DotNetNuke

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages settings for the DotNetNuke Data Source
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Public Class Settings
        Inherits ReportsSettingsBase
        Implements IDataSourceSettingsControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Public Overrides Sub LoadSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            Me.SqlDataSourceCommonSettingsControl.LoadSettings(Settings)
        End Sub

        Public Overrides Sub SaveSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            Me.SqlDataSourceCommonSettingsControl.SaveSettings(Settings)
        End Sub

        Public ReadOnly Property DataSourceClass() As String Implements IDataSourceSettingsControl.DataSourceClass
            Get
                Return GetType(DotNetNuke.DotNetNukeDataSource).FullName
            End Get
        End Property
    End Class

End Namespace