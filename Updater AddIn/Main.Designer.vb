<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.AppIcon = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UpdaterLog = New System.Windows.Forms.RichTextBox()
        Me.Title = New System.Windows.Forms.Label()
        Me.DownloadStatus = New System.Windows.Forms.Label()
        Me.StatusBar = New System.Windows.Forms.ProgressBar()
        CType(Me.AppIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AppIcon
        '
        Me.AppIcon.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AppIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.AppIcon.ErrorImage = Nothing
        Me.AppIcon.InitialImage = Nothing
        Me.AppIcon.Location = New System.Drawing.Point(431, 8)
        Me.AppIcon.Name = "AppIcon"
        Me.AppIcon.Size = New System.Drawing.Size(45, 45)
        Me.AppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.AppIcon.TabIndex = 11
        Me.AppIcon.TabStop = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(36, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(412, 19)
        Me.Label1.TabIndex = 10
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UpdaterLog
        '
        Me.UpdaterLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdaterLog.Location = New System.Drawing.Point(36, 59)
        Me.UpdaterLog.Name = "UpdaterLog"
        Me.UpdaterLog.ReadOnly = True
        Me.UpdaterLog.Size = New System.Drawing.Size(412, 130)
        Me.UpdaterLog.TabIndex = 6
        Me.UpdaterLog.Text = ""
        '
        'Title
        '
        Me.Title.AutoSize = True
        Me.Title.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.Location = New System.Drawing.Point(12, 9)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(164, 31)
        Me.Title.TabIndex = 7
        Me.Title.Text = "Actualizador"
        '
        'DownloadStatus
        '
        Me.DownloadStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DownloadStatus.Location = New System.Drawing.Point(112, 192)
        Me.DownloadStatus.Name = "DownloadStatus"
        Me.DownloadStatus.Size = New System.Drawing.Size(261, 35)
        Me.DownloadStatus.TabIndex = 8
        Me.DownloadStatus.Text = "Esperando..."
        Me.DownloadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StatusBar
        '
        Me.StatusBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusBar.Location = New System.Drawing.Point(12, 230)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.Size = New System.Drawing.Size(460, 19)
        Me.StatusBar.Step = 1
        Me.StatusBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.StatusBar.TabIndex = 9
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 261)
        Me.Controls.Add(Me.AppIcon)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UpdaterLog)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.DownloadStatus)
        Me.Controls.Add(Me.StatusBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AppUpdater Add-In"
        CType(Me.AppIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AppIcon As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents UpdaterLog As RichTextBox
    Friend WithEvents Title As Label
    Friend WithEvents DownloadStatus As Label
    Friend WithEvents StatusBar As ProgressBar
End Class
