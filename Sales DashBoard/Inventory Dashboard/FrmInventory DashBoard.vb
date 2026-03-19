Imports System.IO
Imports System.Globalization
Imports System.Data.OleDb
Imports System.Diagnostics

Public Class FrmInventory_Dashboard

    Public amtStockValue As Decimal = 0D
    Private Current_Stock As Integer = 0
    Private amtLowStock As Integer = 0
    Private zarCulture As CultureInfo = CultureInfo.CreateSpecificCulture("en-ZA")

    Public Sub LoadInventoryTotalsFromFile()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                'Total SKUs
                Using cmd As New OleDbCommand("SELECT COUNT(*) FROM Product_Details", conn)
                    lblTotalSKUs.Text = cmd.ExecuteScalar().ToString()
                End Using

                'Total Stock Value
                Using cmd As New OleDbCommand("SELECT SUM(Current_Stock * Unit_Price) FROM Product_Details", conn)
                    Dim result = cmd.ExecuteScalar()
                    Dim total As Decimal = If(result Is Nothing OrElse IsDBNull(result), 0D, Convert.ToDecimal(result))
                    lblTotalStockValue.Text = total.ToString("C", zarCulture)
                End Using

                'Low Stock Items
                Using cmd As New OleDbCommand("SELECT COUNT(*) FROM Product_Details WHERE Current_Stock <= Reorder_Level", conn)
                    Dim result = cmd.ExecuteScalar()
                    lblLowStockItems.Text = If(result Is Nothing OrElse IsDBNull(result), "0", result.ToString())
                End Using

            End Using

        Catch ex As Exception
            MessageBox.Show("Failed to load totals! " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Debug.WriteLine(ex.ToString())
        End Try

    End Sub

    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString())
    End Sub

    Private Function SplitCsvLine(line As String) As List(Of String)
        Dim result As New List(Of String)()
        Dim current As New System.Text.StringBuilder()
        Dim inQuotes As Boolean = False

        For i As Integer = 0 To line.Length - 1
            Dim ch As Char = line(i)

            If ch = """"c Then
                If inQuotes Then
                    Dim isEscaped As Boolean = (i + 1 < line.Length AndAlso line(i + 1) = """"c)
                    If isEscaped Then
                        current.Append(""""c)
                        i += 1
                    Else
                        inQuotes = False
                    End If
                Else
                    inQuotes = True
                End If
            ElseIf ch = ","c AndAlso Not inQuotes Then
                result.Add(current.ToString())
                current.Clear()
            Else
                current.Append(ch)
            End If
        Next

        result.Add(current.ToString())
        Return result
    End Function

    Private Sub FrmInventoryDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventoryTotalsFromFile()
    End Sub

    Private Sub btnPurchaseOrder_Click(sender As Object, e As EventArgs) Handles btnPurchaseOrder.Click
        Dim frm As New Order_Form
        frm.ShowDialog()
    End Sub

    Private Sub btnInventoryReport_Click(sender As Object, e As EventArgs) Handles btnInventoryReport.Click
        Dim frm As New Inventory_Report
        frm.Show()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub btnProducts_Click(sender As Object, e As EventArgs) Handles btnProducts.Click
        Dim frm As New ProductMgtFrm
        frm.Show()
    End Sub

    Private Sub btnAdjustStock_Click(sender As Object, e As EventArgs) Handles btnAdjustStock.Click
        Dim frm As New Stock_Adjustment
        frm.Show()
    End Sub

    Private Sub btnSupplers_Click(sender As Object, e As EventArgs) Handles btnSupplers.Click
        Dim frm As New Supplier
        frm.ShowDialog()
    End Sub

    Private Sub btnStockMovement_Click(sender As Object, e As EventArgs) Handles btnStockMovement.Click
        Dim frm As New Stock_Movement
        frm.Show()
    End Sub

End Class
