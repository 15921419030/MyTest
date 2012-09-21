
Imports Donglisoft.Entlib
Imports Donglisoft.Entity

Partial Public Class TestEntlib
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dbo As New DataObjectCollection(Of Fregions)
        
        Me.GridView1.DataSource = dbo.AsDataTable()
        Me.GridView1.DataBind()

    End Sub

End Class