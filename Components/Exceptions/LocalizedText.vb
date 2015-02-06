Imports DotNetNuke.Services.Localization
Imports System.Runtime.Serialization

Namespace DotNetNuke.Modules.Reports.Exceptions
    <Serializable()> _
    Public Class LocalizedText
        Implements ISerializable

#Region " Constants "

        Private Const SER_KEY_ResourceKey As String = "ResourceKey"
        Private Const SER_KEY_ResourceFile As String = "ResourceFile"
        Private Const SER_KEY_FormatArgs As String = "FormatArg"
        Private Const SER_KEY_FormatArgsCount As String = "FormatArgsCount"
        Private Const RESX_ModuleCommon As String = "~/DesktopModules/Reports/App_LocalResources/SharedResources.resx"


#End Region

        Private _resourceKey As String
        Private _resourceFile As String
        Private _formatArguments As String()

        Public ReadOnly Property ResourceKey() As String
            Get
                Return _resourceKey
            End Get
        End Property

        Public ReadOnly Property ResourceFile() As String
            Get
                Return _resourceFile
            End Get
        End Property

        Public ReadOnly Property FormatArguments() As String()
            Get
                Return _formatArguments
            End Get
        End Property

        Public Sub New(ByVal resourceKey As String)
            Me.New(resourceKey, RESX_ModuleCommon, New String() {})
        End Sub

        Public Sub New(ByVal resourceKey As String, ByVal formatArguments As String())
            Me.New(resourceKey, RESX_ModuleCommon, formatArguments)
        End Sub

        Public Sub New(ByVal resourceKey As String, ByVal resourceFile As String, ByVal ParamArray formatArguments As String())
            _resourceKey = resourceKey
            _resourceFile = resourceFile
            _formatArguments = formatArguments
        End Sub

        Public Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            Me._resourceKey = info.GetString(SER_KEY_ResourceKey)
            Me._resourceFile = info.GetString(SER_KEY_ResourceFile)

            Dim count As Integer = info.GetInt32(SER_KEY_FormatArgsCount)
            ReDim _formatArguments(count)
            For i As Integer = 0 To count
                _formatArguments(i) = info.GetString(String.Concat(SER_KEY_FormatArgs, i.ToString))
            Next
        End Sub

        Public Function GetLocalizedText() As String
            Dim localized As String = String.Empty
            If Not String.IsNullOrEmpty(_resourceFile) Then
                localized = Localization.GetString(_resourceKey, _resourceFile)
            Else
                localized = Localization.GetString(_resourceKey)
            End If
            If _formatArguments IsNot Nothing AndAlso _formatArguments.Length > 0 Then
                Return String.Format(localized, _formatArguments)
            Else
                Return localized
            End If
        End Function

        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
            info.AddValue(SER_KEY_ResourceKey, Me._resourceKey)
            info.AddValue(SER_KEY_ResourceFile, Me._resourceFile)
            info.AddValue(SER_KEY_FormatArgsCount, Me._formatArguments.Length)
            For i As Integer = 0 To Me._formatArguments.Length
                info.AddValue(String.Concat(SER_KEY_FormatArgs, i.ToString()), Me._formatArguments(i))
            Next
        End Sub
    End Class
End Namespace