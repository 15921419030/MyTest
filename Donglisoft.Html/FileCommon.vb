
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Web

Public Class FileCommon
    ''' <summary>
    ''' 写入文件内容，重新覆盖
    ''' </summary>
    ''' <param name="filenam">文件相对路径</param>
    ''' <param name="content">需要写入的内容</param>
    Public Shared Sub WriteHtml(ByVal filenam As String, ByVal content As String)

        '指定要生成的HTML文件
        Dim fname As String = System.Web.HttpContext.Current.Server.MapPath(filenam)
        '创建文件信息对象
        Dim finfo As New FileInfo(fname)
        '以打开或者写入的形式创建文件流
        Using fs As New FileStream(fname, FileMode.Create)
            '根据上面创建的文件流创建写数据流
            Dim sw As New StreamWriter(fs, System.Text.Encoding.GetEncoding("gb2312"))
            '把新的内容写到创建的HTML页面中
            sw.WriteLine(content)
            sw.Flush()
            sw.Close()
        End Using
    End Sub

    ''' <summary>
    ''' 写文件  当文件不存时，则创建文件，并追加文件  调用示列：string Path = Server.MapPath("~/Log/Log.txt");  string Strings = "这是我写的内容啊"; EC.FileObj.WriteFile(Path,Strings);
    ''' </summary>
    ''' <param name="Path">文件路径</param>
    ''' <param name="Strings">文件内容</param>
    Public Shared Sub WriteFile(ByVal Path As String, ByVal Strings As String)
        If Not System.IO.File.Exists(Path) Then
            Dim f As System.IO.FileStream = System.IO.File.Create(Path)
            f.Close()
        End If
        Dim f2 As New System.IO.StreamWriter(Path, True, System.Text.Encoding.GetEncoding("gb2312"))
        f2.WriteLine(Strings)
        f2.Close()
        f2.Dispose()
    End Sub

    ''' <summary>
    ''' 将指定路径中的文件内容读出来 以字符串形式返回
    ''' </summary>
    ''' <param name="filenam">文件相对路径</param>
    ''' <returns></returns>
    Public Shared Function ReadFiles(ByVal filePath As String) As String
        If File.Exists(filePath) Then
            '读取文本　
            Dim sr As New StreamReader(filePath, System.Text.Encoding.[Default])
            Dim str As String = sr.ReadToEnd()
            sr.Close()
            Return str
        Else
            Return ""
        End If
    End Function
    ''' <summary>
    ''' 创建文件夹
    ''' </summary>
    ''' <param name="Path"></param>
    Public Shared Sub FolderCreate(ByVal Path As String)
        ' 判断目标目录是否存在如果不存在则新建之
        If Not Directory.Exists(Path) Then
            Directory.CreateDirectory(Path)
        End If
    End Sub

    ''' <summary>
    ''' 创建文件
    ''' </summary>
    ''' <param name="Path"></param>
    Public Shared Sub FileCreate(ByVal Path As String)
        Dim CreateFile As New FileInfo(Path)
        '创建文件 
        If Not CreateFile.Exists Then
            Dim FS As FileStream = CreateFile.Create()
            FS.Close()
        End If
    End Sub

    ''' <summary>
    ''' 递归删除文件夹目录及文件
    ''' </summary>
    ''' <param name="dir"></param> 
    ''' <returns></returns>
    Public Shared Sub DeleteFolder(ByVal dir As String)
        If Directory.Exists(dir) Then
            '如果存在这个文件夹删除之 
            For Each d As String In Directory.GetFileSystemEntries(dir)
                If File.Exists(d) Then
                    File.Delete(d)
                Else
                    '直接删除其中的文件                        
                    DeleteFolder(d)
                    '递归删除子文件夹 
                End If
            Next
            '删除已空文件夹                 
            Directory.Delete(dir, True)
        End If
    End Sub
    ''' <summary>
    ''' 删除目录下的所有文件
    ''' </summary>
    ''' <param name="strPath"></param>
    Public Shared Sub DelAllFile(ByVal strPath As String)
        If Directory.GetFiles(strPath).Length > 0 Then
            For Each FileName As String In Directory.GetFiles(strPath)
                File.Delete(FileName)

            Next
        End If

    End Sub
    ''' <summary>
    ''' 删除指定目录下的文件
    ''' </summary>
    ''' <param name="strPath"></param>
    Public Shared Sub DeleteFile(ByVal strPath As String)
        If File.Exists(strPath) Then
            File.Delete(strPath)
        End If
    End Sub
    ''' <summary>
    ''' 指定文件夹下面的所有内容copy到目标文件夹下面
    ''' </summary>
    ''' <param name="srcPath">原始路径</param>
    ''' <param name="aimPath">目标文件夹</param>
    Public Shared Sub CopyDir(ByVal srcPath As String, ByVal aimPath As String)
        Try
            ' 检查目标目录是否以目录分割字符结束如果不是则添加之
            If aimPath(aimPath.Length - 1) <> Path.DirectorySeparatorChar Then
                aimPath += Path.DirectorySeparatorChar
            End If
            ' 判断目标目录是否存在如果不存在则新建之
            If Not Directory.Exists(aimPath) Then
                Directory.CreateDirectory(aimPath)
            End If
            ' 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            '如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            'string[] fileList = Directory.GetFiles(srcPath);
            Dim fileList As String() = Directory.GetFileSystemEntries(srcPath)
            '遍历所有的文件和目录
            For Each file__1 As String In fileList
                '先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                If Directory.Exists(file__1) Then
                    CopyDir(file__1, aimPath & Path.GetFileName(file__1))
                Else
                    '否则直接Copy文件
                    File.Copy(file__1, aimPath & Path.GetFileName(file__1), True)
                End If
            Next
        Catch ee As Exception
            Throw ee
        End Try
    End Sub
    ''' <summary>
    ''' 复制文件到指定目录并重命名
    ''' </summary>
    ''' <param name="oldPath"></param>
    ''' <param name="newPath"></param>
    ''' <param name="newFile"></param>
    Public Shared Sub CopyFile(ByVal oldPath As String, ByVal newPath As String, ByVal newFile As String)
        Try
            If Not Directory.Exists(newPath) Then
                Directory.CreateDirectory(newPath)
            End If
            File.Copy(oldPath, newPath & newFile, True)
        Catch ee As Exception
            Throw ee
        End Try
    End Sub
    ''' <summary>
    ''' 获取指定文件详细属性
    ''' </summary>
    ''' <param name="filePath">文件详细路径</param>
    ''' <returns></returns>
    Public Shared Function GetFileAttibe(ByVal filePath As String) As String
        Dim str As String = ""
        Dim objFI As New System.IO.FileInfo(filePath)
        str += "详细路径:" & objFI.FullName & "<br/>文件名称:" & objFI.Name & "<br/>文件长度:" & objFI.Length.ToString() & "字节<br/>创建时间" & objFI.CreationTime.ToString() & "<br/>最后访问时间:" & objFI.LastAccessTime.ToString() & "<br/>修改时间:" & objFI.LastWriteTime.ToString() & "<br/>所在目录:" & objFI.DirectoryName & "<br/>扩展名:" & objFI.Extension
        Return str
    End Function
    ''' <summary>
    ''' 把DataSet内容导出excel并返回客户端 
    ''' </summary>
    ''' <param name="dsData">数据集</param>
    ''' <param name="strName">列名数组</param>
    ''' <param name="strCaption">数据标题</param>
    ''' <param name="strFileName">保存为的文件名</param>
    Public Shared Sub DataToExcel(ByVal dsData As System.Data.DataSet, ByVal strName As String(), ByVal strCaption As String, ByVal strFileName As String)
        Dim dtData As System.Data.DataTable = dsData.Tables(0)
        Dim dgExport As System.Web.UI.WebControls.GridView = Nothing
        ' 当前对话 
        Dim curContext As System.Web.HttpContext = System.Web.HttpContext.Current
        ' IO用于导出并返回excel文件 
        Dim strWriter As System.IO.StringWriter = Nothing
        Dim htmlWriter As System.Web.UI.HtmlTextWriter = Nothing

        If dtData IsNot Nothing Then
            Dim attachment As String = "attachment; filename=" & System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8) & ".xls"
            ' 设置编码和附件格式 
            Dim Response As HttpResponse = HttpContext.Current.Response
            Response.ClearContent()
            Response.AddHeader("content-disposition", attachment)
            'Response.ContentType = "application/ms-excel";
            Response.ContentType = "application/vnd.ms-excel"
            Response.ContentEncoding = System.Text.Encoding.UTF7
            Response.Charset = ""

            ' 导出excel文件 
            strWriter = New System.IO.StringWriter()
            htmlWriter = New System.Web.UI.HtmlTextWriter(strWriter)

            ' 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
            dgExport = New System.Web.UI.WebControls.GridView()
            dgExport.Caption = strCaption
            dgExport.RowStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left
            dgExport.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left
            dgExport.HeaderStyle.Font.Bold = False
            dgExport.DataSource = dtData.DefaultView

            If strName.Length > 0 Then
                Dim i As Integer
                For i = 0 To strName.Length - 1
                    dtData.Columns(i).ColumnName = strName(i)
                Next
            End If
            dgExport.AllowPaging = False
            dgExport.DataBind()

            ' 返回客户端 
            dgExport.RenderControl(htmlWriter)
            curContext.Response.Write(strWriter.ToString())
            curContext.Response.[End]()
        End If
    End Sub
End Class
