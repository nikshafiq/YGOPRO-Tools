Imports YGOPRO_PuzzleEditor.GlobalVariables
Imports System.Data.SQLite
Imports System.IO

Public Class ScripFile
    Public Shared filename As String = "000.lua"
    Public Shared ScripFilename As String
    Public Shared stream As StreamReader
    Public Shared lines() As String
    Public Shared line As String
    Public Shared selectedcard As String
    Public Shared selectedname As String
    Public Shared enemyp As String
    Public Shared playerp As String


    Public Shared Sub SearchbyId()


        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT id, name, desc FROM texts WHERE (id = @id)"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@id", selectedcard)
        Dim ds As DataSet = New DataSet()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        con.Open()
        Dim table As New DataTable()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(table)
        Dim source As New BindingSource
        source.DataSource = table
        Form1.DataGridView1.DataSource = source
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
    End Sub

    Public Shared Sub SearchbyName()


        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT id, name, desc FROM texts WHERE (name = @name)"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@id", selectedname)
        Dim ds As DataSet = New DataSet()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        con.Open()
        Dim table As New DataTable()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(table)
        Dim source As New BindingSource
        source.DataSource = table
        Form1.DataGridView1.DataSource = source
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
    End Sub


    Public Shared Sub LoadFile(filepath As String)


        stream = New IO.StreamReader(filepath)
        ScripFilename = filepath
        Form1.TextBox10.Text = filepath
        Form1.TextBox46.Text = Path.GetFileNameWithoutExtension(filepath)

        Do While stream.Peek() >= 0
            line = stream.ReadLine
            If line.Contains("--") Then
                Form1.RichTextBox3.AppendText(line & vbNewLine)
                Form1.RichTextBox5.AppendText(line & vbNewLine)
            End If

            If line.Contains("Debug.SetAIName(") Then
                ainm = line.Split("(").Last

                AIname = ainm.Split(")").First
                Form1.TextBox42.Text = AIname.Trim(My.Settings.Chars)
                Form1.RichTextBox5.AppendText(line & vbNewLine)

            End If
            If line.Contains("Debug.ReloadFieldBegin(") Then
                dueltp = line.Split("(").Last
                Dueltype = dueltp.Split("+").First
                Form1.ComboBox5.Text = Dueltype
                duelmd = dueltp.Split("+").Last
                Duelmode = duelmd.Split(")").First
                Form1.ComboBox6.Text = Duelmode
                Form1.RichTextBox5.AppendText(line & vbNewLine)
            End If
            If line.Contains("Debug.SetPlayerInfo(0,") Then
                playerlp = Split(line, ",", 2, CompareMethod.Text).Last
                Playerpoints = Split(playerlp, ",", 1, CompareMethod.Text).Last
                playerp = Playerpoints.Split(",").First
                Form1.TextBox8.Text = playerp
                Form1.TextBox44.Text = "Player"
                Form1.RichTextBox5.AppendText(line & vbNewLine)
            End If
            If line.Contains("Debug.SetPlayerInfo(1,") Then
                enemylp = Split(line, ",", 2, CompareMethod.Text).Last
                Enemypoints = Split(enemylp, ",", 1, CompareMethod.Text).Last
                enemyp = Enemypoints.Split(",").First
                Form1.TextBox11.Text = enemyp
                Form1.RichTextBox5.AppendText(line & vbNewLine)
            End If
            If line.Contains("0,0,LOCATION_MZONE,0") Then
                monster11 = line.Split("(").Last
                monstercard11 = monster11.Split(",").First
                selectedcard = monstercard11
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard11 & ".jpg"
                Form1.PictureBox2.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox2, Form1.TextBox4.Text)
                If monster11.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton1.Checked = CheckState.Checked
                    Form1.RadioButton5.Checked = CheckState.Checked
                ElseIf monster11.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton2.Checked = CheckState.Checked
                    Form1.RadioButton5.Checked = CheckState.Checked
                ElseIf monster11.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton2.Checked = CheckState.Checked
                    Form1.RadioButton6.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("0,0,LOCATION_MZONE,1") Then
                monster12 = line.Split("(").Last
                monstercard12 = monster12.Split(",").First
                selectedcard = monstercard12
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard12 & ".jpg"
                Form1.PictureBox3.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox3, Form1.TextBox4.Text)
                If monster12.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton3.Checked = CheckState.Checked
                    Form1.RadioButton23.Checked = CheckState.Checked
                ElseIf monster12.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton3.Checked = CheckState.Checked
                    Form1.RadioButton24.Checked = CheckState.Checked
                ElseIf monster12.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton4.Checked = CheckState.Checked
                    Form1.RadioButton24.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("0,0,LOCATION_MZONE,2") Then
                monster13 = line.Split("(").Last
                monstercard13 = monster13.Split(",").First
                selectedcard = monstercard13
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard13 & ".jpg"
                Form1.PictureBox4.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox4, Form1.TextBox4.Text)
                If monster13.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton7.Checked = CheckState.Checked
                    Form1.RadioButton25.Checked = CheckState.Checked
                ElseIf monster13.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton7.Checked = CheckState.Checked
                    Form1.RadioButton26.Checked = CheckState.Checked
                ElseIf monster13.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton8.Checked = CheckState.Checked
                    Form1.RadioButton26.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("0,0,LOCATION_MZONE,3") Then
                monster14 = line.Split("(").Last
                monstercard14 = monster14.Split(",").First
                selectedcard = monstercard14
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard14 & ".jpg"
                Form1.PictureBox5.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox5, Form1.TextBox4.Text)
                If monster14.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton9.Checked = CheckState.Checked
                    Form1.RadioButton27.Checked = CheckState.Checked
                ElseIf monster14.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton9.Checked = CheckState.Checked
                    Form1.RadioButton28.Checked = CheckState.Checked
                ElseIf monster14.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton10.Checked = CheckState.Checked
                    Form1.RadioButton28.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("0,0,LOCATION_MZONE,4") Then
                monster15 = line.Split("(").Last
                monstercard15 = monster15.Split(",").First
                selectedcard = monstercard15
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard15 & ".jpg"
                Form1.PictureBox6.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox6, Form1.TextBox4.Text)
                If monster15.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton11.Checked = CheckState.Checked
                    Form1.RadioButton29.Checked = CheckState.Checked
                ElseIf monster15.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton11.Checked = CheckState.Checked
                    Form1.RadioButton30.Checked = CheckState.Checked
                ElseIf monster15.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton12.Checked = CheckState.Checked
                    Form1.RadioButton30.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("0,0,LOCATION_SZONE,0") Then
                szone11 = line.Split("(").Last
                spellcard11 = szone11.Split(",").First
                selectedcard = spellcard11
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard11 & ".jpg"
                Form1.PictureBox7.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox7, Form1.TextBox4.Text)
                If szone11.Contains("POS_FACEUP") Then
                    Form1.RadioButton13.Checked = CheckState.Checked

                ElseIf szone11.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton14.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,1") Then
                szone12 = line.Split("(").Last
                spellcard12 = szone12.Split(",").First
                selectedcard = spellcard12
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard12 & ".jpg"
                Form1.PictureBox8.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox8, Form1.TextBox4.Text)
                If szone12.Contains("POS_FACEUP") Then
                    Form1.RadioButton15.Checked = CheckState.Checked

                ElseIf szone12.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton16.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,2") Then
                szone13 = line.Split("(").Last
                spellcard13 = szone13.Split(",").First
                selectedcard = spellcard13
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard13 & ".jpg"
                Form1.PictureBox9.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox9, Form1.TextBox4.Text)
                If szone13.Contains("POS_FACEUP") Then
                    Form1.RadioButton17.Checked = CheckState.Checked

                ElseIf szone13.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton18.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,3") Then
                szone14 = line.Split("(").Last
                spellcard14 = szone14.Split(",").First
                selectedcard = spellcard14
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard14 & ".jpg"
                Form1.PictureBox10.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox10, Form1.TextBox4.Text)
                If szone14.Contains("POS_FACEUP") Then
                    Form1.RadioButton19.Checked = CheckState.Checked

                ElseIf szone14.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton20.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,4") Then
                szone15 = line.Split("(").Last
                spellcard15 = szone15.Split(",").First
                selectedcard = spellcard15
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard15 & ".jpg"
                Form1.PictureBox11.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox11, Form1.TextBox4.Text)
                If szone15.Contains("POS_FACEUP") Then
                    Form1.RadioButton21.Checked = CheckState.Checked

                ElseIf szone15.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton22.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,5") Then
                szone16 = line.Split("(").Last
                spellcard16 = szone16.Split(",").First
                selectedcard = spellcard16
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard16 & ".jpg"
                Form1.PictureBox21.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox21, Form1.TextBox4.Text)
                If szone16.Contains("POS_FACEUP") Then
                    Form1.RadioButton65.Checked = CheckState.Checked

                ElseIf szone16.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton66.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,6") Then
                szone17 = line.Split("(").Last
                spellcard17 = szone17.Split(",").First
                selectedcard = spellcard17
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard17 & ".jpg"
                Form1.PictureBox122.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox122, Form1.TextBox4.Text)
                If szone17.Contains("POS_FACEUP") Then
                    Form1.RadioButton69.Checked = CheckState.Checked

                ElseIf szone17.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton70.Checked = CheckState.Checked

                End If


            End If

            If line.Contains("0,0,LOCATION_SZONE,7") Then
                szone18 = line.Split("(").Last
                spellcard18 = szone18.Split(",").First
                selectedcard = spellcard18
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard18 & ".jpg"
                Form1.PictureBox123.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox123, Form1.TextBox4.Text)
                If szone18.Contains("POS_FACEUP") Then
                    Form1.RadioButton71.Checked = CheckState.Checked

                ElseIf szone18.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton72.Checked = CheckState.Checked

                End If


            End If

            If line.Contains(",0,0,LOCATION_HAND,0,") Then

                If hand11 = Nothing Then

                    hand11 = line.Split("(").Last
                    handcard11 = hand11.Split(",").First
                    selectedcard = handcard11
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard11 & ".jpg"
                    Form1.PictureBox12.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox12, Form1.TextBox4.Text)


                ElseIf hand12 = Nothing Then

                    hand12 = line.Split("(").Last
                    handcard12 = hand12.Split(",").First
                    selectedcard = handcard12
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard12 & ".jpg"
                    Form1.PictureBox13.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox13, Form1.TextBox4.Text)

                ElseIf hand13 = Nothing Then

                    hand13 = line.Split("(").Last
                    handcard13 = hand13.Split(",").First
                    selectedcard = handcard13
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard13 & ".jpg"
                    Form1.PictureBox14.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox14, Form1.TextBox4.Text)

                ElseIf hand14 = Nothing Then

                    hand14 = line.Split("(").Last
                    handcard14 = hand14.Split(",").First
                    selectedcard = handcard14
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard14 & ".jpg"
                    Form1.PictureBox15.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox15, Form1.TextBox4.Text)

                ElseIf hand15 = Nothing Then

                    hand15 = line.Split("(").Last
                    handcard15 = hand15.Split(",").First
                    selectedcard = handcard15
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard15 & ".jpg"
                    Form1.PictureBox16.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox16, Form1.TextBox4.Text)



                End If


            End If


            If line.Contains("0,0,LOCATION_DECK,0") Then
                Form1.PictureBox17.Image = My.Resources.cardback
                Form1.Label10.Text += 1
                If deck11 = Nothing Then

                    deck11 = line.Split("(").Last
                    deckcard11 = deck11.Split(",").First
                    selectedcard = deckcard11
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard11 & ".jpg"
                    Form1.PictureBox45.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox45, Form1.TextBox4.Text)


                ElseIf deck12 = Nothing Then

                    deck12 = line.Split("(").Last
                    deckcard12 = deck12.Split(",").First
                    selectedcard = deckcard12
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard12 & ".jpg"
                    Form1.PictureBox44.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox44, Form1.TextBox4.Text)

                ElseIf deck13 = Nothing Then

                    deck13 = line.Split("(").Last
                    deckcard13 = deck13.Split(",").First
                    selectedcard = deckcard13
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard13 & ".jpg"
                    Form1.PictureBox43.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox43, Form1.TextBox4.Text)

                ElseIf deck14 = Nothing Then

                    deck14 = line.Split("(").Last
                    deckcard14 = deck14.Split(",").First
                    selectedcard = deckcard14
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard14 & ".jpg"
                    Form1.PictureBox42.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox42, Form1.TextBox4.Text)

                ElseIf deck15 = Nothing Then

                    deck15 = line.Split("(").Last
                    deckcard15 = deck15.Split(",").First
                    selectedcard = deckcard15
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard15 & ".jpg"
                    Form1.PictureBox41.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox41, Form1.TextBox4.Text)

                ElseIf deck16 = Nothing Then
                    deck16 = line.Split("(").Last
                    deckcard16 = deck16.Split(",").First
                    selectedcard = deckcard16
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard16 & ".jpg"
                    Form1.PictureBox82.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox82, Form1.TextBox4.Text)

                ElseIf deck17 = Nothing Then
                    deck17 = line.Split("(").Last
                    deckcard17 = deck17.Split(",").First
                    selectedcard = deckcard17
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard17 & ".jpg"
                    Form1.PictureBox83.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox83, Form1.TextBox4.Text)

                ElseIf deck18 = Nothing Then
                    deck18 = line.Split("(").Last
                    deckcard18 = deck18.Split(",").First
                    selectedcard = deckcard18
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard18 & ".jpg"
                    Form1.PictureBox84.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox84, Form1.TextBox4.Text)

                ElseIf deck19 = Nothing Then
                    deck19 = line.Split("(").Last
                    deckcard19 = deck19.Split(",").First
                    selectedcard = deckcard19
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard19 & ".jpg"
                    Form1.PictureBox85.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox85, Form1.TextBox4.Text)

                ElseIf deck10 = Nothing Then
                    deck10 = line.Split("(").Last
                    deckcard16 = deck10.Split(",").First
                    selectedcard = deckcard10
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard10 & ".jpg"
                    Form1.PictureBox86.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox86, Form1.TextBox4.Text)

                End If

            End If


            If line.Contains("0,0,LOCATION_GRAVE,0") Then
                Form1.PictureBox18.Image = My.Resources.cardback
                Form1.Label11.Text += 1
                If grave11 = Nothing Then

                    grave11 = line.Split("(").Last
                    gravecard11 = grave11.Split(",").First
                    selectedcard = gravecard11
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard11 & ".jpg"
                    Form1.PictureBox55.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox55, Form1.TextBox4.Text)


                ElseIf grave12 = Nothing Then

                    grave12 = line.Split("(").Last
                    gravecard12 = grave12.Split(",").First
                    selectedcard = gravecard12
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard12 & ".jpg"
                    Form1.PictureBox54.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox54, Form1.TextBox4.Text)

                ElseIf grave13 = Nothing Then

                    grave13 = line.Split("(").Last
                    gravecard13 = grave13.Split(",").First
                    selectedcard = gravecard13
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard13 & ".jpg"
                    Form1.PictureBox53.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox53, Form1.TextBox4.Text)

                ElseIf grave14 = Nothing Then

                    grave14 = line.Split("(").Last
                    gravecard14 = grave14.Split(",").First
                    selectedcard = gravecard14
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard14 & ".jpg"
                    Form1.PictureBox52.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox52, Form1.TextBox4.Text)

                ElseIf grave15 = Nothing Then

                    grave15 = line.Split("(").Last
                    gravecard15 = grave15.Split(",").First
                    selectedcard = gravecard15
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard15 & ".jpg"
                    Form1.PictureBox51.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox51, Form1.TextBox4.Text)

                ElseIf grave16 = Nothing Then

                    grave16 = line.Split("(").Last
                    gravecard16 = grave16.Split(",").First
                    selectedcard = gravecard16
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard16 & ".jpg"
                    Form1.PictureBox87.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox87, Form1.TextBox4.Text)

                ElseIf grave17 = Nothing Then

                    grave17 = line.Split("(").Last
                    gravecard17 = grave17.Split(",").First
                    selectedcard = gravecard17
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard17 & ".jpg"
                    Form1.PictureBox88.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox88, Form1.TextBox4.Text)

                ElseIf grave18 = Nothing Then

                    grave18 = line.Split("(").Last
                    gravecard18 = grave18.Split(",").First
                    selectedcard = gravecard18
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard18 & ".jpg"
                    Form1.PictureBox89.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox89, Form1.TextBox4.Text)


                ElseIf grave19 = Nothing Then

                    grave19 = line.Split("(").Last
                    gravecard19 = grave19.Split(",").First
                    selectedcard = gravecard19
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard19 & ".jpg"
                    Form1.PictureBox90.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox90, Form1.TextBox4.Text)


                ElseIf grave20 = Nothing Then

                    grave20 = line.Split("(").Last
                    gravecard20 = grave20.Split(",").First
                    selectedcard = gravecard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard20 & ".jpg"
                    Form1.PictureBox91.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox91, Form1.TextBox4.Text)

                End If

            End If

            If line.Contains("0,0,LOCATION_REMOVED,0") Then
                Form1.PictureBox19.Image = My.Resources.cardback
                Form1.Label12.Text += 1
                If removed11 = Nothing Then

                    removed11 = line.Split("(").Last
                    removedcard11 = removed11.Split(",").First
                    selectedcard = removedcard11
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard11 & ".jpg"
                    Form1.PictureBox65.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox65, Form1.TextBox4.Text)


                ElseIf removed12 = Nothing Then

                    removed12 = line.Split("(").Last
                    removedcard12 = removed12.Split(",").First
                    selectedcard = removedcard12
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard12 & ".jpg"
                    Form1.PictureBox64.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox64, Form1.TextBox4.Text)

                ElseIf removed13 = Nothing Then

                    removed13 = line.Split("(").Last
                    removedcard13 = removed13.Split(",").First
                    selectedcard = removedcard13
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard13 & ".jpg"
                    Form1.PictureBox63.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox63, Form1.TextBox4.Text)

                ElseIf removed14 = Nothing Then

                    removed14 = line.Split("(").Last
                    removedcard14 = removed14.Split(",").First
                    selectedcard = removedcard14
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard14 & ".jpg"
                    Form1.PictureBox62.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox62, Form1.TextBox4.Text)

                ElseIf removed15 = Nothing Then

                    removed15 = line.Split("(").Last
                    removedcard15 = removed15.Split(",").First
                    selectedcard = removedcard15
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard15 & ".jpg"
                    Form1.PictureBox61.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox61, Form1.TextBox4.Text)

                ElseIf removed16 = Nothing Then

                    removed16 = line.Split("(").Last
                    removedcard16 = removed16.Split(",").First
                    selectedcard = removedcard16
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard16 & ".jpg"
                    Form1.PictureBox92.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox92, Form1.TextBox4.Text)

                ElseIf removed17 = Nothing Then

                    removed17 = line.Split("(").Last
                    removedcard17 = removed17.Split(",").First
                    selectedcard = removedcard17
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard17 & ".jpg"
                    Form1.PictureBox93.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox93, Form1.TextBox4.Text)

                ElseIf removed18 = Nothing Then

                    removed18 = line.Split("(").Last
                    removedcard18 = removed18.Split(",").First
                    selectedcard = removedcard18
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard18 & ".jpg"
                    Form1.PictureBox94.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox94, Form1.TextBox4.Text)


                ElseIf removed19 = Nothing Then

                    removed19 = line.Split("(").Last
                    removedcard19 = removed19.Split(",").First
                    selectedcard = removedcard19
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard19 & ".jpg"
                    Form1.PictureBox95.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox95, Form1.TextBox4.Text)

                ElseIf removed20 = Nothing Then

                    removed20 = line.Split("(").Last
                    removedcard20 = removed20.Split(",").First
                    selectedcard = removedcard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard20 & ".jpg"
                    Form1.PictureBox96.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox96, Form1.TextBox4.Text)

                End If


            End If


            If line.Contains("0,0,LOCATION_EXTRA,0") Then
                Form1.PictureBox20.Image = My.Resources.cardback
                Form1.Label13.Text += 1
                If extra11 = Nothing Then

                    extra11 = line.Split("(").Last
                    extracard11 = extra11.Split(",").First
                    selectedcard = extracard11
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard11 & ".jpg"
                    Form1.PictureBox75.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox75, Form1.TextBox4.Text)


                ElseIf extra12 = Nothing Then

                    extra12 = line.Split("(").Last
                    extracard12 = extra12.Split(",").First
                    selectedcard = extracard12
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard12 & ".jpg"
                    Form1.PictureBox74.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox74, Form1.TextBox4.Text)

                ElseIf extra13 = Nothing Then

                    extra13 = line.Split("(").Last
                    extracard13 = extra13.Split(",").First
                    selectedcard = extracard13
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard13 & ".jpg"
                    Form1.PictureBox73.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox73, Form1.TextBox4.Text)

                ElseIf extra14 = Nothing Then

                    extra14 = line.Split("(").Last
                    extracard14 = extra14.Split(",").First
                    selectedcard = extracard14
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard14 & ".jpg"
                    Form1.PictureBox72.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox72, Form1.TextBox4.Text)

                ElseIf extra15 = Nothing Then

                    extra15 = line.Split("(").Last
                    extracard15 = extra15.Split(",").First
                    selectedcard = extracard15
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard15 & ".jpg"
                    Form1.PictureBox71.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox71, Form1.TextBox4.Text)

                ElseIf extra16 = Nothing Then

                    extra16 = line.Split("(").Last
                    extracard16 = extra16.Split(",").First
                    selectedcard = extracard16
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard16 & ".jpg"
                    Form1.PictureBox97.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox97, Form1.TextBox4.Text)

                ElseIf extra17 = Nothing Then

                    extra17 = line.Split("(").Last
                    extracard17 = extra17.Split(",").First
                    selectedcard = extracard17
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard17 & ".jpg"
                    Form1.PictureBox98.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox98, Form1.TextBox4.Text)


                ElseIf extra18 = Nothing Then

                    extra18 = line.Split("(").Last
                    extracard18 = extra18.Split(",").First
                    selectedcard = extracard18
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard18 & ".jpg"
                    Form1.PictureBox99.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox99, Form1.TextBox4.Text)

                ElseIf extra19 = Nothing Then

                    extra19 = line.Split("(").Last
                    extracard19 = extra19.Split(",").First
                    selectedcard = extracard19
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard19 & ".jpg"
                    Form1.PictureBox100.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox100, Form1.TextBox4.Text)

                ElseIf extra10 = Nothing Then

                    extra10 = line.Split("(").Last
                    extracard10 = extra10.Split(",").First
                    selectedcard = extracard10
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard10 & ".jpg"
                    Form1.PictureBox101.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox101, Form1.TextBox4.Text)

                End If



            End If


            If line.Contains("1,1,LOCATION_MZONE,0") Then
                monster21 = line.Split("(").Last
                monstercard21 = monster21.Split(",").First
                selectedcard = monstercard21
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard21 & ".jpg"
                Form1.PictureBox22.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox22, Form1.TextBox4.Text)
                If monster21.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton35.Checked = CheckState.Checked
                    Form1.RadioButton45.Checked = CheckState.Checked
                ElseIf monster21.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton35.Checked = CheckState.Checked
                    Form1.RadioButton46.Checked = CheckState.Checked
                ElseIf monster21.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton36.Checked = CheckState.Checked
                    Form1.RadioButton46.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("1,1,LOCATION_MZONE,1") Then
                monster22 = line.Split("(").Last
                monstercard22 = monster22.Split(",").First
                selectedcard = monstercard22
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard22 & ".jpg"
                Form1.PictureBox23.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox23, Form1.TextBox4.Text)
                If monster22.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton37.Checked = CheckState.Checked
                    Form1.RadioButton47.Checked = CheckState.Checked
                ElseIf monster22.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton37.Checked = CheckState.Checked
                    Form1.RadioButton48.Checked = CheckState.Checked
                ElseIf monster22.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton38.Checked = CheckState.Checked
                    Form1.RadioButton48.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("1,1,LOCATION_MZONE,2") Then
                monster23 = line.Split("(").Last
                monstercard23 = monster23.Split(",").First
                selectedcard = monstercard23
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard23 & ".jpg"
                Form1.PictureBox24.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox24, Form1.TextBox4.Text)
                If monster23.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton39.Checked = CheckState.Checked
                    Form1.RadioButton49.Checked = CheckState.Checked
                ElseIf monster23.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton39.Checked = CheckState.Checked
                    Form1.RadioButton50.Checked = CheckState.Checked
                ElseIf monster23.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton40.Checked = CheckState.Checked
                    Form1.RadioButton50.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("1,1,LOCATION_MZONE,3") Then
                monster24 = line.Split("(").Last
                monstercard24 = monster24.Split(",").First
                selectedcard = monstercard24
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard24 & ".jpg"
                Form1.PictureBox25.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox25, Form1.TextBox4.Text)
                If monster24.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton41.Checked = CheckState.Checked
                    Form1.RadioButton51.Checked = CheckState.Checked
                ElseIf monster24.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton41.Checked = CheckState.Checked
                    Form1.RadioButton52.Checked = CheckState.Checked
                ElseIf monster24.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton42.Checked = CheckState.Checked
                    Form1.RadioButton52.Checked = CheckState.Checked
                End If
            End If

            If line.Contains("1,1,LOCATION_MZONE,4") Then
                monster25 = line.Split("(").Last
                monstercard25 = monster25.Split(",").First
                selectedcard = monstercard25
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & monstercard25 & ".jpg"
                Form1.PictureBox26.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox26, Form1.TextBox4.Text)
                If monster25.Contains("POS_FACEUP_ATTACK") Then
                    Form1.RadioButton43.Checked = CheckState.Checked
                    Form1.RadioButton53.Checked = CheckState.Checked
                ElseIf monster25.Contains("POS_FACEUP_DEFENCE") Then
                    Form1.RadioButton43.Checked = CheckState.Checked
                    Form1.RadioButton54.Checked = CheckState.Checked
                ElseIf monster25.Contains("POS_FACEDOWN_DEFENCE") Then
                    Form1.RadioButton44.Checked = CheckState.Checked
                    Form1.RadioButton54.Checked = CheckState.Checked
                End If
            End If


            If line.Contains("1,1,LOCATION_SZONE,0") Then
                szone21 = line.Split("(").Last
                spellcard21 = szone21.Split(",").First
                selectedcard = spellcard21
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard21 & ".jpg"
                Form1.PictureBox27.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox27, Form1.TextBox4.Text)
                If szone21.Contains("POS_FACEUP") Then
                    Form1.RadioButton55.Checked = CheckState.Checked

                ElseIf szone21.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton56.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,1") Then
                szone22 = line.Split("(").Last
                spellcard22 = szone22.Split(",").First
                selectedcard = spellcard22
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard22 & ".jpg"
                Form1.PictureBox28.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox28, Form1.TextBox4.Text)
                If szone22.Contains("POS_FACEUP") Then
                    Form1.RadioButton57.Checked = CheckState.Checked

                ElseIf szone22.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton58.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,2") Then
                szone23 = line.Split("(").Last
                spellcard23 = szone23.Split(",").First
                selectedcard = spellcard23
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard23 & ".jpg"
                Form1.PictureBox29.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox29, Form1.TextBox4.Text)
                If szone23.Contains("POS_FACEUP") Then
                    Form1.RadioButton59.Checked = CheckState.Checked

                ElseIf szone23.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton60.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,3") Then
                szone24 = line.Split("(").Last
                spellcard24 = szone24.Split(",").First
                selectedcard = spellcard24
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard24 & ".jpg"
                Form1.PictureBox30.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox30, Form1.TextBox4.Text)
                If szone24.Contains("POS_FACEUP") Then
                    Form1.RadioButton61.Checked = CheckState.Checked

                ElseIf szone24.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton62.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,4") Then
                szone25 = line.Split("(").Last
                spellcard25 = szone25.Split(",").First
                selectedcard = spellcard25
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard25 & ".jpg"
                Form1.PictureBox31.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox31, Form1.TextBox4.Text)
                If szone25.Contains("POS_FACEUP") Then
                    Form1.RadioButton63.Checked = CheckState.Checked

                ElseIf szone25.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton64.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,5") Then
                szone26 = line.Split("(").Last
                spellcard26 = szone26.Split(",").First
                selectedcard = spellcard26
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard26 & ".jpg"
                Form1.PictureBox40.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox40, Form1.TextBox4.Text)
                If szone26.Contains("POS_FACEUP") Then
                    Form1.RadioButton67.Checked = CheckState.Checked

                ElseIf szone26.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton68.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,6") Then
                szone27 = line.Split("(").Last
                spellcard27 = szone27.Split(",").First
                selectedcard = spellcard27
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard27 & ".jpg"
                Form1.PictureBox124.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox124, Form1.TextBox4.Text)
                If szone27.Contains("POS_FACEUP") Then
                    Form1.RadioButton73.Checked = CheckState.Checked

                ElseIf szone27.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton74.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_SZONE,7") Then
                szone28 = line.Split("(").Last
                spellcard28 = szone28.Split(",").First
                selectedcard = spellcard28
                Form1.RichTextBox5.AppendText(line & vbNewLine)
                Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & spellcard28 & ".jpg"
                Form1.PictureBox125.ImageLocation = current
                SearchbyId()
                Form1.ToolTip1.SetToolTip(Form1.PictureBox125, Form1.TextBox4.Text)
                If szone28.Contains("POS_FACEUP") Then
                    Form1.RadioButton75.Checked = CheckState.Checked

                ElseIf szone28.Contains("POS_FACEDOWN") Then
                    Form1.RadioButton76.Checked = CheckState.Checked

                End If

            End If

            If line.Contains("1,1,LOCATION_HAND,0") Then

                If hand21 = Nothing Then

                    hand21 = line.Split("(").Last
                    handcard21 = hand21.Split(",").First
                    selectedcard = handcard21
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard21 & ".jpg"
                    Form1.PictureBox32.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox32, Form1.TextBox4.Text)


                ElseIf hand22 = Nothing Then

                    hand22 = line.Split("(").Last
                    handcard22 = hand22.Split(",").First
                    selectedcard = handcard22
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard22 & ".jpg"
                    Form1.PictureBox33.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox33, Form1.TextBox4.Text)

                ElseIf hand23 = Nothing Then

                    hand23 = line.Split("(").Last
                    handcard23 = hand23.Split(",").First
                    selectedcard = handcard23
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard23 & ".jpg"
                    Form1.PictureBox34.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox34, Form1.TextBox4.Text)

                ElseIf hand24 = Nothing Then

                    hand24 = line.Split("(").Last
                    handcard24 = hand24.Split(",").First
                    selectedcard = handcard24
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard24 & ".jpg"
                    Form1.PictureBox35.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox35, Form1.TextBox4.Text)

                ElseIf hand25 = Nothing Then

                    hand25 = line.Split("(").Last
                    handcard25 = hand25.Split(",").First
                    selectedcard = handcard25
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & handcard25 & ".jpg"
                    Form1.PictureBox36.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox36, Form1.TextBox4.Text)

                End If


            End If


            If line.Contains("1,1,LOCATION_DECK,0") Then
                Form1.PictureBox37.Image = My.Resources.cardback
                Form1.Label28.Text += 1
                If deck21 = Nothing Then

                    deck21 = line.Split("(").Last
                    deckcard21 = deck21.Split(",").First
                    selectedcard = deckcard21
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard21 & ".jpg"
                    Form1.PictureBox50.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox50, Form1.TextBox4.Text)


                ElseIf deck22 = Nothing Then

                    deck22 = line.Split("(").Last
                    deckcard22 = deck22.Split(",").First
                    selectedcard = deckcard22
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard22 & ".jpg"
                    Form1.PictureBox49.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox49, Form1.TextBox4.Text)

                ElseIf deck23 = Nothing Then

                    deck23 = line.Split("(").Last
                    deckcard23 = deck23.Split(",").First
                    selectedcard = deckcard23
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard23 & ".jpg"
                    Form1.PictureBox48.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox48, Form1.TextBox4.Text)

                ElseIf deck24 = Nothing Then

                    deck24 = line.Split("(").Last
                    deckcard24 = deck24.Split(",").First
                    selectedcard = deckcard24
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard24 & ".jpg"
                    Form1.PictureBox47.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox47, Form1.TextBox4.Text)

                ElseIf deck25 = Nothing Then

                    deck25 = line.Split("(").Last
                    deckcard25 = deck25.Split(",").First
                    selectedcard = deckcard25
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard25 & ".jpg"
                    Form1.PictureBox46.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox46, Form1.TextBox4.Text)

                ElseIf deck26 = Nothing Then

                    deck26 = line.Split("(").Last
                    deckcard26 = deck26.Split(",").First
                    selectedcard = deckcard26
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard26 & ".jpg"
                    Form1.PictureBox102.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox102, Form1.TextBox4.Text)

                ElseIf deck27 = Nothing Then

                    deck27 = line.Split("(").Last
                    deckcard27 = deck27.Split(",").First
                    selectedcard = deckcard27
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard27 & ".jpg"
                    Form1.PictureBox103.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox103, Form1.TextBox4.Text)

                ElseIf deck28 = Nothing Then

                    deck28 = line.Split("(").Last
                    deckcard28 = deck28.Split(",").First
                    selectedcard = deckcard28
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard28 & ".jpg"
                    Form1.PictureBox104.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox104, Form1.TextBox4.Text)

                ElseIf deck29 = Nothing Then

                    deck29 = line.Split("(").Last
                    deckcard29 = deck29.Split(",").First
                    selectedcard = deckcard29
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard29 & ".jpg"
                    Form1.PictureBox105.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox105, Form1.TextBox4.Text)

                ElseIf deck20 = Nothing Then

                    deck20 = line.Split("(").Last
                    deckcard20 = deck20.Split(",").First
                    selectedcard = deckcard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & deckcard20 & ".jpg"
                    Form1.PictureBox106.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox106, Form1.TextBox4.Text)

                End If

            End If

            If line.Contains("1,1,LOCATION_GRAVE,0") Then
                Form1.PictureBox38.Image = My.Resources.cardback
                Form1.Label27.Text += 1
                If grave21 = Nothing Then

                    grave21 = line.Split("(").Last
                    gravecard21 = grave21.Split(",").First
                    selectedcard = gravecard21
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard21 & ".jpg"
                    Form1.PictureBox60.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox60, Form1.TextBox4.Text)


                ElseIf grave22 = Nothing Then

                    grave22 = line.Split("(").Last
                    gravecard22 = grave22.Split(",").First
                    selectedcard = gravecard22
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard22 & ".jpg"
                    Form1.PictureBox59.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox59, Form1.TextBox4.Text)

                ElseIf grave23 = Nothing Then

                    grave23 = line.Split("(").Last
                    gravecard23 = grave23.Split(",").First
                    selectedcard = gravecard23
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard23 & ".jpg"
                    Form1.PictureBox58.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox58, Form1.TextBox4.Text)

                ElseIf grave24 = Nothing Then

                    grave24 = line.Split("(").Last
                    gravecard24 = grave24.Split(",").First
                    selectedcard = gravecard24
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard24 & ".jpg"
                    Form1.PictureBox57.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox57, Form1.TextBox4.Text)

                ElseIf grave25 = Nothing Then

                    grave25 = line.Split("(").Last
                    gravecard25 = grave25.Split(",").First
                    selectedcard = gravecard25
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard25 & ".jpg"
                    Form1.PictureBox56.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox56, Form1.TextBox4.Text)

                ElseIf grave26 = Nothing Then

                    grave26 = line.Split("(").Last
                    gravecard26 = grave26.Split(",").First
                    selectedcard = gravecard26
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard26 & ".jpg"
                    Form1.PictureBox107.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox107, Form1.TextBox4.Text)

                ElseIf grave27 = Nothing Then

                    grave27 = line.Split("(").Last
                    gravecard27 = grave27.Split(",").First
                    selectedcard = gravecard27
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard27 & ".jpg"
                    Form1.PictureBox108.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox108, Form1.TextBox4.Text)

                ElseIf grave28 = Nothing Then

                    grave28 = line.Split("(").Last
                    gravecard28 = grave28.Split(",").First
                    selectedcard = gravecard28
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard28 & ".jpg"
                    Form1.PictureBox109.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox109, Form1.TextBox4.Text)


                ElseIf grave29 = Nothing Then

                    grave29 = line.Split("(").Last
                    gravecard29 = grave29.Split(",").First
                    selectedcard = gravecard29
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard29 & ".jpg"
                    Form1.PictureBox110.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox110, Form1.TextBox4.Text)

                ElseIf grave20 = Nothing Then

                    grave20 = line.Split("(").Last
                    gravecard20 = grave20.Split(",").First
                    selectedcard = gravecard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & gravecard20 & ".jpg"
                    Form1.PictureBox111.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox111, Form1.TextBox4.Text)


                End If

            End If


            If line.Contains("1,1,LOCATION_REMOVED,0") Then
                Form1.PictureBox39.Image = My.Resources.cardback
                Form1.Label23.Text += 1
                If removed21 = Nothing Then

                    removed21 = line.Split("(").Last
                    removedcard21 = removed21.Split(",").First
                    selectedcard = removedcard21
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard21 & ".jpg"
                    Form1.PictureBox70.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox70, Form1.TextBox4.Text)


                ElseIf removed22 = Nothing Then

                    removed22 = line.Split("(").Last
                    removedcard22 = removed22.Split(",").First
                    selectedcard = removedcard22
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard22 & ".jpg"
                    Form1.PictureBox69.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox69, Form1.TextBox4.Text)

                ElseIf removed23 = Nothing Then

                    removed23 = line.Split("(").Last
                    removedcard23 = removed23.Split(",").First
                    selectedcard = removedcard23
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard23 & ".jpg"
                    Form1.PictureBox68.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox68, Form1.TextBox4.Text)

                ElseIf removed24 = Nothing Then

                    removed24 = line.Split("(").Last
                    removedcard24 = removed24.Split(",").First
                    selectedcard = removedcard24
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard24 & ".jpg"
                    Form1.PictureBox67.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox67, Form1.TextBox4.Text)

                ElseIf removed25 = Nothing Then

                    removed25 = line.Split("(").Last
                    removedcard25 = removed25.Split(",").First
                    selectedcard = removedcard25
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard25 & ".jpg"
                    Form1.PictureBox66.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox66, Form1.TextBox4.Text)

                ElseIf removed26 = Nothing Then

                    removed26 = line.Split("(").Last
                    removedcard26 = removed26.Split(",").First
                    selectedcard = removedcard26
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard26 & ".jpg"
                    Form1.PictureBox112.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox112, Form1.TextBox4.Text)


                ElseIf removed27 = Nothing Then

                    removed27 = line.Split("(").Last
                    removedcard27 = removed27.Split(",").First
                    selectedcard = removedcard27
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard27 & ".jpg"
                    Form1.PictureBox113.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox113, Form1.TextBox4.Text)


                ElseIf removed28 = Nothing Then

                    removed28 = line.Split("(").Last
                    removedcard28 = removed28.Split(",").First
                    selectedcard = removedcard28
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard28 & ".jpg"
                    Form1.PictureBox114.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox114, Form1.TextBox4.Text)


                ElseIf removed29 = Nothing Then

                    removed29 = line.Split("(").Last
                    removedcard29 = removed29.Split(",").First
                    selectedcard = removedcard29
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard29 & ".jpg"
                    Form1.PictureBox115.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox115, Form1.TextBox4.Text)


                ElseIf removed20 = Nothing Then

                    removed20 = line.Split("(").Last
                    removedcard20 = removed20.Split(",").First
                    selectedcard = removedcard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & removedcard20 & ".jpg"
                    Form1.PictureBox116.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox116, Form1.TextBox4.Text)

                End If


            End If


            If line.Contains("1,1,LOCATION_EXTRA,0") Then
                Form1.PictureBox81.Image = My.Resources.cardback
                Form1.Label30.Text += 1
                If extra21 = Nothing Then

                    extra21 = line.Split("(").Last
                    extracard21 = extra21.Split(",").First
                    selectedcard = extracard21
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard21 & ".jpg"
                    Form1.PictureBox80.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox80, Form1.TextBox4.Text)


                ElseIf extra22 = Nothing Then

                    extra22 = line.Split("(").Last
                    extracard22 = extra22.Split(",").First
                    selectedcard = extracard22
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard22 & ".jpg"
                    Form1.PictureBox79.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox79, Form1.TextBox4.Text)

                ElseIf extra23 = Nothing Then

                    extra23 = line.Split("(").Last
                    extracard23 = extra23.Split(",").First
                    selectedcard = extracard23
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard23 & ".jpg"
                    Form1.PictureBox78.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox78, Form1.TextBox4.Text)

                ElseIf extra24 = Nothing Then

                    extra24 = line.Split("(").Last
                    extracard24 = extra24.Split(",").First
                    selectedcard = extracard24
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard24 & ".jpg"
                    Form1.PictureBox77.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox77, Form1.TextBox4.Text)

                ElseIf extra25 = Nothing Then

                    extra25 = line.Split("(").Last
                    extracard25 = extra25.Split(",").First
                    selectedcard = extracard25
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard25 & ".jpg"
                    Form1.PictureBox76.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox76, Form1.TextBox4.Text)

                ElseIf extra26 = Nothing Then

                    extra26 = line.Split("(").Last
                    extracard26 = extra26.Split(",").First
                    selectedcard = extracard26
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard26 & ".jpg"
                    Form1.PictureBox117.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox117, Form1.TextBox4.Text)

                ElseIf extra27 = Nothing Then

                    extra27 = line.Split("(").Last
                    extracard27 = extra27.Split(",").First
                    selectedcard = extracard27
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard27 & ".jpg"
                    Form1.PictureBox118.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox118, Form1.TextBox4.Text)

                ElseIf extra28 = Nothing Then

                    extra28 = line.Split("(").Last
                    extracard28 = extra28.Split(",").First
                    selectedcard = extracard28
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard28 & ".jpg"
                    Form1.PictureBox119.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox119, Form1.TextBox4.Text)


                ElseIf extra29 = Nothing Then

                    extra29 = line.Split("(").Last
                    extracard29 = extra29.Split(",").First
                    selectedcard = extracard29
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard29 & ".jpg"
                    Form1.PictureBox120.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox120, Form1.TextBox4.Text)

                ElseIf extra20 = Nothing Then

                    extra20 = line.Split("(").Last
                    extracard20 = extra20.Split(",").First
                    selectedcard = extracard20
                    SearchbyId()
                    Form1.RichTextBox5.AppendText(line & vbNewLine)
                    Dim current As String = My.Settings.GameDirectory & "\pics\thumbnail\" & extracard20 & ".jpg"
                    Form1.PictureBox121.ImageLocation = current
                    Form1.ToolTip1.SetToolTip(Form1.PictureBox121, Form1.TextBox4.Text)

                End If



            End If

            If line.Contains("Debug.ShowHint(") Then

                If HINT1 = Nothing Then


                    HINT1 = line.Split("(").Last

                    SHOWHINT1 = HINT1.Split(")").First
                    Form1.TextBox12.Text = SHOWHINT1.Trim(My.Settings.Chars)
                    Form1.RichTextBox5.AppendText(line & vbNewLine)

                ElseIf HINT2 = Nothing Then

                    HINT2 = line.Split("(").Last

                    SHOWHINT2 = HINT2.Split(")").First
                    Form1.TextBox13.Text = SHOWHINT2.Trim(My.Settings.Chars)
                    Form1.RichTextBox5.AppendText(line & vbNewLine)

                End If

            End If

        Loop

        stream.Close()

        stream.Dispose()



    End Sub




    Public Shared Sub ClearScript()
        If Not (Form1.PictureBox2.Image) Is Nothing Then

            Form1.PictureBox2.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox2, "")

        End If
        If Not (Form1.PictureBox3.Image) Is Nothing Then

            Form1.PictureBox3.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox3, "")

        End If
        If Not (Form1.PictureBox4.Image) Is Nothing Then

            Form1.PictureBox4.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox4, "")

        End If
        If Not (Form1.PictureBox5.Image) Is Nothing Then

            Form1.PictureBox5.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox5, "")

        End If
        If Not (Form1.PictureBox6.Image) Is Nothing Then

            Form1.PictureBox6.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox6, "")

        End If

        If Not (Form1.PictureBox7.Image) Is Nothing Then

            Form1.PictureBox7.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox7, "")

        End If
        If Not (Form1.PictureBox8.Image) Is Nothing Then

            Form1.PictureBox8.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox8, "")

        End If

        If Not (Form1.PictureBox9.Image) Is Nothing Then

            Form1.PictureBox9.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox9, "")

        End If

        If Not (Form1.PictureBox10.Image) Is Nothing Then

            Form1.PictureBox10.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox10, "")

        End If

        If Not (Form1.PictureBox11.Image) Is Nothing Then

            Form1.PictureBox11.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox11, "")

        End If

        If Not (Form1.PictureBox12.Image) Is Nothing Then

            Form1.PictureBox12.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox12, "")

        End If
        If Not (Form1.PictureBox13.Image) Is Nothing Then

            Form1.PictureBox13.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox13, "")

        End If
        If Not (Form1.PictureBox14.Image) Is Nothing Then

            Form1.PictureBox14.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox14, "")

        End If
        If Not (Form1.PictureBox15.Image) Is Nothing Then

            Form1.PictureBox15.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox15, "")

        End If

        If Not (Form1.PictureBox16.Image) Is Nothing Then

            Form1.PictureBox16.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox16, "")

        End If
        If Not (Form1.PictureBox17.Image) Is Nothing Then

            Form1.PictureBox17.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox17, "")

        End If

        If Not (Form1.PictureBox18.Image) Is Nothing Then

            Form1.PictureBox18.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox18, "")

        End If

        If Not (Form1.PictureBox19.Image) Is Nothing Then

            Form1.PictureBox19.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox19, "")

        End If

        If Not (Form1.PictureBox20.Image) Is Nothing Then

            Form1.PictureBox20.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox20, "")

        End If

        If Not (Form1.PictureBox21.Image) Is Nothing Then

            Form1.PictureBox21.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox21, "")

        End If

        If Not (Form1.PictureBox22.Image) Is Nothing Then

            Form1.PictureBox22.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox22, "")

        End If
        If Not (Form1.PictureBox23.Image) Is Nothing Then

            Form1.PictureBox23.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox23, "")

        End If
        If Not (Form1.PictureBox24.Image) Is Nothing Then

            Form1.PictureBox24.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox24, "")

        End If
        If Not (Form1.PictureBox25.Image) Is Nothing Then

            Form1.PictureBox25.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox25, "")

        End If
        If Not (Form1.PictureBox26.Image) Is Nothing Then

            Form1.PictureBox26.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox26, "")

        End If
        If Not (Form1.PictureBox27.Image) Is Nothing Then

            Form1.PictureBox27.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox27, "")

        End If
        If Not (Form1.PictureBox28.Image) Is Nothing Then

            Form1.PictureBox28.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox28, "")

        End If
        If Not (Form1.PictureBox29.Image) Is Nothing Then

            Form1.PictureBox29.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox29, "")

        End If

        If Not (Form1.PictureBox30.Image) Is Nothing Then

            Form1.PictureBox30.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox30, "")

        End If

        If Not (Form1.PictureBox31.Image) Is Nothing Then

            Form1.PictureBox31.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox31, "")

        End If

        If Not (Form1.PictureBox32.Image) Is Nothing Then

            Form1.PictureBox32.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox32, "")

        End If
        If Not (Form1.PictureBox33.Image) Is Nothing Then

            Form1.PictureBox33.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox33, "")

        End If
        If Not (Form1.PictureBox34.Image) Is Nothing Then

            Form1.PictureBox34.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox34, "")

        End If
        If Not (Form1.PictureBox35.Image) Is Nothing Then

            Form1.PictureBox35.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox35, "")

        End If
        If Not (Form1.PictureBox36.Image) Is Nothing Then

            Form1.PictureBox36.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox36, "")

        End If
        If Not (Form1.PictureBox37.Image) Is Nothing Then

            Form1.PictureBox37.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox37, "")

        End If
        If Not (Form1.PictureBox38.Image) Is Nothing Then

            Form1.PictureBox38.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox38, "")

        End If
        If Not (Form1.PictureBox39.Image) Is Nothing Then

            Form1.PictureBox39.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox39, "")

        End If
        If Not (Form1.PictureBox40.Image) Is Nothing Then

            Form1.PictureBox40.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox40, "")

        End If
        If Not (Form1.PictureBox41.Image) Is Nothing Then

            Form1.PictureBox41.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox41, "")

        End If
        If Not (Form1.PictureBox42.Image) Is Nothing Then

            Form1.PictureBox42.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox42, "")

        End If
        If Not (Form1.PictureBox43.Image) Is Nothing Then

            Form1.PictureBox43.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox43, "")

        End If
        If Not (Form1.PictureBox44.Image) Is Nothing Then

            Form1.PictureBox44.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox44, "")

        End If
        If Not (Form1.PictureBox45.Image) Is Nothing Then

            Form1.PictureBox45.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox45, "")

        End If
        If Not (Form1.PictureBox46.Image) Is Nothing Then

            Form1.PictureBox46.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox46, "")

        End If

        If Not (Form1.PictureBox46.Image) Is Nothing Then

            Form1.PictureBox46.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox46, "")

        End If
        If Not (Form1.PictureBox47.Image) Is Nothing Then

            Form1.PictureBox47.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox47, "")

        End If
        If Not (Form1.PictureBox48.Image) Is Nothing Then

            Form1.PictureBox48.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox48, "")

        End If
        If Not (Form1.PictureBox49.Image) Is Nothing Then

            Form1.PictureBox49.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox49, "")

        End If
        If Not (Form1.PictureBox50.Image) Is Nothing Then

            Form1.PictureBox50.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox50, "")

        End If
        If Not (Form1.PictureBox51.Image) Is Nothing Then

            Form1.PictureBox51.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox51, "")

        End If
        If Not (Form1.PictureBox52.Image) Is Nothing Then

            Form1.PictureBox52.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox52, "")

        End If
        If Not (Form1.PictureBox53.Image) Is Nothing Then

            Form1.PictureBox53.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox53, "")

        End If
        If Not (Form1.PictureBox54.Image) Is Nothing Then

            Form1.PictureBox54.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox54, "")


        End If
        If Not (Form1.PictureBox55.Image) Is Nothing Then

            Form1.PictureBox55.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox55, "")

        End If
        If Not (Form1.PictureBox56.Image) Is Nothing Then

            Form1.PictureBox56.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox56, "")

        End If
        If Not (Form1.PictureBox57.Image) Is Nothing Then

            Form1.PictureBox57.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox57, "")

        End If
        If Not (Form1.PictureBox58.Image) Is Nothing Then

            Form1.PictureBox58.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox58, "")

        End If
        If Not (Form1.PictureBox59.Image) Is Nothing Then

            Form1.PictureBox59.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox59, "")

        End If
        If Not (Form1.PictureBox60.Image) Is Nothing Then

            Form1.PictureBox60.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox60, "")

        End If
        If Not (Form1.PictureBox61.Image) Is Nothing Then

            Form1.PictureBox61.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox61, "")

        End If
        If Not (Form1.PictureBox62.Image) Is Nothing Then

            Form1.PictureBox62.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox62, "")

        End If
        If Not (Form1.PictureBox63.Image) Is Nothing Then

            Form1.PictureBox63.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox63, "")

        End If
        If Not (Form1.PictureBox64.Image) Is Nothing Then

            Form1.PictureBox64.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox64, "")

        End If
        If Not (Form1.PictureBox65.Image) Is Nothing Then

            Form1.PictureBox65.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox65, "")

        End If
        If Not (Form1.PictureBox66.Image) Is Nothing Then

            Form1.PictureBox66.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox66, "")

        End If
        If Not (Form1.PictureBox67.Image) Is Nothing Then

            Form1.PictureBox67.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox67, "")

        End If
        If Not (Form1.PictureBox68.Image) Is Nothing Then

            Form1.PictureBox68.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox68, "")

        End If
        If Not (Form1.PictureBox69.Image) Is Nothing Then

            Form1.PictureBox69.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox69, "")

        End If
        If Not (Form1.PictureBox70.Image) Is Nothing Then

            Form1.PictureBox70.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox70, "")

        End If

        If Not (Form1.PictureBox71.Image) Is Nothing Then

            Form1.PictureBox71.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox71, "")

        End If
        If Not (Form1.PictureBox72.Image) Is Nothing Then

            Form1.PictureBox72.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox72, "")

        End If
        If Not (Form1.PictureBox73.Image) Is Nothing Then

            Form1.PictureBox73.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox73, "")

        End If
        If Not (Form1.PictureBox74.Image) Is Nothing Then

            Form1.PictureBox74.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox74, "")

        End If
        If Not (Form1.PictureBox75.Image) Is Nothing Then

            Form1.PictureBox75.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox75, "")

        End If
        If Not (Form1.PictureBox76.Image) Is Nothing Then

            Form1.PictureBox76.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox76, "")

        End If
        If Not (Form1.PictureBox77.Image) Is Nothing Then

            Form1.PictureBox77.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox77, "")

        End If
        If Not (Form1.PictureBox78.Image) Is Nothing Then

            Form1.PictureBox78.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox78, "")

        End If
        If Not (Form1.PictureBox79.Image) Is Nothing Then

            Form1.PictureBox79.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox79, "")

        End If
        If Not (Form1.PictureBox80.Image) Is Nothing Then

            Form1.PictureBox80.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox80, "")

        End If
        If Not (Form1.PictureBox81.Image) Is Nothing Then

            Form1.PictureBox81.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox81, "")

        End If
        If Not (Form1.PictureBox82.Image) Is Nothing Then

            Form1.PictureBox82.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox82, "")

        End If
        If Not (Form1.PictureBox83.Image) Is Nothing Then

            Form1.PictureBox83.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox83, "")


        End If
        If Not (Form1.PictureBox84.Image) Is Nothing Then

            Form1.PictureBox84.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox84, "")

        End If
        If Not (Form1.PictureBox85.Image) Is Nothing Then

            Form1.PictureBox85.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox85, "")

        End If
        If Not (Form1.PictureBox86.Image) Is Nothing Then

            Form1.PictureBox86.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox86, "")

        End If
        If Not (Form1.PictureBox87.Image) Is Nothing Then

            Form1.PictureBox87.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox87, "")

        End If
        If Not (Form1.PictureBox88.Image) Is Nothing Then

            Form1.PictureBox88.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox88, "")

        End If
        If Not (Form1.PictureBox89.Image) Is Nothing Then

            Form1.PictureBox89.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox89, "")

        End If
        If Not (Form1.PictureBox90.Image) Is Nothing Then

            Form1.PictureBox90.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox90, "")

        End If
        If Not (Form1.PictureBox91.Image) Is Nothing Then

            Form1.PictureBox91.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox91, "")

        End If
        If Not (Form1.PictureBox92.Image) Is Nothing Then

            Form1.PictureBox92.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox92, "")

        End If
        If Not (Form1.PictureBox93.Image) Is Nothing Then

            Form1.PictureBox93.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox93, "")

        End If
        If Not (Form1.PictureBox94.Image) Is Nothing Then

            Form1.PictureBox94.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox94, "")

        End If
        If Not (Form1.PictureBox95.Image) Is Nothing Then

            Form1.PictureBox95.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox95, "")

        End If
        If Not (Form1.PictureBox96.Image) Is Nothing Then

            Form1.PictureBox96.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox96, "")

        End If
        If Not (Form1.PictureBox97.Image) Is Nothing Then

            Form1.PictureBox97.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox97, "")

        End If
        If Not (Form1.PictureBox98.Image) Is Nothing Then

            Form1.PictureBox98.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox98, "")

        End If
        If Not (Form1.PictureBox99.Image) Is Nothing Then

            Form1.PictureBox99.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox99, "")

        End If
        If Not (Form1.PictureBox100.Image) Is Nothing Then

            Form1.PictureBox100.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox100, "")

        End If
        If Not (Form1.PictureBox101.Image) Is Nothing Then

            Form1.PictureBox101.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox101, "")

        End If
        If Not (Form1.PictureBox102.Image) Is Nothing Then

            Form1.PictureBox102.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox102, "")

        End If
        If Not (Form1.PictureBox103.Image) Is Nothing Then

            Form1.PictureBox103.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox103, "")

        End If
        If Not (Form1.PictureBox104.Image) Is Nothing Then

            Form1.PictureBox104.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox104, "")

        End If
        If Not (Form1.PictureBox105.Image) Is Nothing Then

            Form1.PictureBox105.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox105, "")

        End If
        If Not (Form1.PictureBox106.Image) Is Nothing Then

            Form1.PictureBox106.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox106, "")

        End If
        If Not (Form1.PictureBox107.Image) Is Nothing Then

            Form1.PictureBox107.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox107, "")

        End If

        If Not (Form1.PictureBox108.Image) Is Nothing Then

            Form1.PictureBox108.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox108, "")

        End If
        If Not (Form1.PictureBox109.Image) Is Nothing Then

            Form1.PictureBox109.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox109, "")

        End If
        If Not (Form1.PictureBox110.Image) Is Nothing Then

            Form1.PictureBox110.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox110, "")

        End If
        If Not (Form1.PictureBox111.Image) Is Nothing Then

            Form1.PictureBox111.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox111, "")

        End If
        If Not (Form1.PictureBox112.Image) Is Nothing Then

            Form1.PictureBox112.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox112, "")

        End If
        If Not (Form1.PictureBox113.Image) Is Nothing Then

            Form1.PictureBox113.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox113, "")

        End If
        If Not (Form1.PictureBox114.Image) Is Nothing Then

            Form1.PictureBox114.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox114, "")

        End If
        If Not (Form1.PictureBox115.Image) Is Nothing Then

            Form1.PictureBox115.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox115, "")

        End If
        If Not (Form1.PictureBox116.Image) Is Nothing Then

            Form1.PictureBox116.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox116, "")

        End If
        If Not (Form1.PictureBox117.Image) Is Nothing Then

            Form1.PictureBox117.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox117, "")

        End If
        If Not (Form1.PictureBox118.Image) Is Nothing Then

            Form1.PictureBox118.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox118, "")

        End If
        If Not (Form1.PictureBox119.Image) Is Nothing Then

            Form1.PictureBox119.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox119, "")

        End If
        If Not (Form1.PictureBox120.Image) Is Nothing Then

            Form1.PictureBox120.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox120, "")

        End If
        If Not (Form1.PictureBox121.Image) Is Nothing Then

            Form1.PictureBox121.Image = Nothing
            Form1.ToolTip1.SetToolTip(Form1.PictureBox121, "")

        End If



        If hand11 <> Nothing Then

            hand11 = Nothing
        End If

        If hand12 <> Nothing Then

            hand12 = Nothing
        End If

        If hand13 <> Nothing Then

            hand13 = Nothing
        End If
        If hand14 <> Nothing Then

            hand14 = Nothing
        End If
        If hand15 <> Nothing Then

            hand15 = Nothing
        End If

        If deck11 <> Nothing Then

            deck11 = Nothing
        End If

        If deck12 <> Nothing Then

            deck12 = Nothing
        End If

        If deck13 <> Nothing Then

            deck13 = Nothing
        End If

        If deck14 <> Nothing Then

            deck14 = Nothing
        End If

        If deck15 <> Nothing Then

            deck15 = Nothing
        End If
        If deck16 <> Nothing Then

            deck16 = Nothing
        End If
        If deck17 <> Nothing Then

            deck17 = Nothing
        End If

        If deck18 <> Nothing Then

            deck18 = Nothing
        End If
        If deck19 <> Nothing Then

            deck19 = Nothing
        End If
        If deck10 <> Nothing Then

            deck10 = Nothing
        End If

        If removed11 <> Nothing Then
            removed11 = Nothing
        End If

        If removed12 <> Nothing Then
            removed12 = Nothing
        End If

        If removed13 <> Nothing Then
            removed13 = Nothing
        End If
        If removed14 <> Nothing Then
            removed14 = Nothing
        End If
        If removed15 <> Nothing Then
            removed15 = Nothing
        End If

        If removed16 <> Nothing Then
            removed16 = Nothing
        End If

        If removed17 <> Nothing Then
            removed17 = Nothing
        End If

        If removed18 <> Nothing Then
            removed18 = Nothing
        End If

        If removed19 <> Nothing Then
            removed19 = Nothing
        End If

        If removed10 <> Nothing Then
            removed10 = Nothing
        End If

        If grave11 <> Nothing Then

            grave11 = Nothing
        End If

        If grave12 <> Nothing Then

            grave12 = Nothing
        End If

        If grave13 <> Nothing Then

            grave13 = Nothing
        End If

        If grave14 <> Nothing Then

            grave14 = Nothing
        End If

        If grave15 <> Nothing Then

            grave15 = Nothing
        End If

        If grave16 <> Nothing Then

            grave16 = Nothing
        End If

        If grave17 <> Nothing Then

            grave17 = Nothing
        End If

        If grave18 <> Nothing Then

            grave18 = Nothing
        End If

        If grave19 <> Nothing Then

            grave19 = Nothing
        End If

        If grave10 <> Nothing Then

            grave10 = Nothing
        End If

        If extra11 <> Nothing Then

            extra11 = Nothing
        End If
        If extra12 <> Nothing Then

            extra12 = Nothing
        End If
        If extra13 <> Nothing Then

            extra13 = Nothing
        End If
        If extra14 <> Nothing Then

            extra14 = Nothing
        End If
        If extra15 <> Nothing Then

            extra15 = Nothing
        End If
        If extra16 <> Nothing Then

            extra16 = Nothing
        End If
        If extra17 <> Nothing Then

            extra17 = Nothing
        End If
        If extra18 <> Nothing Then

            extra18 = Nothing
        End If
        If extra19 <> Nothing Then

            extra19 = Nothing
        End If
        If extra10 <> Nothing Then

            extra10 = Nothing
        End If

        If hand21 <> Nothing Then

            hand21 = Nothing
        End If
        If hand22 <> Nothing Then

            hand22 = Nothing
        End If
        If hand23 <> Nothing Then

            hand23 = Nothing
        End If
        If hand24 <> Nothing Then

            hand24 = Nothing
        End If
        If hand25 <> Nothing Then

            hand25 = Nothing
        End If
        If hand25 <> Nothing Then

            hand25 = Nothing
        End If

        If deck21 <> Nothing Then

            deck21 = Nothing
        End If
        If deck22 <> Nothing Then

            deck22 = Nothing
        End If
        If deck23 <> Nothing Then

            deck23 = Nothing
        End If
        If deck24 <> Nothing Then

            deck24 = Nothing
        End If
        If deck25 <> Nothing Then

            deck25 = Nothing
        End If

        If deck26 <> Nothing Then

            deck26 = Nothing
        End If

        If deck27 <> Nothing Then

            deck27 = Nothing
        End If
        If deck28 <> Nothing Then

            deck28 = Nothing
        End If
        If deck29 <> Nothing Then

            deck29 = Nothing
        End If
        If deck20 <> Nothing Then

            deck20 = Nothing
        End If
        If grave21 <> Nothing Then

            grave21 = Nothing
        End If
        If grave22 <> Nothing Then

            grave22 = Nothing
        End If
        If grave23 <> Nothing Then

            grave23 = Nothing
        End If
        If grave24 <> Nothing Then

            grave24 = Nothing
        End If
        If grave25 <> Nothing Then

            grave25 = Nothing
        End If
        If grave26 <> Nothing Then

            grave26 = Nothing
        End If
        If grave27 <> Nothing Then

            grave27 = Nothing
        End If
        If grave28 <> Nothing Then

            grave28 = Nothing
        End If
        If grave29 <> Nothing Then

            grave29 = Nothing
        End If
        If grave20 <> Nothing Then

            grave20 = Nothing
        End If

        If removed21 <> Nothing Then

            removed21 = Nothing
        End If
        If removed22 <> Nothing Then

            removed22 = Nothing
        End If
        If removed23 <> Nothing Then

            removed23 = Nothing
        End If
        If removed24 <> Nothing Then

            removed24 = Nothing
        End If
        If removed25 <> Nothing Then

            removed25 = Nothing
        End If
        If removed26 <> Nothing Then

            removed26 = Nothing
        End If
        If removed27 <> Nothing Then

            removed27 = Nothing
        End If
        If removed28 <> Nothing Then

            removed28 = Nothing
        End If
        If removed29 <> Nothing Then

            removed29 = Nothing
        End If
        If removed20 <> Nothing Then

            removed20 = Nothing
        End If


        If extra21 <> Nothing Then

            extra21 = Nothing
        End If
        If extra22 <> Nothing Then

            extra22 = Nothing
        End If
        If extra23 <> Nothing Then

            extra23 = Nothing
        End If
        If extra24 <> Nothing Then

            extra24 = Nothing
        End If
        If extra25 <> Nothing Then

            extra25 = Nothing
        End If
        If extra26 <> Nothing Then

            extra26 = Nothing
        End If
        If extra27 <> Nothing Then

            extra27 = Nothing
        End If
        If extra28 <> Nothing Then

            extra28 = Nothing
        End If
        If extra29 <> Nothing Then

            extra29 = Nothing
        End If
        If extra20 <> Nothing Then

            extra20 = Nothing
        End If

        If Form1.Label10.Text <> Nothing Then

            Form1.Label10.Text = 0
        End If

        If Form1.Label11.Text <> Nothing Then

            Form1.Label11.Text = 0
        End If

        If Form1.Label12.Text <> Nothing Then

            Form1.Label12.Text = 0
        End If
        If Form1.Label13.Text <> Nothing Then

            Form1.Label13.Text = 0
        End If
        If Form1.Label23.Text <> Nothing Then

            Form1.Label23.Text = 0
        End If
        If Form1.Label27.Text <> Nothing Then

            Form1.Label27.Text = 0
        End If
        If Form1.Label28.Text <> Nothing Then

            Form1.Label28.Text = 0
        End If

        If Form1.Label30.Text <> Nothing Then

            Form1.Label30.Text = 0
        End If


    End Sub


    Public Shared Sub createfile(filepath As String)

        Try

            ScripFilename = filepath
            If Not File.Exists(ScripFilename) Then
                File.Create(ScripFilename).Dispose()
            End If
            Dim writer As New StreamWriter(ScripFilename)
            comment = "--" & Form1.RichTextBox3.Text
            writer.WriteLine(comment)
            Form1.RichTextBox6.AppendText(comment & vbNewLine)

            If Form1.RadioButton31.Checked = True Then

                aiscript = "require (" + My.Settings.Chars + "ai.ai-debug" + My.Settings.Chars + ")"
                writer.WriteLine(aiscript)
                Form1.RichTextBox6.AppendText(reload & vbNewLine)
            End If

            If Form1.RadioButton32.Checked = True Then

                aiscript = "require (" + My.Settings.Chars + "ai.ai-cheating" + My.Settings.Chars + ")"
                writer.WriteLine(aiscript)
                Form1.RichTextBox6.AppendText(reload & vbNewLine)
            End If

            writer.WriteLine()
            ainm = "Debug.SetAIName(" & My.Settings.Chars & Form1.TextBox42.Text & My.Settings.Chars & ")"
            writer.WriteLine(ainm)
            Form1.RichTextBox6.AppendText(ainm & vbNewLine)

            If Form1.CheckBox1.Checked = False And Form1.CheckBox2.Checked = False Then
                reload = "Debug.ReloadFieldBegin(" & Form1.ComboBox5.SelectedItem & "+" & Form1.ComboBox6.SelectedItem & ")"
                writer.WriteLine(reload)
                Form1.RichTextBox6.AppendText(reload & vbNewLine)

            ElseIf Form1.CheckBox1.Checked = True Then
                reload = "Debug.ReloadFieldBegin(DUEL_ATTACK_FIRST_TURN+DUEL_SIMPLE_AI)"
                writer.WriteLine(reload)
                Form1.RichTextBox6.AppendText(reload & vbNewLine)

            ElseIf Form1.CheckBox2.Checked = True Then
                reload = "Debug.ReloadFieldBegin(DUEL_TEST_MODE+0x80)"
                writer.WriteLine(reload)
                Form1.RichTextBox6.AppendText(reload & vbNewLine)


            End If

            playerlp = "Debug.SetPlayerInfo(0," & Form1.TextBox8.Text & ",0,0)"
            writer.WriteLine(playerlp)
            Form1.RichTextBox6.AppendText(playerlp & vbNewLine)
            enemylp = "Debug.SetPlayerInfo(1," & Form1.TextBox11.Text & ",0,0)"
            writer.WriteLine(enemylp)
            Form1.RichTextBox6.AppendText(enemylp & vbNewLine)
            writer.WriteLine()



            If (monster11 <> Nothing) Then

                If Form1.RadioButton1.Checked And Form1.RadioButton5.Checked Then
                    pos11 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton2.Checked And Form1.RadioButton5.Checked Then

                    pos11 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton2.Checked And Form1.RadioButton6.Checked Then
                    pos11 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster11 = (My.Settings.AddCard & monstercard11 & ",0,0,LOCATION_MZONE,0," & pos11)
                writer.WriteLine(monster11)
                Form1.RichTextBox6.AppendText(monster11 & vbNewLine)
            End If

            If (monster12 <> Nothing) Then

                If Form1.RadioButton3.Checked And Form1.RadioButton23.Checked Then
                    pos12 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton3.Checked And Form1.RadioButton24.Checked Then

                    pos12 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton4.Checked And Form1.RadioButton24.Checked Then
                    pos12 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster12 = (My.Settings.AddCard & monstercard12 & ",0,0,LOCATION_MZONE,1," & pos12)
                writer.WriteLine(monster12)
                Form1.RichTextBox6.AppendText(monster12 & vbNewLine)
            End If

            If (monster13 <> Nothing) Then

                If Form1.RadioButton7.Checked And Form1.RadioButton25.Checked Then
                    pos13 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton7.Checked And Form1.RadioButton26.Checked Then

                    pos13 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton8.Checked And Form1.RadioButton26.Checked Then
                    pos13 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster13 = (My.Settings.AddCard & monstercard13 & ",0,0,LOCATION_MZONE,2," & pos13)
                writer.WriteLine(monster13)
                Form1.RichTextBox6.AppendText(monster13 & vbNewLine)
            End If

            If (monster14 <> Nothing) Then

                If Form1.RadioButton9.Checked And Form1.RadioButton27.Checked Then
                    pos14 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton9.Checked And Form1.RadioButton28.Checked Then

                    pos14 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton10.Checked And Form1.RadioButton28.Checked Then
                    pos14 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster14 = (My.Settings.AddCard & monstercard14 & ",0,0,LOCATION_MZONE,3," & pos14)
                writer.WriteLine(monster14)
                Form1.RichTextBox6.AppendText(monster14 & vbNewLine)
            End If

            If (monster15 <> Nothing) Then

                If Form1.RadioButton11.Checked And Form1.RadioButton29.Checked Then
                    pos15 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton11.Checked And Form1.RadioButton30.Checked Then

                    pos15 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton12.Checked And Form1.RadioButton30.Checked Then
                    pos15 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster15 = (My.Settings.AddCard & monstercard15 & ",0,0,LOCATION_MZONE,4," & pos15)
                writer.WriteLine(monster15)
                Form1.RichTextBox6.AppendText(monster15 & vbNewLine)
            End If

            If (szone11 <> Nothing) Then

                If Form1.RadioButton13.Checked Then
                    poss1 = "POS_FACEUP)"

                ElseIf Form1.RadioButton14.Checked Then

                    poss1 = "POS_FACEDOWN)"

                End If
                szone11 = (My.Settings.AddCard & spellcard11 & ",0,0,LOCATION_SZONE,0," & poss1)
                writer.WriteLine(szone11)
                Form1.RichTextBox6.AppendText(szone11 & vbNewLine)
            End If

            If (szone12 <> Nothing) Then

                If Form1.RadioButton15.Checked Then
                    poss2 = "POS_FACEUP)"

                ElseIf Form1.RadioButton16.Checked Then

                    poss2 = "POS_FACEDOWN)"

                End If
                szone12 = (My.Settings.AddCard & spellcard12 & ",0,0,LOCATION_SZONE,1," & poss2)
                writer.WriteLine(szone12)
                Form1.RichTextBox6.AppendText(szone12 & vbNewLine)
            End If

            If (szone13 <> Nothing) Then

                If Form1.RadioButton17.Checked Then
                    poss3 = "POS_FACEUP)"

                ElseIf Form1.RadioButton18.Checked Then

                    poss3 = "POS_FACEDOWN)"

                End If
                szone13 = (My.Settings.AddCard & spellcard13 & ",0,0,LOCATION_SZONE,2," & poss3)
                writer.WriteLine(szone13)
                Form1.RichTextBox6.AppendText(szone13 & vbNewLine)
            End If

            If (szone14 <> Nothing) Then

                If Form1.RadioButton19.Checked Then
                    poss4 = "POS_FACEUP)"

                ElseIf Form1.RadioButton20.Checked Then

                    poss4 = "POS_FACEDOWN)"

                End If
                szone14 = (My.Settings.AddCard & spellcard14 & ",0,0,LOCATION_SZONE,3," & poss4)
                writer.WriteLine(szone14)
                Form1.RichTextBox6.AppendText(szone14 & vbNewLine)
            End If

            If (szone15 <> Nothing) Then

                If Form1.RadioButton21.Checked Then
                    poss5 = "POS_FACEUP)"

                ElseIf Form1.RadioButton22.Checked Then

                    poss5 = "POS_FACEDOWN)"

                End If
                szone15 = (My.Settings.AddCard & spellcard15 & ",0,0,LOCATION_SZONE,4," & poss5)
                writer.WriteLine(szone15)
                Form1.RichTextBox6.AppendText(szone15 & vbNewLine)
            End If

            If (szone16 <> Nothing) Then

                If Form1.RadioButton65.Checked Then
                    poss6 = "POS_FACEUP)"

                ElseIf Form1.RadioButton66.Checked Then

                    poss6 = "POS_FACEDOWN)"

                End If
                szone16 = (My.Settings.AddCard & spellcard16 & ",0,0,LOCATION_SZONE,5," & poss6)
                writer.WriteLine(szone16)
                Form1.RichTextBox6.AppendText(szone16 & vbNewLine)
            End If

            If (szone17 <> Nothing) Then

                If Form1.RadioButton69.Checked Then
                    poss7 = "POS_FACEUP)"

                ElseIf Form1.RadioButton70.Checked Then

                    poss7 = "POS_FACEDOWN)"

                End If
                szone17 = (My.Settings.AddCard & spellcard17 & ",0,0,LOCATION_SZONE,6," & poss7)
                writer.WriteLine(szone17)
                Form1.RichTextBox6.AppendText(szone17 & vbNewLine)
            End If

            If (szone18 <> Nothing) Then

                If Form1.RadioButton71.Checked Then
                    poss8 = "POS_FACEUP)"

                ElseIf Form1.RadioButton72.Checked Then

                    poss8 = "POS_FACEDOWN)"

                End If
                szone18 = (My.Settings.AddCard & spellcard18 & ",0,0,LOCATION_SZONE,7," & poss8)
                writer.WriteLine(szone18)
                Form1.RichTextBox6.AppendText(szone18 & vbNewLine)
            End If

            If hand11 <> Nothing Then


                hand11 = (My.Settings.AddCard & handcard11 & ",0,0,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand11)
                Form1.RichTextBox6.AppendText(hand11 & vbNewLine)
            End If

            If hand12 <> Nothing Then


                hand12 = (My.Settings.AddCard & handcard12 & ",0,0,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand12)
                Form1.RichTextBox6.AppendText(hand12 & vbNewLine)
            End If

            If hand13 <> Nothing Then


                hand13 = (My.Settings.AddCard & handcard13 & ",0,0,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand13)
                Form1.RichTextBox6.AppendText(hand13 & vbNewLine)
            End If

            If hand14 <> Nothing Then


                hand14 = (My.Settings.AddCard & handcard14 & ",0,0,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand14)
                Form1.RichTextBox6.AppendText(hand14 & vbNewLine)
            End If

            If hand15 <> Nothing Then


                hand15 = (My.Settings.AddCard & handcard15 & ",0,0,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand15)
                Form1.RichTextBox6.AppendText(hand15 & vbNewLine)
            End If

            If deck11 <> Nothing Then


                deck11 = (My.Settings.AddCard & deckcard11 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck11)
                Form1.RichTextBox6.AppendText(deck11 & vbNewLine)
            End If

            If deck12 <> Nothing Then


                deck12 = (My.Settings.AddCard & deckcard12 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck12)
                Form1.RichTextBox6.AppendText(deck12 & vbNewLine)
            End If

            If deck13 <> Nothing Then


                deck13 = (My.Settings.AddCard & deckcard13 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck13)
                Form1.RichTextBox6.AppendText(deck13 & vbNewLine)
            End If

            If deck14 <> Nothing Then


                deck14 = (My.Settings.AddCard & deckcard14 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck14)
                Form1.RichTextBox6.AppendText(deck14 & vbNewLine)
            End If

            If deck15 <> Nothing Then


                deck15 = (My.Settings.AddCard & deckcard15 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck15)
                Form1.RichTextBox6.AppendText(deck15 & vbNewLine)
            End If

            If deck16 <> Nothing Then


                deck16 = (My.Settings.AddCard & deckcard16 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck16)
                Form1.RichTextBox6.AppendText(deck16 & vbNewLine)
            End If

            If deck17 <> Nothing Then


                deck17 = (My.Settings.AddCard & deckcard17 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck17)
                Form1.RichTextBox6.AppendText(deck17 & vbNewLine)
            End If

            If deck18 <> Nothing Then


                deck18 = (My.Settings.AddCard & deckcard18 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck18)
                Form1.RichTextBox6.AppendText(deck18 & vbNewLine)
            End If
            If deck19 <> Nothing Then


                deck19 = (My.Settings.AddCard & deckcard19 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck19)
                Form1.RichTextBox6.AppendText(deck19 & vbNewLine)
            End If
            If deck10 <> Nothing Then


                deck10 = (My.Settings.AddCard & deckcard10 & ",0,0,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck10)
                Form1.RichTextBox6.AppendText(deck10 & vbNewLine)
            End If

            If grave11 <> Nothing Then


                grave11 = (My.Settings.AddCard & gravecard11 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave11)
                Form1.RichTextBox6.AppendText(grave11 & vbNewLine)
            End If

            If grave12 <> Nothing Then


                grave12 = (My.Settings.AddCard & gravecard12 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave12)
                Form1.RichTextBox6.AppendText(grave12 & vbNewLine)
            End If
            If grave13 <> Nothing Then


                grave13 = (My.Settings.AddCard & gravecard13 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave13)
                Form1.RichTextBox6.AppendText(grave13 & vbNewLine)
            End If
            If grave14 <> Nothing Then


                grave14 = (My.Settings.AddCard & gravecard14 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave14)
                Form1.RichTextBox6.AppendText(grave14 & vbNewLine)
            End If
            If grave15 <> Nothing Then


                grave15 = (My.Settings.AddCard & gravecard15 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave15)
                Form1.RichTextBox6.AppendText(grave15 & vbNewLine)
            End If
            If grave16 <> Nothing Then


                grave16 = (My.Settings.AddCard & gravecard16 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave16)
                Form1.RichTextBox6.AppendText(grave16 & vbNewLine)
            End If
            If grave17 <> Nothing Then


                grave17 = (My.Settings.AddCard & gravecard17 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave17)
                Form1.RichTextBox6.AppendText(grave17 & vbNewLine)
            End If

            If grave18 <> Nothing Then


                grave18 = (My.Settings.AddCard & gravecard18 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave18)
                Form1.RichTextBox6.AppendText(grave18 & vbNewLine)
            End If

            If grave19 <> Nothing Then


                grave19 = (My.Settings.AddCard & gravecard19 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave19)
                Form1.RichTextBox6.AppendText(grave19 & vbNewLine)
            End If


            If grave10 <> Nothing Then


                grave10 = (My.Settings.AddCard & gravecard10 & ",0,0,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave10)
                Form1.RichTextBox6.AppendText(grave10 & vbNewLine)
            End If

            If removed11 <> Nothing Then


                removed11 = (My.Settings.AddCard & removedcard11 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed11)
                Form1.RichTextBox6.AppendText(removed11 & vbNewLine)
            End If
            If removed12 <> Nothing Then

                removed12 = (My.Settings.AddCard & removedcard12 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed12)
                Form1.RichTextBox6.AppendText(removed11 & vbNewLine)
            End If
            If removed13 <> Nothing Then


                removed13 = (My.Settings.AddCard & removedcard13 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed13)
                Form1.RichTextBox6.AppendText(removed13 & vbNewLine)
            End If
            If removed14 <> Nothing Then


                removed14 = (My.Settings.AddCard & removedcard14 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed14)
                Form1.RichTextBox6.AppendText(removed14 & vbNewLine)
            End If
            If removed15 <> Nothing Then


                removed15 = (My.Settings.AddCard & removedcard15 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed15)
                Form1.RichTextBox6.AppendText(removed15 & vbNewLine)
            End If
            If removed16 <> Nothing Then


                removed16 = (My.Settings.AddCard & removedcard16 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed16)
                Form1.RichTextBox6.AppendText(removed16 & vbNewLine)
            End If
            If removed17 <> Nothing Then


                removed17 = (My.Settings.AddCard & removedcard17 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed17)
                Form1.RichTextBox6.AppendText(removed17 & vbNewLine)
            End If
            If removed18 <> Nothing Then


                removed18 = (My.Settings.AddCard & removedcard18 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed18)
                Form1.RichTextBox6.AppendText(removed18 & vbNewLine)
            End If
            If removed19 <> Nothing Then


                removed19 = (My.Settings.AddCard & removedcard19 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed19)
                Form1.RichTextBox6.AppendText(removed19 & vbNewLine)
            End If
            If removed10 <> Nothing Then


                removed10 = (My.Settings.AddCard & removedcard10 & ",0,0,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed10)
                Form1.RichTextBox6.AppendText(removed10 & vbNewLine)
            End If

            If extra11 <> Nothing Then


                extra11 = (My.Settings.AddCard & extracard11 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra11)
                Form1.RichTextBox6.AppendText(extra11 & vbNewLine)
            End If
            If extra12 <> Nothing Then


                extra12 = (My.Settings.AddCard & extracard12 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra12)
                Form1.RichTextBox6.AppendText(extra12 & vbNewLine)
            End If
            If extra13 <> Nothing Then


                extra13 = (My.Settings.AddCard & extracard13 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra13)
                Form1.RichTextBox6.AppendText(extra13 & vbNewLine)
            End If
            If extra14 <> Nothing Then


                extra14 = (My.Settings.AddCard & extracard14 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra14)
                Form1.RichTextBox6.AppendText(extra14 & vbNewLine)
            End If
            If extra15 <> Nothing Then


                extra15 = (My.Settings.AddCard & extracard15 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra15)
                Form1.RichTextBox6.AppendText(extra15 & vbNewLine)
            End If
            If extra16 <> Nothing Then


                extra16 = (My.Settings.AddCard & extracard16 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra16)
                Form1.RichTextBox6.AppendText(extra16 & vbNewLine)
            End If
            If extra17 <> Nothing Then

                extra17 = (My.Settings.AddCard & extracard17 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra17)
                Form1.RichTextBox6.AppendText(extra17 & vbNewLine)
            End If
            If extra18 <> Nothing Then

                extra18 = (My.Settings.AddCard & extracard18 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra18)
                Form1.RichTextBox6.AppendText(extra11 & vbNewLine)
            End If
            If extra19 <> Nothing Then

                extra19 = (My.Settings.AddCard & extracard19 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra19)
                Form1.RichTextBox6.AppendText(extra19 & vbNewLine)
            End If
            If extra10 <> Nothing Then

                extra10 = (My.Settings.AddCard & extracard10 & ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra10)
                Form1.RichTextBox6.AppendText(extra10 & vbNewLine)
            End If

            If (monster21 <> Nothing) Then

                If Form1.RadioButton35.Checked And Form1.RadioButton45.Checked Then
                    pos21 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton35.Checked And Form1.RadioButton46.Checked Then

                    pos21 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton36.Checked And Form1.RadioButton46.Checked Then
                    pos21 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster21 = (My.Settings.AddCard & monstercard21 & ",1,1,LOCATION_MZONE,0," & pos21)
                writer.WriteLine(monster21)
                Form1.RichTextBox6.AppendText(monster21 & vbNewLine)
            End If

            If (monster22 <> Nothing) Then

                If Form1.RadioButton37.Checked And Form1.RadioButton47.Checked Then
                    pos22 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton37.Checked And Form1.RadioButton48.Checked Then

                    pos22 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton38.Checked And Form1.RadioButton48.Checked Then
                    pos22 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster22 = (My.Settings.AddCard & monstercard22 & ",1,1,LOCATION_MZONE,1," & pos22)
                writer.WriteLine(monster22)
                Form1.RichTextBox6.AppendText(monster21 & vbNewLine)
            End If

            If (monster23 <> Nothing) Then

                If Form1.RadioButton39.Checked And Form1.RadioButton49.Checked Then
                    pos23 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton39.Checked And Form1.RadioButton50.Checked Then

                    pos23 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton40.Checked And Form1.RadioButton50.Checked Then
                    pos23 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster23 = (My.Settings.AddCard & monstercard23 & ",1,1,LOCATION_MZONE,2," & pos23)
                writer.WriteLine(monster23)
                Form1.RichTextBox6.AppendText(monster21 & vbNewLine)
            End If

            If (monster24 <> Nothing) Then

                If Form1.RadioButton41.Checked And Form1.RadioButton51.Checked Then
                    pos24 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton41.Checked And Form1.RadioButton52.Checked Then

                    pos24 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton42.Checked And Form1.RadioButton52.Checked Then
                    pos24 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster24 = (My.Settings.AddCard & monstercard24 & ",1,1,LOCATION_MZONE,3," & pos24)
                writer.WriteLine(monster24)
                Form1.RichTextBox6.AppendText(monster21 & vbNewLine)
            End If

            If (monster25 <> Nothing) Then

                If Form1.RadioButton43.Checked And Form1.RadioButton53.Checked Then
                    pos25 = "POS_FACEUP_ATTACK)"

                ElseIf Form1.RadioButton43.Checked And Form1.RadioButton54.Checked Then

                    pos25 = "POS_FACEUP_DEFENCE)"
                ElseIf Form1.RadioButton44.Checked And Form1.RadioButton54.Checked Then
                    pos25 = "POS_FACEDOWN_DEFENCE)"

                End If
                monster25 = (My.Settings.AddCard & monstercard25 & ",1,1,LOCATION_MZONE,4," & pos25)
                writer.WriteLine(monster25)
                Form1.RichTextBox6.AppendText(monster25 & vbNewLine)
            End If

            If (szone21 <> Nothing) Then

                If Form1.RadioButton55.Checked Then
                    posz1 = "POS_FACEUP)"

                ElseIf Form1.RadioButton56.Checked Then

                    posz1 = "POS_FACEDOWN)"

                End If
                szone21 = (My.Settings.AddCard & spellcard21 & ",1,1,LOCATION_SZONE,0," & posz1)
                writer.WriteLine(szone21)
                Form1.RichTextBox6.AppendText(szone21 & vbNewLine)
            End If

            If (szone22 <> Nothing) Then

                If Form1.RadioButton57.Checked Then
                    posz2 = "POS_FACEUP)"

                ElseIf Form1.RadioButton58.Checked Then

                    posz2 = "POS_FACEDOWN)"

                End If
                szone22 = (My.Settings.AddCard & spellcard22 & ",1,1,LOCATION_SZONE,1," & posz2)
                writer.WriteLine(szone22)
                Form1.RichTextBox6.AppendText(szone22 & vbNewLine)
            End If

            If (szone23 <> Nothing) Then

                If Form1.RadioButton59.Checked Then
                    posz3 = "POS_FACEUP)"

                ElseIf Form1.RadioButton60.Checked Then

                    posz3 = "POS_FACEDOWN)"

                End If
                szone23 = (My.Settings.AddCard & spellcard23 & ",1,1,LOCATION_SZONE,2," & posz3)
                writer.WriteLine(szone23)
                Form1.RichTextBox6.AppendText(szone23 & vbNewLine)
            End If

            If (szone24 <> Nothing) Then

                If Form1.RadioButton61.Checked Then
                    posz4 = "POS_FACEUP)"

                ElseIf Form1.RadioButton62.Checked Then

                    posz4 = "POS_FACEDOWN)"

                End If
                szone24 = (My.Settings.AddCard & spellcard24 & ",1,1,LOCATION_SZONE,3," & posz4)
                writer.WriteLine(szone24)
                Form1.RichTextBox6.AppendText(szone24 & vbNewLine)
            End If

            If (szone25 <> Nothing) Then

                If Form1.RadioButton63.Checked Then
                    posz5 = "POS_FACEUP)"

                ElseIf Form1.RadioButton64.Checked Then

                    posz5 = "POS_FACEDOWN)"

                End If
                szone25 = (My.Settings.AddCard & spellcard25 & ",1,1,LOCATION_SZONE,4," & posz5)
                writer.WriteLine(szone25)
                Form1.RichTextBox6.AppendText(szone25 & vbNewLine)
            End If

            If (szone26 <> Nothing) Then

                If Form1.RadioButton67.Checked Then
                    posz6 = "POS_FACEUP)"

                ElseIf Form1.RadioButton68.Checked Then

                    posz6 = "POS_FACEDOWN)"

                End If
                szone26 = (My.Settings.AddCard & spellcard26 & ",1,1,LOCATION_SZONE,5," & posz6)
                writer.WriteLine(szone26)
                Form1.RichTextBox6.AppendText(szone26 & vbNewLine)
            End If


            If (szone27 <> Nothing) Then

                If Form1.RadioButton73.Checked Then
                    posz7 = "POS_FACEUP)"

                ElseIf Form1.RadioButton74.Checked Then

                    posz7 = "POS_FACEDOWN)"

                End If
                szone27 = (My.Settings.AddCard & spellcard27 & ",1,1,LOCATION_SZONE,6," & posz7)
                writer.WriteLine(szone27)
                Form1.RichTextBox6.AppendText(szone27 & vbNewLine)
            End If

            If (szone28 <> Nothing) Then

                If Form1.RadioButton75.Checked Then
                    posz8 = "POS_FACEUP)"

                ElseIf Form1.RadioButton76.Checked Then

                    posz8 = "POS_FACEDOWN)"

                End If
                szone28 = (My.Settings.AddCard & spellcard28 & ",1,1,LOCATION_SZONE,7," & posz8)
                writer.WriteLine(szone28)
                Form1.RichTextBox6.AppendText(szone28 & vbNewLine)
            End If

            If hand21 <> Nothing Then


                hand21 = (My.Settings.AddCard & handcard21 & ",1,1,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand21)
                Form1.RichTextBox6.AppendText(hand21 & vbNewLine)
            End If

            If hand22 <> Nothing Then


                hand22 = (My.Settings.AddCard & handcard22 & ",1,1,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand22)
                Form1.RichTextBox6.AppendText(hand22 & vbNewLine)
            End If

            If hand23 <> Nothing Then

                hand23 = (My.Settings.AddCard & handcard23 & ",1,1,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand23)
                Form1.RichTextBox6.AppendText(hand23 & vbNewLine)
            End If

            If hand24 <> Nothing Then


                hand24 = (My.Settings.AddCard & handcard24 & ",1,1,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand24)
                Form1.RichTextBox6.AppendText(hand24 & vbNewLine)
            End If

            If hand25 <> Nothing Then


                hand25 = (My.Settings.AddCard & handcard25 & ",1,1,LOCATION_HAND,0,POS_FACEUP)")
                writer.WriteLine(hand25)
                Form1.RichTextBox6.AppendText(hand25 & vbNewLine)
            End If

            If deck21 <> Nothing Then


                deck21 = (My.Settings.AddCard & deckcard21 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck21)
                Form1.RichTextBox6.AppendText(deck21 & vbNewLine)
            End If

            If deck22 <> Nothing Then


                deck22 = (My.Settings.AddCard & deckcard22 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck22)
                Form1.RichTextBox6.AppendText(deck22 & vbNewLine)
            End If

            If deck23 <> Nothing Then


                deck23 = (My.Settings.AddCard & deckcard23 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck23)
                Form1.RichTextBox6.AppendText(deck23 & vbNewLine)
            End If

            If deck24 <> Nothing Then


                deck24 = (My.Settings.AddCard & deckcard24 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck24)
                Form1.RichTextBox6.AppendText(deck24 & vbNewLine)
            End If

            If deck25 <> Nothing Then


                deck25 = (My.Settings.AddCard & deckcard25 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck25)
                Form1.RichTextBox6.AppendText(deck25 & vbNewLine)
            End If
            If deck26 <> Nothing Then


                deck26 = (My.Settings.AddCard & deckcard26 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck26)
                Form1.RichTextBox6.AppendText(deck26 & vbNewLine)
            End If
            If deck27 <> Nothing Then


                deck27 = (My.Settings.AddCard & deckcard27 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck27)
                Form1.RichTextBox6.AppendText(deck21 & vbNewLine)
            End If
            If deck28 <> Nothing Then


                deck28 = (My.Settings.AddCard & deckcard28 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck28)
                Form1.RichTextBox6.AppendText(deck28 & vbNewLine)
            End If
            If deck29 <> Nothing Then


                deck29 = (My.Settings.AddCard & deckcard29 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck29)
                Form1.RichTextBox6.AppendText(deck29 & vbNewLine)
            End If
            If deck20 <> Nothing Then


                deck20 = (My.Settings.AddCard & deckcard20 & ",1,1,LOCATION_DECK,0,POS_FACEUP)")
                writer.WriteLine(deck20)
                Form1.RichTextBox6.AppendText(deck20 & vbNewLine)
            End If

            If grave21 <> Nothing Then


                grave21 = (My.Settings.AddCard & gravecard21 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave21)
                Form1.RichTextBox6.AppendText(grave21 & vbNewLine)
            End If

            If grave22 <> Nothing Then


                grave22 = (My.Settings.AddCard & gravecard22 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave22)
                Form1.RichTextBox6.AppendText(grave22 & vbNewLine)
            End If
            If grave23 <> Nothing Then


                grave23 = (My.Settings.AddCard & gravecard23 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave23)
                Form1.RichTextBox6.AppendText(grave23 & vbNewLine)
            End If
            If grave24 <> Nothing Then


                grave24 = (My.Settings.AddCard & gravecard24 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave24)
                Form1.RichTextBox6.AppendText(grave24 & vbNewLine)
            End If
            If grave25 <> Nothing Then


                grave25 = (My.Settings.AddCard & gravecard25 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave25)
                Form1.RichTextBox6.AppendText(grave25 & vbNewLine)
            End If
            If grave26 <> Nothing Then


                grave26 = (My.Settings.AddCard & gravecard26 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave26)
                Form1.RichTextBox6.AppendText(grave26 & vbNewLine)
            End If
            If grave27 <> Nothing Then


                grave27 = (My.Settings.AddCard & gravecard27 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave27)
                Form1.RichTextBox6.AppendText(grave27 & vbNewLine)
            End If
            If grave28 <> Nothing Then


                grave28 = (My.Settings.AddCard & gravecard28 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave28)
                Form1.RichTextBox6.AppendText(grave28 & vbNewLine)
            End If
            If grave29 <> Nothing Then


                grave29 = (My.Settings.AddCard & gravecard29 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave29)
                Form1.RichTextBox6.AppendText(grave29 & vbNewLine)
            End If
            If grave20 <> Nothing Then


                grave20 = (My.Settings.AddCard & gravecard20 & ",1,1,LOCATION_GRAVE,0,POS_FACEUP)")
                writer.WriteLine(grave20)
                Form1.RichTextBox6.AppendText(grave20 & vbNewLine)
            End If

            If removed21 <> Nothing Then


                removed21 = (My.Settings.AddCard & removedcard21 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed21)
                Form1.RichTextBox6.AppendText(removed21 & vbNewLine)
            End If
            If removed22 <> Nothing Then


                removed22 = (My.Settings.AddCard & removedcard22 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed22)
                Form1.RichTextBox6.AppendText(removed21 & vbNewLine)
            End If
            If removed23 <> Nothing Then


                removed23 = (My.Settings.AddCard & removedcard23 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed23)
                Form1.RichTextBox6.AppendText(removed23 & vbNewLine)
            End If
            If removed24 <> Nothing Then


                removed24 = (My.Settings.AddCard & removedcard24 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed24)
                Form1.RichTextBox6.AppendText(removed24 & vbNewLine)
            End If
            If removed25 <> Nothing Then


                removed25 = (My.Settings.AddCard & removedcard25 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed25)
                Form1.RichTextBox6.AppendText(removed25 & vbNewLine)
            End If
            If removed26 <> Nothing Then


                removed26 = (My.Settings.AddCard & removedcard26 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed26)
                Form1.RichTextBox6.AppendText(removed26 & vbNewLine)
            End If
            If removed27 <> Nothing Then


                removed27 = (My.Settings.AddCard & removedcard27 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed27)
                Form1.RichTextBox6.AppendText(removed27 & vbNewLine)
            End If
            If removed28 <> Nothing Then


                removed28 = (My.Settings.AddCard & removedcard28 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed28)
                Form1.RichTextBox6.AppendText(removed28 & vbNewLine)
            End If
            If removed29 <> Nothing Then


                removed29 = (My.Settings.AddCard & removedcard29 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed29)
                Form1.RichTextBox6.AppendText(removed29 & vbNewLine)
            End If
            If removed20 <> Nothing Then


                removed20 = (My.Settings.AddCard & removedcard20 & ",1,1,LOCATION_REMOVED,0,POS_FACEUP)")
                writer.WriteLine(removed20)
                Form1.RichTextBox6.AppendText(removed20 & vbNewLine)
            End If

            If extra21 <> Nothing Then


                extra21 = (My.Settings.AddCard & extracard21 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra21)
                Form1.RichTextBox6.AppendText(extra21 & vbNewLine)
            End If
            If extra22 <> Nothing Then


                extra22 = (My.Settings.AddCard & extracard22 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra22)
                Form1.RichTextBox6.AppendText(extra22 & vbNewLine)
            End If
            If extra23 <> Nothing Then


                extra23 = (My.Settings.AddCard & extracard23 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra23)
                Form1.RichTextBox6.AppendText(extra23 & vbNewLine)
            End If
            If extra24 <> Nothing Then


                extra24 = (My.Settings.AddCard & extracard24 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra24)
                Form1.RichTextBox6.AppendText(extra24 & vbNewLine)
            End If
            If extra25 <> Nothing Then


                extra25 = (My.Settings.AddCard & extracard25 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra25)
                Form1.RichTextBox6.AppendText(extra25 & vbNewLine)
            End If
            If extra26 <> Nothing Then


                extra26 = (My.Settings.AddCard & extracard26 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra26)
                Form1.RichTextBox6.AppendText(extra26 & vbNewLine)
            End If
            If extra27 <> Nothing Then


                extra27 = (My.Settings.AddCard & extracard27 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra27)
                Form1.RichTextBox6.AppendText(extra27 & vbNewLine)
            End If
            If extra28 <> Nothing Then


                extra28 = (My.Settings.AddCard & extracard28 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra28)
                Form1.RichTextBox6.AppendText(extra28 & vbNewLine)
            End If
            If extra29 <> Nothing Then


                extra29 = (My.Settings.AddCard & extracard29 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra29)
                Form1.RichTextBox6.AppendText(extra29 & vbNewLine)
            End If
            If extra20 <> Nothing Then


                extra20 = (My.Settings.AddCard & extracard20 & ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)")
                writer.WriteLine(extra20)
                Form1.RichTextBox6.AppendText(extra20 & vbNewLine)
            End If

            If Form1.RadioButton31.Checked = True Then
                writer.WriteLine("c1=Debug.AddCard(0, 0, 0, LOCATION_REMOVED, 0, POS_FACEDOWN)")

            ElseIf Form1.RadioButton32.Checked Then
                writer.WriteLine("c1=Debug.AddCard(0, 0, 0, LOCATION_REMOVED, 0, POS_FACEDOWN)")
            End If


            reload2 = "Debug.ReloadFieldEnd()"
            writer.WriteLine(reload2)
            Form1.RichTextBox6.AppendText(reload2 & vbNewLine)

            If Not Form1.TextBox12.Text = Nothing Then

                HINT1 = "Debug.ShowHint(" & My.Settings.Chars & Form1.TextBox12.Text & My.Settings.Chars & ")"
                writer.WriteLine(HINT1)
                Form1.RichTextBox6.AppendText(HINT1 & vbNewLine)
            End If

            If Not Form1.TextBox13.Text = Nothing Then
                HINT2 = "Debug.ShowHint(" & My.Settings.Chars & Form1.TextBox13.Text & My.Settings.Chars & ")"
                writer.WriteLine(HINT2)
                Form1.RichTextBox6.AppendText(HINT2 & vbNewLine)
            End If



            If Form1.RadioButton33.Checked Then
                aux1 = "Duel.GetTurnCount()"
                writer.WriteLine(aux1)


                Form1.RichTextBox6.AppendText(aux1 & vbNewLine)

            ElseIf Form1.RadioButton34.Checked Then

                aux2 = "aux.BeginPuzzle()"
                writer.WriteLine(aux2)

                Form1.RichTextBox6.AppendText(aux2 & vbNewLine)


            End If

            If Form1.RadioButton31.Checked = True Then
                aicommand = "OnStartOfDuel()"
                writer.WriteLine(aicommand)
                writer.Write(Form1.RichTextBox4.Text)
                Form1.RichTextBox6.AppendText(aux1 & vbNewLine)

            ElseIf Form1.RadioButton32.Checked Then
                aicommand = "OnStartOfDuel()"
                writer.WriteLine(aicommand)
                writer.Write(Form1.RichTextBox4.Text)
                Form1.RichTextBox6.AppendText(aux1 & vbNewLine)
            End If


            writer.Close()


            Interaction.MsgBox(ScripFilename & vbNewLine & "File created", MsgBoxStyle.ApplicationModal, Nothing)
        Catch exception1 As Exception

            Dim exception As Exception = exception1
            Interaction.MsgBox("File cannot be created", MsgBoxStyle.ApplicationModal, Nothing)

        End Try
    End Sub
End Class
