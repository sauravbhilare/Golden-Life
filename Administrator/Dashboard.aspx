<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Administrator_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .inner h3 {
            font-size: 25px !important;
            display: inline-block;
        }

        .inner svg {
            font-size: 25px !important;
            display: inline-block;
        }


        .mytable1 td {
            padding: 0.4rem;
            text-align: center;
        }

        .mytable1 th {
            padding: 0.5rem;
        }

        .mytable1 thead th {
            text-align: center;
        }
    </style>

    <style>
        .tables.display.dataTable.w-100.basic-datatable.table-bordered.orderTable th,
        .tables.display.dataTable.w-100.basic-datatable.table-bordered.orderTable td {
            padding: 7px;
            background-color: white;
            color: black;
        }

        .tables.display.dataTable.w-100.basic-datatable.table-bordered.orderTable tr {
            padding: 7px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblOrgId" runat="server" Visible="false"></asp:Label>
    <div class="row pt-2 mb-2">
        <div class="col-md-6"></div>
        <div class="col-md-6 d-none">
            <div class="btn-group float-right">
                <div class="btn-group">
                    <asp:LinkButton ID="llkbutton" class="btn btn  btn-secondary btn-icon dropdown-toggle " runat="server" Text="FilterBy " data-toggle="dropdown" Style="text-decoration: none;">
                    </asp:LinkButton>
                    <ul class="dropdown-menu dropdown-menu-right">
                        <li>
                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="All" CssClass=" dropdown-item " OnClick="LinkButton3_Click">Overall</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="Today" CssClass=" dropdown-item " OnClick="LinkButton3_Click">Today</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Yesterday" CssClass=" dropdown-item" OnClick="LinkButton3_Click">Yesterday</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="ThisMonth" CssClass=" dropdown-item" OnClick="LinkButton3_Click">This Month</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument="LastMonth" CssClass=" dropdown-item" OnClick="LinkButton3_Click">Last Month</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="row m-0">
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-info">
                <div class="inner">
                    <%--<i class="fa-sharp fa-solid fa-indian-rupee-sign"></i>--%>
                    <h3 runat="server" id="ttlUsers"><asp:Label ID="lblttlUsers" runat="server">0</asp:Label></h3>
                    <p>Total Users</p>
                </div>
                <div class="icon">
                    <i class="fa fa-user" style="font-size: 55px; top: 30px;"></i>
                </div>
                <a href="View_Customer.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-success">
                <div class="inner">
                    <%--<i class="fa-sharp fa-solid fa-indian-rupee-sign"></i>--%>
                    <h3 runat="server" id="CompletedOrder"><asp:Label ID="lblttlActive" runat="server">0</asp:Label></h3>
                    <p>Active Subscriptions</p>
                </div>
                <div class="icon">
                    <i class="fa-solid fa-crown" style="font-size: 55px; top: 30px;"></i>
                </div>
                <a href="View_Customer.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-danger">
                <div class="inner">
                    <h3 runat="server" id="stockOut"><asp:Label ID="lblExpired" runat="server">0</asp:Label></h3>
                    <p>Expired Subscriptions</p>
                </div>
                <div class="icon">
                    <i class="fa fa-user-xmark" style="font-size: 55px; top: 30px;"></i>
                </div>
                <a href="View_Customer.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box bg-warning" style="color: #ffffff !important;">
                <div class="inner">
                    <%--<i class="fa-sharp fa-solid fa-indian-rupee-sign"></i>--%>
                    <h3 runat="server" id="StockIn"><asp:Label ID="lblInActive" runat="server">0</asp:Label></h3>
                    <p>Not Subscribed</p>
                </div>
                <div class="icon">
                    <i class="fa fa-user-slash" style="font-size: 55px; top: 30px;"></i>
                </div>
                <a href="View_Customer.aspx" class="small-box-footer" style="color: #ffffff !important;">More info <i class="fas fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <!-- ./col -->
    </div>
    <div class="row">
        <div class="col-lg-12 col-md-7 col-sm-12 col-xs-12">
            <div class="row justify-content-center">
                <div class="col-sm-12">

                    <div class="card-body">
                        <h5><b>Today's Payments</b></h5>
                        <div class="">
                            <asp:GridView ID="gvPayments" runat="server" CssClass="tables display dataTable w-100 basic-datatable table-bordered orderTable "
                                ShowHeaderWhenEmpty="True" EmptyDataText="Sorry !! No Records Found"
                                EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false"
                                EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" GridLines="None"
                                DataKeyNames="Sr_No">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsdafdfrno" Style="text-align: center" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Plan">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPlan" runat="server" Text='<%# Eval("Plan_Name") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaidOn" runat="server" Text='<%# Eval("PaidOn", "{0:dd/MM/yyyy}") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  <%--  <asp:TemplateField HeaderText="Cnv. Fee">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConvenienceFee" runat="server" Text='<%# Eval("Convenience_Fee") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Total Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transaction Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTxnId" runat="server" Text='<%# Eval("Txn_Id") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentId" runat="server" Text='<%# Eval("Payment_Id") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentStatus" runat="server" Text='<%# Eval("Status") %>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="black"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                <PagerStyle BackColor="White" ForeColor="black" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black"
                                    HorizontalAlign="Center" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>

                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-lg-12 col-md-7 col-sm-12 col-xs-12">
            <div class="row justify-content-center">
                <div class="col-sm-12">

                    <div class="card-body">
                        <h5><b>Today's Registrations</b></h5>
                        <div class="">
                            <asp:GridView ID="gvCustomers" runat="server" CssClass="tables display dataTable w-100 basic-datatable table-bordered orderTable "
                                ShowHeaderWhenEmpty="True" EmptyDataText="Sorry !! No Records Found"
                                EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false"
                                EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" GridLines="None"
                                DataKeyNames="Id">
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
                                            <asp:Label ID="lblHusbandName" runat="server" Text='<%# Eval("Husband_Name") + " " +  Eval("Last_Name")%>' class="f-12"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wife Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWifeName" runat="server" Text='<%# Eval("Wife_Name") + " " +  Eval("Last_Name") %>' class="f-12"></asp:Label>
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
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="black"
                                    HorizontalAlign="Center" VerticalAlign="Middle" />
                                <PagerStyle BackColor="White" ForeColor="black" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black"
                                    HorizontalAlign="Center" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>

                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-lg-3 d-none">
            <div class="row">
                <div class="col-lg-12 col-12">
                    <!-- small box -->
                    <div class="small-box bg-warning" style="color: #ffffff !important;">
                        <div class="inner">
                            <%--<i class="fa-sharp fa-solid fa-indian-rupee-sign"></i>--%>
                            <h3 runat="server" id="pendingTickets">0</h3>
                            <p>Pending Tickets</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-ticket" style="font-size: 55px; top: 30px;"></i>
                        </div>
                        <a href="View_Tickets.aspx" class="small-box-footer" style="color: #ffffff !important;">More info <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>

                <div class="col-lg-12 col-12">
                    <!-- small box -->
                    <div class="small-box bg-info">
                        <div class="inner">
                            <%--<i class="fa-sharp fa-solid fa-indian-rupee-sign"></i>--%>
                            <h3 runat="server" id="resolvedTickets">0</h3>
                            <p>Resolved Tickets</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-ticket" style="font-size: 55px; top: 30px;"></i>
                        </div>
                        <a href="View_Tickets.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>



