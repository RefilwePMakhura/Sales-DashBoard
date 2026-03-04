Imports System.IO
Imports System.Data.OleDb

Public Class Add_Sales
    Private Sub ClearForm()
        txtQuantity.Clear()
        txtUnitPrice.Clear()

    End Sub
    Public Sub ReloadCustomersCombo()
        cmbCustomer.Items.Clear()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT DISTINCT Customer_Name FROM Customer_Details ORDER BY Customer_Name"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            cmbCustomer.Items.Add(reader("Customer_Name").ToString())
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            ' Optional: select first customer
            If cmbCustomer.Items.Count > 0 AndAlso cmbCustomer.SelectedIndex = -1 Then
                cmbCustomer.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading customer from database: " & ex.Message)
        End Try
    End Sub
    Private Sub PopulateFormControls()

        cmbCustomer.Text = Customer
        cmbProduct.Text = Product
        txtQuantity.Text = Quantity.ToString()
        txtUnitPrice.Text = UnitPrice.ToString()


    End Sub

    Private Sub AddSalesToGrid()
        ' Collect values from form controls
        Dim Customer As String = cmbCustomer.Text
        Dim Product As String = cmbProduct.Text
        Dim Quantity As Integer
        Dim Unit_Price As Decimal = txtUnitPrice.Text
        Dim totalprice As Decimal = TextBox1.Text
        Dim Sale_Date As Date = dtpSaleDate.Value
        Dim Transaction_Number As String = txtTransactionNumber.Text
        ' Check for valid inputs
        If Not Integer.TryParse(txtQuantity.Text, Quantity) Then
            MessageBox.Show("Please enter a valid quantity.")
            Exit Sub
        End If

        'If Not Decimal.TryParse(txtUnitPrice.Text, unitPrice) Then
        '    MessageBox.Show("Please enter a valid unit price.")
        '    Exit Sub
        'End If
        UnitPrice = Quantity * UnitPrice
        ' Calculate total amount
        Dim amount As Decimal = UnitPrice

        ' Add row to DataGridView
        dgvRecords.Rows.Add(Customer, Product, Quantity, amount, totalprice, "New", Sale_Date.ToShortDateString(), Transaction_Number)
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



    Dim Customer As String
    ' Dim CustomerPromt As String = "Enter customer name."
    Dim Transaction_Number As String
    Dim Product As String
    Dim ProductPromt As String = "Enter product name."

    Dim Quantity As String
    Dim QuantityPromt As String = "Enter quantity. "

    Dim UnitPrice As Integer
    Dim UnitPrompt As String = "Enter the unit price for product."


    Private Function CalculateUnitPrice() As Double
        Return (Quantity * UnitPrice)
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub frmSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReloadCustomersCombo()
        LoadData()
        ClearForm()
        LoadProductsToComboBox()
        btnEdit.Enabled = False
        btnSave.Enabled = True
        LoadSales()
        PromptSalesAfterWonStage(txtTransactionNumber.Text.Trim())

        'SetupDataGridView()
        ' LoadSalesData()

        'Esisting stage setup
        cboStage.Items.Clear()

        cboStage.Items.AddRange(New Object() {"New", "Qualified", "Proposition", "Won"})
        If cboStage.Items.Count > 0 Then cboStage.SelectedIndex = 0

        'NEW: load customers from file



        'Default date

        dtpSaleDate.Value = Date.Today


    End Sub




    Private Sub dataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRecords.SelectionChanged

        'btnEdit.Enabled = True
        'btnSave.Enabled = True


        'If dgvRecords.CurrentRow Is Nothing Then Return

        '' Assuming you have columns named exactly as in your table or you can use index
        'cmbCustomer.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Customer").Value)
        'cmbProduct.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Product").Value)
        'txtQuantity.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Quantity").Value)
        'txtTransactionNumber.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Transaction_Number").Value) ' key
        'txtUnitPrice.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Unit_Price").Value)
        'TextBox1.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Total_Price").Value)
        'cboStage.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Stage").Value)
        'If dgvRecords.CurrentRow.Cells("Sale_Date").Value IsNot DBNull.Value Then
        '    dtpSaleDate.Value = Convert.ToDateTime(dgvRecords.CurrentRow.Cells("Sale_Date").Value)
        'End If
    End Sub

    ' -------------------------------
    ' LOAD PRODUCTS FROM DATABASE
    ' -------------------------------
    Private Sub LoadProductsToComboBox()
        cmbProduct.Items.Clear()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT [Product_Name], [Unit_Price] FROM [Product_Details] ORDER BY [Product_Name]"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            cmbProduct.Items.Add(reader("Product_Name").ToString())
                        End While
                    End Using
                End Using
                conn.Close()
            End Using

            If cmbProduct.Items.Count > 0 AndAlso cmbProduct.SelectedIndex = -1 Then
                cmbProduct.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message)
        End Try
    End Sub
    Private Function GetRows(table As String, field As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(Module1.ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * from {table} WHERE {field} = ?", conn)
        cmd.Parameters.AddWithValue("?", OleDbType.VarChar).Value = value
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)

    End Function
    Private Sub PromptSalesAfterWonStage(InvoiceNumber As String)
        If cboStage.Text.Trim().ToLower = "won" Then
            Dim Results As DialogResult = MessageBox.Show($"Sale has been sucessfully saved under Stage: Won." & vbCrLf & $"Do you want to Create an sales Order for transation no:{Transaction_Number}?", "Create Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If Results = DialogResult.Yes Then

                Dim Sales_Order As New Sales_Order
                Sales_Order.Product = cmbProduct.Text
                Sales_Order.Quantity = txtQuantity.Text
                Sales_Order.Total_Amount = TextBox1.Text
                Sales_Order.Customer = cmbCustomer.Text

                      Sales_Order.ShowDialog()
            End If
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try
            Dim TransactionNumber As String = Module1.GenerateTransactionNumber
            txtTransactionNumber.Text = TransactionNumber
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()



                Using cmd As New OleDbCommand("INSERT INTO [Sales_Details] ([Customer], [Product], [Quantity], [Transaction_Number], [Unit_Price],[Total_Price], [Sale_Date], [Stage]) VALUES (?,?,?,?,?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("@Customer", cmbCustomer.Text)
                    cmd.Parameters.AddWithValue("@Product", cmbProduct.Text)
                    cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text)
                    cmd.Parameters.AddWithValue("@Transaction_Number", txtTransactionNumber.Text)
                    cmd.Parameters.AddWithValue("@Unit_Price", txtUnitPrice.Text)
                    cmd.Parameters.AddWithValue("@Total_Price", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@Sale_Date", dtpSaleDate.Text)
                    cmd.Parameters.AddWithValue("@Stage", cboStage.Text)
                    cmd.ExecuteNonQuery()
                    'If affected > 0 Then
                    '    MessageBox.Show("Record Saved successfully!")
                    '    If cboStage.Text = "Won" Then
                    '        MessageBox.Show("Customer has complete all the stage!")
                    '        MessageBox.Show("Do you want to procced to Sale_Order form" & MessageBoxButtons.YesNo)
                    '        Dim Add_Sales As New Sales_Order
                    '        Sales_Order.ShowDialog()
                    '    End If
                    ' End If
                End Using
                conn.Close()

                Dim dash = TryCast(Me.Owner, Form1)
                If dash IsNot Nothing Then
                    dash.LoadTotalsFromDatabase() ' This requires making LoadTotalsFromFile public in Form1
                    dash.RefreshLabels()
                End If
            End Using
            ClearForm()
            LoadData()
            AddSalesToGrid()

        Catch ex As Exception
            ' MessageBox.Show("Sale saved to : " & "Saved")

        End Try
        ' Configure DatGridView properties
        dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRecords.ReadOnly = True
        dgvRecords.AllowUserToAddRows = False
        dgvRecords.MultiSelect = False
        dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill

    End Sub

    Private Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Sales_Details]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                dgvRecords.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub



    Private Sub btnEditCustomer_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            If dgvRecords.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a sale to edit")
                Exit Sub
            End If
            Dim transactionNumber As String = dgvRecords.SelectedRows(0).Cells("Transaction_Number").Value.ToString



            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "UPDATE [Sales_Details] SET [Stage]=? WHERE [Transaction_Number]=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.Add("@p1", OleDbType.VarWChar).Value = cboStage.Text
                    cmd.Parameters.Add("@p2", OleDbType.VarWChar).Value = transactionNumber

                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()
                '   txtQuantity.ReadOnly = True
            End Using
            LoadData()
            Dim dash = TryCast(Me.Owner, Form1)
            If dash IsNot Nothing Then
                dash.LoadTotalsFromDatabase()
                dash.RefreshLabels()
            End If

            Dim stage As String = cboStage.Text.Trim()
            stage = stage.Substring(0, 1).ToUpper() & stage.Substring(1).ToLower()
            stage = stage

            MessageBox.Show("Sale Updated Successfully")

        Catch ex As Exception
            MessageBox.Show("Update Failed:" & ex.Message)
        End Try

        PromptSalesAfterWonStage(txtTransactionNumber.Text.Trim())

    End Sub

    Private Sub dgvRecords_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRecords.CellClick

        btnEdit.Enabled = True
        btnSave.Enabled = False
        btnClear.Enabled = True



        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvRecords.Rows(e.RowIndex)

            txtQuantity.Text = row.Cells("Quantity").Value.ToString()
            txtUnitPrice.Text = row.Cells("Unit_Price").Value.ToString()
            cmbCustomer.Text = row.Cells("Customer").Value.ToString()
            txtTransactionNumber.Text = row.Cells("Transaction_Number").Value.ToString()
            cboStage.Text = row.Cells("Stage").Value.ToString

        End If


        '' Check if a row is selected
        'If dgvRecords.SelectedRows.Count > 0 Then
        '    Dim selectedRow = dgvRecords.SelectedRows(0)

        '    ' Get values from selected row
        '    Dim Customer As String = selectedRow.Cells("Customer").Value.ToString()
        '    Dim Product As String = selectedRow.Cells("Product").Value.ToString()
        '    Dim Quantity As Integer = selectedRow.Cells("Quantity").Value.ToString()
        '    Dim Transaction_Number As String = selectedRow.Cells("Transaction_Number").Value.ToString()
        '    Dim Unit_Price As String = selectedRow.Cells("Unit_Price").Value.ToString()
        '    Dim Total_Price As String = selectedRow.Cells("Total_Price").Value.ToString()
        '    Dim Stage As String = selectedRow.Cells("Stage").Value.ToString()
        '    Dim Sale_Date As String = selectedRow.Cells("Sale_Date").Value.ToString()
        '    ' Remove currency symbol and format from amount if present
        '    Dim amountDecimal As Decimal = 0
        '    If Decimal.TryParse(Unit_Price.Replace("$", "").Replace("R", "").Trim(), amountDecimal) Then
        '        Unit_Price = amountDecimal.ToString()
        '    End If

        '    ' Populate controls with selected row data
        '    cmbCustomer.Text = Customer
        '    cmbProduct.Text = Product
        '    txtUnitPrice.Text = Unit_Price
        '    TextBox1.Text = Total_Price
        '    txtQuantity.Text = Quantity
        '    txtTransactionNumber.Text = Transaction_Number
        '    ' Set stage in combo box
        '    For i As Integer = 0 To cboStage.Items.Count - 1
        '        If cboStage.Items(i).ToString().Equals(Stage, StringComparison.OrdinalIgnoreCase) Then
        '            cboStage.SelectedIndex = i
        '            Exit For
        '        End If
        '    Next

        '    ' Set date
        '    Try
        '        dtpSaleDate.Value = DateTime.Parse(Unit_Price)
        '    Catch ex As Exception
        '        ' Default to today if date parsing fails
        '        dtpSaleDate.Value = Date.Today
        '    End Try
        'End If

    End Sub

    Private Sub LoadSales()
        Dim dt As New DataTable()

        Dim conn As New OleDbConnection(ConnectionString)
        Dim cmd As New OleDbCommand("SELECT * FROM Sales_Details", conn)
        Dim da As New OleDbDataAdapter(cmd)
        da.Fill(dt)

        dgvRecords.DataSource = dt
        dgvRecords.ClearSelection()
        dgvRecords.CurrentCell = Nothing

    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProduct.SelectedIndexChanged


        Dim selectedProduct As String = cmbProduct.Text.Trim()
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
                        txtUnitPrice.Text = CDec(result).ToString("F2")
                    Else
                        txtUnitPrice.Text = "0.00"
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading unit price: " & ex.Message)
        End Try




        'Dim selectedProduct As String = cmbProduct.Text.Trim()
        If selectedProduct = "" Then Exit Sub

        ' Load product image
        Dim imageFolder As String = "C:\Temp\ProductImages"
        Dim imagePath As String = Path.Combine(imageFolder, selectedProduct.Replace(" ", "") & ".png")
        If File.Exists(imagePath) Then
            PictureBox1.Image = Image.FromFile(imagePath)
        Else
            PictureBox1.Image = Nothing
        End If

    End Sub


    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        btnSave.Enabled = False
        btnEdit.Enabled = False
        Try
            If dgvRecords.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a row to delete")
                Exit Sub
            End If
            Dim transactionID As String = dgvRecords.SelectedRows(0).Cells("Transaction_Number").Value.ToString
            Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to delete this sale?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirm = DialogResult.No Then
                Exit Sub
            End If
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "DELETE FROM [Sales_Details] WHERE [Transaction_Number]=?"

                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.Add("@p1", OleDbType.VarWChar).Value = transactionID
                    cmd.ExecuteNonQuery()

                End Using
                conn.Close()
            End Using

            Dim dash = TryCast(Me.Owner, Form1)
            If dash IsNot Nothing Then
                dash.LoadTotalsFromDatabase()
                dash.RefreshLabels()

            End If

            MessageBox.Show("sale deleted successfully")
            LoadData()
        Catch ex As Exception
            MessageBox.Show("delete failed: " & ex.Message)
        End Try
        ClearForm()


    End Sub

    Private Sub txtQuantity_TextChanged(sender As Object, e As EventArgs) Handles txtQuantity.TextChanged
        UpdateTotalPrice()
    End Sub

    Private Sub UpdateTotalPrice()
        Dim qty As Integer
        Dim unitPrice As Decimal

        If Integer.TryParse(txtQuantity.Text, qty) AndAlso Decimal.TryParse(txtUnitPrice.Text, unitPrice) Then
            TextBox1.Text = (qty * unitPrice).ToString("F2")
        Else
            TextBox1.Text = "0.00"
        End If
    End Sub

    Private Sub txtUnitPrice_TextChanged(sender As Object, e As EventArgs) Handles txtUnitPrice.TextChanged
        UpdateTotalPrice()
    End Sub

    Private Sub dgvRecords_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRecords.CellContentClick

    End Sub
End Class

