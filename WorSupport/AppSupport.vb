Imports System.IO
Imports System.Net
Imports System.Text
Public Class AppSupport
    Dim ComputerInfo As String
    Dim DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons\AppFiles"
    Dim SendAdjunto As Boolean = True 'False PARA NO ENVIAR TELEMETRIA.
    Dim logContent As String
    Dim IDentification As String = CreateIdentification("Identification")
    Dim logContentFileName As String

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
        logContentFileName = "\" & IDentification & "[" & Format(DateAndTime.TimeOfDay, "hh") &
                    "_" & Format(DateAndTime.TimeOfDay, "mm") &
                    "_" & Format(DateAndTime.TimeOfDay, "ss") &
                    "@" & My.Application.Info.AssemblyName & "]Telemetry.log"
        Try
            If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
            ComputerInfo = vbCrLf &
                "Aplicacion: " & My.Application.Info.AssemblyName &
                vbCrLf & "      Version: " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" &
                vbCrLf & "      Titulo: " & My.Application.Info.Title &
                vbCrLf & "      Almacenado en: " & WorSupport.DirAppPath &
                vbCrLf & "      Compañia: " & My.Application.Info.CompanyName &
                vbCrLf & "      WorSupport: " & WorSupport.WorSupportVersion & " (" & WorSupport.WorSupportCompilate & ")" &
                vbCrLf & "Sistema Operativo: " & My.Computer.Info.OSFullName & My.Computer.Info.OSVersion &
                vbCrLf & "      RAM: " & My.Computer.Info.TotalPhysicalMemory &
                vbCrLf & "      Cuenta OS: " & My.User.Name &
                vbCrLf & "      Pantalla: " & My.Computer.Screen.Bounds.ToString & " | (Area en Uso: " & My.Computer.Screen.WorkingArea.ToString & ")" &
                vbCrLf & "      Idioma OS: " & My.Computer.Info.InstalledUICulture.NativeName &
                vbCrLf & "      Hora y Fecha Local: " & My.Computer.Clock.LocalTime &
                vbCrLf & "Informacion Compilada por la Aplicacion Nativa de Soporte"
            Label6.Text = "Assembly: " & My.Application.Info.AssemblyName &
                vbCrLf & "Product: " & My.Application.Info.ProductName &
                vbCrLf & "Version: " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" &
                vbCrLf & "By: " & My.Application.Info.Trademark &
                vbCrLf & "WorSupport: " & WorSupport.WorSupportVersion
            '& vbCrLf & "Running from: " & Application.ExecutablePath
        Catch ex As Exception
            Console.WriteLine("[AppSupport@AppSupport]Esto es vergonzoso, error: " & ex.Message)
            AddToLog("AppSupport_Load@AppSupport", "Error: " & ex.Message, True)
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = Nothing Or TextBox2.Text = Nothing Then
        Else
            If TextBox1.Text.Contains("&") Or TextBox2.Text.Contains("&") Then
                If WorSupport.AppLanguage = 0 Then
                    MsgBox("No puedes ingresar símbolos especiales (&)", MsgBoxStyle.Information, "Worcome Security")
                Else
                    MsgBox("You cannot enter special symbols (&)", MsgBoxStyle.Information, "Worcome Security")
                End If
            Else
                UploadSupportMsg()
            End If
        End If
    End Sub

    Sub UploadSupportMsg()
        Try
            Dim request As WebRequest = WebRequest.Create(AppService.DIR_AppSupport & "/postSupport.php")
            request.Method = "POST"
            Dim postData As String = "email=" & TextBox1.Text & "&mensaje=" &
                TextBox2.Text &
                vbCrLf & vbCrLf & vbCrLf & ComputerInfo &
                vbCrLf & vbCrLf & "Informacion del Envio: " & vbCrLf &
                ((Format(DateAndTime.TimeOfDay, "hh")) & ":") & ((Format(DateAndTime.TimeOfDay, "mm")) & ":") & ((Format(DateAndTime.TimeOfDay, "ss")) & " ") & ((Format(DateAndTime.TimeOfDay, "tt")) & " ") & (" " & (DateAndTime.Today)) &
                vbCrLf & "Idioma Selecionado: " & WorSupport.AppLanguage &
                vbCrLf & "Desde el formulario: " & Me.Text &
                vbCrLf & vbCrLf & "Los mensajes pueden ser respondidos al correo que proporciono el usuario." &
                vbCrLf & "ID: " & IDentification
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
            If SendAdjunto = True Then
                SaveLogContent()
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
        End Try
    End Sub

#Region "TelemetryAttach"
    'AddToLog("Nombre_Formulario_Aqui", "Error_o_Log_aqui", False_True)
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
        Try
            Dim finalContent As String = Nothing
            If flag = True Then
                finalContent = " [!!!]"
            End If
            Dim Message As String = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & finalContent & " [" & from & "] " & content
            logContent &= vbCrLf & Message
            Console.WriteLine("[" & from & "]" & finalContent & " " & content)
            'Try
            '    My.Computer.FileSystem.WriteAllText(DIRCommons & "\Activity.log", vbCrLf & Message, True)
            'Catch
            'End Try
        Catch ex As Exception
            Console.WriteLine("[AddToLog@AppSupport]Error: " & ex.Message)
        End Try
    End Sub

    Sub SaveLogContent()
        Try
            If My.Computer.FileSystem.FileExists(DIRCommons & logContentFileName) = True Then
                My.Computer.FileSystem.DeleteFile(DIRCommons & logContentFileName)
            End If
            My.Computer.FileSystem.WriteAllText(DIRCommons & logContentFileName, logContent, False)
            If logContent = Nothing Then
                If My.Computer.FileSystem.FileExists(DIRCommons & logContentFileName) = True Then
                    My.Computer.FileSystem.DeleteFile(DIRCommons & logContentFileName)
                End If
                SendAdjunto = False
            Else
                UploadAdjunto(DIRCommons & logContentFileName)
            End If
        Catch ex As Exception
            Console.WriteLine("[SaveErrorLog@AppSupport]Esto es vergonzoso, error: " & ex.Message)
        End Try
    End Sub

    Sub UploadAdjunto(ByVal dir As String)
        'Post PHP con la ruta del archivo a subir
        Try
            Dim requestTLM As WebRequest = WebRequest.Create(DIR_Telemetry & "/postTelemetry.php")
            requestTLM.Method = "POST"
            Dim postDataTLM As String = "ident=" & My.Application.Info.AssemblyName & "_" & IDentification & "&log=" & logContent
            Dim byteArrayTLM As Byte() = Encoding.UTF8.GetBytes(postDataTLM)
            requestTLM.ContentType = "application/x-www-form-urlencoded"
            requestTLM.ContentLength = byteArrayTLM.Length
            Dim dataStreamTLM As Stream = requestTLM.GetRequestStream()
            dataStreamTLM.Write(byteArrayTLM, 0, byteArrayTLM.Length)
            dataStreamTLM.Close()
            Dim responseTLM As WebResponse = requestTLM.GetResponse()
            Console.WriteLine(CType(responseTLM, HttpWebResponse).StatusDescription)
            responseTLM.Close()
        Catch ex As Exception
        End Try
    End Sub

    Function CreateIdentification(ByVal CreatedString As String)
        Dim obj As New Random()
        Dim posibles As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim longitud As Integer = posibles.Length
        Dim letra As Char
        Dim longitudnuevacadena As Integer = 35
        Dim nuevacadena As String = Nothing
        For i As Integer = 0 To longitudnuevacadena - 1
            letra = posibles(obj.[Next](longitud))
            nuevacadena += letra.ToString()
        Next
        Return nuevacadena
    End Function
#End Region

#Region "LANGSelector"
    Sub LANG_Español()
        'Formulario Soporte
        Label1.Text = "Soporte"
        Label2.Text = "Escribenos tu Duda, Consulta o Problema"
        Label3.Text = "Tu Correo:"
        Label4.Text = "Mensaje:"
        Button1.Text = "Enviar >"
    End Sub
    Sub LANG_English()
        'Support Form
        Label1.Text = "Support"
        Label2.Text = "Write us your Doubt, Consultation or Problem"
        Label3.Text = "Your email:"
        Label4.Text = "Message:"
        Button1.Text = "Send >"
    End Sub
#End Region
End Class