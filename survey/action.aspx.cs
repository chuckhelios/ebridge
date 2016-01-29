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

public partial class survey_action : System.Web.UI.Page
{

    string BASE_URL = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt");
    Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {

        bool DEBUG = (bool)Application["DEBUG"];

        if (Request.QueryString[0] == "initial_post")
        {
            string ParticipantId = Session["PARTICIPANT_ID"] as string;

            if (string.IsNullOrEmpty(ParticipantId) || Session["SCHOOL"]== null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the invitation email to access the site.</SPAN>"); return; }
            
            string DialogBody = Request.Form["MSG"];
            SchoolInfo= Helper.FindSchool(ParticipantId);
            string BASE_URL;
            if ((bool)Application["DEBUG"]) BASE_URL = SchoolInfo["test_server"];
            else BASE_URL = SchoolInfo["email_server"];

            if (DialogBody == "0")
            {
                DialogBody = "I am interested in talking more about my concerns or my survey feedback.";
            }

            if (DialogBody == "1")
            {
                DialogBody = "I am interested in talking more about available resources.";
            }

            if (DialogBody == "2")
            {
                DialogBody = "I am interested in talking more about other things.";
            }

            Db.Execute("INSERT INTO message (from_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");

            DialogBody = "Participant " + ParticipantId + " posted the following message at eBridge (while Counselor is not available to chat):\r\n\n " + DialogBody.Replace("\r", "") + "\r\n\n To reply, click " + BASE_URL + "/dialog/thread_counselor.aspx?p=" + ParticipantId;

            string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor where site= '" + SchoolInfo["code"] + "'");

            for (int i = 0; i < CounselorEmail.Length; i++)
            {
                if (CounselorEmail[i][0].Substring(0, 1) != "#")
                {
                    string CounselorName = Db.GetRecord("SELECT name_first, name_last FROM counselor WHERE id = '" + Session["DESIGNATED_COUNSELOR"].ToString() + "'")[0];
                    Console.Write("this is sending the email");
                    string email_subject = "eBridge: Student (assigned to " + CounselorName + ") posted a message at " + DateTime.Now.ToString();
                    string email_body = DialogBody;

                    Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
                    //if ((bool)Application["DEBUG"]) Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
                    //else Utility.SendGMail(CounselorEmail[i][0], "counselor", SchoolInfo["email"], "The eBridge Team", email_subject, email_body);
					
                }
            }

            Session["IniMessage"] = "ADDITIONAL_POST";
            Response.Write("<SCRIPT type='text/javascript'>location.href='../dialog/frame.aspx?p=participant&p1=" + ParticipantId + "';</SCRIPT>");
        }

        if (Request.QueryString[0] == "data")
        {
            string ParticipantId = Session["PARTICIPANT_ID"] as string;
            SchoolInfo= Helper.FindSchool(ParticipantId);
            if (string.IsNullOrEmpty(ParticipantId)) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the invitation email to access the site.</SPAN>"); return; }

            // Request.Form is the POST data from form, and at index 0 it has the page index and the page name; 
            int Index = int.Parse(Request.Form[0].Split(',')[0]);
            string QuestionCode = Request.Form[0].Split(',')[1];

            // from index 1 forwards SurveyResponse contains the responses by section
            string SurveyResponses = ""; string _temp = "";

            // this is the case for pages with soft prompt and no response; see DRG5 for example - Request.Form would only have 1 entry
            for (int i = 1; i < Request.Form.Count; i++) // skip 0, index info
            { // this method only to strip off trailing commas
                _temp = Request.Form[i];

                if (_temp.Length != 0)
                {
                    _temp = (_temp.Substring(_temp.Length - 1) == "," ? _temp.Substring(0, _temp.Length - 1) : _temp); // remove trailing ,
                }
                else
                {
                    _temp = ".";
                }

                SurveyResponses += _temp + "|";
            }

            SurveyResponses = SurveyResponses.Substring(0, SurveyResponses.Length - 1); // remove trailing pipe


            // the responses for categories with more than 1 response are placed entirely:
            // for example, DEM3-> African American/Black, non-Hispanic,American Indian/Alaskan Native,Asian/Asian-American,Asian Indian,Chinese,Filipino,Japanese,
            // so this would be fine
            Db.Execute("DELETE FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + QuestionCode + "'");
            Db.Execute("INSERT INTO screening_response VALUES ('" + ParticipantId + "','" + QuestionCode + "','" + SurveyResponses.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");

            Index++;

            if (QuestionCode == "HMSB")
            {
                // the last step of the survey
                if (Logic.InterventionEligibility(ParticipantId))
                {
                    SchoolInfo = Helper.FindSchool(ParticipantId);
                    Utility.UpdateStatus(ParticipantId, "INTERVENTION ELIGIBILITY", "Y");                    
                    // set the seed randomization number
                    int RanGrp = Helper.RandomizeGroup1(SchoolInfo["code"], ParticipantId);          
                    Response.Redirect("screen.aspx?q=" + Index.ToString());
                }
                else
                {
                    // not eligible for survey, and is directed to the end page, negative exit.
                    Utility.UpdateStatus(ParticipantId, "INTERVENTION ELIGIBILITY", "N");
                    Utility.LogActivity(ParticipantId, "SURVEY COMPLETED", "not eligible for full survey");

                    Response.Write(PageElements.PageHeader("Logo Only"));
                    if (DEBUG) Response.Write(PageElements.EligibilityReasoning(ParticipantId, "Intervention"));
                    Response.Write(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\question\\NEG_EXIT.htm"));
                    Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
                }
            }
            else if (QuestionCode == "PHQ2_SUI")
            {
                if (Logic.FullSurveyEligibility(ParticipantId))
                {
                    SchoolInfo = Helper.FindSchool(ParticipantId);
                    Utility.UpdateStatus(ParticipantId, "FULL SURVEY ELIGIBILITY", "Y");
                    Response.Redirect("screen.aspx?q=" + Index.ToString());
                }
                else
                {
                    Utility.UpdateStatus(ParticipantId, "FULL SURVEY ELIGIBILITY", "N");
                    Utility.LogActivity(ParticipantId, "SURVEY COMPLETED", "not eligible for full survey");

                    Response.Write(PageElements.PageHeader("Logo Only"));
                    if (DEBUG) Response.Write(PageElements.EligibilityReasoning(ParticipantId, "Survey"));
                    Response.Write(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\question\\NEG_EXIT.htm"));
		            Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
                }
            }
            else if (QuestionCode == "SER")
            {
                if (Logic.InterventionEligibility(ParticipantId))
                {
                    //Utility.UpdateStatus(ParticipantId, "INTERVENTION ELIGIBILITY", "Y");
                    Response.Redirect("screen.aspx?q=" + Index.ToString());
                }
                else
                {
                    //Utility.UpdateStatus(ParticipantId, "INTERVENTION ELIGIBILITY", "N");
                    Utility.LogActivity(ParticipantId, "SURVEY COMPLETED", "not eligible for intervention");

                    Response.Write(PageElements.PageHeader("Logo Only"));
                    if (DEBUG) Response.Write(PageElements.EligibilityReasoning(ParticipantId, "Intervention"));
                    Response.Write(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\question\\NEG_EXIT.htm"));
                    Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
                }
            }
            else if (QuestionCode == "ALC3")
            {
                if (SurveyResponses == "0|0|0")
                {
                    Db.Execute("DELETE FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC7'");
                    Db.Execute("INSERT INTO screening_response VALUES ('" + ParticipantId + "','ALC7','0|0|0|0|0|0|0','" + DateTime.Now.ToString() + "')");
                    Response.Redirect("screen.aspx?p=" + (Index + 1).ToString());
                }
                else
                {
                    Response.Redirect("screen.aspx?p=" + Index.ToString());
                }
            }
            else
            {
                Response.Redirect("screen.aspx?p=" + Index.ToString());
            }
        }
    }
}