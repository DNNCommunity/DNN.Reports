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

Namespace DotNetNuke.Modules.Reports.DataSources.ADO

    ''' <summary>
    ''' A Data Source that provides data by querying a Generic ADO.Net Data Provider
    ''' </summary>
    Public Class GenericADODataSource
        Inherits ADODataSourceBase

        Private Function GetProviderFactory() As DbProviderFactory
            Return DbProviderFactories.GetFactory( _
                CurrentReport.DataSourceSettings.Item(ReportsController.SETTING_ADO_ProviderName))
        End Function

        Protected Overrides Function CreateConnection() As DbConnection
            Me.ValidateRequiredReportSetting(ReportsController.SETTING_ADO_ProviderName)

            ' Get the provider factory
            Dim factory As DbProviderFactory = GetProviderFactory()

            ' Create the connection, then populate the connection string
            Dim conn As DbConnection = factory.CreateConnection()
            conn.ConnectionString = CreateConnectionString()
            Return conn
        End Function

        Protected Overrides Function CreateDataSourceException(ByVal dbException As DbException) As Exceptions.DataSourceException
            Throw New DataSourceException(New LocalizedText("DSError.Text", _
                                                            ExtensionContext.ResolveExtensionResourcesPath("DataSource.ascx.resx"), _
                                                            dbException.ErrorCode.ToString(), dbException.Message), _
                                          String.Format("An error occurred while executing the data source (Code: {0}): {1}", _
                                                        dbException.ErrorCode, dbException.Message))
        End Function

        Protected Overrides Function CreateParameter(ByVal name As String, ByVal value As Object) As System.Data.Common.DbParameter
            Dim factory As DbProviderFactory = GetProviderFactory()
            Dim paramPrefix As String = SettingsUtil.GetDictionarySetting(Of String)(CurrentReport.DataSourceSettings, ReportsController.SETTING_ADO_ParamPrefix, String.Empty)
            Dim param As DbParameter = factory.CreateParameter()
            param.ParameterName = String.Concat(paramPrefix, name)
            param.Value = value
            Return param
        End Function

    End Class

End Namespace
