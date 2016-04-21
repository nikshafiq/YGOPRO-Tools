Imports System.IO
Imports YGOPRO_PuzzleEditor.ScripFile
Imports System.Net


Public Class Form2

    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


        Try

            Getfiles()

        Catch ex As Exception
            MessageBox.Show("Cannot load server files")
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        If (Not ListBox1.SelectedItem Is Nothing) Then
            Dim current As String = ListBox1.SelectedItem.ToString

            ClearScript()
            Form1.RichTextBox5.Clear()
            Form1.RichTextBox3.Clear()
            LoadFile(My.Settings.GameDirectory + "/single/" + current)


        End If

    End Sub


    Private Sub Getfiles()

        ListBox1.Items.Clear()

        Dim DirectoryInfo As DirectoryInfo = New DirectoryInfo(My.Settings.GameDirectory + "/single")
        For Each File As FileInfo In DirectoryInfo.GetFiles

            ListBox1.Items.Add(File.Name)

        Next



    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Try


        
        OpenFileDialog1.InitialDirectory = My.Settings.GameDirectory & "\single"
        OpenFileDialog1.Filter = "Puzzle lua file (*.lua)|*.lua|All Files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then

            Dim target As String = My.Settings.GameDirectory + "/single/" + Path.GetFileName(OpenFileDialog1.FileName)
            File.Copy(OpenFileDialog1.FileName, target)

            ListBox1.Items.Clear()
            Dim DirectoryInfo As DirectoryInfo = New DirectoryInfo(My.Settings.GameDirectory + "/single")
            For Each File As FileInfo In DirectoryInfo.GetFiles

                ListBox1.Items.Add(File.Name)

            Next

        End If
            MsgBox("File Imported")

        Catch ex As Exception
            MsgBox("Cannot Import file")
        End Try
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Try

       
        SaveFileDialog1.InitialDirectory = My.Settings.GameDirectory & "\single"
        SaveFileDialog1.Filter = "Lua Files (*.lua)|*.lua|All Files (*.*)|*.*"
        SaveFileDialog1.FilterIndex = 1

        If Me.SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then


            createfile()
            My.Computer.FileSystem.CopyFile((Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\New.lua"), Me.SaveFileDialog1.FileName, True)

        End If
            MsgBox("File Exported")

        Catch ex As Exception
            MsgBox("Cannot export file")
        End Try
    End Sub
End Class