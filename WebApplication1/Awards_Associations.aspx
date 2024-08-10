<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="Awards_Associations.aspx.cs" Inherits="atsweb.Awards_Associations" %>

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
            <!-- start projects-pg-section -->
     <section class="projects-pg-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="projects-grids clearfix">
                        <asp:Repeater ID="rptProjects" runat="server">
                            <ItemTemplate>
                                <div class="grid compact">
                                    <div class="project-pic">
                                        <asp:Image ID="imgProject" runat="server" ImageUrl='<%# Eval("ImagePath") %>' CssClass="img-responsive" />
                                    </div>
                                    <div class="details">
                                        <div class="inner">
                                            <h4 class="title"><%# Eval("Title") %></h4>
                                            <p class="description"><%# Eval("Description") %></p>
                                            <a href="#" class="theme-btn small-btn">View</a>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <div class="grid compact right-text">
                                    <div class="details">
                                        <div class="inner">
                                            <h4 class="title"><%# Eval("Title") %></h4>
                                            <p class="description"><%# Eval("Description") %></p>
                                            <a href="#" class="theme-btn small-btn">View</a>
                                        </div>
                                    </div>
                                    <div class="project-pic">
                                        <asp:Image ID="imgProject" runat="server" ImageUrl='<%# Eval("ImagePath") %>' CssClass="img-responsive" />
                                    </div>
                                </div>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div> <!-- end container -->
        </section>--%>
    <!-- end projects-pg-section -->
</asp:Content>
