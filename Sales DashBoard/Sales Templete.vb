Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Mail

Public Class Sales_Templete



    '================= SALES DATA ONLY =================
    Public Property Customer As String = ""
    Public Property EmailAddress As String = ""
        Public Property Stage As String = ""
    Public Property Sale_Date As Date = Date.Today

    Public Property Product As String = ""
    Public Property Quantity As Integer = 0
        Public Property UnitPrice As Decimal = 0
        Public Property TotalPrice As Decimal = 0

        '================= LOAD =================
        Private Sub Lead_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

            RichTextBox2.ReadOnly = True
            Button4.Text = "Unlock"

            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(New String() {
            "Sales Report",
            "Invoice Report",
            "Summary Report"
        })

        ComboBox2.Items.Clear()
        ComboBox2.Items.AddRange(New String() {"Low", "Medium", "High", "Urgent"})
        ComboBox2.SelectedIndex = 0
        ComboBox1.SelectedIndex = 0

        ' SET DATA
        TextBox1.Text = Customer
        TextBox2.Text = EmailAddress

        LoadTemplatePreview()

        End Sub

        '================= SAVE EMAIL =================
        Private Sub SaveEmailHistory(emailTo As String, subject As String, body As String)
            Try
                Using conn As New OleDbConnection(connectionString)
                    conn.Open()

                    Dim query As String = "INSERT INTO EmailHistory (LeadName, EmailAddress, Subject, Body) VALUES (?, ?, ?, ?)"

                    Using cmd As New OleDbCommand(query, conn)
                    cmd.Parameters.AddWithValue("?", Customer)
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

        '================= TEMPLATE =================
        Private Function GetTemplate(templateName As String) As String

            Select Case templateName

                Case "Invoice Report"
                    Return "Subject: Invoice Report" & vbCrLf & vbCrLf &
                       "Dear [Customer]," & vbCrLf & vbCrLf &
                       "Please find your invoice details below:" & vbCrLf & vbCrLf &
                       "Product: [Product]" & vbCrLf &
                       "Quantity: [Quantity]" & vbCrLf &
                       "Unit Price: [UnitPrice]" & vbCrLf &
                       "Total Price: [TotalPrice]" & vbCrLf &
                       "Date: [DateCreated]" & vbCrLf & vbCrLf &
                       "Thank you for your purchase." & vbCrLf &
                       "Regards," & vbCrLf &
                       "Sales Team"

                Case "Summary Report"
                Return "Subject: Sales Summary" & vbCrLf & vbCrLf &
                       "Customer: [Customer]" & vbCrLf &
                       "Product: [Product]" & vbCrLf &
                       "Quantity: [Quantity]" & vbCrLf &
                       "Total Price: [TotalPrice]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Date: [Sale_Date]" & vbCrLf & vbCrLf &
                       "Notes:" & vbCrLf &
                       "[Notes]"

            Case Else ' Sales Report
                Return "Subject: Sales Report" & vbCrLf & vbCrLf &
                       "Dear [Customer]," & vbCrLf & vbCrLf &
                       "Here are your sales details:" & vbCrLf & vbCrLf &
                       "Product: [Product]" & vbCrLf &
                       "Quantity: [Quantity]" & vbCrLf &
                       "Unit Price: [UnitPrice]" & vbCrLf &
                       "Total Price: [TotalPrice]" & vbCrLf &
                       "Stage: [Stage]" & vbCrLf &
                       "Date: [Sale_Date]" & vbCrLf & vbCrLf &
                       "Notes:" & vbCrLf &
                       "[Notes]" & vbCrLf & vbCrLf &
                       "Regards," & vbCrLf &
                       "Sales Team"

        End Select

        End Function

        '================= BUILD REPORT =================
        Private Function BuildFinalReport() As String

            Dim template As String = GetTemplate(ComboBox1.Text)

        template = template.Replace("[Customer]", Customer)
        template = template.Replace("[Product]", ProductName)
            template = template.Replace("[Quantity]", Quantity.ToString())
            template = template.Replace("[UnitPrice]", UnitPrice.ToString("F2"))
            template = template.Replace("[TotalPrice]", TotalPrice.ToString("F2"))
            template = template.Replace("[Stage]", Stage)
        template = template.Replace("[Sale_Date]", Sale_Date.ToString("dd MMM yyyy"))
        '     template = template.Replace("[Notes]", TextBox3.Text.Trim())

        Return template

        End Function

        '================= PREVIEW =================
        Private Sub LoadTemplatePreview()
            RichTextBox2.Text = BuildFinalReport()
        End Sub

        '================= GENERATE =================
        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text.Trim() = "" Then
            MessageBox.Show("Enter Customer Name")
            Exit Sub
        End If

        If TextBox2.Text.Trim() = "" Then
            MessageBox.Show("Enter Email")
            Exit Sub
        End If

        RichTextBox2.Text = BuildFinalReport()
            MessageBox.Show("Report Generated")

        End Sub

        '================= TEMPLATE CHANGE =================
        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
            LoadTemplatePreview()
        End Sub

        '================= SEND EMAIL =================
        Private Sub SendEmail()

            Try
            Dim smtp As New SmtpClient(My.Settings.SmptHost, My.Settings.SmptPort)
            smtp.EnableSsl = My.Settings.SmptSslEnable
            smtp.Credentials = New NetworkCredential(My.Settings.SmptUsername, My.Settings.SmptPassword)

            Dim body As String = RichTextBox2.Text
                Dim subject As String = ComboBox1.Text

            Dim emailTo As String = TextBox2.Text

            Dim mail As New MailMessage()
            mail.From = New MailAddress(My.Settings.SmptFrom)
            mail.To.Add(emailTo)
                mail.Subject = subject
                mail.Body = body

                smtp.Send(mail)

                SaveEmailHistory(emailTo, subject, body)

                MessageBox.Show("Email Sent & Saved")

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End Sub

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

        '================= LOCK / UNLOCK =================
        Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

            If RichTextBox2.ReadOnly Then
                RichTextBox2.ReadOnly = False
                Button4.Text = "Lock"
            Else
                RichTextBox2.ReadOnly = True
                Button4.Text = "Unlock"
            End If

        End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SendEmail()
    End Sub
End Class

