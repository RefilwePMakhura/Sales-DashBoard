Public Class FrmAnalysis

    Private Sub FrmAnalysis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' CRM Funnel Stages
        Dim stages() As String = {
            "Lead Generation", "Lead Qualification", "Lead Assignment",
            "Initial Contact", "Needs Analysis", "Proposal",
            "Deal Closure", "Post Sale Support", "Reporting"
        }

        ' Corresponding Percentages
        Dim percentages() As Integer = {20, 15, 10, 12, 8, 10, 7, 8, 10}

        ' Clear existing chart settings
        Chart1.Series.Clear()
        Chart1.Titles.Clear()
        Chart1.ChartAreas.Clear()

        ' Create Chart Area
        Dim chartArea As New DataVisualization.Charting.ChartArea("MainArea")
        Chart1.ChartAreas.Add(chartArea)

        ' Create Line Series
        Dim lineSeries As New DataVisualization.Charting.Series("CRM Funnel")
        lineSeries.ChartType = DataVisualization.Charting.SeriesChartType.Line
        lineSeries.BorderWidth = 3
        lineSeries.Color = Color.Teal
        lineSeries.MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
        lineSeries.MarkerSize = 8
        lineSeries.IsValueShownAsLabel = True

        ' Add Data Points
        For i As Integer = 0 To stages.Length - 1
            lineSeries.Points.AddXY(stages(i), percentages(i))
        Next

        ' Add Title
        Chart1.Titles.Add("CRM Funnel Analytics - Line Chart")

        ' Add Series to Chart
        Chart1.Series.Add(lineSeries)
    End Sub

End Class