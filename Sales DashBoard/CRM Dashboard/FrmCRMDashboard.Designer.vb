<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CRM_DashBoard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTopLead = New System.Windows.Forms.Label()
        Me.lblLeadsConversion = New System.Windows.Forms.Label()
        Me.lblOpportunities = New System.Windows.Forms.Label()
        Me.lblInteractions = New System.Windows.Forms.Label()
        Me.lblActiveLeads = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblConversionPercent = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnLeadGeneration = New System.Windows.Forms.Button()
        Me.lblPipelinePercent = New System.Windows.Forms.Label()
        Me.pbConversion = New System.Windows.Forms.ProgressBar()
        Me.pbPipeline = New System.Windows.Forms.ProgressBar()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Controls.Add(Me.lblTopLead)
        Me.Panel1.Controls.Add(Me.lblLeadsConversion)
        Me.Panel1.Controls.Add(Me.lblOpportunities)
        Me.Panel1.Controls.Add(Me.lblInteractions)
        Me.Panel1.Controls.Add(Me.lblActiveLeads)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(803, 71)
        Me.Panel1.TabIndex = 0
        '
        'lblTopLead
        '
        Me.lblTopLead.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTopLead.Location = New System.Drawing.Point(701, 36)
        Me.lblTopLead.Name = "lblTopLead"
        Me.lblTopLead.Size = New System.Drawing.Size(70, 23)
        Me.lblTopLead.TabIndex = 8
        '
        'lblLeadsConversion
        '
        Me.lblLeadsConversion.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblLeadsConversion.Location = New System.Drawing.Point(517, 38)
        Me.lblLeadsConversion.Name = "lblLeadsConversion"
        Me.lblLeadsConversion.Size = New System.Drawing.Size(97, 23)
        Me.lblLeadsConversion.TabIndex = 7
        '
        'lblOpportunities
        '
        Me.lblOpportunities.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblOpportunities.Location = New System.Drawing.Point(361, 36)
        Me.lblOpportunities.Name = "lblOpportunities"
        Me.lblOpportunities.Size = New System.Drawing.Size(70, 23)
        Me.lblOpportunities.TabIndex = 0
        '
        'lblInteractions
        '
        Me.lblInteractions.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblInteractions.Location = New System.Drawing.Point(191, 38)
        Me.lblInteractions.Name = "lblInteractions"
        Me.lblInteractions.Size = New System.Drawing.Size(70, 23)
        Me.lblInteractions.TabIndex = 6
        '
        'lblActiveLeads
        '
        Me.lblActiveLeads.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblActiveLeads.Location = New System.Drawing.Point(21, 34)
        Me.lblActiveLeads.Name = "lblActiveLeads"
        Me.lblActiveLeads.Size = New System.Drawing.Size(70, 23)
        Me.lblActiveLeads.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Location = New System.Drawing.Point(680, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 62)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Top Lead Source"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(512, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(116, 62)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Lead Conversion"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(344, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(116, 62)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Open Opportunities"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(176, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 62)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Interactions This Month"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 62)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Active Leads"
        '
        'lblConversionPercent
        '
        Me.lblConversionPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblConversionPercent.Location = New System.Drawing.Point(598, 301)
        Me.lblConversionPercent.Name = "lblConversionPercent"
        Me.lblConversionPercent.Size = New System.Drawing.Size(30, 20)
        Me.lblConversionPercent.TabIndex = 1
        Me.lblConversionPercent.Text = "0"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel2.Controls.Add(Me.Chart1)
        Me.Panel2.Location = New System.Drawing.Point(475, 77)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(328, 221)
        Me.Panel2.TabIndex = 2
        '
        'Chart1
        '
        ChartArea2.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend2)
        Me.Chart1.Location = New System.Drawing.Point(3, 1)
        Me.Chart1.Name = "Chart1"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Size = New System.Drawing.Size(328, 218)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel3.Controls.Add(Me.Button11)
        Me.Panel3.Controls.Add(Me.Button12)
        Me.Panel3.Controls.Add(Me.Button13)
        Me.Panel3.Controls.Add(Me.Button14)
        Me.Panel3.Controls.Add(Me.Button15)
        Me.Panel3.Location = New System.Drawing.Point(0, 420)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(803, 79)
        Me.Panel3.TabIndex = 0
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button11.Location = New System.Drawing.Point(43, 17)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(84, 40)
        Me.Button11.TabIndex = 0
        Me.Button11.Text = "New Lead"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button12.Location = New System.Drawing.Point(523, 17)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(84, 40)
        Me.Button12.TabIndex = 1
        Me.Button12.Text = "Settings"
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button13
        '
        Me.Button13.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button13.Location = New System.Drawing.Point(203, 17)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(84, 40)
        Me.Button13.TabIndex = 2
        Me.Button13.Text = "Contact Log"
        Me.Button13.UseVisualStyleBackColor = False
        '
        'Button14
        '
        Me.Button14.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button14.Location = New System.Drawing.Point(363, 17)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(84, 40)
        Me.Button14.TabIndex = 3
        Me.Button14.Text = "Activity Dashboard"
        Me.Button14.UseVisualStyleBackColor = False
        '
        'Button15
        '
        Me.Button15.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Button15.Location = New System.Drawing.Point(683, 17)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(84, 40)
        Me.Button15.TabIndex = 4
        Me.Button15.Text = "Marketing"
        Me.Button15.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel4.Controls.Add(Me.Button9)
        Me.Panel4.Controls.Add(Me.Button8)
        Me.Panel4.Controls.Add(Me.Button7)
        Me.Panel4.Controls.Add(Me.Button6)
        Me.Panel4.Controls.Add(Me.Button5)
        Me.Panel4.Controls.Add(Me.Button4)
        Me.Panel4.Controls.Add(Me.Button3)
        Me.Panel4.Controls.Add(Me.Button2)
        Me.Panel4.Controls.Add(Me.btnLeadGeneration)
        Me.Panel4.Location = New System.Drawing.Point(0, 77)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(469, 337)
        Me.Panel4.TabIndex = 3
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(22, 139)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(102, 50)
        Me.Button9.TabIndex = 8
        Me.Button9.Text = "Initial Contact"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(22, 250)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(102, 47)
        Me.Button8.TabIndex = 7
        Me.Button8.Text = "Deal Closer"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(344, 33)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(91, 45)
        Me.Button7.TabIndex = 6
        Me.Button7.Text = "Lead Assignment"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(344, 140)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(91, 46)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Proposal"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(344, 248)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(91, 47)
        Me.Button5.TabIndex = 4
        Me.Button5.Text = "Reporting"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(189, 250)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(90, 47)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Past Sales Support"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(189, 141)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(90, 46)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Needs Analysis"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(190, 33)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(90, 45)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Lead Qualification"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnLeadGeneration
        '
        Me.btnLeadGeneration.Location = New System.Drawing.Point(24, 33)
        Me.btnLeadGeneration.Name = "btnLeadGeneration"
        Me.btnLeadGeneration.Size = New System.Drawing.Size(102, 45)
        Me.btnLeadGeneration.TabIndex = 0
        Me.btnLeadGeneration.Text = "Lead Generation"
        Me.btnLeadGeneration.UseVisualStyleBackColor = True
        '
        'lblPipelinePercent
        '
        Me.lblPipelinePercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPipelinePercent.Location = New System.Drawing.Point(598, 353)
        Me.lblPipelinePercent.Name = "lblPipelinePercent"
        Me.lblPipelinePercent.Size = New System.Drawing.Size(30, 17)
        Me.lblPipelinePercent.TabIndex = 9
        Me.lblPipelinePercent.Text = "0"
        '
        'pbConversion
        '
        Me.pbConversion.Location = New System.Drawing.Point(475, 327)
        Me.pbConversion.Name = "pbConversion"
        Me.pbConversion.Size = New System.Drawing.Size(328, 23)
        Me.pbConversion.TabIndex = 4
        '
        'pbPipeline
        '
        Me.pbPipeline.Location = New System.Drawing.Point(475, 378)
        Me.pbPipeline.Name = "pbPipeline"
        Me.pbPipeline.Size = New System.Drawing.Size(328, 23)
        Me.pbPipeline.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(483, 304)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(103, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Conversion Percent:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(483, 359)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Pipeline Percent:"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 478)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(807, 22)
        Me.StatusStrip1.TabIndex = 10
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(119, 17)
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        '
        'CRM_DashBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(807, 500)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.lblConversionPercent)
        Me.Controls.Add(Me.lblPipelinePercent)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pbPipeline)
        Me.Controls.Add(Me.pbConversion)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "CRM_DashBoard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CRM_DashBoard"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Button11 As Button
    Friend WithEvents Button12 As Button
    Friend WithEvents Button13 As Button
    Friend WithEvents Button14 As Button
    Friend WithEvents Button15 As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Button9 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnLeadGeneration As Button
    Friend WithEvents lblTopLead As Label
    Friend WithEvents lblLeadsConversion As Label
    Friend WithEvents lblOpportunities As Label
    Friend WithEvents lblInteractions As Label
    Friend WithEvents lblActiveLeads As Label
    Friend WithEvents pbConversion As ProgressBar
    Friend WithEvents pbPipeline As ProgressBar
    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents lblConversionPercent As Label
    Friend WithEvents lblPipelinePercent As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
End Class
