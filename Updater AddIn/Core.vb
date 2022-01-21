Module GlobalUses
    Public DIRCommons As String = "C:\Users\" & Environment.UserName & "\AppData\Local\Worcome_Studios\Commons"
    Public DIRUpdates As String = DIRCommons & "\Updates"

    Public parametros As String
End Module
Module Utility
    Sub AddToLog(ByVal from As String, ByVal content As String, Optional ByVal flag As Boolean = False)
        Try
            Dim finalContent As String = Nothing
            If flag = True Then
                finalContent = " [!!!]"
            End If
            Dim Message As String = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy") & finalContent & " [" & from & "] " & content
            Console.WriteLine("[" & from & "]" & finalContent & " " & content)
            Try
                My.Computer.FileSystem.WriteAllText(DIRCommons & "\AutoClicker.log", vbCrLf & Message, True)
            Catch
            End Try
        Catch ex As Exception
            Console.WriteLine("[AddToInstallerLog@Telemetry]Error: " & ex.Message)
        End Try
    End Sub
End Module
Module StartUp

    Sub Init()
        Try



        Catch ex As Exception
            AddToLog("Init@StartUp", "Error: " & ex.Message, True)
        End Try
    End Sub

End Module
Module AssemblyInfo
    Public AssemblyName As String
    Public AssemblyVersion As String
    Public AssemblyPath As String
    Public AssemblyProductName As String
    Public ProgramName As String

    Sub LoadAssemblyInfo()
        Try
            AppService.StartAppService(False, False, False, True, AssemblyName, AssemblyVersion) 'Offline, SecureMode, SignRegistry, AppStatus
        Catch ex As Exception
            AddToLog("LoadAssemblyInfo@AssemblyInfo", "Error: " & ex.Message, True)
        End Try
    End Sub

End Module
Module Updater
    Public InstallPath As String

    Public IsComplement As Boolean = False
    Public IsSilent As Boolean = False
    Public IsSeachForUpdates As Boolean = False
    Public IsDownloadUpdates As Boolean = False
    Public IsInstallUpdates As Boolean = False
    Public IsUpdateRegistry As Boolean = False
    Public IsForced As Boolean = False

    Public SV_InstallerStatus As String
    Public SV_AssemblyName As String
    Public SV_ProductName As String
    Public SV_Version As String
    Public SV_DownloadURL As String
    Public SV_PackageSize As String

End Module