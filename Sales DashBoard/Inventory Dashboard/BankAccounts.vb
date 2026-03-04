Imports System.Data.OleDb

Public Class BankAccounts
    Private Sub BankAccounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.AddRange(New String() {"USD", "ZAR", "EUR"})
        ComboBox1.SelectedIndex = 0
        LoadBankAccount()
        Dim ACCID As String = Module1.GenerateACCID
        TextBox1.Text = ACCID

        Dim refence As String = Module1.Generaterefence
        DataGridView1.Text = refence
    End Sub


    Public Sub LoadBankAccount()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [BankAccount]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()



                Using cmd As New OleDbCommand("INSERT INTO [BankAccount] ([BankAccountID], [BankName], [Reference], [OpeningBalance], [ClosingBalance], [IsActive], [Currency]) VALUES (?,?,?,?,?,?,?)", conn)

                    cmd.Parameters.AddWithValue("@BankAccountID", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@BankName", TextBox2.Text)
                    cmd.Parameters.AddWithValue("@Currency", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("@Reference", Generaterefence)
                    cmd.Parameters.AddWithValue("@OpeningBalance", TextBox5.Text)
                    cmd.Parameters.AddWithValue("@ClosingBalance", TextBox4.Text)
                    cmd.Parameters.AddWithValue("@IsActive", CheckBox2.Checked)
                    cmd.ExecuteNonQuery()
                End Using
                conn.Close()
                LoadBankAccount()

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