using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class chat_action : System.Web.UI.Page
{
    string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
    Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString[0] == "chat_input")
        {
            string ChatApptId = Request.QueryString[1];
            string ChatDisplayMode = Request.QueryString[2];
            string ParticipantId = Request.QueryString[3];
            string DesignatedCounselor = Utility.GetStatus(ParticipantId, "COUNSELOR");
            
            //one the server it has to be this way
            string DialogBody = Request.Form[3].Trim();

            //string DialogBody = Request.Form[1].Trim();

            if (ChatDisplayMode != "c") Db.Execute("INSERT INTO message (from_id, message_body, date_time, remark) VALUES ('" + ParticipantId + "','" + "[CHAT] " + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "','" + ChatApptId + "')");
            else Db.Execute("INSERT INTO message (to_id, message_body, date_time, remark) VALUES ('" + ParticipantId + "','" + "[CHAT] " + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "','" + ChatApptId + "')");

            Response.Write("<SCRIPT type='text/javascript'>location.replace('chat_input.aspx?p=" + ChatApptId + "&p1=" + ChatDisplayMode + "&p2=" + ParticipantId + "')</SCRIPT>");
        }

        if (Request.QueryString[0] == "add_schedule")
        {
            string CID = Request.Form[0];

            try
            {
                string ParticipantId = Request.Form[1].Split(',')[1].Trim();

                string[] _s = Request.Form[1].Split(',')[0].Split('-');

                _s[0] = _s[0].Trim();
                _s[1] = _s[0].Split(' ')[0] + " " + _s[1].Trim();

                Db.Execute("INSERT INTO schedule (couselor_id, datetime_start, datetime_end, participant_id, email_reminder_day, email_reminder_hour) VALUES('"
                    + CID + "',#" + _s[0] + "#, #" + _s[1] + "#,'" + ParticipantId + "',0,0)");
                Response.Write("<SCRIPT type='text/javascript'>location.replace('schedule.aspx?p=" + CID + "');</SCRIPT>");
            }
            catch
            {
                Response.Write("<SCRIPT type='text/javascript'>alert('What have you been smoking? Check the date format and try again.');history.go(-1);</SCRIPT>");
            }
        }

        if (Request.QueryString[0] == "remove_schedule")
        {
            Db.Execute("DELETE FROM schedule WHERE unique_id = " + Request.QueryString[1]);
            Response.Write("<SCRIPT type='text/javascript'>location.replace('schedule.aspx?p=" + Request.QueryString[2] + "');</SCRIPT>");
        }

        if (Request.QueryString[0] == "chat_login")
        {
            string ChatId = Request.Form[0];
            string ParticipantId = Request.Form[1];
            string Password = Request.Form[2];

            // log activity
            Utility.LogActivity(ParticipantId, "PARTICIPANT LOGGED IN (CHAT)", Request.UserHostAddress + "|" + Request.UserAgent);

            if (Password == Db.GetRecord("SELECT password FROM participant WHERE id = '" + ParticipantId + "'")[0])
            {
                Session["PARTICIPANT_LOGIN"] = "Y";
                Session["DESIGNATED_COUNSELOR"] = Utility.GetStatus(ParticipantId, "COUNSELOR");

                Db.Execute("DELETE FROM online_status WHERE id = '" + ParticipantId + "'");
                //Db.Execute("INSERT INTO online_status VALUES ('" + ParticipantId + "','" + Session["DESIGNATED_COUNSELOR"].ToString() + "','" + DateTime.Now + "')");

                Response.Redirect("../chat/?p=" + ChatId + "&p1=p&p2=" + ParticipantId);
            }
            else
            {
                Utility.LogActivity(ParticipantId, "PARTICIPANT LOGIN FAILED", Request.UserHostAddress + "|" + Request.UserAgent);
                Response.Write("<SCRIPT type='text/javascript'>alert('The password you entered is incorrect. Please try again.');history.go(-1);</SCRIPT>");
            }
        }

        if (Request.QueryString[0] == "retrieve_pwd")
        {
            string ParticipantId = Request.QueryString[1];

            // log activity
            Utility.LogActivity(ParticipantId, "PASSWORD RETRIEVED", Request.UserHostAddress + "|" + Request.UserAgent);

            string[] Participant = Db.GetRecord("SELECT name_first, email, password FROM participant WHERE id = '" + ParticipantId + "'");

            if (Participant == null) { Response.Redirect("login.aspx?p=" + ParticipantId); return; }

            SchoolInfo = Helper.FindSchool(ParticipantId);

            string ParticipantFirstName = Participant[0];
            string ParticipantEmail = Participant[1];

            string MsgBody = "Below is your password at eBridge: " + Participant[2];

            MsgBody += PageElements.EmailSignature();

            /*if ((bool)Application["DEBUG"])*/ Utility.SendPHPMail(ParticipantEmail, SchoolInfo["email"], "The eBridge Team", "eBridge: Password Recovery", MsgBody);
            //else Utility.SendGMail(ParticipantEmail, "participant", SchoolInfo["email"], "The eBridge Team", "eBridge: Password Recovery", MsgBody);

            Response.Write("<SCRIPT type='text/javascript'>alert('Your password has been sent to your email.');history.go(-1);</SCRIPT>");
        }

        /*if (Request.QueryString[0] == "cancel_chat_appt")
        {
            Db.Execute("DELETE FROM schedule WHERE unique_id = " + Request.QueryString[1] + " AND participant_id = '" + Request.QueryString[1] + "'");
            Response.Write("<SCRIPT type='text/javascript'>alert('Your appointment with the Counselor has been cancelled');</SCRIPT>");
        }

        if (Request.QueryString[0] == "add_chat_appt")
        {
            string ParticipantId = Request.Form[0];
            string ChartUniqueId = Request.Form[1];

            Db.Execute("UPDATE schedule SET participant_id = '" + ParticipantId + "' WHERE unique_id = " + ChartUniqueId);

            DateTime ApptDateTime = DateTime.Parse(Db.GetRecord("SELECT datetime_start FROM schedule WHERE unique_id = " + ChartUniqueId)[0]);

            Response.Write(PageElements.PageHeader("Plain"));
            Response.Write("Your appointment with the Counselor has been scheduled. You will receive an email with a link to a private and "
                    + "secure IM chat with the Counselor to be used at the time of your scheduled chat. "
                    + "You will also be provided with a link if you need to reschedule the chat.");

            string EmailBody = "Thank you again for completing the survey. Below please find the informaiton about your appointment to chat privately online with the Counselor:\r\n"
                    + ApptDateTime.DayOfWeek + ", " + ApptDateTime.ToShortDateString() + "\n"
                    + BASE_URL + "/dialog/chart.aspx?p=" + ParticipantId + "\r\n"
                    + "To cancel, please click click the following link:\n"
                    + BASE_URL + "/dialog/action.aspx?p=cancel_chat_appt&amp;p1=" + ChartUniqueId + "&p2=" + ParticipantId;

            string ParticipantEmail = Db.GetRecord("SELECT email FROM participant WHERE id = '" + ParticipantId + "'")[0];
            Utility.SendPHPMail(ParticipantEmail, "ebridgeteam@umich.edu", "The eBridge Team", "your appointment to chat with ebridge counselor", EmailBody);

            string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor");
            string CID = Utility.GetStatus(ParticipantId, "COUNSELOR");

            for (int i = 0; i < CounselorEmail.Length; i++)
            {
                if (CounselorEmail[i][0].Substring(0, 1) != "#")
                {
                    string CounselorName = Db.GetRecord("SELECT name_first & ' ' & name_last FROM counselor WHERE id = '" + CID + "'")[0];
                    Utility.SendPHPMail(CounselorEmail[i][0], "ebridgeteam@umich.edu", "The eBridge Team", "eBridge: Student (assigned to "
                        + CounselorName + ") made a chat appointment", "The student is assigned to "
                        + CounselorName + ".\r\n" + BASE_URL + "/chat/schedule.aspx?p=" + CID);
                }
            }

            Response.Write(PageElements.PageFooter(false));
        }*/
    }
}
