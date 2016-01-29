<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="admin_giftcard_index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="padding: 10px">
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="lbPasscode" runat="server" Text="Passcode:" style='font-family:arial;font-size:12px'></asp:Label><br />
        <asp:TextBox ID="tbPasscode" runat="server" style='font-family:arial;font-size:12px'></asp:TextBox><br />
        <asp:Button ID="btGo" runat="server" Text="   Go   " OnClick="btGo_Click" style='font-family:arial;font-size:12px' />
    
        <br />
        <br />
        <asp:Label ID="lbMessage" style='font-family:arial;font-size:12px' runat="server"></asp:Label>
    
    </div>
    </form>
</body>
</html>
