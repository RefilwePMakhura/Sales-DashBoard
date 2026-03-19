Imports System.IO
Imports System.Globalization
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Diagnostics

Public Class Create_Invoice

    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"

    Public Property SelectedProduct As String
    Public Property SelectedQuantity As String
    Public Property SelectedSubtotal As String
    Public Property SelectedDiscount As String
    Public Property SelectedVAT As String
    Public Property SelectedTotal As String

    Public Sub New()
        InitializeComponent()
    End Sub

    Private invoiceid As String = ""

    Public Sub addInvoiceItem(Product As String,
                              Quantity As Integer,
                              Subtotal As Decimal,
                              VAT As Decimal,
                              Discount As Decimal,
                              Total As Decimal)

        DgvInvoiceLines.Rows.Add(Product,
                                 Quantity,
                                 Discount.ToString("0.00"),
                                 Subtotal.ToString("0.00"),
                                 VAT.ToString("0.00"),
                                 Total.ToString("0.00"),
                                 "0.00")
    End Sub

    Private Function GetLastTransactionFromSales() As String
        Try
            Dim salesFilePath As String = "C:\Temp\Sales.csv"

            If File.Exists(salesFilePath) Then
                Dim allLines() As String = File.ReadAllLines(salesFilePath)

                If allLines.Length > 1 Then
                    Dim lastLine As String = allLines.Last()
                    Dim parts() As String = lastLine.Split(","c)

                    If parts.Length > 0 Then
                        Return parts(0)
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error reading transaction number: " & ex.Message)
        End Try

        Return ""
    End Function

    Private Sub LoadSalesOrderDetails(invoiceIDValue As String)
        Try
            Using conn As New OleDbConnection(Module1.ConnectionString)
                Using cmd As New OleDbCommand("SELECT Product, Quantity, Discount, Subtotal, VAT, Total FROM Sales_Order WHERE InvoiceID = ?", conn)
                    cmd.Parameters.AddWithValue("?", invoiceIDValue)

                    Dim dt As New DataTable()
                    Using da As New OleDbDataAdapter(cmd)
                        da.Fill(dt)
                    End Using

                    DgvInvoiceLines.Rows.Clear()

                    For Each dr As DataRow In dt.Rows
                        Dim r As Integer = DgvInvoiceLines.Rows.Add()
                        DgvInvoiceLines.Rows(r).Cells("Product").Value = dr("Product").ToString()
                        DgvInvoiceLines.Rows(r).Cells("Quantity").Value = ToInt(dr("Quantity"))
                        DgvInvoiceLines.Rows(r).Cells("Discount").Value = ToDecimalValue(dr("Discount")).ToString("0.00")
                        DgvInvoiceLines.Rows(r).Cells("Subtotal").Value = ToDecimalValue(dr("Subtotal")).ToString("0.00")
                        DgvInvoiceLines.Rows(r).Cells("VAT").Value = ToDecimalValue(dr("VAT")).ToString("0.00")
                        DgvInvoiceLines.Rows(r).Cells("Total").Value = ToDecimalValue(dr("Total")).ToString("0.00")
                    Next
                End Using
            End Using

            CalculateGrandTotals()

        Catch ex As Exception
            MessageBox.Show("Error loading sales order details: " & ex.Message)
        End Try
    End Sub

    Private Sub clearform()
        txtDiscount.Clear()
        txtSubtotal.Clear()
        txtTax.Clear()
        txtTotalAmount.Clear()
        TextBox2.Clear()
    End Sub

    Private Sub Create_Invoice_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SetupInvoiceGrid()
            LoadCompanyInfo()

            cmbStatus.Items.Clear()
            cmbStatus.Items.AddRange(New Object() {"Pending", "Paid"})
            cmbStatus.SelectedIndex = 0

            cmbTerms.Items.Clear()
            cmbTerms.Items.AddRange(New String() {"Cash", "EFT", "Debit Card", "Bank Deposit"})
            cmbTerms.SelectedIndex = 0

            ReloadCustomersCombo()

            DgvInvoiceLines.ClearSelection()
            DgvInvoiceLines.CurrentCell = Nothing

            AddHandler DgvInvoiceLines.DataError, AddressOf DgvInvoiceLines_DataError

            If Not String.IsNullOrWhiteSpace(SelectedProduct) Then
                Dim r As Integer = DgvInvoiceLines.Rows.Add()
                With DgvInvoiceLines.Rows(r)
                    .Cells("Product").Value = SelectedProduct
                    .Cells("Quantity").Value = If(String.IsNullOrWhiteSpace(SelectedQuantity), "1", SelectedQuantity)
                    .Cells("Discount").Value = If(String.IsNullOrWhiteSpace(SelectedDiscount), "0.00", SelectedDiscount)
                    .Cells("Subtotal").Value = If(String.IsNullOrWhiteSpace(SelectedSubtotal), "0.00", SelectedSubtotal)
                    .Cells("VAT").Value = If(String.IsNullOrWhiteSpace(SelectedVAT), "0.00", SelectedVAT)
                    .Cells("Total").Value = If(String.IsNullOrWhiteSpace(SelectedTotal), "0.00", SelectedTotal)
                    .Cells("UnitPrice").Value = If(String.IsNullOrWhiteSpace(SelectedSubtotal), "0.00", SelectedSubtotal)
                End With
            End If

            Try
                For Each f As Form In Application.OpenForms
                    If TypeOf f Is Add_Sales Then
                        Dim salesForm As Add_Sales = DirectCast(f, Add_Sales)
                        Exit For
                    End If
                Next

                If String.IsNullOrEmpty(txtTransactionNo.Text) Then
                    txtTransactionNo.Text = GetLastTransactionFromSales()
                End If

            Catch ex As Exception
                MessageBox.Show("Error loading transaction number: " & ex.Message)
            End Try

            CalculateGrandTotals()

        Catch ex As Exception
            MessageBox.Show("Error loading invoice form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetProducts() As DataTable
        Dim dt As New DataTable

        Using conn As New OleDbConnection(Module1.ConnectionString)
            conn.Open()

            Dim sql As String = "SELECT [Product_Name], [Unit_Price] FROM [Product_Details] ORDER BY [Product_Name]"

            Using da As New OleDbDataAdapter(sql, conn)
                da.Fill(dt)
            End Using
        End Using

        Return dt
    End Function

    Private Sub LoadCompanyInfo()
        Try
            Using conn As New OleDbConnection(Module1.ConnectionString)
                conn.Open()

                Dim cmd As New OleDbCommand("SELECT TOP 1 * FROM CompanySettings", conn)

                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        Label10.Text = dr("CompanyName").ToString()
                        Label11.Text = dr("Address").ToString()
                        Label21.Text = dr("Phone").ToString()
                        Label23.Text = dr("Email").ToString()

                        Dim logoPath As String = dr("LogoPath").ToString()
                        If IO.File.Exists(logoPath) Then
                            PictureBox1.Image = Image.FromFile(logoPath)
                        End If
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading company info: " & ex.Message)
        End Try
    End Sub

    Public Sub SetupInvoiceGrid()
        DgvInvoiceLines.Columns.Clear()
        DgvInvoiceLines.AutoGenerateColumns = False
        DgvInvoiceLines.AllowUserToAddRows = False

        Dim colProduct As New DataGridViewComboBoxColumn()
        colProduct.Name = "Product"
        colProduct.HeaderText = "Product"
        colProduct.DataSource = GetProducts()
        colProduct.DisplayMember = "Product_Name"
        colProduct.ValueMember = "Product_Name"
        colProduct.DataPropertyName = "Product_Name"
        DgvInvoiceLines.Columns.Add(colProduct)

        Dim colQty As New DataGridViewTextBoxColumn()
        colQty.Name = "Quantity"
        colQty.HeaderText = "Quantity"
        DgvInvoiceLines.Columns.Add(colQty)

        Dim colDiscount As New DataGridViewTextBoxColumn()
        colDiscount.Name = "Discount"
        colDiscount.HeaderText = "Discount %"
        DgvInvoiceLines.Columns.Add(colDiscount)

        Dim colSubtotal As New DataGridViewTextBoxColumn()
        colSubtotal.Name = "Subtotal"
        colSubtotal.HeaderText = "Subtotal"
        colSubtotal.ReadOnly = True
        DgvInvoiceLines.Columns.Add(colSubtotal)

        Dim colVAT As New DataGridViewTextBoxColumn()
        colVAT.Name = "VAT"
        colVAT.HeaderText = "VAT"
        colVAT.ReadOnly = True
        DgvInvoiceLines.Columns.Add(colVAT)

        Dim colTotal As New DataGridViewTextBoxColumn()
        colTotal.Name = "Total"
        colTotal.HeaderText = "Total"
        colTotal.ReadOnly = True
        DgvInvoiceLines.Columns.Add(colTotal)

        Dim colUnitPrice As New DataGridViewTextBoxColumn()
        colUnitPrice.Name = "UnitPrice"
        colUnitPrice.HeaderText = "Unit Price"
        colUnitPrice.Visible = False
        DgvInvoiceLines.Columns.Add(colUnitPrice)

        DgvInvoiceLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvInvoiceLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Function safeDecimal(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then Return 0D

        Dim s As String = value.ToString().Trim()
        Dim result As Decimal

        Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, result)
        If result = 0D Then
            Decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, result)
        End If

        Return result
    End Function

    Private Function ToDecimalValue(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then Return 0D

        Dim result As Decimal
        Decimal.TryParse(Convert.ToString(value), result)
        Return result
    End Function

    Private Function ToInt(value As Object) As Integer
        If value Is Nothing OrElse IsDBNull(value) Then Return 0

        Dim result As Integer
        Integer.TryParse(Convert.ToString(value), result)
        Return result
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSaveInvoice.Click
        TextBox11.Text = Module1.GenerateProduct_ID()
        txtInvoiceID.Text = Module1.GenerateInvoiceID()
        TextBox12.Text = Module1.GenerateInvoiceNO()
        txtTransactionNo.Text = Module1.GenerateTransactionNumber()

        Try
            Using conn As New OleDbConnection(Module1.ConnectionString)
                conn.Open()

                For Each row As DataGridViewRow In DgvInvoiceLines.Rows
                    If row.IsNewRow Then Continue For
                    If row.Cells("Product").Value Is Nothing Then Continue For

                    Using cmd As New OleDbCommand(
                        "INSERT INTO Invoice_Details ([InvoiceID], [Product], [Quantity], [Subtotal], [Discount], [VAT], [Total], [Status]) VALUES (?,?,?,?,?,?,?,?)", conn)

                        cmd.Parameters.AddWithValue("@p1", txtInvoiceID.Text.Trim())
                        cmd.Parameters.AddWithValue("@p2", row.Cells("Product").Value.ToString())
                        cmd.Parameters.AddWithValue("@p3", ToInt(row.Cells("Quantity").Value))
                        cmd.Parameters.AddWithValue("@p4", ToDecimalValue(row.Cells("Subtotal").Value))
                        cmd.Parameters.AddWithValue("@p5", ToDecimalValue(row.Cells("Discount").Value))
                        cmd.Parameters.AddWithValue("@p6", ToDecimalValue(row.Cells("VAT").Value))
                        cmd.Parameters.AddWithValue("@p7", ToDecimalValue(row.Cells("Total").Value))
                        cmd.Parameters.AddWithValue("@p8", cmbStatus.Text.Trim())

                        cmd.ExecuteNonQuery()
                    End Using
                Next
            End Using

            CalculateGrandTotals()

            'conn.Close()
            UpdateTotals()

            If frmInvoiceManagement.Visible Then
                frmInvoiceManagement.LoadData()
                frmInvoiceManagement.BringToFront()
            Else
                frmInvoiceManagement.LoadData()
                frmInvoiceManagement.Show()
            End If

            clearform()

            MessageBox.Show("Invoice saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show("Failed to save: " & Environment.NewLine & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString())
    End Sub

    Private Sub DgvInvoiceLines_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DgvInvoiceLines.EditingControlShowing
    End Sub

    Private Sub DgvInvoiceLines_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellEndEdit
        If e.RowIndex < 0 Then Exit Sub

        Dim colName As String = DgvInvoiceLines.Columns(e.ColumnIndex).Name

        If colName = "Quantity" OrElse colName = "Product" OrElse colName = "Discount" Then
            CalculateRow(DgvInvoiceLines.Rows(e.RowIndex))
            CalculateGrandTotals()
        End If
    End Sub

    Private Sub ProductCombo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim combo As ComboBox = CType(sender, ComboBox)
        Dim selectedProduct As String = combo.Text
        Dim dtProducts As DataTable = TryCast(DgvInvoiceLines.Tag, DataTable)

        If dtProducts Is Nothing Then Exit Sub

        Dim found() As DataRow = dtProducts.Select("[Product_Name] = '" & selectedProduct.Replace("'", "''") & "'")

        If found.Length > 0 Then
            Dim unitPrice As Decimal = 0D
            Decimal.TryParse(found(0)("Unit_Price").ToString(), unitPrice)

            Dim currentRow As DataGridViewRow = DgvInvoiceLines.CurrentRow
            If currentRow IsNot Nothing Then
                currentRow.Cells("UnitPrice").Value = unitPrice.ToString("0.00")
                currentRow.Cells("Quantity").Value = 1
                currentRow.Cells("Discount").Value = "0.00"
                CalculateRow(currentRow)
                CalculateGrandTotals()
            End If
        End If
    End Sub

    Private Sub btnAddInvoice_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
        DgvInvoiceLines.Rows.Add("", 1, "0.00", "0.00", "0.00", "0.00", "0.00")
    End Sub

    Private Sub RecalculateInvoiceTotal()
        CalculateGrandTotals()
    End Sub

    Private Sub DgvInvoiceLines_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellValueChanged
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DgvInvoiceLines.Rows(e.RowIndex)
        Dim colName As String = DgvInvoiceLines.Columns(e.ColumnIndex).Name

        If colName = "Product" Then
            Dim productName As Object = row.Cells("Product").Value
            If productName Is Nothing OrElse String.IsNullOrWhiteSpace(productName.ToString()) Then Exit Sub

            Try
                Using conn As New OleDbConnection(Module1.ConnectionString)
                    conn.Open()

                    Using cmd As New OleDbCommand("SELECT Unit_Price FROM [Product_Details] WHERE Product_Name = ?", conn)
                        cmd.Parameters.AddWithValue("@p1", productName.ToString())

                        Dim price As Object = cmd.ExecuteScalar()

                        If price IsNot Nothing AndAlso Not IsDBNull(price) Then
                            Dim unitPrice As Decimal = Convert.ToDecimal(price)
                            row.Cells("UnitPrice").Value = unitPrice.ToString("0.00")

                            If row.Cells("Quantity").Value Is Nothing OrElse ToInt(row.Cells("Quantity").Value) <= 0 Then
                                row.Cells("Quantity").Value = 1
                            End If

                            If row.Cells("Discount").Value Is Nothing Then
                                row.Cells("Discount").Value = "0.00"
                            End If

                            CalculateRow(row)
                            CalculateGrandTotals()
                        End If
                    End Using
                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading product price: " & ex.Message)
            End Try

        ElseIf colName = "Quantity" OrElse colName = "Discount" Then
            CalculateRow(row)
            CalculateGrandTotals()
        End If
    End Sub

    Private Sub ReloadCustomersCombo()
        Try
            Using conn As New OleDbConnection(Module1.ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT DISTINCT Customer_Name FROM Customer_Details ORDER BY Customer_Name"

                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        ComboBox3.Items.Clear()

                        While reader.Read()
                            ComboBox3.Items.Add(reader("Customer_Name").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading customer from database: " & ex.Message)
        End Try
    End Sub

    Private Sub DgvInvoiceLines_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvInvoiceLines.DataError
        e.ThrowException = False
    End Sub

    Private Sub CalculateRow(row As DataGridViewRow)
        If row Is Nothing OrElse row.IsNewRow Then Exit Sub

        Dim qty As Integer = ToInt(row.Cells("Quantity").Value)
        If qty <= 0 Then qty = 1

        Dim unitPrice As Decimal = ToDecimalValue(row.Cells("UnitPrice").Value)
        Dim discountPercent As Decimal = ToDecimalValue(row.Cells("Discount").Value)

        If discountPercent < 0 Then discountPercent = 0D
        If discountPercent > 100 Then discountPercent = 100D

        Dim gross As Decimal = qty * unitPrice
        Dim discountAmount As Decimal = gross * (discountPercent / 100D)
        Dim subtotal As Decimal = gross - discountAmount
        Dim vat As Decimal = subtotal * 0.15D
        Dim totalWithVAT As Decimal = subtotal + vat

        row.Cells("Quantity").Value = qty
        row.Cells("Discount").Value = discountPercent.ToString("0.00")
        row.Cells("Subtotal").Value = Math.Round(subtotal, 2).ToString("0.00")
        row.Cells("VAT").Value = Math.Round(vat, 2).ToString("0.00")
        row.Cells("Total").Value = Math.Round(totalWithVAT, 2).ToString("0.00")
    End Sub

    Private Sub CalculateGrandTotals()
        Dim subtotalSum As Decimal = 0D
        Dim vatSum As Decimal = 0D
        Dim discountSum As Decimal = 0D
        Dim totalSum As Decimal = 0D
        Dim qtyTotal As Integer = 0

        For Each row As DataGridViewRow In DgvInvoiceLines.Rows
            If row.IsNewRow Then Continue For

            qtyTotal += ToInt(row.Cells("Quantity").Value)
            subtotalSum += ToDecimalValue(row.Cells("Subtotal").Value)
            vatSum += ToDecimalValue(row.Cells("VAT").Value)

            Dim unitPrice As Decimal = ToDecimalValue(row.Cells("UnitPrice").Value)
            Dim qty As Integer = ToInt(row.Cells("Quantity").Value)
            Dim discountPercent As Decimal = ToDecimalValue(row.Cells("Discount").Value)
            Dim gross As Decimal = qty * unitPrice
            discountSum += gross * (discountPercent / 100D)

            totalSum += ToDecimalValue(row.Cells("Total").Value)
        Next

        TextBox2.Text = qtyTotal.ToString()
        txtSubtotal.Text = subtotalSum.ToString("0.00")
        txtTax.Text = vatSum.ToString("0.00")
        txtDiscount.Text = discountSum.ToString("0.00")
        txtTotalAmount.Text = totalSum.ToString("0.00")
    End Sub

    Private Sub UpdateTotals()
        CalculateGrandTotals()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        frmInvoiceManagement.ShowDialog()
    End Sub

    Private Sub btnApplyPayment_Click(sender As Object, e As EventArgs) Handles btnApplyPayment.Click
        PaymentFrm.ShowDialog()
    End Sub

    Private Sub btnRemoveLine_Click(sender As Object, e As EventArgs) Handles btnRemoveLine.Click
        If DgvInvoiceLines.SelectedRows.Count > 0 Then
            DgvInvoiceLines.Rows.Remove(DgvInvoiceLines.SelectedRows(0))
            CalculateGrandTotals()
        Else
            MessageBox.Show("Select a line to remove.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Using r = GetRows("Customer_Details", "Customer_Name", ComboBox3.Text)
            If r.Read() Then
                TextBox1.Text = r("Email").ToString()
                TextBox5.Text = r("Address").ToString()
            End If
        End Using
    End Sub

    Private Function GetRows(table As String, field As String, value As String) As OleDbDataReader
        Dim conn As New OleDbConnection(Module1.ConnectionString)
        Dim cmd As New OleDbCommand($"SELECT * FROM [{table}] WHERE [{field}] = ?", conn)
        cmd.Parameters.AddWithValue("?", value)
        conn.Open()
        Return cmd.ExecuteReader(CommandBehavior.CloseConnection)
    End Function

    Private Sub txtDiscount_TextChanged(sender As Object, e As EventArgs) Handles txtDiscount.TextChanged
    End Sub

    Private Sub btnSendInvoice_Click(sender As Object, e As EventArgs) Handles btnSendInvoice.Click
    End Sub

    Private Sub DgvInvoiceLines_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellContentClick
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
    End Sub

End Class