Imports System.Data.OleDb
Imports System.Windows.Forms.DataVisualization.Charting

Public Class CRM_DashBoard

    Private Sub frmCRMDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadProgressBars()
            LoadDashboard()
            LoadLeadSourceChart()
            ToolStripStatusLabel1.Text = "Logged in as: " & Session.CurrentUser
        Catch ex As Exception
            MessageBox.Show("Error loading dashboard: " & ex.Message)
        End Try
    End Sub
    Public Sub LoadDashboard()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                ' Active Leads
                ' Active Leads (NOT Lost or Won)
                Dim cmdActive As New OleDbCommand("SELECT COUNT(*) FROM NewLead WHERE Status <> 'Lost' AND Status <> 'Won'", conn)
                lblActiveLeads.Text = cmdActive.ExecuteScalar().ToString()


                ' Open Opportunities (Qualified)
                Dim cmdOpportunities As New OleDbCommand("SELECT COUNT(*) FROM NewLead WHERE Status = 'Qualified'", conn)
                lblOpportunities.Text = cmdOpportunities.ExecuteScalar().ToString()


                ' Leads This Month
                Dim cmdMonth As New OleDbCommand(
                "SELECT COUNT(*) FROM [NewLead] " &
                "WHERE [DateCreated] >= DateSerial(Year(Date()), Month(Date()), 1) " &
                "AND [DateCreated] < DateSerial(Year(Date()), Month(Date()) + 1, 1)", conn)
                lblInteractions.Text = cmdMonth.ExecuteScalar().ToString()

                ' Converted Leads
                Dim cmdWon As New OleDbCommand(
                "SELECT COUNT(*) FROM [NewLead] WHERE [Status] = 'Converted'", conn)
                lblLeadsConversion.Text = cmdWon.ExecuteScalar().ToString()

                ' Top Source
                Dim cmdSource As New OleDbCommand(
                "SELECT TOP 1 [Source], COUNT(*) AS Total " &
                "FROM [NewLead] " &
                "WHERE [Source] Is Not Null AND [Source] <> '' " &
                "GROUP BY [Source] " &
                "ORDER BY COUNT(*) DESC", conn)

                Using reader As OleDbDataReader = cmdSource.ExecuteReader()
                    If reader.Read() Then
                        lblTopLead.Text = reader("Source").ToString()
                    Else
                        lblTopLead.Text = "-"
                    End If
                End Using

            End Using
        Catch ex As Exception
            MessageBox.Show("Dashboard Error: " & ex.Message)
        End Try
    End Sub
    Public Sub LoadProgressBars()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                ' Total Leads
                Dim totalCmd As New OleDbCommand("SELECT COUNT(*) FROM NewLead", conn)
                Dim totalLeads As Integer = Convert.ToInt32(totalCmd.ExecuteScalar())

                If totalLeads = 0 Then totalLeads = 1 ' avoid division by zero

                ' -------------------------------
                ' 1. Conversion Rate (Won Leads)
                ' -------------------------------
                Dim wonCmd As New OleDbCommand("SELECT COUNT(*) FROM NewLead WHERE Status = 'Won'", conn)
                Dim wonLeads As Integer = Convert.ToInt32(wonCmd.ExecuteScalar())

                Dim conversionPercent As Integer = CInt((wonLeads / totalLeads) * 100)
                pbConversion.Value = conversionPercent

                ' Optional label
                lblConversionPercent.Text = conversionPercent & "%"

                ' -------------------------------
                ' 2. Pipeline Progress (Active Leads)
                ' -------------------------------
                Dim activeCmd As New OleDbCommand("SELECT COUNT(*) FROM NewLead WHERE Status <> 'New'", conn)
                Dim activeLeads As Integer = Convert.ToInt32(activeCmd.ExecuteScalar())

                Dim pipelinePercent As Integer = CInt((activeLeads / totalLeads) * 100)
                pbPipeline.Value = pipelinePercent

                ' Optional label
                lblPipelinePercent.Text = pipelinePercent & "%"

            End Using

        Catch ex As Exception
            MessageBox.Show("Progress bar error: " & ex.Message)
        End Try
    End Sub
    Private Function GetScalar(conn As OleDbConnection, query As String) As Integer
        Using cmd As New OleDbCommand(query, conn)
            Return Convert.ToInt32(cmd.ExecuteScalar())
        End Using
    End Function
    Private Sub LoadLeadSourceChart()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim query As String = "SELECT [Source], COUNT(*) AS Total FROM [NewLead] WHERE [Source] Is Not Null AND [Source] <> '' GROUP BY [Source]"

                Dim cmd As New OleDbCommand(query, conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                Chart1.Series.Clear()
                Chart1.Titles.Clear()
                Chart1.ChartAreas.Clear()
                Chart1.Legends.Clear()

                Chart1.ChartAreas.Add(New ChartArea("MainArea"))
                Chart1.Legends.Add(New Legend("MainLegend"))

                Dim series As New Series("Lead Sources")
                series.ChartType = SeriesChartType.Pie
                series.IsValueShownAsLabel = True
                series.Label = "#VALX (#PERCENT)"
                series.LegendText = "#VALX"

                While reader.Read()
                    series.Points.AddXY(reader("Source").ToString(),
                                    Convert.ToInt32(reader("Total")))
                End While

                Chart1.Series.Add(series)
                Chart1.Titles.Add("Top Lead Source")
            End Using

        Catch ex As Exception
            MessageBox.Show("Chart error: " & ex.Message)
        End Try
    End Sub
    'Private Sub btnNewLead_Click(sender As Object, e As EventArgs) Handles btnNewLead.Click
    '    Dim f As New New_Lead()
    '    f.ShowDialog()
    'End Sub
    'Private Sub btnContactLog_Click(sender As Object, e As EventArgs) Handles btnContactLog.Click
    '    Dim f As New Contact_Log()
    '    f.ShowDialog()
    'End Sub
    'Private Sub btnAnalysis_Click(sender As Object, e As EventArgs) Handles btnAnalysis.Click
    '    Dim f As New Analytics()
    '    f.ShowDialog()
    'End Sub
    'Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
    '    Dim f As New Setting()
    '    f.ShowDialog()
    'End Sub
    'Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
    '    Me.Close()
    'End Sub
    Private Sub btnLeadGeneration_Click(sender As Object, e As EventArgs) Handles btnLeadGeneration.Click
        MessageBox.Show("Open Lead Generation module")
    End Sub


    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim frm As New Settings
        frm.Show()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim frm As New FrmNewLead
        frm.Show()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim frm As New Contact_Log
        frm.Show()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim frm As New dashact
        frm.ShowDialog()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Dim frm As New DashboardMarketing
        frm.Show()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles lblInteractions.Click
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub
End Class