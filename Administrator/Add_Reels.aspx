<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="Add_Reels.aspx.cs" Inherits="Administrator_Add_Images" %>

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
    <asp:Label ID="lblImageID" runat="server" Text="" Visible="false"></asp:Label>
    <section class="content-header">
        <div class="container-fluid">
            <div class="row mb-2 justify-content-between">
                <div class="col-sm-6">
                    <h1>Reels</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlViewReels" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/View_Reels.aspx">View Reels</asp:HyperLink>
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
                    <h3 class="card-title">Add Reels</h3>
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
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtName">Title</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtTitle" runat="server" class="form-control" placeholder="Title"></asp:TextBox>
                                <!-- Set Display to Dynamic or Static -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                    ForeColor="red"
                                    runat="server"
                                    ErrorMessage="Please Enter Title"
                                    Display="Dynamic"
                                    ValidationGroup="Expense"
                                    ControlToValidate="txtTitle">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                            <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtName">Youtube Link</label>
                                <span style="color: red">*</span>
                                <asp:TextBox ID="txtytlnk" runat="server" class="form-control" placeholder="Title"></asp:TextBox>
                                <!-- Set Display to Dynamic or Static -->
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    ForeColor="red"
                                    runat="server"
                                    ErrorMessage="Please Enter Youtube URL"
                                    Display="Dynamic"
                                    ValidationGroup="Expense"
                                    ControlToValidate="txtytlnk">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                      <%--  <div class="col-md-6">
                            <div class="form-group">
                                <label class="control-label">Image:</label>
                                  <span style="color: red">*</span>
                                <div class="input-group">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="ImageFile" runat="server" class="custom-file-input upload" />
                                        <label class="custom-file-label" for="ImageFile">Choose file</label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    ForeColor="red"
                                    runat="server"
                                    ErrorMessage="Please Add Image"
                                    Display="Dynamic"
                                    ValidationGroup="Expense"
                                    ControlToValidate="ImageFile">
                                </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="(Note: Image should not be greater than 1MB.)" Style="font-size: 12px;"></asp:Label>
                            </div>
                        </div>--%>
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtRemarks">Description</label>
                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="2" runat="server" class="form-control" placeholder="Description" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="btnSubmit_Click" CausesValidation="true" ValidationGroup="Expense" class="btn btn-secondary" Style="padding: 6px 24px;" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Expense" ShowMessageBox="True" ShowSummary="False" />
                        <div class="clearfix"></div>
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

</asp:Content>

