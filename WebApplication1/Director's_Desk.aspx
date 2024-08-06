<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="Director's_Desk.aspx.cs" Inherits="atsweb.Director_s_Desk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <!-- start page-title -->
        <section class="page-title">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <h2>Director Desk</h2>
                        <ol class="breadcrumb">
                            <li><a href="home.aspx">Home</a></li>
                            <li>Director Desk</li>
                        </ol>
                    </div>
                </div> <!-- end row -->
            </div> <!-- end container -->
        </section>
        <!-- end page-title -->
        <!-- start features-section-s2 -->
        <section class="features-section-s2 section-padding">
            <div class="container text-justify">
                <div class="row">
                    <div class="col col-lg-10 col-lg-offset-1">
                        <div class="img-holder">
                            <asp:Image ID="img_director" runat="server" />
                        </div>
                        <div class="features-grids">
                            <div class="grid">
                                <div class="icon">
                                    <i class="fi flaticon-expand"></i>
                                </div>
                                <h3>Board of Director</h3>
                                <p>
                                <asp:Label ID="lbl_business_insights" runat="server" Text="Label"></asp:Label> </p>
                            </div>
                            <div class="grid">
                                <div class="icon">
                                    <i class="fi flaticon-3d"></i>
                                </div>
                                <h3>A Word from Our Director</h3>
                                <p>
                                    <asp:Label ID="lbl_Market_reacherch" runat="server" Text="Label"></asp:Label> </p>
                            </div>
                            <div class="grid">
                                <div class="icon">
                                    <i class="fi flaticon-gear"></i>
                                </div>
                                <h3>Contact Our Director</h3>
                                <p>
                                 <asp:Label ID="lbl_Thought_leadership" runat="server" Text="Label"></asp:Label> </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div> <!-- end container -->
        </section>
        <!-- end features-section-s2 -->
        <!-- start team-section -->
        <section class="team-section section-padding">
            <div class="container">
                <div class="row">
                    <div class="col col-lg-9">
                        <div class="section-title">
                            <span>Team members</span>
                            <h2>Meet the team</h2>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-xs-12">
                        <div class="team-grids clearfix">
                            <div class="grid">
                                <div class="img-holder">
                                    <asp:Image ID="img_Team_member1" runat="server" />
                                  
                                </div>
                                <div class="details">
                                    <h3><asp:Label ID="lbl_Team_Member1" runat="server" Text="Label"></asp:Label></h3>
                                    <span>Engineer</span>
                                </div>
                            </div>
                            <div class="grid">
                                <div class="img-holder">
                                    <asp:Image ID="img_Team_member2" runat="server" />
                                   
                                </div>
                                <div class="details">
                                    <h3><asp:Label ID="lbl_Team_Member2" runat="server" Text="Label"></asp:Label></h3>
                                    <span>HR Manager</span>
                                </div>
                            </div>
                            <div class="grid">
                                <div class="img-holder">
                                    <asp:Image ID="img_Team_member3" runat="server" />

                                </div>
                                <div class="details">
                                    <h3><asp:Label ID="lbl_Team_Member3" runat="server" Text="Label"></asp:Label></h3>
                                    <span>Founder</span>
                                </div>
                            </div>
                            <div class="grid">
                                <div class="img-holder">
                                    <asp:Image ID="img_Team_member4" runat="server" />
                                    
                                </div>
                                <div class="details">
                                    <h3><asp:Label ID="lbl_Team_Member4" runat="server" Text="Label"></asp:Label></h3>
                                    <span>ASS Enginer</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div> <!-- end container -->
        </section>
        <!-- end team-section -->
</asp:Content>
