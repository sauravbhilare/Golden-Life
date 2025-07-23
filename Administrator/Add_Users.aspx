<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="Add_Users.aspx.cs" Inherits="Administrator_PlaceOrder" EnableEventValidation="false" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="aspajax" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .txtGSTNumber {
            text-transform: uppercase;
        }

            .txtGSTNumber::placeholder {
                text-transform: capitalize;
            }
    </style>
    <script>
        function isNumberKeyDot(evt) {
            var charCode = evt.keyCode;
            if ((charCode == 8 || charCode == 46 || charCode == 9) || (charCode > 47 && charCode < 58))
                return true;
            else
                return false;
        }
        function isNumberKey(evt) {
            debugger;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style>
        .highlight {
            color: black !important;
            list-style-type: none;
            background: #c5bfbf;
            width: 200px !important;
            padding-left: 5px !important;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblUserId" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblOrgid" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblName" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblImage" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblOpeningStock" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblNetTotal" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblCustomerId" runat="server" Text="" Visible="false"></asp:Label>

    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Events & News</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlViewUsers" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/View_Customer.aspx">View Users</asp:HyperLink>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </section>
    <section class="content">
        <div class="container-fluid">
            <div class="card card-default">
                <!-- card-header -->
                <div class="card-header">
                    <h3 class="card-title">Add Events & News</h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <%--<button type="button" class="btn btn-tool" data-card-widget="remove">
                            <i class="fas fa-times"></i>
                          </button>--%>
                    </div>
                </div>
                <!-- card-header -->
                <div class="card-body">

                    <asp:HiddenField ID="hfCustomers" ClientIDMode="Static" runat="server" />
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtHusbandName">Husband Name</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtHusbandName" runat="server" class="form-control" placeholder="Husband Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="red" runat="server" ErrorMessage="Please Enter Husband Name" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtHusbandName" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtWifeName">Wife Name</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtWifeName" runat="server" class="form-control" placeholder="Wife Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Please Enter Wife Name" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtWifeName" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtLastName">Last Name</label>
                                <%--                                <span style="color: red">*</span>--%>
                                <asp:TextBox ID="txtLastName" runat="server" class="form-control" placeholder="Last Name"></asp:TextBox>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="red" runat="server" ErrorMessage="Please Enter Wife Name" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtLastName" />--%>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtHusbandDOB">Husband DOB</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtHusbandDOB" runat="server" class="form-control txtGSTNumber" placeholder="Date" TextMode="Date" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Husband DOB is Required" Display="Dynamic" ValidationGroup="Expense" ControlToValidate="txtHusbandDOB" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtWifeDOB">Wife DOB</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtWifeDOB" runat="server" class="form-control txtGSTNumber" placeholder="Date" TextMode="Date" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="red" runat="server" ErrorMessage="Wife DOB is Required" Display="Dynamic" ValidationGroup="Expense" ControlToValidate="txtWifeDOB" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtAnniversaryDate">Anniversary Date</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtAnniversaryDate" runat="server" class="form-control txtGSTNumber" placeholder="Date" TextMode="Date" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="red" runat="server" ErrorMessage="Anniversary Date is Required" Display="Dynamic" ValidationGroup="Expense" ControlToValidate="txtAnniversaryDate" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Photo:</label>
                                <span style="color: red">*</span>
                                <div class="input-group">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="PhotoFile" runat="server" class="custom-file-input upload" />
                                        <label class="custom-file-label" for="PhotoFile">Choose file</label>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="red" runat="server" ErrorMessage="Photo is Required" Display="Dynamic" ValidationGroup="Expense" ControlToValidate="PhotoFile" />
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="(Note: Image should not be greater than 1MB.)" Style="font-size: 12px;"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtMobile">Mobile</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtMobile" runat="server" class="form-control" placeholder="Mobile" MaxLength="10" onkeypress="return isNumberKey(event)" AutoComplete="off"></asp:TextBox>

                                <!-- Required field validator -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="red" runat="server" ErrorMessage="Please Enter Mobile" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtMobile" />

                                <!-- Regular expression validator for 10 digits -->
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMobile" ErrorMessage="Mobile must be 10 digits" ForeColor="red"
                                    ValidationExpression="^\d{10}$" Display="Dynamic" ValidationGroup="Expense" />
                            </div>
                        </div>

                        <!-- JavaScript function to allow only numbers -->
                        <script type="text/javascript">
                            function isNumberKey(evt) {
                                var charCode = (evt.which) ? evt.which : evt.keyCode;
                                // Allow only numbers (0-9)
                                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                                    return false;
                                }
                                return true;
                            }
                        </script>


                    </div>

                    <!-- Submit Button -->
                    <div class="row justify-content-center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="btnSubmit_Click" CausesValidation="true" ValidationGroup="Expense" class="btn btn-secondary" Style="padding: 6px 24px;" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Expense" ShowMessageBox="True" ShowSummary="False" />
                    </div>


                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <div class="Loader">
                                <div class="lds-roller">
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div class="card-footer">
                </div>
            </div>
        </div>
    </section>

    <script>
        $('.ch1').select2();
        $('.select2-container').css("width", "100%");

        function pageLoad() {
            $('.ch1').select2();
            $('.select2-container').css("width", "100%");
        }
    </script>


    <%--    <script src="https://code.jquery.com/jquery-1.12.4.js">
    </script>--%>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js">
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            let text = $("#hfCustomers").val();
            let data = JSON.parse(text);

            var items = data.Customers;
            $(".autosuggestion").autocomplete({
                source: items,
                classes: {
                    "ui-autocomplete": "highlight"
                }
            });
        })
    </script>



</asp:Content>

