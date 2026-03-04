Public Class CRM_DashBoard
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim FrmCRMDashboard As New FrmSettings
        FrmSettings.Show()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim FrmCRMDashboard As New FrmNewLead
        FrmNewLead.Show()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim FrmCRMDashboard As New Contact_Log
        Contact_Log.Show()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim CRMDashboard As New FrmAnalysis
        FrmAnalysis.Show()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Me.Close()
    End Sub
End Class

Friend Class FrmContactLog
    Friend Shared Sub Show()

    End Sub
End Class
