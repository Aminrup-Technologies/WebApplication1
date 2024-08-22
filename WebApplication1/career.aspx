<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="career.aspx.cs" Inherits="atsweb.career" %>

<%@ Import Namespace="atsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .panel-heading a {
            font-size: 12px;
            font-weight: bold;
            text-decoration: none;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .panel-heading a .badge {
            font-size: 10px;
            background-color: #007bff;
            color: #fff;
            padding: 3px 5px;
        }

        .panel-body p {
            margin: 3px 0;
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
        }

        .btn-primary:hover {
            background-color: #0056b3;
            border-color: #004085;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-title">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h3>&nbsp;</h3>
                    <ol class="breadcrumb">
                        <li><a href="home.aspx">Home</a></li>
                        <li>Career</li>
                    </ol>
                </div>
            </div>
        </div>
    </section>

    <section class="faq-pg-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>We are Hiring</span>
                        <h2>Current Openings</h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col col-xs-12">
                    <div class="faq-section">
                        <div class="panel-group faq-accordion theme-accordion-s1" id="accordion">
                            <br />
                            <asp:Repeater ID="rptJobOpenings" runat="server">
                                <ItemTemplate>
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            <a class='<%# Container.ItemIndex == 0 ? "collapsed" : "" %>' data-toggle="collapse" data-parent="#accordion" href='<%# "#collapse-" + Container.ItemIndex %>'>
                                                <%# Eval("PositionName") %>
                                                <span class="badge"><%# Eval("NoOfOpenings") %> Openings</span>
                                            </a>
                                        </div>
                                        <div id='<%# "collapse-" + Container.ItemIndex %>' class="panel-collapse collapse <%# Container.ItemIndex == 0 ? "in" : "" %>'">
                                            <div class="panel-body">
                                                <p><strong>Work Location:</strong> <%# Eval("WorkLocation") %></p>
                                                <p><strong>Job Description:</strong> <%# Eval("JobDescription") %></p>
                                                <p><strong>Min Qualification:</strong> <%# Eval("MinQualification") %></p>
                                                <p><strong>Min Experience:</strong> <%# Eval("MinExperience") %></p>
                                                <p id="p_salary" runat="server" visible="false"><strong>Salary:</strong> <%# Eval("Salary") %></p>
                                                <asp:Button ID="btnApplyNow" runat="server" Text="Apply Now" OnClick="btnApplyNow_Click" CommandArgument='<%# Eval("JobID") %>' CssClass="btn btn-primary" />
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
