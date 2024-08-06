<%@ Page Title="" Language="C#" MasterPageFile="~/atsSite.Master" AutoEventWireup="true" CodeBehind="services.aspx.cs" Inherits="atsweb.services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head>
        <!-- Meta Tags -->
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <meta name="author" content="irstheme">

        <!-- Page Title -->
        <title>Itus - Industry, Industrial, Factory and Engineering HTML Template </title>

        <!-- Icon fonts -->
        <link href="assets/css/themify-icons.css" rel="stylesheet">
        <link href="assets/css/flaticon.css" rel="stylesheet">

        <!-- Bootstrap core CSS -->
        <link href="assets/css/bootstrap.min.css" rel="stylesheet">

        <!-- Plugins for this template -->
        <link href="assets/css/animate.css" rel="stylesheet">
        <link href="assets/css/owl.carousel.css" rel="stylesheet">
        <link href="assets/css/owl.theme.css" rel="stylesheet">
        <link href="assets/css/slick.css" rel="stylesheet">
        <link href="assets/css/slick-theme.css" rel="stylesheet">
        <link href="assets/css/owl.transitions.css" rel="stylesheet">
        <link href="assets/css/jquery.fancybox.css" rel="stylesheet">
        <link href="assets/css/odometer-theme-default.css" rel="stylesheet">

        <!-- Custom styles for this template -->
        <link href="assets/css/style.css" rel="stylesheet">

        <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
        <!--[if lt IE 9]>
 <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
 <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
 <![endif]-->

    </head>
    <body>

        <!-- start page-wrapper -->
        <div class="page-wrapper">

            <!-- start preloader -->
            <div class="preloader">
                <div class="load">
                    <div class="gear one">
                        <svg viewbox="0 0 100 100" fill="#94DDFF">
                            <path d="M97.6,55.7V44.3l-13.6-2.9c-0.8-3.3-2.1-6.4-3.9-9.3l7.6-11.7l-8-8L67.9,20c-2.9-1.7-6-3.1-9.3-3.9L55.7,2.4H44.3l-2.9,13.6      c-3.3,0.8-6.4,2.1-9.3,3.9l-11.7-7.6l-8,8L20,32.1c-1.7,2.9-3.1,6-3.9,9.3L2.4,44.3v11.4l13.6,2.9c0.8,3.3,2.1,6.4,3.9,9.3      l-7.6,11.7l8,8L32.1,80c2.9,1.7,6,3.1,9.3,3.9l2.9,13.6h11.4l2.9-13.6c3.3-0.8,6.4-2.1,9.3-3.9l11.7,7.6l8-8L80,67.9      c1.7-2.9,3.1-6,3.9-9.3L97.6,55.7z M50,65.6c-8.7,0-15.6-7-15.6-15.6s7-15.6,15.6-15.6s15.6,7,15.6,15.6S58.7,65.6,50,65.6z"></path>
                        </svg>
                    </div>
                    <div class="gear two">
                        <svg viewbox="0 0 100 100" fill="#ffbd34">
                            <path d="M97.6,55.7V44.3l-13.6-2.9c-0.8-3.3-2.1-6.4-3.9-9.3l7.6-11.7l-8-8L67.9,20c-2.9-1.7-6-3.1-9.3-3.9L55.7,2.4H44.3l-2.9,13.6      c-3.3,0.8-6.4,2.1-9.3,3.9l-11.7-7.6l-8,8L20,32.1c-1.7,2.9-3.1,6-3.9,9.3L2.4,44.3v11.4l13.6,2.9c0.8,3.3,2.1,6.4,3.9,9.3      l-7.6,11.7l8,8L32.1,80c2.9,1.7,6,3.1,9.3,3.9l2.9,13.6h11.4l2.9-13.6c3.3-0.8,6.4-2.1,9.3-3.9l11.7,7.6l8-8L80,67.9      c1.7-2.9,3.1-6,3.9-9.3L97.6,55.7z M50,65.6c-8.7,0-15.6-7-15.6-15.6s7-15.6,15.6-15.6s15.6,7,15.6,15.6S58.7,65.6,50,65.6z"></path>
                        </svg>
                    </div>
                    <div class="gear three">
                        <svg viewbox="0 0 100 100" fill="#0a172b">
                            <path d="M97.6,55.7V44.3l-13.6-2.9c-0.8-3.3-2.1-6.4-3.9-9.3l7.6-11.7l-8-8L67.9,20c-2.9-1.7-6-3.1-9.3-3.9L55.7,2.4H44.3l-2.9,13.6      c-3.3,0.8-6.4,2.1-9.3,3.9l-11.7-7.6l-8,8L20,32.1c-1.7,2.9-3.1,6-3.9,9.3L2.4,44.3v11.4l13.6,2.9c0.8,3.3,2.1,6.4,3.9,9.3      l-7.6,11.7l8,8L32.1,80c2.9,1.7,6,3.1,9.3,3.9l2.9,13.6h11.4l2.9-13.6c3.3-0.8,6.4-2.1,9.3-3.9l11.7,7.6l8-8L80,67.9      c1.7-2.9,3.1-6,3.9-9.3L97.6,55.7z M50,65.6c-8.7,0-15.6-7-15.6-15.6s7-15.6,15.6-15.6s15.6,7,15.6,15.6S58.7,65.6,50,65.6z"></path>
                        </svg>
                    </div>
                </div>
            </div>
            <!-- star services-pg-section -->
            <section class="services-pg-section section-padding">
                <div class="container">
                    <div class="row">
                        <div class="col col-xs-12">
                            <div class="service-grids clearfix">
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-1.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">COKE PLANT,CDQ BUCKET REPLACEMENT.
                                        </a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-box">
                                        <img src="../assets/images/services/Picture2.jpg" alt="">
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">RMHS STACKER WHEEL REPLACEMENT
                                        </a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/Picture3.jpg" alt="">
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">CONVEYOR LINE BELT REPLACEMENT JOB
                                        </a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-4.jpg" alt="">
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Petroleum and Gas</a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-5.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Industrial Cleaning Services</a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-6.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Painting and Protective</a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-1.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Mechanical Engineering</a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-2.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Pharmaceutical Research</a></h3>
                                    </div>
                                </div>
                                <div class="grid">
                                    <div class="img-holder">
                                        <img src="assets/images/services/img-3.jpg" alt>
                                        <div class="view-details">
                                            <a href="#">Get Details</a>
                                        </div>
                                    </div>
                                    <div class="details">
                                        <h3><a href="#">Asbestos Remediation</a></h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- end container -->
            </section>
            <!-- All JavaScript files
        ================================================== -->
            <script src="assets/js/jquery.min.js"></script>
            <script src="assets/js/bootstrap.min.js"></script>

            <!-- Plugins for this template -->
            <script src="assets/js/jquery-plugin-collection.js"></script>

            <!-- Custom script for this template -->
            <script src="assets/js/script.js"></script>
    </body>
</asp:Content>
