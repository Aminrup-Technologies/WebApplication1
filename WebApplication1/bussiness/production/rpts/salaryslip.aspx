<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="salaryslip.aspx.cs" Inherits="WebApplication1.bussiness.production.rpts.salaryslip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ATS Salary Slip</title>
    <link rel="shortcut icon" href="../../../erp_images/ats_translogo.png" />
    <style type="text/css">
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table border="1" width="500px">
            <tr>
                <td style="width: 200px;">Select Employee ID</td>
                <td>
                    <asp:DropDownList ID="ddlEmployeeID" runat="server">
                        <asp:ListItem Text="1001" Value="1001">
                        </asp:ListItem>
                        <asp:ListItem Text="1002" Value="1002">
                        </asp:ListItem>
                        <asp:ListItem Text="1003" Value="1003">
                        </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Select Month</td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Text="Aug/2017" Value="Aug/2017">
                        </asp:ListItem>
                        <asp:ListItem Text="Sept/2017" Value="Sept/2017">
                        </asp:ListItem>
                        <asp:ListItem Text="Oct/2017" Value="Oct/2017">
                        </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClick="btnGenerate_Click" /></td>
                <td>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red">
                    </asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
