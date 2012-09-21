
Imports System.Data
Imports System.Data.SqlClient
Imports Donglisoft.Data

Partial Public Class PageControl
    Inherits System.Web.UI.UserControl

    '定义属性
    Protected strConn As String
    Protected strTableName As String
    Protected strDataField As String
    Protected strDataFieldText As String
    Protected strOrderField As String
    Protected strOrderType As String = "desc"
    Protected strWhere As String = ""
    Protected strKeyField As String
    Protected intTotalRowsCount As Integer
    Protected intPageSize As Integer
    Protected bolDisplayCheckBox As Boolean
    Protected bolIsSelectedCheckBox As Boolean = False

    Protected strSelectedItems As String = String.Empty

    Protected strClickEventFunction As String
    Protected strDoubleClickEventFunction As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If (DataField <> String.Empty And DataFieldText <> String.Empty) Then
                Dim intDataFieldLength As Integer = DataField.Split(",").Length
                Dim intDataFieldTextLength As Integer = DataFieldText.Split(",").Length

                Dim arrDataField(intDataFieldLength) As String
                Dim arrDataFieldText(intDataFieldTextLength) As String
                Dim arrDataFooterText(0) As String

                arrDataField = DataField.Split(",")
                arrDataFieldText = DataFieldText.Split(",")

                '设置复选框
                If (intDataFieldLength = intDataFieldTextLength) Then
                    '//是否添加复选框
                    If DisplayCheckBox = True Then
                        Me.GridView1.Columns(0).Visible = True
                    Else
                        Me.GridView1.Columns(0).Visible = False
                    End If

                    For i As Integer = 0 To arrDataField.Length - 1
                        Dim newBoundField As New System.Web.UI.WebControls.BoundField()
                        newBoundField.HeaderText = arrDataFieldText(i)
                        newBoundField.DataField = arrDataField(i)
                        newBoundField.SortExpression = arrDataField(i)
                        GridView1.Columns.Add(newBoundField)
                    Next
                End If

            End If

            Dim obj As Object = Cache(Convert.ToString([GetType]()) & "totalOrders")
            If obj Is Nothing Then
                Cache(Convert.ToString([GetType]()) & "totalOrders") = intTotalRowsCount
                AspNetPager1.RecordCount = intTotalRowsCount
            Else
                AspNetPager1.RecordCount = CInt(obj)
            End If
            BindGridView()
        End If
    End Sub

    Protected Sub AspNetPager1_PageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AspNetPager1.PageChanged
        BindGridView()
    End Sub

    Public Sub BindGridView()
        Dim paras As SqlParameter() = New SqlParameter() { _
        New SqlParameter("@tblName", strTableName), _
        New SqlParameter("@strFields", strDataField), _
        New SqlParameter("@strOrder", strOrderField), _
        New SqlParameter("@strOrderType", strOrderType), _
        New SqlParameter("@PageSize", intPageSize), _
        New SqlParameter("@PageIndex", AspNetPager1.CurrentPageIndex), _
        New SqlParameter("@strWhere", strWhere)}

        GridView1.DataSource = SqlHelper.ExecuteReader(strConn, "proc_SplitPage", paras)
        GridView1.DataBind()
    End Sub

    '/// <summary>
    '/// 设置或获取PageControl控件连接串
    '/// </summary>
    Public Property Conn() As String
        Get
            If (strConn <> String.Empty And strConn <> Nothing) Then
                Return strConn
            Else
                Throw New Exception("没有接收到PagerControl控件的数据源连接字符串！")
            End If
        End Get
        Set(ByVal Value As String)
            strConn = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取PageControl查询的表名
    '/// </summary>
    Public Property TableName() As String
        Get
            If (strTableName <> String.Empty And strTableName <> Nothing) Then
                Return strConn
            Else
                Throw New Exception("没有接收到PagerControl控件的表名！")
            End If
        End Get
        Set(ByVal Value As String)
            strTableName = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取数据字段字符串,以逗号分隔
    '/// </summary>
    Public Property DataField() As String
        Get
            If (strDataField <> String.Empty And strDataField <> Nothing) Then
                Return strDataField
            Else
                Throw New Exception("没有接收到PagerControl控件的数据源字段DataField属性")
            End If
        End Get
        Set(ByVal Value As String)
            strDataField = Value
        End Set
    End Property


    '/// <summary>
    '/// 设置或获取数据字段对应的标题文本，以逗号分隔
    '/// </summary>
    Public Property DataFieldText() As String
        Get
            If (strDataFieldText <> String.Empty And strDataFieldText <> Nothing) Then
                Return strDataFieldText
            Else
                Throw New Exception("没有设置PagerControl控件的数据字段对应的标题文本DataFieldText属性")
            End If
        End Get
        Set(ByVal Value As String)
            strDataFieldText = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取数据排序字段,以逗号分隔
    '/// </summary>
    Public Property OrderField() As String
        Get
            If (strOrderField <> String.Empty And strOrderField <> Nothing) Then
                Return strOrderField
            Else
                Throw New Exception("没有接收到PagerControl控件的排序属性")
            End If
        End Get
        Set(ByVal Value As String)
            strOrderField = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取数据排序字段,以逗号分隔
    '/// </summary>
    Public Property SqlWhere() As String
        Get
            If (strWhere <> String.Empty And strWhere <> Nothing) Then
                Return strWhere
            Else
                Throw New Exception("没有接收到PagerControl控件的排序属性")
            End If
        End Get
        Set(ByVal Value As String)
            strWhere = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取传入的数据表中主键字段名称
    '/// </summary>
    Public Property KeyField() As String
        Get
            If (strKeyField <> String.Empty And strKeyField <> Nothing) Then
                Return strKeyField
            Else
                Throw New Exception("未设置PagerControl的KeyField属性，或此属性不能为空！")
            End If

        End Get
        Set(ByVal Value As String)
            strKeyField = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取传入查询表的行数
    '/// </summary>
    Public Property TotalRowsCount() As Integer
        Get
            Return intTotalRowsCount
        End Get
        Set(ByVal Value As Integer)
            intTotalRowsCount = Value
        End Set
    End Property


    '/// <summary>
    '/// 设置或获取PageControl控件每页显示记录的数量
    '/// </summary>
    Public Property PageSize() As Integer
        Get
            If (intPageSize <> 0) Then
                Return intPageSize
            Else
                Return 10   '如果没有初始化PageSize，默认为10
            End If
        End Get
        Set(ByVal Value As Integer)
            intPageSize = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取一个属性，以确定是否在列中显示复选框
    '/// </summary>
    Public Property DisplayCheckBox() As Boolean
        Get
            Return bolDisplayCheckBox
        End Get
        Set(ByVal Value As Boolean)
            bolDisplayCheckBox = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取一个属性，该属性为被选中的项的主键的值列表。
    '/// </summary>
    Public ReadOnly Property SelectedItems() As String
        Get
            Dim strTempSelectedItems As String = ""
            Dim row As GridViewRow

            For Each row In GridView1.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim cb As CheckBox
                    cb = DirectCast(row.FindControl("chkSelect"), CheckBox)
                    If cb.Checked = True Then
                        strSelectedItems += row.Attributes("keyvalue") + ","
                    End If
                End If
            Next
            Return strSelectedItems
        End Get
    End Property

    '/// <summary>
    '/// 设置或获取单击事件客户端处理函数名称
    '/// </summary>
    Public Property ClickEventFunction() As String
        Get
            Return strClickEventFunction
        End Get
        Set(ByVal Value As String)
            strClickEventFunction = Value
        End Set
    End Property

    '/// <summary>
    '/// 设置或获取双击事件客户端处理函数名称
    '/// </summary>
    Public Property DoubleClickEventFunction() As String
        Get
            Return strDoubleClickEventFunction
        End Get
        Set(ByVal Value As String)
            strDoubleClickEventFunction = Value
        End Set
    End Property

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound


        If e.Row.RowType = DataControlRowType.Header Then
            If ViewState("sortdirection") <> "" OrElse ViewState("sortdirection") <> String.Empty Then
                ShowSortImage(e.Row)
            End If
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            If (ClickEventFunction <> Nothing And ClickEventFunction <> String.Empty) Then
                e.Row.Attributes.Add("onclick", ClickEventFunction + "(this);f_PagerControl_onclick(this);")
            End If

            If (DoubleClickEventFunction <> Nothing And DoubleClickEventFunction <> String.Empty) Then
                e.Row.Attributes.Add("ondblclick", DoubleClickEventFunction + "(this);")
            End If
            e.Row.Attributes.Add("onmouseover", "if(this.className != 'GridViewSelectedRowStyle'){currentClass=this.className;this.className = 'PagerControlMouseOverStyle'}")
            e.Row.Attributes.Add("onmouseout", "if(this.className != 'GridViewSelectedRowStyle'){this.className = currentClass};")

            '设置主键
            Dim intKeyFieldLength As Integer = KeyField.Split(",").Length
            Dim arr_KeyField(intKeyFieldLength) As String
            arr_KeyField = KeyField.Split(",")
            Dim strKeyValue As String = ""


            '//多关键字
            Dim i As Integer
            For i = 0 To intKeyFieldLength - 1
                If (arr_KeyField(i) <> String.Empty And arr_KeyField(i) <> Nothing) Then
                    strKeyValue = e.Row.DataItem(arr_KeyField(i)).ToString()
                    If i = 0 Then
                        e.Row.Attributes.Add("keyvalue", strKeyValue)
                    Else
                        e.Row.Attributes.Add("keyvalue" + i.ToString, strKeyValue)
                    End If
                End If
            Next

        End If

    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        If String.IsNullOrEmpty(ViewState("sortdirection")) Then
            ViewState("sortdirection") = SortDirection.Descending.ToString().ToUpper
        Else
            If ViewState("sortdirection") = SortDirection.Ascending.ToString().ToUpper Then
                ViewState("sortdirection") = SortDirection.Descending.ToString().ToUpper
            Else
                ViewState("sortdirection") = SortDirection.Ascending.ToString().ToUpper
            End If
        End If

        ViewState("sortorder") = e.SortExpression

        strOrderType = IIf(ViewState("sortdirection") = "ASCENDING", "desc", "asc")
        BindGridView()

    End Sub

    Public Property AscendingImageUrl() As String
        Get
            If ViewState("SortImageAsc") IsNot Nothing Then
                Return ViewState("SortImageAsc")
            End If
            Return ""
        End Get
        Set(ByVal value As String)
            ViewState("SortImageAsc") = value
        End Set
    End Property

    Public Property DescendingImageUrl() As String
        Get
            If ViewState("SortImageDesc") IsNot Nothing Then
                Return ViewState("SortImageDesc")
            End If
            Return ""
        End Get
        Set(ByVal value As String)
            ViewState("SortImageDesc") = value
        End Set
    End Property

    Private Sub ShowSortImage(ByVal row As GridViewRow)
        For Each tc As TableCell In row.Cells
            If tc.Controls.Count > 0 And TypeOf tc.Controls(0) Is LinkButton Then
                Dim st As String = DirectCast(tc.Controls(0), LinkButton).CommandArgument
                If st = ViewState("sortorder") Then
                    Dim img As New System.Web.UI.WebControls.Image
                    img.ImageUrl = IIf(ViewState("sortdirection") = "ASCENDING", AscendingImageUrl, DescendingImageUrl)
                    tc.Controls.Add(img)
                    Exit Sub
                End If
            End If
        Next
    End Sub

End Class