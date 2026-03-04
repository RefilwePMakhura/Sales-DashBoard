Imports System.IO
Public Class Inventory_Report

    Private ReadOnly dataFilePath As String = Path.Combine("C:\Temp\inventory_reports", "reports.csv")


    ' --------------------------------------
    ' Button1: Save new report
    ' --------------------------------------
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If TextBox1.Text.Trim() = "" Or TextBox3.Text.Trim() = "" Then
                MessageBox.Show("Please fill in Report Number and Report details.", "Missing Information")
                Exit Sub
            End If

            Dim reportNumber As String = TextBox1.Text.Trim()
            Dim reportDate As String = DateTimePicker1.Value.ToShortDateString()
            Dim reportText As String = TextBox2.Text.Trim().Replace(",", ";")
            Dim feedback As String = TextBox3.Text.Trim().Replace(",", ";")

            ' Prevent duplicate Report Numbers
            If File.ReadAllLines(dataFilePath).Skip(1).Any(Function(line) line.StartsWith(reportNumber & ",")) Then
                MessageBox.Show("A report with this number already exists.", "Duplicate")
                Exit Sub
            End If

            ' Append new record
            Using writer As New StreamWriter(dataFilePath, True)
                writer.WriteLine($"{reportNumber},{reportDate},{reportText},{feedback}")
            End Using

            MessageBox.Show("Report saved successfully.", "Saved")

            ' Clear fields
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()

        Catch ex As Exception
            MessageBox.Show("Error saving report: " & ex.Message)
        End Try
    End Sub

    ' --------------------------------------
    ' Button2: Analyze reports
    ' --------------------------------------
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            If Not File.Exists(dataFilePath) OrElse File.ReadAllLines(dataFilePath).Length <= 1 Then
                MessageBox.Show("No reports to analyze.", "Analysis")
                Exit Sub
            End If

            Dim lines = File.ReadAllLines(dataFilePath).Skip(1).ToList()
            Dim totalReports As Integer = lines.Count
            Dim latestDate As Date = Date.MinValue
            Dim feedbackCount As Integer = 0

            For Each line In lines
                Dim parts = line.Split(","c)
                If parts.Length >= 4 Then
                    Dim d As Date
                    If Date.TryParse(parts(1), d) AndAlso d > latestDate Then latestDate = d
                    If Not String.IsNullOrWhiteSpace(parts(3)) Then feedbackCount += 1
                End If
            Next

            Dim summary As String =
                $"📊 Inventory Report Summary" & vbCrLf &
                $"Total Reports: {totalReports}" & vbCrLf &
                $"Most Recent Report Date: {latestDate.ToShortDateString()}" & vbCrLf &
                $"Reports with Feedback: {feedbackCount}"

            MessageBox.Show(summary, "Report Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error analyzing reports: " & ex.Message)
        End Try
    End Sub

    ' --------------------------------------
    ' Button3: Delete a report by number
    ' --------------------------------------
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If TextBox1.Text.Trim() = "" Then
                MessageBox.Show("Please enter the Report Number to delete.", "Missing Field")
                Exit Sub
            End If

            If Not File.Exists(dataFilePath) Then
                MessageBox.Show("No report file found.", "Not Found")
                Exit Sub
            End If

            Dim lines = File.ReadAllLines(dataFilePath).ToList()
            Dim found As Boolean = False

            ' Keep header
            Dim newLines As New List(Of String) From {lines(0)}

            For i As Integer = 1 To lines.Count - 1
                If lines(i).StartsWith(TextBox1.Text.Trim() & ",") Then
                    found = True
                Else
                    newLines.Add(lines(i))
                End If
            Next

            If Not found Then
                MessageBox.Show("No report found with this number.", "Not Found")
                Exit Sub
            End If

            File.WriteAllLines(dataFilePath, newLines)
            MessageBox.Show("Report deleted successfully.", "Deleted")

            ' Clear textboxes
            TextBox1.Clear()
            TextBox2.Clear()
            TextBox3.Clear()

        Catch ex As Exception
            MessageBox.Show("Error deleting report: " & ex.Message)
        End Try
    End Sub

    Private Sub Inventory_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Try
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath))

                ' If no file exists, create with header
                If Not File.Exists(dataFilePath) Then
                    Using writer As New StreamWriter(dataFilePath, False)
                        writer.WriteLine("ReportNumber,ReportDate,ReportText,Feedback")
                    End Using
                End If

            Catch ex As Exception
                MessageBox.Show("Error initializing report form: " & ex.Message)
            End Try
        End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
