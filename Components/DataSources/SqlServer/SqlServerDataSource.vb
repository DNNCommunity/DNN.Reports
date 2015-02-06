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

Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Modules.Reports.Data
Imports DotNetNuke.Modules.Reports.Exceptions

Namespace DotNetNuke.Modules.Reports.DataSources.SqlServer

    ''' <summary>
    ''' A Data Source that provides data by querying the Sql Data Provider
    ''' </summary>
    Public Class SqlServerDataSource
        Inherits ADODataSourceBase

        Protected Overrides Function CreateConnectionString() As String
            Dim connStr As String = MyBase.CreateConnectionString()
            If Not String.IsNullOrEmpty(connStr) Then
                Return connStr
            End If

            ' No connection string setting, we need to build it

            ' First validate the fields
            Dim server As String = _
                SettingsUtil.GetDictionarySetting(Of String)(Me.CurrentReport.DataSourceSettings, _
                                                             ReportsController.SETTING_Server, _
                                                             String.Empty)
            Dim database As String = _
                SettingsUtil.GetDictionarySetting(Of String)(Me.CurrentReport.DataSourceSettings, _
                                                             ReportsController.SETTING_Database, _
                                                             String.Empty)
            Dim useIntegratedSecurity As Boolean = _
                SettingsUtil.GetDictionarySetting(Of Boolean)(Me.CurrentReport.DataSourceSettings, _
                                                              ReportsController.SETTING_Sql_UseIntegratedSecurity, _
                                                              False)
            Dim userName As String = _
                SettingsUtil.GetDictionarySetting(Of String)(Me.CurrentReport.DataSourceSettings, _
                                                             ReportsController.SETTING_UserName, _
                                                             String.Empty)
            Dim password As String = _
                SettingsUtil.GetDictionarySetting(Of String)(Me.CurrentReport.DataSourceSettings, _
                                                             ReportsController.SETTING_Password, _
                                                             String.Empty)

            Dim csBuilder As New SqlConnectionStringBuilder()
            If Not String.IsNullOrEmpty(server) Then csBuilder.Add("Server", server)
            If Not String.IsNullOrEmpty(database) Then csBuilder.Add("Database", database)
            If Not useIntegratedSecurity Then
                If Not String.IsNullOrEmpty(userName) Then csBuilder.Add("User ID", userName)
                If Not String.IsNullOrEmpty(password) Then csBuilder.Add("Password", password)
                csBuilder.Add("Trusted_Connection", "False")
            Else
                csBuilder.Add("Trusted_Connection", "True")
            End If

            Return csBuilder.ConnectionString
        End Function

        Protected Overrides Function CreateConnection() As DbConnection
            Return New SqlConnection(CreateConnectionString())
        End Function

        Protected Overrides Function CreateDataSourceException(ByVal dbException As DbException) As Exceptions.DataSourceException
            Dim ex As SqlException = DirectCast(dbException, SqlException)
            Throw New DataSourceException(New LocalizedText("SqlError.Text", _
                                                            ExtensionContext.ResolveExtensionResourcesPath("DataSource.ascx.resx"), _
                                                            ex.LineNumber.ToString(), ex.Message), _
                                          String.Format("There is an error in your SQL at line {0}: {1}", ex.LineNumber, ex.Message))
        End Function

        Protected Overrides Function CreateParameter(ByVal name As String, ByVal value As Object) As System.Data.Common.DbParameter
            Return New SqlParameter(String.Concat("@", name), value)
        End Function

    End Class

End Namespace
