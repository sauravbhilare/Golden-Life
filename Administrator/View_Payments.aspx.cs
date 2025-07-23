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
                cmd = new SqlCommand(@"Select *, isnull((Select Plan_Name  FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] where [Sr_No]=p.[Subscription_Id]),0)  as Plan_Name
                FROM [goldenlife].[goldenlife].[Payment_Master] p where Org_Id=@Org_Id order by [Sr_No] desc", con);
            }
            else
            {
                StringBuilder query = new StringBuilder();
                if (dropType.SelectedValue != "-1")
                {
                    query.Append(" and [Status]=@Status");
                }
                //if (ddlStaff.SelectedValue != "-1")
                //{
                //    query.Append(" and Id in (select Ticket_Id from hillsopt.Ticket_Staffs where Staff_Id=@StaffId) ");
                //}
                if (txtFromDate.Text != "" && txtTodate.Text != "")
                {
                    query.Append(" and convert(date,PaidOn,103) between convert(date,@fromDate,103) and convert(date, @toDate,103)");
                }
                cmd = new SqlCommand(@"Select *, isnull((Select Plan_Name  FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] where [Sr_No]=p.[Subscription_Id]),0)  as Plan_Name
                FROM [goldenlife].[goldenlife].[Payment_Master] p where 1=1 " + query.ToString() + " And Org_Id=@Org_Id order by [Sr_No] desc", con);
            }
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Status", dropType.SelectedValue);
            if (txtFromDate.Text != "" && txtTodate.Text != "")
            {
                cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(txtTodate.Text));
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvPayments.DataSource = dt;
            gvPayments.DataBind();
            if (dt.Rows.Count > 0)
            {
                lbtnExport.Visible = true;
                gvPayments.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPayments.FooterRow.TableSection = TableRowSection.TableFooter;

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
        //    cmd = new SqlCommand(@"Select *, isnull((Select Plan_Name  FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] where [Sr_No]=p.[Subscription_Id]),0)  as Plan_Name
        //        FROM [goldenlife].[goldenlife].[Payment_Master] p where Org_Id=@Org_Id order by [Sr_No] desc ", con);
        //    cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);
        //    con.Open();
        //    gvPayments.DataSource = dt;
        //    gvPayments.DataBind();
        //    if (dt.Rows.Count > 0)
        //    {
        //        lbtnExport.Visible = true;
        //        gvPayments.HeaderRow.TableSection = TableRowSection.TableHeader;
        //        gvPayments.FooterRow.TableSection = TableRowSection.TableFooter;

        //        ViewState["Payments"] = dt;
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
    protected void gvPayments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UpdatePayment")
        {
            string PaymentId = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#upatePayment').modal('show');", true);

            BindPaymentDetails(PaymentId);
        }
        else if (e.CommandName == "Deletecustomer")
        {
            string id = e.CommandArgument.ToString();
            con.Open();

            try
            {
                cmd = new SqlCommand(@"Delete From [goldenlife].[goldenlife].[Payment_Master] where Sr_No=@Sr_No", con);
                cmd.Parameters.AddWithValue("@Sr_No", id);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Order deleted successfully.')", true);
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

        Bind();
    }


    protected void BindPaymentDetails(string PaymentId)
    {
        lblPaymentId.Text = PaymentId;
        try
        {
            con.Open();
            cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[Payment_Master] where Org_Id=@Org_Id and Sr_No=@Sr_No order by [Sr_No] desc", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("@Sr_No", PaymentId);

            SqlDataReader reader = cmd.ExecuteReader();

            // Check if data is available
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Bind the data to your labels
                    txtUsername.Text = reader["Name"].ToString();
                    lblSrNo.Text = reader["User_Id"].ToString();
                    txtTransactionId.Text = reader["Txn_Id"].ToString();
                    txtPaymentId.Text = reader["Payment_Id"].ToString();
                    ddlPaymentStatus.SelectedValue = reader["Status"].ToString();



                }
            }

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
        }
    }



    protected void gvPayments_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    private void ExportGridToExcel()
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust based on your license

            if (ViewState["Payments"] != null)
            {
                DataTable dt = (DataTable)ViewState["Payments"];
                dt.Columns.Remove("Org_Id");
                dt.Columns.Remove("User_Id");
                dt.Columns.Remove("Subscription_Id");
                dt.Columns.Remove("Sign");
                dt.Columns.Remove("AddedOn");
                dt.Columns.Remove("User_Type");
                //dt.Columns.Remove("Added_On");


                // Create a new Excel package
                using (ExcelPackage pck = new ExcelPackage())
                {
                    // Create a worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Payments");

                    // Load data to the worksheet
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(9).Style.Numberformat.Format = "dd/mm/yyyy";

                    // Response for Excel download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Payments.xlsx");
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

    protected void gvPayments_RowDataBound(object sender, GridViewRowEventArgs e)
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


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Check if the selected payment status is "Paid" and txtPaymentId is empty
            if (ddlPaymentStatus.SelectedValue == "Paid" && string.IsNullOrWhiteSpace(txtPaymentId.Text))
            {
                // Show an error message if Payment ID is required but not provided
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "error('Payment ID is required when status is Paid.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#upatePayment').modal('show');", true);
                return; // Exit the method
            }

            con.Open();
            t = con.BeginTransaction(); // Start transaction

            // Update Payment_Master table
            cmd = new SqlCommand(@"UPDATE [goldenlife].[Payment_Master] 
                               SET [Payment_Id] = @Payment_Id, [Status] = @Status 
                               WHERE Org_Id=@Org_Id and Sr_No=@Sr_No", con, t);
            cmd.Parameters.AddWithValue("Sr_No", lblPaymentId.Text);
            cmd.Parameters.AddWithValue("Org_Id", lblOrgId.Text);
            cmd.Parameters.AddWithValue("Payment_Id", txtPaymentId.Text);
            cmd.Parameters.AddWithValue("Status", ddlPaymentStatus.SelectedValue);
            cmd.ExecuteNonQuery();

            if (ddlPaymentStatus.SelectedValue == "Paid")
            {
                DateTime subValidFrom = DateTime.Now; // PaidOn date
                DateTime subValidTill = subValidFrom.AddYears(1); // Add one year to PaidOn date

                // Update User_Master table for Paid status
                cmd = new SqlCommand(@"UPDATE [goldenlife].[User_Master] 
                                   SET [Sub_Valid_From] = @Sub_Valid_From, [Sub_Valid_Till] = @Sub_Valid_Till, 
                                   [Is_Subscribed] = @Is_Subscribed, [Payment_Id] = @Payment_Id, [Status] = @Status 
                                   WHERE [Org_Id]=@Org_Id AND [Id]=@Id", con, t);

                cmd.Parameters.AddWithValue("@Id", lblSrNo.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", subValidFrom);
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", subValidTill);
                cmd.Parameters.AddWithValue("@Is_Subscribed", "1");
                cmd.Parameters.AddWithValue("@Payment_Id", txtTransactionId.Text);
                cmd.Parameters.AddWithValue("@Status", "1");

                cmd.ExecuteNonQuery();
            }
            else if (ddlPaymentStatus.SelectedValue == "Unpaid")
            {
                // Update User_Master table for Unpaid status
                cmd = new SqlCommand(@"UPDATE [goldenlife].[User_Master] 
                                   SET [Sub_Valid_From] = @Sub_Valid_From, [Sub_Valid_Till] = @Sub_Valid_Till, 
                                   [Is_Subscribed] = @Is_Subscribed, [Payment_Id] = @Payment_Id, [Status] = @Status 
                                   WHERE [Org_Id]=@Org_Id AND [Id]=@Id", con, t);

                cmd.Parameters.AddWithValue("@Id", lblSrNo.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", DBNull.Value);
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", DBNull.Value);
                cmd.Parameters.AddWithValue("@Is_Subscribed", "0");
                cmd.Parameters.AddWithValue("@Payment_Id", txtTransactionId.Text);
                cmd.Parameters.AddWithValue("@Status", "1");

                cmd.ExecuteNonQuery();
            }

            // Commit the transaction if all commands execute successfully
            t.Commit();

            // Success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "success('Payment Status Updated successfully.');", true);
        }
        catch (Exception ex)
        {
            // Rollback the transaction if any error occurs
            t.Rollback();

            // Handle the error
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "error('An error occurred: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
        finally
        {
            con.Close();
            Bind(); // Rebind data if needed
        }
    }


    protected void ddlPaymentStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Check if the selected value is "Unpaid"
        if (ddlPaymentStatus.SelectedValue == "Unpaid")
        {
            txtPaymentId.Text = ""; // Clear the Payment ID text box
        }

        // Register the script to reinitialize the DataTable
        Bind();
    }



    protected void lbtnFilter_Click(object sender, EventArgs e)
    {
        Bind();
    }
}