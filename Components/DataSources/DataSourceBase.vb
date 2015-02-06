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
Imports System.Data
Imports DotNetNuke.Entities.Modules
Imports System.Data.Common
Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.DataSources

    ''' <summary>
    ''' Base class for Data Sources
    ''' </summary>
    Public MustInherit Class DataSourceBase
        Inherits ReportsExtensionBase
        Implements IDataSource

        Public Overridable Function ExecuteReport(ByVal report As ReportInfo, _
                                      ByVal hostModule As Entities.Modules.PortalModuleBase, _
                                      ByVal inputParameters As IDictionary(Of String, Object)) As System.Data.DataView Implements IDataSource.ExecuteReport
            ' Verify Method Contract
            If report Is Nothing Then Throw New ArgumentNullException("report")
            If inputParameters Is Nothing Then inputParameters = New Dictionary(Of String, Object)

            ' Verify DataSourceClass property
            If Not report.DataSourceClass.Equals(Me.GetType().FullName) Then
                Throw New InvalidOperationException("The specified report is not configured to use this data source")
            End If

            Return CreateDataView()
        End Function

        ''' <summary>
        ''' Constructs an empty data table configured for loading data for the reports module.
        ''' </summary>
        ''' <returns>A empty data table, configured for loading data for the reports module</returns>
        Protected Function CreateDataView() As DataView
            Return CreateDataView(Nothing)
        End Function

        ''' <summary>
        ''' Constructs a data table configured for loading data for the reports module. Optionally
        ''' reads in the contents of the specified reader
        ''' </summary>
        ''' <param name="reader">
        ''' The reader to load from, or a null reference (Nothing in VB.net) 
        ''' to construct an empty table
        ''' </param>
        ''' <returns>A data table, optionally filled with data from the specified reader</returns>
        ''' <remarks>
        ''' IMPORTANT: This method does NOT wrap the recieved reader in a
        ''' <see cref="Converters.ConvertingDataReader" />. In order to support converters,
        ''' implementors should wrap the data reader they recieve from their data source
        ''' in a <see cref="Converters.ConvertingDataReader" /> and then pass it to this method.
        ''' If that is done, the <see cref="CanProcessConverters" /> property should be overriden
        ''' to return true.
        ''' </remarks>
        Protected Function CreateDataView(ByVal reader As IDataReader) As DataView
            Dim dt As New DataTable("QueryResults")
            If reader IsNot Nothing Then dt.Load(reader)
            Return New DataView(dt)
        End Function

        Public Overridable ReadOnly Property CanProcessConverters() As Boolean Implements IDataSource.CanProcessConverters
            Get
                Return False
            End Get
        End Property
    End Class

End Namespace
