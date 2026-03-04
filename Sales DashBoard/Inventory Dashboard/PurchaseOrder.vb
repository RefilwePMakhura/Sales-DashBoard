Imports System.IO
Public Class PurchaseOrder

    Private ReadOnly dataFilePath As String = Path.Combine("C:\Temp\purchase_orders", "purchase_orders.csv")

    Private Sub PurchaseOrderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Try
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath))

                ' Setup DataGridView columns
                With DataGridView1
                    .Columns.Clear()
                    .Columns.Add("PurchaseOrderID", "PO ID")
                    .Columns.Add("SupplierID", "Supplier ID")
                    .Columns.Add("OrderDate", "Order Date")
                    .Columns.Add("ExpectedDate", "Expected Date")
                    .Columns.Add("TotalValue", "Total Value (R)")
                    .Columns.Add("POStatus", "PO Status")
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    .AllowUserToAddRows = False
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    .MultiSelect = False
                End With

                LoadPurchaseOrdersFromFile()

            Catch ex As Exception
                MessageBox.Show("Error loading form: " & ex.Message)
            End Try
        End Sub

        ' -------------------------
        ' Load CSV into DataGridView
        ' -------------------------
        Private Sub LoadPurchaseOrdersFromFile()
            If Not File.Exists(dataFilePath) Then Return

            Try
                DataGridView1.Rows.Clear()
                Dim allLines = File.ReadAllLines(dataFilePath)

                ' Skip header
                For i As Integer = 1 To allLines.Length - 1
                    Dim line = allLines(i).Trim()
                    If String.IsNullOrWhiteSpace(line) Then Continue For
                    Dim values As String() = line.Split(","c)
                    If values.Length >= 6 Then
                        DataGridView1.Rows.Add(values(0).Trim(), values(1).Trim(), values(2).Trim(), values(3).Trim(), values(4).Trim(), values(5).Trim())
                    End If
                Next

            Catch ex As Exception
                MessageBox.Show("Error loading purchase orders: " & ex.Message)
            End Try
        End Sub

        ' -------------------------
        ' Save all DataGridView to CSV
        ' -------------------------
        Private Sub SavePurchaseOrdersToFile()
            Try
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath))
                Using writer As New StreamWriter(dataFilePath, False)
                    writer.WriteLine("PurchaseOrderID,SupplierID,OrderDate,ExpectedDate,TotalValue,POStatus")
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        If Not row.IsNewRow Then
                            Dim values As String() = {
                            row.Cells("PurchaseOrderID").Value.ToString().Replace(",", ";"),
                            row.Cells("SupplierID").Value.ToString().Replace(",", ";"),
                            row.Cells("OrderDate").Value.ToString(),
                            row.Cells("ExpectedDate").Value.ToString(),
                            row.Cells("TotalValue").Value.ToString(),
                            row.Cells("POStatus").Value.ToString()
                        }
                            writer.WriteLine(String.Join(",", values))
                        End If
                    Next
                End Using
            Catch ex As Exception
                MessageBox.Show("Error saving purchase orders: " & ex.Message)
            End Try
        End Sub

        ' -------------------------
        ' Add Purchase Button
        ' -------------------------
        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
            Try
                ' Validation
                If String.IsNullOrWhiteSpace(txtPurchaseOrderID.Text) Or String.IsNullOrWhiteSpace(txtSupplierID.Text) Then
                    MessageBox.Show("PO ID and Supplier ID are required.", "Missing Fields")
                    Exit Sub
                End If

                Dim totalValue As Decimal
                If Not Decimal.TryParse(txtTotalValue.Text, totalValue) OrElse totalValue < 0 Then
                    MessageBox.Show("Total Value must be a positive number.", "Invalid Input")
                    Exit Sub
                End If

                Dim poStatus As String = txtPOStatus.Text.Trim()
                If poStatus = "" Then poStatus = "Pending"

                ' Add to DataGridView
                DataGridView1.Rows.Add(txtPurchaseOrderID.Text.Trim(), txtSupplierID.Text.Trim(), DateTimePicker1.Value.ToShortDateString(), DateTimePicker2.Value.ToShortDateString(), totalValue.ToString("0.00"), poStatus)

                ' Save CSV
                SavePurchaseOrdersToFile()

                MessageBox.Show("Purchase Order added successfully!", "Success")

                ' Clear fields
                txtPurchaseOrderID.Text = ""
                txtSupplierID.Text = ""
                txtTotalValue.Text = ""
                txtPOStatus.Text = ""
                DateTimePicker1.Value = DateTime.Now
                DateTimePicker2.Value = DateTime.Now

            Catch ex As Exception
                MessageBox.Show("Error adding purchase order: " & ex.Message)
            End Try
        End Sub

        ' -------------------------
        ' Remove Selected PO
        ' -------------------------
        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
            Try
                If DataGridView1.SelectedRows.Count = 0 Then
                    MessageBox.Show("Please select a purchase order to remove.", "No Selection")
                    Exit Sub
                End If

                Dim result As DialogResult = MessageBox.Show("Are you sure you want to remove the selected purchase order?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result <> DialogResult.Yes Then Exit Sub

                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
                DataGridView1.Rows.Remove(selectedRow)

                SavePurchaseOrdersToFile()
                MessageBox.Show("Purchase order removed successfully.", "Removed")

            Catch ex As Exception
                MessageBox.Show("Error removing purchase order: " & ex.Message)
            End Try
        End Sub

        ' -------------------------
        ' Save PO Button
        ' -------------------------
        Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
            SavePurchaseOrdersToFile()
            MessageBox.Show("All purchase orders saved.", "Saved")
        End Sub

        ' -------------------------
        ' Receive PO Button
        ' -------------------------
        Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
            If DataGridView1.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a purchase order to mark as received.", "No Selection")
                Exit Sub
            End If

            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            selectedRow.Cells("POStatus").Value = "Received"

            SavePurchaseOrdersToFile()
            MessageBox.Show("Purchase order marked as Received.", "Updated")
        End Sub

        ' -------------------------
        ' Auto-fill textboxes on row selection
        ' -------------------------
        Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
            If DataGridView1.SelectedRows.Count = 0 Then Exit Sub
            Dim row As DataGridViewRow = DataGridView1.SelectedRows(0)
            txtPurchaseOrderID.Text = row.Cells("PurchaseOrderID").Value.ToString()
            txtSupplierID.Text = row.Cells("SupplierID").Value.ToString()
            txtTotalValue.Text = row.Cells("TotalValue").Value.ToString()
            txtPOStatus.Text = row.Cells("POStatus").Value.ToString()
            DateTimePicker1.Value = DateTime.Parse(row.Cells("OrderDate").Value.ToString())
            DateTimePicker2.Value = DateTime.Parse(row.Cells("ExpectedDate").Value.ToString())
        End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles txtTotalValue.TextChanged

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()
    End Sub

End Class
