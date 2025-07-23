<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="View_Customer.aspx.cs" Inherits="Administrator_View_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .dropdown-menu.show {
            boder-size: border-box;
            position: inherit;
            transform: translate3d(-75px, 16px, 0px) !important;
            top: 0px;
            left: 0px;
            will-change: transform;
            z-index: 4;
        }

        .orderTable thead tr {
            height: 40px;
        }

        .productTable thead tr {
            color: White !important;
            background-color: #343A40 !important;
            font-weight: bold !important;
        }

            .productTable thead tr th {
                padding-left: 10px;
            }

        .productTable tbody tr td {
            padding-left: 10px;
        }

        .orderTable tbody tr td {
            padding-left: 10px;
        }

        .dropdown-menu.show {
            min-width: 7rem;
        }
    </style>

    <!-- Modal Style -->
    <style>
        /* Specific gradient for the modal */
        .modal-body .gradient-custom {
            /* fallback for old browsers */
            background: #f6d365;
            /* Chrome 10-25, Safari 5.1-6 */
            background: -webkit-linear-gradient(to right bottom, rgba(246, 211, 101, 1), rgba(253, 160, 133, 1));
            /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
            background: linear-gradient(to right bottom, rgba(246, 211, 101, 1), rgba(253, 160, 133, 1));
        }

        /* Adjust the modal height to avoid overflow */
        .modal-body section.vh-100 {
            height: auto;
        }
    </style>


    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label Text="" runat="server" ID="lblOrgId" Visible="false"></asp:Label>
    <asp:Label Text="" runat="server" ID="lblOrderId" Visible="false"></asp:Label>
    <asp:Label Text="" runat="server" ID="lbluserId" Visible="false"></asp:Label>
    <asp:Label Text="" runat="server" ID="lblName" Visible="false"></asp:Label>
    <asp:Label ID="lblPhoto" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblPhoto2" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblSrNo" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label Text="" runat="server" ID="lblprodid" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Users</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlAddUsers" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/Add_Users.aspx">Add Users</asp:HyperLink>
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
                    <h3 class="card-title">Filter</h3>
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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbtnFilter" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <label class="control-label ">Subscription Status :<span style="color: Red"></span></label>
                                    <asp:DropDownList ID="dropType" runat="server" CssClass="ch1 form-control">
                                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <%--<div class="col-sm-3">
                                    <label class="control-label ">Name :<span style="color: Red"></span></label>
                                    <asp:DropDownList ID="ddlStaff" runat="server" CssClass="ch1 form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStaff_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>--%>
                                <div class="col-sm-3">
                                    <label class="control-label ">From Date :<span style="color: Red"></span></label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                                        </div>

                                        <asp:TextBox ID="txtFromDate" runat="server" type="text" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" placeholder="DD/MM/YYYY" data-mask></asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-sm-3">
                                    <label class="control-label ">To Date :<span style="color: Red"></span></label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="far fa-calendar-alt"></i></span>
                                        </div>

                                        <asp:TextBox ID="txtTodate" runat="server" type="text" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" placeholder="DD/MM/YYYY" data-mask></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3" style="margin-top: 31px;">
                                    <asp:LinkButton ID="lbtnFilter" runat="server" class="btn  btn-secondary" Style="width: 100%;" OnClick="lbtnFilter_Click" >Filter</asp:LinkButton>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="card-footer">
                </div>
            </div>
        </div>
    </section>

    <section class="content">
        <div class="container-fluid">
            <div class="card card-default">
                <!-- card-header -->
                <div class="card-header">
                    <h3 class="card-title">User List</h3>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lbtnExport" />
                        </Triggers>
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-sm-12">
                                    <%--  <div class="panel-header">
                                        <h5 class="txt-dark "><i class="zmdi zmdi-comment-text mr-10"></i><b>Customers Details</b> </h5>
                                    </div>--%>
                                    <div class="row justify-content-center">
                                        <div class="col-sm-12">
                                            <div class="card-body">
                                                <div class="">
                                                    <asp:GridView ID="gvCustomers" runat="server" CssClass="tables display dataTable w-100 basic-datatable table-bordered orderTable "
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="Sorry !! No Records Found"
                                                        EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" GridLines="None"
                                                        DataKeyNames="Id" OnRowCommand="gvCustomers_RowCommand" OnRowCreated="gvCustomers_RowCreated" OnRowDataBound="gvCustomers_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="#">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsdafdfrno" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Photo">
                                                                <ItemTemplate>
                                                                    <a href='../Attachment/UserImg/<%# Eval("Photo")%>' target="_blank">
                                                                        <img src='../Attachment/UserImg/<%# Eval("Photo")%>' style="font-size: x-small; width: 50px; height: 50px" />
                                                                    </a>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Husband Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHusbandName" runat="server" Text='<%# Eval("Name") + " " +  Eval("Last_Name")%>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Wife Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWifeName" runat="server" Text='<%# Eval("Spouse_Name") + " " +  Eval("Last_Name") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Mobile">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMobile" runat="server" Text='<%# Eval("Mobile") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Subscription Start">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStart" runat="server" Text='<%# Eval("Sub_Valid_From", "{0:dd/MM/yyyy}") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Subscription End">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEnd" runat="server" Text='<%# Eval("Sub_Valid_Till", "{0:dd/MM/yyyy}") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="btn-group-vertical">
                                                                        <div class="btn-group">
                                                                            <asp:LinkButton ID="llkbutton" runat="server" data-toggle="dropdown" Style="text-decoration: none;"><i class="fa fa-ellipsis-v" aria-hidden="true" style=" color:Black;"></i>
                                                                            </asp:LinkButton>
                                                                            <ul class="dropdown-menu" style="width: 135px;">
                                                                                <li style="font-size: 17px;">
                                                                                    <asp:LinkButton ID="LinkButtonView" runat="server" CssClass=" p-2" CommandArgument='<%# Eval("Id")%>' CommandName="ViewUsers" CausesValidation="false" Style="font-size: 17px; color: Black;">          
                                                                                <i class="fa-solid fa-eye"></i> View Details</asp:LinkButton>
                                                                                </li>
                                                                                <li style="font-size: 17px;">
                                                                                    <asp:LinkButton ID="LinSub" runat="server" CssClass=" p-2" CommandArgument='<%# Eval("Id")%>' CommandName="Subscription" CausesValidation="false" Style="font-size: 17px; color: Black;">          
                                                                                <i class="fa-solid fa-coins"></i> Subscription</asp:LinkButton>
                                                                                </li>
                                                                                <li style="font-size: 17px;">
                                                                                    <asp:LinkButton ID="LinkButtonsummery" runat="server" CssClass=" p-2" CommandArgument='<%# Eval("Id")%>' CommandName="customeredit" CausesValidation="false" Style="font-size: 17px; color: Black;">          
                                                                                <i class="fa-solid fa-edit"></i> Edit</asp:LinkButton>
                                                                                </li>
                                                                                <li style="font-size: 17px;" runat="server" id="liDelete">
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass=" p-2" OnClientClick="return confirm('Do you want to delete this User ?');" CommandArgument='<%# Eval("Id")%>' CommandName="Deletecustomer" CausesValidation="false" Style="font-size: 17px; color: red;">          
                                                                                <i class="fa-solid fa-trash"></i> Delete</asp:LinkButton>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#343a40" Font-Bold="True" ForeColor="White"
                                                            HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" HorizontalAlign="Left" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"
                                                            HorizontalAlign="Center" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    </asp:GridView>

                                                </div>
                                            </div>
                                        </div>
                                        <asp:LinkButton ID="lbtnExport" CausesValidation="false" UseSubmitBehavior="false" runat="server" Visible="false"
                                            class="btn btn-secondary btn-icon left-icon mr-10" Style="margin-top: 5px;" OnClick="btnexport_Click">Export to Excel</asp:LinkButton>
                                    </div>

                                </div>
                            </div>

                            <!-- Modal -->
                            <div class="modal fade" id="userDetailsModal" tabindex="-1" aria-labelledby="userDetailsModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="userDetailsModalLabel">User Details</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                            <asp:Label ID="lblMessage" runat="server" Visible="false" Style="color: red; font-weight: 500;"></asp:Label>
                                            <section class="" style="background-color: #f4f5f7;" runat="server" id="modalSection">

                                                <div class="row d-flex justify-content-center align-items-center">
                                                    <div class="col col-lg-12 mb-4 mb-lg-0">

                                                        <div class="row g-0">
                                                            <div class="col-md-4 gradient-custom text-center text-white"
                                                                style="border-top-left-radius: .5rem; border-bottom-left-radius: .5rem;">
                                                                <img id="userImage" runat="server" alt="Avatar" class="img-fluid my-5" style="width: 110px;" />
                                                                <p>
                                                                    Mr.
                                                                    <asp:Label ID="lblHusband" runat="server">-</asp:Label>
                                                                </p>
                                                                <p>
                                                                    Mrs.
                                                                    <asp:Label ID="lblWife" runat="server">-</asp:Label>
                                                                </p>
                                                            </div>
                                                            <div class="col-md-8">
                                                                <div class="card-body p-4">
                                                                    <h6><b>Personal Information</b></h6>
                                                                    <hr class="mt-0 mb-4">
                                                                    <div class="row pt-1">
                                                                        <div class="col-6">
                                                                            <h6>Husband DOB</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblHDOB" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <h6>Wife DOB</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblWDOB" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row pt-1">
                                                                        <div class="col-6">
                                                                            <h6>Anniversary Date</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblAnniversaryDate" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <h6>Mobile</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblMobile" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>
                                                                    <h6><b>Plan Details</b></h6>
                                                                    <hr class="mt-0 mb-4">
                                                                    <div class="row pt-1">
                                                                        <div class="col-6 mb-3">
                                                                            <h6>Valid From</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblValidFrom" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="col-6 mb-3">
                                                                            <h6>Valid Till</h6>
                                                                            <p class="text-muted">
                                                                                <asp:Label ID="lblValidTill" runat="server">-</asp:Label>
                                                                            </p>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </section>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <script>
                                var prm = Sys.WebForms.PageRequestManager.getInstance();
                                if (prm != null) {
                                    prm.add_endRequest(function (sender, e) {
                                        if (sender._postBackSettings.panelsToUpdate != null) {

                                            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'DD/MM/YYYY' })
                                            //Money Euro
                                            $('[data-mask]').inputmask();
                                            datatable();

                                        }
                                    });
                                };


                                $(function () {
                                    //Datemask dd/mm/yyyy
                                    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'DD/MM/YYYY' })
                                    //Money Euro
                                    $('[data-mask]').inputmask()
                                    datatable();
                                })
                            </script>

                            <script type="text/javascript">
                                function datatable() {
                                    $(document).ready(function () {
                                        $(".basic-datatable").DataTable({

                                        });
                                    });
                                }
                            </script>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </section>

    <!-- Modal -->
    <div class="modal fade" id="SubscriptionModal" tabindex="-1" aria-labelledby="userDetailsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="upSubscriptPlan" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSubUpdate" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title">Update Subscriptions</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtName">Add Subscription</label>
                                        <asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="Title" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtMeasurement">Subscription Plan</label>
                                        <asp:DropDownList ID="ddlSubplan" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSubplan_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Please select Subscription Plan" Display="None" ValidationGroup="Expense"
                                            InitialValue="-1" ControlToValidate="ddlSubplan">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtName">Amount</label>
                                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" placeholder="Amount" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="txtMeasurement">Payment Status</label>

                                        <asp:DropDownList ID="ddlPaymentStatus1" runat="server" CssClass="form-control" Enabled="false" OnSelectedIndexChanged="ddlPaymentStatus_SelectedIndexChanged1" AutoPostBack="true">
                                            <asp:ListItem Text="Unpaid" Value="Unpaid"></asp:ListItem>
                                            <asp:ListItem Text="Paid" Value="Paid" Selected></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Type is Required" Display="None" ValidationGroup="Expense"
                                                            InitialValue="-1" ControlToValidate="ddlType">
                                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-md-4" runat="server" id="paytypeDiv" visible="true">
                                    <div class="form-group">
                                        <label for="txtMeasurement">Payment Type</label>

                                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                            <asp:ListItem Text="Online" Value="Online"></asp:ListItem>
                                            <%--<asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Type is Required" Display="None" ValidationGroup="Expense"
                                                            InitialValue="-1" ControlToValidate="ddlType">
                                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-md-4" runat="server" id="TransactionIdDiv" visible="false">
                                    <div class="form-group">
                                        <label for="txtName">Transaction Id</label>
                                        <asp:TextBox ID="txtTransactionID" runat="server" class="form-control" placeholder="Transaction Id"></asp:TextBox>
                                    </div>
                                </div>
                                <%--  <div class="col-md-4" runat="server" id="paymentIdDiv" visible="false">
                                    <div class="form-group">
                                        <label for="txtName">Payment Id</label>
                                        <asp:TextBox ID="txtPaymentId" runat="server" class="form-control" placeholder="Payment Id"></asp:TextBox>
                                    </div>
                                </div>--%>
                                <div></div>
                                <div class="col-md-4" runat="server" id="BankNameDiv" visible="false">
                                    <div class="form-group">
                                        <label for="txtName">Bank Name</label>
                                        <asp:TextBox ID="txtBankName" runat="server" class="form-control" placeholder="Bank Name"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4" runat="server" id="ChqIdDiv" visible="false">
                                    <div class="form-group">
                                        <label for="txtName">Cheque Id</label>
                                        <asp:TextBox ID="txtChqId" runat="server" class="form-control" placeholder="Cheque Id"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSubUpdate" runat="server" CssClass="btn btn-secondary" Text="Update" OnClick="btnSubUpdate_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>
        </div>
    </div>

</asp:Content>

