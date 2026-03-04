Public Class Invoice_Report
    Private Sub SetUpDataGridView()
        dgvInvoiceRecords.Columns.Clear()

        'Define Columns
        dgvInvoiceRecords.Columns.Add("Columns1", "Invoice No")
        dgvInvoiceRecords.Columns.Add("Columns2", "Cutomer Name")
        dgvInvoiceRecords.Columns.Add("Columns3", "Date")
        dgvInvoiceRecords.Columns.Add("Columns4", "Subtotal")
        dgvInvoiceRecords.Columns.Add("Columns5", "Tax")
        dgvInvoiceRecords.Columns.Add("Columns6", "Grand Total")


        'Configure DataGridView Properties

        dgvInvoiceRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvInvoiceRecords.ReadOnly = True
        dgvInvoiceRecords.AllowUserToAddRows = False
        dgvInvoiceRecords.MultiSelect = False
        dgvInvoiceRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub Invoice_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetUpDataGridView()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
        frmInvoiceManagement.Show()
    End Sub
End Class