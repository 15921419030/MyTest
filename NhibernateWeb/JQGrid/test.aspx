<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="test.aspx.vb" Inherits="NhibernateWeb.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" media="screen" href="../JQueryUI/css/ui-lightness/jquery-ui-1.8.20.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="jqgrid/css/ui.jqgrid.css" />
    <script src="jqgrid/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="jqgrid/js/i18n/grid.locale-cn.js" type="text/javascript"></script>
    <script src="jqgrid/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            jQuery("#list2").jqGrid({
                url: 'JQGridService.asmx/GetJQGridData2',
                data: {},
                datatype: 'json',
                mtype: 'POST',
                loadonce: true,
                ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
                serializeGridData: function(postData) {
                    return JSON.stringify(postData);
                },
                jsonReader: {
                    root: function(obj) {
                        var data = eval("(" + obj.d + ")");
                        return data.rows;
                    },
                    page: function(obj) {
                        var data = eval("(" + obj.d + ")");
                        return data.page;
                    },
                    total: function(obj) {
                        var data = eval("(" + obj.d + ")");
                        return data.total;
                    },
                    records: function(obj) {
                        var data = eval("(" + obj.d + ")");
                        return data.records;
                    },
                    repeatitems: false
                },
                colNames: ['Inv No', 'name', 'sex', 'email', 'age'],
                colModel: [
                    { name: 'ID', index: 'ID', width: 55 },
                    { name: 'Names', index: 'Names', width: 90 },
                    { name: 'Sex', index: 'Sex', width: 100 },
                    { name: 'Age', index: 'Age', width: 80, align: "right" },
                    { name: 'Remark', index: 'Remark', width: 80, align: "right" }
                ],
                rowNum: 10,
                width: 600,
                rowList: [10, 20, 30],
                pager: '#pager2',
                sortname: 'name',
                viewrecords: true,
                sortorder: "asc",
                caption: "JSON Example"
            });
            jQuery("#list2").jqGrid('navGrid', '#pager2', { edit: false, add: false, del: false });
        });  
    </script> 
</head>
<body>
    <form id="form1" runat="server">
    <div>  
        <table id="list2" style="width:600px;"></table> 
        <div id="pager2"></div>  
    </div>  
    </form>
</body>
</html>
