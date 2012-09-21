
''' <summary>
''' 指定实体对应数据库名称 
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Class)> _
Public Class DBNameAttribute
    Inherits Attribute

    Private _DBName As String

    Public Sub New(ByVal DBName As String)
        _DBName = DBName
    End Sub

    Public ReadOnly Property DBName() As String
        Get
            Return _DBName
        End Get
    End Property
End Class
''' <summary>
''' 指定实体对应架构名称
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Class)> _
Public Class SchemaAttribute
    Inherits Attribute
    Private _Schema As String

    Public Sub New(ByVal Schema As String)
        _Schema = Schema
    End Sub

    Public ReadOnly Property Schema() As String
        Get
            Return _Schema
        End Get
    End Property
End Class

''' <summary>
''' 指定实体对象匹配的数据库表或字段名称
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Class Or AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class MapToAttribute
    Inherits Attribute
    Private _MapTo As String

    Public Sub New(ByVal MapTo As String)
        _MapTo = MapTo
    End Sub

    Public ReadOnly Property MapTo() As String
        Get
            Return _MapTo
        End Get
    End Property

End Class
''' <summary>
''' 指定是否是主键，缺省为非主键
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class PrimaryKeyAttribute
    Inherits Attribute
    Private _IsPrimaryKey As Boolean

    Public Sub New(ByVal IsPrimaryKey As Boolean)
        _IsPrimaryKey = IsPrimaryKey
    End Sub

    Public ReadOnly Property IsPrimaryKey() As Boolean
        Get
            Return _IsPrimaryKey
        End Get
    End Property
End Class
''' <summary>
''' 指定是否是外键，缺省为非外键
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class ForeignKeyAttribute
    Inherits Attribute

    Public Sub New()

    End Sub

End Class

<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class NTextOrBlobAttribute
    Inherits Attribute

    Public Sub New()

    End Sub
End Class
''' <summary>
''' 标明字段长度，如果是字符型为充许的最大长度；如果是数值型，为充许的最大值；如果是二进制类型，表示充许的最大字节数
''' </summary>
''' <remarks>字符型默认为50,数值默认为0,二进制默认为0</remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class SizeAttribute
    Inherits Attribute
    Private _Size As Int32

    Public Sub New(ByVal Size As Int32)
        _Size = Size
    End Sub

    Public ReadOnly Property Size() As Int32
        Get
            Return _Size
        End Get
    End Property
End Class
''' <summary>
''' 标明字段是否充许为空，默认为非空
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class AllowNullAttribute
    Inherits Attribute
    Private _IsAllowNull As Boolean

    Public Sub New(ByVal IsAllowNull As Boolean)
        _IsAllowNull = IsAllowNull
    End Sub

    Public ReadOnly Property IsAllowNull() As Boolean
        Get
            Return _IsAllowNull
        End Get
    End Property
End Class
''' <summary>
''' 标识是否是自增字段
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class IdentityAttribute
    Inherits Attribute
    Private _IsIdentity As Boolean

    Public Sub New(ByVal IsIdentity As Boolean)
        _IsIdentity = IsIdentity
    End Sub

    Public ReadOnly Property IsIdentity() As Boolean
        Get
            Return _IsIdentity
        End Get
    End Property

End Class
''' <summary>
''' 标明属于锁定控制字段
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class LockFieldAttribute
    Inherits Attribute

    Public Sub New()

    End Sub

End Class
''' <summary>
''' 标明属于排序控制字段
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class OrderFieldAttribute
    Inherits Attribute

    Public Sub New()

    End Sub

End Class
''' <summary>
''' 标明数据字典内容字段
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class NameFieldAttribute
    Inherits Attribute

    Public Sub New()

    End Sub

End Class
''' <summary>
''' 标明数据字段显示标题
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class TitleAttribute
    Inherits Attribute
    Private _Title As String
    Public Sub New(ByVal Title As String)
        _Title = Title
    End Sub

    Public ReadOnly Property Title() As String
        Get
            Return _Title
        End Get
    End Property
End Class
''' <summary>
''' 标明域或属性不持久化
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public Class NonPersistedAttribute
    Inherits Attribute

    Public Sub New()

    End Sub
End Class