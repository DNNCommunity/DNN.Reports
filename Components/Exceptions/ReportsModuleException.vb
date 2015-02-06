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
Imports System.Runtime.Serialization

Namespace DotNetNuke.Modules.Reports.Exceptions
    ''' <summary>
    ''' Base class for exceptions which include information for localizing their text,
    ''' through the use of external resource files
    ''' </summary>
    <Serializable()> _
    Public MustInherit Class ReportsModuleException
        Inherits ApplicationException

        Private Const SER_KEY_LocalizedText As String = "LocalizedText"

#Region " Private Fields "

        Private _localizedMessage As LocalizedText

#End Region

#Region " Public Properties "

        Public ReadOnly Property LocalizedMessage() As String
            Get
                If _localizedMessage IsNot Nothing Then
                    Return _localizedMessage.GetLocalizedText()
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets the resource key used to localize the exception message
        ''' </summary>
        Public ReadOnly Property ResourceKey() As String
            Get
                If _localizedMessage IsNot Nothing Then
                    Return _localizedMessage.ResourceKey
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets the resource file used to localize the exception message, defaults
        ''' to the application's global resource file
        ''' </summary>
        Public ReadOnly Property ResourceFile() As String
            Get
                If _localizedMessage IsNot Nothing Then
                    Return _localizedMessage.ResourceFile
                End If
                Return Nothing
            End Get
        End Property

        ''' <summary>
        ''' Gets a read only list of the arguments used to format the localized exception
        ''' message
        ''' </summary>
        Public ReadOnly Property FormatArgs() As IEnumerable(Of String)
            Get
                If _localizedMessage IsNot Nothing Then
                    Return _localizedMessage.FormatArguments
                End If
                Return New String() {}
            End Get
        End Property

#End Region

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText)
            MyBase.New()
            _localizedMessage = localizedMessage
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal innerException As Exception)
            MyBase.New(String.Empty, innerException)
            _localizedMessage = localizedMessage
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal message As String)
            MyBase.New(message)
            _localizedMessage = localizedMessage
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
            _localizedMessage = localizedMessage
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String)
            Me.New(New LocalizedText(resourceKey))
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal ParamArray formatArguments As String())
            Me.New(New LocalizedText(resourceKey, formatArguments))
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String)
            Me.New(New LocalizedText(resourceKey, resourceFile, New String() {}))
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal ParamArray formatArguments As String())
            Me.New(New LocalizedText(resourceKey, resourceFile, formatArguments))
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal innerException As Exception)
            Me.New(New LocalizedText(resourceKey), innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal innerException As Exception, ByVal ParamArray formatArguments As String())
            Me.New(New LocalizedText(resourceKey, formatArguments), innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal innerException As Exception)
            Me.New(New LocalizedText(resourceKey, resourceFile, New String() {}), innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal innerException As Exception, ByVal ParamArray formatArguments As String())
            Me.New(New LocalizedText(resourceKey, resourceFile, formatArguments), innerException)
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
            _localizedMessage = TryCast(info.GetValue(SER_KEY_LocalizedText, GetType(LocalizedText)), LocalizedText)
        End Sub

        Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.GetObjectData(info, context)
            info.AddValue(SER_KEY_LocalizedText, _localizedMessage, GetType(LocalizedText))
        End Sub

    End Class

End Namespace
