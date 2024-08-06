<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="career.aspx.cs" Inherits="atsweb.career" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Add any specific head content here -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <section class="page-title">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <h2>Career</h2>
                        <ol class="breadcrumb">
                            <li><a href="home.aspx">Home</a></li>
                            <li>Career</li>
                        </ol>
                    </div>
                </div> <!-- end row -->
            </div> <!-- end container -->
        </section>
    <section class="section-padding bg-gray contact-bottom">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>current oppenings</span>
                        <h2>current oppenings</h2>
                    </div>
                </div>
            </div>
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-lg-12 col-sm-12 openings">
                    <h3 class="txtBlack">Current openings :</h3>
                    <div class="row mb-2">
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Account</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Label ID="Label1" runat="server" Text="No. of Openings" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text="5" Font-Size="Medium" CssClass="badge badge-success px-2"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button1" runat="server" Text="View Details" CssClass="btn btn-success"/>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Supervisor</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Label ID="Label3" runat="server" Text="No. of Openings" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="5" Font-Size="Medium" CssClass="badge badge-success px-2"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button2" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Skilled Labour</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">17</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button3" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Mis Executive</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">10</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button4" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Field Employee</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">10</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button5" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Store and Purchase Head</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">10</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button6" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Chief Financial Officer</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">5</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button7" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Chief Operating Officer</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">2</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button8" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>GM/DGM/AGM Maintenance</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">25</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button9" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>Mechanic/Auto Electrician/Equipment Mechanic</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">50</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button10" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <h5>General / Deputy Business Development Manager</h5>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <span style="font-size: 16px">No. of Openings</span>
                            <span class="badge badge-success px-2" style="font-size: 14px">5</span>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-12 form-group">
                            <asp:Button ID="Button11" runat="server" Text="View Details" CssClass="btn btn-success" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-lg-4 col-sm-12" id="div_imghiring" runat="server" visible="false">
                    <asp:Image ID="imghiring" runat="server" CssClass="img-fluid rounded" AlternateText="We Are Hiring" />
                </div>
            </div>
        </div>
      </div>
    </section>
    
        <!-- start faq-section -->
        <section class="faq-pg-section section-padding">
            <div class="container">
                <div class="row">
                    <div class="col col-xs-12">
                        <div class="section-title-s2">
                             <span>current oppenings</span>
                            <h2>current oppenings</h2>
                            
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-xs-12">
                        <div class="faq-section">
                            <div class="panel-group faq-accordion theme-accordion-s1" id="accordion">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse-1" aria-expanded="true">1. Question Number One?</a>
                                    </div>
                                    <div id="collapse-1" class="panel-collapse collapse in">
                                        <div class="panel-body">
                                            <p> A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur boa who sat upright, raising a heavy fur muff that covered the whole of her lower arm towards the viewer. Gregor then turned to look out the window at the dull weather Drops.</p>
                                            <p> Hamsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with.</p>
                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapse-2">1. Question Number Two?</a>
                                    </div>
                                    <div id="collapse-2" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <p> A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur boa who sat upright, raising a heavy fur muff that covered the whole of her lower arm towards the viewer. Gregor then turned to look out the window at the dull weather Drops.</p>
                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapse-3">3. Question Number Three?</a>
                                    </div>
                                    <div id="collapse-3" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <p> A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur boa who sat upright, raising a heavy fur muff that covered the whole of her lower arm towards the viewer. Gregor then turned to look out the window at the dull weather Drops.</p>
                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapse-4">3. Question Number Four?</a>
                                    </div>
                                    <div id="collapse-4" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <p> A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur boa who sat upright, raising a heavy fur muff that covered the whole of her lower arm towards the viewer. Gregor then turned to look out the window at the dull weather Drops.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div> <!-- end faq-section -->
                    </div>
                </div>
            </div> <!-- end container -->
        </section>
        <!-- end faq-section -->
</asp:Content>
