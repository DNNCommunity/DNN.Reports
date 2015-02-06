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
Imports DotNetNuke.Modules.Reports.Extensions
Imports System.Globalization

Namespace DotNetNuke.Modules.Reports.Visualizers.Xslt

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages XSLT Transform Visualizer Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits ReportsSettingsBase

        Private Property StoredExtensionObjects() As IList(Of ExtensionObjectInfo)
            Get
                ' Can't store a list in ViewState... have to convert to an array for now
                Dim objArray As ExtensionObjectInfo() = TryCast(ViewState("StoredExtensionObjects"), ExtensionObjectInfo())
                If objArray Is Nothing Then
                    Return New List(Of ExtensionObjectInfo)
                Else
                    Return New List(Of ExtensionObjectInfo)(objArray)
                End If
            End Get
            Set(ByVal value As IList(Of ExtensionObjectInfo))
                If value.Count = 0 Then
                    ViewState("StoredExtensionObjects") = Nothing
                Else
                    Dim arr(value.Count - 1) As ExtensionObjectInfo
                    value.CopyTo(arr, 0)
                    ViewState("StoredExtensionObjects") = arr
                End If
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            DataBind()

            If Me.ParentModule.UserInfo.IsSuperUser Then
                mvExtensionObjects.SetActiveView(vwAllowed)
            Else
                mvExtensionObjects.SetActiveView(vwNotAllowed)
            End If
        End Sub

        Public Overrides Sub LoadSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            ctlTransform.Url = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Xslt_TransformFile, String.Empty)
            ctlTransform.DataBind()

            If Me.ParentModule.UserInfo.IsSuperUser Then
                StoredExtensionObjects = ReportsController.GetXsltExtensionObjects(TabModuleId)
            End If
        End Sub

        Public Overrides Sub SaveSettings(ByVal VisualizerSettings As Dictionary(Of String, String))
            VisualizerSettings(ReportsController.SETTING_Xslt_TransformFile) = ctlTransform.Url

            If StoredExtensionObjects IsNot Nothing AndAlso Me.ParentModule.UserInfo.IsSuperUser Then
                ReportsController.SetXsltExtensionObjects(TabModuleId, StoredExtensionObjects)
            End If
        End Sub

        Public Overrides Sub DataBind()
            If Not IsPostBack Then
                For Each col As DataControlField In grdExtensionObjects.Columns
                    If TypeOf col Is BoundField Then
                        col.HeaderText = DotNetNuke.Services.Localization.Localization.GetString(col.HeaderText, Me.LocalResourceFile)
                    End If
                Next
            End If

            grdExtensionObjects.DataSource = StoredExtensionObjects
            grdExtensionObjects.DataBind()
        End Sub

        Protected Sub grdExtensionObjects_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdExtensionObjects.RowDeleting
            Dim list As IList(Of ExtensionObjectInfo) = StoredExtensionObjects
            list.RemoveAt(e.RowIndex)
            StoredExtensionObjects = list
            DataBind()
        End Sub

        Protected Sub btnAddExtensionObject_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddExtensionObject.Click
            Dim newObj As New ExtensionObjectInfo() With { _
                .XmlNamespace = txtXmlns.Text, _
                .ClrType = txtClrType.Text _
            }
            Dim list As IList(Of ExtensionObjectInfo) = StoredExtensionObjects
            list.Add(newObj)
            StoredExtensionObjects = list
            DataBind()
        End Sub
    End Class

End Namespace
