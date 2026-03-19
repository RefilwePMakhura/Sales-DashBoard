Imports System.Data.OleDb

Public Class Order_Form

    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Private ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"
    Private ProductPrice As Decimal = 0D

    Private Sub Order_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  SetupDataGridView()
        LoadData()
        ReloadCustomersCombo()
        LoadSuppliers()
        LoadProductsToComboBox()
        LoadCompanyInfo()
        CalculateTotal()
    End Sub

    Private Sub SetupDataGridView()
        DataGridView1.Columns.Clear()
        DataGridView1.AutoGenerateColumns = False
        DataGridView1.AllowUserToAddRows = False

        Dim colPOID As New DataGridViewTextBoxColumn With {
            .Name = "PO_ID",
            .HeaderText = "PO_ID",
            .DataPropertyName = "PO_ID"
        }
        DataGridView1.Columns.Add(colPOID)

        Dim colSupplier As New DataGridViewTextBoxColumn With {
            .Name = "Supplier",
            .HeaderText = "Supplier",
            .DataPropertyName = "Supplier"
        }
        DataGridView1.Columns.Add(colSupplier)

        Dim colDate As New DataGridViewTextBoxColumn With {
            .Name = "PO_Date",
            .HeaderText = "PO Date",
            .DataPropertyName = "PO_Date"
        }
        DataGridView1.Columns.Add(colDate)

        Dim colCustomer As New DataGridViewTextBoxColumn With {
            .Name = "Customer",
            .HeaderText = "Customer",
            .DataPropertyName = "Customer"
        }
        DataGridView1.Columns.Add(colCustomer)

        Dim colProduct As New DataGridViewTextBoxColumn With {
            .Name = "Product",
            .HeaderText = "Product",
            .DataPropertyName = "Product"
        }
        DataGridView1.Columns.Add(colProduct)

        Dim colQty As New DataGridViewTextBoxColumn With {
            .Name = "Quantity",
            .HeaderText = "Quantity",
            .DataPropertyName = "Quantity"
        }
        DataGridView1.Columns.Add(colQty)

        Dim colUnitPrice As New DataGridViewTextBoxColumn With {
            .Name = "Unit_Price",
            .HeaderText = "Unit Price",
            .DataPropertyName = "Unit_Price"
        }
        DataGridView1.Columns.Add(colUnitPrice)

        Dim colDiscount As New DataGridViewTextBoxColumn With {
            .Name = "Discount",
            .HeaderText = "Discount (%)",
            .DataPropertyName = "Discount"
        }
        DataGridView1.Columns.Add(colDiscount)

        Dim colVAT As New DataGridViewTextBoxColumn With {
            .Name = "VAT",
            .HeaderText = "VAT (15%)",
            .DataPropertyName = "VAT"
        }
        DataGridView1.Columns.Add(colVAT)

        Dim colTotal As New DataGridViewTextBoxColumn With {
            .Name = "Total",
            .HeaderText = "Total",
            .DataPropertyName = "Total"
        }
        DataGridView1.Columns.Add(colTotal)


    End Sub

    Private Sub LoadProductsToComboBox()
        ComboBox4.Items.Clear()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT [Product_Name] FROM [Product_Details] ORDER BY [Product_Name]"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox4.Items.Add(reader("Product_Name").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadData()

        Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand("SELECT * FROM [Order]", conn)
            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(dt)

        End Using

        DataGridView1.DataSource = dt
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing

    End Sub

    Private Sub LoadSuppliers()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT [SupplierName] FROM [Supplier] ORDER BY [SupplierName]", conn)
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

    Public Sub ReloadCustomersCombo()
        ComboBox2.Items.Clear()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT DISTINCT [Customer_Name] FROM [Customer_Details] ORDER BY [Customer_Name]"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox2.Items.Add(reader("Customer_Name").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading customers: " & ex.Message)
        End Try
    End Sub

    Private Function GetRow(tableName As String, fieldName As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * FROM {tableName} WHERE {fieldName} = ?", conn)
        cmd.Parameters.AddWithValue("?", value)
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = -1 Then Exit Sub

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT [Contact_Person], [PhoneNumber], [Physical_Address], [EmailAddress] FROM [Supplier] WHERE [SupplierName] = ?", conn)
                cmd.Parameters.AddWithValue("?", ComboBox1.Text)

                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        TextBox3.Text = dr("Contact_Person").ToString()
                        TextBox1.Text = dr("PhoneNumber").ToString()
                        TextBox4.Text = dr("Physical_Address").ToString()
                        TextBox6.Text = dr("EmailAddress").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load supplier details: " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = -1 Then Exit Sub
        LoadCustomerInfo(ComboBox2.Text)
    End Sub

    Private Sub LoadCustomerInfo(customerName As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT TOP 1 [Address] FROM [Customer_Details] WHERE [Customer_Name] = ?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", customerName)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            TextBox2.Text = reader("Address").ToString()
                        Else
                            TextBox2.Clear()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load customer details: " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox4.SelectedIndex = -1 Then Exit Sub

        Try
            Using r = GetRow("[Product_Details]", "[Product_Name]", ComboBox4.Text)
                If r.Read() Then
                    ProductPrice = Convert.ToDecimal(r("Unit_Price"))
                    TextBox7.Text = ProductPrice.ToString("0.00")
                End If
            End Using

            CalculateTotal()

        Catch ex As Exception
            MessageBox.Show("Failed to load product price: " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        CalculateTotal()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        CalculateTotal()
    End Sub

    Private Sub CalculateTotal()
        Dim qty As Decimal = 0D
        Dim discount As Decimal = 0D

        Decimal.TryParse(TextBox11.Text, qty)
        Decimal.TryParse(TextBox5.Text, discount)

        Dim subtotal As Decimal = qty * ProductPrice
        subtotal -= subtotal * (discount / 100D)

        Dim totalTax As Decimal = subtotal * 0.15D
        Dim total As Decimal = subtotal + totalTax

        TextBox9.Text = totalTax.ToString("0.00")
        TextBox10.Text = total.ToString("0.00")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ComboBox1.SelectedIndex < 0 Then
            MessageBox.Show("Please select supplier.")
            Exit Sub
        End If

        If ComboBox2.SelectedIndex < 0 Then
            MessageBox.Show("Please select customer.")
            Exit Sub
        End If

        If ComboBox4.SelectedIndex < 0 Then
            MessageBox.Show("Please select product.")
            Exit Sub
        End If

        Dim supplier As String = ComboBox1.Text
        Dim customer As String = ComboBox2.Text
        Dim product As String = ComboBox4.Text
        Dim poDate As Date = DateTimePicker1.Value
        Dim poID As String = "PO" & DateTime.Now.ToString("yyyyMMddHHmmss")

        Dim qty As Decimal = 0D
        Dim discount As Decimal = 0D
        Dim vat As Decimal = 0D
        Dim total As Decimal = 0D

        Decimal.TryParse(TextBox11.Text, qty)
        Decimal.TryParse(TextBox5.Text, discount)
        Decimal.TryParse(TextBox9.Text, vat)
        Decimal.TryParse(TextBox10.Text, total)

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String =
                    "INSERT INTO [Order] " &
                    "([PO_ID], [Product], [Quantity], [Amount], [Delivery], [VAT], [Total]) " &
                    "VALUES (?,?,?,?,?,?,?)"

                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", poID)
                    cmd.Parameters.AddWithValue("?", product)
                    cmd.Parameters.AddWithValue("?", qty)
                    cmd.Parameters.AddWithValue("?", ProductPrice)
                    cmd.Parameters.AddWithValue("?", discount)
                    cmd.Parameters.AddWithValue("?", vat)
                    cmd.Parameters.AddWithValue("?", total)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Order saved successfully.")
            LoadData()

        Catch ex As Exception
            MessageBox.Show("Failed to save: " & ex.Message)
        End Try
    End Sub

    Public Sub ColorGrid()
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.IsNewRow Then Continue For

            If row.Cells("Status").Value IsNot Nothing Then
                Dim statusValue As String = row.Cells("Status").Value.ToString()

                If statusValue = "Paid" Then
                    row.DefaultCellStyle.BackColor = Color.LightBlue
                ElseIf statusValue = "Pending" Then
                    row.DefaultCellStyle.BackColor = Color.LightPink
                End If
            End If
        Next
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count = 0 Then Return

        Try
            Dim selectedRow = DataGridView1.SelectedRows(0)

            '    ComboBox1.Text = If(selectedRow.Cells("Supplier").Value IsNot Nothing, selectedRow.Cells("Supplier").Value.ToString(), "")
            '  ComboBox2.Text = If(selectedRow.Cells("Customer").Value IsNot Nothing, selectedRow.Cells("Customer").Value.ToString(), "")
            ComboBox4.Text = If(selectedRow.Cells("Product").Value IsNot Nothing, selectedRow.Cells("Product").Value.ToString(), "")
            TextBox11.Text = If(selectedRow.Cells("Quantity").Value IsNot Nothing, selectedRow.Cells("Quantity").Value.ToString(), "")
            TextBox7.Text = If(selectedRow.Cells("Amount").Value IsNot Nothing, selectedRow.Cells("Amount").Value.ToString(), "")
            TextBox5.Text = If(selectedRow.Cells("Delivery").Value IsNot Nothing, selectedRow.Cells("Delivery").Value.ToString(), "")
            TextBox9.Text = If(selectedRow.Cells("VAT").Value IsNot Nothing, selectedRow.Cells("VAT").Value.ToString(), "")
            TextBox10.Text = If(selectedRow.Cells("Total").Value IsNot Nothing, selectedRow.Cells("Total").Value.ToString(), "")

        Catch ex As Exception
            MessageBox.Show("Error selecting row: " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub

        Try
            Dim pay As New Payable()

            pay.SelectedPO_ID = DataGridView1.Rows(e.RowIndex).Cells("PO_ID").Value.ToString()
            '   pay.SelectedSupplierID = DataGridView1.Rows(e.RowIndex).Cells("SupplierID").Value.ToString()
            pay.SelectedCustomer = ComboBox1.Text.ToString()
            pay.SelectedTax = DataGridView1.Rows(e.RowIndex).Cells("VAT").Value.ToString()
            pay.SelectedQty = CInt(DataGridView1.Rows(e.RowIndex).Cells("Quantity").Value)
            pay.InvoiceAmount = CDec(DataGridView1.Rows(e.RowIndex).Cells("Total").Value)

            ' only use this if Product_ID is in your grid or table
            ' pay.SelectedProductID = DataGridView1.Rows(e.RowIndex).Cells("Product_ID").Value.ToString()

            pay.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error opening payment form: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadCompanyInfo()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim cmd As New OleDbCommand("SELECT TOP 1 * FROM [CompanySettings]", conn)
                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        Label12.Text = dr("CompanyName").ToString()
                        Label13.Text = dr("Address").ToString()
                        Label14.Text = dr("Phone").ToString()
                        Label15.Text = dr("Email").ToString()

                        Dim logoPath As String = dr("LogoPath").ToString()
                        If IO.File.Exists(logoPath) Then
                            PictureBox1.Image = Image.FromFile(logoPath)
                        End If
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Failed to load company info: " & ex.Message)
        End Try
    End Sub

End Class