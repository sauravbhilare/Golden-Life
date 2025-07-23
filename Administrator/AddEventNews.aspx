<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="AddEventNews.aspx.cs" Inherits="Administrator_PlaceOrder" EnableEventValidation="false" %>

<%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="aspajax" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tempusdominus-bootstrap-4/5.39.0/css/tempusdominus-bootstrap-4.min.css" integrity="sha512-DdQK42+dB+BPRLkmB3KcE2WiqTKjMx1K0Bq2povhqtTqFkDU5KR9k7c1zGyN/4WqIimylI+IthpZP4gC8Bxw+g==" crossorigin="anonymous" referrerpolicy="no-referrer" />
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
    <asp:Label ID="lblEventId" runat="server" Text="" Visible="false"></asp:Label>

    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Events & News</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlViewOrder" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/View_Event_News.aspx">View Event & News</asp:HyperLink>
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
                        <!-- Type Dropdown -->
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="ddlType">Type</label>
                                <span style="color: red">*</span>
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Event" Value="Event"></asp:ListItem>
                                    <asp:ListItem Text="News" Value="News"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server" ErrorMessage="Type is Required" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="ddlType" />
                            </div>
                        </div>

                        <!-- Title -->
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtTitle">Title</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtTitle" runat="server" class="form-control" placeholder="Title"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="red" runat="server" ErrorMessage="Please Enter Title" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtTitle" />
                            </div>
                        </div>

                        <!-- Date -->
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="txtDate">Date</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtdate" runat="server" class="form-control txtGSTNumber" placeholder="Date" TextMode="Date" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Date is Required" Display="Dynamic" ValidationGroup="Expense" ControlToValidate="txtdate" />
                            </div>
                        </div>

                        <!-- From Time -->
                        <div class="col-md-3" runat="server" id="fromdiv">
                            <div class="form-group">
                                <label for="txtFromTime">From Time</label>
                                <div class="input-group date" id="fromTimePicker" data-target-input="nearest">
                                    <asp:TextBox ID="txtFromTime" runat="server" class="form-control datetimepicker-input" placeholder="From Time" data-target="#fromTimePicker" AutoComplete="off"></asp:TextBox>
                                    <div class="input-group-append" data-target="#fromTimePicker" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-clock"></i></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- To Time -->
                        <div class="col-md-3" runat="server" id="todiv">
                            <div class="form-group">
                                <label for="txtToTime">To Time</label>
                                <div class="input-group date" id="toTimePicker" data-target-input="nearest">
                                    <asp:TextBox ID="txtToTime" runat="server" class="form-control datetimepicker-input" placeholder="To Time" data-target="#toTimePicker" AutoComplete="off"></asp:TextBox>
                                    <div class="input-group-append" data-target="#toTimePicker" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-clock"></i></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Thumbnail File Upload -->
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="control-label">Thumbnail:</label>
                                <div class="input-group">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="ThumbnailFile" runat="server" class="custom-file-input upload" />
                                        <label class="custom-file-label" for="ThumbnailFile">Choose file</label>
                                    </div>
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="(Note: Image should not be greater than 1MB.)" Style="font-size: 12px;"></asp:Label>
                            </div>
                        </div>

                        <!-- Multiple File Upload -->
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="control-label">Multiple Images:</label>
                                <div class="input-group">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input upload" AllowMultiple="true" />
                                        <label class="custom-file-label" for="ThumbnailFile">Choose file</label>
                                    </div>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="(Note: Image should not be greater than 1MB.)" Style="font-size: 12px;"></asp:Label>
                            </div>
                        </div>

                        <!-- Capacity -->
                        <div class="col-md-4" runat="server" id="CapacityDiv">
                            <div class="form-group">
                                <label for="txtCapacity">Capacity</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtCapacity" runat="server" class="form-control" placeholder="Capacity" onkeypress="return isNumberKeyDot(event);" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="red" runat="server" ErrorMessage="Capacity is Required" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtCapacity" />
                            </div>
                        </div>

                        <!-- Location -->
                        <div class="col-md-8" runat="server" id="locationdiv">
                            <div class="form-group">
                                <label for="txtLocation">Location (Address)</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtLocation" TextMode="MultiLine" Rows="2" runat="server" class="form-control" placeholder="Location" AutoComplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="red" runat="server" ErrorMessage="Location is Required" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtLocation" />
                            </div>
                        </div>

                        <!-- Description -->
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtDescription">Description</label>
                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="2" runat="server" class="form-control" placeholder="Description" AutoComplete="off"></asp:TextBox>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="red" runat="server" ErrorMessage="Description is Required" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="txtDescription" />--%>
                            </div>
                        </div>
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

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js" integrity="sha512-qTXRIMyZIFb+1NRbIyrb+7WBXdxV1+ThZP5y6hKW12Zk5VjKH3h+CvH5BxyTmP+mwkd5x5Oa4b9Od8DA3UlLIA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tempusdominus-bootstrap-4/5.39.0/js/tempusdominus-bootstrap-4.min.js" integrity="sha512-1Fcl+mFQGA3rSKNZrC5sU+gAYQEdJNx4yTlS4QufB+OT3g3cwVhUis8oeV8FNxuWeLaQkx33iA0oIbIix7UdnA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
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
    <script>
        $(document).ready(function () {
            $('#fromTimePicker').datetimepicker({
                format: 'LT' // 'LT' is for localized time format, adjust as needed
            });
            $('#toTimePicker').datetimepicker({
                format: 'LT'
            });
        });
    </script>


</asp:Content>

