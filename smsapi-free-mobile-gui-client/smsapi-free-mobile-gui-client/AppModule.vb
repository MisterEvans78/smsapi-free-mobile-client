Module AppModule
    Public Function GetVersionString() As String
        If (Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed")?.ToLower() = "true") Then
            Return Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion")
        Else
            Return My.Application.Info.Version.ToString()
        End If
    End Function
End Module
