Public Partial Class UCTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim conn As String = "data source =WANGBF\SQL2008EX;uid=sa;pwd=sa;database=nhibernate;pooling=true"

        With PageControl1
            .Conn = conn
            .TableName = "v_test"
            .DataField = "ID,Username,Age,Address"
            .DataFieldText = "编号,姓名,年龄,地址"
            .OrderField = "ID"
            .SqlWhere = ""
            .KeyField = "ID"
            .TotalRowsCount = 1000
            .PageSize = 10
            .DisplayCheckBox = True
            .ClickEventFunction = "f_click"
            .DoubleClickEventFunction = "f_dbclick"
            .AscendingImageUrl = "~/Images/arrow_asc.png"
            .DescendingImageUrl = "~/Images/arrow_desc.png"
        End With

    End Sub

End Class