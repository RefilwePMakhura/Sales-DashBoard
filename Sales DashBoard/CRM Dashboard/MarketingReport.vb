Imports System.Data.OleDb
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting

Public Class MarketingReport
    ' Private ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{File}"";Persist Security Info=False;"

    Private Sub LoadChart()

        Using conn As New OleDbConnection(ConnectionString)
            Chart1.Series("Spend").Points.Clear()
            Chart1.Series("Revenue").Points.Clear()

            Dim da As New OleDbDataAdapter("SELECT Name, Budget FROM Campaigns", conn)

            Dim dt As New DataTable
            da.Fill(dt)

            For Each row As DataRow In dt.Rows

                Chart1.Series("Spend").Points.AddXY(
                    row("Name").ToString(),
                    row("Budget"))
            Next

            Dim da2 As New OleDbDataAdapter("SELECT CampaignID, SUM(ResponseValue)vAs Revenue FROM CampaignResponses
GROUP BY CampaignID", conn)

            Dim dt2 As New DataTable
            da2.Fill(dt2)
            For Each row As DataRow In dt2.Rows

                Chart1.Series("Revenue").Points.AddXY(
                    row("CampaignID").ToString(),
                    row("Revenue"))
            Next
        End Using

    End Sub

    Private Sub MarketingReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("Campaign ROI")
        ComboBox1.Items.Add("Campaign Performance")
        ComboBox1.SelectedIndex = 0

        setupChart()
    End Sub
    Private Sub setupChart()
        Chart1.Series.Clear()
        Chart1.Series.Add("Spend")
        Chart1.Series.Add("Revenue")

        Chart1.Series("Spend").ChartType = SeriesChartType.Column

        Chart1.Series("Revenue").ChartType = SeriesChartType.Column
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim cmd1 As New OleDbCommand("SELECT COUNT (*) FROM Campaigns WHERE StartDate Between ? AND ?", conn)
                cmd1.Parameters.AddWithValue("@From", DateTimePicker1.Value)
                cmd1.Parameters.AddWithValue("@To", DateTimePicker2.Value)
                Label8.Text = cmd1.ExecuteScalar().ToString()

                Dim cmd2 As New OleDbCommand(
                    "SELECT SUM (Budget) FROM Campaigns WHERE StartDate Between ? AND ?", conn)
                cmd2.Parameters.AddWithValue("@From", DateTimePicker1.Value)
                cmd2.Parameters.AddWithValue("@To", DateTimePicker2.Value)
                If cmd2.ExecuteScalar() IsNot DBNull.Value Then
                    Label9.Text = cmd2.ExecuteScalar().ToString()
                Else
                    Label9.Text = "0"
                End If

                Dim cmd3 As New OleDbCommand(
                    "SELECT SUM (ResponseValue) FROM CampaignResponses WHERE ResponseDate Between ? AND ?", conn)
                cmd3.Parameters.AddWithValue("@From", DateTimePicker1.Value)
                cmd3.Parameters.AddWithValue("@To", DateTimePicker2.Value)
                If cmd3.ExecuteScalar() IsNot DBNull.Value Then
                    Label10.Text = cmd3.ExecuteScalar().ToString()
                Else
                    Label10.Text = "0"
                End If

                Dim spend As Double = Val(Label9.Text)

                Dim roi As Double = Val(Label10.Text)

                Label11.Text = roi.ToString()
                LoadChart()

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sw As New StreamWriter("MarketingReport.csv")
        sw.WriteLine("Total Campaigns," & Label8.Text)
        sw.WriteLine("Total Spend," & Label9.Text)
        sw.WriteLine("Revenue," & Label10.Text)
        sw.WriteLine("ROI," & Label11.Text)
        sw.Close()

        MessageBox.Show("Exported")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class