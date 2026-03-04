Imports System
Imports System.IO
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Data.OleDb
Public Class frmInvoiceManagement
    Dim amtTotalAmount As Decimal = 0D
    Dim amtOutstanding As Integer = 0
    Dim amtOverdue As Integer = 0
    Dim amtPaid As Integer = 0
    Dim zarCulture
    Public Sub LoadTotalsFromFile()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Using cmd As New OleDbCommand(
                "SELECT COUNT (*) FROM Invoice_Details", conn)
                lblTotalInvoice.Text = cmd.ExecuteScalar().ToString
            End Using

            Using cmd As New OleDbCommand(
                "SELECT COUNT(*) FROM Invoice_Details WHERE Status ='Paid'", conn)
                Dim result = cmd.ExecuteScalar()
                lblOutstandingamount.Text = If(IsDBNull(result), 0D, CDec(result)).ToString("N2")
            End Using
        End Using

    End Sub
    Public Sub UpdateInvoiceStatus()
        Dim OverdueCount As Integer = 0
        Dim PaidCount As Integer = 0
        Dim OutstandingCount As Integer = 0

        For Each row As DataGridViewRow In dgvInvoiceRecords.Rows
            If row.IsNewRow Then Continue For

            Dim Status As String = row.Cells("Status").Value.ToString
            Dim DueDate As Date
            If Status = "Paid" Then
                PaidCount += 1
                row.DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf Today > DueDate.AddDays(1) Then
                OverdueCount += 1
                row.DefaultCellStyle.BackColor = Color.Red
                row.Cells("Status").Value = "Overdue"
            Else
                OutstandingCount += 1
                row.DefaultCellStyle.BackColor = Color.Yellow
            End If
        Next
        lblOutstandingamount.Text = OutstandingCount.ToString
        lblOverdue.Text = OverdueCount.ToString
        lblPaid.Text = PaidCount.ToString
    End Sub

    Private Sub frmInvoiceManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        UpdateInvoiceStatus()
        PaymentFrm.UpdateInvoiceAsPaid()
        LoadTotalsFromFile()
    End Sub


    Public Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Invoice_Details]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                dgvInvoiceRecords.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub
    Private Function SplitCsvLine(line As String) As List(Of String)
        Dim result As New List(Of String)()
        Dim current As New System.Text.StringBuilder()
        Dim inQuotes As Boolean = False

        For i As Integer = 0 To line.Length - 1
            Dim ch As Char = line(i)

            If ch = """"c Then
                ' Quote encountered
                If inQuotes Then
                    ' Check for escaped quote ("")
                    Dim isEscaped As Boolean = (i + 1 < line.Length AndAlso line(i + 1) = """"c)
                    If isEscaped Then
                        current.Append(""""c)
                        i += 1 ' skip the second quote
                    Else
                        inQuotes = False
                    End If
                Else
                    inQuotes = True
                End If
            ElseIf ch = ","c AndAlso Not inQuotes Then
                ' Field separator
                result.Add(current.ToString())
                current.Clear()
            Else
                current.Append(ch)
            End If
        Next

        ' Add last field
        result.Add(current.ToString())

        Return result
    End Function
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnNewPayment.Click
        Dim frmInvoiceManagement As New PaymentFrm
        PaymentFrm.Show()
    End Sub


    Private Sub btnCreateInvoice_Click(sender As Object, e As EventArgs) Handles btnCreateInvoice.Click
        Dim frmInvoiceManagement As New Create_Invoice
        Create_Invoice.ShowDialog()
    End Sub

    Private Sub btnEditInvoice_Click(sender As Object, e As EventArgs) Handles btnEditInvoice.Click

    End Sub

    Private Sub btnInvoiceReport_Click(sender As Object, e As EventArgs) Handles btnInvoiceReport.Click
        Dim frmInvoiceManagement As New Invoice_Report
        Invoice_Report.Show()
    End Sub

    'Private Function IsInvoiceAlreadyPaid(Customer As String) As Boolean
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()

    '        Dim cmd As New OleDbCommand(
    '            "SELECT Status FROM Invoice_Details WHERE Customer = ?", conn)

    '        cmd.Parameters.AddWithValue("@Customer", Customer)
    '        Dim status As String = cmd.ExecuteScalar().ToString()

    '        Return status = "Paid"
    '    End Using
    'End Function

    Private Sub dgvInvoiceRecords_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInvoiceRecords.CellClick
        If e.RowIndex >= 0 Then

            If dgvInvoiceRecords.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select an invoice")

                Exit Sub
            End If
            Dim row As DataGridViewRow = dgvInvoiceRecords.SelectedRows(0)
            Dim Status As String = row.Cells("Status").Value.ToString()

            If Status = "Paid" Then


                MessageBox.Show("Invoice already as Paid.")
                Exit Sub
            End If

            Dim PaymentFrm As New PaymentFrm

            PaymentFrm.txtInvoiceNo.Text = dgvInvoiceRecords.Rows(e.RowIndex).Cells("Invoice_ID").Value.ToString
            PaymentFrm.TextBox1.Text = dgvInvoiceRecords.Rows(e.RowIndex).Cells("Status").Value.ToString
            PaymentFrm.txtTotalAmount.Text = dgvInvoiceRecords.Rows(e.RowIndex).Cells("Total_Amount").Value.ToString
            Dim refence As String = Module1.Generaterefence
            PaymentFrm.TextBox2.Text = refence

            PaymentFrm.ShowDialog()
        End If
    End Sub

    Private Sub UpgradeInvoicestatis()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BankAccounts.ShowDialog()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub


End Class