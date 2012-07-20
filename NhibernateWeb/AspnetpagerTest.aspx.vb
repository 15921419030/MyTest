
Imports System.Data
Imports System.Data.SqlClient

Public Class AspnetpagerTest
    Inherits System.Web.UI.Page

    Dim conn As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        conn = "Data Source=LBWIN7;Initial Catalog=AspNetPageDB;User ID=sa;Password=sa"

        If Not IsPostBack Then
            'cache the number of total records to improve performance
            Dim obj As Object = Cache(Convert.ToString([GetType]()) & "totalOrders")
            If obj Is Nothing Then
                Dim totalOrders As Integer = CInt(SqlHelper.ExecuteScalar(conn, "P_GetOrderNumber", Nothing))
                Cache(Convert.ToString([GetType]()) & "totalOrders") = totalOrders
                AspNetPager1.RecordCount = totalOrders
            Else
                AspNetPager1.RecordCount = CInt(obj)
            End If
        End If

    End Sub

    Protected Sub AspNetPager1_PageChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AspNetPager1.PageChanged
        GridView1.DataSource = SqlHelper.ExecuteReader(conn, "P_GetPagedOrders2000", New SqlParameter("@startIndex", AspNetPager1.StartRecordIndex), New SqlParameter("@endIndex", AspNetPager1.EndRecordIndex))
        GridView1.DataBind()

    End Sub

End Class