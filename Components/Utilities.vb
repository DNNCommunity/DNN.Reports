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
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals

Imports System.Drawing
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports UISkin = DotNetNuke.UI.Skins.Skin
Imports System
Imports System.Collections
Imports System.Globalization
Imports System.IO
Imports DotNetNuke.Modules.Reports.Converters

Namespace DotNetNuke.Modules.Reports

    Public Module ArrayUtils
        Public Function Contains(ByVal arr As Array, ByVal elem As Object) As Boolean
            Return Array.IndexOf(arr, elem) <> -1
        End Function
    End Module

    Public Module ConverterUtils
        Public Sub AddConverter(ByVal Converters As IDictionary(Of String, IList(Of ConverterInstanceInfo)), ByVal Converter As ConverterInstanceInfo)
            If String.IsNullOrEmpty(Converter.FieldName.Trim) Then Return
            If Not Converters.ContainsKey(Converter.FieldName) OrElse _
                            Converters(Converter.FieldName) Is Nothing Then
                Converters(Converter.FieldName) = New List(Of ConverterInstanceInfo)
            End If
            Converters(Converter.FieldName).Add(Converter)
        End Sub
    End Module

    Public Module DropDownUtils
        Public Sub TrySetValue(ByVal DropDown As DropDownList, ByVal TryValue As String, ByVal DefValue As String)
            If DropDown.Items.FindByValue(TryValue) Is Nothing Then
                DropDown.SelectedValue = DefValue
            Else
                DropDown.SelectedValue = TryValue
            End If
        End Sub
    End Module

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SettingsUtil Module provides utilities for accessing settings
    ''' </summary>
    ''' <history>
    '''     [anurse]     08/31/2006    Documented
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Module SettingsUtil

        Public Function FieldIsNotSet(ByVal settings As Dictionary(Of String, String), ByVal setting As String) As Boolean
            Return Not settings.ContainsKey(setting) OrElse String.IsNullOrEmpty(settings.Item(setting))
        End Function

        ' Gets a setting from the dictionary provided, or returns defValue if the setting is null
        Public Function GetDictionarySetting(Of T)(ByVal settings As Dictionary(Of String, String), ByVal key As String, ByVal defValue As T) As T
            If Not settings.ContainsKey(key) Then Return defValue
            Dim sVal As String = settings(key)
            Dim retValue As T = defValue
            If Not String.IsNullOrEmpty(sVal) Then
                Try
                    retValue = DirectCast(Convert.ChangeType(sVal, GetType(T)), T)
                Catch ex As Exception
                    retValue = defValue
                End Try
            End If
            Return retValue
        End Function

        ' Gets a setting from the hashtable provided, or returns defValue if the setting is null
        Public Function GetHashtableSetting(Of T)(ByVal settings As IDictionary, ByVal key As String, ByVal defValue As T) As T
            If Not settings.Contains(key) Then Return defValue
            Dim o As Object = settings(key)
            Dim retValue As T = defValue
            If o IsNot Nothing Then
                Try
                    retValue = DirectCast(Convert.ChangeType(o, GetType(T)), T)
                Catch ex As Exception
                    retValue = defValue
                End Try
            End If
            Return retValue
        End Function

    End Module

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SettingsUtil Module provides general utilities
    ''' </summary>
    ''' <history>
    '''     [anurse]     08/31/2006    Documented
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Module Utilities

        Public Function RemoveSpaces(ByVal Str As String) As String
            ' Regex lets us remove all whitespace, not just " "
            Return Regex.Replace(Str, "\s+", "")
        End Function

        Public Function MapFileIdPath(ByVal PortalSettings As PortalSettings, ByVal FileId As String) As String
            
            If Not String.IsNullOrEmpty(FileId) Then

                ' There are 2 different ways the filename is returned, can start file fileid= and a filenumber
                If FileId.ToLower().StartsWith("fileid=", StringComparison.OrdinalIgnoreCase) Then
                    Dim intFileId As Integer = Int32.Parse(FileId.Substring(7))
                    Return MapFileIdPath(PortalSettings, intFileId)
                Else
                    ' Or is is a real filename
                    Return String.Concat(PortalSettings.HomeDirectoryMapPath, FileId).Replace("/", "\")
                End If
            Else
                Return String.Empty
            End If

        End Function

        Public Function MapFileIdPath(ByVal PortalSettings As PortalSettings, ByVal FileId As Integer) As String
            Dim objFileCtrl As New DotNetNuke.Services.FileSystem.FileController()
            Dim objFile As DotNetNuke.Services.FileSystem.FileInfo = objFileCtrl.GetFileById(FileId, PortalSettings.PortalId)
            Dim sProtectedExtension As String = String.Empty
            If objFile.StorageLocation = DotNetNuke.Services.FileSystem.FolderController.StorageLocationTypes.DatabaseSecure Then
                Return String.Empty
            ElseIf objFile.StorageLocation = DotNetNuke.Services.FileSystem.FolderController.StorageLocationTypes.SecureFileSystem Then
                sProtectedExtension = DotNetNuke.Common.Globals.glbProtectedExtension
            End If
            Return String.Concat(PortalSettings.HomeDirectoryMapPath, objFile.Folder, objFile.FileName, sProtectedExtension)
        End Function

        Public Function ParseColorString(ByVal colorString As String) As Color
            Dim bIsShortString As Boolean = False
            If String.IsNullOrEmpty(colorString) Then Return Color.Black
            If colorString.StartsWith("#") Then colorString = colorString.Substring(1)
            If colorString.Length = 3 Then
                bIsShortString = True
            ElseIf colorString.Length <> 6 Then
                Return Color.Black
            End If

            Dim clr As Color = Color.Black
            Try
                Dim iR As Integer = ParseHexCode(colorString, bIsShortString, 0)
                Dim iG As Integer = ParseHexCode(colorString, bIsShortString, 1)
                Dim iB As Integer = ParseHexCode(colorString, bIsShortString, 2)
                clr = Color.FromArgb(iR, iG, iB)
            Catch fe As FormatException
                Return Color.Black
            End Try
            Return clr
        End Function

        Private Function ParseHexCode(ByVal colorString As String, ByVal bIsShortString As Boolean, ByVal Position As Integer) As Integer
            Dim len As Integer = 2
            If bIsShortString Then
                len = 1
            Else
                Position *= 2
            End If
            Return Int32.Parse(colorString.Substring(Position, len), NumberStyles.HexNumber)
        End Function

        Friend Function GetExtensions(ByVal rootPhysicalPath As String, _
                                               ByVal extensionFolder As String) As IEnumerable(Of String)
            ' Initialize the list
            Dim exts As New List(Of String)()

            ' Get the extension sub-directory and verify its existance
            Dim extensionDir As DirectoryInfo = New DirectoryInfo( _
                Path.Combine(rootPhysicalPath, extensionFolder))
            If Not extensionDir.Exists Then Return exts ' No extensions to load

            ' Iterate across the subdirectories
            For Each dir As DirectoryInfo In extensionDir.GetDirectories()
                exts.Add(dir.Name)
            Next

            Return exts
        End Function

        Friend Function GetExtensionFile(ByVal rootPhysicalPath As String, _
                                                ByVal extensionFolder As String, _
                                                ByVal extensionName As String, _
                                                ByVal path As String) As String
            ' Get the extension sub-directory and verify its existance
            Dim extensionDir As DirectoryInfo = New DirectoryInfo( _
                IO.Path.Combine(IO.Path.Combine(rootPhysicalPath, extensionFolder), extensionName))
            If Not extensionDir.Exists Then Return Nothing ' Extension doesn't exist

            ' Find the path
            Return IO.Path.Combine(extensionDir.FullName, path)
        End Function


        Friend Function LoadExtensionControl(Of TControlType As Class) _
                                                   (ByVal rootPhysicalPath As String, _
                                                    ByVal extensionFolder As String, _
                                                    ByVal extensionName As String, _
                                                    ByVal controlName As String, _
                                                    ByVal parentControl As TemplateControl) _
                                                    As TControlType
            ' Find the control path and verify its existance
            Dim ctrlPath As String = GetExtensionFile(rootPhysicalPath, extensionFolder, _
                                                      extensionName, "Settings.ascx")
            If Not File.Exists(ctrlPath) Then Return Nothing ' Control doesn't exist

            ' Load the control and return it
            Return TryCast(parentControl.LoadControl(ctrlPath), TControlType)
        End Function

        Public Function GetGridLinesSetting(ByVal VisualizerSettings As Dictionary(Of String, String)) As GridLines
            Dim gridLines As String = SettingsUtil.GetDictionarySetting(Of String)(VisualizerSettings, ReportsController.SETTING_Grid_GridLines, ReportsController.DEFAULT_Grid_GridLines)
            If Boolean.TrueString.Equals(gridLines, StringComparison.InvariantCultureIgnoreCase) Then
                Return WebControls.GridLines.Both
            ElseIf Boolean.FalseString.EndsWith(gridLines, StringComparison.InvariantCultureIgnoreCase) Then
                Return WebControls.GridLines.None
            Else
                Return DirectCast([Enum].Parse(GetType(GridLines), gridLines), GridLines)
            End If
        End Function

    End Module

End Namespace