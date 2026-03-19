Imports System.Data.OleDb
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.IO

Public Class Report

    Private Sub Activities_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load Status Filter
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange({"All", "Planned", "In Progress", "Completed", "Overdue"})
        ComboBox1.SelectedIndex = 0

        ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser

        ' Optional: Update overdue on load
        UpdateOverdueTasks()
    End Sub

    ' 🔹 COLOR ROWS BASED ON STATUS
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

    ' 🔹 GENERATE REPORT
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        UpdateOverdueTasks()

        Dim dt As New DataTable()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim sql As String = "SELECT ActivityID, Name, Status, DueDate, Priority FROM Activities WHERE DueDate BETWEEN ? AND ?"

            If ComboBox1.Text <> "All" Then
                sql &= " AND Status=?"
            End If

            sql &= " ORDER BY DueDate ASC"

            Dim cmd As New OleDbCommand(sql, conn)

            ' Parameters (order matters in OleDb)
            cmd.Parameters.AddWithValue("@StartDate", DateTimePicker1.Value.Date)
            cmd.Parameters.AddWithValue("@EndDate", DateTimePicker2.Value.Date)

            If ComboBox1.Text <> "All" Then
                cmd.Parameters.AddWithValue("@Status", ComboBox1.Text)
            End If

            Dim da As New OleDbDataAdapter(cmd)
            da.Fill(dt)
        End Using

        DataGridView1.DataSource = dt

        If dt.Rows.Count = 0 Then
            MessageBox.Show("No records found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        LoadTotals(dt)
        LoadChart()
    End Sub

    ' 🔹 TOTALS
    Private Sub LoadTotals(dt As DataTable)

        TextBox1.Text = dt.Rows.Count.ToString()

        Dim completed As Integer = 0
        Dim pending As Integer = 0

        For Each row As DataRow In dt.Rows
            If row("Status").ToString() = "Completed" Then
                completed += 1
            Else
                pending += 1
            End If
        Next

        TextBox2.Text = completed.ToString()
        TextBox3.Text = pending.ToString()
    End Sub

    ' 🔹 CHART
    Private Sub LoadChart()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim sql As String = "SELECT Status, COUNT(*) AS Total FROM Activities WHERE DueDate BETWEEN ? AND ?"

            If ComboBox1.Text <> "All" Then
                sql &= " AND Status=?"
            End If

            sql &= " GROUP BY Status"

            Dim cmd As New OleDbCommand(sql, conn)

            ' Parameters
            cmd.Parameters.AddWithValue("@From", DateTimePicker1.Value.Date)
            cmd.Parameters.AddWithValue("@To", DateTimePicker2.Value.Date)

            If ComboBox1.Text <> "All" Then
                cmd.Parameters.AddWithValue("@Status", ComboBox1.Text)
            End If

            Dim reader = cmd.ExecuteReader()

            Chart1.Series.Clear()
            Chart1.Titles.Clear()
            Chart1.Titles.Add("Activity Report")

            Dim series = Chart1.Series.Add("Status")
            series.ChartType = SeriesChartType.Pie

            While reader.Read()
                series.Points.AddXY(reader("Status").ToString(), reader("Total"))
            End While

        End Using
    End Sub

    ' 🔹 UPDATE OVERDUE TASKS
    Private Sub UpdateOverdueTasks()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Activities SET Status='Overdue' WHERE DueDate < Date() AND Status <> 'Completed'", conn)

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' 🔹 RESET BUTTON
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ComboBox1.SelectedIndex = 0
        DateTimePicker1.Value = Date.Today.AddMonths(-1)
        DateTimePicker2.Value = Date.Today

        DataGridView1.DataSource = Nothing
        Chart1.Series.Clear()

        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
    End Sub

    '' 🔹 EXPORT TO CSV (Simple Example)
    'Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    '    If DataGridView1.Rows.Count = 0 Then
    '        MessageBox.Show("No data to export.")
    '        Exit Sub
    '    End If

    '    Dim sfd As New SaveFileDialog()
    '    sfd.Filter = "CSV Files|*.csv"

    '    If sfd.ShowDialog() = DialogResult.OK Then
    '        Using sw As New StreamWriter(sfd.FileName)

    '            ' Headers
    '            For i As Integer = 0 To DataGridView1.Columns.Count - 1
    '                sw.Write(DataGridView1.Columns(i).HeaderText)
    '                If i < DataGridView1.Columns.Count - 1 Then sw.Write(",")
    '            Next
    '            sw.WriteLine()

    '            ' Rows
    '            For Each row As DataGridViewRow In DataGridView1.Rows
    '                If Not row.IsNewRow Then
    '                    For i As Integer = 0 To DataGridView1.Columns.Count - 1
    '                        sw.Write(row.Cells(i).Value?.ToString())
    '                        If i < DataGridView1.Columns.Count - 1 Then sw.Write(",")
    '                    Next
    '                    sw.WriteLine()
    '                End If
    '            Next

    '        End Using

    '        MessageBox.Show("Export successful!")
    '    End If
    'End Sub

End Class