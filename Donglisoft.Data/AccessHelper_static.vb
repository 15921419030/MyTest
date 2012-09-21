Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Reflection

Partial Public Class AccessHelper
    Shared Function get_defualt_dbpath() As String
        Return Path.GetDirectoryName(Assembly.GetAssembly(GetType(AccessHelper)).Location) + "\db.mdb"
    End Function

End Class

