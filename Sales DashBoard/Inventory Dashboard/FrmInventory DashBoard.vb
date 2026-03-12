Imports System.IO
Imports System.Globalization
Imports System.Data.OleDb

Public Class FrmInventory_Dashboard

    'Simple totals (Kept in memory while the app runs) 
    Public amtStockValue As Decimal = 0D
    Private Current_Stock As Integer = 0
    Private amtLowStock As Integer = 0
    Dim zarCulture





    Public Sub LoadInventoryTotalsFromFile()

        'amtStockValue = 0
        'totalSKU = 0
        'amtLowStock = 0

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand(
               "SELECT SUM(Current_Stock) FROM [Product_Details]", conn)
                    lblTotalSKUs.Text = If(IsDBNull(cmd.ExecuteScalar()), 0, cmd.ExecuteScalar()).ToString()
                End Using

                Using cmd As New OleDbCommand(
             "SELECT SUM(Current_Stock * Unit_Price) FROM [Product_Details]", conn)

                    Dim total As Decimal =
                        If(IsDBNull(cmd.ExecuteScalar()),
                        0D,
Convert.ToDecimal(cmd.ExecuteScalar()))

                    lblTotalStockValue.Text = total.ToString("c", zarCulture)

                End Using

                Using cmd As New OleDbCommand(
                    "SELECT COUNT (*) FROM Product_Details WHERE Current_Stock <= Recorder_Level", conn)
                    Dim result = cmd.ExecuteScalar()
                    If result Is Nothing Then
                        lblLowStockItems.Text = "0"
                    Else
                        lblLowStockItems.Text = result.ToString()
                    End If

                End Using
            End Using



        Catch ex As Exception
            MessageBox.Show("Failed to load totals!" & ex.Message)
            MessageBox.Show("Stack Trace:" & ex.StackTrace)
            Debug.WriteLine(ex.ToString)
        End Try


        'totalSKU += 0
        'amtLowStock += 1
        'amtStockValue += 0
        'lblTotalStockValue.Text = amtStockValue.ToString()
        'lblLowStockItems.Text = amtLowStock.ToString()
        'lblTotalSKUs.Text = totalSKU.ToString()
    End Sub
    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString)
    End Sub

    'Public Sub UpdateProductTotals(Optional showAlert As Boolean = True)
    '    Try
    '        Using conn As New OleDb.OleDbConnection(ConnectionString)
    '            conn.Open()

    '            Dim totalCurrent_Stock As Integer = 0
    '            Dim totalValue As Decimal = 0D
    '            Dim lowStockCount As Integer = 0
    '            Const LOW_STOCK_THRESHOLD As Integer = 50

    '            ' 1. Count total products
    '            Using cmd As New OleDb.OleDbCommand("SELECT COUNT(*) FROM Product_Details", conn)
    '                Current_Stock = CInt(cmd.ExecuteScalar())
    '            End Using

    '            ' 2. Calculate total stock value
    '            Using cmd As New OleDb.OleDbCommand("SELECT SUM(Unit_Price * Current_Stock) FROM Product_Details", conn)
    '                Dim result = cmd.ExecuteScalar()
    '                If Not IsDBNull(result) Then totalValue = CDec(result)
    '            End Using

    '            ' 3. Count low stock products
    '            Using cmd As New OleDb.OleDbCommand("SELECT COUNT(*) FROM Product_Details WHERE Current_Stock < ?", conn)
    '                cmd.Parameters.AddWithValue("?", LOW_STOCK_THRESHOLD)
    '                lowStockCount = CInt(cmd.ExecuteScalar())
    '            End Using

    '            ' 4. Update dashboard labels
    '            lblTotalSKUs.Text = totalCurrent_Stock.ToString()
    '            lblTotalStockValue.Text = totalValue.ToString("C", CultureInfo.CreateSpecificCulture("en-ZA"))
    '            lblLowStockItems.Text = lowStockCount.ToString()

    '            ' 5. Show alert if needed
    '            If showAlert AndAlso lowStockCount > 0 Then
    '                MessageBox.Show($"{lowStockCount} product(s) are low in stock (< {LOW_STOCK_THRESHOLD}).",
    '                            "Low Stock Alert",
    '                            MessageBoxButtons.OK,
    '                            MessageBoxIcon.Warning)
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        MessageBox.Show("Error updating dashboard totals: " & ex.Message)
    '    End Try
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

    'Public Sub RefreshInventoryLabels()
    '    ' Show amounts as South African Rand (ZAR)
    '    lblTotalStockValue.Text = amtStockValue.ToString("C", zarCulture)

    '    ' Show total number of saved sales in this session
    '    lblLowStockItems.Text = amtLowStock.ToString()
    '    lblTotalSKUs.Text = Current_Stock.ToString()
    'End Sub

    'Public Sub ApplyInventory(SKU As String, amount As Decimal)
    '    'Normalize stage and update totals 
    '    Select Case SKU.Trim().ToLower()
    '        Case "total sock value"
    '            amtStockValue += amount

    '    End Select
    '    amtLowStock += 1
    '    totalSKU += 1
    '    RefreshInventoryLabels()
    'End Sub

    Private Sub FrmInventoryDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventoryTotalsFromFile()
        '   RefreshInventoryLabels()
        'UpdateProductTotals(False)
    End Sub



    Private Sub btnPurchaseOrder_Click(sender As Object, e As EventArgs) Handles btnPurchaseOrder.Click
        Dim InventoryDashboardFrm As New Order_Form
        Order_Form.ShowDialog()
        ' Me.Hide() ' Hide instead of close if you want to reopen later
    End Sub

    Private Sub btnInventoryReport_Click(sender As Object, e As EventArgs) Handles btnInventoryReport.Click
        Dim InventoryDashboardFrm As New Inventory_Report
        Inventory_Report.Show()
        ' Me.Hide() ' Hide instead of close if you want to reopen later
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Form1.Show()
    End Sub

    'Private Sub InventoryDashboardFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    LoadInventoryTotalsFromFile()
    '    RefreshInventoryLabels()
    'End Sub

    Private Sub btnProducts_Click(sender As Object, e As EventArgs) Handles btnProducts.Click
        Dim InventoryDashboardFrm As New ProductMgtFrm
        ProductMgtFrm.Show()
        'Me.Hide() ' Hide instead of close if you want to reopen later
    End Sub

    Private Sub btnAdjustStock_Click(sender As Object, e As EventArgs) Handles btnAdjustStock.Click
        Dim InventoryDashboardFrm As New Stock_Adjustment
        Stock_Adjustment.Show()
        ' Me.Hide() ' Hide instead of close if you want to reopen later
    End Sub

    Private Sub btnSupplers_Click(sender As Object, e As EventArgs) Handles btnSupplers.Click
        Dim InventoryDashboardFrm As New Supplier
        Supplier.ShowDialog()
        '  Me.Hide() ' Hide instead of close if you want to reopen later
    End Sub

    Private Sub btnStockMovement_Click(sender As Object, e As EventArgs) Handles btnStockMovement.Click
        Dim FrmInventory_DashBoard As New Stock_Movement
        Stock_Movement.Show()
    End Sub

End Class
