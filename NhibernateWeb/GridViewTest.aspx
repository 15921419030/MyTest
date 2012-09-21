<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GridViewTest.aspx.vb" Inherits="NhibernateWeb.GridViewTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="Js/colorbox/colorbox.css" />
    <script type="text/javascript" src="Js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="Js/colorbox/jquery.colorbox-min.js"></script>
    <script type="text/javascript">
        $(function() {
            $(".ajax").colorbox({
                width: "500",
                height: "250",
                onClosed: function() {
                    alert("关闭!");
                }
            });
            $("#btnAdd").click(function() {
                $(".ajax").attr("href", "UserInfoEdit.aspx");
                $(".ajax").attr("title", "添加用户信息");
                $(".ajax").click();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
            EnableSortingAndPagingCallbacks="True">
        </asp:GridView>
    
    </div>
    <div>
        <a class='ajax' href="" title="Homer Defined" style="display:none;">Outside HTML (Ajax)</a>
        <input id="btnAdd" type="button" value="添加" />
    </div>
    <div>
        测试
    </div>
    </form>
</body>
</html>
