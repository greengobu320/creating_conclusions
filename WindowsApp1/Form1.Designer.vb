<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.KryptonToolStrip1 = New Krypton.Toolkit.KryptonToolStrip()
        Me.KryptonSplitContainer1 = New Krypton.Toolkit.KryptonSplitContainer()
        Me.KryptonToolStrip2 = New Krypton.Toolkit.KryptonToolStrip()
        Me.KryptonStatusStrip1 = New Krypton.Toolkit.KryptonStatusStrip()
        Me.KryptonDataGridView1 = New Krypton.Toolkit.KryptonDataGridView()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.KryptonSplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.KryptonSplitContainer1.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.KryptonSplitContainer1.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.KryptonSplitContainer1.Panel2.SuspendLayout()
        Me.KryptonSplitContainer1.SuspendLayout()
        Me.KryptonStatusStrip1.SuspendLayout()
        CType(Me.KryptonDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'KryptonToolStrip1
        '
        Me.KryptonToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.KryptonToolStrip1.Name = "KryptonToolStrip1"
        Me.KryptonToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.KryptonToolStrip1.TabIndex = 0
        Me.KryptonToolStrip1.Text = "KryptonToolStrip1"
        '
        'KryptonSplitContainer1
        '
        Me.KryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.KryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KryptonSplitContainer1.Location = New System.Drawing.Point(0, 25)
        Me.KryptonSplitContainer1.Name = "KryptonSplitContainer1"
        '
        'KryptonSplitContainer1.Panel2
        '
        Me.KryptonSplitContainer1.Panel2.Controls.Add(Me.KryptonDataGridView1)
        Me.KryptonSplitContainer1.Panel2.Controls.Add(Me.KryptonStatusStrip1)
        Me.KryptonSplitContainer1.Panel2.Controls.Add(Me.KryptonToolStrip2)
        Me.KryptonSplitContainer1.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile
        Me.KryptonSplitContainer1.Size = New System.Drawing.Size(800, 425)
        Me.KryptonSplitContainer1.SplitterDistance = 216
        Me.KryptonSplitContainer1.SplitterWidth = 10
        Me.KryptonSplitContainer1.TabIndex = 1
        '
        'KryptonToolStrip2
        '
        Me.KryptonToolStrip2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KryptonToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.KryptonToolStrip2.Name = "KryptonToolStrip2"
        Me.KryptonToolStrip2.Size = New System.Drawing.Size(574, 25)
        Me.KryptonToolStrip2.TabIndex = 0
        Me.KryptonToolStrip2.Text = "KryptonToolStrip2"
        '
        'KryptonStatusStrip1
        '
        Me.KryptonStatusStrip1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KryptonStatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.KryptonStatusStrip1.Location = New System.Drawing.Point(0, 403)
        Me.KryptonStatusStrip1.Name = "KryptonStatusStrip1"
        Me.KryptonStatusStrip1.ProgressBars = Nothing
        Me.KryptonStatusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode
        Me.KryptonStatusStrip1.Size = New System.Drawing.Size(574, 22)
        Me.KryptonStatusStrip1.TabIndex = 1
        Me.KryptonStatusStrip1.Text = "KryptonStatusStrip1"
        '
        'KryptonDataGridView1
        '
        Me.KryptonDataGridView1.AllowUserToAddRows = False
        Me.KryptonDataGridView1.AllowUserToDeleteRows = False
        Me.KryptonDataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.KryptonDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KryptonDataGridView1.Location = New System.Drawing.Point(0, 25)
        Me.KryptonDataGridView1.Name = "KryptonDataGridView1"
        Me.KryptonDataGridView1.Size = New System.Drawing.Size(574, 378)
        Me.KryptonDataGridView1.TabIndex = 2
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.KryptonSplitContainer1)
        Me.Controls.Add(Me.KryptonToolStrip1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.KryptonSplitContainer1.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.KryptonSplitContainer1.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KryptonSplitContainer1.Panel2.ResumeLayout(False)
        Me.KryptonSplitContainer1.Panel2.PerformLayout()
        CType(Me.KryptonSplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KryptonSplitContainer1.ResumeLayout(False)
        Me.KryptonStatusStrip1.ResumeLayout(False)
        Me.KryptonStatusStrip1.PerformLayout()
        CType(Me.KryptonDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents KryptonToolStrip1 As Krypton.Toolkit.KryptonToolStrip
    Friend WithEvents KryptonSplitContainer1 As Krypton.Toolkit.KryptonSplitContainer
    Friend WithEvents KryptonDataGridView1 As Krypton.Toolkit.KryptonDataGridView
    Friend WithEvents KryptonStatusStrip1 As Krypton.Toolkit.KryptonStatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents KryptonToolStrip2 As Krypton.Toolkit.KryptonToolStrip
    Friend WithEvents Timer1 As Timer
End Class
