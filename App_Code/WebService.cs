using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web.Script.Services;
using System.Globalization;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    static String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

    static string Org_Id = "1";
    SqlTransaction t;

    static SqlTransaction transaction;
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void GenerateOTP(string Mobile)
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd, cmd1;

        Status status = new Status();
        status.status = 0;
        string Blank = "";
        int res = 0;
        if (Mobile.Length == 10)
        {
            bool IsExist = false, OTPSent = false;
            // string OTP = new Random().Next(1000, 9999).ToString();
            string OTP = "1234";
            try
            {
                con.Open();
                cmd1 = new SqlCommand(@"select 1 from [goldenlife].[Admin_Master] where Mobile=@Mobile and Org_Id=" + Org_Id + " and Status=1", con);
                cmd1.Parameters.AddWithValue("@Mobile", Mobile);

                DataTable dt1 = new DataTable();
                new SqlDataAdapter(cmd1).Fill(dt1);
                if (dt1.Rows.Count > 0)
                {

                    if (Mobile == "1234567890" || Mobile == "9699416999" || Mobile == "9876543210" || Mobile == "8097455599" || Mobile == "8097455577")
                    {
                        OTP = "1234";
                    }
                    IsExist = true;
                    cmd = new SqlCommand(@"update [goldenlife].[Admin_Master] set Otp=@Otp  where Mobile=@Mobile  and Org_Id=" + Org_Id + "  and Status=1", con);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    cmd.Parameters.AddWithValue("@Otp", OTP);
                    cmd.ExecuteNonQuery();
                    status.status = 1;
                }
                else
                {
                    cmd = new SqlCommand(@"select 1 from [goldenlife].[User_Master] where Mobile=@Mobile and Org_Id=" + Org_Id + "  and Status=1", con);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);

                    //con.Open();
                    DataTable dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        if (Mobile == "1234567890" || Mobile == "9699416999" || Mobile == "9876543210" || Mobile == "8097455599" || Mobile == "8097455577")
                        {
                            OTP = "1234";
                        }
                        IsExist = true;
                        cmd = new SqlCommand(@"update [goldenlife].[User_Master] set Otp=@Otp where Mobile=@Mobile and Org_Id=" + Org_Id + "  and Status=1", con);
                        cmd.Parameters.AddWithValue("@Mobile", Mobile);
                        cmd.Parameters.AddWithValue("@Otp", OTP);
                        cmd.ExecuteNonQuery();
                        status.status = 1;
                    }
                    else
                    {
                        cmd = new SqlCommand(@"insert into [goldenlife].[User_Master] ([Org_Id],[Husband_Name],[Wife_Name],[Last_Name],[Husband_DOB],
                    [Wife_DOB],[Anniversary_Date],[Photo],[Mobile],[Password],[OTP],[GCM],[Sub_Valid_Till],[Status],[Added_On])
                    values(@Org_Id,@Husband_Name,@Wife_Name,@Last_Name,@Husband_DOB,@Wife_DOB,@Anniversary_Date,@Photo,@Mobile,@Password,
                    @OTP,@GCM,@Sub_Valid_Till,@Status,@Added_On)", con);
                        cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
                        cmd.Parameters.AddWithValue("@Husband_Name", Blank);
                        cmd.Parameters.AddWithValue("@Wife_Name", Blank);
                        cmd.Parameters.AddWithValue("@Last_Name", Blank);
                        cmd.Parameters.AddWithValue("@Husband_DOB", Blank);
                        cmd.Parameters.AddWithValue("@Wife_DOB", Blank);
                        cmd.Parameters.AddWithValue("@Anniversary_Date", Blank);
                        cmd.Parameters.AddWithValue("@Photo", Blank);
                        cmd.Parameters.AddWithValue("@Mobile", Mobile);
                        cmd.Parameters.AddWithValue("@Password", Blank);
                        cmd.Parameters.AddWithValue("@OTP", OTP);
                        cmd.Parameters.AddWithValue("@GCM", Blank);
                        cmd.Parameters.AddWithValue("@Sub_Valid_Till", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", "1");
                        cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                        // res = cmd.ExecuteNonQuery();
                        if (res == 1) status.status = 1;

                    }
                }
            }

            catch (Exception ex)
            {
                string e = ex.ToString();
            }
            finally
            {
                con.Close();
            }

            if (status.status == 1)
            {

                //  SMS.SendOtp(mobile, OTP);
                //SMS.SendOtp1(mobile, OTP);
                //Whatsapp.SendText(mobile, "Dear Student " + OTP + " is your OTP for TikKar. Code is valid for 2 minutes only. DO NOT share this OTP with anyone.");
            }
        }


        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }


    [WebMethod]
    public void VerifyOTP(string mobile, string otp, string GCM)
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        VerificationStatus status = new VerificationStatus();
        status.status = 0;
        status.id = 0;
        status.role = "";

        if (otp.Length == 4)
        {
            string MobileNum = mobile;
            string OTP = otp;
            bool IsVerified = false;

            try
            {
                con.Open();
                cmd = new SqlCommand(@"select * from [goldenlife].[Admin_Master] where Mobile=@mobile and OTP=@otp and Org_Id=" + Org_Id + "", con);
                cmd.Parameters.AddWithValue("@mobile", MobileNum);
                cmd.Parameters.AddWithValue("@otp", OTP);
                DataTable dtotp = new DataTable();
                new SqlDataAdapter(cmd).Fill(dtotp);
                if (dtotp.Rows.Count > 0)
                {
                    cmd = new SqlCommand(@"update [goldenlife].[Admin_Master] set Otp=@Otp, GCM=@GCM where Mobile=@Mobile  and Org_Id=" + Org_Id + "", con);
                    cmd.Parameters.AddWithValue("@Mobile", MobileNum);
                    cmd.Parameters.AddWithValue("@GCM", GCM);
                    cmd.Parameters.AddWithValue("@Otp", OTP);
                    cmd.ExecuteNonQuery();

                    status.status = 2;
                    status.id = Convert.ToInt32(dtotp.Rows[0]["Id"].ToString());
                    status.role = "Admin";
                }
                else
                {
                    cmd = new SqlCommand(@"select * from [goldenlife].[User_Master] where mobile=@mobile and otp=@otp and Org_Id=" + Org_Id + "", con);
                    cmd.Parameters.AddWithValue("@mobile", MobileNum);
                    cmd.Parameters.AddWithValue("@otp", OTP);
                    DataTable dtotp1 = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dtotp1);
                    if (dtotp1.Rows.Count > 0)
                    {
                        cmd = new SqlCommand(@"update [goldenlife].[User_Master] set Otp=@Otp, GCM=@GCM where Mobile=@Mobile  and Org_Id=" + Org_Id + "", con);
                        cmd.Parameters.AddWithValue("@Mobile", MobileNum);
                        cmd.Parameters.AddWithValue("@GCM", GCM);
                        cmd.Parameters.AddWithValue("@Otp", OTP);
                        cmd.ExecuteNonQuery();

                        status.status = 1;
                        status.id = Convert.ToInt32(dtotp1.Rows[0]["Id"].ToString());
                        status.role = "User";
                        //string Name = dtotp1.Rows[0]["FName"].ToString().Trim();
                        //if (!String.IsNullOrEmpty(Name)) status.IsProfile = 1;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                con.Close();
            }
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void RegisterUser(string Husband_Name, string Wife_Name, string Last_Name, string Husband_DOB, string Wife_DOB,
        string Anniversary_Date, string Photo, string Mobile)
    {
        Status status = new Status();
        status.status = 0;
        string Blank = "";
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        // string OTP = new Random().Next(1000, 9999).ToString();
        string OTP = "1234";

        try
        {
            con.Open();


            cmd = new SqlCommand(@"select * from [goldenlife].[User_Master] where Mobile=@Mobile and Org_Id=" + Org_Id + "", con);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            DataTable dtotp = new DataTable();
            new SqlDataAdapter(cmd).Fill(dtotp);
            if (dtotp.Rows.Count > 0)
            {
                status.status = 2;
            }
            else
            {
                if (Mobile == "1234567890" || Mobile == "9699416999" || Mobile == "9876543210" || Mobile == "8097455599" || Mobile == "8097455577")
                {
                    OTP = "1234";
                }

                cmd = new SqlCommand(@"insert into [goldenlife].[User_Master] ([Org_Id],[Husband_Name],[Wife_Name],[Last_Name],[Husband_DOB],
                    [Wife_DOB],[Anniversary_Date],[Photo],[Mobile],[Password],[OTP],[GCM],[Sub_Valid_From],[Sub_Valid_Till],[Status],[Payment_Id],[Is_Subscribed],[Added_On])
                    values(@Org_Id,@Husband_Name,@Wife_Name,@Last_Name,@Husband_DOB,@Wife_DOB,@Anniversary_Date,@Photo,@Mobile,@Password,
                    @OTP,@GCM,@Sub_Valid_From,@Sub_Valid_Till,@Status,@Payment_Id,@Is_Subscribed,@Added_On)", con);
                cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
                cmd.Parameters.AddWithValue("@Husband_Name", Husband_Name);
                cmd.Parameters.AddWithValue("@Wife_Name", Wife_Name);
                cmd.Parameters.AddWithValue("@Last_Name", Last_Name);
                cmd.Parameters.AddWithValue("@Husband_DOB", Husband_DOB);
                cmd.Parameters.AddWithValue("@Wife_DOB", Wife_DOB);
                cmd.Parameters.AddWithValue("@Anniversary_Date", Anniversary_Date);
                cmd.Parameters.AddWithValue("@Photo", Photo);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Password", "staff@123");
                cmd.Parameters.AddWithValue("@OTP", OTP);
                cmd.Parameters.AddWithValue("@GCM", Blank);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", DBNull.Value);
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.Parameters.AddWithValue("@Payment_Id", "");
                cmd.Parameters.AddWithValue("@Is_Subscribed", "0");
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                int res = cmd.ExecuteNonQuery();

                if (res == 1) status.status = 1;
            }
        }
        catch (Exception ex)
        {
            String e = ex.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void UpdateProfile(string Id, string Husband_Name, string Wife_Name, string Last_Name, string Husband_DOB, string Wife_DOB,
        string Anniversary_Date, string Photo)
    {
        Status status = new Status();
        status.status = 0;

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();

            cmd = new SqlCommand(@"update [goldenlife].[User_Master]  set [Husband_Name]=@Husband_Name,[Wife_Name]=@Wife_Name,[Last_Name]=@Last_Name,[Husband_DOB]=@Husband_DOB,
                    [Wife_DOB]=@Wife_DOB,[Anniversary_Date]=@Anniversary_Date,[Photo]=@Photo where Id=@Id and Org_Id=@Org_Id", con);

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@Husband_Name", Husband_Name);
            cmd.Parameters.AddWithValue("@Wife_Name", Wife_Name);
            cmd.Parameters.AddWithValue("@Last_Name", Last_Name);
            cmd.Parameters.AddWithValue("@Husband_DOB", Husband_DOB);
            cmd.Parameters.AddWithValue("@Wife_DOB", Wife_DOB);
            cmd.Parameters.AddWithValue("@Anniversary_Date", Anniversary_Date);
            cmd.Parameters.AddWithValue("@Photo", Photo);

            int res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch
        {
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendUserImg()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/UserImg/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetUsers(string Id, string Role)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        string query = "";
        if(Role == "Admin")
        {
            if (Id != "")
            {
                query = " and Id=" + Id + " ";
            }

            cmd = new SqlCommand(@"SELECT * FROM [goldenlife].[Admin_Master] where 1=1 and Org_Id=" + Org_Id + " " + query + " ", con);

        }
        else
        {
            if (Id != "")
            {
                query = " and Id=" + Id + " ";
            }

            cmd = new SqlCommand(@"SELECT *, isnull(Convert(varchar(20),[Sub_Valid_From], 0), '') as Sub_Valid_From_Convert,
            isnull(Convert(varchar(20),[Sub_Valid_Till], 0), '') as Sub_Valid_Till_Convert FROM [goldenlife].[User_Master] where 1=1 and Org_Id=" + Org_Id + " " + query + " ", con);

        }

        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }

    [WebMethod]
    public void ClearGCM(string Mobile, string Role)
    {
        SqlConnection con = new SqlConnection(strConnString);

        Status status = new Status();
        status.status = 0;

        try
        {
            con.Open();
            t = con.BeginTransaction();

            if (Role == "Admin")
            {
                SqlCommand cmd = new SqlCommand("Update [goldenlife].[Admin_Master] set GCM=NULL where Mobile='" + Mobile + "' AND Org_Id='" + Org_Id + "'", con, t);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("Update [goldenlife].[User_Master] set GCM=NULL  where Mobile='" + Mobile + "' AND Org_Id='" + Org_Id + "'", con, t);
                cmd.ExecuteNonQuery();
            }
            t.Commit();
            status.status = 1;

        }
        catch (SqlException ex)
        {
            t.Rollback();

        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }
        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }


    //-----------------BANNER AND PROMO CRUD------------------------
    [WebMethod]
    public void BannerPromoCRUD(string For, string Sr_No, string Type, string Image, string Is_Redirect, string Url, string Added_By_Id, string Added_By_Name)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;


        SqlConnection con = new SqlConnection(strConnString);
        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        SqlCommand cmd;

        try
        {
            if (For == "Insert")
            {
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Banner_Promo_Master]
                ([Org_Id],[Type],[Image],[Is_Redirect],[Url],[Added_By_Id],[Added_By_Name],[Added_On])
                 VALUES(@Org_Id,@Type,@Image,@Is_Redirect,@Url,@Added_By_Id,@Added_By_Name,@Added_On)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Banner_Promo_Master] set Type=@Type,[Image]=@Image,[Url]=@Url, Is_Redirect=@Is_Redirect
                where Sr_No=@Sr_No and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [goldenlife].[Banner_Promo_Master] where Sr_No=@Sr_No and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Sr_No", Sr_No);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Is_Redirect", Is_Redirect);
            cmd.Parameters.AddWithValue("@Url", Url);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            t.Commit();
            status.status = 1;

        }
        catch (SqlException ex)
        {
            t.Rollback();

        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendBannerPromoImg()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/BannerPromoImg/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetBannerPromo(string Type)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "Banner")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Added_On], 0) as [Added_On_Convert]
            FROM [goldenlife].[Banner_Promo_Master] where Org_Id=@Org_Id and Type='Banner' order by Sr_No desc", con);
        }
        else if (Type == "Promo")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Added_On], 0) as [Added_On_Convert]
            FROM [goldenlife].[Banner_Promo_Master] where Org_Id=@Org_Id and Type='Promo' order by Sr_No desc", con);
        }
        else if (Type == "")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [goldenlife].[Banner_Promo_Master] where  Org_Id=@Org_Id
            order by Sr_No desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_Id);

        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));

    }

    //------------EVENTS / NEWS --------------------
    [WebMethod]
    public void EventsNewsCRUD(string For, string Sr_No, string Type, string Title, string Description, string Thumbnail, string Date, string From_Time, string To_Time,
        string Location, string Added_By_Id, string Added_By_Name)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();
            if (For == "Insert")
            {
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Event_News_Master] ([Type],[Org_Id],[Title],[Description],[Thumbnail],[Date],[From_Time],[To_Time],
                [Location],[Status],[Added_By_Id],[Added_By_Name],[Added_On])
                VALUES(@Type,@Org_Id,@Title,@Description,@Thumbnail,@Date,@From_Time,@To_Time,@Location,1,@Added_By_Id,@Added_By_Name,@Added_On)", con);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Event_News_Master]  set [Type]=@Type,[Title]=@Title, Description=@Description,Thumbnail=@Thumbnail,
                Date=@Date, From_Time=@From_Time, To_Time=@To_Time, Location=@Location, Added_By_Id=@Added_By_Id, Added_By_Name=@Added_By_Name where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [goldenlife].[Event_News_Master] where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
            cmd.Parameters.AddWithValue("@Sr_No", Sr_No);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Thumbnail", Thumbnail);
            cmd.Parameters.AddWithValue("@Date", Date);
            cmd.Parameters.AddWithValue("@From_Time", From_Time);
            cmd.Parameters.AddWithValue("@To_Time", To_Time);
            cmd.Parameters.AddWithValue("@Location", Location);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendEventNewsImg()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/EventNewsImg/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetEventsNews(string Date)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;
        if (Date == "")
        {
            cmd = new SqlCommand(@"SELECT *,isnull(Convert(varchar(20),[From_Time], 0), '') as From_Time_Convert, 
            isnull(Convert(varchar(20),[To_Time], 0), '') as To_Time_Convert, isnull(Convert(varchar(20),[Date], 0), '') as Date_Convert,
            isnull(Convert(varchar(20),[Added_On], 0), '') as Added_On_Convert FROM [goldenlife].[Event_News_Master] where  Org_Id=" + Org_Id + "", con);
        }
        else
        {
            cmd = new SqlCommand(@"SELECT *,isnull(Convert(varchar(20),[From_Time], 0), '') as From_Time_Convert, 
            isnull(Convert(varchar(20),[To_Time], 0), '') as To_Time_Convert, isnull(Convert(varchar(20),[Date], 0), '') as Date_Convert,
            isnull(Convert(varchar(20),[Added_On], 0), '') as Added_On_Convert FROM [goldenlife].[Event_News_Master] where Org_Id=" + Org_Id + " and  isnull(Convert(varchar(20),[Date], 103), '') ='" + Date + "'", con);

        }
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            //  con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);

            dt.Columns.Remove("From_Time");
            dt.Columns.Remove("To_Time");
            dt.Columns.Remove("Date");
            dt.Columns.Remove("Added_On");

            if (dt.Rows.Count > 0)
            {
                Dictionary<string, object> row;

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    list.Add(row);
                }
            }

        }
        catch (Exception e) { }
        finally
        {
            // con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }


    //------------PHOTO / REELS --------------------
    [WebMethod]
    public void PhotoReelsCRUD(string For, string Sr_No, string Type, string Title, string Description, string Img, string Yt_Id, string Added_By_Id, string Added_By_Name)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();
            if (For == "Insert")
            {
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Photo_Reels_Master] ([Type],[Org_Id],[Title],[Description],[Img],[Yt_Id],[Status],[Added_By_Id],[Added_By_Name],[Added_On])
                VALUES(@Type,@Org_Id,@Title,@Description,@Img,@Yt_Id,1,@Added_By_Id,@Added_By_Name,@Added_On)", con);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Photo_Reels_Master] set [Type]=@Type,[Title]=@Title,Description=@Description,Img=@Img,Yt_Id=@Yt_Id,
              Added_By_Id=@Added_By_Id, Added_By_Name=@Added_By_Name where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [goldenlife].[Photo_Reels_Master] where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
            cmd.Parameters.AddWithValue("@Sr_No", Sr_No);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Img", Img);
            cmd.Parameters.AddWithValue("@Yt_Id", Yt_Id);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendPhotoReelsImg()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/PhotoReelsImg/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendPhotoReelsMultiple(string Added_By_Id, string Added_By_Name)
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        SqlConnection con = new SqlConnection(strConnString);
        con.Open();

        SqlCommand cmd;

        try
        {

            var request = HttpContext.Current.Request;
            var files = request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/PhotoReelsImg/" + file.FileName));

                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Photo_Reels_Master] ([Type],[Org_Id],[Title],[Description],[Img],[Yt_Id],[Status],[Added_By_Id],[Added_By_Name],[Added_On])
                VALUES(@Type,@Org_Id,@Title,@Description,@Img,@Yt_Id,1,@Added_By_Id,@Added_By_Name,@Added_On)", con);

                cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
                cmd.Parameters.AddWithValue("@Type", "Photo");
                cmd.Parameters.AddWithValue("@Title", "");
                cmd.Parameters.AddWithValue("@Description", "");
                cmd.Parameters.AddWithValue("@Img", file.FileName);
                cmd.Parameters.AddWithValue("@Yt_Id", "");
                cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
                cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
                cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
                cmd.ExecuteNonQuery();
            }



            //HttpContext.Current.Response.Write("File uploaded successfully!");
            status.status = 1;
            status.ex = "File uploaded successfully!";
        }
        catch (Exception ex)
        {
            // Handle exceptions
            string e = ex.ToString();
            //HttpContext.Current.Response.Write("Error: " + e);
            status.status = 0;
            status.ex = e;
        }
        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void GetPhotoReels(string Date)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;
        if (Date == "")
        {
            cmd = new SqlCommand(@"SELECT *,isnull(Convert(varchar(20),[Added_On], 0), '') as Added_On_Convert 
            FROM [goldenlife].[Photo_Reels_Master] where  Org_Id=" + Org_Id + "", con);
        }
        else
        {
            cmd = new SqlCommand(@"SELECT *,isnull(Convert(varchar(20),[Added_On], 0), '') as Added_On_Convert 
            FROM [goldenlife].[Photo_Reels_Master] where Org_Id=" + Org_Id + "", con);

        }

        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }


    //------------SUBSCRIPTION PLAN --------------------
    [WebMethod]
    public void SubscriptionPlanCRUD(string For, string Sr_No, string Plan_Name, string Description, string Amount, string Convenience_Fee, string Total_Amount,
        string Added_By_Id, string Added_By_Name)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();
            if (For == "Insert")
            {
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Subscription_Plan_Master] ([Org_Id],[Plan_Name],[Description],[Amount],[Convenience_Fee],[Total_Amount],[Added_By_Id],[Added_By_Name],[Added_On])
                VALUES(@Org_Id,@Plan_Name,@Description,@Amount,@Convenience_Fee,@Total_Amount,@Added_By_Id,@Added_By_Name,@Added_On)", con);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Subscription_Plan_Master] set Plan_Name=@Plan_Name,Description=@Description,Amount=@Amount,Convenience_Fee=@Convenience_Fee,
                Total_Amount=@Total_Amount,Added_By_Id=@Added_By_Id,Added_By_Name=@Added_By_Name
                    where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [goldenlife].[Subscription_Plan_Master] where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
            cmd.Parameters.AddWithValue("@Sr_No", Sr_No);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@Plan_Name", Plan_Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Convenience_Fee", Convenience_Fee);
            cmd.Parameters.AddWithValue("@Total_Amount", Total_Amount);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void GetSubscriptionPlan()
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"SELECT *,isnull(Convert(varchar(20),[Added_On], 0), '') as Added_On_Convert 
            FROM [goldenlife].[Subscription_Plan_Master] where  Org_Id=" + Org_Id + "", con);

        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }

    //------------VISITOR --------------------
    [WebMethod]
    public void VisitorCRUD(string For, string Sr_No, string User_Id, string Event_Id, string No_Of_Guest, string Photo, string Status, string Amount)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();
            if (For == "Insert")
            {
                cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Visitor_Master] ([Org_Id],[User_Id],[Event_Id],[No_Of_Guest],[Photo],[Status],[Amount],[Payment_Id],[Payment_Status],[Added_On])
                VALUES(@Org_Id,@User_Id,@Event_Id,@No_Of_Guest,@Photo,@Status,@Amount,@Payment_Id,@Payment_Status,@Added_On)", con);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Visitor_Master] set [No_Of_Guest]=@No_Of_Guest,[Photo]=@Photo where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else if (For == "UpdateStatus")
            {
                cmd = new SqlCommand(@"update [goldenlife].[Visitor_Master] set [Status]=@Status,[Amount]=@Amount where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [goldenlife].[Visitor_Master] where Sr_No=@Sr_No and Org_Id=@Org_Id", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
            cmd.Parameters.AddWithValue("@Sr_No", Sr_No);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@User_Id", User_Id);
            cmd.Parameters.AddWithValue("@Event_Id", Event_Id);
            cmd.Parameters.AddWithValue("@No_Of_Guest", No_Of_Guest);
            cmd.Parameters.AddWithValue("@Photo", Photo);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Payment_Id", "");
            cmd.Parameters.AddWithValue("@Payment_Status", "");
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendVisitorImg()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Attachment/VisitorImg/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetVisitor(string Event_Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@" Select v.* ,[Husband_Name],[Wife_Name],[Last_Name],[Husband_DOB],[Wife_DOB]
      ,[Anniversary_Date], isnull(Convert(varchar(20),v.[Added_On], 0), '') as Added_On_Convert 
            FROM [goldenlife].[Visitor_Master] 
            v inner join [goldenlife].[User_Master] u on u.Id = v.User_Id  WHERE v.Event_Id=@Event_Id and v.Org_Id=" + Org_Id + "", con);
        cmd.Parameters.AddWithValue("@Event_Id", Event_Id);

        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }


    [WebMethod]
    public void InsertMyPayments(string User_Id, string Name, string Subscription_Id, string Amount, string Convenience_Fee, string Total_Amount, string Txn_Id,
       string User_Type)
    {
        Status status = new Status();
        status.status = 0;

        Int32 Id = 0;

        SqlConnection con = new SqlConnection(strConnString);
        SqlTransaction transaction;
        con.Open();
        transaction = con.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [goldenlife].[Payment_Master]
           ([Org_Id],[User_Id],[Name],[Subscription_Id],[Amount],[Convenience_Fee],[Total_Amount],[Txn_Id],[Payment_Id],[Sign],[Status],[AddedOn],[PaidOn],[User_Type])
     VALUES(@Org_Id,@User_Id,@Name,@Subscription_Id,@Amount,@Convenience_Fee,@Total_Amount,@Txn_Id,@Payment_Id,@Sign,@Status,@AddedOn,@PaidOn,@User_Type); select Scope_Identity()", con, transaction);
            cmd.Parameters.AddWithValue("@Org_Id", Org_Id);
            cmd.Parameters.AddWithValue("@User_Id", User_Id);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Subscription_Id", Subscription_Id);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@Convenience_Fee", Convenience_Fee);
            cmd.Parameters.AddWithValue("@Total_Amount", Total_Amount);
            cmd.Parameters.AddWithValue("@Txn_Id", Txn_Id);
            cmd.Parameters.AddWithValue("@Payment_Id", "");
            cmd.Parameters.AddWithValue("@Sign", "");
            cmd.Parameters.AddWithValue("@Status", "Unpaid");
            cmd.Parameters.AddWithValue("@AddedOn", DateTime.Now);
            cmd.Parameters.AddWithValue("@PaidOn", DateTime.Now);
            cmd.Parameters.AddWithValue("@User_Type", User_Type);
            Id = Convert.ToInt32(cmd.ExecuteScalar());


            transaction.Commit();
            status.status = 1;


        }
        catch (SqlException sq)
        {
            transaction.Rollback();
            status.status = 0;
        }
        catch (Exception e)
        {
            string ex = e.ToString();

        }
        finally
        {
            con.Close();
        }
        Context.Response.Write(new JavaScriptSerializer().Serialize(status));

    }

    [WebMethod]
    public void verifyOrderMyPayments(string Txn_Id)
    {
        SqlConnection con = new SqlConnection(strConnString);

        SqlCommand cmd = new SqlCommand("select * from [goldenlife].[Payment_Master] where Txn_Id=@Txn_Id COLLATE SQL_Latin1_General_CP1_CS_AS", con);
        cmd.Parameters.AddWithValue("@Txn_Id", Txn_Id);


        List<Dictionary<string, object>> list = getData(cmd);
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));

    }

    [WebMethod]
    public void onPaySuccessUpdateMyPayments(string User_Id, string Txn_Id, string Payment_Id, string Sign, string User_Type)
    {
        Status status = new Status();
        status.status = 0;

        SqlConnection con = new SqlConnection(strConnString);
        SqlTransaction transaction;
        con.Open();
        transaction = con.BeginTransaction();
        SqlCommand cmd;
        try
        {
            cmd = new SqlCommand("Update [goldenlife].[Payment_Master] set PaidOn=getdate(),Status='Paid', Payment_Id=@Payment_Id, Sign=@Sign  where Txn_Id=@Txn_Id COLLATE SQL_Latin1_General_CP1_CS_AS", con, transaction);
            cmd.Parameters.AddWithValue("@Payment_Id", Payment_Id);
            cmd.Parameters.AddWithValue("@Sign", Sign);
            cmd.Parameters.AddWithValue("@Txn_Id", Txn_Id);
           // cmd.ExecuteNonQuery();



            if(User_Type== "User")
            {
                cmd = new SqlCommand("Update [goldenlife].[User_Master] set Payment_Id=@Payment_Id,[Is_Subscribed]=1,[Sub_Valid_From]=@Sub_Valid_From,[Sub_Valid_Till]=@Sub_Valid_Till  where Id=@Id", con, transaction);
                cmd.Parameters.AddWithValue("@Id", User_Id);
                cmd.Parameters.AddWithValue("@Payment_Id", Payment_Id);
                cmd.Parameters.AddWithValue("@Sub_Valid_From", Convert.ToDateTime(DateTime.Now));
                cmd.Parameters.AddWithValue("@Sub_Valid_Till", DateTime.Now.AddMonths(12));
                //cmd.Parameters.AddWithValue("@Sub_Valid_Till", Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.AddDays(7))));
                int count = cmd.ExecuteNonQuery();
            }else
            {
                cmd = new SqlCommand("Update [goldenlife].[Visitor_Master] set [Payment_Status]=@Payment_Status,[Payment_Id]=@Payment_Id  where Sr_No=@Id", con, transaction);
                cmd.Parameters.AddWithValue("@Id", User_Id);
                cmd.Parameters.AddWithValue("@Payment_Status", "Paid");
                cmd.Parameters.AddWithValue("@Payment_Id", Payment_Id);
                int count = cmd.ExecuteNonQuery();  
            }
            


            transaction.Commit();
            status.status = 1;


        }
        catch (SqlException sq)
        {
            transaction.Rollback();
            status.status = 0;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            status.status = 0;
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));

    }


   
    //GET JSON DATA
    private List<Dictionary<string, object>> getData(SqlCommand cmd)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);


            if (dt.Rows.Count > 0)
            {
                Dictionary<string, object> row;

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    list.Add(row);
                }
            }
        }
        catch (Exception e)
        {
            String ex = e.ToString();
        }
        finally
        {

        }

        return list;

    }

    //public class CustomerLogin
    //{
    //    public string Cust_Id { get; set; }
    //    public string Org_Id { get; set; }
    //    public string FName { get; set; }
    //    public string LName { get; set; }
    //    public string Mobile { get; set; }
    //    public string Password { get; set; }
    //    public string OTP { get; set; }
    //    public string Email { get; set; }
    //    public string Adderss { get; set; }
    //    public string Pincode { get; set; }
    //}

    public class Status
    {
        public int status { get; set; }
    }

    public class VerificationStatus
    {
        public int status { get; set; }
        public int id { get; set; }
        public string role { get; set; }

        public VerificationStatus() { }
    }

    public class Profile
    {
        public string User_Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Mobile { get; set; }
        public string Otp { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string App_Status { get; set; }
        public string Last_Seen { get; set; }
        public string Ref_Code { get; set; }
        public string Status { get; set; }
        public string Added_on { get; set; }
        public string Code { get; set; }
        public string Discount_Percent { get; set; }

    }


    public class StatusWithError
    {
        public int status { get; set; }
        public string ex { get; set; }
    }


    public class EditTextCoupon
    {
        public string Row_Id { get; set; }
        public string Discount_Type { get; set; }
        public string Coupon_Code { get; set; }
        public string Percent_off { get; set; }
        public string Amount { get; set; }
        public string Coupon_Description { get; set; }


    }


}
