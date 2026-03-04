<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInventory_Dashboard
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblTotalStockValue = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblLowStockItems = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblTotalSKUs = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnProducts = New System.Windows.Forms.Button()
        Me.btnStockMovement = New System.Windows.Forms.Button()
        Me.btnSupplers = New System.Windows.Forms.Button()
        Me.btnAdjustStock = New System.Windows.Forms.Button()
        Me.btnPurchaseOrder = New System.Windows.Forms.Button()
        Me.btnInventoryReport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox3)
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(854, 131)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblTotalStockValue)
        Me.GroupBox2.Location = New System.Drawing.Point(232, 15)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(140, 100)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Total Stock Value"
        '
        'lblTotalStockValue
        '
        Me.lblTotalStockValue.AutoSize = True
        Me.lblTotalStockValue.Location = New System.Drawing.Point(41, 45)
        Me.lblTotalStockValue.Name = "lblTotalStockValue"
        Me.lblTotalStockValue.Size = New System.Drawing.Size(36, 13)
        Me.lblTotalStockValue.TabIndex = 1
        Me.lblTotalStockValue.Text = "R0.00"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblLowStockItems)
        Me.GroupBox3.Location = New System.Drawing.Point(444, 15)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(140, 100)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Low Stock Items"
        '
        'lblLowStockItems
        '
        Me.lblLowStockItems.AutoSize = True
        Me.lblLowStockItems.Location = New System.Drawing.Point(52, 45)
        Me.lblLowStockItems.Name = "lblLowStockItems"
        Me.lblLowStockItems.Size = New System.Drawing.Size(13, 13)
        Me.lblLowStockItems.TabIndex = 2
        Me.lblLowStockItems.Text = "0"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Location = New System.Drawing.Point(656, 15)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(140, 100)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Pending Receipts"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(50, 45)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "0"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblTotalSKUs)
        Me.GroupBox1.Location = New System.Drawing.Point(20, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(140, 100)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Total SKUs"
        '
        'lblTotalSKUs
        '
        Me.lblTotalSKUs.AutoSize = True
        Me.lblTotalSKUs.Location = New System.Drawing.Point(48, 45)
        Me.lblTotalSKUs.Name = "lblTotalSKUs"
        Me.lblTotalSKUs.Size = New System.Drawing.Size(13, 13)
        Me.lblTotalSKUs.TabIndex = 0
        Me.lblTotalSKUs.Text = "0"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Chart1)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Location = New System.Drawing.Point(376, 135)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(481, 312)
        Me.Panel2.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(306, 154)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "Avg days to stock:"
        '
        'Chart1
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(6, 27)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Size = New System.Drawing.Size(294, 252)
        Me.Chart1.TabIndex = 4
        Me.Chart1.Text = "Chart1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(306, 66)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 13)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "On hand value:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(306, 32)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Quick Stats:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 11)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Stock Distribuction "
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel3.Controls.Add(Me.Button13)
        Me.Panel3.Controls.Add(Me.Button12)
        Me.Panel3.Controls.Add(Me.Button11)
        Me.Panel3.Controls.Add(Me.Button10)
        Me.Panel3.Controls.Add(Me.Button9)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(3, 135)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(371, 312)
        Me.Panel3.TabIndex = 2
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(30, 206)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(309, 23)
        Me.Button13.TabIndex = 5
        Me.Button13.Text = "Suppliers"
        Me.Button13.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Location = New System.Drawing.Point(30, 256)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(309, 23)
        Me.Button12.TabIndex = 4
        Me.Button12.Text = "Stock Adjustment"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Location = New System.Drawing.Point(30, 156)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(309, 23)
        Me.Button11.TabIndex = 3
        Me.Button11.Text = "Stock Movements"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(30, 106)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(309, 23)
        Me.Button10.TabIndex = 2
        Me.Button10.Text = "Products"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(30, 56)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(309, 23)
        Me.Button9.TabIndex = 1
        Me.Button9.Text = "Overview"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Inventory Actions"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel5.Controls.Add(Me.Button1)
        Me.Panel5.Controls.Add(Me.btnProducts)
        Me.Panel5.Controls.Add(Me.btnStockMovement)
        Me.Panel5.Controls.Add(Me.btnSupplers)
        Me.Panel5.Controls.Add(Me.btnAdjustStock)
        Me.Panel5.Controls.Add(Me.btnPurchaseOrder)
        Me.Panel5.Controls.Add(Me.btnInventoryReport)
        Me.Panel5.Controls.Add(Me.btnClose)
        Me.Panel5.Location = New System.Drawing.Point(0, 449)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(857, 138)
        Me.Panel5.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button1.Location = New System.Drawing.Point(123, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 45)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Overview"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnProducts
        '
        Me.btnProducts.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnProducts.Location = New System.Drawing.Point(307, 20)
        Me.btnProducts.Name = "btnProducts"
        Me.btnProducts.Size = New System.Drawing.Size(95, 45)
        Me.btnProducts.TabIndex = 1
        Me.btnProducts.Text = "Products"
        Me.btnProducts.UseVisualStyleBackColor = False
        '
        'btnStockMovement
        '
        Me.btnStockMovement.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnStockMovement.Location = New System.Drawing.Point(484, 20)
        Me.btnStockMovement.Name = "btnStockMovement"
        Me.btnStockMovement.Size = New System.Drawing.Size(95, 45)
        Me.btnStockMovement.TabIndex = 2
        Me.btnStockMovement.Text = "Stock Movement"
        Me.btnStockMovement.UseVisualStyleBackColor = False
        '
        'btnSupplers
        '
        Me.btnSupplers.BackColor = System.Drawing.Color.Yellow
        Me.btnSupplers.Location = New System.Drawing.Point(669, 20)
        Me.btnSupplers.Name = "btnSupplers"
        Me.btnSupplers.Size = New System.Drawing.Size(95, 45)
        Me.btnSupplers.TabIndex = 3
        Me.btnSupplers.Text = "Suppliers"
        Me.btnSupplers.UseVisualStyleBackColor = False
        '
        'btnAdjustStock
        '
        Me.btnAdjustStock.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnAdjustStock.Location = New System.Drawing.Point(123, 83)
        Me.btnAdjustStock.Name = "btnAdjustStock"
        Me.btnAdjustStock.Size = New System.Drawing.Size(95, 45)
        Me.btnAdjustStock.TabIndex = 4
        Me.btnAdjustStock.Text = "Stock Adjustment"
        Me.btnAdjustStock.UseVisualStyleBackColor = False
        '
        'btnPurchaseOrder
        '
        Me.btnPurchaseOrder.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnPurchaseOrder.Location = New System.Drawing.Point(306, 83)
        Me.btnPurchaseOrder.Name = "btnPurchaseOrder"
        Me.btnPurchaseOrder.Size = New System.Drawing.Size(95, 45)
        Me.btnPurchaseOrder.TabIndex = 5
        Me.btnPurchaseOrder.Text = "Purchase Order"
        Me.btnPurchaseOrder.UseVisualStyleBackColor = False
        '
        'btnInventoryReport
        '
        Me.btnInventoryReport.BackColor = System.Drawing.Color.DarkGray
        Me.btnInventoryReport.Location = New System.Drawing.Point(485, 83)
        Me.btnInventoryReport.Name = "btnInventoryReport"
        Me.btnInventoryReport.Size = New System.Drawing.Size(95, 45)
        Me.btnInventoryReport.TabIndex = 6
        Me.btnInventoryReport.Text = "Inventory Report"
        Me.btnInventoryReport.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Lime
        Me.btnClose.Location = New System.Drawing.Point(669, 83)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(95, 45)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'FrmInventory_Dashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(858, 587)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FrmInventory_Dashboard"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "inventory Dashboard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnInventoryReport As Button
    Friend WithEvents btnPurchaseOrder As Button
    Friend WithEvents btnAdjustStock As Button
    Friend WithEvents btnSupplers As Button
    Friend WithEvents btnStockMovement As Button
    Friend WithEvents btnProducts As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel5 As Panel
    Friend WithEvents btnClose As Button
    Friend WithEvents lblTotalStockValue As Label
    Friend WithEvents lblLowStockItems As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblTotalSKUs As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents Label8 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Button13 As Button
    Friend WithEvents Button12 As Button
    Friend WithEvents Button11 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents Button9 As Button
End Class
