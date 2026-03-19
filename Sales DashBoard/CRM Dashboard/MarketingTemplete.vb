Imports System.Data.OleDb
Imports System.IO
Imports System.Net
Imports System.Net.Mail

Public Class MarketingTemplete


    Public CampaignName As String
        Public CampaignType As String
        Public CampaignStatus As String



        Public Property SelectedLeadID As Integer = 0
        Public Property LeadName As String = ""
        Public Property Phone As String = ""
        Public Property EmailAddress As String = ""
        Public Property LeadSource As String = ""
        Public Property Stage As String = ""
        Public Property Status As String = ""
        Public Property DateCreated As Date = Date.Today

        Private Sub Lead_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Try
            RichTextBox1.ReadOnly = True
            Button7.Text = "Unlock"

            ' Load templates
            ComboBox1.Items.Clear()
                ComboBox1.Items.AddRange(New String() {
                "Email Marketing",
                "SMS Marketing",
                "Referral",
                "Promotion"
            })
                ComboBox1.SelectedIndex = 0
                LoadCategories()
                ' Load emails into checklist
                LoadEmails()

                ' Fill fields
                ' TextBox2.Text = LeadName

                ' Preview
                LoadTemplatePreview()

            Catch ex As Exception
                MessageBox.Show("Error loading form: " & ex.Message)
            End Try
        End Sub

        Private Sub LoadCategories()
            ComboBox2.Items.Clear()
            ComboBox2.Items.Add("All")
            ComboBox2.Items.Add("Food")
            ComboBox2.Items.Add("Drinks")
            ComboBox2.Items.Add("Snacks")

            ComboBox2.SelectedIndex = 0
        End Sub

        Private Sub LoadEmails()
            Try
                CheckedListBox1.Items.Clear()

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String = ""

                If ComboBox2.Text = "All" Then
                    query = "SELECT Email FROM Customer_Details WHERE Email IS NOT NULL"
                Else
                    query = "SELECT Email FROM Customer_Details WHERE Email IS NOT NULL AND Category = ?"
                End If

                Using cmd As New OleDbCommand(query, conn)

                    If ComboBox2.Text <> "All" Then
                        cmd.Parameters.AddWithValue("?", ComboBox2.Text)
                    End If

                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            CheckedListBox1.Items.Add(reader("Email").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
                MessageBox.Show("Error loading emails: " & ex.Message)
            End Try
        End Sub



        Private Sub SaveEmailHistory(emailTo As String, subject As String, body As String)
            Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String = "INSERT INTO EmailHistory (LeadName, EmailAddress, Subject, Body) VALUES (?, ?, ?, ?)"

                Using cmd As New OleDbCommand(query, conn)
                    cmd.Parameters.AddWithValue("?", LeadName)
                    cmd.Parameters.AddWithValue("?", emailTo)
                    cmd.Parameters.AddWithValue("?", subject)
                    cmd.Parameters.AddWithValue("?", body)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        Catch ex As Exception
                MessageBox.Show("Error saving email history: " & ex.Message)
            End Try
        End Sub


        Private Function GetTemplate(templateName As String) As String
            Select Case templateName

                Case "Email Marketing"
                Return "Subject: New Marketing" & vbCrLf & vbCrLf &
                       "Dear Sir/Madam" & vbCrLf & vbCrLf &
                       "" & CampaignName & vbCrLf & vbCrLf & "We happy to introduce our new offers designed especially for you with massive discount " & vbCrLf & "Stay tuned for more exclusive deals" & vbCrLf & vbCrLf & " best Regards" & vbCrLf & ProductName() & "AND " & "Team" & vbCrLf & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf &
                       "Regards," & vbCrLf & "CRM Department"

            Case "SMS Marketing"
                    Return "Subject: Socia Media Marketing" & vbCrLf & vbCrLf &
                       "Dear Sir/Madam" & vbCrLf & vbCrLf &
                       "New Special offer available " & "do not miss out on this special offer visit us today" & vbCrLf & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf & vbCrLf &
                       "Action:" & vbCrLf & "[Action]"

                Case "Referral"
                Return "Subject: Referral" & vbCrLf & vbCrLf &
                       "DearSir/Madam" & vbCrLf & vbCrLf &
                       "Invite your friends to exprience " & ProductName() & "And enjoy special referral rewards" & vbCrLf & vbCrLf


            Case "Promotion"
                    Return "Subject: Promotion" & vbCrLf & vbCrLf &
                       "Limited time promotion" & vbCrLf & "Take advantage of our exlusive offers" & vbCrLf


                Case Else
                    Return ""
            End Select
        End Function

        'Select Case CampaignType
        'Case "Email Marketing"
        '            RichTextBox2.Text = "Dear " & CampaignName & vbCrLf & vbCrLf & "We happy to introduce our new offers designed especially for you with massive discount " & vbCrLf & "Stay tuned for more exclusive deals" & vbCrLf & vbCrLf & " best Regards" & vbCrLf & Product() & "AND " & "Team"
        '        Case "SMS Marketing"
        '            RichTextBox2.Text = "New Special offer available " & "do not miss out on this special offer visit us today"
        '        Case "Referral"
        '            RichTextBox2.Text = "Invite your friends to exprience " & Product() & "And enjoy special referral rewards"
        '        Case "Promotion"
        '            RichTextBox2.Text = "Limited time promotion" & vbCrLf & "Take advantage of our exlusive offers"
        '        Case "Event"
        '            RichTextBox2.Text = "You are invited to our upcoming event " & vbCrLf & "Join us for more information"

        '    End Select

        Private Function BuildFinalReport() As String
            Dim template As String = GetTemplate(ComboBox1.Text)

            'template = template.Replace("[ManagerName]", TextBox2.Text.Trim())
            template = template.Replace("[LeadName]", LeadName)
            template = template.Replace("[Phone]", Phone)
            template = template.Replace("[Email]", EmailAddress)
            template = template.Replace("[LeadSource]", LeadSource)
            template = template.Replace("[Stage]", Stage)
            template = template.Replace("[Status]", Status)
            template = template.Replace("[DateCreated]", DateCreated.ToString("dd MMM yyyy"))
        template = template.Replace("[Action]", TextBox1.Text.Trim())

        Return template
        End Function

        Private Sub LoadTemplatePreview()
        RichTextBox1.Text = BuildFinalReport()
    End Sub


        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SendEmails()
    End Sub


        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
            LoadTemplatePreview()
        End Sub


        Private Sub SendEmails()
            Try
                If CheckedListBox1.CheckedItems.Count = 0 Then
                    MessageBox.Show("Select at least one recipient.")
                    Exit Sub
                End If

            Dim smtp As New SmtpClient(My.Settings.SmptHost, My.Settings.SmptPort)
            smtp.EnableSsl = My.Settings.SmptSslEnable
            smtp.Credentials = New NetworkCredential(My.Settings.SmptUsername, My.Settings.SmptPassword)

            Dim mail As New MailMessage()
            mail.From = New MailAddress(My.Settings.SmptFrom)

            Dim subject As String = "CRM Lead Report"
            Dim body As String = RichTextBox1.Text

            ' ADD MULTIPLE RECIPIENTS
            For Each item In CheckedListBox1.CheckedItems
                    ' 👉 Use BCC for privacy (recommended)
                    mail.Bcc.Add(item.ToString())

                    ' Save history
                    SaveEmailHistory(item.ToString(), subject, body)
                Next

                mail.Subject = subject
                mail.Body = body

                smtp.Send(mail)

                MessageBox.Show("Emails sent successfully!")

            Catch ex As Exception
                MessageBox.Show("Email error: " & ex.Message)
            End Try
        End Sub

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub ButtonSelectAll_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If ComboBox1.SelectedIndex = -1 Then
                MessageBox.Show("Select a template.")
                Exit Sub
            End If

            'If TextBox2.Text.Trim() = "" Then
            '    MessageBox.Show("Enter Manager Name.")
            '    Exit Sub
            'End If

            RichTextBox1.Text = BuildFinalReport()
            MessageBox.Show("Report generated.")

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub ButtonClearAll_Click(sender As Object, e As EventArgs) Handles Button6.Click

    End Sub

        Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
            LoadEmails()
        End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If RichTextBox1.ReadOnly Then
            RichTextBox1.ReadOnly = False
            Button7.Text = "Lock"
        Else
            RichTextBox1.ReadOnly = True
            Button7.Text = "Unlock"
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, False)
        Next
    End Sub
End Class