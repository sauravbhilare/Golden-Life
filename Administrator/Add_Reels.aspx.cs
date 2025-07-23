using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;


public partial class Administrator_Add_Images : System.Web.UI.Page
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
            //txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //BindCustomer();
            if (Request.QueryString["Sr_No"] != null)
            {
                lblImageID.Text = Request.QueryString["Sr_No"].ToString();
                BindDetails();
                btnSubmit.Text = "Update";
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        con.Open();
        Random r = new Random();
        if (btnSubmit.Text == "Submit")
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Photo_Reels_Master]([Org_Id],[Folder_Id],[Type],[Title],[Description],[Img],[Yt_Id],[Status],[Added_By_Id],[Added_By_Name],[Added_On])VALUES(@Org_Id,@Folder_Id,@Type,@Title,@Description,@Img,@Yt_Id,@Status,@Added_By_Id,@Added_By_Name,@Added_On)", con);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Folder_Id", "0");
                cmd.Parameters.AddWithValue("@Type", "Reel");  // Example for dropdown
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Img", "");
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Yt_Id", txtytlnk.Text);
                cmd.Parameters.AddWithValue("@Status", "1");  // Example for status dropdown
                cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                cmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);


                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Reel Added Successfully!')", true);
            }
            catch (Exception ex)
            {
                t.Rollback();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();
                txtTitle.Text = "";
                txtytlnk.Text = "";
                txtDescription.Text = "";
            }
        }
        else if (btnSubmit.Text == "Update")
        {
            try
            {
                cmd = new SqlCommand(@"UPDATE [goldenlife].[Photo_Reels_Master] SET [Org_Id] = @Org_Id,[Title] = @Title,[Description] = @Description,[Yt_Id] = @Yt_Id WHERE [Org_Id]=@Org_Id and [Sr_No]=@Sr_No ", con);
                cmd.Parameters.AddWithValue("@Sr_No", lblImageID.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Yt_Id", txtytlnk.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);


                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Reel Updated Successfully!');setTimeout(function(){ window.location.href = 'View_Reels.aspx'; }, 1000);", true);
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
    }

    protected void BindDetails()
    {
        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Photo_Reels_Master] where [Sr_No]=" + lblImageID.Text, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtTitle.Text = dr["Title"].ToString();
                txtDescription.Text = dr["Description"].ToString();
                txtytlnk.Text = dr["Yt_Id"].ToString();
            }


        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }
}