Imports System.IO
Imports System.Net
Imports System.Text
Public Class AppSupport
    Dim ComputerInfo As String
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons\AppFiles"
    Dim SendAdjunto As Boolean = True 'False PARA NO ENVIAR TELEMETRIA.

    Private Sub AppSupport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If WorSupport.AppLanguage = 1 Then
            LANG_Español()
        Else
            LANG_English()
        End If
        Label5.Text = WorSupport.WorSupportVersion
        Me.Text = WorSupport.ThisAssemblyProductName & " | AppSupport Native Support Service"
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
        Else
            Me.Close() 'END_FORM
        End If
        Try
            If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
            telemetryIdentification = CreateIdentification(35)
        Catch ex As Exception
            AddTelemetryToLog("AppSupport_Load@AppSupport", "Error: " & ex.Message, True)
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSendMessage.Click
        If tbUserEmail.Text = Nothing Or tbUserMessage.Text = Nothing Then
        Else
            If tbUserEmail.Text.Contains("&") Or tbUserMessage.Text.Contains("+") Or tbUserMessage.Text.Contains("#") Then
                If WorSupport.AppLanguage = 0 Then
                    MsgBox("No puedes ingresar símbolos especiales", MsgBoxStyle.Information, "Worcome Security")
                Else
                    MsgBox("You cannot enter special symbols", MsgBoxStyle.Information, "Worcome Security")
                End If
            Else
                UploadSupportMsg()
            End If
        End If
    End Sub

    Sub UploadSupportMsg()
        Try
            Dim request As WebRequest = WebRequest.Create(ServerSwitch.DIR_AppSupport & "/postSupport.php")
            request.Method = "POST"
            Dim postData As String = "email=" & tbUserEmail.Text & "&mensaje=" & "# WorSupport > AppSupport" &
                "[Assembly]" &
                vbCrLf & "AssemblyName=" & My.Application.Info.AssemblyName &
                vbCrLf & "AssemblyVersion=" & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" &
                vbCrLf & "WorSupport=" & WorSupport.WorSupportVersion & " (" & WorSupport.WorSupportCompilate & ")" &
                vbCrLf & "Title=" & My.Application.Info.Title &
                vbCrLf & "Company=" & My.Application.Info.CompanyName &
                vbCrLf & "ExecutablePath=" & WorSupport.DirAppPath &
                vbCrLf & "CommandLineArgs=" & Command() &
                vbCrLf & "[Machine]" &
                vbCrLf & "OS=" & My.Computer.Info.OSFullName & " (" & My.Computer.Info.OSVersion & ") @ " & My.Computer.Info.OSPlatform &
                vbCrLf & "RAM=" & My.Computer.Info.AvailablePhysicalMemory & "/" & My.Computer.Info.TotalPhysicalMemory &
                vbCrLf & "Account=" & My.User.Name &
                vbCrLf & "Screen=" & My.Computer.Screen.WorkingArea.ToString & "/" & My.Computer.Screen.Bounds.ToString &
                vbCrLf & "Languaje=" & My.Computer.Info.InstalledUICulture.NativeName & " (" & My.Computer.Info.InstalledUICulture.EnglishName & ")" &
                vbCrLf & "TimeAndDate=" & Now.ToShortTimeString & " " & Now.ToShortDateString &
                vbCrLf & "[WorSupport]" &
                vbCrLf & "_DIRCommons=" & _DIRCommons &
                vbCrLf & "WorSupportIsActived=" & WorSupportIsActived &
                vbCrLf & "AppServiceSuccess=" & AppServiceSuccess &
                vbCrLf & "ServerSwitchSuccess=" & ServerSwitchSuccess &
                vbCrLf & "SignRegistrySuccess=" & SignRegistrySuccess &
                vbCrLf & "AppStatusSuccess=" & AppStatusSuccess &
                vbCrLf & ";AppService" &
                vbCrLf & "SignRegistryStatus=" & SignRegistryStatus &
                vbCrLf & "AppStatusStatus=" & AppStatusStatus &
                vbCrLf & "AppRegistry=" & AppRegistry.ToString &
                vbCrLf & "AppServiceConfig=" & AppServiceConfig.ToString &
                vbCrLf & "OfflineApp=" & OfflineApp &
                vbCrLf & "SecureMode=" & SecureMode &
                vbCrLf & ";ServerSwitch" &
                vbCrLf & "UsingServer=" & UsingServer &
                vbCrLf & "DIR_AppStatus=" & DIR_AppStatus &
                vbCrLf & "DIR_AppUpdate=" & DIR_AppUpdate &
                vbCrLf & "DIR_AppHelper=" & DIR_AppHelper &
                vbCrLf & "DIR_AppLicenser=" & DIR_AppLicenser &
                vbCrLf & "DIR_AppSupport=" & DIR_AppSupport &
                vbCrLf & "DIR_Telemetry=" & DIR_Telemetry &
                vbCrLf & "UsingServer=" & SW_UsingServer &
                vbCrLf & "Structure=" & SW_Structure &
                vbCrLf & ";SignRegistry" &
                vbCrLf & "Reg_Assembly=" & Reg_Assembly &
                vbCrLf & "Reg_Version=" & Reg_Version &
                vbCrLf & "Reg_InstalledDate=" & Reg_InstalledDate &
                vbCrLf & "Reg_LastStart=" & Reg_LastStart &
                vbCrLf & "Reg_Directory=" & Reg_Directory &
                vbCrLf & "Reg_AllUsersCanUse=" & Reg_AllUsersCanUse &
                vbCrLf & ";AppStatus" &
                vbCrLf & "Assembly_Status=" & Assembly_Status &
                vbCrLf & "Assembly_Name=" & Assembly_Name &
                vbCrLf & "Assembly_Version=" & Assembly_Version &
                vbCrLf & "Runtime_VisitURL=" & Runtime_VisitURL &
                vbCrLf & "Runtime_Message=" & Runtime_Message &
                vbCrLf & "Runtime_ArgumentLine=" & Runtime_ArgumentLine &
                vbCrLf & "Runtime_Command=" & Runtime_Command &
                vbCrLf & "Updates_Critical=" & Updates_Critical &
                vbCrLf & "Updates_Message=" & Updates_Message &
                vbCrLf & "Updates_CanUseOldVersion=" & Updates_CanUseOldVersion &
                vbCrLf & "Installer_Status=" & Installer_Status &
                vbCrLf & "Installer_BinDownload=" & Installer_BinDownload &
                vbCrLf & "Installer_InsDownload=" & Installer_InsDownload &
                vbCrLf & "Installer_CanDowngrade=" & Installer_CanDowngrade &
                vbCrLf & "Installer_NeedStartUp=" & Installer_NeedStartUp &
                vbCrLf & "Installer_NeedRestart=" & Installer_NeedRestart &
                vbCrLf & "Installer_NeedElevateAccess=" & Installer_NeedElevateAccess &
                vbCrLf & "Installer_InstallFolder=" & Installer_InstallFolder &
                vbCrLf & "Installer_BitArch=" & Installer_BitArch &
                vbCrLf & "[AppSupport]" &
                vbCrLf & "Identification=" & telemetryIdentification &
                vbCrLf & "E-mail=" & tbUserEmail.Text &
                vbCrLf & "Message" &
                vbCrLf &
                vbCrLf & tbUserMessage.Text
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
            If SendAdjunto = True Then
                SaveTelemetryLogContent()
            End If
            If CType(response, HttpWebResponse).StatusDescription = "OK" Then
                If WorSupport.AppLanguage = 1 Then
                    MsgBox("Mensaje enviado correctamente!", MsgBoxStyle.Information, "Worcome Security")
                Else
                    MsgBox("Message sent correctly!", MsgBoxStyle.Information, "Worcome Security")
                End If
            Else
                If WorSupport.AppLanguage = 1 Then
                    MsgBox("No se pudo obtener una respuesta del servidor.", MsgBoxStyle.Information, "Worcome Security")
                Else
                    MsgBox("Could not get a response from the server.", MsgBoxStyle.Information, "Worcome Security")
                End If
            End If
            response.Close()
            Me.Close() 'END_FORM
        Catch ex As Exception
            If WorSupport.AppLanguage = 1 Then
                MsgBox("Ocurrio un error al tratar de enviar el mensaje.", MsgBoxStyle.Critical, "Worcome Security")
            Else
                MsgBox("Error trying to send the message.", MsgBoxStyle.Critical, "Worcome Security")
            End If
        End Try
    End Sub

#Region "LANGSelector"
    Sub LANG_Español()
        'Formulario Soporte
        Label1.Text = "Soporte"
        Label2.Text = "Escribenos tu Duda, Consulta o Problema"
        Label3.Text = "Tu Correo:"
        Label4.Text = "Mensaje:"
        btnSendMessage.Text = "Enviar >"
    End Sub
    Sub LANG_English()
        'Support Form
        Label1.Text = "Support"
        Label2.Text = "Write us your Doubt, Consultation or Problem"
        Label3.Text = "Your email:"
        Label4.Text = "Message:"
        btnSendMessage.Text = "Send >"
    End Sub
#End Region
End Class