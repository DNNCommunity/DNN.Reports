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

Imports System.Data
Imports System.Data.Common

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Modules.Reports.Exceptions
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Reports.Extensions

    ''' <summary>
    ''' Contains complete path context for a Reports Module Extension
    ''' </summary>
    Public Class ExtensionContext

#Region " Private Fields "

        Private _extensionType As String
        Private _moduleFolder As String
        Private _moduleResourcesFolder As String
        Private _extensionsFolder As String
        Private _extensionFolder As String
        Private _extensionResourcesFolder As String

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' Gets the type of extension that this context was created for
        ''' </summary>
        Public ReadOnly Property ExtensionType() As String
            Get
                Return _extensionType
            End Get
        End Property

        ''' <summary>
        ''' Gets the web path to the module's root folder
        ''' </summary>
        Public ReadOnly Property ModuleFolder() As String
            Get
                Return _moduleFolder
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical path to the module's root folder
        ''' </summary>
        Public ReadOnly Property MappedModuleFolder() As String
            Get
                If HttpContext.Current Is Nothing Then Return String.Empty

                Return HttpContext.Current.Server.MapPath(_moduleFolder)
            End Get
        End Property

        ''' <summary>
        ''' Gets the web path to the module's App_LocalResources folder
        ''' </summary>
        Public ReadOnly Property ModuleResourcesFolder() As String
            Get
                Return _moduleResourcesFolder
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical path to the module's App_LocalResources folder
        ''' </summary>
        Public ReadOnly Property MappedModuleResourcesFolder() As String
            Get
                If HttpContext.Current Is Nothing Then Return String.Empty

                Return HttpContext.Current.Server.MapPath(_moduleResourcesFolder)
            End Get
        End Property

        ''' <summary>
        ''' Gets the web path to the folder containing all extensions of the same type
        ''' as the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property ExtensionsFolder() As String
            Get
                Return _extensionsFolder
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical path to the folder containing all extensions of the same type
        ''' as the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property MappedExtensionsFolder() As String
            Get
                If HttpContext.Current Is Nothing Then Return String.Empty

                Return HttpContext.Current.Server.MapPath(_extensionsFolder)
            End Get
        End Property

        ''' <summary>
        ''' Gets the web path to the folder containing the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property ExtensionFolder() As String
            Get
                Return _extensionFolder
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical path to the folder containing the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property MappedExtensionFolder() As String
            Get
                If HttpContext.Current Is Nothing Then Return String.Empty

                Return HttpContext.Current.Server.MapPath(_extensionFolder)
            End Get
        End Property

        ''' <summary>
        ''' Gets the web path to the App_LocalResources folder for the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property ExtensionResourcesFolder() As String
            Get
                Return _extensionResourcesFolder
            End Get
        End Property

        ''' <summary>
        ''' Gets the physical path to the App_LocalResources folder for the extension that this context was created for
        ''' </summary>
        Public ReadOnly Property MappedExtensionResourcesFolder() As String
            Get
                If HttpContext.Current Is Nothing Then Return String.Empty

                Return HttpContext.Current.Server.MapPath(_extensionResourcesFolder)
            End Get
        End Property

#End Region

#Region " Constructors "

        Public Sub New(ByVal moduleFolder As String, ByVal extensionType As String, ByVal extensionName As String)
            Me.New(moduleFolder, extensionType, String.Concat(extensionType, "s"), extensionName)
        End Sub

        Public Sub New(ByVal moduleFolder As String, ByVal extensionType As String, ByVal extensionTypeFolder As String, ByVal extensionName As String)
            Me.New(extensionType, _
                   moduleFolder, _
                   String.Format("{0}/{1}", moduleFolder, Localization.LocalResourceDirectory), _
                   String.Format("{0}/{1}", moduleFolder, extensionTypeFolder), _
                   String.Format("{0}/{1}/{2}", moduleFolder, extensionTypeFolder, extensionName), _
                   String.Format("{0}/{1}/{2}/{3}", _
                                 moduleFolder, _
                                 extensionTypeFolder, _
                                 extensionName, _
                                 Localization.LocalResourceDirectory))
        End Sub

        Public Sub New(ByVal extensionType As String, _
                       ByVal moduleFolder As String, _
                       ByVal moduleResourcesFolder As String, _
                       ByVal extensionsFolder As String, _
                       ByVal extensionFolder As String, _
                       ByVal extensionResourcesFolder As String)
            Me._extensionType = extensionType
            Me._moduleFolder = moduleFolder
            Me._moduleResourcesFolder = moduleResourcesFolder
            Me._extensionsFolder = extensionsFolder
            Me._extensionFolder = extensionFolder
            Me._extensionResourcesFolder = extensionResourcesFolder
        End Sub

#End Region

#Region " Public Methods "

        Public Function ResolveModulePath(ByVal path As String) As String
            Return String.Format("{0}/{1}", Me.ModuleFolder, path)
        End Function

        Public Function ResolveModuleResourcesPath(ByVal path As String) As String
            Return String.Format("{0}/{1}", Me.ModuleResourcesFolder, path)
        End Function

        Public Function ResolveExtensionsPath(ByVal path As String) As String
            Return String.Format("{0}/{1}", Me.ExtensionsFolder, path)
        End Function

        Public Function ResolveExtensionPath(ByVal path As String) As String
            Return String.Format("{0}/{1}", Me.ExtensionFolder, path)
        End Function

        Public Function ResolveExtensionResourcesPath(ByVal path As String) As String
            Return String.Format("{0}/{1}", Me.ExtensionResourcesFolder, path)
        End Function

#End Region

    End Class

End Namespace
