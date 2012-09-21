
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Donglisoft.Data


Partial Public Class MSLibTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim db As Database
        db = DatabaseFactory.CreateDatabase()
        Dim ds As DataSet
        'ds = db.ExecuteDataSet(CommandType.Text, "select * from FRegions")

        ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnectionStr(), CommandType.Text, "select * from FRegions")

        GridView1.DataSource = ds.Tables(0).DefaultView
        GridView1.DataBind()

    End Sub

End Class