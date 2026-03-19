Imports System.Data.OleDb

Public Class Campaign_Activity_Scheduler
    Private Sub SaveActivity(activityStatus As String)
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim query As String = "INSERT INTO CampaignActivities" & "(CampaignID, Channel, Send"

        End Using
    End Sub
End Class