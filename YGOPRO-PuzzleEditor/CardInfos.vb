
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports YGOPRO_PuzzleEditor.Enums


Namespace YGOPro_Launcher.CardDatabase

    Public Class CardInfos


        Public Sub New(ByVal carddata As String())
            Id = Int32.Parse(carddata(0))
            Ot = Int32.Parse(carddata(1))
            AliasId = Int32.Parse(carddata(2))
            SetCode = Int32.Parse(carddata(3))
            Type = Int32.Parse(carddata(4))
            Level = Int32.Parse(carddata(5))
            Race = Int32.Parse(carddata(6))
            Attribute = Int32.Parse(carddata(7))
            Atk = Int32.Parse(carddata(8))
            Def = Int32.Parse(carddata(9))
            Category = Int32.Parse(carddata(10))
        End Sub

        Public Sub SetCardText(ByVal cardtext As String())
            Name = cardtext(1)
            Description = cardtext(2)
            Dim effects As New List(Of String)()

            For i As Integer = 3 To cardtext.Length - 1
                If cardtext(i) <> "" Then
                    effects.Add(cardtext(i))
                End If
            Next
            EffectStrings = effects.ToArray()

        End Sub

        Public Function GetCardTypes() As CardType()
            Dim types As New List(Of CardType)()
            Dim typeArray = [Enum].GetValues(GetType(CardType))
            For Each type As CardType In typeArray
                If ((Me.Type And CInt(type)) <> 0) Then
                    types.Add(type)
                End If
            Next
            Return types.ToArray()
        End Function

        Public Shared Function GetCardTypes(ByVal Type__1 As Integer) As CardType()
            Dim types As New List(Of CardType)()
            Dim typeArray = [Enum].GetValues(GetType(CardType))
            For Each type__2 As CardType In typeArray
                If ((Type__1 And CInt(type__2)) <> 0) Then
                    types.Add(type__2)
                End If
            Next
            Return types.ToArray()
        End Function
        Public Function GetCardSets(ByVal setArray As List(Of Integer)) As Integer()
            Dim sets As New List(Of Integer)()

            sets.Add(setArray.IndexOf(SetCode And &HFFFF))
            sets.Add(setArray.IndexOf(SetCode >> &H10))

            Return sets.ToArray()
        End Function

        Public Function Clone() As Object
            Return DirectCast(Me.MemberwiseClone(), CardInfos)
        End Function

        Public Property AliasId() As Integer
            Get
                Return m_AliasId
            End Get
            Set(ByVal value As Integer)
                m_AliasId = value
            End Set
        End Property
        Private m_AliasId As Integer

        Public Property Atk() As Integer
            Get
                Return m_Atk
            End Get
            Set(ByVal value As Integer)
                m_Atk = value
            End Set
        End Property
        Private m_Atk As Integer

        Public Property Attribute() As Integer
            Get
                Return m_Attribute
            End Get
            Set(ByVal value As Integer)
                m_Attribute = value
            End Set
        End Property
        Private m_Attribute As Integer

        Public Property Def() As Integer
            Get
                Return m_Def
            End Get
            Set(ByVal value As Integer)
                m_Def = value
            End Set
        End Property
        Private m_Def As Integer

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
        Private m_Description As String

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Private Set(ByVal value As Integer)
                m_Id = value
            End Set
        End Property
        Private m_Id As Integer

        Public Property Level() As Integer
            Get
                Return m_Level
            End Get
            Set(ByVal value As Integer)
                m_Level = value
            End Set
        End Property
        Private m_Level As Integer

        Public Name As String = ""

        Public Property Race() As Integer
            Get
                Return m_Race
            End Get
            Set(ByVal value As Integer)
                m_Race = value
            End Set
        End Property
        Private m_Race As Integer

        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property
        Private m_Type As Integer

        Public Property Category() As Integer
            Get
                Return m_Category
            End Get
            Set(ByVal value As Integer)
                m_Category = value
            End Set
        End Property
        Private m_Category As Integer

        Public Property Ot() As Integer
            Get
                Return m_Ot
            End Get
            Set(ByVal value As Integer)
                m_Ot = value
            End Set
        End Property
        Private m_Ot As Integer

        Public Property EffectStrings() As String()
            Get
                Return m_EffectStrings
            End Get
            Set(ByVal value As String())
                m_EffectStrings = value
            End Set
        End Property
        Private m_EffectStrings As String()

        Public Property SetCode() As Integer
            Get
                Return m_SetCode
            End Get
            Set(ByVal value As Integer)
                m_SetCode = value
            End Set
        End Property
        Private m_SetCode As Integer

    End Class
End Namespace
