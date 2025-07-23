using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;
using OfficeOpenXml;

public partial class Administrator_View_Tickets : System.Web.UI.Page
{
    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd, cmd1;
    SqlTransaction t;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblName = this.Master.FindControl("Name") as Label;
        lblOrgid = this.Master.FindControl("orgg_Id") as Label;
        if (!IsPostBack)
        {
            //BindStaffs();
            BindGrid();
        }
    }
    protected void BindGrid()
    {
        try
        {
            cmd = new SqlCommand(@"Select * From [goldenlife].[goldenlife].[Subscription_Plan_Master] order by [Added_On] desc", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvPlan.DataSource = dt;
            gvPlan.DataBind();
            if (dt.Rows.Count > 0)
            {
                btnexport.Visible = true;
                gvPlan.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPlan.FooterRow.TableSection = TableRowSection.TableFooter;

                ViewState["Customers"] = dt;
            }
            else
            {
                btnexport.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        if (e.CommandName == "Deleterow")
        {
            con.Open();
            try
            {
                cmd = new SqlCommand("Delete from [goldenlife].[goldenlife].[Subscription_Plan_Master] where [Sr_No]=" + id, con);
                cmd.ExecuteNonQuery();


                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Plan Deleted Successfully!')", true);
            }
            catch (SqlException ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();
                BindGrid();
            }
        }
        else if (e.CommandName == "Editrow")
        {
            string PlanId = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#PlanModal').modal('show');", true);
            BindPlanDetails(PlanId);
            ScriptManager.RegisterStartupScript(this, GetType(), "InitDataTable", "datatable();", true);

        }

        BindGrid();
    }
    protected void BindPlanDetails(string PlanId)
    {
        lblPlanId.Text = PlanId;
        try
        {
            cmd = new SqlCommand(@"Select * from [goldenlife].[goldenlife].[Subscription_Plan_Master] where [Sr_No]=" + PlanId, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                txtPlanName.Text = dr["Plan_Name"].ToString();
                txtAmount.Text = dr["Amount"].ToString();
                txtConvenienceFee.Text = dr["Convenience_Fee"].ToString();
                txtTotalAmount.Text = dr["Total_Amount"].ToString();
                txtDiscription.Text = dr["Description"].ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
        }
        finally
        {
            con.Close();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void btnexport_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }
    private void ExportGridToExcel()
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust based on your license

            if (ViewState["Customers"] != null)
            {
                DataTable dt = (DataTable)ViewState["Customers"];
                //dt.Columns.Remove("Id");
                //dt.Columns.Remove("Org_Id");
                //dt.Columns.Remove("Photo");
                //dt.Columns.Remove("Password");
                //dt.Columns.Remove("OTP");
                //dt.Columns.Remove("GCM");
                //dt.Columns.Remove("Is_Subscribed");
                //dt.Columns.Remove("Payment_Id");
                //dt.Columns.Remove("Status");
                //dt.Columns.Remove("Added_On");


                // Create a new Excel package
                using (ExcelPackage pck = new ExcelPackage())
                {
                    // Create a worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Subscription_Plans");

                    // Load data to the worksheet
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(11).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy";

                    // Response for Excel download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Subscription_Plans.xlsx");
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


    protected void lbtnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "datatable()", true);
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            cmd = new SqlCommand(@"UPDATE [goldenlife].[Subscription_Plan_Master]SET [Plan_Name] = @Plan_Name,[Amount] = @Amount,[Convenience_Fee] = @Convenience_Fee,[Total_Amount] = @Total_Amount,[Description] = @Description WHERE Org_Id=@Org_Id and Sr_No=@Sr_No", con);
            cmd.Parameters.AddWithValue("Sr_No", lblPlanId.Text);
            cmd.Parameters.AddWithValue("Org_Id", lblOrgid.Text);
            cmd.Parameters.AddWithValue("Plan_Name", txtPlanName.Text);
            cmd.Parameters.AddWithValue("Amount", txtAmount.Text);
            cmd.Parameters.AddWithValue("Convenience_Fee", txtConvenienceFee.Text);
            cmd.Parameters.AddWithValue("Total_Amount", txtTotalAmount.Text);
            cmd.Parameters.AddWithValue("Description", txtDiscription.Text);
            cmd.ExecuteNonQuery();

            // Success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Payment Status Updated successfully.');", true);

            // Close the modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModal", "$('#PlanModal').modal('hide');", true);
        }
        catch (Exception ex)
        {
            // Handle the error
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "error('An error occurred: " + ex.Message.Replace("'", "\\'") + "');", true);
        }
        finally
        {
            con.Close();
            BindGrid(); // Rebind data if needed
            ScriptManager.RegisterStartupScript(this, GetType(), "InitDataTable", "datatable();", true);

        }

    }
}