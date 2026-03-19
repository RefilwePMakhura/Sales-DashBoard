Imports System.Data.OleDb
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms.DataVisualization.Charting


Public Class dashact
    Private Sub Activities_Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ReloadDashboard()
        ApplyPermissions()
        LoadCategories1()
        UpdateOverdueTasks()
        LoadActivities()
        LoadKPIs()
        LoadChart()
        CheckDueTasks()

        ToolStripStatusLabel2.Text = "Logged in as: " & Session.CurrentUser
    End Sub
    'Public Sub ReloadDashboard()

    '    ApplyPermissions()
    '    LoadCategories1()
    '    UpdateOverdueTasks()
    '    LoadActivities()
    '    LoadKPIs()
    '    'LoadChart()
    '    ' CheckDueTasks()

    '     ToolStripStatusLabel2.Text = "Logged in as: " & Session.CurrentUser

    'End Sub
    'Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click

    '    Dim result As DialogResult =
    '    MessageBox.Show("Are you sure you want to switch user?",
    '                    "Switch User",
    '                    MessageBoxButtons.YesNo,
    '                    MessageBoxIcon.Question)

    '    If result = DialogResult.No Then Exit Sub

    '    Try

    '        Session.Clear()


    '        For Each frm As Form In Application.OpenForms.Cast(Of Form).ToList()
    '            frm.Hide()
    '        Next


    '        Dim loginForm As New Login()
    '        loginForm.StartPosition = FormStartPosition.CenterScreen
    '        loginForm.Show()

    '    Catch ex As Exception
    '        MessageBox.Show("Error switching user: " & ex.Message)
    '    End Try

    'End Sub
    Private Sub ApplyPermissions()

            Dim role As String = Session.CurrentRole

            If role = "Admin" Then Exit Sub

            If role = "Manager" Then
                Button8.Enabled = False
            End If

            If role = "Staff" Then
                Button8.Enabled = False
                Button11.Enabled = False
                Button5.Enabled = False
                CheckBox1.Checked = True
                CheckBox1.Enabled = False
            End If

        End Sub


        Private Sub CheckDueTasks()

            Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim overdue As Integer
                Dim today As Integer
                Dim upcoming As Integer

                Dim filter As String = " AND AssignedToUserID=?"

                Dim cmd1 As New OleDbCommand(
                "SELECT COUNT(*) FROM Activities WHERE DueDate < Date() AND Status <> 'Completed'" & filter, conn)
                cmd1.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                overdue = Convert.ToInt32(cmd1.ExecuteScalar())

                Dim cmd2 As New OleDbCommand(
                "SELECT COUNT(*) FROM Activities WHERE DueDate = Date() AND Status <> 'Completed'" & filter, conn)
                cmd2.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                today = Convert.ToInt32(cmd2.ExecuteScalar())

                Dim cmd3 As New OleDbCommand(
                "SELECT COUNT(*) FROM Activities WHERE DueDate > Date() AND Status <> 'Completed'" & filter, conn)
                cmd3.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                upcoming = Convert.ToInt32(cmd3.ExecuteScalar())

                ToolStripStatusLabel1.Text =
                "🔴 Overdue: " & overdue &
                " | 🟡 Today: " & today &
                " | 🔵 Upcoming: " & upcoming

            End Using

        Catch ex As Exception
                ToolStripStatusLabel1.Text = "Notification error"
            End Try

        End Sub

        Private Sub LoadCategories1()
            ComboBox2.Items.Clear()
            ComboBox2.Items.Add("All")
            ComboBox2.Items.Add("Planned")
            ComboBox2.Items.Add("In Progress")
            ComboBox2.Items.Add("Completed")
            ComboBox2.Items.Add("Overdue")
            ComboBox2.SelectedIndex = 0
        End Sub

        Private Sub UpdateOverdueTasks()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Activities SET Status='Overdue' WHERE DueDate < Date() AND Status <> 'Completed'", conn)

            cmd.ExecuteNonQuery()
        End Using

    End Sub


        Private Sub LoadActivities()

            Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim sql As String =
                "SELECT A.ActivityTypeID, A.Name,
                        A.StartDate, A.DueDate, A.Priority, A.Status
                 FROM Activities A
                 LEFT JOIN ActivityTypes T ON A.ActivityTypeID = T.ActivityTypeID
                 WHERE 1=1"

            Dim cmd As New OleDbCommand()
            cmd.Connection = conn

            If ComboBox2.Text <> "All" Then
                sql &= " AND A.Status=?"
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = ComboBox2.Text
            End If

            If TextBox1.Text.Trim() <> "" Then
                sql &= " AND A.Subject LIKE ?"
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = "%" & TextBox1.Text.Trim() & "%"
            End If

            ' 🔥 STAFF ONLY SEE THEIR TASKS
            If Session.CurrentRole = "Staff" Or CheckBox1.Checked Then
                sql &= " AND A.AssignedToUserID=?"
                cmd.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
            End If

            cmd.CommandText = sql

            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(dt)

        End Using

        DataGridView1.DataSource = dt

        End Sub

    Private Sub LoadKPIs()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim filter As String = ""
            Dim isUser As Boolean = (Session.CurrentRole <> "Admin")

            If isUser Then
                filter = " WHERE AssignedToUserID=?"
            End If

            Dim cmd1 As New OleDbCommand("SELECT COUNT(*) FROM Activities" & filter, conn)
            Dim cmd2 As New OleDbCommand("SELECT COUNT(*) FROM Activities WHERE DueDate < Date() AND Status <> 'Completed'" & If(isUser, " AND AssignedToUserID=?", ""), conn)
            Dim cmd3 As New OleDbCommand("SELECT COUNT(*) FROM Activities WHERE Status='Planned'" & If(isUser, " AND AssignedToUserID=?", ""), conn)
            Dim cmd4 As New OleDbCommand("SELECT COUNT(*) FROM Activities WHERE Status='Completed'" & If(isUser, " AND AssignedToUserID=?", ""), conn)

            If isUser Then
                cmd1.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                cmd2.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                cmd3.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
                cmd4.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
            End If

            Label3.Text = cmd1.ExecuteScalar().ToString()
            Label2.Text = cmd2.ExecuteScalar().ToString()
            Label1.Text = cmd3.ExecuteScalar().ToString()
            Label4.Text = cmd4.ExecuteScalar().ToString()

        End Using

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
            Manage_Activity.ShowDialog()
        End Sub

        Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

            If DataGridView1.CurrentRow Is Nothing Then Exit Sub

            Dim frm As New Activity_Details
        frm.ActivityID = DataGridView1.CurrentRow.Cells("ActivityTypeID").Value
        frm.ShowDialog()

            LoadActivities()

        End Sub

        Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
            Button6.PerformClick()
        End Sub

        Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
            If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim id As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ActivityTypeID").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Activities SET Status='Completed' WHERE ActivityTypeID=?", conn)

            cmd.Parameters.Add("?", OleDbType.Integer).Value = id
            cmd.ExecuteNonQuery()

        End Using

        LoadActivities()

        End Sub

        Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
            My_Tasks.ShowDialog()
        End Sub

        Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
            If Session.CurrentRole <> "Admin" Then
                MessageBox.Show("access denied")
                Exit Sub
            End If
            If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim id As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ActivityTypeID").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "DELETE FROM Activities WHERE ActivityTypeID=?", conn)

            cmd.Parameters.Add("?", OleDbType.Integer).Value = id
            cmd.ExecuteNonQuery()

        End Using

        LoadActivities()
        End Sub
        Private Sub LoadChart()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim sql As String =
            "SELECT Status, COUNT(*) AS Total 
             FROM Activities WHERE 1=1"

            Dim cmd As New OleDbCommand()
            cmd.Connection = conn


            If Session.CurrentRole = "Staff" Then
                sql &= " AND AssignedToUserID=?"
                cmd.Parameters.Add("?", OleDbType.Integer).Value = Session.CurrentUserID
            End If

            sql &= " GROUP BY Status"

            cmd.CommandText = sql

            Dim reader = cmd.ExecuteReader()


            Chart1.Series.Clear()
            Chart1.Titles.Clear()

            Chart1.Titles.Add("Activities Overview")

            Dim series = Chart1.Series.Add("Activities")
            series.ChartType = SeriesChartType.Pie


            series.IsValueShownAsLabel = True

            While reader.Read()
                series.Points.AddXY(
                reader("Status").ToString(),
                Convert.ToInt32(reader("Total"))
            )
            End While

        End Using

    End Sub

        Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Report.ShowDialog()
    End Sub

        Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
            LoadActivities()
        End Sub

        Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
            LoadActivities()
        End Sub

        Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
            LoadActivities()
        End Sub

    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadKPIs()
        LoadActivities()
        LoadChart()
    End Sub
End Class
