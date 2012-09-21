<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PicTest.aspx.vb" Inherits="NhibernateWeb.PicTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .box {
	        /*非IE的主流浏览器识别的垂直居中的方法*/
	        display: table-cell;
	        vertical-align:middle;
	        /*设置水平居中*/
	        text-align:center;
	        /* 针对IE的Hack */
	        *display: block;
	        *font-size: 175px;/*约为高度的0.873，200*0.873 约为175*/
	        *font-family:Arial;/*防止非utf-8引起的hack失效问题，如gbk编码*/
	        width:200px;
	        height:200px;
	        border: 1px solid #eee;
        }
        .box img {
	        /*设置图片垂直居中*/
	        vertical-align:middle;
	        /*非IE6下的等比缩放*/
	        max-height:50px;
	        max-width:50px;
	        /*IE6下的等比缩放，注意expression其实是运行了一个JS程序，所以如果图片很多的话会引起CPU占用率高*/
	        width:expression(this.width >50 && this.height < = this.width ? 50: true);    	
	        height:expression(this.height > 50 && this.width < = this.height ? 50 : true);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="box">     
        <a href="#"><img alt="" src="http://www.google.com/intl/en/images/logo.gif" /></a>
    </div> 
    <asp:TextBox ID="txtPic" runat="server"></asp:TextBox>
    </form>
</body>
</html>
