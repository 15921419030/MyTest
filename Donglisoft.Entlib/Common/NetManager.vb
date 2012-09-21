Imports System.Runtime.InteropServices

Namespace Common
    Public Class NetManager
        ''' <summary>
        ''' 获取请求相对路径
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetScriptName() As String
            Dim Request As System.Web.HttpRequest = System.Web.HttpContext.Current.Request
            Return Request.ServerVariables("SCRIPT_NAME").ToLower()
        End Function

        ''' <summary>
        ''' 获取浏览器用户代码信息，用于外接程序请求
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetUserAgent() As String
            Dim Request As System.Web.HttpRequest = System.Web.HttpContext.Current.Request
            Return Request.UserAgent.ToLower()
        End Function

        ''' <summary>
        ''' 获取虚拟目录根路径
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRootPath() As String
            Dim Request As System.Web.HttpRequest = System.Web.HttpContext.Current.Request
            Dim sName As String = Request.ServerVariables("SCRIPT_NAME").ToLower()
            Dim arr As String() = sName.Split("/")
            Dim sPre As String = ""
            If arr.Length >= 2 Then sPre = "/" + arr(1) + "/"
            Return sPre
        End Function

        <DllImport("Iphlpapi.dll")> _
        Public Shared Function SendARP(ByVal dest As Int32, ByVal host As Int32, ByRef mac As Int64, ByRef length As Int32) As Integer

        End Function

        <DllImport("Ws2_32.dll")> _
        Public Shared Function inet_addr(ByVal ip As String) As Int32

        End Function
        ''' <summary>
        ''' 获取请求IP地址
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetIP() As String
            Dim reqest As System.Web.HttpRequest = System.Web.HttpContext.Current.Request
            Dim ip As String = ""
            If Not reqest Is Nothing Then
                ip = reqest.ServerVariables("HTTP_X_FORWARDED_FOR")
                If (ip Is Nothing OrElse ip = "" OrElse ip = "unknown") Then
                    ip = reqest.ServerVariables("REMOTE_ADDR")
                End If
                If (ip Is Nothing OrElse ip = "" OrElse ip = "unknown") Then
                    ip = reqest.UserHostAddress
                End If
            End If
            If ip = "::1" Then ip = "127.0.0.1"
            Return ip
        End Function
        ''' <summary>
        ''' 获取请求网卡Mac地址
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getMACAddress() As String
            Dim reqest As System.Web.HttpRequest = System.Web.HttpContext.Current.Request
            'Dim userip As String = reqest.UserHostAddress
            Dim strClientIP As String = reqest.UserHostAddress.ToString().Trim()
            Dim ldest As Int32 = Common.NetManager.inet_addr(strClientIP)
            '目的地的ip 
            'Dim lhost As Int32 = Fpl.Common.NetManager.inet_addr("")
            '本地服务器的ip 
            Dim macinfo As New Int64()
            Dim len As Int32 = 6
            Dim res As Integer = Common.NetManager.SendARP(ldest, 0, macinfo, len)
            Dim mac_src As String = macinfo.ToString("X")
            If mac_src = "0" Then
                Return "00-00-00-00-00-00" '服务器本机访问
            End If

            While mac_src.Length < 12
                mac_src = mac_src.Insert(0, "0")
            End While

            Dim mac_dest As String = ""

            For i As Integer = 0 To 10
                If 0 = (i Mod 2) Then
                    If i = 10 Then
                        mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2))
                    Else
                        mac_dest = "-" & mac_dest.Insert(0, mac_src.Substring(i, 2))
                    End If
                End If
            Next
            Return mac_dest


            'Dim str As String = ""
            'Dim macAddress As String = ""
            'Try
            '    Dim p As Process = New Process()
            '    p.StartInfo.FileName = "nbtstat"
            '    p.StartInfo.Arguments = "-A " + ip
            '    p.StartInfo.UseShellExecute = False
            '    p.StartInfo.CreateNoWindow = True
            '    p.StartInfo.RedirectStandardOutput = True
            '    p.Start()
            '    For i As Integer = 1 To 100
            '        str = p.StandardOutput.ReadLine()
            '        If Not str Is Nothing Then
            '            If str.IndexOf("MAC 地址") > 0 Or str.IndexOf("MAC Address") > 0 Then
            '                macAddress = str.Split("=")(1).Trim()
            '                Exit For
            '            End If
            '        End If
            '    Next
            '    If macAddress = "" Then
            '        'Dim idest As Int32 = inet_addr(ip)
            '        'Dim ihost As Int32 = inet_addr("")
            '        'Dim imac As Int64 = New Int64()
            '        'Dim len As Int32 = 6
            '        'Dim res As Int32 = SendARP(idest, 0, imac, len)
            '        'Dim strMac As String = imac.ToString("X")

            '        'While (strMac.Length < 12)
            '        '    strMac = strMac.Insert(0, "0")
            '        'End While
            '        'Dim strMacDest As String = ""
            '        'For i As Integer = 0 To 11
            '        '    If (0 = i Mod 2) Then
            '        '        If i = 10 Then
            '        '            strMacDest = strMacDest.Insert(0, strMac.Substring(i, 2))
            '        '        Else
            '        '            strMacDest = "-" + strMacDest.Insert(0, strMac.Substring(i, 2))
            '        '        End If
            '        '    End If
            '        'Next
            '        'macAddress = strMacDest
            '        macAddress = "00-00-00-00-00-00"
            '    End If
            'Catch ex As Exception

            'End Try
            'Return macAddress
        End Function

    End Class

End Namespace
