
Imports log4net
Imports System.Data
Imports System.Data.SqlClient
Imports Donglisoft.Data

Partial Public Class GridViewTest
    Inherits System.Web.UI.Page

    Dim log As ILog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''创建日志记录组件实例   
        'log = log4net.LogManager.GetLogger("test")
        ''记录错误日志   
        'log.Error("error", New Exception("发生了一个异常"))
        ''记录严重错误   
        'log.Fatal("fatal", New Exception("发生了一个致命错误"))
        ''记录一般信息   
        'log.Info("info")
        ''记录调试信息   
        'log.Debug("debug")
        ''记录警告信息   
        'log.Warn("warn")

        Dim conn As String = "data source =WANGBF\SQL2008EX;uid=sa;pwd=sa;database=nhibernate;pooling=true"
        'Dim objcn As New SqlConnection(conn)
        'Dim ds As New DataSet
        'objcn.Open()
        'Dim da As New SqlDataAdapter("select * from userinfo", conn)
        'da.Fill(ds)

        'Me.GridView2.DataSource = ds.Tables(0).DefaultView
        'Me.GridView2.DataBind()

        'objcn.Close()


        Dim connection As SqlConnection = Nothing
        Dim ds As DataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT top 100 * FROM userinfo")

        Me.GridView2.DataSource = ds.Tables(0).DefaultView
        Me.GridView2.DataBind()

    End Sub


    Private Function GetConnection(ByVal connectionString As String) As SqlConnection
        Dim connection As New SqlConnection(connectionString)

        connection.Open()

        Return connection
    End Function

End Class