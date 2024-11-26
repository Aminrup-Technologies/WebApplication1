<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="contact_us.aspx.cs" Inherits="atsweb.contact_us" %>

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
                        <li>Contact</li>
                    </ol>
                </div>
            </div>
            <!-- end row -->
        </div>
        <!-- end container -->
    </section>
    <!-- end page-title -->

    <!-- start contact-pg-section -->
    <section class="contact-pg-section section-padding">
        <div class="container">
            <div class="row">
                <div class="col col-xs-12">
                    <div class="section-title-s2">
                        <span>Our contact info</span>
                        <h2>Contact with us</h2>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col col-lg-6">
                    <div class="contact-form">
                        <form method="post" class="contact-validation-active" id="contact-form-s2">
                            <div class="form-group mt-3">

                                <asp:TextBox ID="name" runat="server" placeholder="  Your Name"
                                    CssClass="half-col" Width="550px" Height="56px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFV_name" runat="server" ErrorMessage="Name is Requried" ControlToValidate="name" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>

                            </div>

                            <div class="form-group mt-3">
                                <asp:TextBox ID="email" runat="server" CssClass="half-col"
                                    placeholder="  Your Email" Width="550px" TabIndex="1" Height="56px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFV_email" runat="server" ErrorMessage="Email is Requried" ControlToValidate="email" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group mt-3">
                                <asp:TextBox ID="phone" runat="server" placeholder="  Your Phone"
                                    class="half-col" Width="550px" TabIndex="1" Height="56px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFV_phone" runat="server" ErrorMessage="Phone is Requried" ControlToValidate="phone" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group mt-3">
                                <asp:TextBox ID="address" runat="server" placeholder="  Address"
                                    class="half-col" Width="550px" TabIndex="1" Height="56px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFV_address" runat="server" ErrorMessage="Address is Requried" ControlToValidate="address" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group mt-3">
                                <asp:TextBox ID="note" runat="server" TextMode="MultiLine"
                                    placeholder="  Description..." class="half-col"
                                    Width="550px" Height="96px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFV_note" runat="server" ErrorMessage="Description is Requried" ControlToValidate="note" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="submit-btn-wrapper mt-3">
                                <asp:Button ID="submitBtn" runat="server" CssClass="theme-btn-s3"
                                    Text="Appointment" OnClick="SubmitBtn_Click" />

                                <div id="loader" style="display: none;" class="mt-3">
                                    <i class="ti-reload"></i>
                                </div>
                            </div>

                            <div class="clearfix error-handling-messages mt-3">
                                <br />
                                <br />
                                <asp:Label ID="success" runat="server" Text="Thank you" CssClass="success" Visible="false"></asp:Label>
                                <asp:Label ID="error" runat="server" Text="Error occurred while sending email. Please try again later." CssClass="error" Visible="false"></asp:Label>
                            </div>
                        </form>
                    </div>
                </div>

                <div class="col col-lg-6">
                    <div class="info-box-outer">
                        <div class="info-box">
                            <div class="grid">
                                <h3>Address</h3>
                                <p>3rd Floor, Aastha City Centre, Mills & Godown Area, near Tinkonia Hotel, Hirasingh Bagan, Sakchi, Jamshedpur, Jharkhand 831001</p>
                            </div>
                            <div class="grid">
                                <h3>Phone</h3>
                                <p>654974-644545-344, &nbsp; 654974-644545-344</p>
                            </div>
                            <div class="grid">
                                <h3>Email</h3>
                                <p>group@info.com, &nbsp; deom@ex.com</p>
                            </div>
                            <div class="grid" id="social_handle" runat="server" visible="false">
                                <ul class="social">
                                    <li><a href="#"><i class="ti-facebook"></i></a></li>
                                    <li><a href="#"><i class="ti-twitter-alt"></i></a></li>
                                    <li><a href="#"><i class="ti-linkedin"></i></a></li>
                                    <li><a href="#"><i class="ti-pinterest"></i></a></li>
                                    <li><a href="#"><i class="ti-vimeo-alt"></i></a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col col-xs-12">
                    <div class="contact-map">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3677.9998221769356!2d86.20311217535118!3d22.80247047932871!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x2e210e2fc505d1e3%3A0xcadfb2757be7bf53!2sAutomation%20%26%20Technical%20Services%20(ATS)!5e0!3m2!1sen!2sin!4v1722619504008!5m2!1sen!2sin" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- end contact-pg-section -->

</asp:Content>
