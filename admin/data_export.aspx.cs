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

public partial class admin_data_export : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ADMIN_LOGIN"] == null) { Response.Redirect("login.aspx"); return; }

        Response.Write(PageElements.DialogPanelHeader().Replace("700px", "450px"));

        int TOTAL_PARTICIPANT = Db.GetCount("SELECT COUNT(*) FROM participant WHERE remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");
        int SURVEY_VISITED = Db.GetCount("SELECT COUNT(*) FROM participant WHERE survey_visited IS NOT NULL AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");
        int SURVEY_INITIATED = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT participant_id FROM screening_response WHERE participant_id NOT IN (SELECT id FROM participant WHERE remark = 'test' OR remark = 'XXXX-XXXXXX-XXXX'))"); 
        int SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM participant WHERE survey_completed IS NOT NULL AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");
        int FULL_SURVEY_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM participant WHERE full_survey_eligibility = 'Y' AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");
        int INTERVENTION_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM participant WHERE intervention_eligibility = 'Y' AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");
        int NumberOfStudentsVisitedDialog = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT participant_id FROM activity_log WHERE activity = 'STUDENT: MSG THREAD READ' AND participant_id NOT IN (SELECT id FROM participant WHERE remark = 'test' OR remark = 'XXXX-XXXXXX-XXXX'))");
        int NumberOfStudentsPostedMessage = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT from_id FROM message WHERE from_id NOT IN (SELECT id FROM participant WHERE remark = 'test' OR remark = 'XXXX-XXXXXX-XXXX'))");

        int FOLLOWUP_SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT participant_id FROM status WHERE status_code = 'FOLLOWUP SURVEY COMPLETED' AND participant_id NOT IN (SELECT id FROM participant WHERE remark = 'test' OR remark = 'XXXX-XXXXXX-XXXX'))");
        int FOLLOWUP_SURVEY_STARTED = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT participant_id FROM activity_log WHERE activity = 'FOLLOWUP SURVEY VISITED' AND participant_id NOT IN (SELECT id FROM participant WHERE remark = 'test' OR remark = 'XXXX-XXXXXX-XXXX'))");

        Response.Write("<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");

        Response.Write("<TR height='40px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='5' style='font-weight:bold'>Total # of participants: " + TOTAL_PARTICIPANT.ToString() + "</TD></TR>");
        Response.Write("<TR height='30px'><TD></TD></TR>");
        Response.Write("<TR><TD style='font-weight:bold'>Screening</TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='5'>Visited screening page: " + SURVEY_VISITED.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Start screening survey (some data received): " + SURVEY_INITIATED.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Completed screening survey (either 2-page or full): " + SURVEY_COMPLETED.ToString() + "</TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='5'>Eligible for full survey: " + FULL_SURVEY_ELIGIBILITY.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Eligible for dialogue: " + INTERVENTION_ELIGIBILITY.ToString() + "</TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='5'>Visited dialogue: " + NumberOfStudentsVisitedDialog + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Posted at least one message: " + NumberOfStudentsPostedMessage + "</TD></TR>");
        Response.Write("<TR height='20px'><TD></TD></TR>");
        Response.Write("<TR><TD style='font-weight:bold'>Followup survey</TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='5'>Invited to followup survey (same as 'eligible for dialogue'): " + INTERVENTION_ELIGIBILITY.ToString() + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Visited followup survey page: " + FOLLOWUP_SURVEY_STARTED + "</TD></TR>");
        Response.Write("<TR><TD colspan='5'>Completed followup survey: " + FOLLOWUP_SURVEY_COMPLETED + "</TD></TR>");


        Response.Write("<TR height='40px'><TD></TD></TR>"
            + "<TR><TD colspan='2' style='padding-left:2px'>Last export date: " + File.GetLastWriteTime(AppDomain.CurrentDomain.BaseDirectory + "admin/export/screening.csv").ToString() + "</TD></TR>"
            + "<TR height='20px'><TD></TD></TR>"
            + "<TR><TD><INPUT type='button' disabled value=' Regenerate ' style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px' onclick=\"location.replace('action.aspx?p=export');\"></TD><TD></TD></TR>"
            + "<TR height='30px'><TD></TD></TR>"
            + "<TR><TD style='padding-left:2px;font-weight:bold'>Report type</TD><TD style='padding-left:50px;font-weight:bold'>File</TD>"
            + "<TR height='10px'><TD></TD></TR>"
            + "<TR><TD style='padding-left:2px'>Screening</TD><TD style='padding-left:50px'><A href='./export/screening.csv'>./export/screening.csv</A></TD></TR>"
            + "<TR><TD style='padding-left:2px'>Follow-up</TD><TD style='padding-left:50px'><A href='./export/follow-up.csv'>./export/follow-up.csv</A></TD></TR>"
            + "<TR><TD style='padding-left:2px'>Code book</TD><TD style='padding-left:50px'><A href='./export/eBridge_Code_Book.xls'>./export/eBridge_Code_Book.xls</A></TD></TR>"
            + "</TR><TR height='10px'><TD></TD></TR>");

        Response.Write("<TR height='30px'><TD></TD></TR></TABLE>");

        Response.Write(PageElements.PageFooter(null));
    }
}
