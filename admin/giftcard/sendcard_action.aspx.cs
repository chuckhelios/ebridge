using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class admin_giftcard_sendcard_action : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	Response.Buffer = false;
		
        if (Session["giftcard"] == null) { Response.Redirect("index.aspx"); return; }

        if (Request.Form["ID"].ToString().Trim().Length == 0 || Request.Form["ID"].ToString().Trim().Length == 0)
        {
            Response.Write("Study IDs and Amazon Codes cannot be null.");
            return;
        }

        string[] StudyID = Request.Form["ID"].ToString().Trim().Split('\r');
        string[] AmazonCode = Request.Form["AMAZON"].ToString().Trim().Split('\r');

        if (StudyID.Length != AmazonCode.Length)
        {
            Response.Write("There are " + StudyID.Length + " Study IDs, and " + AmazonCode.Length
                + " Amazon Codes. The numbers do not match. Go back and fix.");
            return;
        }

        // to force buffer to be sent for some browsers
        Response.Write("<!DOCTYPE HTML><BODY><SPAN style='display:none'>");
        for (int i = 0; i < 200; i++) Response.Write(i.ToString());
        Response.Write("</SPAN>\r\n");

        Response.Write("<SPAN style='font-family:arial;font-size:12px'>");
        Response.Write("Please wait. Emails are being sent.<P>");
        Response.Flush();

        string[] _r = null;
        string _email_masked = "";

        for (int i = 0; i < StudyID.Length; i++)
        {
            StudyID[i] = StudyID[i].Trim();
            AmazonCode[i] = AmazonCode[i].Trim();

            _r = Db.GetRecord("SELECT email, name_first FROM participant WHERE id = '" + StudyID[i] + "'");

            try
            {
                SendEmail(StudyID[i], _r[0], _r[1], AmazonCode[i]);

                _email_masked = _r[0].Split('@')[0].Substring(0, 1) + "***" + _r[0].Split('@')[0].Substring(_r[0].Split('@')[0].Length - 1, 1) + "@" + _r[0].Split('@')[1];

                Response.Write("#" + (i + 1).ToString() + ": " + StudyID[i] + ", sent to "+_email_masked+".<BR>");
                Response.Flush();

                Thread.Sleep(1000);
            }
            catch
            {
                Response.Write("#" + (i + 1).ToString() + ": " + StudyID[i] + " " + AmazonCode[i] + ", sent failed");
                Response.Flush();

                Thread.Sleep(1000);
            }
        }

        Response.Write("<BR>Done.</SPAN></BODY></HTML>");
    }

    protected void SendEmail(string StudyID, string RecipientEmail, string RecipientName, string AmazonCode)
    {
        string SenderEmail = "";
        string BCCEmail = "kzheng@umich.edu";// "reblin@med.umich.edu";

        if (StudyID.Substring(8, 1) == "m") SenderEmail = "ebridgeteam@umich.edu";
        if (StudyID.Substring(8, 1) == "n") SenderEmail = "ebridge@unr.edu";
        if (StudyID.Substring(8, 1) == "s") SenderEmail = "ebridgeproject@stanford.edu";
        if (StudyID.Substring(8, 1) == "i") SenderEmail = "ebridgeteam@uiowa.edu"; // confirm

        string SenderName = "The eBridge Team";

        string Subject = Request.Form["SUBJECT"].ToString();
        string Body = Request.Form["BODY"].ToString();

        Body = Body.Replace("_AMAZON_GIFT_CODE_", AmazonCode);

        Utility.SendPHPMail(RecipientEmail, SenderEmail, SenderName, Subject, Body.Replace("_NAME_FIRST_", RecipientName));

        // Utility.SendPHPMail(BCCEmail, SenderEmail, SenderName, Subject, Body.Replace("_NAME_FIRST_", "<first name (masked)>"));

    }
}