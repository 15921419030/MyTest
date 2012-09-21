Imports System
Imports System.Web
Imports System.ComponentModel

''' <summary>
''' 强类型数据集合（支持从DataObject继承的类）
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
Public Class DataObjectCollection(Of T As {New, DataObject})
    Inherits List(Of T)
    Implements IListSource

    Private sqlGen As DataAccess.SqlGenerator
    Private _selectFields As String = "*"

    Private _DBAgent As UserDBAgent
    Private _DBName As String
    Private _SchemaName As String
    Private _pageSize As Integer = 20

    Protected Friend _data As DataTable
    ''' <summary>
    ''' 构造函数，数据环境从默认的Setting里面获得
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _DBAgent = New UserDBAgent()
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        sqlGen = New DataAccess.SqlGenerator(entity)
    End Sub

    Public Sub New(ByVal DBName As String, ByVal Schema As String)
        _DBAgent = New UserDBAgent()
        _DBName = DBName
        _SchemaName = Schema
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        entity.SetDBName(DBName)
        entity.SetSchemaName(Schema)
        sqlGen = New DataAccess.SqlGenerator(entity)
    End Sub

    ''' <summary>
    ''' 构造函数，将数据环境作为参数传入并可以接受其他数据库
    ''' </summary>
    ''' <param name="DBAgent"></param>
    ''' <param name="DBName"></param>
    ''' <param name="Schema"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal DBAgent As UserDBAgent, ByVal DBName As String, ByVal Schema As String)
        _DBAgent = DBAgent
        _DBName = DBName
        _SchemaName = Schema
        If _DBAgent Is Nothing Then _DBAgent = New UserDBAgent()
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        entity.SetDBName(DBName)
        entity.SetSchemaName(Schema)
        sqlGen = New DataAccess.SqlGenerator(entity, _DBAgent.GetProvider())
    End Sub

    Public Sub New(ByVal Context As Settings.DBContext)
        _DBAgent = New UserDBAgent(Context)
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        sqlGen = New DataAccess.SqlGenerator(entity, _DBAgent.GetProvider())
    End Sub
    ''' <summary>
    ''' 构造函数，将数据环境作为参数传入
    ''' </summary>
    ''' <param name="DBAgent">数据操作代理类</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal DBAgent As UserDBAgent)
        _DBAgent = DBAgent
        If _DBAgent Is Nothing Then _DBAgent = New UserDBAgent()
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        sqlGen = New DataAccess.SqlGenerator(entity, _DBAgent.GetProvider())
    End Sub
    ''' <summary>
    ''' 构造函数，数据库类型为Sql Server
    ''' </summary>
    ''' <param name="connectionString">连接字符串</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal connectionString As String)
        _DBAgent = New UserDBAgent(connectionString)
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        sqlGen = New DataAccess.SqlGenerator(entity, _DBAgent.GetProvider())
    End Sub
    ''' <summary>
    ''' 构造函数，传入指定类型的数据库和连接串
    ''' </summary>
    ''' <param name="provider">数据库类型</param>
    ''' <param name="connectionString">数据库连接字符串</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal provider As UserDBProvider, ByVal connectionString As String)
        _DBAgent = New UserDBAgent(provider, connectionString)
        Dim entity As T = New T
        entity.AssignDBA(_DBAgent)
        sqlGen = New DataAccess.SqlGenerator(entity, provider)
    End Sub
    ''' <summary>
    ''' 构造函数，从DataTable初始化数据，不需要调用AsDataTable和AsCollection
    ''' </summary>
    ''' <param name="Data">包含数据的DataTable</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Data As DataTable)
        _DBAgent = New UserDBAgent()
        sqlGen = Nothing
        _data = Data
        DataTableToCollection(Data)
    End Sub

    Public ReadOnly Property ContainsListCollection() As Boolean Implements System.ComponentModel.IListSource.ContainsListCollection
        Get
            Return Count > 0
        End Get
    End Property

    Public Function GetList() As System.Collections.IList Implements System.ComponentModel.IListSource.GetList
        Return Me
    End Function

    Private Sub DataTableToCollection(ByVal dt As DataTable)
        If dt Is Nothing Then Exit Sub
        Dim i As Integer
        Clear()
        Dim Data As DataTable = dt
        For i = 0 To Data.Rows.Count - 1
            Dim dd As T = CreateInstance()
            dd.Reload(Data.Rows(i))
            Add(dd)
        Next
    End Sub

    Private Sub RetrieveData()
        If Not sqlGen Is Nothing Then
            Dim strsql As String = sqlGen.getSelectSql()
            Try
                If sqlGen.Condition Is Nothing Then
                    _data = _DBAgent.GetDataTable(strsql)
                Else
                    _data = _DBAgent.GetDataTable(strsql, sqlGen.Condition.GetParams(_DBAgent.GetContext()))
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub RetrievePageData(ByVal OrderField As String, _
                                 ByVal OrderKind As Integer, _
                                 ByVal PageIndex As Integer, Optional ByVal PageSize As Integer = 20)
        If Not sqlGen Is Nothing Then
            Dim strPageSql As String = sqlGen.getSelectPageSql(OrderField, OrderKind, PageIndex, PageSize)
            Try
                If sqlGen.Condition Is Nothing Then
                    _data = _DBAgent.GetDataTable(strPageSql)
                Else
                    _data = _DBAgent.GetDataTable(strPageSql, sqlGen.Condition.GetParams(_DBAgent.GetContext()))
                End If
            Catch ex As Exception

            End Try
        End If
        _pageSize = PageSize
    End Sub
    ''' <summary>
    ''' 创建对应类型的一个新实例
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateInstance() As T
        Dim obj As New T
        obj.AssignDBA(_DBAgent)
        obj.SetDBName(_DBName)
        obj.SetSchemaName(_SchemaName)
        Return obj
    End Function

    ''' <summary>
    ''' 指定要返回的属性列表
    ''' </summary>
    ''' <param name="selectFields">用,号分隔的属性字符串</param>
    ''' <remarks></remarks>
    Public Sub [Select](ByVal selectFields As String)
        _selectFields = selectFields
        If Not sqlGen Is Nothing Then sqlGen.SelectFields = _selectFields
    End Sub
    ''' <summary>
    ''' 指定查询条件
    ''' </summary>
    ''' <param name="condition">查询条件</param>
    ''' <remarks></remarks>
    Public Sub Where(ByVal condition As CriteriaOperator)
        If Not sqlGen Is Nothing Then sqlGen.Condition = condition
    End Sub
    ''' <summary>
    ''' 指定排序规则
    ''' </summary>
    ''' <param name="order">排序规则，格式为" FieldName [asc|desc], …"</param>
    ''' <remarks></remarks>
    Public Sub Order(ByVal order As OrderCollection)
        If Not sqlGen Is Nothing Then sqlGen.Order = order
    End Sub

    Public Sub Distinct()
        If Not sqlGen Is Nothing Then sqlGen.Distinct = True
    End Sub

    Public Sub Top(ByVal value As Integer)
        If Not sqlGen Is Nothing Then sqlGen.TopRowNum = value
    End Sub
    ''' <summary>
    ''' 获取数据并以DataTable返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AsDataTable() As DataTable
        RetrieveData()
        Return _data
    End Function
    ''' <summary>
    ''' 获取数据并以强类型集合返回
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AsCollection() As DataObjectCollection(Of T)
        RetrieveData()
        DataTableToCollection(_data)
        Return Me
    End Function
    ''' <summary>
    ''' 获取分页数据并以DataTable返回
    ''' </summary>
    ''' <param name="OrderField">主排序字段</param>
    ''' <param name="OrderKind">主排序类型，0代表升序，大于0代表降序</param>
    ''' <param name="PageIndex">页面索引</param>
    ''' <param name="PageSize">每页记录数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AsPageTable(ByVal OrderField As String, _
                                 ByVal OrderKind As Integer, _
                                 ByVal PageIndex As Integer, Optional ByVal PageSize As Integer = 20) As DataTable
        RetrievePageData(OrderField, OrderKind, PageIndex, PageSize)
        Return _data
    End Function
    ''' <summary>
    ''' 获取分页数据并以强类型集合返回
    ''' </summary>
    ''' <param name="OrderField">主排序字段</param>
    ''' <param name="OrderKind">主排序类型，0代表升序，大于0代表降序</param>
    ''' <param name="PageIndex">页面索引</param>
    ''' <param name="PageSize">每页记录数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AsPageCollection(ByVal OrderField As String, _
                                 ByVal OrderKind As Integer, _
                                 ByVal PageIndex As Integer, Optional ByVal PageSize As Integer = 20) As DataObjectCollection(Of T)
        RetrievePageData(OrderField, OrderKind, PageIndex, PageSize)
        DataTableToCollection(_data)
        Return Me
    End Function

    Public Function GetCount() As Integer
        If Not sqlGen Is Nothing Then
            Dim strsql As String = sqlGen.getSelectCountSql()
            Try
                Dim dt As DataTable
                If sqlGen.Condition Is Nothing Then
                    dt = _DBAgent.GetDataTable(strsql)
                Else
                    dt = _DBAgent.GetDataTable(strsql, sqlGen.Condition.GetParams(_DBAgent.GetContext()))
                End If
                Return dt.Rows(0)(0)
            Catch ex As Exception
                Return 0
            End Try
        End If
    End Function
    ''' <summary>
    ''' 从XML内容中加载数据，XML格式与DataSet导出XML格式兼容
    ''' </summary>
    ''' <param name="XmlData">包括数据的XML字符串</param>
    ''' <remarks></remarks>
    Public Sub FromXML(ByVal XmlData As String)
        If XmlData Is Nothing OrElse XmlData.Trim() = "" Then
            XmlData = "<" & GetType(T).Name & "DS/>"
        End If
        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.LoadXml(XmlData)
        If xmlDoc.FirstChild.Name = GetType(T).Name & "DS" Then
            Dim xmlList As Xml.XmlNodeList = xmlDoc.FirstChild.ChildNodes
            Dim i As Integer
            Clear()
            For i = 0 To xmlList.Count - 1
                Dim obj As New T()
                obj.FromXML(xmlList.Item(i).OuterXml)
                Add(obj)
            Next
        End If
        sqlGen = Nothing
    End Sub
    ''' <summary>
    ''' 将强类型集合导出为与DataSet导出的XML格式兼容的XML数据
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToXML() As String
        Dim obj As T
        Dim BeginTag As String = "<" & GetType(T).Name & "DS>"
        Dim EndTag As String = "</" & GetType(T).Name & "DS>"
        Dim strXML As String = BeginTag
        For Each obj In Me
            strXML &= obj.ToXML()
        Next
        Return strXML & EndTag
    End Function

    Public Function ToJson(Optional ByVal PageEnabled As Boolean = False) As String
        Dim obj As T
        Dim sJson As New System.Text.StringBuilder()
        sJson.Append("{")
        sJson.Append(GetType(T).Name)
        sJson.Append(":[")
        For i As Integer = 0 To Me.Count - 1
            obj = Me.Item(i)
            sJson.Append(obj.ToJson())
            If i < Me.Count - 1 Then
                sJson.Append(",")
            End If
        Next
        sJson.Append("]")
        If PageEnabled Then
            Dim RecCount As Integer = GetCount()
            Dim PageCount As Integer
            If _pageSize < 1 Then
                PageCount = 1
            Else
                PageCount = RecCount \ _pageSize
                If RecCount Mod _pageSize > 0 Then PageCount += 1
            End If
            sJson.Append(",PageCount:'")
            sJson.Append(PageCount.ToString())
            sJson.Append("',PageSize:'")
            sJson.Append(_pageSize.ToString())
            sJson.Append("',RecCount:'")
            sJson.Append(RecCount.ToString)
            sJson.Append("'")
        End If
        sJson.Append("}")
        Return sJson.ToString()
    End Function
    ''' <summary>
    ''' 提交全部更改到数据库
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CommitChanges() As Boolean
        Dim i As Integer
        Dim Result As Boolean = True
        _DBAgent.BeginTrans()
        Dim params As New List(Of IDataParameter)
        For i = 0 To Count - 1
            If Item(i).IsDeleted Then
                Result = Result And Item(i).Delete()
            Else
                Result = Result And Item(i).Save()
            End If
        Next

        If Result Then
            Try
                _DBAgent.CommitTrans()
                Result = True
            Catch ex As Exception
                _DBAgent.RollbackTrans()
                Result = False
            End Try
        Else
            _DBAgent.RollbackTrans()
            Result = False
        End If
        Return Result
    End Function

    ''' <summary>
    ''' 从数据库中删除指定条件数据（在WHERE函数里面指定条件）
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete() As Boolean
        If Not sqlGen Is Nothing Then
            Dim strSql As String = sqlGen.getDeleteSql()
            If sqlGen.Condition Is Nothing Then
                Return _DBAgent.ExecSql(strSql)
            Else
                Return _DBAgent.ExecSql(strSql, sqlGen.Condition.GetParams(Me._DBAgent.GetContext()))
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetLastErrorMsg() As String
        If _DBAgent Is Nothing Then
            Return ""
        Else
            Return _DBAgent.LastErrorMsg
        End If
    End Function

End Class