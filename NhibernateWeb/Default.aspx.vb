
Imports log4net
Imports NHibernate
Imports NHibernate.Cfg
Imports NhibernateEntity.NhibernateEntity

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Dim log As ILog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '创建日志记录组件实例   
        log = log4net.LogManager.GetLogger("王百锋")
        '记录错误日志   
        log.Error("error", New Exception("发生了一个异常"))
        '记录严重错误   
        log.Fatal("fatal", New Exception("发生了一个致命错误"))
        '记录一般信息   
        log.Info("info")
        '记录调试信息   
        log.Debug("debug")
        '记录警告信息   
        log.Warn("warn")

        BindGwData()

    End Sub

    Private Sub BindGwData()
        Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
        Try
            Dim query As IQuery = session.CreateQuery("from UserInfo")
            GridView2.DataSource = query.List(Of UserInfo)()
            GridView2.DataBind()
        Catch ex As Exception
            'Throw New Exception(ex.Message)
        End Try
    End Sub

    'Private Sub AddUserInfo()

    'End Sub

    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    Dim trans As ITransaction
    '    Dim userinfo As New UserInfo
    '    Try
    '        userinfo.UserName = Me.txtNames.Text
    '        userinfo.Age = Me.txtAge.Text
    '        userinfo.Address = Me.txtAddress.Text
    '        trans = session.BeginTransaction()
    '        session.Save(userinfo)
    '        trans.Commit()
    '    Catch ex As Exception
    '        log.Error("error", New Exception("发生了一个异常"))
    '    End Try

    '    BindGwData()

    'End Sub

    'Protected Sub btnQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQuery.Click
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    Dim userinfo As UserInfo
    '    Dim trans As ITransaction = session.BeginTransaction()
    '    Try
    '        'userinfo = DirectCast(session.[Get](GetType(UserInfo), Integer.Parse(Me.txtID.Text)), UserInfo)
    '        'trans.Commit()
    '        'Me.txtNames.Text = userinfo.UserName
    '        'Me.txtAge.Text = userinfo.Age
    '        'Me.txtAddress.Text = userinfo.Address

    '        userinfo = findById(Integer.Parse(Me.txtID.Text))
    '        Me.txtNames.Text = userinfo.UserName
    '        Me.txtAge.Text = userinfo.Age
    '        Me.txtAddress.Text = userinfo.Address

    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Function findById(ByVal id As Integer) As UserInfo
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    Dim info As UserInfo = session.[Get](Of UserInfo)(id)
    '    Return info
    'End Function

    'Private Sub Update(ByRef userinfo As UserInfo)
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    session.Update(userinfo)
    '    session.Flush()
    '    session.Close()
    'End Sub

    'Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
    '    Dim info As UserInfo
    '    info = findById(Integer.Parse(Me.txtID.Text))
    '    info.Address = Me.txtAddress.Text
    '    Update(info)
    'End Sub

    'Private Sub Delete(ByRef userinfo As UserInfo)
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    session.Delete(userinfo)
    '    session.Flush()
    '    session.Close()
    'End Sub

    'Protected Sub btnDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDel.Click
    '    Dim info As UserInfo
    '    info = findById(Integer.Parse(Me.txtID.Text))
    '    Delete(info)
    '    BindGwData()
    'End Sub

    'Private Function Create(ByRef userinfo As UserInfo) As Integer

    '    Dim newID As Integer
    '    Dim session As ISession = NHibernateHelp.NHibernateHelper.GetCurrentSession()
    '    newID = session.Save(userinfo)
    '    session.Flush()
    '    session.Close()

    'End Function
End Class
