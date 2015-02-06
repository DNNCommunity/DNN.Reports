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
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.DataSources.SqlServer

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages settings for the Sql Data Source
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
            If Not ReportsClientAPI.IsSupported Then
                Me.AutomaticConnStrRadio.AutoPostBack = True
                Me.ManualConnStrRadio.AutoPostBack = True
                Me.IntegratedSecurityCheckBox.AutoPostBack = True

                AddHandler AutomaticConnStrRadio.CheckedChanged, AddressOf ConnStrModeCheckedChanged
                AddHandler ManualConnStrRadio.CheckedChanged, AddressOf ConnStrModeCheckedChanged
                AddHandler IntegratedSecurityCheckBox.CheckedChanged, AddressOf IntegratedSecurityCheckBox_CheckedChanged
            Else
                ReportsClientAPI.Import(Me.Page)
                ReportsClientAPI.ShowHideByRadioButtons(Me.Page, _
                                                        AutomaticConnStrRadio, _
                                                        ManualConnStrRadio, _
                                                        AutomaticConnStrConfig, _
                                                        ManualConnStrConfig)
                ReportsClientAPI.ShowHideByCheckBox(Me.Page, _
                                                    IntegratedSecurityCheckBox, _
                                                    UserNameRow, _
                                                    False)
                ReportsClientAPI.ShowHideByCheckBox(Me.Page, _
                                                    IntegratedSecurityCheckBox, _
                                                    PasswordRow, _
                                                    False)

                ' Show the correct table
                UpdateControlVisibility()
            End If
        End Sub

        Public Overrides Sub LoadSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            Dim useConnectionString As Boolean = SettingsUtil.GetDictionarySetting(Of Boolean)(Settings, ReportsController.SETTING_UseConnectionString, False)
            Me.AutomaticConnStrRadio.Checked = Not useConnectionString
            Me.ManualConnStrRadio.Checked = useConnectionString
            ConnStrTextBox.Text = SettingsUtil.GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_ConnectionString, String.Empty)
            ServerTextBox.Text = SettingsUtil.GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_Server, String.Empty)
            DatabaseTextBox.Text = SettingsUtil.GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_Database, String.Empty)
            IntegratedSecurityCheckBox.Checked = SettingsUtil.GetDictionarySetting(Of Boolean)(Settings, ReportsController.SETTING_Sql_UseIntegratedSecurity, False)
            If Not IntegratedSecurityCheckBox.Checked Then
                UserNameTextBox.Text = SettingsUtil.GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_UserName, String.Empty)
                PasswordTextBox.Text = SettingsUtil.GetDictionarySetting(Of String)(Settings, ReportsController.SETTING_Password, String.Empty)
            End If
            SqlDataSourceCommonSettingsControl.LoadSettings(Settings)

            UpdateControlVisibility()
        End Sub

        Public Overrides Sub SaveSettings(ByVal Settings As System.Collections.Generic.Dictionary(Of String, String))
            Settings.Clear()
            If Me.AutomaticConnStrRadio.Checked Then
                Settings.Add(ReportsController.SETTING_UseConnectionString, "False")
                Settings.Add(ReportsController.SETTING_Server, ServerTextBox.Text)
                Settings.Add(ReportsController.SETTING_Database, DatabaseTextBox.Text)
                Settings.Add(ReportsController.SETTING_Sql_UseIntegratedSecurity, IntegratedSecurityCheckBox.Checked.ToString())
                If Not IntegratedSecurityCheckBox.Checked Then
                    Settings.Add(ReportsController.SETTING_UserName, UserNameTextBox.Text)
                    Settings.Add(ReportsController.SETTING_Password, PasswordTextBox.Text)
                End If
            ElseIf Me.ManualConnStrRadio.Checked Then
                Settings.Add(ReportsController.SETTING_UseConnectionString, "True")
                Settings.Add(ReportsController.SETTING_ConnectionString, ConnStrTextBox.Text)
            End If
            SqlDataSourceCommonSettingsControl.SaveSettings(Settings)
        End Sub

        Private Sub ConnStrModeCheckedChanged(ByVal sender As Object, ByVal args As EventArgs)
            UpdateControlVisibility()
        End Sub

        Private Sub IntegratedSecurityCheckBox_CheckedChanged(ByVal sender As Object, ByVal args As EventArgs)
            UpdateControlVisibility()
        End Sub

        Private Sub UpdateControlVisibility()
            If Me.AutomaticConnStrRadio.Checked Then
                ReportsClientAPI.ClientShow(Me.AutomaticConnStrConfig)
                ReportsClientAPI.ClientHide(Me.ManualConnStrConfig)
            ElseIf Me.ManualConnStrRadio.Checked Then
                ReportsClientAPI.ClientShow(Me.ManualConnStrConfig)
                ReportsClientAPI.ClientHide(Me.AutomaticConnStrConfig)
            End If

            If Me.IntegratedSecurityCheckBox.Checked Then
                ReportsClientAPI.ClientHide(Me.UserNameRow)
                ReportsClientAPI.ClientHide(Me.PasswordRow)
            Else
                ReportsClientAPI.ClientShow(Me.UserNameRow)
                ReportsClientAPI.ClientShow(Me.PasswordRow)
            End If
        End Sub

        Public ReadOnly Property DataSourceClass() As String Implements IDataSourceSettingsControl.DataSourceClass
            Get
                Return GetType(SqlServerDataSource).FullName
            End Get
        End Property
    End Class

End Namespace