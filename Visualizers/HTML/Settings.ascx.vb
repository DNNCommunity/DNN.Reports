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
Imports DotNetNuke.Common.Utilities

Imports System.Collections.Generic

Imports System.IO
Imports System.Web.UI

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.Visualizers.Html

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages HTML Template Visualizer Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits ReportsSettingsBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        End Sub

        Public Overrides Sub LoadSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            ctlTemplate.Url = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Html_TemplateFile, Null.NullString)
            ctlTemplate.DataBind()
        End Sub

        Public Overrides Sub SaveSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            VisualizerSettings(ReportsController.SETTING_Html_TemplateFile) = ctlTemplate.Url
        End Sub
    End Class

End Namespace
