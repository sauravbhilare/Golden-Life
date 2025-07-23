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
                if (btnSubmit.Text == "Update")
                {
                    RequiredFieldValidator1.Enabled = false;
                }
            }
            ddlFolderBind();
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
                cmd.Parameters.AddWithValue("@Folder_Id", ddlFolder.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", "Photo");  // Example for dropdown
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
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
                    while (File.Exists("~/Attachment/PhotoReelsImg/" + f1));

                    if (IsValidFile)
                    {
                        ImageFile.SaveAs(Server.MapPath("~/Attachment/PhotoReelsImg/" + f1));
                        cmd.Parameters.AddWithValue("@Img", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Image File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Img", "");
                }
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@Yt_Id", "");
                cmd.Parameters.AddWithValue("@Status", "1");  // Example for status dropdown
                cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                cmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);


                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Image Added Successfully!')", true);
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
                txtDescription.Text = "";
            }
        }
        else if (btnSubmit.Text == "Update")
        {
            try
            {
                cmd = new SqlCommand(@"UPDATE [goldenlife].[Photo_Reels_Master] SET [Org_Id] = @Org_Id,[Folder_Id]=@Folder_Id,[Title] = @Title,[Description] = @Description,[Img] = @Img WHERE [Org_Id]=@Org_Id and [Sr_No]=@Sr_No ", con);
                cmd.Parameters.AddWithValue("@Sr_No", lblImageID.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Folder_Id", ddlFolder.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", "Photo");  // Example for dropdown
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
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
                    while (File.Exists("~/Attachment/PhotoReelsImg/" + f1));

                    if (IsValidFile)
                    {
                        ImageFile.SaveAs(Server.MapPath("~/Attachment/PhotoReelsImg/" + f1));
                        cmd.Parameters.AddWithValue("@Img", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Image File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Img", lblImage.Text);
                }
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);


                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Image Updated Successfully!');setTimeout(function(){ window.location.href = 'View_Images.aspx'; }, 1000);", true);
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
                ddlFolder.SelectedValue = dr["Folder_Id"].ToString();
                txtTitle.Text = dr["Title"].ToString();
                txtDescription.Text = dr["Description"].ToString();
                lblImage.Text = dr["Img"].ToString();
            }


        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void lnkAddFolder_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#FolderModal').modal('show');", true);
    }

    protected void btnFolderAdd_Click(object sender, EventArgs e)
    {
        try
        {
            cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Folder_Master]([Org_Id],[Folder_Name],[Added_On]) VALUES(@Org_Id,@Folder_Name,@Added_On)", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            cmd.Parameters.AddWithValue("@Folder_Name", txtFolderName.Text);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);

            con.Open();
            cmd.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Folder Added Successfully!')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
        }
        finally
        {
            con.Close();
            ddlFolder.Items.Clear();
            ddlFolderBind();
        }
    }

    protected void ddlFolderBind()
    {
        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Folder_Master] where [Org_Id]=@Org_Id order by [Folder_Id]", con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ddlFolder.DataSource = dt;
            ddlFolder.DataTextField = "Folder_Name";
            ddlFolder.DataValueField = "Folder_Id";
            ddlFolder.DataBind();
            ddlFolder.Items.Insert(0, new ListItem("---Select Folder---", "-1"));
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }
}