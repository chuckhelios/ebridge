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

public partial class admin_action : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString[0] == "send_email")
        {
            string EmailToAddress = Request.Form[0];
            string EmailSubject = Request.Form[1];
            string EmailBody = Request.Form[2];

            //Utility.SendPHPMail(EmailToAddress, "ebridgeteam@umich.edu", "The eBridge Team", EmailSubject, EmailBody);
            //Utility.SendPHPMail("kzheng@umich.edu", "ebridgeteam@umich.edu", "The eBridge Team", EmailSubject, EmailBody);

            Response.Write("<SCRIPT type='text/javascript'>alert('Email sent.');history.go(-1);</SCRIPT>");
        }

        if (Request.QueryString[0] == "login")
        {
            string Password = Request.Form[0];

            if (Password == "1")
            {
                Session["ADMIN_LOGIN"] = "Y";

                Response.Redirect("data_export.aspx");
            }
            else
            {
                Response.Write("<SCRIPT type='text/javascript'>alert('The password you entered is incorrect. Please try again.');history.go(-1);</SCRIPT>");
            }
        }

        if (Request.QueryString[0] == "export")
        {
            string _output = "";
            string[] _response = null;
            string[] _participant = null;

            Response.Write("<HTML><BODY style='font-family:arial;font-size:12px;line-height:150%'>");
            Response.Write("<IMG id='L' src='loading.gif'><P>"); for (int i = 0; i < 1000; i++) Response.Write(" ");
            Response.Write("<B style='color:red'>Exporting, please wait.</B><P>&nbsp;<BR>");
            Response.Flush();

            // survey: screening
            Response.Write("1. Screening responses");

            string[][] _pid = Db.GetRecords("SELECT id FROM participant WHERE survey_visited IS NOT NULL AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");

            string[] QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\q.txt").Split(',');

            _output = "\"PID,SEX_REGISTRAR,SURVEY_STARTED_DATE_TIME,DURATION,SURVEY_COMPLETED,FULL_SURVEY_ELIGIBLE,INTERVENTION_ELIGIBLE,DIALOG_VISITED,MSG_POSTED,DEM_1,DEM_2,DEM_3,AUDIT_1,AUDIT_2,AUDIT_3,PHQ9_1,PHQ9_2,SUI,PHQ9_3,PHQ9_4,PHQ9_5,PHQ9_6,PHQ9_7,PHQ9_8,PHQ9_9,HM_1,HM_2,AUDIT_4,AUDIT_5,AUDIT_6,AUDIT_7,AUDIT_8,AUDIT_9,AUDIT_10,ILL,SER_1,SER_2,GOA,VAL\"\r\n".Replace(",", "|");

            for (int i = 0; i < _pid.Length; i++)
            {
                _participant = Db.GetRecord("SELECT gender, survey_visited, survey_completed, full_survey_eligibility, intervention_eligibility FROM participant WHERE id = '" + _pid[i][0] + "'");

                string Gender = _participant[0];
                string SurveyInitiated = _participant[1].Replace("NULL", "--");
                string SurveyCompleted = _participant[2].Replace("NULL", "--");
                string SurveySpan = ""; if (SurveyInitiated != "--" && SurveyCompleted != "--") SurveySpan = DateTime.Parse(SurveyCompleted).Subtract(DateTime.Parse(SurveyInitiated)).ToString();
                if (SurveyCompleted == "--") SurveyCompleted = "0"; else SurveyCompleted = "1";

                string FullSurveyEligibility = _participant[3].Replace("NULL", "").Replace("Y", "1").Replace("N", "0");
                string InterventionEligibility = _participant[4].Replace("NULL", "").Replace("Y", "1").Replace("N", "0");
                if (FullSurveyEligibility == "0") InterventionEligibility = "0";

                int VisitedDialog = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity = 'STUDENT: MSG THREAD READ' AND participant_id = '" + _pid[i][0] + "'");
                if (VisitedDialog > 0) VisitedDialog = 1;
                int NumberOfStudentsPostedMessage = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + _pid[i][0] + "'");

                _output += "\"" + _pid[i][0] + "|" + Gender + "|" + SurveyInitiated + "|" + SurveySpan + "|" + SurveyCompleted + "|" + FullSurveyEligibility + "|" + InterventionEligibility + "|" + (InterventionEligibility=="1"?VisitedDialog.ToString():"") + "|" + (InterventionEligibility=="1"?NumberOfStudentsPostedMessage.ToString():"") + "|";

                for (int j = 0; j < QuestionCodes.Length; j++)
                {
                    _response = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + _pid[i][0] + "' AND question_code = '" + QuestionCodes[j] + "'");

                    if (_response != null)
                    {
                        if (_response[0] == ",None") _response[0] = "None";
                        if (_response[0] == ",Not applicable") _response[0] = "Not applicable";

                        _output += _response[0].Replace("\"", "\"\"");
                    }

                    _output += "|";
                }

                _output = _output.Substring(0, _output.Length - 1) + "\"" + (i == _pid.Length - 1 ? "" : "\r\n");
            }

            _output = _output.Replace("|", "\",\"").Replace("--", ".");

            //Response.Write("<P>" + _output + "<P>");

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "admin/export/screening.csv", _output);

            Response.Write(" .......... done, " + _pid.Length.ToString() + " records exported.");
            Response.Flush();


            // survey: follow-up
            Response.Write("<P>2. Follow-up survey responses");

            _pid = Db.GetRecords("SELECT id FROM participant WHERE intervention_eligibility = 'Y' AND remark IS NULL OR (remark <> 'test' AND remark <> 'XXXX-XXXXXX-XXXX')");

            QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey2\\q.txt").Split(',');

            _output = "\"PID,SEX_REGISTRAR,FOLLOWUP_SURVEY_STARTED_DATE_TIME,FOLLOWUP_SURVEY_COMPLETED,DIALOG_VISITED,MSG_POSTED,DEM,SER_1,SER_2,SER_2A,SER_3,SER_3A,SER_4,AUDIT_1,AUDIT_2,AUDIT_3,PHQ9_1,PHQ9_2,HM_1,HM_2,KB_1,KB_2,KB_3,KB_4,KB_5,KB_6,KB_7,SER_OPEN,EXP_OPEN\"\r\n".Replace(",", "|");

            string[] _temp = null;

            for (int i = 0; i < _pid.Length; i++)
            {
                _participant = Db.GetRecord("SELECT gender FROM participant WHERE id = '" + _pid[i][0] + "'");

                string Gender = _participant[0];
                int FOLLOWUP_SURVEY_COMPLETED = Db.GetCount("SELECT COUNT(*) FROM status WHERE status_code = 'FOLLOWUP SURVEY COMPLETED' AND participant_id = '" + _pid[i][0] + "'");

                string FOLLOWUP_SURVEY_STARTED_DATE_TIME;
                _temp = Db.GetRecord("SELECT date_time FROM activity_log WHERE activity = 'FOLLOWUP SURVEY VISITED' AND participant_id = '" + _pid[i][0] + "' ORDER BY id DESC");
                if (_temp != null) FOLLOWUP_SURVEY_STARTED_DATE_TIME = _temp[0]; else FOLLOWUP_SURVEY_STARTED_DATE_TIME = "";

                int VisitedDialog = Db.GetCount("SELECT COUNT(*) FROM activity_log WHERE activity = 'STUDENT: MSG THREAD READ' AND participant_id = '" + _pid[i][0] + "'");
                if (VisitedDialog > 0) VisitedDialog = 1;

                int NumberOfStudentsPostedMessage = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + _pid[i][0] + "'");


                _output += "\"" + _pid[i][0] + "|" + Gender + "|" + FOLLOWUP_SURVEY_STARTED_DATE_TIME + "|" + FOLLOWUP_SURVEY_COMPLETED + "|" + VisitedDialog + "|" + NumberOfStudentsPostedMessage + "|";

                for (int j = 0; j < QuestionCodes.Length; j++)
                {
                    _response = Db.GetRecord("SELECT response FROM followup_response WHERE participant_id = '" + _pid[i][0] + "' AND question_code = '" + QuestionCodes[j] + "'");

                    if (_response != null)
                    {
                        _output += _response[0].Replace("\"", "\"\"").Replace("Other ,", "Other,");
                    }

                    _output += "|";
                }

                _output = _output.Substring(0, _output.Length - 1) + "\"" + (i == _pid.Length - 1 ? "" : "\r\n");
            }

            _output = _output.Replace("|", "\",\"").Replace("--", "");

            //Response.Write("<P>" + _output + "<P>");

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "admin/export/follow-up.csv", _output);

            Response.Write(" .......... done, " + _pid.Length.ToString() + " records exported.");
            Response.Write("<P>Done.");
            Response.Write("<SCRIPT>history.go(-1);</SCRIPT>");
            Response.Flush();


            Response.Write("</BODY></HTML>");
            //Response.Write("<SCRIPT>document.getElementById('L').style.display='none'</SCRIPT>");
        }
    }
}
