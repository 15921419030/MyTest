'********************************************************
'ModuleName: Entity Class Userinfo
'Author:     Fpl Tools
'CreateDate: 2012-09-20
'********************************************************
Imports Donglisoft.Entlib
<MapTo("UserInfo")> _
Public Class Userinfo
    Inherits DataObject
    Private _Id As Integer
    <PrimaryKey(True)> _
    <Identity(True)> _
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property
    Private _UserName As String
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property
    Private _Age As Integer
    Public Property Age() As Integer
        Get
            Return _Age
        End Get
        Set(ByVal value As Integer)
            _Age = value
        End Set
    End Property
    Private _Address As String
    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property
End Class
