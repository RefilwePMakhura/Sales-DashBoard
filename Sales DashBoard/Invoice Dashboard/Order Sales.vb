Imports System.Data.OleDb

Public Class Order_Sales

    Private ordersubtotal As Decimal = 0D
    Private orderVAT As Decimal = 0D
    Private orderDiscount As Decimal = 0D
    Private orderTotal As Decimal = 0D

    Private Sub Order_Sales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
        LoadCustomers
        LoadProducts
        GenerateInvoiceID()


        TextBox6.Text = "0.00"
        TextBox8.Text = "0.00"
        TextBox4.Text = "0.00"
        TextBox5.Text = "0.00"
        TextBox9.Text = "1"
    End Sub
    Private Sub SetupGrid()
        DataGridView1.Columns.Clear()
        DataGridView1.Rows.Clear()
        DataGridView1.AllowUserToAddRows = False

        DataGridView1.Columns.Add("ProductName", "Product")
        DataGridView1.Columns.Add("Quantity", "Quantity")
        DataGridView1.Columns.Add("UnitPrice", "UnitPrice")
        DataGridView1.Columns.Add("Amount", "Amount")
        DataGridView1.Columns.Add("VAT", "VAT")
        DataGridView1.Columns.Add("Discount", "Discount")
        DataGridView1.Columns.Add("Total", "Total")
    End Sub
    Private Sub LoadCustomers()
        Try
            ComboBox2.Items.Clear()

            Using conn = Getconnection
                conn.open
                Dim sql As String = "SELECT Customer FROM Customer_Details"
                Using cmd As New OleDbCommand(sql, conn)
                    Using dr = cmd.ExecuteReader
                        While dr.Read
                            ComboBox2.Items.Add(SafeStr(dr("Customer")))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading customer:" & ex.Message)
        End Try
    End Sub
    Private Sub LoadProducts()
        Try
            ComboBox1.Items.Clear()

            Using conn = Getconnection
                conn.open
                Dim sql As String = "SELECT Product_Name FROM Product_Details"
                Using cmd As New OleDbCommand(sql, conn)
                    Using dr = cmd.ExecuteReader
                        While dr.Read
                            ComboBox1.Items.Add(SafeStr(dr("Product_Details")))
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading products:" & ex.Message)
        End Try
    End Sub

    Private Sub GenerateInvoiceID()
        TextBox7.Text = "SO-" & DateTime.Now.ToString("yyMMddHHmmss")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            Using conn = Getconnection
                conn.open

                Dim sql As String = "SELECT Unit_Price FROM Product_Details WHERE Product_Name=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@p1", ComboBox1.Text.Trim())
                    Dim result = cmd.ExecuteScalar
                    If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                        TextBox5.Text = CDec(result).ToString("0.00")
                    Else
                        TextBox5.Text = "0.00"
                    End If
                End Using
            End Using

            calculateLine

        Catch ex As Exception
            MessageBox.Show("Error loading price" & ex.Message)
        End Try
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        calculateLine
    End Sub
    Private Sub calculateLine()
        Dim qty As Integer = SafeInt(TextBox9.Text)
        Dim Price As Integer = SafeInt(TextBox5.Text)
        TextBox4.Text = (qty * Price).ToString("0.00")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If ComboBox1.Text.Trim() - "" Then
                MessageBox.Show("Select Product")
                Exit Sub
            End If

            Dim product As String = ComboBox1.Text.Trim()
            Dim qty As Integer = SafeInt(TextBox9.Text)
            Dim UnitPrice As Decimal = SafeDec(TextBox5.Text)
        Catch ex As Exception

        End Try
    End Sub
End Class