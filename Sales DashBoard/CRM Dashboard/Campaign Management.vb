Imports System.Data.OleDb
Public Class Campaign_Management
    Public Property EditCampaignID As Integer = 0

    Private Sub Campaign_Management_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(New String() {"Email", "SMS", "Social", "Event"})

        ComboBox2.Items.Clear()
        ComboBox2.Items.AddRange(New String() {"Planned", "Active", "Paused", "Completed"})

        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 1
        '     TextBox4.Text = "0.00"


        Loadcampaign()

        ' LoadFilterCombos()
        'LoadSelectedCampaign()
    End Sub
    Private Sub Loadcampaign()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String = "SELECT CampaignID, CampaignName, Type, Status, StartDate, EndDate, Budget FROM Campaigns"
                Dim da As New OleDbDataAdapter(query, conn)

                Dim dt As New DataTable
                da.Fill(dt)

                DataGridView1.DataSource = dt
                conn.Close()
            End Using


        Catch ex As Exception

        End Try
    End Sub
    Private Sub Clearfields()

        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        TextBox4.Clear()
    End Sub
    'Private Sub LoadSelectedCampaign()
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()

    '        Dim query As String = "SELECT * FROM Campaigns WHERE CampaignID=?"

    '        Dim cmd As New OleDbCommand(query, conn)
    '        cmd.Parameters.AddWithValue("CampaignID", EditCampaignID)

    '        Dim reader As OleDbDataReader = cmd.ExecuteReader
    '        If reader.Read() Then

    '            TextBox1.Text = reader("CampaignID").ToString()
    '            TextBox2.Text = reader("Name").ToString()
    '            TextBox3.Text = reader("Description").ToString()
    '            ComboBox1.Text = reader("Type").ToString()
    '            ComboBox2.Text = reader("Status").ToString()
    '            DateTimePicker1.Value = Convert.ToDateTime(reader("StartDate"))
    '            DateTimePicker2.Value = Convert.ToDateTime(reader("EndDate"))
    '            TextBox4.Text = reader("Budget").ToString()
    '        End If
    '        conn.Close()
    '    End Using
    'End Sub
    'Private Sub LoadFilterCombos()
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()

    '        Dim cmd1 As New OleDbCommand("SELECT DISTINCT Type FROM Campaigns", conn)
    '        Dim reader1 = cmd1.ExecuteReader
    '        ComboBox2.Items.Clear()
    '        While reader1.Read
    '            ComboBox1.Items.Add(reader1("Type").ToString())
    '        End While

    '        Dim cmd2 As New OleDbCommand("SELECT DISTINCT Status FROM Campaigns", conn)
    '        Dim reader2 = cmd2.ExecuteReader()
    '        ComboBox1.Items.Clear()
    '        While reader2.Read
    '            ComboBox2.Items.Add(reader2("Status").ToString())
    '        End While
    '        conn.Close()
    '    End Using

    'End Sub

    'Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
    '    Using conn As New OleDbConnection(ConnectionString)
    '        Dim query As String = "SELECT * FROM Campaigns WHERE Status=?"
    '        Dim da As New OleDbDataAdapter(query, conn)

    '        da.SelectCommand.Parameters.AddWithValue("@Status", ComboBox2.Text)

    '        Dim dt As New DataTable
    '        da.Fill(dt)

    '        DataGridView1.DataSource = dt
    '    End Using
    'End Sub

    'Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
    '    Using conn As New OleDbConnection(ConnectionString)

    '        Dim query As String = "SELECT * FROM Campaigns WHERE Tpye=?"
    '        Dim da As New OleDbDataAdapter(query, conn)

    '        da.SelectCommand.Parameters.AddWithValue("@Type", ComboBox1.Text)

    '        Dim dt As New DataTable
    '        da.Fill(dt)

    '        DataGridView1.DataSource = dt
    '    End Using
    'End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If e.RowIndex > 0 Then
                Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                TextBox1.Text = row.Cells("CampaignID").Value.ToString()
                TextBox2.Text = row.Cells("CampaignName").Value.ToString()
                TextBox3.Text = row.Cells("Description").Value.ToString()
                ComboBox1.Text = row.Cells("Type").Value.ToString()
                DateTimePicker1.Value = Convert.ToDateTime(row.Cells("StartDate").Value)
                DateTimePicker2.Value = Convert.ToDateTime(row.Cells("EndDate").Value)
                TextBox4.Text = row.Cells("Budget").Value.ToString()
                ComboBox2.Text = row.Cells("Status").Value.ToString()

            End If
            Loadcampaign()
        Catch ex As Exception

        End Try

    End Sub

    'Private Function ValidateForm() As Boolean
    '    If TextBox2.Text.Trim() = "" Then
    '        MessageBox.Show("Enter campaign name.")

    '        TextBox1.Focus()
    '        Return False
    '    End If

    '    If DateTimePicker2.Value.Date < DateTimePicker1.Value.Date Then
    '        MessageBox.Show("End date cannot be before start date.")
    '        Return False
    '    End If

    '    Dim budget As Decimal
    '    If Not Decimal.TryParse(TextBox4.Text.Trim(), budget) Then
    '        MessageBox.Show("Enter a valid budget.")
    '        TextBox4.Focus()
    '        Return False
    '    End If

    '    Return True
    'End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()


                Dim query As String = "INSERT INTO Campaigns(CampaignID, CampaignName, Description, StartDate, EndDate, Type, Status, Budget) VALUES (?,?,?,?,?,?,?,?)"
                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@CampaignID", TextBox1.Text)
                cmd.Parameters.AddWithValue("@CampaignName", TextBox2.Text)
                cmd.Parameters.AddWithValue("@Description", TextBox3.Text)
                cmd.Parameters.AddWithValue("@StartDate", DateTimePicker1.Text)
                cmd.Parameters.AddWithValue("@EndDate", DateTimePicker1.Text)
                cmd.Parameters.AddWithValue("@Type", ComboBox1.Text)
                cmd.Parameters.AddWithValue("@Status", ComboBox2.Text)
                cmd.Parameters.AddWithValue("@Budget", TextBox4.Text)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Campaign Saved")
            End Using

            Loadcampaign()
            Clearfields()

        Catch ex As Exception
            MessageBox.Show("Error saving campaign: " & ex.Message)
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub
    'Private Sub LoadCampaigns()
    '    Try
    '        Using conn As New OleDbConnection(ConnectionString)
    '            conn.Open()

    '            Dim query As String = "SELECT CampaignName, Type, Status, StartDate, EndDate, Budget FROM Campaigns "
    '            Dim da As New OleDbDataAdapter(query, conn)
    '            Dim dt As New DataTable()
    '            da.Fill(dt)
    '            DataGridView1.DataSource = dt
    '        End Using
    '    Catch ex As Exception
    '        MessageBox.Show("Failed to load data:" & ex.Message)
    '    End Try
    'End Sub
    'Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
    '    Using conn As New OleDbConnection(ConnectionString)
    '        Dim query As String = "SELECT * FROM Campaigns WHERE Type=?"
    '        Dim da As New OleDbDataAdapter(query, conn)

    '        da.SelectCommand.Parameters.AddWithValue("@Type", ComboBox1.Text)

    '        Dim dt As New DataTable
    '        da.Fill(dt)

    '        DataGridView1.DataSource = dt
    '    End Using
    'End Sub
End Class