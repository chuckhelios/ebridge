using System;
using System.Collections;
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
using System.Linq;
using System.Web.Script.Serialization;


public partial class dialog_action : System.Web.UI.Page
{
    string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
    Dictionary<string, string> SchoolInfo;

    // webmethod have to be declared as static methods, which mean they can't access any of the object variables
    // but session variables are available afaik with the option below. 

    [System.Web.Services.WebMethod(EnableSession=true)]
    public static string ParticipantLookup(string prtid)
    {
        ebridgeEntities _db = new ebridgeEntities();
        JavaScriptSerializer serial = new JavaScriptSerializer();
        Dictionary<string, string> SchoolInfo = HttpContext.Current.Session["SCHOOL"] as Dictionary<string, string>;
        string CounselorId = HttpContext.Current.Session["COUNSELOR_ID"] as string;
        if (SchoolInfo == null)
        {
            HttpContext.Current.Session.Clear();
            return "invalid login";
        }
        else {
            PARTICIPANT _p = _db.PARTICIPANT.First(p=>p.ID==prtid);
            if (_p != null) return serial.Serialize(new Dictionary<string, string>(){ {"name_first" , _p.NAME_FIRST}, {"gender", _p.GENDER } });
            else return "failure";
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SendEmail(string prt, string sbj, string msg){
        ebridgeEntities _db = new ebridgeEntities();
        JavaScriptSerializer serial = new JavaScriptSerializer();
        Dictionary<string, string> SchoolInfo = HttpContext.Current.Session["SCHOOL"] as Dictionary<string, string>;
        string CounselorId = HttpContext.Current.Session["COUNSELOR_ID"] as string;

        if (SchoolInfo == null)
        {
            HttpContext.Current.Session.Clear();
            return "redirect";
        }
        // log activity
        
        PARTICIPANT _p = _db.PARTICIPANT.First(p=>p.ID==prt);
        if (_p != null) { 
            string EmailToAddress = _p.EMAIL;
            Utility.SendPHPMail(EmailToAddress, SchoolInfo["email"], "The eBridge Team", sbj, msg);
            Utility.LogActivity(SchoolInfo["code"] + "_" + CounselorId, "EMAIL SENT", "to participant: " + _p.ID);
            return "success";
        }
        else
        {
            return "no match";
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string Login(string ut, string usr, string pwd, string rd, string nn)
    {
        ebridgeEntities _db = new ebridgeEntities();
        HttpContext.Current.Session.Clear();
        JavaScriptSerializer serial = new JavaScriptSerializer();

        if (ut == "participant") {
            if (_db.PARTICIPANT.Any(p => p.ID == usr && p.PASSWORD == pwd))
            {
                HttpContext.Current.Session["PARTICIPANT_ID"]=usr;
                HttpContext.Current.Session["SCHOOL"]=Helper.FindSchool(usr);
                HttpContext.Current.Session["DESIGNATED_COUNSELOR"]=Utility.GetStatus(usr, "COUNSELOR");
            }
            else return serial.Serialize(new Dictionary<string, string>() { { "message", "no match" } });
        }
        else if (ut == "counselor") {
            COUNSELOR _c = _db.COUNSELOR.First(c=>c.USERNAME==usr && c.PASSWORD==pwd);
            if (_c != null)
            {
                HttpContext.Current.Session["COUNSELOR_ID"] = _c.ID;
                HttpContext.Current.Session["SCHOOL"]=Helper.FindSchool(_c.SITE);
            }
            else return serial.Serialize(new Dictionary<string, string>() { { "message", "no match" } });
        }
        if (rd.Contains("login_alt.aspx")) rd = "list_alt.aspx";
        return serial.Serialize(new Dictionary<string, string>() { { "redirect", rd }, { "message", "success" } });
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string setHours(string[] hours)
    {
        JavaScriptSerializer serial = new JavaScriptSerializer();

        string CounselorId= HttpContext.Current.Session["COUNSELOR_ID"] as string;
        Dictionary<string, string> SchoolInfo = HttpContext.Current.Session["SCHOOL"] as Dictionary<string, string>;
        if (string.IsNullOrEmpty(CounselorId) || SchoolInfo == null) return "failure";

        string _tmp = "";
        foreach (string v in hours)
        {
            _tmp += v + '|';
        }
        _tmp = _tmp.Substring(0, _tmp.Length - 1);

        ebridgeEntities _db = new ebridgeEntities();
        _db.SITE.Add(new SITE{ DATE_TIME= DateTime.Now, ID=SchoolInfo["code"], HOURS=_tmp});
        _db.SaveChanges();
        return "success";

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString[0] == "change_hours")
        {

            // needs to be some validation
            string CounselorSite = Session["SCHOOL"] as string;
            string CounselorId = Session["COUNSELOR_ID"] as string;

            if (string.IsNullOrEmpty(CounselorSite) || string.IsNullOrEmpty(CounselorId))
            {
                Session.Abandon(); Response.Redirect("login_counselor.aspx?p=list"); return;
            }
            
            string QueryString = "INSERT INTO SITE VALUES ('{0}','{1}','{2}')";

            string _tmp = "";

            string[] Weekdays = new string[6] { "Title", "Mon", "Tue", "Wed", "Thur", "Fri" };

            foreach (string key in Weekdays)
            {
                _tmp+=Request.Form[key]+'|';
            }
            _tmp = _tmp.Substring(0, _tmp.Length - 1);

            Db.Execute(string.Format(QueryString, new string[3] {CounselorSite, _tmp, DateTime.Now.ToString()}));
            Utility.LogActivity(CounselorSite + "_" + CounselorId, "HOURS CHANGED", Request.UserHostAddress + "|" + Request.UserAgent);

            Response.Write("<SCRIPT type='text/javascript'>alert('Hours changed successfully.');</SCRIPT>");
            Response.Write("<SCRIPT type='text/javascript'>parent.location='list.aspx';</SCRIPT>");
            return;
        }

        if (Request.QueryString[0] == "student_post")
        {
            Session["IniMessage"] = null;

            string ParticipantId = Request.Form[0];
            string DialogBody = Request.Form[1];
            SchoolInfo= Helper.FindSchool(ParticipantId);
            string BASE_URL;
            if ((bool)Application["DEBUG"]) BASE_URL = SchoolInfo["test_server"];
            else BASE_URL = SchoolInfo["email_server"];

            if (Request.Form.Count == 2)
            {
                Db.Execute("INSERT INTO message (from_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");

                DialogBody = "Participant " + ParticipantId + " posted the following message at eBridge:\r\n\n" + DialogBody.Replace("\r", "") + "\r\n\n To reply, click " + BASE_URL + "/dialog/thread_counselor.aspx?p=" + ParticipantId;

                if (DialogBody.IndexOf("I would like to schedule a private session to chat with the Counselor online") != -1)
                {
                    Db.Execute("INSERT INTO message (to_id, message_body, date_time) VALUES ('" + ParticipantId + "','The Counselor will look over your suggested times and respond to you within 24 hours with a confirmed time. "
                        + "In the meantime feel free to post a message to the Counselor.','" + DateTime.Now.ToString() + "')");
                }
                
            }
            else
            {
                string RespondingToId = Request.Form[2];
                string BriefResponse = Request.Form[3];

                Db.Execute("INSERT INTO message (from_id, message_body, date_time, responding_to) VALUES ('" + ParticipantId + "','" + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "'," + RespondingToId + ")");
                if (BriefResponse != "") Db.Execute("INSERT INTO message (from_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + BriefResponse.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");
                DialogBody = "Participant " + ParticipantId + " posted the following message at eBridge:\r\n\n" + "Responding to a posted question: " + DialogBody.Replace("\r", "") + "\r\n\nTo reply, click " + BASE_URL + "/dialog/thread_counselor.aspx?p=" + ParticipantId;
            }

            string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor where site='"+SchoolInfo["code"]+"'");

            for (int i = 0; i < CounselorEmail.Length; i++)
            {
                if (CounselorEmail[i][0].Substring(0, 1) != "#")
                {

                    string CounselorName = Db.GetRecord("SELECT name_first, name_last FROM counselor WHERE site = '"+SchoolInfo["code"]+ "' and id = '" + Session["DESIGNATED_COUNSELOR"].ToString() + "'")[0];
                    string email_subject = "eBridge: Student (assigned to " + CounselorName + ") posted a message at " + DateTime.Now.ToString();
                    string email_body = DialogBody;
                    Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
                    //if ((bool)Application["DEBUG"]) Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
                    //else Utility.SendGMail(CounselorEmail[i][0], "counselor", SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
				}
            }

            Response.Redirect("thread.aspx?p=" + ParticipantId);
        }

        /*
        if (Request.QueryString[0] == "student_im_post")
        {
            string ParticipantId = Request.Form[0];
            string DialogBody = Request.Form[1];
            string DesignatedCounselorId = Session["DESIGNATED_COUNSELOR"].ToString();

            Db.Execute("INSERT INTO message (from_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + "[IM] " + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");
            string LastMessageId = Db.GetRecord("SELECT MAX(id) FROM message WHERE from_id = '" + ParticipantId + "'")[0];
            Db.Execute("INSERT INTO chat_notice VALUES (" + LastMessageId + ",'" + ParticipantId + "','" + DesignatedCounselorId + "')");

            Response.Write("<SCRIPT type='text/javascript'>parent.Lightview.hide();</SCRIPT>");
        }

        if (Request.QueryString[0] == "student_ignore_im")
        {
            Db.Execute("UPDATE message SET remark = 'read' WHERE to_id = '" + Request.Form[0] + "' AND LEFT(message_body,4) = '[IM]'");
            Response.Write("<SCRIPT type='text/javascript'>window.location.href='chat.aspx?p=participant&p1=" + Request.Form[0] + "';</SCRIPT>");
        }
        */

        if (Request.QueryString[0] == "counselor_post")
        {
            string ParticipantId = Request.Form[0];
            string ParticipantFirstName = Request.Form[1];
            string ParticipantEmail = Request.Form[2];
            string DialogBody = Request.Form[3];
            SchoolInfo = Helper.FindSchool(ParticipantId);
            string BASE_URL;
            if ((bool)Application["DEBUG"]) BASE_URL = SchoolInfo["test_server"];
            else BASE_URL = SchoolInfo["email_server"];

            //if (Db.GetRecords("SELECT * FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "'") == null)
            //{
                //string InitialMessage = "New automated  message: Hello, I'm the eBridge Counselor. I look forward to talking with you online. I will respond to your messages within 24 hours. Just a reminder I can't see your email address or any identifying information about you.";

                //Db.Execute("INSERT INTO message (to_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + InitialMessage.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");
            //}

            Db.Execute("INSERT INTO message (to_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");

            string __constructMessage = DialogBody.Replace("\r", "");
            if (__constructMessage.IndexOf("_") != -1) __constructMessage = "[The content of the message cannot be displayed. To view it, please log on to eBridge.]";
            else if (__constructMessage.Length > 250) __constructMessage = __constructMessage.Substring(0, 250) + " ...";

            DialogBody = "You have a new message at eBridge. \r\n\n" + __constructMessage + "\r\n\nTo reply, please go to the site shown below:\r\n\n"
                    + BASE_URL + "/dialog/login.aspx?p=" + ParticipantId;

            //if (Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + ParticipantId + "'") < 4) DialogBody += "\r\nAs a reminder, your password is " + Db.GetRecord("SELECT password FROM participant WHERE id = '" + ParticipantId + "'")[0] + ".";
            DialogBody += "\r\n\n As a reminder, your password is: " + Db.GetRecord("SELECT password FROM participant WHERE id = '" + ParticipantId + "'")[0] + ".";
            DialogBody += "\r\n\n Please note that the eBridge website is not monitored 24/7. However, you will respond a response within 24 hours.";
            DialogBody += PageElements.EmailSignature();
            Utility.SendPHPMail(ParticipantEmail, SchoolInfo["email"], "The eBridge Team", "eBridge: Counselor posted a message at " + DateTime.Now.ToString(), DialogBody);
            //if((bool)Application["DEBUG"]) Utility.SendPHPMail(ParticipantEmail, SchoolInfo["email"], "The eBridge Team", "eBridge: Counselor posted a message at " + DateTime.Now.ToString(), DialogBody);
            //else Utility.SendGMail(ParticipantEmail, "participant", SchoolInfo["email"], "The eBridge Team", "eBridge: Counselor posted a message at " + DateTime.Now.ToString(), DialogBody);

            Response.Redirect("thread_counselor.aspx?p=" + ParticipantId);
        }

        /*
        if (Request.QueryString[0] == "counselor_im_post")
        {
            string ParticipantId = Request.Form[0];
            string MessageId = Request.Form[1];
            string DialogBody = Request.Form[2];

            Db.Execute("INSERT INTO message (to_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + "[IM] " + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");
            Db.Execute("DELETE FROM chat_notice WHERE message_id = " + MessageId);

            Response.Write("<SCRIPT type='text/javascript'>parent.Lightview.hide();</SCRIPT>");
        }
        */

        if (Request.QueryString[0] == "counselor_logout")
        {
            string CounselorId = Session["COUNSELOR_ID"] as string; string CounselorSite = Session["SCHOOL"] as string;
            if (string.IsNullOrEmpty(CounselorId) || string.IsNullOrEmpty(CounselorSite))
            {
                Session.Abandon();
                Response.Write("<SCRIPT type='text/javascript'>alert('Session had already timed out.');</SCRIPT>");
            }
            else
            {
                string QueryString = "DELETE FROM online_status WHERE id = '{0}' and site ='{1}'";
                Db.Execute(string.Format(QueryString, new string[2]{CounselorId, CounselorSite}));
                // log activity
                Utility.LogActivity(CounselorId + '_' + CounselorSite, "COUNSELOR LOGOUT", Request.UserHostAddress + "|" + Request.UserAgent);
                Session.Abandon();
                Response.Write("<SCRIPT type='text/javascript'>alert('Logged out successfully.');</SCRIPT>");
            }
            // this handles the redirect in javascript
            Response.Write("<SCRIPT type='text/javascript'>parent.location='list.aspx';</SCRIPT>");
            Response.Redirect("list.aspx");
            return;
        }

        if (Request.QueryString[0] == "counselor_login")
        {

            if (Session["COUNSELOR_ID"] != null) Session.Abandon();

            string PageRequested = Request.Form[0];
            string Username = Request.Form[1];
            string Password = Request.Form[2];

            string[] CounselorId = Db.GetRecord("SELECT id, username, site FROM counselor WHERE username = '" + Username + "' AND password = '" + Password + "'");

            if (CounselorId != null)
            {
                Session["PARTICIPANT_LOGIN"] = "N";
                Session["COUNSELOR_ID"] = CounselorId[0];
                Session["SCHOOL"]= CounselorId[2];
                Session.Timeout=150;

                if (Request.Form.Count == 3)
                {
                    // log activity
                    Utility.LogActivity(CounselorId[1] + '_' + CounselorId[2], "COUNSELOR LOGIN", Request.UserHostAddress + "|" + Request.UserAgent);

                    Db.Execute("Insert into online_status values ('" + CounselorId[0] + "', 'COUNSELOR', '" + DateTime.Now + "','" + CounselorId[2] + "')");
                }

                if (PageRequested == "list")
                {
                    if (Request.Form.Count == 3) Response.Redirect("list.aspx");
                    else Response.Redirect("list.aspx");
                }
                else if (PageRequested == "transcript") Response.Redirect("transcript.aspx");
                else if (PageRequested == "email") Response.Redirect("email.aspx");
                else if (PageRequested.Contains("thread_counselor")) Response.Redirect(PageRequested);
                else if (PageRequested == "chat") Response.Redirect(Session["REDIRECT"].ToString());
                else Response.Redirect("frame.aspx?p=counselor&p1=" + PageRequested);
            }
            else
            {
                Response.Write("<SCRIPT type='text/javascript'>alert('The password you entered is incorrect. Please try again.');history.go(-1);</SCRIPT>");
            }
            return;
        }

        if (Request.QueryString[0] == "participant_login")
        {
            string ParticipantId = Request.Form[0];
            string Password = Request.Form[1];

            // log activity
            Utility.LogActivity(ParticipantId, "PARTICIPANT LOGGED IN", Request.UserHostAddress + "|" + Request.UserAgent);
            string[] Participant = Db.GetRecord("SELECT password, email FROM participant WHERE id = '" + ParticipantId + "'");
            if (Password == Participant[0])
            {
                SchoolInfo = Helper.FindSchool(ParticipantId);
                Session["SCHOOL"] = SchoolInfo;
                Session["PARTICIPANT_ID"]=ParticipantId;
                Session["PARTICIPANT_LOGIN"] = "Y";
                Session["EMAIL"] = Participant[1];
                Session["DESIGNATED_COUNSELOR"] = Utility.GetStatus(ParticipantId, "COUNSELOR");
                Session.Timeout=150;

                Db.Execute("DELETE FROM online_status WHERE id = '" + ParticipantId + "'");
				
                Db.Execute("INSERT INTO online_status VALUES ('" + ParticipantId + "','" + Session["DESIGNATED_COUNSELOR"].ToString() + "','" + DateTime.Now + "','" + SchoolInfo["code"]+ "')");

                Response.Redirect("dialog_alt.aspx?p=participant&p1=" + ParticipantId);
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

            string ParticipantFirstName = Participant[0];
            string ParticipantEmail = Participant[1];
            SchoolInfo=Helper.FindSchool(ParticipantId);
            string BASE_URL;
            if ((bool)Application["DEBUG"]) BASE_URL = SchoolInfo["test_server"];
            else BASE_URL = SchoolInfo["email_server"];


            string MsgBody = "Below is your password at eBridge:\r\n" + Participant[2] + "\r\nPlease use it to logon to the site shown below:\r\n"
                    + BASE_URL + "/dialog/login.aspx?p=" + ParticipantId;

            MsgBody += PageElements.EmailSignature();

            Utility.SendPHPMail(ParticipantEmail, SchoolInfo["email"], "The eBridge Team", "eBridge: Password Recovery", MsgBody);
            //if ((bool)Application["DEBUG"]) Utility.SendPHPMail(ParticipantEmail, SchoolInfo["email"], "The eBridge Team", "eBridge: Password Recovery", MsgBody);
            //else Utility.SendGMail(ParticipantEmail, "participant", SchoolInfo["email"], "The eBridge Team", "eBridge: Password Recovery", MsgBody);
			
            Response.Write("<SCRIPT type='text/javascript'>alert('Your password has been sent to your email.');history.go(-1);</SCRIPT>");
        }

        if (Request.QueryString[0] == "change_pwd")
        {
            string ParticipantId = Request.Form[0];

            // log activity
            Utility.LogActivity(ParticipantId, "PASSWORD CHANGED", Request.UserHostAddress + "|" + Request.UserAgent);

            Db.Execute("UPDATE participant SET [password] = '" + Request.Form[1] + "' WHERE id = '" + ParticipantId + "'");
            Response.Write("<SCRIPT type='text/javascript'>alert('Password updated successfully.');window.location.replace('login.aspx?p=" + Request.Form[0] + "')</SCRIPT>");
        }

        if (Request.QueryString[0] == "faq")
        {
            // log activity
            Utility.LogActivity("student", "FAQ PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

            Response.Write(PageElements.DialogPanelHeader());

            Response.Write("<DIV style='padding-top:40px;padding-bottom:10px'><B>Frequently asked questions (FAQs)</B><P>&nbsp;<BR>");
            Response.Write("<B>Who is the professional counselor available to correspond with me?</B><P>");
            Response.Write("The counselor is a licensed mental health professional at the University of Michigan who is knowledgeable about mental health concerns that students often face. This individual is prepared to help you consider options for services and support that may be helpful.<P>");
            Response.Write("<B>What do I have to gain from corresponding with the counselor?</B><P>");
            Response.Write("Our goal is to help you consider services or other types of support that may be beneficial to your well-being. The counselor can help you think through your current situation and weigh your options.Regardless of whether you decide to try any services, you may find this thought process to be helpful.<P>");
            Response.Write("<B>More about your privacy</B><P>");
            Response.Write("The counselor will never see your name or any other identifying information (such as email address). The emails you receive during your communications with the counselor are sent by an automated process. Similarly, the online chat feature does not reveal any identifying information about you to the counselor.<P>");
            Response.Write("<B>What happens when I post a private message?</B><P>");
            Response.Write("The Counselor will be notified by email that a message has been posted. The Counselor logs in to a secure password protected site to view and respond to your message. You will receive notification by email when the Counselor has responded to your message. You will be able to view the message in your email but will link to the eBridge site to respond to the Counselor.<P>");
            Response.Write("<B>How long will I have to wait for responses from the counselor?</B><P>");
            Response.Write("The counselor will usually respond to your messages within 24 hours.<P>");
            Response.Write("<B>What about scheduling a time to chat with the Counselor?</B><P>");
            Response.Write("The Counselor is available to chat with you at a time that is convenient for you. If you select this option you can schedule a specific time to talk with the Counselor.<P>");
            Response.Write("<B>How does the online-drop in work?</B><P>");
            Response.Write("Counselors are online Monday-Friday 10:00 am – 5:00 pm. You can log in to eBridge during this time and post a message to the Counselor. The Counselor will respond to you within 10 minutes. By selecting this option you will also receive a reminder message with the link and log in information again.<P>&nbsp;<BR>");
            Response.Write("<A href='../survey/receipt.aspx?p=" + Request.QueryString[1] + "' style='font-weight:bold;font-size:17px'>Talk to the Counselor</A></DIV>");
            Response.Write(PageElements.PageFooter(null));
        }
    }
}