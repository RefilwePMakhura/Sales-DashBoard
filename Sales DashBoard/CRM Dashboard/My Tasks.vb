Imports System.Data.OleDb
Imports System.Drawing

Public Class My_Tasks

    ' =========================
    ' FORM LOAD
    ' =========================
    Private Sub My_Tasks_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange({"All", "Planned", "In Progress", "Completed", "Overdue"})
        ComboBox1.SelectedIndex = 0

        ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser

        UpdateOverdueTasks()
        LoadMyTasks()

    End Sub

    ' =========================
    ' LOAD TASKS
    ' =========================
    Private Sub LoadMyTasks()

        Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            ' ⚠ FIXED SQL (removed WHERE 1=1 issue and corrected logic)
            Dim sql As String =
                "SELECT ActivityID, Name, DueDate, Priority, Status
                 FROM Activities
                 WHERE DueDate <= ?"

            If ComboBox1.Text <> "All" Then
                sql &= " AND Status = ?"
            End If

            sql &= " ORDER BY DueDate ASC"

            Dim cmd As New OleDbCommand(sql, conn)

            ' ⚠ OleDb uses POSITIONAL parameters
            cmd.Parameters.AddWithValue("?", DateTimePicker1.Value.Date)

            If ComboBox1.Text <> "All" Then
                cmd.Parameters.AddWithValue("?", ComboBox1.Text)
            End If

            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(dt)

        End Using

        DataGridView1.DataSource = dt

        If dt.Rows.Count = 0 Then
            RichTextBox1.Clear()
        End If

    End Sub

    ' =========================
    ' UPDATE OVERDUE TASKS
    ' =========================
    Private Sub UpdateOverdueTasks()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Activities
                 SET Status='Overdue'
                 WHERE DueDate < Date() AND Status <> 'Completed'", conn)

            cmd.ExecuteNonQuery()

        End Using

    End Sub

    ' =========================
    ' LOAD NOTES
    ' =========================
    Private Sub LoadNotes(activityID As Integer)

        RichTextBox1.Clear()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "SELECT NoteText, CreatedDate
                 FROM ActivityNotes
                 WHERE ActivityID=?
                 ORDER BY CreatedDate ASC", conn)

            cmd.Parameters.AddWithValue("?", activityID)

            Dim reader = cmd.ExecuteReader()

            Dim isUser As Boolean = True

            While reader.Read()

                Dim noteDate As DateTime = Convert.ToDateTime(reader("CreatedDate"))
                Dim noteText As String = reader("NoteText").ToString()

                If isUser Then
                    RichTextBox1.SelectionAlignment = HorizontalAlignment.Right
                    RichTextBox1.SelectionBackColor = Color.LightBlue
                    RichTextBox1.SelectionFont = New Font("Segoe UI", 10)

                    RichTextBox1.AppendText("You: " & noteText & vbCrLf)
                Else
                    RichTextBox1.SelectionAlignment = HorizontalAlignment.Left
                    RichTextBox1.SelectionBackColor = Color.LightGray
                    RichTextBox1.SelectionFont = New Font("Segoe UI", 10)

                    RichTextBox1.AppendText("Note: " & noteText & vbCrLf)
                End If

                RichTextBox1.SelectionFont = New Font("Segoe UI", 8, FontStyle.Italic)
                RichTextBox1.AppendText(noteDate.ToString("HH:mm dd MMM") & vbCrLf & vbCrLf)

                isUser = Not isUser

            End While

        End Using

        RichTextBox1.SelectionStart = RichTextBox1.Text.Length
        RichTextBox1.ScrollToCaret()

    End Sub

    ' =========================
    ' SELECT TASK → LOAD NOTES
    ' =========================
    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged

        If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim idObj = DataGridView1.CurrentRow.Cells("ActivityID").Value
        If idObj Is Nothing Then Exit Sub

        LoadNotes(Convert.ToInt32(idObj))

    End Sub

    ' =========================
    ' DOUBLE CLICK → DETAILS
    ' =========================
    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick

        If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim frm As New Activity_Details
        frm.ActivityID = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ActivityID").Value)
        frm.ShowDialog()

        LoadMyTasks()

    End Sub

    ' =========================
    ' ADD NOTE
    ' =========================
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Select a task first.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("Enter a note.")
            Exit Sub
        End If

        Dim activityID As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ActivityID").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "INSERT INTO ActivityNotes(ActivityID, NoteText, CreatedDate)
                 VALUES (?, ?, ?)", conn)

            cmd.Parameters.AddWithValue("?", activityID)
            cmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())
            cmd.Parameters.AddWithValue("?", DateTime.Now)

            cmd.ExecuteNonQuery()

        End Using

        TextBox1.Clear()
        LoadNotes(activityID)

    End Sub

    ' =========================
    ' FILTER EVENTS
    ' =========================
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        LoadMyTasks()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        LoadMyTasks()
    End Sub

    ' =========================
    ' ROW COLORING
    ' =========================
    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        If e.RowIndex < 0 Then Exit Sub

        If DataGridView1.Rows(e.RowIndex).Cells("Status").Value Is Nothing Then Exit Sub

        Dim status As String = DataGridView1.Rows(e.RowIndex).Cells("Status").Value.ToString()

        Select Case status
            Case "Completed"
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGreen
            Case "Overdue"
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightCoral
            Case "In Progress"
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightYellow
            Case "Planned"
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue
        End Select

    End Sub

End Class