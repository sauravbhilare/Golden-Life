using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
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
            cmd = new SqlCommand(@"Select * FROM [goldenlife].[goldenlife].[Photo_Reels_Master] where Org_Id=@Org_Id and Type='Reel' order by [Sr_No] desc", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            gvReels.DataSource = dt;
            gvReels.DataBind();
            if (dt.Rows.Count > 0)
            {
                //lbtnExport.Visible = true;
                gvReels.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvReels.FooterRow.TableSection = TableRowSection.TableFooter;

                ViewState["Reels"] = dt;
            }
            else
            {
                //lbtnExport.Visible = false;
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
    protected void gvReels_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ReelsEdit")
        {
            lblOrderId.Text = e.CommandArgument.ToString();

            Response.Redirect("Add_Reels.aspx?Sr_no=" + lblOrderId.Text);
        }
        else if (e.CommandName == "DeleteReels")
        {
            string id = e.CommandArgument.ToString();
            con.Open();

            try
            {
                cmd = new SqlCommand(@"Delete From [goldenlife].[goldenlife].[Photo_Reels_Master] where Org_Id=@Org_Id and Sr_No=@Sr_No", con);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
                cmd.Parameters.AddWithValue("@Sr_No", id);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Image deleted successfully.')", true);
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




    protected void gvReels_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    private void ExportGridToExcel()
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust based on your license

            if (ViewState["Reels"] != null)
            {
                DataTable dt = (DataTable)ViewState["Reels"];
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
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reels");

                    // Load data to the worksheet
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(11).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy";

                    // Response for Excel download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Reels.xlsx");
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

    protected void gvReels_RowDataBound(object sender, GridViewRowEventArgs e)
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

}