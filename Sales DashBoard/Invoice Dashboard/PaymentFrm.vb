Imports System.IO
Imports System.Data.OleDb
Public Class PaymentFrm
    Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Rama's IT Centre.accdb")

    Private SelectedAccount_ID As String = ""
    Public Property SelectedSupplierID As String
    Public Property SelectedPO_ID As String
    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbPaymentMethod.Items.Add("Cash")
        cmbPaymentMethod.Items.Add("Voucher")
        cmbPaymentMethod.Items.Add("Card")
        cmbPaymentMethod.Items.Add("EFT")
        ComboBox1.Items.AddRange(New String() {"Standard", "Absa"})
        LoadAcc()
        '      txtInvoiceNo.Text = SelectedPO_ID
        ' cmbStatus.Items.AddRange(New String() {"Pending", "Paid"})
        'If TextBox1.Text = "Paid" Then
        '    btnSavePayment.Enabled = False
        'Else
        '    btnSavePayment.Enabled = True
        'End If
        'LoadBankAccounts()
        '    UpdateInvoiceAsPaid()
    End Sub

    ' Calculate change as user types
    Private Sub txtAmountPaid_TextChanged(sender As Object, e As EventArgs) Handles txtAmountPaid.TextChanged
        Dim total As Decimal
        Dim paid As Decimal

        Decimal.TryParse(txtTotalAmount.Text, total)
        Decimal.TryParse(txtAmountPaid.Text, paid)

        If paid >= total Then
            txtChange.Text = (paid - total).ToString("0.00")
        Else
            txtChange.Text = "0.00"
        End If

        Dim Amount As Decimal
        Dim newBalance As Decimal
        Dim AmountPaid As Decimal

        Decimal.TryParse(txtTotalAmount.Text, Amount)
        Decimal.TryParse(txtChange.Text, newBalance)
        Decimal.TryParse(txtAmountPaid.Text, AmountPaid)

        If AmountPaid < Amount Then
            txtChange.Text = (Amount - AmountPaid).ToString("F2")
        Else
            txtChange.Text = "0.00"
        End If

    End Sub
    Private Function GetOutstandingForPO() As Decimal
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("SELECT Nz([Total],0) FROM [Invoice_Details] WHERE [InvoiceID]=?", conn)
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
    Private Function GetSupplierOutstanding() As Decimal

        Using conn As New OleDbConnection(ConnectionString)

            conn.Open()

            Using cmd As New OleDbCommand(
            "SELECT Owed 
FROM Payment 
WHERE InvoiceID=?", conn)

                cmd.Parameters.AddWithValue("?", SelectedPO_ID)

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


    ' PROCESS PAYMENT BUTTON (APPLY / PAY)
    'Private Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
    '    Try
    '        If String.IsNullOrWhiteSpace(txtInvoiceNo.Text) OrElse String.IsNullOrWhiteSpace(txtAmountPaid.Text) Then
    '            MessageBox.Show("Please fill required fields")
    '            Exit Sub
    '        End If
    '        Dim totalDue As Decimal
    '        Dim paymentAmount As Decimal
    '        If Not Decimal.TryParse(txtTotalAmount.Text, totalDue) OrElse Not Decimal.TryParse(txtAmountPaid.Text, paymentAmount) Then
    '            MessageBox.Show("Invalid Amount")
    '            Exit Sub
    '        End If
    '        If paymentAmount <= 0 Then
    '            MessageBox.Show("Invalid")
    '            Exit Sub
    '        End If
    '        Dim newBalance As Decimal = totalDue - paymentAmount
    '        Dim Status As String = If(newBalance <= 0, "Paid", "Pending")
    '        Using conn As New OleDbConnection(ConnectionString)
    '            conn.Open()
    '            Using cmd As New OleDbCommand("INSERT INTO [Payment] ([Invoice_Number], [Payment_Date], [Customer], [Total_Amount], [Payment_Method],[Amount_Paid], [Status]) VALUES (?,?,?,?,?,?,?)", conn)
    '                cmd.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '                cmd.Parameters.AddWithValue("?", DateTimePicker1.Value.Date)
    '                cmd.Parameters.AddWithValue("?", cmbCustomer.Text)
    '                cmd.Parameters.AddWithValue("?", txtTotalAmount.Text)
    '                cmd.Parameters.AddWithValue("?", cmbPaymentMethod.Text)
    '                cmd.Parameters.AddWithValue("?", txtAmountPaid.Text)
    '                '  cmd.Parameters.AddWithValue("?", txtChange.Text)
    '                cmd.Parameters.AddWithValue("?", TextBox1.Text)

    '                cmd.ExecuteNonQuery()
    '            End Using

    '            Using cmdUpdate As New OleDbCommand("Update [Invoice_Details] SET TotalWthVAT=?, Status=? WHERE InvoiceID=?", conn)
    '                cmdUpdate.Parameters.AddWithValue("?", Math.Max(newBalance, 0))
    '                cmdUpdate.Parameters.AddWithValue("?", TextBox1.Text)
    '                cmdUpdate.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '                cmdUpdate.ExecuteNonQuery()

    '            End Using

    '            Using cmdTrans As New OleDbCommand("INSERT INTO [BankTransaction] ([TransactionID],[BankAccount],[BankAccountID],[TransactionDate],[Amount],[Type],[ReferenceType],[ReferenceID],[Notes]", conn)
    '                cmdTrans.Parameters.AddWithValue("?", cmbPaymentMethod.SelectedValue)
    '                cmdTrans.Parameters.AddWithValue("?", DateTimePicker1.Value.Date)
    '                cmdTrans.Parameters.AddWithValue("?", txtAmountPaid.Text)
    '                'cmdTrans.Parameters.AddWithValue("?", "Deposit")
    '                'cmdTrans.Parameters.AddWithValue("?", "Invoice_Details")
    '                cmdTrans.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '                cmdTrans.ExecuteNonQuery()
    '            End Using
    '            Using cmdBal As New OleDbCommand("Update INTO [BankAccount] SET ClosingBalance= CDbl (ClosingBalance)+ CDbl (?) ", conn)
    '                cmdBal.Parameters.AddWithValue("?", OleDbType.Currency).Value = paymentAmount
    '                cmdBal.Parameters.AddWithValue("?", OleDbType.Integer).Value = CInt(cmbPaymentMethod.SelectedValue)

    '                cmdBal.ExecuteNonQuery()
    '            End Using
    '        End Using
    '        ' ParentForm.LoadData()
    '        MessageBox.Show(If(Status = "Paid", "Invoice_Details Paid", "Partial payment recorded"))
    '        Me.Close()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    'Public Sub RunMonthlyRollover()
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()
    '        Dim cmd As New OleDbCommand("UPDATE [BankAccount] SET OpeningBalance = ClosingBalance, LastRolloverDate = Date() Where Month (LastRolloverDate)<> Month(Date()) OR Year(Date)) ", conn)
    '        cmd.ExecuteNonQuery()

    '    End Using
    'End Sub
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
    ' SAVE PAYMENT BUTTON
    Private Sub btnSavePayments_Click(sender As Object, e As EventArgs) Handles btnSavePayment.Click



        Dim paymentAmount As Decimal = CDec(txtAmountPaid.Text)
        Dim InvoiceNo As String = txtInvoiceNo.Text

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim trans As OleDbTransaction = conn.BeginTransaction

            Try



                Dim getcmd As New OleDbCommand(
                    "SELECT Total_Amount FROM Invoice_Details WHERE InvoiceID= ?", conn, trans)
                getcmd.Parameters.AddWithValue("@Total_Amount", txtTotalAmount)
                getcmd.Parameters.AddWithValue("@InvoiceID", InvoiceNo)

                getcmd.ExecuteReader()
                'dr.Read()

                Dim invoice As Decimal
                Dim paid As Decimal

                If Not Decimal.TryParse(
        txtTotalAmount.Text, invoice) Then Exit Sub

                If Not Decimal.TryParse(
        txtAmountPaid.Text, paid) Then Exit Sub

                If paid <= 0 Then

                    MessageBox.Show("Invalid payment")

                    Exit Sub

                End If
                If Not Decimal.TryParse(txtAmountPaid.Text, paid) Then
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



                If Not AdjustAmount(paid) Then Exit Sub

                Dim NewBalance As Decimal =
        Math.Max(0, invoice - paid)


                Dim status As String
                If NewBalance = 0D Then
                    status = "Paid"
                Else
                    status = "Partially Paid"
                End If

                txtChange.Text = NewBalance.ToString()
                TextBox1.Text = status

                'txtTotalAmount.Text = NewBalance
                Dim updatecmd As New OleDbCommand(
                    "UPDATE Invoice_Details
                    SET Status = ?
                    WHERE InvoiceID =?", conn, trans)

                updatecmd.Parameters.AddWithValue("@Status", status)
                updatecmd.Parameters.AddWithValue("@InvoiceID", InvoiceNo)
                ' updatecmd.Parameters.AddWithValue("@Total_Amount", NewBalance)
                updatecmd.ExecuteNonQuery()


                Dim cmd As New OleDbCommand("INSERT INTO [Payment] ([InvoiceID], [Total_Amount], [Owed], [Payment_Method], [Amount_Paid], [Status])" & "VALUES (?,?,?,?,?,?)", conn, trans)
                cmd.Parameters.AddWithValue("InvoiceID", txtInvoiceNo.Text)
                cmd.Parameters.AddWithValue("Total_Amount", txtTotalAmount.Text)
                cmd.Parameters.AddWithValue("Owed", txtChange.Text)
                cmd.Parameters.AddWithValue("Payment_Method", cmbPaymentMethod.Text)
                cmd.Parameters.AddWithValue("Amount_Paid", txtAmountPaid.Text)
                cmd.Parameters.AddWithValue("Status", status)
                cmd.ExecuteNonQuery()

                Using cmdStock As New OleDbCommand("UPDATE [BankAccount] SET ClosingBalance = IIf(ClosingBalance Is Null, 0, ClosingBalance) + ? WHERE BankName = ?", conn, trans)

                    cmdStock.Parameters.AddWithValue("ClosingBalance", OleDbType.Integer).Value = txtAmountPaid.Text
                    cmdStock.Parameters.AddWithValue("BankName", OleDbType.VarChar).Value = ComboBox1.Text
                    cmdStock.ExecuteNonQuery()
                End Using


                Using cmdTrans As New OleDbCommand("INSERT INTO [BankTransaction] ([TransactionID], [BankAccount], [TransactionDate], [Amount], [Outstanding], [Type], [Reference], [ReferenceID])VALUES (?,?,?,?,?,?,?,?)", conn, trans)
                    cmdTrans.Parameters.AddWithValue("?", txtInvoiceNo.Text)
                    cmdTrans.Parameters.AddWithValue("?", ComboBox1.Text)
                    cmdTrans.Parameters.AddWithValue("?", DateTimePicker1.Value.Date)
                    cmdTrans.Parameters.AddWithValue("?", paymentAmount)
                    cmdTrans.Parameters.AddWithValue("?", txtChange.Text)
                    cmdTrans.Parameters.AddWithValue("?", "Deposit")
                    cmdTrans.Parameters.AddWithValue("?", "Invoice")
                    cmdTrans.Parameters.AddWithValue("?", Generaterefence)
                    cmdTrans.ExecuteNonQuery()
                End Using
                Using cmdoutstanding As New OleDbCommand(
                        "UPDATE [Invoice_Details] SET [Total]=? WHERE [InvoiceID]=?",
                        conn, trans)

                    cmdoutstanding.Parameters.AddWithValue("?", NewBalance)
                    '   cmd.Parameters.AddWithValue("?", status)
                    cmd.Parameters.AddWithValue("?", SelectedPO_ID)

                    cmd.ExecuteNonQuery()
                End Using
                trans.Commit()
                MessageBox.Show("Payment Saved Successfully")
                conn.Close()
                UpdateInvoiceAsPaid()
                frmInvoiceManagement.UpdateInvoiceStatus()
                BankAccounts.LoadBankAccount()

                If Application.OpenForms().OfType(Of Order_Form).Any Then
                    Dim frm As Order_Form = Application.OpenForms().OfType(Of Order_Form).First

                    'frm.LoadOrderData()
                    '       frm.ColorGrid()
                End If

            Catch ex As Exception
                'ShowStack()
                trans.Rollback()
                MessageBox.Show("Error saving payment:" & vbCrLf & ex.Message, "Error")
                MessageBox.Show("Stack Trace:", ex.StackTrace)
            End Try
        End Using

    End Sub
    Private Sub ShowStack()
        Dim st As New StackTrace(True)
        MessageBox.Show(st.ToString)
    End Sub
    Private Sub SavePayment()

    End Sub


    '  MAIN PAYMENT PROCESS FUNCTION
    Private Sub ProcessPayment()
        ' Validate payment method
        If cmbPaymentMethod.SelectedIndex = -1 Then
            MessageBox.Show("Please select a payment method.", "Payment Method Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate amount paid
        If txtAmountPaid.Text.Trim() = "" Then
            MessageBox.Show("Please enter the amount paid.", "Amount Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim total As Decimal
        Dim paid As Decimal

        If Not Decimal.TryParse(txtTotalAmount.Text, total) Then
            MessageBox.Show("Invalid total amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not Decimal.TryParse(txtAmountPaid.Text, paid) Then
            MessageBox.Show("Invalid amount paid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Check if enough money is given
        If paid < total Then
            MessageBox.Show("Amount paid is less than total.", "Insufficient Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Calculate change
        txtChange.Text = (paid - total).ToString("0.00")

        ' Update invoice status immediately
        UpdateInvoiceAsPaid()

        ' Save payment to CSV
        SavePayment()

        ' Success message
        MessageBox.Show("Payment saved successfully and invoice marked as Paid!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Close form
        Me.Close()
    End Sub

    ' Update Invoice Management datagrid
    Public Sub UpdateInvoiceAsPaid()
        For Each row As DataGridViewRow In frmInvoiceManagement.dgvInvoiceRecords.Rows
            row.Cells("Status").Value = "Paid"
            row.DefaultCellStyle.BackColor = Color.LightGreen
            Exit For
        Next
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtCustomer_TextChanged(sender As Object, e As EventArgs)

    End Sub



    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedValue IsNot Nothing AndAlso Not IsDBNull(ComboBox1.SelectedValue) Then
            SelectedAccount_ID = ComboBox1.SelectedValue.ToString()
        End If

    End Sub

End Class

