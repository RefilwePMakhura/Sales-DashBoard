Imports System.Net
Imports System.Net.Mail
Imports System.Data.OleDb

Public Class FrmNewLead

    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Private ReadOnly ConnectionString As String =
        $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"

    ' =========================
    ' Form Load
    ' =========================
    Private Sub FrmNewLead_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(New String() {"Facebook", "Instagram", "Website", "Referral", "Walk-in", "Other"})

        ComboBox2.Items.Clear()
        ComboBox2.Items.AddRange(New String() {
            "Lead Generation",
            "Lead Qualification",
            "Lead Assignment",
            "Proposal",
            "Needs Analysis",
            "Initial Contact",
            "Deal Closure",
            "Post Sale Support",
            "Reporting"
        })
        ComboBox3.Items.Clear()
        ComboBox3.Items.AddRange(New String() {"Active", "Converted", "Lost"})

        'ComboBox4.Items.Clear()
        'ComboBox4.Items.AddRange(New String() {"-- Select User --", "Admin", "Sales Rep", "Manager"})
        ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser
        DateTimePicker1.Value = Date.Today
        LoadData()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.SelectedRows.Count = 0 Then Return

        Try
            Dim selectedRow = DataGridView1.SelectedRows(0)
            TextBox1.Text = If(selectedRow.Cells("LeadName").Value IsNot Nothing, selectedRow.Cells("LeadName").Value.ToString(), "")
            TextBox4.Text = If(selectedRow.Cells("Phone").Value IsNot Nothing, selectedRow.Cells("Phone").Value.ToString(), "")
            TextBox3.Text = If(selectedRow.Cells("Email").Value IsNot Nothing, selectedRow.Cells("Email").Value.ToString(), "")
            ComboBox1.Text = If(selectedRow.Cells("Source").Value IsNot Nothing, selectedRow.Cells("Source").Value.ToString(), "")
            ComboBox3.Text = If(selectedRow.Cells("Stage").Value IsNot Nothing, selectedRow.Cells("Stage").Value.ToString(), "")
            ComboBox2.Text = If(selectedRow.Cells("Status").Value IsNot Nothing, selectedRow.Cells("Status").Value.ToString(), "")

        Catch ex As Exception
            MessageBox.Show("Error selecting row: " & ex.Message)
        End Try
    End Sub
    Private Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [NewLead]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub

    ' =========================
    ' SAVE BUTTON
    ' =========================
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        Try
            If TextBox1.Text.Trim() = "" Then
                MessageBox.Show("Please enter Lead Name.")
                Exit Sub
            End If

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String =
                    "INSERT INTO [NewLead] ([LeadName], [Phone], [Email], [Source], [Stage], [Status], [DateCreated]) " &
                    "VALUES (?, ?, ?, ?, ?, ?, ?)"

                Using cmd As New OleDbCommand(query, conn)
                    cmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())
                    cmd.Parameters.AddWithValue("?", TextBox4.Text.Trim())
                    cmd.Parameters.AddWithValue("?", TextBox3.Text.Trim())
                    cmd.Parameters.AddWithValue("?", ComboBox1.Text.Trim())
                    cmd.Parameters.AddWithValue("?", ComboBox2.Text.Trim())
                    cmd.Parameters.AddWithValue("?", ComboBox3.Text.Trim())
                    cmd.Parameters.AddWithValue("?", DateTimePicker1.Value)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Lead saved successfully.")

            TextBox1.Clear()
            TextBox3.Clear()
            TextBox4.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.SelectedIndex = -1
            ComboBox3.SelectedIndex = -1
            DateTimePicker1.Value = Date.Today

        Catch ex As Exception
            MessageBox.Show("Error saving lead: " & ex.Message)
        End Try

    End Sub

    ' =========================
    ' CANCEL / CLOSE
    ' =========================
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ClearForm()
    End Sub

    ' =========================
    ' CLEAR FORM
    ' =========================
    Private Sub ClearForm()
        TextBox1.Clear() ' Full Name
        ' TextBox2.Clear() ' Notes
        TextBox3.Clear() ' Email
        TextBox4.Clear() ' Phone
        'TextBox5.Clear() ' Company
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        '  ComboBox4.SelectedIndex = 0


    End Sub

    ' =========================
    ' EMAIL FUNCTION
    ' =========================
    Private Sub SendLeadEmail(toEmail As String, leadName As String)
        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress("refilwemakhura12@gmail.com")
                mail.To.Add(toEmail)
                mail.Subject = "New Lead Created"
                mail.Body =
                    "Hello " & leadName & "," & vbCrLf & vbCrLf &
                    "Your lead has been created successfully." & vbCrLf & vbCrLf &
                    "Phone Number: " & TextBox4.Text & vbCrLf &
                                        "Date: " & DateTimePicker1.Value.ToShortDateString() & vbCrLf &
                    "LeadSource: " & ComboBox1.Text & vbCrLf &
                    "Status: " & ComboBox2.Text & vbCrLf &
                    "Priority " & ComboBox3.Text & vbCrLf &
                                "For more Information Contact
                                    0768656794
                                   refilwemakhura12@gmail.com " & vbCrLf & vbCrLf &
                                "Regards," & vbCrLf &
                    "Rama's IT Centre"

                Using smtp As New SmtpClient("smtp.gmail.com")
                    smtp.Port = 587
                    smtp.Credentials = New Net.NetworkCredential(
                     "refilwemakhura12@gmail.com",
            "pktb glrx opor dbky")
                    smtp.EnableSsl = True

                    smtp.Send(mail)
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Email Error: " & ex.Message)
        End Try
    End Sub

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr As New MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub ToolStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridView1.CurrentRow Is Nothing Then
                MessageBox.Show("Please select a record.")
                Exit Sub
            End If

            Dim row As DataGridViewRow = DataGridView1.CurrentRow
            Dim frm As New Templete()

            frm.EmailAddress = If(IsDBNull(row.Cells("Email").Value), "", row.Cells("Email").Value.ToString())
            frm.LeadName = If(IsDBNull(row.Cells("LeadName").Value), "", row.Cells("LeadName").Value.ToString())
            frm.Phone = If(IsDBNull(row.Cells("Phone").Value), "", row.Cells("Phone").Value.ToString())
            frm.LeadSource = If(IsDBNull(row.Cells("Source").Value), "", row.Cells("Source").Value.ToString())
            frm.Stage = If(IsDBNull(row.Cells("Stage").Value), "", row.Cells("Stage").Value.ToString())
            frm.Status = If(IsDBNull(row.Cells("Status").Value), "", row.Cells("Status").Value.ToString())
            frm.DateCreated = If(IsDBNull(row.Cells("DateCreated").Value), "", row.Cells("DateCreated").Value.ToString())
            frm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
        'LeadReport.ShowDialog()


    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        'Dim temp As New Templete()

        'temp.EmailAddress = DataGridView1.Rows(e.RowIndex).Cells("Email").Value.ToString()
    End Sub
End Class