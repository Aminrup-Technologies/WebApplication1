<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="helpdesk_ticketdetails.aspx.cs" Inherits="WebApplication1.bussiness.production.helpdesk_ticketdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tdc-container {
            background: #fff;
            border: 1px solid #ddd;
            border-radius: 12px;
            padding: 30px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            font-size: 15px;
            margin-bottom: 30px;
            max-width: 1600px;
        }

        .tdc-section-title {
            font-size: 30px;
            font-weight: 600;
            color: #333;
            margin-bottom: 20px;
            border-bottom: 2px solid #ccc;
            padding-bottom: 10px;
            text-align: center;
        }

        .tdc-grid-row {
            display: flex;
            flex-wrap: wrap;
            border-top: 1px solid #ddd;
            border-left: 1px solid #ddd;
        }

        .tdc-grid-col {
            flex: 1 1 25%;
            min-width: 220px;
            padding: 15px;
            border-right: 1px solid #ddd;
            border-bottom: 1px solid #ddd;
            box-sizing: border-box;
        }

        .tdc-info-label {
            font-weight: 500;
            color: #6c757d;
        }

        .tdc-info-value {
            font-weight: 600;
            color: #2d3436;
            display: inline-block;
            margin-top: 5px;
        }

        .tdc-badge-status {
            display: inline-block;
            padding: 5px 10px;
            border-radius: 6px;
            font-size: 14px;
            font-weight: 500;
            background-color: #17a2b8;
            color: #fff;
            margin-top: 5px;
        }

        .tdc-description {
            background: #f8f9fa;
            border: 1px solid #ccc;
            border-radius: 8px;
            padding: 15px;
            font-style: italic;
            color: #555;
            min-height: 100px;
        }

        .tdc-text-right {
            text-align: right;
        }

        @media (max-width: 991px) {
            .tdc-grid-col {
                flex: 1 1 50%;
            }
        }

        @media (max-width: 600px) {
            .tdc-grid-col {
                flex: 1 1 100%;
            }
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
    <div class="right_col" role="main" style="padding-right: 0;">
        <div class="container mt-4" style="padding-right: 0;">

            <!-- Main Ticket Details -->
            <div class="tdc-container">
                <div class="tdc-section-title">🎫 Ticket Overview</div>
                <div class="tdc-grid-row">
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Ticket ID:</span><br />
                        <asp:Label ID="lblTicketId" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Created On:</span><br />
                        <asp:Label ID="lblCreatedOn" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Created By:</span><br />
                        <asp:Label ID="lblCreatedBy" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Region:</span><br />
                        <asp:Label ID="lblRegion" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Company:</span><br />
                        <asp:Label ID="lblCompany" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Status:</span><br />
                        <asp:Label ID="lblStatus" runat="server" CssClass="tdc-badge-status" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Priority Level:</span><br />
                        <asp:Label ID="lblPriority" runat="server" CssClass="text-danger font-weight-bold" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Root Category 1:</span><br />
                        <asp:Label ID="lblRoot1" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Root Category 2:</span><br />
                        <asp:Label ID="lblRoot2" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Root Category 3:</span><br />
                        <asp:Label ID="lblRoot3" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Date Open:</span><br />
                        <asp:Label ID="lblDateOpen" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Date Closed:</span><br />
                        <asp:Label ID="lblDateClosed" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Assigned To:</span><br />
                        <asp:Label ID="lblAssignedTo" runat="server" CssClass="tdc-info-value" />
                    </div>
                    <div class="tdc-grid-col">
                        <span class="tdc-info-label">Assigned On:</span><br />
                        <asp:Label ID="lblAssignedOn" runat="server" CssClass="tdc-info-value" />
                    </div>
                </div>

                <div class="mt-4">
                    <span class="tdc-info-label">Description:</span>
                    <div class="tdc-description mt-2">
                        <asp:Label ID="lblDescription" runat="server" />
                    </div>
                </div>

                <div class="tdc-text-right mt-4">
                    <asp:Button ID="btnBack" runat="server" Text="← Back to Tickets" CssClass="btn-primary" OnClick="btnBack_Click" />
                </div>
            </div>

            <!-- Manage Ticket Actions -->
<div id="ticketActionsContainer" class="tdc-container mt-5" runat="server" visible="true"
     style="background: #fff; border-radius: 12px; padding: 20px; box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08); font-size: 16px; margin-bottom: 30px; max-width: 1600px;">

    <!-- Status Message -->
    <asp:Label ID="lblTicketStatusMessage" runat="server"
               style="display: block; background-color: #f8d7da; color: #721c24; padding: 12px; border: 1px solid #f5c6cb; border-radius: 6px; font-size: 15px; margin-bottom: 20px; font-weight: 500;" />

    <!-- Section Title -->
    <div class="tdc-section-title tdc-text-center"
         style="margin-bottom: 20px; border-bottom: 2px solid #eee; padding-bottom: 10px;">
        <h3 style="font-size: 26px; font-weight: 600; color: #333;">🛠️ Manage Ticket</h3>
    </div>

    <!-- Flex Container for Assign & Close -->
    <div style="display: flex; flex-wrap: wrap; gap: 20px; justify-content: space-between;">

        <!-- Assign Ticket -->
        <asp:Panel ID="pnlAssignTicket" runat="server" Visible="true"
                   style="flex: 1 1 48%; background-color: #f9f9f9; border-radius: 8px; padding: 16px; border: 1px solid #ddd; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);">
            <h4 style="font-size: 20px; font-weight: 600; color: #007bff; margin-bottom: 15px;">🎯 Assign Ticket</h4>

            <div style="display: flex; flex-direction: column; gap: 12px;">
                <label style="font-weight: 500; color: #495057;">Employee Code:</label>
                <asp:TextBox ID="txtEmployeeCode" runat="server" AutoPostBack="true" OnTextChanged="txtEmployeeCode_TextChanged"
                             placeholder="Enter employee code..."
                             style="width: 100%; padding: 8px 10px; font-size: 14px; border: 1px solid #ccc; border-radius: 6px;" />

                <label style="font-weight: 500; color: #495057;">Employee Name:</label>
                <asp:TextBox ID="txtEmployeeName" runat="server" ReadOnly="true" Enabled="false"
                             placeholder="Employee name will appear here"
                             style="width: 100%; padding: 8px 10px; font-size: 14px; border: 1px solid #ccc; border-radius: 6px; background-color: #f8f9fa;" />
            </div>

            <div style="text-align: right; margin-top: 15px;">
                <asp:Button ID="btnAssignTicket" runat="server" Text="Assign Ticket" OnClick="btnAssignTicket_Click"
                            style="padding: 10px 20px; font-size: 14px; border-radius: 6px; background-color: #007bff; color: #fff; border: none; cursor: pointer;" />
            </div>
        </asp:Panel>

        <!-- Close Ticket -->
        <asp:Panel ID="pnlCloseTicket" runat="server" Visible="true"
                   style="flex: 1 1 48%; background-color: #f9f9f9; border-radius: 8px; padding: 16px; border: 1px solid #ddd; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);">
            <h4 style="font-size: 20px; font-weight: 600; color: #28a745; margin-bottom: 15px;">✅ Close Ticket</h4>

            <div style="display: flex; flex-direction: column; gap: 12px;">
                <label style="font-weight: 500; color: #495057;">Remarks:</label>
                <asp:TextBox ID="txtClosingRemarks" runat="server" TextMode="MultiLine" Rows="3"
                             placeholder="Enter remarks here..."
                             style="width: 100%; padding: 8px 10px; font-size: 14px; border: 1px solid #ccc; border-radius: 6px;" />

                <label style="font-weight: 500; color: #495057;">Upload Photo (optional):</label>
                <asp:FileUpload ID="fuClosingPhoto" runat="server" onchange="showImagePreview(this)"
                                style="width: 100%; padding: 8px 10px; font-size: 14px; border: 1px solid #ccc; border-radius: 6px;" />
                <asp:Image ID="imgPreview1" runat="server" Visible="false"
                           style="max-width: 120px; max-height: 120px; margin-top: 10px; border-radius: 6px; border: 1px solid #ddd;" />
            </div>

            <div style="text-align: right; margin-top: 15px;">
                <asp:Button ID="btnCloseTicket" runat="server" Text="Close Ticket" OnClick="btnCloseTicket_Click"
                            style="padding: 10px 20px; font-size: 14px; border-radius: 6px; background-color: #dc3545; color: #fff; border: none; cursor: pointer;" />
            </div>
        </asp:Panel>

    </div>

    <!-- Withdraw Ticket -->
    <asp:Panel ID="pnlWithdrawTicket" runat="server" Visible="true"
               style="margin-top: 20px; background-color: #f9f9f9; border-radius: 8px; padding: 16px; border: 1px solid #ddd; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);">
        <h4 style="font-size: 20px; font-weight: 600; color: #ffc107; margin-bottom: 15px;">🛑 Withdraw Ticket</h4>

        <div style="background-color: #fff3cd; border: 1px solid #ffeeba; color: #856404; padding: 12px 15px; border-radius: 6px; margin-bottom: 15px; font-size: 14px;">
            ⚠️ Are you sure you want to withdraw this ticket? This action cannot be undone.
        </div>

        <div style="text-align: right;">
            <asp:Button ID="btnWithdrawTicket" runat="server" Text="Withdraw Ticket" OnClick="btnWithdrawTicket_Click"
                        style="padding: 10px 20px; font-size: 14px; border-radius: 6px; background-color: #ffc107; color: #fff; border: none; cursor: pointer;" />
        </div>
    </asp:Panel>
</div>




        </div>
    </div>


<script type="text/javascript">

    function showImagePreview(input) {
       
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.getElementById('<%= imgPreview1.ClientID %>');
                img.src = e.target.result;
                img.style.display = "block";
            };

            reader.readAsDataURL(input.files[0]);
        }
    }
</script>

</asp:Content>
