Imports System.Data.OleDb
Imports System.Diagnostics

Public Class Payable

    Public Property SelectedSupplierID As String
    Private SelectedAccount_ID As String

    Public Property SelectedCustomer As String
    Public Property InvoiceAmount As Decimal
    Public Property SelectedTax As String
    Public Property SelectedQty As Integer

    Private Sub Payable_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox2.Text = SelectedSupplierID
        TextBox3.Text = SelectedCustomer
        TextBox5.Text = InvoiceAmount.ToString("0.00")
        TextBox6.Text = SelectedTax
        TextBox7.Text = SelectedQty.ToString

        DateTimePicker1.Value = Date.Today
        DateTimePicker2.Value = Date.Today

        LoadAcc()

    End Sub

    '================ LOAD ACCOUNTS =================

    Private Sub LoadAcc()

        Using conn As New OleDbConnection(ConnectionString)

            conn.Open()

            Dim da As New OleDbDataAdapter(
            "SELECT Account_ID,Account_Name 
             FROM Bank_Account 
             WHERE IsActive=True", conn)

            Dim dt As New DataTable

            da.Fill(dt)

            ComboBox1.DataSource = dt
            ComboBox1.DisplayMember = "Account_Name"
            ComboBox1.ValueMember = "Account_ID"

            ComboBox1.SelectedIndex = -1

        End Using

    End Sub

    '================ ACCOUNT SELECT =================

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object,
    e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ComboBox1.SelectedIndex >= 0 Then

            SelectedAccount_ID =
            ComboBox1.SelectedValue.ToString()

        End If

    End Sub

    '================ DEDUCT BANK MONEY =================

    Private Function AdjustAmount(amount As Decimal) As Boolean

        Try

            Using conn As New OleDbConnection(ConnectionString)

                conn.Open()

                Dim currentBalance As Decimal = 0

                Using cmd As New OleDbCommand(
                "SELECT Closing_Balance 
 FROM Bank_Account 
 WHERE Account_ID = ?", conn)

                    cmd.Parameters.AddWithValue("?", SelectedAccount_ID)

                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing Then

                        MessageBox.Show("Account not found")

                        Return False

                    End If

                    currentBalance = CDec(result)

                End Using

                If currentBalance < amount Then

                    MessageBox.Show("Insufficient Funds. Available R" &
                    currentBalance)

                    Return False

                End If

                Using cmd As New OleDbCommand(
                "UPDATE Bank_Account 
 SET Closing_Balance=
 Closing_Balance-?
 WHERE Account_ID=?", conn)

                    cmd.Parameters.AddWithValue("?", amount)

                    cmd.Parameters.AddWithValue("?", SelectedAccount_ID)

                    cmd.ExecuteNonQuery()

                End Using

            End Using

            Return True

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            Return False

        End Try

    End Function

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

    '================ PAYMENT BUTTON =================

    Private Sub Button2_Click(sender As Object,
    e As EventArgs) Handles Button2.Click

        If String.IsNullOrEmpty(SelectedAccount_ID) Then

            MessageBox.Show("Select bank account")

            Exit Sub

        End If

        Dim invoice As Decimal
        Dim paid As Decimal

        If Not Decimal.TryParse(TextBox5.Text,
        invoice) Then Exit Sub

        If Not Decimal.TryParse(TextBox4.Text,
        paid) Then Exit Sub

        If paid <= 0 Then

            MessageBox.Show("Invalid payment")

            Exit Sub

        End If

        If Not AdjustAmount(paid) Then Exit Sub

        Dim NewBalance As Decimal =
        invoice - paid

        Dim Status As String

        If NewBalance > 0 Then

            Status = "Partially Paid"

        ElseIf NewBalance = 0 Then

            Status = "Paid"

        Else

            Status = "Over Paid"

        End If

        TextBox8.Text = Status

        TextBox1.Text =
        Math.Abs(NewBalance).ToString("0.00")

        Dim PreviousOwed =
        GetSupplierOutstanding()

        If PreviousOwed > 0 Then

            Dim res = MessageBox.Show(
            "Supplier owed " &
            PreviousOwed.ToString("C2") &
            " Pay now?",
            "Outstanding",
            MessageBoxButtons.YesNo)

            If res = DialogResult.No Then Exit Sub

        End If

        Try

            Using conn As New OleDbConnection(ConnectionString)

                conn.Open()

                '======= INSERT PAYMENT =======

                Using cmd As New OleDbCommand("INSERT INTO Cart
(Supplier_ID,PayDate,DueDate,
Supplier,Account_Name,Owed,
Amount,Tax,Quantity,Status,Payable)

VALUES (?,?,?,?,?,?,?,?,?,?,?)", conn)



                    cmd.Parameters.AddWithValue("?",
                    SelectedSupplierID)

                    cmd.Parameters.AddWithValue("?",
                    DateTimePicker1.Value)

                    cmd.Parameters.AddWithValue("?",
                    DateTimePicker2.Value)

                    cmd.Parameters.AddWithValue("?",
                    TextBox3.Text)

                    cmd.Parameters.AddWithValue("?",
                    ComboBox1.Text)

                    cmd.Parameters.AddWithValue("?",
                    CDec(TextBox1.Text))

                    cmd.Parameters.AddWithValue("?",
                    invoice)

                    cmd.Parameters.AddWithValue("?",
                    TextBox6.Text)

                    cmd.Parameters.AddWithValue("?",
                    CInt(TextBox7.Text))

                    cmd.Parameters.AddWithValue("?",
                    Status)

                    cmd.Parameters.AddWithValue("?",
                    paid)

                    cmd.ExecuteNonQuery()

                End Using

                '======= UPDATE ORDER =======

                Using cmd As New OleDbCommand("UPDATE [Order]
SET Unit_Cost=?,
Status=?
WHERE PO_ID=?", conn)



                    cmd.Parameters.AddWithValue("?",
NewBalance)

                    cmd.Parameters.AddWithValue("?",
                    Status)

                    cmd.Parameters.AddWithValue("?",
                    SelectedSupplierID)

                    cmd.ExecuteNonQuery()

                End Using

                '======= UPDATE STOCK =======

                Using cmd As New OleDbCommand("UPDATE ProductManagement
SET Current_Stock=
IIF(Current_Stock Is Null,0,
Current_Stock)+?
WHERE ProductID=?", conn)



                    cmd.Parameters.AddWithValue("?",
                    SelectedQty)

                    cmd.Parameters.AddWithValue("?",
                    SelectedSupplierID)

                    cmd.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show(
            "Payment saved successfully")

        Catch ex As Exception

            MessageBox.Show(ex.Message)

            Debug.WriteLine(ex)

        End Try

    End Sub

    '================ AUTO CALCULATE BALANCE =================

    Private Sub TextBox4_TextChanged(sender As Object,
    e As EventArgs) Handles TextBox4.TextChanged

        Dim invoice As Decimal
        Dim paid As Decimal

        Decimal.TryParse(TextBox5.Text,
        invoice)

        Decimal.TryParse(TextBox4.Text,
        paid)

        TextBox1.Text =
        (invoice - paid).ToString("0.00")

    End Sub

End Class