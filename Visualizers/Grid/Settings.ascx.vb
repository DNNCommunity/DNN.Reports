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
Imports System.Web.UI

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.Visualizers.Grid

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages Grid Visualizer Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits ReportsSettingsBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not ReportsClientAPI.IsSupported Then
                chkPageData.AutoPostBack = True
                AddHandler chkPageData.CheckedChanged, AddressOf chkPageData_CheckedChanged
            Else
                ReportsClientAPI.Import(Me.Page)
                ReportsClientAPI.ShowHideByCheckBox(Me.Page, chkPageData, rowPageSize)
            End If
        End Sub

        Public Overrides Sub LoadSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            chkPageData.Checked = SettingsUtil.GetDictionarySetting(Of Boolean)(VisualizerSettings, ReportsController.SETTING_Grid_EnablePaging, False)
            chkSortData.Checked = SettingsUtil.GetDictionarySetting(Of Boolean)(VisualizerSettings, ReportsController.SETTING_Grid_EnableSorting, False)
            txtPageSize.Text = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Grid_PageSize, "10")
            chkShowHeader.Checked = SettingsUtil.GetDictionarySetting(Of Boolean)(VisualizerSettings, ReportsController.SETTING_Grid_ShowHeader, True)
            txtAdditionalCSS.Text = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Grid_AdditionalCSS, "")
            txtCSSClass.Text = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Grid_CSSClass, "")
            Dim gridLines As String = Utilities.GetGridLinesSetting(VisualizerSettings).ToString()
            DropDownUtils.TrySetValue(ddlGridLines, _
                                      SettingsUtil.GetDictionarySetting(Of String)( _
                                            VisualizerSettings, _
                                            ReportsController.SETTING_Grid_GridLines, _
                                            ReportsController.DEFAULT_Grid_GridLines), _
                                      ReportsController.DEFAULT_Grid_GridLines)
            UpdatePageSizeText()
        End Sub

        Public Overrides Sub SaveSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            If Not typvalPageSize.IsValid Then
                txtPageSize.Text = "10"
            End If

            VisualizerSettings(ReportsController.SETTING_Grid_EnablePaging) = chkPageData.Checked.ToString()
            VisualizerSettings(ReportsController.SETTING_Grid_EnableSorting) = chkSortData.Checked.ToString()
            VisualizerSettings(ReportsController.SETTING_Grid_PageSize) = txtPageSize.Text
            VisualizerSettings(ReportsController.SETTING_Grid_ShowHeader) = chkShowHeader.Checked.ToString()
            VisualizerSettings(ReportsController.SETTING_Grid_GridLines) = ddlGridLines.SelectedValue()
            VisualizerSettings(ReportsController.SETTING_Grid_AdditionalCSS) = txtAdditionalCSS.Text
            VisualizerSettings(ReportsController.SETTING_Grid_CSSClass) = txtCSSClass.Text
        End Sub

        Private Sub UpdatePageSizeText()
            If chkPageData.Checked Then
                rowPageSize.Style(HtmlTextWriterStyle.Display) = String.Empty
            Else
                rowPageSize.Style(HtmlTextWriterStyle.Display) = "none"
            End If
        End Sub

        Private Sub chkPageData_CheckedChanged(ByVal sender As Object, ByVal args As EventArgs)
            UpdatePageSizeText()
        End Sub

    End Class

End Namespace
