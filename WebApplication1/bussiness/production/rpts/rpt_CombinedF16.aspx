<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_CombinedF16.aspx.cs" Inherits="WebApplication1.bussiness.production.rpts.rpt_CombinedF16" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Combined F16</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="printable" style="width:1244px">
            <table style="border-collapse:collapse;">
                <thead style="border-collapse:collapse;">
                    <tr>
                        <td style="width:1244px">
                            <%--<table style="width:1244px;" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="border: 1px solid #595959; background-color: white; width:50%; border-right:none; padding:3px;" align="center">
                                        <span>
                                            <img src="../../images/ueshortlogo.jpg" alt="Logo"/>
                                        </span>
                                    </td>
                                    <td style="border: 1px solid #595959; background-color: white; width:50%;" align="center">
                                        <span style="font: normal 26px/24px Century Gothic; font-weight: bold;">FORM 29</span>
                                    </td>
                                </tr>
                            </table>--%>
                        </td>
                    </tr>
                </thead>


                <tbody style="border-collapse:collapse;">
                    <tr>
                        <td style="width:1244px">
                            <table style="width:1244px" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTotalData" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:justify;">
                                          &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </tbody>

                <tfoot>
                    <tr>
                        <td style="text-align:justify;">
                              &nbsp;
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <asp:HiddenField ID="hfGridHtml" runat="server" />
        <asp:Button ID="btn_export" runat="server" Text="Export" OnClick="btn_export_Click" />
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <script type="text/javascript">
            $(function () {
                $("[id*=btn_export]").click(function () {
                    $("[id*=hfGridHtml]").val($("#printable").html());
                });
            });
        </script>
    </form>
</body>
</html>
