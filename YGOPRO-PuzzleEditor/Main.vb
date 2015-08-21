Imports MySql.Data.MySqlClient

Public Class Main

    Private Sub Main_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim con As MySqlConnection = New MySqlConnection("server=www.db4free.net;User Id=francot514;password=mario110c;database=ygopro;Persist Security Info=True")
        Try
            con.Open()
        Catch ex As Exception
            MsgBox("You cannot connect")
        End Try
        Dim query As String
        query = "SELECT username, password FROM profiles WHERE username='" & TextBox1.Text & " ' AND password ='" & TextBox3.Text & " '"

        Dim sql As MySqlCommand = New MySqlCommand(query, con)
        Dim ds As DataSet = New DataSet()
        Dim DataAdapter1 As MySqlDataAdapter = New MySqlDataAdapter()

        Dim mydata As MySqlDataReader
        mydata = sql.ExecuteReader
        If mydata.HasRows = 0 Then
            MsgBox("User or password are not correct")
        Else
            MsgBox("Sucessfully logged in")
            Me.Hide()
            Form1.Show()

            My.Settings.Username = TextBox1.Text
            My.Settings.Password = TextBox2.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim constring = "server=www.db4free.net;User Id=francot514;password=mario110c;database=ygopro;Persist Security Info=True"

        Try
            Dim iReturn As Boolean
            Using SQLConnection As New MySqlConnection(constring)
                Using sqlCommand As New MySqlCommand()
                    With sqlCommand
                        .CommandText = "INSERT INTO accounts (`id`, `username`, `password`, `email`, `computer`) values (@id,@username,@password,@email,@computer)"
                        .Connection = SQLConnection
                        .CommandType = CommandType.Text
                        .Parameters.AddWithValue("@id", TextBox4.Text)
                        .Parameters.AddWithValue("@username", TextBox1.Text)
                        .Parameters.AddWithValue("@password", TextBox3.Text)
                        .Parameters.AddWithValue("@email", TextBox2.Text)
                        .Parameters.AddWithValue("@computer", My.Computer.Name)

                    End With

                    Try
                        SQLConnection.Open()
                        sqlCommand.ExecuteNonQuery()
                        iReturn = True
                        MsgBox("Account created")
                    Catch ex As MySqlException
                        MsgBox(ex.Message.ToString)
                        iReturn = False
                        MsgBox("Account cannot be created")
                    Finally
                        SQLConnection.Close()
                    End Try
                End Using
            End Using

            Return


        Catch ex As Exception
            MsgBox("Account information not correct")
        End Try
    End Sub
End Class