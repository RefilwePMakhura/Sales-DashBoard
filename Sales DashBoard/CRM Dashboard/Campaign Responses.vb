Imports System.Data.OleDb
Imports System.IO

Public Class Campaign_Responses
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to export")
            Return
        End If

        Try
            Dim csvPath As String = Path.combine(Application.StartupPath, "CampaignExport.csv")
            Using sw As New StreamWriter(csvPath)
                Dim headers = DataGridView1.Columns.Cast(Of DataGridViewColumn)().Select(Function(c) c.HeaderText)
                sw.WriteLine()

            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadCampaigns()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT CampaignName FROM Campaigns"
                Dim dt As New DataTable
                Using adapter As New OleDbDataAdapter(sql, conn)
                    adapter.Fill(dt)
                    ComboBox1.DataSource = dt
                    ComboBox1.DisplayMember = "CampaignName"
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading campaigns:" & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using conn As New OleDbConnection(ConnectionString)

                conn.Open()
                Dim sql As String = "SELECT * FROM CampaignResponses WHERE CampaignName = ? AND ResponseTpye = ? "
                Dim dt As New DataTable

                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Name", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("@Tpye", ComboBox2.Text)

                    Dim adapter As New OleDbDataAdapter(cmd)
                    adapter.Fill(dt)
                    DataGridView1.DataSource = dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching response:" & ex.Message)
        End Try
    End Sub

    Private Sub Campaign_Responses_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCampaigns()

        ComboBox2.Items.AddRange({"Email", "Phone", "Social Media", "Web Form "})
    End Sub
End Class