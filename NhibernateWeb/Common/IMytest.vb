Public Interface IMytest
    Function Mytest() As String
    Sub Myname()
End Interface


Public Class Mytest
    Implements IMytest

    Sub New()

    End Sub

    Public Sub Myname() Implements IMytest.Myname

    End Sub

    Public Function Mytest() As String Implements IMytest.Mytest
        Return "test"
    End Function

End Class
