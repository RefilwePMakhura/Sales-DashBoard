Public Class Session
    Public Shared CurrentUser As String
    Public Shared CurrentRole As String
    Public Shared CurrentUserID As Integer

    Public Shared Sub Clear()
        CurrentUserID = 0
        CurrentUser = ""
        CurrentRole = ""
    End Sub
End Class
