<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="View_Reels.aspx.cs" Inherits="Administrator_View_Customer" %>

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
    <asp:Label Text="" runat="server" ID="lblprodid" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Reels</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlAddImages" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/Add_Reels.aspx">+Add</asp:HyperLink>
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
                    <h3 class="card-title">Reels List</h3>
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
                                        <h5 class="txt-dark "><i class="zmdi zmdi-comment-text mr-10"></i><b>Banner Details</b> </h5>
                                    </div>--%>
                                    <div class="row justify-content-center">
                                        <div class="col-sm-12">
                                            <div class="card-body">
                                                <div class="">
                                                    <asp:GridView ID="gvReels" runat="server" CssClass="tables display dataTable w-100 basic-datatable table-bordered orderTable "
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="Sorry !! No Records Found"
                                                        EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" GridLines="None"
                                                        DataKeyNames="Sr_No" OnRowCommand="gvReels_RowCommand" OnRowCreated="gvReels_RowCreated" OnRowDataBound="gvReels_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="#">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsdafdfrno" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                          <%--  <asp:TemplateField HeaderText="Image">
                                                                <ItemTemplate>
                                                                    <a href='../Attachment/PhotoReelsImg/<%# Eval("Img")%>' target="_blank">
                                                                        <img src='../Attachment/PhotoReelsImg/<%# Eval("Img")%>' style="font-size: x-small; width: 50px; height: 50px" />
                                                                    </a>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Title">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title")%>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Youtube Link">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYtId" runat="server" Text='<%# Eval("Yt_Id") %>' class="f-12"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div class="btn-group-vertical">
                                                                        <div class="btn-group">
                                                                            <asp:LinkButton ID="llkbutton" runat="server" data-toggle="dropdown" Style="text-decoration: none;"><i class="fa fa-ellipsis-v" aria-hidden="true" style=" color:Black;"></i>
                                                                            </asp:LinkButton>
                                                                            <ul class="dropdown-menu">
                                                                                <li style="font-size: 17px;">
                                                                                    <asp:LinkButton ID="LinkButtonsummery" runat="server" CssClass=" p-2" CommandArgument='<%# Eval("Sr_No")%>' CommandName="ReelsEdit" CausesValidation="false" Style="font-size: 17px; color: Black;">          
                                                                                <i class="fa-solid fa-edit"></i> Edit</asp:LinkButton>
                                                                                </li>
                                                                                <li style="font-size: 17px;" runat="server" id="liDelete">
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass=" p-2" OnClientClick="return confirm('Do you want to delete this Reel ?');" CommandArgument='<%# Eval("Sr_No")%>' CommandName="DeleteReels" CausesValidation="false" Style="font-size: 17px; color: red;">          
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
</asp:Content>

