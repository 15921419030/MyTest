
Imports MySql.Data.MySqlClient

Partial Public Class MySQL
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim connStr As String
        Dim conn As New MySqlConnection
        Dim da As MySqlDataAdapter
        Dim ds As New DataSet

        connStr = "server=localhost;user id=root; password=111111; database=test; pooling=false"

        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            da = New MySqlDataAdapter("select * from myuser", conn)
            da.Fill(ds)
            Me.GridView1.DataSource = ds.Tables(0).DefaultView
            Me.GridView1.DataBind()

        Catch ex As Exception

        End Try


    End Sub

End Class