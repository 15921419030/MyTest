<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UCTest.aspx.vb" Inherits="NhibernateWeb.UCTest" %>

<%@ Register src="PageControl.ascx" tagname="PageControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" rel="Stylesheet" href="Js/artdialog/skins/green.css" />
    <link type="text/css" rel="Stylesheet" href="PagecontrolStyle.css" />
    <script type="text/javascript" src="Js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="Js/artdialog/jquery.artDialog.js"></script>
    <script type="text/javascript" src="Js/artdialog/plugins/iframeTools.js"></script>
    <script type="text/javascript">
        function f_click(o) {
            $("#hidID").val(o.keyvalue);
        }
        function f_dbclick(o) {
            alert("多选");
        }
    </script>
    <script type="text/javascript" >
        $(function() {

            $("#btnEdit").click(function() {
                $.dialog({
                    content: $("#useradd").html(),
                    button: [
                    {
                        name: '确定',
                        callback: function() {
                            this.alert('添加成功!');
                            return false;
                        },
                        focus: true
                    }, {
                        name: '关闭',
                        callback: function() {
                            this.close();
                        }
                    }
                    ]
                });


//                $.dialog.open("./ValidateTest.aspx", { width: '500',
//                    ok: function() {
//                        $.dialog.alert("添加成功!");
//                    }
//                });

            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hidID" />
    <div style="clear:both;">
        <uc1:PageControl ID="PageControl1" runat="server" />
    </div>
    <div style="width:100%;clear:both;">
        <div style="margin:0 auto;background:#eee;text-align:center;">
            <input type="button" id="btnEdit" value="编辑" />
        </div>
    </div>
    </form>
    <div id="useradd" style="display:none;">
        <form id="fAdd" action="">
            <div>用户名：<input id="UID" type="text" /></div>
            <div>密码：<input id="Pwd" type="text" /></div>
        </form>
    </div>
</body>
</html>
