
Imports System.Data
Imports System.Data.SqlClient
Imports Donglisoft.Data

Partial Public Class AspnetpagerTest
    Inherits System.Web.UI.Page
    Dim conn As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = "data source =WANGBF\SQL2008EX;uid=sa;pwd=sa;database=nhibernate;pooling=true"

        'Dim connection As SqlConnection = Nothing
        'Dim ds As DataSet = SqlHelper.ExecuteDataset(conn, CommandType.Text, "SELECT * FROM userinfo")

        'Me.GridView1.DataSource = ds.Tables(0).DefaultView
        'Me.GridView1.DataBind()


        If Not IsPostBack Then
            'cache the number of total records to improve performance
            Dim obj As Object = Cache(Convert.ToString([GetType]()) & "totalOrders")
            If obj Is Nothing Then
                Dim totalOrders As Integer = CInt(SqlHelper.ExecuteScalar(conn, "P_GetCustomNumber"))
                Cache(Convert.ToString([GetType]()) & "totalOrders") = totalOrders
                AspNetPager1.RecordCount = totalOrders
            Else
                AspNetPager1.RecordCount = CInt(obj)
            End If

            BindGridView("id", "asc")

        End If

    End Sub

    Protected Sub AspNetPager1_PageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AspNetPager1.PageChanged
        BindGridView("id", "asc")
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting

        If String.IsNullOrEmpty(ViewState("sortdirection")) Then
            ViewState("sortdirection") = SortDirection.Ascending.ToString()
        Else
            If ViewState("sortdirection") = SortDirection.Ascending.ToString() Then
                ViewState("sortdirection") = SortDirection.Descending.ToString()
            Else
                ViewState("sortdirection") = SortDirection.Ascending.ToString()
            End If
        End If

        Dim strOrderType As String
        strOrderType = IIf(ViewState("sortdirection") = "Ascending", "desc", "asc")

        BindGridView(e.SortExpression.ToString(), strOrderType)

    End Sub


    Public Sub BindGridView(ByVal strOrder As String, ByVal strOrderType As String)

        Dim paras As SqlParameter() = New SqlParameter() { _
        New SqlParameter("@tblName", "v_test"), _
        New SqlParameter("@strFields", "*"), _
        New SqlParameter("@strOrder", strOrder), _
        New SqlParameter("@strOrderType", strOrderType), _
        New SqlParameter("@PageSize", AspNetPager1.PageSize), _
        New SqlParameter("@PageIndex", AspNetPager1.CurrentPageIndex), _
        New SqlParameter("@strWhere", "")}

        GridView1.DataSource = SqlHelper.ExecuteReader(conn, "proc_SplitPage", paras)
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'e.Row.ID = e.Row.Cells[0].Text
                e.Row.Attributes.Add("onclick", "GridView_selectRow(this)")
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class