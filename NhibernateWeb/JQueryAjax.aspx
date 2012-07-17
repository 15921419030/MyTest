<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="JQueryAjax.aspx.vb" Inherits="NhibernateWeb.JQueryAjax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Js/jquery-1.7.2.min.js"></script>
    <style type="text/css">
        body{margin:0px;}
        #mengbanTb{width:200px;height:50px;border:1px solid #000;margin:0 auto;background-color:White;z-index:100;}
    </style>
    <script type="text/javascript">

        //创建遮罩层
        jQuery.createMask = function() {
            var height = document.documentElement.clientHeight;
            var width = document.documentElement.clientWidth;
            var bodyHeight = $("body").height();

            if (bodyHeight > height) {
                height = bodyHeight;
            }

            var mask = {};
            if ($("#mask_div").length == 0) {
                $("body").append('<div id="mask_div" style="position:absolute;top:0;left:0;filter:alpha(opacity=80);-moz-opacity:0.8;opacity:.8;"></div>')
            }
            mask = $("#mask_div");
            mask.css({ "width": width, "height": height, "background": "#ccc" });
        };

        //移除遮罩层
        jQuery.removeMask = function() {
            $("#mask_div").remove();
        };


        function ShowLoading() {
//            var w=$(window).width();
//            var h=$(window).height();
//            $('#mengban').css("width",w).css("height",h)
//            .css("background","#ddd").css("z-index","0")
//            .css("position", "absolute").css("opacity", 0.8);
//            $('#mengban').css("left", 0).css("top", 0);
//            $('#mengbanTb').css("margin-top", (h/2-50));
            //            $('#mengbanTb').show();

            $.createMask();
            
        }

        function HideLoading() { 
            
        }

        $(function() {
            $("#btnAjax").click(function() {
                alert("test");
                ShowLoading();

                //                if ($("#txtID").val() != "") {
                //                    $.ajax({
                //                        type: "get",
                //                        url: "http://www.cnblogs.com/rss",
                //                        beforeSend: function() {
                //                            //ShowLoading();
                //                        },
                //                        success: function(data, textStatus) {
                //                            $(".ajax.ajaxResult").html("");
                //                            $("item", data).each(function(i, domEle) {
                //                                $(".ajax.ajaxResult").append("<li>" + $(domEle).children("title").text() + "</li>");
                //                            });
                //                        },
                //                        complete: function(XMLHttpRequest, textStatus) {
                //                            //HideLoading();
                //                        },
                //                        error: function() {
                //                            //请求出错处理
                //                        }
                //                    });
                //                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        用户编号：<asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    
    </div>
    <div>
        <input id="btnAjax" type="button" value="Ajax测试" />
    </div>
    <div id="mengban">
        <div id="mengbanTb">正在保存信息......</div>
    </div>
    </form>
</body>
</html>
