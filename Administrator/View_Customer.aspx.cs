using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrator_View_Customer : System.Web.UI.Page
{

    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd, cmd1;
    SqlTransaction t;
    string query = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblName = this.Master.FindControl("Name") as Label;
        lblOrgId = this.Master.FindControl("orgg_Id") as Label;
        lbluserId = this.Master.FindControl("user_Id") as Label;
        if (!IsPostBack)
        {
            Bind();

        }
    }
    protected void Bind()
    {

        try
        {
            if (dropType.SelectedValue == "-1" && txtFromDate.Text == "" && txtTodate.Text == "")
            {
                cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[User_Master] where Org_Id=@Org_Id order by [Id] desc", con);
            }
            else
            {
                StringBuilder query = new StringBuilder();
                if (dropType.SelectedValue != "-1")
                {
                    query.Append(" and [Is_Subscribed]=@Is_Subscribed");
                }
                //if (ddlStaff.SelectedValue != "-1")
                //{
                //    query.Append(" and Id in (select Ticket_Id from hillsopt.Ticket_Staffs where Staff_Id=@StaffId) ");
                //}
                if (txtFromDate.Text != "" && txtTodate.Text != "")
                {
                    query.Append(" and convert(date,Added_On,103) between convert(date,@fromDate,103) and convert(date, @toDate,103)");
                }
                cmd = new SqlCommand(@"Select * From  [goldenlife].[goldenlife].[User_Master] where 1=1 " + query.ToString() + " And Org_Id=@Org_Id order by [Added_On] desc", con);
            }
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Is_Subscribed", dropType.SelectedValue);
            if (txtFromDate.Text != "" && txtTodate.Text != "")
            {
                cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(txtTodate.Text));
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvCustomers.DataSource = dt;
            gvCustomers.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbtnExport.Visible = true;
                gvCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvCustomers.FooterRow.TableSection = TableRowSection.TableFooter;

                ViewState["Customers"] = dt;
            }
            else
            {
                lbtnExport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Alert.Show(ex.ToString());
        }
        finally
        {
            con.Close();
        }

        //try
        //{
        //    cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[User_Master] where Org_Id=@Org_Id order by [Id] desc", con);
        //    cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);
        //    con.Open();
        //    gvCustomers.DataSource = dt;
        //    gvCustomers.DataBind();
        //    if (dt.Rows.Count > 0)
        //    {
        //        lbtnExport.Visible = true;
        //        gvCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
        //        gvCustomers.FooterRow.TableSection = TableRowSection.TableFooter;

        //        ViewState["Customers"] = dt;
        //    }
        //    else
        //    {
        //        lbtnExport.Visible = false;
        //    }



        //}
        //catch (Exception ex)
        //{

        //}
        //finally
        //{
        //    con.Close();
        //}

    }
    protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "customeredit")
        {
            lblOrderId.Text = e.CommandArgument.ToString();

            Response.Redirect("Add_Users.aspx?Id=" + lblOrderId.Text);
        }
        else if (e.CommandName == "Deletecustomer")
        {
            string id = e.CommandArgument.ToString();
            con.Open();

            try
            {
                cmd = new SqlCommand(@"Delete From [goldenlife].[goldenlife].[User_Master] where Id=@Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('User deleted successfully.')", true);
            }
            catch (Exception wr)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Error Occured.')", true);
            }
            finally
            {
                con.Close();
            }
        }
        else if (e.CommandName == "ViewUsers")
        {
            string userId = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#userDetailsModal').modal('show');", true);

            BindUserDetails(userId);
        }
        else if (e.CommandName == "Subscription")
        {
            string userSubId = e.CommandArgument.ToString();
            lblSrNo.Text = userSubId;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#SubscriptionModal').modal('show');", true);
            BindSubPlanDrop();
            BindSuscription(userSubId);
        }

        Bind();
    }

    protected void BindSuscription(string userSubId)
    {
        try
        {
            con.Open();
            cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[User_Master] where Org_Id=@Org_Id and Id=@Id order by [Id] desc", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Id", userSubId);
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if data is available
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    // Bind the data to your labels
                    txtUsername.Text = reader["Name"].ToString() + " " + reader["Last_Name"].ToString();


                }
            }


            reader.Close();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
        }

    }


    protected void BindUserDetails(string userId)
    {
        try
        {
            con.Open();
            cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[User_Master] where Org_Id=@Org_Id and Id=@Id order by [Id] desc", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Id", userId);
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if data is available
            if (reader.HasRows)
            {
                modalSection.Visible = true;
                lblMessage.Visible = false;
                while (reader.Read())
                {
                    // Bind the data to your labels
                    lblHusband.Text = reader["Name"].ToString() + " " + reader["Last_Name"].ToString();
                    lblWife.Text = reader["Spouse_Name"].ToString() + " " + reader["Last_Name"].ToString();
                    lblHDOB.Text = reader["DOB"].ToString();
                    lblWDOB.Text = reader["Spouse_DOB"].ToString();
                    lblAnniversaryDate.Text = reader["Anniversary_Date"].ToString();
                    lblMobile.Text = reader["Mobile"].ToString();
                    lblValidFrom.Text = Convert.ToDateTime(reader["Sub_Valid_From"]).ToString("yyyy-MM-dd");
                    lblValidTill.Text = Convert.ToDateTime(reader["Sub_Valid_Till"]).ToString("yyyy-MM-dd");
                    string imageName = reader["Photo"].ToString(); // Assuming this column contains the image name
                    userImage.Src = "../Attachment/UserImg/" + imageName;
                }
            }
            else
            {
                modalSection.Visible = false;
                lblMessage.Visible = true;
                // Handle no records found (optional)
                lblMessage.Text = "No user details found.";
            }

            reader.Close();



        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
        }

    }

    protected void gvCustomers_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    private void ExportGridToExcel()
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust based on your license

            if (ViewState["Customers"] != null)
            {
                DataTable dt = (DataTable)ViewState["Customers"];
                dt.Columns.Remove("Id");
                dt.Columns.Remove("Org_Id");
                dt.Columns.Remove("Photo");
                dt.Columns.Remove("Password");
                dt.Columns.Remove("OTP");
                dt.Columns.Remove("GCM");
                //dt.Columns.Remove("Is_Subscribed");
                dt.Columns.Remove("Payment_Id");
                dt.Columns.Remove("Status");
                //dt.Columns.Remove("Added_On");


                // Create a new Excel package
                using (ExcelPackage pck = new ExcelPackage())
                {
                    // Create a worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Users");

                    // Load data to the worksheet
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(11).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy";

                    // Response for Excel download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Users.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Tell the compiler that the control is rendered
         * explicitly by overriding the VerifyRenderingInServerForm event.*/
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if(e.Row.RowType==DataControlRowType.DataRow)
        //{
        //    string orderstatus=(e.Row.FindControl("lblorderStatus") as Label).Text;

        //    if (orderstatus.ToLower() != "completed")
        //    {
        //        (e.Row.FindControl("liDelete") as HtmlGenericControl).Visible = true;
        //    }
        //    else
        //    {
        //        (e.Row.FindControl("liDelete") as HtmlGenericControl).Visible = false;
        //    }
        //}

    }

    protected void BindSubPlanDrop()
    {
        con.Open();
        try
        {
            cmd = new SqlCommand("SELECT * FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] WHERE [Org_Id]=@Org_Id", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt); // Use only SqlDataAdapter here, no need for SqlDataReader

            if (dt.Rows.Count > 0)
            {
                ddlSubplan.DataSource = dt;
                ddlSubplan.DataTextField = "Plan_Name";
                ddlSubplan.DataValueField = "Sr_No";
                ddlSubplan.DataBind();
                ddlSubplan.Items.Insert(0, new ListItem("Select Plan", "-1")); // Insert "All" at the top
            }
        }
        catch (Exception ex)
        {
            // Handle exception (optional)
        }
        finally
        {
            con.Close();
        }
    }



    protected void ddlSubplan_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] where Org_Id=@Org_Id and Sr_No=@Sr_No", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Sr_No", ddlSubplan.SelectedValue);
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if data is available
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    // Bind the data to your labels
                    txtAmount.Text = reader["Amount"].ToString();

                }
            }
            reader.Close();
        }

        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            Bind();
            //  ScriptManager.RegisterStartupScript(this, GetType(), "InitDataTable", "datatable();", true);
        }
    }

    protected void ddlPaymentStatus_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddlPaymentStatus1.SelectedValue == "Paid")
        {
            paytypeDiv.Visible = true;

        }
        else
        {
            paytypeDiv.Visible = false;
            ddlPaymentType.ClearSelection();
            TransactionIdDiv.Visible = false;
            //paymentIdDiv.Visible = false;
            BankNameDiv.Visible = false;
            ChqIdDiv.Visible = false;
        }
        Bind();
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentType.SelectedValue == "Online")
        {
            TransactionIdDiv.Visible = true;
            //paymentIdDiv.Visible = true;

            BankNameDiv.Visible = false;
            ChqIdDiv.Visible = false;
        }
        else if (ddlPaymentType.SelectedValue == "Cheque")
        {
            TransactionIdDiv.Visible = false;
            //paymentIdDiv.Visible = false;

            BankNameDiv.Visible = true;
            ChqIdDiv.Visible = true;
        }
        else
        {
            TransactionIdDiv.Visible = false;
            //paymentIdDiv.Visible = false;

            BankNameDiv.Visible = false;
            ChqIdDiv.Visible = false;
        }
        Bind();
    }

    protected void btnSubUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Open connection
            con.Open();

            // Start transaction
            t = con.BeginTransaction();

            // Insert into Payment_Master
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Payment_Master]
            ([Org_Id], [User_Id], [Name], [Subscription_Id], [Amount], [Convenience_Fee], 
            [Total_Amount], [Payment_Type], [Txn_Id], [Payment_Id], [Bank_Name], [Chq_Id], 
            [Sign], [Status], [AddedOn], [PaidOn], [User_Type]) 
            VALUES (@Org_Id, @User_Id, @Name, @Subscription_Id, @Amount, @Convenience_Fee, 
            @Total_Amount, @Payment_Type, @Txn_Id, @Payment_Id, @Bank_Name, @Chq_Id, 
            @Sign, @Status, @AddedOn, @PaidOn, @User_Type)", con, t);

            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@User_Id", lblSrNo.Text);
            cmd.Parameters.AddWithValue("@Name", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Subscription_Id", ddlSubplan.SelectedValue);
            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
            cmd.Parameters.AddWithValue("@Convenience_Fee", "0");
            cmd.Parameters.AddWithValue("@Total_Amount", txtAmount.Text);
            cmd.Parameters.AddWithValue("@Payment_Type", ddlPaymentType.SelectedValue);
            cmd.Parameters.AddWithValue("@Txn_Id", txtTransactionID.Text);
            cmd.Parameters.AddWithValue("@Payment_Id", txtTransactionID.Text);
            cmd.Parameters.AddWithValue("@Bank_Name", txtBankName.Text);
            cmd.Parameters.AddWithValue("@Chq_Id", txtChqId.Text);
            cmd.Parameters.AddWithValue("@Sign", "");
            cmd.Parameters.AddWithValue("@Status", ddlPaymentStatus1.SelectedValue);
            cmd.Parameters.AddWithValue("@AddedOn", DateTime.Now);  // current date/time for AddedOn
            cmd.Parameters.AddWithValue("@PaidOn", DateTime.Now);   // current date/time for PaidOn
            cmd.Parameters.AddWithValue("@User_Type", "User");

            cmd.ExecuteNonQuery(); // Execute INSERT command

            // If Payment Status is Paid, Update User_Master with Subscription Details
            if (ddlPaymentStatus1.SelectedValue == "Paid")
            {
                DateTime subValidFrom = DateTime.Now; // PaidOn date
                DateTime subValidTill = subValidFrom.AddYears(1); // Add one year to PaidOn date

                cmd = new SqlCommand(@"UPDATE [goldenlife].[User_Master] 
                SET [Sub_Valid_From] = @Sub_Valid_From, [Sub_Valid_Till] = @Sub_Valid_Till, 
                [Is_Subscribed] = @Is_Subscribed, [Payment_Id] = @Payment_Id, [Status] = @Status 
                WHERE [Org_Id]=@Org_Id AND [Id]=@Id", con, t);

                cmd.Parameters.AddWithValue("@Id", lblSrNo.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", subValidFrom);
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", subValidTill);
                cmd.Parameters.AddWithValue("@Is_Subscribed", "1");
                cmd.Parameters.AddWithValue("@Payment_Id", txtTransactionID.Text);
                cmd.Parameters.AddWithValue("@Status", "1");

                cmd.ExecuteNonQuery(); // Execute UPDATE command
            }
            else if (ddlPaymentStatus1.SelectedValue == "Unpaid")
            {
                cmd = new SqlCommand(@"UPDATE [goldenlife].[User_Master] 
                SET [Sub_Valid_From] = @Sub_Valid_From, [Sub_Valid_Till] = @Sub_Valid_Till, 
                [Is_Subscribed] = @Is_Subscribed, [Payment_Id] = @Payment_Id, [Status] = @Status 
                WHERE [Org_Id]=@Org_Id AND [Id]=@Id", con, t);

                cmd.Parameters.AddWithValue("@Id", lblSrNo.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", DBNull.Value);
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", DBNull.Value);
                cmd.Parameters.AddWithValue("@Is_Subscribed", "0");
                cmd.Parameters.AddWithValue("@Payment_Id", txtTransactionID.Text);
                cmd.Parameters.AddWithValue("@Status", "1");

                cmd.ExecuteNonQuery(); // Execute UPDATE command
            }

            // Commit the transaction if everything is successful
            t.Commit();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Subscription Added Successfully!')", true);
        }
        catch (Exception ex)
        {
            // Rollback the transaction in case of an error
            if (t != null)
            {
                t.Rollback();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
        }
        finally
        {
            con.Close(); // Close the connection
            Bind();
        }
    }

    protected void lbtnFilter_Click(object sender, EventArgs e)
    {
        Bind();
    }
}