<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AppAbout
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppAbout))
        Me.picWorcome = New System.Windows.Forms.PictureBox()
        Me.chbOfflineMode = New System.Windows.Forms.CheckBox()
        Me.llblUseGuide = New System.Windows.Forms.LinkLabel()
        Me.btnCheckUpdates = New System.Windows.Forms.Button()
        Me.llblSupport = New System.Windows.Forms.LinkLabel()
        Me.lblAppDescription = New System.Windows.Forms.Label()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.btnLicense = New System.Windows.Forms.Button()
        Me.gbItems = New System.Windows.Forms.GroupBox()
        Me.btnLangSelector = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.picWorcome, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTitle.SuspendLayout()
        Me.gbItems.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'picWorcome
        '
        Me.picWorcome.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.picWorcome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picWorcome.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picWorcome.Image = Global.WorSupport.My.Resources.Resources.Worcome_PNL_Logo
        Me.picWorcome.Location = New System.Drawing.Point(5, 11)
        Me.picWorcome.Name = "picWorcome"
        Me.picWorcome.Size = New System.Drawing.Size(130, 130)
        Me.picWorcome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picWorcome.TabIndex = 0
        Me.picWorcome.TabStop = False
        '
        'chbOfflineMode
        '
        Me.chbOfflineMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chbOfflineMode.AutoSize = True
        Me.chbOfflineMode.Cursor = System.Windows.Forms.Cursors.Hand
        Me.chbOfflineMode.Location = New System.Drawing.Point(141, 95)
        Me.chbOfflineMode.Name = "chbOfflineMode"
        Me.chbOfflineMode.Size = New System.Drawing.Size(84, 17)
        Me.chbOfflineMode.TabIndex = 0
        Me.chbOfflineMode.Text = "Modo offline"
        Me.chbOfflineMode.UseVisualStyleBackColor = True
        '
        'llblUseGuide
        '
        Me.llblUseGuide.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llblUseGuide.AutoSize = True
        Me.llblUseGuide.Cursor = System.Windows.Forms.Cursors.Hand
        Me.llblUseGuide.Location = New System.Drawing.Point(312, 123)
        Me.llblUseGuide.Name = "llblUseGuide"
        Me.llblUseGuide.Size = New System.Drawing.Size(64, 13)
        Me.llblUseGuide.TabIndex = 3
        Me.llblUseGuide.TabStop = True
        Me.llblUseGuide.Text = "Guia de uso"
        '
        'btnCheckUpdates
        '
        Me.btnCheckUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheckUpdates.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCheckUpdates.Location = New System.Drawing.Point(141, 118)
        Me.btnCheckUpdates.Name = "btnCheckUpdates"
        Me.btnCheckUpdates.Size = New System.Drawing.Size(96, 23)
        Me.btnCheckUpdates.TabIndex = 1
        Me.btnCheckUpdates.Text = "Check Updates"
        Me.btnCheckUpdates.UseVisualStyleBackColor = True
        '
        'llblSupport
        '
        Me.llblSupport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llblSupport.AutoSize = True
        Me.llblSupport.BackColor = System.Drawing.Color.Transparent
        Me.llblSupport.Cursor = System.Windows.Forms.Cursors.Hand
        Me.llblSupport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llblSupport.Location = New System.Drawing.Point(243, 121)
        Me.llblSupport.Name = "llblSupport"
        Me.llblSupport.Size = New System.Drawing.Size(63, 16)
        Me.llblSupport.TabIndex = 2
        Me.llblSupport.TabStop = True
        Me.llblSupport.Text = "Soporte"
        '
        'lblAppDescription
        '
        Me.lblAppDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAppDescription.BackColor = System.Drawing.Color.Transparent
        Me.lblAppDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppDescription.Location = New System.Drawing.Point(12, 53)
        Me.lblAppDescription.Name = "lblAppDescription"
        Me.lblAppDescription.Size = New System.Drawing.Size(570, 36)
        Me.lblAppDescription.TabIndex = 5
        Me.lblAppDescription.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAppName
        '
        Me.lblAppName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAppName.BackColor = System.Drawing.Color.Transparent
        Me.lblAppName.Font = New System.Drawing.Font("Segoe UI Semibold", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppName.ForeColor = System.Drawing.Color.Black
        Me.lblAppName.Location = New System.Drawing.Point(3, 5)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(588, 48)
        Me.lblAppName.TabIndex = 4
        Me.lblAppName.Text = "Wor: AppName"
        Me.lblAppName.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblAppName.UseCompatibleTextRendering = True
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.BackColor = System.Drawing.Color.Transparent
        Me.lblInfo.ForeColor = System.Drawing.Color.DarkGray
        Me.lblInfo.Location = New System.Drawing.Point(481, 5)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(108, 14)
        Me.lblInfo.TabIndex = 7
        Me.lblInfo.Text = "0.0.0.0"
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlTitle
        '
        Me.pnlTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlTitle.Controls.Add(Me.lblInfo)
        Me.pnlTitle.Controls.Add(Me.lblAppName)
        Me.pnlTitle.Controls.Add(Me.lblAppDescription)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(594, 95)
        Me.pnlTitle.TabIndex = 8
        '
        'btnLicense
        '
        Me.btnLicense.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLicense.Location = New System.Drawing.Point(468, 118)
        Me.btnLicense.Name = "btnLicense"
        Me.btnLicense.Size = New System.Drawing.Size(96, 23)
        Me.btnLicense.TabIndex = 4
        Me.btnLicense.Text = "Licencia"
        Me.btnLicense.UseVisualStyleBackColor = True
        '
        'gbItems
        '
        Me.gbItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbItems.Controls.Add(Me.btnLangSelector)
        Me.gbItems.Controls.Add(Me.picWorcome)
        Me.gbItems.Controls.Add(Me.btnLicense)
        Me.gbItems.Controls.Add(Me.btnCheckUpdates)
        Me.gbItems.Controls.Add(Me.llblSupport)
        Me.gbItems.Controls.Add(Me.llblUseGuide)
        Me.gbItems.Controls.Add(Me.chbOfflineMode)
        Me.gbItems.Location = New System.Drawing.Point(12, 352)
        Me.gbItems.Name = "gbItems"
        Me.gbItems.Size = New System.Drawing.Size(570, 147)
        Me.gbItems.TabIndex = 10
        Me.gbItems.TabStop = False
        '
        'btnLangSelector
        '
        Me.btnLangSelector.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLangSelector.Location = New System.Drawing.Point(468, 19)
        Me.btnLangSelector.Name = "btnLangSelector"
        Me.btnLangSelector.Size = New System.Drawing.Size(96, 23)
        Me.btnLangSelector.TabIndex = 5
        Me.btnLangSelector.Text = "Lang Selector"
        Me.btnLangSelector.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 95)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(594, 416)
        Me.Panel1.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(570, 250)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Un_largo_texto_aqui_que_sea_muy_util_para_el_usuario_como_las_cosas_que_tiene_cos" &
    "as_que_lo_hacen_genial_y_ojala_mas_cosas_para_que_si_uwu_uwu_owo_uwu"
        '
        'AppAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 511)
        Me.Controls.Add(Me.gbItems)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTitle)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(800, 700)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(455, 300)
        Me.Name = "AppAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About"
        CType(Me.picWorcome, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTitle.ResumeLayout(False)
        Me.gbItems.ResumeLayout(False)
        Me.gbItems.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picWorcome As PictureBox
    Friend WithEvents chbOfflineMode As CheckBox
    Friend WithEvents llblUseGuide As LinkLabel
    Friend WithEvents btnCheckUpdates As Button
    Friend WithEvents llblSupport As LinkLabel
    Friend WithEvents lblAppDescription As Label
    Friend WithEvents lblAppName As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents pnlTitle As Panel
    Friend WithEvents btnLicense As Button
    Friend WithEvents gbItems As GroupBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents btnLangSelector As Button
End Class
