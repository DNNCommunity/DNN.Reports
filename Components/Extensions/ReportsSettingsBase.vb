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

Namespace DotNetNuke.Modules.Reports.Extensions

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Provides an abstract base class for reports module settings controls
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	01/15/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ReportsSettingsBase
        Inherits ReportsControlBase
        Implements IReportsSettingsControl

        Public Const FILENAME_SettingsASCX As String = "Settings.ascx"

        Protected Overrides ReadOnly Property ASCXFileName() As String
            Get
                Return FILENAME_SettingsASCX
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads settings from the specified settings dictionary into the visualizer
        ''' settings UI
        ''' </summary>
        ''' <param name="Settings">The settings dictionary to load into the UI</param>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overridable Sub LoadSettings(ByVal Settings As Dictionary(Of String, String)) Implements IReportsSettingsControl.LoadSettings

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Save settings to the specified settings dictionary from the visualizer
        ''' settings UI
        ''' </summary>
        ''' <param name="Settings">The settings dictionary to save from the UI</param>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overridable Sub SaveSettings(ByVal Settings As Dictionary(Of String, String)) Implements IReportsSettingsControl.SaveSettings

        End Sub

    End Class

End Namespace
