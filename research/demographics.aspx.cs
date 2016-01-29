using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class research_demographics : System.Web.UI.Page
{
    private int Cohort = 2011;

    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Participants = Db.GetRecords("SELECT id, gender, year FROM participant WHERE id <> 'G6tE9FJ9gl' ORDER BY id");//id = 'NSRQ9KkIzF' AND 

        string[] DEM = null, FULL_SURVEY_ELIGIBILITY = null, INTERVENTION_ELIGIBILITY = null, INTERVENTION = null;
        string[] FirstFeedbackVisitDate = null, FirstDialogVisitDate = null, FirstStudentPostDate = null, FirstCounselorPostDate = null;

        Response.Write("<TABLE border=1>");
        Response.Write("<TR><TD>id</TD><TD>study_code</TD><TD>gender</TD><TD>school_year</Td><td>age</td><td>race</td><td>ethnicity (s01 cohort only)</td><td>survey_started</td><td>survey_completed</td><td>alc_score</td><td>alc_positive</td><td>phq2_score</td><td>phq2_positive</td><td>phq9_suicide</td><td>suicide_attempt</td><td>full_survey_eligibility</td><td>currently_on_medication</td><td>currently_receiving_counselingortherapy</td><td>intervention_eligibility</td><td>feedback_visits</td><td>intervention</td><td>dialog_visits</td><td>number_of_student_posts</td><td>first_feedback_visit_date</td><td>first_dialog_visit_date</td><td>first_student_post_date</td><td>first_counselor_post_date</td></tr>");

        for (int i = 0; i < Participants.Length; i++)
        {
            string _race = ""; string _ethnicity = ""; string _age = "";

            DEM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + Participants[i][0] + "' AND question_code = 'DEM'");

            if (DEM != null)
            {
                _age = Logic.CheckCode("AGE", DEM[0].Split('|')[0]);
                _race = DEM[0].Split('|')[2];
                
                if (Cohort == 2011)
                {
                    _ethnicity = DEM[0].Split('|')[3];
                    // ethnicity was not collected except the Spring 2011 cohort.
                }
                else
                {
                   _ethnicity = ".";
                }
            }
            else
            {
                _age = "."; _race = "."; _ethnicity = ".";
            }

            int SURVEY_STARTED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY STARTED' AND id = '" + Participants[i][0] + "'");
            if (SURVEY_STARTED > 0) SURVEY_STARTED = 1;

            int SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'SURVEY COMPLETED' AND id = '" + Participants[i][0] + "'");
            if (SURVEY_COMPLETED > 0) SURVEY_COMPLETED = 1;

            // status only have 1 record for each person each data type
            FULL_SURVEY_ELIGIBILITY = Db.GetRecord("SELECT status_value FROM status WHERE status_code = 'FULL SURVEY ELIGIBILITY' AND id = '" + Participants[i][0] + "'");
            string _full_survey_eligibility = "";

            if (FULL_SURVEY_ELIGIBILITY != null)
            {
                _full_survey_eligibility = FULL_SURVEY_ELIGIBILITY[0];
                if (_full_survey_eligibility == "Y") _full_survey_eligibility = "1";
                else _full_survey_eligibility = "0";
            }
            else _full_survey_eligibility = ".";

            INTERVENTION_ELIGIBILITY = Db.GetRecord("SELECT status_value FROM status WHERE status_code = 'INTERVENTION ELIGIBILITY' AND id = '" + Participants[i][0] + "'");

            string _intervention_eligibility = "";

            if (INTERVENTION_ELIGIBILITY != null)
            {
                _intervention_eligibility = INTERVENTION_ELIGIBILITY[0];

                if (_intervention_eligibility == "Y") _intervention_eligibility = "1";
                else _intervention_eligibility = "0";
            }
            else _intervention_eligibility = ".";

            int PersonalizedFeedback = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'PERSONALIZED FEEDBACK VISITED' AND id = '" + Participants[i][0] + "'");
            string _personalized_feedback = "";

            if (PersonalizedFeedback == 0 && _intervention_eligibility != "1") _personalized_feedback = ".";
            else _personalized_feedback = PersonalizedFeedback.ToString();

            INTERVENTION = Db.GetRecord("SELECT status_value FROM status WHERE status_code = 'INTERVENTION' AND id = '" + Participants[i][0] + "'");
            string _intervention = "";

            if (INTERVENTION != null) _intervention = INTERVENTION[0];
            else _intervention = ".";

            int DialogVisit = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity_code = 'STUDENT: MSG THREAD READ' AND id = '" + Participants[i][0] + "'");
            string _dialog_visit = "";

            if (DialogVisit == 0 && _intervention != "1") _dialog_visit = ".";
            else _dialog_visit = DialogVisit.ToString();

            int NumberOfPosts = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + Participants[i][0] + "'");
            string _number_of_posts = "";

            if (NumberOfPosts == 0 && _intervention != "1") _number_of_posts = ".";
            else _number_of_posts = NumberOfPosts.ToString();

            FirstFeedbackVisitDate = Db.GetRecord("SELECT date_time FROM activity_log WHERE unique_id = (SELECT MIN(unique_id) FROM activity_log WHERE activity_code = 'PERSONALIZED FEEDBACK VISITED' AND id = '" + Participants[i][0] + "')");
            string _first_feedback_visit_date = "";

            if (FirstFeedbackVisitDate != null) _first_feedback_visit_date = FirstFeedbackVisitDate[0];
            else _first_feedback_visit_date = ".";

            FirstDialogVisitDate = Db.GetRecord("SELECT date_time FROM activity_log WHERE unique_id = (SELECT MIN(unique_id) FROM activity_log WHERE activity_code = 'STUDENT: MSG THREAD READ' AND id = '" + Participants[i][0] + "')");
            string _first_dialog_visit_date = "";

            if (FirstDialogVisitDate != null) _first_dialog_visit_date = FirstDialogVisitDate[0];
            else _first_dialog_visit_date = ".";

            FirstStudentPostDate = Db.GetRecord("SELECT date_time FROM message WHERE id = (SELECT MIN(id) FROM message WHERE from_id = '" + Participants[i][0] + "')");
            string _first_student_post_date = "";

            if (FirstStudentPostDate != null) _first_student_post_date = FirstStudentPostDate[0];
            else _first_student_post_date = ".";

            if (Cohort == 2011)
            {
                FirstCounselorPostDate = Db.GetRecord("SELECT date_time FROM message WHERE id = (SELECT MIN(id) FROM message WHERE to_id = '" + Participants[i][0] + "' AND message_body <> '_FIRST_VISIT_HIDDEN_')");
                //auto-message was changed for the last cohort
            }
            else
            {
                FirstCounselorPostDate = Db.GetRecord("SELECT date_time FROM message WHERE id = (SELECT MIN(id) FROM message WHERE to_id = '" + Participants[i][0] + "' AND message_body NOT LIKE 'Thanks for taking the time to look at your personalized feedback. I look forward to talking with you online. %')");
            }
            string _first_counselor_post_date = "";

            if (FirstCounselorPostDate != null) _first_counselor_post_date = FirstCounselorPostDate[0];
            else _first_counselor_post_date = ".";

            string ALCScore = ".";
            try { ALCScore = Logic.ALCScore(Participants[i][0]).ToString(); }
            catch { }

            string AlcPositive = ".";
            try { AlcPositive = Logic.AlcPositive(Participants[i][0]).ToString().Replace("True", "1").Replace("False", "0"); }
            catch { }

            string PHQ2Score = ".";
            try { PHQ2Score = Logic.PHQ2Score(Participants[i][0]).ToString(); }
            catch { }

            string PHQ2Positive = ".";
            try { PHQ2Positive = Logic.PHQ2Positive(Participants[i][0]).ToString().Replace("True", "1").Replace("False", "0"); }
            catch { }

            string PHQ9_Suicide = ".";
            try { PHQ9_Suicide = Logic.PHQ9_Suicide(Participants[i][0]).ToString().Replace("True", "1").Replace("False", "0"); }
            catch { }

            string SuicideLifeTimeAttempt = ".";
            try { SuicideLifeTimeAttempt = Logic.SuicideLifeTimeAttempt(Participants[i][0]).ToString().Replace("True", "1").Replace("False", "0"); }
            catch { }

            string CurrentlyTakingPrescribedMedication = ".";
            string CurrentlyReceivingCounselingOrTherapy = ".";

            try
            {
                string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + Participants[i][0] + "' AND question_code ='SER'")[0].Split('|');
                if (_ser[0] == "0") CurrentlyTakingPrescribedMedication = "1";
                else CurrentlyTakingPrescribedMedication = "0";
            }
            catch { }

            try
            {
                string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + Participants[i][0] + "' AND question_code ='SER'")[0].Split('|');
                if (_ser[1] == "0") CurrentlyReceivingCounselingOrTherapy = "1";
                else CurrentlyReceivingCounselingOrTherapy = "0";
            }
            catch { }

            Response.Write("<TR><TD>" + (i + 1).ToString() + "</TD><TD>" + Participants[i][0] + "</TD><TD>" + Participants[i][1] + "</TD><TD>" + Participants[i][2] + "</TD><TD>" + _age + "</TD><TD>" + _race + "</TD><TD>" + _ethnicity + "</TD><TD>" + SURVEY_STARTED + "</TD><TD>" + SURVEY_COMPLETED + "</TD><TD>" + ALCScore + "</TD><TD>" + AlcPositive + "</TD><TD>" + PHQ2Score + "</TD><TD>" + PHQ2Positive + "</TD><TD>" + PHQ9_Suicide + "</TD><TD>" + SuicideLifeTimeAttempt + "</TD><TD>" + _full_survey_eligibility + "</TD><TD>" + CurrentlyTakingPrescribedMedication + "</TD><TD>" + CurrentlyReceivingCounselingOrTherapy + "</TD><TD>" + _intervention_eligibility + "</TD><TD>" + _personalized_feedback + "</TD><TD>" + _intervention + "</TD><TD>" + _dialog_visit + "</TD><TD>" + _number_of_posts + "</TD><TD>" + _first_feedback_visit_date + "</TD><TD>" + _first_dialog_visit_date + "</TD><TD>" + _first_student_post_date + "</TD><TD>" + _first_counselor_post_date + "</TD></TR>");

            Response.Flush();
        }

        Response.Write("</TABLE>");
    }
}