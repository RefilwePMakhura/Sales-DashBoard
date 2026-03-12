
Imports System.IO
Imports System.Globalization
Imports System.ComponentModel
Imports System.Data.OleDb

Public Class Create_Invoice
    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Public Property SelectedProduct As String
    Public Property SelectedQuantity As String
    Public Property SelectedSubtotal As String
    Public Property SelectedDiscount As String
    Public Property SelectedVAT As String
    Public Property SelectedTotal As String

    '--- Constructor ---
    ' Private InvoiceID As Integer

    Public Sub New()
        InitializeComponent()

    End Sub
    Dim invoiceid As String
    Public Sub addInvoiceItem(Product As String, Quantity As Integer, Subtotal As Decimal, VAT As Decimal, Discount As Decimal, Total As Decimal)
        DgvInvoiceLines.Rows.Add(InvoiceID, Product, Quantity, Subtotal.ToString("0.00"), VAT.ToString("0.00"), Discount.ToString("0.00"), Total.ToString("0.00"))

    End Sub
    Private Function GetLastTransactionFromSales() As String
        Try
            Dim salesFilePath As String = "C:\Temp\Sales.csv"
            If File.Exists(salesFilePath) Then
                Dim allLines() As String = File.ReadAllLines(salesFilePath)
                If allLines.Length > 1 Then
                    ' Get the last line and split by comma
                    Dim lastLine As String = allLines.Last()
                    Dim parts() As String = lastLine.Split(","c)
                    Return parts(0) ' Assuming TransactNo is the first column
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error reading transaction number: " & ex.Message)
        End Try
        Return ""
    End Function

    Private Sub LoadSalesOrderDetails(InvoiceID As String)
        Dim conn As New OleDb.OleDbConnection(ConnectionString)
        Dim cmd As New OleDb.OleDbCommand("SELECT Quantity, Discount, Amount, VAT, Total FROM Sales_Order WHERE InvoiceID =?", conn)
        cmd.Parameters.AddWithValue("?", InvoiceID)

        Dim da As New OleDb.OleDbDataAdapter(cmd)
        Dim dt As New DataTable()

        da.Fill(dt)
        DgvInvoiceLines.DataSource = dt
    End Sub

    Private Sub clearform()
        txtDiscount.Clear()
        txtSubtotal.Clear()
        txtTax.Clear()
        txtTotalAmount.Clear()
    End Sub
    '--- When Form Loads ---
    Private Sub Create_Invoice_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Try
        SetupInvoiceGrid()

        '    '  txtInvoiceID.Text =InvoiceID()

        '    If SelectedProduct <> "" Then
        '        addInvoiceItem(SelectedProduct, SelectedQuantity, SelectedSubtotal, SelectedVAT, SelectedDiscount, SelectedTotal)

        '    End If

        CalculateGrandTotals()

        'Catch ex As Exception
        '    MessageBox.Show("Error loading invoice form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try



        LoadCompanyInfo()
        UpdateTotals()
        cmbStatus.Items.AddRange(New Object() {"Pending", "Paid"})
        cmbStatus.SelectedIndex = 0

        cmbTerms.Items.AddRange(New String() {"Cash", "EFT", "Debit Card", "Bank Deposit"})
        cmbTerms.SelectedIndex = 0
        ReloadCustomersCombo()
        '   clearform()
        ' Load products BEFORE setting up grid
        '   LoadProducts()

        'If Not String.IsNullOrWhiteSpace(OrderID) Then
        '    txtInvoiceID.Text = InvoiceID
        '    LoadSalesOrderDetails(InvoiceID)
        'End If
        DgvInvoiceLines.ClearSelection()
        DgvInvoiceLines.CurrentCell = Nothing
        AddHandler DgvInvoiceLines.DataError, AddressOf DgvInvoiceLines_DataError

        If Not String.IsNullOrWhiteSpace(SelectedProduct) Then
            Dim r As Integer = DgvInvoiceLines.Rows.Add()
            With DgvInvoiceLines.Rows(r)
                .Cells("Product").Value = SelectedProduct
                .Cells("Quantity").Value = SelectedQuantity
                .Cells("Subtotal").Value = SelectedSubtotal
                .Cells("Discount").Value = Sales_Order.TextBox6.Text.ToString()
                .Cells("VAT").Value = SelectedVAT
                .Cells("Total").Value = SelectedTotal
                CalculateGrandTotals()
            End With
        End If
        Try
            ' Try to fetch Transaction No from open Sales form
            For Each f As Form In Application.OpenForms
                If TypeOf f Is Add_Sales Then
                    Dim salesForm As Add_Sales = DirectCast(f, Add_Sales)
                    ' TextBox6.Text = frmSales.CurrentTransactionNo
                    Exit For
                End If
            Next

            ' If not found, read last Transaction No from CSV file
            If String.IsNullOrEmpty(txtTransactionNo.Text) Then
                txtTransactionNo.Text = GetLastTransactionFromSales()
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading transaction number: " & ex.Message)
        End Try
        ' LoadProductsToInvoiceGrid()
        ReloadCustomersCombo()


    End Sub
    Private Sub CalculateGrandTotals()
        Dim subtotalsum As Decimal = 0D
        Dim vatsum As Decimal = 0D
        Dim discountsum As Decimal = 0D
        Dim totalsum As Decimal = 0D
        Dim qtytotal As Integer
        For Each row As DataGridViewRow In DgvInvoiceLines.Rows
            If row.IsNewRow Then Continue For

            If row.Cells("Subtotal").Value IsNot Nothing AndAlso
               IsNumeric(row.Cells("Subtotal").Value) Then
                subtotalsum +=
                    CDec(row.Cells("Subtotal").Value)
            End If

            If row.Cells("Quantity").Value Then
                qtytotal +=
                    CInt(row.Cells("Quantity").Value)
            End If

            If row.Cells("VAT").Value IsNot Nothing AndAlso
               IsNumeric(row.Cells("VAT").Value) Then
                vatsum +=
                    CDec(row.Cells("VAT").Value)
            End If

            If row.Cells("Discount").Value IsNot Nothing AndAlso
               IsNumeric(row.Cells("Discount").Value) Then
                discountsum +=
                    CDec(row.Cells("Discount").Value)
            End If

            If row.Cells("Total").Value IsNot Nothing AndAlso
               IsNumeric(row.Cells("Total").Value) Then
                totalsum +=
                    CDec(row.Cells("Total").Value)
            End If
        Next

        TextBox2.Text = qtytotal.ToString("")
        txtSubtotal.Text = subtotalsum.ToString("0.00")
        txtTax.Text = vatsum.ToString("0.00")
        txtDiscount.Text = discountsum.ToString("0.00")
        txtTotalAmount.Text = totalsum.ToString("0.00")
    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DgvInvoiceLines.EditingControlShowing
        'If TypeOf e.Control Is ComboBox Then
        '    Dim combo As ComboBox = CType(e.Control, ComboBox)
        '    RemoveHandler combo.SelectedIndexChanged, AddressOf ProductCombo_SelectedIndexChanged
        '    AddHandler combo.SelectedIndexChanged, AddressOf ProductCombo_SelectedIndexChanged
        'End If
        '   My.Settings.UserName =
    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellEndEdit
        If e.RowIndex < 0 Then Exit Sub
        Dim colName = DgvInvoiceLines.Columns(e.ColumnIndex).Name
        If colName = "Quantity" OrElse colName = "Product" Then
            CalculateRow(DgvInvoiceLines.Rows(e.RowIndex))

        End If
    End Sub




    Private Sub ProductCombo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim combo As ComboBox = CType(sender, ComboBox)
        Dim selectedProduct As String = combo.Text
        Dim discount As Decimal = 0D
        Dim dtProducts As DataTable = TryCast(DgvInvoiceLines.Tag, DataTable)
        If dtProducts Is Nothing Then Exit Sub

        ' Find product details
        Dim found() As DataRow = dtProducts.Select("[Product_Name] = '" & selectedProduct.Replace("'", "''") & "'")
        If found.Length > 0 Then
            Dim InvoiceId As String = txtInvoiceID.Text
            Dim unitPrice As Decimal = 0
            Decimal.TryParse(found(0)("Subtotal").ToString(), unitPrice)
            Dim currentRow As DataGridViewRow = DgvInvoiceLines.CurrentRow
            currentRow.Cells("Subtotal").Value = unitPrice.ToString("F2")
            currentRow.Cells("Quantity").Value = ""
            currentRow.Cells("InvoiceID").Value = InvoiceId.ToString()
            currentRow.Cells("Total").Value = unitPrice.ToString("F2")
            currentRow.Cells("Discount").Value = discount.ToString(unitPrice * 0.05)
        End If
    End Sub

    Private Sub btnAddInvoice_Click(sender As Object, e As EventArgs) Handles btnAddLine.Click
        DgvInvoiceLines.Rows.Add("", "", 1, 0, 0, DgvInvoiceLines.Rows.Count + 1)


    End Sub


    '======================
    ' RECALCULATE INVOICE TOTAL
    '======================
    Private Sub RecalculateInvoiceTotal()
        Dim grandTotal As Decimal = 0D
        For Each row As DataGridViewRow In DgvInvoiceLines.Rows
            If row.IsNewRow Then Continue For
            Dim lineTotal As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("Total").Value), lineTotal)
            grandTotal += lineTotal
        Next

        ' Show total in a label or textbox (make sure you have one named txtTotal)
        If Me.Controls.ContainsKey("txtsubtotal") Then
            CType(Me.Controls("txtsubtotal"), TextBox).Text = grandTotal.ToString("N2")
        End If
        'PopulateSummaryToGrid()
    End Sub
    Private Function GetProducts() As DataTable
        Dim dt As New DataTable
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "SELECT [Product_Name], [Unit_Price] FROM [Product_Details] ORDER BY [Product_Name]"
            Using da As New OleDbDataAdapter(sql, conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Private Sub LoadCompanyInfo()
        Using conn As New OleDb.OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand("SELECT TOP 1 * FROM CompanySettings", conn)
            Using dr As OleDb.OleDbDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    Label10.Text = dr("CompanyName").ToString()
                    Label11.Text = dr("Address").ToString()
                    Label21.Text = dr("Phone").ToString()
                    Label23.Text = dr("Email").ToString

                    Dim logoPath As String = dr("LogoPath").ToString()
                    If IO.File.Exists(logoPath) Then
                        PictureBox1.Image = Image.FromFile(logoPath)
                    End If
                End If
            End Using
        End Using
    End Sub


    'Public Sub LoadProduct()
    '    Dim dt As New DataTable
    '    Dim da As New OleDbDataAdapter("SELECT Product_Name FROM Products", conn)
    '    da.Fill(dt)

    '    Dim col As DataGridViewComboBoxColumn = CType(DgvInvoiceLines.Columns("Product"), DataGridViewComboBoxColumn)
    '    col.DataSource = dt
    '    col.DisplayMember = "Product_Name"
    '    col.ValueMember = "Product_Name"
    '    col.DataPropertyName = "Product_Name"

    'End Sub
    Public Sub SetupInvoiceGrid()
        DgvInvoiceLines.Columns.Clear()
        DgvInvoiceLines.AutoGenerateColumns = False
        '      DgvInvoiceLines.AllowUserToAddRows = False
        Dim colProduct As New DataGridViewComboBoxColumn
        colProduct.Name = "Product"
        colProduct.HeaderText = "Product"
        colProduct.DataSource = GetProducts()
        colProduct.DisplayMember = "Product_Name"
        colProduct.ValueMember = "Product_Name"
        colProduct.DataPropertyName = "Product_Name"
        DgvInvoiceLines.Columns.Add(colProduct)
        '  DgvInvoiceLines.Columns.Add("Unit Price", "Unit Price")
        DgvInvoiceLines.Columns.Add("Quantity", "Quantity")
        DgvInvoiceLines.Columns.Add("Discount", "Discount")
        DgvInvoiceLines.Columns.Add("Subtotal", "Subtotal")
        DgvInvoiceLines.Columns.Add("VAT", "VAT")
        DgvInvoiceLines.Columns.Add("Total", "Total")


        DgvInvoiceLines.AllowUserToAddRows = False
        DgvInvoiceLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvInvoiceLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Function safeDecimal(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then Return 0D

        Dim s As String = value.ToString().Trim()
        s = s.Replace("", "")
        s = s.Replace("", "")

        Dim result As Decimal
        Decimal.TryParse(s, Globalization.NumberStyles.Any,
        Globalization.CultureInfo.InvariantCulture, result)
        Return result

    End Function
    '======================
    ' SAVE BUTTON CODE
    '======================
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSaveInvoice.Click
        '===Generate unique IDs===
        TextBox11.Text = Module1.GenerateProduct_ID()
        txtInvoiceID.Text = Module1.GenerateInvoiceID()
        TextBox12.Text = Module1.GenerateInvoiceNO()
        Dim TransactionNumber As String = Module1.GenerateTransactionNumber
        txtTransactionNo.Text = TransactionNumber

        Try

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                For Each row As DataGridViewRow In DgvInvoiceLines.Rows

                    If row.IsNewRow Then Continue For
                    If row.Cells("Product").Value Is Nothing Then Continue For

                    Using cmd As New OleDbCommand("INSERT INTO Invoice_Details " &
       " ([InvoiceID], [Product], [Quantity], [Subtotal], [Discount], [VAT], [Total], [Status]) " &
        "VALUES (?,?,?,?,?,?,?,?)", conn)

                        cmd.Parameters.AddWithValue("@p1", txtInvoiceID.Text.Trim())
                        cmd.Parameters.AddWithValue("@p2", row.Cells("Product").Value.ToString())
                        cmd.Parameters.AddWithValue("@p3", row.Cells("Quantity").Value)
                        cmd.Parameters.AddWithValue("@p4", row.Cells("Subtotal").Value)
                        cmd.Parameters.AddWithValue("@p5", row.Cells("Discount").Value)
                        cmd.Parameters.AddWithValue("@p6", Val(row.Cells("VAT").Value))
                        cmd.Parameters.AddWithValue("@p7", Val(row.Cells("Total").Value))
                        cmd.Parameters.AddWithValue("@p8", cmbStatus.Text.Trim())
                        '  cmd.Parameters.AddWithValue("@p9", DateTimePicker1.Value)
                        cmd.ExecuteNonQuery()
                    End Using

                Next
                conn.Close()
                UpdateTotals()
                frmInvoiceManagement.LoadData()
                frmInvoiceManagement.ShowDialog()
            End Using
            ' Cart.ShowDialog()
            clearform()
            MessageBox.Show("Invoice Saved successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As OleDbException
            MessageBox.Show("Database error" & ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            MessageBox.Show(
        $"Failed to save:{Environment.NewLine}{ex}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)
            MessageBox.Show("Stack Trace:" & ex.StackTrace)
            Debug.WriteLine(ex.ToString)
        End Try
    End Sub
    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString)
    End Sub
    Private Sub RecalculateRow(rowindex As Integer)
        Dim row = DgvInvoiceLines.Rows(rowindex)

        Dim qty As Decimal = CInt(row.Cells("Quantity").Value)
        Dim price As Decimal = CInt(row.Cells("Subtotal").Value)
        Dim discountPercent As Decimal = CInt(row.Cells("Discount").Value)

        Dim gross As Decimal = qty * price
        Dim discountAmount As Decimal = gross * discountPercent / 100D
        '  Dim subtotal As Decimal = gross - discountAmount
        ' Dim vat As Decimal = subtotal * 0.15D
        Dim totalWithVAT As Decimal = gross - discountAmount

        'row.Cells("LineTotal").Value = gross
        ' row.Cells("VAT").Value = vat
        row.Cells("Total").Value = txtTotalAmount.Text

        txtSubtotal.Text = gross.ToString("0.00")
        '    txtTax.Text = vatsum.ToString("0.00")
        txtDiscount.Text = discountAmount.ToString("0.00")
        txtTotalAmount.Text = totalWithVAT.ToString("0.00")

    End Sub
    Dim qty As Integer
    Private Sub DgvInvoice_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellValueChanged
        If e.RowIndex < 0 Then Exit Sub
        Dim row = DgvInvoiceLines.Rows(e.RowIndex)
        If DgvInvoiceLines.Columns(e.ColumnIndex).Name = "Product" Then

            Dim Product_Name = row.Cells("Product").Value
            If Product_Name Is Nothing Then Exit Sub
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Unit_Price FROM[Product_Details] WHERE Product_Name= ?", conn)
                cmd.Parameters.AddWithValue("@p1", Product_Name)
                Dim price = cmd.ExecuteScalar()
                If price IsNot Nothing AndAlso Not IsDBNull(price) Then
                    Dim unitPrice As Decimal = Convert.ToDecimal(price)
                    row.Cells("Subtotal").Value = unitPrice
                    row.Cells("Quantity").Value = TextBox2.Text
                End If
                CalculateRow(row)
                UpdateTotals()
                RecalculateInvoiceTotal()
                CalculateGrandTotals()
            End Using
        End If
    End Sub
    Private Sub ReloadCustomersCombo()
        '  ComboBox3.Items.Clear()

        Try
            Using conn As New OleDbConnection(ConnectionString)
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
                conn.Close()
            End Using

            '' Optional: select first customer
            'If ComboBox3.Items.Count > 0 AndAlso ComboBox3.SelectedIndex = -1 Then
            '    ComboBox3.SelectedIndex = 0
            'End If

        Catch ex As Exception
            MessageBox.Show("Error loading customer from database: " & ex.Message)
        End Try
    End Sub
    '======================
    ' CLASS DEFINITION
    '======================




    'Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    '    LoadProducts()
    '    MessageBox.Show("Products refreshed from Manage Products.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
    'End Sub




    Private Sub DgvInvoiceLines_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvInvoiceLines.DataError
        e.ThrowException = False
    End Sub

    Private Sub CalculateRow(row As DataGridViewRow)
        If row Is Nothing OrElse row.IsNewRow Then Exit Sub

        Dim qty As Integer
        Dim unitPrice As Decimal = 0D
        Dim discountPercent As Decimal = 0D

        Integer.TryParse(row.Cells("Quantity").Value?.ToString(), qty)
        Decimal.TryParse(row.Cells("Subtotal").Value?.ToString(), unitPrice)
        Decimal.TryParse(row.Cells("Discount").Value?.ToString(), discountPercent)

        Dim gross As Decimal = qty * unitPrice
        Dim discountAmount As Decimal = gross * (discountPercent / 100D)
        Dim subtotal As Decimal = gross - discountAmount
        Dim vat As Decimal = subtotal * 0.15D
        Dim totalWithVAT As Decimal = subtotal + vat

        row.Cells("Subtotal").Value = Math.Round(subtotal, 2)
        row.Cells("VAT").Value = Math.Round(vat, 2)
        row.Cells("Total").Value = Math.Round(totalWithVAT, 2)


    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        frmInvoiceManagement.ShowDialog()
    End Sub

    Private Sub btnApplyPayment_Click(sender As Object, e As EventArgs) Handles btnApplyPayment.Click
        PaymentFrm.ShowDialog()
    End Sub

    Private Sub btnRemoveLine_Click(sender As Object, e As EventArgs) Handles btnRemoveLine.Click

        ' === REMOVE LINE ===

        If DgvInvoiceLines.SelectedRows.Count > 0 Then
            DgvInvoiceLines.Rows.Remove(DgvInvoiceLines.SelectedRows(0))
            UpdateTotals()
        Else
            MessageBox.Show("Select a line to remove.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        If DgvInvoiceLines.SelectedRows.Count > 0 Then
            DgvInvoiceLines.Rows.Remove(DgvInvoiceLines.SelectedRows(0))
            UpdateTotals()
        End If
    End Sub

    Private Sub UpdateTotals()
        Dim subtotal As Decimal = 0D
        Dim discountRate As Decimal = 0D
        Dim vatRate As Decimal = 0.15D        ' 15% VAT

        ' --- Step 1: Calculate subtotal from all invoice lines ---
        For Each row As DataGridViewRow In DgvInvoiceLines.Rows
            If row.IsNewRow Then Continue For
            Dim linetotal As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("Subtotal").Value), linetotal)
            subtotal += linetotal
        Next

        Decimal.TryParse(txtDiscount.Text, discountRate)
        Dim tax = subtotal * vatRate
        Dim total = subtotal + tax - discountRate
        If total < 0 Then total = 0D

        txtSubtotal.Text = subtotal.ToString("N2")
        txtTax.Text = tax.ToString("N2")
        txtTotalAmount.Text = total.ToString("N2")

    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Using r = GetRows("Customer_Details", "Customer_Name", ComboBox3.Text)
            If r.Read() Then
                TextBox1.Text = r("Email").ToString()
                ' TextBox1.Text = r("Contact").ToString()
                TextBox5.Text = r("Address").ToString()

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
    Private Sub txtDiscount_TextChanged(sender As Object, e As EventArgs) Handles txtDiscount.TextChanged
        'Dim DiscountValue As Decimal = 0D

        'If Not Decimal.TryParse(txtDiscount.Text, DiscountValue) Then
        '    DiscountValue = 0D
        'End If

        'If DiscountValue < 0 Then
        '    DiscountValue = 0D
        'End If
        CalculateGrandTotals()
        UpdateTotals()
        'For Each row As DataGridViewRow In DgvInvoiceLines.Rows
        '    If row.IsNewRow Then Continue For row.Cells("Discount").Value = CDec(txtDiscount.Text)
        '    RecalculateRow(row.Index)
        'Next

    End Sub

    Private Sub btnSendInvoice_Click(sender As Object, e As EventArgs) Handles btnSendInvoice.Click

    End Sub

    Private Sub DgvInvoiceLines_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvInvoiceLines.CellContentClick

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        CalculateGrandTotals()
    End Sub
End Class