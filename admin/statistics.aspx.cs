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

public partial class admin_statistics : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Participants = Db.GetRecords("SELECT p.id FROM participant p, status s WHERE p.id = s.id AND s.status_code = 'INTERVENTION' AND s.status_value = '1'");

        if (Participants == null)
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>No students are eligible for the intervention. Check back later.</SPAN>");
            return;
        }

        try
        {
            Response.Write(PageElements.DialogPanelHeader().Replace("middle", "top"));

            Response.Write("<TABLE width='900px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
            Response.Write("<TR height='40px'><TD></TD></TR>");

            int TOTAL_PARTICIPANT = Db.GetCount("SELECT COUNT(*) FROM participant WHERE id <> 'G6tE9FJ9gl'");
            Response.Write("<TR><TD colspan='10' style='font-weight:bold'>Total # of participants: " + TOTAL_PARTICIPANT.ToString() + "</TD></TR>");

            Response.Write("<TR height='10px'><TD></TD></TR>");

            int SURVEY_STARTED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY STARTED'");
            Response.Write("<TR><TD colspan='10'>Survey started: " + SURVEY_STARTED.ToString() + "</TD></TR>");

            int SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY COMPLETED'");
            Response.Write("<TR><TD colspan='10'>Survey completed: " + SURVEY_COMPLETED.ToString() + "</TD></TR>");

            int FULL_SURVEY_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'FULL SURVEY ELIGIBILITY' AND status_value = 'Y'");
            Response.Write("<TR><TD colspan='10'>Eligible for full survey (i.e., troubled students): " + FULL_SURVEY_ELIGIBILITY.ToString() + "</TD></TR>");

            int INTERVENTION_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'INTERVENTION ELIGIBILITY' AND status_value = 'Y'");
            Response.Write("<TR><TD colspan='10'>Eligible for intervention: (i.e., not currently on treatment): " + INTERVENTION_ELIGIBILITY.ToString() + "</TD></TR>");

            int INTERVENTION = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'INTERVENTION' AND status_value = '1'");
            Response.Write("<TR><TD colspan='10'>Randomized into intervention group: " + INTERVENTION.ToString() + "</TD></TR>");

            int NumberOfStudentsVisitedDialog = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT id FROM activity_log WHERE activity_code = 'STUDENT: MSG THREAD READ')");
            Response.Write("<TR><TD colspan='10'>Dialog page visited: " + NumberOfStudentsVisitedDialog.ToString() + "</TD></TR>");

            int NumberOfStudentPosted = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT from_id FROM message)");
            int NumberOfStudentPosts = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id IS NOT NULL");
            int NumberOfCounselorPosts = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id IS NULL");
            int NumberOfAutoMessages = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT to_id FROM message)");
            Response.Write("<TR><TD colspan='10'>Students who posted: " + NumberOfStudentPosted.ToString() + "</TD></TR>");
            Response.Write("<TR><TD colspan='10'>Student posts: " + NumberOfStudentPosts.ToString() + "</TD></TR>");
            Response.Write("<TR><TD colspan='10'>Counselor posts (excluding automated messages): " + (NumberOfCounselorPosts - NumberOfAutoMessages).ToString() + "</TD></TR>");

            Response.Write("<TR height='10px'><TD></TD></TR>");

            int FOLLOW_UP_SURVEY_COMPLETED = Db.GetRecords("SELECT DISTINCT id FROM activity_log WHERE activity_code = 'FOLLOWUP SURVEY COMPLETED'").Length;
            Response.Write("<TR><TD colspan='10'>Followup Survey completed: " + FOLLOW_UP_SURVEY_COMPLETED.ToString() + " (out of " + INTERVENTION_ELIGIBILITY + ")</TD></TR>");

            int FOLLOW_UP_SURVEY_COMPLETED_INTERVENTION = Db.GetRecords("SELECT DISTINCT id FROM activity_log WHERE activity_code = 'FOLLOWUP SURVEY COMPLETED' AND id IN (SELECT id FROM status WHERE status_code = 'INTERVENTION' AND status_value = '1')").Length;

            Response.Write("<TR><TD colspan='10'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Control Group: " + FOLLOW_UP_SURVEY_COMPLETED_INTERVENTION.ToString() + " (out of " + INTERVENTION.ToString() + ")</TD></TR>");
            Response.Write("<TR><TD colspan='10'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Intervention Group: " + (FOLLOW_UP_SURVEY_COMPLETED - FOLLOW_UP_SURVEY_COMPLETED_INTERVENTION).ToString() + " (out of " + (INTERVENTION_ELIGIBILITY - INTERVENTION).ToString() + ")</TD></TR>");

            string[][] FOLLOW_UP_SURVEY_COMPLETED_ID = Db.GetRecords("SELECT id, date_time FROM activity_log WHERE activity_code = 'FOLLOWUP SURVEY COMPLETED' ORDER BY date_time");

            Response.Write("<TR height='10px'><TD></TD></TR>");
            Response.Write("<TR height='10px'><TD>IDs</TD></TR>");
            int _count = 0;
            for (int i = 0; i < FOLLOW_UP_SURVEY_COMPLETED_ID.Length; i++)
            {
                bool _rep = false;
                for (int j = 0; j < i; j++)
                {
                    if (FOLLOW_UP_SURVEY_COMPLETED_ID[i][0] == FOLLOW_UP_SURVEY_COMPLETED_ID[j][0])
                    {
                        _rep = true; break;
                    }
                }

                if (!_rep)
                {
                    _count++;
                    Response.Write("<TR><TD colspan='10'>" + _count.ToString() + ". " + FOLLOW_UP_SURVEY_COMPLETED_ID[i][0] + ", Completion Date: " + Db.GetRecord("SELECT MAX(date_time) FROM activity_log WHERE activity_code = 'FOLLOWUP SURVEY COMPLETED' AND id = '" + FOLLOW_UP_SURVEY_COMPLETED_ID[i][0] + "'")[0].Split(' ')[0] + "</TD></TR>");
                }

            }


            Response.Write("</TABLE></BODY></HTML>");
        }
        catch
        {
            Response.Write("<TR height='10px'><TD></TD><TR><TR><TD><SPAN style='font-family:verdana;font-size:10px;color:red'>Follow-up survey results are not yet available.</SPAN></TD></TR>");
        }
    }
}
