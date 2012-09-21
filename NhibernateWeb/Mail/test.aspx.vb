
Imports System.Net.Mail

Partial Public Class test1
    Inherits System.Web.UI.Page


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        'Page.Error += New EventHandler(Page_Error())
        'Page.[Error] += New System.EventHandler(Page_Error)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim client As New SmtpClient()
        'client.Credentials = New System.Net.NetworkCredential("jason2008819@Gmail.com", "wangbaifeng")
        'client.Port = 587
        'client.Host = "smtp.gmail.com"
        'client.EnableSsl = True
        'Try
        '    client.Send(InitMail("479379222@qq.com"))
        '    Me.lblMail.Text = "发送成功"
        'Catch ex As Exception
        '    Me.lblMail.Text = "发送失败"
        'End Try

        Dim mail As New List(Of String)

        mail.Add("wangbaifeng@fubosoft.com")
        mail.Add("479379222@QQ.com")

        Dim mailhelper As New MailHelper
        mailhelper.Recepient = mail
        mailhelper.Subject = "标题"
        mailhelper.Body = "正文"

        mailhelper.Username = "jason2008819@Gmail.com"
        mailhelper.Password = "wangbaifeng"
        mailhelper.Port = 587
        mailhelper.Host = "smtp.gmail.com"

        Try
            mailhelper.SendMail()
            Me.lblMail.Text = "发送成功"
        Catch ex As Exception
            Me.lblMail.Text = "发送失败"
        End Try

        'lblMail.Text = "1"
        'Throw (New ArgumentNullException())

    End Sub

    Public Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim objErr As Exception = Server.GetLastError().GetBaseException()
        ' 获取错误
        Dim err As String = "1.error in:      " + Request.Url.ToString() + "</br>" + "2.error Message:      " + objErr.Message.ToString() + "</br>" + "3.stack Trace:       " + objErr.StackTrace.ToString() + "</br>"

        Response.Write(err.ToString())
        '输出错误信息
        ' Response.Redirect("ErrorPage.htm"); //可以重定向到友好的错误页面

        Server.ClearError()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

    End Sub


    Protected Overrides Sub OnError(ByVal e As EventArgs)

        Dim err As String
        Dim objErr As Exception
        objErr = Server.GetLastError().GetBaseException()
        err = "发生异常页: " + Request.Url.ToString() + "<br>" + "异常信息: " + objErr.Message + "<br>"

        Me.lblMail.Text = err


        'Dim ex As Exception = Server.GetLastError()
        'If TypeOf ex Is NotImplementedException Then
        '    Server.Transfer("errorpages/notImplemented.aspx")
        'Else
        '    Server.Transfer("errorpages/apperror.aspx")
        'End If
        'Server.ClearError()


        'Exception objErr = Server.GetLastError().GetBaseException();
        'string error = "发生异常页: " + Request.Url.ToString() + "<br>";
        'error += "异常信息: " + objErr.Message + "<br>";
        'Server.ClearError();
        'Application["error"] = error;
        'Response.Redirect("~/ErrorPage/ErrorPage.aspx");



    End Sub

    Public Function InitMail(ByVal Address As String) As MailMessage
        Dim mail As New MailMessage()
        '发件人
        mail.From = New MailAddress(Address)
        '收件人
        mail.[To].Add(New MailAddress(Address))
        mail.[To].Add(New MailAddress("wangbaifeng@fubosoft.com"))
        '主题
        mail.Subject = "ASP.NET send mail testing!"
        '内容
        mail.Body = "Welcome to join us,王先生!"
        '邮件主题和正文编码格式
        mail.SubjectEncoding = System.Text.Encoding.UTF8
        mail.BodyEncoding = System.Text.Encoding.UTF8
        '邮件正文是Html编码
        mail.IsBodyHtml = True
        '优先级
        mail.Priority = MailPriority.High
        '密件抄送收件人
        mail.Bcc.Add(Address)
        '抄送收件人
        mail.CC.Add(Address)

        ''添加附件
        'mail.Attachments.Add(New Attachment("d:\1.txt"))
        'mail.Attachments.Add(New Attachment("d:\2.txt"))
        'mail.Attachments.Add(New Attachment("d:\3.txt"))

        Return mail

    End Function

End Class


