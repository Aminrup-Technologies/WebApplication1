<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="supplymemo.aspx.cs" Inherits="WebApplication1.bussiness.production.rpts.supplymemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge" />
    <title>Daily Supply Memo</title>
    <link rel="shortcut icon" href="../../../erp_images/ats_translogo.png" />
    <link href="A4.css" rel="stylesheet" />
    <style type="text/css">
        * {
            font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
        }

        .ZeroBorderTable1 {
            border-collapse: collapse;
            border: 1px solid #808080;
            width: 100%;
        }

        .DocHeaderLeft {
            text-align: left;
            padding: 1px;
            border: 1px solid #808080;
            border-right: none;
            border-bottom: none;
            width: 18%;
            font-size: x-small;
        }

        .DocHeaderCenter {
            text-align: center;
            border: 1px solid #808080;
            border-bottom: none;
            width: 62%;
            font-size: x-small;
        }

        .DocHeaderRight {
            text-align: right;
            padding: 1px;
            border: 1px solid #808080;
            border-left: none;
            border-bottom: none;
            width: 20%;
            font-size: x-small;
        }


        .ZeroBorderReportHeading {
            width: 100%;
            height: 71px;
        }

        .ZeroBorderReportHeadingRight {
            vertical-align: top;
            width: 100%;
            height: 60px;
        }

        .DocHeaderReportHeadingCenter {
            width: 100%;
            font-size: small;
            border-bottom: none;
        }

        .DocHeaderReportHeadingRight {
            text-align: right;
            padding: 2px;
            width: 100%;
            font-size: 7pt;
            border-bottom: none;
        }

        .ReportHeading {
            font-size: 15pt;
            text-align: center;
            font-weight: bolder;
        }

        .ReportAddressHeading {
            font-size: 7.2pt;
            text-align: center;
        }

        .BlankRow {
            border: 1px solid #808080;
            line-height: 10px;
        }

        /*tbody styling*/
        .DocIDDataHdrCol1 {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-bottom: none;
            width: 20%;
            font-size: small;
        }

        .DocIDDataHdrCol2 {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-left: none;
            border-bottom: none;
            width: 25%;
            font-size: small;
        }

        .DocIDDataHdrCol3 {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-left: none;
            border-right: none;
            border-bottom: none;
            width: 20%;
            font-size: small;
        }

        .DocIDDataHdrCol3A {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-left: none;
            border-bottom: none;
            width: 20%;
            font-size: small;
        }

        .DocIDDataHdrCol4 {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-bottom: none;
            width: 30%;
            font-size: small;
        }

        .LineBreak {
            line-height: 5px;
            background-color: beige;
            border: 1px solid #595959;
            border-bottom: none;
        }

        .PageHeading {
            font-size: 15pt;
            text-align: center;
            font-weight: bolder;
            border: 1px solid #595959;
            border-bottom: none;
        }

        .SubHeading1 {
            text-align: center;
            color: black;
            border: 1px solid #595959;
            border-top: none;
            font-weight: bold;
            background-color: lightgrey;
            width: 50%;
        }

        .SubHeading2 {
            text-align: center;
            color: black;
            border: 1px solid #595959;
            border-top: none;
            font-weight: bold;
            background-color: lightgrey;
            width: 50%;
        }

        .ManpowerGridCenter {
            text-align: center;
        }

        .ManpowerGridLeft {
            text-align: left;
        }

        .TBTSigCol1 {
            width: 50%;
            border: 1px solid black;
            border-top: none;
            text-align: center;
            padding-left: 2px;
            border-right: none;
        }

        .TBTSigCol2 {
            width: 50%;
            border: 1px solid black;
            border-top: none;
            text-align: center;
            padding-left: 2px;
        }

        @media print {
            .no-print {
                display: none;
            }
        }
    </style>
</head>

<body style="--bleeding: 0.5cm; --margin: 0.3cm;">
    <form id="form1" runat="server" class="page">
        <div class="">
            <table class="ZeroBorderTable1">
                <thead class="ZeroBorder">
                    <tr>
                        <td class="DocHeaderLeft"><span>DOC : #ATS/ACC/SM-01</span></td>
                        <td class="DocHeaderCenter"><span>Vendor code : A538</span></td>
                        <td class="DocHeaderRight"><span>SMJ-ID : </span>
                            <asp:Label ID="lbl_smjid1" runat="server" Text="ATS-BIL/27123/1" Font-Bold="true"></asp:Label>&nbsp;</td>
                    </tr>

                    <tr>
                        <td class="DocHeaderLeft" style="text-align: center;">
                            <span>
                                <img alt="AUTOMATION & TECHNICAL SERVICES" src="../../../erp_images/ats_translogo.png" width="100" height="80" /></span>
                        </td>
                        <td class="DocHeaderCenter">
                            <table class="ZeroBorderReportHeading">
                                <tr>
                                    <td class="DocHeaderReportHeadingCenter"><span class="ReportHeading">AUTOMATION & TECHNICAL SERVICES</span></td>
                                </tr>
                                <tr>
                                    <td class="DocHeaderReportHeadingCenter"><span>MECHANICAL, PIPING FABRICATION & ERECTION</span></td>
                                </tr>
                                <tr>
                                    <td class="ReportAddressHeading"><span>Reg. Office : Near Samudayik Vikas Bhawan, Jemco Basti, Telco, Jamshedpur - 831004.</span></td>
                                </tr>
                            </table>
                        </td>
                        <td class="DocHeaderRight">
                            <table class="ZeroBorderReportHeadingRight">
                                <tr>
                                    <td class="DocHeaderReportHeadingRight"><span>&nbsp;</span></td>
                                </tr>
                                <tr>
                                    <td class="DocHeaderReportHeadingRight"><span>&nbsp;</span></td>
                                </tr>
                                <tr>
                                    <td class="DocHeaderReportHeadingRight"><span>E-mail: accounts@atswork.in</span></td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3" class="BlankRow">&nbsp;</td>
                    </tr>
                </thead>

                <tbody>
                    <tr style="border-collapse: collapse;">
                        <td colspan="4">
                            <table cellspacing="0" cellpadding="0" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <td class="PageHeading" colspan="4"><span>Daily Manpower Supply Memo</span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>
                                    <tr>
                                        <td class="SubHeading1" colspan="2"><span>Supply Memo Details</span></td>
                                        <td class="SubHeading2" colspan="2"><span>Ref. JOB Details</span></td>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>SMJ-ID</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_smjid2" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="Medium" Text="SMJ-ID"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB-ID</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobid" runat="server" ForeColor="Blue" Font-Bold="true" Font-Size="Medium" Text="JOBID"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="DocIDDataHdrCol2"><span>Creation Date</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_smjdate" runat="server" ForeColor="Blue" Font-Bold="true" Text="Creation Date"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB Date</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobdatedetails" runat="server" ForeColor="Blue" Font-Bold="true" Text="Job Date"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>Creator Name</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_smjcreatorname" runat="server" Font-Bold="true" Text="Supv Name"></asp:Label>&nbsp;(<asp:Label ID="lbl_smjcreatorwrk" runat="server" Font-Bold="true" Text="Supv Wrk"></asp:Label>)
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>PO No.</span> :
                                            <asp:Label ID="lbl_pono" runat="server" ForeColor="Blue" Font-Bold="true" Text="PONO"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol4"><span>Permit No</span> :
                                        <asp:Label ID="lbl_permitno" runat="server" ForeColor="Blue" Font-Bold="true" Text="PermitNo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>JOB Description</span></td>
                                        <td colspan="3" class="DocIDDataHdrCol3A">
                                            <span style="padding-left: 2px;">
                                                <asp:Label ID="lbl_jobtitle" runat="server" Text="Job title" Font-Bold="true" ForeColor="Blue"></asp:Label></span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>JOB Department</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_jobdept" runat="server" Font-Bold="true" Text="JOB DEPT"></asp:Label>&nbsp;[<asp:Label ID="lbl_jobrgn" runat="server" Font-Bold="true" Text="JOB REGION"></asp:Label>]
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB Location</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobloc" runat="server" Font-Bold="true" Text="TBT LOC"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>JOB Supervisor</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_jobsupvname" runat="server" Font-Bold="true" Text="Supv Name"></asp:Label>&nbsp;(<asp:Label ID="lbl_jobsupvwrk" runat="server" Font-Bold="true" Text="Supv Wrk"></asp:Label>)
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>ATS Area Incharge</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_siteincharge" runat="server" Font-Bold="true" Text="Site Incharge"></asp:Label>&nbsp;(<asp:Label ID="lbl_inchargewrk" runat="server" Font-Bold="true" Text="incharge_wrk"></asp:Label>)
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>Department Officer</span></td>
                                        <td class="DocIDDataHdrCol2"></td>
                                        <td class="DocIDDataHdrCol3"><span></span></td>
                                        <td class="DocIDDataHdrCol4"><span></span></td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td class="PageHeading" colspan="4"><span>Details of Manpower Supplied</span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="ManpowerGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center" BorderStyle="Solid" BorderColor="Black">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="25%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_employeename" runat="server" Text='<%# Bind("employeename") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridLeft" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_PO_SkillCategory" runat="server" Text='<%# Bind("PO_SkillCategory") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-Width="16%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_PO_EmpDesignation" runat="server" Text='<%# Bind("PO_EmpDesignation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="In-Time" HeaderStyle-Width="18%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Inpunch_Time" runat="server" Text='<%# Bind("Inpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Out-Time" HeaderStyle-Width="18%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Outpunch_Time" runat="server" Text='<%# Bind("Outpunch_Time","{0:dd-MM-yyyy hh:mm tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Safetypass" HeaderStyle-Width="16%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_safetypassno" runat="server" Text='<%# Bind("safetypassno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="3%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ShiftCalc" runat="server" Text='<%# Bind("ShiftCalc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataRowStyle Font-Names="Monaco,monospace;" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                <%--<AlternatingRowStyle BackColor="#DFDFDF" />--%>
                                                <RowStyle Font-Names="Monaco,monospace;" Font-Size="8pt" Font-Bold="true" ForeColor="Black" />
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr id="Unified_LIGrid_0" runat="server" visible="true">
                                        <td class="PageHeading" colspan="4"><span>Supply of Service No.</span></td>
                                    </tr>
                                    <tr id="Unified_LIGrid_2" runat="server" visible="true">
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr id="Unified_LIGrid" runat="server" visible="false">
                                        <td colspan="4">
                                            <asp:GridView ID="LineItems_Grid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center" BorderStyle="Solid" BorderColor="Black">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Service Number" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ServiceNumber" runat="server" Text='<%# Bind("ServiceNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Service Description" HeaderStyle-Width="40%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Service_Description" runat="server" Text='<%# Bind("Service_Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridLeft" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Order Quantity" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Order_Quantity" runat="server" Text='<%# Bind("Order_Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Shift" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Shift_Skill" runat="server" Text='<%# Bind("Shift_Skill") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataRowStyle Font-Names="Monaco,monospace;" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                <%--<AlternatingRowStyle BackColor="#DFDFDF" />--%>
                                                <RowStyle Font-Names="Monaco,monospace;" Font-Size="12pt" Font-Bold="true" ForeColor="Black" />
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="ShiftGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center" BorderStyle="Solid" BorderColor="Black">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Highly-Skilled" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Total_HSShiftCount" runat="server" Text='<%# Bind("Total_HSShiftCount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lbl_Footer_Total_HSShiftCount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                        <FooterStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Skilled" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Total_SShiftCount" runat="server" Text='<%# Bind("Total_SShiftCount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lbl_Footer_Total_SShiftCount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                        <FooterStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Semi-Skilled" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Total_SSShiftCount" runat="server" Text='<%# Bind("Total_SSShiftCount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lbl_Footer_Total_SSShiftCount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                        <FooterStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Un-Skilled" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Total_USShiftCount" runat="server" Text='<%# Bind("Total_USShiftCount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lbl_Footer_Total_USShiftCount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                        <FooterStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="TOTAL" HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Total_ShiftCount" runat="server" Text='<%# Bind("Total_ShiftCount") %>' Font-Bold="true"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lbl_Footer_Total_ShiftCount" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="headergrid" />
                                                        <ItemStyle CssClass="ManpowerGridCenter" />
                                                        <FooterStyle CssClass="ManpowerGridCenter" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataRowStyle Font-Names="Monaco,monospace;" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle BackColor="#cccccc" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                <%--<AlternatingRowStyle BackColor="#DFDFDF" />--%>
                                                <RowStyle Font-Names="Monaco,monospace;" Font-Size="12pt" Font-Bold="true" ForeColor="Black" />
                                            </asp:GridView>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td class="SubHeading1" colspan="2"><span>For ATS Signature</span></td>
                                        <td class="SubHeading2" colspan="2"><span>Signature of Department</span></td>
                                    </tr>
                                    <tr>
                                        <td class="TBTSigCol1" colspan="2" style="line-height: 80px;">&nbsp;</td>
                                        <td class="TBTSigCol2" colspan="2" style="line-height: 80px;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="TBTSigCol1" colspan="2"><span>Date : ___/____/_______</span></td>
                                        <td class="TBTSigCol2" colspan="2"><span>Date : ___/____/_______</span></td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                </tfoot>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <asp:Button ID="btn_back" CssClass="no-print" runat="server" Text="Go Back" OnClick="btn_back_Click"/>
    </form>
    
    <%--<button class="no-print" onclick="history.back()">Back</button>--%>
</body>
</html>
