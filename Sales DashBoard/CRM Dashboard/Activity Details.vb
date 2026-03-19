Imports System.Data.OleDb

Public Class Activity_Details


    Public ActivityID As Integer

        Private Sub Activity_Details_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadDetails()
            LoadNotes()
        ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser
    End Sub


        Private Sub LoadDetails()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
            "SELECT [Name], [Description], CreatedBy
             FROM Activities
             WHERE ActivityID = ?", conn)

            cmd.Parameters.Add("?", OleDbType.Integer).Value = ActivityID

            Using r = cmd.ExecuteReader()

                If r.Read() Then
                    TextBox1.Text = r("Name").ToString()
                    TextBox2.Text = r("Description").ToString()
                End If

            End Using

        End Using

    End Sub


        Private Sub LoadNotes()

            Dim dt As New DataTable()

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim cmd As New OleDbCommand(
            "SELECT NoteText, CreatedDate
             FROM ActivityNotes
             WHERE ActivityID = ?
             ORDER BY CreatedDate DESC", conn)

                cmd.Parameters.Add("?", OleDbType.Integer).Value = ActivityID

                Dim da As New OleDbDataAdapter(cmd)
                da.Fill(dt)

            End Using

            DataGridView1.DataSource = dt

        End Sub


        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

            If String.IsNullOrWhiteSpace(TextBox3.Text) Then
                MessageBox.Show("Enter a note")
                Exit Sub
            End If

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
            "INSERT INTO ActivityNotes (ActivityID, NoteText, CreatedDate, CreatedBy)
             VALUES (?, ?, ?, ?)", conn)

            cmd.Parameters.Add("?", OleDbType.Integer).Value = ActivityID
            cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = TextBox3.Text.Trim()
            cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now
            cmd.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID

            cmd.ExecuteNonQuery()

        End Using

        TextBox3.Clear()
            LoadNotes()

        End Sub

    End Class