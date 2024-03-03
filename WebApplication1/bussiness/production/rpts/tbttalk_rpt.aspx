<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tbttalk_rpt.aspx.cs" Inherits="WebApplication1.bussiness.production.rpts.tbttalk_rpt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TBT Report</title>
    <link rel="shortcut icon" href="../../../erp_images/ats_translogo.png" />
    <style type="text/css">
        body {
            font-family: 'Century Gothic';
            font-size: small;
        }

        .Print {
            width: 844px;
        }

        .ZeroBorder {
            border-collapse: collapse;
            border: 1px solid #595959;
        }

        .DocHeaderLeft {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-right: none;
            border-bottom: none;
            width: 25%;
            font-size: small;
        }

        .DocHeaderCenter {
            text-align: center;
            padding: 2px;
            border: 1px solid #595959;
            border-bottom: none;
            width: 50%;
            font-size: small;
        }

        .DocHeaderRight {
            text-align: right;
            padding: 2px;
            border: 1px solid #595959;
            border-left: none;
            border-bottom: none;
            width: 25%;
            font-size: small;
        }

        .ReportHeading {
            font-size: x-large;
            text-align: right;
            font-weight: bolder;
        }

        .ZeroBorderReportHeading {
            border-collapse: collapse;
        }

        .DocHeaderReportHeadingLeft {
            text-align: left;
            padding: 2px;
            border-right: none;
            border-bottom: none;
            width: 25%;
            font-size: small;
        }

        .DocHeaderReportHeadingCenter {
            text-align: center;
            padding: 2px;
            width: 50%;
            font-size: small;
            border-bottom: none;
        }

        .DocHeaderReportHeadingRight {
            text-align: right;
            padding: 2px;
            border-left: none;
            border-bottom: none;
            width: 25%;
            font-size: small;
        }

        .DocSubHeader {
            column-span: all;
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-bottom: none;
            font-size: small;
        }

        .DocIDData {
            border-collapse: collapse;
            border-spacing: 0px;
            padding: 0px;
        }

        .DocIDDataHdrCol1 {
            text-align: left;
            padding: 2px;
            border: 1px solid #595959;
            border-left: none;
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
            width: 30%;
            font-size: small;
        }

        .DocIDDataHdrCol3 {
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
            border-right: none;
            border-bottom: none;
            width: 30%;
            font-size: small;
        }

        .SubHeading1 {
            text-align: center;
            color: white;
            border-top: none;
            font-weight: bolder;
            background-color: #b731dd;
            border-right-color: white;
        }

        .SubHeading2 {
            text-align: center;
            color: white;
            border-top: none;
            font-weight: bolder;
            background-color: #b731dd;
        }

        .LineBreak {
            line-height: 5px;
            background-color: beige;
            border: 1px solid black;
            border-left: none;
            border-right: none;
        }

        .LeftSubHeading1 {
            text-align: left;
            color: white;
            font-weight: bolder;
            background-color: #b731dd;
            border-right-color: white;
            border-bottom: none;
        }

        .TBTInnerTbl td {
            border-collapse: collapse;
            font-size: small;
        }

        .TBTInnerTbl_Col1 {
            width: 4%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
        }

        .TBTInnerTbl_Col2 {
            width: 90%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: left;
            padding-left: 2px;
        }

        .TBTInnerTbl_Col2Center {
            width: 90%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
        }

        .TBTInnerTbl_Col3 {
            width: 3%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
        }

        .TBTInnerTbl_Col4 {
            width: 3%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
        }

        .TBTInnerTblHead {
            text-align: center;
            color: white;
            font-weight: bolder;
            background-color: #b731dd;
            border-right-color: white;
        }

        .TBTAttenCol1 {
            width: 30%;
            border: 1px solid black;
            border-bottom: none;
            text-align: center;
            padding-left: 2px;
        }

        .TBTAttenCol2 {
            width: 70%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
            padding-left: 2px;
        }

        .TBTSigCol1 {
            width: 50%;
            border: 1px solid black;
            border-bottom: none;
            text-align: center;
            padding-left: 2px;
        }

        .TBTSigCol2 {
            width: 50%;
            border: 1px solid black;
            border-right: none;
            border-bottom: none;
            text-align: center;
            padding-left: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="printablebox" class="Print">
            <table class="Print ZeroBorder">
                <thead class="Print ZeroBorder">
                    <tr>
                        <td class="DocHeaderLeft"><span>DOC #ATS/CSM/TBT-01</span></td>
                        <td class="DocHeaderCenter"><span>Rev #03</span></td>
                        <td class="DocHeaderRight">EFFT DATE : 19/12/2018</td>
                    </tr>

                    <tr>
                        <td class="DocHeaderCenter" style="width: 25%;">
                            <span>
                                <img alt="TATA STEEL BSL" src="../../../erp_images/tsllogo.png" width="130" height="80" /></span>
                        </td>
                        <td class="DocHeaderCenter" style="width: 50%;">
                            <table class="ZeroBorderReportHeading">
                                <tr>
                                    <td class="DocHeaderReportHeadingCenter"><span class="ReportHeading">TOOL BOX TALK REPORT</span></td>
                                </tr>
                                <tr>
                                    <td class="DocHeaderReportHeadingCenter"><span>SAFETY STANDARD-CONTRACTOR SAFETY MANAGEMENT</span></td>
                                </tr>
                                <tr>
                                    <td class="DocHeaderReportHeadingCenter"><span>SS/GEN-54, VERSION-01</span></td>
                                </tr>
                            </table>
                        </td>
                        <td class="DocHeaderCenter" style="width: 25%;">
                            <span>
                                <img alt="AUTOMATION 7 TECHNICAL SERVICES" src="../../../erp_images/ats_translogo.png" width="100" height="80" /></span>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3" class="DocSubHeader"><span>Annexure - 11 : Procedure and format for Tool Box Meeting</span></td>
                    </tr>

                    <tr>
                        <td colspan="3"></td>
                    </tr>
                </thead>

                <tbody>
                    <tr>
                        <td colspan="3" class="DocSubHeader DocIDData">
                            <table class="Print DocIDData">
                                <thead class="Print DocIDData">
                                    <tr>
                                        <td class="DocIDDataHdrCol1 SubHeading1" colspan="2"><span>Tool Box Talk Data</span></td>
                                        <td class="DocIDDataHdrCol4 SubHeading2" colspan="2"><span>Ref. JOB Details</span></td>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>TBT ID</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtid" runat="server" ForeColor="Brown" Font-Bold="true" Text="TBTID"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>Ref. JOB ID</span> :
                                            <asp:Label ID="lbl_jobid" runat="server" ForeColor="Blue" Font-Bold="true" Text="JOBID"></asp:Label></td>
                                        <td class="DocIDDataHdrCol4"><span>Permit No</span> :
                                            <asp:Label ID="lbl_permitno" runat="server" ForeColor="Blue" Font-Bold="true" Text="JOBID"></asp:Label></td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>TBT Meeting Date</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtdate" runat="server" ForeColor="Brown" Font-Bold="true" Text="TBT DATE"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3">
                                            <span>JOB Date & Shift</span>
                                        </td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobdate" runat="server" ForeColor="Blue" Font-Bold="true" Text="JOB DATE"></asp:Label>
                                            [<asp:Label ID="lbl_jobshift" runat="server" Text="JOB SHIFT"></asp:Label>]
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>TBT Region & Department</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtrgn" runat="server" Font-Bold="true" Text="TBT REGION"></asp:Label>&nbsp;-&nbsp;<asp:Label ID="lbl_tbtdept" runat="server" Font-Bold="true" Text="TBT DEPT"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB Region & Department</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobrgn" runat="server" Font-Bold="true" Text="JOB REGION"></asp:Label>&nbsp;-&nbsp;<asp:Label ID="lbl_jobdept" runat="server" Font-Bold="true" Text="JOB DEPT"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>TBT Meeting Location</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtloc" runat="server" Font-Bold="true" Text="TBT LOC"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB WorkSite</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobworksite" runat="server" Font-Bold="true" Text="JOB Worksite"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>TBT Trainer Name</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtsupvname" runat="server" Font-Bold="true" Text="TBT SUPV"></asp:Label>&nbsp;(&nbsp;<asp:Label ID="lbl_tbtsupvwrk" runat="server" Font-Bold="true" Text="TBT Supv Wrk"></asp:Label>)
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>JOB Supervisor Name</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobsupvname" runat="server" Font-Bold="true" Text="Supv Name"></asp:Label>&nbsp;(&nbsp;<asp:Label ID="lbl_jobsupvwrk" runat="server" Font-Bold="true" Text="Supv Wrk"></asp:Label>)
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>Line Manager</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_linemngr" runat="server" Font-Bold="true" Text="Line Manager" ForeColor="DarkBlue"></asp:Label>
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>Site In-Charge (ATS)</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_siteincharge" runat="server" Font-Bold="true" Text="Site Incharge"></asp:Label>&nbsp;(&nbsp;<asp:Label ID="lbl_inchargewrk" runat="server" Font-Bold="true" Text="incharge_wrk"></asp:Label>)
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>Safety Supervisor Name</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtsfysupvname" runat="server" Font-Bold="true" ForeColor="ForestGreen" Text="Safety Supv"></asp:Label>&nbsp;(&nbsp;<asp:Label ID="lbl_tbtsfysupvwrk" runat="server" Font-Bold="true" Text="sftysupv_wrk"></asp:Label>)
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>Approval Status</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_sftysupvapp" runat="server" Text="Pending" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>Safety Officer Name</span></td>
                                        <td class="DocIDDataHdrCol2">
                                            <asp:Label ID="lbl_tbtsfyofname" runat="server" Font-Bold="true" Text="Safety Officer" ForeColor="Green"></asp:Label>&nbsp;(&nbsp;<asp:Label ID="lbl_tbtsfyofcwrk" runat="server" Font-Bold="true" Text="sftysupv_wrk"></asp:Label>)
                                        </td>
                                        <td class="DocIDDataHdrCol3"><span>Approval Status</span></td>
                                        <td class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_sftyofcapp" runat="server" ForeColor="Red" Font-Bold="true" Text="Pending"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                                    </tr>
                                    <tr>
                                        <td class="DocIDDataHdrCol1"><span>JOB Title</span></td>
                                        <td colspan="3" class="DocIDDataHdrCol4">
                                            <asp:Label ID="lbl_jobtitle" runat="server" Text="Job title" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                        </td>
                                    </tr>

                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br style="border: none;" />

            <table class="Print ZeroBorder">
                <thead class="Print">
                    <tr>
                        <td class="TBTAttenCol1 LeftSubHeading1">
                            <span>Tool Box Talk Photograph</span>
                        </td>
                        <td class="TBTAttenCol2 LeftSubHeading1">
                            <span>Tool Box Talk Members</span>
                        </td>
                    </tr>
                </thead>
            </table>

            <table class="Print ZeroBorder">
                <%-- <thead class="TBTInnerTblHead">
                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>Sl.</span></td>
                        <td class="TBTInnerTbl_Col2Center"><span>ITEMS</span></td>
                        <td class="TBTInnerTbl_Col3"><span>Yes</span></td>
                        <td class="TBTInnerTbl_Col4"><span>No</span></td>
                    </tr>
                </thead>--%>
                <tbody>
                    <tr>
                        <td class="TBTAttenCol1"><span>
                            <asp:Image ID="TBT_Img" runat="server" Width="250" Height="250" ImageAlign="Middle" /></span></td>
                        <td class="TBTAttenCol2" colspan="3">
                            <asp:GridView ID="TBTMemebersGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name of TBT Members" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="DocHeaderLeft" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID No" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmployeeWrk" runat="server" Text='<%# Bind("EmployeeWrk") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Designation" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EmpDesignation" runat="server" Text='<%# Bind("EmpDesignation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gatepass No" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_GatePassNo" runat="server" Text='<%# Bind("GatePassNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle Font-Names="Century Gothic" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle BackColor="#ccccff" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <AlternatingRowStyle BackColor="#DFDFDF" />
                                <RowStyle Font-Names="Century Gothic" Font-Size="10pt" />
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>

            <br style="border: none;" />

            <table class="Print ZeroBorder">
                <thead class="Print">
                    <tr>
                        <td colspan="4" class="LeftSubHeading1">
                            <span>ITEMS DISCUSSED : ( Indicate if not discussed)</span>
                        </td>
                    </tr>
                </thead>
            </table>

            <br style="border: none;" />

            <table class="Print ZeroBorder TBTInnerTbl">
                <thead class="TBTInnerTblHead">
                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>Sl.</span></td>
                        <td class="TBTInnerTbl_Col2Center"><span>ITEMS</span></td>
                        <td class="TBTInnerTbl_Col3"><span>Yes</span></td>
                        <td class="TBTInnerTbl_Col4"><span>No</span></td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>1.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Safety contact and review of action items from last meeting</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2Center" colspan="3">
                            <asp:GridView ID="PrevActionableGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Reported By" HeaderStyle-Width="12%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Employee_Name" runat="server" Text='<%# Bind("Employee_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Feedbck_typ" runat="server" Text='<%# Bind("Feedbck_typ") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Reported Items" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Feedbck_dsp" runat="server" Text='<%# Bind("Feedbck_dsp") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textleft WordWrap" />
                                    </asp:TemplateField>

                                    <asp:ImageField DataImageUrlField="BeforePhoto_Path" ControlStyle-Width="110" ControlStyle-Height="110" HeaderText="Photograph">
                                        <ControlStyle Height="110px" Width="110px"></ControlStyle>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:ImageField>

                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Status" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Closing Remarks" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_clsng_rmrks" runat="server" Text='<%# Bind("clsng_rmrks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textleft WordWrap" />
                                    </asp:TemplateField>

                                    <asp:ImageField DataImageUrlField="AfterPhoto_Path" ControlStyle-Width="110" ControlStyle-Height="110" HeaderText="Photograph">
                                        <ControlStyle Height="110px" Width="110px"></ControlStyle>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:ImageField>
                                </Columns>
                                <EmptyDataRowStyle Font-Names="Century Gothic" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle BackColor="#ccccff" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <AlternatingRowStyle BackColor="#DFDFDF" />
                                <RowStyle Font-Names="Century Gothic" Font-Size="10pt" />
                            </asp:GridView>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>2.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Items of General Safety Importance to the Total Work Site : (Ask employee to mention any incident / Nearmiss during the past day which may have or have resulted into damage to property or injury to Compay or Contractor Personnel)</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3">
                            <asp:GridView ID="PastIncidentGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Incident Reporting in Last Two Days...!!" Width="100%" HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="12%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Incident_Id" runat="server" Text='<%# Bind("Incident_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Incident_Type" runat="server" Text='<%# Bind("Incident_Type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Date" runat="server" Text='<%# Bind("Date", "{0:dd-MM-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Detailed Description" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Incident_Descp" runat="server" Text='<%# Bind("Incident_Descp") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textleft WordWrap" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Shared By" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Submitted_by" runat="server" Text='<%# Bind("Submitted_by") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataRowStyle Font-Names="Century Gothic" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle BackColor="#ccccff" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Height="20px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <AlternatingRowStyle BackColor="#DFDFDF" />
                                <RowStyle Font-Names="Century Gothic" Font-Size="10pt" />
                            </asp:GridView>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>3.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Items of Safety Interest to this Group : (e.g. Red Stripes, Orange Stripes, Green Stripes, Safety Alerts of Safety Communication, Hazards or Safety Conditions Applicable to this group's works areas)</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image6" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3"><span>
                            <asp:Label ID="lbl_sftyintrst" runat="server" Text="N/A" Font-Bold="true"></asp:Label></span></td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>4.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Standard Operation Procedures (SOP) relevant to this Group</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image7" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image8" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3"><span>
                            <asp:Label ID="lbl_tbtsopno" runat="server" Text="SOP NO" Font-Bold="true"></asp:Label></span></td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>5.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Reminders to Employees of their Personal Responsibilities to Ensure and Maintain:</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image9" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image10" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3">
                            <span>
                                <asp:CheckBoxList ID="chkbxrspons" runat="server" RepeatColumns="4" Font-Size="Small" CellPadding="2" CellSpacing="2" Width="100%"></asp:CheckBoxList>
                            </span>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>6.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Hazardous materilas relevant to this Group Work's Area</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image11" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image12" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3"><span>
                            <asp:Label ID="lbl_hazards" runat="server" Text="N/A" Font-Bold="True"></asp:Label></span></td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>7.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Safety Message Handouts / Circular to be Shared with Contract Employees</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image13" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image14" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3"><span>
                            <asp:Label ID="lbl_sftmsg" runat="server" Text="N/A" Font-Bold="True"></asp:Label></span></td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>8.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Safety Alert Tips relevant to this Work's Area</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image15" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image16" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3"><span>
                            <asp:Label ID="lbl_sftyalert" runat="server" Text="N/A" Font-Bold="True"></asp:Label></span></td>
                    </tr>

                    <tr>
                        <td colspan="4" class="LineBreak"><span>&nbsp;</span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span>9.</span></td>
                        <td class="TBTInnerTbl_Col2"><span>Actions resulting from this meeting and points raised by Contract Employees and Supervisors</span></td>
                        <td class="TBTInnerTbl_Col3"><span>
                            <asp:Image ID="Image17" runat="server" ImageUrl="~/erp_images/right.png" Visible="false" /></span></td>
                        <td class="TBTInnerTbl_Col4"><span>
                            <asp:Image ID="Image18" runat="server" ImageUrl="~/erp_images/right.png" Visible="true" /></span></td>
                    </tr>

                    <tr>
                        <td class="TBTInnerTbl_Col1"><span></span></td>
                        <td class="TBTInnerTbl_Col2" colspan="3">
                            <asp:GridView ID="NewActionableGrid" runat="server" BorderWidth="0" AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" ShowHeaderWhenEmpty="True" EmptyDataText="No Entries Found...!!" Width="100%" HorizontalAlign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl" Visible="True" HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_slno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Report By" HeaderStyle-Width="12%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Employee_Name" runat="server" Text='<%# Bind("Employee_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Feedbck_typ" runat="server" Text='<%# Bind("Feedbck_typ") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Reported Items" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Feedbck_dsp" runat="server" Text='<%# Bind("Feedbck_dsp") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textleft WordWrap" />
                                    </asp:TemplateField>

                                    <asp:ImageField DataImageUrlField="BeforePhoto_Path" ControlStyle-Width="110" ControlStyle-Height="110" HeaderText="Photograph">
                                        <ControlStyle Height="110px" Width="110px"></ControlStyle>
                                        <HeaderStyle CssClass="headergrid" />
                                        <ItemStyle CssClass="grid-textcenter" />
                                    </asp:ImageField>
                                </Columns>
                                <EmptyDataRowStyle Font-Names="Century Gothic" Font-Size="Small" Height="15px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle BackColor="#ccccff" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Height="18px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                <AlternatingRowStyle BackColor="#DFDFDF" />
                                <RowStyle Font-Names="Century Gothic" Font-Size="10pt" />
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>

            <br style="border: none;" />


            <table class="Print ZeroBorder">
                <thead class="TBTInnerTblHead">
                    <tr>
                        <th colspan="2" class="LeftSubHeading1">
                            <span>Signature by Representative(s)</span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr style="line-height: 50px;">
                        <td class="TBTSigCol1" rowspan="2"><span>Signature of Contractor / Line Manager</span></td>
                        <td class="TBTSigCol2"><span>&nbsp;</span></td>
                    </tr>
                    <tr>
                        <%--<td class="TBTSigCol1"><span></span></td>--%>
                        <td class="TBTSigCol2"><span>Date : ___ / ____ / _______</span></td>
                    </tr>
                </tbody>
            </table>

            <br style="border: none;" />

            <table class="Print ZeroBorder">
                <thead class="TBTInnerTblHead">
                    <tr></tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="TBTSigCol1">
                            <table border="0">
                                <thead class="TBTInnerTblHead">
                                    <tr>
                                        <th colspan="5">Safety Supervisor Approval</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td rowspan="5">
                                            <asp:Image ID="app1" runat="server" ImageUrl="~/erp_images/SingleTick.gif" Width="100px" Height="80px" Visible="false" />
                                            <asp:Image ID="pen1" runat="server" ImageUrl="~/erp_images/pending.png" Width="100px" Height="100px" Visible="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label15" runat="server" Text="Safety Sup. :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftysupvname" runat="server" Text="" Font-Bold="true" ForeColor="blue"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label17" runat="server" Text="Status :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftysupvapp1" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label19" runat="server" Text="Approval Date :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftysupvdt" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label21" runat="server" Text="Remarks :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftysupvrmrks" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td class="TBTSigCol2">
                            <table>
                                <thead class="TBTInnerTblHead">
                                    <tr>
                                        <th colspan="5">Safety Officer Approval</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td rowspan="5">
                                            <asp:Image ID="app2" runat="server" ImageUrl="~/erp_images/SingleTick.gif" Width="100px" Height="80px" Visible="false" />
                                            <asp:Image ID="pen2" runat="server" ImageUrl="~/erp_images/pending.png" Width="100px" Height="100px" Visible="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label23" runat="server" Text="Safety Officer :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftyofcrname" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label25" runat="server" Text="Status :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftyofcrapp" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label27" runat="server" Text="Approval Date :"></asp:Label></td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftyofcrdt" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%; text-align: left; padding-left: 5px;">
                                            <asp:Label ID="Label29" runat="server" Text="Remarks :"></asp:Label>
                                        </td>
                                        <td style="width: 60%;">
                                            <asp:Label ID="lbl_sftyofcrrmrks" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>
    </form>
</body>
</html>
