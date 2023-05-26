<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="vw_emp_attencal.aspx.cs" Inherits="WebApplication1.bussiness.production.vw_emp_attencal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Site.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title" id="toprow" runat="server" visible="false">
                <div class="title_left">
                    <h3>View Monthly Attendance</h3>
                </div>
                <div class="title_right">
                    <div class="col-md-5 col-sm-5 form-group pull-right top_search">
                    </div>
                </div>
            </div>

            <div class="clearfix"></div>

            <div class="row" id="hidden_Div" runat="server" visible="false">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <div class="col-lg-12">
                                            <asp:Calendar ID="Calendar1" runat="server" ShowGridLines="True" Width="650px" OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="#FFFFCC" BorderColor="#FFCC66" BorderWidth="3px" Font-Names="Verdana" Font-Size="10pt" ForeColor="#663399" Height="300px">
                                                <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <NextPrevStyle Font-Size="19pt" ForeColor="#FFFFCC" />
                                                <OtherMonthDayStyle ForeColor="#CC9966" />
                                                <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                                <SelectorStyle BackColor="#FFCC66" />
                                                <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="19pt" ForeColor="#FFFFCC" />
                                                <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                            </asp:Calendar>
                                            <asp:GridView ID="GridView1" runat="server" Caption="Selected Event Details" Width="700px" AutoGenerateColumns="false" CellPadding="5" ForeColor="#333333" GridLines="Both">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Id" DataField="Id" />
                                                    <asp:BoundField HeaderText="First Name" DataField="CreatedDate" />
                                                    <asp:BoundField HeaderText="Last Name" DataField="EmployeeName" />
                                                    <asp:BoundField HeaderText="Date of Birth" DataField="ProvidedOT" />
                                                    <asp:BoundField HeaderText="Mobile No" DataField="WorkedHours" />
                                                </Columns>
                                                <HeaderStyle BackColor="#990000" Font-Bold="true" ForeColor="White" />
                                                <RowStyle BackColor="#FFFBd6" ForeColor="#333333" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Click on Calender dates to View Details</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                                <%--<li><a class="close-link"><i class="fa fa-close"></i></a></li>--%>
                            </ul>
                            <div class="clearfix"></div>
                        </div>

                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="calendarOuter">
                                                    <div class="calendarWrapper">
                                                        <asp:Calendar ID="Calendar2" runat="server" CssClass="myCalendar" DayNameFormat="Short" Font-Names="Tahoma"
                                                            OnDayRender="Calendar2_DayRender" OnVisibleMonthChanged="Calendar2_VisibleMonthChanged"
                                                            CellSpacing="0" CellPadding="0" title="Calendar"
                                                            Style="border-width: 1px; border-style: solid; font-family: Tahoma; border-collapse: collapse;">
                                                            <OtherMonthDayStyle CssClass="calDay otherMonthDay" />
                                                            <DayStyle CssClass="calDay" />
                                                            <DayHeaderStyle CssClass="calDayHeader" ForeColor="#2d3338" />
                                                            <SelectedDayStyle Font-Bold="True" CssClass="calDaySelected" />
                                                            <TodayDayStyle CssClass="calToday" />
                                                            <SelectorStyle CssClass="calSelector" />
                                                            <NextPrevStyle CssClass="calNextPrev" />
                                                            <TitleStyle CssClass="calTitle" />
                                                        </asp:Calendar>
                                                    </div>
                                                </div>

                                                <hr />
                                                <div class="detailsOuter">
                                                    <asp:Label ID="lblSelectedDate" runat="server" CssClass="SelectedDate">No date selected</asp:Label>
                                                    <table id="tblDetails" class="details">
                                                        <tr>
                                                            <td colspan="2" class="empty"></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="clear: left;"></div>
                                                <div id="divHidden">
                                                    <asp:HiddenField ID="hdnDay0" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay1" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay2" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay3" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay4" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay5" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay6" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay7" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay8" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay9" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay10" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay11" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay12" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay13" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay14" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay15" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay16" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay17" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay18" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay19" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay20" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay21" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay22" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay23" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay24" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay25" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay26" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay27" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay28" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay29" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay30" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay31" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay32" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay33" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay34" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay35" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay36" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay37" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay38" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay39" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay40" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnDay41" runat="server" Value="" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            var anchor = $("td.calToday").find("a").eq(0);
            $(anchor).click();
        });

        function MouseOver(cell) {
            $(cell).addClass("hovered");
        }
        function MouseOut(cell) {
            $(cell).removeClass("hovered");
        }

        // Called when user clicks (taps) a day on the calendar.  Index is 0-41 (6 rows of 7 days).
        function Showday(dayLink, index) {
            // code-behind replaces postback with javascript call, so we have to set "selected" class manually
            $('.myCalendar td').removeClass('calDaySelected');     // remove "selected" from all 42 days....
            $(dayLink).parent().addClass('calDaySelected');        // then set "selected" on the clicked day

            // Get the anchor's title attribute, show it to the user in the "details" div
            $('#<%= lblSelectedDate.ClientID %>').text($(dayLink).attr('title'));

            // find the hidden field with activity descriptions -- nth-child uses 1-based index, but the "index" parameter is 0-based
            var inputText = $('#divHidden').find('input:hidden:nth-child(' + (index + 1) + ')').val();
            if (inputText == '') {
                var html = '<tr><td colspan="2" class="empty">No activities found</td></tr>';
            }
            else {
                //var activities = inputText.split('|');  // each activity is separated with '|'
                //var html = '';
                //for (var i = 0; i < activities.length; i++) {
                //    var fields = activities[i].split('~');
                //    var timeStr = fields[0];
                //    var nameStr = fields[1];
                //    var descStr = fields[2];
                //    html += '<tr><td class=\"time\">' + timeStr + '</td><td class=\"desc\"><strong>' + nameStr + '</strong>: ' + descStr + '</td></tr>';
                //}

                var activities = inputText.split('|');  // each activity is separated with '|'
                var html = '';
                for (var i = 0; i < activities.length; i++) {
                    var fields = activities[i].split('~');
                    var a1 = fields[0];
                    var a2 = fields[1];
                    var a3 = fields[2];
                    var a4 = fields[3];
                    var a5 = fields[4];
                    var a6 = fields[5];
                    var a7 = fields[6];
                    var a8 = fields[7];
                    var a9 = fields[8];
                    var a10 = fields[9];
                    var a11 = fields[10];
                    var a12 = fields[11];
                    var a13 = fields[12];
                    html += '<tr><td class=\"time\">JOB Date :</td><td class=\"desc\">' + a1 + '</td></tr>';
                    html += '<tr><td class=\"time\">JOB No. :</td><td class=\"desc\">' + a2 + '</td></tr>';
                    html += '<tr><td class=\"time\">Supervisor :</td><td class=\"desc\">' + a3 + '</td></tr>';
                    html += '<tr><td class=\"time\">PO & Permit :</td><td class=\"desc\">' + a4 + '</td></tr>';
                    html += '<tr><td class=\"time\">Dept. & Location :</td><td class=\"desc\">' + a5 + '</td></tr>';
                    html += '<tr><td class=\"time\">ATS In-charge :</td><td class=\"desc\">' + a6 + '</td></tr>';
                    html += '<tr><td class=\"time\">Clock IN :</td><td class=\"desc\">' + a7 + '</td></tr>';
                    html += '<tr><td class=\"time\">Clock OUT :</td><td class=\"desc\">' + a8 + '</td></tr>';
                    html += '<tr><td class=\"time\">Clock Hours :</td><td class=\"desc\">' + a9 + '</td></tr>';
                    html += '<tr><td class=\"time\">Lunch :</td><td class=\"desc\">' + a10 + '</td></tr>';
                    html += '<tr><td class=\"time\">Over Time :</td><td class=\"desc\">' + a11 + '</td></tr>';
                    html += '<tr><td class=\"time\">Work Status :</td><td class=\"desc\">' + a12 + '</td></tr>';
                    html += '<tr><td class=\"time\">Status Code :</td><td class=\"desc\">' + a13 + '</td></tr>';

                }
            }
            $('#tblDetails tbody').empty();
            $('#tblDetails tbody').append(html);
        }
    </script>
</asp:Content>
