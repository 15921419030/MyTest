<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PageControl.ascx.vb" Inherits="NhibernateWeb.PageControl" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate> 
        <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="true" 
            AutoGenerateColumns="false" CellPadding="1" CssClass="PagerControlStyleGrid">
            <RowStyle CssClass="GridViewRowStyle" HorizontalAlign="Center"/>    
            <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
            <HeaderStyle CssClass="GridViewHeaderStyle" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="ChckBoxSelItems" runat="server" name="ChckBoxSelItems" onclick="ChckBoxSelItems(this)" type="checkbox" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <webdiyer:AspNetPager ID="AspNetPager1" CssClass="aspnetPagerStyle" runat="server"  horizontalalign="Center" 
            ShowCustomInfoSection="Left" CustomInfoSectionWidth="300" ShowPageIndexBox="always" PageIndexBoxType="DropDownList"
            CustomInfoHTML="第<font color='red'><b>%currentPageIndex%</b></font>页，共%PageCount%页，每页显示%PageSize%条记录"
            FirstPageText="首页" LastPageText="尾页" PrevPageText="上页" NextPageText="下页">
        </webdiyer:AspNetPager>
    </ContentTemplate>
</asp:UpdatePanel>
<input id="hid_SelectedItems" style="WIDTH: 17px; HEIGHT: 22px" type="hidden" size="1" name="hid_SelectedItems" />
<script type="text/javascript">
    var prevselitem = null;
    var currentClass = null;
    var prwvselitemClass = null;
    function f_PagerControl_onclick(row) {
        if (prevselitem != null) {
            prevselitem.className = prwvselitemClass;
        }
        prwvselitemClass = currentClass;
        row.className = 'GridViewSelectedRowStyle';
        prevselitem = row;
    }

    //全选&全不选
    function ChckBoxSelItems(o) {
        $(".PagerControlStyleGrid input[type=checkbox]").attr("checked", o.checked);
    }
 </script>
