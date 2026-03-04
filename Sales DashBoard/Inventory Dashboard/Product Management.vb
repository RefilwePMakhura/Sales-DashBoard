Imports System.IO
Imports System.Globalization
Imports System.Data.OleDb
Public Class ProductMgtFrm

    Private ReadOnly _dataFilePath As String = "C:\Temp\Inventory\Suppliers_management\Product_management.csv"

    Dim Sku As String
    Dim Product As String
    Dim Category As String
    Dim ProductID As Decimal
    Dim UnitPrice As Decimal
    Public CurrentStock As String
    Public Recorderlevel As String
    Dim Suppliers As String
    Dim LowCount As Integer


    Dim ProdPrompt As String = "Enter the name of the product"

    Dim UnitPrompt As String = "Enter the price of the product"

    Dim CurrentPrompt As String = "Enter the quantity of your current stock"

    Dim ReorderPrompt As String = "Enter the reorder level of the product"

    Public Function GetProductList() As Dictionary(Of String, Decimal)
        Dim productList As New Dictionary(Of String, Decimal)

        ' Assuming you have a DataGridView named dgvProducts
        ' with columns: "ProductName" and "Price"
        For Each row As DataGridViewRow In dgvRecords.Rows
            If row.IsNewRow Then Continue For

            Dim name As String = Convert.ToString(row.Cells("ProductName").Value)
            Dim price As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("Price").Value), price)

            If Not String.IsNullOrWhiteSpace(name) AndAlso Not productList.ContainsKey(name) Then
                productList.Add(name, price)
            End If
        Next

        Return productList
    End Function
    Private Sub SaveProductImage(productName As String)
        If PictureBox1.Image Is Nothing Then Exit Sub
        Dim imageFolder As String = "C:\Temp\ProductImages"
        If Not Directory.Exists(imageFolder) Then Directory.CreateDirectory(imageFolder)

        ' Save image as PNG using product name
        Dim fileName As String = Path.Combine(imageFolder, productName.Replace(" ", "") & ".png")
        PictureBox1.Image.Save(fileName, Imaging.ImageFormat.Png)
    End Sub
    'Private Sub LoadInventoryData()
    '    ' Clear existing rows
    '    dgvRecords.Rows.Clear()

    '    ' Path to sales data CSV file
    '    Dim directoryPath As String = "C:\Temp\Inventory\Product_Management"
    '    Dim filePath As String = System.IO.Path.Combine(directoryPath, "product_management.csv")

    '    If Not System.IO.File.Exists(filePath) Then
    '        Exit Sub ' No data file yet
    '    End If

    '    ' Read CSV and populate DataGridView
    '    Using reader As New System.IO.StreamReader(filePath)
    '        ' Skip header line
    '        If Not reader.EndOfStream Then
    '            reader.ReadLine()
    '        End If

    '        ' Read each data line
    '        While Not reader.EndOfStream
    '            Dim line As String = reader.ReadLine()

    '            ' Parse the CSV line
    '            Dim parts As List(Of String) = SplitCsvLine(line)

    '            If parts.Count >= 8 Then
    '                ' Add row to DataGridView
    '                dgvRecords.Rows.Add(parts(0), parts(1), parts(3), parts(2), parts(4), parts(5), parts(6), parts(7))
    '            End If
    '        End While
    '    End Using
    'End Sub

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



    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        FrmInventory_Dashboard.Show()
    End Sub

    Private Sub dgvRecords_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRecords.CellContentClick

        ' Check if a row is selected
        'If dgvRecords.SelectedRows.Count > 0 Then
        '    Dim selectedRow = dgvRecords.SelectedRows(0)

        '    ' Get values from selected row
        '    Dim Sku As String = selectedRow.Cells("Column1").Value.ToString()
        '    Dim Product As String = selectedRow.Cells("Column2").Value.ToString()
        '    Dim Category As String = selectedRow.Cells("Column3").Value.ToString()
        '    Dim ProductID As String = selectedRow.Cells("Column4").Value.ToString()
        '    Dim UnitPrice As String = selectedRow.Cells("Column5").Value.ToString()
        '    Dim CurrentStock As String = selectedRow.Cells("Column6").Value.ToString()
        '    Dim Recorderlevel As String = selectedRow.Cells("Column7").Value.ToString()
        '    Dim Suppliers As String = selectedRow.Cells("Column8").Value.ToString()
        'End If
        '' Populate controls with selected row data
        'txtSKU.Text = Sku
        'txtProductName.Text = Product
        'cmbCategory.Text = Category
        'txtProductID.Text = ProductID
        'txtUnitPrice.Text = UnitPrice
        'txtCurrentStock.Text = CurrentStock
        'txtReorderlevel.Text = Recorderlevel
        'txtSupplier.Text = Suppliers


    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRecords.SelectionChanged
        If dgvRecords.CurrentRow Is Nothing Then Return

        ' Assuming you have columns named exactly as in your table or you can use index
        txtSKU.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("SKU").Value)
        txtProductName.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Product_Name").Value)
        txtProductID.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Product_ID").Value)
        cmbCategory.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Category").Value) ' key
        txtUnitPrice.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Unit_Price").Value)
        txtCurrentStock.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Current_Stock").Value)
        txtReorderlevel.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Reorder_level").Value)
        txtSupplier.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Supplier_ID").Value)
    End Sub
    Private Sub LoadFromFileToGridDirect()
        If String.IsNullOrEmpty(_dataFilePath) OrElse Not File.Exists(_dataFilePath) Then
            MessageBox.Show("No saved sales found.")
            Return
        End If

        Try
            dgvRecords.Rows.Clear()


            '  Read each line of the CSV
            For Each line As String In File.ReadAllLines(_dataFilePath)
                If String.IsNullOrWhiteSpace(line) Then Continue For

                '   Split on commas but handle the quotes
                Dim values As String() = ParseCsvLine(line)

                '    Expecting: SupplierName, Product, unitPrice, Quantity, Stage, SaleDate
                If values.Length >= 8 Then
                    Dim i As Integer = dgvRecords.Rows.Add()
                    Dim r As DataGridViewRow = dgvRecords.Rows(i)
                    r.Cells("Column1").Value = values(0)
                    r.Cells("Column2").Value = values(1)
                    r.Cells("Column3").Value = values(2)
                    r.Cells("Column4").Value = values(3)
                    r.Cells("Column5").Value = values(4)
                    r.Cells("Column6").Value = values(5)
                    r.Cells("Column7").Value = values(6)
                    r.Cells("Column8").Value = values(7)
                    'optional: Stage = values(4)
                End If
            Next
            dgvRecords.Refresh()

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try

    End Sub

    Private Function ParseCsvLine(line As String) As String()
        Dim result As New List(Of String)
        Dim current As New Text.StringBuilder()
        Dim inQuotes As Boolean = False

        For Each ch As Char In line
            If ch = """"c Then
                inQuotes = Not inQuotes
            ElseIf ch = ","c AndAlso Not inQuotes Then
                result.Add(current.ToString())
                current.Clear()
            Else
                current.Append(ch)
            End If
        Next
        result.Add(current.ToString())
        Return result.ToArray()
    End Function

    Private Sub PopulateFormControl()
        txtSKU.Text = Sku
        txtProductName.Text = Product
        cmbCategory.Text = Category
        txtProductID.Text = ProductID
        txtUnitPrice.Text = UnitPrice
        txtCurrentStock.Text = CurrentStock
        txtReorderlevel.Text = Recorderlevel
        txtSupplier.Text = Suppliers
    End Sub
    'Private ReadOnly directoryPath As String = "C:\Temp"

    'Private ReadOnly filePath As String = Path.Combine("C:\Temp", "product.txt")

    Private Sub EnsureFolder()
        'If Not Directory.Exists(directoryPath) Then
        '    Directory.CreateDirectory(directoryPath)
        'End If

    End Sub

    Private Sub LoadProductCombo()

        'txtProductName.Text = ""

        'If File.Exists(filePath) Then
        '    Dim lines = File.ReadAllLines(filePath)

        '    For Each line In lines
        '        Dim name As String = line.Trim()
        '        If name <> "" Then

        '            txtProductName.Text.Trim(name)
        '        End If
        '    Next
        'End If

    End Sub



    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim sku As String = Module1.GenerateSku()
        txtSKU.Text = sku

        Dim supplierID As String = Module1.GenerateSupplierID()
        txtSupplier.Text = supplierID

        Dim ProductID As String = Module1.GenerateProduct_ID()
        txtProductID.Text = ProductID
        Try

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("INSERT INTO [Product_Details] ([SKU], [Product_Name], [Product_ID], [Category], [Unit_Price],[Current_Stock], [Reorder_Level], [Supplier_ID]) VALUES (?,?,?,?,?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("@SKU", txtSKU.Text)
                    cmd.Parameters.AddWithValue("@Product_Name", txtProductName.Text)
                    cmd.Parameters.AddWithValue("@Product_ID", txtProductID.Text)
                    cmd.Parameters.AddWithValue("@Category", cmbCategory.Text)
                    cmd.Parameters.AddWithValue("@Unit_Price", txtUnitPrice.Text)
                    cmd.Parameters.AddWithValue("@Current_Stock", txtCurrentStock.Text)
                    cmd.Parameters.AddWithValue("@Reorder_level", txtReorderlevel.Text)
                    cmd.Parameters.AddWithValue("@Supplier_ID", txtSupplier.Text)

                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()
                LoadData()
                Dim dash = TryCast(Me.Owner, FrmInventory_Dashboard)
                If dash IsNot Nothing Then
                    dash.LoadInventoryTotalsFromFile() ' This requires making LoadTotalsFromFile public in Form1
                    'dash.RefreshInventoryLabels()
                End If

            End Using

        Catch ex As Exception
            MessageBox.Show("Sale saved to : " & "Saved")

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

                Dim sql As String = "SELECT * FROM [Product_Details]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                dgvRecords.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub
    Private Sub AppendCustomerToFile(name As String)
        ''Use quoting only if you expect commas; keeping it basic here: one name per line

        'Using writer As New StreamWriter(filePath, True)
        '    writer.WriteLine(name)
        'End Using
    End Sub

    Private Sub btnEditProduct_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            ' Edit functionality - Update the selected record in the file
            If dgvRecords.SelectedRows.Count > 0 Then
                ' Validate the form inputs first
                '  If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
                '  MessageBox.Show("Please fill in Suppliers Name, contact, Email, and Address.", "Missing Information")
                ' Exit Sub
                'End If

                '   Dim Contact As Decimal
                '  If Not Decimal.TryParse(TextBox2.Text, Contact) Then
                '  MessageBox.Show("Please enter a valid numeric value for Contact.", "Invalid Input")
                ' Exit Sub
                'End If

                ' If Contact <= 0D Then
                '  MessageBox.Show("Amount must be greater than 0.", "Invalid Amount")
                'Exit Sub
                '  End If

                ' Get the selected row index
                Dim selectedIndex As Integer = dgvRecords.SelectedRows(0).Index

                ' CSV file path
                Dim directoryPath As String = "C:\Temp\Inventory\Product_management"
                Dim filePath As String = System.IO.Path.Combine(directoryPath, "product_management.csv")

                If Not System.IO.File.Exists(filePath) Then
                    MessageBox.Show("CSV file does not exist.", "File Error")
                    Exit Sub
                End If

                Dim lines As New List(Of String)

                Using reader As New System.IO.StreamReader(filePath)
                    ' Read header
                    If Not reader.EndOfStream Then lines.Add(reader.ReadLine())
                    ' Read all data lines
                    While Not reader.EndOfStream
                        lines.Add(reader.ReadLine())
                    End While
                End Using

                ' Update the selected line (add 1 to account for header)
                If selectedIndex < lines.Count - 1 Then
                    Dim updatedLine As String = txtSKU.Text.Trim() & "," &
                          txtProductName.Text.Trim() & "," &
                          cmbCategory.Text.Trim() & "," &
                          txtProductID.Text.Trim() & "," &
                          txtUnitPrice.Text.Trim() & "," &
                                txtCurrentStock.Text.Trim() & "," &
                                        txtReorderlevel.Text.Trim() & "," &
                          txtSupplier.Text.Trim()

                    lines(selectedIndex + 1) = updatedLine

                    'LoadInventoryData()

                    Dim dash = TryCast(Me.Owner, FrmInventory_Dashboard)
                    If dash IsNot Nothing Then
                        dash.LoadInventoryTotalsFromFile()
                        'dash.RefreshInventoryLabels()
                    End If
                    Dim h = TryCast(Me.Owner, Create_Invoice)
                    If h IsNot Nothing Then

                    End If

                    ' Write all lines back to the file
                    Try
                        System.IO.File.WriteAllLines(filePath, lines)
                        MessageBox.Show("Record updated successfully.", "Update Complete")
                        'LoadInventoryData() ' Refresh the DataGridView
                    Catch ex As Exception
                        MessageBox.Show("File access error: " & ex.Message, "File Error")
                        Exit Sub
                    End Try
                Else
                    MessageBox.Show("Selected row index is out of range.", "Update Error")
                End If
            Else
                MessageBox.Show("No row selected.", "Selection Error")
            End If

        Catch ex As Exception
            MessageBox.Show("Error saving lead: " & ex.Message & Environment.NewLine & "StackTrace:" & Environment.NewLine & ex.StackTrace,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmProductMgt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadData()

        EnsureFolder()
        LoadProductCombo()

        cmbCategory.Items.AddRange(New String() {"MANUFACTURE"})


    End Sub

    Private Sub btnDeleteProduct_Click(sender As Object, e As EventArgs) Handles btnDeleteProduct.Click
        If dgvRecords.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a Product Management to delete.", "No Selection")
            Exit Sub
        End If

        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to delete this Product Management ?", "Confirm Delete", MessageBoxButtons.YesNo)
        If confirm = DialogResult.No Then Exit Sub

        Dim selectedIndex As Integer = dgvRecords.SelectedRows(0).Index
        Dim filePath As String = "C:\Temp\Inventory\Product_management\Product_management.csv"

        If Not File.Exists(filePath) Then
            MessageBox.Show("File not found.")
            Exit Sub
        End If

        Dim lines As List(Of String) = File.ReadAllLines(filePath).ToList()

        ' Skip header line (index 0)
        If selectedIndex + 1 < lines.Count Then
            lines.RemoveAt(selectedIndex + 1)
            File.WriteAllLines(filePath, lines)
            MessageBox.Show("Product Management deleted successfully.", "Deleted")
            LoadFromFileToGridDirect()
        Else
            MessageBox.Show("Could not delete record. Please try again.")
            txtProductName.Text = " "
            cmbCategory.Text = " "
            txtCurrentStock.Text = ""
            txtReorderlevel.Text = " "
            txtSKU.Text = " "
            txtSupplier.Text = " "
            txtProductID.Text = " "
            txtUnitPrice.Text = " "
        End If


    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub btnAddProduct_Click(sender As Object, e As EventArgs) Handles btnAddProduct.Click
        Product = InputBox(ProdPrompt)
        txtProductName.Text = Product

        UnitPrice = InputBox(UnitPrompt)
        txtUnitPrice.Text = UnitPrice

        CurrentStock = InputBox(CurrentPrompt)
        txtCurrentStock.Text = CurrentStock

        Recorderlevel = InputBox(ReorderPrompt)
        txtReorderlevel.Text = Recorderlevel

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

    End Sub

    Private Sub txtProductName_TextChanged(sender As Object, e As EventArgs) Handles txtProductName.TextChanged

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using ofd As New OpenFileDialog()
            ofd.Title = "Select an image"
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*"
            ofd.CheckFileExists = True
            ofd.CheckPathExists = True

            If ofd.ShowDialog() = DialogResult.OK Then
                Try
                    ' Load the selected image
                    Dim img As Image = Image.FromFile(ofd.FileName)

                    ' Dispose previous image to avoid memory leaks
                    If PictureBox1.Image IsNot Nothing Then
                        PictureBox1.Image.Dispose()
                        PictureBox1.Image = Nothing
                    End If

                    ' Assign a copy to avoid locking the original file
                    PictureBox1.Image = New Bitmap(img)
                    PictureBox1.SizeMode = PictureBoxSizeMode.Zoom

                    ' Clean up
                    img.Dispose()
                Catch ex As Exception
                    MessageBox.Show($"Unable to load image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class