<% 
    If InStr(UCase(Request.ServerVariables("SERVER_NAME")), UCase("dev.vocatus.net")) > 0 Then
        Response.Redirect("/dev")
    End If
%> 
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>

</body>
</html>
