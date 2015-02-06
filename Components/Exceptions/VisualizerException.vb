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
Imports System
Imports System.Runtime.Serialization

Namespace DotNetNuke.Modules.Reports.Exceptions

    ''' <summary>
    ''' Exception thrown when an error occurs during the visualization of a report
    ''' </summary>
    Public Class VisualizerException
        Inherits ReportsModuleException

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText)
            MyBase.New(localizedMessage)
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal innerException As Exception)
            MyBase.New(localizedMessage, innerException)
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal message As String)
            MyBase.New(localizedMessage, message)
        End Sub

        Public Sub New(ByVal localizedMessage As LocalizedText, ByVal message As String, ByVal innerException As Exception)
            MyBase.New(localizedMessage, innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String)
            MyBase.New(resourceKey)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal ParamArray formatArguments As String())
            MyBase.New(resourceKey, formatArguments)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String)
            MyBase.New(resourceKey, resourceFile)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal ParamArray formatArguments As String())
            MyBase.New(New LocalizedText(resourceKey, resourceFile, formatArguments))
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal innerException As Exception)
            MyBase.New(resourceKey, innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal innerException As Exception, ByVal ParamArray formatArguments As String())
            MyBase.New(resourceKey, innerException, formatArguments)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal innerException As Exception)
            MyBase.New(resourceKey, resourceFile, innerException)
        End Sub

        <Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")> _
        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal innerException As Exception, ByVal ParamArray formatArguments As String())
            MyBase.New(resourceKey, resourceFile, innerException, formatArguments)
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

    End Class

End Namespace
