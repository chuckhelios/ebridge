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

public partial class admin_email_everybody : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Emails = Db.GetRecords("SELECT id, email, name_first FROM participant ORDER BY id");

        string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
        string _body = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\admin\\recruit_email.txt").Replace("\r", "");

        for (int i = 6709; i < Emails.Length; i++)
        {
            string DialogBody = _body;

            string ParticipantId = Emails[i][0];
            string ParticipantEmail = Emails[i][1];
            string NameFirst = Emails[i][2];
            string SurveyCompleted = Utility.GetLog(ParticipantId, "SURVEY COMPLETED");

            //string ParticipantId = "G6tE9FJ9gl";
            //string ParticipantEmail = "kzheng@umich.edu";
            //string NameFirst = "Kai";

            DialogBody = DialogBody.Replace("_NAME_FIRST_", NameFirst);
            DialogBody = DialogBody.Replace("_URL_", BASE_URL + "/survey/index.aspx?p=" + ParticipantId);

            try
            {
                if (SurveyCompleted == "NULL")
                {
                    //Utility.SendPHPMail(ParticipantEmail, "ebridgeteam@umich.edu", "The eBridge Team", "Students' eBridge to Health (Final Opportunity)", DialogBody);
                    Thread.Sleep(15000);

                    Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + "</SPAN><BR>");
                }
                else
                {
                    Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + " --- skip, taken on " + SurveyCompleted + "</SPAN><BR>");
                }

                Response.Flush();
            }
            catch
            {
                Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + ", error, wait and try again</SPAN><BR>"); Response.Flush();
                i = i - 1;
                Thread.Sleep(60000);
            }
        }

        Response.Write("<P><SPAN style='font-family:arial;font-size:12px'>Done.</SPAN>");
    }
}