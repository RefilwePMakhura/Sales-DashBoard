Imports System.IO
Imports System.Data.OleDb
Public Class Customer_Details


    Private Sub frmManageCustomer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadCustomerIntoList()
    End Sub

    Private Sub LoadCustomerIntoList()
        lstCustomers.Items.Clear()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT Customer_Name FROM [Customer_Details] ORDER BY Customer_Name"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader
                        While reader.Read()
                            lstCustomers.Items.Add(reader("Customer_Name").ToString)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error Loading: " & ex.Message, "database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub lstCustomers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCustomers.SelectedIndexChanged
        'LoadCustomerIntoList()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("INSERT INTO [Customer_Details] ([Customer_Name],[Address],[Contact],[Email]) VALUES (?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("@Customer", txtCustomerName.Text)
                    cmd.Parameters.AddWithValue("@Address", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@Contact", TextBox3.Text)
                    cmd.Parameters.AddWithValue("@Email", TextBox2.Text)
                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()
                LoadCustomerIntoList()
                'LoadCustomerCombo()
            End Using

        Catch ex As Exception
            '  MessageBox.Show("Sale saved to : " & "Saved")
        Finally
            MessageBox.Show("Customer added.", "Success")

        End Try
    End Sub

End Class