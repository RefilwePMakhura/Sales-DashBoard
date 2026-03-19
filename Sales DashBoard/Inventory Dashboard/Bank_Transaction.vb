Imports System.Data.OleDb

Public Class Bank_Transaction

    Private Sub LoadData()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [BankTransaction]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadBankAccounts()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT BankAccountID, BankName FROM [BankAccount] WHERE IsActive= True", conn)
                Dim da As New OleDbDataAdapter(cmd)
                Dim dt As New DataTable
                da.Fill(dt)
                ComboBox1.DataSource = dt
                ComboBox1.DisplayMember = "BankName"
                ComboBox1.ValueMember = "BankAccountID"
                ComboBox1.SelectedIndex = -1

            End Using

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub LoadTransactions(Optional AccountNumber As Integer = -1)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim sql As String = "SELECT * FROM [BankTransaction]"
                If AccountNumber <> -1 Then
                    sql &= "WHERE AccountNumber=" & AccountNumber
                End If
                Dim da As New OleDbDataAdapter(sql, conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub UpdateClosingBalance(AccountNumber As Integer, amount As Decimal, type As String)
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim cmdSelect As New OleDbCommand("SELECT ClosingBalance FROM [BankAccount]WHERE BankAccountID=?", conn)
                cmdSelect.Parameters.AddWithValue("?", AccountNumber)
                Dim currentbalance As Decimal = Convert.ToDecimal(cmdSelect.ExecuteScalar())
                If type = "Deposit" Then
                    currentbalance += amount
                ElseIf type = "Withdrawal" Then
                    currentbalance -= amount
                End If
                Dim cmdUpdate As New OleDbCommand("UPDATE [BankAccount] SET ClosingBalance=? WHERE BankAccountID=?", conn)
                cmdUpdate.Parameters.AddWithValue("?", currentbalance)
                cmdUpdate.Parameters.AddWithValue("?", AccountNumber)
                cmdUpdate.ExecuteNonQuery()
                conn.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If ComboBox1.SelectedIndex = -1 Or ComboBox2.SelectedIndex = -1 Then
                MessageBox.Show("SELECT A BANK ACCOUNT")
                Exit Sub
            End If
            Dim amount As Decimal = Convert.ToDecimal(TextBox3.Text)
            Dim bankaccount As Integer = ComboBox2.SelectedValue

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("INSERT INTO [BankTransacton] ([BankAccount], [TransactionID], [TransactionDate], [Amount],[Type], [Reference], [ReferenceID]) VALUES (?,?,?,?,?,?,?,?)", conn)

                    cmd.Parameters.AddWithValue("@BankAccount", ComboBox2.Text)
                    cmd.Parameters.AddWithValue("@TransactionID", TextBox4.Text)
                    cmd.Parameters.AddWithValue("@TransactionDate", DateTimePicker1.Value.Date)
                    cmd.Parameters.AddWithValue("@Amount", TextBox3.Text)
                    cmd.Parameters.AddWithValue("@Type", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("@Reference", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@ReferenceID", TextBox5.Text)
                    cmd.ExecuteNonQuery()
                End Using

                conn.Close()

            End Using
            UpdateClosingBalance(bankaccount, amount, ComboBox2.SelectedItem.ToString())
            MessageBox.Show("Saved successful!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(
        $"Failed to save:{Environment.NewLine}{ex}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)


        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBox1.Text = row.Cells("TransactionID").Value.ToString()
            ComboBox2.SelectedValue = row.Cells("BankAccount").Value
            DateTimePicker1.Value = Convert.ToDateTime(row.Cells("TransactionDate").Value)
            TextBox3.Text = row.Cells("Amount").Value.ToString()
            ComboBox2.SelectedItem = row.Cells("Type").Value.ToString()
            TextBox3.Text = row.Cells("Reference").Value.ToString()
            TextBox4.Text = row.Cells("ReferenceID").Value.ToString()
        End If
    End Sub

    Private Sub Bank_Transaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.Items.AddRange(New String() {"Deposit", "Withdrawal"})
        LoadBankAccounts()
        LoadData()
    End Sub
End Class