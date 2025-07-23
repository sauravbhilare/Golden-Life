<%@ Page Title="" Language="C#" MasterPageFile="~/Administrator/AdminMaster.master" AutoEventWireup="true" CodeFile="Add_Images.aspx.cs" Inherits="Administrator_Add_Images" %>

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
                    <h1>Images</h1>
                </div>
                <div class="col-sm-2 float-sm-right">
                    <asp:HyperLink ID="hlViewImages" runat="server" class="btn btn-sm bg-gradient-dark pt-2 float-right" NavigateUrl="~/Administrator/View_Images.aspx">View Images</asp:HyperLink>
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
                    <h3 class="card-title">Add Images</h3>
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
                        <div class="col-md-8">
                            <asp:UpdatePanel ID="up1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="ddlFolder">Folder</label>
                                                <span style="color: red">*</span>
                                                <asp:LinkButton ID="lnkAddFolder" runat="server" CssClass="ml-2" OnClick="lnkAddFolder_Click"> +Add Folder</asp:LinkButton>
                                                <asp:DropDownList ID="ddlFolder" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="red" runat="server" ErrorMessage="Folder Name is Required" Display="Dynamic" ValidationGroup="Expense" InitialValue="" ControlToValidate="ddlFolder" />
                                            </div>
                                        </div>
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
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-4">
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
                        </div>
                    </div>
                    <asp:UpdatePanel ID="up2" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="row">
                                <!-- Description Field -->
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label for="txtRemarks">Description</label>
                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="2" runat="server" class="form-control" placeholder="Description" AutoComplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- Centered Submit Button -->
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center align-items-center">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" UseSubmitBehavior="true" OnClick="btnSubmit_Click"
                                        CausesValidation="true" ValidationGroup="Expense" class="btn btn-secondary" Style="padding: 6px 24px;" />
                                </div>
                            </div>

                            <!-- Validation Summary -->
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center align-items-center">
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Expense" ShowMessageBox="True" ShowSummary="False" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hfCustomers" ClientIDMode="Static" runat="server" />
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

    <!-- Modal -->
    <div class="modal fade" id="FolderModal" tabindex="-1" aria-labelledby="userDetailsModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add Folder</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group w-100">
                        <label for="txtFolderName">Folder Name <span style="color: red">*</span></label>
                        <asp:TextBox ID="txtFolderName" runat="server" CssClass="form-control" placeholder="Folder Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFolderName" runat="server" ControlToValidate="txtFolderName"
                            ErrorMessage="Folder Name is required" ForeColor="red" Display="Dynamic" ValidationGroup="AddFolderGroup" />
                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                    <asp:Button ID="btnFolderAdd" runat="server" CssClass="btn btn-secondary" Text="Submit"
                        OnClick="btnFolderAdd_Click" ValidationGroup="AddFolderGroup" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>

