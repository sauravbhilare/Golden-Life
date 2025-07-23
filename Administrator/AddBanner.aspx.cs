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

public partial class Administrator_PlaceOrder : System.Web.UI.Page
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
            ddlType.Enabled = false;
            //BindCustomer();
            if (Request.QueryString["Sr_No"] != null)
            {
                lblBannerId.Text = Request.QueryString["Sr_No"].ToString();
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
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Banner_Promo_Master]([Org_Id],[Type],[Image],[Is_Redirect],[Url],[Added_By_Id],[Added_By_Name],[Added_On])VALUES(@Org_Id,@Type,@Image,@Is_Redirect,@Url,@Added_By_Id,@Added_By_Name,@Added_On)", con);

                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (ImageFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(ImageFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = ImageFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists("~/Attachment/BannerPromoImg/" + f1));

                    if (IsValidFile)
                    {
                        ImageFile.SaveAs(Server.MapPath("~/Attachment/BannerPromoImg/" + f1));
                        cmd.Parameters.AddWithValue("@Image", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Photo File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", "");
                }
                cmd.Parameters.AddWithValue("@Is_Redirect", ddlRedirect.SelectedValue);
                cmd.Parameters.AddWithValue("@Url", txtUrl.Text);
                cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                cmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Banner Image Added Successfully!')", true);
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();
                ddlRedirect.ClearSelection();
                txtUrl.Text = "";

            }
        }
        else if (btnSubmit.Text == "Update")
        {
            try
            {
                cmd = new SqlCommand(@"UPDATE [goldenlife].[Banner_Promo_Master] SET [Type] = @Type,[Image] = @Image,[Is_Redirect] = @Is_Redirect,[Url] = @Url WHERE Org_Id=@Org_Id and Sr_No=@Sr_No", con);
                cmd.Parameters.AddWithValue("@Sr_No", lblBannerId.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (ImageFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(ImageFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = ImageFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists("~/Attachment/BannerPromoImg/" + f1));

                    if (IsValidFile)
                    {
                        ImageFile.SaveAs(Server.MapPath("~/Attachment/BannerPromoImg/" + f1));
                        cmd.Parameters.AddWithValue("@Image", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Photo File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Image", lblImage.Text);
                }
                cmd.Parameters.AddWithValue("@Is_Redirect", ddlRedirect.SelectedValue);
                cmd.Parameters.AddWithValue("@Url", txtUrl.Text);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Banner Updated Successfully!');setTimeout(function(){ window.location.href = 'View_Banner.aspx'; }, 1000);", true);
            }
            catch (Exception ex)
            {
                t.Rollback();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();
                // ddlCustomer.ClearSelection();
                ddlType.ClearSelection();
                //txtAmount.Text = "";
                //txtRemarks.Text = "";

            }
        }
    }


    protected void BindDetails()
    {

        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Banner_Promo_Master] where [Sr_No]=" + lblBannerId.Text, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ddlType.SelectedValue = dr["Type"].ToString();
                lblImage.Text = dr["Image"].ToString();
                ddlRedirect.SelectedValue = dr["Is_Redirect"].ToString();
                txtUrl.Text = dr["Url"].ToString();
            }


        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }
    protected void btnAddCutomer_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            cmd = new SqlCommand("INSERT INTO [hillsopt].[Customer]([Name],[Mobile],[Address],[AddedOn],[Added_By_Id],[Added_By],[Org_Id])VALUES(@Name,@Mobile,@Address,@AddedOn,@Added_By_Id,@Added_By,@Org_Id)", con);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Mobile", txtmobile.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddrress.Text);
            cmd.Parameters.AddWithValue("@AddedOn", DateTime.Now);
            cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
            cmd.Parameters.AddWithValue("@Added_By", lblName.Text);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);


            cmd.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Customer Added Successfully!')", true);
        }
        catch (Exception ex)
        {
            ex.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Error Occured...!')", true);
        }
        finally
        {
            con.Close();
            txtName.Text = "";
            txtmobile.Text = "";
            //txtShippingAddress.Text = "";
            txtAddrress.Text = "";
            // BindCustomer();
        }
    }



    protected void ddlRedirect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRedirect.SelectedValue == "Yes")
        {
            URLDiv.Visible = true;
        }
        else
        {
            URLDiv.Visible = false;
        }
    }
}