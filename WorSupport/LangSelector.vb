Public Class LangSelector

    Private Sub LangSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If DomainUpDown1.Text = "Idioma / Language" Then
            MsgBox("Elige un Idioma" & vbCrLf & "Select a Language", MsgBoxStyle.Critical, "Worcome Security")
        Else
            If DomainUpDown1.SelectedItem = "Español(España)" Then
                WorSupport.AppLanguage = 1
                'Idioma.Español.LANG_Español()
                'My.Settings.Espanglish = "ESP"
            ElseIf DomainUpDown1.SelectedItem = "English(Unites States)" Then
                WorSupport.AppLanguage = 0
                'Idioma.Ingles.LANG_English()
                'My.Settings.Espanglish = "ENG"
            End If
            My.Settings.Save()
            My.Settings.Reload()
            Me.Hide()
            Me.DialogResult = DialogResult.OK

            Selector.Show()

        End If
    End Sub

    Private Sub LangSelector_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub
End Class