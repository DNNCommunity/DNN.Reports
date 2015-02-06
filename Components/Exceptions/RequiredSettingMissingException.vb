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

Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.Exceptions

    ''' <summary>
    ''' Exception thrown when an error occurs during the execution of a report because the
    ''' data source is not properly configured
    ''' </summary>
    Public Class RequiredSettingMissingException
        Inherits DataSourceException

        Private _setting As String

        Public ReadOnly Property Setting() As String
            Get
                Return _setting
            End Get
        End Property

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal setting As String, ByVal extensionContext As ExtensionContext)
            MyBase.New(New LocalizedText("MissingSetting.Text", _
                                         extensionContext.ResolveModuleResourcesPath("ViewReports.ascx.resx"), _
                                         setting), _
                       String.Format("Could not connect to data source, the following required setting is not set: {0}", setting))
            Me._setting = setting
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
            _setting = info.GetString("Setting")
        End Sub

        Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.GetObjectData(info, context)
            info.AddValue("Setting", _setting, GetType(String))
        End Sub

    End Class

End Namespace
