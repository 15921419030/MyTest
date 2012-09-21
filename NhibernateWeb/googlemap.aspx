<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="googlemap.aspx.vb" Inherits="NhibernateWeb.googlemap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>

    <script type="text/javascript" src="https://maps.google.com/maps/api/js?sensor=false&language=zh"></script> 
    <script type="text/javascript">
        function initialize() {
            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var myOptions = { zoom: 8, center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"),
          myOptions);
        }  
    </script>

</head>
<body onload="initialize()"> 
    <form id="form1" runat="server">
    <div>
        <div id="map_canvas" style="width:100%; height:500px;"></div> 
    </div>
    </form>
</body>
</html>
