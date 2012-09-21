
Imports System
Imports System.Data
Imports System.Data.Common
Imports Donglisoft.Entlib.DataAccess
Imports Donglisoft.Entlib.Common
Imports Donglisoft.Entlib.Exceptions
''' <summary>
''' 数据库类型
''' </summary>
''' <remarks></remarks>
Public Enum UserDBProvider
    MSSqlServer = 0
    Access = 1
    Oracle = 2
    MySql = 3
End Enum
''' <summary>
''' 数据库操作类
''' </summary>
''' <remarks></remarks>
Public Class UserDBAgent
    Implements IDisposable

    Private _conn As IDbConnection
    Private _provider As UserDBProvider
    Private _trans As IDbTransaction
    Private _lastErrorMsg As String

    Public Sub New()
        Me.New(Settings.DBContext.GetDBType(), Settings.DBContext.GetConnectionString())
    End Sub

    Public Sub New(ByVal connectionString As String)
        _lastErrorMsg = ""
        _provider = UserDBProvider.MSSqlServer
        _conn = New SqlClient.SqlConnection(connectionString)
    End Sub

    Public Sub New(ByVal provider As UserDBProvider, ByVal connectionString As String)
        _lastErrorMsg = ""
        _provider = provider
        Select Case provider
            Case UserDBProvider.Access
                _conn = New OleDb.OleDbConnection(connectionString)
            Case UserDBProvider.Oracle
                _conn = New Oracle.DataAccess.Client.OracleConnection(connectionString)
            Case UserDBProvider.MySql
                _conn = New MySql.Data.MySqlClient.MySqlConnection(connectionString)
            Case Else
                _conn = New SqlClient.SqlConnection(connectionString)
        End Select
    End Sub

    Public Sub New(ByVal Context As Settings.DBContext)
        _lastErrorMsg = ""
        _provider = Context.DBType
        Dim connectionString As String = Context.ConnectionString
        Select Case _provider
            Case UserDBProvider.Access
                _conn = New OleDb.OleDbConnection(connectionString)
            Case UserDBProvider.Oracle
                _conn = New Oracle.DataAccess.Client.OracleConnection(connectionString)
            Case UserDBProvider.MySql
                _conn = New MySql.Data.MySqlClient.MySqlConnection(connectionString)
            Case Else
                _conn = New SqlClient.SqlConnection(connectionString)
        End Select
    End Sub

    Public Function GetContext() As Settings.DBContext
        Dim _Context As Settings.DBContext
        _Context = New Settings.DBContext()
        _Context.DBType = _provider
        _Context.ConnectionString = _conn.ConnectionString
        Return _Context
    End Function
    ''' <summary>
    ''' 获取最后一条出错的错误消息
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LastErrorMsg() As String
        Get
            Return _lastErrorMsg
        End Get
    End Property

    Public Function GetDBType(ByVal VBType As Type, Optional ByVal Length As Integer = 0, Optional ByVal Identity As Boolean = False) As Integer
        Select Case _provider
            Case UserDBProvider.Access
                Return AccessProvider.GetDBType(VBType, Length, Identity)
            Case UserDBProvider.Oracle
                Return 0
            Case UserDBProvider.MySql
                Return 0
            Case Else
                Return DataAccess.MsSqlProvider.GetDBType(VBType, Length, Identity)
        End Select
    End Function

    Public Function GetVBType(ByVal DBType As String) As String
        Select Case _provider
            Case UserDBProvider.Access
                Return AccessProvider.GetVBType(DBType)
            Case UserDBProvider.Oracle
                Return ""
            Case UserDBProvider.MySql
                Return ""
            Case Else
                Return DataAccess.MsSqlProvider.GetVBType(DBType)
        End Select
    End Function

    Public Function GetProvider() As UserDBProvider
        Return _provider
    End Function

#Region "创建通用接口"
    ''' <summary>
    ''' 创建通用参数
    ''' </summary>
    ''' <param name="ParamName">参数名称</param>
    ''' <param name="ParamValue">参数值</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateParameter(ByVal ParamName As String, ByVal ParamValue As Object) As IDataParameter
        Dim infParam As IDataParameter

        Dim pre As String = GetParamPrev()
        If Not ParamName.StartsWith(pre) Then ParamName = pre + ParamName

        Select Case _provider
            Case UserDBProvider.Access
                infParam = New OleDb.OleDbParameter(ParamName, ParamValue)
            Case UserDBProvider.Oracle
                Dim ov As Object
                If ParamValue.GetType().Name.IndexOf("Date") >= 0 Then
                    ov = CDate(ParamValue).ToString("yyyy-MM-dd HH:mm:ss")
                ElseIf ParamValue.GetType().Name = "Boolean" Then
                    ov = CInt(ParamValue)
                Else
                    ov = ParamValue
                End If

                infParam = New Oracle.DataAccess.Client.OracleParameter(ParamName, ov)
            Case UserDBProvider.MySql
                infParam = New MySql.Data.MySqlClient.MySqlParameter(ParamName, ParamValue)
            Case Else
                infParam = New SqlClient.SqlParameter(ParamName, ParamValue)
        End Select
        Return infParam
    End Function
    ''' <summary>
    ''' 创建返回参数
    ''' </summary>
    ''' <param name="ParamName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateOutputParameter(ByVal ParamName As String, Optional ByVal ParamType As SqlDbType = SqlDbType.NVarChar, Optional ByVal ParamSize As Integer = 0) As IDataParameter
        Dim infParam As IDataParameter
        Select Case _provider
            Case UserDBProvider.Access
                infParam = Nothing
            Case UserDBProvider.Oracle
                infParam = New Oracle.DataAccess.Client.OracleParameter(ParamName, _
                                                                        Oracle.DataAccess.Client.OracleDbType.RefCursor, _
                                                                        ParameterDirection.Output)
            Case UserDBProvider.MySql
                infParam = New MySql.Data.MySqlClient.MySqlParameter(ParamName, ParamType, ParamSize)
                infParam.Direction = ParameterDirection.Output
            Case UserDBProvider.MSSqlServer
                infParam = New SqlClient.SqlParameter(ParamName, ParamType, ParamSize)
                infParam.Direction = ParameterDirection.Output
            Case Else
                infParam = Nothing
        End Select
        Return infParam
    End Function
    ''' <summary>
    ''' 创建通用数据适配器
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateDataAdapter() As IDbDataAdapter
        Dim infAdapt As IDbDataAdapter
        Select Case _provider
            Case UserDBProvider.Access
                infAdapt = New OleDb.OleDbDataAdapter()
            Case UserDBProvider.Oracle
                infAdapt = New Oracle.DataAccess.Client.OracleDataAdapter()
            Case UserDBProvider.MySql
                infAdapt = New MySql.Data.MySqlClient.MySqlDataAdapter()
            Case Else
                infAdapt = New SqlClient.SqlDataAdapter()
        End Select
        Return infAdapt
    End Function
    ''' <summary>
    ''' 自动构建插入、更新和删除语句
    ''' </summary>
    ''' <param name="adapt"></param>
    ''' <remarks></remarks>
    Public Sub BuilderCommand(ByVal adapt As IDbDataAdapter)
        Dim Builder As ComponentModel.Component
        Select Case _provider
            Case UserDBProvider.Access
                Builder = New OleDb.OleDbCommandBuilder(adapt)
            Case UserDBProvider.Oracle
                Builder = New Oracle.DataAccess.Client.OracleCommandBuilder(adapt)
            Case UserDBProvider.MySql
                Builder = New MySql.Data.MySqlClient.MySqlCommandBuilder(adapt)
            Case Else
                Builder = New SqlClient.SqlCommandBuilder(adapt)
        End Select

    End Sub

#End Region

    Public Function GetParamPrev() As String
        Select Case _provider
            Case UserDBProvider.Oracle
                Return ":"
            Case UserDBProvider.MySql
                Return "?"
            Case Else
                Return "@"
        End Select
    End Function

    Public Shared Function GetParamPrev(ByVal Provider As UserDBProvider) As String
        Select Case Provider
            Case UserDBProvider.Oracle
                Return ":"
            Case UserDBProvider.MySql
                Return "?"
            Case Else
                Return "@"
        End Select
    End Function

    Public Function GetQuotedField(ByVal FieldName As String) As String
        Select Case _provider
            Case UserDBProvider.MySql, UserDBProvider.Oracle
                Return FieldName
            Case Else
                Return "[" + FieldName + "]"
        End Select
    End Function

    Private Sub OpenConn()
        Select Case _conn.State
            Case ConnectionState.Broken
                _conn.Close()
                _conn.Open()
            Case ConnectionState.Closed
                _conn.Open()
        End Select
    End Sub

    Private Sub CloseConn()
        If Not _trans Is Nothing Then
            Try
                _trans.Commit()
                _trans = Nothing
            Catch ex As Exception
                _lastErrorMsg = ex.Message
                _trans.Rollback()
                _trans = Nothing
            End Try
        End If
        Select Case _conn.State
            Case ConnectionState.Closed
            Case Else
                _conn.Close()
        End Select
    End Sub

    Private Sub CloseConnNoTrans()
        If _trans Is Nothing Then
            Select Case _conn.State
                Case ConnectionState.Closed
                Case Else
                    _conn.Close()
            End Select
        End If
    End Sub

#Region "事务处理函数"
    Public Sub BeginTrans()
        OpenConn()
        Try
            _trans = _conn.BeginTransaction()
        Catch ex As Exception
            _lastErrorMsg = ex.Message
        End Try

    End Sub

    Public Sub CommitTrans()
        If _trans Is Nothing Then
            Throw New UserDBException(UserDBException.ErrorCodes.CommitTransError)
        Else
            _trans.Commit()
            _trans = Nothing
            CloseConn()
        End If
    End Sub

    Public Sub RollbackTrans()
        If _trans Is Nothing Then
            Throw New UserDBException(UserDBException.ErrorCodes.RollbackTransError)
        Else
            _trans.Rollback()
            _trans = Nothing
            CloseConn()
        End If
    End Sub

    Public Function GetTrans() As IDbTransaction
        Return _trans
    End Function
    ''' <summary>
    ''' 此方法仅适用于SqlServer数据库，不推荐使用，只用于兼容旧版本程序
    ''' </summary>
    ''' <param name="SavePointName"></param>
    ''' <remarks></remarks>
    Public Sub SaveTrans(Optional ByVal SavePointName As String = "")
        If String.IsNullOrEmpty(SavePointName) Then SavePointName = "save"
        If GetProvider() = UserDBProvider.MSSqlServer Then
            CType(_trans, System.Data.SqlClient.SqlTransaction).Save(SavePointName)
        End If
    End Sub

#End Region

#Region "普通数据操作方法"
    ''' <summary>
    ''' 执行普通SQL语句，不返回数据
    ''' </summary>
    ''' <param name="sql">执行的SQL语句</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecSql(ByVal sql As String, Optional ByVal IsAutoError As Boolean = False) As Boolean
        Dim cmd As IDbCommand = _conn.CreateCommand()
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.Connection = _conn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        Try
            OpenConn()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), sql, ex.Message)
            Return False
        Finally
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 执行带参数的SQL语句，不返回数据
    ''' </summary>
    ''' <param name="sql">执行的带参数的SQL语句</param>
    ''' <param name="params">参数列表</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecSql(ByVal sql As String, ByVal params As List(Of IDataParameter), _
                            Optional ByVal IsAutoError As Boolean = False) As Boolean
        Dim i As Integer
        Dim cmd As IDbCommand = _conn.CreateCommand()
        If _provider = UserDBProvider.Oracle Then
            CType(cmd, Oracle.DataAccess.Client.OracleCommand).BindByName = True
        End If
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.Connection = _conn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        For i = 0 To params.Count - 1
            cmd.Parameters.Add(params(i))
        Next
        cmd.CommandText = sql
        Try
            OpenConn()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message + "sql:" + sql
            Log.WriteLog_Database(Me.GetType().ToString(), sql, ex.Message)
            Return False
        Finally
            cmd.Parameters.Clear()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 执行不带参数的存储过程
    ''' </summary>
    ''' <param name="procName">存储过程名称</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecProc(ByVal procName As String, Optional ByVal IsAutoError As Boolean = False) As Boolean

        Dim cmd As IDbCommand = _conn.CreateCommand()

        cmd.Connection = _conn
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procName
        cmd.CommandTimeout = 0  '设置执行不超时
        Try
            OpenConn()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), "procedure:" & procName, ex.Message)
            Return False
        Finally
            '_conn.Close()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 执行带参数的存储过程
    ''' </summary>
    ''' <param name="procName">存储过程名称</param>
    ''' <param name="params">参数列表</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExecProc(ByVal procName As String, ByRef params As List(Of IDataParameter), _
                             Optional ByVal IsAutoError As Boolean = False) As Boolean
        Dim i As Integer
        Dim cmd As IDbCommand = _conn.CreateCommand()
        If _provider = UserDBProvider.Oracle Then
            CType(cmd, Oracle.DataAccess.Client.OracleCommand).BindByName = True
        End If
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procName
        For i = 0 To params.Count - 1
            cmd.Parameters.Add(params(i))
        Next
        Try
            OpenConn()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), "procedure:" & procName, ex.Message)
            Return False
        Finally
            cmd.Parameters.Clear()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 通过普通SQL获取数据集
    ''' </summary>
    ''' <param name="sql">获取数据的SQL语句</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal sql As String, Optional ByVal IsAutoError As Boolean = False) As DataSet
        Dim ds As New DataSet()
        Dim cmd As IDbCommand = _conn.CreateCommand()
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        Try
            Dim adapt As IDbDataAdapter = CreateDataAdapter()
            adapt.SelectCommand = cmd
            adapt.Fill(ds)
            Return ds
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), sql, ex.Message)
            Return Nothing
        Finally
            '_conn.Close()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 通过普通SQL获取数据集，并返回数据适配器
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <param name="Adapt"></param>
    ''' <param name="IsAutoError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal sql As String, ByRef Adapt As IDbDataAdapter, _
                               Optional ByVal IsAutoError As Boolean = False) As DataSet
        Dim ds As New DataSet()
        Dim cmd As IDbCommand = _conn.CreateCommand()
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        Adapt = CreateDataAdapter()
        Try
            Adapt.SelectCommand = cmd
            BuilderCommand(Adapt)
            Adapt.Fill(ds)
            Return ds
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), sql, ex.Message)
            Return Nothing
        Finally
            '_conn.Close()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function

    ''' <summary>
    ''' 通过带参数的SQL语句获取数据集,存储过程方法请使用GetProcData函数
    ''' </summary>
    ''' <param name="sql">带参数的SQL语句</param>
    ''' <param name="params">参数列表</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal sql As String, ByVal params As List(Of IDataParameter), _
                            Optional ByVal IsAutoError As Boolean = False) As DataSet
        Dim ds As New DataSet()
        Dim i As Integer
        Dim cmd As IDbCommand = _conn.CreateCommand()
        If _provider = UserDBProvider.Oracle Then
            CType(cmd, Oracle.DataAccess.Client.OracleCommand).BindByName = True
        End If
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.Text
        cmd.CommandText = sql
        For i = 0 To params.Count - 1
            cmd.Parameters.Add(params(i))
        Next
        Try
            Dim adapt As IDbDataAdapter = CreateDataAdapter()
            adapt.SelectCommand = cmd
            adapt.Fill(ds)
            Return ds
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), sql, ex.Message)
            Return Nothing
        Finally
            '_conn.Close()
            cmd.Parameters.Clear()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 获取不带参数的存储过程返回的数据
    ''' </summary>
    ''' <param name="procName">存储过程名称</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcData(ByVal procName As String, Optional ByVal IsAutoError As Boolean = False) As DataSet
        Dim ds As New DataSet()

        Dim cmd As IDbCommand = _conn.CreateCommand()
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procName
        Try
            Dim adapt As IDbDataAdapter = CreateDataAdapter()
            adapt.SelectCommand = cmd
            adapt.Fill(ds)
            Return ds
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), "procedure:" & procName, ex.Message)
            Return Nothing
        Finally
            '_conn.Close()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function
    ''' <summary>
    ''' 获取带参数的存储过程数据
    ''' </summary>
    ''' <param name="procName">存储过程名称</param>
    ''' <param name="params">参数列表</param>
    ''' <param name="IsAutoError">标明是否自动提示错误信息</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcData(ByVal procName As String, ByVal params As List(Of IDataParameter), _
                            Optional ByVal IsAutoError As Boolean = False) As DataSet
        Dim ds As New DataSet()
        Dim i As Integer
        Dim cmd As IDbCommand = _conn.CreateCommand()
        'ODP.NET参数默认的绑定方式是按位置不是按名称，要设置按名称绑定才能正确按参数名进行匹配
        If _provider = UserDBProvider.Oracle Then
            CType(cmd, Oracle.DataAccess.Client.OracleCommand).BindByName = True
        End If
        cmd.Connection = _conn
        cmd.CommandTimeout = 0  '设置执行不超时
        If Not _trans Is Nothing Then cmd.Transaction = _trans
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procName
        '标识是否有返回参数，主要用于oracle数据库返回数据集
        Dim bHasOutputParam As Boolean = False
        For i = 0 To params.Count - 1
            cmd.Parameters.Add(params(i))
            bHasOutputParam = bHasOutputParam Or (params(i).Direction = ParameterDirection.Output)
        Next
        If Not bHasOutputParam And _provider = UserDBProvider.Oracle Then
            cmd.Parameters.Add(CreateOutputParameter("RETCUR"))
        End If
        Try
            Dim adapt As IDbDataAdapter = CreateDataAdapter()
            adapt.SelectCommand = cmd
            adapt.Fill(ds)
            Return ds
        Catch ex As Exception
            'If IsAutoError Then InfoTip.ShowWebMessage("数据库操作失败！")
            _lastErrorMsg = ex.Message
            Log.WriteLog_Database(Me.GetType().ToString(), "procedure:" & procName, ex.Message)
            Return Nothing
        Finally
            '_conn.Close()
            cmd.Parameters.Clear()
            cmd = Nothing
            CloseConnNoTrans()
        End Try
    End Function

    Public Function GetDataTable(ByVal sql As String, Optional ByVal IsAutoError As Boolean = False) As DataTable
        Return GetDataSet(sql, IsAutoError).Tables(0)
    End Function

    Public Function GetDataTable(ByVal sql As String, ByVal params As List(Of IDataParameter), _
                            Optional ByVal IsAutoError As Boolean = False) As DataTable
        Return GetDataSet(sql, params, IsAutoError).Tables(0)
    End Function

    Public Function FillDropDownList(ByVal lSQL As String, ByRef DDL As System.Web.UI.WebControls.DropDownList, _
                                     ByVal lTextField As String, Optional ByVal lValueField As String = "", _
                                     Optional ByVal strFirstDefault As String = "", _
                                     Optional ByVal ClearIt As Boolean = True, _
                                     Optional ByVal IsAutoError As Boolean = False, _
                                     Optional ByVal ErrorMessage As String = "") As String
        Dim ErrText As String = ""
        Dim objDataTable As DataTable
        Dim i As Integer
        If ClearIt Then
            DDL.Items.Clear()
        End If
        objDataTable = Me.GetDataTable(lSQL)

        If Trim(strFirstDefault) <> "" Then '默认选择的添加
            DDL.Items.Add(strFirstDefault)
            DDL.Items.Item(DDL.Items.Count - 1).Value = ""
        End If

        If objDataTable.Rows.Count > 0 Then
            For i = 0 To objDataTable.Rows.Count - 1
                DDL.Items.Add(objDataTable.Rows(i).Item(lTextField)) '添加字段
                If Trim(lValueField) <> "" Then
                    DDL.Items.Item(DDL.Items.Count - 1).Value = Trim(CStr(objDataTable.Rows(i).Item(lValueField))) '添加相应的值
                End If
            Next
        End If
        If IsAutoError = True Then
            ErrText = "绑定数据出错："
            If ErrorMessage <> "" Then
                InfoTip.ShowWebMessage(ErrorMessage)
            Else
                InfoTip.ShowWebMessage(ErrText)
            End If
        End If
        Return ErrText
    End Function

    Public Function FillListBox(ByVal lSQL As String, ByRef DDL As System.Web.UI.WebControls.ListBox, _
                                 ByVal lTextField As String, Optional ByVal lValueField As String = "", _
                                 Optional ByVal strFirstDefault As String = "", _
                                 Optional ByVal ClearIt As Boolean = True, _
                                 Optional ByVal IsAutoError As Boolean = False, _
                                 Optional ByVal ErrorMessage As String = "") As String
        Dim ErrText As String = ""
        Dim objDataTable As DataTable
        Dim i As Integer
        If ClearIt Then
            DDL.Items.Clear()

        End If
        objDataTable = Me.GetDataTable(lSQL)

        If Trim(strFirstDefault) <> "" Then '默认选择的添加
            DDL.Items.Add(strFirstDefault)
            DDL.Items.Item(DDL.Items.Count - 1).Value = ""
        End If

        If objDataTable.Rows.Count > 0 Then
            For i = 0 To objDataTable.Rows.Count - 1
                DDL.Items.Add(objDataTable.Rows(i).Item(lTextField)) '添加字段
                If Trim(lValueField) <> "" Then
                    DDL.Items.Item(DDL.Items.Count - 1).Value = objDataTable.Rows(i).Item(lValueField) '添加相应的值
                End If
            Next
        End If
        If IsAutoError = True And ErrText <> "" Then
            ErrText = "绑定数据出错"
            If ErrorMessage <> "" Then
                InfoTip.ShowWebMessage(ErrorMessage)
            Else
                InfoTip.ShowWebMessage(ErrText)
            End If
        End If
        Return ErrText
    End Function
#End Region

#Region "数据实体操作方法"
    ''' <summary>
    ''' 根据主键重新加载实体数据
    ''' </summary>
    ''' <param name="obj">要加载的实体数据类型</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Reload(ByRef obj As DataObject) As Boolean
        Dim dt As DataTable = Nothing
        Dim i As Integer
        Dim PrimaryKeys() As String = obj.GetPrimaryKeys()
        Dim LoadFields As PersistFieldCollection = obj.GetFieldsInfo
        Dim sCon As String = " where 1=1 "
        For i = PrimaryKeys.GetLowerBound(0) To PrimaryKeys.GetUpperBound(0)
            Dim sKeySearch As String
            Dim pfi As PersistField = LoadFields.Item(PrimaryKeys(i))
            sKeySearch = " and {0} = {1} "
            sCon &= String.Format(sKeySearch, GetQuotedField(PrimaryKeys(i)), GetParamPrev() & PrimaryKeys(i))
        Next

        If LoadFields.Count > 0 Then
            Dim sd As String = "select {0} from {1} "
            Dim s0, s1 As String
            s0 = ""
            s1 = obj.GetMapTo()
            For i = 0 To LoadFields.Count - 1
                If Not LoadFields.Item(i).IsNonPersisted Or LoadFields.Item(i).IsPrimary Then
                    s0 &= GetQuotedField(LoadFields.Item(i).MapTo) & ","
                End If

            Next
            If s0.EndsWith(",") Then s0 = s0.Remove(s0.Length - 1, 1)
            Dim strSql As String = String.Format(sd, s0, s1) & sCon
            Dim params As New List(Of IDataParameter)(PrimaryKeys.GetLength(0))
            For i = PrimaryKeys.GetLowerBound(0) To PrimaryKeys.GetUpperBound(0)
                Dim pfi As PersistField = LoadFields.Item(PrimaryKeys(i))
                Dim param As IDataParameter = CreateParameter(GetParamPrev() & PrimaryKeys(i), pfi.Value)
                params.Add(param)
            Next
            dt = GetDataTable(strSql, params, False)
        End If
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return False
        Else
            For i = 0 To LoadFields.Count - 1
                Dim pf As PersistField = LoadFields.Item(i)
                If Not pf.IsPrimary Then
                    obj.SetMemberValue(pf.Name, dt.Rows(0)(pf.MapTo))
                End If
            Next
            Return True
        End If
    End Function
    ''' <summary>
    ''' 从数据行中加载实体
    ''' </summary>
    ''' <param name="obj">要加载的实体对象</param>
    ''' <param name="Data">包含实体数据的数据行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Reload(ByRef obj As DataObject, ByVal Data As DataRow) As Boolean
        Dim dt As DataTable = Nothing
        Dim i As Integer
        Dim LoadFields As PersistFieldCollection = obj.GetFieldsInfo
        If Data Is Nothing Then
            Return False
        Else
            For i = 0 To Data.Table.Columns.Count - 1
                Dim colName As String = Data.Table.Columns(i).ColumnName
                Dim pf As PersistField = LoadFields.Item(colName, True)
                If Not pf Is Nothing AndAlso Not pf.IsNonPersisted Then
                    Dim value As Object = Data(i)
                    obj.SetMemberValue(pf.Name, value)
                End If

            Next
            Return True
        End If
    End Function

    ''' <summary>
    ''' 插入实体数据到数据库
    ''' </summary>
    ''' <param name="obj">要插入的实体数据类型</param>
    ''' <param name="BatchMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Insert(ByVal obj As DataObject, Optional ByVal BatchMode As Boolean = False) As Boolean
        Dim _sqlGen As New SqlGenerator(obj, _provider)
        Dim es As EntitySql = _sqlGen.getEntityInsertSql(BatchMode)
        If BatchMode Then
            Return True
        Else
            If es.Sql.Trim() = "" Then
                Return True
            Else
                Return ExecSql(es.Sql, es.ParamList, False)
            End If
        End If

    End Function
    ''' <summary>
    ''' 更新实体数据到数据库
    ''' </summary>
    ''' <param name="obj">要更新的实体数据类型</param>
    ''' <param name="BatchMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Update(ByVal obj As DataObject, Optional ByVal BatchMode As Boolean = False) As Boolean
        Dim _sqlGen As New SqlGenerator(obj, _provider)
        Dim es As EntitySql = _sqlGen.getEntityUpdateSql(BatchMode)
        If BatchMode Then
            Return True
        Else
            If es.Sql = "" Then
                Return True
            Else
                Return ExecSql(es.Sql, es.ParamList, False)
            End If
        End If
    End Function
    ''' <summary>
    ''' 删除实体数据
    ''' </summary>
    ''' <param name="obj">实体对象</param>
    ''' <param name="BatchMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete(ByVal obj As DataObject, Optional ByVal BatchMode As Boolean = False) As Boolean
        Dim _sqlGen As New SqlGenerator(obj, _provider)
        Dim es As EntitySql = _sqlGen.getEntityDeleteSql(BatchMode)
        If BatchMode Then
            obj._IsDeleted = True
            Return True
        Else
            Return ExecSql(es.Sql, es.ParamList, False)
        End If
    End Function

#End Region

    Private disposedValue As Boolean = False        ' 检测冗余的调用

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 释放其他状态(托管对象)。
            End If
            CloseConn()
            ' TODO: 释放您自己的状态(非托管对象)。
            ' TODO: 将大型字段设置为 null。
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' Visual Basic 添加此代码是为了正确实现可处置模式。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入上面的 Dispose(ByVal disposing As Boolean) 中。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
''' <summary>
''' 处理实体数据操作的事务
''' </summary>
''' <remarks></remarks>
Public Class UserDBTransaction
    Private _dbAgent As UserDBAgent

    Public Sub New()
        _dbAgent = New UserDBAgent()
    End Sub

    Public Sub New(ByVal connectionString As String)
        _dbAgent = New UserDBAgent(connectionString)
    End Sub

    Public Sub New(ByVal provider As UserDBProvider, ByVal connectionString As String)
        _dbAgent = New UserDBAgent(provider, connectionString)
    End Sub

    Public Sub New(ByVal context As Settings.DBContext)
        _dbAgent = New UserDBAgent(context)
    End Sub
    ''' <summary>
    ''' 实体增加到事务
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Public Sub Add(ByVal obj As DataObject)
        obj.AssignDBA(_dbAgent)
    End Sub

    Public Sub BeginTrans()
        _dbAgent.BeginTrans()
    End Sub

    Public Sub CommitTrans()
        _dbAgent.CommitTrans()
    End Sub

    Public Sub RollbackTrans()
        _dbAgent.RollbackTrans()
    End Sub

    Public Function GetDBAgent() As UserDBAgent
        Return _dbAgent
    End Function
End Class



