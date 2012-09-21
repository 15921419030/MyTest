
Imports System
Imports System.Reflection
Imports System.ComponentModel
Imports Donglisoft.Entlib.DataAccess
Imports System.Text
Imports System.Text.RegularExpressions


''' <summary>
''' 持久域类
''' </summary>
''' <remarks></remarks>
Public Class PersistField
    ''' <summary>
    '''  实体类成员名
    ''' </summary>
    ''' <remarks></remarks>
    Public Name As String
    ''' <summary>
    ''' 标识字段对应的标题
    ''' </summary>
    ''' <remarks></remarks>
    Public Title As String
    ''' <summary>
    ''' 映射到数据库中的字段名
    ''' </summary>
    ''' <remarks></remarks>
    Public MapTo As String
    Public FieldType As Type
    Private _Value As Object
    Public Size As UInt32 = 0
    ''' <summary>
    ''' 标识主键
    ''' </summary>
    ''' <remarks></remarks>
    Public IsPrimary As Boolean = False
    ''' <summary>
    ''' 标识自增量字段
    ''' </summary>
    ''' <remarks></remarks>
    Public IsIdentity As Boolean = False
    Public IsAllowNull As Boolean = False
    ''' <summary>
    ''' 标识锁定字段
    ''' </summary>
    ''' <remarks></remarks>
    Public IsLockField As Boolean = False
    ''' <summary>
    ''' 标识排序字段
    ''' </summary>
    ''' <remarks></remarks>
    Public IsOrderField As Boolean = False
    ''' <summary>
    ''' 标识数据字典名称字段
    ''' </summary>
    ''' <remarks></remarks>
    Public IsNameField As Boolean = False
    ''' <summary>
    ''' 标明是否持久化，缺省为False；如果设为True，将不与数据库字段建立映射
    ''' </summary>
    ''' <remarks></remarks>
    Public IsNonPersisted As Boolean = False
    ''' <summary>
    ''' 标识外键
    ''' </summary>
    ''' <remarks></remarks>
    Public IsForeign As Boolean = False

    Public IsNTextOrBlob As Boolean = False

    Public ValueChanged As Boolean = False

    Public Sub New(ByVal FieldName As String)
        Name = FieldName
    End Sub

    Public Property Value() As Object
        Get
            If _Value Is Nothing Then
                If FieldType.Name = "Byte[]" Then
                    Return New Byte() {}
                Else
                    Return DBNull.Value
                End If
            Else
                Return _Value
            End If
        End Get
        Set(ByVal value As Object)
            If Not ValueChanged Then
                If value Is Nothing And _Value Is Nothing Then
                    ValueChanged = True
                ElseIf value Is Nothing And Not _Value Is Nothing Then
                    ValueChanged = True
                ElseIf Not value Is Nothing And _Value Is Nothing Then
                    ValueChanged = True
                Else
                    If value.GetType().Name = "Byte[]" Then
                        ValueChanged = True
                    Else
                        ValueChanged = (_Value <> value)
                    End If
                End If

            End If
            _Value = value
        End Set
    End Property

    Public Function GetCommitValue() As Object
        Dim ov As Object
        If Value Is Nothing Or Value Is DBNull.Value Then
            ov = DBNull.Value
        ElseIf (FieldType.Name = "Date" Or FieldType.Name = "DateTime") AndAlso Value = #12:00:00 AM# Then
            ov = DBNull.Value
        ElseIf (FieldType.Name = "String" AndAlso Value = "") Then
            ov = DBNull.Value
        ElseIf (IsForeign AndAlso _
                (FieldType.Name.StartsWith("Int") Or _
                 FieldType.Name = "Long" Or _
                 FieldType.Name = "Short" Or _
                 FieldType.Name = "Byte")) AndAlso Value = 0 Then
            ov = DBNull.Value
        Else
            ov = Value
        End If
        Return ov
    End Function

End Class
''' <summary>
''' 持久域或属性集合类
''' </summary>
''' <remarks></remarks>
Public Class PersistFieldCollection
    Private FieldList As List(Of PersistField)
    '定义当前搜索的域名称
    Private SearchName As String

    Public ReadOnly Property Count() As Integer
        Get
            Return FieldList.Count
        End Get
    End Property

    Public Sub New()
        FieldList = New List(Of PersistField)
    End Sub

    Public Sub Add(ByVal FieldInfo As PersistField)
        FieldList.Add(FieldInfo)
    End Sub

    Public Sub Clear()
        FieldList.Clear()
    End Sub

    Public Sub Delete(ByVal FieldInfo As PersistField)
        FieldList.Remove(FieldInfo)
    End Sub

    Public Sub Delete(ByVal Index As Integer)
        FieldList.RemoveAt(Index)
    End Sub

    Public Function IndexOf(ByVal FieldInfo As PersistField) As Integer
        Return FieldList.IndexOf(FieldInfo)
    End Function
    ''' <summary>
    ''' 根据实体属性名称或者映射的数据库表字段名称获取实体域
    ''' </summary>
    ''' <param name="Name">属性或者字段名</param>
    ''' <param name="ByMapTo">ByMapTo为True表明根据字段名，为False根据实体属性名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Item(ByVal Name As String, Optional ByVal ByMapTo As Boolean = False) As PersistField
        SearchName = Name
        If ByMapTo Then
            Return FieldList.Find(AddressOf FindByMapTo)
        Else
            Return FieldList.Find(AddressOf FindByName)
        End If

    End Function

    Public Function Item(ByVal Index As Integer) As PersistField
        Return FieldList.Item(Index)
    End Function

    Private Function FindByName(ByVal FieldInfo As PersistField) As Boolean
        If FieldInfo.Name.ToLower() = SearchName.ToLower() Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function FindByMapTo(ByVal FieldInfo As PersistField) As Boolean
        If FieldInfo.MapTo.ToLower() = SearchName.ToLower() Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim str As String = ""
        Dim i As Integer
        For i = 0 To Count - 1
            If Not Item(i).IsNonPersisted Then
                str &= str + Item(i).MapTo + ","
            End If
        Next
        If str.EndsWith(",") Then str = str.Remove(str.Length - 1, 1)
        Return str
    End Function

End Class
''' <summary>
''' 数据实体基类
''' </summary>
''' <remarks>所有要持久化的数据类的基类，不能直接使用，只能从此类继承</remarks>
Public MustInherit Class DataObject
    Implements INotifyPropertyChanged

    Private _DBName As String
    Private _SchemaName As String
    Private MyPrimaryKeys() As String
    Private MyPersistFields As PersistFieldCollection
    Private _MapTo As String
    Protected Friend _IsDeleted As Boolean = False
    Private _IsPersisted As Boolean
    Private _DBAgent As UserDBAgent
    Private _LockField As PersistField = Nothing
    Private _OrderField As PersistField = Nothing
    Private _NameField As PersistField = Nothing
    Protected Friend _EntitySql As EntitySql
    '调试用
    'Public DebugResult As String

    Public Function IsDeleted() As Boolean
        Return _IsDeleted
    End Function
    ''' <summary>
    ''' 如果返回True，表明数据已经存在，否则为新数据
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsPersisted() As Boolean
        Return _IsPersisted
    End Function
    ''' <summary>
    ''' 返回最后一条错误消息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastErrorMsg() As String
        If _DBAgent Is Nothing Then
            Return ""
        Else
            Return _DBAgent.LastErrorMsg
        End If

    End Function

    Protected Sub New()
        MyPersistFields = New PersistFieldCollection()
        ListFields()
    End Sub

    Protected Sub New(ByVal DBAgent As UserDBAgent)
        _DBAgent = DBAgent
        MyPersistFields = New PersistFieldCollection()
        ListFields()
    End Sub

    Protected Sub New(ByVal connectionString As String)
        _DBAgent = New UserDBAgent(connectionString)
        MyPersistFields = New PersistFieldCollection()
        ListFields()
    End Sub

    Protected Sub New(ByVal provider As UserDBProvider, ByVal connectionString As String)
        _DBAgent = New UserDBAgent(provider, connectionString)
        MyPersistFields = New PersistFieldCollection()
        ListFields()
    End Sub

    Protected Sub New(ByVal Context As Settings.DBContext)
        _DBAgent = New UserDBAgent(Context)
        MyPersistFields = New PersistFieldCollection()
        ListFields()
    End Sub

    Public Sub AssignDBA(ByVal DBAgent As UserDBAgent)
        _DBAgent = DBAgent
    End Sub
    ''' <summary>
    ''' 复制实体类
    ''' </summary>
    ''' <param name="Entity"></param>
    ''' <remarks></remarks>
    Public Sub Assign(ByVal Entity As DataObject)
        If Not Entity Is Nothing Then
            If Me.GetType().Name = Entity.GetType().Name Then
                For i = 0 To MyPersistFields.Count - 1
                    Dim sName As String = MyPersistFields.Item(i).Name
                    Entity.SetMemberValue(sName, MyPersistFields.Item(i).Value)
                Next
            End If
        End If

    End Sub

    Private Sub ListFields()
        Dim i, j As Integer
        Dim sPrimaryKey As String = ""
        Dim MyMemberInfo() As MemberInfo
        MyMemberInfo = Me.GetType().GetMembers(BindingFlags.Public Or BindingFlags.Instance)
        For i = MyMemberInfo.GetLowerBound(0) To MyMemberInfo.GetUpperBound(0)
            If MyMemberInfo(i).MemberType = MemberTypes.Field Or MyMemberInfo(i).MemberType = MemberTypes.Property Then
                Dim pf As New PersistField(MyMemberInfo(i).Name)
                pf.MapTo = pf.Name        '缺省与Name同名
                pf.IsPrimary = False      '缺省非主键
                pf.IsAllowNull = False    '缺省不充许空值
                pf.IsIdentity = False     '缺省非自增
                pf.Size = 0               '缺省为0，表示不判断Size
                pf.IsForeign = False      '缺省非外键
                '收集成员定制属性信息
                Dim obj() As Object = MyMemberInfo(i).GetCustomAttributes(True)
                For j = obj.GetLowerBound(0) To obj.GetUpperBound(0)
                    Dim myObj As Object = obj(j)
                    If TypeOf (myObj) Is MapToAttribute Then
                        pf.MapTo = CType(myObj, MapToAttribute).MapTo
                    ElseIf TypeOf (myObj) Is PrimaryKeyAttribute Then
                        pf.IsPrimary = CType(myObj, PrimaryKeyAttribute).IsPrimaryKey
                    ElseIf TypeOf (myObj) Is SizeAttribute Then
                        pf.Size = CType(myObj, SizeAttribute).Size
                    ElseIf TypeOf (myObj) Is AllowNullAttribute Then
                        pf.IsAllowNull = CType(myObj, AllowNullAttribute).IsAllowNull
                    ElseIf TypeOf (myObj) Is IdentityAttribute Then
                        pf.IsIdentity = CType(myObj, IdentityAttribute).IsIdentity
                    ElseIf TypeOf (myObj) Is LockFieldAttribute Then
                        pf.IsLockField = True
                    ElseIf TypeOf (myObj) Is OrderFieldAttribute Then
                        pf.IsOrderField = True
                    ElseIf TypeOf (myObj) Is NameFieldAttribute Then
                        pf.IsNameField = True
                    ElseIf TypeOf (myObj) Is TitleAttribute Then
                        pf.Title = CType(myObj, TitleAttribute).Title
                    ElseIf TypeOf (myObj) Is NonPersistedAttribute Then
                        pf.IsNonPersisted = True
                    ElseIf TypeOf (myObj) Is ForeignKeyAttribute Then
                        pf.IsForeign = True
                    ElseIf TypeOf (myObj) Is NTextOrBlobAttribute Then
                        pf.IsNTextOrBlob = True
                    End If
                Next
                '获取成员值
                Select Case MyMemberInfo(i).MemberType
                    Case MemberTypes.Field
                        Dim fi As FieldInfo = DirectCast(MyMemberInfo(i), FieldInfo)
                        pf.Value = fi.GetValue(Me)
                        pf.FieldType = fi.FieldType
                    Case MemberTypes.Property
                        Dim pi As PropertyInfo = DirectCast(MyMemberInfo(i), PropertyInfo)
                        pf.Value = pi.GetValue(Me, Nothing)
                        pf.FieldType = pi.PropertyType
                End Select
                '收集主键信息
                If pf.IsPrimary Then sPrimaryKey &= sPrimaryKey & pf.MapTo & ","
                '收集排序和锁定字段
                If pf.IsLockField Then
                    _LockField = pf

                    If Not (pf.FieldType.Name.StartsWith("Int") Or pf.FieldType.Name.StartsWith("UInt") Or _
                            pf.FieldType.Name = "Short" Or pf.FieldType.Name = "Long" Or pf.FieldType.Name = "Boolean") Then
                        Throw New Exceptions.UserDBException("LockField只能标识整数或布尔类型")
                    End If
                End If

                If pf.IsOrderField Then
                    _OrderField = pf
                    If Not (pf.FieldType.Name.StartsWith("Int") Or pf.FieldType.Name.StartsWith("UInt") Or _
                            pf.FieldType.Name = "Short" Or pf.FieldType.Name = "Long") Then
                        Throw New Exceptions.UserDBException("OrderField只能标识整数类型")
                    End If
                End If
                '收集用于数据字典的名称域
                If pf.IsNameField Then _NameField = pf
                MyPersistFields.Add(pf)
            End If
        Next

        '处理主键
        If sPrimaryKey.EndsWith(",") Then sPrimaryKey = sPrimaryKey.Remove(sPrimaryKey.Length - 1, 1)
        MyPrimaryKeys = sPrimaryKey.Split(",")
    End Sub
    '刷新成员变量值
    Private Sub RefreshListValue()
        Dim i As Integer
        For i = 0 To MyPersistFields.Count - 1
            If Not MyPersistFields.Item(i).IsNonPersisted Then
                SetMemberValue(MyPersistFields.Item(i).Name, GetMemberValue(MyPersistFields.Item(i).Name))
            End If

        Next
    End Sub
    ''' <summary>
    ''' 根据主键重新加载数据实体
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reload()
        RefreshListValue()
        _IsPersisted = GetDBAgent().Reload(Me)
    End Sub
    ''' <summary>
    ''' 从数据行中加载数据实体
    ''' </summary>
    ''' <param name="Data"></param>
    ''' <remarks></remarks>
    Public Sub Reload(ByVal Data As DataRow)
        _IsPersisted = GetDBAgent().Reload(Me, Data)
    End Sub
    ''' <summary>
    ''' 保存实体数据到数据库中
    ''' </summary>
    ''' <param name="BatchMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Save(Optional ByVal BatchMode As Boolean = False) As Boolean
        Dim dbAgent As UserDBAgent = GetDBAgent()
        Dim bResult As Boolean
        RefreshListValue()
        If _IsPersisted Then
            bResult = dbAgent.Update(Me, BatchMode)
        Else
            If dbAgent.Insert(Me, BatchMode) Then
                _IsPersisted = True
                bResult = True
            Else
                bResult = False
            End If
        End If
        If bResult Then
            Dim i As Integer
            For i = 0 To MyPersistFields.Count - 1
                MyPersistFields.Item(i).ValueChanged = False
            Next
        End If
        Return bResult
    End Function
    ''' <summary>
    ''' 从数据库中删除实体数据
    ''' </summary>
    ''' <param name="BatchMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete(Optional ByVal BatchMode As Boolean = False) As Boolean

        If _IsPersisted Then
            Return GetDBAgent().Delete(Me, BatchMode)
        Else
            Return False
        End If
    End Function

    Public Function GetDBName() As String
        If Not _DBName Is Nothing AndAlso _DBName <> "" Then Return _DBName
        Dim obj() As Object = Me.GetType().GetCustomAttributes(True)
        Dim sDBName As String = ""
        Dim i As Integer
        For i = obj.GetLowerBound(0) To obj.GetUpperBound(0)
            If TypeOf (obj(i)) Is DBNameAttribute Then
                sDBName = CType(obj(i), DBNameAttribute).DBName
            End If
        Next
        If Not (sDBName = "" Or sDBName Is Nothing) Then
            Dim pd As UserDBProvider = GetDBAgent().GetProvider()
            If pd = UserDBProvider.MSSqlServer Then
                sDBName = "[" + sDBName + "]"
            ElseIf pd = UserDBProvider.Oracle Then
                sDBName = """" + sDBName.ToUpper() + """"
            End If
        End If
        Return sDBName
    End Function

    Public Sub SetDBName(ByVal DBName As String)
        _DBName = DBName
    End Sub

    Public Function GetSchemaName() As String
        If Not _SchemaName Is Nothing AndAlso _SchemaName <> "" Then Return _SchemaName
        Dim obj() As Object = Me.GetType().GetCustomAttributes(True)
        Dim strSchemaName As String = ""
        Dim i As Integer
        For i = obj.GetLowerBound(0) To obj.GetUpperBound(0)
            If TypeOf (obj(i)) Is SchemaAttribute Then
                strSchemaName = CType(obj(i), SchemaAttribute).Schema
            End If
        Next
        If Not (strSchemaName = "" Or strSchemaName Is Nothing) Then
            Dim pd As UserDBProvider = GetDBAgent().GetProvider()
            If pd = UserDBProvider.MSSqlServer Then
                strSchemaName = "[" + strSchemaName + "]"
            ElseIf pd = UserDBProvider.Oracle Then
                strSchemaName = """" + strSchemaName.ToUpper() + """"
            End If
        End If
        Return strSchemaName
    End Function

    Public Sub SetSchemaName(ByVal SchemaName As String)
        _SchemaName = SchemaName
    End Sub
    ''' <summary>
    ''' 获取映射的表字段名称
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMapTo() As String
        Dim sMapTo As String = ""
        If Not _MapTo Is Nothing AndAlso _MapTo <> "" Then
            sMapTo = _MapTo
        Else
            Dim obj() As Object = Me.GetType().GetCustomAttributes(True)

            Dim i As Integer
            For i = obj.GetLowerBound(0) To obj.GetUpperBound(0)
                If TypeOf (obj(i)) Is MapToAttribute Then
                    sMapTo = CType(obj(i), MapToAttribute).MapTo
                End If
            Next
            If sMapTo = "" Or sMapTo Is Nothing Then
                sMapTo = Me.GetType().Name
            End If
            Dim pd As UserDBProvider = GetDBAgent().GetProvider()
            If pd = UserDBProvider.MSSqlServer Then
                sMapTo = "[" + sMapTo + "]"
            ElseIf pd = UserDBProvider.Oracle Then
                sMapTo = """" + sMapTo.ToUpper() + """"
            End If

        End If
        Dim strDBName As String = GetDBName()
        Dim strSchema As String = GetSchemaName()
        If strDBName <> "" And strSchema <> "" Then
            sMapTo = strDBName + "." + strSchema + "." + sMapTo
        ElseIf strSchema <> "" Then
            sMapTo = strSchema + "." + sMapTo
        End If
        Return sMapTo

    End Function
    ''' <summary>
    ''' 设置映射的表字段名称
    ''' </summary>
    ''' <param name="MapTo"></param>
    ''' <remarks></remarks>
    Public Sub SetMapTo(ByVal MapTo As String)
        _MapTo = MapTo
    End Sub
    ''' <summary>
    ''' 获取所有字段信息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFieldsInfo() As PersistFieldCollection
        Return MyPersistFields
    End Function
    ''' <summary>
    ''' 获取主键信息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPrimaryKeys() As String()
        Return MyPrimaryKeys
    End Function
    ''' <summary>
    ''' 获取数据字典的名称域
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNameField() As PersistField
        Return _NameField
    End Function
    ''' <summary>
    ''' 获取锁定域
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLockField() As PersistField
        Return _LockField
    End Function

    Public Sub SetLockField(ByVal FieldName As String)
        _LockField = MyPersistFields.Item(FieldName)
    End Sub
    ''' <summary>
    ''' 获取排序域
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOrderField() As PersistField
        Return _OrderField
    End Function
    ''' <summary>
    ''' 根据成员名称设置成员值
    ''' </summary>
    ''' <param name="MemberName"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Sub SetMemberValue(ByVal MemberName As String, ByVal Value As Object)
        Dim pfi As PersistField = MyPersistFields.Item(MemberName)
        Dim sTypeName = pfi.FieldType.Name
        Dim memInfo As MemberInfo = Me.GetType().GetMember(MemberName)(0)
        If Value Is DBNull.Value Or Value Is Nothing Then
            Value = Nothing
        Else
            If sTypeName = "Integer" Or sTypeName = "Int32" Then
                Value = CInt(Value)
            ElseIf sTypeName = "Int16" Or sTypeName = "Short" Then
                Value = CShort(Value)
            ElseIf sTypeName = "Int64" Or sTypeName = "Long" Then
                Value = CLng(Value)
            ElseIf sTypeName = "Decimal" Then
                Value = CDec(Value)
            ElseIf sTypeName = "Double" Then
                Value = CDbl(Value)
            ElseIf sTypeName = "Single" Then
                Value = CSng(Value)
            ElseIf sTypeName = "Byte" Then
                Value = CByte(Value)
            ElseIf sTypeName = "Boolean" Then
                Value = CBool(Value)
            ElseIf sTypeName = "String" And pfi.IsNTextOrBlob Then
                If Not Value Is Nothing AndAlso Value.GetType().Name = "Byte[]" Then
                    Value = System.Text.Encoding.Default.GetString(Value)
                End If
            End If
        End If

        Select Case memInfo.MemberType
            Case MemberTypes.Field
                DirectCast(memInfo, FieldInfo).SetValue(Me, Value)
            Case MemberTypes.Property
                DirectCast(memInfo, PropertyInfo).SetValue(Me, Value, Nothing)
        End Select
        MyPersistFields.Item(MemberName).Value = Value
    End Sub

    Public Sub SetPrimaryValues(ByVal PrimaryValues() As Object)
        If PrimaryValues Is Nothing Then Return
        If PrimaryValues.Length <> MyPrimaryKeys.Length Then Throw New Exception("传入的主键值数组与主键个数不匹配！")
        Dim i As Integer
        For i = MyPrimaryKeys.GetLowerBound(0) To MyPrimaryKeys.GetUpperBound(0)
            SetMemberValue(MyPrimaryKeys(i), PrimaryValues(i))
        Next
    End Sub
    ''' <summary>
    ''' 根据成员名称获取成员值
    ''' </summary>
    ''' <param name="MemberName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMemberValue(ByVal MemberName As String) As Object
        Dim memInfo As MemberInfo = Me.GetType().GetMember(MemberName)(0)
        Select Case memInfo.MemberType
            Case MemberTypes.Field
                Return DirectCast(memInfo, FieldInfo).GetValue(Me)
            Case MemberTypes.Property
                Return DirectCast(memInfo, PropertyInfo).GetValue(Me, Nothing)
            Case Else
                Return Nothing
        End Select
    End Function

    Protected Friend Function GetDBAgent() As UserDBAgent
        Dim dbAgent As UserDBAgent
        If _DBAgent Is Nothing Then
            dbAgent = New UserDBAgent()
            _DBAgent = dbAgent
        Else
            dbAgent = _DBAgent
        End If
        Return dbAgent
    End Function

    Public Function ToXML() As String
        RefreshListValue()
        Dim i As Integer
        Dim xmlDoc As New Xml.XmlDocument()
        Dim Node As Xml.XmlNode = xmlDoc.CreateNode(Xml.XmlNodeType.Element, Me.GetType().Name, "")
        For i = 0 To MyPersistFields.Count - 1
            Dim pf As PersistField = MyPersistFields.Item(i)
            If Not pf.IsNonPersisted Then
                Dim childNode As Xml.XmlNode = xmlDoc.CreateNode(Xml.XmlNodeType.Element, pf.Name, "")
                Dim ss As String
                Dim sTypeName As String = pf.FieldType.Name.ToLower()
                If sTypeName.StartsWith("date") Then
                    ss = CDate(pf.Value).ToString("yyyy-MM-dd HH:mm:ss")
                    If ss.EndsWith("00:00:00") Then ss = ss.Substring(0, ss.Length - 9)
                Else
                    ss = pf.Value.ToString()
                End If
                childNode.InnerText = ss
                Node.AppendChild(childNode)
            End If
        Next
        xmlDoc.AppendChild(Node)
        Return xmlDoc.InnerXml
    End Function

    Public Sub FromXML(ByVal xmlData As String)
        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.LoadXml(xmlData)
        If xmlDoc.FirstChild.Name = Me.GetType().Name Then
            Dim xmlList As Xml.XmlNodeList = xmlDoc.FirstChild.ChildNodes
            Dim i As Integer
            For i = 0 To xmlList.Count - 1
                Try
                    Dim sTypeName As String = GetFieldsInfo().Item(xmlList.Item(i).Name).FieldType.Name.ToLower()
                    Dim sValue As String = xmlList.Item(i).InnerText
                    If sTypeName = "Integer" Or sTypeName = "Int32" Then
                        sValue = CInt(sValue)
                    ElseIf sTypeName = "Long" Or sTypeName = "Int64" Then
                        sValue = CLng(sValue)
                    ElseIf sTypeName = "Short" Or sTypeName = "Int16" Then
                        sValue = CShort(sValue)
                    ElseIf sTypeName = "byte" Then
                        sValue = CByte(sValue)
                    ElseIf sTypeName.StartsWith("date") Then
                        sValue = CDate(sValue)
                    ElseIf sTypeName = "bit" Or sTypeName = "boolean" Then
                        sValue = CBool(sValue)
                    ElseIf sTypeName = "double" Or sTypeName = "float" Then
                        sValue = CDbl(sValue)
                    ElseIf sTypeName = "decimal" Then
                        sValue = CDec(sValue)
                    End If
                    SetMemberValue(xmlList.Item(i).Name, sValue)
                Catch ex As Exception

                End Try
            Next
        End If

    End Sub

    Public Function ToJson() As String
        RefreshListValue()
        Dim i As Integer
        Dim sb As New System.Text.StringBuilder()
        sb.Append("{")
        For i = 0 To MyPersistFields.Count - 1
            Dim pf As PersistField = MyPersistFields.Item(i)
            If Not pf.IsNonPersisted Then
                sb.Append(pf.Name)
                sb.Append(":""")
                Dim ss As String
                Dim sTypeName As String = pf.FieldType.Name.ToLower()
                If sTypeName.StartsWith("date") Then
                    ss = CDate(pf.Value).ToString("yyyy-MM-dd HH:mm:ss")
                    If ss.EndsWith("00:00:00") Then ss = ss.Substring(0, ss.Length - 9)
                Else
                    If sTypeName.EndsWith("byte[]") Then '重构
                        If CType(pf.Value, Byte()).Length > 0 Then
                            ss = pf.Value.ToString()
                        Else
                            ss = ""
                        End If
                    Else
                        ss = pf.Value.ToString()
                    End If
                End If
                sb.Append(Ajax.ToJsonString(ss))
                sb.Append("""")
                If i < MyPersistFields.Count - 1 Then
                    sb.Append(",")
                End If
            End If
        Next
        sb.Append("}")
        Return sb.ToString()
    End Function

    Public Sub FromJson(ByVal jsonData As String, Optional ByVal IsExists As Boolean = True)
        'Dim ms As MatchCollection = Regex.Matches(jsonData, _
        '"(?<key>\w+):(?<value>\'[\w\s]+\')|(?<key>\w+):(?<value>\""[\w\s]+\"")|(?<key>\'\w+\'):(?<value>\'[\w\s]+\')|(?<key>\""\w+\""):(?<value>\""[\w\s]+\"")")
        'Dim i As Integer
        'For i = 0 To ms.Count - 1
        '    Debug.WriteLine(ms.Item(i).Groups("key").ToString() + ":" + ms.Item(i).Groups("value").ToString())
        'Next

        Dim Data As String() = jsonData.Substring(1, jsonData.Length - 2).Split(",")
        For i = Data.GetLowerBound(0) To Data.GetUpperBound(0)
            Dim Item As String() = Data(i).Split(":")
            If Item.Length = 2 Then
                Try
                    Dim sName As String
                    Dim sValue As Object
                    sName = Item(0)
                    If (sName.StartsWith("'") And sName.EndsWith("'")) Or (sName.StartsWith("""") And sName.EndsWith("""")) Then
                        sName = sName.Substring(1, sName.Length - 2)
                    End If
                    sValue = Item(1)
                    If (sValue.StartsWith("'") And sValue.EndsWith("'")) Or (sValue.StartsWith("""") And sValue.EndsWith("""")) Then
                        sValue = sValue.Substring(1, sValue.Length - 2)
                    End If
                    sValue = sValue.ToString().Replace("#comma", ",").Replace("#semicolon", ":")
                    Dim sTypeName As String = GetFieldsInfo().Item(sName).FieldType.Name.ToLower()
                    If sTypeName = "Integer" Or sTypeName = "Int32" Then
                        sValue = CInt(sValue)
                    ElseIf sTypeName = "Long" Or sTypeName = "Int64" Then
                        sValue = CLng(sValue)
                    ElseIf sTypeName = "Short" Or sTypeName = "Int16" Then
                        sValue = CShort(sValue)
                    ElseIf sTypeName = "byte" Then
                        sValue = CByte(sValue)
                    ElseIf sTypeName.StartsWith("date") Then
                        sValue = CDate(sValue)
                    ElseIf sTypeName = "bit" Or sTypeName = "boolean" Then
                        sValue = CBool(sValue)
                    ElseIf sTypeName = "double" Or sTypeName = "float" Then
                        sValue = CDbl(sValue)
                    ElseIf sTypeName = "decimal" Then
                        sValue = CDec(sValue)
                    End If

                    SetMemberValue(sName, sValue)
                Catch ex As Exception

                End Try
            End If
        Next
        _IsPersisted = IsExists
    End Sub


#Region "数据字典控制函数，包括排序、锁定等"
    ''' <summary>
    ''' 查询字典数据
    ''' </summary>
    ''' <param name="IsShowLockData">是否显示锁定数据</param>
    ''' <param name="Names">查询内容</param>
    ''' <returns>返回字典数据表</returns>
    ''' <remarks></remarks>
    Public Function SelectDictData(Optional ByVal IsShowLockData As Boolean = False, Optional ByVal Names As String = "") As DataTable
        Dim dba As New UserDBAgent()
        Dim dt As New DataTable()
        Dim strSql As String = "select * from " & GetMapTo() & " where 1=1 "
        If Not IsShowLockData And Not _LockField Is Nothing Then
            strSql &= " and (" & _LockField.MapTo & "=0 or " & _LockField.MapTo & " is null) "
        ElseIf Not _LockField Is Nothing Then
            strSql &= " and " & _LockField.MapTo & "=1 "
        End If
        Dim params As List(Of IDataParameter)
        If Names <> "" And Not _NameField Is Nothing Then
            strSql &= " and " & _NameField.MapTo & " like " & dba.GetParamPrev() & _NameField.MapTo
            params = New List(Of IDataParameter)(0)
            params.Add(dba.CreateParameter(dba.GetParamPrev() & _NameField.MapTo, "%" & Names.Trim() & "%"))
        Else
            params = Nothing
        End If

        If Not _OrderField Is Nothing Then strSql &= " order by " & _OrderField.MapTo
        Try
            If params Is Nothing Then
                dt = dba.GetDataTable(strSql)
            Else
                dt = dba.GetDataTable(strSql, params)
            End If

        Catch ex As Exception
            dt = Nothing
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' 根据ID交换排序
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Target"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OrderExchange(ByVal Source As DataObject, ByVal Target As DataObject) As Boolean
        If Not _OrderField Is Nothing Then
            Dim userTrans As New UserDBTransaction()
            Try

                Dim orderSrc As Integer = Source.GetMemberValue(_OrderField.Name)
                Dim orderTar As Integer = Target.GetMemberValue(_OrderField.Name)
                If orderSrc >= 0 And orderTar >= 0 Then
                    userTrans.BeginTrans()
                    Target.SetMemberValue(_OrderField.Name, orderSrc)
                    Source.SetMemberValue(_OrderField.Name, orderTar)
                    userTrans.Add(Target)
                    userTrans.Add(Source)
                    Dim hasError As Boolean = False
                    hasError = hasError Or Not Target.Save()
                    hasError = hasError Or Not Source.Save()
                    If hasError Then
                        userTrans.RollbackTrans()
                    Else
                        userTrans.CommitTrans()
                    End If

                Else
                    Return False
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return True
        End If
    End Function
    ''' <summary>
    ''' 根据ID冒泡排序
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Target"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OrderUpdown(ByVal Source As DataObject, ByVal Target As DataObject) As Boolean
        If Not _OrderField Is Nothing Then
            'If Source = Target Then Return True
            Dim userTrans As New UserDBTransaction()
            Dim dba As UserDBAgent = userTrans.GetDBAgent()
            Try
                Dim orderSrc As Integer = Source.GetMemberValue(_OrderField.Name)
                Dim orderTar As Integer = Target.GetMemberValue(_OrderField.Name)
                If orderSrc >= 0 And orderTar >= 0 Then
                    Dim strSql As String = ""
                    userTrans.BeginTrans()
                    userTrans.Add(Source)
                    userTrans.Add(Target)
                    If Math.Abs(orderSrc - orderTar) = 1 Then
                        '如果是相邻并且排序号相差为1,则交换顺序
                        Target.SetMemberValue(_OrderField.Name, orderSrc)
                        Source.SetMemberValue(_OrderField.Name, orderTar)
                    ElseIf orderSrc > orderTar Then
                        '如果是从下往上，则源ID对应序号为目标ID对应序号，同时对源ID与目标ID之间的序号+1
                        strSql = "update " & GetMapTo() & " set " & _OrderField.MapTo & "=" & _OrderField.MapTo & _
                            "+1 where " & _OrderField.MapTo & ">=" & dba.GetParamPrev() & "OrderTar and " & _OrderField.MapTo & _
                            "<" & dba.GetParamPrev() & "OrderSrc; "
                        Source.SetMemberValue(_OrderField.Name, orderTar)
                    ElseIf orderSrc < orderTar Then
                        '如果是从上往下，则将源ID对应序号为目标ID对应序号，同时对源ID与目标ID之间的序号-1
                        strSql = "update " & GetMapTo() & " set " & _OrderField.MapTo & "=" & _OrderField.MapTo & _
                            "-1 where " & _OrderField.MapTo & ">" & dba.GetParamPrev() & "OrderSrc and " & _
                            _OrderField.MapTo & "<=" & dba.GetParamPrev() & "OrderTar; "
                        Source.SetMemberValue(_OrderField.Name, orderTar)
                    Else
                        '如果序号相同
                        'cmd.CommandText &= "update " & _TableName & " set " & _OrderField & "=" & _IDField & _
                        '" where " & _OrderField & "=@OrderSrc; "
                        'cmd.CommandText &= "update " & _TableName & " set " & _OrderField & "=@OrderTar where " & _IDField & "=@SourceID "
                    End If

                    Dim hasError As Boolean = False
                    If strSql <> "" Then
                        Dim params As New List(Of IDataParameter)(1)
                        params.Add(dba.CreateParameter(dba.GetParamPrev() & "OrderSrc", orderSrc))
                        params.Add(dba.CreateParameter(dba.GetParamPrev() & "OrderTar", orderTar))
                        hasError = hasError Or Not dba.ExecSql(strSql, params)
                    End If
                    hasError = hasError Or Not Source.Save()
                    hasError = hasError Or Not Target.Save()

                    If hasError Then
                        userTrans.RollbackTrans()
                    Else
                        userTrans.CommitTrans()
                    End If

                Else
                    Return False
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return True
        End If
    End Function
    ''' <summary>
    ''' 根据主键值锁定数据
    ''' </summary>
    ''' <param name="PrimaryValues"></param>
    ''' <remarks></remarks>
    Public Sub Lock(ByVal PrimaryValues() As Object)
        SetPrimaryValues(PrimaryValues)
        Reload()
        If _IsPersisted Then
            SetMemberValue(_LockField.Name, 1)
            Save()
        End If
    End Sub
    ''' <summary>
    ''' 根据主键值解锁数据
    ''' </summary>
    ''' <param name="PrimaryValues"></param>
    ''' <remarks></remarks>
    Public Sub UnLock(ByVal PrimaryValues() As Object)
        SetPrimaryValues(PrimaryValues)
        Reload()
        If _IsPersisted Then
            SetMemberValue(_LockField.Name, 0)
            Save()
        End If
    End Sub
    ''' <summary>
    ''' 根据主键值删除数据
    ''' </summary>
    ''' <param name="PrimaryValues"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal PrimaryValues() As Object)
        SetPrimaryValues(PrimaryValues)
        Reload()
        If _IsPersisted Then
            Delete()
        End If
    End Sub
    ''' <summary>
    ''' 根据主键值更新数据
    ''' </summary>
    ''' <param name="PrimaryValues"></param>
    ''' <param name="Value"></param>
    ''' <remarks></remarks>
    Public Function Update(ByVal PrimaryValues() As Object, ByVal Value As String) As Boolean
        SetPrimaryValues(PrimaryValues)
        Reload()
        If _IsPersisted Then
            SetMemberValue(_NameField.Name, Value)
            Return Save()
        Else
            Return True
        End If
    End Function
    ''' <summary>
    ''' 根据主键值和名称插入数据，如果主键为自增，则不用传入
    ''' </summary>
    ''' <param name="PrimaryValues"></param>
    ''' <param name="Value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Insert(ByVal PrimaryValues() As Object, ByVal Value As String) As Boolean
        SetPrimaryValues(PrimaryValues)
        SetMemberValue(_NameField.Name, Value)
        If Not _OrderField Is Nothing Then
            SetMemberValue(_OrderField.Name, GenID(0, _OrderField.MapTo))
        End If
        _IsPersisted = False
        Return Save()

    End Function
    ''' <summary>
    ''' 生成主键新值，仅限于单主键，数据类型为整形或者数字组成的字符串
    ''' </summary>
    ''' <param name="DefaultSize">如果是整形ID，DefaultSize传0；如果是字符型，传入生成编号的长度</param>
    ''' <returns></returns>
    ''' <remarks>数据字典控制函数</remarks>
    Public Function GenID(Optional ByVal DefaultSize As Integer = 0, _
                          Optional ByVal FieldName As String = "") As Object
        Dim pk As String
        If String.IsNullOrEmpty(FieldName) Then
            pk = MyPrimaryKeys(0)
        Else
            pk = FieldName
        End If
        Dim strSql As String = "select max({0}) from {1}"
        Dim id As Object = GetDBAgent.GetDataTable(String.Format(strSql, pk, GetMapTo())).Rows(0)(0)
        Dim idValue As Integer
        Try
            idValue = CInt(id)
        Catch ex As Exception
            idValue = 0
        End Try
        If id.GetType().Name.ToLower.IndexOf("string") >= 0 Then
            Dim pf As PersistField = MyPersistFields.Item(MyPrimaryKeys(0))
            Dim pkSize As Integer
            If pf.Size > 0 Then
                pkSize = pf.Size
            Else
                pkSize = id.ToString().Length
            End If
            Return (idValue + 1).ToString("D" & pkSize & "")
        ElseIf ((id Is Nothing) Or (IsDBNull(id))) And (DefaultSize > 0) Then
            Return (idValue + 1).ToString("D" & DefaultSize & "")
        Else
            Return idValue + 1
        End If
    End Function

#End Region

    Public Event PropertyChanged(ByVal sender As Object, _
                                 ByVal e As System.ComponentModel.PropertyChangedEventArgs) _
                                 Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


    Protected Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

End Class

