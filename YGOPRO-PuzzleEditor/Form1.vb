Imports System.IO
Imports System.Data.SQLite
Imports YGOPRO_PuzzleEditor.Commands
Imports YGOPRO_PuzzleEditor.Enums
Imports YGOPRO_PuzzleEditor.ScripFile
Imports YGOPRO_PuzzleEditor.GlobalVariables

Public Class Form1

    Private connection As SQLiteConnection
    Private status As Boolean
    Private CardTable As DataTable
    Private pictureselected As PictureBox


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If My.Settings.GameDirectory = Nothing Then
                Dim Response As Integer

                ' Displays a message box with the yes and no options.
                Response = MsgBox(Prompt:="Please open the YGO PRO cards.cdb folder?", Buttons:=vbYesNo)

                ' If statement to check if the yes button was selected.
                If Response = vbYes Then
                    FolderBrowserDialog1.Description = "YGO PRO FOLDER"
                    If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
                        GameDirectoryBox.Text = FolderBrowserDialog1.SelectedPath
                        My.Settings.GameDirectory = FolderBrowserDialog1.SelectedPath
                    End If

                Else
                    ' The no button was selected.
                    MsgBox("Now you cannot edit files using database")
                End If
            Else
                GameDirectoryBox.Text = My.Settings.GameDirectory
            End If
        Catch ex As Exception
            MsgBox("Now you cannot edit files using database")
        End Try

        If (GameDirectoryBox.Text <> String.Empty) Then
            CardTable = New DataTable()

            Dim path As String = (GameDirectoryBox.Text & "\cards.cdb")
            Dim epath As String = (GameDirectoryBox.Text & "\expansions\cards-tf.cdb")
            GameDirectoryBox.Text = My.Settings.GameDirectory
            My.Settings.Dabatase = path
            My.Settings.ExpansionDB = epath
            My.Settings.Save()

            Dim constring As String = "data source=" & My.Settings.Dabatase
            connection = New SQLiteConnection(constring)
            Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
            Dim ds As DataSet = New DataSet()
            Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection.Open()
            DataAdapter1.SelectCommand = sql
            DataAdapter1.Fill(CardTable)
            connection.Close()

            Dim constring2 As String = "data source=" & epath
            Dim connection2 As New SQLiteConnection(constring2)
            Dim sql2 As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection2)
            Dim ds2 As DataSet = New DataSet()
            Dim DataAdapter2 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection2.Open()
            DataAdapter2.SelectCommand = sql2
            DataAdapter2.Fill(CardTable)
            connection2.Close()

            Dim source As New BindingSource
            source.DataSource = CardTable
            DataGridView1.DataSource = source

            ToolStripStatusLabel16.Text = "Total Cards: " & DataGridView1.Rows.Count - 1

            DataGridView1.Columns.Item("id").Visible = False
            DataGridView1.Columns.Item("desc").Visible = False
            DataGridView1.Columns.Item("level").Visible = False
            DataGridView1.Columns.Item("type").Visible = False
            DataGridView1.Columns.Item("race").Visible = False

        End If

        If Not My.Settings.GameDirectory Is Nothing Then

            Me.BackgroundImage = Image.FromFile(My.Settings.GameDirectory & "\textures\bg.JPG")
            Me.BackgroundImageLayout = ImageLayout.Stretch


        End If

        If File.Exists(My.Settings.GameDirectory & "/script.txt") Then
            Me.RichTextBox4.AppendText(File.ReadAllText(My.Settings.GameDirectory & "/script.txt"))

        End If


    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Try

        
        If ComboBox1.SelectedItem = "Monster" Then
            SelectforTypeMonster()
        ElseIf ComboBox1.SelectedItem = "Spell" Then
            SelectforTypeSpell()
        ElseIf ComboBox1.SelectedItem = "Trap" Then
            SelectforTypeTrap()
        Else
            SelectFromName()
        End If
        Catch ex As Exception
            MsgBox("Cannot select for name")
        End Try


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim file As String = My.Settings.GameDirectory & "\pics\thumbnail\"
        For Each item In ListBox1.Items.ToString
            ImageList1.Images.Add(Image.FromFile(file & item.ToString & ".jpg"))
        Next item
        ListView1.LargeImageList = ImageList1
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub DataGridView1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        PictureBox1.BackgroundImage = Nothing

        Try
            TextBox3.Text = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
            TextBox4.Text = DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value
            RichTextBox2.Text = DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value
            Dim file As String = My.Settings.GameDirectory & "\pics\"
            PictureBox1.BackgroundImage = Bitmap.FromFile(file & TextBox3.Text & ".jpg")
            ToolStripStatusLabel15.Text = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        Catch ex As Exception
            MsgBox("Cell value is null")
        End Try
        
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        
    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        BindList()
    End Sub

    Private Sub ListView1_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim current As String = ListView1.FocusedItem.Text
        Dim selected As ListViewItem = ListView1.FocusedItem
        Dim file As String = My.Settings.GameDirectory & "\pics"
        PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        TextBox3.Text = current
        TextBox4.Text = selected.SubItems(1).Text
        RichTextBox2.Text = selected.SubItems(2).Text
    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RichTextBox1.TextChanged
        Try
            If ComboBox1.SelectedItem = "Monster" Then
                SelectforTypeMonster()
            ElseIf ComboBox1.SelectedItem = "Spell" Then
                SelectforTypeSpell()
            ElseIf ComboBox1.SelectedItem = "Trap" Then
                SelectforTypeTrap()
            Else
                SelectFromDescription()
            End If
        Catch ex As Exception
            MsgBox("Cannot select from name")
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Monster" Then
            SelectforTypeMonster()
            Me.ComboBox2.DataSource = System.Enum.GetValues(GetType(MonsterCategory))
            Me.ComboBox4.DataSource = System.Enum.GetValues(GetType(Races))

        ElseIf ComboBox1.SelectedItem = "Spell" Then
            Me.ComboBox2.DataSource = System.Enum.GetValues(GetType(SpellCategory))

        ElseIf ComboBox1.SelectedItem = "Trap" Then

            Me.ComboBox2.DataSource = System.Enum.GetValues(GetType(TrapCategory))
        End If
        
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

        If ComboBox1.SelectedItem = "Monster" Then

            Try
                SelectforTypeMonster()
            Catch ex As Exception
                MsgBox("Cannot select for type monster")
            End Try

        ElseIf ComboBox1.SelectedItem = "Spell" Then
            Try
                SelectforTypeSpell()
            Catch ex As Exception
                MsgBox("Cannot select for type spell")
            End Try

        ElseIf ComboBox1.SelectedItem = "Trap" Then

            Try
                SelectforTypeTrap()
            Catch ex As Exception
                MsgBox("Cannot select for type trap")
            End Try
        End If

        

    End Sub



    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Monster" Then

            If ComboBox4.SelectedValue = Nothing Then
            Else
                Try
                    SelectforMonsterLevel()
                Catch ex As Exception
                    MsgBox("Cannot select for level category")
                End Try
            End If
            

        Else

            MsgBox("Cannot select for level")
        End If
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Monster" Then
            If ComboBox3.SelectedItem = "" Then

            Else
                Try
                    SelectforMonsterRace()
                Catch ex As Exception
                    MsgBox("Cannot select for race category")
                End Try
            End If
            

        Else

            MsgBox("Cannot select for race")
        End If
    End Sub

    Private Sub PictureBox17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox17.Click
        Dim current As String = Me.TextBox3.Text
    End Sub


    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click

        ClearScript()
        RichTextBox5.Clear()
        RichTextBox3.Clear()

        OpenFileDialog1.InitialDirectory = My.Settings.GameDirectory & "\single"
        OpenFileDialog1.Filter = "Puzzle lua file (*.lua)|*.lua|All Files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then

            LoadFile(OpenFileDialog1.FileName)
        Else
            MsgBox("Error reading file")
        End If


    End Sub

    Private Sub DeckToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim process As New Process
        process.StartInfo.WorkingDirectory = My.Settings.GameDirectory
        process.StartInfo.FileName = "ygopro_vs_ai_debug.exe"
        process.StartInfo.Arguments = "-j"
        process.Start()
    End Sub

    Private Sub PuzzlesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub


    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click

        Try
            createfile(My.Settings.GameDirectory + "/single/New.lua")

            SaveFileDialog1.InitialDirectory = My.Settings.GameDirectory & "\single"
            SaveFileDialog1.Filter = "Lua Files (*.lua)|*.lua|All Files (*.*)|*.*"
            SaveFileDialog1.FilterIndex = 1

            If Me.SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then


                My.Computer.FileSystem.CopyFile((My.Settings.GameDirectory + "/single/New.lua"), SaveFileDialog1.FileName, True)

            End If


            MsgBox("File Saved")

        Catch ex As Exception
            MsgBox("File has not been saved")
        End Try


    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("YGOPRO Puzzle Editor by francot514." & vbNewLine & "YGPRO is created by Fluorhydrie")
    End Sub

    Private Sub AddStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CopyToolStripMenuItem.Click


        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then


                idselected = Me.TextBox3.Text
                monstercard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)

                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard11 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem14.Text = monstercard11 & " - " & monster11
                Me.ToolStripMenuItem48.Text = monstercard11 & " - " & monster11

                If Not ListBox1.Items.Contains(monstercard11) Then
                    Me.ListBox1.Items.Add(monstercard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox3 Then

                idselected = Me.TextBox3.Text
                monstercard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard12 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem15.Text = monstercard12 & " - " & monster12
                Me.ToolStripMenuItem49.Text = monstercard12 & " - " & monster12

                If Not ListBox1.Items.Contains(monstercard12) Then
                    Me.ListBox1.Items.Add(monstercard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox4 Then

                idselected = Me.TextBox3.Text
                monstercard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard13 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem16.Text = monstercard13 & " - " & monster13
                Me.ToolStripMenuItem50.Text = monstercard13 & " - " & monster13

                If Not ListBox1.Items.Contains(monstercard13) Then
                    Me.ListBox1.Items.Add(monstercard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox5 Then

                idselected = Me.TextBox3.Text
                monstercard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard14 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem17.Text = monstercard14 & " - " & monster14
                Me.ToolStripMenuItem51.Text = monstercard14 & " - " & monster14

                If Not ListBox1.Items.Contains(monstercard14) Then
                    Me.ListBox1.Items.Add(monstercard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox6 Then

                idselected = Me.TextBox3.Text
                monstercard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard15 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem18.Text = monstercard15 & " - " & monster15
                Me.ToolStripMenuItem52.Text = monstercard15 & " - " & monster15

                If Not ListBox1.Items.Contains(monstercard15) Then
                    Me.ListBox1.Items.Add(monstercard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox7 Then

                idselected = Me.TextBox3.Text
                spellcard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard11 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem24.Text = spellcard11 & " - " & szone11
                Me.ToolStripMenuItem60.Text = spellcard11 & " - " & szone11

                If Not ListBox1.Items.Contains(spellcard11) Then
                    Me.ListBox1.Items.Add(spellcard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox8 Then

                idselected = Me.TextBox3.Text
                spellcard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard12 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem25.Text = spellcard12 & " - " & szone12
                Me.ToolStripMenuItem61.Text = spellcard12 & " - " & szone12

                If Not ListBox1.Items.Contains(spellcard12) Then
                    Me.ListBox1.Items.Add(spellcard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox9 Then

                idselected = Me.TextBox3.Text
                spellcard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard13 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem26.Text = spellcard13 & " - " & szone13
                Me.ToolStripMenuItem62.Text = spellcard13 & " - " & szone13

                If Not ListBox1.Items.Contains(spellcard13) Then
                    Me.ListBox1.Items.Add(spellcard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox10 Then

                idselected = Me.TextBox3.Text
                spellcard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard14 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem27.Text = spellcard14 & " - " & szone14
                Me.ToolStripMenuItem63.Text = spellcard14 & " - " & szone14

                If Not ListBox1.Items.Contains(spellcard14) Then
                    Me.ListBox1.Items.Add(spellcard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox11 Then

                idselected = Me.TextBox3.Text
                spellcard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard15 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem28.Text = spellcard15 & " - " & szone15
                Me.ToolStripMenuItem64.Text = spellcard15 & " - " & szone15

                If Not ListBox1.Items.Contains(spellcard15) Then
                    Me.ListBox1.Items.Add(spellcard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox12 Then

                idselected = Me.TextBox3.Text
                handcard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard11 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem36.Text = handcard11 & " - " & hand11
                Me.ToolStripMenuItem74.Text = handcard11 & " - " & hand11

                If Not ListBox1.Items.Contains(handcard11) Then
                    Me.ListBox1.Items.Add(handcard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox13 Then

                idselected = Me.TextBox3.Text
                handcard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)

                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard12 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem37.Text = handcard12 & " - " & hand12
                Me.ToolStripMenuItem75.Text = handcard12 & " - " & hand12

                If Not ListBox1.Items.Contains(handcard12) Then
                    Me.ListBox1.Items.Add(handcard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox14 Then

                idselected = Me.TextBox3.Text
                handcard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard13 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem38.Text = handcard13 & " - " & hand13
                Me.ToolStripMenuItem76.Text = handcard13 & " - " & hand13

                If Not ListBox1.Items.Contains(handcard13) Then
                    Me.ListBox1.Items.Add(handcard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox15 Then

                idselected = Me.TextBox3.Text
                handcard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard14 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem39.Text = handcard14 & " - " & hand14
                Me.ToolStripMenuItem77.Text = handcard14 & " - " & hand14

                If Not ListBox1.Items.Contains(handcard14) Then
                    Me.ListBox1.Items.Add(handcard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox16 Then

                idselected = Me.TextBox3.Text
                handcard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard15 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem40.Text = handcard15 & " - " & hand15
                Me.ToolStripMenuItem78.Text = handcard15 & " - " & hand15

                If Not ListBox1.Items.Contains(handcard15) Then
                    Me.ListBox1.Items.Add(handcard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox21 Then

                idselected = Me.TextBox3.Text
                spellcard16 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone16 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard16 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.FieldToolStripMenuItem.Text = spellcard16 & " - " & szone16
                Me.ToolStripMenuItem65.Text = spellcard16 & " - " & szone16

                If Not ListBox1.Items.Contains(spellcard16) Then
                    Me.ListBox1.Items.Add(spellcard16)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox22 Then

                idselected = Me.TextBox3.Text
                monstercard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard21 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem19.Text = monstercard21 & " - " & monster21
                Me.ToolStripMenuItem54.Text = monstercard21 & " - " & monster21

                If Not ListBox1.Items.Contains(monstercard21) Then
                    Me.ListBox1.Items.Add(monstercard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox23 Then

                idselected = Me.TextBox3.Text
                monstercard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard22 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem20.Text = monstercard22 & " - " & monster22
                Me.ToolStripMenuItem55.Text = monstercard22 & " - " & monster22

                If Not ListBox1.Items.Contains(monstercard22) Then
                    Me.ListBox1.Items.Add(monstercard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox24 Then

                idselected = Me.TextBox3.Text
                monstercard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard23 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem21.Text = monstercard23 & " - " & monster23
                Me.ToolStripMenuItem56.Text = monstercard23 & " - " & monster23

                If Not ListBox1.Items.Contains(monstercard23) Then
                    Me.ListBox1.Items.Add(monstercard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox25 Then

                idselected = Me.TextBox3.Text
                monstercard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard24 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem22.Text = monstercard24 & " - " & monster24
                Me.ToolStripMenuItem57.Text = monstercard24 & " - " & monster24

                If Not ListBox1.Items.Contains(monstercard24) Then
                    Me.ListBox1.Items.Add(monstercard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox26 Then

                idselected = Me.TextBox3.Text
                monstercard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                monster25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard25 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem23.Text = monstercard25 & " - " & monster25
                Me.ToolStripMenuItem58.Text = monstercard25 & " - " & monster25

                If Not ListBox1.Items.Contains(monstercard25) Then
                    Me.ListBox1.Items.Add(monstercard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox27 Then

                idselected = Me.TextBox3.Text
                spellcard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard21 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem30.Text = spellcard21 & " - " & szone21
                Me.ToolStripMenuItem67.Text = spellcard21 & " - " & szone21

                If Not ListBox1.Items.Contains(spellcard21) Then
                    Me.ListBox1.Items.Add(spellcard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox28 Then

                idselected = Me.TextBox3.Text
                spellcard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard22 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem31.Text = spellcard22 & " - " & szone22
                Me.ToolStripMenuItem68.Text = spellcard22 & " - " & szone22

                If Not ListBox1.Items.Contains(spellcard22) Then
                    Me.ListBox1.Items.Add(spellcard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox29 Then

                idselected = Me.TextBox3.Text
                spellcard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard23 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem32.Text = spellcard23 & " - " & szone23
                Me.ToolStripMenuItem69.Text = spellcard23 & " - " & szone23

                If Not ListBox1.Items.Contains(spellcard23) Then
                    Me.ListBox1.Items.Add(spellcard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox30 Then

                idselected = Me.TextBox3.Text
                spellcard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard24 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem33.Text = spellcard24 & " - " & szone24
                Me.ToolStripMenuItem70.Text = spellcard24 & " - " & szone24

                If Not ListBox1.Items.Contains(spellcard24) Then
                    Me.ListBox1.Items.Add(spellcard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox31 Then

                idselected = Me.TextBox3.Text
                spellcard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard25 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem34.Text = spellcard25 & " - " & szone25
                Me.ToolStripMenuItem71.Text = spellcard25 & " - " & szone25

                If Not ListBox1.Items.Contains(spellcard25) Then
                    Me.ListBox1.Items.Add(spellcard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox40 Then

                idselected = Me.TextBox3.Text
                spellcard26 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                szone26 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard26 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.FieldToolStripMenuItem1.Text = spellcard26 & " - " & szone26
                Me.ToolStripMenuItem72.Text = spellcard26 & " - " & szone26

                If Not ListBox1.Items.Contains(spellcard26) Then
                    Me.ListBox1.Items.Add(spellcard26)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox32 Then

                idselected = Me.TextBox3.Text
                handcard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard21 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem42.Text = handcard21 & " - " & hand21
                Me.ToolStripMenuItem80.Text = handcard21 & " - " & hand21


                If Not ListBox1.Items.Contains(handcard21) Then
                    Me.ListBox1.Items.Add(handcard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox33 Then

                idselected = Me.TextBox3.Text
                handcard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard22 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem43.Text = handcard22 & " - " & hand22
                Me.ToolStripMenuItem81.Text = handcard22 & " - " & hand22


                If Not ListBox1.Items.Contains(handcard22) Then
                    Me.ListBox1.Items.Add(handcard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox34 Then

                idselected = Me.TextBox3.Text
                handcard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard23 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem44.Text = handcard23 & " - " & hand23

                Me.ToolStripMenuItem82.Text = handcard23 & " - " & hand23

                If Not ListBox1.Items.Contains(handcard23) Then
                    Me.ListBox1.Items.Add(handcard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox35 Then

                idselected = Me.TextBox3.Text
                handcard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard24 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem45.Text = handcard24 & " - " & hand24
                Me.ToolStripMenuItem83.Text = handcard24 & " - " & hand24

                If Not ListBox1.Items.Contains(handcard24) Then
                    Me.ListBox1.Items.Add(handcard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox36 Then

                idselected = Me.TextBox3.Text
                handcard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                hand25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard25 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem46.Text = handcard25 & " - " & hand25
                Me.ToolStripMenuItem84.Text = handcard25 & " - " & hand25

                If Not ListBox1.Items.Contains(handcard25) Then
                    Me.ListBox1.Items.Add(handcard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If



            ElseIf pictureselected Is PictureBox45 Then

                idselected = Me.TextBox3.Text
                deckcard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard11 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem4.Text = deckcard11 & " - " & deck11


                If Not ListBox1.Items.Contains(deckcard11) Then
                    Me.ListBox1.Items.Add(deckcard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox44 Then

                idselected = Me.TextBox3.Text
                deckcard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard12 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem5.Text = deckcard12 & " - " & deck12


                If Not ListBox1.Items.Contains(deckcard12) Then
                    Me.ListBox1.Items.Add(deckcard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox43 Then

                idselected = Me.TextBox3.Text
                deckcard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard13 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem6.Text = deckcard11 & " - " & deck13


                If Not ListBox1.Items.Contains(deckcard13) Then
                    Me.ListBox1.Items.Add(deckcard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox42 Then

                idselected = Me.TextBox3.Text
                deckcard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard14 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem7.Text = deckcard14 & " - " & deck14


                If Not ListBox1.Items.Contains(deckcard14) Then
                    Me.ListBox1.Items.Add(deckcard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox41 Then

                idselected = Me.TextBox3.Text
                deckcard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard15 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem8.Text = deckcard15 & " - " & deck15


                If Not ListBox1.Items.Contains(deckcard15) Then
                    Me.ListBox1.Items.Add(deckcard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox82 Then

                idselected = Me.TextBox3.Text
                deckcard16 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck16 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard16 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem9.Text = deckcard16 & " - " & deck16


                If Not ListBox1.Items.Contains(deckcard16) Then
                    Me.ListBox1.Items.Add(deckcard16)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox83 Then

                idselected = Me.TextBox3.Text
                deckcard17 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck17 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard17 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem10.Text = deckcard17 & " - " & deck17


                If Not ListBox1.Items.Contains(deckcard17) Then
                    Me.ListBox1.Items.Add(deckcard17)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox84 Then

                idselected = Me.TextBox3.Text
                deckcard18 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck18 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard18 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem11.Text = deckcard18 & " - " & deck18


                If Not ListBox1.Items.Contains(deckcard18) Then
                    Me.ListBox1.Items.Add(deckcard18)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox85 Then

                idselected = Me.TextBox3.Text
                deckcard19 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck19 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard19 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem12.Text = deckcard19 & " - " & deck19


                If Not ListBox1.Items.Contains(deckcard19) Then
                    Me.ListBox1.Items.Add(deckcard19)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox86 Then

                idselected = Me.TextBox3.Text
                deckcard10 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck10 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard10 & ".jpg"
                Me.pictureselected.ImageLocation = current
                Me.ToolStripMenuItem13.Text = deckcard10 & " - " & deck10


                If Not ListBox1.Items.Contains(deckcard10) Then
                    Me.ListBox1.Items.Add(deckcard10)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox50 Then

                idselected = Me.TextBox3.Text
                deckcard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard21 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem86.Text = deckcard21 & " - " & deck21

                If Not ListBox1.Items.Contains(deckcard21) Then
                    Me.ListBox1.Items.Add(deckcard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox49 Then

                idselected = Me.TextBox3.Text
                deckcard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard22 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem87.Text = deckcard22 & " - " & deck22

                If Not ListBox1.Items.Contains(deckcard22) Then
                    Me.ListBox1.Items.Add(deckcard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox48 Then

                idselected = Me.TextBox3.Text
                deckcard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard23 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem88.Text = deckcard23 & " - " & deck23

                If Not ListBox1.Items.Contains(deckcard23) Then
                    Me.ListBox1.Items.Add(deckcard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox47 Then

                idselected = Me.TextBox3.Text
                deckcard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard24 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem89.Text = deckcard24 & " - " & deck24

                If Not ListBox1.Items.Contains(deckcard24) Then
                    Me.ListBox1.Items.Add(deckcard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox46 Then

                idselected = Me.TextBox3.Text
                deckcard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard25 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem90.Text = deckcard25 & " - " & deck25

                If Not ListBox1.Items.Contains(deckcard25) Then
                    Me.ListBox1.Items.Add(deckcard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox102 Then

                idselected = Me.TextBox3.Text
                deckcard26 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck26 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard26 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem91.Text = deckcard26 & " - " & deck26

                If Not ListBox1.Items.Contains(deckcard26) Then
                    Me.ListBox1.Items.Add(deckcard26)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox103 Then

                idselected = Me.TextBox3.Text
                deckcard27 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck27 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard27 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem92.Text = deckcard27 & " - " & deck27

                If Not ListBox1.Items.Contains(deckcard27) Then
                    Me.ListBox1.Items.Add(deckcard27)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox104 Then

                idselected = Me.TextBox3.Text
                deckcard28 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck28 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard28 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem93.Text = deckcard28 & " - " & deck28

                If Not ListBox1.Items.Contains(deckcard28) Then
                    Me.ListBox1.Items.Add(deckcard28)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox105 Then

                idselected = Me.TextBox3.Text
                deckcard29 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck29 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard29 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem94.Text = deckcard29 & " - " & deck29

                If Not ListBox1.Items.Contains(deckcard29) Then
                    Me.ListBox1.Items.Add(deckcard29)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)


                End If


            ElseIf pictureselected Is PictureBox106 Then

                idselected = Me.TextBox3.Text
                deckcard20 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                deck20 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard20 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem95.Text = deckcard20 & " - " & deck20

                If Not ListBox1.Items.Contains(deckcard20) Then
                    Me.ListBox1.Items.Add(deckcard20)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox55 Then

                idselected = Me.TextBox3.Text
                gravecard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard11 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem96.Text = gravecard11 & " - " & grave11

                If Not ListBox1.Items.Contains(gravecard11) Then
                    Me.ListBox1.Items.Add(gravecard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox54 Then

                idselected = Me.TextBox3.Text
                gravecard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard12 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem97.Text = gravecard12 & " - " & grave12

                If Not ListBox1.Items.Contains(gravecard12) Then
                    Me.ListBox1.Items.Add(gravecard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox53 Then

                idselected = Me.TextBox3.Text
                gravecard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard13 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem98.Text = gravecard13 & " - " & grave13

                If Not ListBox1.Items.Contains(gravecard13) Then
                    Me.ListBox1.Items.Add(gravecard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox52 Then

                idselected = Me.TextBox3.Text
                gravecard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard14 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem99.Text = gravecard14 & " - " & grave14

                If Not ListBox1.Items.Contains(gravecard14) Then
                    Me.ListBox1.Items.Add(gravecard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox51 Then

                idselected = Me.TextBox3.Text
                gravecard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard15 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem100.Text = gravecard15 & " - " & grave15

                If Not ListBox1.Items.Contains(gravecard15) Then
                    Me.ListBox1.Items.Add(gravecard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox87 Then

                idselected = Me.TextBox3.Text
                gravecard16 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave16 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard16 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem101.Text = gravecard16 & " - " & grave16

                If Not ListBox1.Items.Contains(gravecard16) Then
                    Me.ListBox1.Items.Add(gravecard16)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox88 Then

                idselected = Me.TextBox3.Text
                gravecard17 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave17 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard17 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem102.Text = gravecard17 & " - " & grave17

                If Not ListBox1.Items.Contains(gravecard17) Then
                    Me.ListBox1.Items.Add(gravecard17)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox89 Then

                idselected = Me.TextBox3.Text
                gravecard18 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave18 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard18 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem103.Text = gravecard18 & " - " & grave18

                If Not ListBox1.Items.Contains(gravecard18) Then
                    Me.ListBox1.Items.Add(gravecard18)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox90 Then

                idselected = Me.TextBox3.Text
                gravecard19 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave19 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard19 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem104.Text = gravecard19 & " - " & grave19

                If Not ListBox1.Items.Contains(gravecard19) Then
                    Me.ListBox1.Items.Add(gravecard19)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox91 Then

                idselected = Me.TextBox3.Text
                gravecard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave10 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard10 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem105.Text = gravecard10 & " - " & grave10

                If Not ListBox1.Items.Contains(gravecard10) Then
                    Me.ListBox1.Items.Add(gravecard10)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox60 Then

                idselected = Me.TextBox3.Text
                gravecard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard21 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem106.Text = gravecard21 & " - " & grave21

                If Not ListBox1.Items.Contains(gravecard21) Then
                    Me.ListBox1.Items.Add(gravecard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox59 Then

                idselected = Me.TextBox3.Text
                gravecard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard22 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem107.Text = gravecard22 & " - " & grave22

                If Not ListBox1.Items.Contains(gravecard22) Then
                    Me.ListBox1.Items.Add(gravecard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox58 Then

                idselected = Me.TextBox3.Text
                gravecard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard23 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem108.Text = gravecard23 & " - " & grave23

                If Not ListBox1.Items.Contains(gravecard23) Then
                    Me.ListBox1.Items.Add(gravecard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox57 Then

                idselected = Me.TextBox3.Text
                gravecard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard24 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem109.Text = gravecard24 & " - " & grave24

                If Not ListBox1.Items.Contains(gravecard24) Then
                    Me.ListBox1.Items.Add(gravecard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox56 Then

                idselected = Me.TextBox3.Text
                gravecard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard25 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem110.Text = gravecard25 & " - " & grave25

                If Not ListBox1.Items.Contains(gravecard25) Then
                    Me.ListBox1.Items.Add(gravecard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox107 Then

                idselected = Me.TextBox3.Text
                gravecard26 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave26 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard26 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem111.Text = gravecard26 & " - " & grave26

                If Not ListBox1.Items.Contains(gravecard26) Then
                    Me.ListBox1.Items.Add(gravecard26)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox108 Then

                idselected = Me.TextBox3.Text
                gravecard27 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave27 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard27 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem112.Text = gravecard27 & " - " & grave27

                If Not ListBox1.Items.Contains(gravecard27) Then
                    Me.ListBox1.Items.Add(gravecard27)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox109 Then

                idselected = Me.TextBox3.Text
                gravecard28 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave28 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard28 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem113.Text = gravecard28 & " - " & grave28

                If Not ListBox1.Items.Contains(gravecard28) Then
                    Me.ListBox1.Items.Add(gravecard28)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox110 Then

                idselected = Me.TextBox3.Text
                gravecard29 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave29 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard29 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem114.Text = gravecard29 & " - " & grave29

                If Not ListBox1.Items.Contains(gravecard29) Then
                    Me.ListBox1.Items.Add(gravecard29)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox111 Then

                idselected = Me.TextBox3.Text
                gravecard20 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                grave20 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard20 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem111.Text = gravecard20 & " - " & grave20

                If Not ListBox1.Items.Contains(gravecard20) Then
                    Me.ListBox1.Items.Add(gravecard20)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox65 Then

                idselected = Me.TextBox3.Text
                removedcard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard11 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem117.Text = removedcard11 & " - " & removed11

                If Not ListBox1.Items.Contains(removedcard11) Then
                    Me.ListBox1.Items.Add(removedcard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox64 Then

                idselected = Me.TextBox3.Text
                removedcard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard12 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem118.Text = removedcard12 & " - " & removed12

                If Not ListBox1.Items.Contains(removedcard12) Then
                    Me.ListBox1.Items.Add(removedcard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox63 Then

                idselected = Me.TextBox3.Text
                removedcard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard13 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem119.Text = removedcard13 & " - " & removed13

                If Not ListBox1.Items.Contains(removedcard13) Then
                    Me.ListBox1.Items.Add(removedcard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox62 Then

                idselected = Me.TextBox3.Text
                removedcard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard14 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem120.Text = removedcard14 & " - " & removed14

                If Not ListBox1.Items.Contains(removedcard14) Then
                    Me.ListBox1.Items.Add(removedcard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox61 Then

                idselected = Me.TextBox3.Text
                removedcard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard15 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem121.Text = removedcard15 & " - " & removed15

                If Not ListBox1.Items.Contains(removedcard15) Then
                    Me.ListBox1.Items.Add(removedcard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox92 Then

                idselected = Me.TextBox3.Text
                removedcard16 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed16 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard16 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem123.Text = removedcard16 & " - " & removed16

                If Not ListBox1.Items.Contains(removedcard16) Then
                    Me.ListBox1.Items.Add(removedcard16)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox93 Then

                idselected = Me.TextBox3.Text
                removedcard17 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed17 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard17 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem124.Text = removedcard17 & " - " & removed17

                If Not ListBox1.Items.Contains(removedcard17) Then
                    Me.ListBox1.Items.Add(removedcard17)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox94 Then

                idselected = Me.TextBox3.Text
                removedcard18 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed18 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard18 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem125.Text = removedcard18 & " - " & removed18

                If Not ListBox1.Items.Contains(removedcard18) Then
                    Me.ListBox1.Items.Add(removedcard18)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox95 Then

                idselected = Me.TextBox3.Text
                removedcard19 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed19 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard19 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem126.Text = removedcard19 & " - " & removed19

                If Not ListBox1.Items.Contains(removedcard19) Then
                    Me.ListBox1.Items.Add(removedcard19)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox96 Then

                idselected = Me.TextBox3.Text
                removedcard10 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed10 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard10 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem126.Text = removedcard10 & " - " & removed10

                If Not ListBox1.Items.Contains(removedcard10) Then
                    Me.ListBox1.Items.Add(removedcard10)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox70 Then

                idselected = Me.TextBox3.Text
                removedcard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard21 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem130.Text = removedcard21 & " - " & removed21

                If Not ListBox1.Items.Contains(removedcard21) Then
                    Me.ListBox1.Items.Add(removedcard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox69 Then

                idselected = Me.TextBox3.Text
                removedcard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard22 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem131.Text = removedcard22 & " - " & removed22

                If Not ListBox1.Items.Contains(removedcard22) Then
                    Me.ListBox1.Items.Add(removedcard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox68 Then

                idselected = Me.TextBox3.Text
                removedcard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard23 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem132.Text = removedcard23 & " - " & removed23

                If Not ListBox1.Items.Contains(removedcard23) Then
                    Me.ListBox1.Items.Add(removedcard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox67 Then

                idselected = Me.TextBox3.Text
                removedcard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard24 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem133.Text = removedcard24 & " - " & removed24

                If Not ListBox1.Items.Contains(removedcard24) Then
                    Me.ListBox1.Items.Add(removedcard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox66 Then

                idselected = Me.TextBox3.Text
                removedcard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard25 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem134.Text = removedcard25 & " - " & removed25

                If Not ListBox1.Items.Contains(removedcard25) Then
                    Me.ListBox1.Items.Add(removedcard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox112 Then

                idselected = Me.TextBox3.Text
                removedcard26 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed26 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard26 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem136.Text = removedcard26 & " - " & removed26

                If Not ListBox1.Items.Contains(removedcard26) Then
                    Me.ListBox1.Items.Add(removedcard26)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox113 Then

                idselected = Me.TextBox3.Text
                removedcard27 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed27 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard27 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem137.Text = removedcard27 & " - " & removed27

                If Not ListBox1.Items.Contains(removedcard27) Then
                    Me.ListBox1.Items.Add(removedcard27)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            ElseIf pictureselected Is PictureBox114 Then

                idselected = Me.TextBox3.Text
                removedcard28 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed28 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard28 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem138.Text = removedcard28 & " - " & removed28

                If Not ListBox1.Items.Contains(removedcard28) Then
                    Me.ListBox1.Items.Add(removedcard28)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox115 Then

                idselected = Me.TextBox3.Text
                removedcard29 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed29 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard29 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem139.Text = removedcard29 & " - " & removed29

                If Not ListBox1.Items.Contains(removedcard29) Then
                    Me.ListBox1.Items.Add(removedcard29)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox116 Then

                idselected = Me.TextBox3.Text
                removedcard20 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                removed20 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard20 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem140.Text = removedcard20 & " - " & removed20

                If Not ListBox1.Items.Contains(removedcard20) Then
                    Me.ListBox1.Items.Add(removedcard20)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox75 Then

                idselected = Me.TextBox3.Text
                extracard11 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra11 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard11 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem143.Text = extracard11 & " - " & extra11

                If Not ListBox1.Items.Contains(extracard11) Then
                    Me.ListBox1.Items.Add(extracard11)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox74 Then

                idselected = Me.TextBox3.Text
                extracard12 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra12 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard12 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem144.Text = extracard12 & " - " & extra12

                If Not ListBox1.Items.Contains(extracard12) Then
                    Me.ListBox1.Items.Add(extracard12)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox73 Then

                idselected = Me.TextBox3.Text
                extracard13 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra13 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard13 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem145.Text = extracard13 & " - " & extra13

                If Not ListBox1.Items.Contains(extracard13) Then
                    Me.ListBox1.Items.Add(extracard13)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox72 Then

                idselected = Me.TextBox3.Text
                extracard14 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra14 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard14 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem146.Text = extracard14 & " - " & extra14

                If Not ListBox1.Items.Contains(extracard14) Then
                    Me.ListBox1.Items.Add(extracard14)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox71 Then

                idselected = Me.TextBox3.Text
                extracard15 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra15 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard15 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem147.Text = extracard15 & " - " & extra15

                If Not ListBox1.Items.Contains(extracard15) Then
                    Me.ListBox1.Items.Add(extracard15)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox97 Then

                idselected = Me.TextBox3.Text
                extracard16 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra16 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard16 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem149.Text = extracard16 & " - " & extra16

                If Not ListBox1.Items.Contains(extracard16) Then
                    Me.ListBox1.Items.Add(extracard16)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox98 Then

                idselected = Me.TextBox3.Text
                extracard17 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra17 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard17 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem150.Text = extracard17 & " - " & extra17

                If Not ListBox1.Items.Contains(extracard17) Then
                    Me.ListBox1.Items.Add(extracard17)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox99 Then

                idselected = Me.TextBox3.Text
                extracard18 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra18 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard18 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem151.Text = extracard18 & " - " & extra18

                If Not ListBox1.Items.Contains(extracard18) Then
                    Me.ListBox1.Items.Add(extracard18)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox100 Then

                idselected = Me.TextBox3.Text
                extracard19 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra19 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard19 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem152.Text = extracard19 & " - " & extra19

                If Not ListBox1.Items.Contains(extracard19) Then
                    Me.ListBox1.Items.Add(extracard19)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox101 Then

                idselected = Me.TextBox3.Text
                extracard10 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra10 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard10 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem153.Text = extracard10 & " - " & extra10

                If Not ListBox1.Items.Contains(extracard10) Then
                    Me.ListBox1.Items.Add(extracard10)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox80 Then

                idselected = Me.TextBox3.Text
                extracard21 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra21 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard21 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem156.Text = extracard21 & " - " & extra21

                If Not ListBox1.Items.Contains(extracard21) Then
                    Me.ListBox1.Items.Add(extracard21)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox79 Then

                idselected = Me.TextBox3.Text
                extracard22 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra22 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard22 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem157.Text = extracard22 & " - " & extra22

                If Not ListBox1.Items.Contains(extracard22) Then
                    Me.ListBox1.Items.Add(extracard22)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox78 Then

                idselected = Me.TextBox3.Text
                extracard23 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra23 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard23 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem158.Text = extracard23 & " - " & extra23

                If Not ListBox1.Items.Contains(extracard23) Then
                    Me.ListBox1.Items.Add(extracard23)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox77 Then

                idselected = Me.TextBox3.Text
                extracard24 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra24 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard24 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem159.Text = extracard24 & " - " & extra24

                If Not ListBox1.Items.Contains(extracard24) Then
                    Me.ListBox1.Items.Add(extracard24)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox76 Then

                idselected = Me.TextBox3.Text
                extracard25 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra25 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard25 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem160.Text = extracard25 & " - " & extra25

                If Not ListBox1.Items.Contains(extracard25) Then
                    Me.ListBox1.Items.Add(extracard25)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox117 Then

                idselected = Me.TextBox3.Text
                extracard26 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra26 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard26 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem162.Text = extracard26 & " - " & extra26

                If Not ListBox1.Items.Contains(extracard26) Then
                    Me.ListBox1.Items.Add(extracard26)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox118 Then

                idselected = Me.TextBox3.Text
                extracard27 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra27 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard27 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem163.Text = extracard27 & " - " & extra27

                If Not ListBox1.Items.Contains(extracard27) Then
                    Me.ListBox1.Items.Add(extracard27)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox119 Then

                idselected = Me.TextBox3.Text
                extracard28 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra28 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard28 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem164.Text = extracard28 & " - " & extra28

                If Not ListBox1.Items.Contains(extracard28) Then
                    Me.ListBox1.Items.Add(extracard28)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox120 Then

                idselected = Me.TextBox3.Text
                extracard29 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra29 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard29 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem165.Text = extracard29 & " - " & extra29

                If Not ListBox1.Items.Contains(extracard29) Then
                    Me.ListBox1.Items.Add(extracard29)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If

            ElseIf pictureselected Is PictureBox121 Then

                idselected = Me.TextBox3.Text
                extracard20 = Me.TextBox3.Text

                cardselected = Me.TextBox4.Text
                extra20 = Me.TextBox4.Text
                Me.ToolTip1.SetToolTip(pictureselected, cardselected)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard20 & ".jpg"
                Me.pictureselected.ImageLocation = current

                Me.ToolStripMenuItem166.Text = extracard20 & " - " & extra20

                If Not ListBox1.Items.Contains(extracard20) Then
                    Me.ListBox1.Items.Add(extracard20)
                End If

                If Not ListBox2.Items.Contains(cardselected) Then
                    Me.ListBox2.Items.Add(cardselected)
                End If


            End If

        End If

    End Sub


    Private Sub PictureBox2_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox2.MouseEnter
        pictureselected = PictureBox2
        If Not PictureBox2.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox2)
        End If
    End Sub

    Private Sub PictureBox3_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox3.MouseEnter
        pictureselected = PictureBox3
        If Not PictureBox3.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox3)
        End If
    End Sub

    Private Sub PictureBox4_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox4.MouseEnter
        pictureselected = PictureBox4
        If Not PictureBox4.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox4)
        End If
    End Sub

    Private Sub PictureBox5_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox5.MouseEnter
        pictureselected = PictureBox5
        If Not PictureBox5.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox5)
        End If
    End Sub

    Private Sub PictureBox6_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox6.MouseEnter
        pictureselected = PictureBox6
        If Not PictureBox6.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox6)
        End If
    End Sub

    Private Sub PictureBox7_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox7.MouseEnter
        pictureselected = PictureBox7
        If Not PictureBox7.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox7)
        End If
    End Sub

    Private Sub PictureBox8_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox8.MouseEnter
        pictureselected = PictureBox8
        If Not PictureBox8.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox8)
        End If
    End Sub
    Private Sub PictureBox9_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox9.MouseEnter
        pictureselected = PictureBox9
        If Not PictureBox9.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox9)
        End If
    End Sub
    Private Sub PictureBox10_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox10.MouseEnter
        pictureselected = PictureBox10
        If Not PictureBox10.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox10)
        End If
    End Sub
    Private Sub PictureBox11_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox11.MouseEnter
        pictureselected = PictureBox11
        If Not PictureBox11.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox11)
        End If
    End Sub
    Private Sub PictureBox12_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox12.MouseEnter
        pictureselected = PictureBox12
        If Not PictureBox12.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox12)
        End If
    End Sub
    Private Sub PictureBox13_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox13.MouseEnter
        pictureselected = PictureBox13
        If Not PictureBox13.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox13)
        End If
    End Sub
    Private Sub PictureBox14_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox14.MouseEnter
        pictureselected = PictureBox14
        If Not PictureBox14.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox14)
        End If
    End Sub
    Private Sub PictureBox15_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox15.MouseEnter
        pictureselected = PictureBox15
        If Not PictureBox15.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox15)
        End If
    End Sub
    Private Sub PictureBox16_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox16.MouseEnter
        pictureselected = PictureBox16
        If Not PictureBox16.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox16)
        End If
    End Sub
    Private Sub PictureBox17_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox17.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox17, "Main Deck")
        If Not PictureBox17.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox17)
        End If
    End Sub
    Private Sub PictureBox18_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox18.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox18, "Graveyard")
        If Not PictureBox18.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox18)
        End If
    End Sub
    Private Sub PictureBox19_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox19.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox19, "Banished")
        If Not PictureBox19.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox19)
        End If
    End Sub
    Private Sub PictureBox20_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox20.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox20, "Extra Deck")
        If Not PictureBox20.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox20)
        End If
    End Sub
    Private Sub PictureBox21_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox21.MouseEnter
        pictureselected = PictureBox21
        If Not PictureBox21.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox21)
        End If
    End Sub
    Private Sub PictureBox22_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox22.MouseEnter
        pictureselected = PictureBox22
        If Not PictureBox22.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox22)
        End If
    End Sub
    Private Sub PictureBox23_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox23.MouseEnter
        pictureselected = PictureBox23
        If Not PictureBox23.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox23)
        End If
    End Sub
    Private Sub PictureBox24_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox24.MouseEnter
        pictureselected = PictureBox24
        If Not PictureBox24.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox24)
        End If
    End Sub
    Private Sub PictureBox25_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox25.MouseEnter
        pictureselected = PictureBox25
        If Not PictureBox25.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox25)
        End If
    End Sub
    Private Sub PictureBox26_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox26.MouseEnter
        pictureselected = PictureBox26
        If Not PictureBox26.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox26)
        End If
    End Sub
    Private Sub PictureBox27_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox27.MouseEnter
        pictureselected = PictureBox27
        If Not PictureBox27.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox27)
        End If
    End Sub
    Private Sub PictureBox28_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox28.MouseEnter
        pictureselected = PictureBox28
        If Not PictureBox28.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox28)
        End If
    End Sub
    Private Sub PictureBox29_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox29.MouseEnter
        pictureselected = PictureBox29
        If Not PictureBox29.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox29)
        End If
    End Sub
    Private Sub PictureBox30_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox30.MouseEnter
        pictureselected = PictureBox30
        If Not PictureBox30.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox30)
        End If
    End Sub
    Private Sub PictureBox31_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox31.MouseEnter
        pictureselected = PictureBox31
        If Not PictureBox31.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox31)
        End If
    End Sub
    Private Sub PictureBox32_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox32.MouseEnter
        pictureselected = PictureBox32
        If Not PictureBox32.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox32)
        End If
    End Sub
    Private Sub PictureBox33_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox33.MouseEnter
        pictureselected = PictureBox33
        If Not PictureBox33.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox33)
        End If
    End Sub
    Private Sub PictureBox34_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox34.MouseEnter
        pictureselected = PictureBox34
        If Not PictureBox34.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox34)
        End If
    End Sub
    Private Sub PictureBox35_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox35.MouseEnter
        pictureselected = PictureBox35
        If Not PictureBox35.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox35)
        End If
    End Sub
    Private Sub PictureBox36_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox36.MouseEnter
        pictureselected = PictureBox36
        If Not PictureBox36.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox36)
        End If
    End Sub
    Private Sub PictureBox37_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox37.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox37, "Main Deck")
        If Not PictureBox37.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox37)
        End If
    End Sub
    Private Sub PictureBox38_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox38.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox38, "Graveyard")
        If Not PictureBox38.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox38)
        End If
    End Sub
    Private Sub PictureBox39_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox39.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox39, "Banished")
        If Not PictureBox38.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox39)
        End If
    End Sub
    Private Sub PictureBox40_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox40.MouseEnter
        pictureselected = PictureBox40
        If Not PictureBox40.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox40)
        End If
    End Sub
    Private Sub PictureBox81_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox81.MouseEnter
        Me.ToolTip1.SetToolTip(PictureBox81, "Extra Deck")
        If Not PictureBox81.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox81)
        End If
    End Sub
    Private Sub PictureBox41_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox41.MouseEnter
        pictureselected = PictureBox41
        If Not PictureBox41.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox41)
        End If
    End Sub
    Private Sub PictureBox42_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox42.MouseEnter
        pictureselected = PictureBox42
        If Not PictureBox42.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox42)
        End If
    End Sub
    Private Sub PictureBox43_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox43.MouseEnter
        pictureselected = PictureBox43
        If Not PictureBox43.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox43)
        End If
    End Sub
    Private Sub PictureBox44_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox44.MouseEnter
        pictureselected = PictureBox44
        If Not PictureBox44.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox44)
        End If
    End Sub
    Private Sub PictureBox45_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox45.MouseEnter
        pictureselected = PictureBox45
        If Not PictureBox45.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox45)
        End If
    End Sub
    Private Sub PictureBox46_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox46.MouseEnter
        pictureselected = PictureBox46
        If Not PictureBox46.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox46)
        End If
    End Sub
    Private Sub PictureBox47_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox47.MouseEnter
        pictureselected = PictureBox47
        If Not PictureBox47.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox47)
        End If
    End Sub
    Private Sub PictureBox48_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox48.MouseEnter
        pictureselected = PictureBox48
        If Not PictureBox48.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox48)
        End If
    End Sub
    Private Sub PictureBox49_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox49.MouseEnter
        pictureselected = PictureBox49
        If Not PictureBox49.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox49)
        End If
    End Sub
    Private Sub PictureBox50_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox50.MouseEnter
        pictureselected = PictureBox50
        If Not PictureBox50.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox50)
        End If
    End Sub
    Private Sub PictureBox51_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox51.MouseEnter
        pictureselected = PictureBox51
        If Not PictureBox51.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox51)
        End If
    End Sub
    Private Sub PictureBox52_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox52.MouseEnter
        pictureselected = PictureBox52
        If Not PictureBox52.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox52)
        End If
    End Sub
    Private Sub PictureBox53_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox53.MouseEnter
        pictureselected = PictureBox53
        If Not PictureBox53.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox53)
        End If
    End Sub
    Private Sub PictureBox54_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox54.MouseEnter
        pictureselected = PictureBox54
        If Not PictureBox54.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox54)
        End If
    End Sub
    Private Sub PictureBox55_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox55.MouseEnter
        pictureselected = PictureBox55
        If Not PictureBox55.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox55)
        End If
    End Sub
    Private Sub PictureBox56_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox56.MouseEnter
        pictureselected = PictureBox56
        If Not PictureBox56.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox56)
        End If
    End Sub
    Private Sub PictureBox57_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox57.MouseEnter
        pictureselected = PictureBox57
        If Not PictureBox57.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox57)
        End If
    End Sub
    Private Sub PictureBox58_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox58.MouseEnter
        pictureselected = PictureBox58
        If Not PictureBox58.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox58)
        End If
    End Sub
    Private Sub PictureBox59_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox59.MouseEnter
        pictureselected = PictureBox59
        If Not PictureBox59.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox59)
        End If
    End Sub
    Private Sub PictureBox60_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox60.MouseEnter
        pictureselected = PictureBox60
        If Not PictureBox60.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox60)
        End If
    End Sub
    Private Sub PictureBox61_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox61.MouseEnter
        pictureselected = PictureBox61
        If Not PictureBox61.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox61)
        End If
    End Sub
    Private Sub PictureBox62_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox62.MouseEnter
        pictureselected = PictureBox62
        If Not PictureBox62.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox62)
        End If
    End Sub
    Private Sub PictureBox63_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox63.MouseEnter
        pictureselected = PictureBox63
        If Not PictureBox63.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox63)
        End If
    End Sub
    Private Sub PictureBox64_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox56.MouseEnter
        pictureselected = PictureBox64
        If Not PictureBox64.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox64)
        End If
    End Sub
    Private Sub PictureBox65_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox65.MouseEnter
        pictureselected = PictureBox65
        If Not PictureBox65.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox65)
        End If
    End Sub
    Private Sub PictureBox66_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox66.MouseEnter
        pictureselected = PictureBox66
        If Not PictureBox66.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox66)
        End If
    End Sub
    Private Sub PictureBox67_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox67.MouseEnter
        pictureselected = PictureBox67
        If Not PictureBox67.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox67)
        End If
    End Sub
    Private Sub PictureBox68_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox68.MouseEnter
        pictureselected = PictureBox68
        If Not PictureBox68.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox68)
        End If
    End Sub
    Private Sub PictureBox69_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox69.MouseEnter
        pictureselected = PictureBox69
        If Not PictureBox69.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox69)
        End If
    End Sub
    Private Sub PictureBox70_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox70.MouseEnter
        pictureselected = PictureBox70
        If Not PictureBox70.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox70)
        End If
    End Sub
    Private Sub PictureBox71_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox71.MouseEnter
        pictureselected = PictureBox71
        If Not PictureBox71.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox71)
        End If
    End Sub
    Private Sub PictureBox72_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox72.MouseEnter
        pictureselected = PictureBox72
        If Not PictureBox72.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox72)
        End If
    End Sub
    Private Sub PictureBox73_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox73.MouseEnter
        pictureselected = PictureBox73
        If Not PictureBox73.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox73)
        End If
    End Sub
    Private Sub PictureBox74_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox74.MouseEnter
        pictureselected = PictureBox74
        If Not PictureBox74.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox74)
        End If
    End Sub
    Private Sub PictureBox75_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox75.MouseEnter
        pictureselected = PictureBox75
        If Not PictureBox75.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox75)
        End If
    End Sub
    Private Sub PictureBox76_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox76.MouseEnter
        pictureselected = PictureBox76
        If Not PictureBox76.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox76)
        End If
    End Sub
    Private Sub PictureBox77_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox77.MouseEnter
        pictureselected = PictureBox77
        If Not PictureBox77.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox77)
        End If
    End Sub
    Private Sub PictureBox78_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox78.MouseEnter
        pictureselected = PictureBox78
        If Not PictureBox78.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox78)
        End If
    End Sub
    Private Sub PictureBox79_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox79.MouseEnter
        pictureselected = PictureBox79
        If Not PictureBox79.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox79)
        End If
    End Sub
    Private Sub PictureBox80_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox80.MouseEnter
        pictureselected = PictureBox80
        If Not PictureBox80.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox80)
        End If
    End Sub
    Private Sub PictureBox82_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox82.MouseEnter
        pictureselected = PictureBox82
        If Not PictureBox82.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox82)
        End If
    End Sub
    Private Sub PictureBox83_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox83.MouseEnter
        pictureselected = PictureBox83
        If Not PictureBox83.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox83)
        End If
    End Sub
    Private Sub PictureBox84_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox84.MouseEnter
        pictureselected = PictureBox84
        If Not PictureBox84.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox84)
        End If
    End Sub
    Private Sub PictureBox85_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox85.MouseEnter
        pictureselected = PictureBox85
        If Not PictureBox85.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox85)
        End If
    End Sub
    Private Sub PictureBox86_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox86.MouseEnter
        pictureselected = PictureBox86
        If Not PictureBox86.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox86)
        End If
    End Sub
    Private Sub PictureBox87_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox87.MouseEnter
        pictureselected = PictureBox87
        If Not PictureBox87.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox87)
        End If
    End Sub
    Private Sub PictureBox88_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox88.MouseEnter
        pictureselected = PictureBox88
        If Not PictureBox88.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox88)
        End If
    End Sub
    Private Sub PictureBox89_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox89.MouseEnter
        pictureselected = PictureBox89
        If Not PictureBox89.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox89)
        End If
    End Sub
    Private Sub PictureBox90_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox90.MouseEnter
        pictureselected = PictureBox90
        If Not PictureBox90.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox90)
        End If
    End Sub
    Private Sub PictureBox91_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox91.MouseEnter
        pictureselected = PictureBox91
        If Not PictureBox91.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox91)
        End If
    End Sub
    Private Sub PictureBox92_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox92.MouseEnter
        pictureselected = PictureBox92
        If Not PictureBox92.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox92)
        End If
    End Sub
    Private Sub PictureBox93_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox93.MouseEnter
        pictureselected = PictureBox93
        If Not PictureBox93.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox93)
        End If
    End Sub
    Private Sub PictureBox94_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox94.MouseEnter
        pictureselected = PictureBox94
        If Not PictureBox94.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox94)
        End If
    End Sub
    Private Sub PictureBox95_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox95.MouseEnter
        pictureselected = PictureBox95
        If Not PictureBox95.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox95)
        End If
    End Sub
    Private Sub PictureBox96_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox96.MouseEnter
        pictureselected = PictureBox96
        If Not PictureBox96.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox96)
        End If
    End Sub
    Private Sub PictureBox97_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox97.MouseEnter
        pictureselected = PictureBox97
        If Not PictureBox97.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox97)
        End If
    End Sub
    Private Sub PictureBox98_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox98.MouseEnter
        pictureselected = PictureBox98
        If Not PictureBox98.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox98)
        End If
    End Sub
    Private Sub PictureBox99_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox99.MouseEnter
        pictureselected = PictureBox99
        If Not PictureBox99.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox99)
        End If
    End Sub
    Private Sub PictureBox100_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox100.MouseEnter
        pictureselected = PictureBox100
        If Not PictureBox100.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox100)
        End If
    End Sub
    Private Sub PictureBox101_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox101.MouseEnter
        pictureselected = PictureBox101
        If Not PictureBox101.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox101)
        End If
    End Sub
    Private Sub PictureBox102_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox102.MouseEnter
        pictureselected = PictureBox102
        If Not PictureBox102.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox102)
        End If
    End Sub
    Private Sub PictureBox103_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox103.MouseEnter
        pictureselected = PictureBox103
        If Not PictureBox103.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox103)
        End If
    End Sub
    Private Sub PictureBox104_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox104.MouseEnter
        pictureselected = PictureBox104
        If Not PictureBox104.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox104)
        End If
    End Sub
    Private Sub PictureBox105_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox105.MouseEnter
        pictureselected = PictureBox105
        If Not PictureBox105.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox105)
        End If
    End Sub
    Private Sub PictureBox106_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox106.MouseEnter
        pictureselected = PictureBox106
        If Not PictureBox106.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox106)
        End If
    End Sub
    Private Sub PictureBox107_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox107.MouseEnter
        pictureselected = PictureBox107
        If Not PictureBox107.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox107)
        End If
    End Sub
    Private Sub PictureBox108_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox108.MouseEnter
        pictureselected = PictureBox108
        If Not PictureBox108.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox108)
        End If
    End Sub
    Private Sub PictureBox109_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox109.MouseEnter
        pictureselected = PictureBox109
        If Not PictureBox109.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox109)
        End If
    End Sub
    Private Sub PictureBox110_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox110.MouseEnter
        pictureselected = PictureBox110
        If Not PictureBox110.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox110)
        End If
    End Sub
    Private Sub PictureBox111_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox111.MouseEnter
        pictureselected = PictureBox111
        If Not PictureBox111.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox111)
        End If
    End Sub
    Private Sub PictureBox112_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox112.MouseEnter
        pictureselected = PictureBox112
        If Not PictureBox112.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox112)
        End If
    End Sub
    Private Sub PictureBox113_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox113.MouseEnter
        pictureselected = PictureBox113
        If Not PictureBox113.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox113)
        End If
    End Sub
    Private Sub PictureBox114_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox114.MouseEnter
        pictureselected = PictureBox114
        If Not PictureBox114.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox114)
        End If
    End Sub
    Private Sub PictureBox115_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox115.MouseEnter
        pictureselected = PictureBox115
        If Not PictureBox115.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox115)
        End If
    End Sub
    Private Sub PictureBox116_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox116.MouseEnter
        pictureselected = PictureBox116
        If Not PictureBox116.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox116)
        End If
    End Sub
    Private Sub PictureBox117_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox117.MouseEnter
        pictureselected = PictureBox117
        If Not PictureBox117.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox117)
        End If
    End Sub
    Private Sub PictureBox118_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox118.MouseEnter
        pictureselected = PictureBox118
        If Not PictureBox118.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox118)
        End If
    End Sub
    Private Sub PictureBox119_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox119.MouseEnter
        pictureselected = PictureBox119
        If Not PictureBox119.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox119)
        End If
    End Sub
    Private Sub PictureBox120_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox120.MouseEnter
        pictureselected = PictureBox120
        If Not PictureBox120.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox120)
        End If
    End Sub
    Private Sub PictureBox121_MouseEnter(sender As Object, e As System.EventArgs) Handles PictureBox121.MouseEnter
        pictureselected = PictureBox121
        If Not PictureBox121.Image Is Nothing Then
            ToolStripStatusLabel15.Text = ToolTip1.GetToolTip(PictureBox121)
        End If
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then


                idselected = monstercard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox2)


            ElseIf pictureselected Is PictureBox3 Then
                idselected = monstercard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox3)

            ElseIf pictureselected Is PictureBox4 Then
                idselected = monstercard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox4)
            ElseIf pictureselected Is PictureBox5 Then
                idselected = monstercard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox5)
            ElseIf pictureselected Is PictureBox6 Then
                idselected = monstercard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox6)
            ElseIf pictureselected Is PictureBox7 Then
                idselected = spellcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox7)
            ElseIf pictureselected Is PictureBox8 Then
                idselected = spellcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox8)
            ElseIf pictureselected Is PictureBox9 Then
                idselected = spellcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox9)
            ElseIf pictureselected Is PictureBox10 Then
                idselected = spellcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox10)
            ElseIf pictureselected Is PictureBox11 Then
                idselected = spellcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox11)
            ElseIf pictureselected Is PictureBox21 Then
                idselected = spellcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox21)
            ElseIf pictureselected Is PictureBox12 Then
                idselected = handcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox12)
            ElseIf pictureselected Is PictureBox13 Then
                idselected = handcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox13)
            ElseIf pictureselected Is PictureBox14 Then
                idselected = handcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox14)
            ElseIf pictureselected Is PictureBox15 Then
                idselected = handcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox15)
            ElseIf pictureselected Is PictureBox16 Then
                idselected = handcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox16)
            ElseIf pictureselected Is PictureBox22 Then
                idselected = monstercard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox22)
            ElseIf pictureselected Is PictureBox23 Then
                idselected = monstercard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox23)
            ElseIf pictureselected Is PictureBox24 Then
                idselected = monstercard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox24)
            ElseIf pictureselected Is PictureBox25 Then
                idselected = monstercard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox25)
            ElseIf pictureselected Is PictureBox26 Then
                idselected = monstercard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox26)
            ElseIf pictureselected Is PictureBox27 Then
                idselected = spellcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox27)
            ElseIf pictureselected Is PictureBox28 Then
                idselected = spellcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox28)
            ElseIf pictureselected Is PictureBox29 Then
                idselected = spellcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox29)
            ElseIf pictureselected Is PictureBox30 Then
                idselected = spellcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox30)
            ElseIf pictureselected Is PictureBox31 Then
                idselected = spellcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox31)
            ElseIf pictureselected Is PictureBox40 Then
                idselected = spellcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox40)
            ElseIf pictureselected Is PictureBox32 Then
                idselected = handcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox32)
            ElseIf pictureselected Is PictureBox33 Then
                idselected = handcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox33)
            ElseIf pictureselected Is PictureBox34 Then
                idselected = handcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox34)
            ElseIf pictureselected Is PictureBox35 Then
                idselected = handcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox35)
            ElseIf pictureselected Is PictureBox36 Then
                idselected = handcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox36)
            ElseIf pictureselected Is PictureBox45 Then
                idselected = deckcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox45)
            ElseIf pictureselected Is PictureBox44 Then
                idselected = deckcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox44)
            ElseIf pictureselected Is PictureBox43 Then
                idselected = deckcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox43)
            ElseIf pictureselected Is PictureBox42 Then
                idselected = deckcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox42)
            ElseIf pictureselected Is PictureBox41 Then
                idselected = deckcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox41)
            ElseIf pictureselected Is PictureBox82 Then
                idselected = deckcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox82)
            ElseIf pictureselected Is PictureBox83 Then
                idselected = deckcard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox82)
            ElseIf pictureselected Is PictureBox84 Then
                idselected = deckcard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox84)
            ElseIf pictureselected Is PictureBox85 Then
                idselected = deckcard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox85)
            ElseIf pictureselected Is PictureBox86 Then
                idselected = deckcard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox86)
            ElseIf pictureselected Is PictureBox50 Then
                idselected = deckcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox50)
            ElseIf pictureselected Is PictureBox49 Then
                idselected = deckcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox49)
            ElseIf pictureselected Is PictureBox48 Then
                idselected = deckcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox48)
            ElseIf pictureselected Is PictureBox47 Then
                idselected = deckcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox47)
            ElseIf pictureselected Is PictureBox46 Then
                idselected = deckcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox46)
            ElseIf pictureselected Is PictureBox102 Then
                idselected = deckcard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox102)
            ElseIf pictureselected Is PictureBox103 Then
                idselected = deckcard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox103)
            ElseIf pictureselected Is PictureBox104 Then
                idselected = deckcard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox104)
            ElseIf pictureselected Is PictureBox105 Then
                idselected = deckcard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox105)
            ElseIf pictureselected Is PictureBox106 Then
                idselected = deckcard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox106)
            ElseIf pictureselected Is PictureBox55 Then
                idselected = gravecard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox55)
            ElseIf pictureselected Is PictureBox54 Then
                idselected = gravecard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox54)
            ElseIf pictureselected Is PictureBox53 Then
                idselected = gravecard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox53)
            ElseIf pictureselected Is PictureBox52 Then
                idselected = gravecard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox52)
            ElseIf pictureselected Is PictureBox51 Then
                idselected = gravecard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox51)
            ElseIf pictureselected Is PictureBox87 Then
                idselected = gravecard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox87)
            ElseIf pictureselected Is PictureBox88 Then
                idselected = gravecard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox88)
            ElseIf pictureselected Is PictureBox89 Then
                idselected = gravecard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox89)
            ElseIf pictureselected Is PictureBox90 Then
                idselected = gravecard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox90)
            ElseIf pictureselected Is PictureBox91 Then
                idselected = gravecard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox91)
            ElseIf pictureselected Is PictureBox60 Then
                idselected = gravecard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox60)
            ElseIf pictureselected Is PictureBox59 Then
                idselected = gravecard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox59)
            ElseIf pictureselected Is PictureBox58 Then
                idselected = gravecard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox58)
            ElseIf pictureselected Is PictureBox57 Then
                idselected = gravecard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox57)
            ElseIf pictureselected Is PictureBox56 Then
                idselected = gravecard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox56)
            ElseIf pictureselected Is PictureBox107 Then
                idselected = gravecard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox107)
            ElseIf pictureselected Is PictureBox108 Then
                idselected = gravecard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox108)
            ElseIf pictureselected Is PictureBox109 Then
                idselected = gravecard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox109)
            ElseIf pictureselected Is PictureBox110 Then
                idselected = gravecard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox110)
            ElseIf pictureselected Is PictureBox111 Then
                idselected = gravecard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox111)
            ElseIf pictureselected Is PictureBox65 Then
                idselected = removedcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox65)
            ElseIf pictureselected Is PictureBox64 Then
                idselected = removedcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox64)
            ElseIf pictureselected Is PictureBox63 Then
                idselected = removedcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox63)
            ElseIf pictureselected Is PictureBox62 Then
                idselected = removedcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox62)
            ElseIf pictureselected Is PictureBox61 Then
                idselected = removedcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox61)
            ElseIf pictureselected Is PictureBox92 Then
                idselected = removedcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox92)
            ElseIf pictureselected Is PictureBox93 Then
                idselected = removedcard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox93)
            ElseIf pictureselected Is PictureBox94 Then
                idselected = removedcard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox94)
            ElseIf pictureselected Is PictureBox95 Then
                idselected = removedcard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox95)
            ElseIf pictureselected Is PictureBox96 Then
                idselected = removedcard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox96)
            ElseIf pictureselected Is PictureBox70 Then
                idselected = removedcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox70)
            ElseIf pictureselected Is PictureBox69 Then
                idselected = removedcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox69)
            ElseIf pictureselected Is PictureBox68 Then
                idselected = removedcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox68)
            ElseIf pictureselected Is PictureBox67 Then
                idselected = removedcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox67)
            ElseIf pictureselected Is PictureBox66 Then
                idselected = removedcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox66)
            ElseIf pictureselected Is PictureBox112 Then
                idselected = removedcard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox112)
            ElseIf pictureselected Is PictureBox113 Then
                idselected = removedcard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox113)
            ElseIf pictureselected Is PictureBox114 Then
                idselected = removedcard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox114)
            ElseIf pictureselected Is PictureBox115 Then
                idselected = removedcard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox115)
            ElseIf pictureselected Is PictureBox116 Then
                idselected = removedcard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox116)
            ElseIf pictureselected Is PictureBox75 Then
                idselected = extracard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox75)
            ElseIf pictureselected Is PictureBox74 Then
                idselected = extracard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox74)
            ElseIf pictureselected Is PictureBox73 Then
                idselected = extracard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox73)
            ElseIf pictureselected Is PictureBox72 Then
                idselected = extracard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox72)
            ElseIf pictureselected Is PictureBox71 Then
                idselected = extracard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox71)
            ElseIf pictureselected Is PictureBox97 Then
                idselected = extracard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox97)
            ElseIf pictureselected Is PictureBox98 Then
                idselected = extracard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox98)
            ElseIf pictureselected Is PictureBox99 Then
                idselected = extracard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox99)
            ElseIf pictureselected Is PictureBox100 Then
                idselected = extracard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox100)
            ElseIf pictureselected Is PictureBox101 Then
                idselected = extracard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox101)
            ElseIf pictureselected Is PictureBox80 Then
                idselected = extracard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox80)
            ElseIf pictureselected Is PictureBox79 Then
                idselected = extracard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox79)
            ElseIf pictureselected Is PictureBox78 Then
                idselected = extracard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox78)
            ElseIf pictureselected Is PictureBox77 Then
                idselected = extracard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox77)
            ElseIf pictureselected Is PictureBox76 Then
                idselected = extracard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox76)
            ElseIf pictureselected Is PictureBox117 Then
                idselected = extracard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox117)
            ElseIf pictureselected Is PictureBox118 Then
                idselected = extracard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox118)
            ElseIf pictureselected Is PictureBox119 Then
                idselected = extracard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox119)
            ElseIf pictureselected Is PictureBox120 Then
                idselected = extracard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox120)
            ElseIf pictureselected Is PictureBox121 Then
                idselected = extracard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox121)

            End If


        End If
    End Sub

    Private Sub PasteCardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PasteCardToolStripMenuItem.Click
        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then

                monstercard11 = idselected
                monster11 = cardselected

                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard11 & ".jpg"
                PictureBox2.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox2, cardselected)
                Me.ToolStripMenuItem14.Text = monstercard11 & " - " & monster11
                Me.ToolStripMenuItem48.Text = monstercard11 & " - " & monster11

            ElseIf pictureselected Is PictureBox3 Then

                monstercard12 = idselected
                monster12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard12 & ".jpg"
                PictureBox3.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox3, cardselected)
                Me.ToolStripMenuItem15.Text = monstercard12 & " - " & monster12
                Me.ToolStripMenuItem49.Text = monstercard12 & " - " & monster12

            ElseIf pictureselected Is PictureBox4 Then

                monstercard13 = idselected
                monster13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard13 & ".jpg"
                PictureBox4.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox4, cardselected)
                Me.ToolStripMenuItem16.Text = monstercard13 & " - " & monster13
                Me.ToolStripMenuItem50.Text = monstercard13 & " - " & monster13

            ElseIf pictureselected Is PictureBox5 Then

                monstercard14 = idselected
                monster14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard14 & ".jpg"
                PictureBox5.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox5, cardselected)
                Me.ToolStripMenuItem17.Text = monstercard14 & " - " & monster14
                Me.ToolStripMenuItem51.Text = monstercard14 & " - " & monster14

            ElseIf pictureselected Is PictureBox6 Then

                monstercard15 = idselected
                monster15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard15 & ".jpg"
                PictureBox6.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox6, cardselected)
                Me.ToolStripMenuItem18.Text = monstercard15 & " - " & monster15
                Me.ToolStripMenuItem52.Text = monstercard15 & " - " & monster15

            ElseIf pictureselected Is PictureBox7 Then

                spellcard11 = idselected
                szone11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard11 & ".jpg"
                PictureBox7.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox7, cardselected)
                Me.ToolStripMenuItem24.Text = spellcard11 & " - " & szone11
                Me.ToolStripMenuItem60.Text = spellcard11 & " - " & szone11

            ElseIf pictureselected Is PictureBox8 Then

                spellcard12 = idselected
                szone12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard12 & ".jpg"
                PictureBox8.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox8, cardselected)
                Me.ToolStripMenuItem25.Text = spellcard12 & " - " & szone12
                Me.ToolStripMenuItem61.Text = spellcard12 & " - " & szone12

            ElseIf pictureselected Is PictureBox9 Then

                spellcard13 = idselected
                szone13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard13 & ".jpg"
                PictureBox9.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox9, cardselected)
                Me.ToolStripMenuItem26.Text = spellcard13 & " - " & szone13
                Me.ToolStripMenuItem62.Text = spellcard13 & " - " & szone13

            ElseIf pictureselected Is PictureBox10 Then

                spellcard14 = idselected
                szone14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard14 & ".jpg"
                PictureBox10.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox10, cardselected)
                Me.ToolStripMenuItem27.Text = spellcard14 & " - " & szone14
                Me.ToolStripMenuItem63.Text = spellcard14 & " - " & szone14

            ElseIf pictureselected Is PictureBox11 Then

                spellcard15 = idselected
                szone15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard15 & ".jpg"
                PictureBox11.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox11, cardselected)
                Me.ToolStripMenuItem28.Text = spellcard15 & " - " & szone15
                Me.ToolStripMenuItem64.Text = spellcard15 & " - " & szone15

            ElseIf pictureselected Is PictureBox21 Then

                spellcard16 = idselected
                szone16 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard16 & ".jpg"
                PictureBox21.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox21, cardselected)
                Me.FieldToolStripMenuItem.Text = spellcard16 & " - " & szone16
                Me.ToolStripMenuItem65.Text = spellcard16 & " - " & szone16

            ElseIf pictureselected Is PictureBox12 Then

                handcard11 = idselected
                hand11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard11 & ".jpg"
                PictureBox12.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox12, cardselected)
                Me.ToolStripMenuItem36.Text = handcard11 & " - " & hand11
                Me.ToolStripMenuItem74.Text = handcard11 & " - " & hand11


            ElseIf pictureselected Is PictureBox13 Then

                handcard12 = idselected
                hand12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard12 & ".jpg"
                PictureBox13.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox13, cardselected)
                Me.ToolStripMenuItem37.Text = handcard12 & " - " & hand12
                Me.ToolStripMenuItem75.Text = handcard12 & " - " & hand12

            ElseIf pictureselected Is PictureBox14 Then

                handcard13 = idselected
                hand13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard13 & ".jpg"
                PictureBox14.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox14, cardselected)
                Me.ToolStripMenuItem38.Text = handcard13 & " - " & hand13
                Me.ToolStripMenuItem76.Text = handcard13 & " - " & hand13

            ElseIf pictureselected Is PictureBox15 Then

                handcard14 = idselected
                hand14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard14 & ".jpg"
                PictureBox15.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox15, cardselected)
                Me.ToolStripMenuItem39.Text = handcard14 & " - " & hand14
                Me.ToolStripMenuItem77.Text = handcard14 & " - " & hand14

            ElseIf pictureselected Is PictureBox16 Then

                handcard15 = idselected
                hand15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard15 & ".jpg"
                PictureBox16.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox16, cardselected)
                Me.ToolStripMenuItem40.Text = handcard15 & " - " & hand15
                Me.ToolStripMenuItem78.Text = handcard15 & " - " & hand15

            ElseIf pictureselected Is PictureBox22 Then

                monstercard21 = idselected
                monster21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard21 & ".jpg"
                PictureBox21.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox21, cardselected)
                Me.ToolStripMenuItem19.Text = monstercard21 & " - " & monster21
                Me.ToolStripMenuItem54.Text = monstercard21 & " - " & monster21

            ElseIf pictureselected Is PictureBox23 Then

                monstercard22 = idselected
                monster22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard22 & ".jpg"
                PictureBox23.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox23, cardselected)
                Me.ToolStripMenuItem20.Text = monstercard22 & " - " & monster22
                Me.ToolStripMenuItem55.Text = monstercard22 & " - " & monster22

            ElseIf pictureselected Is PictureBox24 Then

                monstercard23 = idselected
                monster23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard23 & ".jpg"
                PictureBox24.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox24, cardselected)
                Me.ToolStripMenuItem21.Text = monstercard23 & " - " & monster23
                Me.ToolStripMenuItem56.Text = monstercard23 & " - " & monster23

            ElseIf pictureselected Is PictureBox25 Then

                monstercard24 = idselected
                monster24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard24 & ".jpg"
                PictureBox25.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox25, cardselected)
                Me.ToolStripMenuItem22.Text = monstercard24 & " - " & monster24
                Me.ToolStripMenuItem57.Text = monstercard24 & " - " & monster24

            ElseIf pictureselected Is PictureBox26 Then

                monstercard25 = idselected
                monster25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard25 & ".jpg"
                PictureBox26.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox26, cardselected)
                Me.ToolStripMenuItem23.Text = monstercard25 & " - " & monster25
                Me.ToolStripMenuItem58.Text = monstercard25 & " - " & monster25

            ElseIf pictureselected Is PictureBox27 Then

                spellcard21 = idselected
                szone21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard21 & ".jpg"
                PictureBox27.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox27, cardselected)
                Me.ToolStripMenuItem30.Text = spellcard21 & " - " & szone21
                Me.ToolStripMenuItem67.Text = spellcard21 & " - " & szone21

            ElseIf pictureselected Is PictureBox28 Then

                spellcard22 = idselected
                szone22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard22 & ".jpg"
                PictureBox28.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox28, cardselected)
                Me.ToolStripMenuItem31.Text = spellcard22 & " - " & szone22
                Me.ToolStripMenuItem68.Text = spellcard22 & " - " & szone22

            ElseIf pictureselected Is PictureBox29 Then

                spellcard23 = idselected
                szone23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard23 & ".jpg"
                PictureBox29.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox29, cardselected)
                Me.ToolStripMenuItem32.Text = spellcard23 & " - " & szone23
                Me.ToolStripMenuItem69.Text = spellcard23 & " - " & szone23

            ElseIf pictureselected Is PictureBox30 Then

                spellcard24 = idselected
                szone24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard24 & ".jpg"
                PictureBox30.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox30, cardselected)
                Me.ToolStripMenuItem33.Text = spellcard24 & " - " & szone24
                Me.ToolStripMenuItem70.Text = spellcard24 & " - " & szone24

            ElseIf pictureselected Is PictureBox31 Then

                spellcard25 = idselected
                szone25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard25 & ".jpg"
                PictureBox31.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox31, cardselected)
                Me.ToolStripMenuItem34.Text = spellcard25 & " - " & szone25
                Me.ToolStripMenuItem71.Text = spellcard25 & " - " & szone25

            ElseIf pictureselected Is PictureBox40 Then

                spellcard26 = idselected
                szone26 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard26 & ".jpg"
                PictureBox40.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox40, cardselected)
                Me.FieldToolStripMenuItem1.Text = spellcard26 & " - " & szone26
                Me.ToolStripMenuItem72.Text = spellcard26 & " - " & szone26

            ElseIf pictureselected Is PictureBox32 Then

                handcard21 = idselected
                hand21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard21 & ".jpg"
                PictureBox32.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox32, cardselected)
                Me.ToolStripMenuItem42.Text = handcard21 & " - " & hand21
                Me.ToolStripMenuItem80.Text = handcard21 & " - " & hand21

            ElseIf pictureselected Is PictureBox33 Then

                handcard22 = idselected
                hand22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard22 & ".jpg"
                PictureBox33.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox33, cardselected)
                Me.ToolStripMenuItem43.Text = handcard22 & " - " & hand22
                Me.ToolStripMenuItem81.Text = handcard22 & " - " & hand22

            ElseIf pictureselected Is PictureBox34 Then

                handcard23 = idselected
                hand23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard23 & ".jpg"
                PictureBox34.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox34, cardselected)
                Me.ToolStripMenuItem44.Text = handcard23 & " - " & hand23
                Me.ToolStripMenuItem82.Text = handcard23 & " - " & hand23

            ElseIf pictureselected Is PictureBox35 Then

                handcard24 = idselected
                hand24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard24 & ".jpg"
                PictureBox35.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox35, cardselected)
                Me.ToolStripMenuItem45.Text = handcard24 & " - " & hand24
                Me.ToolStripMenuItem83.Text = handcard24 & " - " & hand24

            ElseIf pictureselected Is PictureBox36 Then

                handcard25 = idselected
                hand25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard25 & ".jpg"
                PictureBox36.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox36, cardselected)
                Me.ToolStripMenuItem46.Text = handcard25 & " - " & hand25
                Me.ToolStripMenuItem84.Text = handcard25 & " - " & hand25

            ElseIf pictureselected Is PictureBox45 Then

                deckcard11 = idselected
                deck11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard11 & ".jpg"
                PictureBox45.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox45, cardselected)
                Me.ToolStripMenuItem4.Text = deckcard11 & " - " & deck11



            ElseIf pictureselected Is PictureBox44 Then

                deckcard12 = idselected
                deck12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard12 & ".jpg"
                PictureBox44.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox44, cardselected)
                Me.ToolStripMenuItem5.Text = deckcard12 & " - " & deck12

            ElseIf pictureselected Is PictureBox43 Then

                deckcard13 = idselected
                deck13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard13 & ".jpg"
                PictureBox43.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox43, cardselected)
                Me.ToolStripMenuItem6.Text = deckcard13 & " - " & deck13

            ElseIf pictureselected Is PictureBox42 Then

                deckcard14 = idselected
                deck14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard14 & ".jpg"
                PictureBox42.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox42, cardselected)
                Me.ToolStripMenuItem7.Text = deckcard14 & " - " & deck14

            ElseIf pictureselected Is PictureBox41 Then

                deckcard15 = idselected
                deck15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard15 & ".jpg"
                PictureBox41.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox41, cardselected)
                Me.ToolStripMenuItem8.Text = deckcard15 & " - " & deck15

            ElseIf pictureselected Is PictureBox82 Then

                deckcard16 = idselected
                deck16 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard16 & ".jpg"
                PictureBox82.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox82, cardselected)
                Me.ToolStripMenuItem9.Text = deckcard16 & " - " & deck16

            ElseIf pictureselected Is PictureBox83 Then

                deckcard17 = idselected
                deck17 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard17 & ".jpg"
                PictureBox83.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox83, cardselected)
                Me.ToolStripMenuItem10.Text = deckcard17 & " - " & deck17

            ElseIf pictureselected Is PictureBox84 Then

                deckcard18 = idselected
                deck18 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard18 & ".jpg"
                PictureBox84.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox84, cardselected)
                Me.ToolStripMenuItem11.Text = deckcard18 & " - " & deck18

            ElseIf pictureselected Is PictureBox85 Then

                deckcard19 = idselected
                deck19 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard19 & ".jpg"
                PictureBox85.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox85, cardselected)
                Me.ToolStripMenuItem9.Text = deckcard19 & " - " & deck19

            ElseIf pictureselected Is PictureBox86 Then

                deckcard10 = idselected
                deck10 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard10 & ".jpg"
                PictureBox86.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox86, cardselected)
                Me.ToolStripMenuItem9.Text = deckcard10 & " - " & deck10

            ElseIf pictureselected Is PictureBox50 Then

                deckcard21 = idselected
                deck21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard21 & ".jpg"
                PictureBox50.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox50, cardselected)
                Me.ToolStripMenuItem86.Text = deckcard21 & " - " & deck21

            ElseIf pictureselected Is PictureBox49 Then

                deckcard22 = idselected
                deck22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard22 & ".jpg"
                PictureBox49.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox49, cardselected)
                Me.ToolStripMenuItem87.Text = deckcard22 & " - " & deck22

            ElseIf pictureselected Is PictureBox48 Then

                deckcard23 = idselected
                deck23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard23 & ".jpg"
                PictureBox48.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox48, cardselected)
                Me.ToolStripMenuItem88.Text = deckcard23 & " - " & deck23

            ElseIf pictureselected Is PictureBox47 Then

                deckcard24 = idselected
                deck24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard24 & ".jpg"
                PictureBox47.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox47, cardselected)
                Me.ToolStripMenuItem89.Text = deckcard24 & " - " & deck24

            ElseIf pictureselected Is PictureBox46 Then

                deckcard25 = idselected
                deck25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard25 & ".jpg"
                PictureBox46.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox46, cardselected)
                Me.ToolStripMenuItem90.Text = deckcard25 & " - " & deck25


            ElseIf pictureselected Is PictureBox102 Then

                deckcard26 = idselected
                deck26 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard26 & ".jpg"
                PictureBox102.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox102, cardselected)
                Me.ToolStripMenuItem91.Text = deckcard26 & " - " & deck26

            ElseIf pictureselected Is PictureBox103 Then

                deckcard27 = idselected
                deck27 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard27 & ".jpg"
                PictureBox103.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox103, cardselected)
                Me.ToolStripMenuItem92.Text = deckcard27 & " - " & deck27

            ElseIf pictureselected Is PictureBox104 Then

                deckcard28 = idselected
                deck28 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard28 & ".jpg"
                PictureBox104.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox104, cardselected)
                Me.ToolStripMenuItem93.Text = deckcard28 & " - " & deck28

            ElseIf pictureselected Is PictureBox105 Then

                deckcard29 = idselected
                deck29 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard29 & ".jpg"
                PictureBox105.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox105, cardselected)
                Me.ToolStripMenuItem94.Text = deckcard29 & " - " & deck29

            ElseIf pictureselected Is PictureBox106 Then

                deckcard20 = idselected
                deck20 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard20 & ".jpg"
                PictureBox106.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox106, cardselected)
                Me.ToolStripMenuItem95.Text = deckcard20 & " - " & deck20

            ElseIf pictureselected Is PictureBox55 Then

                gravecard11 = idselected
                grave11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard11 & ".jpg"
                PictureBox55.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox55, cardselected)
                Me.ToolStripMenuItem96.Text = gravecard11 & " - " & grave11

            ElseIf pictureselected Is PictureBox54 Then

                gravecard12 = idselected
                grave12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard12 & ".jpg"
                PictureBox54.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox54, cardselected)
                Me.ToolStripMenuItem97.Text = gravecard12 & " - " & grave12

            ElseIf pictureselected Is PictureBox53 Then

                gravecard13 = idselected
                grave13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard13 & ".jpg"
                PictureBox53.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox53, cardselected)
                Me.ToolStripMenuItem98.Text = gravecard13 & " - " & grave13

            ElseIf pictureselected Is PictureBox52 Then

                gravecard14 = idselected
                grave14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard14 & ".jpg"
                PictureBox52.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox52, cardselected)
                Me.ToolStripMenuItem99.Text = gravecard14 & " - " & grave14

            ElseIf pictureselected Is PictureBox51 Then

                gravecard15 = idselected
                grave15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard15 & ".jpg"
                PictureBox51.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox51, cardselected)
                Me.ToolStripMenuItem98.Text = gravecard15 & " - " & grave15

            ElseIf pictureselected Is PictureBox87 Then

                gravecard16 = idselected
                grave16 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard16 & ".jpg"
                PictureBox87.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox87, cardselected)
                Me.ToolStripMenuItem101.Text = gravecard16 & " - " & grave16

            ElseIf pictureselected Is PictureBox88 Then

                gravecard17 = idselected
                grave17 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard17 & ".jpg"
                PictureBox88.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox88, cardselected)
                Me.ToolStripMenuItem102.Text = gravecard17 & " - " & grave17

            ElseIf pictureselected Is PictureBox89 Then

                gravecard18 = idselected
                grave18 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard18 & ".jpg"
                PictureBox89.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox89, cardselected)
                Me.ToolStripMenuItem103.Text = gravecard18 & " - " & grave18

            ElseIf pictureselected Is PictureBox90 Then

                gravecard19 = idselected
                grave19 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard19 & ".jpg"
                PictureBox90.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox90, cardselected)
                Me.ToolStripMenuItem104.Text = gravecard19 & " - " & grave19

            ElseIf pictureselected Is PictureBox91 Then

                gravecard10 = idselected
                grave10 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard10 & ".jpg"
                PictureBox91.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox91, cardselected)
                Me.ToolStripMenuItem105.Text = gravecard10 & " - " & grave10

            ElseIf pictureselected Is PictureBox60 Then

                gravecard21 = idselected
                grave21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard21 & ".jpg"
                PictureBox60.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox60, cardselected)
                Me.ToolStripMenuItem106.Text = gravecard21 & " - " & grave21

            ElseIf pictureselected Is PictureBox59 Then

                gravecard22 = idselected
                grave22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard22 & ".jpg"
                PictureBox59.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox59, cardselected)
                Me.ToolStripMenuItem107.Text = gravecard22 & " - " & grave22

            ElseIf pictureselected Is PictureBox58 Then

                gravecard23 = idselected
                grave23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard23 & ".jpg"
                PictureBox58.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox58, cardselected)
                Me.ToolStripMenuItem108.Text = gravecard23 & " - " & grave23

            ElseIf pictureselected Is PictureBox57 Then

                gravecard24 = idselected
                grave24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard24 & ".jpg"
                PictureBox57.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox57, cardselected)
                Me.ToolStripMenuItem109.Text = gravecard24 & " - " & grave24

            ElseIf pictureselected Is PictureBox56 Then

                gravecard25 = idselected
                grave25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard25 & ".jpg"
                PictureBox56.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox56, cardselected)
                Me.ToolStripMenuItem110.Text = gravecard25 & " - " & grave25

            ElseIf pictureselected Is PictureBox107 Then

                gravecard26 = idselected
                grave26 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard26 & ".jpg"
                PictureBox107.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox107, cardselected)
                Me.ToolStripMenuItem111.Text = gravecard26 & " - " & grave26

            ElseIf pictureselected Is PictureBox108 Then

                gravecard27 = idselected
                grave27 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard27 & ".jpg"
                PictureBox108.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox108, cardselected)
                Me.ToolStripMenuItem112.Text = gravecard27 & " - " & grave27

            ElseIf pictureselected Is PictureBox109 Then

                gravecard28 = idselected
                grave28 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard28 & ".jpg"
                PictureBox109.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox109, cardselected)
                Me.ToolStripMenuItem113.Text = gravecard28 & " - " & grave28

            ElseIf pictureselected Is PictureBox110 Then

                gravecard29 = idselected
                grave29 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard29 & ".jpg"
                PictureBox110.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox110, cardselected)
                Me.ToolStripMenuItem114.Text = gravecard29 & " - " & grave29

            ElseIf pictureselected Is PictureBox111 Then

                gravecard20 = idselected
                grave20 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard20 & ".jpg"
                PictureBox111.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox111, cardselected)
                Me.ToolStripMenuItem115.Text = gravecard20 & " - " & grave20

            ElseIf pictureselected Is PictureBox65 Then

                removedcard11 = idselected
                removed11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard11 & ".jpg"
                PictureBox65.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox65, cardselected)
                Me.ToolStripMenuItem117.Text = removedcard11 & " - " & removed11

            ElseIf pictureselected Is PictureBox64 Then

                removedcard12 = idselected
                removed12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard12 & ".jpg"
                PictureBox64.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox64, cardselected)
                Me.ToolStripMenuItem118.Text = removedcard12 & " - " & removed12

            ElseIf pictureselected Is PictureBox63 Then

                removedcard13 = idselected
                removed13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard13 & ".jpg"
                PictureBox63.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox63, cardselected)
                Me.ToolStripMenuItem119.Text = removedcard13 & " - " & removed13

            ElseIf pictureselected Is PictureBox62 Then

                removedcard14 = idselected
                removed14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard14 & ".jpg"
                PictureBox62.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox62, cardselected)
                Me.ToolStripMenuItem120.Text = removedcard14 & " - " & removed14

            ElseIf pictureselected Is PictureBox61 Then

                removedcard15 = idselected
                removed15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard15 & ".jpg"
                PictureBox61.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox61, cardselected)
                Me.ToolStripMenuItem121.Text = removedcard15 & " - " & removed15

            ElseIf pictureselected Is PictureBox92 Then

                removedcard16 = idselected
                removed16 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard16 & ".jpg"
                PictureBox92.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox92, cardselected)
                Me.ToolStripMenuItem123.Text = removedcard16 & " - " & removed16

            ElseIf pictureselected Is PictureBox93 Then

                removedcard17 = idselected
                removed17 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard17 & ".jpg"
                PictureBox93.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox93, cardselected)
                Me.ToolStripMenuItem124.Text = removedcard17 & " - " & removed17

            ElseIf pictureselected Is PictureBox94 Then

                removedcard18 = idselected
                removed18 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard18 & ".jpg"
                PictureBox94.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox94, cardselected)
                Me.ToolStripMenuItem125.Text = removedcard18 & " - " & removed18

            ElseIf pictureselected Is PictureBox95 Then

                removedcard19 = idselected
                removed19 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard19 & ".jpg"
                PictureBox95.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox95, cardselected)
                Me.ToolStripMenuItem126.Text = removedcard19 & " - " & removed19

            ElseIf pictureselected Is PictureBox96 Then

                removedcard10 = idselected
                removed10 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard10 & ".jpg"
                PictureBox96.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox96, cardselected)
                Me.ToolStripMenuItem127.Text = removedcard10 & " - " & removed10


            ElseIf pictureselected Is PictureBox70 Then

                removedcard21 = idselected
                removed21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard21 & ".jpg"
                PictureBox70.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox70, cardselected)
                Me.ToolStripMenuItem130.Text = removedcard21 & " - " & removed21

            ElseIf pictureselected Is PictureBox69 Then

                removedcard22 = idselected
                removed22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard22 & ".jpg"
                PictureBox69.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox69, cardselected)
                Me.ToolStripMenuItem131.Text = removedcard22 & " - " & removed22

            ElseIf pictureselected Is PictureBox68 Then

                removedcard23 = idselected
                removed23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard23 & ".jpg"
                PictureBox68.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox68, cardselected)
                Me.ToolStripMenuItem132.Text = removedcard23 & " - " & removed23


            ElseIf pictureselected Is PictureBox67 Then

                removedcard24 = idselected
                removed24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard24 & ".jpg"
                PictureBox67.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox67, cardselected)
                Me.ToolStripMenuItem133.Text = removedcard24 & " - " & removed24

            ElseIf pictureselected Is PictureBox66 Then

                removedcard25 = idselected
                removed25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard25 & ".jpg"
                PictureBox70.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox70, cardselected)
                Me.ToolStripMenuItem134.Text = removedcard25 & " - " & removed25

            ElseIf pictureselected Is PictureBox112 Then

                removedcard26 = idselected
                removed26 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard26 & ".jpg"
                PictureBox112.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox112, cardselected)
                Me.ToolStripMenuItem136.Text = removedcard26 & " - " & removed26

            ElseIf pictureselected Is PictureBox113 Then

                removedcard27 = idselected
                removed27 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard27 & ".jpg"
                PictureBox113.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox113, cardselected)
                Me.ToolStripMenuItem137.Text = removedcard27 & " - " & removed27

            ElseIf pictureselected Is PictureBox114 Then

                removedcard28 = idselected
                removed28 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard28 & ".jpg"
                PictureBox114.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox114, cardselected)
                Me.ToolStripMenuItem138.Text = removedcard28 & " - " & removed28

            ElseIf pictureselected Is PictureBox115 Then

                removedcard29 = idselected
                removed29 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard29 & ".jpg"
                PictureBox115.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox115, cardselected)
                Me.ToolStripMenuItem139.Text = removedcard29 & " - " & removed29

            ElseIf pictureselected Is PictureBox116 Then

                removedcard20 = idselected
                removed20 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard20 & ".jpg"
                PictureBox116.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox116, cardselected)
                Me.ToolStripMenuItem140.Text = removedcard20 & " - " & removed20

            ElseIf pictureselected Is PictureBox75 Then

                extracard11 = idselected
                extra11 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard11 & ".jpg"
                PictureBox75.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox75, cardselected)
                Me.ToolStripMenuItem143.Text = extracard11 & " - " & extra11

            ElseIf pictureselected Is PictureBox74 Then

                extracard12 = idselected
                extra12 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard12 & ".jpg"
                PictureBox74.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox74, cardselected)
                Me.ToolStripMenuItem144.Text = extracard12 & " - " & extra12

            ElseIf pictureselected Is PictureBox73 Then

                extracard13 = idselected
                extra13 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard13 & ".jpg"
                PictureBox73.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox73, cardselected)
                Me.ToolStripMenuItem145.Text = extracard13 & " - " & extra13

            ElseIf pictureselected Is PictureBox72 Then

                extracard14 = idselected
                extra14 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard14 & ".jpg"
                PictureBox72.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox72, cardselected)
                Me.ToolStripMenuItem146.Text = extracard14 & " - " & extra14

            ElseIf pictureselected Is PictureBox71 Then

                extracard15 = idselected
                extra15 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard15 & ".jpg"
                PictureBox71.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox71, cardselected)
                Me.ToolStripMenuItem147.Text = extracard15 & " - " & extra15

            ElseIf pictureselected Is PictureBox97 Then

                extracard16 = idselected
                extra16 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard16 & ".jpg"
                PictureBox97.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox97, cardselected)
                Me.ToolStripMenuItem149.Text = extracard16 & " - " & extra16

            ElseIf pictureselected Is PictureBox98 Then

                extracard17 = idselected
                extra17 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard17 & ".jpg"
                PictureBox98.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox98, cardselected)
                Me.ToolStripMenuItem150.Text = extracard17 & " - " & extra17

            ElseIf pictureselected Is PictureBox99 Then

                extracard18 = idselected
                extra18 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard18 & ".jpg"
                PictureBox99.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox99, cardselected)
                Me.ToolStripMenuItem151.Text = extracard18 & " - " & extra18

            ElseIf pictureselected Is PictureBox100 Then

                extracard19 = idselected
                extra19 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard16 & ".jpg"
                PictureBox100.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox100, cardselected)
                Me.ToolStripMenuItem152.Text = extracard19 & " - " & extra19


            ElseIf pictureselected Is PictureBox101 Then

                extracard10 = idselected
                extra10 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard10 & ".jpg"
                PictureBox101.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox101, cardselected)
                Me.ToolStripMenuItem153.Text = extracard10 & " - " & extra10

            ElseIf pictureselected Is PictureBox80 Then

                extracard21 = idselected
                extra21 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard21 & ".jpg"
                PictureBox80.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox80, cardselected)
                Me.ToolStripMenuItem156.Text = extracard21 & " - " & extra21

            ElseIf pictureselected Is PictureBox79 Then

                extracard22 = idselected
                extra22 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard22 & ".jpg"
                PictureBox79.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox79, cardselected)
                Me.ToolStripMenuItem157.Text = extracard22 & " - " & extra22

            ElseIf pictureselected Is PictureBox78 Then

                extracard23 = idselected
                extra23 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard23 & ".jpg"
                PictureBox78.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox78, cardselected)
                Me.ToolStripMenuItem158.Text = extracard23 & " - " & extra23

            ElseIf pictureselected Is PictureBox77 Then

                extracard24 = idselected
                extra24 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard24 & ".jpg"
                PictureBox77.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox77, cardselected)
                Me.ToolStripMenuItem159.Text = extracard24 & " - " & extra24

            ElseIf pictureselected Is PictureBox76 Then

                extracard25 = idselected
                extra25 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard25 & ".jpg"
                PictureBox76.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox76, cardselected)
                Me.ToolStripMenuItem160.Text = extracard25 & " - " & extra25


            ElseIf pictureselected Is PictureBox117 Then

                extracard26 = idselected
                extra26 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard26 & ".jpg"
                PictureBox117.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox117, cardselected)
                Me.ToolStripMenuItem162.Text = extracard26 & " - " & extra26

            ElseIf pictureselected Is PictureBox118 Then

                extracard27 = idselected
                extra27 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard27 & ".jpg"
                PictureBox118.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox118, cardselected)
                Me.ToolStripMenuItem163.Text = extracard27 & " - " & extra27

            ElseIf pictureselected Is PictureBox119 Then

                extracard28 = idselected
                extra28 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard28 & ".jpg"
                PictureBox119.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox119, cardselected)
                Me.ToolStripMenuItem164.Text = extracard28 & " - " & extra28

            ElseIf pictureselected Is PictureBox120 Then

                extracard29 = idselected
                extra29 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard29 & ".jpg"
                PictureBox120.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox120, cardselected)
                Me.ToolStripMenuItem165.Text = extracard29 & " - " & extra29

            ElseIf pictureselected Is PictureBox121 Then

                extracard20 = idselected
                extra20 = cardselected
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard20 & ".jpg"
                PictureBox121.ImageLocation = current
                Me.ToolTip1.SetToolTip(PictureBox121, cardselected)
                Me.ToolStripMenuItem166.Text = extracard20 & " - " & extra20

            End If



        End If
    End Sub

    Private Sub CutCardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CutCardToolStripMenuItem.Click
        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then



                idselected = monstercard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox2)

                Me.PictureBox2.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox2, Nothing)
                Me.ToolStripMenuItem14.Text = "1"
                Me.ToolStripMenuItem48.Text = "1"
                monstercard11 = Nothing
                monster11 = Nothing

            ElseIf pictureselected Is PictureBox3 Then

                idselected = monstercard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox3)

                Me.PictureBox3.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox3, Nothing)
                Me.ToolStripMenuItem15.Text = "2"
                Me.ToolStripMenuItem49.Text = "2"
                monstercard12 = Nothing
                monster12 = Nothing

            ElseIf pictureselected Is PictureBox4 Then
                idselected = monstercard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox4)

                Me.PictureBox4.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox4, Nothing)
                Me.ToolStripMenuItem16.Text = "3"
                Me.ToolStripMenuItem50.Text = "3"
                monstercard13 = Nothing
                monster13 = Nothing

            ElseIf pictureselected Is PictureBox5 Then
                idselected = monstercard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox5)

                Me.PictureBox5.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox5, Nothing)
                Me.ToolStripMenuItem17.Text = "4"
                Me.ToolStripMenuItem51.Text = "4"
                monstercard14 = Nothing
                monster14 = Nothing

            ElseIf pictureselected Is PictureBox6 Then
                idselected = monstercard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox6)

                Me.PictureBox6.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox6, Nothing)
                Me.ToolStripMenuItem18.Text = "5"
                Me.ToolStripMenuItem52.Text = "5"
                monstercard15 = Nothing
                monster15 = Nothing

            ElseIf pictureselected Is PictureBox7 Then
                idselected = spellcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox7)

                Me.PictureBox7.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox7, Nothing)
                Me.ToolStripMenuItem24.Text = "1"
                Me.ToolStripMenuItem60.Text = "1"
                spellcard11 = Nothing
                szone11 = Nothing

            ElseIf pictureselected Is PictureBox8 Then
                idselected = spellcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox8)

                Me.PictureBox8.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox8, Nothing)
                Me.ToolStripMenuItem25.Text = "2"
                Me.ToolStripMenuItem61.Text = "2"
                spellcard12 = Nothing
                szone12 = Nothing

            ElseIf pictureselected Is PictureBox9 Then
                idselected = spellcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox9)

                Me.PictureBox9.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox9, Nothing)
                Me.ToolStripMenuItem26.Text = "3"
                Me.ToolStripMenuItem62.Text = "3"
                spellcard13 = Nothing
                szone13 = Nothing

            ElseIf pictureselected Is PictureBox10 Then
                idselected = spellcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox10)

                Me.PictureBox10.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox10, Nothing)
                Me.ToolStripMenuItem27.Text = "4"
                Me.ToolStripMenuItem63.Text = "4"
                spellcard14 = Nothing
                szone14 = Nothing

            ElseIf pictureselected Is PictureBox11 Then
                idselected = spellcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox11)

                Me.PictureBox11.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox11, Nothing)
                Me.ToolStripMenuItem28.Text = "5"
                Me.ToolStripMenuItem64.Text = "5"
                spellcard15 = Nothing
                szone15 = Nothing

            ElseIf pictureselected Is PictureBox21 Then
                idselected = spellcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox21)

                Me.PictureBox21.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox21, Nothing)
                Me.FieldToolStripMenuItem.Text = "Field"
                Me.ToolStripMenuItem65.Text = "Field"
                spellcard16 = Nothing
                szone16 = Nothing

            ElseIf pictureselected Is PictureBox12 Then
                idselected = handcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox12)

                Me.PictureBox12.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox12, Nothing)
                Me.ToolStripMenuItem36.Text = "1"
                Me.ToolStripMenuItem74.Text = "1"
                handcard11 = Nothing
                hand11 = Nothing

            ElseIf pictureselected Is PictureBox13 Then
                idselected = handcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox13)

                Me.PictureBox13.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox13, Nothing)
                Me.ToolStripMenuItem37.Text = "2"
                Me.ToolStripMenuItem75.Text = "2"
                handcard12 = Nothing
                hand12 = Nothing

            ElseIf pictureselected Is PictureBox14 Then
                idselected = handcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox14)

                Me.PictureBox14.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox14, Nothing)
                Me.ToolStripMenuItem38.Text = "3"
                Me.ToolStripMenuItem76.Text = "3"
                handcard13 = Nothing
                hand13 = Nothing

            ElseIf pictureselected Is PictureBox15 Then
                idselected = handcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox15)

                Me.PictureBox15.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox15, Nothing)
                Me.ToolStripMenuItem39.Text = "4"
                Me.ToolStripMenuItem77.Text = "4"
                handcard14 = Nothing
                hand14 = Nothing

            ElseIf pictureselected Is PictureBox16 Then
                idselected = handcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox16)

                Me.PictureBox16.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox16, Nothing)
                Me.ToolStripMenuItem40.Text = "5"
                Me.ToolStripMenuItem78.Text = "5"
                handcard15 = Nothing
                hand15 = Nothing

            ElseIf pictureselected Is PictureBox22 Then
                idselected = monstercard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox22)

                Me.PictureBox22.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox22, Nothing)
                Me.ToolStripMenuItem19.Text = "1"
                Me.ToolStripMenuItem54.Text = "1"
                monstercard21 = Nothing
                monster21 = Nothing

            ElseIf pictureselected Is PictureBox23 Then
                idselected = monstercard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox23)

                Me.PictureBox23.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox23, Nothing)
                Me.ToolStripMenuItem20.Text = "2"
                Me.ToolStripMenuItem55.Text = "2"
                monstercard22 = Nothing
                monster22 = Nothing

            ElseIf pictureselected Is PictureBox24 Then
                idselected = monstercard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox24)

                Me.PictureBox24.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox24, Nothing)
                Me.ToolStripMenuItem21.Text = "3"
                Me.ToolStripMenuItem56.Text = "3"
                monstercard23 = Nothing
                monster23 = Nothing

            ElseIf pictureselected Is PictureBox25 Then
                idselected = monstercard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox25)

                Me.PictureBox25.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox25, Nothing)
                Me.ToolStripMenuItem22.Text = "4"
                Me.ToolStripMenuItem57.Text = "4"
                monstercard24 = Nothing
                monster24 = Nothing

            ElseIf pictureselected Is PictureBox26 Then
                idselected = monstercard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox26)

                Me.PictureBox26.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox26, Nothing)
                Me.ToolStripMenuItem23.Text = "5"
                Me.ToolStripMenuItem58.Text = "5"
                monstercard25 = Nothing
                monster25 = Nothing

            ElseIf pictureselected Is PictureBox27 Then
                idselected = spellcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox27)

                Me.PictureBox27.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox27, Nothing)
                Me.ToolStripMenuItem30.Text = "1"
                Me.ToolStripMenuItem67.Text = "1"
                spellcard21 = Nothing
                szone21 = Nothing

            ElseIf pictureselected Is PictureBox28 Then
                idselected = spellcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox28)

                Me.PictureBox28.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox28, Nothing)
                Me.ToolStripMenuItem31.Text = "2"
                Me.ToolStripMenuItem68.Text = "2"
                spellcard22 = Nothing
                szone22 = Nothing

            ElseIf pictureselected Is PictureBox29 Then
                idselected = spellcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox29)

                Me.PictureBox29.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox29, Nothing)
                Me.ToolStripMenuItem32.Text = "3"
                Me.ToolStripMenuItem69.Text = "3"
                spellcard23 = Nothing
                szone23 = Nothing

            ElseIf pictureselected Is PictureBox30 Then
                idselected = spellcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox30)

                Me.PictureBox30.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox30, Nothing)
                Me.ToolStripMenuItem33.Text = "4"
                Me.ToolStripMenuItem70.Text = "4"
                spellcard24 = Nothing
                szone24 = Nothing

            ElseIf pictureselected Is PictureBox31 Then
                idselected = spellcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox31)

                Me.PictureBox31.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox31, Nothing)
                Me.ToolStripMenuItem34.Text = "5"
                Me.ToolStripMenuItem71.Text = "5"
                spellcard25 = Nothing
                szone25 = Nothing

            ElseIf pictureselected Is PictureBox40 Then
                idselected = spellcard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox40)

                Me.PictureBox40.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox40, Nothing)
                Me.FieldToolStripMenuItem1.Text = "Field"
                Me.ToolStripMenuItem72.Text = "Field"
                spellcard26 = Nothing
                szone26 = Nothing

            ElseIf pictureselected Is PictureBox32 Then
                idselected = handcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox32)

                Me.PictureBox32.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox32, Nothing)
                Me.ToolStripMenuItem42.Text = "1"
                Me.ToolStripMenuItem80.Text = "1"
                handcard21 = Nothing
                hand21 = Nothing

            ElseIf pictureselected Is PictureBox33 Then
                idselected = handcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox33)

                Me.PictureBox33.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox33, Nothing)
                Me.ToolStripMenuItem43.Text = "2"
                Me.ToolStripMenuItem81.Text = "2"
                handcard22 = Nothing
                hand22 = Nothing

            ElseIf pictureselected Is PictureBox34 Then
                idselected = handcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox34)

                Me.PictureBox34.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox34, Nothing)
                Me.ToolStripMenuItem44.Text = "3"
                Me.ToolStripMenuItem82.Text = "3"
                handcard23 = Nothing
                hand23 = Nothing

            ElseIf pictureselected Is PictureBox35 Then
                idselected = handcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox35)

                Me.PictureBox35.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox35, Nothing)
                Me.ToolStripMenuItem45.Text = "4"
                Me.ToolStripMenuItem83.Text = "4"
                handcard24 = Nothing
                hand24 = Nothing

            ElseIf pictureselected Is PictureBox36 Then
                idselected = handcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox36)

                Me.PictureBox36.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox36, Nothing)
                Me.ToolStripMenuItem46.Text = "5"
                Me.ToolStripMenuItem84.Text = "5"
                handcard25 = Nothing
                hand25 = Nothing

            ElseIf pictureselected Is PictureBox45 Then
                idselected = deckcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox45)

                Me.PictureBox45.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox45, Nothing)
                Me.ToolStripMenuItem4.Text = "1"
                deckcard11 = Nothing
                deck11 = Nothing

            ElseIf pictureselected Is PictureBox44 Then
                idselected = deckcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox44)

                Me.PictureBox44.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox44, Nothing)
                Me.ToolStripMenuItem5.Text = "2"
                deckcard12 = Nothing
                deck12 = Nothing

            ElseIf pictureselected Is PictureBox43 Then
                idselected = deckcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox43)

                Me.PictureBox43.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox43, Nothing)
                Me.ToolStripMenuItem6.Text = "3"
                deckcard13 = Nothing
                deck13 = Nothing

            ElseIf pictureselected Is PictureBox42 Then
                idselected = deckcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox42)

                Me.PictureBox42.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox42, Nothing)
                Me.ToolStripMenuItem7.Text = "4"
                deckcard14 = Nothing
                deck14 = Nothing

            ElseIf pictureselected Is PictureBox41 Then
                idselected = deckcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox41)

                Me.PictureBox41.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox41, Nothing)
                Me.ToolStripMenuItem8.Text = "5"
                deckcard15 = Nothing
                deck15 = Nothing

            ElseIf pictureselected Is PictureBox82 Then
                idselected = deckcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox82)

                Me.PictureBox82.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox82, Nothing)
                Me.ToolStripMenuItem9.Text = "6"
                deckcard16 = Nothing
                deck16 = Nothing

            ElseIf pictureselected Is PictureBox83 Then
                idselected = deckcard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox83)

                Me.PictureBox83.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox83, Nothing)
                Me.ToolStripMenuItem10.Text = "7"
                deckcard17 = Nothing
                deck17 = Nothing

            ElseIf pictureselected Is PictureBox84 Then
                idselected = deckcard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox84)

                Me.PictureBox84.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox84, Nothing)
                Me.ToolStripMenuItem11.Text = "8"
                deckcard18 = Nothing
                deck18 = Nothing

            ElseIf pictureselected Is PictureBox85 Then
                idselected = deckcard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox85)

                Me.PictureBox85.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox85, Nothing)
                Me.ToolStripMenuItem12.Text = "9"
                deckcard19 = Nothing
                deck19 = Nothing

            ElseIf pictureselected Is PictureBox86 Then
                idselected = deckcard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox86)

                Me.PictureBox86.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox86, Nothing)
                Me.ToolStripMenuItem11.Text = "10"
                deckcard10 = Nothing
                deck10 = Nothing

            ElseIf pictureselected Is PictureBox50 Then
                idselected = deckcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox50)

                Me.PictureBox50.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox50, Nothing)
                Me.ToolStripMenuItem86.Text = "1"
                deckcard21 = Nothing
                deck21 = Nothing

            ElseIf pictureselected Is PictureBox49 Then
                idselected = deckcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox49)

                Me.PictureBox49.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox49, Nothing)
                Me.ToolStripMenuItem87.Text = "2"
                deckcard22 = Nothing
                deck22 = Nothing

            ElseIf pictureselected Is PictureBox48 Then
                idselected = deckcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox48)

                Me.PictureBox48.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox48, Nothing)
                Me.ToolStripMenuItem88.Text = "3"
                deckcard23 = Nothing
                deck23 = Nothing

            ElseIf pictureselected Is PictureBox47 Then
                idselected = deckcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox47)

                Me.PictureBox47.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox47, Nothing)
                Me.ToolStripMenuItem89.Text = "4"
                deckcard24 = Nothing
                deck24 = Nothing

            ElseIf pictureselected Is PictureBox46 Then
                idselected = deckcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox46)

                Me.PictureBox46.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox46, Nothing)
                Me.ToolStripMenuItem90.Text = "5"
                deckcard25 = Nothing
                deck25 = Nothing


            ElseIf pictureselected Is PictureBox102 Then
                idselected = deckcard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox102)

                Me.PictureBox102.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox102, Nothing)
                Me.ToolStripMenuItem91.Text = "6"
                deckcard26 = Nothing
                deck26 = Nothing

            ElseIf pictureselected Is PictureBox103 Then
                idselected = deckcard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox103)

                Me.PictureBox103.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox103, Nothing)
                Me.ToolStripMenuItem92.Text = "7"
                deckcard27 = Nothing
                deck27 = Nothing

            ElseIf pictureselected Is PictureBox104 Then
                idselected = deckcard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox104)

                Me.PictureBox104.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox104, Nothing)
                Me.ToolStripMenuItem93.Text = "8"
                deckcard28 = Nothing
                deck28 = Nothing

            ElseIf pictureselected Is PictureBox105 Then
                idselected = deckcard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox103)

                Me.PictureBox105.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox105, Nothing)
                Me.ToolStripMenuItem94.Text = "9"
                deckcard29 = Nothing
                deck29 = Nothing

            ElseIf pictureselected Is PictureBox106 Then
                idselected = deckcard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox106)

                Me.PictureBox106.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox106, Nothing)
                Me.ToolStripMenuItem95.Text = "10"
                deckcard20 = Nothing
                deck20 = Nothing

            ElseIf pictureselected Is PictureBox55 Then
                idselected = gravecard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox55)

                Me.PictureBox55.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox55, Nothing)
                Me.ToolStripMenuItem96.Text = "1"
                gravecard11 = Nothing
                grave11 = Nothing

            ElseIf pictureselected Is PictureBox54 Then
                idselected = gravecard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox54)

                Me.PictureBox54.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox54, Nothing)
                Me.ToolStripMenuItem97.Text = "2"
                gravecard12 = Nothing
                grave12 = Nothing

            ElseIf pictureselected Is PictureBox53 Then
                idselected = gravecard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox53)

                Me.PictureBox53.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox53, Nothing)
                Me.ToolStripMenuItem98.Text = "3"
                gravecard13 = Nothing
                grave13 = Nothing

            ElseIf pictureselected Is PictureBox52 Then
                idselected = gravecard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox52)

                Me.PictureBox52.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox52, Nothing)
                Me.ToolStripMenuItem99.Text = "4"
                gravecard14 = Nothing
                grave14 = Nothing

            ElseIf pictureselected Is PictureBox51 Then
                idselected = gravecard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox51)

                Me.PictureBox51.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox51, Nothing)
                Me.ToolStripMenuItem100.Text = "1"
                gravecard15 = Nothing
                grave15 = Nothing

            ElseIf pictureselected Is PictureBox87 Then
                idselected = gravecard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox87)

                Me.PictureBox87.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox87, Nothing)
                Me.ToolStripMenuItem101.Text = "6"
                gravecard16 = Nothing
                grave16 = Nothing

            ElseIf pictureselected Is PictureBox88 Then
                idselected = gravecard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox88)

                Me.PictureBox88.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox88, Nothing)
                Me.ToolStripMenuItem102.Text = "7"
                gravecard17 = Nothing
                grave17 = Nothing


            ElseIf pictureselected Is PictureBox89 Then
                idselected = gravecard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox89)

                Me.PictureBox89.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox89, Nothing)
                Me.ToolStripMenuItem103.Text = "8"
                gravecard18 = Nothing
                grave18 = Nothing

            ElseIf pictureselected Is PictureBox90 Then
                idselected = gravecard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox90)

                Me.PictureBox90.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox90, Nothing)
                Me.ToolStripMenuItem104.Text = "9"
                gravecard19 = Nothing
                grave19 = Nothing


            ElseIf pictureselected Is PictureBox91 Then
                idselected = gravecard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox91)

                Me.PictureBox91.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox91, Nothing)
                Me.ToolStripMenuItem105.Text = "10"
                gravecard10 = Nothing
                grave10 = Nothing

            ElseIf pictureselected Is PictureBox60 Then
                idselected = gravecard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox60)

                Me.PictureBox60.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox60, Nothing)
                Me.ToolStripMenuItem106.Text = "1"
                gravecard21 = Nothing
                grave21 = Nothing

            ElseIf pictureselected Is PictureBox59 Then
                idselected = gravecard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox61)

                Me.PictureBox59.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox59, Nothing)
                Me.ToolStripMenuItem107.Text = "2"
                gravecard22 = Nothing
                grave22 = Nothing

            ElseIf pictureselected Is PictureBox58 Then
                idselected = gravecard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox62)

                Me.PictureBox58.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox58, Nothing)
                Me.ToolStripMenuItem108.Text = "3"
                gravecard23 = Nothing
                grave23 = Nothing

            ElseIf pictureselected Is PictureBox57 Then
                idselected = gravecard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox63)

                Me.PictureBox57.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox57, Nothing)
                Me.ToolStripMenuItem109.Text = "4"
                gravecard24 = Nothing
                grave24 = Nothing

            ElseIf pictureselected Is PictureBox56 Then
                idselected = gravecard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox56)

                Me.PictureBox56.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox56, Nothing)
                Me.ToolStripMenuItem110.Text = "5"
                gravecard25 = Nothing
                grave25 = Nothing

            ElseIf pictureselected Is PictureBox107 Then
                idselected = gravecard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox107)

                Me.PictureBox107.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox107, Nothing)
                Me.ToolStripMenuItem111.Text = "6"
                gravecard26 = Nothing
                grave26 = Nothing

            ElseIf pictureselected Is PictureBox108 Then
                idselected = gravecard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox108)

                Me.PictureBox108.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox108, Nothing)
                Me.ToolStripMenuItem112.Text = "7"
                gravecard27 = Nothing
                grave27 = Nothing

            ElseIf pictureselected Is PictureBox109 Then
                idselected = gravecard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox109)

                Me.PictureBox109.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox109, Nothing)
                Me.ToolStripMenuItem113.Text = "8"
                gravecard28 = Nothing
                grave28 = Nothing

            ElseIf pictureselected Is PictureBox110 Then
                idselected = gravecard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox110)

                Me.PictureBox110.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox110, Nothing)
                Me.ToolStripMenuItem114.Text = "9"
                gravecard29 = Nothing
                grave29 = Nothing


            ElseIf pictureselected Is PictureBox111 Then
                idselected = gravecard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox111)

                Me.PictureBox111.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox111, Nothing)
                Me.ToolStripMenuItem115.Text = "10"
                gravecard20 = Nothing
                grave20 = Nothing

            ElseIf pictureselected Is PictureBox65 Then
                idselected = removedcard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox65)

                Me.PictureBox65.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox65, Nothing)
                Me.ToolStripMenuItem117.Text = "1"
                removedcard11 = Nothing
                removed11 = Nothing

            ElseIf pictureselected Is PictureBox64 Then
                idselected = removedcard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox64)

                Me.PictureBox64.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox64, Nothing)
                Me.ToolStripMenuItem118.Text = "2"
                removedcard12 = Nothing
                removed12 = Nothing

            ElseIf pictureselected Is PictureBox63 Then
                idselected = removedcard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox63)

                Me.PictureBox63.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox63, Nothing)
                Me.ToolStripMenuItem119.Text = "3"
                removedcard13 = Nothing
                removed13 = Nothing

            ElseIf pictureselected Is PictureBox62 Then
                idselected = removedcard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox62)

                Me.PictureBox62.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox62, Nothing)
                Me.ToolStripMenuItem120.Text = "4"
                removedcard14 = Nothing
                removed14 = Nothing

            ElseIf pictureselected Is PictureBox61 Then
                idselected = removedcard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox61)

                Me.PictureBox61.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox61, Nothing)
                Me.ToolStripMenuItem121.Text = "5"
                removedcard15 = Nothing
                removed15 = Nothing


            ElseIf pictureselected Is PictureBox92 Then
                idselected = removedcard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox82)

                Me.PictureBox92.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox92, Nothing)
                Me.ToolStripMenuItem123.Text = "6"
                removedcard16 = Nothing
                removed16 = Nothing

            ElseIf pictureselected Is PictureBox93 Then
                idselected = removedcard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox93)

                Me.PictureBox93.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox93, Nothing)
                Me.ToolStripMenuItem124.Text = "7"
                removedcard17 = Nothing
                removed17 = Nothing

            ElseIf pictureselected Is PictureBox94 Then
                idselected = removedcard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox94)

                Me.PictureBox94.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox94, Nothing)
                Me.ToolStripMenuItem125.Text = "8"
                removedcard18 = Nothing
                removed18 = Nothing

            ElseIf pictureselected Is PictureBox95 Then
                idselected = removedcard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox95)

                Me.PictureBox95.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox95, Nothing)
                Me.ToolStripMenuItem126.Text = "9"
                removedcard19 = Nothing
                removed19 = Nothing

            ElseIf pictureselected Is PictureBox96 Then
                idselected = removedcard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox96)

                Me.PictureBox96.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox96, Nothing)
                Me.ToolStripMenuItem127.Text = "10"
                removedcard10 = Nothing
                removed10 = Nothing

            ElseIf pictureselected Is PictureBox70 Then
                idselected = removedcard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox70)

                Me.PictureBox70.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox70, Nothing)
                Me.ToolStripMenuItem130.Text = "1"
                removedcard21 = Nothing
                removed21 = Nothing

            ElseIf pictureselected Is PictureBox69 Then
                idselected = removedcard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox69)

                Me.PictureBox69.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox69, Nothing)
                Me.ToolStripMenuItem131.Text = "2"
                removedcard22 = Nothing
                removed22 = Nothing

            ElseIf pictureselected Is PictureBox68 Then
                idselected = removedcard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox68)

                Me.PictureBox68.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox68, Nothing)
                Me.ToolStripMenuItem132.Text = "3"
                removedcard23 = Nothing
                removed23 = Nothing

            ElseIf pictureselected Is PictureBox67 Then
                idselected = removedcard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox67)

                Me.PictureBox67.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox67, Nothing)
                Me.ToolStripMenuItem133.Text = "4"
                removedcard24 = Nothing
                removed24 = Nothing

            ElseIf pictureselected Is PictureBox66 Then
                idselected = removedcard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox66)

                Me.PictureBox66.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox66, Nothing)
                Me.ToolStripMenuItem134.Text = "5"
                removedcard25 = Nothing
                removed25 = Nothing

            ElseIf pictureselected Is PictureBox112 Then
                idselected = removedcard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox112)

                Me.PictureBox112.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox112, Nothing)
                Me.ToolStripMenuItem136.Text = "6"
                removedcard26 = Nothing
                removed26 = Nothing

            ElseIf pictureselected Is PictureBox113 Then
                idselected = removedcard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox113)

                Me.PictureBox113.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox113, Nothing)
                Me.ToolStripMenuItem137.Text = "7"
                removedcard27 = Nothing
                removed27 = Nothing

            ElseIf pictureselected Is PictureBox114 Then
                idselected = removedcard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox114)

                Me.PictureBox114.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox114, Nothing)
                Me.ToolStripMenuItem138.Text = "8"
                removedcard28 = Nothing
                removed28 = Nothing

            ElseIf pictureselected Is PictureBox115 Then
                idselected = removedcard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox115)

                Me.PictureBox115.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox115, Nothing)
                Me.ToolStripMenuItem139.Text = "9"
                removedcard29 = Nothing
                removed29 = Nothing

            ElseIf pictureselected Is PictureBox116 Then
                idselected = removedcard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox116)

                Me.PictureBox116.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox116, Nothing)
                Me.ToolStripMenuItem140.Text = "10"
                removedcard20 = Nothing
                removed20 = Nothing

            ElseIf pictureselected Is PictureBox75 Then
                idselected = extracard11
                cardselected = Me.ToolTip1.GetToolTip(PictureBox75)

                Me.PictureBox75.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox75, Nothing)
                Me.ToolStripMenuItem143.Text = "1"
                extracard11 = Nothing
                extra11 = Nothing

            ElseIf pictureselected Is PictureBox74 Then
                idselected = extracard12
                cardselected = Me.ToolTip1.GetToolTip(PictureBox74)

                Me.PictureBox74.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox74, Nothing)
                Me.ToolStripMenuItem144.Text = "2"
                extracard12 = Nothing
                extra12 = Nothing

            ElseIf pictureselected Is PictureBox73 Then
                idselected = extracard13
                cardselected = Me.ToolTip1.GetToolTip(PictureBox73)

                Me.PictureBox73.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox73, Nothing)
                Me.ToolStripMenuItem145.Text = "3"
                extracard13 = Nothing
                extra13 = Nothing

            ElseIf pictureselected Is PictureBox72 Then
                idselected = extracard14
                cardselected = Me.ToolTip1.GetToolTip(PictureBox72)

                Me.PictureBox72.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox72, Nothing)
                Me.ToolStripMenuItem146.Text = "4"
                extracard14 = Nothing
                extra14 = Nothing

            ElseIf pictureselected Is PictureBox71 Then
                idselected = extracard15
                cardselected = Me.ToolTip1.GetToolTip(PictureBox71)

                Me.PictureBox71.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox71, Nothing)
                Me.ToolStripMenuItem147.Text = "5"
                extracard15 = Nothing
                extra15 = Nothing

            ElseIf pictureselected Is PictureBox97 Then
                idselected = extracard16
                cardselected = Me.ToolTip1.GetToolTip(PictureBox97)

                Me.PictureBox97.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox97, Nothing)
                Me.ToolStripMenuItem149.Text = "6"
                extracard16 = Nothing
                extra16 = Nothing

            ElseIf pictureselected Is PictureBox98 Then
                idselected = extracard17
                cardselected = Me.ToolTip1.GetToolTip(PictureBox98)

                Me.PictureBox98.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox98, Nothing)
                Me.ToolStripMenuItem150.Text = "7"
                extracard17 = Nothing
                extra17 = Nothing

            ElseIf pictureselected Is PictureBox99 Then
                idselected = extracard18
                cardselected = Me.ToolTip1.GetToolTip(PictureBox99)

                Me.PictureBox99.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox99, Nothing)
                Me.ToolStripMenuItem151.Text = "8"
                extracard18 = Nothing
                extra18 = Nothing

            ElseIf pictureselected Is PictureBox100 Then
                idselected = extracard19
                cardselected = Me.ToolTip1.GetToolTip(PictureBox100)

                Me.PictureBox100.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox100, Nothing)
                Me.ToolStripMenuItem152.Text = "9"
                extracard19 = Nothing
                extra19 = Nothing

            ElseIf pictureselected Is PictureBox101 Then
                idselected = extracard10
                cardselected = Me.ToolTip1.GetToolTip(PictureBox101)

                Me.PictureBox101.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox101, Nothing)
                Me.ToolStripMenuItem153.Text = "10"
                extracard10 = Nothing
                extra10 = Nothing

            ElseIf pictureselected Is PictureBox80 Then
                idselected = extracard21
                cardselected = Me.ToolTip1.GetToolTip(PictureBox80)

                Me.PictureBox80.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox80, Nothing)
                Me.ToolStripMenuItem156.Text = "1"
                extracard21 = Nothing
                extra21 = Nothing

            ElseIf pictureselected Is PictureBox79 Then
                idselected = extracard22
                cardselected = Me.ToolTip1.GetToolTip(PictureBox79)

                Me.PictureBox79.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox79, Nothing)
                Me.ToolStripMenuItem157.Text = "2"
                extracard22 = Nothing
                extra22 = Nothing

            ElseIf pictureselected Is PictureBox78 Then
                idselected = extracard23
                cardselected = Me.ToolTip1.GetToolTip(PictureBox78)

                Me.PictureBox78.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox78, Nothing)
                Me.ToolStripMenuItem158.Text = "3"
                extracard23 = Nothing
                extra23 = Nothing

            ElseIf pictureselected Is PictureBox77 Then
                idselected = extracard24
                cardselected = Me.ToolTip1.GetToolTip(PictureBox77)

                Me.PictureBox77.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox77, Nothing)
                Me.ToolStripMenuItem159.Text = "4"
                extracard24 = Nothing
                extra24 = Nothing

            ElseIf pictureselected Is PictureBox76 Then
                idselected = extracard25
                cardselected = Me.ToolTip1.GetToolTip(PictureBox76)

                Me.PictureBox76.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox76, Nothing)
                Me.ToolStripMenuItem160.Text = "5"
                extracard25 = Nothing
                extra25 = Nothing

            ElseIf pictureselected Is PictureBox117 Then
                idselected = extracard26
                cardselected = Me.ToolTip1.GetToolTip(PictureBox117)

                Me.PictureBox117.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox117, Nothing)
                Me.ToolStripMenuItem162.Text = "6"
                extracard26 = Nothing
                extra26 = Nothing

            ElseIf pictureselected Is PictureBox118 Then
                idselected = extracard27
                cardselected = Me.ToolTip1.GetToolTip(PictureBox118)

                Me.PictureBox118.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox118, Nothing)
                Me.ToolStripMenuItem163.Text = "7"
                extracard27 = Nothing
                extra27 = Nothing


            ElseIf pictureselected Is PictureBox119 Then
                idselected = extracard28
                cardselected = Me.ToolTip1.GetToolTip(PictureBox119)

                Me.PictureBox119.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox119, Nothing)
                Me.ToolStripMenuItem164.Text = "8"
                extracard28 = Nothing
                extra28 = Nothing

            ElseIf pictureselected Is PictureBox120 Then
                idselected = extracard29
                cardselected = Me.ToolTip1.GetToolTip(PictureBox120)

                Me.PictureBox120.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox120, Nothing)
                Me.ToolStripMenuItem165.Text = "9"
                extracard29 = Nothing
                extra29 = Nothing

            ElseIf pictureselected Is PictureBox121 Then
                idselected = extracard20
                cardselected = Me.ToolTip1.GetToolTip(PictureBox121)

                Me.PictureBox121.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox121, Nothing)
                Me.ToolStripMenuItem166.Text = "10"
                extracard20 = Nothing
                extra20 = Nothing

            End If


        End If
    End Sub

    Private Sub DeleteCardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteCardToolStripMenuItem.Click
        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox2.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox2, Nothing)
                monstercard11 = Nothing
                monster11 = Nothing
                Me.ToolStripMenuItem14.Text = "1"
                Me.ToolStripMenuItem48.Text = "1"

            ElseIf pictureselected Is PictureBox3 Then

                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox3.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox3, Nothing)
                monstercard12 = Nothing
                monster12 = Nothing
                Me.ToolStripMenuItem15.Text = "2"
                Me.ToolStripMenuItem49.Text = "2"

            ElseIf pictureselected Is PictureBox4 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox4.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox4, Nothing)
                monstercard13 = Nothing
                monster13 = Nothing
                Me.ToolStripMenuItem16.Text = "3"
                Me.ToolStripMenuItem50.Text = "3"

            ElseIf pictureselected Is PictureBox5 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox5.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox5, Nothing)
                monstercard14 = Nothing
                monster14 = Nothing
                Me.ToolStripMenuItem17.Text = "4"
                Me.ToolStripMenuItem51.Text = "4"

            ElseIf pictureselected Is PictureBox6 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox6.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox6, Nothing)
                monstercard15 = Nothing
                monster15 = Nothing
                Me.ToolStripMenuItem18.Text = "5"
                Me.ToolStripMenuItem52.Text = "5"

            ElseIf pictureselected Is PictureBox7 Then
                idselected = Nothing
                cardselected = Nothing


                Me.PictureBox7.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox7, Nothing)
                spellcard11 = Nothing
                szone11 = Nothing
                Me.ToolStripMenuItem24.Text = "1"
                Me.ToolStripMenuItem60.Text = "1"


            ElseIf pictureselected Is PictureBox8 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox8.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox8, Nothing)
                spellcard12 = Nothing
                szone12 = Nothing
                Me.ToolStripMenuItem25.Text = "2"
                Me.ToolStripMenuItem61.Text = "2"

            ElseIf pictureselected Is PictureBox9 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox9.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox9, Nothing)
                spellcard13 = Nothing
                szone13 = Nothing
                Me.ToolStripMenuItem26.Text = "3"
                Me.ToolStripMenuItem62.Text = "3"

            ElseIf pictureselected Is PictureBox10 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox10.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox10, Nothing)
                spellcard14 = Nothing
                szone14 = Nothing
                Me.ToolStripMenuItem27.Text = "4"
                Me.ToolStripMenuItem63.Text = "4"

            ElseIf pictureselected Is PictureBox11 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox11.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox11, Nothing)
                spellcard15 = Nothing
                szone15 = Nothing
                Me.ToolStripMenuItem28.Text = "5"
                Me.ToolStripMenuItem64.Text = "5"

            ElseIf pictureselected Is PictureBox12 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox12.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox12, Nothing)
                handcard11 = Nothing
                hand11 = Nothing
                Me.ToolStripMenuItem36.Text = "1"
                Me.ToolStripMenuItem74.Text = "1"

            ElseIf pictureselected Is PictureBox13 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox13.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox13, Nothing)
                handcard12 = Nothing
                hand12 = Nothing
                Me.ToolStripMenuItem37.Text = "2"
                Me.ToolStripMenuItem75.Text = "2"

            ElseIf pictureselected Is PictureBox14 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox14.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox14, Nothing)
                handcard13 = Nothing
                hand13 = Nothing
                Me.ToolStripMenuItem38.Text = "3"
                Me.ToolStripMenuItem76.Text = "3"

            ElseIf pictureselected Is PictureBox15 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox15.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox15, Nothing)
                handcard14 = Nothing
                hand14 = Nothing
                Me.ToolStripMenuItem39.Text = "4"
                Me.ToolStripMenuItem77.Text = "4"

            ElseIf pictureselected Is PictureBox16 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox16.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox16, Nothing)
                handcard15 = Nothing
                hand15 = Nothing
                Me.ToolStripMenuItem40.Text = "5"
                Me.ToolStripMenuItem78.Text = "5"

            ElseIf pictureselected Is PictureBox21 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox21.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox21, Nothing)
                spellcard16 = Nothing
                szone16 = Nothing
                Me.FieldToolStripMenuItem.Text = "Field"
                Me.ToolStripMenuItem65.Text = "Field"

            ElseIf pictureselected Is PictureBox22 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox22.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox22, Nothing)
                monstercard21 = Nothing
                monster21 = Nothing
                Me.ToolStripMenuItem19.Text = "1"
                Me.ToolStripMenuItem54.Text = "1"

            ElseIf pictureselected Is PictureBox23 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox23.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox23, Nothing)
                monstercard22 = Nothing
                monster22 = Nothing
                Me.ToolStripMenuItem20.Text = "2"
                Me.ToolStripMenuItem55.Text = "2"

            ElseIf pictureselected Is PictureBox24 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox24.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox24, Nothing)
                monstercard23 = Nothing
                monster23 = Nothing
                Me.ToolStripMenuItem21.Text = "3"
                Me.ToolStripMenuItem56.Text = "3"

            ElseIf pictureselected Is PictureBox25 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox25.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox25, Nothing)
                monstercard24 = Nothing
                monster24 = Nothing
                Me.ToolStripMenuItem22.Text = "4"
                Me.ToolStripMenuItem57.Text = "4"

            ElseIf pictureselected Is PictureBox26 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox26.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox26, Nothing)
                monstercard25 = Nothing
                monster25 = Nothing
                Me.ToolStripMenuItem23.Text = "5"
                Me.ToolStripMenuItem58.Text = "5"

            ElseIf pictureselected Is PictureBox27 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox27.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox27, Nothing)
                spellcard21 = Nothing
                szone21 = Nothing
                Me.ToolStripMenuItem30.Text = "1"
                Me.ToolStripMenuItem67.Text = "1"

            ElseIf pictureselected Is PictureBox28 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox28.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox28, Nothing)
                spellcard22 = Nothing
                szone22 = Nothing
                Me.ToolStripMenuItem31.Text = "2"
                Me.ToolStripMenuItem68.Text = "2"

            ElseIf pictureselected Is PictureBox29 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox29.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox29, Nothing)
                spellcard23 = Nothing
                szone23 = Nothing
                Me.ToolStripMenuItem32.Text = "3"
                Me.ToolStripMenuItem69.Text = "3"

            ElseIf pictureselected Is PictureBox30 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox30.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox30, Nothing)
                spellcard24 = Nothing
                szone24 = Nothing
                Me.ToolStripMenuItem33.Text = "4"
                Me.ToolStripMenuItem70.Text = "4"

            ElseIf pictureselected Is PictureBox31 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox31.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox31, Nothing)
                spellcard25 = Nothing
                szone25 = Nothing
                Me.ToolStripMenuItem34.Text = "5"
                Me.ToolStripMenuItem71.Text = "5"

            ElseIf pictureselected Is PictureBox32 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox32.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox32, Nothing)
                handcard21 = Nothing
                hand21 = Nothing
                Me.ToolStripMenuItem42.Text = "1"
                Me.ToolStripMenuItem80.Text = "1"

            ElseIf pictureselected Is PictureBox33 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox33.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox33, Nothing)
                handcard22 = Nothing
                hand22 = Nothing
                Me.ToolStripMenuItem43.Text = "2"
                Me.ToolStripMenuItem81.Text = "2"

            ElseIf pictureselected Is PictureBox34 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox34.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox34, Nothing)
                handcard23 = Nothing
                hand23 = Nothing
                Me.ToolStripMenuItem44.Text = "3"
                Me.ToolStripMenuItem82.Text = "3"

            ElseIf pictureselected Is PictureBox35 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox35.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox35, Nothing)
                handcard24 = Nothing
                hand24 = Nothing
                Me.ToolStripMenuItem45.Text = "4"
                Me.ToolStripMenuItem83.Text = "4"

            ElseIf pictureselected Is PictureBox36 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox36.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox36, Nothing)
                handcard25 = Nothing
                hand25 = Nothing
                Me.ToolStripMenuItem46.Text = "5"
                Me.ToolStripMenuItem84.Text = "5"

            ElseIf pictureselected Is PictureBox40 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox40.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox40, Nothing)
                spellcard26 = Nothing
                szone26 = Nothing
                Me.FieldToolStripMenuItem1.Text = "Field"
                Me.ToolStripMenuItem72.Text = "Field"

            ElseIf pictureselected Is PictureBox45 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox45.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox45, Nothing)
                deckcard11 = Nothing
                deck11 = Nothing
                Me.ToolStripMenuItem4.Text = "1"

            ElseIf pictureselected Is PictureBox44 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox44.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox44, Nothing)
                deckcard12 = Nothing
                deck12 = Nothing
                Me.ToolStripMenuItem5.Text = "2"

            ElseIf pictureselected Is PictureBox43 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox43.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox43, Nothing)
                deckcard13 = Nothing
                deck13 = Nothing
                Me.ToolStripMenuItem6.Text = "3"

            ElseIf pictureselected Is PictureBox42 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox42.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox42, Nothing)
                deckcard14 = Nothing
                deck14 = Nothing
                Me.ToolStripMenuItem7.Text = "4"

            ElseIf pictureselected Is PictureBox41 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox41.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox41, Nothing)
                deckcard15 = Nothing
                deck15 = Nothing
                Me.ToolStripMenuItem8.Text = "5"

            ElseIf pictureselected Is PictureBox82 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox82.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox82, Nothing)
                deckcard16 = Nothing
                deck16 = Nothing
                Me.ToolStripMenuItem9.Text = "6"

            ElseIf pictureselected Is PictureBox83 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox83.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox83, Nothing)
                deckcard17 = Nothing
                deck17 = Nothing
                Me.ToolStripMenuItem10.Text = "7"

            ElseIf pictureselected Is PictureBox84 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox84.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox84, Nothing)
                deckcard18 = Nothing
                deck18 = Nothing
                Me.ToolStripMenuItem11.Text = "8"

            ElseIf pictureselected Is PictureBox85 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox41.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox85, Nothing)
                deckcard19 = Nothing
                deck19 = Nothing
                Me.ToolStripMenuItem12.Text = "9"

            ElseIf pictureselected Is PictureBox86 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox86.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox86, Nothing)
                deckcard10 = Nothing
                deck10 = Nothing
                Me.ToolStripMenuItem13.Text = "10"

            ElseIf pictureselected Is PictureBox50 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox50.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox50, Nothing)
                deckcard21 = Nothing
                deck21 = Nothing
                Me.ToolStripMenuItem86.Text = "1"

            ElseIf pictureselected Is PictureBox49 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox49.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox49, Nothing)
                deckcard22 = Nothing
                deck22 = Nothing
                Me.ToolStripMenuItem87.Text = "2"

            ElseIf pictureselected Is PictureBox48 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox48.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox48, Nothing)
                deckcard23 = Nothing
                deck23 = Nothing
                Me.ToolStripMenuItem88.Text = "3"

            ElseIf pictureselected Is PictureBox47 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox47.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox47, Nothing)
                deckcard24 = Nothing
                deck24 = Nothing
                Me.ToolStripMenuItem89.Text = "4"

            ElseIf pictureselected Is PictureBox46 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox46.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox46, Nothing)
                deckcard25 = Nothing
                deck25 = Nothing
                Me.ToolStripMenuItem90.Text = "5"

            ElseIf pictureselected Is PictureBox102 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox102.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox102, Nothing)
                deckcard26 = Nothing
                deck26 = Nothing
                Me.ToolStripMenuItem91.Text = "6"

            ElseIf pictureselected Is PictureBox103 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox103.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox103, Nothing)
                deckcard27 = Nothing
                deck27 = Nothing
                Me.ToolStripMenuItem92.Text = "7"

            ElseIf pictureselected Is PictureBox104 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox104.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox104, Nothing)
                deckcard28 = Nothing
                deck28 = Nothing
                Me.ToolStripMenuItem93.Text = "8"

            ElseIf pictureselected Is PictureBox105 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox105.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox105, Nothing)
                deckcard29 = Nothing
                deck29 = Nothing
                Me.ToolStripMenuItem94.Text = "9"

            ElseIf pictureselected Is PictureBox106 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox106.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox106, Nothing)
                deckcard20 = Nothing
                deck20 = Nothing
                Me.ToolStripMenuItem95.Text = "10"

            ElseIf pictureselected Is PictureBox55 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox55.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox55, Nothing)
                gravecard11 = Nothing
                grave11 = Nothing
                Me.ToolStripMenuItem96.Text = "1"

            ElseIf pictureselected Is PictureBox54 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox54.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox54, Nothing)
                gravecard12 = Nothing
                grave12 = Nothing
                Me.ToolStripMenuItem97.Text = "2"

            ElseIf pictureselected Is PictureBox53 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox53.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox53, Nothing)
                gravecard13 = Nothing
                grave13 = Nothing
                Me.ToolStripMenuItem98.Text = "3"

            ElseIf pictureselected Is PictureBox52 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox52.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox52, Nothing)
                gravecard14 = Nothing
                grave14 = Nothing
                Me.ToolStripMenuItem99.Text = "4"

            ElseIf pictureselected Is PictureBox51 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox51.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox51, Nothing)
                gravecard15 = Nothing
                grave15 = Nothing
                Me.ToolStripMenuItem100.Text = "5"

            ElseIf pictureselected Is PictureBox87 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox87.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox87, Nothing)
                gravecard16 = Nothing
                grave16 = Nothing
                Me.ToolStripMenuItem101.Text = "6"


            ElseIf pictureselected Is PictureBox88 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox88.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox88, Nothing)
                gravecard17 = Nothing
                grave17 = Nothing
                Me.ToolStripMenuItem102.Text = "7"

            ElseIf pictureselected Is PictureBox89 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox89.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox89, Nothing)
                gravecard18 = Nothing
                grave18 = Nothing
                Me.ToolStripMenuItem103.Text = "8"

            ElseIf pictureselected Is PictureBox87 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox90.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox90, Nothing)
                gravecard19 = Nothing
                grave19 = Nothing
                Me.ToolStripMenuItem104.Text = "9"

            ElseIf pictureselected Is PictureBox91 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox91.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox91, Nothing)
                gravecard10 = Nothing
                grave10 = Nothing
                Me.ToolStripMenuItem105.Text = "10"

            ElseIf pictureselected Is PictureBox60 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox60.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox60, Nothing)
                gravecard21 = Nothing
                grave21 = Nothing
                Me.ToolStripMenuItem106.Text = "1"

            ElseIf pictureselected Is PictureBox59 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox59.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox59, Nothing)
                gravecard22 = Nothing
                grave22 = Nothing
                Me.ToolStripMenuItem107.Text = "2"

            ElseIf pictureselected Is PictureBox58 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox58.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox58, Nothing)
                gravecard23 = Nothing
                grave23 = Nothing
                Me.ToolStripMenuItem108.Text = "3"

            ElseIf pictureselected Is PictureBox57 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox57.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox57, Nothing)
                gravecard24 = Nothing
                grave24 = Nothing
                Me.ToolStripMenuItem109.Text = "4"

            ElseIf pictureselected Is PictureBox56 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox56.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox56, Nothing)
                gravecard25 = Nothing
                grave25 = Nothing
                Me.ToolStripMenuItem110.Text = "5"

            ElseIf pictureselected Is PictureBox107 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox107.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox107, Nothing)
                gravecard26 = Nothing
                grave26 = Nothing
                Me.ToolStripMenuItem111.Text = "6"

            ElseIf pictureselected Is PictureBox108 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox108.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox108, Nothing)
                gravecard27 = Nothing
                grave27 = Nothing
                Me.ToolStripMenuItem112.Text = "7"

            ElseIf pictureselected Is PictureBox109 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox109.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox109, Nothing)
                gravecard28 = Nothing
                grave28 = Nothing
                Me.ToolStripMenuItem113.Text = "8"

            ElseIf pictureselected Is PictureBox110 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox110.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox110, Nothing)
                gravecard29 = Nothing
                grave29 = Nothing
                Me.ToolStripMenuItem114.Text = "9"

            ElseIf pictureselected Is PictureBox111 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox111.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox111, Nothing)
                gravecard20 = Nothing
                grave20 = Nothing
                Me.ToolStripMenuItem115.Text = "10"

            ElseIf pictureselected Is PictureBox65 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox65.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox65, Nothing)
                removedcard11 = Nothing
                removed11 = Nothing
                Me.ToolStripMenuItem117.Text = "1"

            ElseIf pictureselected Is PictureBox64 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox64.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox64, Nothing)
                removedcard12 = Nothing
                removed12 = Nothing
                Me.ToolStripMenuItem118.Text = "2"

            ElseIf pictureselected Is PictureBox63 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox63.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox63, Nothing)
                removedcard13 = Nothing
                removed13 = Nothing
                Me.ToolStripMenuItem119.Text = "3"

            ElseIf pictureselected Is PictureBox62 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox62.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox62, Nothing)
                removedcard14 = Nothing
                removed14 = Nothing
                Me.ToolStripMenuItem120.Text = "4"

            ElseIf pictureselected Is PictureBox61 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox61.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox61, Nothing)
                removedcard15 = Nothing
                removed15 = Nothing
                Me.ToolStripMenuItem121.Text = "5"

            ElseIf pictureselected Is PictureBox92 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox92.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox92, Nothing)
                removedcard16 = Nothing
                removed16 = Nothing
                Me.ToolStripMenuItem123.Text = "6"

            ElseIf pictureselected Is PictureBox93 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox93.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox93, Nothing)
                removedcard17 = Nothing
                removed17 = Nothing
                Me.ToolStripMenuItem124.Text = "7"

            ElseIf pictureselected Is PictureBox94 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox94.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox94, Nothing)
                removedcard18 = Nothing
                removed18 = Nothing
                Me.ToolStripMenuItem125.Text = "8"

            ElseIf pictureselected Is PictureBox95 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox95.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox95, Nothing)
                removedcard19 = Nothing
                removed19 = Nothing
                Me.ToolStripMenuItem126.Text = "9"

            ElseIf pictureselected Is PictureBox96 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox96.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox96, Nothing)
                removedcard10 = Nothing
                removed10 = Nothing
                Me.ToolStripMenuItem127.Text = "10"

            ElseIf pictureselected Is PictureBox70 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox70.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox70, Nothing)
                removedcard21 = Nothing
                removed21 = Nothing
                Me.ToolStripMenuItem130.Text = "1"


            ElseIf pictureselected Is PictureBox69 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox69.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox69, Nothing)
                removedcard22 = Nothing
                removed22 = Nothing
                Me.ToolStripMenuItem131.Text = "2"

            ElseIf pictureselected Is PictureBox68 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox68.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox68, Nothing)
                removedcard23 = Nothing
                removed23 = Nothing
                Me.ToolStripMenuItem132.Text = "3"

            ElseIf pictureselected Is PictureBox67 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox67.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox67, Nothing)
                removedcard24 = Nothing
                removed24 = Nothing
                Me.ToolStripMenuItem133.Text = "4"

            ElseIf pictureselected Is PictureBox66 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox66.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox66, Nothing)
                removedcard25 = Nothing
                removed25 = Nothing
                Me.ToolStripMenuItem134.Text = "5"


            ElseIf pictureselected Is PictureBox112 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox112.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox112, Nothing)
                removedcard26 = Nothing
                removed26 = Nothing
                Me.ToolStripMenuItem136.Text = "6"

            ElseIf pictureselected Is PictureBox113 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox113.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox113, Nothing)
                removedcard27 = Nothing
                removed27 = Nothing
                Me.ToolStripMenuItem137.Text = "7"

            ElseIf pictureselected Is PictureBox114 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox114.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox114, Nothing)
                removedcard28 = Nothing
                removed28 = Nothing
                Me.ToolStripMenuItem138.Text = "8"

            ElseIf pictureselected Is PictureBox115 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox115.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox115, Nothing)
                removedcard29 = Nothing
                removed29 = Nothing
                Me.ToolStripMenuItem139.Text = "9"

            ElseIf pictureselected Is PictureBox116 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox116.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox116, Nothing)
                removedcard20 = Nothing
                removed20 = Nothing
                Me.ToolStripMenuItem140.Text = "10"

            ElseIf pictureselected Is PictureBox75 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox75.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox75, Nothing)
                extracard11 = Nothing
                extra11 = Nothing
                Me.ToolStripMenuItem143.Text = "1"

            ElseIf pictureselected Is PictureBox74 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox74.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox74, Nothing)
                extracard12 = Nothing
                extra12 = Nothing
                Me.ToolStripMenuItem144.Text = "2"

            ElseIf pictureselected Is PictureBox73 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox73.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox73, Nothing)
                extracard13 = Nothing
                extra13 = Nothing
                Me.ToolStripMenuItem145.Text = "3"

            ElseIf pictureselected Is PictureBox72 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox72.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox72, Nothing)
                extracard14 = Nothing
                extra14 = Nothing
                Me.ToolStripMenuItem146.Text = "4"

            ElseIf pictureselected Is PictureBox71 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox71.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox71, Nothing)
                extracard15 = Nothing
                extra15 = Nothing
                Me.ToolStripMenuItem147.Text = "5"

            ElseIf pictureselected Is PictureBox97 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox97.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox97, Nothing)
                extracard16 = Nothing
                extra16 = Nothing
                Me.ToolStripMenuItem149.Text = "6"

            ElseIf pictureselected Is PictureBox98 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox98.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox98, Nothing)
                extracard17 = Nothing
                extra17 = Nothing
                Me.ToolStripMenuItem150.Text = "7"

            ElseIf pictureselected Is PictureBox99 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox99.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox99, Nothing)
                extracard18 = Nothing
                extra18 = Nothing
                Me.ToolStripMenuItem151.Text = "8"

            ElseIf pictureselected Is PictureBox100 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox100.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox100, Nothing)
                extracard19 = Nothing
                extra19 = Nothing
                Me.ToolStripMenuItem152.Text = "9"

            ElseIf pictureselected Is PictureBox101 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox101.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox101, Nothing)
                extracard10 = Nothing
                extra10 = Nothing
                Me.ToolStripMenuItem153.Text = "10"

            ElseIf pictureselected Is PictureBox80 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox80.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox80, Nothing)
                extracard21 = Nothing
                extra21 = Nothing
                Me.ToolStripMenuItem156.Text = "1"

            ElseIf pictureselected Is PictureBox79 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox79.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox79, Nothing)
                extracard22 = Nothing
                extra22 = Nothing
                Me.ToolStripMenuItem157.Text = "2"

            ElseIf pictureselected Is PictureBox78 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox78.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox78, Nothing)
                extracard23 = Nothing
                extra23 = Nothing
                Me.ToolStripMenuItem158.Text = "3"

            ElseIf pictureselected Is PictureBox77 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox77.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox77, Nothing)
                extracard24 = Nothing
                extra24 = Nothing
                Me.ToolStripMenuItem159.Text = "4"

            ElseIf pictureselected Is PictureBox76 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox76.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox76, Nothing)
                extracard25 = Nothing
                extra25 = Nothing
                Me.ToolStripMenuItem160.Text = "5"

            ElseIf pictureselected Is PictureBox117 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox117.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox117, Nothing)
                extracard26 = Nothing
                extra26 = Nothing
                Me.ToolStripMenuItem162.Text = "6"

            ElseIf pictureselected Is PictureBox118 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox118.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox118, Nothing)
                extracard27 = Nothing
                extra27 = Nothing
                Me.ToolStripMenuItem163.Text = "7"

            ElseIf pictureselected Is PictureBox119 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox119.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox119, Nothing)
                extracard28 = Nothing
                extra28 = Nothing
                Me.ToolStripMenuItem164.Text = "8"

            ElseIf pictureselected Is PictureBox120 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox120.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox120, Nothing)
                extracard29 = Nothing
                extra29 = Nothing
                Me.ToolStripMenuItem165.Text = "9"

            ElseIf pictureselected Is PictureBox121 Then
                idselected = Nothing
                cardselected = Nothing

                Me.PictureBox121.Image = Nothing
                Me.ToolTip1.SetToolTip(PictureBox121, Nothing)
                extracard20 = Nothing
                extra20 = Nothing
                Me.ToolStripMenuItem166.Text = "10"


            End If


        End If
    End Sub

    Private Sub SearchCardToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SearchCardToolStripMenuItem.Click
        If Not pictureselected Is Nothing Then

            If pictureselected Is PictureBox2 Then
                idselected = monstercard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox3 Then

                idselected = monstercard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox4 Then
                idselected = monstercard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox5 Then
                idselected = monstercard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox6 Then
                idselected = monstercard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox7 Then
                idselected = spellcard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox8 Then
                idselected = spellcard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox9 Then
                idselected = spellcard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox10 Then
                idselected = spellcard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox11 Then
                idselected = spellcard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox21 Then
                idselected = spellcard16
                SearchbyCard()

            ElseIf pictureselected Is PictureBox12 Then
                idselected = handcard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox13 Then
                idselected = handcard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox14 Then
                idselected = handcard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox15 Then
                idselected = handcard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox16 Then
                idselected = handcard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox22 Then
                idselected = monstercard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox23 Then
                idselected = monstercard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox24 Then
                idselected = monstercard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox25 Then
                idselected = monstercard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox26 Then
                idselected = monstercard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox27 Then
                idselected = spellcard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox28 Then
                idselected = spellcard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox29 Then
                idselected = spellcard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox30 Then
                idselected = spellcard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox31 Then
                idselected = spellcard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox40 Then
                idselected = spellcard26
                SearchbyCard()

            ElseIf pictureselected Is PictureBox32 Then
                idselected = handcard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox33 Then
                idselected = handcard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox34 Then
                idselected = handcard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox35 Then
                idselected = handcard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox36 Then
                idselected = handcard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox45 Then
                idselected = deckcard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox44 Then
                idselected = deckcard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox43 Then
                idselected = deckcard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox42 Then
                idselected = deckcard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox41 Then
                idselected = deckcard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox82 Then
                idselected = deckcard16
                SearchbyCard()

            ElseIf pictureselected Is PictureBox83 Then
                idselected = deckcard17
                SearchbyCard()

            ElseIf pictureselected Is PictureBox84 Then
                idselected = deckcard18
                SearchbyCard()

            ElseIf pictureselected Is PictureBox85 Then
                idselected = deckcard19
                SearchbyCard()

            ElseIf pictureselected Is PictureBox86 Then
                idselected = deckcard10
                SearchbyCard()

            ElseIf pictureselected Is PictureBox50 Then
                idselected = deckcard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox49 Then
                idselected = deckcard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox48 Then
                idselected = deckcard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox47 Then
                idselected = deckcard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox46 Then
                idselected = deckcard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox102 Then
                idselected = deckcard26
                SearchbyCard()

            ElseIf pictureselected Is PictureBox103 Then
                idselected = deckcard27
                SearchbyCard()

            ElseIf pictureselected Is PictureBox104 Then
                idselected = deckcard28
                SearchbyCard()

            ElseIf pictureselected Is PictureBox105 Then
                idselected = deckcard29
                SearchbyCard()

            ElseIf pictureselected Is PictureBox106 Then
                idselected = deckcard20
                SearchbyCard()

            ElseIf pictureselected Is PictureBox55 Then
                idselected = gravecard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox54 Then
                idselected = gravecard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox53 Then
                idselected = gravecard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox52 Then
                idselected = gravecard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox51 Then
                idselected = gravecard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox87 Then
                idselected = gravecard16
                SearchbyCard()

            ElseIf pictureselected Is PictureBox88 Then
                idselected = gravecard17
                SearchbyCard()

            ElseIf pictureselected Is PictureBox89 Then
                idselected = gravecard18
                SearchbyCard()

            ElseIf pictureselected Is PictureBox90 Then
                idselected = gravecard19
                SearchbyCard()

            ElseIf pictureselected Is PictureBox91 Then
                idselected = gravecard10
                SearchbyCard()

            ElseIf pictureselected Is PictureBox60 Then
                idselected = gravecard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox59 Then
                idselected = gravecard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox58 Then
                idselected = gravecard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox57 Then
                idselected = gravecard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox56 Then
                idselected = gravecard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox107 Then
                idselected = gravecard26
                SearchbyCard()

            ElseIf pictureselected Is PictureBox108 Then
                idselected = gravecard27
                SearchbyCard()

            ElseIf pictureselected Is PictureBox109 Then
                idselected = gravecard28
                SearchbyCard()

            ElseIf pictureselected Is PictureBox110 Then
                idselected = gravecard29
                SearchbyCard()

            ElseIf pictureselected Is PictureBox111 Then
                idselected = gravecard20
                SearchbyCard()

            ElseIf pictureselected Is PictureBox55 Then
                idselected = removedcard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox54 Then
                idselected = removedcard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox53 Then
                idselected = removedcard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox52 Then
                idselected = removedcard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox51 Then
                idselected = removedcard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox92 Then
                idselected = removedcard16
                SearchbyCard()

            ElseIf pictureselected Is PictureBox93 Then
                idselected = removedcard17
                SearchbyCard()

            ElseIf pictureselected Is PictureBox94 Then
                idselected = removedcard18
                SearchbyCard()

            ElseIf pictureselected Is PictureBox95 Then
                idselected = removedcard19
                SearchbyCard()

            ElseIf pictureselected Is PictureBox96 Then
                idselected = removedcard10
                SearchbyCard()

            ElseIf pictureselected Is PictureBox70 Then
                idselected = removedcard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox69 Then
                idselected = removedcard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox68 Then
                idselected = removedcard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox67 Then
                idselected = removedcard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox66 Then
                idselected = removedcard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox112 Then
                idselected = removedcard26
                SearchbyCard()

            ElseIf pictureselected Is PictureBox113 Then
                idselected = removedcard27
                SearchbyCard()

            ElseIf pictureselected Is PictureBox114 Then
                idselected = removedcard28
                SearchbyCard()

            ElseIf pictureselected Is PictureBox115 Then
                idselected = removedcard29
                SearchbyCard()

            ElseIf pictureselected Is PictureBox116 Then
                idselected = removedcard20
                SearchbyCard()

            ElseIf pictureselected Is PictureBox75 Then
                idselected = extracard11
                SearchbyCard()

            ElseIf pictureselected Is PictureBox74 Then
                idselected = extracard12
                SearchbyCard()

            ElseIf pictureselected Is PictureBox73 Then
                idselected = extracard13
                SearchbyCard()

            ElseIf pictureselected Is PictureBox72 Then
                idselected = extracard14
                SearchbyCard()

            ElseIf pictureselected Is PictureBox71 Then
                idselected = extracard15
                SearchbyCard()

            ElseIf pictureselected Is PictureBox97 Then
                idselected = extracard16
                SearchbyCard()

            ElseIf pictureselected Is PictureBox98 Then
                idselected = extracard17
                SearchbyCard()

            ElseIf pictureselected Is PictureBox99 Then
                idselected = extracard18
                SearchbyCard()

            ElseIf pictureselected Is PictureBox100 Then
                idselected = extracard19
                SearchbyCard()

            ElseIf pictureselected Is PictureBox101 Then
                idselected = extracard10
                SearchbyCard()


            ElseIf pictureselected Is PictureBox80 Then
                idselected = extracard21
                SearchbyCard()

            ElseIf pictureselected Is PictureBox79 Then
                idselected = extracard22
                SearchbyCard()

            ElseIf pictureselected Is PictureBox78 Then
                idselected = extracard23
                SearchbyCard()

            ElseIf pictureselected Is PictureBox77 Then
                idselected = extracard24
                SearchbyCard()

            ElseIf pictureselected Is PictureBox76 Then
                idselected = extracard25
                SearchbyCard()

            ElseIf pictureselected Is PictureBox117 Then
                idselected = extracard26
                SearchbyCard()

            ElseIf pictureselected Is PictureBox118 Then
                idselected = extracard27
                SearchbyCard()

            ElseIf pictureselected Is PictureBox119 Then
                idselected = extracard28
                SearchbyCard()

            ElseIf pictureselected Is PictureBox120 Then
                idselected = extracard29
                SearchbyCard()

            ElseIf pictureselected Is PictureBox121 Then
                idselected = extracard20
                SearchbyCard()

            End If
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim Response = MsgBox(Prompt:="Do you want to create a new puzzle?", Buttons:=vbYesNo)

        ' If statement to check if the yes button was selected.
        If Response = vbYes Then

            ClearScript()
            MsgBox("New puzzle created")
        Else

            MsgBox("Canceled")
        End If

    End Sub




    Private Sub ToolStripMenuItem14_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem14.Click
        If Me.ToolStripMenuItem14.CheckState = CheckState.Checked Then
            idselected = monstercard11
            cardselected = Me.ToolTip1.GetToolTip(PictureBox2)

            Me.ToolStripMenuItem15.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem16.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem17.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem18.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem14.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem15_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem15.Click
        If Me.ToolStripMenuItem15.CheckState = CheckState.Checked Then
            idselected = monstercard12
            cardselected = Me.ToolTip1.GetToolTip(PictureBox3)

            Me.ToolStripMenuItem14.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem16.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem17.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem18.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem15.CheckState = CheckState.Checked

        End If
    End Sub


    Private Sub ToolStripMenuItem16_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem16.Click
        If Me.ToolStripMenuItem16.CheckState = CheckState.Checked Then
            idselected = monstercard13
            cardselected = Me.ToolTip1.GetToolTip(PictureBox4)

            Me.ToolStripMenuItem15.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem14.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem17.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem18.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem16.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem17_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem17.Click
        If Me.ToolStripMenuItem17.CheckState = CheckState.Checked Then
            idselected = monstercard14
            cardselected = Me.ToolTip1.GetToolTip(PictureBox5)

            Me.ToolStripMenuItem15.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem16.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem14.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem18.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem17.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem18_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem18.Click
        If Me.ToolStripMenuItem18.CheckState = CheckState.Checked Then
            idselected = monstercard15
            cardselected = Me.ToolTip1.GetToolTip(PictureBox6)

            Me.ToolStripMenuItem15.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem16.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem17.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem14.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem18.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem19_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem19.Click
        If Me.ToolStripMenuItem19.CheckState = CheckState.Checked Then
            idselected = monstercard21
            cardselected = Me.ToolTip1.GetToolTip(PictureBox22)

            Me.ToolStripMenuItem20.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem21.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem22.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem23.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem19.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem20_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem20.Click
        If Me.ToolStripMenuItem20.CheckState = CheckState.Checked Then
            idselected = monstercard22
            cardselected = Me.ToolTip1.GetToolTip(PictureBox23)

            Me.ToolStripMenuItem19.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem21.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem22.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem23.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem20.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem21_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem21.Click
        If Me.ToolStripMenuItem21.CheckState = CheckState.Checked Then
            idselected = monstercard23
            cardselected = Me.ToolTip1.GetToolTip(PictureBox24)

            Me.ToolStripMenuItem20.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem19.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem22.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem23.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem21.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem22_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem22.Click
        If Me.ToolStripMenuItem22.CheckState = CheckState.Checked Then
            idselected = monstercard24
            cardselected = Me.ToolTip1.GetToolTip(PictureBox25)

            Me.ToolStripMenuItem20.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem21.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem19.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem23.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem22.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem23_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem23.Click
        If Me.ToolStripMenuItem23.CheckState = CheckState.Checked Then
            idselected = monstercard25
            cardselected = Me.ToolTip1.GetToolTip(PictureBox26)

            Me.ToolStripMenuItem20.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem21.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem22.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem19.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem23.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem24_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem24.Click
        If Me.ToolStripMenuItem24.CheckState = CheckState.Checked Then
            idselected = spellcard11
            cardselected = Me.ToolTip1.GetToolTip(PictureBox7)

            Me.ToolStripMenuItem25.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem26.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem27.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem28.CheckState = CheckState.Unchecked
            Me.FieldToolStripMenuItem.CheckState = CheckState.Unchecked
        Else

            Me.ToolStripMenuItem24.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem25_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem25.Click
        If Me.ToolStripMenuItem25.CheckState = CheckState.Checked Then
            idselected = spellcard12
            cardselected = Me.ToolTip1.GetToolTip(PictureBox8)

            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem26.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem27.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem28.CheckState = CheckState.Unchecked
            Me.FieldToolStripMenuItem.CheckState = CheckState.Unchecked
        Else

            Me.ToolStripMenuItem25.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem26_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem26.Click
        If Me.ToolStripMenuItem26.CheckState = CheckState.Checked Then
            idselected = spellcard13
            cardselected = Me.ToolTip1.GetToolTip(PictureBox9)

            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem27.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem28.CheckState = CheckState.Unchecked
            Me.FieldToolStripMenuItem.CheckState = CheckState.Unchecked
        Else

            Me.ToolStripMenuItem26.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem27_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem27.Click
        If Me.ToolStripMenuItem27.CheckState = CheckState.Checked Then
            idselected = spellcard14
            cardselected = Me.ToolTip1.GetToolTip(PictureBox10)

            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem26.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem28.CheckState = CheckState.Unchecked
            Me.FieldToolStripMenuItem.CheckState = CheckState.Unchecked
        Else

            Me.ToolStripMenuItem27.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem28_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem28.Click
        If Me.ToolStripMenuItem28.CheckState = CheckState.Checked Then
            idselected = spellcard15
            cardselected = Me.ToolTip1.GetToolTip(PictureBox11)

            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem26.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem27.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.FieldToolStripMenuItem.CheckState = CheckState.Unchecked
        Else

            Me.ToolStripMenuItem28.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem30_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem30.Click
        If Me.ToolStripMenuItem30.CheckState = CheckState.Checked Then
            idselected = spellcard21
            cardselected = Me.ToolTip1.GetToolTip(PictureBox27)

            Me.ToolStripMenuItem31.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem32.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem33.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem34.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem30.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem31_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem31.Click
        If Me.ToolStripMenuItem31.CheckState = CheckState.Checked Then
            idselected = spellcard22
            cardselected = Me.ToolTip1.GetToolTip(PictureBox28)

            Me.ToolStripMenuItem30.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem32.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem33.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem34.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem31.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem32_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem32.Click
        If Me.ToolStripMenuItem32.CheckState = CheckState.Checked Then
            idselected = spellcard23
            cardselected = Me.ToolTip1.GetToolTip(PictureBox29)

            Me.ToolStripMenuItem31.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem30.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem33.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem34.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem32.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem33_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem33.Click
        If Me.ToolStripMenuItem33.CheckState = CheckState.Checked Then
            idselected = spellcard24
            cardselected = Me.ToolTip1.GetToolTip(PictureBox30)

            Me.ToolStripMenuItem31.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem32.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem30.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem34.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem33.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem34_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem34.Click
        If Me.ToolStripMenuItem34.CheckState = CheckState.Checked Then
            idselected = spellcard25
            cardselected = Me.ToolTip1.GetToolTip(PictureBox31)

            Me.ToolStripMenuItem31.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem32.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem33.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem30.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem34.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub FieldToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FieldToolStripMenuItem.Click
        If Me.FieldToolStripMenuItem.CheckState = CheckState.Checked Then
            idselected = spellcard16
            cardselected = Me.ToolTip1.GetToolTip(PictureBox21)

            Me.ToolStripMenuItem25.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem24.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem26.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem27.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem28.CheckState = CheckState.Unchecked
        Else

            Me.FieldToolStripMenuItem.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub FieldToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles FieldToolStripMenuItem1.Click
        If Me.FieldToolStripMenuItem1.CheckState = CheckState.Checked Then
            idselected = spellcard26
            cardselected = Me.ToolTip1.GetToolTip(PictureBox40)

            Me.ToolStripMenuItem31.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem32.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem33.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem30.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem34.CheckState = CheckState.Unchecked
        Else

            Me.FieldToolStripMenuItem1.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem36_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem36.Click
        If Me.ToolStripMenuItem36.CheckState = CheckState.Checked Then
            idselected = handcard11
            cardselected = Me.ToolTip1.GetToolTip(PictureBox12)

            Me.ToolStripMenuItem37.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem38.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem39.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem40.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem36.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem37_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem37.Click
        If Me.ToolStripMenuItem37.CheckState = CheckState.Checked Then
            idselected = handcard12
            cardselected = Me.ToolTip1.GetToolTip(PictureBox13)

            Me.ToolStripMenuItem36.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem38.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem39.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem40.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem37.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem38_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem38.Click
        If Me.ToolStripMenuItem38.CheckState = CheckState.Checked Then
            idselected = handcard13
            cardselected = Me.ToolTip1.GetToolTip(PictureBox14)

            Me.ToolStripMenuItem37.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem36.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem39.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem40.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem38.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem39_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem39.Click
        If Me.ToolStripMenuItem39.CheckState = CheckState.Checked Then
            idselected = handcard14
            cardselected = Me.ToolTip1.GetToolTip(PictureBox15)

            Me.ToolStripMenuItem37.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem38.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem36.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem40.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem39.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem40_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem40.Click
        If Me.ToolStripMenuItem40.CheckState = CheckState.Checked Then
            idselected = handcard15
            cardselected = Me.ToolTip1.GetToolTip(PictureBox16)

            Me.ToolStripMenuItem37.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem38.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem39.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem36.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem40.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem42_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem42.Click
        If Me.ToolStripMenuItem42.CheckState = CheckState.Checked Then
            idselected = handcard21
            cardselected = Me.ToolTip1.GetToolTip(PictureBox32)

            Me.ToolStripMenuItem43.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem44.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem45.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem46.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem42.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem43_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem43.Click
        If Me.ToolStripMenuItem43.CheckState = CheckState.Checked Then
            idselected = handcard22
            cardselected = Me.ToolTip1.GetToolTip(PictureBox33)

            Me.ToolStripMenuItem42.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem44.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem45.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem46.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem43.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem44_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem44.Click
        If Me.ToolStripMenuItem44.CheckState = CheckState.Checked Then
            idselected = handcard23
            cardselected = Me.ToolTip1.GetToolTip(PictureBox34)

            Me.ToolStripMenuItem43.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem42.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem45.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem46.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem44.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem45_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem45.Click
        If Me.ToolStripMenuItem45.CheckState = CheckState.Checked Then
            idselected = handcard24
            cardselected = Me.ToolTip1.GetToolTip(PictureBox35)

            Me.ToolStripMenuItem43.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem44.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem42.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem46.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem45.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem46_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem46.Click
        If Me.ToolStripMenuItem46.CheckState = CheckState.Checked Then
            idselected = handcard25
            cardselected = Me.ToolTip1.GetToolTip(PictureBox36)

            Me.ToolStripMenuItem43.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem44.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem45.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem42.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem46.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem48_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem48.Click
        If Me.ToolStripMenuItem48.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox2.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox2, Nothing)
            monstercard11 = Nothing
            monster11 = Nothing

            Me.ToolStripMenuItem49.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem50.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem51.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem52.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem48.Text = "1"
            Me.ToolStripMenuItem14.Text = "1"

        Else

            Me.ToolStripMenuItem48.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem49_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem49.Click
        If Me.ToolStripMenuItem49.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox3.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox3, Nothing)
            monstercard12 = Nothing
            monster12 = Nothing
            Me.ToolStripMenuItem48.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem50.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem51.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem52.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem49.Text = "1"
            Me.ToolStripMenuItem15.Text = "1"
        Else

            Me.ToolStripMenuItem49.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem50_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem50.Click
        If Me.ToolStripMenuItem50.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox4.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox4, Nothing)
            monstercard13 = Nothing
            monster13 = Nothing
            Me.ToolStripMenuItem49.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem48.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem51.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem52.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem50.Text = "1"
            Me.ToolStripMenuItem16.Text = "1"
        Else

            Me.ToolStripMenuItem50.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem51_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem51.Click
        If Me.ToolStripMenuItem51.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox5.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox5, Nothing)
            monstercard14 = Nothing
            monster14 = Nothing
            Me.ToolStripMenuItem49.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem48.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem50.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem52.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem51.Text = "1"
            Me.ToolStripMenuItem17.Text = "1"
        Else

            Me.ToolStripMenuItem51.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem52_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem52.Click
        If Me.ToolStripMenuItem52.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox6.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox6, Nothing)
            monstercard15 = Nothing
            monster15 = Nothing
            Me.ToolStripMenuItem49.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem48.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem51.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem50.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem52.Text = "1"
            Me.ToolStripMenuItem18.Text = "1"
        Else

            Me.ToolStripMenuItem52.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem54_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem54.Click
        If Me.ToolStripMenuItem54.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox22.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox22, Nothing)
            monstercard21 = Nothing
            monster21 = Nothing
            Me.ToolStripMenuItem55.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem56.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem57.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem58.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem54.Text = "1"
            Me.ToolStripMenuItem19.Text = "1"
        Else

            Me.ToolStripMenuItem54.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem55_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem55.Click
        If Me.ToolStripMenuItem55.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox23.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox23, Nothing)
            monstercard22 = Nothing
            monster22 = Nothing
            Me.ToolStripMenuItem54.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem56.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem57.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem58.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem55.Text = "1"
            Me.ToolStripMenuItem20.Text = "1"
        Else

            Me.ToolStripMenuItem55.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem56_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem56.Click
        If Me.ToolStripMenuItem56.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox24.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox24, Nothing)
            monstercard23 = Nothing
            monster23 = Nothing
            Me.ToolStripMenuItem54.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem55.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem57.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem58.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem56.Text = "1"
            Me.ToolStripMenuItem21.Text = "1"
        Else

            Me.ToolStripMenuItem56.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem57_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem57.Click
        If Me.ToolStripMenuItem57.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox25.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox25, Nothing)
            monstercard24 = Nothing
            monster24 = Nothing
            Me.ToolStripMenuItem54.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem55.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem56.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem58.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem57.Text = "1"
            Me.ToolStripMenuItem22.Text = "1"
        Else

            Me.ToolStripMenuItem57.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem58_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem58.Click
        If Me.ToolStripMenuItem58.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox26.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox26, Nothing)
            monstercard25 = Nothing
            monster25 = Nothing
            Me.ToolStripMenuItem54.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem55.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem57.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem56.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem58.Text = "1"
            Me.ToolStripMenuItem23.Text = "1"
        Else

            Me.ToolStripMenuItem58.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem60_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem60.Click
        If Me.ToolStripMenuItem60.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox7.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox7, Nothing)
            spellcard11 = Nothing
            szone11 = Nothing
            Me.ToolStripMenuItem61.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem62.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem63.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem64.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem65.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem60.Text = "1"
            Me.ToolStripMenuItem24.Text = "1"
        Else

            Me.ToolStripMenuItem60.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem61_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem61.Click
        If Me.ToolStripMenuItem61.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox8.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox8, Nothing)
            spellcard12 = Nothing
            szone12 = Nothing
            Me.ToolStripMenuItem60.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem62.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem63.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem64.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem65.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem61.Text = "1"
            Me.ToolStripMenuItem25.Text = "1"
        Else

            Me.ToolStripMenuItem61.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem62_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem62.Click
        If Me.ToolStripMenuItem62.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox9.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox9, Nothing)
            spellcard13 = Nothing
            szone13 = Nothing
            Me.ToolStripMenuItem60.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem61.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem63.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem64.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem65.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem62.Text = "1"
            Me.ToolStripMenuItem26.Text = "1"
        Else

            Me.ToolStripMenuItem62.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem63_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem63.Click
        If Me.ToolStripMenuItem63.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox10.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox10, Nothing)
            spellcard14 = Nothing
            szone14 = Nothing
            Me.ToolStripMenuItem61.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem62.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem60.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem64.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem65.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem63.Text = "1"
            Me.ToolStripMenuItem27.Text = "1"
        Else

            Me.ToolStripMenuItem63.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem64_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem64.Click
        If Me.ToolStripMenuItem64.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox11.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox11, Nothing)
            spellcard15 = Nothing
            szone15 = Nothing
            Me.ToolStripMenuItem61.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem62.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem63.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem60.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem65.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem64.Text = "1"
            Me.ToolStripMenuItem28.Text = "1"
        Else

            Me.ToolStripMenuItem64.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem65_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem65.Click
        If Me.ToolStripMenuItem65.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox21.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox21, Nothing)
            spellcard16 = Nothing
            szone16 = Nothing
            Me.ToolStripMenuItem61.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem62.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem63.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem64.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem60.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem65.Text = "1"
            Me.FieldToolStripMenuItem.Text = "1"
        Else

            Me.ToolStripMenuItem65.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem67_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem67.Click
        If Me.ToolStripMenuItem67.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox27.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox27, Nothing)
            spellcard21 = Nothing
            szone21 = Nothing
            Me.ToolStripMenuItem68.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem69.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem71.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem72.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem67.Text = "1"
            Me.ToolStripMenuItem30.Text = "1"
        Else

            Me.ToolStripMenuItem67.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem68_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem68.Click
        If Me.ToolStripMenuItem68.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox28.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox28, Nothing)
            spellcard22 = Nothing
            szone22 = Nothing
            Me.ToolStripMenuItem67.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem69.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem71.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem72.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem68.Text = "1"
            Me.ToolStripMenuItem31.Text = "1"
        Else

            Me.ToolStripMenuItem68.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem69_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem69.Click
        If Me.ToolStripMenuItem69.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox29.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox29, Nothing)
            spellcard23 = Nothing
            szone23 = Nothing
            Me.ToolStripMenuItem68.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem67.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem71.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem72.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem69.Text = "1"
            Me.ToolStripMenuItem32.Text = "1"
        Else

            Me.ToolStripMenuItem69.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem70_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem70.Click
        If Me.ToolStripMenuItem70.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox30.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox30, Nothing)
            spellcard24 = Nothing
            szone24 = Nothing
            Me.ToolStripMenuItem68.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem69.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem67.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem71.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem72.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem67.Text = "1"
            Me.ToolStripMenuItem33.Text = "1"
        Else

            Me.ToolStripMenuItem70.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem71_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem71.Click
        If Me.ToolStripMenuItem71.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox27.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox27, Nothing)
            spellcard25 = Nothing
            szone25 = Nothing
            Me.ToolStripMenuItem68.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem69.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem67.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem71.Text = "1"
            Me.ToolStripMenuItem34.Text = "1"
        Else

            Me.ToolStripMenuItem71.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem72_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem72.Click
        If Me.ToolStripMenuItem72.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox40.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox40, Nothing)
            spellcard26 = Nothing
            szone26 = Nothing
            Me.ToolStripMenuItem68.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem69.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem70.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem71.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem67.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem72.Text = "1"
            Me.FieldToolStripMenuItem1.Text = "1"
        Else

            Me.ToolStripMenuItem72.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem74_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem74.Click
        If Me.ToolStripMenuItem74.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox12.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox12, Nothing)
            handcard11 = Nothing
            hand11 = Nothing
            Me.ToolStripMenuItem75.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem76.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem77.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem78.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem74.Text = "1"
            Me.ToolStripMenuItem36.Text = "1"
        Else

            Me.ToolStripMenuItem74.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem75_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem75.Click
        If Me.ToolStripMenuItem75.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox13.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox13, Nothing)
            handcard12 = Nothing
            hand12 = Nothing
            Me.ToolStripMenuItem74.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem76.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem77.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem78.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem75.Text = "1"
            Me.ToolStripMenuItem37.Text = "1"
        Else

            Me.ToolStripMenuItem75.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem76_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem76.Click
        If Me.ToolStripMenuItem76.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox14.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox14, Nothing)
            handcard13 = Nothing
            hand13 = Nothing
            Me.ToolStripMenuItem75.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem74.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem77.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem78.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem76.Text = "1"
            Me.ToolStripMenuItem38.Text = "1"
        Else

            Me.ToolStripMenuItem76.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem77_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem77.Click
        If Me.ToolStripMenuItem77.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox15.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox15, Nothing)
            handcard14 = Nothing
            hand14 = Nothing
            Me.ToolStripMenuItem75.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem76.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem74.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem78.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem77.Text = "1"
            Me.ToolStripMenuItem39.Text = "1"
        Else

            Me.ToolStripMenuItem77.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem78_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem78.Click
        If Me.ToolStripMenuItem78.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox16.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox16, Nothing)
            handcard15 = Nothing
            hand15 = Nothing
            Me.ToolStripMenuItem75.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem76.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem77.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem74.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem78.Text = "1"
            Me.ToolStripMenuItem40.Text = "1"
        Else

            Me.ToolStripMenuItem78.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem80_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem80.Click
        If Me.ToolStripMenuItem80.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox32.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox32, Nothing)
            handcard21 = Nothing
            hand21 = Nothing
            Me.ToolStripMenuItem81.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem82.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem83.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem84.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem80.Text = "1"
            Me.ToolStripMenuItem42.Text = "1"
        Else

            Me.ToolStripMenuItem80.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem81_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem81.Click
        If Me.ToolStripMenuItem81.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox33.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox33, Nothing)
            handcard22 = Nothing
            hand22 = Nothing
            Me.ToolStripMenuItem80.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem82.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem83.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem84.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem81.Text = "1"
            Me.ToolStripMenuItem43.Text = "1"
        Else

            Me.ToolStripMenuItem81.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem82_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem82.Click
        If Me.ToolStripMenuItem82.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox34.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox34, Nothing)
            handcard23 = Nothing
            hand23 = Nothing
            Me.ToolStripMenuItem80.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem81.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem83.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem84.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem82.Text = "1"
            Me.ToolStripMenuItem44.Text = "1"
        Else

            Me.ToolStripMenuItem82.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem83_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem83.Click
        If Me.ToolStripMenuItem83.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox35.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox35, Nothing)
            handcard24 = Nothing
            hand24 = Nothing
            Me.ToolStripMenuItem80.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem82.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem81.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem84.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem83.Text = "1"
            Me.ToolStripMenuItem45.Text = "1"
        Else

            Me.ToolStripMenuItem83.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem84_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem84.Click
        If Me.ToolStripMenuItem84.CheckState = CheckState.Checked Then
            idselected = Nothing
            cardselected = Nothing

            Me.PictureBox36.Image = Nothing
            Me.ToolTip1.SetToolTip(PictureBox36, Nothing)
            handcard25 = Nothing
            hand25 = Nothing
            Me.ToolStripMenuItem80.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem82.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem83.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem81.CheckState = CheckState.Unchecked

            Me.ToolStripMenuItem84.Text = "1"
            Me.ToolStripMenuItem46.Text = "1"
        Else

            Me.ToolStripMenuItem84.CheckState = CheckState.Checked
        End If
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem4.Click
        If Me.ToolStripMenuItem4.CheckState = CheckState.Checked Then
            idselected = deckcard11
            cardselected = Me.ToolTip1.GetToolTip(PictureBox45)

            Me.ToolStripMenuItem5.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem6.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem7.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem8.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem4.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem5.Click
        If Me.ToolStripMenuItem5.CheckState = CheckState.Checked Then
            idselected = deckcard12
            cardselected = Me.ToolTip1.GetToolTip(PictureBox44)

            Me.ToolStripMenuItem4.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem6.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem7.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem8.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem5.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem6_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem6.Click
        If Me.ToolStripMenuItem6.CheckState = CheckState.Checked Then
            idselected = deckcard13
            cardselected = Me.ToolTip1.GetToolTip(PictureBox43)

            Me.ToolStripMenuItem5.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem4.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem7.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem8.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem6.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem7_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem7.Click
        If Me.ToolStripMenuItem7.CheckState = CheckState.Checked Then
            idselected = deckcard14
            cardselected = Me.ToolTip1.GetToolTip(PictureBox42)

            Me.ToolStripMenuItem5.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem6.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem4.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem8.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem7.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem8_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem8.Click
        If Me.ToolStripMenuItem8.CheckState = CheckState.Checked Then
            idselected = deckcard15
            cardselected = Me.ToolTip1.GetToolTip(PictureBox41)

            Me.ToolStripMenuItem5.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem6.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem7.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem4.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem8.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem86_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem86.Click
        If Me.ToolStripMenuItem86.CheckState = CheckState.Checked Then
            idselected = deckcard21
            cardselected = Me.ToolTip1.GetToolTip(PictureBox50)

            Me.ToolStripMenuItem87.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem88.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem89.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem90.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem86.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem87_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem87.Click
        If Me.ToolStripMenuItem87.CheckState = CheckState.Checked Then
            idselected = deckcard22
            cardselected = Me.ToolTip1.GetToolTip(PictureBox49)

            Me.ToolStripMenuItem86.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem88.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem89.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem90.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem87.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem88_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem88.Click
        If Me.ToolStripMenuItem88.CheckState = CheckState.Checked Then
            idselected = deckcard23
            cardselected = Me.ToolTip1.GetToolTip(PictureBox48)

            Me.ToolStripMenuItem87.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem86.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem89.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem90.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem88.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem89_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem89.Click
        If Me.ToolStripMenuItem89.CheckState = CheckState.Checked Then
            idselected = deckcard24
            cardselected = Me.ToolTip1.GetToolTip(PictureBox47)

            Me.ToolStripMenuItem87.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem88.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem86.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem90.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem89.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem90_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem90.Click
        If Me.ToolStripMenuItem90.CheckState = CheckState.Checked Then
            idselected = deckcard25
            cardselected = Me.ToolTip1.GetToolTip(PictureBox46)

            Me.ToolStripMenuItem87.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem88.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem89.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem86.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem90.CheckState = CheckState.Checked

        End If
    End Sub



    Private Sub ToolStripMenuItem9_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem9.Click
        If Me.ToolStripMenuItem9.CheckState = CheckState.Checked Then
            idselected = deckcard16
            cardselected = Me.ToolTip1.GetToolTip(PictureBox82)

            Me.ToolStripMenuItem10.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem11.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem12.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem13.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem9.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem10_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem10.Click
        If Me.ToolStripMenuItem10.CheckState = CheckState.Checked Then
            idselected = deckcard17
            cardselected = Me.ToolTip1.GetToolTip(PictureBox83)

            Me.ToolStripMenuItem9.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem11.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem12.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem13.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem10.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem11_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem11.Click
        If Me.ToolStripMenuItem11.CheckState = CheckState.Checked Then
            idselected = deckcard18
            cardselected = Me.ToolTip1.GetToolTip(PictureBox84)

            Me.ToolStripMenuItem9.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem10.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem12.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem13.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem11.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem12_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem12.Click
        If Me.ToolStripMenuItem12.CheckState = CheckState.Checked Then
            idselected = deckcard19
            cardselected = Me.ToolTip1.GetToolTip(PictureBox85)

            Me.ToolStripMenuItem9.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem10.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem11.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem13.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem12.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem13_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem13.Click
        If Me.ToolStripMenuItem13.CheckState = CheckState.Checked Then
            idselected = deckcard10
            cardselected = Me.ToolTip1.GetToolTip(PictureBox86)

            Me.ToolStripMenuItem9.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem10.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem11.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem12.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem13.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem91_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem91.Click
        If Me.ToolStripMenuItem91.CheckState = CheckState.Checked Then
            idselected = deckcard26
            cardselected = Me.ToolTip1.GetToolTip(PictureBox102)

            Me.ToolStripMenuItem92.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem93.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem94.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem95.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem91.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem92_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem92.Click
        If Me.ToolStripMenuItem92.CheckState = CheckState.Checked Then
            idselected = deckcard27
            cardselected = Me.ToolTip1.GetToolTip(PictureBox103)

            Me.ToolStripMenuItem91.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem93.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem94.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem95.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem92.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem93_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem93.Click
        If Me.ToolStripMenuItem93.CheckState = CheckState.Checked Then
            idselected = deckcard28
            cardselected = Me.ToolTip1.GetToolTip(PictureBox104)

            Me.ToolStripMenuItem91.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem92.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem94.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem95.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem93.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem94_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem94.Click
        If Me.ToolStripMenuItem94.CheckState = CheckState.Checked Then
            idselected = deckcard29
            cardselected = Me.ToolTip1.GetToolTip(PictureBox105)

            Me.ToolStripMenuItem92.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem93.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem91.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem95.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem94.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub ToolStripMenuItem95_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem95.Click
        If Me.ToolStripMenuItem95.CheckState = CheckState.Checked Then
            idselected = deckcard20
            cardselected = Me.ToolTip1.GetToolTip(PictureBox106)

            Me.ToolStripMenuItem92.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem93.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem91.CheckState = CheckState.Unchecked
            Me.ToolStripMenuItem94.CheckState = CheckState.Unchecked

        Else

            Me.ToolStripMenuItem95.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub CustomDatabaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CustomDatabaseToolStripMenuItem.Click
        CardDatabase.Show()
    End Sub


    Private Sub PuzzleListToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PuzzleListToolStripMenuItem.Click
        Form2.Show()
    End Sub



    Private Sub StartGameToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StartGameToolStripMenuItem.Click

        If (filename <> String.Empty) Then

            My.Computer.FileSystem.CopyFile(ScripFilename, My.Settings.GameDirectory + "/single/" + filename, True)

            Dim process As New Process
            process.StartInfo.WorkingDirectory = My.Settings.GameDirectory
            process.StartInfo.FileName = "ygopro_vs_ai_debug.exe"
            process.StartInfo.Arguments = "-s"
            process.Start()

        End If
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SettingsToolStripMenuItem.Click

    End Sub


End Class
