
Imports System
Imports System.Web


Public Class InfoTip
    '***显示提示信息
    Public Shared Sub ShowWebMessage(ByVal strMsg As String, Optional ByVal IsEnd As Boolean = False)
        Dim strHtm As String
        strMsg = Replace(strMsg, Chr(13) & Chr(10), "\n")
        strMsg = Replace(strMsg, "'", "\'")

        If IsEnd = True Then
            strHtm = "<html>"
            strHtm += "<head>"
            strHtm += "<title>信息提示</title>"
            strHtm += "<meta name='vs_targetSchema' content='http://schemas.microsoft.com/intellisense/ie5'>"
            strHtm += "<script language='javascript'> alert('" & strMsg & "'); </script>"
            strHtm += " <style type='text/css'>"
            strHtm += ".but2 {"
            strHtm += " background\-color:#9CDB73;"
            strHtm += " z-index: 1;"
            strHtm += " border-top: 1px solid #FFFFFF;"
            strHtm += " border-right: 1px solid #666666;"
            strHtm += " border-bottom: 1px solid #666666;"
            strHtm += " border-left: 1px solid #FFFFFF;"
            strHtm += "}"
            strHtm += "</style>"

            strHtm += "</head>"
            strHtm += " <body><div align=center><br><br>"
            strHtm += "  <table width='173' height='85' border='1' cellspacing='0' bordercolor='#38c06b'>"
            strHtm += " <tr>"
            strHtm += "  <td height='22' bgcolor='#69D392'>信息提示</td>"
            strHtm += "  </tr>"
            strHtm += "  <tr>"
            strHtm += "  <td height='46' align='center' bgcolor='#e3ffcb'> "
            strHtm += "    <input id='Button1' type='button' value='关闭' name='Button1' onClick='window.close()' class='but2'>"
            strHtm += "    &nbsp; "
            strHtm += "   <input id='Button22' type='button' value='返回' name='Button2' onClick='window.history.back()' class='but2'>"
            strHtm += "  </td>"
            strHtm += "   </tr>"
            strHtm += "  </table>"
            strHtm += "</div></body>"
            strHtm += "</html>"
            HttpContext.Current.Response.Write(strHtm)
            HttpContext.Current.Response.End()
        Else
            HttpContext.Current.Response.Write("<script language='javascript'> alert('" & strMsg & "'); </script>")
        End If
    End Sub

End Class


