<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MegerJsCss.aspx.vb" Inherits="NhibernateWeb.MegerJsCss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" href="http://localhost/NhibernateWeb/PagecontrolStyle.css" />
    <link type="text/css" href="Css/index.css" rel="Stylesheet"/>
    <script type="text/javascript" src="http://localhost/NhibernateWeb/Js/jquery?v=js/jquery-1.7.2.js,js/validate/jquery.metadata.js"></script>
    <script type="text/javascript">
        $(function() {
            jQuery.mytest = function() {
                alert("ok");
            }
        })

        $(function() {
            $.mytest();
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%;height:500px;">
    
    </div>
    </form>
</body>
</html>
