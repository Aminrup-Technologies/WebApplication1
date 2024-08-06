<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="hse.aspx.cs" Inherits="atsweb.hse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

                <!-- start page-title -->
        <section class="page-title">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <h2>HSE</h2>
                        <ol class="breadcrumb">
                            <li><a href="hse.aspx">Home</a></li>
                            <li>HSE</li>
                        </ol>
                    </div>
                </div> <!-- end row -->
            </div> <!-- end container -->
        </section>
        <!-- end page-title -->

        <!-- start projects-pg-section -->
        <section class="projects-pg-section section-padding">
            <div class="container">

               <div class="row">
                    <div class="col col-xs-12">
                        <div class="section-title-s2">
                            <span>Health, Safety, and Environment</span>
                            <h2>Our HSE Policy</h2>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col col-xs-12">
                        <div class="projects-grids clearfix">
                            <div class="grid">
                                <div class="project-pic">                                    
                                    <asp:Image ID="img_comp_hse" runat="server" />
                                </div>
                                <div class="details">
                                    <div class="inner">
                                        <h4>Our HSE Commitment</h4>
                                        <ul>
                                            <li>Regular "Toolbox Talks" and Safety Meetings.</li>
                                            <li>Internal Safety Audits to ensure compliance.</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>    
                        </div>
                    </div>
                </div>

                <div class="row content">
                    <div class="col col-xs-12">
                        <div class="overview">
                            <h2>We Prioritize Health, Safety, and Environment(HSE) Awareness</h2>
                            <p class="word-wrap">Health, Safety and Environment awareness is major priority for us. The Company has thus developed the HSE Policy, HSE Action Plan and Employer/Employee Safety Charter
                                as instruments of managing Health, Safety and Environmental Protection throughout the whole Organization.
                                The company employs a full time Safety Advisor and each site is manned by a dedicated Safety Officer. Daily “Toolbox Talks” and Safety Meetings are regularly conducted at all our worksites, and internal Safety Audits carried out to ensure compliance with laid down procedures and regulations. We take pride in recognitions by our various clients from whom we have received awards for outstanding safety performance. We have also been prompt in implementing lessons learnt from incidents we encountered to prevent recurrence.
                            </p>
                        </div>
                    </div>
                </div>
                
            </div> <!-- end container -->
        </section>
        <!-- end projects-pg-section -->

</asp:Content>
