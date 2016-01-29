﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class statistics_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Participants = Db.GetRecords("SELECT p.id FROM participant p, status s WHERE p.id = s.id AND s.status_code = 'INTERVENTION' AND s.status_value = '1'");

        if (Participants == null)
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>No students are eligible for the intervention. Check back later.</SPAN>");
            return;
        }

        Response.Write(PageElements.DialogPanelHeader().Replace("middle", "top"));

        Response.Write("<TABLE width='900px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='40px'><TD></TD></TR>");

        int TOTAL_PARTICIPANT = Db.GetCount("SELECT COUNT(*) FROM participant WHERE id <> 'G6tE9FJ9gl'");
        Response.Write("<TR><TD style='font-weight:bold'>Total # of participants: " + TOTAL_PARTICIPANT.ToString() + "</TD></TR>");

        Response.Write("<TR height='10px'><TD></TD></TR>");

        int SURVEY_STARTED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY STARTED'");
        Response.Write("<TR><TD>Survey started: " + SURVEY_STARTED.ToString("N0") + "</TD></TR>");

        int SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY COMPLETED'");
        Response.Write("<TR><TD>Survey completed: " + SURVEY_COMPLETED.ToString("N0") + "</TD></TR>");

        int FULL_SURVEY_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'FULL SURVEY ELIGIBILITY' AND status_value = 'Y'");
        Response.Write("<TR><TD>Eligible for full survey (i.e., troubled students): " + FULL_SURVEY_ELIGIBILITY.ToString() + "</TD></TR>");

        int INTERVENTION_ELIGIBILITY = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'INTERVENTION ELIGIBILITY' AND status_value = 'Y'");
        Response.Write("<TR><TD>Eligible for intervention: (i.e., not currently on treatment): " + INTERVENTION_ELIGIBILITY.ToString() + "</TD></TR>");

        int INTERVENTION = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'INTERVENTION' AND status_value = '1'");
        Response.Write("<TR><TD>Randomized into intervention group: " + INTERVENTION.ToString() + "</TD></TR>");

        int NumberOfStudentsVisitedDialog = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT id FROM activity_log WHERE activity_code = 'STUDENT: MSG THREAD READ')");
        Response.Write("<TR><TD>Dialog page visited: " + NumberOfStudentsVisitedDialog.ToString() + "</TD></TR>");

        int NumberOfStudentPosted = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT from_id FROM message)");
        int NumberOfStudentPosts = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id IS NOT NULL");
        int NumberOfCounselorPosts = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id IS NULL");
        int NumberOfAutoMessages = Db.GetCount("SELECT COUNT(*) FROM (SELECT DISTINCT to_id FROM message)");
        Response.Write("<TR><TD>Students who posted: " + NumberOfStudentPosted.ToString() + "</TD></TR>");
        Response.Write("<TR><TD>Student posts: " + NumberOfStudentPosts.ToString() + "</TD></TR>");
        Response.Write("<TR><TD>Counselor posts (excluding automated messages): " + (NumberOfCounselorPosts - NumberOfAutoMessages).ToString() + "</TD></TR>");

        int NumberOfFemale = Db.GetCount("SELECT COUNT(*) FROM participant WHERE gender = 'F' AND id <> 'G6tE9FJ9gl'");
        int NumberOfMale = Db.GetCount("SELECT COUNT(*) FROM participant WHERE gender = 'M' AND id <> 'G6tE9FJ9gl'");
        Response.Write("<TR><TD>&nbsp</TD></TR>");
        Response.Write("<TR><TD>Female: " + NumberOfFemale.ToString("N0") + "</TD></TR>");
        Response.Write("<TR><TD>Male: " + NumberOfMale.ToString("N0") + "</TD></TR>");

        string[][] ListOfParticipants = Db.GetRecords("SELECT id, gender FROM participant WHERE id <> 'G6tE9FJ9gl'");

        Response.Write("<TR><TD>&nbsp</TD></TR>");

        Response.Write("<TR><TD>");

        for (int i = 0; i < ListOfParticipants.Length; i++)
        {
            string[] DEM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ListOfParticipants[i][0] + "' AND question_code = 'DEM'");

            string[] INT = Db.GetRecord("SELECT status_value FROM status WHERE status_code = 'INTERVENTION' AND id = '" + ListOfParticipants[i][0] + "'");

            Response.Write((i + 1).ToString() + "," + ListOfParticipants[i][0] + "|" + ListOfParticipants[i][1] + "|" + (DEM != null ? DEM[0].Substring(4) : "") + "|" + (INT != null ? INT[0] : "") + "<BR>");
            Response.Flush();
        }

        Response.Write("</TD></TR>");
        Response.Write("</TABLE>");

        Response.Write("</BODY></HTML>");
    }
}