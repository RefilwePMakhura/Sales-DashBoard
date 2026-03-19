Imports System.Data.OleDb

Public Class Manage_User


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Session.CurrentRole <> "Admin" Then
            MessageBox.Show("Only Admin can activate users")
            Exit Sub
        End If
        If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim userId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ID_Number").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Login_Details SET IsActive=True WHERE ID_Number=?", conn)

            cmd.Parameters.AddWithValue("?", userId)
            cmd.ExecuteNonQuery()
        End Using

        MessageBox.Show("User activated")
        LoadUsers()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Session.CurrentRole <> "Admin" Then
            MessageBox.Show("Only Admin can deactivate users ")
            Exit Sub
        End If
        If DataGridView1.CurrentRow Is Nothing Then Exit Sub

        Dim userId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ID_Number").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                "UPDATE Login_Details SET IsActive=False WHERE ID_Number=?", conn)

            cmd.Parameters.AddWithValue("?", userId)
            cmd.ExecuteNonQuery()
        End Using

        MessageBox.Show("User deactivated")
        LoadUsers()

    End Sub
    Private Sub LoadUsers()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim da As New OleDbDataAdapter(
                "SELECT ID_Number, First_Name, User_Name, Role, IsActive FROM Login_Details", conn)

            Dim dt As New DataTable()
            da.Fill(dt)

            DataGridView1.DataSource = dt
        End Using

    End Sub
    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        If e.RowIndex < 0 Then Exit Sub

        If DataGridView1.Rows(e.RowIndex).Cells("IsActive").Value IsNot Nothing Then

            Dim isActive As Boolean = Convert.ToBoolean(DataGridView1.Rows(e.RowIndex).Cells("IsActive").Value)

            If isActive Then
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGreen
            Else
                DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightCoral
            End If

        End If

    End Sub
    Private Sub ManageUsers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("Adim")
        ComboBox1.Items.Add("Manager")
        ComboBox1.Items.Add("Staff")
        LoadUsers()
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        If DataGridView1.CurrentRow IsNot Nothing Then
            ComboBox1.Text = DataGridView1.CurrentRow.Cells("Role").Value.ToString()
        End If

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Session.CurrentRole <> "Admin" Then
            MessageBox.Show("Only Admin can change roles")
            Exit Sub
        End If

        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Select a user first")
            Exit Sub
        End If

        If ComboBox1.Text = "" Then
            MessageBox.Show("Select a role")
            Exit Sub
        End If

        Dim userId As Integer = Convert.ToInt32(DataGridView1.CurrentRow.Cells("ID_Number").Value)

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()

            Dim cmd As New OleDbCommand(
                    "UPDATE Login_Details SET Role=? WHERE ID_Number=?", conn)

            cmd.Parameters.AddWithValue("?", ComboBox1.Text)
            cmd.Parameters.AddWithValue("?", userId)

            cmd.ExecuteNonQuery()
        End Using

        MessageBox.Show("Role updated successfully")
        LoadUsers()

    End Sub

End Class