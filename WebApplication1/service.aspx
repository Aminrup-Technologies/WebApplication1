<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="service.aspx.cs" Inherits="atsweb.service" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- start page-title -->
        <section class="page-title">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <h2>Services</h2>
                        <ol class="breadcrumb">
                            <li><a href="home.aspx">Home</a></li>
                            <li>Services</li>
                        </ol>
                    </div>
                </div> <!-- end row -->
            </div> <!-- end container -->
        </section>
        <!-- end page-title -->
    <section class="services-pg-section section-padding bg-dark-black">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>Our services info</span>
                        <h2>Our Services</h2>
                        <p>
                            We offer Project & Maintenance services to the well-known Indian Standards, or depending on the client’s preference. We also offer Electrical Maintenance works as a package, using qualified and technically competent persons. M/S Automation & Technical Services specializes in the following.
                        </p>
                    </div>
                </div>
            </div>
        
        <div class="container">

            <div class="row">
                <div class="col col-xs-12">
                    <div class="service-grids clearfix">
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img1" runat="server" Height="350" Width="360" />


                            </div>
                            <div class="details">
                                <h3><a href="#">COKE PLANT, CDQ BUCKET REPLACEMENT</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Tata Steel,</li>
                                    <li class="list-inline-item">Kalinganagar</li>
                                </ul>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img2" runat="server" Height="350" Width="360" />

                            </div>
                            <div class="details">
                                <h3><a href="#">RMHS STACKER WHEEL REPLACEMENT</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Tata Steel,</li>
                                    <li class="list-inline-item">Meramandali</li>
                                </ul>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img3" runat="server" Height="350" Width="360" />

                            </div>
                            <div class="details">
                                <h3><a href="#">CONVEYOR LINE BELT REPLACEMENT</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Factory,</li>
                                    <li class="list-inline-item">Jamshedpu</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
            </div>
    </section>
    <!-- stat cta-s2-section -->
    <div class="container">
        <section class="cta-s2-section" style="background-color: black">
            <div class="container">
                <div class="row">
                    <div class="col col-sm-9">
                        <h2 class="h2 mb-2">Trusted Construction <span class="text-primary">Since 2010</span></h2>
                        <p>
                            We provide comprehensive EPC services based on recognized Indian 
                            standards or customised to suit client preferences. Our maintenance packages include skilled 
                            professionals for mechanical and electrical works.
                        </p>
                    </div>
                    <div class="col col-sm-3">
                        <asp:LinkButton href="contact_us.aspx" ID="contact" runat="server" CssClass="theme-btn-s4">Contact With Us</asp:LinkButton>
                    </div>
                   
                </div>
            </div>
        </section>
    </div>
    <!-- end cta-s2-section -->
    <section class="services-pg-section section-padding bg-dark-black">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="service-grids clearfix">
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img4" runat="server" Height="350" Width="360" />

                            </div>
                            <div class="details">
                                <h3><a href="#">CASTER SEGMENT SHOP MAINTENANCE</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Tata Steel, </li>
                                    <li class="list-inline-item">Neelacha</li>
                                </ul>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img5" runat="server" Height="350" Width="360" />

                            </div>
                            <div class="details">
                                <h3><a href="#">SHUTDOWN-BREAKDOWN JOB</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Steel Industry,</li>
                                    <li class="list-inline-item">India</li>
                                </ul>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img6" runat="server" Height="350" Width="360" />

                            </div>
                            <div class="details">
                                <h3><a href="#">GLOBAL FABRICATION JOB</a></h3>
                                <ul class="list-inline text-muted text-small mb-0">
                                    <li class="list-inline-item">Steel Industry,</li>
                                    <li class="list-inline-item">India</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
        <!-- end container -->
        <!-- end services-pg-section -->
    </section>
</asp:Content>
