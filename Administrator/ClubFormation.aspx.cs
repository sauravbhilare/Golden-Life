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
        lblUserId = this.Master.FindControl("user_Id") as Label;
        if (!IsPostBack)
        {
            if (Request.QueryString["Sr_No"] != null)
            {
                lblEventId.Text = Request.QueryString["Sr_No"].ToString();
                BindDetails();
                // btnSubmit.Text = "Update";
            }

            BindGrid();
            ddlEventsBind();
        }
    }
    protected void BindGrid()
    {
        try
        {

            cmd = new SqlCommand(@"Select * From  [goldenlife].[goldenlife].[User_Master] Where [Is_Subscribed]='1' order by [Added_On] desc", con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
            if (dt.Rows.Count > 0)
            {
                //btnexport.Visible = true;
                gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvUsers.FooterRow.TableSection = TableRowSection.TableFooter;

                ViewState["Events"] = dt;
            }
            else
            {
                //btnexport.Visible = false;
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

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        if (e.CommandName == "Deleterow")
        {
            con.Open();
            try
            {
                cmd = new SqlCommand("Delete from [goldenlife].[goldenlife].[Event_News_Master] where [Sr_No]=" + id, con);
                cmd.ExecuteNonQuery();




                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Event & News Deleted Successfully!')", true);
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
            Response.Redirect("AddEventNews.aspx?Sr_No=" + id, false);
        }
        else if (e.CommandName == "Formationrow")
        {
            Response.Redirect("AddEventNews.aspx?Sr_No=" + id, false);
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
                //ddlstatus.SelectedValue = dr["Status"].ToString();
                //txtstaffRemark.Text = dr["StaffRemark"].ToString();
                //txtremark.Text = dr["AdminRemark"].ToString();
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
    protected void ddlEventsBind()
    {
        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Event_News_Master] where [Org_Id]=@Org_Id order by [Sr_No]", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ddlEventTitle.DataSource = dt;
            ddlEventTitle.DataTextField = "Title";
            ddlEventTitle.DataValueField = "Sr_No";
            ddlEventTitle.DataBind();
            ddlEventTitle.Items.Insert(0, new ListItem("---Select Event---", "-1"));
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }


    protected void ddlEventTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cmd = new SqlCommand(@"Select * from [goldenlife].[goldenlife].[Event_News_Master] where [Sr_No]= @Sr_No And [Org_Id]=@Org_Id", con);
            cmd.Parameters.AddWithValue("@Sr_No", ddlEventTitle.SelectedValue);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                TxtCapacity.Text = dr["Capacity"].ToString();
                txtLocation.Text = dr["Location"].ToString();



            }
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

    protected void BindDetails()
    {
        try
        {
            cmd = new SqlCommand(@"Select * from [goldenlife].[goldenlife].[Event_News_Master] where [Sr_No]= @Sr_No And [Org_Id]=@Org_Id", con);
            cmd.Parameters.AddWithValue("@Sr_No", lblEventId.Text);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                TxtCapacity.Text = dr["Capacity"].ToString();
                txtLocation.Text = dr["Location"].ToString();
                ddlEventTitle.SelectedValue = dr["Sr_No"].ToString();

            }
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
}