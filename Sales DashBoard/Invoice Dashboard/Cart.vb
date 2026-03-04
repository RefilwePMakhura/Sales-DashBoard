Imports System.Data.OleDb

Public Class Cart
    Public Property SelectedInvoice As Decimal

    Public Sub LoadAllPurchaseOrders()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                Dim sql As String = ("SELECT PO_ID,Product,Delivery, Quantity, Amount,  LineTotal, VAT, Total FROM [Order] WHERE Supplier = ? Order BY PO_ID DESC")
                Dim da As New OleDbDataAdapter(sql, conn)
                Dim dt As New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub



    Private Sub loadPurchaseOrderBYSupplier(supplier As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = ("SELECT PO_ID,Product,Delivery, Quantity, Amount,  LineTotal, VAT, Total FROM [Order] WHERE Supplier = ? Order BY PO_ID DESC")
                Dim da As New OleDbDataAdapter(sql, conn)
                da.SelectCommand.Parameters.AddWithValue("?", supplier)
                Dim dt As New DataTable
                da.Fill(dt)
                DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Order]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub
    Private Sub loadSupplierinfo()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT SupplierName FROM [Supplier] ORDER BY SupplierName", conn)
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    ComboBox1.Items.Clear()
                    While reader.Read()
                        ComboBox1.Items.Add(reader("Supplier_Name").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load Supplier:" & ex.Message)
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Bank_Transaction.ShowDialog()
    End Sub
    Public Sub UpdateInvoiceStatus()
        Dim PaidCount As Integer = 0
        Dim OutstandingCount As Integer = 0

        For Each row As DataGridViewRow In DataGridView1.Rows
            If Not row.IsNewRow AndAlso row.Cells("Status").Value IsNot Nothing Then

                Dim StatusValue As String = row.Cells("Status").Value.ToString.Trim()

                If StatusValue = "Paid" Then
                    row.DefaultCellStyle.BackColor = Color.LightGreen
                    PaidCount += 1

                ElseIf StatusValue = "Pending" Then
                    row.DefaultCellStyle.BackColor = Color.LightSalmon
                    PaidCount += 1
                Else
                End If
                End If
        Next
        Label9.Text = PaidCount.ToString
        Label8.Text = OutstandingCount.ToString
        Label7.Text = DataGridView1.Rows.Count - 1
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click



        If DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select an order to apply payment.", "No Selection")
            Exit Sub
        End If

        Dim SelectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
        Dim Status As String = SelectedRow.Cells("Status").Value.ToString().Trim()

        If Status = "Paid" Then
            MessageBox.Show("This Order is already paid")
            Exit Sub
        End If

        Dim PaymentFrm As New PaymentFrm()


        PaymentFrm.ComboBox1.Text = SelectedRow.Cells("").Value?.ToString()
        PaymentFrm.TextBox1.Text = SelectedRow.Cells("Status").Value?.ToString()
        PaymentFrm.txtAmountPaid.Text = SelectedRow.Cells("OpeningBalance").Value?.ToString()

        PaymentFrm.ShowDialog()
    End Sub
    Public Sub UpdateInvoiceStatus(Invoice As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim cmd As New OleDbCommand("UPDATE BankAccount SET Status = ? WHERE InvoiceID = ?", conn)

                cmd.Parameters.AddWithValue("@Status", "Paid")
                cmd.Parameters.AddWithValue("@InvoiceID", Invoice)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error Updating invoice:" & ex.Message)
        End Try
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = -1 Then Exit Sub
        If ComboBox1.Text = "ALL" Then
            LoadAllPurchaseOrders()
        Else
            loadPurchaseOrderBYSupplier(ComboBox1.Text)
        End If
        UpdateInvoiceStatus()
    End Sub

    Private Sub Cart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        UpdateInvoiceStatus()
        LoadAllPurchaseOrders()
    End Sub
    Private Sub LoadPOIntoCart(PONumber As String)
        Try
            Dim dt As New DataTable()

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT [ProductName], [Quantity], [Amount], [Total], [VAT] FROM [Order] WHERE "
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        LoadAllPurchaseOrders()
        UpdateInvoiceStatus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Order_Form.ShowDialog()
    End Sub
End Class