﻿Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class Form1
    Private Const _url As String = "https://smsapi.free-mobile.fr/sendmsg"

#If DEBUG Then
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text += " (Debug)"
    End Sub
#End If

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        EnableControls(False)
        UseWaitCursor = True
        Try
            Dim client As New HttpClient()

            Dim parameters As New SmsApiParameters With {.User = TextBox1.Text, .Pass = TextBox2.Text, .Msg = TextBox3.Text}

            Dim serializedParameters As New StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json")

            Dim response = Await client.PostAsync(_url, serializedParameters)

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
        EnableControls()
    End Sub

    Private Sub TextBoxes_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged
        If TextBox1.Text <> String.Empty AndAlso TextBox2.Text <> String.Empty AndAlso TextBox3.Text <> String.Empty Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub EnableControls(Optional enable As Boolean = True)
        Button1.Enabled = enable
        TextBox1.Enabled = enable
        TextBox2.Enabled = enable
        TextBox3.Enabled = enable
    End Sub

    Private Sub AProposToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AProposToolStripMenuItem.Click
        With New StringBuilder()
            .AppendLine($"Version : {GetVersionString()}")
            .AppendLine("Permet d'envoyer un SMS via l'API fourni par Free Mobile.")
            .AppendLine()
            .AppendLine("L'option « Notifications par SMS » doit être activé sur votre forfait mobile Free.")
            MessageBox.Show(.ToString())
        End With
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        With New StringBuilder()
            .AppendLine("Pour envoyer des SMS via ce logiciel, vous devez activer l'option « Notifications par SMS » sur votre forfait Free Mobile.")
            .AppendLine("Une clé d'identification vous sera fournie une fois l'option activée.")
            .AppendLine("Dans le champ « Utilisateur », il faut saisir votre identifiant Free Mobile (celui que vous utilisez pour accéder à votre espace abonné).")
            MessageBox.Show(.ToString(), "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End With
    End Sub
End Class
