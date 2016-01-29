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

public partial class dialog_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Buffer = false;
        string QueryString;

        // log activity
        Utility.LogActivity("counselor", "PARTICIPANT LIST PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        string CounselorSite= Session["SCHOOL"] as string;
        string CounselorId = Session["COUNSELOR_ID"] as string;

        if (string.IsNullOrEmpty(CounselorSite) || string.IsNullOrEmpty(CounselorId))
        {
        Session.Abandon(); Response.Redirect("login_counselor.aspx?p=list"); return; 
        }

        QueryString = @"SELECT p.id FROM participant p, status s WHERE p.id = s.id AND s.status_code = 'INTERVENTION'
        AND s.status_value = '1' and right(p.id,1)='{0}'";

        string[][] Participants = Db.GetRecords(string.Format(QueryString, CounselorSite));

        if (Participants == null)
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>No students are eligible for the intervention. Check back later.</SPAN>");
            return;
        }
        /*
	    [0]	"total participants"	string
		[1]	"survey started"	string
		[2]	"survey completed"	string
		[3]	"visited dialog"	string
		[4]	"followup started"	string
		[5]	"followup completed"	string
		[6]	"full survey eligible"	string
		[7]	"intervention eligible"	string
		[8]	"intervention group"	string
		[9]	"control group"	string
		[10]	"student messages"	string
		[11]	"counselor messages"	string
        */

        Dictionary<string, string> QueryStrings = Helper.ListQueryStrings(CounselorSite);

        string _style= 
        " style='padding-left:5px;padding-right:5px;padding-top:3px;padding-bottom:3px;"
        +"cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\""
        +" onmouseout=\"this.style.backgroundColor='white'\" align='right'";

        Response.Write(PageElements.DialogPanelHeader().Replace("middle", "top"));

        Response.Write("<TABLE width='900px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR><TD colspan=7></TD><TD colspan=3" + _style + "><A href='action.aspx?p=counselor_logout' style='color:blue'>Log Out</A></TD></TR>");
        Response.Write("<TR><TD colspan=7></TD><TD colspan=3" + _style + "><A href='change_hours.aspx' style='color:blue'>Change Hours</A></TD></TR>");
        Response.Write("<TR height='40px'><TD></TD></TR>");

        int TOTAL_PARTICIPANT = Db.GetCount(QueryStrings["total participants"]);
        Response.Write("<TR><TD colspan='10' style='font-weight:bold'>Total # of participants: " + TOTAL_PARTICIPANT.ToString("N0") + "</TD></TR>");

        Response.Write("<TR height='10px'><TD></TD></TR>");

        int SURVEY_STARTED = Db.GetCount(QueryStrings["survey started"]);
        Response.Write("<TR><TD colspan='10'>Survey started: " + SURVEY_STARTED.ToString("N0") + "</TD></TR>");

        int SURVEY_COMPLETED = Db.GetCount(QueryStrings["survey completed"]);
        Response.Write("<TR><TD colspan='10'>Survey completed: " + SURVEY_COMPLETED.ToString("N0") + "</TD></TR>");
        // (i.e., troubled students)
        int FULL_SURVEY_ELIGIBILITY = Db.GetCount(QueryStrings["full survey eligible"]);
        Response.Write("<TR><TD colspan='10'>Eligible for full survey: " + FULL_SURVEY_ELIGIBILITY.ToString() + "</TD></TR>");
        // (i.e., completed full survey)
        int INTERVENTION_ELIGIBILITY = Db.GetCount(QueryStrings["intervention eligible"]);
        Response.Write("<TR><TD colspan='10'>Eligible for intervention: " + INTERVENTION_ELIGIBILITY.ToString() + "</TD></TR>");

        int INTERVENTION = Db.GetCount(QueryStrings["intervention group"]);
        Response.Write("<TR><TD colspan='10'>Randomized into intervention group: " + INTERVENTION.ToString() + "</TD></TR>");

        int CONTROL = Db.GetCount(QueryStrings["control group"]);
        Response.Write("<TR><TD colspan='10'>Randomized into control group: " + CONTROL.ToString() + "</TD></TR>");

        int NumberOfStudentsVisitedDialog = Db.GetCount(QueryStrings["visited dialog"]);
        Response.Write("<TR><TD colspan='10'>Dialog page visited: " + NumberOfStudentsVisitedDialog.ToString() + "</TD></TR>");

        int NumberOfStudentPosted = Db.GetCount(QueryStrings["students posted"]);

        //int NumberOfStudentPostedSubstantiveContent = Db.GetCount("SELECT count(DISTINCT from_id) FROM message WHERE from_id IS NOT NULL"
        //+ "AND message_body = 'I am interested in connecting with the Counselor later but not right now.' "
        //+ "AND message_body = 'I would like to talk to you about ...' "
        //+ "AND message_body = 'I would like to schedule a private session to chat with the Counselor online. Below is my availability:'");

        int NumberOfStudentPosts = Db.GetCount(QueryStrings["student messages"]);
        int NumberOfCounselorPosts = Db.GetCount(QueryStrings["counselor messages"]);
        //int NumberOfAutoMessages = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT to_id FROM message WHERE to_id IS NOT NULL)");
        Response.Write("<TR><TD colspan='10'>Student(s) who posted: " + NumberOfStudentPosted.ToString() + "</TD></TR>");
        //Response.Write("<TR><TD colspan='10'>Student(s) who posted substantive content: " + NumberOfStudentPostedSubstantiveContent.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='10'>Student posts: " + NumberOfStudentPosts.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='10'>Counselor posts: " + NumberOfCounselorPosts.ToString() + "</TD></TR>");
        //Response.Write("<TR><TD colspan='10'>Chat appointment scheduled: " + Db.GetCount("SELECT COUNT(*) FROM schedule") + "</TD></TR>");

        int FOLLOWUP_SURVEY_STARTED = Db.GetCount(QueryStrings["followup started"]);
        int FOLLOWUP_SURVEY_COMPLETED = Db.GetCount(QueryStrings["followup completed"]);
    
        Response.Write("<TR height='20px'><TD></TD></TR>");

        Response.Write("<TR><TD colspan='10'>Followup survey started: " + FOLLOWUP_SURVEY_STARTED.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='10'>Followup survey completed: " + FOLLOWUP_SURVEY_COMPLETED.ToString() + "</TD></TR>");

        Response.Write("<TR height='50px'><TD></TD></TR>"
            + "<TR><TD valign='top' style='font-weight:bold'>ID</TD>"
            //+ "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Eligible for Dialog</TD>"
            + "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Assigned to</TD>"
            //+ "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Online?</TD>" 
            + "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Couselor Attention Needed</TD>"
            + "<TD width='100px' align='center' valign='top' style='padding-left:10px;font-weight:bold'>Last Student Post</TD>"
            + "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'># of Student Posts</TD>"
            + "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'># of Counselor Posts</TD>"
            //+ "<TD width='100px' align='center' valign='top' style='padding-left:10px;font-weight:bold'>Survey Initiated</TD>"
            //+ "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Eligible for Full Survey</TD>"
            + "<TD align='center' valign='top' style='padding-left:10px;font-weight:bold'>Survey Completed On</TD>"
             + "<TD align='left' valign='top' style='padding-left:10px;font-weight:bold'>Last Counselor Post</TD>"
            //+ "<TD align='right' valign='top' style='padding-left:10px;font-weight:bold'>Duration</TD>"
            + "</TR><TR height='10px'><TD></TD></TR>");

        for (int i = 0; i < Participants.Length; i++)
        {
            string ParticipantId = Participants[i][0];
            string AssignedTo = Utility.GetStatus(ParticipantId, "COUNSELOR");
            //string Online = "";
            //if (Db.GetCount("SELECT COUNT(*) FROM online_status WHERE id = '" + ParticipantId + "'") == 0) Online = "no";
            //else Online = "<SPAN style='color:red'>yes</SPAN>";
            QueryString = "SELECT username FROM counselor WHERE id = '{0}' and site='{1}'";
            AssignedTo = "<B>" + Db.GetRecord(String.Format(QueryString, new string[2] { AssignedTo, CounselorSite }))[0] + "</B>";
            //string SurveyStarted = Utility.GetLog(ParticipantId, "SURVEY STARTED").Replace("NULL", "--");
            string SurveyCompleted = Utility.GetLog(ParticipantId, "SURVEY COMPLETED");
            //string SurveySpan = ""; if (SurveyStarted != "--" && SurveyCompleted != "--") SurveySpan = DateTime.Parse(SurveyCompleted).Subtract(DateTime.Parse(SurveyStarted)).ToString();
            //if (SurveyCompleted == "--") SurveyCompleted = "no"; else SurveyCompleted = "yes"; if (SurveyStarted == "--") SurveyCompleted = "--";

            //string FullSurveyEligibility = Utility.GetStatus(ParticipantId, "FULL SURVEY ELIGIBILITY").Replace("NULL", "--").Replace("Y", "yes").Replace("N", "no");
            //string InterventionEligibility = Utility.GetStatus(ParticipantId, "INTERVENTION ELIGIBILITY").Replace("NULL", "--").Replace("Y", "yes").Replace("N", "no");

            //if (InterventionEligibility == "yes") InterventionEligibility = "<B>" + InterventionEligibility + "</B>";

            string LastPost = ""; string[] _lvt = Db.GetRecord("SELECT date_time FROM message WHERE id = (SELECT MAX(id) FROM message WHERE from_id = '" + ParticipantId + "')");
            if (_lvt == null) LastPost = "--"; else LastPost = _lvt[0];

            string LastPostContent = ""; string[] _lpc = Db.GetRecord("SELECT message_body FROM message WHERE id = (SELECT MAX(id) FROM message WHERE from_id = '" + ParticipantId + "')");
            if (_lpc == null) LastPostContent = "--"; else LastPostContent = _lpc[0];

            if (LastPostContent.Length > 250) LastPostContent = LastPostContent.Substring(0, 250) + " ...";



            string LastCounselorPost = ""; string[] _lct = Db.GetRecord("SELECT date_time FROM message WHERE id = (SELECT MAX(id) FROM message WHERE to_id = '" + ParticipantId + "')");
            if (_lct == null) LastCounselorPost = "--"; else LastCounselorPost = _lct[0];

            string LastCounselorPostContent = ""; string[] _lcpc = Db.GetRecord("SELECT message_body FROM message WHERE id = (SELECT MAX(id) FROM message WHERE to_id = '" + ParticipantId + "')");
            if (_lcpc == null) LastCounselorPostContent = "--"; else LastCounselorPostContent = _lcpc[0];

            if (LastCounselorPostContent.Length > 150) LastCounselorPostContent = LastCounselorPostContent.Substring(0, 150) + " ...";

            if (LastCounselorPostContent == "_FIRST_VISIT_HIDDEN_") LastCounselorPostContent = "<SPAN style='color:red'>student awaiting for greeting</SPAN>";

            string CounselorAttentionNeeded = ""; string[] _mpt = Db.GetRecord("SELECT from_id, to_id FROM message WHERE id = (SELECT MAX(id) FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "')");
            if (_mpt == null || _mpt[0] != "NULL") CounselorAttentionNeeded = "<A href='frame.aspx?p=counselor&p1=" + ParticipantId + "' target='_blank' style='font-weight:bold;color:red'>yes</A>";
            else CounselorAttentionNeeded = "no";

            //if (Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "'") == 1)
            //{
            //    CounselorAttentionNeeded = "<A href='frame.aspx?p=counselor&p1=" + ParticipantId + "' target='_blank' style='font-weight:bold;color:red'>yes</A>";
            //}

            Response.Write("<TR height='28px'" + (i % 2 == 1 ? "" : " style='background-color:#D3D3D3'") + ">");

            Response.Write("<TD valign='top' style='padding-top:5px;padding-bottom:5px'>" + "<A href='frame.aspx?p=counselor&p1=" + ParticipantId +  "' target='_blank' style='color:blue'>" + ParticipantId + "</A>" + "</TD>");//"<SUP style='color:green;font-weight:bold'> " + Participants[i][1].Replace("NULL", "").Replace("test", "test") + "</SUP>

            //Response.Write("<TD align='center' style='padding-left:10px'>" + InterventionEligibility + "</TD>");
            Response.Write("<TD align='center' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + AssignedTo + "</TD>");
            //Response.Write("<TD align='center' style='padding-left:10px'>" + Online + "</TD>");

            Response.Write("<TD align='center' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + CounselorAttentionNeeded + "</TD>");
            Response.Write("<TD align='left' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px' width='200px'>" + FormateDateString(LastPost) + (LastPostContent == "--" ? "" : "<P><I>" + LastPostContent) + "</I></TD>");
            Response.Write("<TD align='center' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + ParticipantId + "'").ToString() + "</TD>");

            int NC = Db.GetCount("SELECT COUNT(*) FROM message WHERE to_id = '" + ParticipantId + "'");
            if (NC == -1) NC = 0;

            Response.Write("<TD align='center' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + NC.ToString() + "</TD>");

            //Response.Write("<TD align='center' style='padding-left:10px'>" + SurveyStarted + "</TD>");
            //Response.Write("<TD align='center' style='padding-left:10px'>" + FullSurveyEligibility + "</TD>");
            Response.Write("<TD align='center' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + FormateDateString(SurveyCompleted) + "</TD>");
            Response.Write("<TD align='left' valign='top' style='padding-left:10px;padding-top:5px;padding-bottom:5px'>" + FormateDateString(LastCounselorPost) + (LastCounselorPostContent == "--" ? "" : "<P><I>" + LastCounselorPostContent) + "</I></TD>");
            //Response.Write("<TD align='right' style='padding-left:10px'>" + (SurveySpan.Replace("-", "") == "" ? "--" : SurveySpan.Replace("-", "")) + "</TD>");
            Response.Write("</TR>");

            Response.Flush();
        }

        Response.Write(PageElements.PageFooter(null));
    }


    private string FormateDateString(string InputDate)
    {
        if (InputDate == "--") return "--";
        else if(InputDate == "NULL") return "NULL"; // added to make the list page work
        else return (InputDate.Split('/')[0].Length == 1 ? "0" + InputDate.Split('/')[0] : InputDate.Split('/')[0]) + "/" +
InputDate.Split('/')[1] + "/" + InputDate.Split('/')[2].Substring(2, 2) + " "
            + (InputDate.Split(' ')[1].Split(':')[0].Length == 1 ? "0" + InputDate.Split(' ')[1].Split(':')[0] :
InputDate.Split(' ')[1].Split(':')[0]) + ":" + InputDate.Split(' ')[1].Split(':')[1] + " " + InputDate.Split(' ')[2].ToLower();
    }
}