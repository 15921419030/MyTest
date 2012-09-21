Imports System.Web
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports Yahoo.Yui.Compressor

Public Class MegerCompress
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/javascript"
        'context.Response.Write("function f(){}")

        Dim s As String = context.Request("v")
        Dim jspath() As String = s.Split(",")
        Dim strContent As New StringBuilder


        For i As Integer = 0 To jspath.Length - 1

        Next
        Dim fpath As String = System.Web.HttpContext.Current.Server.MapPath(jspath(0))

        If File.Exists(fpath) Then
            '读取文本　
            Dim sr As New StreamReader(fpath, System.Text.Encoding.UTF8)
            Dim str As String = sr.ReadToEnd()
            sr.Close()
            strContent.Append(str)
        End If

        Dim js As New JavaScriptCompressor(strContent.ToString(), False, Encoding.UTF8, System.Globalization.CultureInfo.CurrentCulture)

        context.Response.Write(js.Compress)

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class