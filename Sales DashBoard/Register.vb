Imports System.Data.OleDb
Imports System.IO


Public Class Register

    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Private ReadOnly ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"
        Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Username As String = TextBox6.Text.Trim()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                ' 1) Check if the username already exists
                Using checkCmd As New OleDbCommand("SELECT COUNT(*) FROM [Login_Details] WHERE [User_Name] = ?", conn)
                    checkCmd.Parameters.AddWithValue("@p1", Username)
                    Dim userExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If userExists > 0 Then

                        MessageBox.Show("Username is already taken. Please choose a different username.")
                        Return
                    End If
                End Using

                ' 2) Validate password match before inserting
                If TextBox7.Text <> TextBox8.Text Then
                    MessageBox.Show("Password does not match")
                    Return
                End If


              
                conn.Close()

            End Using
            MessageBox.Show("Register successful!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(
        $"Failed to save:{Environment.NewLine}{ex}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)


        End Try
    End Sub
End Class