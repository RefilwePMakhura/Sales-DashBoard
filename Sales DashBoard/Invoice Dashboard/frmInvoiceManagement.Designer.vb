<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInvoiceManagement
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboCustomerFilter = New System.Windows.Forms.ComboBox()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnFilter = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dgvInvoiceRecords = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnDeleteInvoice = New System.Windows.Forms.Button()
        Me.btnExportCSV = New System.Windows.Forms.Button()
        Me.btnNewPayment = New System.Windows.Forms.Button()
        Me.btnPrintInvoice = New System.Windows.Forms.Button()
        Me.btnInvoiceReport = New System.Windows.Forms.Button()
        Me.btnEditInvoice = New System.Windows.Forms.Button()
        Me.btnCreateInvoice = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTotalInvoice = New System.Windows.Forms.Label()
        Me.lblOutstandingamount = New System.Windows.Forms.Label()
        Me.lblOverdue = New System.Windows.Forms.Label()
        Me.lblPaid = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvInvoiceRecords, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Controls.Add(Me.cboCustomerFilter)
        Me.Panel1.Controls.Add(Me.DateTimePicker2)
        Me.Panel1.Controls.Add(Me.DateTimePicker1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnFilter)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(0, 1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(879, 88)
        Me.Panel1.TabIndex = 0
        '
        'cboCustomerFilter
        '
        Me.cboCustomerFilter.FormattingEnabled = True
        Me.cboCustomerFilter.Location = New System.Drawing.Point(110, 55)
        Me.cboCustomerFilter.Name = "cboCustomerFilter"
        Me.cboCustomerFilter.Size = New System.Drawing.Size(200, 21)
        Me.cboCustomerFilter.TabIndex = 14
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(110, 9)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker2.TabIndex = 3
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(582, 9)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker1.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(481, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "End Date:"
        '
        'btnFilter
        '
        Me.btnFilter.BackColor = System.Drawing.Color.Silver
        Me.btnFilter.Location = New System.Drawing.Point(582, 52)
        Me.btnFilter.Name = "btnFilter"
        Me.btnFilter.Size = New System.Drawing.Size(200, 25)
        Me.btnFilter.TabIndex = 2
        Me.btnFilter.Text = "Filter"
        Me.btnFilter.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(33, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Start Date:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Customer Filter:"
        '
        'dgvInvoiceRecords
        '
        Me.dgvInvoiceRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInvoiceRecords.Location = New System.Drawing.Point(0, 90)
        Me.dgvInvoiceRecords.Name = "dgvInvoiceRecords"
        Me.dgvInvoiceRecords.Size = New System.Drawing.Size(635, 182)
        Me.dgvInvoiceRecords.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.btnDeleteInvoice)
        Me.Panel3.Controls.Add(Me.btnExportCSV)
        Me.Panel3.Controls.Add(Me.btnNewPayment)
        Me.Panel3.Controls.Add(Me.btnPrintInvoice)
        Me.Panel3.Controls.Add(Me.btnInvoiceReport)
        Me.Panel3.Controls.Add(Me.btnEditInvoice)
        Me.Panel3.Controls.Add(Me.btnCreateInvoice)
        Me.Panel3.Location = New System.Drawing.Point(0, 275)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(879, 77)
        Me.Panel3.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Button1.Location = New System.Drawing.Point(789, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 36)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "Bank Account"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnDeleteInvoice
        '
        Me.btnDeleteInvoice.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDeleteInvoice.Location = New System.Drawing.Point(244, 17)
        Me.btnDeleteInvoice.Name = "btnDeleteInvoice"
        Me.btnDeleteInvoice.Size = New System.Drawing.Size(75, 36)
        Me.btnDeleteInvoice.TabIndex = 15
        Me.btnDeleteInvoice.Text = "Delete Invoice"
        Me.btnDeleteInvoice.UseVisualStyleBackColor = False
        '
        'btnExportCSV
        '
        Me.btnExportCSV.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnExportCSV.Location = New System.Drawing.Point(571, 17)
        Me.btnExportCSV.Name = "btnExportCSV"
        Me.btnExportCSV.Size = New System.Drawing.Size(75, 36)
        Me.btnExportCSV.TabIndex = 4
        Me.btnExportCSV.Text = "Export CSV"
        Me.btnExportCSV.UseVisualStyleBackColor = False
        '
        'btnNewPayment
        '
        Me.btnNewPayment.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnNewPayment.Location = New System.Drawing.Point(680, 17)
        Me.btnNewPayment.Name = "btnNewPayment"
        Me.btnNewPayment.Size = New System.Drawing.Size(75, 36)
        Me.btnNewPayment.TabIndex = 5
        Me.btnNewPayment.Text = "New Payment"
        Me.btnNewPayment.UseVisualStyleBackColor = False
        '
        'btnPrintInvoice
        '
        Me.btnPrintInvoice.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPrintInvoice.Location = New System.Drawing.Point(462, 17)
        Me.btnPrintInvoice.Name = "btnPrintInvoice"
        Me.btnPrintInvoice.Size = New System.Drawing.Size(75, 36)
        Me.btnPrintInvoice.TabIndex = 4
        Me.btnPrintInvoice.Text = "Print Invoice"
        Me.btnPrintInvoice.UseVisualStyleBackColor = False
        '
        'btnInvoiceReport
        '
        Me.btnInvoiceReport.BackColor = System.Drawing.Color.Aqua
        Me.btnInvoiceReport.Location = New System.Drawing.Point(353, 17)
        Me.btnInvoiceReport.Name = "btnInvoiceReport"
        Me.btnInvoiceReport.Size = New System.Drawing.Size(75, 36)
        Me.btnInvoiceReport.TabIndex = 3
        Me.btnInvoiceReport.Text = "Invoice Report"
        Me.btnInvoiceReport.UseVisualStyleBackColor = False
        '
        'btnEditInvoice
        '
        Me.btnEditInvoice.BackColor = System.Drawing.Color.Gray
        Me.btnEditInvoice.Location = New System.Drawing.Point(135, 17)
        Me.btnEditInvoice.Name = "btnEditInvoice"
        Me.btnEditInvoice.Size = New System.Drawing.Size(75, 36)
        Me.btnEditInvoice.TabIndex = 1
        Me.btnEditInvoice.Text = "Edit Invoice"
        Me.btnEditInvoice.UseVisualStyleBackColor = False
        '
        'btnCreateInvoice
        '
        Me.btnCreateInvoice.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCreateInvoice.Location = New System.Drawing.Point(26, 17)
        Me.btnCreateInvoice.Name = "btnCreateInvoice"
        Me.btnCreateInvoice.Size = New System.Drawing.Size(75, 36)
        Me.btnCreateInvoice.TabIndex = 0
        Me.btnCreateInvoice.Text = "Create Invoice"
        Me.btnCreateInvoice.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel2.Controls.Add(Me.lblTotalInvoice)
        Me.Panel2.Controls.Add(Me.lblOutstandingamount)
        Me.Panel2.Controls.Add(Me.lblOverdue)
        Me.Panel2.Controls.Add(Me.lblPaid)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(637, 90)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(240, 184)
        Me.Panel2.TabIndex = 4
        '
        'lblTotalInvoice
        '
        Me.lblTotalInvoice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTotalInvoice.Location = New System.Drawing.Point(166, 30)
        Me.lblTotalInvoice.Name = "lblTotalInvoice"
        Me.lblTotalInvoice.Size = New System.Drawing.Size(26, 25)
        Me.lblTotalInvoice.TabIndex = 15
        Me.lblTotalInvoice.Text = "0"
        Me.lblTotalInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblOutstandingamount
        '
        Me.lblOutstandingamount.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblOutstandingamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblOutstandingamount.Location = New System.Drawing.Point(166, 68)
        Me.lblOutstandingamount.Name = "lblOutstandingamount"
        Me.lblOutstandingamount.Size = New System.Drawing.Size(26, 25)
        Me.lblOutstandingamount.TabIndex = 16
        Me.lblOutstandingamount.Text = "0"
        Me.lblOutstandingamount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblOverdue
        '
        Me.lblOverdue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblOverdue.Location = New System.Drawing.Point(166, 106)
        Me.lblOverdue.Name = "lblOverdue"
        Me.lblOverdue.Size = New System.Drawing.Size(26, 25)
        Me.lblOverdue.TabIndex = 17
        Me.lblOverdue.Text = "0"
        Me.lblOverdue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPaid
        '
        Me.lblPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPaid.Location = New System.Drawing.Point(166, 144)
        Me.lblPaid.Name = "lblPaid"
        Me.lblPaid.Size = New System.Drawing.Size(26, 25)
        Me.lblPaid.TabIndex = 18
        Me.lblPaid.Text = "0"
        Me.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(28, 150)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(28, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Paid"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(28, 112)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Overdue"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(28, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(102, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Outstanding amount"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(28, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Total Invoices"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Quick Stats:"
        '
        'frmInvoiceManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(879, 356)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.dgvInvoiceRecords)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmInvoiceManagement"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Invoice Management"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvInvoiceRecords, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DateTimePicker2 As DateTimePicker
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnFilter As Button
    Friend WithEvents btnNewPayment As Button
    Friend WithEvents btnPrintInvoice As Button
    Friend WithEvents btnInvoiceReport As Button
    Friend WithEvents btnEditInvoice As Button
    Friend WithEvents btnCreateInvoice As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents dgvInvoiceRecords As DataGridView
    Friend WithEvents btnExportCSV As Button
    Friend WithEvents cboCustomerFilter As ComboBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblOutstandingamount As Label
    Friend WithEvents lblTotalInvoice As Label
    Friend WithEvents lblOverdue As Label
    Friend WithEvents lblPaid As Label
    Friend WithEvents btnDeleteInvoice As Button
    Friend WithEvents Button1 As Button
End Class
