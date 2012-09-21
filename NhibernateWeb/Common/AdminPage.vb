
Imports System.IO

Public Class AdminPage
    Inherits Page


    Private _a As String

    Public Property A() As Object
        Get
            Return _a
        End Get
        Set(ByVal value As Object)
            _a = value
        End Set
    End Property


    '重写Page OnPreInit事件
    Protected Overrides Sub OnPreInit(ByVal e As EventArgs)
        MyBase.OnPreInit(e)

        '如果是登陆页面,取消登陆验证
        If GetRequestFilename().Equals("Mes_Login.aspx") Then
            GoTo Finish
        End If

        '开始页面登陆验证
        Dim isLog As Boolean = False
        Try
            isLog = Session("LOGIN_FLAG").ToString().Equals("1")
        Catch ex As Exception
        End Try

        If Not isLog Then
            Alert("您还未登录")
            Response.Redirect("~/Mes_Login.aspx?ReturnUrl=" + Request.RawUrl)
        End If
Finish:

        If True Then

        End If



    End Sub

    ''' <summary>
    ''' 在页面弹出提示框(集成于基类BasePage)
    ''' </summary>
    ''' <param name="msg">在提示框中显示的内容</param>
    Public Sub Alert(ByVal msg As String)
        Me.RegisterClientScriptBlock("js", "<script>alert('" + msg + "')</script>")
    End Sub

    ''' <summary>
    ''' 在页面弹出提示框(静态型)
    ''' </summary>
    ''' <param name="msg">在提示框中显示的内容</param>
    Public Shared Sub Alert(ByVal pg As Page, ByVal msg As String)
        pg.RegisterClientScriptBlock("js", "<script>alert('" + msg + "')</script>")
    End Sub

    ''' <summary>
    ''' 取得当前Request Url的文件名称
    ''' </summary>
    ''' <returns>返回当前Request Url的文件名称</returns>
    Public Function GetRequestFilename() As String
        Return Path.GetFileName(Request.Path)
    End Function
End Class
