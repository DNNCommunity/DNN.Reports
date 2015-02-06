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
Imports System.Data.Common

Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Modules.Reports.Converters

Namespace DotNetNuke.Modules.Reports.DataSources

    ''' <summary>
    ''' Base class for ADO.Net-based Data Sources
    ''' </summary>
    Public MustInherit Class ADODataSourceBase
        Inherits SqlDataSourceBase

        Protected Overrides Function ExecuteSQLReport() As System.Data.DataView
            Try
                Dim dr As IDataReader = Nothing

                ' Create the connection
                Dim conn As DbConnection = CreateConnection()

                ' Create and configure a command
                Dim cmd As DbCommand = conn.CreateCommand()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = Me.CurrentReport.DataSourceSettings(ReportsController.SETTING_Query)

                ' Configure parameters
                For Each pair As KeyValuePair(Of String, Object) In Parameters
                    cmd.Parameters.Add(CreateParameter(pair.Key, pair.Value))
                Next

                ' Open the connection
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                ' Execute the command
                dr = cmd.ExecuteReader()

                ' Create the data table
                Dim dv As DataView = CreateDataView(New ConvertingDataReader(dr, Me.CurrentReport.Converters))

                ' Close the connection
                conn.Close()

                Return dv
            Catch ex As DbException
                Throw CreateDataSourceException(ex)
            End Try
        End Function

        ''' <summary>
        ''' Creates a new <see cref="DbConnection"/> and configures it with the correct connection string
        ''' </summary>
        ''' <returns>A configured <see cref="DbConnection"/></returns>
        ''' <remarks>Do not open the connection before returning it</remarks>
        Protected MustOverride Function CreateConnection() As DbConnection

        ''' <summary>
        ''' Creates a new <see cref="DbParameter"/> and configures it with the correct name and value
        ''' </summary>
        ''' <param name="name">The name of the parameter</param>
        ''' <param name="value">The value of the parameter</param>
        ''' <returns>A configured <see cref="DbParameter"/></returns>
        ''' <remarks>
        ''' IMPORTANT: The <paramref name="name"/> parameter does not include any prefixes
        ''' (such as '@') that are often used in Database Query Parameters. For example, in
        ''' a SQL Server Data Source, a parameter name such as 'Foo' will have to be converted
        ''' to '@Foo' in this method.
        ''' </remarks>
        Protected MustOverride Function CreateParameter(ByVal name As String, ByVal value As Object) As DbParameter

        ''' <summary>
        ''' Creates a new <see cref="DataSourceException"/> configured with a message relevant and
        ''' exception recieved from the ADO.Net Database Provider
        ''' </summary>
        ''' <param name="dbException">The exception recieved from the provider</param>
        ''' <returns>A new <see cref="DataSourceException"/></returns>
        Protected MustOverride Function CreateDataSourceException(ByVal dbException As DbException) As DataSourceException

        ''' <summary>
        ''' Helper function to build the connection string, checks the Connection String setting
        ''' called "ConnectionString"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function CreateConnectionString() As String
            ' Check the connection string setting
            If Me.CurrentReport.DataSourceSettings.ContainsKey(ReportsController.SETTING_ConnectionString) Then
                Return Me.CurrentReport.DataSourceSettings(ReportsController.SETTING_ConnectionString)
            End If
            Return String.Empty
        End Function

        Public Overrides ReadOnly Property CanProcessConverters() As Boolean
            Get
                Return True
            End Get
        End Property

    End Class

End Namespace
