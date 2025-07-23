<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="ClubFormation.aspx.cs" Inherits="Administrator_View_Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblOrgid" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblName" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblUserId" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblEventId" runat="server" Text="" Visible="false"></asp:Label>

    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Club Formation</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlAddTickets" runat="server" class="btn btn-sm bg-gradient-dark pt-2" NavigateUrl="~/Administrator/View_Event_News.aspx">View Event & News</asp:HyperLink>
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
                    <h3 class="card-title">Event Details</h3>
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
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="ddlFolder">Event Title</label>
                                        <asp:DropDownList ID="ddlEventTitle" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlEventTitle_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtTitle">Capacity</label>
                                        <asp:TextBox ID="TxtCapacity" runat="server" class="form-control txtGSTNumber" placeholder="Capacity"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Date is Required" Display="None" ValidationGroup="Expense"
                                            ControlToValidate="txtdate">
                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="txtTitle">Location(Address)</label>
                                        <asp:TextBox ID="txtLocation" runat="server" class="form-control txtGSTNumber" placeholder="Location"></asp:TextBox>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Date is Required" Display="None" ValidationGroup="Expense"
                                            ControlToValidate="txtdate">
                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="col-sm-12 text-center">
                                    <asp:LinkButton ID="lbtnFilter" runat="server" class="btn  btn-secondary" Style="width: 20%;">Create Club</asp:LinkButton>
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
                    <h3 class="card-title">Club Formation Users List</h3>
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
                                    <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="RowCommand" />
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
                                                                <asp:GridView ID="gvUsers" DataKeyNames="Id" runat="server" AllowPaging="false" EmptyDataText="Sorry !! No Records Found"
                                                                    ShowHeader="True" ShowHeaderWhenEmpty="True" EmptyDataRowStyle-ForeColor="Red"
                                                                    EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center" AutoGenerateColumns="false"
                                                                    class="table table-bordered table-striped basic-datatable" OnRowCommand="gvUsers_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="#">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSrNo" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="chkSelectAll" runat="server" OnClick="selectAllCheckboxes(this)" />
                                                                                <label for="chkSelectAll">All</label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Type">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblusernme" runat="server" Text='<%# Eval("Husband_Name")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Title">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSpouseName" runat="server" Text='<%# Eval("Wife_Name")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Title">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("Last_Name")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>


                                                                        <asp:TemplateField HeaderText="Operation" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbtnToggle" runat="server" class="btn dropdown-icon" data-toggle="dropdown" Style="text-decoration: none;">
                                                                                <i class="fa fa-ellipsis-v" aria-hidden="true" style=" color:Black;"></i>
                                                                                </asp:LinkButton>
                                                                                <div class="dropdown-menu Action">
                                                                                    <asp:LinkButton ID="LinkButtonEdit" runat="server" CssClass="dropdown-item"
                                                                                        CommandArgument='<%# Eval("Id")%>' OnClientClick="$('#UpdateProgress1').show();"
                                                                                        CommandName="Editrow" CausesValidation="false">Edit
                                                                                    </asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkClubFormation" runat="server" CssClass="dropdown-item"
                                                                                        CommandArgument='<%# Eval("Id")%>' OnClientClick="$('#UpdateProgress1').show();"
                                                                                        CommandName="Formationrow" CausesValidation="false">Club Formation
                                                                                    </asp:LinkButton>
                                                                                    <asp:LinkButton ID="LinkButtonDelete" runat="server" OnClientClick="return confirm('You Will Not be Able to Revert This!')"
                                                                                        CssClass=" dropdown-item" CommandArgument='<%# Eval("Id")%>'
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
    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <script type="text/javascript">
        function selectAllCheckboxes(source) {
            var checkboxes = document.querySelectorAll('[id*=chkSelect]');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
</script>
</asp:Content>



