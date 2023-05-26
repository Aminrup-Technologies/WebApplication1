<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_emp_attencal2.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_emp_attencal2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            padding-top: 10px;
            padding-left: 10px;
            width: 500px;
            height: 300px;
            overflow-y: scroll;
            vertical-align: top;
        }

        .modalPopup1 {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 200px;
            overflow-y: scroll;
            vertical-align: top;
        }
        /* 
    Set the Style for parent CSS Class 
    of Calendar control 
    Parent [CssClass] = myCalendar 
*/
        .myCalendar {
            background-color: #efefef;
            width: 200px;
        }

            /* 
    Common style declaration for hyper linked text 
*/
            .myCalendar a {
                text-decoration: none;
            }

            /* 
    Styles declaration for top title 
    [TitleStyle] [CssClass] = myCalendarTitle 
*/
            .myCalendar .myCalendarTitle {
                font-weight: bold;
            }

            /* 
    Styles declaration for date cells 
    [DayStyle] [CssClass] = myCalendarDay 
*/
            .myCalendar td.myCalendarDay {
                border: solid 2px #fff;
                border-left: 0;
                border-top: 0;
            }

            /* 
    Styles declaration for next/previous month links 
    [NextPrevStyle] [CssClass] = myCalendarNextPrev 
*/
            .myCalendar .myCalendarNextPrev {
                text-align: center;
            }

            /* 
    Styles declaration for Week/Month selector links cells 
    [SelectorStyle] [CssClass] = myCalendarSelector 
*/
            .myCalendar td.myCalendarSelector {
                background-color: #dddddd;
            }

            .myCalendar .myCalendarDay a,
            .myCalendar .myCalendarSelector a,
            .myCalendar .myCalendarNextPrev a {
                display: block;
                line-height: 15px;
            }

        .myCalendarDay noti_Container a {
            display: block;
            line-height: 15px;
        }

        .myCalendar .myCalendarDay a:hover,
        .myCalendar .myCalendarSelector a:hover {
            background-color: #cccccc;
        }

        .myCalendar .myCalendarNextPrev a:hover {
            background-color: #fff;
        }

        #noti_Container {
            position: relative;
            width: 16px;
            height: 16px;
            float: right;
            top: -23px;
        }

        .noti_bubble {
            /*position:absolute;*/
            top: -6px;
            right: -6px;
            padding: 1px 2px 1px 2px;
            background-color: red;
            color: white;
            font-weight: bold;
            font-size: 0.55em;
            border-radius: 30px;
            box-shadow: 1px 1px 1px gray;
        }

        .button-link {
            padding: 2px 5px;
            background: #4479BA;
            color: #FFF;
            -webkit-border-radius: 4px;
            -moz-border-radius: 4px;
            border-radius: 4px;
            border: solid 1px #20538D;
            text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.4);
            -webkit-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
            -moz-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
            box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
            -webkit-transition-duration: 0.2s;
            -moz-transition-duration: 0.2s;
            transition-duration: 0.2s;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            .button-link:hover {
                background: #356094;
                border: solid 1px #2A4E77;
                text-decoration: none;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>View : Monthly Attendance Calender</h3>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <div class="col-lg-12">
                                            <asp:Calendar ID="Calendar1"
                                                runat="server"
                                                DayNameFormat="FirstLetter"
                                                Font-Names="Arial"
                                                Font-Size="11px"
                                                NextMonthText="»"
                                                PrevMonthText="«"
                                                SelectMonthText="»"
                                                SelectWeekText="›"
                                                CssClass="myCalendar"
                                                BorderStyle="None" Width="350" Height="300"
                                                CellPadding="1" OnSelectionChanged="Calendar1_SelectionChanged" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged" OnDayRender="Calendar1_DayRender">
                                                <OtherMonthDayStyle ForeColor="Gray" />
                                                <DayStyle CssClass="myCalendarDay" />
                                                <SelectedDayStyle Font-Bold="True" Font-Size="12px" />
                                                <SelectorStyle CssClass="myCalendarSelector" />
                                                <NextPrevStyle CssClass="myCalendarNextPrev" />
                                                <TitleStyle CssClass="myCalendarTitle" />
                                            </asp:Calendar>

                                            <asp:LinkButton ID="lnkButton" runat="server" CssClass="hide"
                                                OnClick="lnkButton_Click"></asp:LinkButton>
                                            <asp:ModalPopupExtender ID="Calendar1_ModalPopupExtender" BackgroundCssClass="modalBackground" PopupControlID="Panel1" runat="server" DynamicServicePath="" Enabled="True" TargetControlID="hdnlink"></asp:ModalPopupExtender>
                                            <asp:LinkButton ID="hdnlink" Style="display: none;" runat="server" OnClick="hdnlink_Click">Submit</asp:LinkButton>
                                            <br />
                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" BackgroundCssClass="modalBackground" PopupControlID="Panel2" runat="server" DynamicServicePath="" Enabled="True" TargetControlID="hdnlnk2"></asp:ModalPopupExtender>
                                            <asp:LinkButton ID="hdnlnk2" Style="display: none;" runat="server" OnClick="hdnlnk2_Click">Submit</asp:LinkButton>
                                            <br />

                                            <asp:ModalPopupExtender ID="ModalPopupExtender2" BackgroundCssClass="modalBackground" PopupControlID="Panel3" runat="server" DynamicServicePath="" Enabled="True" TargetControlID="hdnlnk3"></asp:ModalPopupExtender>
                                            <asp:LinkButton ID="hdnlnk3" Style="display: none;" runat="server" OnClick="hdnlnk3_Click">Submit</asp:LinkButton>
                                            <br />

                                            <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                                                <table style="width: 95%;">
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" Font-Size="Small" Style="float: right;">Close</asp:LinkButton></td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="GridView2" runat="server" Width="95%" CellPadding="4" AutoGenerateColumns="false" ForeColor="#333333" GridLines="None" OnRowCommand="GridView2_RowCommand">

                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Category" ShowHeader="true">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCategory" runat="server" CausesValidation="False" Text='<%# Eval("Category") %>' CommandName="Category" CommandArgument='<%#Eval("Category") + ";" +Eval("Date")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                                                <table style="width: 95%;">
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="Small" Style="float: right;">Close</asp:LinkButton></td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="GridView1" runat="server" Width="95%" Height="250" ShowHeader="false" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" GridLines="None">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                    <Columns>
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <div style="color: White; background-color: #5D7B9D; font-weight: bold; padding: 5px;">
                                                                    <%# Eval("Event Name") %>
                                                                    <div style="float: right;">
                                                                        <%# Eval("Date") %>
                                                                    </div>
                                                                </div>
                                                                <div>
                                                                    <%# Eval("Description") %>
                                                                </div>
                                                                <div>
                                                                    <%# Eval("Category") %>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup1" align="center" Style="display: none">
                                                <table>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkCancel" runat="server" Font-Size="Small" Style="float: right;">Close</asp:LinkButton></td>
                                                    </tr>

                                                    <tr>
                                                        <td>Event Name  
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEventName" runat="server"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>Location  
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>Category  
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCategory" runat="server">

                                                                <asp:ListItem Text="Concerts" Value="Concerts"></asp:ListItem>
                                                                <asp:ListItem Text="Festivals" Value="Festivals"></asp:ListItem>
                                                                <asp:ListItem Text="Kids & Family" Value="Kids & Family"></asp:ListItem>
                                                                <asp:ListItem Text="Performing Arts" Value="Performing Arts"></asp:ListItem>
                                                                <asp:ListItem Text="Food" Value="Food"></asp:ListItem>
                                                                <asp:ListItem Text="Sports" Value="Sports"></asp:ListItem>
                                                                <asp:ListItem Text="Conferences" Value="Conferences"></asp:ListItem>
                                                                <asp:ListItem Text="Movies" Value="Movies"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>Description  
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkSubmit" runat="server" CssClass="button-link" OnClick="lnkSubmit_Click">Submit</asp:LinkButton><br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
