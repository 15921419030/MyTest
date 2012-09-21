
Partial Public Class CreateHtml
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim b As Boolean

            Dim mytohtml As New MyAspxToHtml

            Dim url, putout As String


            b = mytohtml.ExecAspxToHtml("GridViewTest.aspx", Server.MapPath("admin/grriviewtest.html"))

            Me.Label1.Text = "生成成功"
        Catch ex As Exception
            Me.Label1.Text = "生成失败"
        End Try
    End Sub

End Class