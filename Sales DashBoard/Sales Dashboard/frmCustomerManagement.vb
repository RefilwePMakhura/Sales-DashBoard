Imports System.IO

Public Class frmCustomerManagement

    Private ReadOnly _dataFilePath As String = "C:\Temp\CustomerManagement.txt"

    Private Sub LoadFromFileToGridDirect()
        If Not IO.File.Exists(_dataFilePath) Then Return

        Try
            DataGridView1.Rows.Clear() ' Start fresh

            Dim content As String = IO.File.ReadAllText(_dataFilePath)
            Dim recordSeparator As String = "----"
            Dim records As String() = content.Split(New String() {recordSeparator}, StringSplitOptions.RemoveEmptyEntries)

            For Each record As String In records
                Dim dataValues As New Dictionary(Of String, String)
                Dim lines As String() = record.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

                For Each line As String In lines
                    If String.IsNullOrWhiteSpace(line) Then Continue For
                    Dim idx = line.IndexOf("=")
                    If idx <= 0 Then Continue For
                    dataValues(line.Substring(0, idx).Trim()) = line.Substring(idx + 1).Trim()
                Next

                Dim i As Integer = DataGridView1.Rows.Add()
                Dim r As DataGridViewRow = DataGridView1.Rows(i)

                Dim Contacts As Integer
                If Integer.TryParse(If(dataValues.ContainsKey("Contacts"), dataValues("Contacts"), ""), Contacts) Then
                    r.Cells("Column4").Value = Contacts
                End If
                Dim EmailAddress As Integer
                If Integer.TryParse(If(dataValues.ContainsKey("EmailAddress="), dataValues("EmailAddress="), ""), EmailAddress) Then
                    r.Cells("Column3").Value = EmailAddress
                End If

                Dim IDNumber As Integer
                If Integer.TryParse(If(dataValues.ContainsKey("IDNumber="), dataValues("IDNumber="), ""), IDNumber) Then
                    r.Cells("Column5").Value = IDNumber
                End If
                Dim Address As Double
                If Double.TryParse(If(dataValues.ContainsKey("Address="), dataValues("Address="), "0"), Address) Then
                    r.Cells("Column7").Value = Address
                Else
                    r.Cells("Column7").Value = 0
                End If



                r.Cells("Column1").Value = If(dataValues.ContainsKey("firstname"), dataValues("firstName"), firstName)
                r.Cells("Column2").Value = If(dataValues.ContainsKey("Surname"), dataValues("Surname"), Surname)


            Next

            DataGridView1.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    Sub SaveToFile()
        Dim directoryPath As String = "C:\Temp\Sales\Customer_Management"
        Dim filePath As String = System.IO.Path.Combine(directoryPath, "CustomerManagement.txt")

        ' Ensure the directory exists
        If Not System.IO.Directory.Exists(directoryPath) Then
            System.IO.Directory.CreateDirectory(directoryPath)
        End If

        ' Write all relevant variables to the file
        Using writer As New System.IO.StreamWriter(filePath, False) ' False to overwrite
            writer.WriteLine("FirstName=" & firstName)
            writer.WriteLine("Surname=" & Surname)
            writer.WriteLine("EmailAddress=" & EmailAddress)
            writer.WriteLine("Contacts=" & Contacts)
            writer.WriteLine("IdNumber=" & IDNumber)
            writer.WriteLine("Address=" & Address)

        End Using
    End Sub

    Dim firstName As String
    Dim namePrompt As String = "Enter Customer Name."

    Dim Surname As String
    Dim SurnamePrompt As String = "Enter Customer Surname."

    Dim EmailAddress As String
    Dim EmailPrompt As String = "Enter Customer Email."

    Dim Contacts As Integer
    Dim ContactPrompt As String = " Enter Customer Contacts."

    Dim IDNumber As Double
    Dim IDPrompt As String = "Enter Customer ID"

    Dim Address As String
    Dim AddressPrompt As String = "Enter Customer Physical Address."


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Form1.ShowDialog()

    End Sub

    Private Sub frmCustomerMgmtvb_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomerDetails()
        LoadFromFileToGridDirect()
        SaveToFile()
    End Sub

    Private Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click

        firstName = InputBox(namePrompt)
        TextBox1.Text = firstName

        Surname = InputBox(SurnamePrompt)
        TextBox2.Text = Surname

        EmailAddress = InputBox(EmailPrompt)
        TextBox3.Text = EmailAddress

        Contacts = InputBox(ContactPrompt)
        TextBox4.Text = Contacts

        IDNumber = InputBox(IDPrompt)
        TextBox5.Text = IDNumber

        Address = InputBox(AddressPrompt)
        TextBox6.Text = Address

        SaveToFile()
        MessageBox.Show("Customer Management Saved")

    End Sub

    Private Sub btnEditCustomer_Click(sender As Object, e As EventArgs) Handles btnEditCustomer.Click
        LoadFromFileToGridDirect()
    End Sub

    Private Sub LoadCustomerDetails()
        DataGridView1.Rows.Clear()
    End Sub
End Class