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

Imports System.Data.Common

Imports Microsoft.ApplicationBlocks.Data

Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Common.Utilities
Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports DotNetNuke.Modules.Reports.Visualizers.Xslt

Namespace DotNetNuke.Modules.Reports.Data

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Implements the Reports Data Provider on Microsoft SQL Server
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[anurse]	08/29/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SqlDataProvider
        Inherits DataProvider

#Region "Private Members"

        Private Const ProviderType As String = "data"

        Private _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            ' Read the attributes for this provider

            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

        Public Overrides Function CreateInputParameter(ByVal name As String, ByVal value As Object) As System.Data.Common.DbParameter
            Return New SqlParameter(String.Concat("@", name), value)
        End Function

        Public Overrides Function ExecuteSQL(ByVal strScript As String, ByVal ParamArray parameters As DbParameter()) As IDataReader
            ' HACK: Copy-pasted from Core SqlDataProvider - core doesn't provide a system for parameterized dynamic sql

            ' TODO: Switch to Regex and use IgnoreCase (punting this fix in 5.1 due to testing burden)
            strScript = strScript.Replace("{databaseOwner}", _databaseOwner)
            strScript = strScript.Replace("{dO}", _databaseOwner)
            strScript = strScript.Replace("{do}", _databaseOwner)
            strScript = strScript.Replace("{Do}", _databaseOwner)
            strScript = strScript.Replace("{objectQualifier}", _objectQualifier)
            strScript = strScript.Replace("{oq}", _objectQualifier)
            strScript = strScript.Replace("{oQ}", _objectQualifier)
            strScript = strScript.Replace("{Oq}", _objectQualifier)

            ' Convert the db parameters to sql parameters
            Dim sqlParams(parameters.Length - 1) As SqlParameter
            For i = 0 To parameters.Length - 1
                sqlParams(i) = DirectCast(parameters(i), SqlParameter)
            Next

            Return SqlHelper.ExecuteReader(_connectionString, CommandType.Text, strScript, sqlParams)
        End Function

        Public Overrides Function GetXsltExtensionObjects(ByVal tabModuleId As Integer) As System.Data.IDataReader
            Return DotNetNuke.Data.DataProvider.Instance().ExecuteReader("reports_GetXsltExtensionObjects", tabModuleId)
        End Function

        Public Overrides Sub SetXsltExtensionObjects(ByVal tabModuleId As Integer, ByVal extensionObjects As System.Collections.Generic.IEnumerable(Of Visualizers.Xslt.ExtensionObjectInfo))
            ' Start a Transaction
            Using transaction As New TransactionScope
                DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("reports_ClearXsltExtensionObjects", tabModuleId)

                For Each extensionObject As ExtensionObjectInfo In extensionObjects
                    DotNetNuke.Data.DataProvider.Instance().ExecuteNonQuery("reports_AddXsltExtensionObject", _
                                                                          tabModuleId, _
                                                                          extensionObject.XmlNamespace, _
                                                                          extensionObject.ClrType)
                Next

                transaction.Complete()
            End Using
        End Sub
    End Class

End Namespace
