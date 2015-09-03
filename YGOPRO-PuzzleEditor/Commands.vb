Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.Windows.Forms.ListViewItem
Imports YGOPRO_PuzzleEditor.Enums


Public Class Commands
    Public Shared cardselected As String
    Public Shared idselected As String
    Public Shared itemselect As ToolStripMenuItem

    Public Shared Sub SelectFromName()
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type, B.race, B.level FROM texts A, datas B WHERE (A.id= B.id) AND (name LIKE @name)"
        Dim search As String = "%" + Form1.TextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@name", search)
        Dim ds As DataSet = New DataSet()
        con.Open()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(ds, "texts")
        Form1.DataGridView1.DataSource = ds
        Form1.DataGridView1.DataMember = "texts"
        Dim source As New BindingSource
        source.DataSource = ds
        source.DataMember = "texts"
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file & current & ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("level").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
        Form1.DataGridView1.Columns.Item("race").Visible = False

    End Sub

    Public Shared Sub SelectFromDescription()
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type, B.race, B.level FROM texts A, datas B WHERE (A.id= B.id) AND (desc LIKE @desc)"
        Dim search As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@desc", search)
        Dim ds As DataSet = New DataSet()
        con.Open()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(ds, "texts")
        Form1.DataGridView1.DataSource = ds
        Form1.DataGridView1.DataMember = "texts"
        Dim source As New BindingSource
        source.DataSource = ds
        source.DataMember = "texts"
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file & current & ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("level").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
        Form1.DataGridView1.Columns.Item("race").Visible = False

    End Sub

    Public Shared Sub BindList()
        Dim search As String = "%" + Form1.TextBox14.Text + "%"
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim ds As DataSet = New DataSet("texts")
        Dim strSQL As String = "SELECT id, name, desc FROM texts WHERE name LIKE '" & search & "'"
        Dim da As New SQLiteDataAdapter(strSQL, con)
        da.Fill(ds, "texts")
        Dim dt As New DataTable()
        dt = ds.Tables(0)
        Form1.ListView1.View = View.Details
        Form1.ListView1.GridLines = True
        Form1.ListView1.Columns.Add("id", 100, HorizontalAlignment.Left)
        Form1.ListView1.Columns.Add("name", 150, HorizontalAlignment.Left)
        Form1.ListView1.Columns.Add("desc", 200, HorizontalAlignment.Left)
        For Each dr As DataRow In dt.Rows
            Dim lvi As New ListViewItem(dr("id").ToString())
            lvi.Tag = dr
            Form1.ListView1.Items.Add(lvi)
            For i As Integer = 1 To 2

                lvi.SubItems.Add(If(dr(i) IsNot Nothing, dr(i).ToString, ""))
            Next
        Next
    End Sub

    Public Shared Sub CurrentItem()
        Form1.Label5.DataBindings.Clear()
        Form1.Label6.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim ds As DataSet = New DataSet()

        Dim da As New SQLiteDataAdapter("SELECT * FROM texts", con)
        da.Fill(ds, "texts")
        Form1.ComboBox1.DataSource = System.Enum.GetValues(GetType(CardType))

    End Sub

    Public Shared Sub SelectforTypeMonster()
        Dim monstertype As MonsterCategory = CType(Form1.ComboBox2.SelectedValue, MonsterCategory)
        Form1.TextBox2.Text = monstertype
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type FROM texts A, datas B WHERE (A.id= B.id)  AND (B.type = @type) AND (A.name LIKE @name) AND (A.desc LIKE @desc)"
        Dim num As String = Form1.TextBox2.Text
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", num)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file & current & ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
    End Sub

    Public Shared Sub SelectforMonsterRace()
        Dim monsterrace As Races = CType(Form1.ComboBox4.SelectedValue, Races)
        Form1.TextBox2.Text = monsterrace
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.race FROM texts A, datas B WHERE (A.id= B.id)  AND (B.race = @race) AND (A.name LIKE @name) AND (A.desc LIKE @desc)"
        Dim num As String = Form1.TextBox2.Text
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@race", num)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file & current & ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("race").Visible = False
    End Sub
    Public Shared Sub SelectforMonsterLevel()
        Dim level As Integer = Convert.ToInt32(Form1.ComboBox3.SelectedItem)
        Form1.TextBox6.Text = level
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.level FROM texts A, datas B WHERE (A.id= B.id)  AND (B.level = @level) AND (A.name LIKE @name) AND (A.desc LIKE @desc)"
        Dim num As String = Form1.TextBox6.Text
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@level", num)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file & current & ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("level").Visible = False
    End Sub


    Public Shared Sub SelectforTypeSpell()
        Dim spelltype As SpellCategory = CType(Form1.ComboBox2.SelectedValue, SpellCategory)
        Form1.TextBox2.Text = spelltype
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.level, B.category, B.type FROM texts A, datas B WHERE (A.id= B.id)  AND (B.type = @type)  AND (A.name LIKE @name) AND (A.desc LIKE @desc)"
        Dim num As String = Form1.TextBox2.Text
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", num)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
    End Sub

    Public Shared Sub SelectforTypeTrap()
        Dim traptype As TrapCategory = CType(Form1.ComboBox2.SelectedValue, TrapCategory)
        Form1.TextBox2.Text = traptype
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.level, B.category, B.type FROM texts A, datas B WHERE (A.id= B.id)  AND (B.type = @type) AND (A.name LIKE @name) AND (A.desc LIKE @desc)"
        Dim num As String = Form1.TextBox2.Text
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", num)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
    End Sub

    Public Shared Sub SelectFromMonster()
        Dim monstertype As Array = System.Enum.GetValues(GetType(MonsterCategory))

        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type FROM texts A, datas B WHERE (A.id= B.id)  AND (B.type = @type)"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", monstertype)
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

        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False

    End Sub

    Public Shared Sub SearchforLevel()
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type, B.race, B.level FROM texts A, datas B WHERE (A.id= B.id) AND (B.level = @level) AND (A.name LIKE @name) AND (A.desc LIKE @desc) AND (B.race = @race) AND (B.type = @type)"
        Dim type As String = Form1.TextBox2.Text
        Dim search As String = Form1.ComboBox3.SelectedItem
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim race As String = Form1.TextBox5.Text
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", type)
        sql.Parameters.AddWithValue("@race", race)
        sql.Parameters.AddWithValue("@level", search)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
        Dim ds As DataSet = New DataSet()
        con.Open()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(ds, "texts")
        Form1.DataGridView1.DataSource = ds
        Form1.DataGridView1.DataMember = "texts"
        Dim source As New BindingSource
        source.DataSource = ds
        source.DataMember = "texts"
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("level").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
        Form1.DataGridView1.Columns.Item("race").Visible = False

    End Sub

    Public Shared Sub SearchforRace()
        Dim race As Races = CType(Form1.ComboBox4.SelectedValue, Races)
        Form1.TextBox5.Text = race
        Form1.TextBox7.Text = CType(Form1.ComboBox4.SelectedValue, Races).ToString
        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT A.id, A.name, A.desc, B.type, B.race, B.level FROM texts A, datas B WHERE (A.id= B.id) AND (B.race = @race) AND (A.name LIKE @name) AND (A.desc LIKE @desc) AND (B.type = @type)"
        Dim type As String = Form1.TextBox2.Text
        Dim search As String = Form1.TextBox5.Text
        Dim level As String = Form1.ComboBox3.SelectedItem
        Dim name As String = "%" + Form1.TextBox1.Text + "%"
        Dim desc As String = "%" + Form1.RichTextBox1.Text + "%"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@type", type)
        sql.Parameters.AddWithValue("@race", search)
        sql.Parameters.AddWithValue("@name", name)
        sql.Parameters.AddWithValue("@desc", desc)
        Dim ds As DataSet = New DataSet()
        con.Open()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(ds, "texts")
        Form1.DataGridView1.DataSource = ds
        Form1.DataGridView1.DataMember = "texts"
        Dim source As New BindingSource
        source.DataSource = ds
        source.DataMember = "texts"
        Dim bind1 As New Binding("text", source, "id")
        Form1.TextBox3.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Form1.TextBox4.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Form1.RichTextBox2.DataBindings.Add(bind3)
        con.Close()
        Dim current As String = Form1.TextBox3.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\"
        Form1.PictureBox1.BackgroundImage = Bitmap.FromFile(file + current + ".jpg")
        Form1.DataGridView1.Columns.Item("id").Visible = False
        Form1.DataGridView1.Columns.Item("desc").Visible = False
        Form1.DataGridView1.Columns.Item("level").Visible = False
        Form1.DataGridView1.Columns.Item("type").Visible = False
        Form1.DataGridView1.Columns.Item("race").Visible = False
    End Sub


    Public Shared Sub SearchbyCard()


        Form1.TextBox3.DataBindings.Clear()
        Form1.TextBox4.DataBindings.Clear()
        Form1.RichTextBox2.DataBindings.Clear()
        Dim constring As String = "data source=" & My.Settings.Dabatase
        Dim con As SQLiteConnection = New SQLiteConnection(constring)
        Dim query As String
        query = "SELECT id, name, desc FROM texts WHERE (id = @id)"
        Dim sql As SQLiteCommand = New SQLiteCommand(query, con)
        sql.Parameters.AddWithValue("@id", idselected)
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


End Class
