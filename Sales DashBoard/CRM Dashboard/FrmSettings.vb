Imports System.Data.OleDb
Imports System.Net.Mail

Public Class FrmSettings




    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
        Private ReadOnly ConnectionString As String =
        $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"

        Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            LoadCombos()
            LoadSettingsFromDatabase()
        End Sub

        Private Sub LoadCombos()
            ComboBox1.Items.Clear()
            ComboBox1.Items.AddRange(New String() {"Light", "Dark", "Blue"})

            ComboBox2.Items.Clear()
            ComboBox2.Items.AddRange(New String() {"Dashboard", "Sales", "Inventory", "Reports"})
        End Sub

        Private Sub LoadSettingsFromDatabase()
            Try
                Using conn As New OleDbConnection(ConnectionString)
                    conn.Open()

                    Dim sql As String = "SELECT TOP 1 [Theme], [DefaultPage], [Notifications], [NotificationEmail] FROM [AppSettings] ORDER BY [SettingID]"
                    Using cmd As New OleDbCommand(sql, conn)
                        Using dr As OleDbDataReader = cmd.ExecuteReader()
                            If dr.Read() Then
                                ComboBox1.Text = dr("Theme").ToString()
                                ComboBox2.Text = dr("DefaultPage").ToString()
                                CheckBox1.Checked = CBool(dr("Notifications"))
                                TextBox1.Text = dr("NotificationEmail").ToString()
                            Else
                                ComboBox1.Text = "Light"
                                ComboBox2.Text = "Dashboard"
                                CheckBox1.Checked = False
                                TextBox1.Clear()
                            End If
                        End Using
                    End Using
                End Using

                TextBox1.Enabled = CheckBox1.Checked

            Catch ex As Exception
                MessageBox.Show("Failed to load settings: " & ex.Message)
            End Try
        End Sub

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
            If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
                MessageBox.Show("Please select a theme.")
                Exit Sub
            End If

            If String.IsNullOrWhiteSpace(ComboBox2.Text) Then
                MessageBox.Show("Please select a default page.")
                Exit Sub
            End If

            If CheckBox1.Checked Then
                If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                    MessageBox.Show("Please enter notification email.")
                    Exit Sub
                End If

                If Not IsValidEmail(TextBox1.Text.Trim()) Then
                    MessageBox.Show("Invalid notification email.")
                    Exit Sub
                End If
            End If

            SaveSettingsToDatabase()
        End Sub

        Private Sub SaveSettingsToDatabase()
            Try
                Using conn As New OleDbConnection(ConnectionString)
                    conn.Open()

                'Dim existsSql As String = "SELECT COUNT(*) FROM [AppSettings]"
                'Dim recordCount As Integer

                'Using existsCmd As New OleDbCommand(existsSql, conn)
                '    recordCount = CInt(existsCmd.ExecuteScalar())
                'End Using

                'If recordCount = 0 Then
                Dim insertSql As String =
                        "INSERT INTO [AppSettings] ([Theme], [DefaultPage], [Notifications], [NotificationEmail]) " &
                        "VALUES (?, ?, ?, ?)"

                        Using insertCmd As New OleDbCommand(insertSql, conn)
                            insertCmd.Parameters.AddWithValue("?", ComboBox1.Text)
                            insertCmd.Parameters.AddWithValue("?", ComboBox2.Text)
                            insertCmd.Parameters.AddWithValue("?", CheckBox1.Checked)
                            insertCmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())
                            insertCmd.ExecuteNonQuery()
                        End Using
                    'Else
                '    Dim updateSql As String =
                '    "UPDATE [AppSettings] SET [Theme]=?, [DefaultPage]=?, [Notifications]=?, [NotificationEmail]=? " &
                '    "WHERE [SettingID] = (SELECT TOP 1 [SettingID] FROM [AppSettings] ORDER BY [SettingID])"

                '    Using updateCmd As New OleDbCommand(updateSql, conn)
                '        updateCmd.Parameters.AddWithValue("?", ComboBox1.Text)
                '        updateCmd.Parameters.AddWithValue("?", ComboBox2.Text)
                '        updateCmd.Parameters.AddWithValue("?", CheckBox1.Checked)
                '        updateCmd.Parameters.AddWithValue("?", TextBox1.Text.Trim())
                '        updateCmd.ExecuteNonQuery()
                '    End Using
                'End If
            End Using

                MessageBox.Show("Settings saved successfully.")

            Catch ex As Exception
                MessageBox.Show("Failed to save settings: " & ex.Message)
            End Try
        End Sub

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
            LoadSettingsFromDatabase()
        End Sub

        Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
            Me.Close()
        End Sub

        Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
            TextBox1.Enabled = CheckBox1.Checked

            If Not CheckBox1.Checked Then
                TextBox1.Clear()
            End If
        End Sub

        Private Function IsValidEmail(email As String) As Boolean
            Try
                Dim addr As New MailAddress(email)
                Return addr.Address = email
            Catch
                Return False
            End Try
        End Function

    End Class