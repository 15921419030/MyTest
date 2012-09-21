
Imports System.Net
Imports System.Net.Mail

Public Class MailHelper

    Private _recepient As List(Of String)
    Private _bcc As String
    Private _cc As String
    Private _subject As String
    Private _body As String

    Private _username As String
    Private _password As String
    Private _port As Integer
    Private _host As String

    Public Property Bcc() As String
        Get
            Return _bcc
        End Get
        Set(ByVal value As String)
            _bcc = value
        End Set
    End Property

    Public Property Cc() As String
        Get
            Return _cc
        End Get
        Set(ByVal value As String)
            _cc = value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal value As String)
            _subject = value
        End Set
    End Property

    Public Property Body() As String
        Get
            Return _body
        End Get
        Set(ByVal value As String)
            _body = value
        End Set
    End Property

    Public WriteOnly Property Username() As String
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public WriteOnly Property Port() As Integer
        Set(ByVal value As Integer)
            _port = value
        End Set
    End Property

    Public WriteOnly Property Host() As String
        Set(ByVal value As String)
            _host = value
        End Set
    End Property

    Public Property Recepient() As List(Of String)
        Get
            Return _recepient
        End Get
        Set(ByVal value As List(Of String))
            _recepient = value
        End Set
    End Property

    Public Sub SendMail()

        Dim maillist As List(Of String)

        maillist = Recepient

        ' Instantiate a new instance of sMailMessage
        Dim sMailMessage As New MailMessage()

        ' Set the recepient address of the mail message
        Dim i As Integer = 0
        While i < maillist.Count
            sMailMessage.[To].Add(New MailAddress(maillist(i)))
            i += 1
        End While

        ' Check if the bcc value is nothing or an empty string
        If Not Bcc Is Nothing And Bcc <> String.Empty Then
            ' Set the Bcc address of the mail message
            sMailMessage.Bcc.Add(New MailAddress(Bcc))
        End If

        ' Check if the cc value is nothing or an empty value
        If Not Cc Is Nothing And Cc <> String.Empty Then
            ' Set the CC address of the mail message
            sMailMessage.CC.Add(New MailAddress(Cc))
        End If

        ' Set the subject of the mail message
        sMailMessage.Subject = Subject
        ' Set the body of the mail message
        sMailMessage.Body = Body

        ' Set the format of the mail message body as HTML
        sMailMessage.IsBodyHtml = True
        ' Set the priority of the mail message to normal
        sMailMessage.Priority = MailPriority.Normal

        '邮件主题和正文编码格式
        sMailMessage.SubjectEncoding = System.Text.Encoding.UTF8
        sMailMessage.BodyEncoding = System.Text.Encoding.UTF8
        '邮件正文是Html编码
        sMailMessage.IsBodyHtml = True
        '优先级
        sMailMessage.Priority = MailPriority.High


        ' Instantiate a new instance of SmtpClient
        Dim client As New SmtpClient()
        client.Credentials = New System.Net.NetworkCredential(_username, _password)
        client.Port = _port
        client.Host = _host
        client.EnableSsl = True

        ' Send the mail message
        client.Send(sMailMessage)

        sMailMessage.Dispose()

    End Sub

End Class
