Imports System.IO

Public Class About_Us
    Private Sub About_Us_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '  ShowUserGuide()

    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim filePath As String = "C:\Users\Refilwe\Documents\ERP GUIDELINE.pdf"

            If Not File.Exists(filePath) Then
                MessageBox.Show("Guide file not found in Downloads." & vbCrLf & filePath,
                                "Guide",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
                Exit Sub
            End If

            Process.Start(filePath)

        Catch ex As Exception
            MessageBox.Show("Error opening guide: " & ex.Message,
                            "Guide",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub ShowUserGuide()
    '    Try
    '        Dim Temp As String = Path.Combine(Path.GetTempPath(), "C:\Users\Refilwe\Documents\Screen Dump.docx")
    '        File.WriteAllBytes(Temp, My.Resources.ScreenDump)
    '        WebBrowser1.Navigate(Temp)
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

End Class