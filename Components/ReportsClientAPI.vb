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

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Web.UI

Imports DotNetNuke.UI.Utilities
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Namespace DotNetNuke.Modules.Reports

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Manages the reports Client API Calls
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	08/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ReportsClientAPI

        Public Const ReportsScript As String = "~/DesktopModules/Reports/js/dnn.reports.js"

        Public Shared ReadOnly Property IsSupported() As Boolean
            Get
                Return ClientAPI.BrowserSupportsFunctionality(ClientAPI.ClientFunctionality.DHTML) ' AndAlso MSAJAX.IsInstalled
            End Get
        End Property

        Public Shared Sub Import(ByVal page As Page)
            ClientAPI.RegisterClientReference(page, UI.Utilities.ClientAPI.ClientNamespaceReferences.dnn)
            ClientAPI.RegisterClientReference(page, UI.Utilities.ClientAPI.ClientNamespaceReferences.dnn_dom)

            'If IsSupported Then
            '    MSAJAX.RegisterClientScript(page, "dnn.reports.js", GetType(ReportsClientAPI).Assembly.FullName)
            'End If
            If Not page.ClientScript.IsClientScriptIncludeRegistered("dnn.reports") Then
                page.ClientScript.RegisterClientScriptInclude("dnn.reports", _
                                                              page.ClientScript.GetWebResourceUrl(GetType(ReportsClientAPI), _
                                                                                                  "dnn.reports.js"))
            End If
        End Sub

        Public Shared Sub ShowHideByCheckBox(ByVal page As Page, ByVal checkBox As Control, ByVal target As Control)
            ShowHideByCheckBox(page, checkBox, target, True)
        End Sub

        Public Shared Sub ShowHideByCheckBox(ByVal page As Page, ByVal checkBox As Control, ByVal target As Control, ByVal showWhenChecked As Boolean)
            Dim scpt As String = String.Format("dnn_reports_showHideByCheckbox('{0}', '{1}', {2}); ", _
                                               checkBox.ClientID, _
                                               target.ClientID, _
                                               showWhenChecked.ToString().ToLower)
            ApplyScript(DirectCast(checkBox, IAttributeAccessor), "onclick", scpt)

            ' Register the script on start as well to ensure the correct state is displayed
            page.ClientScript.RegisterStartupScript(GetType(ReportsClientAPI), _
                                                    String.Concat("shcb", _
                                                                  checkBox.ClientID, _
                                                                  target.ClientID), _
                                                    scpt, _
                                                    True)
        End Sub

        Public Shared Sub RegisterBarColorModeSwitching(ByVal radOneColor As RadioButton, ByVal radColorPerBar As RadioButton, ByVal rowOneColor As HtmlTableRow, ByVal rowColorPerBar As HtmlTableRow)
            Dim updateScript As String = String.Format("dnn_reports_updateBarColorMode('{0}', '{1}', '{2}', '{3}');", radOneColor.ClientID, _
                radColorPerBar.ClientID, rowOneColor.ClientID, rowColorPerBar.ClientID)
            ApplyScript(radOneColor, "onclick", updateScript)
            ApplyScript(radColorPerBar, "onclick", updateScript)
        End Sub

        Public Shared Sub RegisterColorPreview(ByVal txtColor As Control, ByVal colorPreview As Control)
            ApplyScript(DirectCast(txtColor, IAttributeAccessor), "onchange", String.Format("dnn_reports_updateColorPreview('{0}', '{1}')", txtColor.ClientID, colorPreview.ClientID))
        End Sub

        Public Shared Sub RegisterReportingServicesModeUpdate(ByVal radLocal As RadioButton, ByVal radServer As RadioButton, ByVal rowFile As Control, _
        ByVal rowDataSource As Control, ByVal rowServerUrl As Control, ByVal rowServerReportPath As Control)

            Dim sScript As String = String.Format("dnn_reports_updateRSMode('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", radLocal.ClientID, _
                radServer.ClientID, rowFile.ClientID, rowDataSource.ClientID, rowServerUrl.ClientID, rowServerReportPath.ClientID)
            ApplyScript(radLocal, "onclick", sScript)
            ApplyScript(radServer, "onclick", sScript)
        End Sub

        Public Shared Sub ShowHideByRadioButtons(ByVal page As Page, _
                                                 ByVal radioButtonA As RadioButton, _
                                                 ByVal radioButtonB As RadioButton, _
                                                 ByVal ctrlA As Control, _
                                                 ByVal ctrlB As Control)

            Dim scpt As String = String.Format("dnn_reports_showHideByRadioButtons('{0}', '{1}', '{2}', '{3}'); ", _
                                               radioButtonA.ClientID, _
                                               radioButtonB.ClientID, _
                                               ctrlA.ClientID, _
                                               ctrlB.ClientID)
            ApplyScript(DirectCast(radioButtonA, IAttributeAccessor), "onclick", scpt)
            ApplyScript(DirectCast(radioButtonB, IAttributeAccessor), "onclick", scpt)

            ' Register the script on start as well to ensure the correct state is displayed
            page.ClientScript.RegisterStartupScript(GetType(ReportsClientAPI), _
                                                    String.Concat("shrb", _
                                                                  radioButtonA.ClientID, _
                                                                  radioButtonB.ClientID, _
                                                                  ctrlA.ClientID, _
                                                                  ctrlB.ClientID), _
                                                    scpt, _
                                                    True)
        End Sub

        ''' <summary>
        ''' Hides the specified control on the client-side, while still rendering
        ''' it to HTML
        ''' </summary>
        ''' <param name="TargetControl">The control to hide</param>
        Public Shared Sub ClientHide(ByVal TargetControl As Control)
            SetCssStyle(TargetControl, HtmlTextWriterStyle.Display, "none")
        End Sub

        ''' <summary>
        ''' Shows a control hidden with <see cref="ClientHide"/>
        ''' </summary>
        ''' <param name="TargetControl">The control to show</param>
        Public Shared Sub ClientShow(ByVal TargetControl As Control)
            SetCssStyle(TargetControl, HtmlTextWriterStyle.Display, "")
        End Sub

        Private Shared Sub SetCssStyle(ByVal TargetControl As Control, ByVal Field As HtmlTextWriterStyle, ByVal Value As String)
            Dim webCtrl As WebControl = TryCast(TargetControl, WebControl)
            If webCtrl IsNot Nothing Then
                webCtrl.Style(Field) = Value
                Return
            End If

            Dim htmlCtrl As HtmlControl = TryCast(TargetControl, HtmlControl)
            If htmlCtrl IsNot Nothing Then
                htmlCtrl.Style(Field) = Value
                Return
            End If

            Throw New ArgumentException("Control must be either a WebControl or an HtmlControl", "Control")
        End Sub

        Public Shared Sub ApplyScript(ByVal control As IAttributeAccessor, ByVal attr As String, ByVal script As String)
            Dim sCurValue As String = control.GetAttribute(attr)
            Dim sNewValue As String = String.Empty
            If Not String.IsNullOrEmpty(sCurValue) Then
                If Not sCurValue.EndsWith(";") Then
                    sNewValue = String.Format("{0}; {1}", sCurValue, script)
                Else
                    sNewValue = String.Concat(sCurValue, script)
                End If
            Else
                sNewValue = script
            End If
            control.SetAttribute(attr, sNewValue)
        End Sub

    End Class

End Namespace
