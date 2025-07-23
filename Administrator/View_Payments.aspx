<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="View_Payments.aspx.cs" Inherits="Administrator_View_Customer" %>

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
    <asp:Label Text="" runat="server" ID="lblPaymentId" Visible="false"></asp:Label>
    <asp:Label Text="" runat="server" ID="lblprodid" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Payment</h1>
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
                                    <label class="control-label ">Payment Status :<span style="color: Red"></span></label>
                                    <asp:DropDownList ID="dropType" runat="server" CssClass="ch1 form-control">
                                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
                                        <asp:ListItem Text="Unpaid" Value="Unpaid"></asp:ListItem>
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
                                    <asp:LinkButton ID="lbtnFilter" runat="server" class="btn  btn-secondary" Style="width: 100%;" OnClick="lbtnFilter_Click">Filter</asp:LinkButton>
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
                    <h3 class="card-title">Payment List</h3>
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
                                        <h5 class="txt-dark "><i class="zmdi zmdi-comment-text mr-10"></i><b>Payments Details</b> </h5>
                                    </div>--%>
                                    <div class="row justify-content-center">
                                        <div class="col-sm-12">
                                            <div class="card-body">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="gvPayments" runat="server" CssClass="tables display dataTable w-100 basic-datatable table-bordered orderTable "
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="Sorry !! No Records Found"
                                                        EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" GridLines="None"
                                                        DataKeyNames="Sr_No" OnRowCommand="gvPayments_RowCommand" OnRowCreated="gvPayments_RowCreated" OnRowDataBound="gvPayments_RowDataBound">

                                                        <Columns>

                                                            <asp:TemplateField HeaderText="#">
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsdafdfrno" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Name">
                                                                <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Plan">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPlan" runat="server" Text='<%# Eval("Plan_Name") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Payment Date">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaidOn" runat="server" Text='<%# Eval("PaidOn", "{0:dd/MM/yyyy}") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--                                                            <asp:TemplateField HeaderText="Cnv. Fee">
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblConvenienceFee" runat="server" Text='<%# Eval("Convenience_Fee") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>


                                                            <asp:TemplateField HeaderText="Total Amount">
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Transaction Id">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTxnId" runat="server" Text='<%# Eval("Txn_Id") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Payment Id">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentId" runat="server" Text='<%# Eval("Payment_Id") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPaymentStatus" runat="server" Text='<%# Eval("Status") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div class="btn-group-vertical">
                                                                        <div class="btn-group">
                                                                            <asp:LinkButton ID="llkbutton" runat="server" data-toggle="dropdown" Style="text-decoration: none;"><i class="fa fa-ellipsis-v" aria-hidden="true" style=" color:Black;"></i>
                                                                            </asp:LinkButton>
                                                                            <ul class="dropdown-menu">
                                                                                <li style="font-size: 17px;">
                                                                                    <asp:LinkButton ID="LinkButtonsummery" runat="server" CssClass="p-2" CommandArgument='<%# Eval("Sr_No")%>' CommandName="UpdatePayment" CausesValidation="false" Style="font-size: 17px; color: Black;">          
                                <i class="fa-solid fa-edit"></i> Edit</asp:LinkButton>
                                                                                </li>
                                                                                <li style="font-size: 17px;" runat="server" id="liDelete">
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="p-2" OnClientClick="return confirm('Do you want to delete this order?');" CommandArgument='<%# Eval("Sr_No")%>' CommandName="Deletecustomer" CausesValidation="false" Style="font-size: 17px; color: red;">          
                                <i class="fa-solid fa-trash"></i> Delete</asp:LinkButton>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#343a40" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" HorizontalAlign="Left" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
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
    <div class="modal fade" id="upatePayment" tabindex="-1" aria-labelledby="userDetailsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpPAyment" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpdate" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="userDetailsModalLabel">Payment Update</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtName">User Name</label>
                                        <asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="Title" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtName">Transaction Id</label>
                                        <asp:TextBox ID="txtTransactionId" runat="server" class="form-control" placeholder="Title" ReadOnly></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtName">Payment Id</label>
                                        <asp:TextBox ID="txtPaymentId" runat="server" class="form-control" placeholder="Payment Id"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtMeasurement">Payment Status</label>

                                        <asp:DropDownList ID="ddlPaymentStatus" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPaymentStatus_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Unpaid" Value="Unpaid"></asp:ListItem>
                                            <asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Type is Required" Display="None" ValidationGroup="Expense"
                                                            InitialValue="-1" ControlToValidate="ddlType">
                                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-secondary" OnClick="btnUpdate_Click" Text="Update" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#btnUpdate').click(function (e) {
                var paymentStatus = $('#<%= ddlPaymentStatus.ClientID %>').val();
                var paymentId = $('#<%= txtPaymentId.ClientID %>').val();

                if (paymentStatus === "Paid" && !paymentId) {
                    e.preventDefault(); // Prevent the form from submitting
                    alert('Payment ID is required when status is Paid.'); // Show alert
                    return false; // Stop further execution
                }
            });
        });
    </script>

</asp:Content>

