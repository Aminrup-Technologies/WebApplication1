<%@ Page Title="Supervisor Wizard" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="supervisor_wizard.aspx.cs" Inherits="WebApplication1.bussiness.production.supervisor_wizard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Content/supervisor-wizard.css") %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="right_col sw-page" role="main">
        <div class="page-title">
            <div class="title_left">
                <h3>Supervisor Wizard</h3>
            </div>
        </div>

        <div class="clearfix"></div>

        <div class="sw-search">
            <div class="form-group">
                <label class="control-label col-md-2 col-sm-2 label-align">JOBID</label>
                <div class="col-md-5 col-sm-6 col-xs-12">
                    <asp:TextBox ID="txt_jobid" runat="server" CssClass="form-control text-uppercase" placeholder="e.g. JOB260424322"></asp:TextBox>
                </div>
                <div class="col-md-4 col-sm-4 col-xs-12">
                    <asp:Button ID="btn_search" runat="server" Text="Load" CssClass="btn btn-success" OnClick="btn_search_Click" />
                    <asp:Button ID="btn_reset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btn_reset_Click" />
                </div>
            </div>
            <div class="clearfix"></div>
        </div>

        <asp:HiddenField ID="hf_jobid" runat="server" />
        <asp:HiddenField ID="hf_visualStep" runat="server" Value="0" />
        <asp:HiddenField ID="hf_progress" runat="server" />

        <div class="sw-stepper" role="tablist" aria-label="Supervisor stages">
            <div id="step_create" runat="server" class="sw-step">1 Create</div>
            <div id="step_in" runat="server" class="sw-step">2 IN</div>
            <div id="step_permit" runat="server" class="sw-step is-optional">3 Permit</div>
            <div id="step_out" runat="server" class="sw-step">4 OUT</div>
            <div id="step_close" runat="server" class="sw-step">5 Close</div>
            <div id="step_complete" runat="server" class="sw-step">6 Complete</div>
        </div>

        <div id="pnl_card" runat="server" class="sw-card" visible="false">
            <h4 id="lit_step_title" runat="server">Current step</h4>
            <dl class="row" style="margin-bottom: 0;">
                <dt class="col-sm-3">JOBID</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_jobid" runat="server"></asp:Label></dd>
                <dt class="col-sm-3">Site</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_site" runat="server"></asp:Label></dd>
                <dt class="col-sm-3">Contractor</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_contractor" runat="server"></asp:Label></dd>
                <dt class="col-sm-3">Shift</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_shift" runat="server"></asp:Label></dd>
                <dt class="col-sm-3">Current state</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_state" runat="server"></asp:Label></dd>
                <dt class="col-sm-3">Recommended next</dt>
                <dd class="col-sm-9"><asp:Label ID="lbl_next" runat="server"></asp:Label></dd>
            </dl>
        </div>

        <div id="pnl_empty" runat="server" class="sw-card">
            <p class="text-muted" style="margin: 0;">Load a JOBID to derive progress, or open Create to start a new job on the existing V2 page. This shell does not write JOB state.</p>
        </div>

        <div class="sw-actions">
            <asp:Button ID="btn_previous" runat="server" Text="Previous" CssClass="btn btn-default" OnClick="btn_previous_Click" />
            <asp:Button ID="btn_open" runat="server" Text="Open Step" CssClass="btn btn-primary" OnClick="btn_open_Click" />
            <asp:Button ID="btn_next" runat="server" Text="Next" CssClass="btn btn-default" OnClick="btn_next_Click" />
        </div>
    </div>

    <script type="text/javascript">
        function showPNotify(title, text, type) {
            new PNotify({
                title: title,
                text: text,
                type: type,
                styling: 'bootstrap3',
                delay: 4000
            });
        }
    </script>
</asp:Content>
