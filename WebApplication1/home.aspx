<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="atsweb.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- start of hero -->
    <section class="hero hero-slider-wrapper hero-style-2">
        <div class="pattern">
            <span></span>
            <span></span>
            <span></span>
            <span></span>
            <span></span>
        </div>

        <div class="hero-slider">
            <div class="slide">
                <asp:Image ID="img_comp_greetinghdr1" runat="server" CssClass="slider-bg" />
                <div class="container">
                    <div class="row">
                        <div class="col col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 slide-caption">
                            <div class="slide-subtitle">
                                <p>
                                    <asp:Label ID="lbl_comp_greetinghdr1" runat="server" Text="Company Greeting 1"></asp:Label>
                                </p>
                            </div>
                            <div class="slide-title">
                                <h2>We are the best industrial company in the world </h2>
                            </div>
                            <div class="btns">
                                <a href="About.aspx" class="theme-btn">More About Us</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="slide">
                <img src="assets/images/slider/slide-3.jpg" alt="" class="slider-bg" />
                <div class="container">
                    <div class="row">
                        <div class="col col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 slide-caption">
                            <div class="slide-subtitle">
                                <p>
                                    <asp:Label ID="lbl_comp_greetinghdr2" runat="server" Text="Company Greeting 2"></asp:Label>
                                </p>
                            </div>
                            <div class="slide-title">
                                <h2>We Provide best industrial in industrial area</h2>
                            </div>
                            <div class="btns">
                                <a href="#" class="theme-btn">More About Us</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="slide">
                <img src="assets/images/slider/slide-4.jpg" alt="" class="slider-bg" />
                <div class="container">
                    <div class="row">
                        <div class="col col-lg-8 col-lg-offset-2 col-md-10 col-md-offset-1 slide-caption">
                            <div class="slide-subtitle">
                                <p>Welcome to itus industry</p>
                            </div>
                            <div class="slide-title">
                                <h2>We are the best industrial company in the world </h2>
                            </div>
                            <div class="btns">
                                <a href="#" class="theme-btn">More About Us</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- end of hero slider -->

    <!-- start about-section-s2 -->
    <section class="about-section-s2 section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-lg-9">
                    <div class="section-title-s2">
                        <span>Welcome to</span>
                        <h2>Our Company</h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col col-lg-7">
                    <div class="about-features clearfix">
                        <div class="grid">
                            <div class="icon">
                                <i class="fi flaticon-worker"></i>
                            </div>
                            <div class="details">
                                <span class="count">01</span>
                                <h3>Team of Professional</h3>
                                <p>Lay peacefully between its four familiar walls. A collection of textile samples lay spread out on the table.</p>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="icon">
                                <i class="fi flaticon-gear"></i>
                            </div>
                            <div class="details">
                                <span class="count">02</span>
                                <h3>Full of services</h3>
                                <p>Lay peacefully between its four familiar walls. A collection of textile samples lay spread out on the table.</p>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="icon">
                                <i class="fi flaticon-energy-saving"></i>
                            </div>
                            <div class="details">
                                <span class="count">03</span>
                                <h3>Smart Technology</h3>
                                <p>Lay peacefully between its four familiar walls. A collection of textile samples lay spread out on the table.</p>
                            </div>
                        </div>
                        <div class="grid">
                            <div class="icon">
                                <i class="fi flaticon-oil"></i>
                            </div>
                            <div class="details">
                                <span class="count">04</span>
                                <h3>27/7 Support</h3>
                                <p>Lay peacefully between its four familiar walls. A collection of textile samples lay spread out on the table.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col col-lg-5"></div>
            </div>
            <div class="row">
                <div class="col col-xs-12">
                    <div class="more-about">
                        <a href="About.aspx">More About Us <i class="fi flaticon-slim-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
        <!-- end container -->
    </section>
    <!-- end about-section-s2 -->

</asp:Content>
