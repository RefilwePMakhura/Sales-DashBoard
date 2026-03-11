Imports System.Data.OleDb

Public Class Sales_Order

    Public Property SelectedProduct As String
    Public Property SelectedQuantity As String
    Public Property SelectedSubtotal As String
    Public Property SelectedDiscount As String
    Public Property SelectedVAT As String
    Public Property SelectedTotal As String
    Private Sub LoadCompanyInfo()
        Using conn As New OleDb.OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand("SELECT TOP 1 * FROM CompanySettings", conn)
            Using dr As OleDb.OleDbDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    Label12.Text = dr("CompanyName").ToString()
                    Label13.Text = dr("Address").ToString()
                    Label14.Text = dr("Phone").ToString()
                    Label15.Text = dr("Email").ToString

                    Dim logoPath As String = dr("LogoPath").ToString()
                    If IO.File.Exists(logoPath) Then
                        PictureBox1.Image = Image.FromFile(logoPath)
                    End If
                End If
            End Using
        End Using
    End Sub

    'Private Function GetRow(table As String, field As String, value As String) As OleDbDataReader
    '    Dim conn As New OleDbConnection(Module1.ConnectionString)
    '    Dim cmd As New OleDbCommand($"SELECT * from {table} WHERE {field} = ?", conn)
    '    cmd.Parameters.AddWithValue("?", OleDbType.VarChar).Value = value
    '    conn.Open()
    '    Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

    'End Function


    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Using r = GetRow("Customer_Details", "Customer_Name", ComboBox2.Text)
            If r.Read() Then
                TextBox3.Text = r("Email").ToString()
                TextBox1.Text = r("Contact").ToString()
                TextBox2.Text = r("Address").ToString()

            End If
        End Using
    End Sub

    Public Property Product As String
    Public Property Quantity As String
    Public Property Total_Amount As String
    Public Property Customer As String
    Private Unit_Price As Decimal = 0D
    Private Sub Sales_Order_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '  CalculateRow()
        LoadCompanyInfo()
        LoadData()
        'salesLoadData()
        ReloadCustomersCombo()
        'Clearform()
        ClearOrderForm()
        LoadSalesOrder()
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing
        ComboBox1.Text = Product
        TextBox9.Text = Quantity
        TextBox4.Text = Total_Amount
        LoadProductsToComboBox()

    End Sub

    Private Sub Clearform()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()

    End Sub
    Private Sub dgvOrderRecords_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        'If e.RowIndex < 0 Then Exit Sub

        'If DataGridView1.Columns(e.ColumnIndex).Name <> "Product" Then Exit Sub
        'Dim row = DataGridView1.Rows(e.RowIndex)
        'Dim ProductName = row.Cells("Product").Value?.ToString()
        'If String.IsNullOrEmpty(ProductName) Then Exit Sub

        'Using conn As New OleDbConnection(ConnectionString)
        '    conn.Open()
        '    Using cmd As New OleDbCommand(" SELECT  Unit_Price FROM [Product_Details] WHERE Product_Name = ?", conn)
        '        cmd.Parameters.AddWithValue("@p1", ProductName)
        '        Dim Price = cmd.ExecuteScalar()
        '        If Price IsNot Nothing Then
        '            row.Cells("Amount").Value = Convert.ToDecimal(Price)
        '            row.Cells("Quantity").Value = 1
        '            'CalculateRow(row)
        '            'CalculateGrandTotal()
        '        End If
        '    End Using
        'End Using

    End Sub

    Private Sub dgvInvoiceLines_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        If row.Cells("IsInvoiced").Value?.ToString() = "Yes" Then
            MessageBox.Show("This Sales order has already been invoiced and cannot br edited",
                            "Editing Locked",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End If
    End Sub
    Private Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Sales_Order]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub
    Public Function GetRow(table As String, field As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(Module1.ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * from {table} WHERE {field} = ?", conn)
        cmd.Parameters.AddWithValue("?", OleDbType.VarChar).Value = value
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

    End Function
  

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim newInvoiceID As String = Module1.GenerateInvoiceID()
        TextBox7.Text = newInvoiceID
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Using cmd As New OleDbCommand("INSERT INTO [Sales_Order] ([Product], [Quantity], [Discount], [Amount], [VAT], [Total], [InvoiceID]) VALUES (?,?,?,?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("?", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("?", TextBox9.Text)
                    cmd.Parameters.AddWithValue("?", TextBox6.Text)
                    cmd.Parameters.AddWithValue("?", TextBox5.Text)
                    cmd.Parameters.AddWithValue("?", TextBox8.Text)
                    cmd.Parameters.AddWithValue("?", TextBox4.Text)
                    cmd.Parameters.AddWithValue("?", TextBox7.Text)
                    cmd.ExecuteNonQuery()
                End Using

                conn.Close()

            End Using

            MessageBox.Show("Sales order saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'Dim GoToCreate_Invoice As DialogResult = MessageBox.Show("Do you want to Create an invoice for this sales order now? ", "Create_Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            'If GoToCreate_Invoice = DialogResult.Yes Then

            'Dim Invoice As New Create_Invoice
            'If Not String.IsNullOrWhiteSpace(SelectedProduct) Then

            Create_Invoice.SetupInvoiceGrid()

            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.IsNewRow Then Continue For
                'If GoToCreate_Invoice = DialogResult.Yes Then
                Dim invoiceForm As New Create_Invoice
                invoiceForm.ComboBox3.Text = ComboBox2.Text
                '  invoiceForm.invoiceid = newInvoiceID
                invoiceForm.SelectedProduct = row.Cells("Product").Value.ToString()
                invoiceForm.SelectedQuantity = CInt(row.Cells("Quantity").Value)
                invoiceForm.SelectedDiscount = 0D
                invoiceForm.SelectedSubtotal = CInt(row.Cells("Amount").Value)
                invoiceForm.SelectedTotal = CInt(row.Cells("Total").Value)
                invoiceForm.SelectedVAT = CInt(row.Cells("VAT").Value)


                invoiceForm.txtDiscount.Text = TextBox6.Text
                invoiceForm.txtSubtotal.Text = TextBox5.Text
                invoiceForm.txtTotalAmount.Text = TextBox4.Text
                invoiceForm.txtTax.Text = TextBox8.Text


                invoiceForm.ShowDialog()
                '  End If
            Next



            '  End If

            ' Next

            LoadData()
            ' Clear DataGridView rows
            'DataGridView1.Rows.Clear()

            '' Allow new entry again
            'DataGridView1.ReadOnly = False
            'DataGridView1.AllowUserToAddRows = True

            ' Reset totals & textboxes
            TextBox5.Clear()   ' Subtotal
            TextBox6.Clear()   ' Discount
            TextBox8.Clear()   ' VAT
            TextBox4.Clear()   ' Grand Total
            ClearOrderForm()

            ' Dim res = MessageBox.Show(
            '"Do you want to Create an invoice for this sales order?",
            '"Create Invoice",
            'MessageBoxButtons.YesNo,
            'MessageBoxIcon.Question)
            ' If res = DialogResult.No Then Exit Sub
            ' Create_Invoice.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Failed to save:  " & ex.Message)
            MessageBox.Show("Stack Trace:" & ex.StackTrace)
            Debug.WriteLine(ex.ToString)
        End Try


        ' Configure DatGridView properties
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.MultiSelect = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill

        ' DataGridView1.Rows.Clear()
    End Sub
    Private Sub PromptInvoice(OrderID As String)

        Dim Results As DialogResult = MessageBox.Show(
            $"Sale Order has been succefully saved." & vbCrLf &
            $"Do you want to view the invoice for this order?",
            "View Invoice?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If Results = DialogResult.Yes Then






        End If
    End Sub
    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString)
    End Sub
    Private Sub ClearOrderForm()
        'DataGridView1.DataSource = Nothing
        'DataGridView1.Rows.Clear()

        TextBox5.Clear()
        TextBox6.Clear()
        TextBox8.Clear()
        TextBox4.Clear()
        TextBox9.Clear()
    End Sub
    'Private Sub UpdateStockAfterPurchase()
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()

    '        For Each row As DataGridViewRow In DataGridView1.Rows
    '            If row.IsNewRow Then Continue For

    '            Dim ProductName As String = row.Cells("Product").Value.ToString()
    '            Dim QuantityPurchased As Integer = Convert.ToInt32(row.Cells("Quantity").Value)

    '            Using cmd As New OleDbCommand(
    '            "UPDATE Product_Details
    '            SET Current_Stock = Current_Stock - ?
    '           WHERE Product_Name = ?", conn)

    '                cmd.Parameters.Add("@Current_Stock", OleDbType.Integer).Value = QuantityPurchased
    '                cmd.Parameters.Add("@Product", OleDbType.VarWChar).Value = ProductName

    '                cmd.ExecuteNonQuery()
    '            End Using
    '        Next
    '    End Using
    'End Sub
    'Private Sub UpdateTotals()
    '    Dim subtotal As Decimal = 0D
    '    Dim discountRate As Decimal = 0D
    '    Dim vatRate As Decimal = 0.15D        ' 15% VAT

    '    ' --- Step 1: Calculate subtotal from all invoice lines ---
    '    For Each row As DataGridViewRow In DataGridView1.Rows
    '        If row.IsNewRow Then Continue For
    '        Dim linetotal As Decimal = 0D
    '        Decimal.TryParse(Convert.ToString(row.Cells("LineTotal").Value), linetotal)
    '        subtotal += linetotal
    '    Next

    '    Decimal.TryParse(TextBox6.Text, discountRate)
    '    Dim tax = subtotal * vatRate
    '    Dim total = subtotal + tax - discountRate
    '    If total < 0 Then total = 0D

    '    TextBox5.Text = subtotal.ToString("N2")
    '    TextBox6.Text = discountRate.ToString()
    '    TextBox8.Text = tax.ToString("N2")
    '    TextBox4.Text = total.ToString("N2")
    'End Sub
    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        'Dim DiscountValue As Decimal = 0D

        'If Not Decimal.TryParse(TextBox6.Text, DiscountValue) Then
        '    DiscountValue = 0D
        'End If

        'If DiscountValue < 0 Then
        '    DiscountValue = 0D
        'End If

        CalculateTotal()
        'For Each row As DataGridViewRow In DataGridView1.Rows
        '    If row.IsNewRow Then Continue For row.Cells("Discount").Value = DiscountValue
        '    'RecalculateRow(row.Index)
        'Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
        '  Create_Invoice.ShowDialog()
    End Sub
    'Private Sub UpdateTotalPrice()
    '    Dim qty As Integer
    '    Dim unitPrice As Decimal

    '    If Integer.TryParse(TextBox9.Text, qty) AndAlso Decimal.TryParse(TextBox5.Text, unitPrice) Then
    '        TextBox4.Text = (qty * unitPrice).ToString("F2")
    '    Else
    '        TextBox4.Text = "0.00"
    '    End If
    'End Sub
    'Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
    '    CalculateRow()
    'End Sub
    'Private Sub DataGridView1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit
    '    Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

    '    If row.Cells("IsInvoiced").Value?.ToString() = "Yes" Then
    '        MessageBox.Show("This sales order has already been invoiced and cannot be edited.",
    '                        "Editing Locked",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Warning)
    '        e.Cancel = True

    '    End If
    'End Sub
    Private Sub LoadSalesOrder()
        Dim dt As New DataTable()

        Dim conn As New OleDbConnection(ConnectionString)
        Dim cmd As New OleDbCommand("SELECT * FROM Sales_Order", conn)
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(dt)

        DataGridView1.DataSource = dt
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing

    End Sub


    Private Function CustomerHasDetails(Customer As String) As Boolean
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim cmd As New OleDbCommand("SELECT COUNT (*) FROM [Customer_Details] WHERE Customer_Name = ?", conn)
            cmd.Parameters.AddWithValue("@P1", Customer)
            Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
        End Using
    End Function
    Private Sub loadSupplierinfo(customer As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT TOP 1 [Contact],[Email],[Address] FROM [Customer_Details] WHERE Customer_Name=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", Supplier)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            TextBox1.Text = reader("Contact").ToString()
                            TextBox3.Text = reader("Email").ToString()
                            TextBox2.Text = reader("Address").ToString()
                        End If
                    End Using
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to Load customer Details!" & ex.Message)
        End Try
    End Sub
    Private Sub LoadProductsToComboBox()
        ComboBox1.Items.Clear()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT [Product_Name], [Unit_Price] FROM [Product_Details] ORDER BY [Product_Name]"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox1.Items.Add(reader("Product_Name").ToString())
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            If ComboBox1.Items.Count > 0 AndAlso ComboBox1.SelectedIndex = -1 Then
                ComboBox1.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedProduct As String = ComboBox1.Text.Trim()
        If selectedProduct = "" Then Exit Sub

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String =
                    "SELECT Unit_Price FROM Product_Details WHERE Product_Name = ?"

                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = selectedProduct

                    Dim result = cmd.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        TextBox5.Text = CDec(result).ToString("F2")
                    Else
                        TextBox5.Text = "0.00"
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading unit price: " & ex.Message)
        End Try
    End Sub
    Dim taxRate As Decimal = 0.15D
    'Private Sub CalculateOrderTotals()
    '    Dim Subtotal As Decimal = 0
    '    Dim discount As Decimal = 0
    '    Dim tax As Decimal = 0
    '    Dim total As Decimal = 0

    '    Decimal.TryParse(TextBox5.Text, Subtotal)
    '    Decimal.TryParse(TextBox6.Text, discount)

    '    Dim afterDiscount As Decimal = Subtotal - discount

    '    tax = afterDiscount * taxRate

    '    TextBox8.Text = tax.ToString("0.00")
    '    total = afterDiscount + tax
    '    TextBox4.Text = total.ToString("0.00")


    'End Sub

    Private Sub CalculateTotal()
        Dim qty As Integer = 0
        Dim discount As Decimal = 0D
        Dim subtotal As Decimal = 0D
        Dim Price As Decimal = 0D
        Dim vat As Decimal = 0D


        Integer.TryParse(TextBox9.Text, qty)
        Decimal.TryParse(TextBox6.Text, discount)
        Decimal.TryParse(TextBox5.Text, Price)

        subtotal = (qty * Price) - discount
        If subtotal < 0 Then subtotal = 0
        vat = subtotal * 0.15

        TextBox4.Text = subtotal.ToString("F2")
        TextBox8.Text = vat.ToString("F2")
        If TextBox9.Text = "" Then
            ' TextBox9.Text = 1
        End If

    End Sub
    Public Sub ReloadCustomersCombo()
        ComboBox2.Items.Clear()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT DISTINCT Customer_Name FROM Customer_Details ORDER BY Customer_Name"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox2.Items.Add(reader("Customer_Name").ToString())
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            ' Optional: select first customer
            If ComboBox2.Items.Count > 0 AndAlso ComboBox2.SelectedIndex = -1 Then
                ComboBox2.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading customer from database: " & ex.Message)
        End Try
    End Sub
    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        ' UpdateTotalPrice()
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        'If TextBox9.Text <> "" Then
        '    Dim qty As Integer = Convert.ToInt32(TextBox9.Text)
        '    Dim subtotal As Decimal = qty * Unit_Price

        '    TextBox5.Text = subtotal.ToString("0.00")
        '    CalculateOrderTotals()
        'End If
        CalculateTotal()
    End Sub
    'Private Sub dataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged

    '    'btnEdit.Enabled = True
    '    'btnSave.Enabled = True


    '    If DataGridView1.CurrentRow Is Nothing Then Return

    '    ' Assuming you have columns named exactly as in your table or you can use index
    '    TextBox6.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Discount").Value)
    '    ComboBox1.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Product").Value)
    '    TextBox9.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Quantity").Value)
    '    'txtTransactionNumber.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Transaction_Number").Value) ' key
    '    TextBox5.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Amount").Value)
    '    TextBox4.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Total").Value)
    '    TextBox8.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("VAT").Value)
    '    'If dgvRecords.CurrentRow.Cells("Sale_Date").Value IsNot DBNull.Value Then
    '    '    dtpSaleDate.Value = Convert.ToDateTime(dgvRecords.CurrentRow.Cells("Sale_Date").Value)
    '    'End If
    'End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 AndAlso
                DataGridView1.Rows(e.RowIndex).Cells(0).Value IsNot Nothing Then

            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            ComboBox1.Text = row.Cells(1).Value?.ToString()
            TextBox6.Text = row.Cells(3).Value?.ToString()
            TextBox5.Text = row.Cells(4).Value?.ToString()
            TextBox8.Text = row.Cells(5).Value?.ToString()
            TextBox4.Text = row.Cells(6).Value?.ToString()
            TextBox9.Text = row.Cells(2).Value?.ToString()
        End If
    End Sub
End Class