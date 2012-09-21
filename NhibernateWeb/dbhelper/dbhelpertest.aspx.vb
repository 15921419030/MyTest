

Imports MySql.Data.MySqlClient
Imports Donglisoft.Data


Partial Public Class dbhelpertest
    Inherits System.Web.UI.Page


    Protected MyStr As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim conn As String = "data source =WANGBF\SQL2008EX;uid=sa;pwd=sa;database=nhibernate;pooling=true"
        'GridView1.DataSource = SqlServerHelper.ReadTable(conn, CommandType.Text, "select * from FRegions")
        'GridView1.DataBind()

        Dim i As Integer = 1

        If i = 1 Then
            GoTo finish
        End If

finish:

        Dim conn As String = System.Configuration.ConfigurationManager.ConnectionStrings("MySqlHelper").ConnectionString

        GridView1.DataSource = MySqlHelper.ReadTable(conn, CommandType.Text, "select * from myuser ")
        GridView1.DataBind()


        'Dim conn As String = "Data Source=ANIPRODB;User Id=fubouser;Password=111111;Integrated Security=no"

        'GridView1.DataSource = OracleHelper.ReadTable(conn, CommandType.Text, "select * from farms ")
        'GridView1.DataBind()



        'Dim test As IMytest
        'Dim mytest As String
        'mytest = test.Mytest()

        MyStr = "Protected变量"




    End Sub

    Public Overloads Function toString() As String
        Return "test"
    End Function

End Class