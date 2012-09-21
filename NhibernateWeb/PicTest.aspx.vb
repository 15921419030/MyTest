
Imports System.IO

Partial Public Class PicTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.txtPic.Text = System.DateTime.Now.ToString("yyyyMMddHHmmss-ffffff")

        Dim savePath As String = ""
        Dim dirPath As String = Context.Server.MapPath(savePath)
        dirPath = dirPath + "/wangbaifeng/" + System.DateTime.Now.ToString("yyyyMMdd") + "/"

        If Not Directory.Exists(dirPath) Then
            Directory.CreateDirectory(dirPath)
        End If

    End Sub

End Class