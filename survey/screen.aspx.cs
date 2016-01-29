using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Net;
using System.Collections.Generic;

public partial class survey_screen : System.Web.UI.Page
{

    private int Index;
    private string[] QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\q.txt").Split(',');
    private string ParticipantId;
    private Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool DEBUG = (bool)Application["DEBUG"];
        if (Session["PARTICIPANT_ID"] == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid Web Request/Session Timed Out: Please use the web address you received in the invitation email to access the site.</SPAN>"); return; }

        ParticipantId = (string)Session["PARTICIPANT_ID"];
        SchoolInfo=Helper.FindSchool(ParticipantId);

        try
        {
            Index = int.Parse(Request.QueryString[0]);
        }
        catch
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; 
        }

        // screening completed, sending email
        if (Index >= QuestionCodes.Length) 
        {
            Utility.LogActivity(ParticipantId, "SURVEY COMPLETED", "all questions completed");

            string _intervention_group = Utility.GetStatus(ParticipantId, "INTERVENTION");
            
            // this is to send email to the person of the password if they are in the intervention group. otherwise email to signal end for control group
            __sendEmailFollowUp(_intervention_group);

            Response.Redirect("receipt.aspx?p=" + ParticipantId + "&p1=0");
            return; 
        }

        // display the page and show progression bar
        Response.Write(PageElements.PageHeader("Plain", "\"__validatePage('#0066CC')\"", SchoolInfo["code"]));

        if (DEBUG && Index != 0) Response.Write(PageElements.PrintDebug(ParticipantId, QuestionCodes[Index - 1]));

        // pass value using form instead of session, as people may use back arrow which cannot be detected using session variables
        Response.Write(__buildQuestions(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory
            + "\\survey\\question\\" + QuestionCodes[Index] + ".htm").Replace("_PROGRESSION_", __buildProgressionBar()).Replace("_INPUT_", "<INPUT type='hidden' name='IDX' value='" + Index + "," + QuestionCodes[Index] + "'>")));

        Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));

    }

    private void __sendEmailFollowUp(string InterventionGroup)
    {
        SchoolInfo = Helper.FindSchool(ParticipantId);
        string BASE_URL;
        if ((bool)Application["DEBUG"]) BASE_URL = SchoolInfo["test_server"];
        else BASE_URL = SchoolInfo["email_server"];

        string EmailBody = "";

        if (InterventionGroup=="1")
        {
            string pw = Db.GetRecord(String.Format("SELECT password FROM participant WHERE id = '{0}'", ParticipantId))[0];
            EmailBody += String.Format(@"Thank you again for completing the survey.  We now invite you to ask questions and discuss your feedback with your professional counselor.

Please go to the site shown below to learn more about this opportunity, or to chat with a counselor online privately: 
                                         
{0}/dialog/login.aspx?p={1}

Your initial password is '{2}'

                                         ", BASE_URL, ParticipantId, pw);

            string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor where site='"+SchoolInfo["code"]+"'");

            for (int i = 0; i < CounselorEmail.Length; i++)
            {   
                if (CounselorEmail[i][0].Substring(0, 1) != "#")
                {
                    Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", "eBridge: Student " + ParticipantId + " completed the survey and is eligible for intervention", "No action required. Click " + BASE_URL + "/dialog/list.aspx to see an updated respondent list");
                    //if ((bool)Application["DEBUG"]) Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", "eBridge: Student " + ParticipantId + " completed the survey and is eligible for intervention", "No action required. Click " + BASE_URL + "/dialog/list.aspx to see an updated respondent list");
                    //else Utility.SendGMail(CounselorEmail[i][0], "participant", SchoolInfo["email"], "The eBridge Team", "eBridge: Student " + ParticipantId + " completed the survey and is eligible for intervention", "No action required. Click " + BASE_URL + "/dialog/list.aspx to see an updated respondent list");
                    
                }
            }
        }
        else
        {
            string EmailPath = String.Format("\\App_Include\\email_templates\\{0}.html", SchoolInfo["code"]);
            string EmailTemplate = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + EmailPath);
            EmailBody += String.Format(EmailTemplate, new string[2]{BASE_URL, ParticipantId});
        }

        EmailBody += PageElements.EmailSignature();

        Utility.SendPHPMail((string)Session["EMAIL"], SchoolInfo["email"], "The eBridge Team", "A Message from the eBridge Team", EmailBody);
        //if ((bool)Application["DEBUG"]) Utility.SendPHPMail((string)Session["EMAIL"], SchoolInfo["email"], "The eBridge Team", "A Message from the eBridge Team", EmailBody);
        //else Utility.SendGMail((string)Session["EMAIL"], "participant", SchoolInfo["email"], "The eBridge Team", "A Message from the eBridge Team", EmailBody);
   }

    private string __buildProgressionBar()
    {
        string _progression = "<TABLE><TR>";
        if (Index < 6)
        {   
            for (int i = 0; i < 6; i++) _progression += "<TD width='13px' height='15px' style='padding-top:2px;background-color:" + (Index == i ? "red" : (Index > i ? "yellowgreen" : "#CDCDCD")) + ";font-size:9px' align='center'>" + (i + 1).ToString() + "</TD>";
            _progression += "<TD style='font-family:arial;font-size:12px;font-size:9px;padding-left:8px'><A href='#' onclick=\"alert('Thanks for checking out the study. You may now close your browser window.');\">Exit Study</A></TD></TR></TABLE>";
            return _progression;
        }
        else 
        {
            for (int i = 6; i < QuestionCodes.Length + 1; i++) _progression += "<TD width='13px' height='15px' style='padding-top:2px;background-color:" + (Index == i ? "red" : (Index > i ? "yellowgreen" : "#CDCDCD")) + ";font-size:9px' align='center'>" + (i+1).ToString() + "</TD>";
            _progression += "<TD style='font-family:arial;font-size:12px;font-size:9px;padding-left:8px'><A href='#' onclick=\"alert('Thanks for checking out the study. You may now close your browser window.');\">Exit Study</A></TD></TR></TABLE>";
            return _progression;
        }
    }

    private string __buildQuestions(string StoredString)
    {        //Set header style for 5-item scales, disagree to agree

        string LikertStyle = @"<TABLE cellpadding='0' 
                                cellspacing='0' 
                                style='font-family:arial;font-size:12px'>
                                <TR><TD></TD>
                                <TD width='285px'></TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Strongly Disagree</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Disagree</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Somewhat Disagree</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Somewhat Agree</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Agree</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Strongly Agree</TD>
                                </TR>
                                <TR height='5px'></TR>";
        
        string HelpfulStyle = @"<TABLE cellpadding='0' 
                                cellspacing='0' 
                                style='font-family:arial;font-size:12px'>
                                <TR><TD></TD>
                                <TD width='285px'></TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Not at all helpful</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>A little helpful</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Quite helpful</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Very helpful</TD>
                                </TR>
                                <TR height='5px'></TR>";

        string RulerStyle = @"<TR>
                              <TD width='285px'></TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>I am not ready to do this/I have no interesting in doing this</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>Sometimes I think about doing this</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>I am strongly considering doing this</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>I have taken steps toward doing this</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>&nbsp;</TD>
                              <TD valign='top' align='center' style='font-size:11px' width='75px'>I already did this</TD>
                              </TR>
                              <TR height='10px'></TR>";




        //Dictionary<string, string> site_data = new Dictionary<string, string>();
        //Get custom info on site
        string ParticipantId = (string)Session["PARTICIPANT_ID"];
        string WarningData = Helper.WarningMessage(ParticipantId);
        Dictionary<string, string> ConsentContent = Helper.ConsentContent(ParticipantId);

        //StoredString = StoredString.Replace("_SCHOOL_NAME_", school_name).Replace("_SCHOOL_DEPT_", school_dept).Replace("_SCHOOL_ADDR_", school_addr).Replace("_SCHOOL_TELE_", school_tele);
        StoredString = StoredString.Replace("_THICK_BEGIN_ITEM_", "<TR height='35px'><TD></TD>").Replace("_END_ITEM_", "</TD></TR>");
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_LIKERT_", LikertStyle);
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_HELPFUL_", HelpfulStyle);
        //StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_RULER_", RulerStyle);

        string PainfulStyle = @"<TABLE cellpadding='0' 
                                cellspacing='0' 
                                style='font-family:arial;font-size:12px'>
                                <TR><TD></TD>
                                <TD width='285px'></TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Very slightly or not at all</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>A little</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Moderately</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Quite a bit</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Extremely</TD>
                                </TR>
                                <TR height='5px'></TR>";

        string ImpulsiveStyle = @"<TABLE cellpadding='0' 
                                cellspacing='0' 
                                style='font-family:arial;font-size:12px'>
                                <TR><TD></TD>
                                <TD width='285px'></TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Agree Strongly</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Agree Some</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Disagree Some</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Disagree Strongly</TD>
                                </TR>
                                <TR height='5px'></TR>";

        string PHQ10Style= @"<TABLE cellpadding='0' 
                                cellspacing='0' 
                                style='font-family:arial;font-size:12px'>
                                <TR><TD></TD>
                                <TD width='285px'></TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Not Difficult at all</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Somewhat Difficult</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Very Difficult</TD>
                                <TD valign='top' align='center' style='font-size:11px' width='75px'>Extremely Difficult</TD>
                                </TR>
                                <TR height='5px'></TR>";
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_PHQ10_", PHQ10Style);
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_IMPULSIVE_", ImpulsiveStyle);
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_PAINFUL_", PainfulStyle);
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_LIKERT_", LikertStyle);
        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_HELPFUL_", HelpfulStyle);
        StoredString = StoredString.Replace("_BEGIN_TITLE_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD style='padding-top:10px;padding-bottom:30px;font-weight:bold'>").Replace("_END_TITLE_", "</TD></TR></TABLE>");


        StoredString = StoredString.Replace("_BEGIN_QUESTION_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>").Replace("_END_QUESTION_", "<TR height='20px'><TD></TD></TR></TABLE>");

        StoredString = StoredString.Replace("_BEGIN_HORIZONTAL_QUESTION_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD></TD><TD width='285px'></TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Not at all</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Several days</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>More than half the days</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Nearly every day</TD></TR><TR height='5px'></TR>");

        StoredString = StoredString.Replace("_HORIZONTAL_STYLE_A_", "style='background-color:#CDCDCD;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='orange'\" onmouseout=\"this.style.backgroundColor='#CDCDCD'\"");
        StoredString = StoredString.Replace("_HORIZONTAL_STYLE_B_", "style='background-color:#ECECEC;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='orange'\" onmouseout=\"this.style.backgroundColor='#ECECEC'\"");

        StoredString = StoredString.Replace("_BEGIN_ITEM_", "<TR height='20px'><TD></TD>").Replace("_END_ITEM_", "</TD></TR>");

        StoredString = StoredString.Replace("_REGULAR_STYLE_H_", "width='60px' align='center' style='padding-top:4px;padding-bottom:2px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='#CDCDCD'\"");
        
        StoredString = StoredString.Replace("_REGULAR_STYLE_", "style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"");
        //StoredString = StoredString.Replace("_REGULAR_STYLE_THICK_", "style='padding-top:2px;padding-bottom:2px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"");
        


        StoredString = StoredString.Replace("_WARNING_", WarningData);

        //Modify second warning content on suicide page so it will display
        string WarningData2 = WarningData.Replace("id='W'", "id='W2'");
        string WarningData3 = WarningData.Replace("id='W'", "id='W3'");
        string WarningData4 = WarningData.Replace("id='W'", "id='W4'");
        System.Diagnostics.Debug.Write(WarningData2);
        StoredString = StoredString.Replace("_WARNING2_", WarningData2);
        StoredString = StoredString.Replace("_WARNING3_", WarningData3);

        //Consent content changes
        StoredString = StoredString.Replace("_SCHOOL_UPPER_", ConsentContent["school_title"].ToUpper()).Replace("_SCHOOL_NAME_", ConsentContent["school_title"]).Replace("_IRB_INFO_", ConsentContent["irb_info"]).Replace("_PRIVACY_INFO_", ConsentContent["privacy_info"]).Replace("_SECONDARY_CONTACT_", ConsentContent["secondary_contact"]);
        StoredString = StoredString.Replace("_MISSING_", "<TABLE id='M' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-left:15px;padding-right:15px;padding-bottom:10px'><TR height='20px'><TD style='padding:12px;color:#FFFFFF;background-color:#009900;font-weight:bold'>We noticed that you did not answer all of the questions. You may answer them now if you are willing to do so, or you can click NEXT again to proceed to the next section of the survey.</TD></TR></TABLE>");

        StoredString = StoredString.Replace("_SUBMIT_BUTTON_REQUIRED_", "<CENTER style='padding-top:20px'><IMG src='../image/next.png' border='0' onclick=\"if (__validatePage('red')){document.forms[0].submit();} else {alert('In order to include you in our study, we need you to answer the question(s) above. If you do not wish to continue in the study, please click on Exit Study in the top-right corner.');return false;}\" style='cursor:hand;cursor:pointer'></CENTER>");
        StoredString = StoredString.Replace("_SUBMIT_BUTTON_NOT_REQUIRED_", "<CENTER style='padding-top:20px'><IMG src='../image/next.png' border='0' onclick=\"if (__validatePage('red')) document.forms[0].submit(); else {if (_missing==0) {document.getElementById('M').style.display='block'; _missing++} else {__validateNonRequired(); document.forms[0].submit();}}\" style='cursor:hand;cursor:pointer'></CENTER>");
        string SERfunction = @"
        if (__validatePage('red')) {
            if (__validateOthers('red')) {
                document.forms[0].submit();
            }
            else {
                if (_missing != 0) {
                    __validateNonRequired(); document.forms[0].submit();
                }
                else {
                    document.getElementById('M').style.display = 'block'; _missing++;   
                }
            }
        }
        else {
            alert('In order to include you in our study, we need you to answer the question(s) above. If you do not wish to continue in the study, please click on Exit Study in the top-right corner.'); return false;
        }";
        StoredString = StoredString.Replace("_SUBMIT_BUTTON_HYBRID_REQUIRED_", string.Format("<CENTER style='padding-top:20px'><IMG src='../image/next.png' border='0' onclick=\"{0}\" style='cursor:hand;cursor:pointer'></CENTER>", SERfunction));

        return StoredString;
    }
}
