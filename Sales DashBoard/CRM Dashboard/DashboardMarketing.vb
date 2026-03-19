Imports System.Data.OleDb
Imports System.Windows.Forms.DataVisualization.Charting
Public Class DashboardMarketing


    '========================
    ' FILTER BY TYPE
    '========================
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
            Try
                If ComboBox1.Text.Trim() = "" Then
                    LoadCampaigns()
                    Exit Sub
                End If

                Dim dt As New DataTable()

                Using conn As New OleDbConnection(ConnectionString)
                    Dim query As String = "SELECT CampaignName, Type, Status, StartDate, EndDate, Budget FROM Campaigns WHERE Type = ?"
                    Using da As New OleDbDataAdapter(query, conn)
                        da.SelectCommand.Parameters.AddWithValue("@p1", ComboBox1.Text.Trim())
                        da.Fill(dt)
                    End Using
                End Using

                DataGridView1.DataSource = dt

            Catch ex As Exception
                MessageBox.Show("Error filtering by Type: " & ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        '========================
        ' LOAD ALL CAMPAIGNS
        '========================
        Private Sub LoadCampaigns()
            Try
                Dim dt As New DataTable()

                Using conn As New OleDbConnection(ConnectionString)
                    Dim sql As String = "SELECT CampaignName, Type, Status, StartDate, EndDate, Budget FROM Campaigns"
                    Using da As New OleDbDataAdapter(sql, conn)
                        da.Fill(dt)
                    End Using
                End Using

                DataGridView1.DataSource = dt

            Catch ex As Exception
                MessageBox.Show("Error loading campaigns: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        '========================
        ' FILTER BY STATUS
        '========================
        Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
            Try
                If ComboBox2.Text.Trim() = "" Then
                    LoadCampaigns()
                    Exit Sub
                End If

                Dim dt As New DataTable()

                Using conn As New OleDbConnection(ConnectionString)
                    Dim query As String = "SELECT CampaignName, Type, Status, StartDate, EndDate, Budget FROM Campaigns WHERE Status = ?"
                    Using da As New OleDbDataAdapter(query, conn)
                        da.SelectCommand.Parameters.AddWithValue("@p1", ComboBox2.Text.Trim())
                        da.Fill(dt)
                    End Using
                End Using

                DataGridView1.DataSource = dt

            Catch ex As Exception
                MessageBox.Show("Error filtering by Status: " & ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        '========================
        ' LOAD KPI VALUES
        '========================
        Private Sub LoadKPIs()
            Try
                Using conn As New OleDbConnection(ConnectionString)
                    conn.Open()

                    Dim sentCount As Integer = 0
                    Dim failedCount As Integer = 0
                    Dim openedCount As Integer = 0

                    Using cmd1 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Sent'", conn)
                        sentCount = Convert.ToInt32(cmd1.ExecuteScalar())
                    End Using

                    Using cmd2 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Failed'", conn)
                        failedCount = Convert.ToInt32(cmd2.ExecuteScalar())
                    End Using

                    Using cmd3 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Opened'", conn)
                        openedCount = Convert.ToInt32(cmd3.ExecuteScalar())
                    End Using

                    Label8.Text = sentCount.ToString()
                    Label9.Text = failedCount.ToString()
                    Label10.Text = openedCount.ToString()
                End Using

            Catch ex As Exception
                MessageBox.Show("Error loading KPIs: " & ex.Message, "KPI Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    '========================
    ' LOAD CHART
    '========================


    Private Sub LoadMarketingChart()
        Try
            Chart1.Series.Clear()
            Chart1.ChartAreas.Clear()
            Chart1.Titles.Clear()

            Dim area As New ChartArea("Area1")
            Chart1.ChartAreas.Add(area)

            Dim emailSeries As New Series("Email Status")
            emailSeries.ChartArea = "Area1"
            emailSeries.ChartType = SeriesChartType.Column
            emailSeries.IsValueShownAsLabel = True
            Chart1.Series.Add(emailSeries)

            Dim sentCount As Integer = 0
            Dim failedCount As Integer = 0
            Dim openedCount As Integer = 0

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd1 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Sent'", conn)
                    sentCount = Convert.ToInt32(cmd1.ExecuteScalar())
                End Using

                Using cmd2 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Failed'", conn)
                    failedCount = Convert.ToInt32(cmd2.ExecuteScalar())
                End Using

                Using cmd3 As New OleDbCommand("SELECT COUNT(*) FROM EmailTracking WHERE Status='Opened'", conn)
                    openedCount = Convert.ToInt32(cmd3.ExecuteScalar())
                End Using
            End Using

            emailSeries.Points.AddXY("Sent", sentCount)
            emailSeries.Points.AddXY("Failed", failedCount)
            emailSeries.Points.AddXY("Opened", openedCount)

            If emailSeries.Points.Count > 0 Then emailSeries.Points(0).Color = Color.Green
            If emailSeries.Points.Count > 1 Then emailSeries.Points(1).Color = Color.Red
            If emailSeries.Points.Count > 2 Then emailSeries.Points(2).Color = Color.Blue

            Chart1.Titles.Add("Marketing Email Performance")
            Chart1.ChartAreas("Area1").AxisX.Title = "Email Status"
            Chart1.ChartAreas("Area1").AxisY.Title = "Number of Emails"

        Catch ex As Exception
            MessageBox.Show("Error loading marketing chart: " & ex.Message)
        End Try
    End Sub
    Private Sub LoadFilterCombos()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Using cmd1 As New OleDbCommand("SELECT DISTINCT Type FROM Campaigns WHERE Type Is Not Null", conn)
                Using reader1 = cmd1.ExecuteReader
                    ComboBox1.Items.Clear()
                    While reader1.Read
                        ComboBox1.Items.Add(reader1("Type").ToString())
                    End While
                End Using
            End Using

            Using cmd2 As New OleDbCommand("SELECT DISTINCT Status FROM Campaigns WHERE Status Is Not Null", conn)
                Using reader2 = cmd2.ExecuteReader()
                    ComboBox2.Items.Clear()
                    While reader2.Read
                        ComboBox2.Items.Add(reader2("Status").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim frm As New Campaign_Management
            frm.ShowDialog()

            LoadFilterCombos()
            LoadMarketingChart()
            LoadKPIs()
            LoadCampaigns()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DashboardMarketing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFilterCombos()
        LoadMarketingChart()
        LoadKPIs()
        LoadCampaigns()
        ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser
        'ComboBox1.Items.Clear()
        'ComboBox1.Items.AddRange(New String() {"Email", "SMS", "Social", "Event"})

        'ComboBox2.Items.Clear()
        'ComboBox2.Items.AddRange(New String() {"Sent", "Failed", "Opened"})

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MarketingTemplete.ShowDialog()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        dashact.ShowDialog()
    End Sub
End Class