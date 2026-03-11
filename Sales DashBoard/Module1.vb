Imports System.Data.OleDb
Imports System.IO

Module Module1
    Public ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Public ReadOnly ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"
    Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Rama's IT Centre.accdb")

    Public Function Getconnection() As OleDbConnection
        Return New OleDbConnection(ConnectionString)
    End Function

    Public Function SafeDec(value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D
        If IsNumeric(value.ToString()) Then
            Return CDec(Val(value.ToString()))
        End If
        Return 0D
    End Function
    Public Function SafeInt(value As Object) As Integer
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0
        If IsNumeric(value.ToString()) Then
            Return CInt(Val(value.ToString()))
        End If
        Return 0
    End Function
    Public Function SafeStr(value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then Return ""
        Return value.ToString()
    End Function
#Region "SKU"
    Private ReadOnly _skuPrefix As String
        Private ReadOnly _counterFile As String = Path.Combine(Application.StartupPath, "C:\Temp\sku_counter.txt")

        Private Function LoadCounter() As Integer
            If File.Exists(_counterFile) Then
                Dim text = File.ReadAllText(_counterFile)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter(value As Integer)
            File.WriteAllText(_counterFile, value.ToString())
        End Sub

        Public Function GenerateSku() As String
            Dim current = LoadCounter() + 1
            SaveCounter(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _skuPrefix, datePart, seqPart)
        End Function

#End Region

#Region "Supplier ID"
        Private ReadOnly _supplierIDPrefix As String
        Private ReadOnly _counterFile1 As String = Path.Combine(Application.StartupPath, "C:\Temp\supplierID_counter.txt")

        Private Function LoadCounter1() As Integer
            If File.Exists(_counterFile1) Then
                Dim text = File.ReadAllText(_counterFile1)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter1(value As Integer)
            File.WriteAllText(_counterFile1, value.ToString())
        End Sub

        Public Function GenerateSupplierID() As String
            Dim current = LoadCounter1() + 1
            SaveCounter1(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _supplierIDPrefix, datePart, seqPart)
        End Function
#End Region

#Region "Product ID"
        Private ReadOnly _Product_IDPrefix As String
        Private ReadOnly _counterFile2 As String = Path.Combine(Application.StartupPath, "C:\Temp\ProductID_counter.txt")

        Private Function LoadCounter2() As Integer
            If File.Exists(_counterFile2) Then
                Dim text = File.ReadAllText(_counterFile2)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter2(value As Integer)
            File.WriteAllText(_counterFile2, value.ToString())
        End Sub

        Public Function GenerateProduct_ID() As String
            Dim current = LoadCounter2() + 1
            SaveCounter2(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _Product_IDPrefix, datePart, seqPart)
        End Function
#End Region

#Region "TransactionNumber"
        Private ReadOnly _TransactionNumberPrefix As String
        Private ReadOnly _counterFile3 As String = Path.Combine(Application.StartupPath, "C:\Temp\TransactionNumber_counter.txt")

        Private Function LoadCounter3() As Integer
            If File.Exists(_counterFile3) Then
                Dim text = File.ReadAllText(_counterFile3)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter3(value As Integer)
            File.WriteAllText(_counterFile3, value.ToString())
        End Sub

        Public Function GenerateTransactionNumber() As String
            Dim current = LoadCounter3() + 1
            SaveCounter3(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _TransactionNumberPrefix, datePart, seqPart)
        End Function
#End Region

#Region "InvoiceID"
        Private ReadOnly _InvoiceIDPrefix As String
        Private ReadOnly _counterFile4 As String = Path.Combine(Application.StartupPath, "C:\Temp\InvoiceID_counter.txt")

        Private Function LoadCounter4() As Integer
            If File.Exists(_counterFile4) Then
                Dim text = File.ReadAllText(_counterFile4)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter4(value As Integer)
            File.WriteAllText(_counterFile4, value.ToString())
        End Sub

        Public Function GenerateInvoiceID() As String
            Dim current = LoadCounter4() + 1
            SaveCounter4(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _InvoiceIDPrefix, datePart, seqPart)
        End Function
#End Region

#Region "InvoiceNo"
        Private _InvoiceNOCounter As Integer = 0
        Private ReadOnly _InvoiceNOPrefix As String = "INV NO"

        Public Function GenerateInvoiceNO() As String
            _InvoiceNOCounter += 1
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = _InvoiceNOCounter.ToString("D4")
            Return String.Format("{0}{1}-{2}", _InvoiceNOPrefix, datePart, seqPart)
        End Function

#End Region
#Region "PO_ID"
        Private ReadOnly _PO_IDPrefix As String
        Private ReadOnly _counterFiles As String = Path.Combine(Application.StartupPath, "C:\Temp\PO_ID_counter.txt")

        Private Function LoadCounters() As Integer
            If File.Exists(_counterFile) Then
                Dim text = File.ReadAllText(_counterFile)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounters(value As Integer)
            File.WriteAllText(_counterFile, value.ToString())
        End Sub

        Public Function GeneratePO_ID() As String
            Dim current = LoadCounters() + 1
            SaveCounter(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _skuPrefix, datePart, seqPart)
        End Function

#End Region

#Region "refence"
        Private ReadOnly _refencePrefix As String
        Private ReadOnly _counterFile5 As String = Path.Combine(Application.StartupPath, "C:\Temp\refence_counter.txt")

        Private Function LoadCounter5() As Integer
            If File.Exists(_counterFile4) Then
                Dim text = File.ReadAllText(_counterFile4)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter5(value As Integer)
            File.WriteAllText(_counterFile4, value.ToString())
        End Sub

        Public Function Generaterefence() As String
            Dim current = LoadCounter5() + 1
            SaveCounter5(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _refencePrefix, datePart, seqPart)
        End Function
#End Region
#Region "ACCID"
        Private ReadOnly _ACCIDPrefix As String
        Private ReadOnly _counterFile7 As String = Path.Combine(Application.StartupPath, "C:\Temp\ACCID_counter.txt")

        Private Function LoadCounter7() As Integer
            If File.Exists(_counterFile7) Then
                Dim text = File.ReadAllText(_counterFile4)
                If Integer.TryParse(text, Nothing) Then
                    Return Math.Max(0, CInt(text))
                End If
            End If
            Return 0
        End Function

        Private Sub SaveCounter7(value As Integer)
            File.WriteAllText(_counterFile4, value.ToString())
        End Sub

        Public Function GenerateACCID() As String
            Dim current = LoadCounter5() + 1
            SaveCounter5(current)
            Dim datePart As String = DateTime.Today.ToString("yyMMdd")
            Dim seqPart As String = current.ToString("D4")
            Return String.Format("{0}{1}-{2}", _ACCIDPrefix, datePart, seqPart)
        End Function
#End Region

    End Module

