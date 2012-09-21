Public Partial Class DivTest
    Inherits System.Web.UI.Page

    Protected basepath As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.myview.Text = Request("UID") + ";" + Request("Pwd")
        Response.Write("<script>alert('已经成功！');window.history.go(-1);</script>")


        Dim path As String
        path = HttpRuntime.AppDomainAppVirtualPath + "：" + HttpRuntime.AppDomainAppVirtualPath

        myview.Text = path

        basepath = System.Configuration.ConfigurationManager.AppSettings("basepath") + HttpRuntime.AppDomainAppVirtualPath


    End Sub

End Class