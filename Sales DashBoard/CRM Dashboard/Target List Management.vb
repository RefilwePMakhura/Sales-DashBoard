Imports System.Data.OleDb

Public Class Target_List_Management
    Private Sub Target_List_Management_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCampaign()
        LoadTarget()

        ComboBox2.Items.Add("Customer")
        ComboBox2.Items.Add("Lead")
    End Sub
    Private Sub LoadCampaign()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim da As New OleDbDataAdapter("SELECT CampaignID, Name FROM Campaigns", conn)
                Dim dt As New DataTable

                da.Fill(dt)

                ComboBox1.DataSource = dt
                ComboBox1.DisplayMember = "Name"
                ComboBox1.ValueMember = "CampaignID"
                conn.Close()
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LoadTarget()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim cmd As New OleDbCommand("INSERT INTO CampaignTargets(CampaignID, ContactType, ContactID, AddDate, OptOut)VALUES (?,?,?,?,?)", conn)
                cmd.Parameters.AddWithValue("CampaignID", ComboBox1.SelectedValue)
                cmd.Parameters.AddWithValue("ContactType", ComboBox2.Text)
                cmd.Parameters.AddWithValue("CampaignID", ComboBox1.SelectedValue)
                cmd.Parameters.AddWithValue("CampaignID", ComboBox1.SelectedValue)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("")
        End Try
    End Sub
End Class