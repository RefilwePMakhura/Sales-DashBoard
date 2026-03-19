

Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Mail
Imports System.Diagnostics

Public Class Payable

    Public Property SelectedSupplierID As String
    Public Property SelectedProductID As String
    Public Property SelectedPO_ID As String
    Public Property SelectedOrderID As String
    Public Property SelectedCustomer As String
    Public Property InvoiceAmount As Decimal
    Public Property SelectedTax As String
    Public Property SelectedQty As Integer

    Private SelectedAccount_ID As String = ""

    Private Sub Payable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = SelectedPO_ID
        TextBox1.Text = SelectedCustomer
        TextBox3.Text = InvoiceAmount.ToString("0.00")   ' Invoice Amount
        TextBox4.Text = ""                               ' Amount Paid
        TextBox5.Text = InvoiceAmount.ToString("0.00")   ' Balance
        TextBox6.Text = SelectedTax
        TextBox7.Text = SelectedQty.ToString()
        TextBox8.Text = "Pending"

        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today
        ' ComboBox1.Items.Clear()
        'ComboBox1.Items.AddRange(New String() {"Absa", "Standard Bank"})
        'ComboBox1.SelectedIndex = 0
        LoadAcc()
    End Sub

    Private Sub LoadAcc()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim da As New OleDbDataAdapter(
                "SELECT [BankAccountID], [BankName] FROM [BankAccount] WHERE [IsActive]=True", conn)

                Dim dt As New DataTable
                da.Fill(dt)

                ComboBox1.DataSource = dt
                ComboBox1.DisplayMember = "BankName"
                ComboBox1.ValueMember = "BankAccountID"

                If ComboBox1.Items.Count > 0 Then
                    ComboBox1.SelectedIndex = 0
                Else
                    ComboBox1.SelectedIndex = -1
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Failed to load bank accounts: " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedValue IsNot Nothing AndAlso Not IsDBNull(ComboBox1.SelectedValue) Then
            SelectedAccount_ID = ComboBox1.SelectedValue.ToString()
        End If
    End Sub

    '================ GET SUPPLIER BALANCE =================

    Private Function GetSupplierOutstanding() As Decimal

        Using conn As New OleDbConnection(ConnectionString)

            conn.Open()

            Using cmd As New OleDbCommand(
            "SELECT Owed 
FROM Cart 
WHERE Supplier_ID=?", conn)

                cmd.Parameters.AddWithValue("?", SelectedSupplierID)

                Dim result = cmd.ExecuteScalar()

                If result Is Nothing OrElse
                IsDBNull(result) Then

                    Return 0

                Else

                    Return CDec(result)

                End If

            End Using

        End Using

    End Function


    Private Function GetOutstandingForPO() As Decimal
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("SELECT Nz([Total],0) FROM [Order] WHERE [PO_ID]=?", conn)
                    cmd.Parameters.AddWithValue("?", SelectedPO_ID)

                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        Return 0D
                    Else
                        Return Convert.ToDecimal(result)
                    End If
                End Using
            End Using

        Catch
            Return 0D
        End Try
    End Function

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim invoice As Decimal = 0D
        Dim paid As Decimal = 0D

        Decimal.TryParse(TextBox3.Text, invoice)
        Decimal.TryParse(TextBox4.Text, paid)

        Dim balance As Decimal = invoice - paid
        If balance < 0 Then balance = 0D

        TextBox5.Text = balance.ToString("0.00")
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'If ComboBox1.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(SelectedAccount_ID) Then
        '    MessageBox.Show("Select bank account.")
        '    Exit Sub
        'End If
        If String.IsNullOrEmpty(
        SelectedAccount_ID) Then

            MessageBox.Show("Select bank account")

            Exit Sub

        End If

        Dim invoice As Decimal
        Dim paid As Decimal

        If Not Decimal.TryParse(
        TextBox5.Text, invoice) Then Exit Sub

        If Not Decimal.TryParse(
        TextBox4.Text, paid) Then Exit Sub

        If paid <= 0 Then

            MessageBox.Show("Invalid payment")

            Exit Sub

        End If
        If Not Decimal.TryParse(TextBox4.Text, paid) Then
            MessageBox.Show("Enter amount paid.")
            Exit Sub
        End If

        If paid <= 0 Then
            MessageBox.Show("Invalid payment amount.")
            Exit Sub
        End If

        'If TextBox4.Text > TextBox3.Text Then
        '    MessageBox.Show("Payment exceeds invoice amount.")
        '    Exit Sub
        'End If



        'If Not AdjustAmount(paid) Then Exit Sub

        Dim NewBalance As Decimal = TextBox5.Text
        'Math.Max(0, invoice - paid)




        Dim status As String
        If newBalance = 0D Then
            status = "Paid"
        Else
            status = "Partially Paid"
        End If

        TextBox5.Text = newBalance.ToString("0.00")
        TextBox8.Text = status

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim trans As OleDbTransaction = conn.BeginTransaction
                Try
                        'Dim currentBalance As Decimal = 0D

                        'Using cmd As New OleDbCommand(
                        '"SELECT IIf(IsNull([ClosingBalance]),0,[ClosingBalance]) FROM [BankAccount] WHERE [BankAccountID]=?",
                        'conn, trans)

                        '    cmd.Parameters.AddWithValue("?", ComboBox1.Text)
                        '    cmd.ExecuteScalar()
                        'End Using

                        'If TextBox4.Text < TextBox3.Text Then
                        '    MessageBox.Show("Insufficient funds.")
                        '    trans.Rollback()
                        '    Exit Sub
                        'End If


                        '                        Using cmd As New OleDbCommand(
                        '                "UPDATE BankAccount 
                        'SET ClosingBalance=
                        'ClosingBalance-?
                        'WHERE BankAccountID=?", conn)

                        '                            cmd.Parameters.AddWithValue("?", paid)

                        '                            cmd.Parameters.AddWithValue("?", SelectedAccount_ID)

                        '                            cmd.ExecuteNonQuery()

                        '                        End Using

                        Using cmd As New OleDbCommand(
                        "INSERT INTO [Cart] " &
                        "([SupplierID], [PayDate], [DueDate], [SupplierName], [AccountName], [Owed], [Amount], [Tax], [Quantity], [Status]) " &
                        "VALUES (?,?,?,?,?,?,?,?,?,?)",
                        conn, trans)

                            cmd.Parameters.AddWithValue("?", TextBox2.Text)
                            cmd.Parameters.AddWithValue("?", DateTimePicker1.Value)
                            cmd.Parameters.AddWithValue("?", DateTimePicker2.Value)
                            cmd.Parameters.AddWithValue("?", TextBox1.Text)
                            cmd.Parameters.AddWithValue("?", ComboBox1.Text)
                            cmd.Parameters.AddWithValue("?", newBalance)
                            cmd.Parameters.AddWithValue("?", invoice)
                            cmd.Parameters.AddWithValue("?", Val(TextBox6.Text))
                            cmd.Parameters.AddWithValue("?", Val(TextBox7.Text))
                            cmd.Parameters.AddWithValue("?", status)
                            cmd.ExecuteNonQuery()
                        End Using
                    Using cmdTrans As New OleDbCommand("INSERT INTO [BankTransaction] ([TransactionID], [BankAccount], [TransactionDate], [Amount], [Outstanding], [Type], [Reference], [ReferenceID])VALUES (?,?,?,?,?,?,?,?)", conn, trans)
                        cmdTrans.Parameters.AddWithValue("?", ComboBox1.SelectedValue)
                        cmdTrans.Parameters.AddWithValue("?", ComboBox1.Text)
                        cmdTrans.Parameters.AddWithValue("?", DateTimePicker1.Value.Date)
                        cmdTrans.Parameters.AddWithValue("?", paid)
                        cmdTrans.Parameters.AddWithValue("?", TextBox5.Text)
                        cmdTrans.Parameters.AddWithValue("?", "Withdrawal")
                        cmdTrans.Parameters.AddWithValue("?", "Orders")
                        cmdTrans.Parameters.AddWithValue("?", Generaterefence)
                        cmdTrans.ExecuteNonQuery()
                    End Using
                    Using cmd As New OleDbCommand(
                        "UPDATE [Order] SET [Total]=? WHERE [PO_ID]=?",
                        conn, trans)

                        cmd.Parameters.AddWithValue("?", NewBalance)
                        '   cmd.Parameters.AddWithValue("?", status)
                        cmd.Parameters.AddWithValue("?", SelectedPO_ID)

                        cmd.ExecuteNonQuery()
                    End Using

                    If Not String.IsNullOrWhiteSpace(SelectedProductID) Then
                            Using cmd As New OleDbCommand(
                            "UPDATE [Product_Details] " &
                            "SET [Current_Stock] = IIf(IsNull([Current_Stock]),0,[Current_Stock]) + ? " &
                            "WHERE [Product_ID] = ?",
                            conn, trans)

                                cmd.Parameters.AddWithValue("?", SelectedQty)
                                cmd.Parameters.AddWithValue("?", SelectedProductID)

                                cmd.ExecuteNonQuery()
                            End Using
                        End If

                        trans.Commit()
                        MessageBox.Show("Payment saved successfully.")

                    Catch ex As Exception
                        trans.Rollback()
                        MessageBox.Show("Transaction failed: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
                        Debug.WriteLine(ex.ToString())
                    End Try
                End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
            Debug.WriteLine(ex.ToString())
        End Try
    End Sub

    Public Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString())
    End Sub

    '================ EMAIL =================
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SendProofOfPaymentEmail()

    End Sub
    Private Sub SendProofOfPaymentEmail()



        Try
            '================ VALIDATION =================
            If String.IsNullOrWhiteSpace(TextBox9.Text) Then
                MessageBox.Show("Enter recipient email")
                Exit Sub
            End If

            '================ CREATE EMAIL =================
            Dim mail As New MailMessage()
            mail.From = New MailAddress("your_email@gmail.com") 'CHANGE THIS
            mail.To.Add(TextBox9.Text.Trim())
            mail.Subject = "Proof of Payment"

            mail.Body =
            "Proof of Payment" & vbCrLf & vbCrLf &
            "Payment Date: " & DateTimePicker2.Value.ToShortDateString() & vbCrLf &
            "Amount Paid: R" & TextBox3.Text & vbCrLf &
             "Tax: " & TextBox6.Text & vbCrLf &
            "Quantity: " & TextBox7.Text & vbCrLf & vbCrLf &
            "Thank you."

            '================ SMTP SETUP =================
            Dim smtp As New SmtpClient("smtp.gmail.com", 587)

            smtp.EnableSsl = True
            smtp.UseDefaultCredentials = False

            smtp.Credentials = New NetworkCredential(
            "refilwemakhura12@gmail.com",         'CHANGE THIS
            "pktb glrx opor dbky")       '⚠️ NOT your normal password

            smtp.Timeout = 30000

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            '================ SEND =================
            smtp.Send(mail)

            MessageBox.Show("✅ Email Sent Successfully")

        Catch ex As SmtpException
            MessageBox.Show("SMTP Error: " & ex.Message)

        Catch ex As Exception
            MessageBox.Show("General Error: " & ex.Message)
        End Try
    End Sub
    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr As New MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If String.IsNullOrEmpty(
        TextBox1.Text) Then

            MessageBox.Show(
            "Select supplier first")

            Exit Sub

        End If

        Dim PreviousOwed As Decimal =
        GetSupplierOutstanding()

        Dim CurrentInvoice As Decimal = 0

        Decimal.TryParse(
        TextBox5.Text,
        CurrentInvoice)

        Dim NewBalance As Decimal =
        CurrentInvoice + PreviousOwed

        TextBox1.Text =
        NewBalance.ToString("0.00")

        MessageBox.Show(
        "Previous owed added: " &
        PreviousOwed.ToString("C2"))

    End Sub

    Private Function AdjustAmount(amount As Decimal) As Boolean
        Try
            If String.IsNullOrWhiteSpace(SelectedAccount_ID) Then
                MessageBox.Show("Select bank account first.")
                Return False
            End If

            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Dim currentBalance As Decimal = 0D

                Using cmd As New OleDbCommand(
                    "SELECT IIf(IsNull([ClosingBalance]),0,[ClosingBalance]) FROM [BankAccount] WHERE [BankName]=?", conn)

                    cmd.Parameters.AddWithValue("?", ComboBox1.Text)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        MessageBox.Show("Account not found.")
                        Return False
                    End If

                    currentBalance = Convert.ToDecimal(result)
                End Using

                If currentBalance < amount Then
                    MessageBox.Show("Insufficient funds. Available R" & currentBalance.ToString("0.00"))
                    Return False
                End If

                Using cmd As New OleDbCommand(
                    "UPDATE [BankAccount] SET [ClosingBalance] = IIf(IsNull([ClosingBalance]),0,[ClosingBalance]) - ? WHERE [BankName]=?", conn)

                    cmd.Parameters.AddWithValue("?", amount)
                    cmd.Parameters.AddWithValue("?", ComboBox1.Text)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then
                        MessageBox.Show("Bank account was not updated.")
                        Return False
                    End If
                End Using
            End Using

            Return True

        Catch ex As Exception
            MessageBox.Show("AdjustAmount error: " & ex.Message)
            Return False
        End Try
    End Function
End Class