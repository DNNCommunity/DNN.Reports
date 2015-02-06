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
Namespace DotNetNuke.Modules.Reports.Converters

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' A wrapper around an existing data reader that applies filters to the field values
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	10/13/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ConvertingDataReader
        Implements System.Data.IDataReader

        Private adaptee As IDataReader
        Private converters As IDictionary(Of String, IList(Of ConverterInstanceInfo))

        Public Sub New(ByVal adaptee As IDataReader, ByVal converters As IDictionary(Of String, IList(Of ConverterInstanceInfo)))
            Me.adaptee = adaptee
            Me.converters = converters
        End Sub

        Public Sub Close() Implements System.Data.IDataReader.Close
            adaptee.Close()
        End Sub

        Public ReadOnly Property Depth() As Integer Implements System.Data.IDataReader.Depth
            Get
                Return adaptee.Depth
            End Get
        End Property

        Public Function GetSchemaTable() As System.Data.DataTable Implements System.Data.IDataReader.GetSchemaTable
            Return adaptee.GetSchemaTable
        End Function

        Public ReadOnly Property IsClosed() As Boolean Implements System.Data.IDataReader.IsClosed
            Get
                Return adaptee.IsClosed
            End Get
        End Property

        Public Function NextResult() As Boolean Implements System.Data.IDataReader.NextResult
            Return adaptee.NextResult
        End Function

        Public Function Read() As Boolean Implements System.Data.IDataReader.Read
            Return adaptee.Read
        End Function

        Public ReadOnly Property RecordsAffected() As Integer Implements System.Data.IDataReader.RecordsAffected
            Get
                Return adaptee.RecordsAffected
            End Get
        End Property

        Public ReadOnly Property FieldCount() As Integer Implements System.Data.IDataRecord.FieldCount
            Get
                Return adaptee.FieldCount
            End Get
        End Property

        Public Function GetBoolean(ByVal i As Integer) As Boolean Implements System.Data.IDataRecord.GetBoolean
            Return GetConverted(Of Boolean)(adaptee.GetName(i), adaptee.GetBoolean(i))
        End Function

        Public Function GetByte(ByVal i As Integer) As Byte Implements System.Data.IDataRecord.GetByte
            Return GetConverted(Of Byte)(adaptee.GetName(i), adaptee.GetByte(i))
        End Function

        Public Function GetBytes(ByVal i As Integer, ByVal fieldOffset As Long, ByVal buffer() As Byte, ByVal bufferoffset As Integer, ByVal length As Integer) As Long Implements System.Data.IDataRecord.GetBytes
            Dim ret As Long = adaptee.GetBytes(i, fieldOffset, buffer, bufferoffset, length)
            buffer = GetConverted(Of Byte())(adaptee.GetName(i), buffer)
            Return ret
        End Function

        Public Function GetChar(ByVal i As Integer) As Char Implements System.Data.IDataRecord.GetChar
            Return GetConverted(Of Char)(adaptee.GetName(i), adaptee.GetChar(i))
        End Function

        Public Function GetChars(ByVal i As Integer, ByVal fieldoffset As Long, ByVal buffer() As Char, ByVal bufferoffset As Integer, ByVal length As Integer) As Long Implements System.Data.IDataRecord.GetChars
            Dim ret As Long = adaptee.GetChars(i, fieldoffset, buffer, bufferoffset, length)
            buffer = GetConverted(Of Char())(adaptee.GetName(i), buffer)
            Return ret
        End Function

        Public Function GetData(ByVal i As Integer) As System.Data.IDataReader Implements System.Data.IDataRecord.GetData
            Return New ConvertingDataReader(adaptee.GetData(i), converters)
        End Function

        Public Function GetDataTypeName(ByVal i As Integer) As String Implements System.Data.IDataRecord.GetDataTypeName
            Return adaptee.GetDataTypeName(i)
        End Function

        Public Function GetDateTime(ByVal i As Integer) As Date Implements System.Data.IDataRecord.GetDateTime
            Return GetConverted(Of DateTime)(adaptee.GetName(i), adaptee.GetDateTime(i))
        End Function

        Public Function GetDecimal(ByVal i As Integer) As Decimal Implements System.Data.IDataRecord.GetDecimal
            Return GetConverted(Of Decimal)(adaptee.GetName(i), adaptee.GetDecimal(i))
        End Function

        Public Function GetDouble(ByVal i As Integer) As Double Implements System.Data.IDataRecord.GetDouble
            Return GetConverted(Of Double)(adaptee.GetName(i), adaptee.GetDouble(i))
        End Function

        Public Function GetFieldType(ByVal i As Integer) As System.Type Implements System.Data.IDataRecord.GetFieldType
            Return adaptee.GetFieldType(i)
        End Function

        Public Function GetFloat(ByVal i As Integer) As Single Implements System.Data.IDataRecord.GetFloat
            Return GetConverted(Of Single)(adaptee.GetName(i), adaptee.GetFloat(i))
        End Function

        Public Function GetGuid(ByVal i As Integer) As System.Guid Implements System.Data.IDataRecord.GetGuid
            Return GetConverted(Of Guid)(adaptee.GetName(i), adaptee.GetGuid(i))
        End Function

        Public Function GetInt16(ByVal i As Integer) As Short Implements System.Data.IDataRecord.GetInt16
            Return GetConverted(Of Short)(adaptee.GetName(i), adaptee.GetInt16(i))
        End Function

        Public Function GetInt32(ByVal i As Integer) As Integer Implements System.Data.IDataRecord.GetInt32
            Return GetConverted(Of Integer)(adaptee.GetName(i), adaptee.GetInt32(i))
        End Function

        Public Function GetInt64(ByVal i As Integer) As Long Implements System.Data.IDataRecord.GetInt64
            Return GetConverted(Of Long)(adaptee.GetName(i), adaptee.GetInt64(i))
        End Function

        Public Function GetName(ByVal i As Integer) As String Implements System.Data.IDataRecord.GetName
            Return adaptee.GetName(i)
        End Function

        Public Function GetOrdinal(ByVal name As String) As Integer Implements System.Data.IDataRecord.GetOrdinal
            Return adaptee.GetOrdinal(name)
        End Function

        Public Function GetString(ByVal i As Integer) As String Implements System.Data.IDataRecord.GetString
            Return GetConverted(Of String)(adaptee.GetName(i), adaptee.GetString(i))
        End Function

        Public Function GetValue(ByVal i As Integer) As Object Implements System.Data.IDataRecord.GetValue
            Return GetConverted(Of Object)(adaptee.GetName(i), adaptee.GetValue(i))
        End Function

        Public Function GetValues(ByVal values() As Object) As Integer Implements System.Data.IDataRecord.GetValues
            Dim ret As Integer = adaptee.GetValues(values)
            For i As Integer = 0 To values.Length - 1
                values(i) = GetConverted(Of Object)(adaptee.GetName(i), values(i))
            Next
            Return ret
        End Function

        Public Function IsDBNull(ByVal i As Integer) As Boolean Implements System.Data.IDataRecord.IsDBNull
            Return adaptee.IsDBNull(i)
        End Function

        Default Public Overloads ReadOnly Property Item(ByVal i As Integer) As Object Implements System.Data.IDataRecord.Item
            Get
                Return GetValue(i)
            End Get
        End Property

        Default Public Overloads ReadOnly Property Item(ByVal name As String) As Object Implements System.Data.IDataRecord.Item
            Get
                Return GetValue(GetOrdinal(name))
            End Get
        End Property

        Private Function GetConverted(Of T)(ByVal fieldName As String, ByVal value As T) As T
            If Not converters.ContainsKey(fieldName) Then Return value

            Dim list As IList(Of ConverterInstanceInfo) = converters(fieldName)
            For Each converter As ConverterInstanceInfo In list
                value = DirectCast(ReportsController.ApplyConverter(value, converter.ConverterName, converter.Arguments), T)
            Next

            Return value
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    adaptee.Dispose()
                End If

                converters = Nothing
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
