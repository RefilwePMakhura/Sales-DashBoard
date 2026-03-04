Imports System.IO


Public Class Supplier_Management
    Private ReadOnly _dataFilePath As String = "C:\Temp\Supplier_Management.csv"

    Dim suppliername As String
    Dim contact As String
    Dim phone As String
    Dim Email As String
    Dim Address As String
    Sub SaveToFile()
        Dim directorypath As String = "C:\Temp\Inventory"
        Dim filepath As String = System.IO.Path.Combine(directorypath, "Suppliers_data.txt")

        'Ensure directory exists
        If Not System.IO.Directory.Exists(directorypath) Then
            System.IO.Directory.CreateDirectory(directorypath)
        End If
        'Write all relevant variables to the file
        Using writer As New System.IO.StreamWriter(filepath, False) ' False to overview
            writer.WriteLine("Suppliername=" & suppliername)
            writer.WriteLine("Contact=" & contact)
            writer.WriteLine("Phone=" & phone)
            writer.WriteLine("Email=" & Email)
            writer.WriteLine("Address=" & Address)
        End Using
    End Sub
    Private Sub LoadFromFile()
        If Not System.IO.File.Exists("C:/Temp") Then
            Return
        End If
        Try
            For Each line As String In System.IO.File.ReadLines("C:/Temp")
                If String.IsNullOrWhiteSpace(line) Then Continue For

                Dim idx As Integer = line.IndexOf("=")
                If idx <= 0 Then Continue For

                Dim Key As String = line.Substring(0, idx).Trim()
                Dim value As String = line.Substring(idx + 1).Trim()

                Select Case Key
                    Case "SupplierName"
                        suppliername = value
                    Case "Contact"
                        contact = value
                    Case "Phone"
                        phone = value
                    Case "Email"
                        Email = value
                    Case "Address"
                        Address = value

                End Select

            Next
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub
    Private Sub Suppliers_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        IntializeDataGridView()
        LoadInventoryData()
    End Sub
    'Private Sub LoadFileToDataGridView(filePath As String)
    '    Dim dt As New DataTable()

    '    ' Read all lines from the file

    '    Dim lines() As String = File.ReadAllLines(filePath)
    '    If lines.Length > 0 Then
    '        ' First row = column headers
    '        Dim headers() As String = lines(0).Split(","c)
    '        For Each header As String In headers
    '            dt.Columns.Add(header)
    '        Next

    '        ' Add the rest of the rows
    '        For i As Integer = 1 To lines.Length - 1
    '            Dim data() As String = lines(i).Split(","c)

    '            ' --- FIX: make sure row length matches header length ---
    '            If data.Length > dt.Columns.Count Then
    '                ReDim Preserve data(dt.Columns.Count - 1)
    '            ElseIf data.Length < dt.Columns.Count Then
    '                Array.Resize(data, dt.Columns.Count)
    '            End If

    '            dt.Rows.Add(data)
    '        Next
    '    End If

    '    ' Bind data to DataGridView
    '    dgvRecords.DataSource = dt
    'End Sub
    Private Sub LoadInventoryData()
        ' Clear existing rows
        DataGridView.Rows.Clear()

        ' Path to sales data CSV file
        Dim directoryPath As String = "C:\Temp\Inventory"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "suppliers_data.csv")

        If Not System.IO.File.Exists(filePath) Then
            Exit Sub ' No data file yet
        End If

        ' Read CSV and populate DataGridView
        Using reader As New System.IO.StreamReader(filePath)
            ' Skip header line
            If Not reader.EndOfStream Then
                reader.ReadLine()
            End If

            ' Read each data line
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()

                ' Parse the CSV line
                Dim parts As List(Of String) = SplitCsvLine(line)

                If parts.Count >= 4 Then
                    ' Add row to DataGridView
                    DataGridView.Rows.Add(parts(0), parts(1), parts(2), parts(3), parts(4))
                End If
            End While
        End Using
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

    Public Sub IntializeDataGridView()
        ' Clear existing columns
        DataGridView.Columns.Clear()

        ' Define columns
        DataGridView.Columns.Add("Column1", "Suppliername")
        DataGridView.Columns.Add("Column2", "Contact")
        DataGridView.Columns.Add("Column3", "Phone")
        DataGridView.Columns.Add("Column4", "Email")
        DataGridView.Columns.Add("Column5", "Address")


        ' Configure DataGridView properties
        DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView.ReadOnly = True
        DataGridView.AllowUserToAddRows = False
        DataGridView.MultiSelect = False
        DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub dgvRecords_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView.SelectionChanged

        ' Check if a row is selected
        If DataGridView.SelectedRows.Count > 0 Then

        End If
        Dim selectedRow = DataGridView.SelectedRows(0)

        ' Get values from selected row
        Dim suppliername As String = selectedRow.Cells("Column1").Value.ToString()
        Dim Contact As String = selectedRow.Cells("Column2").Value.ToString()
        Dim Phone As String = selectedRow.Cells("Column3").Value.ToString()
        Dim Email As String = selectedRow.Cells("Column4").Value.ToString()
        Dim Address As String = selectedRow.Cells("Column5").Value.ToString()


        '' Remove currency symbol and format from amount if present
        'Dim amountDecimal As Decimal = 0
        'If Decimal.TryParse(UnitPr.Replace("$", "").Replace("R", "").Trim(), amountDecimal) Then
        '    UnitPrice = amountDecimal.ToString()
        'End If

        ' Populate controls with selected row data
        txtSupplierName.Text = suppliername
        txtContact.Text = Contact
        txtPhone.Text = Phone
        txtEmail.Text = Email
        txtAddress.Text = Address


        '' Set stage in combo box
        'For i As Integer = 0 To cboStage.Items.Count - 1
        '    If cboStage.Items(i).ToString().Equals(stage, StringComparison.OrdinalIgnoreCase) Then
        '        cboStage.SelectedIndex = i
        '        Exit For
        '    End If
        'Next

        ' Set date
        '    Try
        '        dtpSaleDate.Value = DateTime.Parse(saleDate)
        '    Catch ex As Exception
        '        ' Default to today if date parsing fails
        '        dtpSaleDate.Value = Date.Today
        '    End Try
        'End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        SaveToFile()
        Dim directoryPath As String = "C:\Temp\Inventory"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "suppliers_data.csv")
        If Not System.IO.Directory.Exists(directoryPath) Then
            System.IO.Directory.CreateDirectory(directoryPath)

        End If

        'Append to CSV(Write header the first time

        Dim fileExists As Boolean = System.IO.File.Exists(filePath)

        Using writer As New System.IO.StreamWriter(filePath, True) 'True = append
            If Not fileExists Then
                writer.WriteLine("Suppliername,contact, phone, Email, Address ")

            End If

            'Note for beginners: avoid commas in inputs for now(customer/product)
            'Or we'll add quoting in the next part when we improve the first format
            Dim line As String = txtSupplierName.Text.Trim() & "," &
                txtContact.Text.Trim() & "," &
                        txtPhone.Text.Trim() & "," &
                        txtEmail.Text.Trim() & "," &
                    txtAddress.Text.Trim() & ","

            writer.WriteLine(line)
        End Using

        If (suppliername & contact & phone & Email & Address) = Nothing Then

        Else


            LoadInventoryData()
        End If
        ''Dim filePath As String = "C:\Temp\Customer_Details.csv"
        'If File.Exists(filePath) Then
        '    LoadFileToDataGridView(filePath)
        'Else

        'End If




        'MessageBox.Show("File Not Found" & "FilePath")
        '' Console.WriteLine(ex.StackTrace)
        'MessageBox.Show("File Not Found: " & ex.Message & Environment.NewLine & "StackTrace:" & Environment.NewLine & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        MsgBox("sales have been added Succesfully!")


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            'LoadSalesData()
            ' Edit functionality - Update the selected record in the file
            If DataGridView.SelectedRows.Count > 0 Then
                ' Validate the form inputs first
                If txtSupplierName.Text = "" Or txtContact.Text = "" Or txtEmail.Text = "" Or txtAddress.Text = "" Then
                    MessageBox.Show("Please fill in Suppliers Name, contact, Email, and Address.", "Missing Information")
                    Exit Sub
                End If

                Dim Contact As Decimal
                If Not Decimal.TryParse(txtContact.Text, Contact) Then
                    MessageBox.Show("Please enter a valid numeric value for Contact.", "Invalid Input")
                    Exit Sub
                End If

                If Contact <= 0D Then
                    MessageBox.Show("Amount must be greater than 0.", "Invalid Amount")
                    Exit Sub
                End If

                ' Get the selected row index
                Dim selectedIndex As Integer = DataGridView.SelectedRows(0).Index

                ' Read all lines from the CSV file
                Dim directoryPath As String = "C:\Temp\Inventory\suppliers_management"
                Dim filePath As String = System.IO.Path.Combine(directoryPath, "suppliers_management.csv")
                Dim lines As List(Of String) = New List(Of String)

                Using reader As New System.IO.StreamReader(filePath)
                    ' Read header
                    If Not reader.EndOfStream Then
                        lines.Add(reader.ReadLine())
                    End If

                    ' Read all data lines
                    While Not reader.EndOfStream
                        lines.Add(reader.ReadLine())
                    End While
                End Using

                ' Update the selected line (add 1 to account for header)
                If selectedIndex < lines.Count - 1 Then
                    ' Create updated line
                    Dim updatedLine As String = txtSupplierName.Text.Trim() & "," &
                        txtContact.Text.Trim() & "," &
                         txtPhone.Text.Trim() & "," &
                        txtEmail.Text.Trim() & "," &
                    txtAddress.Text.Trim() & ","



                    lines(selectedIndex + 1) = updatedLine

                    ' Write all lines back to the file
                    Try
                        System.IO.File.WriteAllLines(filePath, lines)
                    Catch ex As Exception
                        MessageBox.Show("File access error: " & ex.Message, "File Error")
                        Exit Sub
                    End Try

                    ' Refresh the data
                    LoadInventoryData()
                End If

            End If

        Catch ex As Exception
            MessageBox.Show("Error saving lead: " & ex.Message & Environment.NewLine & "StackTrace:" & Environment.NewLine & ex.StackTrace,
           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MessageBox.Show("Record updated successfully.", "Update Complete")
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        LoadInventoryData()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub
End Class