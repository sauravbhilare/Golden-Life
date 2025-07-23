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

public partial class Admin_AdminLogin : System.Web.UI.Page
{
    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd, cmd1;
    SqlTransaction t;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "select * from [goldenlife].[goldenlife].[Admin_Master] where Username=@Username and Password=@Password";
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (txtUsername.Text.ToString() == dr["Username"].ToString() && txtPassword.Text.ToString() == dr["Password"].ToString())
                    {
                        //Alert.Show("Login Successful");

                        Response.Cookies["goldenLife"]["Username"] = dr["Username"].ToString();
                        Response.Cookies["goldenLife"]["OrgId"] = dr["Org_Id"].ToString();
                        Response.Cookies["goldenLife"]["Name"] = dr["Name"].ToString();
                        Response.Cookies["goldenLife"]["Role"] = dr["Role"].ToString();
                        Response.Cookies["goldenLife"]["User_Id"] = dr["Id"].ToString();
                        Response.Cookies["goldenLife"].Expires = DateTime.Now.AddHours(8);

                        Response.Redirect("~/Administrator/Dashboard.aspx", false);
                        txtUsername.Text = null;
                        txtPassword.Text = null;
                    }
                    else
                    {

                        txtPassword.Text = "";
                    }
                }
            }
            else
            {
                lblInValid.Visible = true;
                txtPassword.Text = "";
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
}