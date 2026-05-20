Imports System.IO
Imports System.Data.OleDb

Public Class Login

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        Dim username As String = txtUserName.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If username = "" Or password = "" Then
            MessageBox.Show("Please enter username and password")
            Exit Sub
        End If

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String =
                "SELECT ID_Number, User_Name, First_Name, Role, IsActive
                 FROM Login_Details 
                 WHERE User_Name=? AND [Password]=?"

                Using cmd As New OleDbCommand(sql, conn)

                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = password

                    Dim dt As New DataTable()
                    Dim da As New OleDbDataAdapter(cmd)
                    da.Fill(dt)

                    If dt.Rows.Count > 0 Then

                        ' 🔒 Check if user is active
                        Dim isActive As Boolean = Convert.ToBoolean(dt.Rows(0)("IsActive"))

                        If isActive = False Then
                            MessageBox.Show("Your account is deactivated. Contact admin.")
                            Exit Sub
                        End If

                        ' ✅ Store session
                        Session.CurrentUserID = Convert.ToInt32(dt.Rows(0)("ID_Number"))
                        Session.CurrentUser = dt.Rows(0)("First_Name").ToString()
                        Session.CurrentRole = dt.Rows(0)("Role").ToString()

                        MessageBox.Show("Welcome " & Session.CurrentUser)

                        Dim home As New Form1
                        home.Show()
                        Me.Hide()

                    Else
                        MessageBox.Show("Invalid username or password")
                    End If

                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Login error: " & ex.Message)
        End Try

    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click
        Register.ShowDialog()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        About_Us.ShowDialog()
        'Try
        '    Dim filePath As String = "C:\Users\Refilwe\Documents\Screen Dump.docx"

        '    If Not File.Exists(filePath) Then
        '        MessageBox.Show("Guide file not found in Downloads." & vbCrLf & filePath,
        '                        "Guide",
        '                        MessageBoxButtons.OK,
        '                        MessageBoxIcon.Warning)
        '        Exit Sub
        '    End If

        '    Process.Start(filePath)

        'Catch ex As Exception
        '    MessageBox.Show("Error opening guide: " & ex.Message,
        '                    "Guide",
        '                    MessageBoxButtons.OK,
        '                    MessageBoxIcon.Error)
        'End Try
    End Sub



End Class
