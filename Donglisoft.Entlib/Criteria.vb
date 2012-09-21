
Imports System.Text.RegularExpressions

''' <summary>
''' 逻辑操作基类
''' </summary>
''' <remarks></remarks>
Public MustInherit Class CriteriaOperator

    Protected Friend Structure UserDataParameter
        Public ParamName As String
        Public Value As Object
    End Structure

    Protected _Params As New List(Of IDataParameter)
    Protected _UserParams As New List(Of UserDataParameter)

    Public Function GetParams(ByVal Context As Settings.DBContext) As List(Of IDataParameter)
        Dim udo As New UserDBAgent(Context)
        _Params.Clear()
        For i = 0 To _UserParams.Count - 1
            Dim p As UserDataParameter = _UserParams.Item(i)
            _Params.Add(udo.CreateParameter(p.ParamName, p.Value))
        Next
        Return _Params
    End Function

    Protected Friend Function GetUserParams() As List(Of UserDataParameter)
        Return _UserParams
    End Function

    'Public Shared Shadows Narrowing Operator CType(ByVal Condition As String) As CriteriaOperator

    'End Operator

    Public Shared Operator And(ByVal Left As CriteriaOperator, ByVal Right As CriteriaOperator) As GroupOperator
        Dim group As New GroupOperator(GroupOperator.GroupOperatorType.And, Left, Right)
        Return group
    End Operator

    Public Shared Operator Or(ByVal Left As CriteriaOperator, ByVal Right As CriteriaOperator) As GroupOperator
        Dim group As New GroupOperator(GroupOperator.GroupOperatorType.Or, Left, Right)
        Return group
    End Operator

    Public Shared Operator Not(ByVal Right As CriteriaOperator) As NotOperator
        Return New NotOperator(Right)
    End Operator

    Public MustOverride Overloads Function ToString(ByVal Provider As UserDBProvider) As String

End Class

''' <summary>
''' 聚合运算基类
''' </summary>
''' <remarks></remarks>
Public Class CriteriaOperand

End Class

''' <summary>
''' 代表字段
''' </summary>
''' <remarks></remarks>
Public Class OperatorField
    Inherits CriteriaOperator

    Private _PropertyName As String

    Public Overrides Function ToString() As String
        Return _PropertyName
    End Function

    Public Sub New(ByVal PropertyName As String)
        _PropertyName = PropertyName
    End Sub

    Public Shared Shadows Narrowing Operator CType(ByVal PropertyName As String) As OperatorField
        Dim ov As New OperatorField(PropertyName)
        Return ov
    End Operator

    Public Shared Operator Like(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.LikeFull, Left, Right)
    End Operator

    Public Shared Operator >=(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.GreaterEqual, Left, Right)
    End Operator

    Public Shared Operator <=(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.LessEqual, Left, Right)
    End Operator

    Public Shared Operator >(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.Greater, Left, Right)
    End Operator

    Public Shared Operator <(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.Less, Left, Right)
    End Operator

    Public Shared Operator =(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.Equal, Left, Right)
    End Operator

    Public Shared Operator <>(ByVal Left As OperatorField, ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.NotEqual, Left, Right)
    End Operator

    Public Function [in](ByVal Right As OperatorValue) As BinaryOperator
        Return New BinaryOperator(BinaryOperator.BinaryOperatorType.in, Me, Right)
    End Function

    Public Overloads Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        Return _PropertyName
    End Function
End Class

''' <summary>
''' 代表值
''' </summary>
''' <remarks></remarks>
Public Class OperatorValue
    Inherits CriteriaOperator

    Private _Value As Object

    Public Sub New(ByVal value As Object)
        _Value = value
    End Sub

    Public Overrides Function ToString() As String
        Return _Value.ToString()
    End Function

    Public Property Value() As Object
        Get
            Return _Value
        End Get
        Set(ByVal value As Object)
            _Value = value
        End Set
    End Property

    Public Shared Shadows Narrowing Operator CType(ByVal value As Byte) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Int16) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Int32) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Int64) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As String) As OperatorValue
        If value Is Nothing OrElse value.Trim() = "" Then
            Return New OperatorValue("")
        Else
            Return New OperatorValue(value)
        End If
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Double) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Single) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As DateTime) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Shared Shadows Narrowing Operator CType(ByVal value As Decimal) As OperatorValue
        Dim ov As New OperatorValue(value)
        Return ov
    End Operator

    Public Overloads Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        Return _Value.ToString()
    End Function
End Class

''' <summary>
''' 一元运算
''' </summary>
''' <remarks></remarks>
Public Class UnaryOperator
    Inherits CriteriaOperator

    Protected _Operand As CriteriaOperator

    Public Overloads Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        Return _Operand.ToString()
    End Function

    Public Shared Operator Not(ByVal right As UnaryOperator) As UnaryOperator
        Return New NotOperator(right)
    End Operator
End Class

''' <summary>
''' 二元运算
''' </summary>
''' <remarks></remarks>
Public Class BinaryOperator
    Inherits CriteriaOperator

    Private _Left As CriteriaOperator
    Private _Right As CriteriaOperator
    Private _OperatorType As BinaryOperatorType
    Private _paramName As String
    ''' <summary>
    ''' 二元运算类型
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BinaryOperatorType
        Equal = 0
        Greater = 1
        GreaterEqual = 2
        Less = 3
        LessEqual = 4
        NotEqual = 5
        LikeFull = 6
        LikeLeft = 7
        LikeRight = 8
        LikeMiddle = 9
        [in] = 10
    End Enum

    Private Function OperatorTypeToString(ByVal OperatorType As BinaryOperatorType) As String
        Select Case OperatorType
            Case BinaryOperatorType.Equal
                Return "="
            Case BinaryOperatorType.Greater
                Return ">"
            Case BinaryOperatorType.GreaterEqual
                Return ">="
            Case BinaryOperatorType.Less
                Return "<"
            Case BinaryOperatorType.LessEqual
                Return "<="
            Case BinaryOperatorType.NotEqual
                Return "<>"
            Case BinaryOperatorType.in
                Return " in "
            Case Else
                Return " like "
        End Select
    End Function
    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="OperatorType">二元运算类型</param>
    ''' <param name="Left">属性</param>
    ''' <param name="Right">值</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal OperatorType As BinaryOperatorType, ByVal Left As OperatorField, ByVal Right As OperatorValue)
        _OperatorType = OperatorType
        _Left = Left
        _Right = Right
        Dim _SubLeft As String
        If Not Right Is Nothing AndAlso Right.ToString().Trim() <> "%%" Then

            Dim paramValue As Object = CType(_Right, OperatorValue).Value

            If Left.ToString.Length >= 5 Then
                _SubLeft = Left.ToString().Substring(0, 5)
            Else
                _SubLeft = Left.ToString()
            End If
            _paramName = "P_" & _SubLeft & AutoGen.GetUniqueString().Substring(0, 20)

            Dim ri As String = _Right.ToString()
            Select Case _OperatorType
                Case BinaryOperatorType.LikeLeft
                    ri = ri + "%"
                    paramValue = ri
                Case BinaryOperatorType.LikeMiddle
                    ri = "%" + ri + "%"
                    paramValue = ri
                Case BinaryOperatorType.LikeRight
                    ri = "%" + ri
                    paramValue = ri
            End Select
            Dim up As UserDataParameter
            up.ParamName = _paramName
            up.Value = paramValue
            _UserParams.Add(up)
            _UserParams.AddRange(_Left.GetUserParams())
            _UserParams.AddRange(_Right.GetUserParams())
            '_Params.Add(udo.CreateParameter(_paramName, paramValue))
            '_Params.AddRange(_Left.GetParams())
            '_Params.AddRange(_Right.GetParams())
        End If
    End Sub

    Public Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        If _Left Is Nothing Or (_Right Is Nothing OrElse _Right Is DBNull.Value _
                                OrElse _Right.ToString().Trim() = "" OrElse _
                                _Right.ToString().Trim() = "%%") Then
            Return ""
        Else
            If _Right.GetType().Name = "OperatorValue" Then
                If CType(_Right, OperatorValue).Value.GetType().Name.StartsWith("Date") Then
                    If Provider = UserDBProvider.Oracle Then
                        Return "$" & _Left.ToString() & "#" & OperatorTypeToString(_OperatorType) & _
                            "to_date(" & UserDBAgent.GetParamPrev(Provider) & _paramName & ", 'yyyy-MM-dd HH24:MI:SS')"
                    End If
                End If
            End If
            Dim strLeft As String = "$" & _Left.ToString() & "#"
            Dim strRight As String = UserDBAgent.GetParamPrev(Provider) & _paramName
            Dim strOpe As String = OperatorTypeToString(_OperatorType)
            If _OperatorType = BinaryOperatorType.in Then
                If Provider = UserDBProvider.Oracle Then
                    strRight = "instr( ',' || " + strRight + " || ',',',' || " + strLeft + " || ',')>0"
                    strOpe = " "
                    strLeft = ""
                Else
                    strRight = "charindex(',' + cast(" + strLeft + " AS VARCHAR) + ',', ',' + " + strRight + " + ',')>0"
                    strOpe = " "
                    strLeft = ""
                End If
            End If
            Return strLeft & strOpe & strRight
        End If
    End Function

    'Public Shared Shadows Narrowing Operator CType(ByVal BinaryCondition As String) As BinaryOperator
    '    Dim exp As New Regex("LLike|RLike|MLike|Like|<>|>=|<=|>|<|=")
    '    Dim m As Match = exp.Match(BinaryCondition.Trim())
    '    If m.Index > 0 Then
    '        Dim bop As BinaryOperatorType
    '        If m.Value.ToLower() = "llike" Then
    '            bop = BinaryOperatorType.LikeLeft
    '        ElseIf m.Value.ToLower() = "rlike" Then
    '            bop = BinaryOperatorType.LikeRight
    '        ElseIf m.Value.ToLower() = "mlike" Then
    '            bop = BinaryOperatorType.LikeMiddle
    '        ElseIf m.Value.ToLower() = "like" Then
    '            bop = BinaryOperatorType.LikeFull
    '        ElseIf m.Value.ToLower() = "<>" Then
    '            bop = BinaryOperatorType.NotEqual
    '        ElseIf m.Value.ToLower() = ">=" Then
    '            bop = BinaryOperatorType.GreaterEqual
    '        ElseIf m.Value.ToLower() = "<=" Then
    '            bop = BinaryOperatorType.LessEqual
    '        ElseIf m.Value.ToLower() = ">" Then
    '            bop = BinaryOperatorType.Greater
    '        ElseIf m.Value.ToLower() = "<" Then
    '            bop = BinaryOperatorType.Less
    '        Else
    '            bop = BinaryOperatorType.Equal
    '        End If
    '        Dim Left, Right As String
    '        Left = BinaryCondition.Substring(0, m.Index - 1)
    '        Right = BinaryCondition.Substring(m.Index + m.Length, BinaryCondition.Length - m.Index - m.Length + 1)
    '        Try
    '            Return New BinaryOperator(bop, Left, Right)
    '        Catch ex As Exception
    '            Return New BinaryOperator(BinaryOperatorType.Equal, Nothing, Nothing)
    '        End Try
    '    Else
    '        Return New BinaryOperator(BinaryOperatorType.Equal, Nothing, Nothing)
    '    End If
    'End Operator

    'Public Shared Operator And(ByVal Left As BinaryOperator, ByVal Right As CriteriaOperator) As GroupOperator
    '    Dim group As New GroupOperator(GroupOperator.GroupOperatorType.And, Left, Right)
    '    Return group
    'End Operator

    'Public Shared Operator Or(ByVal Left As BinaryOperator, ByVal Right As CriteriaOperator) As GroupOperator
    '    Dim group As New GroupOperator(GroupOperator.GroupOperatorType.Or, Left, Right)
    '    Return group
    'End Operator
End Class

''' <summary>
''' 组之间的逻辑操作类
''' </summary>
''' <remarks></remarks>
Public Class GroupOperator
    Inherits CriteriaOperator

    Public Enum GroupOperatorType
        [Or] = 0
        [And] = 1
    End Enum


    Private _OperatorType As GroupOperatorType
    Private _Left As CriteriaOperator
    Private _Right As CriteriaOperator

    Private Function OperatorTypeToString(ByVal OperatorType As GroupOperatorType) As String
        Select Case OperatorType
            Case GroupOperatorType.And
                Return " and "
            Case GroupOperatorType.Or
                Return " or "
            Case Else
                Return " and "
        End Select
    End Function

    Public Sub New(ByVal OperatorType As GroupOperatorType, ByVal Left As CriteriaOperator, ByVal Right As CriteriaOperator)
        _OperatorType = OperatorType
        _Left = Left
        _Right = Right
        '_Params.AddRange(_Left.GetParams())
        '_Params.AddRange(_Right.GetParams())
        _UserParams.AddRange(_Left.GetUserParams())
        _UserParams.AddRange(_Right.GetUserParams())
    End Sub

    Public Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        Dim strLeft As String = _Left.ToString(Provider).Trim()
        Dim strRight As String = _Right.ToString(Provider).Trim()
        If strLeft = "" Or strRight = "" Then
            Return strLeft & strRight
        Else
            Return " (" & strLeft & OperatorTypeToString(_OperatorType) & strRight & ") "
        End If

    End Function

End Class

Public Class NotOperator
    Inherits UnaryOperator

    Public Sub New(ByVal Operand As CriteriaOperator)
        _Operand = Operand
        If Not _Operand Is Nothing Then _UserParams.AddRange(_Operand.GetUserParams())
    End Sub

    Public Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        Dim str As String = _Operand.ToString(Provider).Trim()
        If _Operand Is Nothing OrElse str = "" Then
            Return ""
        Else
            Return " (not " & str & ") "
        End If
    End Function

End Class

Public Class NullOperator
    Inherits UnaryOperator
    Public Sub New(ByVal Operand As OperatorField)
        _Operand = Operand
    End Sub

    Public Overrides Function ToString(ByVal Provider As UserDBProvider) As String
        If _Operand Is Nothing Then
            Return ""
        Else
            Return "$" & _Operand.ToString(Provider) & "#" & " IS NULL "
        End If
    End Function
End Class

Public Class OrderOperator
    Public Enum OrderType
        Asc = 0
        Desc = 1
    End Enum
    Private _OrderField As String
    Private _OrderType As OrderType

    Public Sub New(ByVal OrderField As String, Optional ByVal OrType As OrderType = OrderType.Asc)
        _OrderField = OrderField
        _OrderType = OrType
    End Sub

    Public Overrides Function ToString() As String
        Dim strOrderType As String = " asc "
        If _OrderType = OrderType.Desc Then strOrderType = " desc "
        Return "$" & _OrderField & "#" & strOrderType
    End Function

    Public Shared Shadows Narrowing Operator CType(ByVal OrderDesc As String) As OrderOperator
        Dim info() As String = OrderDesc.Trim().Split(" ")
        Dim strField As String = ""
        Dim ot As OrderType = OrderType.Asc
        If info.Length = 1 Then
            strField = info(0)
        ElseIf info.Length >= 2 Then
            strField = info(0)
            If info(info.GetUpperBound(0)).Trim().ToLower() = "desc" Then
                ot = OrderType.Desc
            End If
        Else
            Throw New Exception("无效的值传递！")
        End If
        Return New OrderOperator(strField, ot)
    End Operator
End Class

Public Class OrderCollection
    Inherits List(Of OrderOperator)

    Public Overrides Function ToString() As String
        Dim strOrderInfo As String = ""
        Dim i As Integer
        For i = 0 To Count - 1
            strOrderInfo = strOrderInfo & Item(i).ToString() & ","
        Next
        If strOrderInfo.EndsWith(",") Then strOrderInfo = strOrderInfo.Substring(0, strOrderInfo.Length - 1)
        Return strOrderInfo
    End Function

    Public Shared Shadows Narrowing Operator CType(ByVal OrderInfo As String) As OrderCollection
        Dim Orders() As String = OrderInfo.Trim().Split(",")
        Dim i As Integer
        Dim oc As New OrderCollection()
        For i = Orders.GetLowerBound(0) To Orders.GetUpperBound(0)
            oc.Add(Orders(i))
        Next
        Return oc
    End Operator
End Class


Public Module CriteriaModule
    ''' <summary>
    ''' 将字符串转化为OperatorField
    ''' </summary>
    ''' <param name="FieldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function F(ByVal FieldName As String) As OperatorField
        Return New OperatorField(FieldName)
    End Function
    ''' <summary>
    ''' 判断某个字段是否为空
    ''' </summary>
    ''' <param name="Operand"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsNull(ByVal Operand As OperatorField) As NullOperator
        Return New NullOperator(Operand)
    End Function

    Public Function BoolToInt(ByVal value As Boolean) As Integer
        If value Then
            Return 1
        Else
            Return 0
        End If
    End Function
End Module