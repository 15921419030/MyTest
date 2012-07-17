Imports System.Collections

Namespace NhibernateEntity
#Region "Log"

    ''' <summary>
    ''' Log object for NHibernate mapped table 'Log'.
    ''' </summary>
    Public Class Log
#Region "Member Variables"

        Protected _id As Integer
        Protected _date As DateTime
        Protected _thread As String
        Protected _level As String
        Protected _logger As String
        Protected _message As String
        Protected _exception As String

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal [date] As DateTime, ByVal thread As String, ByVal level As String, ByVal logger As String, ByVal message As String, ByVal exception As String)
            Me._date = [date]
            Me._thread = thread
            Me._level = level
            Me._logger = logger
            Me._message = message
            Me._exception = exception
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

        Public Overridable Property [Date]() As DateTime
            Get
                Return _date
            End Get
            Set(ByVal value As DateTime)
                _date = value
            End Set
        End Property

        Public Overridable Property Thread() As String
            Get
                Return _thread
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 255 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Thread", value, value.ToString())
                End If
                _thread = value
            End Set
        End Property

        Public Overridable Property Level() As String
            Get
                Return _level
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 50 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Level", value, value.ToString())
                End If
                _level = value
            End Set
        End Property

        Public Overridable Property Logger() As String
            Get
                Return _logger
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 255 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Logger", value, value.ToString())
                End If
                _logger = value
            End Set
        End Property

        Public Overridable Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 4000 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString())
                End If
                _message = value
            End Set
        End Property

        Public Overridable Property Exception() As String
            Get
                Return _exception
            End Get
            Set(ByVal value As String)
                If value IsNot Nothing AndAlso value.Length > 2000 Then
                    Throw New ArgumentOutOfRangeException("Invalid value for Exception", value, value.ToString())
                End If
                _exception = value
            End Set
        End Property



#End Region
    End Class
#End Region
End Namespace