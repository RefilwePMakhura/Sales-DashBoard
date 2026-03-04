Imports System.Data.OleDb

Public Class Supplier

    Private Sub ClearForm()

        TextBox1.Clear()
        TextBox3.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox4.Clear()

    End Sub
    Private Sub Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadData()
        ClearForm()

    End Sub

    Private Sub dataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged

        If DataGridView1.CurrentRow Is Nothing Then Return

        ' Assuming you have columns named exactly as in your table or you can use index
        TextBox1.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("SupplierName").Value)
        TextBox3.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Contact_Person").Value)
        TextBox4.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("PhoneNumber").Value)
        TextBox5.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("EmailAddress").Value) ' key
        TextBox6.Text = Convert.ToString(DataGridView1.CurrentRow.Cells("Physical_Address").Value)

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Using cmd As New OleDbCommand("INSERT INTO [Supplier] ([SupplierName], [Contact_Person], [PhoneNumber], [EmailAddress],[Physical_Address]) VALUES (?,?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("@SupplierName", TextBox1.Text)
                    cmd.Parameters.AddWithValue("@Contact_Person", TextBox3.Text)
                    cmd.Parameters.AddWithValue("@PhoneNumber", TextBox4.Text)
                    cmd.Parameters.AddWithValue("@EmailAddress", TextBox5.Text)
                    cmd.Parameters.AddWithValue("@Physical_Address", TextBox6.Text)

                    cmd.ExecuteNonQuery()
                End Using

                conn.Close()
            End Using
            LoadData()
            ClearForm()
            MessageBox.Show("Saved successful!")

        Catch ex As Exception
            MessageBox.Show(
        $"Failed to save:{Environment.NewLine}{ex}",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error)


        End Try

        ' Configure DatGridView properties
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.MultiSelect = False
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill

    End Sub

    Private Sub LoadData()

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim sql As String = "SELECT * FROM [Supplier]"
                Dim adapter As New OleDbDataAdapter(sql, conn)
                Dim table As New DataTable
                adapter.Fill(table)
                DataGridView1.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadSuppliers()

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "SELECT [SupplierName], [Contact_Person],[EmailAddress],[Physical_Address] FROM [Supplier] ORDER BY [SupplierName]"
            Using cmd As New OleDbCommand(sql, conn)
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        TextBox1.Text = reader("SupplierName").ToString()
                        TextBox3.Text = reader("Contact_Person").ToString()
                        TextBox5.Text = reader("EmailAddress").ToString()
                        TextBox6.Text = reader("Physical_Address").ToString()
                    End While
                End Using
            End Using
            conn.Close()
        End Using
    End Sub
End Class