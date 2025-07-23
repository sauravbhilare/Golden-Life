using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Administrator_AdminMaster : System.Web.UI.MasterPage
{
    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd, cmd1;
    SqlTransaction t;
    protected void Page_InIt(object sender, EventArgs e)
    {
        if (Request.Cookies["goldenLife"] != null)
        {
            if ((Request.Cookies["goldenLife"]["Username"] != null) && (Request.Cookies["goldenLife"]["Name"] != null))
            {
                orgg_Id.Text = Request.Cookies["goldenLife"]["OrgId"].ToString();
                Name.Text = Request.Cookies["goldenLife"]["Name"].ToString();
                username.Text = Request.Cookies["goldenLife"]["Username"].ToString();
                Role.Text = Request.Cookies["goldenLife"]["Role"].ToString();
                user_Id.Text = Request.Cookies["goldenLife"]["User_Id"].ToString();
            }
        }
        else
        {
            Response.Redirect("~/AdminLogin.aspx", true);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            
        }
    }

    protected void lbrnChane_Click(object sender, EventArgs e)
    {
        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Staff_Details] where [Sr_No]=@user_Id and [Password]=@Password", con);
            cmd.Parameters.AddWithValue("@user_Id", user_Id.Text);
            cmd.Parameters.AddWithValue("@Password", txtOldPssword.Text.Trim());
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count>0)
            {
                cmd1 = new SqlCommand("Update [goldenlife].[goldenlife].[Staff_Details] set [Password]=@Password where [Sr_No]=@user_Id", con);
                cmd1.Parameters.AddWithValue("@user_Id", user_Id.Text);
                cmd1.Parameters.AddWithValue("@Password", txtNewPassword.Text.Trim());
                con.Open();
                cmd1.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Password Changed Successfully!')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Old Password is Incorrect!!')", true);
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
        }
        finally
        {
            con.Close();
            txtOldPssword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
        }
    }
}
