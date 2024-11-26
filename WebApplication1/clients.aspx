<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="clients.aspx.cs" Inherits="atsweb.clients" %>

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
                        <li>Clients</li>
                    </ol>
                </div>
            </div>
            <!-- end row -->
        </div>
        <!-- end container -->
    </section>
    <!-- start clients-section -->
    <section class="team-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>Our clients info</span>
                        <h2>Our Clients</h2>

                    </div>
                </div>
            </div>
        </div>

        <div class="container">

            <div class="row">
                <div class="col col-xs-12">
                    <div class="team-grids clearfix">
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img1" runat="server" Height="150" Width="250" />
                            </div>
                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img2" runat="server" Height="150" Width="250" />
                            </div>

                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img3" runat="server" Height="150" Width="250" />
                            </div>

                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img4" runat="server" Height="150" Width="250" />
                            </div>

                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img5" runat="server" Height="150" Width="250" />
                            </div>

                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img6" runat="server" Height="80" Width="250" />
                            </div>

                        </div>
                        <div class="grid">
                            <div class="img-holder">
                                <asp:Image ID="img7" runat="server" Height="150" Width="250" />
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">

                        <p>* Larsen & Tourbo Limited- Jamshedpur, Arston Engineering Limited- Jamshedpur</p>
                        <p>* Tata Steel Limited- Jamshedpur</p>
                        <p>* Tata Steel Growth Shop- Jamshedpur, Rourkela Steel Plant-Rourkela, Jindal Steel & Power Limited- Angul</p>
                        <p>* Tata Steel BSL Limited- Angul, Tata Steel Ltd- Kalinga Nagar, Jajpur</p>
                        <p>* Tata Steel Ltd NINL- Kalinga Nagar, Jajpur</p>
                        <p>* Nicco Corporation- Jamshedpur, Voltas Ltd, Tata Mines, Nuamundi</p>
                    </div>
                </div>
            </div>
        </div>
        <!-- end container -->
    </section>
</asp:Content>
