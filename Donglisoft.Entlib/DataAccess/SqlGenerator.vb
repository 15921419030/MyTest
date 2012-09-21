
Namespace DataAccess
    Public Structure EntitySql
        Public Sql As String
        Public ParamList As List(Of IDataParameter)
    End Structure

    Public Interface ISqlGenerator
        Function getSelectSql() As String
        Function getInsertSql() As String
        Function getUpdateSql() As String
        Function getDeleteSql() As String
        Function getEntitySelectSql(Optional ByVal BatchMode As Boolean = False) As EntitySql
        Function getEntityInsertSql(Optional ByVal BatchMode As Boolean = False) As EntitySql
        Function getEntityUpdateSql(Optional ByVal BatchMode As Boolean = False) As EntitySql
        Function getEntityDeleteSql(Optional ByVal BatchMode As Boolean = False) As EntitySql
    End Interface

    Public Class SqlGenerator
        Implements ISqlGenerator

        Private _Provider As UserDBProvider
        Private _Distinct As Boolean
        Private _TopRowNum As Integer
        Private _selectFields As String = "*"
        Private _condition As CriteriaOperator
        Private _order As OrderCollection
        Private _Entity As DataObject

        Public Property Distinct() As Boolean
            Get
                Return _Distinct
            End Get
            Set(ByVal value As Boolean)
                _Distinct = value
            End Set
        End Property

        Public Property TopRowNum() As Integer
            Get
                Return _TopRowNum
            End Get
            Set(ByVal value As Integer)
                _TopRowNum = value
            End Set
        End Property

        Public Property SelectFields() As String
            Get
                Return _selectFields
            End Get
            Set(ByVal value As String)
                _selectFields = DealSelectField(value)
            End Set
        End Property

        Public Property Condition() As CriteriaOperator
            Get
                Return _condition
            End Get
            Set(ByVal value As CriteriaOperator)
                _condition = value
            End Set
        End Property

        Public Property Order() As OrderCollection
            Get
                Return _order
            End Get
            Set(ByVal value As OrderCollection)
                _order = value
            End Set
        End Property

        Public Sub New(ByVal Entity As DataObject)
            _Provider = Settings.DBContext.GetDBType()
            _Entity = Entity
        End Sub

        Public Sub New(ByVal Entity As DataObject, ByVal Provider As UserDBProvider)
            _Provider = Provider
            _Entity = Entity
        End Sub

        Private Function ReplaceWithMapTo(ByVal source As String) As String
            '处理字段，将类属性字段替换成数据表的字段，所有属性字段以$开始，以#号结束
            Dim exp As New System.Text.RegularExpressions.Regex("\$\w+\#")
            Dim mc As System.Text.RegularExpressions.Match = exp.Match(source)
            While mc.Success
                Dim src As String = mc.Groups(0).Value
                Dim srcField As String = src.Substring(1, src.Length - 2)
                Dim tar As String = GetQuotedField(_Entity.GetFieldsInfo().Item(srcField).MapTo) & ""
                source = source.Replace(src, tar)
                mc = mc.NextMatch()
            End While
            Return source
        End Function

        Private Function DealSelectField(ByVal Source As String) As String
            If Source.Trim() = "*" Then
                Return Source
            Else
                Dim strTemp As String = ""
                Dim Fields() As String = Source.Split(",")
                If Fields.Length > 0 Then
                    strTemp = ""
                    Dim i As Integer
                    For i = Fields.GetLowerBound(0) To Fields.GetUpperBound(0)
                        strTemp &= GetQuotedField(_Entity.GetFieldsInfo().Item(Fields(i).Trim()).MapTo) & ","
                    Next
                End If
                If strTemp.EndsWith(",") Then strTemp = strTemp.Remove(strTemp.Length - 1, 1)
                Return strTemp
            End If
        End Function


        Public Function getDeleteSql() As String Implements ISqlGenerator.getDeleteSql
            Dim strCon As String = ""
            If Not _condition Is Nothing Then strCon = ReplaceWithMapTo(_condition.ToString(_Provider))
            Dim strSql As String = "DELETE FROM " + _Entity.GetMapTo()
            If Not strCon Is Nothing AndAlso strCon.Trim <> "" Then
                strSql &= " WHERE " + strCon
            End If
            Return strSql
        End Function

        Public Function getInsertSql() As String Implements ISqlGenerator.getInsertSql
            Return "INSERT INTO {0} ({1}) VALUES({2}) "
        End Function

        Public Function getSelectSql() As String Implements ISqlGenerator.getSelectSql

            Dim strCon As String = ""
            Dim strOrder As String = ""

            If Not _condition Is Nothing Then strCon = ReplaceWithMapTo(_condition.ToString(_Provider))
            If Not _order Is Nothing Then strOrder = ReplaceWithMapTo(_order.ToString())
            Dim strSql As String = "SELECT {0} {1} " & _selectFields & " FROM " & _Entity.GetMapTo()
            If strCon.Trim() <> "" Then strSql &= " WHERE " & strCon
            If strOrder.Trim() <> "" Then strSql &= " ORDER BY " & strOrder
            strSql &= " {2}"

            Dim str1 As String = ""
            Dim str2 As String = ""
            Dim str3 As String = ""

            If _Distinct Then str1 = "DISTINCT"
            If _TopRowNum > 0 Then
                Select Case _Provider
                    Case UserDBProvider.Oracle
                        strSql = "SELECT * FROM (" & strSql & ") WHERE ROWNUM<=" & _TopRowNum.ToString() & _
                            " ORDER BY ROWNUM ASC"
                    Case UserDBProvider.MySql
                        str3 = "LIMIT 0, " & _TopRowNum.ToString()
                    Case Else
                        str2 = "TOP " & _TopRowNum.ToString()
                End Select
            End If

            strSql = String.Format(strSql, str1, str2, str3)
            Return strSql

        End Function

        Public Function getSelectCountSql() As String
            Dim strCon As String = ""
            Dim strOrder As String = ""

            If Not _condition Is Nothing Then strCon = ReplaceWithMapTo(_condition.ToString(_Provider))
            If Not _order Is Nothing Then strOrder = ReplaceWithMapTo(_order.ToString())
            Dim strSql As String = "SELECT count(*) FROM " & _Entity.GetMapTo()
            If strCon.Trim() <> "" Then strSql &= " WHERE " & strCon
            Return strSql
        End Function

        Public Function getSelectPageSql(ByVal OrderField As String, _
                                         ByVal OrderKind As Integer, _
                                         ByVal PageIndex As Integer, Optional ByVal PageSize As Integer = 20) As String
            Dim SelectSql As String = ""
            Dim strCon As String = ""
            Dim strOrder As String = ""

            If Not _condition Is Nothing Then strCon = ReplaceWithMapTo(_condition.ToString(_Provider))
            If Not _order Is Nothing Then strOrder = ReplaceWithMapTo(_order.ToString())
            Dim strSql As String = "SELECT {0} {1} " & _selectFields & " FROM " & _Entity.GetMapTo()
            If strCon.Trim() <> "" Then strSql &= " WHERE " & strCon
            strSql &= " {2}"

            Dim str1 As String = ""
            Dim str2 As String = ""
            Dim str3 As String = ""

            If _Distinct Then str1 = "DISTINCT"
            If _TopRowNum > 0 Then
                Select Case _Provider
                    Case UserDBProvider.Oracle
                        strSql = "SELECT * FROM (" & strSql & ") WHERE ROWNUM<=" & _TopRowNum.ToString() & _
                            " ORDER BY ROWNUM ASC"
                    Case UserDBProvider.MySql
                        str3 = "LIMIT 0, " & _TopRowNum.ToString()
                    Case Else
                        str2 = "TOP " & _TopRowNum.ToString()
                End Select
            End If

            strSql = String.Format(strSql, str1, str2, str3)


            Dim sSelectPageSql As String = ""
            Select Case _Provider
                Case UserDBProvider.Oracle
                    Dim strTmp As String = ""
                    Dim sOrderKind As String = "asc"
                    If OrderKind > 0 Then
                        '降序
                        sOrderKind = "desc"
                    End If
                    If strOrder.Trim() <> "" Then
                        strTmp = strOrder
                    ElseIf OrderField.Trim() <> "" Then
                        If OrderField.ToLower().EndsWith(" asc") Or OrderField.ToLower().EndsWith(" desc") Then
                            strTmp = OrderField
                        Else
                            strTmp = OrderField + " " + sOrderKind
                        End If
                    Else
                        For Each Str As String In _Entity.GetPrimaryKeys()
                            If Str.Trim() <> "" Then strTmp += Str + " " + sOrderKind + ","
                        Next
                        If strTmp.EndsWith(",") Then strTmp = strTmp.Remove(strTmp.Length - 1, 1)
                    End If
                    SelectSql = strSql
                    If strTmp.Trim() <> "" Then SelectSql += " ORDER BY " & strTmp
                    sSelectPageSql = "select * from (select a.*,rownum row_num from (" + SelectSql
                    'sSelectPageSql += " order by " + strTmp
                    sSelectPageSql += ") a) b where b.row_num between " + ((PageIndex - 1) * PageSize + 1).ToString()
                    sSelectPageSql += " and " + ((PageIndex * PageSize)).ToString()
                Case UserDBProvider.MySql
                    SelectSql = strSql
                    If strOrder.Trim() <> "" Then SelectSql += " ORDER BY " & strOrder
                    sSelectPageSql = "select * from (" + SelectSql
                    sSelectPageSql += ") a limit " + ((PageIndex - 1) * PageSize + 1).ToString()
                    sSelectPageSql += ", " + ((PageIndex * PageSize)).ToString()
                Case Else

                    Dim sOrderKind As String = "asc"
                    Dim sReverseOrderKind As String = "desc"
                    Dim sCompare As String = ">"
                    Dim sMaxMin As String = "max"
                    If OrderKind > 0 Then
                        '降序
                        sOrderKind = "desc"
                        sReverseOrderKind = "asc"
                        sCompare = "<"
                        sMaxMin = "min"
                    End If
                    'sql server 2005及以上版本分页方法"
                    If _Provider = UserDBProvider.MSSqlServer AndAlso DataAccess.MsSqlProvider.GetVersion(_Entity.GetDBAgent().GetContext().ConnectionString) > 2000 Then
                        SelectSql = strSql
                        Dim strTmp As String = ""
                        If strOrder.Trim() <> "" Then
                            strTmp = strOrder
                        ElseIf OrderField.Trim() <> "" Then
                            If OrderField.ToLower().EndsWith(" asc") Or OrderField.ToLower().EndsWith(" desc") Then
                                strTmp = OrderField
                            Else
                                strTmp = OrderField + " " + sOrderKind
                            End If
                        Else
                            For Each Str As String In _Entity.GetPrimaryKeys()
                                If Str.Trim() <> "" Then strTmp += Str + " " + sOrderKind + ","
                            Next
                            If strTmp.EndsWith(",") Then strTmp = strTmp.Remove(strTmp.Length - 1, 1)
                        End If
                        sSelectPageSql = "select * from (select a.*,row_number() over(order by " + strTmp + ") row_num from (" + SelectSql
                        sSelectPageSql += ") a) b where b.row_num between " + ((PageIndex - 1) * PageSize + 1).ToString()
                        sSelectPageSql += " and " + ((PageIndex * PageSize)).ToString() & " order by b.row_num asc"
                    Else

                        If strOrder.Trim() <> "" Then
                            SelectSql = "select top " + (PageSize * PageIndex).ToString() + " * from (" + strSql + ") b ORDER BY " & strOrder
                        Else
                            SelectSql = strSql
                        End If


                        If PageIndex <= 1 Then
                            sSelectPageSql = "select top " + PageSize.ToString() + " * from (" + SelectSql + ") a order by " + OrderField + " " + sOrderKind
                        Else
                            sSelectPageSql = "select top " + PageSize.ToString()
                            sSelectPageSql += " * from (" + SelectSql + ") as a where [" + OrderField + "]" + sCompare
                            sSelectPageSql += "(select " + sMaxMin + "([" + OrderField + "]) from "
                            sSelectPageSql += "(select top " + ((PageIndex - 1) * PageSize).ToString() & " [" + OrderField + "] from "
                            sSelectPageSql += "(" + SelectSql + ") as c "
                            sSelectPageSql += "where  1=1   order by [" + OrderField + "] " + sOrderKind + ") as tblTmp) and  1=1 "
                            sSelectPageSql += "order by [" + OrderField + "] " + sOrderKind
                        End If
                    End If
            End Select

            Return sSelectPageSql
        End Function

        Public Function getSelectCountSql(ByVal SelectSql As String) As String
            Return "select count(*) from " + SelectSql
        End Function

        Public Function getUpdateSql() As String Implements ISqlGenerator.getUpdateSql
            Return "UPDATE {0} SET {1} WHERE {2}"
        End Function

        Public Function GetQuotedField(ByVal Source As String) As String
            Select Case _Provider
                Case UserDBProvider.MySql, UserDBProvider.Oracle
                    Return Source
                Case Else
                    Return "[" + Source + "]"
            End Select
        End Function

        Public Function getEntityDeleteSql(Optional ByVal BatchMode As Boolean = False) As EntitySql Implements ISqlGenerator.getEntityDeleteSql
            Dim es As New EntitySql
            es.Sql = ""
            es.ParamList = New List(Of IDataParameter)

            Dim obj As DataObject = _Entity
            Dim dba As UserDBAgent = _Entity.GetDBAgent()

            Dim i As Integer
            Dim PrimaryKeys() As String = obj.GetPrimaryKeys()
            Dim DeleteFields As PersistFieldCollection = obj.GetFieldsInfo()
            Dim TableName As String = obj.GetMapTo()
            Dim sCon As String = " where 1=1 "
            Dim paramName As String = GetParamName(BatchMode, PrimaryKeys(i))
            For i = PrimaryKeys.GetLowerBound(0) To PrimaryKeys.GetUpperBound(0)
                Dim sKeySearch As String
                Dim pfi As PersistField = DeleteFields.Item(PrimaryKeys(i))

                sKeySearch = " and {0} = {1} "
                sCon &= String.Format(sKeySearch, GetQuotedField(PrimaryKeys(i)), paramName)

                Dim param As IDataParameter = dba.CreateParameter(paramName, pfi.Value)
                es.ParamList.Add(param)
            Next


            Dim sd As String = "delete from {0} {1} "
            Dim s0, s1 As String
            s0 = TableName
            s1 = sCon
            Dim strSql As String = String.Format(sd, s0, s1)
            es.Sql = strSql
            If BatchMode Then obj._EntitySql = es

            Return es
        End Function

        Public Function getEntityInsertSql(Optional ByVal BatchMode As Boolean = False) As EntitySql Implements ISqlGenerator.getEntityInsertSql
            Dim es As EntitySql
            es.Sql = ""
            es.ParamList = New List(Of IDataParameter)()

            Dim obj As DataObject = _Entity
            Dim dba As UserDBAgent = _Entity.GetDBAgent()
            Dim i As Integer
            Dim Data As PersistFieldCollection = obj.GetFieldsInfo()
            Dim TableName As String = obj.GetMapTo()
            If Data.Count <= 0 Then
                Return es
            End If
            Dim sd As String = "insert into {0} ({1}) values({2}) "
            Dim strSql, s0, s1, s2 As String
            Dim pfi As PersistField

            s0 = TableName
            s1 = ""
            s2 = ""

            For i = 0 To Data.Count - 1
                pfi = Data.Item(i)
                If Not pfi.IsIdentity And Not pfi.IsNonPersisted Then
                    s1 &= GetQuotedField(pfi.MapTo) & ","
                    Dim paramName As String = GetParamName(BatchMode, pfi.MapTo)
                    If pfi.Value.GetType().Name.IndexOf("Date") >= 0 And _Provider = UserDBProvider.Oracle Then
                        s2 &= "to_date(" + paramName & ", 'yyyy-MM-dd HH24:MI:SS'),"
                    Else
                        s2 &= paramName & ","
                    End If

                    Dim param As IDataParameter
                    Dim value As Object = pfi.GetCommitValue()
                    If pfi.IsNTextOrBlob And pfi.FieldType.Name = "String" _
                    And (Not value Is Nothing AndAlso value.GetType().Name = "String") Then
                        If dba.GetProvider() = UserDBProvider.Oracle Then
                            value = System.Text.Encoding.Default.GetBytes(value)
                        End If
                    End If
                    param = dba.CreateParameter(paramName, value)
                    es.ParamList.Add(param)
                End If
            Next

            If s1.EndsWith(",") Then s1 = s1.Remove(s1.Length - 1, 1)
            If s2.EndsWith(",") Then s2 = s2.Remove(s2.Length - 1, 1)

            strSql = String.Format(sd, s0, s1, s2)

            es.Sql = strSql

            If BatchMode Then obj._EntitySql = es

            Return es
        End Function

        Public Function getEntitySelectSql(Optional ByVal BatchMode As Boolean = False) As EntitySql Implements ISqlGenerator.getEntitySelectSql
            Dim es As New EntitySql
            es.Sql = ""
            es.ParamList = New List(Of IDataParameter)
            Return es
        End Function

        Public Function getEntityUpdateSql(Optional ByVal BatchMode As Boolean = False) As EntitySql Implements ISqlGenerator.getEntityUpdateSql
            Dim es As EntitySql
            es.Sql = ""
            es.ParamList = New List(Of IDataParameter)()
            Dim obj As DataObject = _Entity
            Dim dba As UserDBAgent = _Entity.GetDBAgent()
            Dim i As Integer
            Dim Data As PersistFieldCollection = obj.GetFieldsInfo()
            Dim TableName As String = obj.GetMapTo()
            If Data.Count <= 0 Then Return es
            Dim sd As String = "update {0} set {1} where {2} "
            Dim strSql, s0, s1, s2 As String
            Dim pfi As PersistField
            s0 = TableName
            s1 = ""
            s2 = " 1=1 "

            For i = 0 To Data.Count - 1
                pfi = Data.Item(i)
                Dim paramName As String = GetParamName(BatchMode, pfi.MapTo)
                Dim bCreate As Boolean = False
                If pfi.IsPrimary Then
                    s2 &= " and " & GetQuotedField(pfi.MapTo) & "=" & paramName & ","
                    bCreate = True
                ElseIf Not pfi.IsIdentity And Not pfi.IsNonPersisted And pfi.ValueChanged Then
                    If pfi.Value.GetType().Name.IndexOf("Date") >= 0 And _Provider = UserDBProvider.Oracle Then
                        s1 &= GetQuotedField(pfi.MapTo) & "=to_date(" & paramName & ",'yyyy-MM-dd HH24:MI:SS'),"
                    Else
                        s1 &= GetQuotedField(pfi.MapTo) & "=" & paramName & ","
                    End If
                    bCreate = True
                End If

                If bCreate Then
                    Dim param As IDataParameter
                    Dim value As Object = pfi.GetCommitValue()
                    If pfi.IsNTextOrBlob And pfi.FieldType.Name = "String" _
                    And (Not value Is Nothing AndAlso value.GetType().Name = "String") Then
                        If dba.GetProvider() = UserDBProvider.Oracle Then
                            value = System.Text.Encoding.Default.GetBytes(value)
                        End If
                    End If
                    param = dba.CreateParameter(paramName, value)
                    es.ParamList.Add(param)
                End If

            Next

            If s1.EndsWith(",") Then s1 = s1.Remove(s1.Length - 1, 1)
            If s2.EndsWith(",") Then s2 = s2.Remove(s2.Length - 1, 1)

            strSql = String.Format(sd, s0, s1, s2)

            es.Sql = strSql

            If s1.Trim() = "" Then
                es.Sql = ""
            End If

            If BatchMode Then obj._EntitySql = es

            Return es
        End Function

        Private Function GetParamName(ByVal BatchMode As Boolean, ByVal Source As String) As String
            Dim _paramName As String
            If BatchMode Then
                Dim _SubLeft As String

                If Source.ToString.Length >= 5 Then
                    _SubLeft = Source.ToString().Substring(0, 5)
                Else
                    _SubLeft = Source.ToString()
                End If
                'Dim s As String = Now.Millisecond.ToString()
                _paramName = _Entity.GetDBAgent().GetParamPrev() & "P_" & _SubLeft & AutoGen.GetUniqueString().Substring(0, 20)
            Else
                _paramName = _Entity.GetDBAgent().GetParamPrev() & Source
            End If

            Return _paramName

        End Function
    End Class
End Namespace
