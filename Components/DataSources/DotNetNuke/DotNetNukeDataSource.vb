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
Imports DotNetNuke.Modules.Reports.Converters

Namespace DotNetNuke.Modules.Reports.DataSources.DotNetNuke

    ''' <summary>
    ''' A Data Source that provides data by querying the DotNetNuke Data Provider
    ''' </summary>
    Public Class DotNetNukeDataSource
        Inherits SqlDataSourceBase

        Protected Overrides Function ExecuteSQLReport() As DataView
            Try
                ' Convert inputParameters to DB-specific Parameters
                Dim params(Me.Parameters.Count) As DbParameter
                Dim i As Integer = 0
                For Each pair As KeyValuePair(Of String, Object) In Me.Parameters
                    params(i) = DataProvider.Instance.CreateInputParameter(pair.Key, _
                                                                           pair.Value)
                    i += 1
                Next

                ' Execute the query against the data source
                Dim dr As IDataReader = DataProvider.Instance().ExecuteSQL(Me.CurrentReport.DataSourceSettings(QueryKey), _
                                                                           params)
                Return CreateDataView(New ConvertingDataReader(dr, Me.CurrentReport.Converters))
            Catch ex As SqlException
                Throw New DataSourceException(New LocalizedText("SqlError.Text", _
                                                                ExtensionContext.ResolveExtensionResourcesPath("DataSource.ascx.resx"), _
                                                                ex.LineNumber.ToString(), ex.Message), _
                                              String.Format("There is an error in your SQL at line {0}: {1}", _
                                                            ex.LineNumber, ex.Message))
            End Try
        End Function

        Public Overrides ReadOnly Property CanProcessConverters() As Boolean
            Get
                Return True
            End Get
        End Property

    End Class

End Namespace
