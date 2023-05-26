<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testing.aspx.cs" Inherits="WebApplication1.bussiness.production.rpts.testing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" Text="PDF Download" ID="btnExport" CssClass="btn btn-primary btn-sm" OnClick="btnExport_Click" />
        </div>
    </form>
</body>
</html>
