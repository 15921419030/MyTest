'********************************************************
'ModuleName: Entity Class V_test
'Author:     Fpl Tools
'CreateDate: 2012-09-20
'********************************************************
Imports Donglisoft.Entlib
<MapTo("v_test")> _
Public Class V_test
    Inherits DataObject
    Private _Id As Integer
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
