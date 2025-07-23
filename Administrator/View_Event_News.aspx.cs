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
            if (dropType.SelectedValue == "-1" && txtFromDate.Text == "" && txtTodate.Text == "")
            {
                cmd = new SqlCommand(@"Select * From  [goldenlife].[goldenlife].[Event_News_Master] order by [Added_On] desc", con);
            }
            else
            {
                StringBuilder query = new StringBuilder();
                if (dropType.SelectedValue != "-1")
                {
                    query.Append(" and [Type]=@Type");
                }
                //if (ddlStaff.SelectedValue != "-1")
                //{
                //    query.Append(" and Id in (select Ticket_Id from hillsopt.Ticket_Staffs where Staff_Id=@StaffId) ");
                //}
                if (txtFromDate.Text != "" && txtTodate.Text != "")
                {
                    query.Append(" and convert(date,Date,103) between convert(date,@fromDate,103) and convert(date, @toDate,103)");
                }
                cmd = new SqlCommand(@"Select * From  [goldenlife].[goldenlife].[Event_News_Master] where 1=1 " + query.ToString() + " order by [Added_On] desc", con);
            }
            cmd.Parameters.AddWithValue("@Type", dropType.SelectedValue);
            if (txtFromDate.Text != "" && txtTodate.Text != "")
            {
                cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(txtTodate.Text));
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvTickets.DataSource = dt;
            gvTickets.DataBind();
            if (dt.Rows.Count > 0)
            {
                btnexport.Visible = true;
                gvTickets.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvTickets.FooterRow.TableSection = TableRowSection.TableFooter;

                ViewState["Events"] = dt;
            }
            else
            {
                btnexport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Alert.Show(ex.ToString());
        }
        finally
        {

        }
    }

    protected void gvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        if (e.CommandName == "Deleterow")
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                cmd = new SqlCommand("Delete from [goldenlife].[goldenlife].[Event_News_Master] where [Sr_No]=@id", con, transaction);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("Delete from [goldenlife].[goldenlife].[Event_Images] where [Event_Id]=@id", con, transaction);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                // Commit transaction if both commands execute successfully
                transaction.Commit();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Success('Event & News Deleted Successfully!')", true);
            }
            catch (SqlException ex)
            {
                // Rollback transaction in case of error
                transaction.Rollback();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of error
                transaction.Rollback();
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
            Response.Redirect("AddEventNews.aspx?Sr_No=" + id, false);
        }
        else if (e.CommandName == "Formationrow")
        {
            Response.Redirect("ClubFormation.aspx?Sr_No=" + id, false);
        }

        BindGrid();
    }
    protected void BindDetails(string id)
    {
        try
        {
            cmd = new SqlCommand(@"Select * from [hillsopt].[hillsopt].[Raise_Ticket] where [Id]=" + id, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ddlstatus.SelectedValue = dr["Status"].ToString();
                txtstaffRemark.Text = dr["StaffRemark"].ToString();
                txtremark.Text = dr["AdminRemark"].ToString();
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            SqlCommand cmd = new SqlCommand(@"update [hillsopt].[hillsopt].[Raise_Ticket] set [Status]=@Status,
                                            [AdminRemark]=@AdminRemark where [Id]=@Id", con, t);
            cmd.Parameters.AddWithValue("@Id", lblId.Text);
            cmd.Parameters.AddWithValue("@AdminRemark", txtremark.Text);
            cmd.Parameters.AddWithValue("@Status", ddlstatus.SelectedValue);
            con.Open();
            cmd.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Ticket Updated Successfully!')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
        }
        finally
        {
            con.Close();
        }

        BindGrid();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "datatable()", true);
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

            if (ViewState["Events"] != null)
            {
                DataTable dt = (DataTable)ViewState["Events"];
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
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Events_&_News");

                    // Load data to the worksheet
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(11).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy";

                    // Response for Excel download
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Events_&_News.xlsx");
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
    //protected void dropStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlStaff.ClearSelection();
    //    txtFromDate.Text = "";
    //    txtTodate.Text = "";
    //    BindGrid();
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "datatable()", true);
    //}
    protected void ddlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtTodate.Text = "";
        BindGrid();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "datatable()", true);
    }
    protected void lbtnFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "datatable()", true);
    }
}