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

Imports DotNetNuke.Services.Localization

Imports DotNetNuke.Services.Installer
Imports DotNetNuke.Services.Installer.Packages

Namespace DotNetNuke.Modules.Reports

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Packages class manages Module AddIns
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Packages
        Inherits Entities.Modules.PortalModuleBase
        Private Shared ReadOnly BuiltInVersion As Version = BuiltInVersion

        Public Sub Page_Load(ByVal sender As Object, ByVal args As EventArgs) Handles Me.Load
            InstallVisualizerLink.NavigateUrl = _
                Util.InstallURL(Me.TabId, ReportsController.PACKAGETYPE_Visualizer)
            InstallDataSourceLink.NavigateUrl = _
                Util.InstallURL(Me.TabId, ReportsController.PACKAGETYPE_DataSource)

            Localization.LocalizeGridView(PackagesGrid, LocalResourceFile)

            If Not IsPostBack Then
                DataBind()
            End If

            Localization.LocalizeGridView(PackagesGrid, LocalResourceFile)
        End Sub

        Public Overrides Sub DataBind()
            MyBase.DataBind()

            Dim visualizers As IList(Of PackageInfo) = PackageController.GetPackagesByType(ReportsController.PACKAGETYPE_Visualizer)
            Dim dataSources As IList(Of PackageInfo) = PackageController.GetPackagesByType(ReportsController.PACKAGETYPE_DataSource)
            Dim allExtensions As New List(Of PackageInfo)(visualizers)
            allExtensions.AddRange(dataSources)

            ' Add Built-in packages
            allExtensions.Add(CreateBuiltInPackage("Grid", BuiltInVersion, ReportsController.PACKAGETYPE_Visualizer, "Grid"))
            allExtensions.Add(CreateBuiltInPackage("HTML", BuiltInVersion, ReportsController.PACKAGETYPE_Visualizer, "HTML"))
            allExtensions.Add(CreateBuiltInPackage("XSLT", BuiltInVersion, ReportsController.PACKAGETYPE_Visualizer, "XSLT"))
            allExtensions.Add(CreateBuiltInPackage("Generic ADO.Net Provider", BuiltInVersion, ReportsController.PACKAGETYPE_DataSource, "ADO"))
            allExtensions.Add(CreateBuiltInPackage("DotNetNuke", BuiltInVersion, ReportsController.PACKAGETYPE_DataSource, "DNN"))
            allExtensions.Add(CreateBuiltInPackage("Microsoft SQL Server", BuiltInVersion, ReportsController.PACKAGETYPE_DataSource, "SqlServer"))

            PackagesGrid.DataSource = allExtensions
            PackagesGrid.DataBind()

        End Sub

        Private Function CreateBuiltInPackage(ByVal Name As String, ByVal Version As Version, ByVal Type As String, ByVal DescriptionKey As String) As PackageInfo
            Dim pkg As New PackageInfo()
            pkg.PackageID = -1
            pkg.FriendlyName = Name
            pkg.Version = Version
            pkg.PackageType = Type
            pkg.Description = Localization.GetString(String.Concat(DescriptionKey, ".Description"), LocalResourceFile)
            Return pkg
        End Function

        Protected Function GetImage(ByVal id As Integer) As String
            If id < 0 Then
                Return ResolveUrl("images/BuiltInPackage.gif")
            Else
                Return ResolveUrl("~/images/delete.gif")
            End If
        End Function

        Protected Function IsBuiltIn(ByVal id As Integer) As Boolean
            Return id < 0
        End Function

        Protected Function StripPrefix(ByVal value As String) As String
            Return value.Substring(ReportsController.PACKAGETYPE_Prefix.Length)
        End Function

        Protected Function InstallUrl(ByVal id As Integer) As String
            Return Util.UnInstallURL(Me.TabId, id)
        End Function

    End Class

End Namespace
