Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Public Class AppLicenser
    Public IsLicensed As Boolean = False
    ReadOnly DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons\LICFiles"
    ReadOnly pathLicense As String = DIRCommons & "\[LIC]" & My.Application.Info.AssemblyName & ".lic"
    ReadOnly LicenserCryptoKey As String = "8rLrsR2hNOwZVERZWdÑipA7uK89ua8" 'Llave del Licenciador. UNICA Y UNIVERSAL

    ReadOnly AppCryptoKey As String = "" 'Cambia segun aplicacion. Cambio MANUAL <------------------------------
    'Dim GiveSubLicense As Boolean = True 'Debe cambiarlo segun la aplicacion requiera/permita

    Dim des As New TripleDESCryptoServiceProvider
    Dim hashmd5 As New MD5CryptoServiceProvider

    Dim AppLicenserStatus As String

    Dim LicenseKey As String
    Dim LicenseIdent As String
    Dim LicenseIdentification As String
    Dim LicenseEmail As String
    Dim LicenseDate As String
    Dim LicenseVersion As String
    Dim LicenseAssembly As String
    Dim LicenseExpires As String
    Dim IsSubLicense As Boolean
    Dim RandomString As Boolean

    'USO EN CODIGO
    '   AppLicenser.LicenseVerification()
    'Select Case AppLicenser.GetAppLicense()
    '    Case 0 'Sin licencia
    '        'Inactiva, obtener licencia...
    '        If AppLicenser.ShowDialog() = DialogResult.Cancel Then
    '            AppLicenser.Close()
    '        End If
    '    Case 1 'Con licencia
    '        'Activa, continuar
    '
    '    Case Else 'Otra situacion controlada
    'End Select

    Private Sub AppLicenser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then 'Si no existe
            My.Computer.FileSystem.CreateDirectory(DIRCommons) 'se crea
        End If
    End Sub

    Private Sub AppLicenser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Sub SecureClose()
        If My.Computer.FileSystem.FileExists(pathLicense) = True Then
            My.Computer.FileSystem.DeleteFile(pathLicense)
        End If
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Sub LicenseVerification()
        Select Case GetAppLicense()
            Case 0 'Sin licencia
                'Inactiva, obtener licencia...
                If Me.ShowDialog() = DialogResult.Cancel Then
                    Close()
                End If
            Case 1 'Con licencia
                ShowLicenceStatus()
            Case 2 'Otra situacion controlada
            Case Else
                MsgBox("Error al determinar la respuesta del Licenciador", MsgBoxStyle.Critical, "Worcome Security")
        End Select
    End Sub

    'Llamar al Licenciador por la Licencia
    Public Function GetAppLicense() As SByte  'Si devuelve 0 entonces no hay licencia, si devuelve 1 entonces si hay licencia y si devuelve 2 entonces no c sabe
        'CHECKEAMOS QUE UNA INSTANCIA DE AppService SE HAYA EJECUTADO
        If LicenserCryptoKey = Nothing Or AppCryptoKey = Nothing Then
            MsgBox("El licenciador no está bien configurado para este software." & vbCrLf & "Contacte con Soporte", MsgBoxStyle.Critical, "Worcome Security")
            Return 2
        Else
            Dim SwitchAppService As Boolean = False
            Dim SwitchAppServiceFunctions As Boolean = False
            If WorSupport.WorSupportIsActived Then
                If AppService.OfflineApp = False Then
                    If WorSupport.AppServiceSuccess = False Or WorSupport.ServerSwitchSuccess = False Or WorSupport.SignRegistrySuccess = False Or WorSupport.AppStatusSuccess = False Then
                        If MsgBox("Do you want to run an AppService instance?", MsgBoxStyle.YesNo, "Worcome Security") = MsgBoxResult.Yes Then
                            Try
                                AppService.StartAppService(False, False, True, True) 'Offline, SecureMode, SignRegistry, AppStatus
                            Catch
                            End Try
                        End If
                    Else
                        'Verificamos que la ruta de licencias exista
                        If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then 'Si no existe
                            My.Computer.FileSystem.CreateDirectory(DIRCommons) 'se crea
                        End If
                        'Verificamos que la aplicacion este instalada correctamente para poder leer el estado de licencia
                        If AppRegistry.GetValue("Version") IsNot Nothing Or AppRegistry.GetValue("Assembly Path") IsNot Nothing Or AppRegistry.GetValue("AppLicenser") IsNot Nothing Then 'Si esta correctamente instalado
                            'Revisamos que hayan datos en la llave de estado de licencia
                            AppLicenserStatus = Desencriptar(AppRegistry.GetValue("AppLicenser"), AppCryptoKey) 'Cargamos el estado de la licencia en una variable
                            If AppLicenserStatus.Contains("#Activated") Then
                                'Cadena Licencia: #Activated|Identification|AssemblyName|AssemblyVersion|UserEmail|UniqueKey|DateClaimed|Expires|False/True|randomString
                                '   Software Activado!
                                'OJO: Cualquiera podria "piratear" un mismo ensamblado y version, ya que no se ingresa ningun valor unico que solo identifique a un usuario
                                '   Podriamos guardar la ID del usuario si Debugger maneja una base de datos con una UID
                                Dim Licencia() As String = AppLicenserStatus.Split("|")
                                If Licencia(2) = My.Application.Info.AssemblyName Then
                                    If Licencia(3) = My.Application.Info.Version.ToString Or Licencia(3) = "*.*.*.*" Then
                                        'If Licencia(4) = Debugger.Login_Email Then
                                        'Else
                                        'End If
                                        LicenseIdentification = Licencia(1)
                                        LicenseAssembly = Licencia(2)
                                        LicenseVersion = Licencia(3)
                                        LicenseEmail = Licencia(4)
                                        LicenseKey = Licencia(5)
                                        LicenseDate = Licencia(6)
                                        LicenseExpires = Licencia(7)
                                        IsSubLicense = Boolean.Parse(Licencia(8))
                                        RandomString = Licencia(9)
                                        'If RandomString <> Debugger.DB_Hash Then
                                        '    MsgBox("Licencia no compatible con la base de batos del programa.", MsgBoxStyle.Critical, "Worcome Security")
                                        '    Return 0
                                        'End If
                                        If LicenseExpires = "Never" Then
                                            IsLicensed = True
                                            Return 1
                                        Else
                                            Dim date1 As Date = DateTime.Now.ToString("dd/MM/yyyy")
                                            Dim date2 As Date = LicenseExpires
                                            Dim result As Integer = DateTime.Compare(date1, date2)
                                            If result < 0 Then
                                                IsLicensed = True
                                                Return 1
                                            ElseIf result = 0 Then
                                                MsgBox("La licencia vence hoy", MsgBoxStyle.Information, "Worcome Security")
                                                IsLicensed = True
                                                Return 1
                                            Else
                                                MsgBox("La licencia ha vencido", MsgBoxStyle.Information, "Worcome Security")
                                                Return 0
                                            End If
                                        End If
                                    Else
                                        MsgBox("La licencia no coincide con la versión actual del ensamblado", MsgBoxStyle.Critical, "Worcome Security")
                                        Return 0
                                    End If
                                Else
                                    MsgBox("La licencia no coincide con el paquete del ensamblado", MsgBoxStyle.Critical, "Worcome Security")
                                    Return 0
                                End If
                            Else
                                Return 0 'Se dispara porque no hay nada en AppLicenser (="")
                            End If
                        Else
                            Return 0
                        End If
                    End If
                End If
            Else
                End 'END_PROGRAM
            End If
        End If
        Return 0 'Dudo si dejar este return
    End Function

    Sub ShowLicenceStatus()
        Panel2.Visible = True
        Button1.Enabled = False
        Button1.Visible = False
        Panel1.Visible = False
        LinkLabel1.Enabled = False
        LinkLabel1.Visible = False
        Panel1.Enabled = False
        Panel2.Dock = DockStyle.Fill
        Me.Size = New Size(340, 230)
        If LicenseVersion = "*.*.*.*" Then
            LicenseVersion = "All Versions"
        End If
        Dim LicenseEstado As String = "License Status: Activated!"
        If IsSubLicense = True Then
            LicenseEstado &= " (SubLicensed)"
        End If
        Label7.Text = LicenseEstado &
            vbCrLf & "Owner: " & LicenseEmail &
            vbCrLf & "UID: " & LicenseIdentification &
            vbCrLf & "Date: " & LicenseDate &
            vbCrLf & "Expires: " & LicenseExpires &
            vbCrLf & vbCrLf & "Assembly Information" &
            vbCrLf & "Assembly Name: " & LicenseAssembly &
            vbCrLf & "Assembly Version: " & LicenseVersion
        Me.Text = "AppLicensor | Informacion de la Licencia"
        Me.Show()
    End Sub

    Sub DownloadRemoteLicense(ByVal Ident As String)
        Try
            If My.Computer.FileSystem.FileExists(pathLicense) = True Then
                My.Computer.FileSystem.DeleteFile(pathLicense)
            End If
            Dim URLLicFile As String = ServerSwitch.DIR_AppLicenser & "/[" & Ident & "]" & My.Application.Info.AssemblyName & ".lic"
            My.Computer.Network.DownloadFile(URLLicFile, pathLicense)
            ReadLicenseInfo(pathLicense)
        Catch ex As Exception
            Console.WriteLine("[DownloadRemoteLicense@AppLicenser]Error: " & ex.Message)
        End Try
    End Sub

    Sub ReadLicenseInfo(ByVal LicensePath As String)
        Try
            Dim TXVR As New TextBox
            Dim LicenseCleaner As String = My.Computer.FileSystem.ReadAllText(LicensePath)
            If LicenseCleaner.StartsWith("#") = False Then
                LicenseCleaner = LicenseCleaner.Replace("@", "+")
                TXVR.Text = Desencriptar(LicenseCleaner, AppCryptoKey)
                If TXVR.Text.Contains("SubLicense") Then
                    IsSubLicense = True
                End If
                Dim LineasLicencia = TXVR.Lines
                LicenseIdentification = LineasLicencia(1).Split(">"c)(1).Trim()
                LicenseAssembly = LineasLicencia(2).Split(">"c)(1).Trim()
                LicenseVersion = LineasLicencia(3).Split(">"c)(1).Trim()
                LicenseEmail = LineasLicencia(4).Split(">"c)(1).Trim()
                LicenseKey = LineasLicencia(5).Split(">"c)(1).Trim()
                LicenseDate = LineasLicencia(6).Split(">"c)(1).Trim()
                LicenseExpires = LineasLicencia(7).Split(">"c)(1).Trim()
                TextBox1.Text = LicenseAssembly
                TextBox2.Text = LicenseKey
                TextBox3.Text = LicenseVersion
                LicenseIdent = tbIdentificacion.Text
                Panel1.Visible = False
            Else
                TXVR.Text = LicenseCleaner
                Dim LineasLicencia = TXVR.Lines
                LicenseIdentification = LineasLicencia(1).Split(">"c)(1).Trim()
                LicenseAssembly = LineasLicencia(2).Split(">"c)(1).Trim()
                LicenseVersion = LineasLicencia(3).Split(">"c)(1).Trim()
                'ReclamedByUser = LineasLicencia(4).Split(">"c)(1).Trim() 'Nombre de usuario
                LicenseEmail = LineasLicencia(5).Split(">"c)(1).Trim()
                LicenseDate = LineasLicencia(6).Split(">"c)(1).Trim()
                If LicenseIdentification = "Claimed" Then
                    MsgBox("Esta licencia ya fue utilizada" & vbCrLf & "(" & LicenseDate & ")", MsgBoxStyle.Information, "Worcome Security")
                    SecureClose()
                ElseIf LicenseIdentification = "Canceled" Then
                    MsgBox("Esta licencia fue deshabilitada", MsgBoxStyle.Information, "Worcome Security")
                    SecureClose()
                Else
                    TextBox1.Text = LicenseAssembly
                    TextBox2.Text = LicenseKey
                    TextBox3.Text = LicenseVersion
                    LicenseIdent = tbIdentificacion.Text
                    Panel1.Visible = False
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("[ReadLicenseInfo@AppLicenser]Error: " & ex.Message)
        End Try
    End Sub

    Sub LoadRemoteLicense()
        'Obtener los datos de la licencia para compararlos
        '   Contenido Licencia
        '0      #AppLicensor 0.0.0.0 (00.00.00.00) | Worcome Server Services | Sistema de Licencias
        '1      Identification>
        '2      AssemblyName>
        '3      AssemblyVersion>
        '4      UserEmail>
        '5      UniqueKey>
        '6      DateCreated>
        '7      Expires>
        Dim UserEmail = InputBox("Ingrese la dirección de correo electrónico vinculado a la licencia", "Worcome Security")
        Dim UniqueKey = InputBox("Ingrese la llave de activación", "Worcome Security")
        If LicenseIdentification = LicenseIdent Then
            If LicenseAssembly = My.Application.Info.AssemblyName Then
                If LicenseVersion = My.Application.Info.Version.ToString Or LicenseVersion = "*.*.*.*" Then
                    If UserEmail = LicenseEmail Then
                        If UniqueKey = LicenseKey Then
                            'Checkear la fecha de vencimiento de la licencia
                            Dim Año As String = LicenseDate.Remove(0, LicenseDate.LastIndexOf("/") + 1)
                            If Año <= DateTime.Now.ToString("yyyy") Then 'Si la licencia no esta vencida
                                'Software Activado!
                                SoftwareActivation()
                            Else 'Licencia vencida
                                MsgBox("La licencia esta vencida", MsgBoxStyle.Critical, "Worcome Security")
                                SecureClose()
                            End If
                        Else
                            MsgBox("La llave ingresada no coincide con la vinculada a la licencia", MsgBoxStyle.Critical, "Worcome Security")
                        End If
                    Else
                        MsgBox("El correo electrónico no coincide con la vinculada en la licencia", MsgBoxStyle.Critical, "Worcome Security")
                    End If
                Else
                    MsgBox("La versión de la licencia no coincide con la actual", MsgBoxStyle.Critical, "Worcome Security")
                End If
            Else
                MsgBox("El ensamblado en la licencia no coincide con el actual", MsgBoxStyle.Critical, "Worcome Security")
            End If
        Else
            MsgBox("La identificación ingresada no coincide con la vinculada en la licencia", MsgBoxStyle.Critical, "Worcome Security")
        End If
    End Sub

    Sub SoftwareActivation()
        IsLicensed = True
        If My.Computer.Network.IsAvailable Then
            Me.DialogResult = DialogResult.OK
            ClaimRemoteLicense()
            MsgBox("¡Software activado correctamente!", MsgBoxStyle.Information, "Worcome Security")
        Else
            MsgBox("No se puede activar el Software sin conexión a internet", MsgBoxStyle.Critical, "Worcome Security")
            Me.DialogResult = DialogResult.Cancel
        End If
        SecureClose()
    End Sub

    'SI EL USUARIO DESCONECTA EL INTERNET, LA LICENCIA JAMAS SERA CLAIMEADA, POR LO QUE PODRA VOLVER A SER UTILIZADA!!
    '   Solucion: si el usuario no tiene conexion, la activacion no se realiza. Obligadamente debera estar conectado
    Sub ClaimRemoteLicense() 'esto deberia ser una funcion para ver si se ha claimeado la licencia.
        Try
            Dim request As WebRequest = WebRequest.Create(ServerSwitch.DIR_AppLicenser & "/postLicense.php") 'serverswitch debe indexar la ruta general en donde se encuentran todas
            request.Method = "POST"
            Dim content As String = "#AppLicensor " & WorSupport.WorSupportVersion & " (" & WorSupport.WorSupportCompilate & ") | Worcome Server Services | Sistema de Licencias" &
                    vbCrLf & "LicStatus>Claimed" &
                    vbCrLf & "AssemblyName>" & My.Application.Info.AssemblyName &
                    vbCrLf & "AssemblyVersion>" & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ")" &
                    vbCrLf & "User>" & Environment.UserName &
                    vbCrLf & "Email>" & LicenseEmail &
                    vbCrLf & "Date>" & DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy")
            'Seria bueno cambiar este formato de lectura por un GetIniValue()
            Dim postData As String = "ident=" & LicenseIdent & "&assembly=" & My.Application.Info.AssemblyName & "&content=" & content
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
            response.Close()
            '#Activated|Identification|AssemblyName|AssemblyVersion|UserEmail|UniqueKey|DateClaimed|Expires|False/True|randomString
            AppRegistry.SetValue("AppLicenser", Encriptar("#Activated|" & LicenseIdentification & 'Identificacion del usuario
                                                          "|" & LicenseAssembly & 'Ensamblado al que va dirigido
                                                          "|" & LicenseVersion & 'Version del ensamblado al que va dirigido
                                                          "|" & LicenseEmail & 'Correo del usuario dueño
                                                          "|" & LicenseKey & 'Serial de activacion
                                                          "|" & DateTime.Now.ToString("dd/MM/yyyy") & 'Fecha de reclamacion
                                                          "|" & LicenseExpires & 'Fecha en que la licencia expira (Fecha hasta tener que adquirir otra)
                                                          "|" & IsSubLicense & 'Si es Licencia principal (False) o SubLicencia (True)
                                                          "|" & CreateRandomString(10), AppCryptoKey))
            'If GiveSubLicense = True Then
            '    CreateSubLicense()
            'End If
        Catch
            MsgBox("No se puede activar el Software sin conexión a internet" & vbCrLf & "Es posible que haya perdido la licencia", MsgBoxStyle.Critical, "Worcome Security")
            Me.DialogResult = DialogResult.Cancel
        End Try
    End Sub

    'Sub CreateSubLicense()
    '    Try
    '        Dim SubLicensePath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\[SubLicense" & LicenseIdent & "]" & My.Application.Info.AssemblyName & ".lic"
    '        If My.Computer.FileSystem.FileExists(SubLicensePath) = True Then
    '            My.Computer.FileSystem.DeleteFile(SubLicensePath)
    '        End If
    '        Dim content As String = Encriptar("#AppLicensor " & My.Application.Info.Version.ToString & " (" & Application.ProductVersion & ") " & " SubLicense | Worcome Server Services | Sistema de Licencias" &
    '                vbCrLf & "Identification>" & LicenseIdent &
    '                vbCrLf & "AssemblyName>" & LicenseAssembly &
    '                vbCrLf & "AssemblyVersion>" & LicenseVersion &
    '                vbCrLf & "UserEmail>" & LicenseEmail &
    '                vbCrLf & "UniqueKey>" & LicenseKey &
    '                vbCrLf & "DateCreated>" & LicenseDate &
    '                vbCrLf & "Expires>" & LicenseExpires, AppCryptoKey)
    '        My.Computer.FileSystem.WriteAllText(SubLicensePath, content, False)
    '        MsgBox("Sub Licencia generada." & vbCrLf & "Esta segunda licencia podra utilizarla en un segundo computador, debera arrastrar el archivo al casillero 'Identificacion'.", MsgBoxStyle.Information, "Worcome Security")
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbIdentificacion.Enabled = False
        If Panel1.Visible = True Then 'Si esta en el primer paso (Ingresar la Idetificacion)
            If tbIdentificacion.Text = Nothing Then 'Si no ingresa nada
                tbIdentificacion.Enabled = True 'Se desbloquea el textbox
                MsgBox("Ingrese una identificación", MsgBoxStyle.Exclamation, "Worcome Security") 'Mensaje de accion
            Else 'Si hay algo en el textbox
                DownloadRemoteLicense(tbIdentificacion.Text) 'Descarga la licencia con la identificacion vinculada
            End If
        Else 'Esta en el segundo paso (ya tiene la licencia, ahora se deben comparar)
            LoadRemoteLicense()
        End If
    End Sub

#Region "CryptoActions"
    Function CreateRandomString(ByVal StringLength As Integer) As String
        Dim obj As New Random()
        Dim posibles As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
        Dim longitud As Integer = posibles.Length
        Dim letra As Char
        Dim longitudnuevacadena As Integer = StringLength
        Dim nuevacadena As String = Nothing
        For i As Integer = 0 To longitudnuevacadena - 1
            letra = posibles(obj.[Next](longitud))
            nuevacadena += letra.ToString()
        Next
        Return nuevacadena
    End Function
    Function Encriptar(ByVal texto As String, ByVal CryptoKey As String) As String
        If Trim(texto) = "" Then
            Encriptar = ""
        Else
            des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(CryptoKey))
            des.Mode = CipherMode.ECB
            Dim encrypt As ICryptoTransform = des.CreateEncryptor()
            Dim buff() As Byte = UnicodeEncoding.UTF8.GetBytes(texto)
            Encriptar = Convert.ToBase64String(encrypt.TransformFinalBlock(buff, 0, buff.Length))
        End If
        Return Encriptar
    End Function
    Function Desencriptar(ByVal texto As String, ByVal CryptoKey As String) As String
        If Trim(texto) = "" Then
            Desencriptar = ""
        Else
            des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(CryptoKey))
            des.Mode = CipherMode.ECB
            Dim desencrypta As ICryptoTransform = des.CreateDecryptor()
            Dim buff() As Byte = Convert.FromBase64String(texto)
            Desencriptar = UnicodeEncoding.UTF8.GetString(desencrypta.TransformFinalBlock(buff, 0, buff.Length))
        End If
        Return Desencriptar
    End Function
#End Region

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("Serás redirigido a la página web en donde podrás obtener una licencia", MsgBoxStyle.Information, "Worcome Security")
        Process.Start(ServerSwitch.SW_UsingServer & "/Licencias.html")
    End Sub
    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        MsgBox("Es una identificación alfanumérica (con letras y números) aleatoria que identifica tu licencia. Se da como 'ID, UID o IDENT'", MsgBoxStyle.Question, "Worcome Security")
    End Sub

    'Private Sub tbIdentificacion_DragEnter(sender As Object, e As DragEventArgs) Handles tbIdentificacion.DragEnter
    '    If e.Data.GetDataPresent(DataFormats.FileDrop) Then
    '        e.Effect = DragDropEffects.All
    '    End If
    'End Sub

    'Private Sub tbIdentificacion_DragDrop(sender As Object, e As DragEventArgs) Handles tbIdentificacion.DragDrop
    '    'Si se suelta un archivo de licencia dentro del textobox de identificacion
    '    Try
    '        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
    '            tbIdentificacion.Text = "[Sub License]"
    '            Button1.Enabled = False
    '            tbIdentificacion.Enabled = False
    '            Dim strRutaArchivos() As String = e.Data.GetData(DataFormats.FileDrop)
    '            GiveSubLicense = False 'Para no generar una SubLicencia desde una SubLicencia (no es necesario, pero lo agregamos igual)
    '            IsSubLicense = True
    '            ReadLicenseInfo(strRutaArchivos(0))
    '            Panel1.Visible = False
    '            LoadRemoteLicense()
    '        End If
    '    Catch ex As Exception
    '        Console.WriteLine("[tbIdentificacion_DragDrop@AppLicenser]: " & ex.Message)
    '    End Try
    'End Sub
End Class
'ADVERTENCIA
'   SI ALGUIEN COPIA LA LLAVE DEL REGISTRO Y LA COPIA EN OTRO COMPUTADORA, POR LOGICA, ESA LICENCIA FUNCIONARIA, YA QUE SE CIFRA CON UNA MISMA LLAVE Y ADEMAS
'       NO SE GUARDA UN IDENTIFICADOR UNICO QUE DISCRIMINE.