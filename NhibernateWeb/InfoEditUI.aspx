<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InfoEditUI.aspx.vb" Inherits="NhibernateWeb.InfoEditUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Js/jquery-1.7.2.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "InfoUI.aspx";
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户信息管理
    </div>
    <div>
        <input type="button" id="btnBack" value="返回" />
    </div>
    </form>
</body>
</html>
