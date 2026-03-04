Imports System.IO

Public Class Stock_Adjustment

        Dim Product As String
        Dim OldStock As String
        Dim NewStock As String
        Dim Reason As String

        Sub SaveToFile()
            Dim directorypath As String = "C:\Temp\Inventory"
            Dim filepath As String = System.IO.Path.Combine(directorypath, "StockAdjustment_data.txt")

            'Ensure directory exists
            If Not System.IO.Directory.Exists(directorypath) Then
                System.IO.Directory.CreateDirectory(directorypath)
            End If
            'Write all relevant variables to the file
            Using writer As New System.IO.StreamWriter(filepath, False) ' False to overview
                writer.WriteLine("Product=" & Product)
                writer.WriteLine("OldStock=" & OldStock)
                writer.WriteLine("NewStock=" & NewStock)
                writer.WriteLine("Reason=" & Reason)

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
                    Case "Product"
                        Product = value
                    Case "OldStock"
                        OldStock = value
                    Case "NewStock"
                        NewStock = value
                    Case "Reason"
                        Reason = value
                End Select

                Next
            Catch ex As Exception
                MessageBox.Show("Error loading data: " & ex.Message)
            End Try
        End Sub
        Private Sub Suppliers_Load(sender As Object, e As EventArgs) Handles MyBase.Load

            IntializeDataGridView()
            LoadSalesData()
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
    Private Sub LoadSalesData()
        ' Clear existing rows
        DataGridView.Rows.Clear()

        ' Path to sales data CSV file
        Dim directoryPath As String = "C:\Temp\Inventory"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "StockAdjustment_data.csv")

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
                    DataGridView.Rows.Add(parts(0), parts(1), parts(2), parts(3))
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
            DataGridView.Columns.Add("Product", "Product")
            DataGridView.Columns.Add("OldStock", "OldStock")
            DataGridView.Columns.Add("NewStock", "NewStock")
            DataGridView.Columns.Add("Reason", "Reason")



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
        Dim Product As String = selectedRow.Cells("Product").Value.ToString()
        Dim OldStock As String = selectedRow.Cells("OldStock").Value.ToString()
        Dim NewStock As String = selectedRow.Cells("NewStock").Value.ToString()
        Dim Reason As String = selectedRow.Cells("Reason").Value.ToString()


        '' Remove currency symbol and format from amount if present
        'Dim amountDecimal As Decimal = 0
        'If Decimal.TryParse(UnitPr.Replace("$", "").Replace("R", "").Trim(), amountDecimal) Then
        '    UnitPrice = amountDecimal.ToString()
        'End If

        ' Populate controls with selected row data
        TextBox1.Text = Product
        TextBox2.Text = OldStock
        NumericUpDown1.Text = NewStock
        TextBox3.Text = Reason



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



        Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            SaveToFile()
            Dim directoryPath As String = "C:\Temp\Inventory"
            Dim filePath As String = System.IO.Path.Combine(directoryPath, "StockAdjustment_data.csv")
            If Not System.IO.Directory.Exists(directoryPath) Then
                System.IO.Directory.CreateDirectory(directoryPath)

            End If

            'Append to CSV(Write header the first time

            Dim fileExists As Boolean = System.IO.File.Exists(filePath)

            Using writer As New System.IO.StreamWriter(filePath, True) 'True = append
                If Not fileExists Then
                    writer.WriteLine("Product,oldstoct, newstock, reason,")

                End If

            'Note for beginners: avoid commas in inputs for now(customer/product)
            'Or we'll add quoting in the next part when we improve the first format
            Dim line As String = TextBox1.Text.Trim() & "," &
                    TextBox2.Text.Trim() & "," &
                                                       NumericUpDown1.Text.Trim() & "," &
                     TextBox3.Text.Trim() & ","

            writer.WriteLine(line)
            End Using

            If (Product & OldStock & NewStock & Reason) = Nothing Then

            Else


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

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
            Try
                'LoadSalesData()
                ' Edit functionality - Update the selected record in the file
                If DataGridView.SelectedRows.Count > 0 Then
                ' Validate the form inputs first
                If TextBox1.Text = "" Or TextBox2.Text = "" Or NumericUpDown1.Text = "" Or TextBox3.Text = "" Then
                    MessageBox.Show("Please fill in Suppliers Name, contact, Email, and Address.", "Missing Information")
                    Exit Sub
                End If

                Dim Contact As Decimal
                If Not Decimal.TryParse(TextBox2.Text, Contact) Then
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
                    Dim directoryPath As String = "C:\Temp\Inventory\Stock_Adjustment"
                    Dim filePath As String = System.IO.Path.Combine(directoryPath, "Stock_Adjustment.csv")
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
                    Dim updatedLine As String = TextBox1.Text.Trim() & "," &
                    TextBox2.Text.Trim() & "," &
                  NumericUpDown1.Text.Trim() & "," &
                        TextBox3.Text.Trim() & ","




                    lines(selectedIndex + 1) = updatedLine

                        ' Write all lines back to the file
                        Try
                            System.IO.File.WriteAllLines(filePath, lines)
                        Catch ex As Exception
                            MessageBox.Show("File access error: " & ex.Message, "File Error")
                            Exit Sub
                        End Try

                        ' Refresh the data
                        LoadSalesData()
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show("Error saving lead: " & ex.Message & Environment.NewLine & "StackTrace:" & Environment.NewLine & ex.StackTrace,
               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                MessageBox.Show("Record updated successfully.", "Update Complete")
            End Try
        End Sub

        Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

        End Sub

        Private Sub Stock_Adjustment_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        End Sub

        Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
            LoadSalesData()
        End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        FrmInventory_Dashboard.Show()
    End Sub

End Class