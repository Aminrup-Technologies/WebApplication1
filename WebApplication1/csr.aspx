<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="csr.aspx.cs" Inherits="atsweb.csr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        /* General Styles */
        .csr-pg-section-s2 {
            padding: 20px 0;
        }

        .csr-activities-grids {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
        }

        .grid {
            background-color: #f8f8f8;
            border: 1px solid #ddd;
            border-radius: 4px;
            overflow: hidden;
            flex: 1 1 calc(33.333% - 20px); /* 3 columns with 20px gap */
            box-sizing: border-box;
        }

        .activity-pic img {
            width: 100%;
            height: auto;
            display: block;
        }

        .details {
            padding: 10px;
            text-align: center;
        }

            .details h3 {
                margin: 10px 0;
                font-size: 18px;
            }

            .details p {
                font-size: 14px;
                color: #555;
            }

            .details span {
                color: #777;
                font-size: 14px;
                display: block;
                margin-top: 5px;
            }

        /* Responsive Styles */
        @media (max-width: 767px) {
            .grid {
                flex: 1 1 100%; /* Full width on mobile devices */
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- start page-title -->
    <section class="page-title" id="pagetitle" runat="server" visible="true">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h3>&nbsp;</h3>
                    <ol class="breadcrumb">
                        <li><a href="home.aspx">Home</a></li>
                        <li>CSR</li>
                    </ol>
                </div>
            </div>
            <!-- end row -->
        </div>
        <!-- end container -->
    </section>
    <!-- end page-title -->

    <!--  start recent-blog-section -->
    <section class="recent-blog-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>Our Community Engagement</span>
                        <h2>CSR Initiatives</h2>
                        <p>Our CSR initiatives are designed to make a meaningful impact on the communities we serve. We're dedicated to reducing our environmental footprint, promoting social responsibility, and fostering a culture of inclusivity and respect. Through philanthropic efforts, volunteer programs, and sustainable practices, we're committed to creating a brighter future for all.</p>
                    </div>
                </div>
            </div>

            <section class="csr-pg-section-s2 section-padding">
                <div class="container">
                    <div class="row">
                        <div class="col col-xs-12">
                            <div class="csr-activities-grids">
                                <asp:Repeater ID="rptCSRActivities" runat="server">
                                    <ItemTemplate>
                                        <div class="grid">
                                            <div class="activity-pic">
                                                <img src='<%# Eval("ActivityImage") %>' alt='<%# Eval("ActivityTitle") %>' />
                                            </div>
                                            <div class="details">
                                                <h3><a href="#"><%# Eval("ActivityTitle") %></a></h3>
                                                <p><%# Eval("ActivityDescription") %></p>
                                                <span>Date: <%# Eval("ActivityDate", "{0:MMM dd, yyyy}") %></span>
                                                <span>Location: <%# Eval("Location") %></span>
                                                <span>Outcomes: <%# Eval("Outcomes") %></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </div>
        <!-- end container -->
    </section>
    <!-- end recent-blog-section -->

</asp:Content>
