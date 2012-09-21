
Imports MySql.Data
Imports MySql.Data.MySqlClient

Partial Public Class MysqlAspnetpager
    Inherits System.Web.UI.Page

    Private strconn As String = "server=localhost;user id=root; password=111111; database=test; pooling=false"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim strsql As String
            Dim ds As New DataSet
            Dim da As MySqlDataAdapter
            Dim conn As New MySqlConnection(strconn)

            strsql = "select count(id) from myuser"
            da = New MySqlDataAdapter(strsql, conn)
            da.Fill(ds)
            AspNetPager1.RecordCount = ds.Tables(0).Rows(0)(0)
            BindGridView()
        End If

    End Sub

    Protected Sub AspNetPager1_PageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AspNetPager1.PageChanged

        BindGridView()

    End Sub

    Private Sub BindGridView()

        Dim strSql As String
        Dim pageSize As Integer = AspNetPager1.PageSize
        Dim ds As New DataSet
        Dim da As MySqlDataAdapter
        Dim conn As New MySqlConnection(strconn)

        strSql = "SELECT u.ID,u.Names,u.Sex,u.Age from myuser as u order by u.ID desc limit " + AspNetPager1.StartRecordIndex.ToString() + "," + pageSize.ToString() + ""
        da = New MySqlDataAdapter(strSql, conn)
        da.Fill(ds)


        GridView1.DataSource = ds.Tables(0).DefaultView
        GridView1.DataBind()
        'lblPage.Text = AspNetPager1.CurrentPageIndex.ToString() + "/" + AspNetPager1.PageCount.ToString() + "页    " + AspNetPager1.StartRecordIndex.ToString() + "-" + AspNetPager1.EndRecordIndex.ToString() + "/" + AspNetPager1.RecordCount.ToString() + "条记录";

    End Sub

End Class