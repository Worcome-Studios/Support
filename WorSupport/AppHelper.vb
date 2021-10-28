Public Class AppHelper

    Private Sub AppHelper_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label5.Text = WorSupport.WorSupportVersion
        Me.Text = WorSupport.ThisAssemblyProductName & " | AppHelper Native Support Service"
        If WorSupport.WorSupportIsActived Then
            If AppService.OfflineApp = False Then
                If WorSupport.AppServiceSuccess = False Or WorSupport.ServerSwitchSuccess = False Or WorSupport.AppStatusSuccess = False Then
                    If WorSupport.AppLanguage = 1 Then
                        MsgBox("AppService no se ejecutó correctamente. Es posible que tenga problemas con este módulo.", MsgBoxStyle.Exclamation, "Worcome Security")
                    ElseIf WorSupport.AppLanguage = 0 Then
                        MsgBox("AppService did not run correctly. You may have problems with this module.", MsgBoxStyle.Exclamation, "Worcome Security")
                    End If
                End If
            End If
            Start()
        Else
            Me.Close() 'END_FORM
        End If
    End Sub

    Sub Start()
        Try
            If My.Computer.Network.IsAvailable Then
                WebBrowser1.Navigate(ServerSwitch.DIR_AppHelper & "/" & WorSupport.ThisAssemblyName & ".html")
            Else
                MsgBox("Debe conectarse a internet", MsgBoxStyle.Information, "Worcome Security")
                Me.Close() 'END_FORM
            End If
        Catch ex As Exception
            AddTelemetryToLog("Start@AppHelper", "Error: " & ex.Message, True)
        End Try
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Try
            If WebBrowser1.DocumentTitle = "404 Error | Worcome Studios" Then
                If WorSupport.AppLanguage = 1 Then
                    MsgBox("El documento de ayuda todavía no existe dentro del Servidor." & vbCrLf & "Contacte con Soporte", MsgBoxStyle.Information, "Worcome Security")
                ElseIf WorSupport.AppLanguage = 0 Then
                    MsgBox("The help document does not yet exist within the Server." & vbCrLf & "Contact Support", MsgBoxStyle.Information, "Worcome Security")
                End If
                Me.Close() 'END_FORM
                'ElseIf WebBrowser1.DocumentTitle = "Esta página no se puede mostrar" Then
                '    Label1.Text = "Document not found"
                '    MsgBox("No hay conexion a internet", MsgBoxStyle.Critical, "Worcome Security")
                '    Me.Close() 'END_FORM
            Else
                WebBrowser1.Visible = True
            End If
        Catch ex As Exception
            AddTelemetryToLog("WebBrowser1_DocumentCompleted@AppHelper", "Error: " & ex.Message, True)
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        WebBrowser1.Navigate(ServerSwitch.DIR_AppHelper & "/" & WorSupport.ThisAssemblyName & ".html")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        WebBrowser1.Navigate(ServerSwitch.DIR_AppHelper & "/AboutApps/" & WorSupport.ThisAssemblyName & ".html")
    End Sub
End Class