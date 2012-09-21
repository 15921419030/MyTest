Public Class UserDBAssist
    Public Shared Sub DataTrans(Of T As {New, DataObject})(ByVal SourceContext As Settings.DBContext, _
                          ByVal TargetContext As Settings.DBContext)
        Dim data As New DataObjectCollection(Of T)(SourceContext)
        data.AsCollection()
        Dim data1 As New DataObjectCollection(Of T)(TargetContext)
        For i = 0 To data.Count - 1
            Dim obj As T = data1.CreateInstance()
            data.Item(i).Assign(obj)
            data1.Add(obj)
        Next
        data1.CommitChanges()
    End Sub

    Public Shared Sub DataTrans(ByVal Entity As String, ByVal SourceContext As Settings.DBContext, _
                              ByVal TargetContext As Settings.DBContext)
        Dim t As Type = GetType(DataObjectCollection(Of ))
        'If t.IsGenericType Then
        '    Response.Write("Fpl.DataObjectCollection is Generic Type <br>")
        '    Dim args As Type() = Fpl.FPLAssembly.GetTypesFromAssembly("AniProductDB")
        '    Response.Write("AniProductDB including:<br>")
        '    For Each at As Type In args
        '        Response.Write(at.Name & "<BR>")
        '    Next
        'End If
        t = t.MakeGenericType(New Type() {FPLAssembly.GetTypeFromAssembly("AniProductDB", Entity)})
        Dim d As Object = Activator.CreateInstance(t, New Object() {SourceContext})
        Dim d1 As Object = Activator.CreateInstance(t, New Object() {TargetContext})
        t.InvokeMember("AsCollection", Reflection.BindingFlags.Default Or Reflection.BindingFlags.InvokeMethod, _
                       Nothing, d, Nothing)
        For i = 0 To d.Count - 1
            Dim src As DataObject = d.Item(i)
            Dim obj As DataObject = d1.CreateInstance()
            src.Assign(obj)
            d1.Add(obj)
        Next
        d1.CommitChanges()
    End Sub
End Class
