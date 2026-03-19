Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class Register

    ' 🔹 FORM LOAD
    Private Sub Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange({"Admin", "Staff", "Supervisor"})
        ComboBox1.SelectedIndex = 1 ' Default = Staff
    End Sub

    ' 🔹 REGISTER BUTTON
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ' ✅ VALIDATION
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox5.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox6.Text) OrElse
           ComboBox1.SelectedIndex = -1 Then

            MessageBox.Show("Please fill in all fields.")
            Exit Sub
        End If

        ' 🔹 Extra validation (recommended)
        If Not IsNumeric(TextBox3.Text) OrElse TextBox3.Text.Length < 6 Then
            MessageBox.Show("Enter a valid ID number.")
            Exit Sub
        End If

        If Not Regex.IsMatch(TextBox4.Text, "^\d{10}$") Then
            MessageBox.Show("Enter a valid 10-digit cellphone number.")
            Exit Sub
        End If

        Dim username As String = TextBox6.Text.Trim()

        ' 🔐 GENERATE PASSWORD
        Dim generatedPassword As String = GeneratePassword(TextBox2.Text, TextBox3.Text)

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                ' =========================
                ' CHECK IF USER EXISTS
                ' =========================
                Dim checkCmd As New OleDbCommand(
                    "SELECT COUNT(*) FROM Login_Details WHERE User_Name=?", conn)

                checkCmd.Parameters.AddWithValue("?", username)

                Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If exists > 0 Then
                    MessageBox.Show("Username already exists. Choose another.")
                    Exit Sub
                End If

                ' =========================
                ' INSERT USER
                ' =========================
                Dim insertCmd As New OleDbCommand(
                    "INSERT INTO Login_Details 
                    (First_Name, Surname, ID_Number, Contact, Address, User_Name, [Password], Role, IsActive)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)

                insertCmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())
                insertCmd.Parameters.AddWithValue("?", TextBox2.Text.Trim())
                insertCmd.Parameters.AddWithValue("?", TextBox3.Text.Trim())
                insertCmd.Parameters.AddWithValue("?", TextBox4.Text.Trim())
                insertCmd.Parameters.AddWithValue("?", TextBox5.Text.Trim())
                insertCmd.Parameters.AddWithValue("?", username)

                ' ⚠️ For real systems → hash password (see note below)
                insertCmd.Parameters.AddWithValue("?", generatedPassword)

                insertCmd.Parameters.AddWithValue("?", ComboBox1.Text)
                insertCmd.Parameters.AddWithValue("?", True)

                insertCmd.ExecuteNonQuery()
            End Using

            MessageBox.Show(
                "Registration Successful!" & vbCrLf &
                "Username: " & username & vbCrLf &
                "Password: " & generatedPassword)

            Dim login As New Login
            login.Show()
            Me.Hide()

        Catch ex As Exception
            MessageBox.Show("Registration error: " & ex.Message)
        End Try

    End Sub

    ' 🔹 PASSWORD GENERATOR FUNCTION
    Private Function GeneratePassword(name As String, id As String) As String
        Dim rnd As New Random()

        Dim part1 As String = name.Substring(0, Math.Min(3, name.Length))
        Dim part2 As String = id.Substring(Math.Max(0, id.Length - 3))
        Dim numbers As String = rnd.Next(10, 99).ToString()

        Return part1 & part2 & numbers
    End Function

End Class