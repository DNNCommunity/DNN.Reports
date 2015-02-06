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
Imports System.Web.Compilation
Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Xml
Imports System.Xml.Xsl
Imports UISkin = DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.Security

Namespace DotNetNuke.Modules.Reports.Visualizers.Xslt

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Visualizer class displays the XSLT Transform
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
                litContent.Visible = False
            Else
                litContent.Visible = True

                ' Get the extension objects
                Dim extensionObjects As IEnumerable(Of ExtensionObjectInfo) = ReportsController.GetXsltExtensionObjects(TabModuleId)
                Dim argList As New XsltArgumentList()
                For Each extensionObject As ExtensionObjectInfo In extensionObjects
                    Dim obj As Object = CreateExtensionObject(extensionObject.ClrType)
                    If obj IsNot Nothing Then
                        argList.AddExtensionObject(extensionObject.XmlNamespace, obj)
                    End If
                Next

                ' Get the Xslt Url
                Dim sXsl As String = SettingsUtil.GetDictionarySetting(Report.VisualizerSettings, ReportsController.SETTING_Xslt_TransformFile, String.Empty)
                If String.IsNullOrEmpty(sXsl) Then Return
                If sXsl.ToLower().StartsWith("fileid=") Then
                    sXsl = Utilities.MapFileIdPath(Me.ParentModule.PortalSettings, sXsl)
                Else
                    sXsl = System.IO.Path.Combine(ParentModule.PortalSettings.HomeDirectoryMapPath, sXsl.Replace("/", "\"))
                End If
                If String.IsNullOrEmpty(sXsl) Then Return

                ' Serialize the results to Xml
                Dim sbSource As New StringBuilder()
                Using srcWriter As New System.IO.StringWriter(sbSource)
                    Me.ReportResults.WriteXml(srcWriter)
                End Using

                ' Load the Transform and transform the Xml
                Dim sbDest As New StringBuilder()
                Dim xform As New Xsl.XslCompiledTransform()
                Using destWriter As New XmlTextWriter(New System.IO.StringWriter(sbDest))
                    xform.Load(sXsl)
                    xform.Transform(New XPath.XPathDocument(New System.IO.StringReader(sbSource.ToString())), argList, destWriter)
                End Using

                Dim objSec As New PortalSecurity()
                litContent.Text = objSec.InputFilter(sbDest.ToString(), PortalSecurity.FilterFlag.NoScripting)
            End If
            MyBase.DataBind()
        End Sub

        Private Function CreateExtensionObject(ByVal typeName As String) As Object
            ' Get the type from the build manager
            Dim type As Type = BuildManager.GetType(typeName, False)
            If type Is Nothing Then Return Nothing

            ' Construct an instance
            Dim instance As Object = Nothing
            Try
                instance = Activator.CreateInstance(type)
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.LogException(ex)
                Return Nothing
            End Try

            ' Check for the IXsltExtensionObject interface
            Dim extObj As IXsltExtensionObject = TryCast(instance, IXsltExtensionObject)
            If extObj IsNot Nothing Then
                extObj.ParentModule = ParentModule
                extObj.Report = Report
                extObj.ReportResults = ReportResults
            End If

            Return instance
        End Function

#End Region

    End Class

End Namespace
