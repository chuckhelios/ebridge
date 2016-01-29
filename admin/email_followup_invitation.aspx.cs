using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Threading;

public partial class admin_email_followup_invitation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Emails = Db.GetRecords("SELECT id, email, name_first FROM participant WHERE id IN (SELECT DISTINCT id FROM status WHERE status_code = 'INTERVENTION' AND ID <> 'BykwTZZhaB') ORDER BY id");
        string[][] AmazonCode = Db.GetRecords("SELECT code FROM amazon ORDER BY code");

        string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
        string _body = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\admin\\followup_survey_email.txt").Replace("\r", "");

        for (int i = 0; i < Emails.Length; i++)
        {
            string DialogBody = _body;

            string ParticipantId = Emails[i][0];
            string ParticipantEmail = Emails[i][1];
            string NameFirst = Emails[i][2];
            string AmazonGiftCode = AmazonCode[i][0];

            //string ParticipantId = "G6tE9FJ9gl";
            //string ParticipantEmail = "kzheng@umich.edu";
            //string NameFirst = "Kai";
            //string AmazonGiftCode = "codecode";

            DialogBody = DialogBody.Replace("_NAME_FIRST_", NameFirst);
            DialogBody = DialogBody.Replace("_URL_", BASE_URL + "/survey2/index.aspx?p=" + ParticipantId);
            DialogBody = DialogBody.Replace("_AMAZON_GIFT_CODE_", AmazonGiftCode);

            try
            {
                if (Utility.GetLog(ParticipantId, "FOLLOWUP SURVEY COMPLETED") != "NULL")
                {
                    Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + "</SPAN><BR>"); Response.Write("Skip<P>");
                    Response.Flush();
                }
                else
                {
                    //Utility.SendPHPMail(ParticipantEmail, "ebridgeteam@umich.edu", "The eBridge Team", "Last reminder about follow-up questionnaire for e-Bridge to health", DialogBody);
                    Thread.Sleep(15000);

                    Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() +"<P>" + DialogBody.Replace("\n", "<BR>") + "<P>" + "</SPAN><BR>"); 
                    Response.Flush();
                }
            }
            catch
            {
                i = i - 1;
                Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + ", error, wait and try again</SPAN><BR>"); Response.Flush();
                Thread.Sleep(60000);
            }
        }

        Response.Write("<P><SPAN style='font-family:arial;font-size:12px'>Done.</SPAN>");
    }
}