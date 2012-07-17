Imports System.Collections

Namespace NhibernateEntity
#Region "FRegion"

    ''' <summary>
    ''' FRegion object for NHibernate mapped table 'FRegions'.
    ''' </summary>
    Public Class FRegion
#Region "Member Variables"

        Protected _id As String
        Protected _names As String
        Protected _fartherID As String
        Protected _depth As Integer
        Protected _path As String
        Protected _isLeaf As Integer
        Protected _islock As Integer
        Protected _sort As Integer

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal names As String, ByVal fartherID As String, ByVal depth As Integer, ByVal path As String, ByVal isLeaf As Integer, ByVal islock As Integer, _
         ByVal sort As Integer)
            Me._names = names
            Me._fartherID = fartherID
            Me._depth = depth
            Me._path = path
            Me._isLeaf = isLeaf
            Me._islock = islock
            Me._sort = sort
        End Sub

#End Region

#Region "Public Properties"

        Public Overridable Property Id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 10 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString())
                End If
                _id = value
            End Set
        End Property

        Public Overridable Property Names() As String
            Get
                Return _names
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 50 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Names", value, value.ToString())
                End If
                _names = value
            End Set
        End Property

        Public Overridable Property FartherID() As String
            Get
                Return _fartherID
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 10 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for FartherID", value, value.ToString())
                End If
                _fartherID = value
            End Set
        End Property

        Public Overridable Property Depth() As Integer
            Get
                Return _depth
            End Get
            Set(ByVal value As Integer)
                _depth = value
            End Set
        End Property

        Public Overridable Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 150 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Path", value, value.ToString())
                End If
                _path = value
            End Set
        End Property

        Public Overridable Property IsLeaf() As Integer
            Get
                Return _isLeaf
            End Get
            Set(ByVal value As Integer)
                _isLeaf = value
            End Set
        End Property

        Public Overridable Property Islock() As Integer
            Get
                Return _islock
            End Get
            Set(ByVal value As Integer)
                _islock = value
            End Set
        End Property

        Public Overridable Property Sort() As Integer
            Get
                Return _sort
            End Get
            Set(ByVal value As Integer)
                _sort = value
            End Set
        End Property



#End Region
    End Class
#End Region
End Namespace