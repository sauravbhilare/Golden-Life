using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Net.Mail;
using System.IO;
/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ElectionWebService : System.Web.Services.WebService
{
    static string Org_ID = "1";


    //EMAIL
    static string host = "mail.masyscare.com";
    static string enablessl = "false";
    static string username = "alerts@masyscare.com";
    static string password = "masyscare@123";
    static string port = "25";


    static String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    static string GetTicketNo;
    public ElectionWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }



    [WebMethod]
    public void Login(string Username, string Password, string GCM)
    {
        SqlConnection con = new SqlConnection(strConnString);

        SqlCommand cmd;

        VerificationStatus status = new VerificationStatus();
        status.status = 0;
        status.Id = "0";
        status.Role = "";
        String Id, Role;
        try
        {
            con.Open();
            cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Login] where Username=@Username 
            and Password=@Password and Status='1' and Org_Id=@Org_Id", con);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

            DataTable dt = new DataTable();
            new SqlDataAdapter(cmd).Fill(dt);

            if (dt.Rows.Count > 0)
            {

                Id = dt.Rows[0]["Id"].ToString();
                Role = dt.Rows[0]["Role"].ToString();


                cmd = new SqlCommand(@"Update [electionapp].[Login] set GCM=@GCM where Username=@Username 
            and Password=@Password and Status='1' and Org_Id=@Org_Id", con);
                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@GCM", GCM);
                cmd.ExecuteNonQuery();

                status.status = 1;
                status.Id = Id;
                status.Role = Role;


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
    public void GetDetails(string Id, string Role)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Id != "")
        {
            if (Role == "Staff")
            {
                cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Login] where Status='1' and 
                Id=@Id and Role=@Role and Org_Id=@Org_Id  order by Name", con);
            }
            else if (Role == "Admin")
            {
                cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Login] where Status='1'
            and Id=@Id and Role=@Role and Org_Id=@Org_Id order by Name", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
        }
        else
        {
            if (Id == "" && Role == "")
            {
                cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Login] where Status='1'
             and Org_Id=@Org_Id order by Name", con);
            }
            else if (Id == "")
            {
                cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Login] where Status='1'
            and Role=@Role and Org_Id=@Org_Id order by Name", con);
            }
            else
            {
                cmd = new SqlCommand(@"", con);
            }
        }

        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@Role", Role);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);

            //if (Role != "Admin")
            //{
            //    dt.Columns.Remove("Shift_Time_In");
            //    dt.Columns.Remove("Shift_Time_Out");
            //}

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    //-----------------USER LOGIN------------------------
    [WebMethod]
    public void GenerateOTP(string Mobile)
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        Status status = new Status();
        status.status = 0;
        int res = 0;

        if (Mobile.Length == 10)
        {
            bool IsExist = false, OTPSent = false;
            // string OTP = new Random().Next(1000, 9999).ToString();
            string OTP = "1234";


            cmd = new SqlCommand(@"select 1 from [electionapp].[User_Login] where Mobile=@Mobile and [Org_Id]=@Org_Id and Status = '1'", con);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    if (Mobile == "8097455577")
                    {
                        OTP = "1234";
                    }
                    IsExist = true;
                    cmd = new SqlCommand(@"update [electionapp].[User_Login] set OTP=@OTP where Mobile=@Mobile and [Org_Id]=@Org_Id and Status = '1'", con);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    cmd.Parameters.AddWithValue("@OTP", OTP);
                    cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                    res = cmd.ExecuteNonQuery();

                    if (res == 1) status.status = 1;
                }
                else
                {
                    cmd = new SqlCommand(@"insert into [electionapp].[User_Login]
                    ([Org_Id],[F_Name],[L_Name],[Mobile],[Username],[Password],[OTP],[Email],[Gender],[DOB],[Address],[Role],[Status],[GCM],[Date],[Ref_Code])
                values(@Org_Id,@F_Name,@L_Name,@Mobile,@Username,@Password,@OTP,@Email,@Gender,@DOB,@Address,@Role,@Status,@GCM,@Date,(FLOOR(RAND()*(999999-111111+1))+111111))", con);   
                    cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                    cmd.Parameters.AddWithValue("@F_Name", "");
                    cmd.Parameters.AddWithValue("@L_Name", "");
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    cmd.Parameters.AddWithValue("@Username", Mobile);
                    cmd.Parameters.AddWithValue("@Password", "user@123");
                    cmd.Parameters.AddWithValue("@OTP", OTP);
                    cmd.Parameters.AddWithValue("@Email", "");
                    cmd.Parameters.AddWithValue("@Gender", "");
                    cmd.Parameters.AddWithValue("@DOB", "");
                    cmd.Parameters.AddWithValue("@Address", "");
                    cmd.Parameters.AddWithValue("@Role", "User");
                    cmd.Parameters.AddWithValue("@Status", "1");
                    cmd.Parameters.AddWithValue("@GCM", "");
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    res = cmd.ExecuteNonQuery();

                    if (res == 1) status.status = 1;

                }
            }
            catch(Exception ex)
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
                // SMS.SendOtp1(mobile, OTP);
                //Whatsapp.SendText(mobile, "Dear Student " + OTP + " is your OTP for TikKar. Code is valid for 2 minutes only. DO NOT share this OTP with anyone.");
            }
        }


        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void VerifyOTP(string Mobile, string OTP, string GCM)
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        VerificationStatus status = new VerificationStatus();     
        status.Id = "0";
        status.Role = "";

        if (OTP.Length == 4)
        {

            bool IsVerified = false;

            try
            {
                con.Open();

                cmd = new SqlCommand(@"select * from [electionapp].[User_Login] where Mobile=@Mobile and otp=@otp and Org_Id=@Org_Id and Status='1'", con);
                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@otp", OTP);
                DataTable dtotp = new DataTable();
                new SqlDataAdapter(cmd).Fill(dtotp);
                if (dtotp.Rows.Count > 0)
                {
                    cmd = new SqlCommand("update [electionapp].[User_Login] set GCM=@GCM where Mobile=@Mobile and Org_Id=@Org_Id and Status='1'", con);
                    cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    cmd.Parameters.AddWithValue("@GCM", GCM);
                    cmd.ExecuteNonQuery();

                   
                    status.Id = dtotp.Rows[0]["Id"].ToString();
                    status.Role = dtotp.Rows[0]["Role"].ToString();

                    string Name = dtotp.Rows[0]["F_Name"].ToString().Trim();
                    if (!String.IsNullOrEmpty(Name)) status.status = 1;
                }
                else
                {
                    status.Id = "0";
                    status.Role = "";
                    status.status = 0;
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
    public void UpdateProfile(string Mobile, string F_Name, string L_Name, string Email, string Gender, string DOB, string Address)
    {
        Status status = new Status();
        status.status = 0;

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        try
        {
            con.Open();
            cmd = new SqlCommand(@"update [electionapp].[User_Login]
            set [F_Name]=@F_Name,[L_Name]=@L_Name,[Email]=@Email,[Gender]=@Gender,[DOB]=@DOB,[Address]=@Address       
            where Mobile=@Mobile and Org_Id=Org_Id and Status='1'", con);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@F_Name", F_Name);
            cmd.Parameters.AddWithValue("@L_Name", L_Name);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@DOB", DOB);
            cmd.Parameters.AddWithValue("@Address", Address);

            int res = cmd.ExecuteNonQuery();

            if (res == 1) status.status = 1;
        }
        catch(Exception ex)
        {
            string e = ex.ToString();
        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));
    }

    [WebMethod]
    public void GetUserDetails(string Id, string Role)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Id != "")
        {
            cmd = new SqlCommand(@"SELECT * FROM [electionapp].[User_Login] where Status='1' and 
                Id=@Id and Role=@Role and Org_Id=@Org_Id  order by F_Name", con);

        }
        else if (Id == "" && Role == "User")
        {
            cmd = new SqlCommand(@"SELECT * FROM [electionapp].[User_Login] where Status='1' and 
                 Role=@Role and Org_Id=@Org_Id  order by F_Name", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }


        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@Role", Role);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);

            //if (Role != "Admin")
            //{
            //    dt.Columns.Remove("Shift_Time_In");
            //    dt.Columns.Remove("Shift_Time_Out");
            //}

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }



    [WebMethod]
    public void StaffCRUD(string For, string Id, string Name, string Mobile, string Password,
        string Email, string Gender, string DOB, string DOJ, string Address, string Role, string Added_By_Name, string Added_By_Id)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Login]
                ([Org_Id],[Name],[Mobile],[Username],[Password],[Email],[Gender],[DOB],[DOJ],[Address],[Role],[Status],[GCM],[Added_By_Name],[Added_By_Id],[Added_On])
                 VALUES(@Org_Id,@Name,@Mobile,@Username,@Password,@Email,@Gender,@DOB,@DOJ,@Address,@Role,@Status,@GCM,@Added_By_Name,@Added_By_Id,@Added_On)", con, t);

            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Login] set [Name]=@Name,[Mobile]=@Mobile,[Username]=@Username,
                [Password]=@Password,[Email]=@Email,[Gender]=@Gender,[DOB]=@DOB,[DOJ]=@DOJ,[Address]=@Address where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Login] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Username", Mobile);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@DOB", DOB);
            cmd.Parameters.AddWithValue("@DOJ", DOJ);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Role", Role);
            cmd.Parameters.AddWithValue("@Status", "1");
            cmd.Parameters.AddWithValue("@GCM", "");
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_On", DateTime.Now);
            res = cmd.ExecuteNonQuery();

            t.Commit();
            if (res == 1) status.status = 1;


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
    public void InsertDeviceLog(string Name, string Mobile, string Email, string Brand, string Model_Name, string DeviceId,
        string Sdk_Ver, string Android_Ver)
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
            cmd = new SqlCommand(@"SELECT * FROM [electionapp].[Device_Log] where Org_Id=@Org_Id and DeviceId=@DeviceId", con, transaction);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@DeviceId", DeviceId);

            DataTable dt = new DataTable();
            new SqlDataAdapter(cmd).Fill(dt);

            if (dt.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"Update [electionapp].[Device_Log] set Login_At=@Login_At,[Name]=@Name,[Mobile]=@Mobile,[Email]=@Email
                where Org_Id=@Org_Id and DeviceId=@DeviceId", con, transaction);

                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@DeviceId", DeviceId);
                cmd.Parameters.AddWithValue("@Login_At", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                status.status = 1;

                string DeviceStatus = dt.Rows[0]["Status"].ToString();
                if (DeviceStatus == "1")
                {
                    status.status = 1;
                }
                else
                {
                    status.status = 2;
                }

                //Role = dt.Rows[0]["Role"].ToString();

            }
            else
            {

                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Device_Log] 
                    ([Id],[Role],[Name],[Mobile],[Email],[Org_Id],[Login_At],[Brand],[DeviceId],[Model_Name],[Sdk_Ver],[Android_Ver],[Status]) 
                VALUES(@Id, @Role,@Name,@Mobile,@Email,@Org_Id, @Login_At, @Brand, @DeviceId, @Model_Name, @Sdk_Ver, @Android_Ver,@Status )", con, transaction);

                cmd.Parameters.AddWithValue("@Id", "0");
                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Role", "");
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Login_At", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@Brand", Brand);
                cmd.Parameters.AddWithValue("@DeviceId", DeviceId);
                cmd.Parameters.AddWithValue("@Model_Name", Model_Name);
                cmd.Parameters.AddWithValue("@Sdk_Ver", Sdk_Ver);
                cmd.Parameters.AddWithValue("@Android_Ver", Android_Ver);
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.ExecuteNonQuery();
                transaction.Commit();
                status.status = 1;

            }

        }
        catch (SqlException sq)
        {
            transaction.Rollback();

        }
        catch (Exception e)
        {

        }
        finally
        {
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(status));

    }

    //[WebMethod]
    //public void GetDeviceDetails(string Mobile)
    //{
    //    SqlConnection con = new SqlConnection(strConnString);
    //    SqlCommand cmd;

    //    DeviceStatus devicestatus = new DeviceStatus();
    //    devicestatus.devicestatus = "";

    //    try
    //    {
    //        con.Open();
    //        cmd = new SqlCommand(@"select Top 1 DeviceId from tikkar.MobileLogin_Log where Mobile=@mobile order by Sr_No Desc", con);
    //        cmd.Parameters.AddWithValue("@mobile", Mobile);
    //        DataTable dt = new DataTable();
    //        new SqlDataAdapter(cmd).Fill(dt);
    //        if (dt.Rows.Count > 0)
    //        {
    //            string devstatus = dt.Rows[0]["DeviceId"].ToString();
    //            devicestatus.devicestatus = devstatus;
    //        }
    //    }
    //    catch
    //    { }
    //    finally
    //    {
    //        con.Close();
    //    }

    //    Context.Response.Write(new JavaScriptSerializer().Serialize(devicestatus));
    //}




    //-----------------GALLERY CRUD------------------------
    [WebMethod]
    public void GalleryCRUD(string For, string Id, string Image, string Added_By_Id, string Added_By_Name,
    string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Gallery_Master]
                ([Org_Id],[Date],[Image],[Status],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Image,@Status,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Gallery_Master] set [Image]=@Image
                where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Gallery_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Status", "1");
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendGalleryImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Gallery/" + file.FileName));

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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendGalleryImgMultiple(string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Gallery/" + file.FileName));

                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Gallery_Master]
                ([Org_Id],[Date],[Image],[Status],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Image,@Status,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con);


                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Image", file.FileName);
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
                cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
                cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void GetGallery(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Gallery_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Gallery_Master] where Id=@Id  and Org_Id=@Org_Id
            order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------BANNER AND PROMO CRUD------------------------
    [WebMethod]
    public void BannerPromoCRUD(string For, string Id, string Image, string Url, string Type, string Added_By_Id, string Added_By_Name,
    string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Banner_Promo_Master]
                ([Org_Id],[Date],[Image],[Url],[Type],[Status],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Image,@Url,@Type,@Status,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Banner_Promo_Master] set [Image]=@Image,[Url]=@Url
                where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Banner_Promo_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Url", Url);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Status", "1");
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/BannerPromo/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetBannerPromo(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "Banner")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Banner_Promo_Master] where Org_Id=@Org_Id and Type='Banner' order by Id desc", con);
        }
        else if (Type == "Promo")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Banner_Promo_Master] where Org_Id=@Org_Id and Type='Promo' order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Banner_Promo_Master] where Id=@Id  and Org_Id=@Org_Id
            order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------CATEGORY CRUD------------------------

    [WebMethod]
    public void CategoryCRUD(string For, string Id, string Cat_For, string Name, string Icon, string Added_By_Id, string Added_By_Name,
        string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Category_Master]
                ([Org_Id],[Date],[Cat_For],[Name],[Icon],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_For,@Name,@Icon,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Category_Master] set [Name]=@Name,[Icon]=@Icon,
                [Added_By_Id]=@Added_By_Id,[Added_By_Name]=@Added_By_Name,[Added_By_Role]=@Added_By_Role
                where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Category_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_For", Cat_For);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Icon", Icon);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendCategoryIcon()
    {
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/CategoryIcon/" + file.FileName));

        }
        catch (Exception ex)
        {
            string e = ex.ToString();
        }
    }

    [WebMethod]
    public void GetCategory(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Category_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "Cat_For")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Category_Master] where Org_Id=@Org_Id and Cat_For=@Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Category_Master] where Id=@Id  and Org_Id=@Org_Id
            order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------PROFESSIONAL CRUD------------------------
    [WebMethod]
    public void ProfessionalCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description,
        string Mobile, string WhatsApp_No, string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Professional_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Professional_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,Image=@Image
                where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Professional_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendProfessionalImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Professional/" + file.FileName));


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
    public void GetProfessional(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Professional_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Professional_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Professional_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }



    //-----------------EMERGENCY CRUD------------------------
    [WebMethod]
    public void EmergencyCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description,
        string Mobile, string WhatsApp_No, string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Emergency_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Emergency_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,
                Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Emergency_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendEmergencyImg()
    {

        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Emergency/" + file.FileName));

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
    public void GetEmergency(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------EMERGENCY TRANSPORTATION CRUD------------------------
    [WebMethod]
    public void EmergencyTransportationCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description,
        string Mobile, string WhatsApp_No, string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Emergency_Transportation_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Emergency_Transportation_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,
                Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Emergency_Transportation_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendEmergencyTransportationImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/EmergencyTransportation/" + file.FileName));

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
    public void GetEmergencyTransportation(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Transportation_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Transportation_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Emergency_Transportation_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------NGO CRUD------------------------
    [WebMethod]
    public void NGOCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description,
        string Mobile, string WhatsApp_No, string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[NGO_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[NGO_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,
                Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[NGO_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendNGOImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/NGO/" + file.FileName));

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
    public void GetNGO(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[NGO_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[NGO_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[NGO_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    //-----------------Govt Service CRUD------------------------
    [WebMethod]
    public void GovtServiceCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description, string Mobile, string WhatsApp_No,
        string Email, string Image, string Address, string Latitude, string Longitude, string Added_By_Id, string Added_By_Name,
        string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Govt_Service_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Email],[Image],[Address],[Latitude],[Longitude],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Email,@Image,@Address,@Latitude,@Longitude,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Govt_Service_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,
                Email=@Email,Image=@Image,[Address]=@Address,[Latitude]=@Latitude,Longitude=@Longitude where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Govt_Service_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Latitude", Latitude);
            cmd.Parameters.AddWithValue("@Longitude", Longitude);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendGovtServiceImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/GovtService/" + file.FileName));

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
    public void GetGovtService(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Service_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Service_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Service_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------Govt Office CRUD------------------------
    [WebMethod]
    public void GovtOfficeCRUD(string For, string Id, string Cat_Id, string Cat_Name, string Cat_Icon, string Name, string Description, string Mobile, string WhatsApp_No,
        string Email, string Image, string Address, string Latitude, string Longitude, string Added_By_Id, string Added_By_Name,
        string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Govt_Office_Master]
                ([Org_Id],[Date],[Cat_Id],[Cat_Name],[Cat_Icon],[Name],[Description],[Mobile],[WhatsApp_No],[Email],[Image],[Address],[Latitude],[Longitude],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Cat_Id,@Cat_Name,@Cat_Icon,@Name,@Description,@Mobile,@WhatsApp_No,@Email,@Image,@Address,@Latitude,@Longitude,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Govt_Office_Master] set [Cat_Id]=@Cat_Id,[Cat_Name]=@Cat_Name,
                Cat_Icon=@Cat_Icon,[Name]=@Name,Description=@Description,[Mobile]=@Mobile,WhatsApp_No=@WhatsApp_No,
                Email=@Email,Image=@Image,[Address]=@Address,[Latitude]=@Latitude,Longitude=@Longitude where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Govt_Office_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
            cmd.Parameters.AddWithValue("@Cat_Name", Cat_Name);
            cmd.Parameters.AddWithValue("@Cat_Icon", Cat_Icon);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@WhatsApp_No", WhatsApp_No);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Latitude", Latitude);
            cmd.Parameters.AddWithValue("@Longitude", Longitude);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendGovtOfficeImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/GovtOffice/" + file.FileName));

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
    public void GetGovtOffice(string Type, string Cat_Id, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Office_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Office_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByCatId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Govt_Office_Master] where Cat_Id=@Cat_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Cat_Id", Cat_Id);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------Youtube Link CRUD------------------------
    [WebMethod]
    public void YtLinkCRUD(string For, string Id, string Yt_Link, string Thumbnail, string Title, string Description,
        string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Youtube_Link_Master]
                ([Org_Id],[Date],[Yt_Link],[Thumbnail],[Title],[Description],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Yt_Link,@Thumbnail,@Title,@Description,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Youtube_Link_Master] set [Yt_Link]=@Yt_Link,
                Thumbnail=@Thumbnail,Title=@Title,Description=@Description where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Youtube_Link_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Yt_Link", Yt_Link);
            cmd.Parameters.AddWithValue("@Thumbnail", Thumbnail);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendYtLinkImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/YtThumbnail/" + file.FileName));

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
    public void GetYtLink(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Youtube_Link_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Youtube_Link_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------SOCIAL Link ------------------------
    [WebMethod]
    public void SocialMediaSetting(string Fb_Link, string Twitter_Link, string Instagram_Link, string Yt_Link)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;


        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        try
        {

            cmd = new SqlCommand(@"select top 1 Id from [electionapp].[Social_Media_Master] where 
                    [Org_Id]=@Org_Id order by [Id] desc", con, t);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

            SqlDataAdapter sdaa = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sdaa.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"Update [electionapp].[Social_Media_Master]  set [Fb_Link]=@Fb_Link,[Twitter_Link]=@Twitter_Link,
                [Instagram_Link]=@Instagram_Link,[Yt_Link]=@Yt_Link where [Org_Id]=@Org_Id ", con, t);

            }
            else
            {
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Social_Media_Master]
                ([Org_Id],[Fb_Link],[Twitter_Link],[Instagram_Link],[Yt_Link]) 
                VALUES(@Org_Id,@Fb_Link,@Twitter_Link,@Instagram_Link,@Yt_Link);select scope_identity()", con, t);
            }
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Fb_Link", Fb_Link);
            cmd.Parameters.AddWithValue("@Twitter_Link", Twitter_Link);
            cmd.Parameters.AddWithValue("@Instagram_Link", Instagram_Link);
            cmd.Parameters.AddWithValue("@Yt_Link", Yt_Link);
            cmd.ExecuteNonQuery();
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
    public void GetSocialMedia()
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"select * from [electionapp].[Social_Media_Master] where Org_Id=@Org_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);


            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Sr_No", typeof(int));
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    dt.Rows[count]["Sr_No"] = count + 1;
                //}

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
            con.Close();
        }

        // Context.Response.Write(new JavaScriptSerializer().Serialize(list));
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }


    //-----------------EVENT CRUD------------------------
    [WebMethod]
    public void EventCRUD(string For, string Id, string Name, string Place, string Description, string Event_Date, string Mobile,
         string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Event_Master]
                ([Org_Id],[Date],[Name],[Place],[Description],[Event_Date],[Mobile],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Name,@Place,@Description,@Event_Date,@Mobile,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Event_Master] set [Name]=@Name,[Place]=@Place,
                [Description]=@Description,Event_Date=@Event_Date,[Mobile]=@Mobile,Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Event_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);

                SqlCommand cmd2 = new SqlCommand(@"delete from [electionapp].[Event_Multiple_Img_Master] where  Event_Id=@Id and Org_Id=@Org_Id", con, t);
                cmd2.Parameters.AddWithValue("@Id", Id);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Place", Place);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Event_Date", Event_Date);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendEventImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Event/" + file.FileName));

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
    public void GetEvent(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert],
            convert(varchar, [Event_Date], 111) as [Event_Date_Convert], convert(varchar, [Event_Date], 8) as [Event_Time_Convert],
            convert(varchar, [Event_Date], 0) as [Event_Date_Convert_Old]
            FROM [electionapp].[Event_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert],
            convert(varchar, [Event_Date], 111) as [Event_Date_Convert], convert(varchar, [Event_Date], 8) as [Event_Time_Convert],
            convert(varchar, [Event_Date], 0) as [Event_Date_Convert_Old]
            FROM [electionapp].[Event_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");
            dt.Columns.Remove("Event_Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendEventMultipleImg(string Event_Id, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Event/" + file.FileName));

                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Event_Multiple_Img_Master]
                ([Org_Id],[Date],[Event_Id],[Image],[Status],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Event_Id,@Image,@Status,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con);


                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Event_Id", Event_Id);
                cmd.Parameters.AddWithValue("@Image", file.FileName);
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
                cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
                cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void EventMultipleImgUpdateDel(string For, string Id, string Image, string Added_By_Id,
        string Added_By_Name, string Added_By_Role)
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
            if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Event_Multiple_Img_Master] set [Image]=@Image,[Added_By_Id]=@Added_By_Id,
                [Added_By_Name]=@Added_By_Name,Added_By_Role=@Added_By_Role where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Event_Multiple_Img_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void GetEventMultipleImg(string Event_Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Event_Id == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Event_Multiple_Img_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Event_Id != "All" && Event_Id != "")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Event_Multiple_Img_Master] where Event_Id=@Event_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }



        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Event_Id", Event_Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------CURRICULAR ACTIVITY CRUD------------------------
    [WebMethod]
    public void CurricularCRUD(string For, string Id, string Name, string Place, string Description, string Event_Date, string Mobile,
         string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Curricular_Master]
                ([Org_Id],[Date],[Name],[Place],[Description],[Event_Date],[Mobile],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Name,@Place,@Description,@Event_Date,@Mobile,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Curricular_Master] set [Name]=@Name,[Place]=@Place,
                [Description]=@Description,Event_Date=@Event_Date,[Mobile]=@Mobile,Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Curricular_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);

                SqlCommand cmd2 = new SqlCommand(@"delete from [electionapp].[Curricular_Multiple_Img_Master] where  Event_Id=@Id and Org_Id=@Org_Id", con, t);
                cmd2.Parameters.AddWithValue("@Id", Id);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Place", Place);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Event_Date", Event_Date);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendCurricularImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Curricular/" + file.FileName));

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
    public void GetCurricular(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert],
            convert(varchar, [Event_Date], 111) as [Event_Date_Convert], convert(varchar, [Event_Date], 8) as [Event_Time_Convert],
            convert(varchar, [Event_Date], 0) as [Event_Date_Convert_Old]
            FROM [electionapp].[Curricular_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert],
            convert(varchar, [Event_Date], 111) as [Event_Date_Convert], convert(varchar, [Event_Date], 8) as [Event_Time_Convert],
            convert(varchar, [Event_Date], 0) as [Event_Date_Convert_Old]
            FROM [electionapp].[Curricular_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");
            dt.Columns.Remove("Event_Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SendCurricularMultipleImg(string Event_Id, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Curricular/" + file.FileName));

                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Curricular_Multiple_Img_Master]
                ([Org_Id],[Date],[Event_Id],[Image],[Status],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Event_Id,@Image,@Status,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con);


                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Event_Id", Event_Id);
                cmd.Parameters.AddWithValue("@Image", file.FileName);
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
                cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
                cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void CurriculartMultipleImgUpdateDel(string For, string Id, string Image, string Added_By_Id,
        string Added_By_Name, string Added_By_Role)
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
            if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Curricular_Multiple_Img_Master] set [Image]=@Image,[Added_By_Id]=@Added_By_Id,
                [Added_By_Name]=@Added_By_Name,Added_By_Role=@Added_By_Role where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Curricular_Multiple_Img_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void GetCurricularMultipleImg(string Event_Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Event_Id == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Curricular_Multiple_Img_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Event_Id != "All" && Event_Id != "")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Curricular_Multiple_Img_Master] where Event_Id=@Event_Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }


        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Event_Id", Event_Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------SPLASH SCREEN ------------------------
    [WebMethod]
    public void SplashScreenSetting(string Image)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;


        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        try
        {

            cmd = new SqlCommand(@"select top 1 Id from [electionapp].[Splash_Master] where 
                    [Org_Id]=@Org_Id order by [Id] desc", con, t);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

            SqlDataAdapter sdaa = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sdaa.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"Update [electionapp].[Splash_Master] set [Image]=@Image,[Date]=@Date where [Org_Id]=@Org_Id ", con, t);

            }
            else
            {
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Splash_Master]
                ([Org_Id],[Date],[Image],[Status]) VALUES (@Org_Id,@Date,@Image,@Status);select scope_identity()", con, t);
            }
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Status", "1");
            cmd.ExecuteNonQuery();
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
    public void SendSplashImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Splash/" + file.FileName));

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
    public void GetSplashScreen()
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"select *, convert(varchar, [Date], 0) as [Date_Convert]
        from [electionapp].[Splash_Master] where Org_Id=@Org_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Sr_No", typeof(int));
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    dt.Rows[count]["Sr_No"] = count + 1;
                //}

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
            con.Close();
        }

        // Context.Response.Write(new JavaScriptSerializer().Serialize(list));
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }

    //-----------------CAREER CRUD------------------------
    [WebMethod]
    public void CareerCRUD(string For, string Id, string Full_Name, string Mobile, string Email, string Experience,
        string Applied_For, string Expected_Salary, string Short_Desc, string Resume, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Career_Master]
                ([Org_Id],[Date],[Full_Name],[Mobile],[Email],[Experience],[Applied_For],[Expected_Salary],[Short_Desc],[Resume],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Full_Name,@Mobile,@Email,@Experience,@Applied_For,@Expected_Salary,@Short_Desc,@Resume,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Career_Master] set [Full_Name]=@Full_Name,[Mobile]=@Mobile,[Email]=@Email,
                [Experience]=@Experience,[Applied_For]=@Applied_For,[Expected_Salary]=@Expected_Salary,[Short_Desc]=@Short_Desc,[Resume]=@Resume
                     where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Career_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Full_Name", Full_Name);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Experience", Experience);
            cmd.Parameters.AddWithValue("@Applied_For", Applied_For);
            cmd.Parameters.AddWithValue("@Expected_Salary", Expected_Salary);
            cmd.Parameters.AddWithValue("@Short_Desc", Short_Desc);
            cmd.Parameters.AddWithValue("@Resume", Resume);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendCareerImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Career/" + file.FileName));

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
    public void GetCareer(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Career_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Career_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    //-----------------SOS SCREEN ------------------------
    [WebMethod]
    public void SOSSetting(string Id, string Role, string Contact1, string Contact2, string Contact3)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;


        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        try
        {

            cmd = new SqlCommand(@"select top 1 Id from [electionapp].[SOS_Master] where 
                    [Org_Id]=@Org_Id and Id=@Id and Role=@Role order by [Id] desc", con, t);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Role", Role);

            SqlDataAdapter sdaa = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sdaa.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"Update [electionapp].[SOS_Master] set [Contact1]=@Contact1,[Contact2]=@Contact2,
                Contact3=@Contact3 where [Org_Id]=@Org_Id  and Id=@Id and Role=@Role", con, t);

            }
            else
            {
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[SOS_Master]
                ([Org_Id],[Date],[Id],[Role],[Contact1],[Contact2],[Contact3]) VALUES (@Org_Id,@Date,@Id,@Role,@Contact1,@Contact2,@Contact3);select scope_identity()", con, t);
            }
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Role", Role);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Contact1", Contact1);
            cmd.Parameters.AddWithValue("@Contact2", Contact2);
            cmd.Parameters.AddWithValue("@Contact3", Contact3);
            cmd.ExecuteNonQuery();
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
    public void GetSOSContact(string Id, string Role)
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"select *, convert(varchar, [Date], 0) as [Date_Convert]
        from [electionapp].[SOS_Master] where Org_Id=@Org_Id and Id=@Id and Role=@Role ", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@Role", Role);
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Sr_No", typeof(int));
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    dt.Rows[count]["Sr_No"] = count + 1;
                //}

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
            con.Close();
        }

        // Context.Response.Write(new JavaScriptSerializer().Serialize(list));
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }

    //-----------------CANDIDATE PROFILE ------------------------
    [WebMethod]
    public void CandidateProfileSetting(string Name, string Profile_Img, string Cover_Img, string Description,
        string Party_Name, string Gender, string DOB, string Constituency, string Email)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;


        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        try
        {

            cmd = new SqlCommand(@"select top 1 Id from [electionapp].[Candidate_Profile] where 
                    [Org_Id]=@Org_Id order by [Id] desc", con, t);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

            SqlDataAdapter sdaa = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sdaa.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"UPDATE [electionapp].[Candidate_Profile] SET [Date]=@Date,
    [Name]=@Name,[Profile_Img]=@Profile_Img,[Cover_Img]=@Cover_Img,[Description]=@Description,[Party_Name]=@Party_Name,
    [Gender]=@Gender,[DOB]=@DOB,[Constituency]=@Constituency,[Email]=@Email where [Org_Id]=@Org_Id ", con, t);

            }
            else
            {
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Candidate_Profile]
                ([Org_Id],[Date],[Name],[Profile_Img],[Cover_Img],[Description],[Party_Name],[Gender],[DOB],[Constituency],[Email])
        VALUES (@Org_Id,@Date,@Name,@Profile_Img,@Cover_Img,@Description,@Party_Name,@Gender,@DOB,@Constituency,@Email);select scope_identity()", con, t);
            }
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Profile_Img", Profile_Img);
            cmd.Parameters.AddWithValue("@Cover_Img", Cover_Img);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Party_Name", Party_Name);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@DOB", DOB);
            cmd.Parameters.AddWithValue("@Constituency", Constituency);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.ExecuteNonQuery();
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
    public void SendCandidateImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Candidate/" + file.FileName));

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
    public void GetCandidateProfile()
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"select *, convert(varchar, [Date], 0) as [Date_Convert] from [electionapp].[Candidate_Profile] where Org_Id=@Org_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Sr_No", typeof(int));
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    dt.Rows[count]["Sr_No"] = count + 1;
                //}

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
            con.Close();
        }

        // Context.Response.Write(new JavaScriptSerializer().Serialize(list));
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }

    //-----------------POLLING BOOTH CRUD------------------------
    [WebMethod]
    public void PollingBoothCRUD(string For, string Id, string Name, string Address, string Latitude, string Longitute,
          string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Polling_Booth]
                ([Org_Id],[Date],[Name],[Address],[Latitude],[Longitute],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Name,@Address,@Latitude,@Longitute,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Polling_Booth] set [Name]=@Name,[Address]=@Address,
                [Latitude]=@Latitude,[Longitute]=@Longitute where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Polling_Booth] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Latitude", Latitude);
            cmd.Parameters.AddWithValue("@Longitute", Longitute);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void GetPollingBooth(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Polling_Booth] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Polling_Booth] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }

    //-----------------THEME SETTINGS ------------------------
    [WebMethod]
    public void ThemeSetting(string Background_Color, string Foreground_Color)
    {
        Status status = new Status();
        status.status = 0;
        int res = 0;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;


        SqlTransaction t;
        con.Open();
        t = con.BeginTransaction();
        try
        {

            cmd = new SqlCommand(@"select top 1 Id from [electionapp].[Theme_Setting] where 
                    [Org_Id]=@Org_Id order by [Id] desc", con, t);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);


            SqlDataAdapter sdaa = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            sdaa.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                cmd = new SqlCommand(@"Update [electionapp].[Theme_Setting] set Background_Color=@Background_Color,[Foreground_Color]=@Foreground_Color where [Org_Id]=@Org_Id", con, t);

            }
            else
            {
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Theme_Setting] ([Org_Id],[Background_Color],Foreground_Color) VALUES (@Org_Id,@Background_Color,@Foreground_Color);select scope_identity()", con, t);
            }
            cmd.Parameters.AddWithValue("@Background_Color", Background_Color);
            cmd.Parameters.AddWithValue("@Foreground_Color", Foreground_Color);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.ExecuteNonQuery();
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
    public void GetTheme()
    {
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        cmd = new SqlCommand(@"select * from  [electionapp].[Theme_Setting] where Org_Id=@Org_Id", con);
        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);

            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Add("Sr_No", typeof(int));
                //for (int count = 0; count < dt.Rows.Count; count++)
                //{
                //    dt.Rows[count]["Sr_No"] = count + 1;
                //}

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
            con.Close();
        }

        // Context.Response.Write(new JavaScriptSerializer().Serialize(list));
        Context.Response.Write(new JavaScriptSerializer().Serialize(list));
    }


    //-----------------SUGGESTION ------------------------
    [WebMethod]
    public void SuggestionCRUD(string For, string Id, string Full_Name, string Mobile, string Title, string Description,
        string Image, string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Suggestion_Master]
                ([Org_Id],[Date],[Full_Name],[Mobile],[Title],[Description],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role],[Is_Replied],[Reply_Remark],[Replied_On])
                 VALUES(@Org_Id,@Date,@Full_Name,@Mobile,@Title,@Description,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role,@Is_Replied,@Reply_Remark,@Replied_On)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Suggestion_Master] set [Full_Name]=@Full_Name,[Mobile]=@Mobile,[Title]=@Title,
                [Description]=@Description,[Image]=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Suggestion_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Full_Name", Full_Name);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Is_Replied", "No");
            cmd.Parameters.AddWithValue("@Reply_Remark", "");
            cmd.Parameters.AddWithValue("@Replied_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SuggestionReply(string Id, string Reply_Remark)
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
            cmd = new SqlCommand(@"update [electionapp].[Suggestion_Master] set [Is_Replied]='Yes',[Reply_Remark]=@Reply_Remark,[Replied_On]=@Replied_On
               where Id=@Id and Org_Id=@Org_Id", con, t);


            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Replied_On", DateTime.Now);
            cmd.Parameters.AddWithValue("@Reply_Remark", Reply_Remark);

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
    public void SendSuggestionImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Suggestion/" + file.FileName));

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
    public void GetSuggestion(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Suggestion_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Suggestion_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ByUserId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Suggestion_Master] where Added_By_Id=@Id and Added_By_Role='User' and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }



    //-----------------SELFIE ------------------------
    [WebMethod]
    public void SelfieCRUD(string For, string Id,string User_Id, string Role, string Image)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Selfie_Master]
                ([Org_Id],[Date],[User_Id],[Image],[Status],[Role],[Updated_By_Id],[Updated_By_Name],[Updated_By_Role])
                 VALUES(@Org_Id,@Date,@User_Id,@Image,@Status,@Role,@Updated_By_Id,@Updated_By_Name,@Updated_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Selfie_Master] set [Image]=@Image
                where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "UpdateStatus")
            {
                //cmd = new SqlCommand("SELECT Status FROM  [electionapp].[Selfie_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
                //cmd.Parameters.AddWithValue("@Id", Id);
                //cmd.Parameters.AddWithValue("@Org_Id", Org_ID);

                //int GetStatus;
                //DataTable dt = new DataTable();
                //new SqlDataAdapter(cmd).Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    GetStatus = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                //}
                //else
                //{
                //    GetStatus = 0;
                //}

                if (Image == "1")
                {
                    cmd = new SqlCommand(@"update  [electionapp].[Selfie_Master] set [Status]=@Status where Id=@Id and Org_Id=@Org_Id", con, t);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                    cmd.Parameters.AddWithValue("@Status", "1");
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new SqlCommand(@"update  [electionapp].[Selfie_Master] set [Status]=@Status where Id=@Id and Org_Id=@Org_Id", con, t);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                    cmd.Parameters.AddWithValue("@Status", "0");
                    cmd.ExecuteNonQuery();
                }
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Selfie_Master] where  Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            if (For != "UpdateStatus")
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@User_Id", User_Id);
                cmd.Parameters.AddWithValue("@Image", Image);
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@Status", "2");
                cmd.Parameters.AddWithValue("@Updated_By_Id", "0");
                cmd.Parameters.AddWithValue("@Updated_By_Name", "");
                cmd.Parameters.AddWithValue("@Updated_By_Role", "");
            }
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
    public void SendSelfieImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;

        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Selfie/" + file.FileName));

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
    public void GetSelfie(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where Id=@Id  and Org_Id=@Org_Id
            order by Id desc", con);
        }
        else if (Type == "ByUserId")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where User_Id=@Id  and Org_Id=@Org_Id and Role='User'
            order by Id desc", con);
        }
        else if (Type == "Approved")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where Status='1' and Org_Id=@Org_Id 
            order by Id desc", con);
        }
        else if (Type == "Declined")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where Status='0' and  Org_Id=@Org_Id 
            order by Id desc", con);
        }
        else if (Type == "Pending")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Selfie_Master] where Status='2' and  Org_Id=@Org_Id 
            order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }


    //-----------------Wishes CRUD------------------------
    [WebMethod]
    public void WishesCRUD(string For, string Id,  string Title, string Description, string Image,
        string Added_By_Id, string Added_By_Name, string Added_By_Role)
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
                cmd = new SqlCommand(@"INSERT INTO [electionapp].[Wishes_Master]
                ([Org_Id],[Date],[Title],[Description],[Image],[Added_By_Id],[Added_By_Name],[Added_By_Role])
                 VALUES(@Org_Id,@Date,@Title,@Description,@Image,@Added_By_Id,@Added_By_Name,@Added_By_Role)", con, t);
            }
            else if (For == "Update")
            {
                cmd = new SqlCommand(@"update [electionapp].[Wishes_Master] set Title=@Title,Description=@Description,
                Image=@Image where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else if (For == "Delete")
            {
                cmd = new SqlCommand(@"delete from [electionapp].[Wishes_Master] where Id=@Id and Org_Id=@Org_Id", con, t);
            }
            else
            {
                cmd = new SqlCommand(@"", con, t);
            }

            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Image", Image);
            cmd.Parameters.AddWithValue("@Added_By_Id", Added_By_Id);
            cmd.Parameters.AddWithValue("@Added_By_Name", Added_By_Name);
            cmd.Parameters.AddWithValue("@Added_By_Role", Added_By_Role);
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
    public void SendWishesImg()
    {
        StatusWithError status = new StatusWithError();
        status.status = 0;
        try
        {
            var request = HttpContext.Current.Request;
            var file = request.Files["file"];
            file.SaveAs(HttpContext.Current.Server.MapPath("~/Election/Wishes/" + file.FileName));

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
    public void GetWishes(string Type, string Id)
    {

        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd;

        if (Type == "All")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Wishes_Master] where Org_Id=@Org_Id order by Id desc", con);
        }
        else if (Type == "ById")
        {
            cmd = new SqlCommand(@"SELECT *, convert(varchar, [Date], 0) as [Date_Convert]
            FROM [electionapp].[Wishes_Master] where Id=@Id  and Org_Id=@Org_Id order by Id desc", con);
        }
        else
        {
            cmd = new SqlCommand(@"", con);
        }

        cmd.Parameters.AddWithValue("@Org_Id", Org_ID);
        cmd.Parameters.AddWithValue("@Id", Id);

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        try
        {
            con.Open();

            DataTable dt = new DataTable();

            new SqlDataAdapter(cmd).Fill(dt);
            dt.Columns.Remove("Date");

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
            con.Close();
        }

        Context.Response.Write(new JavaScriptSerializer().Serialize(list));


    }



    public class Status
    {
        public int status { get; set; }
    }

    public class StatusWithError
    {
        public int status { get; set; }
        public string ex { get; set; }
    }

    public class VerificationStatus
    {
        public int status { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }

        public VerificationStatus() { }
    }


    public class StaffProfile
    {
        public string Id { get; set; }
        public string Org_Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DOJ { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string GCM { get; set; }
        public string Added_By_Name { get; set; }
        public string Added_By_Id { get; set; }
        public string Added_On { get; set; }

    }

    public class Dashboard
    {
        public string Pending { get; set; }
        public string Resolved { get; set; }
        public string Completed { get; set; }
        public string PendingOrders { get; set; }
        public string CanceledOrders { get; set; }
        public string CompletedOrders { get; set; }
        public string StockInCount { get; set; }
        public string StockOutCount { get; set; }


    }

    public class BarcodeVerify
    {
        public string Product_Id { get; set; }
        public string Product_Code { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Unit_Of_Measurement { get; set; }
        public string Opening_Qty { get; set; }
        public string Barcode { get; set; }

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


}
