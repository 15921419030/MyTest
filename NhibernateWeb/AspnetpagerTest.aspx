<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AspnetpagerTest.aspx.vb" Inherits="NhibernateWeb.AspnetpagerTest" %>

<%@ Register assembly="AspNetPager" namespace="Wuqi.Webdiyer" tagprefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-size:12px;	
        }
        
        
        DIV.digg
        {
            padding-right: 3px;
            padding-left: 3px;
            padding-bottom: 3px;
            margin: 3px;
            padding-top: 3px;
            text-align: center;
        }
        DIV.digg A
        {
            border-right: #aaaadd 1px solid;
            padding-right: 5px;
            border-top: #aaaadd 1px solid;
            padding-left: 5px;
            padding-bottom: 2px;
            margin: 2px;
            border-left: #aaaadd 1px solid;
            color: #000099;
            padding-top: 2px;
            border-bottom: #aaaadd 1px solid;
            text-decoration: none;
        }
        DIV.digg A:hover
        {
            border-right: #000099 1px solid;
            border-top: #000099 1px solid;
            border-left: #000099 1px solid;
            color: #000;
            border-bottom: #000099 1px solid;
        }
        DIV.digg A:active
        {
            border-right: #000099 1px solid;
            border-top: #000099 1px solid;
            border-left: #000099 1px solid;
            color: #000;
            border-bottom: #000099 1px solid;
        }
        DIV.digg SPAN.current
        {
            border-right: #000099 1px solid;
            padding-right: 5px;
            border-top: #000099 1px solid;
            padding-left: 5px;
            font-weight: bold;
            padding-bottom: 2px;
            margin: 2px;
            border-left: #000099 1px solid;
            color: #fff;
            padding-top: 2px;
            border-bottom: #000099 1px solid;
            background-color: #000099;
        }
        DIV.digg SPAN.disabled
        {
            border-right: #eee 1px solid;
            padding-right: 5px;
            border-top: #eee 1px solid;
            padding-left: 5px;
            padding-bottom: 2px;
            margin: 2px;
            border-left: #eee 1px solid;
            color: #ddd;
            padding-top: 2px;
            border-bottom: #eee 1px solid;
        }
    </style>
    <script type="text/javascript">
        function ChckBoxSelItems() {

        }

        function GridView_selectRow() { 
            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" CellPadding="3" 
            ForeColor="Black" GridLines="Vertical" Width="100%" 
            AlternatingRowStyle-CssClass="alt" BackColor="White" BorderColor="#F00" 
            BorderStyle="Solid" BorderWidth="1px">
            <RowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="ChckBoxSelItems" runat="server" name="ChckBoxSelItems" onclick="ChckBoxSelItems()" type="checkbox" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle CssClass="pgr" BackColor="#999999" ForeColor="Black" 
                HorizontalAlign="Center"></PagerStyle>
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle CssClass="alt" BackColor="#CCCCCC"></AlternatingRowStyle>
        </asp:GridView>
        
        <webdiyer:AspNetPager ID="AspNetPager1" CssClass="digg" runat="server" horizontalalign="Center" width="100%" ShowPageIndexBox="Always"  EnableUrlRewriting="true" UrlRewritePattern="./listpage_{0}.html" OnPageChanged="AspNetPager1_PageChanged">
        </webdiyer:AspNetPager>
        
    </div>
    </form>
</body>
</html>
