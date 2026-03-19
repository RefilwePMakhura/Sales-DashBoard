Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Mail

Public Class Manage_Activity





    Public ActivityID As Integer = 0

        Private Sub Manage_Activity_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadActivityTypes()
            LoadCategories()
            LoadCategories1()
            LoadUsers()

            Label7.Text = "Logged in as: " & Session.CurrentUser
        End Sub
        Private Function GetUserEmail(userID As Integer) As String

            Dim email As String = ""

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                    "SELECT Address FROM Login_Details WHERE ID_Number=?", conn)

            cmd.Parameters.Add("?", OleDbType.Integer).Value = userID

            Dim result = cmd.ExecuteScalar()

            If result IsNot Nothing Then
                email = result.ToString()
            End If

        End Using

        Return email

        End Function

        Private Sub LoadUsers()

            Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim da As New OleDbDataAdapter(
    "SELECT ID_Number, First_Name FROM Login_Details", conn)

            da.Fill(dt)
        End Using

        ComboBox3.DataSource = dt
        ComboBox3.DisplayMember = "First_Name"
        ComboBox3.ValueMember = "ID_Number"
        ComboBox3.SelectedIndex = -1

        End Sub

        Private Sub LoadActivityTypes()

            Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim da As New OleDbDataAdapter(
                    "SELECT ActivityTypeID, [Names] FROM ActivityTypes", conn)

            da.Fill(dt)
        End Using

        ComboBox1.DataSource = dt
            ComboBox1.DisplayMember = "Names"
            ComboBox1.ValueMember = "ActivityTypeID"
            ComboBox1.SelectedIndex = -1

        End Sub

        Private Sub LoadCategories1()
            ComboBox4.Items.Clear()
            ComboBox4.Items.Add("Planned")
            ComboBox4.Items.Add("In Progress")
            ComboBox4.Items.Add("Completed")
            ComboBox4.Items.Add("Urgent")
            ComboBox4.SelectedIndex = 0
        End Sub

        Private Sub LoadCategories()
            ComboBox2.Items.Clear()
            ComboBox2.Items.Add("Low")
            ComboBox2.Items.Add("Medium")
            ComboBox2.Items.Add("High")
            ComboBox2.Items.Add("Urgent")
            ComboBox2.SelectedIndex = 0
        End Sub

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

            Try
                If TextBox1.Text = "" Then
                MessageBox.Show("Enter Name")
                Exit Sub
                End If

                Dim activityTypeID As Integer = Convert.ToInt32(ComboBox1.SelectedValue)
                Dim assignedUserID As Integer = Convert.ToInt32(ComboBox3.SelectedValue)

                Dim isUpdate As Boolean = ActivityID > 0
                Dim sql As String

                If isUpdate Then
                sql = "UPDATE Activities SET [Name]=?, [Description]=?, ActivityTypeID=?, StartDate=?, EndDate=?, DueDate=?, Priority=?, [Status]=?, AssignedToUserID=? WHERE ActivityID=?"
            Else
                sql = "INSERT INTO Activities ([Name],[Description],ActivityTypeID,StartDate,EndDate,DueDate,Priority,[Status],CreatedBy,AssignedToUserID,CreatedDate)
                       VALUES (?,?,?,?,?,?,?,?,?,?,?)"
            End If

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand(sql, conn)

                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = TextBox1.Text
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = TextBox2.Text
                    cmd.Parameters.Add("?", OleDbType.Integer).Value = activityTypeID
                    cmd.Parameters.Add("?", OleDbType.Date).Value = DateTimePicker1.Value
                    cmd.Parameters.Add("?", OleDbType.Date).Value = DateTimePicker2.Value
                    cmd.Parameters.Add("?", OleDbType.Date).Value = DateTimePicker3.Value
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = ComboBox2.Text
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = ComboBox4.Text

                    If isUpdate Then
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = assignedUserID
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = ActivityID
                    Else
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = assignedUserID
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now
                    End If

                    cmd.ExecuteNonQuery()

                End Using
            End Using

            MessageBox.Show("Activity saved successfully")


                If Not isUpdate Then

                    Dim userEmail As String = GetUserEmail(assignedUserID)

                    If userEmail <> "" Then

                        Dim emailSubject As String = "New Activity Assigned: " & TextBox1.Text

                        Dim emailBody As String =
                "Hello," & vbCrLf & vbCrLf &
                "You have been assigned a new activity." & vbCrLf &
                "Subject: " & TextBox1.Text & vbCrLf &
                "Priority: " & ComboBox2.Text & vbCrLf &
                "Due Date: " & DateTimePicker3.Value.ToShortDateString()

                        Dim sent As Boolean = SendEmailNotification(userEmail, emailSubject, emailBody)

                        If sent Then
                            MessageBox.Show("Activity saved and email sent successfully!")
                        Else
                            MessageBox.Show("Activity saved, but email failed to send.")
                        End If

                    Else
                        MessageBox.Show("Activity saved, but no email found for user.")
                    End If

                Else
                    MessageBox.Show("Activity updated successfully.")
                End If

                Me.Close()

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try

        End Sub
        Public Function SendEmailNotification(toEmail As String, subject As String, body As String) As Boolean

            Try
            Using smtp As New SmtpClient(My.Settings.SmptHost, My.Settings.SmptPort)

                smtp.EnableSsl = My.Settings.SmptSslEnable
                smtp.Credentials = New NetworkCredential(My.Settings.SmptUsername, My.Settings.SmptPassword)

                Using mail As New MailMessage()

                    mail.From = New MailAddress(My.Settings.SmptFrom)
                    mail.To.Add(toEmail)
                    mail.Subject = subject
                    mail.Body = body

                    smtp.Send(mail)

                End Using

            End Using

            Return True

            Catch ex As Exception
                MessageBox.Show("Email failed: " & ex.Message)
                Return False
            End Try

        End Function


    End Class