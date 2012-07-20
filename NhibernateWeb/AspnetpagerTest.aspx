<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AspnetpagerTest.aspx.vb" Inherits="NhibernateWeb.AspnetpagerTest" %>

<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>

        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" horizontalalign="Center" width="100%" ShowPageIndexBox="Always"
        EnableUrlRewriting="true" UrlRewritePattern="listpage_{0}.html" OnPageChanged="AspNetPager1_PageChanged" NumericButtonTextFormatString="-{0}-">
        </webdiyer:AspNetPager>
    
    </div>
    </form>
</body>
</html>
