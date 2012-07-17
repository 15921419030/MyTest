
Imports log4net
Imports NHibernate
Imports NHibernate.Cfg
Imports NhibernateEntity.NhibernateEntity

Partial Public Class UserInfoEdit
    Inherits System.Web.UI.Page

    Dim log As ILog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '创建日志记录组件实例   
        log = log4net.LogManager.GetLogger("test")

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
        Dim trans As ITransaction
        Dim userinfo As New UserInfo
        Try
            userinfo.UserName = Me.txtNames.Text
            userinfo.Age = Me.txtAge.Text
            userinfo.Address = Me.txtAddress.Text
            trans = session.BeginTransaction()
            session.Save(userinfo)
            trans.Commit()
        Catch ex As Exception
            log.Error("error", New Exception("发生了一个异常"))
        End Try

    End Sub

End Class