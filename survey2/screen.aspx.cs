﻿using System;
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

public partial class survey2_screen : System.Web.UI.Page
{

    private int Index;
    private string[] QuestionCodes;
    private string ParticipantId;
    private Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool DEBUG = (bool)Application["DEBUG"];
        if (Session["PARTICIPANT_ID"] == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        ParticipantId = (string)Session["PARTICIPANT_ID"];
        SchoolInfo= Helper.FindSchool(ParticipantId);

        try
        {
            Index = int.Parse(Request.QueryString[0]);
        }
        catch
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; 
        }

        QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey2\\q1.txt").Split(',');

        /*string ActivelyEngagedStudents = "5PDH20qgtH,a9yKO7y4Ml,c9Ak3ZKu9m,eIJj56mBa3,G6MJlvWSw1,jp6KjP0vZx,mrJ22lcOXJ,nZDaKEx5Le";

        if (ActivelyEngagedStudents.IndexOf(ParticipantId) == -1)
        {
            QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey2\\q1.txt").Split(',');
        }
        else
        {
            QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey2\\q2.txt").Split(',');
        }
        */
        // follow-up survey completed, sending email
        if (Index >= QuestionCodes.Length) 
        {
            Utility.LogActivity(ParticipantId, "FOLLOWUP SURVEY COMPLETED", "all questions completed");

            string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor where site='"+SchoolInfo["code"]+"'");

            for (int i = 0; i < CounselorEmail.Length; i++)
            {
                if (CounselorEmail[i][0].Substring(0, 1) != "#")
                {
                    //if ((bool)Application["DEBUG"]) Utility.SendPHPMail(CounselorEmail[i][0], SchoolInfo["email"], "The eBridge Team", ParticipantId + " responded to the followup survey", "As title");
                    //else Utility.SendGMail(CounselorEmail[i][0], "counselor", SchoolInfo["email"], "The eBridge Team", ParticipantId + " responded to the followup survey", "As title");
                }
            }
        
            Response.Redirect("receipt.aspx?p=" + ParticipantId);
            return; 
        }

        // display the page and show progression bar
        if (Index != 0) Response.Write(PageElements.PageHeader("Plain", "\"__validatePage('#0066CC')\""));
        else
        {
            Response.Write(PageElements.PageHeader("Logo Only", "\"__validatePage('#0066CC')\"").Replace("padding-top:50px;padding-bottom:40px","padding-top:50px;padding-bottom:20px"));
            Response.Write("<DIV style='padding-top:10px;padding-bottom:20px'><HR style='border-left:0px;border-right:0px;border-top:0px;border-bottom:1px dashed #454545' width='585px' align='center'></DIV>");
        }

        string WarningData = Helper.WarningMessage(ParticipantId);
        string WarningData2 = WarningData.Replace("id='W'", "id='W2'");
        if (DEBUG && Index != 0) Response.Write(PageElements.PrintDebug2(ParticipantId, QuestionCodes[Index - 1]));

        // pass value using form instead of session, as people may use back arrow which cannot be detected using session variables
        string _o = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory
            + "\\survey2\\question\\" + QuestionCodes[Index] + ".htm").Replace("_PROGRESSION_", __buildProgressionBar()).Replace("_INPUT_", "<INPUT type='hidden' name='IDX' value='" + Index + "," + QuestionCodes[Index] + "'>");

        _o = _o.Replace("_BEGIN_TITLE_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD style='padding-top:10px;padding-bottom:30px;font-weight:bold'>").Replace("_END_TITLE_", "</TD></TR></TABLE>");

        _o = _o.Replace("_BEGIN_QUESTION_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>").Replace("_END_QUESTION_","<TR height='20px'><TD></TD></TR></TABLE>");

        _o = _o.Replace("_BEGIN_HORIZONTAL_QUESTION_TYPE2_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px' width='595px'><TR><TD></TD><TD width='360px'></TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Strongly agree</TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Agree</TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Somewhat agree</TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Somewhat disagree</TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Disagree</TD><TD valign='top' align='center' style='font-size:11px' width='60px'>Strongly disagree</TD></TR><TR height='5px'></TR>");

        _o = _o.Replace("_BEGIN_HORIZONTAL_QUESTION_TYPE3_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px' width='595px'><TR><TD></TD><TD width='285px'></TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Not difficult at all</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Somewhat Difficult</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Very Difficult</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Extremely Difficult</TD></TR><TR height='5px'></TR>");

        _o = _o.Replace("_BEGIN_HORIZONTAL_QUESTION_", "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px' width='595px'><TR><TD></TD><TD width='285px'></TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Not at all</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Several days</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>More than half the days</TD><TD valign='top' align='center' style='font-size:11px' width='75px'>Nearly every day</TD></TR><TR height='5px'></TR>");



        _o = _o.Replace("_HORIZONTAL_STYLE_A_", "style='background-color:#CDCDCD;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='orange'\" onmouseout=\"this.style.backgroundColor='#CDCDCD'\"");
        _o = _o.Replace("_HORIZONTAL_STYLE_B_", "style='background-color:#ECECEC;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='orange'\" onmouseout=\"this.style.backgroundColor='#ECECEC'\"");
        _o = _o.Replace("_THICK_BEGIN_ITEM_", "<TR height='35px'><TD></TD>").Replace("_END_ITEM_", "</TD></TR>");
        _o = _o.Replace("_BEGIN_ITEM_", "<TR height='20px'><TD></TD>").Replace("_END_ITEM_","</TD></TR>");

        _o = _o.Replace("_REGULAR_STYLE_H_", "width='60px' align='center' style='padding-top:4px;padding-bottom:2px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='#CDCDCD'\"");
        
        _o = _o.Replace("_REGULAR_STYLE_", "style='padding-top:4px;padding-bottom:4px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"");


        _o = _o.Replace("_WARNING_", WarningData);
        _o = _o.Replace("_WARNING2_", WarningData2);

        _o = _o.Replace("_MISSING_", "<TABLE id='M' cellpadding='0' cellspacing='0' style='display:none;font-family:arial;font-size:12px;padding-left:15px;padding-right:15px;padding-bottom:10px'><TR height='20px'><TD style='padding:12px;color:#FFFFFF;background-color:#009900;font-weight:bold'>We noticed that you did not answer all of the questions. You may answer them now if you are willing to do so, or you can click NEXT again to proceed to the next section of the survey.</TD></TR></TABLE>");

        _o = _o.Replace("_SUBMIT_BUTTON_REQUIRED_","<CENTER style='padding-top:20px'><IMG src='../image/next.png' border='0' onclick=\"if (__validatePage('red')){document.forms[0].submit();} else {alert('In order to include you in our study, we need you to answer the question(s) above. If you do not wish to continue in the study, please click on Exit Study in the top-right corner.');return false;}\" style='cursor:hand;cursor:pointer'></CENTER>");
        _o = _o.Replace("_SUBMIT_BUTTON_NOT_REQUIRED_", "<CENTER style='padding-top:20px'><IMG src='../image/next.png' border='0' onclick=\"if (__validatePage('red')) document.forms[0].submit(); else {if (_missing==0) {document.getElementById('M').style.display='block'; _missing++} else document.forms[0].submit();}\" style='cursor:hand;cursor:pointer'></CENTER>");

        if (QuestionCodes[Index] == "OQ1")
        {
            string[] _value = Db.GetRecord("SELECT response FROM followup_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER3'")[0].Split('|');

            if (_value[0] == "0" || _value[1] == "1" || _value[1] == "2" || _value[1] == "3" || _value[1] == "4" || _value[2] == "0" || _value[2] == "1" || _value[3] == "1" || _value[3] == "2" || _value[3] == "3")
            {
                _o = _o.Replace("_OPEN_ENDED_QUESTION_1_", "Earlier in this survey you indicated that you have received at least some mental health services (medication and/or therapy) in the past two months for your mental or emotional health.  We are interested to know more about why you received these services. We would be grateful if you would comment on this:");
            }
            else
            {
                _o = _o.Replace("_OPEN_ENDED_QUESTION_1_", "You indicated earlier that you have not received any mental health services (medication and/or therapy) in the past two months.  We are interested to know more about why you have not received any services. We would be grateful if you would comment on this:");
            }
        }

        if (QuestionCodes[Index] == "OQ2")
        {
            if (Utility.GetStatus(ParticipantId, "INTERVENTION") == "1")
            {
                _o = _o.Replace("_OPEN_ENDED_QUESTION_2_", "As part of this study, about two months ago you completed a survey similar to this one. Then we sent you information summarizing your responses and we invited you to correspond anonymously with a counselor about these topics. We would be very interested in any comments you have about your experience in this study (e.g., things you liked or disliked, things you found helpful or not helpful, other things that might have been helpful to you):");
            }
            else
            {
                _o = _o.Replace("_OPEN_ENDED_QUESTION_2_", "As part of this study, about two months ago you completed a survey similar to this one, and then we sent you some information summarizing your responses. We would be very interested in any comments you have about your experience in this study (e.g., things you liked or disliked, things you found helpful or not helpful, other things that might have been helpful to you):");
            }
        }

        _o = _o.Replace("_OPEN_ENDED_QUESTION_3_", "We would like to offer some type of online service to students who report distress.  What type of services (online counseling, peer-to-peer support/counseling, list of resources, etc.) and what way of delivery (e.g. chatting, email exchange, web-cam, etc.) would be most used by students?");

        Response.Write(_o);

        Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
    }

    private string __buildProgressionBar()
    {
        string _progression = "<TABLE><TR>";
        for (int i = 0; i < QuestionCodes.Length; i++) _progression += "<TD width='13px' height='15px' style='padding-top:2px;background-color:" + (Index == i ? "red" : (Index > i ? "yellowgreen" : "#CDCDCD")) + ";font-size:9px' align='center'>" + (i + 1).ToString() + "</TD>";
        _progression += "<TD style='font-family:arial;font-size:12px;font-size:9px;padding-left:8px'><A href='#' onclick=\"alert('Thanks for checking out the study. You may now close your browser window.');\">Exit Study</A></TD></TR></TABLE>";
        return _progression;
    }
}
