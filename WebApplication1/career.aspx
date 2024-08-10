<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="career.aspx.cs" Inherits="atsweb.career" %>

<%@ Import Namespace="atsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-title">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h3>&nbsp;</h3>
                    <ol class="breadcrumb">
                        <li><a href="home.aspx">Home</a></li>
                        <li>CAREER</li>
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
                                                <asp:Button ID="btn_apply" runat="server" CssClass="btn btn-sm btn-primary" Text="Apply Now" />
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
