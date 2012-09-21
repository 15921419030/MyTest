<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ValidateTest.aspx.vb" Inherits="NhibernateWeb.ValidateTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="Js/validate/jquery.validate.min.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#form1").validate({
                rules: {
                    TextBox1: "required",
                    TextBox2: "required"
                },
                messages: {
                    TextBox1: "Please enter your firstname",
                    TextBox2: "Please enter your lastname"
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>用户名：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></div>
        <div>密码：<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></div>
        <asp:Button ID="Button1" runat="server" Text="Button" />
    </div>
    </form>
</body>
</html>
