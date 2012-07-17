<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InfoUI.aspx.vb" Inherits="NhibernateWeb.InfoUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Js/jquery-1.7.2.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#btnAdd").click(function() {
            window.location.href = "InfoEditUI.aspx";
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="text" id="test" />
        <input type="button" id="btnAdd" value="添加" />
    </div>
    </form>
</body>
</html>
