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
            //BindCustomer();
            if (Request.QueryString["Id"] != null)
            {
                lblCustomerId.Text = Request.QueryString["Id"].ToString();
                BindDetails();
                btnSubmit.Text = "Update";
                if (btnSubmit.Text == "Update")
                {
                    RequiredFieldValidator6.Enabled = false;
                }
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
                SqlCommand cmd = new SqlCommand(@"INSERT INTO [goldenlife].[User_Master]([Org_Id],[Name],[Spouse_Name],[Last_Name],[DOB],[Spouse_DOB],[Anniversary_Date],[Photo],[Mobile],[Password],[OTP],[GCM],[Is_Subscribed],[Payment_Id],[Status],[Added_On])VALUES(@Org_Id,@Name,@Spouse_Name,@Last_Name,@DOB,@Spouse_DOB,@Anniversary_Date,@Photo,@Mobile,@Password,@OTP,@GCM,@Is_Subscribed,@Payment_Id,@Status,@Added_On)", con);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Name", txtHusbandName.Text);
                cmd.Parameters.AddWithValue("@Spouse_Name", txtWifeName.Text);
                cmd.Parameters.AddWithValue("@Last_Name", txtLastName.Text);
                cmd.Parameters.AddWithValue("@DOB", txtHusbandDOB.Text);
                cmd.Parameters.AddWithValue("@Spouse_DOB", txtWifeDOB.Text);
                cmd.Parameters.AddWithValue("@Anniversary_Date", txtAnniversaryDate.Text);
                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (PhotoFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(PhotoFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = PhotoFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists("~/Attachment/UserImg/" + f1));

                    if (IsValidFile)
                    {
                        PhotoFile.SaveAs(Server.MapPath("~/Attachment/UserImg/" + f1));
                        cmd.Parameters.AddWithValue("@Photo", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Thumbnail File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Photo", "");
                }
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.Parameters.AddWithValue("@Password", "staff@123");
                cmd.Parameters.AddWithValue("@OTP", "");
                cmd.Parameters.AddWithValue("@GCM", "");

                cmd.Parameters.AddWithValue("@Is_Subscribed", 0); // Assuming checkbox for subscription
                cmd.Parameters.AddWithValue("@Payment_Id", "");
                cmd.Parameters.AddWithValue("@Status", 1); // Assuming dropdown for status
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Event & News Added Successfully!')", true);
            }
            catch (Exception ex)
            {
                t.Rollback();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();

                txtHusbandName.Text = "";
                txtWifeName.Text = "";
                txtLastName.Text = "";
                txtHusbandDOB.Text = "";
                txtWifeDOB.Text = "";
                txtAnniversaryDate.Text = "";
                txtMobile.Text = "";
            }
        }
        else if (btnSubmit.Text == "Update")
        {
            try
            {
                cmd = new SqlCommand(@"UPDATE [goldenlife].[User_Master] SET [Name] = @Name,[Spouse_Name] = @Spouse_Name,[Last_Name] = @Last_Name,[DOB] = @DOB,[Spouse_DOB] = @Spouse_DOB,[Anniversary_Date] = @Anniversary_Date,[Photo] = @Photo,[Mobile] = @Mobile WHERE [Org_Id]=@Org_Id AND [Id]=@Id ", con);
                cmd.Parameters.AddWithValue("@Id", lblCustomerId.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Name", txtHusbandName.Text);
                cmd.Parameters.AddWithValue("@Spouse_Name", txtWifeName.Text);
                cmd.Parameters.AddWithValue("@Last_Name", txtLastName.Text);
                cmd.Parameters.AddWithValue("@DOB", txtHusbandDOB.Text);
                cmd.Parameters.AddWithValue("@Spouse_DOB", txtWifeDOB.Text);
                cmd.Parameters.AddWithValue("@Anniversary_Date", txtAnniversaryDate.Text);
                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (PhotoFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(PhotoFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = PhotoFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists("~/Attachment/UserImg/" + f1));

                    if (IsValidFile)
                    {
                        PhotoFile.SaveAs(Server.MapPath("~/Attachment/UserImg/" + f1));
                        cmd.Parameters.AddWithValue("@Photo", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Thumbnail File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Photo", lblImage.Text);
                }
                cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                cmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "Suucess('Event & News Updated Successfully!');setTimeout(function(){ window.location.href = 'View_Customer.aspx'; }, 1000);", true);
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
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[User_Master] where [Org_Id]=@Org_Id And [Id]=" + lblCustomerId.Text, con);
            cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtHusbandName.Text = dr["Name"].ToString();
                txtWifeName.Text = dr["Spouse_Name"].ToString();
                txtLastName.Text = dr["Last_Name"].ToString();

                // Check if date columns are not null and format accordingly
                if (dr["DOB"] != DBNull.Value)
                {
                    txtHusbandDOB.Text = Convert.ToDateTime(dr["DOB"]).ToString("yyyy-MM-dd");
                }

                if (dr["Spouse_DOB"] != DBNull.Value)
                {
                    txtWifeDOB.Text = Convert.ToDateTime(dr["Spouse_DOB"]).ToString("yyyy-MM-dd");
                }

                if (dr["Anniversary_Date"] != DBNull.Value)
                {
                    txtAnniversaryDate.Text = Convert.ToDateTime(dr["Anniversary_Date"]).ToString("yyyy-MM-dd");
                }

                lblImage.Text = dr["Photo"].ToString();
                txtMobile.Text = dr["Mobile"].ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception (optional)
        }
        finally
        {
            // Cleanup if needed (optional)
        }
    }



}