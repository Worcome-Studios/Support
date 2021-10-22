Public Class AppAbout

    Private Sub AppAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Text = "About Wor: " & My.Application.Info.ProductName
            lblAppName.Text = My.Application.Info.ProductName
            lblAppDescription.Text = My.Application.Info.Description
            lblInfo.Text = "v" & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")"
            'If OfflineMode = True Then
            '    chbOfflineMode.CheckState = CheckState.Checked
            'Else
            '    chbOfflineMode.CheckState = CheckState.Unchecked
            'End If
        Catch
        End Try
    End Sub

    Private Sub chbOfflineMode_CheckedChanged(sender As Object, e As EventArgs) Handles chbOfflineMode.CheckedChanged
        'If chbOfflineMode.CheckState = CheckState.Checked Then
        '    OfflineMode = True
        'Else
        '    OfflineMode = False
        '    If MsgBox("Do you want to run an AppService instance?", MsgBoxStyle.YesNo, "Worcome Security") = MsgBoxResult.Yes Then
        '        Try
        '            AppService.StartAppService(False, False, True, True)
        '        Catch
        '        End Try
        '    End If
        'End If
        'SaveRegedit()
    End Sub

    Private Sub btnCheckUpdates_Click(sender As Object, e As EventArgs) Handles btnCheckUpdates.Click
        AppUpdate.Show()
        AppUpdate.Focus()
    End Sub

    Private Sub llblSupport_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llblSupport.LinkClicked
        AppSupport.Show()
        AppSupport.Focus()
    End Sub

    Private Sub llblUseGuide_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llblUseGuide.LinkClicked
        AppHelper.Show()
        AppHelper.Focus()
    End Sub

    Private Sub picWorcome_Click(sender As Object, e As EventArgs) Handles picWorcome.Click
        Try
            Process.Start(ServerSwitch.SW_UsingServer)
        Catch
		End Try
    End Sub

    Private Sub btnLicense_Click(sender As Object, e As EventArgs) Handles btnLicense.Click
        'AppLicenser.LicenseVerification()
    End Sub

    Private Sub btnLangSelector_Click(sender As Object, e As EventArgs) Handles btnLangSelector.Click
        'LangSelector.Show()
        'LangSelector.Focus()
    End Sub

    Sub SafeMode()
        gbItems.Enabled = False
    End Sub

End Class