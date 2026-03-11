Imports System.Data.OleDb

Public Class Payable

    Public Property selectedsupplier As String
    Public Property selectedAccountID As String
    Public Property selectedCustomer As String
    Public Property InvoiceAmount As String
    Public Property selectedVAT As String
    Public Property selectedDate As String
    Public Property selectedDue As String
    Public Property selectedQty As String

    Private Sub Payable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = selectedsupplier
        TextBox3.Text = selectedCustomer
        TextBox5.Text = InvoiceAmount.ToString()
        TextBox6.Text = selectedVAT
        TextBox7.Text = selectedQty
        DateTimePicker2.Text = Date.Today
        LoadAcc

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex >= 0 Then

            selectedAccountID = ComboBox1.SelectedValue.ToString
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand("SELECT ClosingBalance FROM BankAccount WHERE AccountID=?", conn)
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = selectedAccountID
                    Dim Results = cmd.ExecuteScalar()
                End Using
            End Using
        End If
    End Sub
    Private Function AdjustAmount(Amount As Integer) As Boolean
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                Dim currentBalance As Integer = 0
                Using cmd As New OleDbCommand("SELECT ClosingBalance FROM BankAccount WHERE AccountID WHERE AccountID =?", conn)
                    cmd.Parameters.AddWithValue("?", selectedAccountID)
                    Dim result = cmd.ExecuteScalar
                    If result Is Nothing Then
                        MessageBox.Show($"Account Update.")
                        Return False
                    End If
                    currentBalance = CInt(result)
                End Using

                If currentBalance < Amount Then
                    MessageBox.Show($"Insufficient funds Only R{currentBalance} Available.", "Funds Check", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
                Using cmd As New OleDbCommand("Update BankAccount SET ClosingBalance = ClosingBalance-? WHERE AccountID = ?", conn)
                    cmd.Parameters.AddWithValue("?", Amount)
                    cmd.Parameters.AddWithValue("?", selectedAccountID)
                    cmd.ExecuteNonQuery()

                End Using

            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Error updating stock" & ex.Message)
            Return False
        End Try
    End Function
    Private Sub LoadAcc()
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim da As New OleDbDataAdapter("SELECT AccountID, BankName FROM BankAccount WHERE IsActive", conn)

            Dim dt As New DataTable
            da.Fill(dt)
            ComboBox1.DataSource = dt
            ComboBox1.DisplayMember = "BankName"
            ComboBox1.ValueMember = "AccountID"
            ComboBox1.SelectedIndex = -1
        End Using
    End Sub
    Private Sub UpdateStockAfterPurchase(conn As OleDbConnection, qty As Integer)

        conn.Open()

        Using cmd As New OleDbCommand(
    "UPDATE Product_Details
                SET Current_Stock = IIf(Current_Stock Is Null, 0,Current_Stock) + ?
               WHERE Product_Name = ?", conn)

            cmd.Parameters.Add("@Current_Stock", OleDbType.Integer).Value = qty
            cmd.Parameters.Add("@Product", OleDbType.VarWChar).Value = TextBox2.Text

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim InvoiceAmount As Decimal
        Dim Paid As Decimal
        Decimal.TryParse(TextBox5.Text, InvoiceAmount)
        Decimal.TryParse(TextBox4.Text, Paid)
        Dim Owed = InvoiceAmount - Paid
        TextBox1.Text = Owed.ToString("0.00")
    End Sub
    Private Function GetSupplierOutstanding(SupplierID As String) As Decimal
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Using cmd As New OleDbCommand("SELECT Owed FROM Cart WHERE SupplierID = ?", conn)
                cmd.Parameters.Add("?", OleDbType.VarChar).Value = selectedsupplier
                Dim Results = cmd.ExecuteScalar
                If Results Is Nothing OrElse IsDBNull(Results) Then
                    Return 0D
                Else
                    Return Convert.ToDecimal(cmd.ExecuteScalar)
                End If
            End Using
        End Using
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Amount As Decimal
        If Not Decimal.TryParse(TextBox5.Text, Amount) OrElse Amount <= 0D Then
            MessageBox.Show("Amount must be greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Val(TextBox4.Text) <= 0 Then
            MessageBox.Show("Invalid amount paid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If selectedAccountID = 0 Then
            MessageBox.Show("select a BankAccount")
            Exit Sub
        End If
        Dim BalanceDue As Decimal
        If Not Decimal.TryParse(TextBox1.Text, BalanceDue) Then
            MessageBox.Show("Invalid BalanceDue.")
            Exit Sub
        End If
        If Not AdjustAmount(Amount) Then
        End If
        Dim OverPay As Decimal = 0D
        Dim NewBalance As Decimal = BalanceDue - Amount
        Dim PurchaseStatus As String
        If NewBalance > 0 Then
            PurchaseStatus = "Partially Paid"
        ElseIf NewBalance = 0 Then
            PurchaseStatus = "Paid"
        Else
            PurchaseStatus = "OverPaid"
        End If
        OverPay = Math.Abs(NewBalance)
        Dim paid As Decimal = CDec(TextBox4.Text)
        Dim Status As String = If(paid >= InvoiceAmount, "Paid", "Partial")

        Dim OrderQty As Integer
        If Not Integer.TryParse(TextBox7.Text, OrderQty) OrElse OrderQty <= 0 Then
            MessageBox.Show("Order quantity must be greater than zero.")
            Exit Sub
        End If

        Dim PreviousOwed = GetSupplierOutstanding(selectedsupplier)

        If PreviousOwed > 0 Then
            Dim Res = MessageBox.Show("This supplier is owed" & PreviousOwed.ToString("C2") & "Do you want to pay it now?", "OutstandingBalance", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If Res = DialogResult.No Then Exit Sub
        End If
        TextBox1.Text = NewBalance.ToString()
        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()

                Using cmd As New OleDbCommand(
                    "INSERT INTO [Cart]
                   ([SupplierID], [PayDate], [DueDate], [Supplier], 
                    [BankName], [Owed], [Amount], [VAT], [Quantity], [Status], [Payable])
                    VALUES (?,?,?,?,?,?,?,?,?,?,?)", conn)
                    cmd.Parameters.AddWithValue("@POID", selectedsupplier)
                    cmd.Parameters.AddWithValue("@PayDate", DateTimePicker1)
                    cmd.Parameters.AddWithValue("@DueDate", DateTimePicker1)
                    cmd.Parameters.AddWithValue("?", ComboBox1.Text)
                    cmd.Parameters.AddWithValue("?", TextBox3.Text)
                    cmd.Parameters.AddWithValue("?", TextBox1.Text)
                    cmd.Parameters.AddWithValue("?", TextBox6.Text)
                    cmd.Parameters.AddWithValue("?", TextBox5.Text)
                    cmd.Parameters.AddWithValue("?", TextBox8.Text)
                    cmd.Parameters.AddWithValue("?", TextBox4.Text)
                    cmd.Parameters.AddWithValue("?", TextBox7.Text)
                    cmd.ExecuteNonQuery()
                End Using
                Using cmd As New OleDbCommand(
                    "UPDATE [Order] SET [Amount] = ? , [Status] = ? WHERE [POID] = ?", conn)
                    cmd.Parameters.AddWithValue("?", NewBalance)
                    cmd.Parameters.AddWithValue("?", PurchaseStatus)
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = selectedsupplier
                    cmd.Parameters.AddWithValue("@PayDate", DateTimePicker1)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmdStock As New OleDbCommand(
                    "UPDATE Product_Details SET Current_Stock = IIf(Current_Stock Is Null, 0, Current_Stock) + ?", conn)
                    cmdStock.ExecuteNonQuery()

                End Using
            End Using
            MessageBox.Show("Purchase Order Paid Successfully", "Success")
        Catch ex As Exception
            MessageBox.Show("Error saving order:" & ex.Message)
            Debug.WriteLine(ex.ToString)
        End Try
        TextBox8.Text = Status.ToString()
    End Sub
End Class