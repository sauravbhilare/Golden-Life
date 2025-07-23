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
            txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //BindCustomer();
            if (Request.QueryString["Sr_No"] != null)
            {
                lblEventId.Text = Request.QueryString["Sr_No"].ToString();
                BindDetails();
                btnSubmit.Text = "Update";
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        Random r = new Random();
        //string File1ToExclude = "";
        string File2ToExclude = "";

        if (btnSubmit.Text == "Submit")
        {
            con.Open();
            SqlTransaction t = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Event_News_Master]
                ([Org_Id], [Type], [Title], [Description], [Thumbnail], [Date], [From_Time], [To_Time], [Location], [Capacity], [Status], [Added_By_Id], [Added_By_Name], [Added_On])
                VALUES (@Org_Id, @Type, @Title, @Description, @Thumbnail, @Date, @From_Time, @To_Time, @Location, @Capacity, @Status, @Added_By_Id, @Added_By_Name, @Added_On);
                SELECT SCOPE_IDENTITY();", con, t);

                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue); // Example for dropdown
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (ThumbnailFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(ThumbnailFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = ThumbnailFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists("~/Attachment/EventNewsImg/" + f1));

                    if (IsValidFile)
                    {
                        ThumbnailFile.SaveAs(Server.MapPath("~/Attachment/EventNewsImg/" + f1));
                        cmd.Parameters.AddWithValue("@Thumbnail", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Image File is not valid!!')", true);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Thumbnail", "");
                }

                // Ensure correct date and time formats
                cmd.Parameters.AddWithValue("@Date", txtdate.Text);
                cmd.Parameters.AddWithValue("@From_Time", txtFromTime.Text);
                cmd.Parameters.AddWithValue("@To_Time", txtToTime.Text);
                cmd.Parameters.AddWithValue("@Location", txtLocation.Text);
                cmd.Parameters.AddWithValue("@Capacity", txtCapacity.Text); // Assuming Capacity is an integer
                cmd.Parameters.AddWithValue("@Status", 1); // Example for status dropdown
                cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                cmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);

                int EventId = Convert.ToInt32(cmd.ExecuteScalar());

                if (FileUpload1.HasFiles)
                {
                    for (int i = 0; i < FileUpload1.PostedFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = FileUpload1.PostedFiles[i];

                        if (postedFile.FileName == File2ToExclude) continue;

                        cmd = new SqlCommand("INSERT INTO [goldenlife].[Event_Images]([Org_Id], [Event_Id], [Image], [Added_By_Id], [Added_By_Name], [Added_On]) VALUES (@Org_Id, @Event_Id, @Image, @Added_By_Id, @Added_By_Name, @Added_On)", con, t);

                        cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                        cmd.Parameters.AddWithValue("@Event_Id", EventId);
                        cmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                        cmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                        cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);

                        if (postedFile.ContentLength > 0)
                        {
                            string fileName2 = System.IO.Path.GetFileName(postedFile.FileName);
                            string multifile1 = r.Next(1, 10000) + fileName2;
                            postedFile.SaveAs(Server.MapPath("~/Attachment/EventNewsImg/") + multifile1);

                            cmd.Parameters.AddWithValue("@Image", multifile1);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }


                t.Commit();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "success", "success('Event & News Added Successfully!')", true);
            }
            catch (Exception ex)
            {
                t.Rollback();
                // Log the exception details (optional)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();

                ddlType.ClearSelection();
                txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTitle.Text = "";
                txtFromTime.Text = "";
                txtToTime.Text = "";
                txtCapacity.Text = "";
                txtLocation.Text = "";
                txtDescription.Text = "";
            }
        }
        else if (btnSubmit.Text == "Update")
        {
            con.Open();
            SqlTransaction t = con.BeginTransaction();
            try
            {
                // Update command
                SqlCommand cmd = new SqlCommand(@"UPDATE [goldenlife].[Event_News_Master] 
                                          SET [Org_Id] = @Org_Id, [Type] = @Type, [Title] = @Title, [Description] = @Description,
                                              [Thumbnail] = @Thumbnail, [Date] = @Date, [From_Time] = @From_Time, 
                                              [To_Time] = @To_Time, [Location] = @Location, [Capacity] = @Capacity 
                                          WHERE [Sr_No] = @Sr_No", con, t);
                cmd.Parameters.AddWithValue("@Sr_No", lblEventId.Text);
                cmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

                string f1 = "";
                string fileName = string.Empty;
                bool IsValidFile = false;
                if (ThumbnailFile.HasFile)
                {
                    string[] validFileTypes = { ".png", ".jpg", ".jpeg", ".bmp" };
                    string extension = System.IO.Path.GetExtension(ThumbnailFile.PostedFile.FileName);

                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (extension.ToLower() == validFileTypes[i])
                        {
                            IsValidFile = true;
                            break;
                        }
                    }
                    fileName = ThumbnailFile.FileName;

                    do
                    {
                        f1 = r.Next(1, 10000) + fileName;
                    }
                    while (File.Exists(Server.MapPath("~/Attachment/EventNewsImg/") + f1));

                    if (IsValidFile)
                    {
                        ThumbnailFile.SaveAs(Server.MapPath("~/Attachment/EventNewsImg/" + f1));
                        cmd.Parameters.AddWithValue("@Thumbnail", f1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Thumbnail File is not valid!!')", true);
                        return;
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Thumbnail", lblImage.Text);
                }

                cmd.Parameters.AddWithValue("@Date", txtdate.Text);
                cmd.Parameters.AddWithValue("@From_Time", txtFromTime.Text);
                cmd.Parameters.AddWithValue("@To_Time", txtToTime.Text);
                cmd.Parameters.AddWithValue("@Location", txtLocation.Text);
                cmd.Parameters.AddWithValue("@Capacity", txtCapacity.Text);
                cmd.ExecuteNonQuery();

                // If FileUpload1.HasFiles is true, delete existing images and insert new ones
                if (FileUpload1.HasFiles)
                {
                    // Delete existing images
                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM [goldenlife].[Event_Images] WHERE [Org_Id] = @Org_Id AND [Event_Id] = @Event_Id", con, t);
                    deleteCmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                    deleteCmd.Parameters.AddWithValue("@Event_Id", lblEventId.Text);
                    deleteCmd.ExecuteNonQuery();

                    // Insert new images
                    for (int i = 0; i < FileUpload1.PostedFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = FileUpload1.PostedFiles[i];
                        if (postedFile.ContentLength > 0)
                        {
                            string fileName2 = System.IO.Path.GetFileName(postedFile.FileName);
                            string multifile1 = r.Next(1, 10000) + fileName2;
                            postedFile.SaveAs(Server.MapPath("~/Attachment/EventNewsImg/") + multifile1);

                            SqlCommand insertCmd = new SqlCommand("INSERT INTO [goldenlife].[Event_Images] ([Org_Id], [Event_Id], [Image], [Added_By_Id], [Added_By_Name], [Added_On]) VALUES (@Org_Id, @Event_Id, @Image, @Added_By_Id, @Added_By_Name, @Added_On)", con, t);
                            insertCmd.Parameters.AddWithValue("@Org_Id", lblOrgid.Text);
                            insertCmd.Parameters.AddWithValue("@Event_Id", lblEventId.Text);
                            insertCmd.Parameters.AddWithValue("@Image", multifile1);
                            insertCmd.Parameters.AddWithValue("@Added_By_Id", lblUserId.Text);
                            insertCmd.Parameters.AddWithValue("@Added_By_Name", lblName.Text);
                            insertCmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                t.Commit();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "success('Event & News Updated Successfully!'); setTimeout(function(){ window.location.href = 'View_Event_News.aspx'; }, 1000);", true);
            }
            catch (Exception ex)
            {
                t.Rollback();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "error('Server is Busy!!')", true);
            }
            finally
            {
                con.Close();
                ddlType.ClearSelection();
                txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTitle.Text = "";
                txtFromTime.Text = "";
                txtToTime.Text = "";
                txtCapacity.Text = "";
                txtLocation.Text = "";
                txtDescription.Text = "";
            }
        }
    }


    protected void BindDetails()
    {
        try
        {
            cmd = new SqlCommand("Select * from [goldenlife].[goldenlife].[Event_News_Master] where [Sr_No]=" + lblEventId.Text, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtdate.Text = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                ddlType.SelectedValue = dr["Type"].ToString();
                if (ddlType.SelectedValue == "News")
                {
                    fromdiv.Visible = false;
                    todiv.Visible = false;
                    locationdiv.Visible = false;
                    CapacityDiv.Visible = false;
                }
                else
                {

                    fromdiv.Visible = true;
                    todiv.Visible = true;
                    locationdiv.Visible = true;
                    CapacityDiv.Visible = true;
                }
                txtTitle.Text = dr["Title"].ToString();
                txtFromTime.Text = dr["From_Time"].ToString();
                txtToTime.Text = dr["To_Time"].ToString();
                txtCapacity.Text = dr["Capacity"].ToString();
                txtLocation.Text = dr["Location"].ToString();
                txtDescription.Text = dr["Description"].ToString();
                lblImage.Text = dr["Thumbnail"].ToString();
            }


        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "News")
        {
            fromdiv.Visible = false;
            todiv.Visible = false;
            locationdiv.Visible = false;
            CapacityDiv.Visible = false;
        }
        else
        {

            fromdiv.Visible = true;
            todiv.Visible = true;
            locationdiv.Visible = true;
            CapacityDiv.Visible = true;
        }
    }
}