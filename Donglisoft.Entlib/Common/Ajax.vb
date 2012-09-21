
Public Class Ajax

    Public Shared Function JsonDecode(ByVal Source As String) As String
        If Source Is Nothing Then Return ""
        Return Source.Replace("#comma", ",").Replace("#semicolon", ":")
    End Function

    Public Shared Function JsonEncode(ByVal Source As String) As String
        If Source Is Nothing Then Return ""
        Return Source.Replace(",", "#comma").Replace(":", "#semicolon")
    End Function

    Public Shared Function ToJsonString(ByVal Source As String) As String
        If Source Is Nothing Then Return ""
        Return Source.Replace("\", "\\").Replace("""", "\""").Replace("'", "\'")
    End Function
    ''' <summary>
    ''' 数据行转Json格式字符串
    ''' </summary>
    ''' <param name="MyRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DataRowToJson(ByVal MyRow As DataRow) As String
        Dim i As Integer
        Dim sb As New System.Text.StringBuilder()
        sb.Append("{")
        For i = 0 To MyRow.Table.Columns.Count - 1
            sb.Append(MyRow.Table.Columns(i).ColumnName)
            sb.Append(":""")
            sb.Append(Ajax.ToJsonString(MyRow.Item(i).ToString()))
            sb.Append("""")
            If i < MyRow.Table.Columns.Count - 1 Then
                sb.Append(",")
            End If
        Next
        sb.Append("}")
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' 数据表转Json字符串
    ''' </summary>
    ''' <param name="MyTable">要转换的目标数据表</param>
    ''' <param name="PageEnabled">是否包含分页信息</param>
    ''' <param name="RecCount">总条数</param>
    ''' <param name="PageSize">单页记录数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DataTableToJson(ByVal MyTable As DataTable, _
                                           Optional ByVal PageEnabled As Boolean = False, _
                                           Optional ByVal RecCount As Integer = 1, _
                                           Optional ByVal PageSize As Integer = 20) As String
        Dim obj As DataRow
        Dim sJson As New System.Text.StringBuilder()
        If MyTable.TableName Is Nothing Or MyTable.TableName.Trim() = "" Then
            sJson.Append("{root:[")
        Else
            sJson.Append("{")
            sJson.Append(MyTable.TableName)
            sJson.Append(":[")
        End If

        For Each obj In MyTable.Rows
            sJson.Append(DataRowToJson(obj))
            If MyTable.Rows.IndexOf(obj) < MyTable.Rows.Count - 1 Then sJson.Append(",")
        Next

        sJson.Append("]")
        If PageEnabled Then
            Dim PageCount As Integer
            If PageSize < 1 Then
                PageCount = 1
            Else
                PageCount = RecCount \ PageSize
                If RecCount Mod PageSize > 0 Then PageCount += 1
            End If
            sJson.Append(",PageCount:'")
            sJson.Append(PageCount.ToString())
            sJson.Append("',PageSize:'")
            sJson.Append(PageSize.ToString())
            sJson.Append("',RecCount:'")
            sJson.Append(RecCount.ToString)
            sJson.Append("'")
        End If
        sJson.Append("}")
        Return sJson.ToString()
    End Function


    ''' <summary>
    ''' DataTable分页并转Json字符串
    ''' </summary>
    ''' <param name="MyTable">要转换的目标数据表</param>
    ''' <param name="PageEnabled">是否包含分页信息</param>
    ''' <param name="PageIndex">页索引</param>
    ''' <param name="PageSize">单页记录数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DataTablePageToJson(ByVal MyTable As DataTable, _
                                           Optional ByVal PageEnabled As Boolean = False, _
                                           Optional ByVal PageIndex As Integer = 1, _
                                           Optional ByVal PageSize As Integer = 20) As String
        Dim obj As DataRow
        Dim sJson As New System.Text.StringBuilder()
        Dim RecCount As Integer
        Dim s, rows As Integer
        Dim count As Integer

        s = 0
        rows = 0
        count = PageIndex * PageSize
        RecCount = MyTable.Rows.Count
        If MyTable.TableName Is Nothing Or MyTable.TableName.Trim() = "" Then
            sJson.Append("{root:[")
        Else
            sJson.Append("{")
            sJson.Append(MyTable.TableName)
            sJson.Append(":[")
        End If

        If PageEnabled = False Then
            For Each obj In MyTable.Rows
                sJson.Append(DataRowToJson(obj))
                If MyTable.Rows.IndexOf(obj) < MyTable.Rows.Count - 1 Then sJson.Append(",")
            Next
        Else
            If PageIndex = 1 Then
                s = 0
            Else
                If count <= RecCount Then
                    s = count - PageSize
                Else
                    s = PageSize * (PageIndex - 1)
                End If
            End If

            If count > RecCount Then
                rows = RecCount
            Else
                rows = count
            End If

            For i As Integer = s To rows - 1
                sJson.Append(DataRowToJson(MyTable.Rows(i)))
                If i < rows - 1 Then sJson.Append(",")
            Next

        End If

        sJson.Append("]")
        If PageEnabled Then
            Dim PageCount As Integer
            If PageSize < 1 Then
                PageCount = 1
            Else
                PageCount = RecCount \ PageSize
                If RecCount Mod PageSize > 0 Then PageCount += 1
            End If
            sJson.Append(",PageCount:'")
            sJson.Append(PageCount.ToString())
            sJson.Append("',PageSize:'")
            sJson.Append(PageSize.ToString())
            sJson.Append("',RecCount:'")
            sJson.Append(RecCount.ToString)
            sJson.Append("'")
        End If
        sJson.Append("}")
        Return sJson.ToString()
    End Function

End Class


