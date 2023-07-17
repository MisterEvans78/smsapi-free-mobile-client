Imports System.Text.Json.Serialization

Public Class SmsApiParameters
    <JsonPropertyName("user")>
    Public Property User As String

    <JsonPropertyName("pass")>
    Public Property Pass As String

    <JsonPropertyName("msg")>
    Public Property Msg As String
End Class
