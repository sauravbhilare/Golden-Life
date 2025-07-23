<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="AddBanner.aspx.cs" Inherits="Administrator_PlaceOrder" EnableEventValidation="false" %>

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
    <asp:Label ID="lblBannerId" runat="server" Text="" Visible="false"></asp:Label>

    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Banners</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlViewOrder" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/View_Banner.aspx">View Banner</asp:HyperLink>
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
                    <h3 class="card-title">Add Banner</h3>
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
                            <asp:UpdatePanel ID="up2" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label for="txtMeasurement">Type</label>
                                                <span style="color: red">*</span>
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Banner" Value="Banner"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Type is Required" Display="None" ValidationGroup="Expense"
                                                    InitialValue="-1" ControlToValidate="ddlType">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label ">
                                    Image :</label><%-- <span style="color: red">*</span>--%>
                                <div class="input-group">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="ImageFile" runat="server" class="custom-file-input upload" />
                                        <label class="custom-file-label" for="fuRoomImage">Choose file</label>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="red" runat="server" ErrorMessage="Please Upload Image" Display="None" ValidationGroup="Expense"
                                            InitialValue="-1" ControlToValidate="ImageFile">
                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="(Note : Image should not be greater than 1MB.)" Style="font-size: 12px;"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <asp:UpdatePanel ID="up3" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label for="txtMeasurement">Is Redirect</label>

                                                <asp:DropDownList ID="ddlRedirect" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlRedirect_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                                <%--      <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="red" runat="server" ErrorMessage="" Display="None" ValidationGroup="Expense"
                                            InitialValue="-1" ControlToValidate="ddlRedirect">
                                        </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-4">
                            <asp:UpdatePanel ID="up4" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12" runat="server" id="URLDiv" visible="false">
                                            <div class="form-group">
                                                <label for="txtMeasurement">URL</label>
                                                <asp:TextBox ID="txtUrl" runat="server" class="form-control txtGSTNumber" placeholder="URL"></asp:TextBox>
                                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Date is Required" Display="None" ValidationGroup="Expense"
                                            ControlToValidate="txtdate">
                                        </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </div>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row justify-content-center">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="btnSubmit_Click" CausesValidation="true" ValidationGroup="Expense" class="btn  btn-secondary" Style="padding: 6px 24px;" />
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Expense" ShowMessageBox="True" ShowSummary="False" />
                                <div class="clearfix">
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

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
    <div class="modal fade" id="customerModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAddCutomer" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="productModalLabel">Add Customer</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>

                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtName">Customer Name</label>
                                        <span style="color: red">*</span>
                                        <asp:TextBox ID="txtName" runat="server" class="form-control autosuggestion" placeholder="Name"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="red" runat="server" ErrorMessage="Customer is Required" Display="None" ValidationGroup="Customer"
                                            ControlToValidate="txtName">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtmobile">Mobile</label>
                                        <asp:TextBox ID="txtmobile" runat="server" class="form-control" MaxLength="10" onkeypress="return isNumberKey(event);" placeholder="Mobile Number" AutoComplete="off"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Mobile Number is Required" Display="None" ValidationGroup="Customer" ControlToValidate="txtmobile"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="txtQuantity">Address</label>
                                        <asp:TextBox ID="txtAddrress" runat="server" class="form-control" placeholder="Billing Address" AutoComplete="off" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <%-- <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtQuantity">Shipping Address</label>
                                        <asp:TextBox ID="txtShippingAddress" runat="server" class="form-control" placeholder="Shipping Address" AutoComplete="off" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row justify-content-center">
                                <asp:Button ID="btnAddCutomer" runat="server" Text="Add Customers" UseSubmitBehavior="true" OnClick="btnAddCutomer_Click" CausesValidation="true" ValidationGroup="Customer" class="btn  btn-secondary" Style="padding: 6px 24px;" />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Customer" ShowMessageBox="True" ShowSummary="False" />
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

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

