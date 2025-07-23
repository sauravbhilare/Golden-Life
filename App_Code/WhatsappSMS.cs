using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for WhatsappSMS
/// </summary>
public static class WhatsappSMS
{
    
    public static void Payment(string mobileno, string msgtext)
    {

        try
                {

            WebClient client = new WebClient();
            string baseurl = "http://bulkwhatsapp.live/wapp/api/send?apikey=d8c9f145-abc1-41af-9a95-70678265b5fd&mobile=91" + mobileno.ToString() + "&msg=" + msgtext.ToString();

            client.OpenRead(baseurl);
        }
                catch (Exception ex)
        {
            // throw new ApplicationException(ex.ToString());
        }
    }

}