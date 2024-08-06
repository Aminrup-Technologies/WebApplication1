<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="Awards_Associations.aspx.cs" Inherits="atsweb.Awards_Associations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- start page-title -->
    <section class="page-title">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <h2>Awards & Associations</h2>
                    <ol class="breadcrumb">
                        <li><a href="home.aspx">Home</a></li>
                        <li>About</li>
                    </ol>
                </div>
            </div> <!-- end row -->
        </div> <!-- end container -->
    </section>
            <!-- start projects-pg-section -->
        <section class="projects-pg-section section-padding">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <div class="projects-grids clearfix">
                            <div class="grid">
                                <div class="project-pic">
                                    <asp:Image ID="img_award1" runat="server" />
                                </div>
                                <div class="details">
                                    <div class="inner">
                                        <div class="count">01</div>
                                        <h4>Metal and Non - Metals</h4>
                                        <p>Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out </p>
                                        <a href="#" class="theme-btn">View Project</a>
                                    </div>
                                </div>
                            </div>
                            <div class="grid right-text">
                                <div class="details">
                                    <div class="inner">
                                        <div class="count">02</div>
                                        <h4>Construction Materials</h4>
                                        <p>Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out </p>
                                        <a href="#" class="theme-btn">View Project</a>
                                    </div>
                                </div>
                                <div class="project-pic">
                                    <asp:Image ID="img_award2" runat="server" />
                                </div>
                            </div>
                            <div class="grid">
                                <div class="project-pic">
                                    <asp:Image ID="img_award3" runat="server" />
                                </div>
                                <div class="details">
                                    <div class="inner">
                                        <div class="count">03</div>
                                        <h4>Construction Materials</h4>
                                        <p>Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out </p>
                                        <a href="#" class="theme-btn">View Project</a>
                                    </div>
                                </div>
                            </div>
                            <div class="grid right-text">
                                <div class="details">
                                    <div class="inner">
                                        <div class="count">04</div>
                                        <h4>Metal and Non - Metals</h4>
                                        <p>Cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out </p>
                                        <a href="#" class="theme-btn">View Project</a>
                                    </div>
                                </div>
                                <div class="project-pic">
                                    <asp:Image ID="img_award4" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-xs-12">
                        <div class="pagination-wrapper">
                            <ul class="pg-pagination">
                                <li>
                                    <a href="#" aria-label="Previous">
                                        <i class="ti-arrow-left"></i>
                                    </a>
                                </li>
                                <li class="active"><a href="#">1</a></li>
                                <li><a href="#">2</a></li>
                                <li><a href="#">3</a></li>
                                <li>
                                    <a href="#" aria-label="Next">
                                        <i class="ti-arrow-right"></i>
                                    </a>
                                </li>
                            </ul>
                        </div>

                    </div>
                </div>
            </div> <!-- end container -->
        </section>
        <!-- end projects-pg-section -->
</asp:Content>
