using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

/// <summary>
/// Summary description for BindDropdown
/// </summary>
public class BindDropdown
{
    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd;
    public BindDropdown()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void bindDropCustomer(DropDownList drop, string Org_Id)
    {
        cmd = new SqlCommand("select * from [hillsopt].[hillsopt].[Customer] where [Org_Id]=@Org_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
        try
        {
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                drop.DataSource = dt;
                drop.DataTextField = "Name";
                drop.DataValueField = "Id";
                drop.DataBind();
                drop.Items.Insert(0, new ListItem("All", "-1"));
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            con.Close();
        }
    }
    public void bindOrderNo(DropDownList drop, string Org_Id, string Customer_Id)
    {
        cmd = new SqlCommand("select * from [hillsopt].[hillsopt].[Order_List_Master] where [Org_Id]=@Org_Id and [Customer_Id]=@Customer_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
        cmd.Parameters.AddWithValue("@Customer_Id", Customer_Id);
        try
        {
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                drop.DataSource = dt;
                drop.DataTextField = "Order_No";
                drop.DataValueField = "Order_Id";
                drop.DataBind();
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            con.Close();
        }
    }
}