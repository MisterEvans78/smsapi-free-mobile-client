Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization

''' <summary>
''' Utiliser l'API fournie par Free Mobile pour s'envoyer un SMS.
''' </summary>
Public Class SmsApi
    Private Const URL As String = "https://smsapi.free-mobile.fr/sendmsg"

    <JsonPropertyName("user")>
    Public Property User As String

    <JsonPropertyName("pass")>
    Public Property Pass As String

    <JsonPropertyName("msg")>
    Public Property Msg As String

    Public Sub New()

    End Sub

    Public Sub New(user As String, pass As String, msg As String)
        Me.User = user
        Me.Pass = pass
        Me.Msg = msg
    End Sub

    ''' <summary>
    ''' Fait un appel � L'API pour envoyer un SMS.
    ''' </summary>
    ''' <returns>Code de statut HTTP.</returns>
    Public Async Function CallApiAsync() As Task(Of HttpStatusCode)
        Dim client As New HttpClient()

        Dim serializedParameters As New StringContent(JsonSerializer.Serialize(Me), Encoding.UTF8, "application/json")

        Dim response = Await client.PostAsync(URL, serializedParameters)

        Return response.StatusCode
    End Function

    Public Shared Function DetermineMessage(statusCode As HttpStatusCode) As String
        Select Case statusCode
            Case HttpStatusCode.OK
                Return "Le SMS a �t� envoy� sur votre mobile."
            Case HttpStatusCode.BadRequest
                Return "Un des param�tres obligatoires est manquant."
            Case HttpStatusCode.PaymentRequired
                Return "Trop de SMS ont �t� envoy�s en trop peu de temps."
            Case HttpStatusCode.Forbidden
                Return "Le service n'est pas activ� sur l'espace abonn�, ou login / cl� incorrect."
            Case HttpStatusCode.InternalServerError
                Return "Erreur c�t� serveur. Veuillez r�essayer ult�rieurement."
            Case Else
                Return $"Erreur HTTP {CInt(statusCode)} ({statusCode})."
        End Select
    End Function
End Class
