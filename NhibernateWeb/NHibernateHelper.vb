Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Imports System.Web


Imports NHibernate
Imports NHibernate.Cfg

Namespace NHibernateHelp
#Region "Helper"
    Public NotInheritable Class NHibernateHelper
        Private Const CurrentSessionKey As String = "nhibernate.current_session"
        Private Shared ReadOnly sessionFactory As ISessionFactory

        Shared Sub New()
            sessionFactory = New Configuration().Configure().BuildSessionFactory()
        End Sub

        Public Shared Function GetCurrentSession() As ISession
            Dim context As HttpContext = HttpContext.Current
            Dim currentSession As ISession = TryCast(context.Items(CurrentSessionKey), ISession)

            If currentSession Is Nothing Then
                currentSession = sessionFactory.OpenSession()
                context.Items(CurrentSessionKey) = currentSession
            End If

            Return currentSession
        End Function

        Public Shared Sub CloseSession()
            Dim context As HttpContext = HttpContext.Current
            Dim currentSession As ISession = TryCast(context.Items(CurrentSessionKey), ISession)

            If currentSession Is Nothing Then
                ' No current session
                Return
            End If

            currentSession.Close()
            context.Items.Remove(CurrentSessionKey)
        End Sub

        Public Shared Sub CloseSessionFactory()
            If sessionFactory IsNot Nothing Then
                sessionFactory.Close()
            End If
        End Sub

    End Class
#End Region
End Namespace

