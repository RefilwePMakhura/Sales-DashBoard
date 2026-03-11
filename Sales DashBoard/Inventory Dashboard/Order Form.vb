Imports System.Data.OleDb

Public Class Order_Form
    Dim Subtotal As Decimal = 0D

    Private Function GetProduct() As DataTable
        Dim dt As New DataTable
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "SELECT Product_Name FROM [Product_Details] GROUP BY Product_Name"
            Using da As New OleDbDataAdapter(sql, conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Private Sub SetUpOrderGrid()
        DataGridView1.Columns.Clear()
        DataGridView1.AutoGenerateColumns = False

        Dim colProduct As New DataGridViewComboBoxColumn With {
          .Name = "Product",
       .HeaderText = "Product",
       .DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox}

        DataGridView1.Columns.Add(colProduct)

        DataGridView1.Columns.Add("Delivery", "Delivery")
        DataGridView1.Columns.Add("Quantity", "Quantity")
        DataGridView1.Columns.Add("Amount", "Amount")
        DataGridView1.Columns.Add("VAT", "VAT")
        DataGridView1.Columns.Add("LineTotal", "LineTotal")
        DataGridView1.Columns.Add("Total", "Total")
    End Sub
    Public Sub LoadOrderData()

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

    Private Sub frmOrderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox3.Items.AddRange(New String() {"Pickup", "Supplier Delivery", "Courier"})
        ComboBox3.SelectedIndex = 0
        LoadCompanyInfo()
        SetUpOrderGrid()
        LoadSuppliers()
        ClearForm()
        ReloadCustomersCombo()
        UpdateStockAfterPurchase()
        ColorGrid()
        LoadProductToCombBox()
    End Sub


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

    Private Sub LoadSuppliers()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT SupplierName FROM Supplier ORDER BY SupplierName", conn)
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    ComboBox1.Items.Clear()
                    While reader.Read()
                        ComboBox1.Items.Add(reader("SupplierName").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load suppliers: " & ex.Message)
        End Try

    End Sub
    Private Sub ReloadCustomersCombo()
        'ComboBox2.Items.Clear()
        'ClearForm()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT DISTINCT Customer_Name FROM Customer_Details ORDER BY Customer_Name"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        ComboBox2.Items.Clear()
                        While reader.Read()
                            ComboBox2.Items.Add(reader("Customer_Name").ToString())
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            ' Optional: select first customer
            'If ComboBox2.Items.Count > 0 AndAlso ComboBox2.SelectedIndex = -1 Then
            '    ComboBox2.SelectedIndex = 0
            'End If

        Catch ex As Exception
            MessageBox.Show("Error loading customer from database: " & ex.Message)
        End Try
    End Sub
    Private Sub loadSupplierinfo(supplier As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT TOP 1 [Contact_Person],[EmailAddress],[Physical_Address] FROM [Supplier] WHERE SupplierName=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", supplier)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            TextBox3.Text = reader("Contact_Person").ToString()
                            TextBox6.Text = reader("EmailAddress").ToString()
                            TextBox4.Text = reader("Physical_Address").ToString()
                        End If
                    End Using
                    conn.Close()
                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadProductToCombBox()
        ComboBox4.Items.Clear()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT [Product_Name], [Unit_Price] FROM [Product_Details] ORDER BY [Product_Name]"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox4.Items.Add(reader("Product_Name").ToString())
                            TextBox7.Text = Subtotal
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            'If ComboBox4.Items.Count > 0 AndAlso ComboBox1.SelectedIndex = -1 Then
            '    ComboBox4.SelectedIndex = 0
            'End If

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub
    Private Sub ClearForm()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox6.Clear()
        TextBox2.Clear()
    End Sub
    Private Sub UpdateStockAfterPurchase()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.IsNewRow Then Continue For

                Dim ProductName As String = row.Cells("Product").Value.ToString()
                Dim QuantityPurchased As Integer = Convert.ToInt32(row.Cells("Quantity").Value)

                Using cmd As New OleDbCommand(
                "UPDATE Product_Details
                SET Current_Stock = Current_Stock + ?
               WHERE Product_Name = ?", conn)

                    cmd.Parameters.Add("@Current_Stock", OleDbType.Integer).Value = QuantityPurchased
                    cmd.Parameters.Add("@Product", OleDbType.VarWChar).Value = ProductName

                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub
    Private Sub dgvOrderRecords_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        Dim pay As New Payable

        pay.selectedsupplier = DataGridView1.Rows(e.RowIndex).Cells("POID").Value.ToString()
        pay.selectedCustomer = DataGridView1.Rows(e.RowIndex).Cells("Supplier").Value.ToString()
        pay.selectedVAT = DataGridView1.Rows(e.RowIndex).Cells("VAT").Value.ToString()
        pay.selectedQty = DataGridView1.Rows(e.RowIndex).Cells("Quantity").Value.ToString()
        pay.selectedDate = DataGridView1.Rows(e.RowIndex).Cells("PODate").Value.ToString()
        pay.InvoiceAmount = DataGridView1.Rows(e.RowIndex).Cells("Total").Value.ToString()
        pay.ShowDialog()
    End Sub
    Private Function CalculateDelivery(Subtotal As Decimal) As Decimal
        If Subtotal > 3000D Then
            Return 150D
        Else
            Return 0D
        End If
    End Function
    Private Sub CalculateRow(row As DataGridViewRow)
        If row Is Nothing OrElse row.IsNewRow Then Exit Sub

        Dim qty As Integer = 0
        Dim unitPrice As Decimal = 0D
        Dim discountPercent As Decimal = 0D

        Integer.TryParse(row.Cells("Quantity").Value?.ToString(), qty)
        Decimal.TryParse(row.Cells("Amount").Value?.ToString(), unitPrice)
        Decimal.TryParse(row.Cells("Delivery").Value?.ToString(), discountPercent)

        Dim gross As Decimal = qty * unitPrice
        ' Dim discountAmount As Decimal = gross + discountPercent
        Dim subtotal As Decimal = gross
        Dim vat As Decimal = subtotal * 0.15D
        Dim totalWithVAT As Decimal = subtotal + vat - discountPercent

        row.Cells("LineTotal").Value = Math.Round(subtotal, 2)
        row.Cells("VAT").Value = Math.Round(vat, 2)
        row.Cells("Total").Value = Math.Round(totalWithVAT, 2)

        TextBox7.Text = subtotal.ToString("N2")
        TextBox5.Text = discountPercent.ToString()
        TextBox9.Text = vat.ToString("N2")
        TextBox10.Text = totalWithVAT.ToString("N2")

    End Sub

    Private Sub dgvOrderRecords_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        If e.RowIndex > 0 Then
            Exit Sub
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            If e.ColumnIndex = DataGridView1.Columns("Product").Index Then

                Dim ProductName As String = row.Cells("Product").Value?.ToString()
                If String.IsNullOrEmpty(ProductName) Then Exit Sub

                Dim dtProduct As DataTable = TryCast(DataGridView1.Tag, DataTable)
                If dtProduct IsNot Nothing Then
                    Dim Found() As DataRow = dtProduct.Select("Product_Name =" & ProductName.Replace("'", "'") & "'")
                    If Found.Length > 0 Then
                        row.Cells("Subtotal").Value = Convert.ToDecimal(Found(0)("Subtotal"))
                    End If
                End If
            End If

            Dim qty As Decimal = 0, UnitPrice As Decimal = 0, Discount As Decimal = 0
            Decimal.TryParse(row.Cells("Quantity").Value?.ToString(), qty)
            Decimal.TryParse(row.Cells("Subtotal").Value?.ToString(), Subtotal)
            Decimal.TryParse(row.Cells("Discount").Value?.ToString(), Discount)

            Dim Linetotal As Decimal = qty * Subtotal
            Dim DiscountAmount As Decimal = Linetotal * (Discount / 100D)
            Dim Amount As Decimal = Linetotal - DiscountAmount
            Dim VAT As Decimal = Amount * 0.15D
            Dim Total As Decimal = Amount + VAT

            row.Cells("LineTotal").Value = Math.Round(Subtotal, 2)
            row.Cells("VAT").Value = Math.Round(VAT, 2)
            row.Cells("Total").Value = Math.Round(Total, 2)

        End If
    End Sub

    Private Sub dgvOrderRecords_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If e.RowIndex < 0 Then Exit Sub

        If DataGridView1.Columns(e.ColumnIndex).Name <> "Product" Then Exit Sub
        Dim row = DataGridView1.Rows(e.RowIndex)
        Dim ProductName = row.Cells("Product").Value?.ToString()
        If String.IsNullOrEmpty(ProductName) Then Exit Sub

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Using cmd As New OleDbCommand(" SELECT  Unit_Price FROM [Product_Details] WHERE Product_Name = ?", conn)
                cmd.Parameters.AddWithValue("@p1", ProductName)
                Dim Price = cmd.ExecuteScalar()
                If Price IsNot Nothing Then
                    row.Cells("Amount").Value = Convert.ToDecimal(Price)
                    row.Cells("Quantity").Value = 1
                    CalculateRow(row)
                    'CalculateGrandTotal()
                End If
            End Using
        End Using

    End Sub
    Private Sub UpdateTotals()

        Dim subtotal As Decimal = 0D
        Dim vatTotal As Decimal = 0D
        Dim grandTotal As Decimal = 0D

        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For

            Dim lineSubtotal As Decimal = 0D
            Dim lineVAT As Decimal = 0D
            Dim lineTotal As Decimal = 0D

            Decimal.TryParse(Convert.ToString(row.Cells("LineTotal").Value), lineSubtotal)
            Decimal.TryParse(Convert.ToString(row.Cells("VAT").Value), lineVAT)
            Decimal.TryParse(Convert.ToString(row.Cells("Total").Value), lineTotal)

            subtotal += lineSubtotal
            vatTotal += lineVAT
            grandTotal += lineTotal
        Next

        TextBox7.Text = subtotal.ToString("N2")
        TextBox9.Text = vatTotal.ToString("N2")
        TextBox10.Text = grandTotal.ToString("N2")

    End Sub
    Public Sub ColorGrid()
        For Each row As DataGridViewRow In DataGridView1.Rows

            If row.IsNewRow Then Continue For
            If row.Cells("Status").Value IsNot Nothing Then
                Dim StatusValue As String = row.Cells("Status").Value.ToString
                If StatusValue = "Paid" Then
                    row.DefaultCellStyle.BackColor = Color.LightGreen
                ElseIf StatusValue = "Pending" Then
                    row.DefaultCellStyle.BackColor = Color.LightPink
                End If
            End If
        Next
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Function GetRow(table As String, field As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(Module1.ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * from {table} WHERE {field} = ?", conn)
        cmd.Parameters.AddWithValue("?", OleDbType.VarChar).Value = value
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Using r = GetRow(" Supplier", "SupplierName", ComboBox1.Text)
            If r.Read() Then
                TextBox3.Text = r("Contact_Person").ToString()
                TextBox6.Text = r("EmailAddress").ToString
                TextBox4.Text = r("Physical_Address").ToString
            End If

        End Using
    End Sub
    Private Function GetRows(table As String, field As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(Module1.ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * from {table} WHERE {field} = ?", conn)
        cmd.Parameters.AddWithValue("?", OleDbType.VarChar).Value = value
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If ComboBox1.SelectedIndex < 0 Then
            MessageBox.Show("Please select a supplier.")
            Exit Sub
        End If

        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Please add at least one product.")
            Exit Sub
        End If

        Dim poID As String = "PO" & DateTime.Now.ToString("yyyyMMddHHmmss")

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                For Each row As DataGridViewRow In DataGridView1.Rows
                    If row.IsNewRow Then Continue For

                    Using cmd As New OleDbCommand("
                    INSERT INTO [Order] 
                    ([PO_ID],[Product],[Delivery],[Quantity],[Amount],[LineTotal],[VAT],[Total],[Status]) 
                    VALUES (?,?,?,?,?,?,?,?,?)", conn)

                        cmd.Parameters.AddWithValue("?", poID)
                        cmd.Parameters.AddWithValue("?", row.Cells("Product").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("Delivery").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("Quantity").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("Amount").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("LineTotal").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("VAT").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("Total").Value)
                        cmd.Parameters.AddWithValue("?", row.Cells("Status").Value)

                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using

            MessageBox.Show("Purchase order saved successfully!")

            UpdateStockAfterPurchase()

            '    DataGridView1.Rows.Clear()
            ComboBox1.SelectedIndex = -1
            UpdateTotals()
            Payable.ShowDialog()
            LoadOrderData()

        Catch ex As Exception
            MessageBox.Show("Failed to save: " & ex.Message)
        End Try

    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Using r = GetRows("Customer_Details", "Customer_Name", ComboBox2.Text)
            If r.Read() Then
                TextBox2.Text = r("Address").ToString()

            End If

        End Using


    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        UpdateTotals()

        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For row.Cells("Delivery").Value = CDec(TextBox5.Text)
            RecalculateRow(row.Index)
        Next
    End Sub
    Private Sub RecalculateRow(rowIndex As Integer)

        If rowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DataGridView1.Rows(rowIndex)
        CalculateRow(row)
        UpdateTotals()

    End Sub
End Class