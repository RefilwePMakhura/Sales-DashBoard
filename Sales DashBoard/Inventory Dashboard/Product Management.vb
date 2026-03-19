Imports System.IO
Imports System.Globalization
Imports System.Data.OleDb
Imports System.Diagnostics

Public Class ProductMgtFrm

    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Private ReadOnly ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"

    Dim Sku As String
    Dim Product As String
    Dim Category As String
    Dim ProductID As String
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

        For Each row As DataGridViewRow In dgvRecords.Rows
            If row.IsNewRow Then Continue For

            Dim name As String = Convert.ToString(row.Cells("Product_Name").Value)
            Dim price As Decimal = 0D
            Decimal.TryParse(Convert.ToString(row.Cells("Unit_Price").Value), price)

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

        Dim fileName As String = Path.Combine(imageFolder, productName.Replace(" ", "") & ".png")
        PictureBox1.Image.Save(fileName, Imaging.ImageFormat.Png)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        FrmInventory_Dashboard.Show()
    End Sub

    Private Sub dgvRecords_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRecords.SelectionChanged
        If dgvRecords.CurrentRow Is Nothing Then Return
        If dgvRecords.CurrentRow.IsNewRow Then Return

        Try
            txtSKU.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("SKU").Value)
            txtProductName.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Product_Name").Value)
            txtProductID.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Product_ID").Value)
            cmbCategory.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Category").Value)
            txtUnitPrice.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Unit_Price").Value)
            txtCurrentStock.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Current_Stock").Value)
            txtReorderlevel.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Reorder_Level").Value)
            txtSupplier.Text = Convert.ToString(dgvRecords.CurrentRow.Cells("Supplier_ID").Value)
        Catch ex As Exception
            MessageBox.Show("Selection error: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Private Sub PopulateFormControl()
        txtSKU.Text = Sku
        txtProductName.Text = Product
        cmbCategory.Text = Category
        txtProductID.Text = ProductID
        txtUnitPrice.Text = UnitPrice.ToString("0.00")
        txtCurrentStock.Text = CurrentStock
        txtReorderlevel.Text = Recorderlevel
        txtSupplier.Text = Suppliers
    End Sub

    Private Sub EnsureFolder()
    End Sub

    Private Sub LoadProductCombo()
    End Sub

    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtProductName.Text) Then
            MessageBox.Show("Enter product name.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(cmbCategory.Text) Then
            MessageBox.Show("Select category.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtUnitPrice.Text) Then
            MessageBox.Show("Enter unit price.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtCurrentStock.Text) Then
            MessageBox.Show("Enter current stock.")
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtReorderlevel.Text) Then
            MessageBox.Show("Enter reorder level.")
            Return False
        End If

        Dim decValue As Decimal
        Dim intValue As Integer

        If Not Decimal.TryParse(txtUnitPrice.Text, decValue) Then
            MessageBox.Show("Invalid unit price.")
            Return False
        End If

        If Not Integer.TryParse(txtCurrentStock.Text, intValue) Then
            MessageBox.Show("Invalid current stock.")
            Return False
        End If

        If Not Integer.TryParse(txtReorderlevel.Text, intValue) Then
            MessageBox.Show("Invalid reorder level.")
            Return False
        End If

        Return True
    End Function

    Private Sub ClearFields()
        txtSKU.Clear()
        txtProductName.Clear()
        txtProductID.Clear()
        cmbCategory.SelectedIndex = -1
        txtUnitPrice.Clear()
        txtCurrentStock.Clear()
        txtReorderlevel.Clear()
        txtSupplier.Clear()
        PictureBox1.Image = Nothing
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not ValidateInputs() Then Exit Sub

        If String.IsNullOrWhiteSpace(txtSKU.Text) Then
            txtSKU.Text = Module1.GenerateSku()
        End If

        If String.IsNullOrWhiteSpace(txtSupplier.Text) Then
            txtSupplier.Text = Module1.GenerateSupplierID()
        End If

        If String.IsNullOrWhiteSpace(txtProductID.Text) Then
            txtProductID.Text = Module1.GenerateProduct_ID()
        End If

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand(
                    "INSERT INTO [Product_Details] " &
                    "([SKU], [Product_Name], [Product_ID], [Category], [Unit_Price], [Current_Stock], [Reorder_Level], [Supplier_ID]) " &
                    "VALUES (?,?,?,?,?,?,?,?)", conn)

                    cmd.Parameters.AddWithValue("?", txtSKU.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtProductName.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtProductID.Text.Trim())
                    cmd.Parameters.AddWithValue("?", cmbCategory.Text.Trim())
                    cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtUnitPrice.Text))
                    cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtCurrentStock.Text))
                    cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtReorderlevel.Text))
                    cmd.Parameters.AddWithValue("?", txtSupplier.Text.Trim())

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            SaveProductImage(txtProductName.Text)
            LoadData()

            Dim dash = TryCast(Me.Owner, FrmInventory_Dashboard)
            If dash IsNot Nothing Then
                dash.LoadInventoryTotalsFromFile()
            End If

            MessageBox.Show("Product saved successfully.")
            ClearFields()

        Catch ex As Exception
            MessageBox.Show("Failed to save product: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try

        dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRecords.ReadOnly = True
        dgvRecords.AllowUserToAddRows = False
        dgvRecords.MultiSelect = False
        dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub LoadData()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Product_Details] ORDER BY [Product_Name]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                dgvRecords.DataSource = table
            End Using

        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If dgvRecords.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to edit.")
            Exit Sub
        End If

        If Not ValidateInputs() Then Exit Sub

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand(
                    "UPDATE [Product_Details] SET " &
                    "[SKU]=?, [Product_Name]=?, [Category]=?, [Unit_Price]=?, [Current_Stock]=?, [Reorder_Level]=?, [Supplier_ID]=? " &
                    "WHERE [Product_ID]=?", conn)

                    cmd.Parameters.AddWithValue("?", txtSKU.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtProductName.Text.Trim())
                    cmd.Parameters.AddWithValue("?", cmbCategory.Text.Trim())
                    cmd.Parameters.AddWithValue("?", Convert.ToDecimal(txtUnitPrice.Text))
                    cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtCurrentStock.Text))
                    cmd.Parameters.AddWithValue("?", Convert.ToInt32(txtReorderlevel.Text))
                    cmd.Parameters.AddWithValue("?", txtSupplier.Text.Trim())
                    cmd.Parameters.AddWithValue("?", txtProductID.Text.Trim())

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        SaveProductImage(txtProductName.Text)
                        LoadData()

                        Dim dash = TryCast(Me.Owner, FrmInventory_Dashboard)
                        If dash IsNot Nothing Then
                            dash.LoadInventoryTotalsFromFile()
                        End If

                        MessageBox.Show("Product updated successfully.")
                    Else
                        MessageBox.Show("No matching product found to update.")
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error updating product: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Private Sub frmProductMgt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadData()
            EnsureFolder()
            LoadProductCombo()

            cmbCategory.Items.Clear()
            cmbCategory.Items.AddRange(New String() {"MANUFACTURE"})

        Catch ex As Exception
            MessageBox.Show("Form load error: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Private Sub btnDeleteProduct_Click(sender As Object, e As EventArgs) Handles btnDeleteProduct.Click
        If dgvRecords.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to delete.", "No Selection")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtProductID.Text) Then
            MessageBox.Show("Product ID is missing.")
            Exit Sub
        End If

        Dim confirm As DialogResult = MessageBox.Show(
            "Are you sure you want to delete this product?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If confirm = DialogResult.No Then Exit Sub

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("DELETE FROM [Product_Details] WHERE [Product_ID]=?", conn)
                    cmd.Parameters.AddWithValue("?", txtProductID.Text.Trim())

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Product deleted successfully.", "Deleted")
                        LoadData()
                        ClearFields()
                    Else
                        MessageBox.Show("Could not delete product. Record not found.")
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Delete failed: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint
    End Sub

    Private Sub btnAddProduct_Click(sender As Object, e As EventArgs) Handles btnAddProduct.Click
        Product = InputBox(ProdPrompt)
        txtProductName.Text = Product

        Dim priceInput As String = InputBox(UnitPrompt)
        Decimal.TryParse(priceInput, UnitPrice)
        txtUnitPrice.Text = UnitPrice.ToString("0.00")

        CurrentStock = InputBox(CurrentPrompt)
        txtCurrentStock.Text = CurrentStock

        Recorderlevel = InputBox(ReorderPrompt)
        txtReorderlevel.Text = Recorderlevel
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        LoadData()
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
                    Dim img As Image = Image.FromFile(ofd.FileName)

                    If PictureBox1.Image IsNot Nothing Then
                        PictureBox1.Image.Dispose()
                        PictureBox1.Image = Nothing
                    End If

                    PictureBox1.Image = New Bitmap(img)
                    PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
                    img.Dispose()

                Catch ex As Exception
                    MessageBox.Show("Unable to load image: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    Debug.WriteLine(ex.ToString())
                End Try
            End If
        End Using
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
    End Sub

End Class