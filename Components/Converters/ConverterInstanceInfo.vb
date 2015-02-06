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
Imports System.Configuration
Imports System.Data
Imports System.Collections.Generic

Namespace DotNetNuke.Modules.Reports.Converters

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Stores information about a single Converter instance
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	10/13/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Serializable()> _
    Public Class ConverterInstanceInfo

#Region " Private Fields "

        Private _FieldName As String
        Private _ConverterName As String
        Private _Arguments As String()

#End Region

#Region " Public Constructor "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Constructs an empty <see cref="ReportInfo"/> object
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
        End Sub

#End Region

#Region " Public Properties "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the name of the field affected by this Converter
        ''' </summary>
        ''' <returns>The name of the field affected by this Converter</returns>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property FieldName() As String
            Get
                Return _FieldName
            End Get
            Set(ByVal Value As String)
                _FieldName = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the name of the Converter to be applied
        ''' </summary>
        ''' <returns>The name of the Converter to be applied</returns>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ConverterName() As String
            Get
                Return _ConverterName
            End Get
            Set(ByVal Value As String)
                _ConverterName = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the arguments to be passed to the Converter
        ''' </summary>
        ''' <returns>The arguments to be passed to the Converter</returns>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Arguments() As String()
            Get
                Return _Arguments
            End Get
            Set(ByVal Value As String())
                _Arguments = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
