'********************************************************
'ModuleName: Entity Class Iptocity
'Author:     Fpl Tools
'CreateDate: 2012-09-20
'********************************************************
Imports Donglisoft.Entlib
<MapTo("IPToCity")> _
Public Class Iptocity
    Inherits DataObject
    Private _IP_ID As Integer
    Public Property IP_ID() As Integer
        Get
            Return _IP_ID
        End Get
        Set(ByVal value As Integer)
            _IP_ID = value
        End Set
    End Property
    Private _IP_Start As Double
    Public Property IP_Start() As Double
        Get
            Return _IP_Start
        End Get
        Set(ByVal value As Double)
            _IP_Start = value
        End Set
    End Property
    Private _IP_End As Double
    Public Property IP_End() As Double
        Get
            Return _IP_End
        End Get
        Set(ByVal value As Double)
            _IP_End = value
        End Set
    End Property
    Private _IP_Province As String
    Public Property IP_Province() As String
        Get
            Return _IP_Province
        End Get
        Set(ByVal value As String)
            _IP_Province = value
        End Set
    End Property
    Private _IP_City As String
    Public Property IP_City() As String
        Get
            Return _IP_City
        End Get
        Set(ByVal value As String)
            _IP_City = value
        End Set
    End Property
End Class
