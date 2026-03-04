Imports System.IO
Imports System.Data.OleDb
Public Class PaymentFrm
    Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Rama's IT Centre.accdb")
    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbPaymentMethod.Items.Add("Cash")
        cmbPaymentMethod.Items.Add("Voucher")
        cmbPaymentMethod.Items.Add("Card")
        cmbPaymentMethod.Items.Add("EFT")
        ComboBox1.Items.AddRange(New String() {"Standard", "Absa", "Nedbank", "Capitec"})
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

    ' SAVE PAYMENT BUTTON
    Private Sub btnSavePayments_Click(sender As Object, e As EventArgs) Handles btnSavePayment.Click



        Dim paymentAmount As Decimal = CDec(txtAmountPaid.Text)
        Dim InvoiceNo As String = txtInvoiceNo.Text

        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim trans = conn.BeginTransaction()

            Try



            Dim getcmd As New OleDbCommand(
                "SELECT Total_Amount FROM Invoice_Details WHERE Invoice_ID= ?", conn, trans)

            getcmd.Parameters.AddWithValue("@Invoice_ID", InvoiceNo)

            Dim dr = getcmd.ExecuteReader()
            dr.Read()

            Dim currentPaid As Decimal
            Dim currentBalance As Decimal = CDec(dr("Total_Amount"))
            dr.Close()

            Dim newpaidTotal As Decimal = currentPaid + paymentAmount
            Dim NewBalance As Decimal = currentBalance - paymentAmount



            Dim newStatus As String

            If NewBalance < 0 Then
                NewBalance = 0
                newStatus = "Paid"
            Else
                newStatus = "Partially Paid"
            End If


                'txtTotalAmount.Text = NewBalance
                Dim updatecmd As New OleDbCommand(
                    "UPDATE Invoice_Details
                    SET Status = ?
                    WHERE Invoice_ID =?", conn, trans)

                updatecmd.Parameters.AddWithValue("@Status", newStatus)
            updatecmd.Parameters.AddWithValue("@Invoice_ID", InvoiceNo)
                ' updatecmd.Parameters.AddWithValue("@Total_Amount", NewBalance)
                updatecmd.ExecuteNonQuery()


                Dim cmd As New OleDbCommand("INSERT INTO [Payment] ([Invoice_Number], [Customer], [Total_Amount], [Payment_Method], [Amount_Paid],[Status])" & "VALUES (?,?,?,?,?,?)", conn, trans)
                '   cmd.Parameters.AddWithValue("@Payment_Date", DateTimePicker1.Value.Date)
                cmd.Parameters.AddWithValue("@Customer", txtChange.Text)
            cmd.Parameters.AddWithValue("@Total_Amount", txtTotalAmount.Text)
            cmd.Parameters.AddWithValue("@Payment_Method", cmbPaymentMethod.Text)
            cmd.Parameters.AddWithValue("@Amount_Paid", txtAmountPaid.Text)
                cmd.Parameters.AddWithValue("@Status", TextBox1.Text)
                cmd.Parameters.AddWithValue("@Invoice_Number", txtInvoiceNo.Text)
            cmd.ExecuteNonQuery()


                '    Dim cmd As New OleDbCommand("UPDATE Invoice_Details SET Total_Amount =?, Status =? WHERE Invoice_ID =?", conn, trans)
                'cmd.Parameters.AddWithValue("@BankName", NewBalance)
                'cmd.Parameters.AddWithValue("@ClosingBalance", newStatus)
                'cmd.Parameters.AddWithValue("@Reference", GenerateInvoiceID)
                'cmd.ExecuteNonQuery()



                Using cmdStock As New OleDbCommand("UPDATE [BankAccount] SET ClosingBalance = IIf(ClosingBalance Is Null, 0, ClosingBalance) + ? WHERE [BankName]", conn, trans)

                    cmdStock.Parameters.AddWithValue("ClosingBalance", OleDbType.Integer).Value = txtAmountPaid.Text
                    cmdStock.Parameters.AddWithValue("BankName", OleDbType.VarChar).Value = ComboBox1.Text
                    cmdStock.ExecuteNonQuery()
                End Using

                trans.Commit()
            MessageBox.Show("Payment Saved Successfully")
            conn.Close()
            UpdateInvoiceAsPaid()
            frmInvoiceManagement.UpdateInvoiceStatus()
            BankAccounts.ShowDialog()
                BankAccounts.LoadBankAccount()

                If Application.OpenForms().OfType(Of Order_Form).Any Then
                    Dim frm As Order_Form = Application.OpenForms().OfType(Of Order_Form).First

                    frm.LoadOrderData()
                    frm.ColorGrid()
                End If

            Catch ex As Exception
            ' trans.Rollback()
            MessageBox.Show("Error saving payement:" & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'MessageBox.Show("Stack Trace:", ex.StackTrace)
            End Try
        End Using

    End Sub
    Private Sub SavePayment()

    End Sub

    'Private Sub UpdateClosingBalance(BankName As Integer)
    '    Using conn As New OleDbConnection(ConnectionString)
    '        conn.Open()

    '        Dim cmdOpening As New OleDbCommand("SELECT OpeningBalance FROM BankAccount WHERE BankName =?", conn)

    '        cmdOpening.Parameters.AddWithValue("BankName", BankName)
    '        Dim 
    '    End Using
    'End Sub
    'Private Sub LoadBankAccounts()

    '    Using conn As New OleDbConnection(ConnectionString)
    '        Dim da As New OleDbDataAdapter("SELECT AccountNumber, BankName FROM [BankAccount] WHERE IsActive=True", conn)
    '        Dim dt As New DataTable
    '        da.Fill(dt)
    '        ComboBox1.DataSource = dt
    '        ComboBox1.DisplayMember = "BankName"
    '        ComboBox1.ValueMember = "AccountNumber"
    '        ComboBox1.SelectedIndex = -1
    '    End Using
    'End Sub



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

    '    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '        If txtInvoiceNo.Text = "" Then
    '            MessageBox.Show("Please enter Invoice Number")
    '            Exit Sub
    '        End If

    '        Dim conn As New OleDbConnection("Your_Connection_String_Here")
    '        conn.Open()

    '        Dim transaction As OleDbTransaction = conn.BeginTransaction()

    '        Try
    '            Dim cmdPayment As New OleDbCommand("
    'INSERT INTO Payment (InvoiceNo, Payment, Customer, TotalAmount,PaymentMethod, AmountPaid, BankAccount,Status)Values (?,?,?,?,?,?,?,?", conn, transaction)


    '            cmdPayment.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '            cmdPayment.Parameters.AddWithValue("?", DateTimePicker1.Value)
    '            cmdPayment.Parameters.AddWithValue("?", cmbStatus.Text)
    '            cmdPayment.Parameters.AddWithValue("?", cmbPaymentMethod.Text)
    '            cmdPayment.Parameters.AddWithValue("?", txtAmountPaid.Text)
    '            cmdPayment.Parameters.AddWithValue("?", txtTotalAmount.Text)
    '            cmdPayment.Parameters.AddWithValue("?", "Paid")
    '            cmdPayment.ExecuteNonQuery()

    '            Dim cmdInvoice As New OleDbCommand("
    'Update Invoices
    'Set Status = ?
    'WHERE InvoiceNumber =?", conn, transaction)

    '            cmdInvoice.Parameters.AddWithValue("?", "Paid")
    '            cmdInvoice.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '            cmdInvoice.ExecuteNonQuery()

    '            Dim cmdBank As New OleDbCommand("
    'Insert INTO BankTransaction(BankAccountID,TransactionDate,Amount,Type,ReferenceType,ReferenceID,Notes)VALUES (?,?,?,?,?,?,?", conn, transaction)
    '            cmdBank.Parameters.AddWithValue("?", ComboBox1.Text)
    '            cmdBank.Parameters.AddWithValue("?", DateTimePicker1.Value)
    '            cmdBank.Parameters.AddWithValue("?", txtAmountPaid.Text)
    '            cmdBank.Parameters.AddWithValue("?", "Deposit")
    '            cmdBank.Parameters.AddWithValue("?", "Create_Details")
    '            cmdBank.Parameters.AddWithValue("?", txtInvoiceNo.Text)
    '            cmdBank.Parameters.AddWithValue("?", "Invoice Payment")
    '            cmdBank.ExecuteNonQuery()

    '            transaction.Commit()
    '            MessageBox.Show("Payment Saved Successfully")

    '        Catch ex As Exception
    '            transaction.Rollback()
    '            MessageBox.Show("Error:" & ex.Message)
    '        Finally
    '            conn.Close()
    '        End Try
    '    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 0 Then
            '        '  txtAmountPaid.Text = ""
            '        Exit Sub
        End If

        '    Dim SelectedBank As String = ComboBox1.Text
        '    Dim ClosingBalance As Decimal = 

        '    Try
        '        Using conn As New OleDbConnection(ConnectionString)
        '            conn.Open()
        '            Using cmd As New OleDbCommand("SELECT ClosingBalance From [BankAccount] WHERE BankName =?", conn)
        '                cmd.Parameters.AddWithValue("?", SelectedBank)
        '                Dim result =
        '                        cmd.ExecuteScalar
        '                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
        '                    Decimal.TryParse(result.ToString(), ClosingBalance)
        '                End If

        '            End Using
        '        End Using
        '        '  txtAmountPaid.Text = ClosingBalance.ToString("0.00")
        '    Catch ex As Exception
        '        MessageBox.Show(ex.Message)
        '    End Try

    End Sub
End Class