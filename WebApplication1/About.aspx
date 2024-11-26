<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="atsweb.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- start page-title -->
    <section class="page-title">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h3>&nbsp;</h3>
                    <ol class="breadcrumb">
                        <li><a href="home.aspx">Home</a></li>
                        <li>About</li>
                    </ol>
                </div>
            </div>
            <!-- end row -->
        </div>
        <!-- end container -->
    </section>
    <br />
    <section class="about-pg-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-md-5">
                    <div class="about-text text-justify">
                        <asp:Label ID="lbl_Header" runat="server"></asp:Label>
                        <asp:Label ID="lbl_About_Company" runat="server"></asp:Label>
                        <div class="info">
                            <asp:Image ID="img_Signature" runat="server" />
                            <h4>Managing Director</h4>
                            <span>CEO of the company</span>
                        </div>
                    </div>
                </div>
                <div class="col col-md-7">
                    <div class="about-pic-video">
                        <div class="ceo-holder">
                            <asp:Image ID="Ceo" runat="server" />
                        </div>
                        <div class="video-holder">
                            <div class="img-holder">
                                <asp:Image ID="about_pic" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col col-xs-12">
                    <div class="experience-text">
                        <h2><span>
                            <asp:Label ID="lbl_Years" runat="server"></asp:Label>
                        </span>years of experience</h2>
                    </div>
                </div>
            </div>
            <div class="row mission-vission">
                <div class="col col-md-4">
                    <div class="details text-justify">
                        <h3>Our Mission</h3>
                        <asp:Label ID="lbl_Mission" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col col-md-4">
                    <div class="details text-justify">
                        <h3>Our Vision</h3>
                        <asp:Label ID="lbl_vision" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col col-md-4">
                    <div class="details text-justify">
                        <h3>Our Commitment</h3>
                        <asp:Label ID="lbl_commitment" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <!-- end container -->
    </section>
    <!-- end about-pg-section -->
    <!-- start about-pg-history -->
    <section class="about-pg-history section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-lg-12">
                    <div class="section-title">
                        <span>Our history</span>
                        <h2>Know Our History</h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col col-xs-12">
                    <div class="history-grids clearfix">
                        <div class="grid">
                            <span class="date">June 2016</span>
                            <h3>Started the company</h3>
                            <p>Pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. "What's happened to me?" he thought. It wasn't a dream. His room, a proper human </p>
                        </div>
                        <div class="grid">
                            <span class="date">July 2017</span>
                            <h3>Add 20 new worker</h3>
                            <p>Pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. "What's happened to me?" he thought. It wasn't a dream. His room, a proper human </p>
                        </div>
                        <div class="grid">
                            <span class="date">Feb 2018</span>
                            <h3>Open 15 new branch</h3>
                            <p>Pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. "What's happened to me?" he thought. It wasn't a dream. His room, a proper human </p>
                        </div>
                        <div class="grid">
                            <span class="date">March 2018</span>
                            <h3>Reached the first milestone</h3>
                            <p>Pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. "What's happened to me?" he thought. It wasn't a dream. His room, a proper human </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- end about-pg-history -->
    <!-- start fun-fact-section -->
    <section class="fun-fact-section">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="funfact-grids">
                        <div class="grid">
                            <span class="icon">
                                <i class="fi flaticon-mine"></i>
                            </span>
                            <div>
                                <h3><span class="odometer" data-count="500">00</span>+</h3>
                            </div>
                            <p>Certified Engineers</p>
                        </div>
                        <div class="grid">
                            <span class="icon">
                                <i class="fi flaticon-face"></i>
                            </span>
                            <div>
                                <h3><span class="odometer" data-count="15">00</span>+</h3>
                            </div>
                            <p>Happy Clients</p>
                        </div>
                        <div class="grid">
                            <span class="icon">
                                <i class="fi flaticon-trophy-1"></i>
                            </span>
                            <div>
                                <h3><span class="odometer" data-count="20"></span>+</h3>
                            </div>
                            <p>Award Won</p>
                        </div>
                        <div class="grid">
                            <span class="icon">
                                <i class="fi flaticon-like-1"></i>
                            </span>
                            <div>
                                <h3><span class="odometer" data-count="120">00</span>+</h3>
                            </div>
                            <p>Projects Done</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end container -->
    </section>
    <!-- end fun-fact-section -->
    <br />
</asp:Content>
