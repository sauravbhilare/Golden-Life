<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="View_Event_News.aspx.cs" Inherits="Administrator_View_Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblOrgid" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblName" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblId" runat="server" Text="" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Events & News</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlAddTickets" runat="server" class="btn btn-sm bg-gradient-dark pt-2" NavigateUrl="~/Administrator/AddEventNews.aspx">+Add Event & News</asp:HyperLink>
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
                                    <label class="control-label ">Type :<span style="color: Red"></span></label>
                                    <asp:DropDownList ID="dropType" runat="server" CssClass="ch1 form-control">
                                        <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Event" Value="Event"></asp:ListItem>
                                        <asp:ListItem Text="News" Value="News"></asp:ListItem>
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
                    <h3 class="card-title">Events & News Lists</h3>
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
                                    <asp:AsyncPostBackTrigger ControlID="gvTickets" EventName="RowCommand" />
                                    <asp:PostBackTrigger ControlID="btnexport" />
                                    <asp:PostBackTrigger ControlID="btnSubmit" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-wrap">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="card-body">
                                                            <div class="">
                                                                <asp:GridView ID="gvTickets" DataKeyNames="Sr_No" runat="server" AllowPaging="false" EmptyDataText="Sorry !! No Records Found"
                                                                    ShowHeader="True" ShowHeaderWhenEmpty="True" EmptyDataRowStyle-ForeColor="Red"
                                                                    EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center" AutoGenerateColumns="false"
                                                                    class="table table-bordered table-striped basic-datatable" OnRowCommand="gvTickets_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="#">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSrNo" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Thumbnail">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="imgThumbnail" runat="server" ImageUrl='<%# ResolveUrl("../Attachment/EventNewsImg/" + Eval("Thumbnail")) %>' Width="50px" Height="50px" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Type">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Title">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Description">
                                                                            <ItemTemplate>
                                                                                <span
                                                                                    class="f-12 text-truncate d-block"
                                                                                    style="max-width: 150px; font-weight: normal;"
                                                                                    data-toggle="tooltip"
                                                                                    data-placement="top"
                                                                                    title='<%# Eval("Description") %>'>
                                                                                    <%# Eval("Description").ToString().Length > 30 ? Eval("Description").ToString().Substring(0, 30) + "..." : Eval("Description") %>
                                                                                </span>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="From Time">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFromTime" runat="server" Text='<%# Eval("From_Time") == DBNull.Value ? "" : (DateTime.Today.Add((TimeSpan)Eval("From_Time")).ToString("hh:mm tt"))  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Time">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblToTime" runat="server" Text='<%# Eval("To_Time") == DBNull.Value ? "" : (DateTime.Today.Add((TimeSpan)Eval("To_Time")).ToString("hh:mm tt"))  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldate" runat="server" Text='<%# Eval("Date","{0:dd/MM/yyyy}")  %>' class="f-12"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Location">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location")  %>' class="f-12"></asp:Label>
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
                                                                                    <asp:LinkButton ID="lnkClubFormation" runat="server" CssClass="dropdown-item"
                                                                                        CommandArgument='<%# Eval("Sr_No")%>' OnClientClick="$('#UpdateProgress1').show();"
                                                                                        CommandName="Formationrow" CausesValidation="false">Club Formation
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


                                    <div class="modal fade" id="ticketSummary" role="dialog">
                                        <div class="modal-dialog modal-lg">
                                            <!-- Modal content-->
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4 class="modal-title">Ticket Summary of
                    <asp:Label ID="lblstaffName" runat="server"></asp:Label></h4>
                                                    <button type="button" class="close" data-dismiss="modal">
                                                        &times;
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="container-fluid">
                                                        <div class="row justify-content-center">
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <label class="control-label ">Staff Remark :<span style="color: Red"></span></label>
                                                                    <asp:TextBox ID="txtstaffRemark" class="form-control" runat="server" TextMode="MultiLine" Rows="2" ReadOnly="True"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <label class="control-label">Order Status :</label>
                                                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="InProcess" Value="InProcess"></asp:ListItem>
                                                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label class="control-label ">Remark :<span style="color: Red"></span></label>
                                                                    <asp:TextBox ID="txtremark" class="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Update Order" class="btn  btn-secondary"
                                                                    ValidationGroup="valid" Style="margin-top: 20px; display: block; margin-left: auto; margin-right: auto;"
                                                                    CausesValidation="false" OnClick="Button1_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
</asp:Content>



