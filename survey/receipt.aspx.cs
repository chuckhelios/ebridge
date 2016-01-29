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
using System.Web.Services;
using System.IO;

public partial class survey_receipt : System.Web.UI.Page
{
    Dictionary<string, string> SchoolInfo;
    double ALC_SCORE, PHQ9_SCORE;
    string NEGATIVE_IMPACT, PHQ9_PERCENTILE, ALC_PERCENTILE, ALC_SEVERITY, PHQ9_SEVERITY, ACADEMIC_PERFORMANCE, SOCIAL_LIFE;
    string[] ALC_ASSESSMENT, PHQ_ASSESSMENT, HEALTH_MIND;

    string ParticipantId, ParticipantFirstName;
    //string GOALS, VALUES;
    string[] PHQ7_HM, HM;//, Goal_Value;
    bool InterventionGroup;
    int PageNumber;

    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Buffer = false;
        
        try
        {
            ParticipantId = Request.QueryString[0];
            ParticipantFirstName = Db.GetRecord("SELECT name_first FROM participant WHERE id = '" + ParticipantId + "'")[0];
            if (Request.QueryString.Count == 1) Response.Redirect("receipt.aspx?p=" + ParticipantId + "&p1=0");
            PageNumber = int.Parse(Request.QueryString[1]);
            if (Utility.GetStatus(ParticipantId, "INTERVENTION") == "1") InterventionGroup = true; else InterventionGroup = false;
            // if participant is in the intervention group, they are required to sign in. otherwise, no password needed. 
            if (Session.Contents.Count ==0 && InterventionGroup) { Response.Redirect("../dialog/login.aspx?p=" + ParticipantId);return; }
        }
        catch
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access your personalized feedback.</SPAN>");
            return;
        }

        // log activity
        Utility.LogActivity(ParticipantId, "PERSONALIZED FEEDBACK VISITED", Request.UserHostAddress + "|" + Request.UserAgent);
        SchoolInfo = Helper.FindSchool(ParticipantId);
        if (InterventionGroup)
        {
            Response.Write("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");

            //int CHAT_ID = int.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "//chat//chatter_count.txt"));
            //CHAT_ID++;

            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//chat//chatter_count.txt", CHAT_ID.ToString());

            string CHAT_ID = ParticipantId;
            Db.Execute("DELETE FROM online_status WHERE id = '" + CHAT_ID + "'");
            
            //Response.Write(CHAT_ID); return;
            
            bool COUSELOR_AVAILBALE = false;

            int CURRENT_HOUR_MILITARY = int.Parse(DateTime.Now.TimeOfDay.ToString().Split(':')[0]);

            //if (CURRENT_HOUR_MILITARY >= 13 && CURRENT_HOUR_MILITARY < 21) COUSELOR_AVAILBALE = true;
            //COUSELOR_AVAILBALE = false;

            Response.Write(PageElements.PageHeader("Plain").Replace("padding-left:40px;padding-right:40px;padding-bottom:30px;padding-top:30px", "").Replace("BODY style='margin:0px;padding:0px'","BODY style='margin:0px;padding:0px;background-color:#454545'"));
            
            Response.Write("<SCRIPT type='text/javascript' src='./include/prototype.js'></SCRIPT>"
                   + "<SCRIPT type='text/javascript' src='./include/scriptaculous-js-1.9.0/src/scriptaculous.js'></SCRIPT>"
                   + "<SCRIPT type='text/javascript' src='./include/lightview2.7.1/js/lightview.js'></SCRIPT>"
                   + "<LINK rel='stylesheet' type='text/css' href='./include/lightview2.7.1/css/lightview.css' />");

            
            Response.Write(PageElements.DrawMessageJHeader(2, 2, 290));

            Response.Write("<SCRIPT type='text/javascript'>document.observe('lightview:hidden', function() {location.reload();});</SCRIPT>");
            //Response.Write("<SCRIPT type='text/javascript'>function killSession(url)"
            //        + "{"
            //            + "if (window.XMLHttpRequest) xmlhttp=new XMLHttpRequest(); else xmlhttp=new ActiveXObject('Microsoft.XMLHTTP');"
            //            + "xmlhttp.open('POST',url,true);"// change to post to fix IE ajax caching issue
            //            + "xmlhttp.send(null);}"
            //        + "}"
            //        + "</SCRIPT>");

            //Response.Write("<SCRIPT type='text/javascript'>window.onunload=\"alert('a');\"</SCRIPT>");
            //Response.Write("window.onbeforeunload = function() {return 'Are you sure you wish to leave this delightful page?';}");


            Session["PARTICIPANT_LOGIN"] = "Y";
            Session["DESIGNATED_COUNSELOR"] = Utility.GetStatus(ParticipantId, "COUNSELOR");

            //Db.Execute("DELETE FROM online_status WHERE id = '" + ParticipantId + "'");
            //Db.Execute("INSERT INTO online_status VALUES ('" + ParticipantId + "','" + Session["DESIGNATED_COUNSELOR"].ToString() + "','" + DateTime.Now + "')");
         
            //__readData();

            Response.Write("<TABLE width='727px' height='755px' cellpadding='0' cellspacing='0' style=\"font-family:arial;font-size:15px;background-image:url('./graph/result.jpg')\">");

            Response.Write("<TR><TD>"
                + "<TABLE width='310px'><TR height='470px'><TD>&nbsp;</TD></TR>"
                + "<TR height='50px'>"
                + "<TD style='cursor:hand;cursor:pointer' onclick=\"Lightview.show({ href: 'feedback.aspx?p=" + ParticipantId + "&p1=distress', rel: 'iframe', "
                    + "title: '<SPAN style=padding-left:12px>My Distress Level</SPAN>', options: {width: 600, height: 410}});\">&nbsp;</TD></TR>"
                + "<TR height='50px'>"
                + "<TD style='cursor:hand;cursor:pointer' onclick=\"Lightview.show({ href: 'feedback.aspx?p=" + ParticipantId + "&p1=alcohol', rel: 'iframe', "
                    + "title: '<SPAN style=padding-left:12px>My Alcohol Use Level</SPAN>', options: {width: 600, height: 470}});\">&nbsp;</TD></TR>"
                //+ "<TR height='50px'>"
                //+ "<TD style='cursor:hand;cursor:pointer' onclick=\"Lightview.show({ href: 'feedback.aspx?p=" + ParticipantId + "&p1=gv', rel: 'iframe', "
                //    + "title: '<SPAN style=padding-left:12px>My Goals and Values</SPAN>', options: {width: 600, height: 200}});\">&nbsp;</TD></TR>"
                + "</TABLE>"
                + "</TD>");

            Response.Flush();

            Response.Write("<TD valign='top' align='left' style='padding-top:300px;padding-left:20px;padding-right:30px'>");

            Response.Write("<B>What might you be interested in talking about?<BR>(Please choose one)<P>&nbsp;<BR>");

            if (COUSELOR_AVAILBALE)
            {
                Response.Write("<INPUT TYPE='radio' name='OPTION' id='CONCERN' value='CONCERN' onclick=\"Lightview.show({ href: '../chat/index.aspx?p=" + CHAT_ID + "&p1=p&p2=" + ParticipantId + "&p3=0', rel: 'iframe', "
                    + "title: '<SPAN style=padding-left:16px>Chat with Counselor</SPAN>', options: {width: 600, height: 550}});\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('CONCERN').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "More about my concerns or my survey feedback</SPAN><P>");

                Response.Write("<INPUT TYPE='radio' name='OPTION' id='RESOURCE' value='RESOURCE' onclick=\"Lightview.show({ href: '../chat/index.aspx?p=" + CHAT_ID + "&p1=p&p2=" + ParticipantId + "&p3=1', rel: 'iframe', "
                    + "title: '<SPAN style=padding-left:16px>Chat with Counselor</SPAN>', options: {width: 600, height: 550}});\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('RESOURCE').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "More about available resources</SPAN><P>");

                Response.Write("<INPUT TYPE='radio' name='OPTION' id='OTHER' value='OTHER' onclick=\"Lightview.show({ href: '../chat/index.aspx?p=" + CHAT_ID + "&p1=p&p2=" + ParticipantId + "&p3=2', rel: 'iframe', "
                    + "title: '<SPAN style=padding-left:16px>Chat with Counselor</SPAN>', options: {width: 600, height: 550}});\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('OTHER').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "Other</SPAN>");
            }
            else
            {
                Response.Write("<FORM name='MAIN' id='MAIN' method='post' action='action.aspx?p=initial_post'>");
                Response.Write("<INPUT name='PID' type='hidden' value='" + ParticipantId + "'>");
                Response.Write("<INPUT name='MSG' id='MSG' type='hidden'>");

                Response.Write("<INPUT TYPE='radio' name='OPTION' id='CONCERN' value='CONCERN' onclick=\"document.getElementById('MSG').value='0';document.getElementById('MAIN').submit()\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('CONCERN').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "More about my concerns or my survey feedback</SPAN><P>");

                Response.Write("<INPUT TYPE='radio' name='OPTION' id='RESOURCE' value='RESOURCE' onclick=\"document.getElementById('MSG').value='1';document.getElementById('MAIN').submit()\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('RESOURCE').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "More about available resources</SPAN><P>");

                Response.Write("<INPUT TYPE='radio' name='OPTION' id='OTHER' value='OTHER' onclick=\"document.getElementById('MSG').value='2';document.getElementById('MAIN').submit()\"> <SPAN style='cursor:hand;cursor:pointer;padding-top:5px;padding-bottom:5px' onclick=\"document.getElementById('OTHER').click()\" onmouseover=\"this.style.backgroundColor='#3399FF'\" onmouseout=\"this.style.backgroundColor='transparent'\">"
                    + "Other</SPAN>");

                Response.Write("</FORM>");
            }

            Response.Write("<P>&nbsp;<BR>");

            //if (COUSELOR_AVAILBALE) Response.Write("<B style='color:red'>A Counselor is available now. You will have the option of chatting with the counselor.</B>");
            //else Response.Write("<B style='color:red'>A Counselor is not available to chat at the moment. Counselor will respond to your message within 24 hours.</B>");

            //Response.Write(" (debug: Time of day: " + DateTime.Now.TimeOfDay + ")");
            
            //Response.Write("<B>Pick an option for <SPAN " + PageElements.DrawMessage("Private").Replace("text-underline-style:dotted", "text-underline-style:dotted;color:red") + ">private</SPAN> "
            //    + "conversation with <SPAN " + PageElements.DrawMessage("Counselor").Replace("text-underline-style:dotted", "text-underline-style:dotted;color:red") + ">the Counselor</SPAN>:</B><P>");

            //Response.Write("<INPUT TYPE='radio' name='OPTION' id='POST' value='POST'> <SPAN style='cursor:hand;cursor:pointer'\" onclick=\"document.getElementById('POST').click()\">"
            //    + "<SPAN " + PageElements.DrawMessage("Post").Replace("text-underline-style:dotted", "text-underline-style:dotted;color:red") + ">"
            //    + "Post a private message</SPAN> to the Counselor who will respond to you within 24 hours</SPAN><BR>");

            //Response.Write("<TEXTAREA name='MSG' id='MSG' maxlength='1000' style='width:300px;height:60px;font-family:verdana;font-size:11px' onfocus='this.value = this.value' onkeydown=\"document.getElementById('POST').click()\"></TEXTAREA><P>");

            //Response.Write("<INPUT TYPE='radio' name='OPTION' id='SCHEDULE_CHAT' value='SCHEDULE_CHAT'> <SPAN style='cursor:hand;cursor:pointer' onclick=\"document.getElementById('SCHEDULE_CHAT').click()\">"
            //    + "Schedule an appointment to <SPAN " + PageElements.DrawMessage("AvailableTimes").Replace("text-underline-style:dotted", "text-underline-style:dotted;color:red") + ">chat privately online</SPAN> with the Counselor.</SPAN><P>");

            //Response.Write("<INPUT TYPE='radio' name='OPTION' id='DEFER' value='DEFER'> <SPAN style='cursor:hand;cursor:pointer'\" onclick=\"document.getElementById('DEFER').click()\">"
            //    + "I am interested in connecting with the Counselor <SPAN" + PageElements.DrawMessage("Chat").Replace("text-underline-style:dotted", "text-underline-style:dotted;color:red") + ">later but not right now</SPAN>.<P>");

            Response.Write("<INPUT TYPE='radio' name='OPTION' value='--' checked style='display:none'>");

            //Response.Write("<INPUT type='submit' value='  GO!  ' style='padding-top:5px;font-family:arial;font-size:16px;font-weight:bold;background-color:#1b9252' onclick=\"if (!IsValid()) {return false;}\">");
            
            Response.Write("</TD>");
            Response.Write("</TR></FORM></TABLE>");

            Response.Write(PageElements.PageFooter("Plain", SchoolInfo["code"]));

            Response.Write("<SCRIPT type='text/javascript'>"
              + "function IsValid() {"
              + "if (document.getElementById('POST').checked&&document.getElementById('MSG').value=='') {alert('Please enter your message to the Counselor in the text area provided on the screen.');document.getElementById('MSG').focus();return false;} "
              + "if (!document.getElementById('POST').checked&&!document.getElementById('SCHEDULE_CHAT').checked&&!document.getElementById('DEFER').checked) {alert('Please select one of the options provided on the screen.');return false;} "
              + "return true;}"
              + "</SCRIPT>");

            /*
            Response.Write("<TABLE width='800px' cellpadding='0' cellspacing='0' style=\"font-family:arial;font-size:12px;background-color:#E8E8E8;background-image:url('./graph/P1-1.png')\"><TR>");

            Response.Write("<TD height='217px' width='125px'></TD><TD width='120px' valign='top' style='padding-top:50px;font-weight:bold'>");

            if (ACADEMIC_PERFORMANCE.ToLower() != "n/a" && ACADEMIC_PERFORMANCE.ToLower() != "not at all" && ACADEMIC_PERFORMANCE != "n/a") Response.Write("On the survey, you shared that emotional distress has interfered with your academic performance " + ACADEMIC_PERFORMANCE + " in the past two weeks. ");
            else Response.Write("On the survey, you shared that emotional distress has not interfered with your academic performance.");

            Response.Write("</TD><TD width='155px'></TD><TD width='120px' valign='top' style='padding-top:60px;font-weight:bold'>");

            if (SOCIAL_LIFE.ToLower() != "n/a" && SOCIAL_LIFE.ToLower() != "not at all") Response.Write("And you've shared that emotional distress has interfered with your social life " + SOCIAL_LIFE + ".");
            else Response.Write("And you've shared that emotional distress has not interfered with your social life.");

            Response.Write("</TD><TD width='125px'></TD><TD valign='top' style='padding-top:45px;font-weight:bold'>");

            Response.Write("Now, let's look at your responses, and how they compare to other college students.");

            Response.Write("</TR></TABLE>");

            Response.Write("<TABLE width='800px' cellpadding='0' cellspacing='0' style=\"font-family:arial;font-size:12px;background-color:#E8E8E8;background-image:url('./graph/P1-2.png')\"><TR>");

            Response.Write("<TD height='217px' width='250px' valign='top'><TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD valign='top' style='padding-top:15px;padding-left:55px;padding-right:35px;font-weight:bold'>");
            
            Response.Write("</TD></TR><TR><TD style='padding-left:8px'><IMG width='249px' src='./graph/phq/dep_" + Math.Floor(PHQ9_SCORE).ToString() + ".png'></TD></TR></TABLE></TD><TD width='125px'></TD><TD width='130px' valign='top' style='padding-top:35px;font-weight:bold'>");

            Response.Write("This means that your depression score is higher than <SPAN style='color:red'>" + PHQ9_PERCENTILE + "%</SPAN> of college students.");

            Response.Write("</TD><TD width='150px'></TD><TD valign='top' style='padding-top:40px;padding-right:20px;font-weight:bold'>");

            Response.Write("In summary, your recent level of depression appears to be <SPAN style='color:red'>" + PHQ9_SEVERITY.ToLower() + "</SPAN> and your emotional health is having <SPAN style='color:red'>" + NEGATIVE_IMPACT + "</SPAN> negative impacts on your life.");

            Response.Write("</TR></TABLE>");

            Response.Write("</TD></TR><TR height='10px'><TD>&nbsp;</TD></TR>");
            Response.Write("<TR><TD align='center' style='padding-bottom:5px;font-weight:bold;font-size:17px'><SPAN style='color:blue;cursor:hand;cursor:pointer' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='blue'\" onclick=\"window.location.href='receipt.aspx?p=" + ParticipantId + "&p1=" + (PageNumber + 1).ToString() + "'\">Next Page &gt;</SPAN></TD></TR>");
            */

            Response.Write("</TD></TR></TABLE>");
        }
        else // None-Intervention
        {
            __readData();

            if (PageNumber == 0)
            {
                Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD style='padding-bottom:20px'>");

                //Response.Write("<B>Dear " + ParticipantFirstName + ",</B><P>"
                Response.Write("&nbsp;<P>Thank you for completing the survey. On the next few screens, you will see a report summarizing your answers to the survey. "
                    + "At the bottom of the page there is also a list of local resources for student mental health, in case you think these might be helpful to "
                    + "you at any point. This concludes your participation in our study&mdash;thank you!");

                Response.Write("<TR height='10px'><TD>&nbsp;</TD></TR>");
                Response.Write("<TR><TD align='right' style='padding-bottom:5px;font-weight:bold;font-size:13px'><SPAN style='color:blue;cursor:hand;cursor:pointer' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='blue'\" onclick=\"window.location.href='receipt.aspx?p=" + ParticipantId + "&p1=" + (PageNumber + 1).ToString() + "'\">Next Page &gt;</SPAN></TD></TR>");
                Response.Write("</TD></TR></TABLE>");

                // emotional health
                Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#E8E8E8'>"
                        + "<TR height='25px'><TD align='center' colspan='2' height='18px' style='font-size:15px;background-color:#6699FF'><B>&nbsp;1. EMOTIONAL HEALTH&nbsp;</B></TD></TR>"
                        + "<TR><TD width='160px' style='padding-left:20px;padding-top:15px'>Depression score *:</TD><TD style='padding-top:15px'><B>" + Math.Floor(PHQ9_SCORE).ToString() + "</B> (out of 27)</TD></TR>"
                        + "<TR><TD style='padding-left:20px'>Depression percentile **:</TD><TD style='font-weight:bold'>" + PHQ9_PERCENTILE + "%</TD></TR>"
                        + "<TR><TD style='padding-left:20px'>Severity category:</TD><TD style='font-weight:bold'>" + PHQ9_SEVERITY + "</TD></TR>"

                        + "<TR><TD colspan='2' style='padding-top:10px;padding-left:20px;font-size:10px'>* Depression score ranges from 0 to 27, where higher numbers indicate higher severity.</TD></TR>"
                        + "<TR><TD colspan='2' style='padding-left:20px;font-size:10px'>** You have a higher depression score than this percentage of college students.</TD></TR>"

                        + "<TR><TD colspan='2' style='padding-left:20px;padding-top:5px'><IMG src='chart_phq/depression_" + Math.Floor(PHQ9_SCORE).ToString() + ".png' width='530px' height='322px'><BR></TD></TR>"
                        + "<TR><TD colspan='2' style='padding-left:20px;padding-right:20px;padding-bottom:20px'><B>How your emotional health is affecting your life:</B>");

                // commented out due to implication with hm measures                    

                //if (ACADEMIC_PERFORMANCE.ToLower() != "n/a" && ACADEMIC_PERFORMANCE.ToLower() != "not at all") Response.Write(" You reported that emotional distress has interfered with your academic performance " + ACADEMIC_PERFORMANCE + " in the past two weeks. ");
                //else Response.Write(" You reported that emotional distress has not interfered with your academic performance.");

                //if (SOCIAL_LIFE.ToLower() != "n/a" && SOCIAL_LIFE.ToLower() != "not at all") Response.Write(" You also reported interference with your social life " + SOCIAL_LIFE + ".");
                //else Response.Write(" You also reported emotional distress has not interfered with your social life.");

                if (PHQ_ASSESSMENT != null)
                {
                    Response.Write(" In addition, you reported problems with:<UL>");
                    for (int i = 0; i < PHQ_ASSESSMENT.Length; i++) Response.Write("<LI>" + PHQ_ASSESSMENT[i] + "</LI>");
                    Response.Write("</UL>");
                }

                Response.Write("<P>In summary, your recent level of depression appears to be <B>" + PHQ9_SEVERITY.ToLower() + "</B> and your emotional health has <B>" + NEGATIVE_IMPACT + "</B> negative impacts on your life.</TD></TR>");

                Response.Write("<TR><TD colspan='2' align='right' style='padding-top:10px;font-weight:bold;font-size:13px;background-color:#FFFFFF'><SPAN style='color:blue;cursor:hand;cursor:pointer' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='blue'\" onclick=\"window.location.href='receipt.aspx?p=" + ParticipantId + "&p1=" + (PageNumber + 1).ToString() + "'\">Next Page &gt;</SPAN></TD></TR>");

                Response.Write("</TABLE><P>&nbsp;<BR>");
            }
            else // Page 2
            {
                Response.Write("&nbsp;<P><TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#E8E8E8'>");
                Response.Write("<TR height='25px'><TD align='center' colspan='2' height='18px' style='font-size:15px;background-color:#6699FF'><B>&nbsp;2. ALCOHOL USE&nbsp;</B></TD></TR>"
                    + "<TR><TD width='160px' style='padding-left:20px;padding-top:15px'>Alcohol risk score *:</TD><TD style='padding-top:15px'><B>" + Math.Floor(ALC_SCORE).ToString() + "</B> (out of 40)</TD></TR>"
                    + "<TR><TD style='padding-left:20px'>Alcohol risk percentile **:</TD><TD style='font-weight:bold'>" + ALC_PERCENTILE + "%</TD></TR>"
                    + "<TR><TD style='padding-left:20px'>Severity classification:</TD><TD style='font-weight:bold'>" + ALC_SEVERITY + "</TD></TR>"
                    + "<TR><TD colspan='2' style='padding-top:10px;padding-left:20px;font-size:10px'>* This score ranges from 0 to 40, where higher numbers indicate a higher likelihood of alcohol-related problems.</TD></TR>"
                    + "<TR><TD colspan='2' style='padding-left:20px;font-size:10px'>** You have a higher alcohol risk score than this percentage of college students.</TD></TR>"
                    + "<TR><TD colspan='2' style='padding-left:10px;padding-top:10px'>");
                Response.Write("<IMG src='chart_alc/alcohol_" + Math.Floor(ALC_SCORE).ToString() + ".png' width='580px' height='116px'>");
                Response.Write("<BR>&nbsp;<BR></TD></TR>");

                if (ALC_ASSESSMENT != null)
                {
                    Response.Write("<TR><TD colspan='2' style='padding-top:50px;padding-left:20px;padding-right:20px;padding-bottom:20px'>");
                    Response.Write(" In addition, you reported specific difficulties with the following:<UL>");
                    for (int i = 0; i < ALC_ASSESSMENT.Length; i++) Response.Write("<LI>" + ALC_ASSESSMENT[i] + "</LI>");
                    Response.Write("</UL>");
                    Response.Write("</TD></TR>");
                }

                //+ "<TR><TD colspan='2' style='padding-left:10px;font-size:10px;background-color:white'>**You have a higher alcohol risk score than this percentage of college students.</TD></TR>"

                Response.Write("</TABLE><P>&nbsp;<BR>");

                //Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#E8E8E8'>"
                 //       + "<TR height='25px'><TD align='center' colspan='2' height='18px' style='font-size:15px;background-color:#6699FF'><B>&nbsp;3. GOALS AND VALUES&nbsp;</B></TD></TR>"
                //        + "<TR><TD style='padding-left:20px;padding-top:15px'>The following goals are what you value: <B>" + GOALS + "</B></TD></TR>"
                //        + "<TR><TD style='padding-left:20px;padding-bottom:20px'>The following values are what you appreciate: <B>" + VALUES + "</B></TD></TR>"
                //        + "</TABLE><P>&nbsp;<BR>");

                // overall summary
                Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#E8E8E8'>"
                    + "<TR height='25px'><TD align='center' colspan='2' height='18px' style='font-size:15px;background-color:#6699FF'><B>&nbsp;3. OVERALL SUMMARY&nbsp;</B></TD></TR>"
                    + "<TR><TD style='padding-left:20px;;padding-top:15px;padding-bottom:15px'>Based on your survey answers:"
                        + "<UL>"
                            + "<LI>You have been experiencing a <B>" + PHQ9_SEVERITY.ToLower() + "</B> level of depression.</LI>");

                if (HEALTH_MIND != null)
                {
                    for (int i = 0; i < HEALTH_MIND.Length; i++) Response.Write("<LI>" + HEALTH_MIND[i] + "</LI>");
                }

                Response.Write("<LI>Your risk of alcohol-related dependence and harm is <B>" + ALC_SEVERITY.ToLower() + "</B>, "
                    + "and you " + (ALC_ASSESSMENT != null ? "reported" : "did not report") + " ways in which your alcohol use is negatively affecting your life.</LI>");
                //            + "<LI>These things are most important to you:</LI>"
                //                + "<UL>"
                //                    + "<LI>Goals: <B>" + GOALS.ToLower() + "</B></LI>"
                //                    + "<LI>Values: <B>" + VALUES.ToLower() + "</B></LI>"
                //                + "</UL>"
                //        + "</UL>"
                //    + "</TD></TR></TABLE><BR>&nbsp;<BR>");

                Response.Write("</TD></TR></TABLE><BR>&nbsp;<BR>");
                Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD align='center'><B>Thank you again for the completing the survey.</B><BR>&nbsp;<BR><BR>&nbsp;<BR><SPAN style='font-size:10px'>You have completed all sections of the survey. You may close the browser window now.");
                Response.Write("</TD></TR></TABLE><BR>&nbsp;<BR>");
            }
        }
    }


        /*


        if (PageNumber < 2)
        {
            string _h = PageElements.PageHeader("Logo Only").Replace("</HEAD>", PageElements.Header().Replace("./", "../dialog/") + "</HEAD>");

            Response.Write(_h);

            __readData();
            __writeData(PageNumber);

            Response.Write(PageElements.PageFooter(true));
        }
        else
        {
            if (InterventionGroup)
            {
               

                Response.Write("<TABLE align='center' width='800px' cellpadding='0' cellspacing='0' style='background-color:#E8E8E8;font-family:arial;font-size:12px'><TR><TD colspan='3' style='padding-bottom:20px;padding-left:10px;background-color:#FFFFFF'>");
                Response.Write("<B><SPAN style='font-size:15px'>People struggling with similar difficulties have found helpful ways to overcome them.</SPAN></B></TD></TR>");

                Response.Write("<TR height='220px'>");
                Response.Write("<TD valign='top' width='33%' style=\"padding-top:30px;padding-left:130px;padding-right:10px;background-image:url('./graph/P3-1.png')\">You now have the option to click <A href='../dialog/frame.aspx?p=participant&p1=" + ParticipantId + "' style='font-weight:bold'>here</A> to start a conversation with an online counselor.</TD>");
                Response.Write("<TD valign='top' width='33%' style=\"padding-top:10px;padding-left:90px;padding-right:20px;background-image:url('./graph/P3-2.png')\">The counselor is a trained mental health professional  available to talk with you and explore options for help and support.</TD>");
                Response.Write("<TD valign='top' width='33%' style=\"padding-top:17px;padding-left:55px;background-image:url('./graph/P3-3.png');font-weight:bold;color:red\">The counselor will respond <BR>to you within 24 hours.</TD>");
                Response.Write("</TR>");

                Response.Write("<TR height='5px'><TD></TD></TR>");

                Response.Write("<TR height='220px'>");
                Response.Write("<TD valign='top' align='center' width='33%' style=\"padding-top:20px;background-image:url('./graph/P3-4.png')\">The email will contain a link to the private counselor dialogue page.</TD>");
                Response.Write("<TD valign='top' align='center' width='33%' style=\"padding-top:30px;background-image:url('./graph/P3-5a.png')\">Remember, this conversation is:</TD>");
                Response.Write("<TD valign='top' align='center' width='33%' style=\"padding-top:12px;background-image:url('./graph/P3-6.png')\"><A href='../dialog/frame.aspx?p=participant&p1=" + ParticipantId + "' style='font-weight:bold;font-size:17px'>Begin a Conversation with<BR>the Counselor</A></TD>");
                Response.Write("</TR>");

                Response.Write("<TR><TD colspan='3' align='center' style='padding-top:30px;padding-bottom:20px;background-color:#FFFFFF'><A href='../dialog/action.aspx?p=faq&p1=" + ParticipantId + "' style='font-weight:bold' target='_blank'>More info</A></TD></TR>");

                Response.Write("</TD></TR></TABLE>");

                
            }
            else
            {
                Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access your personalized feedback.</SPAN>");
            }
        }
    }

    private void __writeData(int PageNumber)
    {
        
        // alcohol use
        if (PageNumber == 1)
        {
            if (InterventionGroup)
            {
                Response.Write("<TABLE width='620px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD style='padding-bottom:20px'>");

                Response.Write("<TABLE width='800px' cellpadding='0' cellspacing='0' style=\"font-family:arial;font-size:12px;background-color:#E8E8E8;background-image:url('./graph/P2-1.png')\"><TR>");

                Response.Write("<TD height='217px' width='120px'></TD><TD width='115px' valign='top' style='padding-top:45px;font-weight:bold'>");

                Response.Write("Let's take a look at your alcohol use. From what you've shared on the survey, your alcohol use may" + (ALC_ASSESSMENT == null ? " not " : " ") + "be exceeding what is safe for you.");

                Response.Write("</TD><TD width='145px'></TD><TD width='130px' valign='top' style='padding-top:48px;font-weight:bold'>");

                Response.Write("Now, let's look at your responses, and how they compare to other college students.");

                Response.Write("</TD><TD width='35px'></TD><TD valign='top'><TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD valign='top' style='padding-top:15px;font-weight:bold'><TR><TD style='padding-top:0px;padding-bottom:30px;padding-left:45px;padding-right:20px;font-weight:bold'>");

                Response.Write("On a scale of 0-40, your alcohol risk score is <SPAN style='color:red'>" + Math.Floor(ALC_SCORE).ToString() + "</SPAN>, which is <SPAN style='color:red'>" + ALC_SEVERITY.ToLower() + "</SPAN>. This means you have a higher risk than <SPAN style='color:red'>" + ALC_PERCENTILE + "%</SPAN> of college students.");

                Response.Write("</TD></TR><TR><TD><IMG width='247px' src='./graph/alc/alc_" + Math.Floor(ALC_SCORE).ToString() + ".png'></TD></TR></TABLE></TD></TABLE>");

                Response.Write("<TABLE width='800px' cellpadding='0' cellspacing='0' style=\"font-family:arial;font-size:12px;background-color:#E8E8E8;background-image:url('./graph/P2-2.png')\"><TR><TR>");

                Response.Write("<TD height='217px' width='110px'></TD><TD width='135px' valign='top' style='padding-top:45px;font-weight:bold'>");

                Response.Write("In summary, your alcohol use has a <SPAN style='color:red'>" + ALC_SEVERITY.ToLower() + "</SPAN> risk of dependence and harmful effects.");

                Response.Write("</TD><TD width='130px'></TD><TD width='130px' valign='top' style='padding-top:35px;font-weight:bold'>");

                string _gv = "<DIV>Additionally, you have identified the goals and values that are important to you as: <SPAN style='color:red'>" + GOALS + "; " + VALUES + "</SPAN>.</DIV>";
                Response.Write(_gv.Replace("not provided.; not provided.", "<I>not provided</I>").Replace("not provided.;", "").Replace("; not provided.", ""));

                Response.Write("</TD><TD width='60px'></TD><TD valign='top' style='padding-top:25px;padding-right:20px;font-weight:bold'>");

                Response.Write("<SPAN style='color:red'>Here is the good news:</SPAN> people struggling with similar difficulties have been able to find helpful ways to overcome them. It seems like you have goals and values that can be helpful to you in overcoming these difficulties.");

                Response.Write("</TR></TABLE>");

                Response.Write("</TD></TR><TR height='10px'><TD>&nbsp;</TD></TR>");
                Response.Write("<TR><TD align='center' style='padding-bottom:5px;font-weight:bold;font-size:17px'><SPAN style='color:blue;cursor:hand;cursor:pointer' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='blue'\" onclick=\"window.location.href='receipt.aspx?p=" + ParticipantId + "&p1=" + (PageNumber + 1).ToString() + "'\">Next Page &gt;</SPAN></TD></TR>");

                Response.Write("</TD></TR></TABLE>");
            }
            else
            
        }


        //__debug();
    }*/

    // better to have multiple db queries in case survey design may change
    private void __readData()
    {
        string Gender = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='DEM'")[0].Split('|')[1];
        string Suicide = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0].Split('|')[2];

        PHQ9_SCORE = Logic.PHQ9Score(ParticipantId);
        PHQ9_PERCENTILE = Logic.PHQ9Percentile(PHQ9_SCORE).ToString();
        PHQ9_SEVERITY = Logic.PHQ9Severity(ParticipantId);
        PHQ_ASSESSMENT = Logic.PHQ9Assessment(ParticipantId);

        ALC_SCORE = Logic.ALCScore(ParticipantId);
        ALC_PERCENTILE = Logic.ALCPercentile(ALC_SCORE).ToString();
        ALC_SEVERITY = Logic.ALCSeverity(ParticipantId);
        ALC_ASSESSMENT = Logic.ALCAssessment(ParticipantId);

        PHQ7_HM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ9_HM'")[0].Split('|');
        
        // no longer using the hm measures
        //HM = (PHQ7_HM[6] + "|" + PHQ7_HM[7]).Split('|');

        //ACADEMIC_PERFORMANCE = Logic.CheckCode("PHQ", HM[0]);
        //SOCIAL_LIFE = Logic.CheckCode("PHQ", HM[1]);

        //Goal_Value = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='GOA_VAL'")[0].Split('|');

        int _neg_impact_counter = 0; for (int i = 0; i < PHQ7_HM.Length; i++) { if (PHQ7_HM[i] != "--" && PHQ7_HM[i] != "0") _neg_impact_counter++; }
        if (_neg_impact_counter == 0) NEGATIVE_IMPACT = "no"; else if (_neg_impact_counter > 0 && _neg_impact_counter <= 3) NEGATIVE_IMPACT = "a few"; else NEGATIVE_IMPACT = "many";

        string _hm = "";
        //below are commented out since hm measures are no longer being used

        //if ((HM[0] == "0" || HM[0] == "--") && (HM[1] == "0" || HM[1] == "--")) _hm += "Your emotional health is <B>not</B> interfering with your academic performance or social activities|";
        //if ((HM[0] != "0" && HM[0] != "--") && (HM[1] == "0" || HM[1] == "--")) _hm += "Your emotional health is interfering with your academic performance but not with your social activities|";
        //if ((HM[0] == "0" || HM[0] == "--") && (HM[1] != "0" && HM[1] != "--")) _hm += "Your emotional health is <B>not</B> interfering with your academic performance but with your social activities|";
        //if ((HM[0] != "0" && HM[0] != "--") && (HM[1] != "0" && HM[1] != "--")) _hm += "Your emotional health is interfering with your academic performance and your social activities|";
        if (_hm.Length != 0) HEALTH_MIND = _hm.Substring(0, _hm.Length - 1).Split('|');
        else HEALTH_MIND = null;

        //if (Goal_Value[0] == "--") Goal_Value[0] = "Not provided."; if (Goal_Value[1] == "--") Goal_Value[1] = "Not provided.";

        //GOALS = Goal_Value[0].ToLower().Replace(",", ", ");
        //VALUES = Goal_Value[1].ToLower().Replace(",", ", ");
    }

    private void __debug()
    {
        PageElements.EligibilityReasoning(ParticipantId, "Intervention");

        Response.Write(PageElements.PrintDebug(ParticipantId, File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\q.txt")));
    }

    //logs specific javascript events to activity log in database, called from javascript functions
    /*[WebMethod]
    public static void log_receipt_actions(string action)
    {
        string participant_id = (string)HttpContext.Current.Session["PARTICIPANT_ID"];
        if (action == "concerns_feedback")
        {
            Utility.LogActivity(participant_id, "CLICKED MORE ABOUT CONCERNS/FEEDBACK", "None");
        }
        else if (action == "available_resources")
        {
            Utility.LogActivity(participant_id, "CLICKED MORE ABOUT AVAILABLE RESOURCES", "None");
        }
        else if (action == "other")
        {
            Utility.LogActivity(participant_id, "CLICKED OTHER", "None");
        }
        else if (action == "distress")
        {
            Utility.LogActivity(participant_id, "CLICKED DISTRESS LEVEL", "None");
        }
        else if (action == "alcohol")
        {
            Utility.LogActivity(participant_id, "CLICKED ALCOHOL", "None");
        }
    }*/

}