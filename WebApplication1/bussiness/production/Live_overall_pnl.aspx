<%@ Page Title="" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="Live_overall_pnl.aspx.cs" Inherits="WebApplication1.bussiness.production.Live_overall_pnl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://unpkg.com/@superset-ui/embedded-sdk"></script>

    <style type="text/css">
        /* Ensure the container takes up the necessary space */
        #superset-container {
            width: 100%;
            height: 800px; /* Adjust height as needed */
            border: 1px solid #ddd;
            border-radius: 4px;
        }

            /* The SDK generates an iframe inside the container, strip its borders */
            #superset-container iframe {
                border: none;
                width: 100%;
                height: 100%;
            }

        /* Desktop Default */
        #superset-container {
            width: 100%;
            height: 800px;
            border: 1px solid #ddd;
            border-radius: 4px;
            overflow: hidden;
        }

        /* Mobile & Tablet Enhancements (Screens smaller than 768px) */
        @media (max-width: 767px) {
            #superset-container {
                height: 1200px; /* Increase height on mobile to allow stacked charts to breathe */
                border: none;
            }

                /* Optional: If charts still look too small, you can slightly scale the iframe */
                #superset-container iframe {
                    width: 100%;
                    height: 100%;
                    transform-origin: 0 0;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-3 col-sm-12 col-lg-12 form-group">

                                    <div id="superset-container">
                                        <div style="padding: 20px; text-align: center; color: #666;">
                                            Loading Dashboard...
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <%--button start--%>
                            <div class="col-md-6 center-margin" id="buttons" runat="server" visible="false">
                                <div class="ln_solid"></div>
                                <div class="item form-group row">
                                    <div class="col-md-6 col-sm-12">
                                        <asp:Label ID="lbl_msg" runat="server" Text="Click SUBMIT to Save Data!!"></asp:Label>
                                    </div>
                                    <div class="col-md-6 col-sm-12">
                                        <button type="button" class="btn btn-danger btn-sm collapse-link">Cancel</button>
                                        <button type="reset" class="btn btn-warning btn-sm">Reset</button>
                                        <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-success btn-sm" OnClientClick="return ValidateFormField()" />
                                    </div>
                                </div>
                            </div>
                            <%--button end--%>

                            <asp:Button ID="ShowPopup" runat="server" Text="Button" class="btn btn-primary" Visible="false" data-toggle="modal" data-target=".bs-example-modal-sm" />
                            <div id="MyPopup" class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-hidden="true">
                                <div class="modal-dialog modal-sm">
                                    <div class="modal-content">

                                        <div class="modal-header">
                                            <h4 class="modal-title" id="myModalLabel2"></h4>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">×</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-md-12 col-sm-12" id="grid" runat="server" visible="false">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>View and Manage : Work Order Data</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="card-box table-responsive">
                                        <span>Hello, How are you?</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
        
        // TODO: Paste the Embedded ID you generated in the Superset Admin UI here
        const myEmbeddedId = "af136682-b041-4765-acbb-67ae85467c11"; 
        
        const supersetDomain = "https://reports.aminruptechnologies.co.in";

        async function fetchGuestTokenFromBackend() {
            try {
                // Call the C# WebMethod in RolesOverview.aspx.cs
                const response = await fetch('Live_overall_pnl.aspx/FetchSupersetGuestToken', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: "{}" 
                });

                // Check if the server returned a 500, 404, or 401 error before trying to parse JSON
                if (!response.ok) {
                    console.error(`HTTP error! Status: ${response.status}`);
                    throw new Error(`Server responded with status ${response.status}`);
                }

                const data = await response.json();
       
                if (data.d && !data.d.includes("error")) {
                    return data.d; 
                } else {
                    console.error("Error from backend:", data.d);
                    throw new Error("Failed to retrieve token: " + data.d);
                }
            } catch (error) {
                console.error("Token fetch failed:", error);
                document.getElementById('superset-container').innerHTML = "<div style='padding: 20px; color: red;'>Failed to load dashboard authorization. Please check the browser console.</div>";
                return null;
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            
            supersetEmbeddedSdk.embedDashboard({
                id: myEmbeddedId,
                supersetDomain: supersetDomain,
                mountPoint: document.getElementById("superset-container"),
                fetchGuestToken: () => fetchGuestTokenFromBackend(),
                dashboardUiConfig: {
                    hideTitle: true,           // Hides the Dashboard Header
                    hideChartControls: false,   // SET TO TRUE: This removes the entire 3-dot menu from all charts
                    hideTab: false,             // Keeps the tabs visible if you have multiple pages
                    filters: {
                        visible: false,      // This hides the filter bar initially
                        expanded: false      // This ensures it is collapsed
                    }
                }
            });
            
        });
    </script>
</asp:Content>
