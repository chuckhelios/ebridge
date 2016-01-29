using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;

public partial class admin_email_amazon_post_survey : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Emails = Db.GetRecords("SELECT a.code, p.email, p.name_first, a.id FROM amazon_post_survey a, participant p WHERE a.id = p.id AND a.code <> NULL");

        string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
        string _body = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\admin\\amazon_post_survey.txt").Replace("\r", "");

        for (int i = 0; i < Emails.Length; i++)
        {
            string DialogBody = _body;

            string ParticipantId = Emails[i][3];
            string ParticipantEmail = Emails[i][1];
            string NameFirst = Emails[i][2];
            string AmazonGiftCode = Emails[i][0];

            DialogBody = DialogBody.Replace("_NAME_FIRST_", NameFirst);
            DialogBody = DialogBody.Replace("_AMAZON_GIFT_CODE_", AmazonGiftCode);

            try
            {
                //Utility.SendPHPMail(ParticipantEmail, "ebridgeteam@umich.edu", "The eBridge Team", "the $25.00 Amazon.com gift card", DialogBody);

                Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + (i + 1).ToString() + ". " + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + "</SPAN><BR>");
                //Response.Write(DialogBody.Replace("\n", "<BR>") + "<P>");
                Response.Flush();

                //Utility.SendPHPMail("kzheng@umich.edu", "ebridgeteam@umich.edu", "The eBridge Team", "the $25.00 Amazon.com gift card", DialogBody);
            }
            catch
            {
                i = i - 1;
                Response.Write("<SPAN style='font-family:arial;font-size:12px'>" + ParticipantId + " (" + ParticipantEmail + "), " + DateTime.Now.ToString() + ", error, wait and try again</SPAN><BR>"); Response.Flush();
                Thread.Sleep(45000);
            }

            Thread.Sleep(15000);
        }

        Response.Write("<P><SPAN style='font-family:arial;font-size:12px'>Done.</SPAN>");
    }
}