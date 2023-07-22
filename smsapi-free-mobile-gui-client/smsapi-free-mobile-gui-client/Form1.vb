Imports System.Net
Imports System.Text

Public Class Form1
#If DEBUG Then
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text += " (Debug)"
    End Sub
#End If

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        EnableControls(False)
        UseWaitCursor = True
        Try
            Dim smsApi As New SmsApi() With {.User = TextBox1.Text, .Pass = TextBox2.Text, .Msg = TextBox3.Text}
            Dim statusCode As HttpStatusCode = Await smsApi.CallApiAsync()

            ' Icone de la messagebox
            Dim msgBoxIcon As MessageBoxIcon
            Select Case statusCode
                Case >= HttpStatusCode.BadRequest
                    msgBoxIcon = MessageBoxIcon.Exclamation
                Case >= HttpStatusCode.InternalServerError
                    msgBoxIcon = MessageBoxIcon.Error
                Case Else
                    msgBoxIcon = MessageBoxIcon.Information
            End Select

            MessageBox.Show(DetermineMessage(statusCode), "Message", MessageBoxButtons.OK, msgBoxIcon)
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

    Private Function DetermineMessage(statusCode As HttpStatusCode) As String
        Select Case statusCode
            Case HttpStatusCode.OK
                Return "Le SMS a été envoyé sur votre mobile."
            Case HttpStatusCode.BadRequest
                Return "Un des paramètres obligatoires est manquant."
            Case HttpStatusCode.PaymentRequired
                Return "Trop de SMS ont été envoyés en trop peu de temps."
            Case HttpStatusCode.Forbidden
                Return "Le service n'est pas activé sur l'espace abonné, ou login / clé incorrect."
            Case HttpStatusCode.InternalServerError
                Return "Erreur côté serveur. Veuillez réessayer ultérieurement."
            Case Else
                Return $"Erreur HTTP {CInt(statusCode)} ({statusCode})."
        End Select
    End Function
End Class
