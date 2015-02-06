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
Imports DotNetNuke.Modules.Reports.Converters

Namespace DotNetNuke.Modules.Reports

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Stores information about a single report that can be displayed by the module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	06/16/2006	Created
    '''     [anurse]    08/02/2007  Added DataSource-related properties and removed Query property
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Serializable()> _
    Public Class ReportInfo

#Region " Private Fields "

        Private _ModuleID As Integer
        Private _Title As String
        Private _Description As String
        Private _Parameters As String
        Private _CacheDuration As Integer
        Private _CreatedBy As Integer
        Private _CreatedOn As DateTime
        Private _Converters As New Dictionary(Of String, IList(Of ConverterInstanceInfo))
        Private _Visualizer As String
        Private _VisualizerSettings As New Dictionary(Of String, String)
        Private _DataSource As String
        Private _DataSourceClass As String
        Private _DataSourceSettings As New Dictionary(Of String, String)
        Private _ShowInfoPane As Boolean
        Private _ShowControls As Boolean
        Private _AutoRunReport As Boolean
        Private _TokenReplace As Boolean


#End Region

#Region " Public Constructor "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Constructs an empty <see cref="ReportInfo"/> object
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
        End Sub

#End Region

#Region " Public Properties "

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the module ID of the report
        ''' </summary>
        ''' <returns>The module ID of the report</returns>
        ''' <history>
        ''' 	[anurse]	06/19/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ModuleID() As Integer
            Get
                Return _ModuleID
            End Get
            Set(ByVal Value As Integer)
                _ModuleID = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the title of the report
        ''' </summary>
        ''' <returns>The title of the report</returns>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a short description of the report
        ''' </summary>
        ''' <returns>A short description of the report</returns>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a comma separated list of URL parameters used by the report
        ''' </summary>
        ''' <returns>A comma separated list of URL parameters used by the report</returns>
        ''' <history>
        ''' 	[anurse]	04/15/2009	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Parameters() As String
            Get
                Return _Parameters
            End Get
            Set(ByVal Value As String)
                _Parameters = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the cache duration for the report
        ''' </summary>
        ''' <returns>The number of minutes to cache the report results</returns>
        ''' <history>
        ''' 	[anurse]	01/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CacheDuration() As Integer
            Get
                Return _CacheDuration
            End Get
            Set(ByVal value As Integer)
                _CacheDuration = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the user ID of the user who created this report
        ''' </summary>
        ''' <returns>The user ID of the user who created this report</returns>
        ''' <history>
        ''' 	[anurse]	06/16/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CreatedBy() As Integer
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As Integer)
                _CreatedBy = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the date that this report was created
        ''' </summary>
        ''' <returns>The date that this report was created</returns>
        ''' <history>
        ''' 	[anurse]	06/19/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property CreatedOn() As DateTime
            Get
                Return _CreatedOn
            End Get
            Set(ByVal value As DateTime)
                _CreatedOn = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a collection of Converters keyed by field name
        ''' </summary>
        ''' <returns>A collection of Converters keyed by field name</returns>
        ''' <history>
        ''' 	[anurse]	10/13/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property Converters() As IDictionary(Of String, IList(Of ConverterInstanceInfo))
            Get
                Return _Converters
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the name of the data source for this report
        ''' </summary>
        ''' <returns>The name of the data source for this report</returns>
        ''' <history>
        ''' 	[anurse]	08/02/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property DataSource() As String
            Get
                Return _DataSource
            End Get
            Set(ByVal Value As String)
                _DataSource = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the name of the class containing the data source for this report
        ''' </summary>
        ''' <returns>The name of the class containing the data source for this report</returns>
        ''' <history>
        ''' 	[anurse]	08/04/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property DataSourceClass() As String
            Get
                Return _DataSourceClass
            End Get
            Set(ByVal Value As String)
                _DataSourceClass = Value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a table of name-value pairs representing the settings
        ''' associated with the data source for this report
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	08/02/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property DataSourceSettings() As Dictionary(Of String, String)
            Get
                Return _DataSourceSettings
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _DataSourceSettings = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets the name of the visualizer to use to display this
        ''' report
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	01/07/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property Visualizer() As String
            Get
                Return _Visualizer
            End Get
            Set(ByVal value As String)
                _Visualizer = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a table of name-value pairs representing the settings
        ''' associated with the visualizer for this report
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	01/07/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property VisualizerSettings() As Dictionary(Of String, String)
            Get
                Return _VisualizerSettings
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                _VisualizerSettings = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a boolean indicating if the info pane should be shown for this report
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	10/17/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ShowInfoPane() As Boolean
            Get
                Return _ShowInfoPane
            End Get
            Set(ByVal value As Boolean)
                _ShowInfoPane = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a boolean indicating if the controls pane should be shown for this report
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	10/17/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ShowControls() As Boolean
            Get
                Return _ShowControls
            End Get
            Set(ByVal value As Boolean)
                _ShowControls = value
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets or sets a boolean indicating if this report should run on demand, or
        ''' if it should be run automatically when the module is viewed
        ''' </summary>
        ''' <history>
        ''' 	[anurse]	10/17/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property AutoRunReport() As Boolean
            Get
                Return _AutoRunReport
            End Get
            Set(ByVal value As Boolean)
                _AutoRunReport = value
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets a value indicating whether token replace should take place.
        ''' </summary>
        ''' <value><c>true</c> if [token replace]; otherwise, <c>false</c>.</value>
        Public Property TokenReplace() As Boolean
            Get
                Return _TokenReplace
            End Get
            Set(ByVal value As Boolean)
                _TokenReplace = value
            End Set
        End Property

#End Region

    End Class

End Namespace
