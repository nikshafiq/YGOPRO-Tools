
Public Class GlobalVariables
    ' Fields

    Public Shared mzone1, mzone2 As New ToolStripMenuItem
    Public Shared szone1, szone2 As ToolStripMenuItem
    Public Shared hand1, hand2 As ToolStripMenuItem
    Public Shared deck1, deck2 As ToolStripMenuItem
    Public Shared grave1, grave2 As ToolStripMenuItem
    Public Shared comment As String
    Public Shared ainm As String
    Public Shared reload, reload2 As String
    Public Shared aux1, aux2 As String
    Public Shared dueltp As String
    Public Shared duelmd As String
    Public Shared playerlp As String
    Public Shared enemylp As String
    Public Shared deck11 As String
    Public Shared deck12 As String
    Public Shared deck13 As String
    Public Shared deck14 As String
    Public Shared deck15 As String
    Public Shared deck16, deck17, deck18, deck19, deck10 As String
    Public Shared deckcard16, deckcard17, deckcard18, deckcard19, deckcard10 As String
    Public Shared deck1p As String = ",0,0,LOCATION_DECK,0,POS_FACEDOWN)"
    Public Shared deck1sum As String
    Public Shared deck21 As String
    Public Shared deck22 As String
    Public Shared deck23 As String
    Public Shared deck24 As String
    Public Shared deck25 As String
    Public Shared deck26, deck27, deck28, deck29, deck20 As String
    Public Shared deckcard26, deckcard27, deckcard28, deckcard29, deckcard20 As String
    Public Shared deck2p As String = ",1,1,LOCATION_DECK,0,POS_FACEDOWN)"
    Public Shared deck2sum As String
    Public Shared extra11 As String
    Public Shared extra12 As String
    Public Shared extra13 As String
    Public Shared extra14 As String
    Public Shared extra15 As String
    Public Shared extra16, extra17, extra18, extra19, extra10 As String
    Public Shared extracard16, extracard17, extracard18, extracard19, extracard10 As String
    Public Shared extra21 As String
    Public Shared extra22 As String
    Public Shared extra23 As String
    Public Shared extra24 As String
    Public Shared extra25 As String
    Public Shared extra26, extra27, extra28, extra29, extra20 As String
    Public Shared extracard26, extracard27, extracard28, extracard29, extracard20 As String
    Public Shared extrapt As String = ",0,0,LOCATION_EXTRA,0,POS_FACEDOWN)"
    Public Shared extrapt2 As String = ",1,1,LOCATION_EXTRA,0,POS_FACEDOWN)"
    Public Shared field1 As String
    Public Shared field2 As String
    Public Shared field2pt As String = ",1,1,LOCATION_SZONE,5,"
    Public Shared fieldpt As String = ",0,0,LOCATION_SZONE,5,"
    Public Shared grave11 As String
    Public Shared grave12 As String
    Public Shared grave13 As String
    Public Shared grave14 As String
    Public Shared grave15 As String
    Public Shared grave16, grave17, grave18, grave19, grave10 As String
    Public Shared gravecard16, gravecard17, gravecard18, gravecard19, gravecard10 As String
    Public Shared grave21 As String
    Public Shared grave22 As String
    Public Shared grave23 As String
    Public Shared grave24 As String
    Public Shared grave25 As String
    Public Shared grave26, grave27, grave28, grave29, grave20 As String
    Public Shared gravecard26, gravecard27, gravecard28, gravecard29, gravecard20 As String
    Public Shared grave2pt As String = ",1,1,LOCATION_GRAVE,0,POS_FACEUP)"
    Public Shared gravept As String = ",0,0,LOCATION_GRAVE,0,POS_FACEUP)"
    Public Shared hand11 As String
    Public Shared hand12 As String
    Public Shared hand13 As String
    Public Shared hand14 As String
    Public Shared hand15 As String
    Public Shared hand21 As String
    Public Shared hand22 As String
    Public Shared hand23 As String
    Public Shared hand24 As String
    Public Shared hand25 As String
    Public Shared handpt As String = ",0,0,LOCATION_HAND,0,POS_FACEUP)"
    Public Shared handpt2 As String = ",1,1,LOCATION_HAND,0,POS_FACEUP)"
    Public Shared monster11 As String
    Public Shared monster12 As String
    Public Shared monster13 As String
    Public Shared monster14 As String
    Public Shared monster15 As String
    Public Shared monster21 As String
    Public Shared monster22 As String
    Public Shared monster23 As String
    Public Shared monster24 As String
    Public Shared monster25 As String
    Public Shared removed11 As String
    Public Shared removed12 As String
    Public Shared removed13 As String
    Public Shared removed14 As String
    Public Shared removed15 As String
    Public Shared removed16, removed17, removed18, removed19, removed10 As String
    Public Shared removedcard16, removedcard17, removedcard18, removedcard19, removedcard10 As String
    Public Shared removed21 As String
    Public Shared removed22 As String
    Public Shared removed23 As String
    Public Shared removed24 As String
    Public Shared removed25 As String
    Public Shared removed26, removed27, removed28, removed29, removed20 As String
    Public Shared removedcard26, removedcard27, removedcard28, removedcard29, removedcard20 As String
    Public Shared removedpt As String = ",0,0,LOCATION_REMOVED,0,POS_FACEUP)"
    Public Shared removedpt2 As String = ",1,1,LOCATION_REMOVED,0,POS_FACEUP)"
    Public Shared szone11 As String
    Public Shared szone12 As String
    Public Shared szone13 As String
    Public Shared szone14 As String
    Public Shared szone15 As String
    Public Shared szone16 As String
    Public Shared szone17 As String
    Public Shared szone18 As String
    Public Shared szone21 As String
    Public Shared szone22 As String
    Public Shared szone23 As String
    Public Shared szone24 As String
    Public Shared szone25 As String
    Public Shared szone26 As String
    Public Shared szone27 As String
    Public Shared szone28 As String
    Public Shared HINT1, HINT2 As String
    Public Shared SHOWHINT1, SHOWHINT2 As String
    Public Shared pos11, pos12, pos13, pos14, pos15 As String
    Public Shared poss1, poss2, poss3, poss4, poss5, poss6, poss7, poss8 As String
    Public Shared posz1, posz2, posz3, posz4, posz5, posz6, posz7, posz8 As String
    Public Shared pos21, pos22, pos23, pos24, pos25 As String
    Public Shared aimode, aitype As Integer
    Public Shared aiscript, aicommand As String

    ' Propertys
    Public Shared Property Duelcomment As String
    Public Shared Property AIname As String
    Public Shared Property Dueltype As String
    Public Shared Property Duelmode As String
    Public Shared Property Playerpoints As String
    Public Shared Property Enemypoints As String
    Public Shared Property monstercard11 As String
    Public Shared Property monstercard12 As String
    Public Shared Property monstercard13 As String
    Public Shared Property monstercard14 As String
    Public Shared Property monstercard15 As String
    Public Shared Property handcard11 As String
    Public Shared Property handcard12 As String
    Public Shared Property handcard13 As String
    Public Shared Property handcard14 As String
    Public Shared Property handcard15 As String
    Public Shared Property deckcard11 As String
    Public Shared Property deckcard12 As String
    Public Shared Property deckcard13 As String
    Public Shared Property deckcard14 As String
    Public Shared Property deckcard15 As String
    Public Shared Property gravecard11 As String
    Public Shared Property gravecard12 As String
    Public Shared Property gravecard13 As String
    Public Shared Property gravecard14 As String
    Public Shared Property gravecard15 As String
    Public Shared Property removedcard11 As String
    Public Shared Property removedcard12 As String
    Public Shared Property removedcard13 As String
    Public Shared Property removedcard14 As String
    Public Shared Property removedcard15 As String
    Public Shared Property extracard11 As String
    Public Shared Property extracard12 As String
    Public Shared Property extracard13 As String
    Public Shared Property extracard14 As String
    Public Shared Property extracard15 As String
    Public Shared Property spellcard11 As String
    Public Shared Property spellcard12 As String
    Public Shared Property spellcard13 As String
    Public Shared Property spellcard14 As String
    Public Shared Property spellcard15 As String
    Public Shared Property spellcard16 As String
    Public Shared Property spellcard17 As String
    Public Shared Property spellcard18 As String
    Public Shared Property monstercard21 As String
    Public Shared Property monstercard22 As String
    Public Shared Property monstercard23 As String
    Public Shared Property monstercard24 As String
    Public Shared Property monstercard25 As String
    Public Shared Property spellcard21 As String
    Public Shared Property spellcard22 As String
    Public Shared Property spellcard23 As String
    Public Shared Property spellcard24 As String
    Public Shared Property spellcard25 As String
    Public Shared Property spellcard26 As String
    Public Shared Property spellcard27 As String
    Public Shared Property spellcard28 As String
    Public Shared Property handcard21 As String
    Public Shared Property handcard22 As String
    Public Shared Property handcard23 As String
    Public Shared Property handcard24 As String
    Public Shared Property handcard25 As String
    Public Shared Property deckcard21 As String
    Public Shared Property deckcard22 As String
    Public Shared Property deckcard23 As String
    Public Shared Property deckcard24 As String
    Public Shared Property deckcard25 As String
    Public Shared Property gravecard21 As String
    Public Shared Property gravecard22 As String
    Public Shared Property gravecard23 As String
    Public Shared Property gravecard24 As String
    Public Shared Property gravecard25 As String
    Public Shared Property removedcard21 As String
    Public Shared Property removedcard22 As String
    Public Shared Property removedcard23 As String
    Public Shared Property removedcard24 As String
    Public Shared Property removedcard25 As String
    Public Shared Property extracard21 As String
    Public Shared Property extracard22 As String
    Public Shared Property extracard23 As String
    Public Shared Property extracard24 As String
    Public Shared Property extracard25 As String

End Class


