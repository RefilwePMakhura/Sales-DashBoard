Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Mail

Public Class Templete

    Public Property SelectedLeadID As Integer = 0
        Public Property LeadName As String = ""
        Public Property Phone As String = ""
        Public Property EmailAddress As String = ""
        Public Property LeadSource As String = ""
        Public Property Stage As String = ""
        Public Property Status As String = ""
        Public Property DateCreated As Date = Date.Today
        Private Sub Template_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = LeadName
        TextBox2.Text = EmailAddress
            Try
                ComboBox1.Items.Clear()
                ComboBox1.Items.AddRange(New String() {
                "New Lead Report",
                "Follow-Up Report",
                "High Priority Lead Report",
                "Lead Summary Report"
            })
            RichTextBox2.ReadOnly = True

            ComboBox2.Items.Clear()
                ComboBox2.Items.AddRange(New String() {"Low", "Medium", "High", "Urgent"})

                ComboBox1.SelectedIndex = 0
                ComboBox2.SelectedIndex = 1

                RichTextBox1.Clear()
                RichTextBox2.Clear()

                LoadTemplatePreview()

            Catch ex As Exception
                MessageBox.Show("Error loading report form: " & ex.Message)
            End Try
        End Sub

        Private Function GetTemplate(templateName As String) As String
            Select Case templateName

                Case "New Lead Report"
                Return "Subject: New Lead Report" & vbCrLf & vbCrLf &
                       "Dear [ManagerName]," & vbCrLf & vbCrLf &
                       "We noticed that you reached out to us on Instagram regarding our product." & vbCrLf & vbCrLf &
                       "Lead Name: [LeadName]" & vbCrLf &
                       "Phone Number: [Phone]" & vbCrLf &
                       "Email Address: [Email]" & vbCrLf &
                       "Lead Source: [LeadSource]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Status: [Status]" & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf &
                       "Urgency: [Urgency]" & vbCrLf & vbCrLf &
                       "Additional Notes:" & vbCrLf &
                       "[Notes]" & vbCrLf & vbCrLf &
                       "Recommended Action:" & vbCrLf &
                       "[Action]" & vbCrLf & vbCrLf &
                        "Thank you for your interest.One of our team members will be contacting you soon to assityou further." & vbCrLf & vbCrLf &
                       "Regards," & vbCrLf &
                       "CRM Department"

            Case "Follow-Up Report"
                Return "Subject: Lead Follow-Up Report" & vbCrLf & vbCrLf &
                       "Dear [ManagerName]," & vbCrLf & vbCrLf &
                                           "We are following up regarding the product you showed interest in. We would like to know the quantity you would like so we can assist you further." & vbCrLf & vbCrLf &
                                          "Lead Name: [LeadName]" & vbCrLf &
                       "Phone Number: [Phone]" & vbCrLf &
                       "Email Address: [Email]" & vbCrLf &
                       "Lead Source: [LeadSource]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Status: [Status]" & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf &
                       "Urgency: [Urgency]" & vbCrLf & vbCrLf &
                       "Additional Notes:" & vbCrLf &
                       "[Notes]" & vbCrLf & vbCrLf &
                       "Recommended Action:" & vbCrLf &
                       "[Action]" & vbCrLf & vbCrLf &
                       "Regards," & vbCrLf &
                       "CRM Department"

            Case "High Priority Lead Report"
                Return "Subject: High Priority Lead Report" & vbCrLf & vbCrLf &
                       "Dear [ManagerName]," & vbCrLf & vbCrLf &
    "We are pleased to inform you that the product you inquired about is available. Your request has been marked as a priority, and one of our team members will contact you shortly so you can finalize everything." & vbCrLf & vbCrLf & "Lead Name: [LeadName]" & vbCrLf &
                       "Phone Number: [Phone]" & vbCrLf &
                       "Email Address: [Email]" & vbCrLf &
                       "Lead Source: [LeadSource]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Status: [Status]" & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf &
                       "Urgency: [Urgency]" & vbCrLf & vbCrLf &
                       "Additional Notes:" & vbCrLf &
                       "[Notes]" & vbCrLf & vbCrLf &
                       "Recommended Action:" & vbCrLf &
                       "[Action]" & vbCrLf & vbCrLf &
                       "Regards," & vbCrLf &
                       "CRM Department"

            Case "Lead Summary Report"
                Return "Subject: Lead Summary Report" & vbCrLf & vbCrLf &
                       "Dear [ManagerName]," & vbCrLf & vbCrLf &
                       "We are delighted to confirm that we will be moving forward together." & vbCrLf & vbCrLf &
                       "Lead Name: [LeadName]" & vbCrLf &
                       "Phone Number: [Phone]" & vbCrLf &
                       "Email Address: [Email]" & vbCrLf &
                       "Lead Source: [LeadSource]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Status: [Status]" & vbCrLf &
                       "Date Created: [DateCreated]" & vbCrLf &
                       "Urgency: [Urgency]" & vbCrLf & vbCrLf &
                       "Additional Notes:" & vbCrLf &
                       "[Notes]" & vbCrLf & vbCrLf &
                       "Recommended Action:" & vbCrLf &
                       "[Action]" & vbCrLf & vbCrLf &
                       "Regards," & vbCrLf &
                       "CRM Department"

            Case Else
                    Return ""
            End Select
        End Function

        Private Function BuildFinalReport() As String
            Dim template As String = GetTemplate(ComboBox1.Text)

        template = template.Replace("[ManagerName]", TextBox1.Text.Trim())
        template = template.Replace("[LeadName]", LeadName)
            template = template.Replace("[Phone]", Phone)
            template = template.Replace("[Email]", EmailAddress)
            template = template.Replace("[LeadSource]", LeadSource)
            template = template.Replace("[Stage]", Stage)
            template = template.Replace("[Status]", Status)
            template = template.Replace("[DateCreated]", DateCreated.ToString("dd MMM yyyy"))
            template = template.Replace("[Urgency]", ComboBox2.Text.Trim())
            template = template.Replace("[Notes]", RichTextBox1.Text.Trim())
        '  template = template.Replace("[Action]", TextBox3.Text.Trim())

        Return template
        End Function

        Private Sub LoadTemplatePreview()
            Try
                RichTextBox2.Text = BuildFinalReport()
            Catch ex As Exception
                MessageBox.Show("Error loading template preview: " & ex.Message)
            End Try
        End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
            Try
                If ComboBox1.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a template.")
                    Exit Sub
                End If

            If TextBox1.Text.Trim() = "" Then
                MessageBox.Show("Please enter Manager Name.")
                Exit Sub
            End If

            If TextBox2.Text.Trim() = "" Then
                    MessageBox.Show("Please enter Recipient Email.")
                    Exit Sub
                End If

                RichTextBox2.Text = BuildFinalReport()
                MessageBox.Show("Report generated successfully.")

            Catch ex As Exception
                MessageBox.Show("Error generating report: " & ex.Message)
            End Try
        End Sub

        Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
            Try
                SendLeadReport()
            Catch ex As Exception
                MessageBox.Show("Error preparing email: " & ex.Message)
            End Try
        End Sub
        Private Sub SendLeadReport()
            Try
                If RichTextBox2.Text.Trim() = "" Then
                    MessageBox.Show("Please generate the report first.")
                    Exit Sub
                End If

                If TextBox2.Text.Trim() = "" Then
                    MessageBox.Show("Please enter Recipient Email.")
                    Exit Sub
                End If

            If My.Settings.SmptHost = "" OrElse My.Settings.SmptFrom = "" Then
                MessageBox.Show("SMTP settings are incomplete. Please check Configuration Settings first.")
                Exit Sub
            End If

            Dim subject As String = (RichTextBox2.Text)
                Dim body As String = (RichTextBox2.Text)

                subject = subject.Replace(vbCr, "").Replace(vbLf, "").Trim()

                If subject = "" Then
                    subject = "Lead Report"
                End If

                Dim mail As New MailMessage()
            mail.From = New MailAddress(My.Settings.SmptFrom)
            mail.To.Add(TextBox2.Text.Trim())
                mail.Subject = subject
                mail.Body = body
                mail.IsBodyHtml = False

            Dim smtp As New SmtpClient(My.Settings.SmptHost, CInt(My.Settings.SmptPort))
            smtp.EnableSsl = My.Settings.SmptSslEnable
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
                smtp.UseDefaultCredentials = False

            If My.Settings.SmptAuth = True Then
                smtp.Credentials = New NetworkCredential(My.Settings.SmptUsername, My.Settings.SmptPassword)
            End If

            smtp.Send(mail)

                MessageBox.Show("Lead report sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error sending lead report: " & ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
            Me.Close()
        End Sub

        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
            Try
                If ComboBox1.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a template.")
                    Exit Sub
                End If

                LoadTemplatePreview()

            Catch ex As Exception
                MessageBox.Show("Error loading template: " & ex.Message)
            End Try
        End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If RichTextBox2.ReadOnly = True Then
            'Unlock
            RichTextBox2.ReadOnly = False
            RichTextBox2.Focus()
            Button1.Text = "Unlock"
        End If
    End Sub
End Class