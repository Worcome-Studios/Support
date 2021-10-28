Class AppUpdate
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons\Updates"
    Dim ServiceFilePath As String

    Private Sub AppUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label5.Text = WorSupport.WorSupportVersion
        Me.Text = WorSupport.ThisAssemblyProductName & " | AppUpdater Native Support Service"
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
        If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then
            My.Computer.FileSystem.CreateDirectory(DIRCommons)
        End If
        If WorSupport.AppLanguage = 1 Then
            Lang_Español()
            Label3.Text = "Producto: " & My.Application.Info.ProductName &
            vbCrLf & "Ensamblado: " & My.Application.Info.AssemblyName &
            vbCrLf & "Versión: " & My.Application.Info.Version.ToString
        Else
            Lang_English()
            Label3.Text = "Product: " & My.Application.Info.ProductName &
            vbCrLf & "Assembly: " & My.Application.Info.AssemblyName &
            vbCrLf & "Version: " & My.Application.Info.Version.ToString
        End If
        ServiceFilePath = DIRCommons & "\" & WorSupport.ThisAssemblyName & ".ini"
        If My.Computer.FileSystem.FileExists(ServiceFilePath) = True Then
            My.Computer.FileSystem.DeleteFile(ServiceFilePath)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(ServerSwitch.DIR_AppHelper & "/AboutApps/" & WorSupport.ThisAssemblyName & ".html#WhatsNew")
    End Sub

    Private Sub btnCheckUpdates_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CheckForUpdates()
    End Sub

    Private Sub btnAssistant_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CheckUpdater()
        Process.Start(DIRCommons & "\Updater.exe", "/SearchForUpdates -" & WorSupport.ThisAssemblyName & " -" & WorSupport.ThisAssemblyVersion & " -" & WorSupport.DirAppPath)
    End Sub

    Sub CheckForUpdates()
        Button1.Enabled = False
        Try
            'LEER LOS VALORES DEL ARCHIVO Y APLICARLOS A LAS VARIABLES
            AppStatus.Assembly_Status = Boolean.TryParse(GetIniValue("Assembly", "Status", AppServiceFilePath), SecureMode)
            AppStatus.Assembly_Name = GetIniValue("Assembly", "Name", AppServiceFilePath)
            AppStatus.Assembly_Version = GetIniValue("Assembly", "Version", AppServiceFilePath)
            AppStatus.Runtime_VisitURL = GetIniValue("Runtime", "VisitURL", AppServiceFilePath)
            AppStatus.Runtime_Message = GetIniValue("Runtime", "Message", AppServiceFilePath)
            AppStatus.Runtime_ArgumentLine = GetIniValue("Runtime", "ArgumentLine", AppServiceFilePath)
            AppStatus.Runtime_Command = GetIniValue("Runtime", "Command", AppServiceFilePath)
            AppStatus.Updates_Critical = Boolean.TryParse(GetIniValue("Updates", "Critical", AppServiceFilePath), SecureMode)
            AppStatus.Updates_Message = GetIniValue("Updates", "Message", AppServiceFilePath)
            AppStatus.Updates_CanUseOldVersion = GetIniValue("Updates", "CanUseOldVersion", AppServiceFilePath)
            AppStatus.Installer_Status = Boolean.TryParse(GetIniValue("Installer", "Status", AppServiceFilePath), True)
            AppStatus.Installer_BinDownload = GetIniValue("Installer", "BinDownload", AppServiceFilePath)
            AppStatus.Installer_InsDownload = GetIniValue("Installer", "InsDownload", AppServiceFilePath)
            AppStatus.Installer_BitArch = GetIniValue("Installer", "BitArch", AppServiceFilePath)
        Catch ex As Exception
            AddTelemetryToLog("CheckForUpdates(0)@AppUpdater", "Error: " & ex.Message, True)
        End Try
        Try
            Dim versionLocal = New Version(WorSupport.ThisAssemblyVersion)
            Dim versionServidor = New Version(AppStatus.Assembly_Version)
            Dim result = versionLocal.CompareTo(versionServidor)
            If (result > 0) Then
                'Sobre-Actualizada
                If WorSupport.AppLanguage = 1 Then
                    Button1.Text = "No hay actualizaciones"
                    MsgBox("Tiene una versión superior a la encontrada en el servidor", MsgBoxStyle.Information, "Worcome Security")
                ElseIf WorSupport.AppLanguage = 0 Then
                    Button1.Text = "No Updates"
                    MsgBox("Has a higher version than the one found on the server", MsgBoxStyle.Information, "Worcome Security")
                End If
                Me.Close() 'END_FORM
            ElseIf (result < 0) Then
                'Actualizacion disponible
                If WorSupport.AppLanguage = 1 Then
                    Button1.Text = "Actualización disponible"
                    MsgBox("Hay una nueva versión disponible!", MsgBoxStyle.Information, "Worcome Security")
                ElseIf WorSupport.AppLanguage = 0 Then
                    Button1.Text = "Update available"
                    MsgBox("There is a new version available!", MsgBoxStyle.Information, "Worcome Security")
                End If
                CheckUpdater()
                Process.Start(DIRCommons & "\Updater.exe", "/DownloadUpdates -" & WorSupport.ThisAssemblyName & " -" & WorSupport.ThisAssemblyVersion & " -" & WorSupport.DirAppPath)
            Else
                'Sin actualizaciones
                If WorSupport.AppLanguage = 1 Then
                    Button1.Text = "No hay actualizaciones"
                    MsgBox("No hay actualizaciones disponibles", MsgBoxStyle.Information, "Worcome Security")
                ElseIf WorSupport.AppLanguage = 0 Then
                    Button1.Text = "No Updates"
                    MsgBox("No updates available", MsgBoxStyle.Information, "Worcome Security")
                End If
                Me.Close() 'END_FORM
            End If
        Catch ex As Exception
            AddTelemetryToLog("CheckForUpdates(1)@AppUpdater", "Error: " & ex.Message, True)
        End Try
        Button1.Enabled = True
    End Sub
    Sub CheckUpdater()
        Try
            'Ver si el ejecutable Updater.exe existe en la ruta
            If My.Computer.FileSystem.FileExists(DIRCommons & "\Updater.exe") = False Then
                If My.Computer.FileSystem.FileExists(DIRCommons & "\AppUpdaterAddIn.zip") = True Then
                    My.Computer.FileSystem.DeleteFile(DIRCommons & "\AppUpdaterAddIn.zip")
                End If
                'No existe, se debe descargar.
                My.Computer.Network.DownloadFile(ServerSwitch.DIR_AppUpdate & "/AppUpdaterAddIn.zip", DIRCommons & "\Updater.zip")
                'descomprime
                Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
                Dim outputFolder As String = DIRCommons
                Dim inputZip As String = DIRCommons & "\Updater.zip"
                IO.Directory.CreateDirectory(outputFolder)
                Dim output As Object = shObj.NameSpace((outputFolder))
                Dim input As Object = shObj.NameSpace((inputZip))
                output.CopyHere((input.Items), 4)
                Threading.Thread.Sleep(50)
                IO.File.Delete(DIRCommons & "\Updater.zip")
            End If
        Catch ex As Exception
            AddTelemetryToLog("CheckUpdater@AppUpdater", "Error: " & ex.Message, True)
        End Try
    End Sub

    Sub Lang_Español()
        Label1.Text = "Comprobar Actualizaciones"
        Button1.Text = "Buscar actualizaciones"
        Button2.Text = "Asistente"
    End Sub
    Sub Lang_English()
        Label1.Text = "Search for Updates"
        Button1.Text = "Search for updates"
        Button2.Text = "Assistant"
    End Sub
End Class