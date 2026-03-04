Imports System.IO
Public Class Stock_Movement
    Dim MovementDate As String
    Dim Product As String
    Dim MovementType As String
    Dim Quantity As String
    Dim UnitCost As String
    Dim RelatedDocumenet As String
    Dim Notes As String




    Private Sub LoadInventoryData()
        ' Clear existing rows
        DataGridView.Rows.Clear()

        ' Path to sales data CSV file
        Dim directoryPath As String = "C:\Temp\Inventory\stock_movement"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "stock_movement.csv")

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

                If parts.Count >= 8 Then
                    ' Add row to DataGridView
                    DataGridView.Rows.Add(parts(0), parts(2), parts(3), parts(4), parts(5), parts(6), parts(7))
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


    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView.SelectionChanged
        ' Check if a row is selected
        If DataGridView.SelectedRows.Count > 0 Then
            Dim selectedRow = DataGridView.SelectedRows(0)

            ' Get values from selected row
            Dim MovementDate As String = selectedRow.Cells("Column1").Value.ToString()
            Dim Product As String = selectedRow.Cells("Column2").Value.ToString()
            Dim MovementType As String = selectedRow.Cells("Column3").Value.ToString()
            Dim Quantity As String = selectedRow.Cells("Column4").Value.ToString()
            Dim UnitCost As String = selectedRow.Cells("Column5").Value.ToString()
            Dim RelatedDocumenet As String = selectedRow.Cells("Column6").Value.ToString()
            Dim Notes As String = selectedRow.Cells("Column7").Value.ToString()

            ' Populate controls with selected row data
            'dtpMovementDate.Text = MovementDate
            txtProduct.Text = Product
            txtMovementType.Text = MovementType
            txtQuantity.Text = Quantity
            txtUnitCost.Text = UnitCost
            txtRelatedDocument.Text = RelatedDocumenet
            txtNotes.Text = Notes
        End If
    End Sub


    Private Sub LoadFromFileToGridDirect()
        If String.IsNullOrEmpty(_dataFilePath) OrElse Not File.Exists(_dataFilePath) Then
            MessageBox.Show("No saved sales found.")
            Return
        End If

        Try
            DataGridView.Rows.Clear()


            '  Read each line of the CSV
            For Each line As String In File.ReadAllLines(_dataFilePath)
                If String.IsNullOrWhiteSpace(line) Then Continue For

                '   Split on commas but handle the quotes
                Dim values As String() = ParseCsvLine(line)

                '    Expecting: SupplierName, Product, unitPrice, Quantity, Stage, SaleDate
                If values.Length >= 8 Then
                    Dim i As Integer = DataGridView.Rows.Add()
                    Dim r As DataGridViewRow = DataGridView.Rows(i)
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
            DataGridView.Refresh()

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try

    End Sub



    Private ReadOnly _dataFilePath As String = "C:\Temp\Inventory\stock_movement.csv"



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
        dtpMovementDate.Text = MovementDate
        txtProduct.Text = Product
        txtMovementType.Text = MovementType
        txtQuantity.Text = Quantity
        txtUnitCost.Text = UnitCost
        txtRelatedDocument.Text = RelatedDocumenet
        txtNotes.Text = Notes
    End Sub



    Private Sub LoadAndPopulate()
        ' SaveToFile()
        PopulateFormControl()
    End Sub




    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim directoryPath As String = "C:\Temp\Inventory\stock_movement"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "stock_movement.csv")
        If Not System.IO.Directory.Exists(directoryPath) Then
            System.IO.Directory.CreateDirectory(directoryPath)

        End If
        'Append to CSV(Write header the first time
        Dim fileExists As Boolean = System.IO.File.Exists(filePath)
        Using writer As New System.IO.StreamWriter(filePath, True) 'True = append
            If Not fileExists Then
                writer.WriteLine("MovementDate, Product, MovementType, Quantity, UnitCost, RelatedDocument, Notes")

            End If

            'Note for beginners: avoid commas in inputs for now(customer/product)
            'Or we'll add quoting in the next part when we improve the first format
            Dim line As String = dtpMovementDate.Text.Trim() & "," &
                          txtProduct.Text.Trim() & "," &
                          txtMovementType.Text.Trim() & "," &
                          txtQuantity.Text.Trim() & "," &
                          txtUnitCost.Text.Trim() & "," &
                                txtRelatedDocument.Text.Trim() & "," &
                                        txtNotes.Text.Trim()

            writer.WriteLine(line)
        End Using
        MessageBox.Show("Sale saved to:" & filePath, "Save")


    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            ' Edit functionality - Update the selected record in the file
            If DataGridView.SelectedRows.Count > 0 Then
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
                Dim selectedIndex As Integer = DataGridView.SelectedRows(0).Index

                ' CSV file path
                Dim directoryPath As String = "C:\Temp\Inventory\stock_movement"
                Dim filePath As String = System.IO.Path.Combine(directoryPath, "stock_movement.csv")

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
                    Dim updatedLine As String = dtpMovementDate.Text.Trim() & "," &
                          txtProduct.Text.Trim() & "," &
                          txtMovementType.Text.Trim() & "," &
                          txtQuantity.Text.Trim() & "," &
                          txtUnitCost.Text.Trim() & "," &
                                txtRelatedDocument.Text.Trim() & "," &
                                        txtNotes.Text.Trim()

                    lines(selectedIndex + 1) = updatedLine

                    ' Write all lines back to the file
                    Try
                        System.IO.File.WriteAllLines(filePath, lines)
                        MessageBox.Show("Record updated successfully.", "Update Complete")
                        LoadInventoryData() ' Refresh the DataGridView
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

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView.CellContentClick
        ' Check if a row is selected
        If DataGridView.SelectedRows.Count > 0 Then
            Dim selectedRow = DataGridView.SelectedRows(0)

            ' Get values from selected row
            Dim MovementDate As String = selectedRow.Cells("Column1").Value.ToString()
            Dim Product As String = selectedRow.Cells("Column2").Value.ToString()
            Dim MovementType As String = selectedRow.Cells("Column3").Value.ToString()
            Dim Quantity As String = selectedRow.Cells("Column4").Value.ToString()
            Dim UnitCost As String = selectedRow.Cells("Column5").Value.ToString()
            Dim RelatedDocument As String = selectedRow.Cells("Column6").Value.ToString()
            Dim Notes As String = selectedRow.Cells("Column7").Value.ToString()

        End If
        ' Populate controls with selected row data
        dtpMovementDate.Text = MovementDate
        txtProduct.Text = Product
        txtMovementType.Text = MovementType
        txtQuantity.Text = Quantity
        txtUnitCost.Text = UnitCost
        txtRelatedDocument.Text = RelatedDocumenet
        txtNotes.Text = Notes

    End Sub

    Private Sub StockMovement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventoryData()
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Stock_Movement_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnAddStock_Click(sender As Object, e As EventArgs) Handles btnAddStock.Click

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class