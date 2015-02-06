Imports System.Data

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

Imports DotNetNuke.Modules.Reports.Extensions

Namespace DotNetNuke.Modules.Reports.Visualizers

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The IVisualizerControl class provides an interface to a Visualizer Control
    ''' </summary>
    ''' <remarks>
    ''' Visualizer Controls are the component of the visualizer that actually renders the report data.
    ''' </remarks>
    ''' <history>
    '''     [anurse]     08/02/2007    Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Interface IVisualizerControl
        Inherits IReportsControl

        ''' <summary>
        ''' Gets a boolean indicating that the visualizer requests that the report
        ''' be automatically loaded and executed
        ''' </summary>
        ReadOnly Property AutoExecuteReport() As Boolean

        ''' <summary>
        ''' Called by the hosting control to configure a report for the visualizer
        ''' </summary>
        ''' <remarks>
        ''' The <paramref name="Results" /> parameter will be a null reference (Nothing
        ''' in VB.Net) if the <see cref="AutoExecuteReport"/> property is false. In order
        ''' to execute the report, use the <see cref="ReportsController.ExecuteReport" /> method.
        ''' </remarks>
        ''' <param name="Report">The report to configure</param>
        ''' <param name="Results">The results of executing the report, or Nothing</param>
        Sub SetReport(ByVal Report As ReportInfo, ByVal Results As DataTable, ByVal FromCache As Boolean)

    End Interface

End Namespace