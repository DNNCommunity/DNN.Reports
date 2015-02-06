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
Imports System.IO
Imports System.Web.UI

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.UI.Utilities

Imports DotNetNuke.Security
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Modules.Reports.Exceptions

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Modules.Reports.Converters
Imports System.Web.Configuration
Imports DotNetNuke.Modules.Reports.Extensions
Imports DotNetNuke.Modules.Reports.DataSources
Imports DotNetNuke.Framework

Namespace DotNetNuke.Modules.Reports

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages Module Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region " Constants "

        Private Const DEFAULT_Visualizer As String = "Grid"
        Private Const DEFAULT_DataSource As String = "DotNetNuke"
        Private Const FILENAME_RESX_DataSource As String = "DataSource.ascx.resx"
        Private Const FILENAME_RESX_Visualizer As String = "Visualizer.ascx.resx"
        Private Const FILENAME_RESX_Settings As String = "Settings.ascx.resx"

#End Region

#Region " Properties "

        Private Property Report() As ReportInfo
            Get
                Return TryCast(ViewState("Report"), ReportInfo)
            End Get
            Set(ByVal value As ReportInfo)
                ViewState("Report") = value
            End Set
        End Property

#End Region

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            If Not Page.IsPostBack Then
                Report = ReportsController.GetReport(ModuleId, TabModuleId)
                If Report Is Nothing Then
                    Report = New ReportInfo()
                End If

                ' If the user has permission to see the Data Source Settings
                If CheckPermissions() Then

                    ' Load the fields
                    txtTitle.Text = Report.Title
                    txtDescription.Text = Report.Description
                    txtParameters.Text = Report.Parameters

                    ' Load the Data Source Settings
                    LoadExtensionSettings("DataSource", _
                                          Report.DataSource, _
                                          "DataSourceName.Text", _
                                          "DataSource.Text", _
                                          DEFAULT_DataSource, _
                                          Me.DataSourceDropDown, _
                                          Me.DataSourceSettings, _
                                          Me.DataSourceNotConfiguredView, _
                                          Report.DataSourceSettings, _
                                          FILENAME_RESX_DataSource, _
                                          True)

                    ' Load the filtering settings
                    Dim encodeBuilder As New StringBuilder
                    Dim decodeBuilder As New StringBuilder
                    For Each list As List(Of ConverterInstanceInfo) In Report.Converters.Values
                        For Each Converter As ConverterInstanceInfo In list
                            Dim builder As StringBuilder = Nothing
                            If "HtmlEncode".Equals(Converter.ConverterName) Then
                                builder = encodeBuilder
                            ElseIf "HtmlDecode".Equals(Converter.ConverterName) Then
                                builder = decodeBuilder
                            End If

                            If builder IsNot Nothing Then
                                If builder.Length > 0 Then
                                    builder.Append(",")
                                End If
                                builder.Append(Converter.FieldName)
                            End If
                        Next
                    Next
                    txtHtmlEncode.Text = encodeBuilder.ToString()
                    txtHtmlDecode.Text = decodeBuilder.ToString()
                End If

                txtCacheDuration.Text = Report.CacheDuration.ToString()
                chkShowInfoPane.Checked = Report.ShowInfoPane
                chkShowControls.Checked = Report.ShowControls
                chkAutoRunReport.Checked = Report.AutoRunReport
                chkTokenReplace.Checked = Report.TokenReplace

                ' Set the caching checkbox
                If Report.CacheDuration <= 0 Then
                    chkCaching.Checked = False
                    Report.CacheDuration = 0
                Else
                    chkCaching.Checked = True
                End If

                ' Update the cache duration text box visibility
                UpdateCachingSpan()

                ' Load Visualizer Settings
                LoadExtensionSettings("Visualizer", _
                                      Report.Visualizer, _
                                      "VisualizerName.Text", _
                                      "Visualizer.Text", _
                                      DEFAULT_Visualizer, _
                                      Me.VisualizerDropDown, _
                                      Me.VisualizerSettings, _
                                      Nothing, _
                                      Report.VisualizerSettings, _
                                      FILENAME_RESX_Visualizer, _
                                      False)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            ' Do not update report definition if the user is not a SuperUser
            If Me.UserInfo.IsSuperUser Then
                ' Update the settings
                UpdateDataSourceSettings()

                ' Save the report definition
                ReportsController.UpdateReportDefinition(ModuleId, Report)
            End If

            ' Non-SuperUsers can change TabModuleSettings (display settings)

            ' Update cache duration (0 => no caching)
            Dim duration As String = "0"
            If chkCaching.Checked Then
                duration = txtCacheDuration.Text
            End If
            Report.CacheDuration = Int32.Parse(duration)
            Report.ShowInfoPane = chkShowInfoPane.Checked
            Report.ShowControls = chkShowControls.Checked
            Report.AutoRunReport = chkAutoRunReport.Checked
            Report.TokenReplace = chkTokenReplace.Checked

            ' and Visualizer Settings
            Report.Visualizer = Me.VisualizerDropDown.SelectedValue()
            Report.VisualizerSettings.Clear()
            Dim settings As IReportsSettingsControl = GetSettingsControlFromView(VisualizerSettings.GetActiveView())
            If settings IsNot Nothing Then
                settings.SaveSettings(Report.VisualizerSettings)
            End If

            ' Save the report view and clear the cache
            ReportsController.UpdateReportView(TabModuleId, Report)

            ' refresh cache
            ModuleController.SynchronizeModule(Me.ModuleId)
            ReportsController.ClearCachedResults(Me.ModuleId)
        End Sub

#Region " Event Handlers "

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            ' Setup the extension lists
            VisualizerDropDown.Items.Clear()
            BuildExtensionList("Visualizer", FILENAME_RESX_Visualizer, "VisualizerName.Text", _
                               "Visualizer.Text", Me.VisualizerDropDown, Me.VisualizerSettings, True, False)

            BuildExtensionList("DataSource", FILENAME_RESX_DataSource, "DataSourceName.Text", _
                               "DataSource.Text", Me.DataSourceDropDown, Me.DataSourceSettings, True, True)

            ' Register Confirm Messages
            DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(btnTestDataSource, _
                Localization.GetString("btnTestDataSource.Confirm", Me.LocalResourceFile))
            DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(btnShowXml, _
                Localization.GetString("btnTestDataSource.Confirm", Me.LocalResourceFile))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Add module.css because it isn't loaded by default here (since the current module
            ' is "Admin/Modules"
            Dim cssId As String = Common.Globals.CreateValidID( _
                String.Format("{0}/{1}", Common.Globals.ApplicationPath, Me.TemplateSourceDirectory))
            DirectCast(Page, CDefault).AddStyleSheet(cssId, ResolveUrl("module.css"))

            ' Update the selected extension on postback
            If IsPostBack Then
                DisplaySelectedExtension("Visualizer", Me.VisualizerDropDown, _
                                         Me.VisualizerSettings, Nothing, _
                                         Report.VisualizerSettings)
                DisplaySelectedExtension("DataSource", Me.DataSourceDropDown, _
                                         Me.DataSourceSettings, Me.DataSourceNotConfiguredView, _
                                         Report.DataSourceSettings)
                Dim haveDataSource As Boolean = Not String.IsNullOrEmpty(Me.DataSourceDropDown.SelectedValue)
                Me.btnTestDataSource.Visible = haveDataSource
                Me.btnShowXml.Visible = haveDataSource

                If "True".Equals(WebConfigurationManager.AppSettings(ReportsController.APPSETTING_AllowCachingWithParameters)) Then
                    CacheWarningLabel.Attributes("ResourceKey") = "CacheWithParametersEnabled.Text"
                End If
            End If

            ' Register ClientAPI Functionality
            If Not ReportsClientAPI.IsSupported Then
                chkCaching.AutoPostBack = True
                AddHandler chkCaching.CheckedChanged, AddressOf chkCaching_CheckedChanged
            Else
                ReportsClientAPI.Import(Me.Page)
                ReportsClientAPI.ShowHideByCheckBox(Me.Page, chkCaching, spanCacheDuration)
            End If

            ' Perform a server-side update of the caching text box span
            UpdateCachingSpan()
        End Sub

        Private Sub chkCaching_CheckedChanged(ByVal sender As Object, ByVal args As EventArgs)
            UpdateCachingSpan()
        End Sub

        Protected Sub btnShowXml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowXml.Click
            rowXmlSource.Visible = False
            rowResults.Visible = False

            Try
                ' Update data source settings
                UpdateDataSourceSettings()

                ' Execute the DataSource
                Dim results As DataTable = ReportsController.ExecuteReport(Report, Nothing, _
                                                                           True, Me)

                ' Serialize the results to Xml
                Dim writer As New StringWriter
                results.WriteXml(writer)
                txtXmlSource.Text = writer.ToString()
                rowXmlSource.Visible = True
            Catch ex As DataSourceException
                ' Format an error message
                lblQueryResults.Text = String.Format(Localization.GetString("TestDSFail.Message", Me.LocalResourceFile), ex.LocalizedMessage)
                lblQueryResults.CssClass = "NormalRed"
                imgQueryResults.ImageUrl = "~/images/red-error.gif"
                rowResults.Visible = True
                rowXmlSource.Visible = False
            End Try
        End Sub

        Protected Sub btnHideXmlSource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHideXmlSource.Click
            rowXmlSource.Visible = False
        End Sub

        Protected Sub btnTestQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTestDataSource.Click
            rowXmlSource.Visible = False
            rowResults.Visible = False

            Try
                ' Update data source settings
                UpdateDataSourceSettings()

                ' Execute the DataSource
                Dim results As DataTable = ReportsController.ExecuteReport(Report, Nothing, _
                                                                           True, Me)

                ' Format a success message
                lblQueryResults.Text = String.Format(Localization.GetString("TestDSSuccess.Message", _
                                                                            Me.LocalResourceFile), _
                                                     results.Rows.Count)
                lblQueryResults.CssClass = "NormalBold"
                imgQueryResults.ImageUrl = "~/images/green-ok.gif"
            Catch ex As DataSourceException
                ' Format an error message
                lblQueryResults.Text = String.Format(Localization.GetString("TestDSFail.Message", Me.LocalResourceFile), ex.LocalizedMessage)
                lblQueryResults.CssClass = "NormalRed"
                imgQueryResults.ImageUrl = "~/images/red-error.gif"
            End Try

            ' Display the results/error message
            rowResults.Visible = True
        End Sub

        Protected Sub btnHideTestResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHideTestResults.Click
            rowResults.Visible = False
        End Sub

#End Region

#Region " Private Methods "

        Private Sub UpdateDataSourceSettings()
            ' Load the data source settings into the report
            Dim security As New PortalSecurity
            Report.Title = txtTitle.Text
            Report.Description = security.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoScripting)
            Report.Parameters = txtParameters.Text
            Report.CreatedBy = Me.UserId
            Report.CreatedOn = DateTime.Now
            Report.DataSource = Me.DataSourceDropDown.SelectedValue

            ' Get the active data source settings control
            Dim activeDataSource As IDataSourceSettingsControl = _
                TryCast(GetSettingsControlFromView(Me.DataSourceSettings.GetActiveView()),  _
                        IDataSourceSettingsControl)

            ' If there is an active data source, save its settings
            If activeDataSource IsNot Nothing Then
                Report.DataSourceClass = activeDataSource.DataSourceClass
                activeDataSource.SaveSettings(Report.DataSourceSettings)
            End If

            ' Update Converter settings
            Report.Converters.Clear()
            If Not String.IsNullOrEmpty(txtHtmlDecode.Text.Trim) Then
                For Each field In txtHtmlDecode.Text.Split(","c)
                    Dim newConverter As New ConverterInstanceInfo()
                    newConverter.FieldName = field
                    newConverter.ConverterName = "HtmlDecode"
                    newConverter.Arguments = Nothing
                    ConverterUtils.AddConverter(Report.Converters, newConverter)
                Next
            End If

            If Not String.IsNullOrEmpty(txtHtmlEncode.Text.Trim) Then
                For Each field In txtHtmlEncode.Text.Split(","c)
                    Dim newConverter As New ConverterInstanceInfo()
                    newConverter.FieldName = field
                    newConverter.ConverterName = "HtmlEncode"
                    newConverter.Arguments = Nothing
                    ConverterUtils.AddConverter(Report.Converters, newConverter)
                Next
            End If
        End Sub

        Private Sub LoadExtensionSettings(ByVal extensionType As String, _
                                          ByRef extensionName As String, _
                                          ByVal nameResourceKey As String, _
                                          ByVal typeResourceKey As String, _
                                          ByVal defaultExtension As String, _
                                          ByVal dropDown As DropDownList, _
                                          ByVal multiView As MultiView, _
                                          ByVal notConfiguredView As View, _
                                          ByVal extensionSettings As Dictionary(Of String, String), _
                                          ByVal resxFileName As String, _
                                          ByVal buildNotSpecifiedItem As Boolean)
            ' Build the list of Data Sources
            BuildExtensionList(extensionType, resxFileName, nameResourceKey, _
                               typeResourceKey, dropDown, multiView, True, buildNotSpecifiedItem)

            ' Check that the Report has a Data Source, if not, use the default
            If String.IsNullOrEmpty(extensionName) Then
                extensionName = defaultExtension
            End If

            ' Find that data source and select it
            Dim extensionItem As ListItem = dropDown.Items.FindByValue(extensionName)
            If extensionItem IsNot Nothing Then
                extensionItem.Selected = True
                DisplaySelectedExtension(extensionType, dropDown, multiView, notConfiguredView, extensionSettings)
            End If
        End Sub

        Private Sub DisplaySelectedExtension(ByVal extensionType As String, _
                                             ByVal dropDown As DropDownList, _
                                             ByVal multiView As MultiView, _
                                             ByVal notConfiguredView As View, _
                                             ByVal extensionSettings As Dictionary(Of String, String))
            ' Get the new active view name
            Dim newActiveViewName As String = Null.NullString
            If Not String.IsNullOrEmpty(dropDown.SelectedValue) Then
                newActiveViewName = GetExtensionViewName(dropDown.SelectedValue, extensionType)
            End If

            ' Get the current active view
            Dim activeView As View = multiView.GetActiveView()
            If activeView IsNot Nothing AndAlso _
               activeView.ID.Equals(newActiveViewName, StringComparison.OrdinalIgnoreCase) Then
                Return ' If current and new active view are the same, just return
            End If

            ' Get the new active view
            Dim newActiveView As View = Nothing
            If String.IsNullOrEmpty(newActiveViewName) Then
                ' If <No Visualizer/DataSource> is selected, use the not configured view
                newActiveView = notConfiguredView
                If newActiveView Is Nothing Then
                    newActiveView = multiView.Views(0)
                End If
            Else
                ' Otherwise, find the view with the new active view name
                newActiveView = DirectCast(multiView.FindControl(newActiveViewName), View)
            End If

            ' Set that view as the active view
            multiView.SetActiveView(newActiveView)

            ' Get the settings control in the new active view
            Dim settingsControl As IReportsSettingsControl = GetSettingsControlFromView(newActiveView)

            ' If we successfully got it, load its settings
            If settingsControl IsNot Nothing Then
                settingsControl.LoadSettings(extensionSettings)
            End If
        End Sub

        Private Sub BuildExtensionList(ByVal extensionType As String, _
                                       ByVal resxFileName As String, _
                                       ByVal nameResourceKey As String, _
                                       ByVal typeResourceKey As String, _
                                       ByVal dropDown As DropDownList, _
                                       ByVal multiView As MultiView, _
                                       ByVal buildView As Boolean, _
                                       ByVal buildNotSpecifiedItem As Boolean)
            ' Map the root physical path by using this control's location
            Dim rootPhysicalPath As String = Server.MapPath(Me.TemplateSourceDirectory)

            ' Load all the Settings.ascx files for the Extension
            Dim extTypeFolder As String = String.Concat(extensionType, "s")
            Dim exts As IEnumerable(Of String) = _
                Utilities.GetExtensions(rootPhysicalPath, extTypeFolder)

            ' Build the Drop down list
            dropDown.Items.Clear()
            If buildNotSpecifiedItem Then
                dropDown.Items.Add(New ListItem( _
                    Localization.GetString(String.Format("No{0}.Text", extensionType), Me.LocalResourceFile), _
                    String.Empty))
            End If
            buildView = buildView AndAlso multiView.Views.Count <= 1
            For Each ext As String In exts
                ' Resolve the extension path
                Dim extPath = String.Format("{0}/{1}", extTypeFolder, ext)

                ' Locate the Local Resource File
                Dim lrWeb As String = String.Format("{0}/{1}/{2}", _
                                                    extPath, _
                                                    Localization.LocalResourceDirectory, _
                                                    resxFileName)
                Dim localResourceFile As String = Me.ResolveUrl(lrWeb)
                If Not File.Exists(Server.MapPath(localResourceFile)) Then Continue For

                ' Locate the Settings control
                Dim ctrlPath As String = Me.ResolveUrl( _
                    String.Format("{0}/{1}/{2}", extTypeFolder, ext, "Settings.ascx"))
                If Not File.Exists(Server.MapPath(ctrlPath)) Then Continue For

                ' Get the name of the data source
                Dim extName As String = Localization.GetString(nameResourceKey, localResourceFile)

                ' Construct the settings control
                Dim ctrl As Control
                Try
                    ctrl = Me.LoadControl(ctrlPath)
                Catch ex As HttpException
                    Continue For
                End Try
                Dim rptExt As IReportsExtension = TryCast(ctrl, IReportsExtension)
                Dim rptCtrl As IReportsControl = TryCast(ctrl, IReportsControl)

                ' Validate implemented interfaces
                If rptCtrl Is Nothing Then Continue For
                If rptExt Is Nothing Then Continue For
                If Not TypeOf ctrl Is IReportsSettingsControl Then Continue For

                ' Construct an extension context
                Dim ctxt As New ExtensionContext(Me.TemplateSourceDirectory, extensionType, ext)

                ' Set properties and initialize extension
                rptExt.Initialize(ctxt)
                ctrl.ID = GetExtensionControlName(ext, extensionType)
                rptCtrl.ParentModule = Me

                ' Don't build the view unless we're asked to AND
                ' the views haven't already been built
                If buildView Then
                    ' Create a View to hold the control
                    Dim view As New View()
                    view.ID = GetExtensionViewName(ext, extensionType)
                    view.Controls.Add(ctrl)

                    ' Add the view to the multi view
                    multiView.Views.Add(view)
                End If

                ' Get the full text for the drop down list item
                Dim itemText As String = String.Format(Localization.GetString(typeResourceKey, _
                                                                              Me.LocalResourceFile), _
                                                       extName)

                ' Create the dropdown list items
                dropDown.Items.Add(New ListItem(itemText, ext))
                'End Try
            Next
        End Sub

        Private Function GetExtensionControlName(ByVal extension As String, ByVal type As String) As String
            Return String.Format("{0}{1}Settings", Utilities.RemoveSpaces(extension), type)
        End Function

        Private Function GetExtensionViewName(ByVal extension As String, ByVal type As String) As String
            Return String.Format("{0}{1}SettingsView", Utilities.RemoveSpaces(extension), type)
        End Function

        Private Function GetSettingsControlFromView(ByVal view As View) As IReportsSettingsControl
            ' Return the first control inside that view
            Return TryCast(view.Controls(0), IReportsSettingsControl)
        End Function

        Private Function CheckPermissions() As Boolean
            If Not Me.UserInfo.IsSuperUser Then
                ReportsSettingsMultiView.SetActiveView(AccessDeniedView)
                Return False
            Else
                ReportsSettingsMultiView.SetActiveView(SuperUserView)
                Return True
            End If
        End Function

        Protected Sub VisualizerDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisualizerDropDown.SelectedIndexChanged
            Report.Visualizer = VisualizerDropDown.SelectedValue
            ReportsController.LoadExtensionSettings(ModuleSettings, TabModuleSettings, Report)
            DisplaySelectedExtension("Visualizer", Me.VisualizerDropDown, Me.VisualizerSettings, _
                                     Nothing, Report.VisualizerSettings)
        End Sub

        Protected Sub DataSourceDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataSourceDropDown.SelectedIndexChanged
            Report.DataSource = DataSourceDropDown.SelectedValue
            ReportsController.LoadExtensionSettings(ModuleSettings, TabModuleSettings, Report)
            DisplaySelectedExtension("DataSource", Me.DataSourceDropDown, Me.DataSourceSettings, _
                                     Me.DataSourceNotConfiguredView, Report.DataSourceSettings)
        End Sub

        Private Sub UpdateCachingSpan()
            If chkCaching.Checked Then
                spanCacheDuration.Style(HtmlTextWriterStyle.Display) = String.Empty
            Else
                spanCacheDuration.Style(HtmlTextWriterStyle.Display) = "none"
            End If
        End Sub

#End Region

    End Class

End Namespace

