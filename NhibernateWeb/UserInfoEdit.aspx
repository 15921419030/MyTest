<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserInfoEdit.aspx.vb" Inherits="NhibernateWeb.UserInfoEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div  style="background-color:White;border:1px solid #000;">
    <div>
        <asp:Label ID="Label4" runat="server" Text="ID："></asp:Label><asp:TextBox ID="txtID"
            runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="用户名："></asp:Label><asp:TextBox ID="txtNames"
            runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label2" runat="server" Text="年龄："></asp:Label><asp:TextBox ID="txtAge"
            runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label3" runat="server" Text="地址："></asp:Label><asp:TextBox ID="txtAddress"
            runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="btnSave" runat="server" Text="保存" />
    &nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnQuery" runat="server" Text="查询" />
    &nbsp;&nbsp;&nbsp; <asp:Button ID="btnUpdate" runat="server" Text="修改" />
    &nbsp;&nbsp;&nbsp; <asp:Button ID="btnDel" runat="server" Text="删除" />
    </div>
    </div>
    </form>
</body>
</html>
