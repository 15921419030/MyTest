
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.IO
Imports System.Text

Public Class AspxToHtml
    ''' <summary>
    ''' Aspx文件url
    ''' </summary>
    Public strUrl As String

    ''' <summary>
    ''' 生成html文件的保存路径
    ''' </summary>
    Public strSavePath As String

    ''' <summary>
    ''' 生成html文件的文件名
    ''' </summary>
    Public strSaveFile As String

    ''' <summary>
    ''' 将strUrl放到strSavePath目录下，保存为strSaveFile
    ''' </summary>
    ''' <returns>是否成功</returns>
    Public Function ExecAspxToHtml() As Boolean
        Try
            Dim strHTML As New StringWriter()
            Dim myPage As System.Web.UI.Page = New Page()
            'System.Web.UI.Page中有个Server对象，我们要利用一下它
            myPage.Server.Execute(strUrl, strHTML)
            '将asp_net.aspx将在客户段显示的html内容读到了strHTML中
            Dim sw As New StreamWriter(strSavePath + strSaveFile, True, System.Text.Encoding.GetEncoding("utf-8"))
            '新建一个文件Test.htm，文件格式为GB2312
            sw.Write(strHTML.ToString())
            '将strHTML中的字符写到Test.htm中
            strHTML.Close()
            '关闭StringWriter
            sw.Close()
            '关闭StreamWriter
            Return True
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 将Url放到Path目录下，保存为FileName
    ''' </summary>
    ''' <param name="Url">aspx页面url</param>
    ''' <param name="FileName">生成html文件的保存路径文件名</param>
    ''' <returns></returns>
    Public Function ExecAspxToHtml(ByVal Url As String, ByVal pathFileName As String) As Boolean
        Try

            Dim sw As New System.IO.StringWriter()
            System.Web.HttpContext.Current.Server.Execute(Url, sw, True)
            '判读时候存在，存在删除
            If File.Exists(pathFileName) Then
                File.Delete(pathFileName)
            End If
            'Response.Write(sw);
            Dim objwrite As New System.IO.StreamWriter(pathFileName, False, System.Text.Encoding.UTF8)
            objwrite.Write(sw)
            objwrite.Flush()
            objwrite.Close()
            objwrite = Nothing

            Return True
        Catch ee As Exception
            System.Web.HttpContext.Current.Response.Write("<br>" + ee.Message)
            Return False
        End Try
    End Function


    Public Sub getUrltoHtml(ByVal Url As String, ByVal Path As String)
        'Url为动态页面地址，Path为生成的静态页面
        Try
            Dim wReq As System.Net.WebRequest = System.Net.WebRequest.Create(Url)
            ' Get the response instance.
            Dim wResp As System.Net.WebResponse = wReq.GetResponse()
            ' Get the response stream.
            Dim respStream As System.IO.Stream = wResp.GetResponseStream()
            ' Dim reader As StreamReader = New StreamReader(respStream)
            Dim reader As New System.IO.StreamReader(respStream, System.Text.Encoding.GetEncoding("gb2312"))
            Dim str As String = reader.ReadToEnd()
            Dim sw As New System.IO.StreamWriter(Path, False, System.Text.Encoding.GetEncoding("gb2312"))
            sw.Write(str)
            sw.Flush()
            sw.Close()
            System.Web.HttpContext.Current.Response.Write("<script>alert('页面生成成功!');</script>")
        Catch ex As System.Exception
            System.Web.HttpContext.Current.Response.Write("<script>alert('页面生成失败!" + ex.Message + "');</script>")
        End Try
    End Sub

    Public Function ExecAspxToHtml(ByVal Url As String, ByVal Path As String, ByVal FileName As String) As Boolean
        Try
            Dim strHTML As New StringWriter()
            Dim myPage As System.Web.UI.Page = New Page()
            'System.Web.UI.Page中有个Server对象，我们要利用一下它 
            myPage.Server.Execute(Url, strHTML)
            '将asp_net.aspx将在客户段显示的html内容读到了strHTML中
            Dim sw As New StreamWriter(Path + FileName, True, System.Text.Encoding.GetEncoding("GB2312"))
            '新建一个文件Test.htm，文件格式为GB2312 
            sw.Write(strHTML.ToString())
            '将strHTML中的字符写到Test.htm中
            strHTML.Close()
            '关闭StringWriter
            sw.Close()
            '关闭StreamWriter
            Return True
        Catch
            Return False
        End Try
    End Function
End Class
