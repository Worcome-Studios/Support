Public Class Main

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        parametros = Command()
        ReadParameters(parametros)
        Init()
    End Sub

    Sub ReadParameters(ByVal args As String)
        Try
            If My.Application.CommandLineArgs.Count > 0 Then
                If args.StartsWith("/SearchForUpdates") Then
                    Try
                        Dim Argumento As String = Args
                        Argumento = Argumento.Replace("/SearchForUpdates ", Nothing)
                        Dim _Args As String() = Argumento.Split("-")
                        AssemblyName = _Args(1).Trim()
                        AssemblyVersion = _Args(2).Trim()
                        AssemblyPath = _Args(3).Trim()
                        ProgramName = _Args(3).Trim()
                        AssemblyProductName = AssemblyName.Replace("Wor", Nothing)
                        ProgramName = ProgramName.Remove(0, ProgramName.LastIndexOf("\") + 1)
                        ProgramName = ProgramName.Replace(".exe", Nothing)
                        InstallPath = IO.Path.GetDirectoryName(AssemblyPath)
                        Console.WriteLine(AssemblyName & ", " & AssemblyVersion & ", " & ProgramName & ", " & AssemblyPath)
                        IsSeachForUpdates = True
                        LoadAssemblyInfo()
                    Catch
                        End
                    End Try
                ElseIf args.StartsWith("/DownloadUpdates") Then

                End If
                Console.WriteLine("No args(), no party.")
                End
            End If
        Catch ex As Exception
            AddToLog("ReadParameters@Main", "Error: " & ex.Message, True)
        End Try
    End Sub
End Class