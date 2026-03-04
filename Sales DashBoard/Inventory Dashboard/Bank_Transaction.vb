Imports System.Data.OleDb

Public Class Bank_Transaction
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("INSERT INTO [BankTransacton] ([BankAccountID], [TransactionID], [TransactionDate], [Amount], [Type], [ReferenceType], [ReferenceID], [Notes]) VALUES (?,?,?,?,?,?,?,?)", conn)

                    cmd.Parameters.AddWithValue("@BankAccountID", ComboBox2.Text)
                    cmd.Parameters.AddWithValue("@TransactionID", TextBox2.Text)
                    cmd.Parameters.AddWithValue("@TransactionDate", DateTimePicker1.Value.Date)
                    cmd.Parameters.AddWithValue("@Amount", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@Type", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("@ReferenceType", ComboBox3.Text)
                    cmd.Parameters.AddWithValue("@ReferenceID", TextBox5.Text)
                    cmd.Parameters.AddWithValue("@Notes", TextBox5.Text)
                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()

            End Using
            MessageBox.Show("Saved successful!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(
        $"Failed to save:{Environment.NewLine}{ex}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)


        End Try
    End Sub
End Class