<%@ Page Title="Cloud ERP | Employee Docuemnts" Language="C#" MasterPageFile="~/bussiness/production/webmaster.Master" AutoEventWireup="true" CodeBehind="usertoggle.aspx.cs" Inherits="WebApplication1.bussiness.production.usertoggle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .toggle-bar {
            background-color: #005baa;
            color: white;
            padding: 12px 20px;
            cursor: pointer;
            font-weight: 600;
            border-radius: 8px;
            margin-bottom: 10px;
            font-size: 16px;
        }

            .toggle-bar i {
                float: right;
                margin-top: 4px;
            }

        .toggle-content {
            display: none;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 8px;
            background-color: #f7f9fc;
            margin-bottom: 20px;
            animation: fadeIn 0.3s ease-in-out;
        }

        .education-block {
            border: 1px solid #d3d3d3;
            background-color: #ffffff;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 25px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
        }

        .edu-header {
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 20px;
            color: #003366;
            border-left: 5px solid #007bff;
            padding-left: 10px;
        }

        .form-section {
            display: flex;
            flex-wrap: wrap;
            gap: 30px;
            align-items: flex-start;
            margin-bottom: 20px;
        }

        .image-upload-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            width: 240px;
            gap: 12px;
        }

        .image-preview {
            width: 240px;
            height: 260px;
            background-color: #eef2f7;
            border: 2px dashed #ccc;
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: hidden;
            border-radius: 10px;
        }

            .image-preview img {
                max-width: 100%;
                max-height: 100%;
            }

        .upload-control {
            width: 100%;
            font-size: 13px;
            padding: 6px 8px;
        }

        .form-fields {
            flex: 1;
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 16px;
        }

            .form-fields label {
                font-weight: 500;
            }

        .action-buttons {
            display: flex;
            justify-content: flex-end;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 25px;
        }

            .action-buttons .btn {
                padding: 8px 18px;
                border-radius: 6px;
                font-size: 14px;
            }

        .validation-msg {
            font-size: 12px;
            color: red;
            margin-top: -10px;
        }

        .success-msg {
            color: green;
            font-weight: 500;
            display: block;
            margin-bottom: 10px;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(-10px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* Responsive */
        @media (max-width: 768px) {
            .form-section {
                flex-direction: column;
            }

            .form-fields {
                grid-template-columns: 1fr;
            }

            .action-buttons {
                justify-content: center;
            }

            .toggle-bar {
                font-size: 15px;
                padding: 10px 15px;
            }
        }
        /* Responsive for Mobile */
        @media (max-width: 768px) {
            .form-section {
                flex-direction: column;
            }

            .form-fields {
                grid-template-columns: 1fr;
            }

            .image-upload-wrapper {
                width: 100%;
                align-items: center;
            }
        }

        /* Highlight the textbox border in red */
        /* Highlight invalid textboxes with red border and background */
        .error-border {
            border: 2px solid #e74c3c !important; /* bright red border */
            background-color: #fdecea !important; /* very light red background */
            box-shadow: 0 0 6px #e74c3c; /* subtle red glow */
            transition: background-color 0.3s ease, border-color 0.3s ease;
        }

            /* Optional: On focus remove red background but keep border */
            .error-border:focus {
                background-color: #fff !important;
                border-color: #e74c3c !important;
                outline: none;
                box-shadow: 0 0 8px #e74c3c;
            }


        /* Optional: success message style */
        .success-msg {
            color: #27ae60; /* green text */
            font-weight: 600;
            font-size: 1rem;
            margin-top: 5px;
        }

        .error-highlight {
            border: 2px solid #e74c3c !important;
            background-color: #fceae9;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-sm-12  ">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Document Management</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">


                            <!-- Aadhaar Card Toggle Section -->
                            <div class="toggle-bar" onclick="toggleSection('aadhaarSection')">
                                Aadhaar Card
                                <i class="fa fa-chevron-down" id="icon-aadhaarSection"></i>
                            </div>
                            <asp:ScriptManager ID="ScriptManager1" runat="server" />

                            <!-- Toggle Content -->
                            <div id="aadhaarSection" class="toggle-content">
                                <asp:UpdatePanel ID="UpdatePanelAadhaar" runat="server">
                                    <ContentTemplate>
                                        <div class="form-section">

                                            <!-- Image Preview + Upload -->

                                            <div>
                                                <div class="image-preview" id="aadhaarPreview">
                                                    <img id="imgAadhaarPreview" runat="server" clientidmode="Static" src="../images/placeholder.png" alt="Preview" class="img-fluid" />
                                                </div>
                                                <asp:FileUpload ID="fuAadhaarImage" runat="server" CssClass="form-control mt-2"
                                                    onchange="previewAadhaarImage(this)" />
                                            </div>


                                            <!-- Form Fields -->
                                            <div class="form-fields">
                                                <div>
                                                    <label for="txtAadhaarNo">Aadhaar Card No</label>
                                                    <asp:TextBox ID="txtAadhaarNo" runat="server" CssClass="form-control" MaxLength="12" onkeyup="validateAadhaar()" />
                                                    <span id="aadhaarValidation" class="validation-msg"></span>
                                                </div>

                                                <div>
                                                    <label for="txtAadhaarName">Name as per Aadhaar</label>
                                                    <asp:TextBox ID="txtAadhaarName" runat="server" CssClass="form-control" onkeyup="validateName()" />
                                                    <span id="nameValidation" class="validation-msg"></span>
                                                </div>

                                                <div>
                                                    <label for="txtIssueDate">Issue Date</label>
                                                    <asp:TextBox ID="txtIssueDate" runat="server" TextMode="Date" CssClass="form-control" onchange="validateDate()" />
                                                    <span id="dateValidation" class="validation-msg"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Action Buttons -->
                                        <div class="action-buttons">
                                            <asp:Button ID="btnSubmitAadhaar" runat="server" Text="Submit" CssClass="btn btn-success"
                                                OnClientClick="return validateAllAadhaar();" OnClick="btnSubmitAadhaar_Click" />
                                            <button type="button" class="btn btn-warning" onclick="clearAadhaarForm()">
                                                <i class="fa fa-eraser"></i>Clear
                                            </button>
                                            <button type="button" class="btn btn-secondary" onclick="toggleSection('aadhaarSection')">
                                                <i class="fa fa-times"></i>Cancel
                                            </button>
                                        </div>

                                        <!-- Status Message -->
                                        <asp:Label ID="lblAadhaarStatus" runat="server" CssClass="success-msg" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmitAadhaar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <!-- PAN Card Toggle -->
                            <div class="toggle-bar" onclick="toggleSection('panSection')">
                                PAN Card <i class="fa fa-chevron-down" id="icon_panSection"></i>
                            </div>
                            <div id="panSection" class="toggle-content">
                                <asp:UpdatePanel ID="updPanCard" runat="server">
                                    <ContentTemplate>
                                        <div class="form-section">
                                            <!-- Image Preview + Upload -->
                                            <div>
                                                <div class="image-preview" id="panImagePreview">
                                                    <img id="imgPanPreview" clientidmode="Static" runat="server" src="../images/default-doc.png" alt="PAN Preview" />
                                                </div>
                                                <div class="mt-2">
                                                    <asp:FileUpload ID="fuPANImage" runat="server" CssClass="form-control" onchange="previewImage(this, 'imgPanPreview')" />
                                                </div>
                                            </div>

                                            <!-- PAN Form Fields -->
                                            <div class="form-fields">
                                                <div>
                                                    <label>PAN Card Number</label>
                                                    <asp:TextBox ID="txtPANNumber" runat="server" CssClass="form-control" MaxLength="10" onkeyup="validatePAN(this)" />
                                                    <span id="panNumberValidation" class="validation-msg"></span>
                                                </div>
                                                <div>
                                                    <label>Name on PAN</label>
                                                    <asp:TextBox ID="txtPANName" runat="server" CssClass="form-control" onkeyup="validateNotEmpty(this, 'panNameValidation')" />
                                                    <span id="panNameValidation" class="validation-msg"></span>
                                                </div>
                                                <div>
                                                    <label>Issue Date</label>
                                                    <asp:TextBox ID="txtPANIssueDate" runat="server" CssClass="form-control" TextMode="Date" onchange="validateDate(this, 'panDateValidation')" />
                                                    <span id="panDateValidation" class="validation-msg"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Action Buttons -->
                                        <div class="action-buttons">
                                            <asp:Button ID="btnSubmitPAN" runat="server" Text="&nbsp;Submit" CssClass="btn btn-success"
                                                OnClientClick="return validatePANForm();" OnClick="btnSubmitPAN_Click" />
                                            <asp:Button ID="btnClearPAN" runat="server" Text="&nbsp;Clear" CssClass="btn btn-warning"
                                                OnClientClick="clearPANForm(); return false;" />
                                            <button type="button" class="btn btn-danger" onclick="toggleSection('panSection')">
                                                <i class="fa fa-times"></i>&nbsp;Cancel
                                            </button>
                                        </div>

                                        <asp:Label ID="lblPanMessage" runat="server" CssClass="success-msg" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmitPAN" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <!-- Bank Details Toggle -->
                            <div class="toggle-bar" onclick="toggleSection('bankSection')">
                                Bank Details <i class="fa fa-chevron-down" id="icon_bankSection"></i>
                            </div>
                            <div id="bankSection" class="toggle-content">
                                <asp:UpdatePanel ID="updBankDetails" runat="server">
                                    <ContentTemplate>
                                        <div class="form-section">
                                            <!-- Image Preview + Upload -->
                                            <div>
                                                <div class="image-preview" id="bankImagePreview">
                                                    <img id="imgBankPreview" clientidmode="Static" runat="server" src="../images/default-doc.png" alt="Bank Preview" />
                                                </div>
                                                <div class="mt-2">
                                                    <asp:FileUpload ID="fuBankImage" runat="server" CssClass="form-control" onchange="previewImage(this, 'imgBankPreview')" />
                                                </div>
                                            </div>

                                            <!-- Bank Form Fields -->
                                            <div class="form-fields">
                                                <div>
                                                    <label>Account Number</label>
                                                    <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" MaxLength="18" />
                                                    <span id="bankAccountValidation" class="validation-msg"></span>
                                                </div>
                                                <div>
                                                    <label>IFSC Code</label>
                                                    <asp:TextBox ID="txtIFSC" runat="server" CssClass="form-control" MaxLength="11" />
                                                    <span id="ifscValidation" class="validation-msg"></span>
                                                </div>
                                                <div>
                                                    <label>Bank Name</label>
                                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
                                                    <span id="bankNameValidation" class="validation-msg"></span>
                                                </div>
                                                <div>
                                                    <label>Account Holder Name</label>
                                                    <asp:TextBox ID="txtAccountHolder" runat="server" CssClass="form-control" />
                                                    <span id="accountHolderValidation" class="validation-msg"></span>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Action Buttons -->
                                        <div class="action-buttons">
                                            <asp:Button ID="btnSubmitBank" runat="server" Text="&nbsp;Submit" CssClass="btn btn-success"
                                                OnClientClick="return validateBankForm();" OnClick="btnSubmitBank_Click" />
                                            <asp:Button ID="btnClearBank" runat="server" Text="&nbsp;Clear" CssClass="btn btn-warning"
                                                OnClientClick="clearBankForm(); return false;" />
                                            <button type="button" class="btn btn-danger" onclick="toggleSection('bankSection')">
                                                <i class="fa fa-times"></i>&nbsp;Cancel
                                            </button>
                                        </div>

                                        <asp:Label ID="lblBankMessage" runat="server" CssClass="success-msg" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmitBank" />
                                    </Triggers>

                                </asp:UpdatePanel>
                            </div>



                            <!-- Education Toggle -->
                            <!-- Step 1: Updated ASPX for Education Section with Validation and Unique Image Upload per Section -->
                            <div class="toggle-bar" onclick="toggleSection('educationSection')">
                                Education <i class="fa fa-chevron-down" id="icon_educationSection"></i>
                            </div>
                            <div id="educationSection" class="toggle-content">
                                <asp:UpdatePanel ID="updEducation" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblEducationStatus" runat="server" CssClass="success-msg" />


                                        <div class="education-block">
                                            <div class="edu-header">10th</div>
                                            <div class="form-section">
                                                <div class="image-upload-wrapper">
                                                    <div class="image-preview">
                                                        <img id="img10Preview" runat="server" clientidmode="Static" src="../images/default-doc.png" alt="10th Document" />
                                                    </div>
                                                    <asp:FileUpload ID="fu10Image" runat="server" CssClass="form-control" onchange="previewImage(this, 'img10Preview')" />
                                                </div>
                                                <div class="form-fields">
                                                    <div>
                                                        <label>Board/Institute</label>
                                                        <asp:TextBox ID="txt10Board" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lbl10BoardError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Passout Year</label>
                                                        <asp:TextBox ID="txt10Year" runat="server" CssClass="form-control" TextMode="Number" />
                                                        <asp:Label ID="lbl10YearError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Percentage / CGPA</label>
                                                        <asp:TextBox ID="txt10Marks" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lbl10MarksError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <!-- 12th Block -->
                                        <div class="education-block">
                                            <div class="edu-header">12th</div>
                                            <div class="form-section">
                                                <div class="image-upload-wrapper">
                                                    <div class="image-preview">
                                                        <img id="img12Preview" runat="server" clientidmode="Static" src="../images/default-doc.png" alt="12th Document" />
                                                    </div>
                                                    <asp:FileUpload ID="fu12Image" runat="server" CssClass="form-control upload-control" onchange="previewImage(this, 'img12Preview')" />
                                                </div>
                                                <div class="form-fields">
                                                    <div>
                                                        <label>Board/Institute</label>
                                                        <asp:TextBox ID="txt12Board" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lbl12BoardError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Passout Year</label>
                                                        <asp:TextBox ID="txt12Year" runat="server" CssClass="form-control" TextMode="Number" />
                                                        <asp:Label ID="lbl12YearError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Percentage / CGPA</label>
                                                        <asp:TextBox ID="txt12Marks" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lbl12MarksError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <!-- UG Block with N/A -->

                                        <div class="education-block">
                                            <div class="edu-header">
                                                UG
        <asp:CheckBox ID="chkUGNA" runat="server" Text="N/A" CssClass="na-checkbox"
            AutoPostBack="false" onclick="toggleNA(this, 'txtUGBoard', 'txtUGYear', 'txtUGMarks', 'fuUGImage', 'imgUGPreview')" />
                                            </div>
                                            <div class="form-section">
                                                <div class="image-upload-wrapper">
                                                    <div class="image-preview">
                                                        <img id="imgUGPreview" runat="server" clientidmode="Static" src="../images/default-doc.png" alt="UG Document" />
                                                    </div>
                                                    <asp:FileUpload ID="fuUGImage" runat="server" CssClass="form-control upload-control"
                                                        onchange="previewImage(this, 'imgUGPreview')" />
                                                </div>
                                                <div class="form-fields">
                                                    <div>
                                                        <label>Board/Institute</label>
                                                        <asp:TextBox ID="txtUGBoard" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lblUGBoardError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Passout Year</label>
                                                        <asp:TextBox ID="txtUGYear" runat="server" CssClass="form-control" TextMode="Number" />
                                                        <asp:Label ID="lblUGYearError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Percentage / CGPA</label>
                                                        <asp:TextBox ID="txtUGMarks" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lblUGMarksError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%-- PG Section --%>
                                        <div class="education-block">
                                            <div class="edu-header">
                                                PG
        <asp:CheckBox ID="chkPGNA" runat="server" Text="N/A" CssClass="na-checkbox"
            AutoPostBack="false" onclick="toggleNA(this, 'txtPGBoard', 'txtPGYear', 'txtPGMarks', 'fuPGImage', 'imgPGPreview')" />
                                            </div>
                                            <div class="form-section">
                                                <div class="image-upload-wrapper">
                                                    <div class="image-preview">
                                                        <img id="imgPGPreview" runat="server" clientidmode="Static" src="../images/default-doc.png" alt="PG Document" />
                                                    </div>
                                                    <asp:FileUpload ID="fuPGImage" runat="server" CssClass="form-control upload-control"
                                                        onchange="previewImage(this, 'imgPGPreview')" />

                                                </div>
                                                <div class="form-fields">
                                                    <div>
                                                        <label>Board/Institute</label>
                                                        <asp:TextBox ID="txtPGBoard" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lblPGBoardError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Passout Year</label>
                                                        <asp:TextBox ID="txtPGYear" runat="server" CssClass="form-control" TextMode="Number" />
                                                        <asp:Label ID="lblPGYearError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                    <div>
                                                        <label>Percentage / CGPA</label>
                                                        <asp:TextBox ID="txtPGMarks" runat="server" CssClass="form-control" />
                                                        <asp:Label ID="lblPGMarksError" runat="server" CssClass="validation-msg text-danger" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <%-- Action Buttons --%>
                                        <div class="action-buttons">
                                            <asp:Button ID="btnSubmitEducation" runat="server" Text="Submit" CssClass="btn btn-success"
                                                OnClick="btnSubmitEducation_Click" />
                                            <asp:Button ID="btnClearEducation" runat="server" Text="Clear" CssClass="btn btn-warning"
                                                OnClientClick="clearEducationFields(); return false;" />
                                            <button type="button" class="btn btn-secondary" onclick="toggleSection('educationSection')">Cancel</button>
                                        </div>

                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSubmitEducation" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>




                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function submitAadhaarDetails() {
            const aadhaarNumber = document.getElementById("txtAadhaarNo").value.trim();
            const aadhaarName = document.getElementById("txtAadhaarName").value.trim();
            const issueDate = document.getElementById("txtIssueDate").value.trim();

            fetch("usertoggle.aspx/SaveAadhaarDetails", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    aadhaarNumber: aadhaarNumber,
                    aadhaarName: aadhaarName,
                    issueDate: issueDate
                })
            })
                .then(response => response.json())
                .then(data => {
                    const result = data.d;
                    const label = document.getElementById("lblAadhaarStatus");
                    label.innerHTML = result.message;
                    label.className = result.success ? "success-msg" : "error-msg";
                })
                .catch(err => {
                    const label = document.getElementById("lblAadhaarStatus");
                    label.innerHTML = "Something went wrong.";
                    label.className = "error-msg";
                });
        }
    </script>




    <script>
        function previewImage(input, imgId) {
            const preview = document.getElementById(imgId);
            if (input.files && input.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }


    </script>


    <script>

        function toggleSection(id) {
            const section = document.getElementById(id);
            const icon = document.getElementById("icon-" + id);
            if (section.style.display === "none" || section.style.display === "") {
                section.style.display = "block";
                icon.className = "fa fa-chevron-up";
            } else {
                section.style.display = "none";
                icon.className = "fa fa-chevron-down";
            }
        }

        function previewAadhaarImage(input) {
            const preview = document.getElementById('imgAadhaarPreview');
            if (input.files && input.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function validateAadhaar() {
            const aadhaar = document.getElementById('<%= txtAadhaarNo.ClientID %>').value;
            const msg = document.getElementById('aadhaarValidation');
            msg.innerText = /^[0-9]{12}$/.test(aadhaar) ? "" : "Wrong Aadhaar Card No";
        }

        function validateName() {
            const name = document.getElementById('<%= txtAadhaarName.ClientID %>').value;
            const msg = document.getElementById('nameValidation');
            msg.innerText = /^[a-zA-Z\s]+$/.test(name) ? "" : "Invalid name";
        }

        function validateDate() {
            const date = document.getElementById('<%= txtIssueDate.ClientID %>').value;
                const msg = document.getElementById('dateValidation');
                const today = new Date().toISOString().split("T")[0];
                msg.innerText = (date && date <= today) ? "" : "Invalid or future date";
            }

            function validateAllAadhaar() {
                validateAadhaar();
                validateName();
                validateDate();

                return !document.getElementById('aadhaarValidation').innerText &&
                       !document.getElementById('nameValidation').innerText &&
                       !document.getElementById('dateValidation').innerText;
            }

            function clearAadhaarForm() {
                document.getElementById('<%= txtAadhaarNo.ClientID %>').value = "";
             document.getElementById('<%= txtAadhaarName.ClientID %>').value = "";
             document.getElementById('<%= txtIssueDate.ClientID %>').value = "";
             document.getElementById('<%= fuAadhaarImage.ClientID %>').value = "";
             document.getElementById('imgAadhaarPreview').src = "~/images/placeholder.png";
             document.getElementById('aadhaarValidation').innerText = "";
             document.getElementById('nameValidation').innerText = "";
             document.getElementById('dateValidation').innerText = "";
             document.getElementById('<%= lblAadhaarStatus.ClientID %>').innerText = "";
         }
    </script>



    <script>
        function previewImage(input, imgId) {
            const file = input.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById(imgId).src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        }

        function validatePAN(input) {
            const panPattern = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
            const valMsg = document.getElementById("panNumberValidation");
            if (!panPattern.test(input.value)) {
                valMsg.innerText = "Invalid PAN Number";
                return false;
            } else {
                valMsg.innerText = "";
                return true;
            }
        }

        function validateNotEmpty(input, spanId) {
            const msg = document.getElementById(spanId);
            if (input.value.trim() === "") {
                msg.innerText = "This field is required";
                return false;
            } else {
                msg.innerText = "";
                return true;
            }
        }

        function validateDate(input, spanId) {
            const msg = document.getElementById(spanId);
            const entered = new Date(input.value);
            const today = new Date();
            if (entered > today) {
                msg.innerText = "Date cannot be in the future";
                return false;
            } else {
                msg.innerText = "";
                return true;
            }
        }

        function validatePANForm() {
            const isPANValid = validatePAN(document.getElementById("<%= txtPANNumber.ClientID %>"));
            const isNameValid = validateNotEmpty(document.getElementById("<%= txtPANName.ClientID %>"), "panNameValidation");
            const isDateValid = validateDate(document.getElementById("<%= txtPANIssueDate.ClientID %>"), "panDateValidation");
            return isPANValid && isNameValid && isDateValid;
        }

        function clearPANForm() {
            document.getElementById('<%= txtPANNumber.ClientID %>').value = "";
            document.getElementById('<%= txtPANName.ClientID %>').value = "";
            document.getElementById('<%= txtPANIssueDate.ClientID %>').value = "";
            document.getElementById('<%= fuPANImage.ClientID %>').value = "";

            // Reset image preview to default placeholder
            document.getElementById('imgPanPreview').src = "../images/default-doc.png";

            // Clear validation messages
            document.getElementById('panNumberValidation').innerText = "";
            document.getElementById('panNameValidation').innerText = "";
            document.getElementById('panDateValidation').innerText = "";

            // Clear status label
            document.getElementById('<%= lblPanMessage.ClientID %>').innerText = "";
        }


    </script>

    <script type="text/javascript">
        function clearBankForm() {
            document.getElementById('<%= txtAccountNumber.ClientID %>').value = "";
            document.getElementById('<%= txtIFSC.ClientID %>').value = "";
            document.getElementById('<%= txtBankName.ClientID %>').value = "";
            document.getElementById('<%= txtAccountHolder.ClientID %>').value = "";
            document.getElementById('<%= fuBankImage.ClientID %>').value = "";
            document.getElementById('imgBankPreview').src = "../images/default-doc.png";

            // Clear validation messages
            document.getElementById('bankAccountValidation').innerText = "";
            document.getElementById('ifscValidation').innerText = "";
            document.getElementById('bankNameValidation').innerText = "";
            document.getElementById('accountHolderValidation').innerText = "";
            document.getElementById('<%= lblBankMessage.ClientID %>').innerText = "";
        }

        function validateBankForm() {
            let isValid = true;

            // Validate Account Number
            let acc = document.getElementById('<%= txtAccountNumber.ClientID %>');
            let accMsg = document.getElementById('bankAccountValidation');
            if (acc.value.trim() === "") {
                accMsg.innerText = "Account Number is required.";
                isValid = false;
            } else {
                accMsg.innerText = "";
            }

            // Validate IFSC
            let ifsc = document.getElementById('<%= txtIFSC.ClientID %>');
        let ifscMsg = document.getElementById('ifscValidation');
        const ifscRegex = /^[A-Z]{4}0[A-Z0-9]{6}$/;
        if (!ifscRegex.test(ifsc.value.trim())) {
            ifscMsg.innerText = "Enter a valid IFSC code (e.g., SBIN0001234).";
            isValid = false;
        } else {
            ifscMsg.innerText = "";
        }

            // Validate Bank Name
        let bank = document.getElementById('<%= txtBankName.ClientID %>');
        let bankMsg = document.getElementById('bankNameValidation');
        if (bank.value.trim() === "") {
            bankMsg.innerText = "Bank Name is required.";
            isValid = false;
        } else {
            bankMsg.innerText = "";
        }

            // Validate Account Holder Name
        let holder = document.getElementById('<%= txtAccountHolder.ClientID %>');
        let holderMsg = document.getElementById('accountHolderValidation');
        if (holder.value.trim() === "") {
            holderMsg.innerText = "Account Holder Name is required.";
            isValid = false;
        } else {
            holderMsg.innerText = "";
        }

        return isValid;
    }

    function previewImage(fileInput, imgId) {
        const file = fileInput.files[0];
        const preview = document.getElementById(imgId);
        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
            };
            reader.readAsDataURL(file);
        } else {
            preview.src = "../images/default-doc.png";
        }
    }
    </script>


    <script type="text/javascript">
        // Show selected image in preview <img> tag
        function previewImage(input, imgId) {
            const file = input.files[0];
            const preview = document.getElementById(imgId);
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                };
                reader.readAsDataURL(file);
            } else {
                preview.src = "../images/default-doc.png";
            }
        }

        // Enable/disable all related fields when N/A is checked
        function toggleNA(checkbox, txtBoardId, txtYearId, txtMarksId, fileUploadId, imgPreviewId) {
            const disabled = checkbox.checked;
            const fields = [txtBoardId, txtYearId, txtMarksId, fileUploadId];
            fields.forEach(id => {
                const el = document.getElementById(id);
                if (el) {
                    el.disabled = disabled;
                    if (el.tagName === "INPUT" && el.type === "file" && disabled) {
                        document.getElementById(imgPreviewId).src = "../images/default-doc.png";
                        el.value = ""; // clear file
                    }
                }
            });

            // Clear validations and values
            if (disabled) {
                [txtBoardId, txtYearId, txtMarksId].forEach(id => {
                    const el = document.getElementById(id);
                    if (el) el.value = "";
                    const valSpan = document.getElementById("val" + id.substring(3)); // e.g. valUGYear
                    if (valSpan) valSpan.innerText = "";
                });
            }
        }

        function clearEducationFields() {
            const fieldGroups = [
                { board: "txt10Board", year: "txt10Year", marks: "txt10Marks", file: "fu10Image", preview: "img10Preview", chk: "chk10NA" },
                { board: "txt12Board", year: "txt12Year", marks: "txt12Marks", file: "fu12Image", preview: "img12Preview", chk: "chk12NA" },
                { board: "txtUGBoard", year: "txtUGYear", marks: "txtUGMarks", file: "fuUGImage", preview: "imgUGPreview", chk: "chkUGNA" },
                { board: "txtPGBoard", year: "txtPGYear", marks: "txtPGMarks", file: "fuPGImage", preview: "imgPGPreview", chk: "chkPGNA" }
            ];

            fieldGroups.forEach(group => {
                const chkBox = document.getElementById(group.chk);
                if (chkBox && chkBox.checked) return;

                [group.board, group.year, group.marks].forEach(id => {
                    const input = document.getElementById(id);
                    if (input) {
                        input.value = "";
                        input.classList.remove("error-border");  // <-- Use your class here
                    }
                    const valSpan = document.getElementById("val" + id.substring(3));
                    if (valSpan) valSpan.innerText = "";
                });

                const fileInput = document.getElementById(group.file);
                if (fileInput) fileInput.value = "";

                const previewImg = document.getElementById(group.preview);
                if (previewImg) previewImg.src = "../images/default-doc.png";
            });

            const lblStatus = document.getElementById("lblEducationStatus");
            if (lblStatus) {
                lblStatus.innerText = "";
                lblStatus.className = "";
            }
        }




        //fieldGroups.forEach(group => {
        //    if (document.getElementById(group.chk)?.checked) return;
        //    ["board", "year", "marks"].forEach(key => {
        //        const txt = document.getElementById(group[key]);
        //        if (txt) txt.value = "";
        //        const val = document.getElementById("val" + group[key].substring(3));
        //        if (val) val.innerText = "";
        //    });

        //    const file = document.getElementById(group.file);
        //    if (file) file.value = "";

        //    const img = document.getElementById(group.preview);
        //    if (img) img.src = "../images/default-doc.png";
        //});}

        fieldGroups.forEach(group => {
            // Avoid optional chaining - works in older browsers too
            const chk = document.getElementById(group.chk);
            if (chk && chk.checked) {
                return;
            }

            ["board", "year", "marks"].forEach(key => {
                const controlId = group[key];
                if (!controlId) return; // guard against undefined

                const txt = document.getElementById(controlId);
                if (txt) txt.value = "";

                // build validation span ID: txtXXXX -> valXXXX
                if (controlId.length > 3) {
                    const val = document.getElementById("val" + controlId.substring(3));
                    if (val) val.innerText = "";
                }
            });

            if (group.file) {
                const file = document.getElementById(group.file);
                if (file) file.value = "";
            }

            if (group.preview) {
                const img = document.getElementById(group.preview);
                if (img) img.src = "../images/default-doc.png";
            }
        });


        // Validate not empty field
        function validateNotEmpty(input, valSpanId) {
            const msgSpan = document.getElementById(valSpanId);
            if (!input.value.trim()) {
                msgSpan.innerText = "This field is required.";
            } else {
                msgSpan.innerText = "";
            }
        }

        // Validate year is not in the future and is within reasonable academic range
        function validateYear(input, valSpanId) {
            const msgSpan = document.getElementById(valSpanId);
            const year = parseInt(input.value);
            const currentYear = new Date().getFullYear();
            if (!year || year < 1950 || year > currentYear) {
                msgSpan.innerText = `Enter valid year (1950 - ${currentYear})`;
            } else {
                msgSpan.innerText = "";
            }
        }
    </script>

    <script>
        function toggleNA(chk, txt1, txt2, txt3, fileCtrl, imgCtrl) {
            const isChecked = chk.checked;
            const setValue = val => document.getElementById(val).value = isChecked ? "N/A" : "";

            [txt1, txt2, txt3].forEach(id => {
                const ctrl = document.getElementById(id);
                ctrl.value = isChecked ? "N/A" : "";
                ctrl.disabled = isChecked;
            });

            document.getElementById(fileCtrl).disabled = isChecked;
            if (isChecked) {
                document.getElementById(imgCtrl).src = "../images/default-doc.png";
            }
        }
    </script>

</asp:Content>
