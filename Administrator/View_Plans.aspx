<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="View_Plans.aspx.cs" Inherits="Administrator_View_Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblOrgid" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblName" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblId" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblPlanId" runat="server" Text="" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Plans</h1>
                </div>
                <%--   <div class="col-sm-1 float-sm-right">
                    <asp:HyperLink ID="hlAddTickets" runat="server" class="btn btn-sm bg-gradient-dark pt-2" NavigateUrl="~/Administrator/AddEventNews.aspx">+Add vent & News</asp:HyperLink>
                </div>--%>
            </div>
        </div>
        <!-- /.container-fluid -->
    </section>
    <%--    <section class="content">
        <div class="container-fluid">
            <div class="card card-default">
                <div class="card-header">
                    <h3 class="card-title">Filter</h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                            <i class="fas fa-minus"></i>
                        </button>

                    </div>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbtnFilter" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <label class="control-label ">Type :<span style="color: Red"></span></label>
                                    <asp:DropDownList ID="dropType" runat="server" CssClass="ch1 form-control"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Event" Value="Event"></asp:ListItem>
                                        <asp:ListItem Text="News" Value="News"></asp:ListItem>
                                    </asp:DropDownList>

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
    </section>--%>
    <section class="content">
        <div class="container-fluid">
            <div class="card card-default">
                <!-- card-header -->
                <div class="card-header">
                    <h3 class="card-title">Plan Lists</h3>
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
                    <div class="row">
                        <div class="col-md-12" style="margin-top: -20px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvPlan" EventName="RowCommand" />
                                    <asp:PostBackTrigger ControlID="btnexport" />
                                 
                                </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-wrap">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="card-body">
                                                            <div class="">
                                                                <asp:GridView ID="gvPlan" DataKeyNames="Sr_No" runat="server" AllowPaging="false" EmptyDataText="Sorry !! No Records Found"
                                                                    ShowHeader="True" ShowHeaderWhenEmpty="True" EmptyDataRowStyle-ForeColor="Red"
                                                                    EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center" AutoGenerateColumns="false"
                                                                    class="table table-bordered table-striped basic-datatable" OnRowCommand="gvPlan_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="#">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSrNo" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Plan Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPlanName" runat="server" Text='<%# Eval("Plan_Name")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Description">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                      <%--  <asp:TemplateField HeaderText="Convenience Fee">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblConvenienceFeee" runat="server" Text='<%# Eval("Convenience_Fee")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderText="Total Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Operation">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbtnToggle" runat="server" class="btn dropdown-icon" data-toggle="dropdown" Style="text-decoration: none;">
                                                                                <i class="fa fa-ellipsis-v" aria-hidden="true" style=" color:Black;"></i>
                                                                                </asp:LinkButton>
                                                                                <div class="dropdown-menu Action">
                                                                                    <asp:LinkButton ID="LinkButtonEdit" runat="server" CssClass="dropdown-item"
                                                                                        CommandArgument='<%# Eval("Sr_No")%>' OnClientClick="$('#UpdateProgress1').show();"
                                                                                        CommandName="Editrow" CausesValidation="false">Edit
                                                                                    </asp:LinkButton>
                                                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" OnClientClick="return confirm('You Will Not be Able to Revert This!')"
                                                                                        CssClass=" dropdown-item" CommandArgument='<%# Eval("Sr_No")%>'
                                                                                        CommandName="Deleterow" CausesValidation="false">Delete
                                                                                    </asp:LinkButton>

                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 form-actions">
                                                        <center>
                                                            <asp:Button ID="btnexport" Visible="false" CausesValidation="false" UseSubmitBehavior="false" runat="server"
                                                                Text="Export to Excel" class="btn btn-secondary btn-icon left-icon mr-10" OnClick="btnexport_Click" ></asp:Button>
                                                        </center>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /Row -->


                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                </div>
            </div>
        </div>
    </section>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.26.0/moment.min.js"></script>
    <script src="https://cdn.datatables.net/plug-ins/1.10.25/sorting/datetime-moment.js"></script>
    <script>
        function ShowTicketSummary() {
            $("#ticketSummary").modal('show');
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'DD/MM/YYYY' })
            //Money Euro
            $('[data-mask]').inputmask();

            $('.basic-datatable').DataTable({
                "columnDefs": [{ orderable: false, targets: -1 }]
            });
            $('.ch1').select2();
        });
        debugger;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {

                    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'DD/MM/YYYY' })
                    //Money Euro
                    $('[data-mask]').inputmask();


                    $('.ch1').select2();
                }
            });
        };
        function datatable() {
            $('.basic-datatable').DataTable({
                "columnDefs": [{ orderable: false, targets: -1 }]
            });
        }
    </script>

    <!-- Modal -->
    <div class="modal fade" id="PlanModal" tabindex="-1" aria-labelledby="userDetailsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpPlan" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="userDetailsModalLabel">User Details</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtName">Plan Name</label>
                                        <asp:TextBox ID="txtPlanName" runat="server" class="form-control" placeholder="Plan Name"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtName">Amount</label>
                                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" placeholder="Amount"
                                            oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1')"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtName">Convenience Fee</label>
                                        <asp:TextBox ID="txtConvenienceFee" runat="server" class="form-control" placeholder="Convenience Fee"
                                            oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1')"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtName">Total Amount</label>
                                        <asp:TextBox ID="txtTotalAmount" runat="server" class="form-control" placeholder="Total Amount"
                                            oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1')"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="txtName">Discription</label>
                                        <asp:TextBox ID="txtDiscription" runat="server" class="form-control" placeholder="Discription"></asp:TextBox>
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
</asp:Content>



