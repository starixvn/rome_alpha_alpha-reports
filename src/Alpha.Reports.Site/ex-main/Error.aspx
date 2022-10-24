<%@ Page Language="C#" EnableSessionState="True" AutoEventWireup="false" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Error!!!!!</title>
    <style type="text/css">
        .error-bg {height: 150px; width: 150px; margin-top: 100px;}
        .error-txt {width: 90%;}
        .error-txt h3 {color: #555; font-family: Arial;}
        .error-txt-dark h3{
            color: #EDEDED;
            font-family: Arial;
        }
    </style>
</head>
<body>
    <center>
        <div class="error-bg <%=DarkModeName%>" style="background: url(<%= "../App_Themes/error2" + DarkModeName + ".png" %>) no-repeat">
        </div>
        <div class="error-txt<%=DarkModeName%>">
            <h3><%= ErrorMessage %></h3>
        </div>
    </center>
</body>
</html>
