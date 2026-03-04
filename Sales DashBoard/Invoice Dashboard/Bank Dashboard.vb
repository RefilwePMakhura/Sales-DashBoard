Imports System.Data.OleDb
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Bank_Dashboard
    Public Sub LoadCurrencies()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand("SELECT DISTINCT Currency FROM [BankAccount]", conn)
            Dim reader = cmd.ExecuteReader()

            ComboBox1.Items.Clear()
            ComboBox1.Items.Add("")

            While reader.Read
                ComboBox1.Items.Add(reader("Currency").ToString())
            End While
        End Using
    End Sub

    Public Sub LoadBankDashboard()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim sql As String = "SELECT * FROM [BankAccount]"
            Dim filters As New List(Of String)

            If CheckBox1.Checked Then
                filters.Add("IsActive = True")
            End If

            If ComboBox1.Text <> "" Then
                filters.Add("Currency =?")
            End If

            If filters.Count > 0 Then
                sql &= "WHERE" & String.Join("AND", filters)
            End If
            Dim dt As New DataTable

            Using cmd As New OleDbCommand(sql, conn)
                If ComboBox1.Text <> "" Then
                    cmd.Parameters.AddWithValue("?", ComboBox1.Text)
                End If

                Using da As New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            DataGridView1.DataSource = dt

            Label5.Text = dt.AsEnumerable().Count(Function(r) r.Field(Of Boolean)("IsActive")).ToString()
            Label7.Text = "R" & dt.AsEnumerable().Sum(Function(r) If(IsDBNull(r("OpeningBalance")), 0D, Convert.ToDecimal(r("OpeningBalance")))).ToString()
            Label8.Text = dt.DefaultView.ToTable(True, "Currency").Rows.Count.ToString()
        End Using
    End Sub
    Public Sub LoadBankCharts()
        Using conn As New OleDbConnection(ConnectionString)

            Try
                conn.Open()

                Dim dt As New DataTable
                Dim sql As String = "SELECT BankName, ClosingBalance FROM [BankAccount] ORDER BY ClosingBalance DESC"

                Using da As New OleDbDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using

                Chart1.Series.Clear()
                Chart1.ChartAreas.Clear()
                Chart1.ChartAreas.Add(New ChartArea("PieArea"))

                Dim pieSeries As New Series("Balance")
                pieSeries.ChartType = SeriesChartType.Pie
                pieSeries.IsValueShownAsLabel = True
                pieSeries.LabelFormat = "N2"
                pieSeries("PieLabelStyle") = "Outside"

                For Each r As DataRow In dt.Rows
                    Dim name As String = r("BankName").ToString()
                    Dim bal As Decimal = If(IsDBNull(r("ClosingBalance")), 0D, Convert.ToDecimal(r("ClosingBalance")))
                    pieSeries.Points.AddXY(name, bal)
                Next

                Chart1.Series.Add(pieSeries)

                Chart2.Series.Clear()
                Chart2.ChartAreas.Clear()
                Chart2.ChartAreas.Add(New ChartArea("BarArea"))

                Dim barSeries As New Series("ClosingBalance")
                barSeries.ChartType = SeriesChartType.Column
                barSeries.IsValueShownAsLabel = True
                barSeries.LabelFormat = "N2"

                For Each r As DataRow In dt.Rows
                    Dim name As String = r("BankName").ToString()
                    Dim bal As Decimal = If(IsDBNull(r("ClosingBalance")), 0D, Convert.ToDecimal(r("ClosingBalance")))
                    barSeries.Points.AddXY(name, bal)
                Next

                Chart2.Series.Add(barSeries)

                Chart2.ChartAreas("BarArea").AxisX.Interval = 1

            Catch ex As Exception
                MessageBox.Show("Error loading bank chart:" & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Bank_Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadBankCharts()
        LoadBankDashboard()
        LoadCurrencies()
    End Sub
End Class