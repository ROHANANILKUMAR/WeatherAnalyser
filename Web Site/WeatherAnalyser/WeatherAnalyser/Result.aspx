<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="WeatherAnalyser.Result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
			<asp:Label ID="Response" runat="server" Text="" Font-Size="Large"></asp:Label>
			<asp:Label ID="Error" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
