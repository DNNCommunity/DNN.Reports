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
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Reports.Visualizers.Xslt
    <Serializable()> _
    Public Class ExtensionObjectInfo
        Implements IHydratable

        Private _extensionObjectId As Integer
        Private _xmlNamespace As String
        Private _clrType As String

        Public Property XmlNamespace() As String
            Get
                Return _xmlNamespace
            End Get
            Set(ByVal value As String)
                _xmlNamespace = value
            End Set
        End Property

        Public Property ClrType() As String
            Get
                Return _clrType
            End Get
            Set(ByVal value As String)
                _clrType = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements Entities.Modules.IHydratable.Fill
            _extensionObjectId = DirectCast(dr("ExtensionObjectId"), Integer)
            _xmlNamespace = DirectCast(dr("XmlNamespace"), String)
            _clrType = DirectCast(dr("ClrType"), String)
        End Sub

        Public Property KeyID() As Integer Implements Entities.Modules.IHydratable.KeyID
            Get
                Return _extensionObjectId
            End Get
            Set(ByVal value As Integer)
                _extensionObjectId = value
            End Set
        End Property
    End Class
End Namespace
