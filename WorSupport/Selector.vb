Public Class Selector

    Private Sub Selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'OfflineMode = False
            'SecureMode = False
            'SignRegistry = True
            'AppStatus = True
            AppService.StartAppService(False, False, True, True)
        Catch ex As Exception
            MsgBox("Error critico con el servicio WorSupport", MsgBoxStyle.Critical, "Worcome Security")
        End Try
        AddTelemetryToLog("Selector_Load@Selector", "Iniciado!", True)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppSupport.Show()
        AppSupport.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        AppAbout.Show()
        AppAbout.Focus()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AppHelper.Show()
        AppHelper.Focus()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        AppUpdate.Show()
        AppUpdate.Focus()
    End Sub
End Class