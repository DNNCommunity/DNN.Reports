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

Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports UISkin = DotNetNuke.UI.Skins.Skin

Namespace DotNetNuke.Modules.Reports.Visualizers.Grid

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Visualizer class displays the Grid
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Visualizer
        Inherits VisualizerControlBase

#Region " Private Properties "

        Private Property SortExpr() As String
            Get
                Dim o As Object = ViewState("SortExpr")
                If o IsNot Nothing AndAlso TypeOf o Is String Then
                    Return o.ToString()
                Else
                    Return Null.NullString
                End If
            End Get
            Set(ByVal value As String)
                ViewState("SortExpr") = value
            End Set
        End Property

        Private Property SortDir() As String
            Get
                Dim o As Object = ViewState("SortDir")
                If o IsNot Nothing AndAlso TypeOf o Is String Then
                    Return o.ToString
                Else
                    Return "ASC"
                End If
            End Get
            Set(ByVal value As String)
                ViewState("SortDir") = value
            End Set
        End Property

#End Region

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

        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdResults.PageIndexChanging
            grdResults.PageIndex = e.NewPageIndex
            DataBind()
        End Sub

        Protected Sub grdResults_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdResults.Sorting
            If e.SortExpression.Equals(Me.SortExpr) Then ' Toggle SortDir
                If Me.SortDir = "ASC" Then
                    Me.SortDir = "DESC"
                Else
                    Me.SortDir = "ASC"
                End If
            Else ' Sort Ascending first when a new column selected for sort
                Me.SortDir = "ASC"
            End If
            Me.SortExpr = e.SortExpression
            DataBind()
        End Sub

        Protected Sub grdResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdResults.RowDataBound
            ' If the row is a header, add sort direction images
            If e.Row IsNot Nothing AndAlso e.Row.RowType = DataControlRowType.Header Then
                For Each cell As TableCell In e.Row.Cells
                    If cell.HasControls Then
                        Dim button As LinkButton = TryCast(cell.Controls(0), LinkButton)
                        If button IsNot Nothing Then

                            ' Is this cell the one that we are sorting?
                            If Me.SortExpr = button.CommandArgument Then
                                Dim image As New System.Web.UI.WebControls.Image()

                                ' Use sort direction to determine image to display
                                If Me.SortDir = "ASC" Then
                                    image.ImageUrl = "~/images/sortascending.gif"
                                Else
                                    image.ImageUrl = "~/images/sortdescending.gif"
                                End If

                                cell.Controls.AddAt(0, image)
                            End If

                        End If
                    End If
                Next
            End If
        End Sub

#End Region

#Region " Overrides "

        Public Overrides Sub DataBind()
            ' Get the report for this module
            If Not Me.ValidateDataSource OrElse Not Me.ValidateResults() Then
                grdResults.Visible = False
            Else
                grdResults.Visible = True
                GenerateColumns(Me.ReportResults)
                Dim view As DataView = New DataView(Me.ReportResults)
                If Not String.IsNullOrEmpty(Me.SortExpr) Then
                    view.Sort = String.Format("{0} {1}", Me.SortExpr, Me.SortDir)
                End If
                grdResults.DataSource = view
                ConfigureGridFromSettings()
            End If
            MyBase.DataBind()
        End Sub

#End Region

#Region " Private Methods "

        Private Sub ConfigureGridFromSettings()
            ' Load Paging and Sorting data from Visualizer Settings
            grdResults.AllowPaging = SettingsUtil.GetDictionarySetting(Of Boolean)(Report.VisualizerSettings, ReportsController.SETTING_Grid_EnablePaging, False)
            grdResults.AllowSorting = SettingsUtil.GetDictionarySetting(Of Boolean)(Report.VisualizerSettings, ReportsController.SETTING_Grid_EnableSorting, False)
            grdResults.PageSize = SettingsUtil.GetDictionarySetting(Of Integer)(Report.VisualizerSettings, ReportsController.SETTING_Grid_PageSize, 10)
            grdResults.ShowHeader = SettingsUtil.GetDictionarySetting(Of Boolean)(Report.VisualizerSettings, ReportsController.SETTING_Grid_ShowHeader, True)
            grdResults.CssClass += SettingsUtil.GetDictionarySetting(Of String)(Report.VisualizerSettings, ReportsController.SETTING_Grid_CSSClass, "")

            Dim styleString As String = SettingsUtil.GetDictionarySetting(Of String)(Report.VisualizerSettings, ReportsController.SETTING_Grid_AdditionalCSS, "")
            For Each styleEntry As String In styleString.Split(";"c)
                Dim styleArray As String() = styleEntry.Split(":"c)
                If styleArray.Length <> 2 Then
                    Continue For
                End If
                grdResults.Style.Add(styleArray(0), styleArray(1))
            Next

            grdResults.GridLines = Utilities.GetGridLinesSetting(Report.VisualizerSettings)
        End Sub

        ' Generates columns on the GridView for the columns of the DataTable
        Private Sub GenerateColumns(ByVal dataTable As DataTable)
            grdResults.Columns.Clear()
            For Each column As DataColumn In dataTable.Columns
                Dim field As New BoundField()
                field.DataField = column.ColumnName
                field.SortExpression = column.ColumnName
                field.HtmlEncode = False
                field.HeaderText = column.ColumnName
                grdResults.Columns.Add(field)
            Next
        End Sub

#End Region

    End Class

End Namespace
