Public Class AutoGen

    Public Shared Function GetUniqueString() As String
        Dim str As String = Guid.NewGuid().ToString()
        str = str.Replace("{", "").Replace("-", "")
        Return str
    End Function

    Public Shared Function GenRandomString(Optional ByVal Len As Integer = 50) As String
        Dim rnd As New Random()
        Dim i As Integer
        Dim strSrc As String = ""
        For i = 0 To 19
            strSrc &= rnd.Next().ToString()
        Next
        If strSrc.Length > Len Then strSrc = strSrc.Substring(0, Len)
        Return strSrc
    End Function

End Class
