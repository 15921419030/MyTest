'********************************************************
'ModuleName: Entity Class Fregions
'Author:     Fpl Tools
'CreateDate: 2012-09-20
'********************************************************
Imports Donglisoft.Entlib

<MapTo("FRegions")> _
Public Class Fregions
    Inherits DataObject
    Private _ID As String
    <PrimaryKey(True)> _
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property
    Private _Names As String
    Public Property Names() As String
        Get
            Return _Names
        End Get
        Set(ByVal value As String)
            _Names = value
        End Set
    End Property
    Private _FartherID As String
    Public Property FartherID() As String
        Get
            Return _FartherID
        End Get
        Set(ByVal value As String)
            _FartherID = value
        End Set
    End Property
    Private _Depth As Integer
    Public Property Depth() As Integer
        Get
            Return _Depth
        End Get
        Set(ByVal value As Integer)
            _Depth = value
        End Set
    End Property
    Private _Path As String
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property
    Private _IsLeaf As Integer
    Public Property IsLeaf() As Integer
        Get
            Return _IsLeaf
        End Get
        Set(ByVal value As Integer)
            _IsLeaf = value
        End Set
    End Property
    Private _Islock As Integer
    Public Property Islock() As Integer
        Get
            Return _Islock
        End Get
        Set(ByVal value As Integer)
            _Islock = value
        End Set
    End Property
    Private _Sort As Integer
    Public Property Sort() As Integer
        Get
            Return _Sort
        End Get
        Set(ByVal value As Integer)
            _Sort = value
        End Set
    End Property
End Class
