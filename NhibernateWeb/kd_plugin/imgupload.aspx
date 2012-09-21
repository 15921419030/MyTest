<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="imgupload.aspx.vb" Inherits="NhibernateWeb.imgupload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../js/kindeditor/themes/default/default.css" />
	<script src="../Js/kindeditor/kindeditor.js" charset="utf-8"  type="text/javascript"></script>
	<script src="../js/kindeditor/lang/zh_CN.js" charset="utf-8"  type="text/javascript"></script>
	<script type="text/javascript">
	    KindEditor.ready(function(K) {
	        var editor = K.editor({
	            allowFileManager: true
	        });
	        K('#image3').click(function() {
	            editor.loadPlugin('image', function() {
	                    editor.plugin.imageDialog({
	                    imageUrl: K('#url3').val(),
	                    clickFn: function(url, title, width, height, border, align) {
	                        K('#url3').val(url);
	                        editor.hideDialog();
	                    }
	                });
	            });
	        });
	    });
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <input type="text" id="url3" value="" /> <input type="button" id="image3" value="选择图片" />（本地上传）
    </div>
    </form>
</body>
</html>
