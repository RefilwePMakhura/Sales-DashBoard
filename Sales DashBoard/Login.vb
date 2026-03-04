Imports System.IO
Imports System.Data.OleDb

Public Class Login

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim UserName As String = txtUserName.Text
        Dim Password As String = txtPassword.Text



        If String.IsNullOrWhiteSpace(UserName) OrElse String.IsNullOrWhiteSpace(Password) Then

            MessageBox.Show("Please enter both username and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT COUNT(*)FROM Login_Details WHERE User_Name = ? AND Password =?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@p1", UserName)
                    cmd.Parameters.AddWithValue("@p2", Password)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    If count > 0 Then
                        MessageBox.Show("Login successful!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        Dim Sales As New Form1()
                        Form1.Show()
                        '  Me.Hide()
                    Else
                        MessageBox.Show("Invalid Credatials")
                        txtPassword.Clear()
                        txtUserName.Clear()
                    End If
                End Using

            End Using
        Catch ex As Exception
            MessageBox.Show($"Error during login:{Environment.NewLine}{ex.Message}", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click
        Register.ShowDialog()
    End Sub
End Class