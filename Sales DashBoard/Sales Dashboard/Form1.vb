Imports System.IO
Imports System.Data.OleDb


Public Class Form1
    'Simple totals (Kept in memory while the app runs)
    Private amtNew As Decimal = 0D
    Private amtQualified As Decimal = 0D
    Private amtProposition As Decimal = 0D
    Private amtWon As Decimal = 0D
    Private totalCount As Integer = 0
    Dim zarCulture
    Private ReadOnly DBFile As String = "C:\Users\Refilwe\Documents\Visual Studio 2015\Projects\Sales DashBoard\Rama's IT Centre.accdb"
    Private ReadOnly ConnectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""{DBFile}"";Persist Security Info=False;"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadTotalsFromDatabase()
        RefreshLabels()
    End Sub

    Private Sub btnAddSales_Click(sender As Object, e As EventArgs) Handles btnAddSales.Click
        ' Dim FrmSalesDashboard As New frmSales
        ' frmSales.Show()

        Dim Form1 As New Add_Sales
        Form1.Owner = Me
        Form1.ShowDialog()
        ' Me.Hide() ' Hide instead of close if you want to reopen it later 


    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles pnlFunnelChart.Paint
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim brushColors() As Brush = {
            Brushes.LightCoral,
            Brushes.Orange,
            Brushes.Gold,
            Brushes.LightGreen
        }

        ' Coordinates for funnel layers: trapezoids decreasing in width
        Dim width As Integer = pnlFunnelChart.Width
        Dim height As Integer = pnlFunnelChart.Height

        ' Y heights of funnel parts
        Dim funnelSections As Integer = 4
        Dim sectionHeight As Integer = height \ funnelSections

        For i As Integer = 0 To funnelSections - 1
            ' Calculate width for top and bottom of trapezoid
            Dim topWidth As Integer = width - (i * width \ funnelSections)
            Dim bottomWidth As Integer = width - ((i + 1) * width \ funnelSections)

            Dim topX As Integer = (width - topWidth) \ 2
            Dim bottomX As Integer = (width - bottomWidth) \ 2

            Dim points() As Point = {
                New Point(topX, i * sectionHeight),
                New Point(topX + topWidth, i * sectionHeight),
                New Point(bottomX + bottomWidth, (i + 1) * sectionHeight),
                New Point(bottomX, (i + 1) * sectionHeight)
            }

            g.FillPolygon(brushColors(i), points)
            g.DrawPolygon(Pens.Black, points)

            ' Optional: Add text labels
            Dim font As New Font("Arial", 10, FontStyle.Bold)
            Dim text As String = $"Stage {i + 1}"
            Dim textSize = g.MeasureString(text, font)
            Dim textX = width \ 2 - textSize.Width \ 2
            Dim textY = i * sectionHeight + sectionHeight \ 2 - textSize.Height \ 2

            g.DrawString(text, font, Brushes.Black, textX, textY)
        Next

    End Sub



    'Load all saved rows from CSV And rebuild totals
    Public Sub LoadTotalsFromDatabase()
        'Debug.WriteLine("Loading")
        'Dim directoryPath As String = "C:\Temp"
        'Dim filePath As String = System.IO.Path.Combine(directoryPath, "sales_data.csv")

        ' Reset totals before rebuild
        amtNew = 0
        amtQualified = 0
        amtProposition = 0
        amtWon = 0
        totalCount = 0



        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            Dim sql As String = "SELECT Stage, SUM(Unit_Price) AS TotalAmt, COUNT(*) AS Cnt FROM [Sales_Details] GROUP BY Stage"
            Using cmd As New OleDbCommand(sql, conn)


                Using rdr = cmd.ExecuteReader()
                    While rdr.Read()
                        Dim stage As String = rdr("Stage").ToString().ToLower()
                        Dim total As Decimal = If(IsDBNull(rdr("TotalAmt")), 0D, CDec(rdr("TotalAmt")))
                        Dim cnt As Integer = CInt(rdr("Cnt"))

                        Select Case stage
                            Case "new" : amtNew += total
                            Case "qualified" : amtQualified += total
                            Case "proposition" : amtProposition += total
                            Case "won" : amtWon += total
                            Case Else
                        End Select

                        totalCount += cnt
                    End While
                End Using
            End Using
        End Using

        'If Not System.IO.File.Exists(filePath) Then
        '    ' Nothing saved yet; keep zeros
        '    Exit Sub
        'End If

        'Using reader As New System.IO.StreamReader(filePath)
        '    ' Read header first line
        '    If Not reader.EndOfStream Then
        '        reader.ReadLine()
        '    End If

        '    ' Read each data line
        '    While Not reader.EndOfStream
        '        Dim line As String = reader.ReadLine()

        '        ' Basic parse for our format:
        '        ' Customer (quoted), Product (quoted), Amount (number), Stage (quoted), SaleDate (yyyy-MM-dd)
        '        ' We only need Amount (index 2) and Stage (index 3) for totals.

        '        ' Simple CSV-split aware of quotes (beginner-friendly approach)
        '        Dim parts As List(Of String) = SplitCsvLine(line)

        '        If parts.Count >= 4 Then
        '            Dim unitprice As Decimal
        '            Dim Quantity = parts(3)
        '            Dim stage As String = parts(5).Trim()

        '            unitprice = unitprice * Quantity

        '            If Decimal.TryParse(parts(3), unitprice) Then
        '                ' Bucket by stage using Select Case
        '                Select Case stage.ToLower()
        '                    Case "new" : amtNew += unitprice
        '                    Case "qualified" : amtQualified += unitprice
        '                    Case "proposition" : amtProposition += unitprice
        '                    Case "won" : amtWon += unitprice

        '                    Case Else
        '                        ' Unknown stage: ignore
        '                End Select

        '                totalCount += 1
        '            End If
        '        End If
        '    End While
        'End Using
    End Sub





    Private Function SplitCsvLine(line As String) As List(Of String)
        Dim result As New List(Of String)()
        Dim current As New System.Text.StringBuilder()
        Dim inQuotes As Boolean = False

        For i As Integer = 0 To line.Length - 1
            Dim ch As Char = line(i)

            If ch = """"c Then
                ' Quote encountered
                If inQuotes Then
                    ' Check for escaped quote ("")
                    Dim isEscaped As Boolean = (i + 1 < line.Length AndAlso line(i + 1) = """"c)
                    If isEscaped Then
                        current.Append(""""c)
                        i += 1 ' skip the second quote
                    Else
                        inQuotes = False
                    End If
                Else
                    inQuotes = True
                End If
            ElseIf ch = ","c AndAlso Not inQuotes Then
                ' Field separator
                result.Add(current.ToString())
                current.Clear()
            Else
                current.Append(ch)
            End If
        Next

        ' Add last field
        result.Add(current.ToString())

        Return result
    End Function


    Public Sub RefreshLabels()
        ' Show amounts as South African Rand (ZAR)
        Label9.Text = amtNew.ToString("C", zarCulture)
        Label10.Text = amtQualified.ToString("C", zarCulture)
        Label11.Text = amtProposition.ToString("C", zarCulture)
        Label13.Text = amtWon.ToString("C", zarCulture)

        ' Show total number of saved sales in this session
        Label8.Text = totalCount.ToString()
    End Sub


    ' Called from the ADD/Save form after a sale is saved
    Public Sub ApplySale(stage As String, UnitPrice As Decimal)
        'Normalize stage and update totals 
        Select Case stage.Trim().ToLower()
            Case "new"
                amtNew += UnitPrice
            Case "qualified"
                amtQualified += UnitPrice
            Case "Proposition"
                amtProposition += UnitPrice
            Case "won"
                amtWon += UnitPrice
            Case Else
                'Unkown stage; do nothing 



        End Select

        totalCount += 1
        RefreshLabels()
    End Sub



    Private Sub btnSaleReport_Click(sender As Object, e As EventArgs) Handles btnSaleReport.Click

        Dim FrmSalesDashboard As New frmSalesReport
        frmSalesReport.Show()
        Me.Hide() ' Hide instead of close if you want to reopen it later 
    End Sub

    Private Sub btnCustomerMgmt_Click(sender As Object, e As EventArgs) Handles btnCustomerMgmt.Click
        Dim FrmSalesDashboard As New frmCustomerManagement
        frmCustomerManagement.ShowDialog()
        'Me.Hide() ' Hide instead of close if you want to reopen it later 

    End Sub

    Private Sub btnProductMgmt_Click(sender As Object, e As EventArgs) Handles btnProductMgmt.Click
        Dim FrmSalesDashboard As New frmProductManagement
        frmProductManagement.ShowDialog()
        Me.Hide() ' Hide instead of close if you want to reopen it later 

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnInvoiceMgmt_Click(sender As Object, e As EventArgs) Handles btnInvoiceMgmt.Click
        Dim FrmSalesDashboard As New frmInvoiceManagement
        frmInvoiceManagement.ShowDialog()
        ' Me.Hide() ' Hide instead of close if you want to reopen it later 

    End Sub

    Private Sub btnCRMDashboard_Click(sender As Object, e As EventArgs) Handles btnCRMDashBoard.Click
        Dim FrmSalesDashboard As New CRM_DashBoard
        CRM_DashBoard.ShowDialog()
        '  Me.Hide() ' Hide instead of close if you want to reopen it later 

    End Sub

    Private Sub btnInventoryDashboard_Click(sender As Object, e As EventArgs) Handles btnInventoryDashboard.Click
        Dim FrmSalesDashboard As New FrmInventory_Dashboard
        FrmInventory_Dashboard.ShowDialog()
        '  Me.Hide() ' Hide instead of close if you want to reopen it later 

    End Sub

    Friend Sub LoadTotalFromFile()

    End Sub

    Private Sub btnManage_Click(sender As Object, e As EventArgs) Handles btnManage.Click
        Dim f As New Customer_Details()
        'Me.Hide() ' Hide instead of close if you want to reopen later

        f.Owner = Me   ' so it call ReloadCustomerCombo after add
        f.ShowDialog(Me)   ' modal, simple for beginners

        'After closing, ensure the latest list is visible

        ReloadCustomerCombo()

    End Sub

    Private Sub ReloadCustomerCombo()

    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FrmSalesDashboard As New Bank_Dashboard
        Bank_Dashboard.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim FrmSalesDashboard As New Sales_Order
        Sales_Order.ShowDialog()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class