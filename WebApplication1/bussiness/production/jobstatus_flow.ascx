<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jobstatus_flow.ascx.cs" Inherits="WebApplication1.bussiness.production.jobstatus_flow" %>
<div id="wizard" class="form_wizard wizard_horizontal">
    <ul class="wizard_steps">
        <li>
            <a href="#step-1">
                <div id="step1" runat="server" class="step_no">1</div>
                <span class="step_descr">Step 1<br />
                    <small>JOBID is Created</small>
                </span>
            </a>
        </li>
        
        <li>
            <a href="#step-2">
                <div id="step2" runat="server" class="step_no">2</div>
                <span class="step_descr">Step 2<br />
                    <small>In-Punch Done</small>
                </span>
            </a>
        </li>
        <li>
            <a href="#step-3">
                <div id="step3" runat="server" class="step_no">3</div>
                <span class="step_descr">Step 3<br />
                    <small>Permit Uploaded</small>
                </span>
            </a>
        </li>
        <li>
            <a href="#step-4">
                <div id="step4" runat="server" class="step_no">4</div>
                <span class="step_descr">Step 4<br />
                    <small>Out-Punch Done</small>
                </span>
            </a>
        </li>
        <li>
            <a href="#step-5">
                <div id="step5" runat="server" class="step_no">5</div>
                <span class="step_descr">Step 5<br />
                    <small>Approved by Approver</small>
                </span>
            </a>
        </li>
    </ul>
</div>
