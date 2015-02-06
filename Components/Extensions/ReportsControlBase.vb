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

Imports System.Diagnostics
Imports DotNetNuke.Framework
Imports System.Web.UI

Namespace DotNetNuke.Modules.Reports.Extensions

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Abstract base class for reports module "sub-controls" (Visualizer
    ''' Controls, etc.)
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	01/15/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class ReportsControlBase
        Inherits UserControlBase
        Implements IReportsControl

        Private _context As ExtensionContext
        Private _parentModule As PortalModuleBase
        Private _localResourceFile As String

        Public Property ParentModule() As PortalModuleBase Implements IReportsControl.ParentModule
            Get
                If _parentModule Is Nothing Then
                    FindParentModule()
                End If
                Return _parentModule
            End Get
            Set(ByVal value As PortalModuleBase)
                _parentModule = value
            End Set
        End Property

        Public ReadOnly Property ExtensionContext() As ExtensionContext
            Get
                Return _context
            End Get
        End Property

        Protected Friend ReadOnly Property ModuleId() As Integer
            Get
                Debug.Assert(_parentModule IsNot Nothing, "Cannot access ModuleId if ParentModule is not set")
                Return _parentModule.ModuleId
            End Get
        End Property

        Protected Friend ReadOnly Property TabModuleId() As Integer
            Get
                Debug.Assert(_parentModule IsNot Nothing, "Cannot access ModuleId if ParentModule is not set")
                Return _parentModule.TabModuleId
            End Get
        End Property

        Public Property LocalResourceFile() As String
            Get
                If String.IsNullOrEmpty(_localResourceFile) Then
                    _localResourceFile = Me.TemplateSourceDirectory & "/" & Services.Localization.Localization.LocalResourceDirectory & "/" & ASCXFileName
                End If
                Return _localResourceFile
            End Get
            Set(ByVal value As String)
                _localResourceFile = value
            End Set
        End Property

        Protected MustOverride ReadOnly Property ASCXFileName() As String

        Private Sub FindParentModule()
            ' Iterate up the parent tree to find the parent
            Dim ctrlCurrent As Control = Me.Parent
            While ctrlCurrent IsNot Nothing AndAlso Not (TypeOf ctrlCurrent Is PortalModuleBase)
                If ctrlCurrent.Equals(ctrlCurrent.Parent) Then Return

                ctrlCurrent = ctrlCurrent.Parent
            End While
            If ctrlCurrent IsNot Nothing Then
                _parentModule = TryCast(ctrlCurrent, PortalModuleBase)
            End If
        End Sub

        Public Overridable Sub Initialize(ByVal context As ExtensionContext) Implements IReportsExtension.Initialize
            Me._context = context
        End Sub

    End Class

End Namespace
