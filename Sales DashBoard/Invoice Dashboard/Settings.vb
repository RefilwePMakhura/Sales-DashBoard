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
    'Private Sub SaveEmailSettings()
    '    Try
    '        SaveSetting("SmtpHost", If(TextBox8 IsNot Nothing, TextBox8.Text.Trim(), ""))
    '        Dim portValue As Integer
    '        If TextBox9 IsNot Nothing AndAlso Integer.TryParse(TextBox9.Text.Trim(), portValue) Then
    '            SaveSetting("SmtpPort", portValue)
    '        End If
    '        SaveSetting("SmtpSslEnable", If(CheckBox1 IsNot Nothing AndAlso CheckBox1.Checked, True, False))
    '        SaveSetting("SmtpFrom", If(TextBox6 IsNot Nothing, TextBox6.Text.Trim(), ""))
    '        SaveSetting("SmtpAuth", If(CheckBox2 IsNot Nothing AndAlso CheckBox2.Checked, True, False))
    '        SaveSetting("SmtpUsername", If(TextBox10 IsNot Nothing, TextBox10.Text.Trim(), ""))
    '        SaveSettings("SmtpPassword", If(TextBox11 IsNot Nothing, TextBox11.Text, ""))
    '    Catch
    '    End Try
    'End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        My.Settings.UserName = TextBox10.Text
        ' My.Settings.Port = TextBox2.Text
        My.Settings.Save()
    End Sub
End Class