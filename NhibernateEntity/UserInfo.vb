Imports System.Collections

Namespace NhibernateEntity
#Region "UserInfo"

    ''' <summary>
    ''' UserInfo object for NHibernate mapped table 'UserInfo'.
    ''' </summary>
    Public Class UserInfo
#Region "Member Variables"

        Protected _id As Integer
        Protected _userName As String
        Protected _age As Integer
        Protected _address As String

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal userName As String, ByVal age As Integer, ByVal address As String)
            Me._userName = userName
            Me._age = age
            Me._address = address
        End Sub

#End Region

#Region "Public Properties"

        Public Overridable Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Overridable Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 50 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for UserName", value, value.ToString())
                End If
                _userName = value
            End Set
        End Property

        Public Overridable Property Age() As Integer
            Get
                Return _age
            End Get
            Set(ByVal value As Integer)
                _age = value
            End Set
        End Property

        Public Overridable Property Address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 50 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString())
                End If
                _address = value
            End Set
        End Property



#End Region
    End Class
#End Region
End Namespace