Imports System.Data.SQLite

Public Class CardDatabase
    Private connection As SQLiteConnection


    Private Sub CardDatabase_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim path As String
        path = (My.Settings.GameDirectory & "\cards.cdb")
        Dim constring As String = "data source=" & My.Settings.Dabatase
        connection = New SQLiteConnection(constring)
        Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
        Dim ds As DataSet = New DataSet()
        Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
        connection.Open()
        Dim table As New DataTable()
        DataAdapter1.SelectCommand = sql
        DataAdapter1.Fill(table)
        Dim source As New BindingSource
        source.DataSource = table
        DataGridView1.DataSource = source
        Dim bind1 As New Binding("text", source, "id")
        Me.TextBox1.DataBindings.Add(bind1)
        Dim bind2 As New Binding("text", source, "name")
        Me.TextBox2.DataBindings.Add(bind2)
        Dim bind3 As New Binding("text", source, "desc")
        Me.RichTextBox1.DataBindings.Add(bind3)
        connection.Close()

        Me.TextBox3.Text = path

    End Sub


    Private Sub TextBox2_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox2.TextChanged
        Dim current As String = Me.TextBox1.Text
        Dim file As String = My.Settings.GameDirectory & "\pics\thumbnail\"
        Me.PictureBox1.ImageLocation = file & current & ".jpg"


    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        Me.TextBox1.DataBindings.Clear()
        Me.TextBox2.DataBindings.Clear()
        Me.RichTextBox1.DataBindings.Clear()


        Me.OpenFileDialog1.InitialDirectory = My.Settings.GameDirectory
        Me.OpenFileDialog1.Filter = "Card Database (*.cdb)|*.cdb|All Files (*.*)|*.*"
        Me.OpenFileDialog1.FilterIndex = 1
        If Me.OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Dim file As String
            file = Me.OpenFileDialog1.FileName
            Dim constring As String = "data source=" & file
            connection = New SQLiteConnection(constring)
            Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
            Dim ds As DataSet = New DataSet()
            Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection.Open()
            Dim table As New DataTable()
            DataAdapter1.SelectCommand = sql
            DataAdapter1.Fill(table)
            Dim source As New BindingSource
            source.DataSource = table
            DataGridView1.DataSource = source
            Dim bind1 As New Binding("text", source, "id")
            Me.TextBox1.DataBindings.Add(bind1)
            Dim bind2 As New Binding("text", source, "name")
            Me.TextBox2.DataBindings.Add(bind2)
            Dim bind3 As New Binding("text", source, "desc")
            Me.RichTextBox1.DataBindings.Add(bind3)
            connection.Close()

            Me.TextBox3.Text = file

        End If

    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Me.TextBox1.DataBindings.Clear()
        Me.TextBox2.DataBindings.Clear()
        Me.RichTextBox1.DataBindings.Clear()


        Me.SaveFileDialog1.InitialDirectory = My.Settings.GameDirectory
        Me.SaveFileDialog1.Filter = "Card Database (*.cdb)|*.cdb|All Files (*.*)|*.*"
        Me.SaveFileDialog1.FilterIndex = 1
        If Me.SaveFileDialog1.ShowDialog = DialogResult.OK Then

            Try
                Dim file As String = Me.TextBox3.Text

                My.Computer.FileSystem.CopyFile(file, SaveFileDialog1.FileName)

                MsgBox("Database was saved")
            Catch ex As Exception
                MsgBox("Cannot save the database")
            End Try
            


        End If
    End Sub

    Private Sub SpanishToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SpanishToolStripMenuItem.Click
        Me.TextBox1.DataBindings.Clear()
        Me.TextBox2.DataBindings.Clear()
        Me.RichTextBox1.DataBindings.Clear()

        Try
            Dim file As String
            file = My.Settings.GameDirectory & "\language\es\cards.cdb"
            Dim constring As String = "data source=" & file
            connection = New SQLiteConnection(constring)
            Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
            Dim ds As DataSet = New DataSet()
            Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection.Open()
            Dim table As New DataTable()
            DataAdapter1.SelectCommand = sql
            DataAdapter1.Fill(table)
            Dim source As New BindingSource
            source.DataSource = table
            DataGridView1.DataSource = source
            Dim bind1 As New Binding("text", source, "id")
            Me.TextBox1.DataBindings.Add(bind1)
            Dim bind2 As New Binding("text", source, "name")
            Me.TextBox2.DataBindings.Add(bind2)
            Dim bind3 As New Binding("text", source, "desc")
            Me.RichTextBox1.DataBindings.Add(bind3)
            connection.Close()
        Catch ex As Exception
            MsgBox("Cannot find spanish database")
        End Try
        
    End Sub

    Private Sub DeutchsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeutchsToolStripMenuItem.Click
        Me.TextBox1.DataBindings.Clear()
        Me.TextBox2.DataBindings.Clear()
        Me.RichTextBox1.DataBindings.Clear()

        Try
            Dim file As String
            file = My.Settings.GameDirectory & "\language\de\cards.cdb"
            Dim constring As String = "data source=" & file
            connection = New SQLiteConnection(constring)
            Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
            Dim ds As DataSet = New DataSet()
            Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection.Open()
            Dim table As New DataTable()
            DataAdapter1.SelectCommand = sql
            DataAdapter1.Fill(table)
            Dim source As New BindingSource
            source.DataSource = table
            DataGridView1.DataSource = source
            Dim bind1 As New Binding("text", source, "id")
            Me.TextBox1.DataBindings.Add(bind1)
            Dim bind2 As New Binding("text", source, "name")
            Me.TextBox2.DataBindings.Add(bind2)
            Dim bind3 As New Binding("text", source, "desc")
            Me.RichTextBox1.DataBindings.Add(bind3)
            connection.Close()
        Catch ex As Exception
            MsgBox("Cannot find deutchs database")
        End Try
    End Sub

    Private Sub OriginalToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OriginalToolStripMenuItem.Click
        Me.TextBox1.DataBindings.Clear()
        Me.TextBox2.DataBindings.Clear()
        Me.RichTextBox1.DataBindings.Clear()

        Try
            Dim file As String
            file = My.Settings.GameDirectory & "\cards.cdb"
            Dim constring As String = "data source=" & file
            connection = New SQLiteConnection(constring)
            Dim sql As SQLiteCommand = New SQLiteCommand("SELECT A.id, A.name, A.desc, B.level, B.type, b.race FROM texts A, datas B WHERE A.id= B.id", connection)
            Dim ds As DataSet = New DataSet()
            Dim DataAdapter1 As SQLiteDataAdapter = New SQLiteDataAdapter()
            connection.Open()
            Dim table As New DataTable()
            DataAdapter1.SelectCommand = sql
            DataAdapter1.Fill(table)
            Dim source As New BindingSource
            source.DataSource = table
            DataGridView1.DataSource = source
            Dim bind1 As New Binding("text", source, "id")
            Me.TextBox1.DataBindings.Add(bind1)
            Dim bind2 As New Binding("text", source, "name")
            Me.TextBox2.DataBindings.Add(bind2)
            Dim bind3 As New Binding("text", source, "desc")
            Me.RichTextBox1.DataBindings.Add(bind3)
            connection.Close()
        Catch ex As Exception
            MsgBox("Cannot find deutchs database")
        End Try
    End Sub
End Class