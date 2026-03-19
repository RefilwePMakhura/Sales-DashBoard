Imports System.Data.OleDb
Imports System.IO
Public Class Contact_Log
    Dim fullnames As String = "Makhura Refilwe Precious"
    Dim idnumber As String = "0702171112080"
    Dim phonenumber As String = "0768656794"
    Dim email As String = "refilwemakhura@gmail.com"
    Dim Dates As Date = "17 February"


    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Try
            Dim selectedLeadID As Integer = GetSelectedLeadID()

            If selectedLeadID = 0 Then
                MessageBox.Show("Please select a lead.")
                Exit Sub
            End If

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String =
                        "INSERT INTO ContactLog (LeadID, ContactDate, ContactType, Notes) VALUES (?, ?, ?, ?)"

                Using cmd As New OleDbCommand(query, conn)
                    cmd.Parameters.AddWithValue("?", selectedLeadID)
                    cmd.Parameters.AddWithValue("?", DateTimePicker1.Value)
                    cmd.Parameters.AddWithValue("?", ComboBox2.Text.Trim())
                    cmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Contact log saved successfully.")

            ComboBox1.SelectedIndex = -1
            ComboBox2.SelectedIndex = -1
            TextBox1.Clear()
            DateTimePicker1.Value = Date.Today

        Catch ex As Exception
            MessageBox.Show("Error saving contact log: " & ex.Message)
        End Try
    End Sub




    Private Sub LoadLogs()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim da As New OleDbDataAdapter(
                "SELECT * FROM ContactLog ORDER BY ContactDate DESC", conn)

                Dim dt As New DataTable
                da.Fill(dt)

                DataGridView1.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub Contact_Log_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' LoadAndPopulate()
        LoadLogs()
        Try
            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(New String() {"Call", "Email", "WhatsApp", "Meeting", "Follow-up"})

            DateTimePicker1.Value = Date.Today
            LoadLeads()
        Catch ex As Exception
            MessageBox.Show("Error loading Contact Log form: " & ex.Message)
        End Try
    End Sub
    Private Function GetSelectedLeadID() As Integer
        If ComboBox1.SelectedIndex = -1 Then Return 0

        Dim parts() As String = ComboBox1.Text.Split("-"c)
        If parts.Length > 0 Then
            Return Val(parts(0).Trim())
        End If

        Return 0
    End Function

    Private Sub LoadLeads()
        Try
            ComboBox1.Items.Clear()

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String = "SELECT LeadName FROM NewLead ORDER BY LeadName"

                Using cmd As New OleDbCommand(query, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            ComboBox1.Items.Add(reader("LeadName").ToString())
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading leads: " & ex.Message)
        End Try
    End Sub

End Class