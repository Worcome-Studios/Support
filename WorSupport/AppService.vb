Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32
Module WorSupport
    Public ReadOnly DIRCommons As String = "C:\Users\" & System.Environment.UserName & "\AppData\Local\Worcome_Studios\Commons"
    Public ReadOnly DIRAppService As String = DIRCommons & "\AppService"
    Public ReadOnly DIRServerSwitch As String = DIRAppService & "\ServerSwitch"
    Public ReadOnly DirAppPath As String = Application.ExecutablePath

    Public ReadOnly WorSupportVersion As String = "1.2.2.0"
    Public ReadOnly WorSupportCompilate As String = "21.10.20.21"

    Public WorSupportIsActived As Boolean = True 'DE ESTAR EN False, NO SE PODRA UTILIZAR NINGUN COMPLEMENTO DE WorSupport.
    Public AppServiceSuccess As Boolean = False
    Public ServerSwitchSuccess As Boolean = False
    Public SignRegistrySuccess As Boolean = False
    Public AppStatusSuccess As Boolean = False

    Public AppServiceFilePath As String = DIRAppService & "\" & ThisAssemblyName & ".ini"
    Public ServerSwitchFilePath As String = DIRServerSwitch & "\[" & ThisAssemblyName & "]WSS_ServerSwitch.ini"
    Public ServerConfigurationFilePath As String = DIRServerSwitch & "\[" & ThisAssemblyName & "]WSS_Configuration.ini"

    Public ThisAssemblyName As String = My.Application.Info.AssemblyName
    Public ThisAssemblyVersion As String = My.Application.Info.Version.ToString
    Public ThisAssemblyProductName As String = My.Application.Info.ProductName

    Public AppLanguage As SByte = 0 '0 = ENG | 1 = ESP
    Public NullResponse As String = "None"

    <DllImport("kernel32")>
    Private Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
        'Use GetIniValue("KEY_HERE", "SubKEY_HERE", "filepath")
    End Function
    Public Function GetIniValue(section As String, key As String, filename As String, Optional defaultValue As String = Nothing) As String
        Dim sb As New StringBuilder(500)
        If GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, filename) > 0 Then
            Return sb.ToString
        Else
            Return defaultValue
        End If
    End Function
End Module
Module AppService
    Public SignRegistryStatus As Boolean
    Public AppStatusStatus As Boolean

    Public AppRegistry As RegistryKey
    Public AppServiceConfig As RegistryKey
    Public Reg_ShowAppServiceMessages As Boolean = True
    Public OfflineApp As Boolean
    Public SecureMode As Boolean

    Public DIR_AppService As String
    Public DIR_AppUpdate As String
    Public DIR_AppHelper As String
    Public DIR_AppLicenser As String
    Public DIR_AppSupport As String
    Public DIR_Telemetry As String

    Sub CommonActions()
        Try
            'CREACION DE LA CARPETAS CARPETAS.
            If My.Computer.FileSystem.DirectoryExists(DIRCommons) = False Then
                My.Computer.FileSystem.CreateDirectory(DIRCommons)
            End If
            If My.Computer.FileSystem.DirectoryExists(DIRAppService) = False Then
                My.Computer.FileSystem.CreateDirectory(DIRAppService)
            End If
            If My.Computer.FileSystem.DirectoryExists(DIRServerSwitch) = False Then
                My.Computer.FileSystem.CreateDirectory(DIRServerSwitch)
            End If
        Catch ex As Exception
            Console.WriteLine("[CommonActions@AppService]Error: " & ex.Message)
        End Try
        Try
            'ELIMINACION DE ARCHIVOS.
            If My.Computer.FileSystem.FileExists(AppServiceFilePath) = True Then
                My.Computer.FileSystem.DeleteFile(AppServiceFilePath)
            End If
            If My.Computer.FileSystem.FileExists(ServerSwitchFilePath) = True Then
                My.Computer.FileSystem.DeleteFile(ServerSwitchFilePath)
            End If
            If My.Computer.FileSystem.FileExists(ServerConfigurationFilePath) = True Then
                My.Computer.FileSystem.DeleteFile(ServerConfigurationFilePath)
            End If
        Catch ex As Exception
            Console.WriteLine("[CommonActions@AppService]Error: " & ex.Message)
        End Try
    End Sub

    'INICIO DE AppService, LLAMABLE DESDE CUALQUIER LUGAR.
    Sub StartAppService(ByVal InitOffLineApp As Boolean,
                        ByVal InitSecureMode As Boolean,
                        ByVal InitSignRegistry As Boolean,
                        ByVal InitAppStatus As Boolean,
                        Optional ByVal InitAssemblyName As String = Nothing,
                        Optional ByVal InitAssemblyVersion As String = Nothing)
        Console.WriteLine("[AppService]Started with" &
                          vbCrLf & "    Offline Mode: " & InitOffLineApp &
                          vbCrLf & "    Secure Mode: " & InitSecureMode &
                          vbCrLf & "    SignRegistry: " & InitSignRegistry &
                          vbCrLf & "    AppStatus: " & InitAppStatus &
                          vbCrLf & "        AssemblyName: " & InitAssemblyName &
                          vbCrLf & "        AssemblyVersion: " & InitAssemblyVersion)
        'APLICACION DE LAS VARIABLES PARA USO GLOBAL.
        OfflineApp = InitOffLineApp
        SecureMode = InitSecureMode
        SignRegistryStatus = InitSignRegistry
        AppStatusStatus = InitAppStatus
        'COMPROBACION DE SecureMODE
        If SecureMode Then
            If My.Computer.Network.IsAvailable = False Then
                MsgBox("Esta aplicación necesita acceso a internet para continuar", MsgBoxStyle.Critical, "Worcome Security")
            End If
            End 'END_PROGRAM
        End If
        Try
            'APLICA EL IDIOMA A LA VARIABLE AppLanguague PARA PODER USADA A NIVEL GLOBAL.
            Dim myCurrentLanguage As InputLanguage = InputLanguage.CurrentInputLanguage
            If myCurrentLanguage.Culture.EnglishName.Contains("Spanish") Then
                AppLanguage = 1
            ElseIf myCurrentLanguage.Culture.EnglishName.Contains("English") Then
                AppLanguage = 0
            Else
                AppLanguage = 0
            End If
        Catch ex As Exception
            Console.WriteLine("[StartAppService(0)@AppService]Error: " & ex.Message)
        End Try
        Try
            'INDICA EL VALOR DE LAS VARIABLES This... PARA PODER USARLAS DE FORMA GLOBAL.
            'ADEMAS ES UTIL PARA SOFTWARE COMO WorApps y WorInstaller.
            '   ASI SE PUEDE OBTENER INFORMACION SOBRE ENSAMBLADOS QUE NO ES NECESARIAMENTE EL ACTUAL.
            If InitAssemblyName IsNot Nothing And InitAssemblyVersion IsNot Nothing Then
                ThisAssemblyName = InitAssemblyName
                ThisAssemblyVersion = InitAssemblyVersion
                ThisAssemblyProductName = InitAssemblyName.Replace("Wor", Nothing)
            End If
            AppServiceFilePath = DIRCommons & "\" & ThisAssemblyName & ".ini"
            ServerSwitchFilePath = DIRServerSwitch & "\[" & ThisAssemblyName & "]WSS_ServerSwitch.ini"
            ServerConfigurationFilePath = DIRServerSwitch & "\[" & ThisAssemblyName & "]WSS_Configuration.ini"
        Catch ex As Exception
            Console.WriteLine("[StartAppService(1)@AppService]Error: " & ex.Message)
        End Try
        Try
            'INDICA LOS VALORES PARA LAS LLAVES DE REGISTRO DE WINDOWS SOBRE LA APLICACION ACTUAL.
            AppRegistry = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\" & ThisAssemblyName, True)
            'CREACION DEL REGISTRO DE WINDOWS PARA LA CONFIGURACION DE AppService.
            AppServiceConfig = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\AppService", True)
            If AppServiceConfig Is Nothing Then
                Registry.CurrentUser.CreateSubKey("Software\\Worcome_Studios\\AppService")
                Dim Reg As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\AppService", True)
                Reg.SetValue("ShowMessages", "True", RegistryValueKind.String)
            End If
            Reg_ShowAppServiceMessages = Boolean.TryParse(AppServiceConfig.GetValue("ShowMessages"), True)
        Catch ex As Exception
            Console.WriteLine("[StartAppService(2)@AppService]Error: " & ex.Message)
        End Try
        CommonActions()
        StartServerSwitch()
    End Sub

    Sub AppServiceEndingProcess()
        Try
            AppServiceConfig.SetValue("Working Server", SW_UsingServer, RegistryValueKind.String)
            AppServiceSuccess = True
        Catch
        End Try
        'AL FINALIZAR TODO AppService, ENTONCES ESTE METODO SERA LLAMADO



    End Sub
End Module
Module ServerSwitch
    'Se podrian dar mas items a esta lista a traves de dropbox u otro servidor
    Public ReadOnly ServerSwitchURLs = {
        "http://worcomestudios.comule.com/WSS_Structure/WSS_MainServerSwitch.ini",
        "http://worcomestudios.mywebcommunity.org/WSS_Structure/WSS_MainServerSwitch.ini",
        "http://worcomecorporations.000webhostapp.com/Source/WSS/WSS_Structure/WSS_MainServerSwitch.ini"}
    Public UsingServer As String = "WS1"

    Dim ServerIndex As SByte = 0
    Dim ServerTrying As SByte = 0

    Dim WithEvents DownloaderArrayServerSwitch As New Net.WebClient
    Dim WithEvents DownloaderArrayServerSwitchStructure As New Net.WebClient
    Dim DownloadURIServerSwitch As Uri
    Dim DownloadURIServerSwitchStructure As Uri

    Dim ServerStatus As String
    Dim IsStructureEnabled As Boolean
    Dim StructureGuide As String
    Dim SW_Working As Boolean
    Public SW_UsingServer As String
    Public SW_Structure As String

    Sub StartServerSwitch() 'LLAMABLE POR MAS DE UNA VEZ.
        Try
            If OfflineApp = False Then
                'OBTENER EL ARCHIVO ServerSwitch.
                '   SI FALLA, ENTONCES SE VOLVERA AQUI Y SE INCREMENTARA EL SERVIDOR CON EL QUE SE DEBE INTENTAR.
                ServerTrying = ServerTrying + 1
                DownloadURIServerSwitch = New Uri(ServerSwitchURLs(ServerIndex))
                DownloaderArrayServerSwitch.DownloadFileAsync(DownloadURIServerSwitch, ServerSwitchFilePath)
            Else
                Console.WriteLine("'AppService' Omitido")
            End If
        Catch ex As Exception
            Console.WriteLine("[StartServerSwitch@ServerSwitch]Error: " & ex.Message)
        End Try
    End Sub

    Private Sub DownloaderArrayServerSwitch_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles DownloaderArrayServerSwitch.DownloadFileCompleted
        'AL DESCARGAR EL FICHERO, SE LLAMA AL METODO PARA PODER LEERLO.
        ReadMainServerSwitchFile()
    End Sub

    Sub ReadMainServerSwitchFile()
        Try
            'LEE LOS VALORES DEL ARCHIVO Y LOS APLICA A SUS VARIABLES.
            UsingServer = GetIniValue("WSS", "Server", ServerSwitchFilePath)
            ServerStatus = GetIniValue("WSS", "Status", ServerSwitchFilePath)
            IsStructureEnabled = Boolean.TryParse(GetIniValue("Structure", "Enabled", ServerSwitchFilePath), False)
            StructureGuide = GetIniValue("Structure", "StructureFile", ServerSwitchFilePath)
            SW_Working = Boolean.TryParse(GetIniValue("ServerSwitch", "Working", ServerSwitchFilePath), False)
            SW_UsingServer = GetIniValue("ServerSwitch", "UsingServer", ServerSwitchFilePath)
            SW_Structure = GetIniValue("ServerSwitch", "WSS_Structure", ServerSwitchFilePath)
        Catch ex As Exception
            Console.WriteLine("[ReadMainServerSwitchFile(0)@ServerSwitch]Error: " & ex.Message)
        End Try
        Try
            'SI ServerTrying >= 3 ENTONCES SE INTENTO DESCARGAR EL ARCHIVO Y NINGUN SERVIDOR RESPONDIO ADECUADAMENTE.
            If ServerTrying >= 3 Then
                Console.WriteLine("[AppService]All servers failed!.")
                AppServiceSuccess = False
                ServerSwitchSuccess = False
                SignRegistrySuccess = False
                AppStatusSuccess = False
                ServerTrying = 0
                Exit Sub
            Else
                'SE VERIFICA QUE LAS VARIABLES UsingServer Y ServerStatus ESTEN HABITADAS. SI NO ENTONCES EL ARCHIVO ESTA VACIO, ESTO INDICA
                ' QUE EL SERVIDOR ESTA CAIDO O NO HA RESPONDIDO CORRECTAMENTE.
                If UsingServer = Nothing Or ServerStatus = Nothing Then
                    If ServerIndex = 0 Then
                        ServerIndex = 1
                    ElseIf ServerIndex = 1 Then
                        ServerIndex = 2
                    ElseIf ServerIndex = 2 Then
                        ServerIndex = 3
                    End If
                    'SE VE SI EL WebClient DownloaderArrayServerSwitch ESTA ANDANDO. SI ES ASI, SE DETIENE.
                    If DownloaderArrayServerSwitch.IsBusy Then
                        DownloaderArrayServerSwitch.Dispose()
                        DownloaderArrayServerSwitch.CancelAsync()
                    End If
                    'SE INTENTA DESCARGAR EL ARCHIVO CON OTRO LINK DISPONIBLE.
                    Console.WriteLine("[AppService]ServerSwitch failed!. Trying with another server " & "(" & ServerIndex & ")")
                    StartServerSwitch()
                    Exit Sub
                End If
            End If
            'EL ARCHIVO ESTA HABITADO, ESTO INDICA QUE EL SERVIDOR RESPONDE CORRECTAMENTE. ESE SERVIDOR SE UTILIZARA.
            Console.WriteLine("[AppService]Using Server: " & UsingServer)
            'COMIENZA LA DESCARGA DEL SIGUIENTE ARCHIVO WSS_MainConfiguration.ini.
            DownloadURIServerSwitchStructure = New Uri(StructureGuide)
            DownloaderArrayServerSwitchStructure.DownloadFileAsync(DownloadURIServerSwitchStructure, ServerConfigurationFilePath)
        Catch ex As Exception
            Console.WriteLine("[ReadMainServerSwitchFile(1)@ServerSwitch]Error: " & ex.Message)
            If SecureMode Then
                If My.Computer.Network.IsAvailable Then
                    MsgBox("No se logro conectar con los Servidores de Servicios de Worcome", MsgBoxStyle.Critical, "Worcome Security")
                Else
                    MsgBox("Esta aplicación necesita acceso a internet para continuar", MsgBoxStyle.Critical, "Worcome Security")
                End If
                End 'END_PROGRAM
            End If
        End Try
    End Sub

    Private Sub DownloaderArrayServerSwitchStructure_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles DownloaderArrayServerSwitchStructure.DownloadFileCompleted
        'AL DESCARGAR EL FICHERO, SE LLAMA AL METODO PARA PODER LEERLO.
        ReadMainConfigurationFile()
    End Sub

    Sub ReadMainConfigurationFile()
        Try
            'DIR_AppService = GetIniValue("ServerSwitch", "DIR_AppService", ServerConfigurationFilePath)
            'DIR_AppUpdate = GetIniValue("ServerSwitch", "DIR_AppUpdate", ServerConfigurationFilePath)
            'DIR_AppHelper = GetIniValue("ServerSwitch", "DIR_AppHelper", ServerConfigurationFilePath)
            'DIR_AppLicenser = GetIniValue("ServerSwitch", "DIR_AppLicenser", ServerConfigurationFilePath)
            'DIR_AppSupport = GetIniValue("ServerSwitch", "DIR_AppSupport", ServerConfigurationFilePath)
            'DIR_Telemetry = GetIniValue("ServerSwitch", "DIR_Telemetry", ServerConfigurationFilePath)

            'SE AGREGAN LOS VALORES DE ARCHIVO A UNA LISTA.
            Dim Analizer As New ArrayList
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_AppService", ServerConfigurationFilePath))
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_AppUpdate", ServerConfigurationFilePath))
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_AppHelper", ServerConfigurationFilePath))
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_AppLicenser", ServerConfigurationFilePath))
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_AppSupport", ServerConfigurationFilePath))
            Analizer.Add(GetIniValue("ServerSwitch", "DIR_Telemetry", ServerConfigurationFilePath))
            Dim it As SByte = 0
            'SE REEMPLAZA EL '/' POR SW_UsingServer & SW_Structure & item
            '   EJEMPLO = http://worcomestudios.comule.com/WSS_Structure/AppService
            While it < Analizer.Count
                Dim item As String = Analizer(it)
                Dim finalVar As String
                If item.StartsWith("/") Then
                    finalVar = item.Replace("/", SW_UsingServer & SW_Structure & "/")
                End If
                Analizer(it) = finalVar
                it += 1
            End While
            'For Each item As String In Analizer

            'Next
            'SE APLICAN LOS VALORES MODIFICADOS A SUS RESPECTIVAS VARIABLES
            DIR_AppService = Analizer(0)
            DIR_AppUpdate = Analizer(1)
            DIR_AppHelper = Analizer(2)
            DIR_AppLicenser = Analizer(3)
            DIR_AppSupport = Analizer(4)
            DIR_Telemetry = Analizer(5)

        Catch ex As Exception
            Console.WriteLine("[ReadMainConfigurationFile(0)@ServerSwitch]Error: " & ex.Message)
        End Try
        ServerSwitchEndingProcess()
    End Sub

    Sub ServerSwitchEndingProcess()
        ServerSwitchSuccess = True
        SignRegistryStep()
    End Sub
End Module
Module SignRegistry
    Dim ProcStatus_1 As Boolean = False
    Dim ProcStatus_2 As Boolean = False
    Dim ProcStatus_3 As Boolean = False
    Dim ProcStatus_4 As Boolean = False

    Public Reg_Assembly As String
    Public Reg_Version As String
    Public Reg_InstalledDate As String
    Public Reg_LastStart As String
    Public Reg_Directory As String
    Public Reg_AllUsersCanUse As String

    Sub SignRegistryStep()
        If SignRegistryStatus Then
            ProcStatus_1 = True
            SignRegistryApplier()
        Else
            Console.WriteLine("[AppService]'SignRegistry' Omitido")
            SignRegistryEndingProcess()
        End If
    End Sub

    Sub SignRegistryApplier()
        Try
            'SE COMPRUEBA QUE SI NO HAY UNA LLAVE CREADA, ENTONCES ESTA SE CREA.
            If AppRegistry Is Nothing Then
                Registry.CurrentUser.CreateSubKey("Software\\Worcome_Studios\\" & ThisAssemblyName)
                AppRegistry = Registry.CurrentUser.OpenSubKey("Software\\Worcome_Studios\\" & ThisAssemblyName, True)
            End If
            ProcStatus_2 = True
        Catch ex As Exception
            Console.WriteLine("[SignRegistryStep(0)@SignRegistry]Error: " & ex.Message)
        End Try
        Try
            'SE LEE EL REGISTRO Y SE APLICAN LOS VALORES EN SUS VARIABLES.
            Reg_Assembly = AppRegistry.GetValue("Assembly")
            Reg_Version = AppRegistry.GetValue("Version")
            Reg_InstalledDate = AppRegistry.GetValue("Installed Date")
            Reg_LastStart = AppRegistry.GetValue("Last Start")
            Reg_Directory = AppRegistry.GetValue("Directory")
            Try
                'SI ES LA APLICACION ORIGINAL, ENTONCES PODRA HACER CAMBIOS.
                'SI NO, ENTONCES SE TRATA DE UN "IMPOSTOR" (WorApps, WorInstaller y otros especificos)
                If My.Application.Info.AssemblyName = ThisAssemblyName Then
                    If Reg_Version = Nothing Then Reg_Version = My.Application.Info.Version.ToString
                    Dim versionApp = New Version(ThisAssemblyVersion)
                    Dim versionReg = New Version(Reg_Version)
                    Dim result = versionApp.CompareTo(versionReg)
                    If (result > 0) Then 'Sobre-actualizado App > Reg
                        If Reg_ShowAppServiceMessages = True Then
                            MsgBox("An inferior version was registered", MsgBoxStyle.Information, "Worcome Security")
                        End If
                    ElseIf (result < 0) Then 'Desactualizado App < Reg
                        If Reg_ShowAppServiceMessages = True Then
                            MsgBox("A higher version was registered", MsgBoxStyle.Information, "Worcome Security")
                        End If
                    End If
                    AppRegistry.SetValue("Version", ThisAssemblyVersion, RegistryValueKind.String)
                    AppRegistry.SetValue("Assembly Path", Application.ExecutablePath, RegistryValueKind.String)
                    AppRegistry.SetValue("Last Start", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), RegistryValueKind.String)
                    AppRegistry.SetValue("Description", My.Application.Info.Description, RegistryValueKind.String)
                    AppRegistry.SetValue("Compilated", Application.ProductVersion, RegistryValueKind.String)
                End If
                ProcStatus_3 = True
            Catch ex As Exception
                If Reg_ShowAppServiceMessages = True Then
                    MsgBox("The installation reg for this app could not be read", MsgBoxStyle.Critical, "Worcome Security")
                End If
                Console.WriteLine("[SignRegistryStep(1.0)@SignRegistry]Error: " & ex.Message)
            End Try
            ProcStatus_4 = True
        Catch ex As Exception
            Console.WriteLine("[SignRegistryStep(1)@SignRegistry]Error: " & ex.Message)
        End Try
        If ProcStatus_1 And ProcStatus_2 And ProcStatus_3 And ProcStatus_4 Then
            SignRegistryStatus = True
        End If
        SignRegistryEndingProcess()
    End Sub

    Sub SignRegistryEndingProcess()
        AppStatusStep()
    End Sub
End Module
Module AppStatus
    Dim ProcStatus_1 As Boolean = False
    Dim ProcStatus_2 As Boolean = False
    Dim ProcStatus_3 As Boolean = False


    Dim WithEvents DownloaderArrayAppService As New Net.WebClient
    Dim DownloadURIAppService As Uri

    Public Assembly_Status As Boolean
    Public Assembly_Name As String
    Public Assembly_Version As String

    Public Runtime_VisitURL As String
    Public Runtime_Message As String
    Public Runtime_ArgumentLine As String
    Public Runtime_Command As String

    Public Updates_Critical As Boolean
    Public Updates_Message As String
    Public Updates_CanUseOldVersion As Boolean

    Public Installer_Status As Boolean
    Public Installer_BinDownload As String
    Public Installer_InsDownload As String
    Public Installer_BitArch As String

    Sub AppStatusStep()
        Try
            If AppStatusStatus Then
                'COMIENZA LA DESCARGA DEL ARCHIVO WorAppName.ini
                DownloadURIAppService = New Uri(DIR_AppService & "/" & ThisAssemblyName & ".ini")
                DownloaderArrayAppService.DownloadFileAsync(DownloadURIAppService, AppServiceFilePath)
                ProcStatus_1 = True
            Else
                Console.WriteLine("[AppService]'AppStatus' Omitido")
                AppStatusEndingProcess()
            End If
        Catch ex As Exception
            Console.WriteLine("[AppStatusStep@AppStatus]Error: " & ex.Message)
        End Try
    End Sub

    Private Sub DownloaderArrayAppService_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles DownloaderArrayAppService.DownloadFileCompleted
        'AL DESCARGAR EL FICHERO, SE LLAMA AL METODO PARA PODER LEERLO.
        ReadAppStatusFile()
    End Sub

    Sub ReadAppStatusFile()
        Try
            'LEER LOS VALORES DEL ARCHIVO Y APLICARLOS A LAS VARIABLES
            Assembly_Status = Boolean.Parse(GetIniValue("Assembly", "Status", AppServiceFilePath))
            Assembly_Name = GetIniValue("Assembly", "Name", AppServiceFilePath)
            Assembly_Version = GetIniValue("Assembly", "Version", AppServiceFilePath)
            Runtime_VisitURL = GetIniValue("Runtime", "VisitURL", AppServiceFilePath)
            Runtime_Message = GetIniValue("Runtime", "Message", AppServiceFilePath)
            Runtime_ArgumentLine = GetIniValue("Runtime", "ArgumentLine", AppServiceFilePath)
            Runtime_Command = GetIniValue("Runtime", "Command", AppServiceFilePath)
            Updates_Critical = Boolean.Parse(GetIniValue("Updates", "Critical", AppServiceFilePath))
            Updates_Message = GetIniValue("Updates", "Message", AppServiceFilePath)
            Updates_CanUseOldVersion = GetIniValue("Updates", "CanUseOldVersion", AppServiceFilePath)
            Installer_Status = Boolean.Parse(GetIniValue("Installer", "Status", AppServiceFilePath))
            Installer_BinDownload = GetIniValue("Installer", "BinDownload", AppServiceFilePath)
            Installer_InsDownload = GetIniValue("Installer", "InsDownload", AppServiceFilePath)
            Installer_BitArch = GetIniValue("Installer", "BitArch", AppServiceFilePath)
            ProcStatus_2 = True
        Catch ex As Exception
            Console.WriteLine("[ReadAppStatusFile@AppStatus]Error: " & ex.Message)
        End Try
        Try
            'INICIAR ACCIONES DEPENDIENDO DE ALGUNAS VARIABLES
            If Assembly_Status Then
                If Runtime_VisitURL <> NullResponse Then
                    Process.Start(Runtime_VisitURL)
                    Console.WriteLine("[AppStatus]Visitando URL: " & Runtime_VisitURL)
                End If
                If Runtime_Message <> NullResponse Then
                    MsgBox(Runtime_Message, MsgBoxStyle.Information, "Worcome Security")
                    Console.WriteLine("[AppStatus]Mostrando mensaje: " & Runtime_Message)
                End If
                If Runtime_ArgumentLine <> NullResponse Then
                    Process.Start(DirAppPath, Runtime_ArgumentLine)
                    Console.WriteLine("[AppStatus]Reiniciando con parametros: " & Runtime_ArgumentLine)
                    End
                End If
                If Runtime_Command <> NullResponse Then
                    Process.Start(Runtime_Command)
                    Console.WriteLine("[AppStatus]Ejecutando comando: " & Runtime_Command)
                End If
                If Assembly_Version = "*.*.*.*" Then
                    Console.WriteLine("[AppStatus]Comprobacion de version omitida.")
                Else
                    Dim versionLocal = New Version(ThisAssemblyVersion)
                    Dim versionServidor = New Version(Assembly_Version)
                    Dim result = versionLocal.CompareTo(versionServidor)
                    If (result > 0) Then
                        Console.WriteLine("[AppStatus]La version actual esta por sobre la del servidor.")
                    ElseIf (result < 0) Then
                        If Updates_Message <> NullResponse Then
                            MsgBox(Updates_Message, MsgBoxStyle.Information, "Worcome Security")
                        End If
                        If Updates_Critical = True Then
                            Console.WriteLine("[AppStatus]Actualizacion critica.")
                            AppUpdate.CheckUpdater()
                            If My.Computer.FileSystem.FileExists(DIRCommons & "\Updater.exe") = True Then
                                MsgBox("Se iniciara un asistente de actualizacion", MsgBoxStyle.Information, "Worcome Security")
                                Process.Start(DIRCommons & "\Updater.exe", "/SearchForUpdates -" & ThisAssemblyVersion & " -" & ThisAssemblyVersion & " -" & DirAppPath)
                            Else
                                AppUpdate.ShowDialog()
                            End If
                            If Updates_CanUseOldVersion = False Then
                                End 'END_PROGRAM
                            End If
                        Else
                            Console.WriteLine("[AppStatus]Una nueva version esta disponible.")
                        End If
                    Else
                        Console.WriteLine("[AppStatus]Version actualizada.")
                    End If
                End If
            Else
                WorSupportIsActived = False
                If SecureMode Then
                    MsgBox("Este programa no esta habilitado para funcionar.", MsgBoxStyle.Critical, "Worcome Security")
                    End 'END_PROGRAM
                End If
            End If
            ProcStatus_3 = True
        Catch ex As Exception
            Console.WriteLine("[ReadAppStatusFile(0)@AppStatus]Error: " & ex.Message)
            If SecureMode Then
                If My.Computer.Network.IsAvailable Then
                    MsgBox("No se logro conectar con los Servidores de Servicios de Worcome", MsgBoxStyle.Critical, "Worcome Security")
                Else
                    MsgBox("Esta aplicación necesita acceso a internet para continuar", MsgBoxStyle.Critical, "Worcome Security")
                End If
                End 'END_PROGRAM
            End If
        End Try
        If ProcStatus_1 And ProcStatus_2 And ProcStatus_3 Then
            AppStatusSuccess = True
        End If
        AppStatusEndingProcess()
    End Sub

    Sub AppStatusEndingProcess()
        AppServiceEndingProcess()
    End Sub
End Module