Imports System.Data.OleDb
Public Class Settings
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            'clear old settings (only one row allowed)
            Dim clearCmd As New OleDbCommand("DELETE FROM CompanySettings", conn)
            clearCmd.ExecuteNonQuery()

            'Insert new settings
            Dim cmd As New OleDb.OleDbCommand("INSERT INTO CompanySettings(CompanyName,Address,Phone,Email,LogoPath) VALUES (?,?,?,?,?)", conn)
            cmd.Parameters.AddWithValue("?", TextBox1.Text)
            cmd.Parameters.AddWithValue("?", TextBox2.Text)
            cmd.Parameters.AddWithValue("?", TextBox3.Text)
            cmd.Parameters.AddWithValue("?", TextBox4.Text)
            cmd.Parameters.AddWithValue("?", TextBox5.Text)

            cmd.ExecuteNonQuery()
        End Using
        MessageBox.Show("Company details saved successfully")

    End Sub

    Public Sub LoadCompanyInfo()
        Using conn As New OleDb.OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand("SELECT TOP 1 * FROM CompanySettings", conn)
            Using dr As OleDb.OleDbDataReader = cmd.ExecuteReader
                If dr.Read() Then
                    TextBox1.Text = dr("CompanyName").ToString()
                    TextBox2.Text = dr("Address").ToString()
                    TextBox3.Text = dr("Phone").ToString()
                    TextBox4.Text = dr("Email").ToString

                    Dim logoPath As String = dr("LogoPath").ToString()
                    If IO.File.Exists(logoPath) Then
                        PictureBoxLogo.Image = Image.FromFile(logoPath)
                    End If
                End If
            End Using
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Image files| * .png;.jpg;.jpeg"
            If ofd.ShowDialog = DialogResult.OK Then
                TextBox5.Text = ofd.FileName
                PictureBoxLogo.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub

    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCompanyInfo()
    End Sub
End Class