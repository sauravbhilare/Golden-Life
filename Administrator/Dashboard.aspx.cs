using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

public partial class Administrator_Dashboard : System.Web.UI.Page
{
    static string conString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
    SqlConnection con = new SqlConnection(conString);
    SqlCommand cmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblOrgId.Text = "1";
            //this.BindFilterTiles("All");
            bindCounts();
            bindTodaysPayments();
            bindTodaysCustomers();
        }
    }
    protected void bindCounts()
    {
        int ttlUsers = 0;
        int ttlActiveUsers = 0;
        int ttlInActiveUsers = 0;
        int ttlExpired = 0;
        try
        {
            con.Open();
            cmd = new SqlCommand(@"Select Count(Id) FROM [goldenlife].[goldenlife].[User_Master] Where Org_Id=@Org_Id", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            ttlUsers = Convert.ToInt32(cmd.ExecuteScalar());
            lblttlUsers.Text = ttlUsers.ToString();

            cmd = new SqlCommand(@"Select Count(Id) FROM [goldenlife].[goldenlife].[User_Master] Where Org_Id=@Org_Id And Is_Subscribed=1", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            ttlActiveUsers = Convert.ToInt32(cmd.ExecuteScalar());
            lblttlActive.Text = ttlActiveUsers.ToString();

            cmd = new SqlCommand(@"SELECT COUNT(Id) FROM [goldenlife].[goldenlife].[User_Master] WHERE [Sub_Valid_Till] < GETDATE() And Org_Id=@Org_Id ", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            ttlExpired = Convert.ToInt32(cmd.ExecuteScalar());
            lblExpired.Text = ttlExpired.ToString();

            cmd = new SqlCommand(@"Select Count(Id) FROM [goldenlife].[goldenlife].[User_Master] Where Org_Id=@Org_Id And Is_Subscribed=0", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            ttlInActiveUsers = Convert.ToInt32(cmd.ExecuteScalar());
            lblInActive.Text = ttlInActiveUsers.ToString();

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

    protected void bindTodaysPayments()
    {
        try
        {
            cmd = new SqlCommand(@"SELECT *, 
                                      ISNULL((SELECT Plan_Name 
                                              FROM [goldenlife].[goldenlife].[Subscription_Plan_Master] 
                                              WHERE [Sr_No]=p.[Subscription_Id]), 0) AS Plan_Name
                               FROM [goldenlife].[goldenlife].[Payment_Master] p 
                               WHERE Org_Id=@Org_Id 
                               AND CAST([AddedOn] AS DATE) = CAST(GETDATE() AS DATE)
                               ORDER BY [Sr_No] DESC", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            gvPayments.DataSource = dt;
            gvPayments.DataBind();
        }
        catch (Exception ex)
        {
            // Optionally log or handle the exception
        }
        finally
        {
            con.Close();
        }
    }

    protected void bindTodaysCustomers()
    {
        try
        {
            cmd = new SqlCommand(@"SELECT * 
                               FROM [goldenlife].[goldenlife].[User_Master] 
                               WHERE Org_Id = @Org_Id 
                               AND CAST([Added_On] AS DATE) = CAST(GETDATE() AS DATE)
                               ORDER BY [Id] DESC", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            gvCustomers.DataSource = dt;
            gvCustomers.DataBind();
        }
        catch (Exception ex)
        {
            // Optionally log or handle the exception
        }
        finally
        {
            con.Close();
        }
    }


    private void BindFilterTiles(string condition)
    {
        string Order = "";
        string stockIn = "";
        string StockOut = "";
        string Tickets = "";
        try
        {
            if (condition == "Today")
            {
                Order = " and convert(date,Ordered_On,103)=convert(date,getdate(),103) ";
                stockIn = " and convert(date,Added_On,103)=convert(date,getdate(),103) ";
                StockOut = " and convert(date,Ordered_On,103)=convert(date,getdate(),103) ";
                Tickets = " and convert(date,AddedOn,103)=convert(date,getdate(),103) ";
            }
            else if (condition == "Yesterday")
            {
                Order = " and convert(date,Ordered_On,103)=convert(date,DATEADD(day, -1, GETDATE()),103) ";
                stockIn = " and convert(date,Added_On,103)=convert(date,DATEADD(day, -1, GETDATE()),103) ";
                StockOut = " and convert(date,Ordered_On,103)=convert(date,DATEADD(day, -1, GETDATE()),103) ";
                Tickets = " and convert(date,AddedOn,103)=convert(date,DATEADD(day, -1, GETDATE()),103) ";
            }
            else if (condition == "ThisMonth")
            {
                Order = " and convert(date,Ordered_On,103) between convert(date,DATEADD(month, DATEDIFF(month, 0, getdate()), 0),103) and convert(date,getdate(),103) ";
                stockIn = " and convert(date,Added_On,103) between convert(date,DATEADD(month, DATEDIFF(month, 0, getdate()), 0),103) and convert(date,getdate(),103) ";
                StockOut = " and convert(date,Ordered_On,103) between convert(date,DATEADD(month, DATEDIFF(month, 0, getdate()), 0),103) and convert(date,getdate(),103) ";
                Tickets = " and convert(date,AddedOn,103) between convert(date,DATEADD(month, DATEDIFF(month, 0, getdate()), 0),103) and convert(date,getdate(),103) ";
            }
            else if (condition == "LastMonth")
            {
                Order = " and convert(date,Ordered_On,103) between convert(date,DATEADD(m,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()), 0)),103) and convert(date,DATEADD(d,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),103) ";
                stockIn = " and convert(date,Added_On,103) between convert(date,DATEADD(m,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()), 0)),103) and convert(date,DATEADD(d,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),103) ";
                StockOut = "and convert(date,Ordered_On,103) between convert(date,DATEADD(m,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()), 0)),103) and convert(date,DATEADD(d,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),103) ";
                Tickets = " and convert(date,AddedOn,103) between convert(date,DATEADD(m,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()), 0)),103) and convert(date,DATEADD(d,-1,DATEADD(mm, DATEDIFF(m,0,GETDATE()),0)),103) ";
            }
            else
            {

            }



            SqlCommand cmd = new SqlCommand(@" select 
            (select count(1) from Order_Details where Order_Status='Pending' and Type='Order' " + Order
            + @") as PendingOrders,(select count(1) from Order_Details where Order_Status='Completed' and Type='Order' " + Order
            + @") as CompletedOrder,(select count(1) from [hillsopt].[hillsopt].[Stock_In] where Org_Id=@Org_Id " + stockIn
            + @") as stockIn,(select count(1) from [hillsopt].[hillsopt].[Order_Details] where Order_Status='Completed'" + StockOut
            + @") as stockout ,(select  count(1) from [hillsopt].[hillsopt].[Raise_Ticket] where [Status]='Pending' " + Tickets
            + @")  as PendingTickets,(select count(1) from [hillsopt].[hillsopt].[Raise_Ticket] where [Status]='Resolved' " + Tickets
            + @" ) as CompletedTickets", con);

            cmd.Parameters.AddWithValue("@Org_Id", lblOrgId.Text);

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                // pendingOrder.InnerText = row["PendingOrders"].ToString();
                CompletedOrder.InnerText = row["CompletedOrder"].ToString();
                StockIn.InnerText = row["stockIn"].ToString();
                stockOut.InnerText = row["stockout"].ToString();
                pendingTickets.InnerText = row["PendingTickets"].ToString();
                resolvedTickets.InnerText = row["CompletedTickets"].ToString();
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
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        this.BindFilterTiles((sender as LinkButton).CommandArgument.ToString());
    }
}