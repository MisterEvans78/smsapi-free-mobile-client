Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class Form1
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        UseWaitCursor = True
        Try
            Dim client As New HttpClient()

            Dim parameters As New SmsApiParameters With {.User = TextBox1.Text, .Pass = TextBox2.Text, .Msg = TextBox3.Text}

            Dim serializedParameters As New StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")

            Dim response = Await client.PostAsync("https://smsapi.free-mobile.fr/sendmsg", serializedParameters)

            ' Icone de la messagebox
            Dim msgBoxIcon As MessageBoxIcon
            Select Case response.StatusCode
                Case >= HttpStatusCode.BadRequest
                    msgBoxIcon = MessageBoxIcon.Exclamation
                Case >= HttpStatusCode.InternalServerError
                    msgBoxIcon = MessageBoxIcon.Error
                Case Else
                    msgBoxIcon = MessageBoxIcon.Information
            End Select

            ' Texte de la messagebox
            Dim message As String
            Select Case response.StatusCode
                Case HttpStatusCode.OK
                    message = "Le SMS a été envoyé sur votre mobile."
                Case HttpStatusCode.BadRequest
                    message = "Un des paramètres obligatoires est manquant."
                Case HttpStatusCode.PaymentRequired
                    message = "Trop de SMS ont été envoyés en trop peu de temps."
                Case HttpStatusCode.Forbidden
                    message = "Le service n'est pas activé sur l'espace abonné, ou login / clé incorrect."
                Case HttpStatusCode.InternalServerError
                    message = "Erreur côté serveur. Veuillez réessayer ultérieurement."
                Case Else
                    message = Await response.Content.ReadAsStringAsync()
            End Select

            MessageBox.Show(message, "Message", MessageBoxButtons.OK, msgBoxIcon)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur")
        End Try
        UseWaitCursor = False
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        CheckTextBoxAreFilled()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        CheckTextBoxAreFilled()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        CheckTextBoxAreFilled()
    End Sub

    Private Sub CheckTextBoxAreFilled()
        If TextBox1.Text <> String.Empty AndAlso TextBox2.Text <> String.Empty AndAlso TextBox3.Text <> String.Empty Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub AProposToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AProposToolStripMenuItem.Click
        Dim content As New StringBuilder()
        content.AppendLine($"Version : {My.Application.Info.Version}")
        content.AppendLine("Permet d'envoyer un SMS via l'API fourni par Free Mobile.")
        content.AppendLine()
        content.AppendLine("L'option « Notification par SMS » doit être activé sur votre forfait mobile Free.")
        MessageBox.Show(content.ToString())
    End Sub
End Class
