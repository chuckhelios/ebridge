using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;

public class PageElements
{

    public static string EmailSignature()
    {
        return "\r\n\n(This is an automatically generated email message. Please do not reply to the sender.)\r\n\nSincerely yours,\r\n\nThe eBridge Team";
    }

    public static string ContactInfo(string ParticipantId)
    {
        string school_data = Helper.ContactData(ParticipantId);

        return school_data;
    }

    public static string LinkSpan()
    {
        return "style='padding-top:2px;padding-bottom:2px;cursor:hand;cursor:pointer;text-decoration:underline' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='black'\"";
    }

    public static string JSHeader()
    {
        return "<SCRIPT type='text/javascript'></SCRIPT>";
    }

    public static string PageHeader(string Style) { return PageHeader(Style, ""); }

    public static string PageHeader(string Style, string Onload) { return PageHeader(Style, Onload, ""); }

    public static string PageHeader(string Style, string OnLoad, string SchoolCode) // style: "Plain", "Full", "Logo Only", "InitialPage", "ConsentPage"
    {
        OnLoad = " onload = " + OnLoad;

        string _ref = 
        @"<link rel='stylesheet' href='//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css'>";

        string _o = "<HTML><HEAD><TITLE>eBridge - A Journey for Your Well Being</TITLE>"
                    + "<script src='//ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js'></script>"
                    + _ref
                    + "</HEAD><BODY style='margin:0px;padding:0px'" + OnLoad + ">"
                    + "<TABLE width='100%' height='100%' cellpadding='0px' cellspacing='0px'><TR><TD style='padding-top:20px;padding-bottom:40px;background-color:#454545' align='center'>"
                    + "<TABLE width='700px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#FFFFFF'>";
        
        if (SchoolCode == "i" && Style =="InitialPage") _o += "<TR><TD style='background-color:rgb(69, 69, 69)'>"+PageElements.ContactInfo(SchoolCode).Replace("_EMERGENCY_RESOURCES_", Helper.EmergencyData(SchoolCode))+"</TD></TR>";
        if (SchoolCode == "i" && Style =="ConsentPage") _o += "<TR><TD style='background-color:rgb(69, 69, 69)'>"+PageElements.ContactInfo(SchoolCode).Replace("_EMERGENCY_RESOURCES_", "")+"</TD></TR>";
        if (Style != "Plain") _o += "<TR><TD align='center' style='padding-top:40px;padding-bottom:20px'><IMG src='../image/mental.png' width='344px' height='95px'></TD></TR>";
        if (Style == "Full") _o += "<TR><TD align='center' style='padding-bottom:40px'><IMG src='../image/journey.png' width='504px'></TD></TR>";
        if (Style == "InitialPage") _o += "<TR><TD align='center''><IMG src='../image/start_survey.jpg' width='700px'border='0' style='cursor:hand;cursor:pointer' onclick=\"window.location.href='consent.aspx'\"></TD></TR>";
        if (Style != "InitialPage") _o += "<TR><TD style='padding-left:40px;padding-right:40px;padding-bottom:30px" + (Style == "Plain" ? ";padding-top:30px" : "") + "'>";

        return _o;
    }

    public static string PageFooter(string Style) { return PageFooter(Style, "");}

    public static string PageFooter(string Style, string SchoolCode) //"InitialPage", "ConsentPage", "Plain"
    {
        string _o = "</TD></TR>";
        if (Style == "Plain" || Style == null) return _o + "</TABLE></TD></TR></TABLE></BODY></HTML>";    
        if (SchoolCode != "i" ) { 
            _o += "<TR><TD style='background-color:rgb(69, 69, 69)'>" + PageElements.ContactInfo(SchoolCode) + "</TD></TR>";
        }
        else
        {
            if (Style != "InitialPage" && Style != "ConsentPage") _o += "<TR><TD style='background-color:rgb(69, 69, 69)'>" + PageElements.ContactInfo(SchoolCode).Replace("_EMERGENCY_RESOURCES_", "") + "</TD></TR>";
        }
        _o += "</TABLE></TD></TR></TABLE></BODY></HTML>";        
        return _o;
    }

    public static string DialogPanelHeader()
    {
        string _o = "<HTML><HEAD><TITLE>eBridge - A Journey for Your Well Being</TITLE>" + JSHeader() + "</HEAD><BODY style='margin:0px;padding:0px'>"
                    + "<TABLE width='100%' height='100%' cellpadding='0px' cellspacing='0px'><TR><TD style='padding-top:20px;padding-bottom:40px;background-color:#454545' align='center' valign='middle'>"
                    + "<TABLE width='700px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#FFFFFF'>";

        _o += "<TR><TD align='center' style='padding-top:30px;padding-bottom:10px;background-color:#D3D3D3;font-weight:bold'><I>e</I><SPAN style='font-size:1px'> </SPAN>Bridge: A Journey for Your Well Being</TD></TR>";

        return _o + "<TR><TD style='padding-left:40px;padding-right:40px;padding-bottom:30px'>";
    }

    public static string ThreadPageHeader(string ParticipantId, string View)
    {
        string _js = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_ebridge_header.txt");

        string _co ="<SCRIPT type='text/javascript'>"
                + "function loadXMLDoc(url,objectid)"
                + "{"
                + "if (window.XMLHttpRequest) xmlhttp=new XMLHttpRequest();"
                + "else xmlhttp=new ActiveXObject('Microsoft.XMLHTTP');"
                + "xmlhttp.open('GET',url,true);"
                + "xmlhttp.send(null);"
                + "xmlhttp.onreadystatechange = function() {if(xmlhttp.readyState == 4) document.getElementById(objectid).innerHTML=xmlhttp.responseText;}"
                + "}"
                + "document.observe('lightview:hidden', function() {"
                + "window.location.reload();"
                + "loadXMLDoc('PageElementFeed.aspx?p1=refresh&p2='+document.getElementById('CURRENT_ID').value,document.getElementById('CURRENT_ID').value);"
                + "})"
                + "</SCRIPT>";

        string _ti ="<SCRIPT type='text/javascript'>"
                +"function __startReresh(){messageInterval = setInterval(\"loadXMLDoc( 'counselors_online.aspx?p="+ParticipantId+"' , 'counselors');\", 10000);}"//;window.scroll(0,document.body.scrollHeight)
                +"__startReresh()"
                +"</SCRIPT>";

        string _o = "<HTML><HEAD><TITLE>eBridge - A Journey for Your Well Being</TITLE>" + _co + _js + _ti + JSHeader()
                    + "</HEAD><BODY class='yui-skin-sam'>"
                    + "<TABLE width='100%' height='100%' cellpadding='0px' cellspacing='0px'><TR><TD style='padding-top:20px;padding-bottom:40px;background-color:#454545;padding-left:50px' valign='top'>"
                    + "<TABLE width='700px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#FFFFFF'>";

        //int StudentPost = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + ParticipantId + "'");

        if (View == "Student")
        {
            _o += "<TR height='195px'><TD valign='bottom' style='background-color:rgb(69, 69, 69)'><TABLE width='250px' cellpadding='0px' cellspacing='0px' style='background-color:#D3D3D3;font-size:12px'><TR><TD style='padding:10px'>Please note that the eBridge website is not monitored 24/7. However, the professional counselor will respond to your message within 24 hours.</TD></TR></TABLE>"
               + "</TD><TD valign='top' style='padding-left:40px;padding-right:40px;padding-top:30px;'>"
               + "<IMG style='width:100% ; height:auto;' src='../image/ebridgenewblue.jpg'></IMG>" 
               + "</TD></TR>";
        }

        else _o += "<TR><TD style='background-color:rgb(69, 69, 69)'></TD><TD align='center' style='padding-top:40px;padding-bottom:10px;color:red;font-weight:bold'>Counselor View (With: " + ParticipantId + ")</TD></TR>";

        _o += "<TR><TD valign='top' style='background-color:rgb(69, 69, 69);padding-right:20px'>";

        string _style = " style='padding-left:15px;padding-right:15px;padding-top:3px;padding-bottom:3px;cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"";

        string GOAL = "n/a", VALUE = "n/a";
        string[] _gv = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='GOA_VAL'");

        if (_gv != null)
        {
            GOAL = _gv[0].Split('|')[0].Replace(",", ", ").Replace(".", "n/a");
            VALUE = _gv[0].Split('|')[1].Replace(",", ", ").Replace(".", "n/a");
        }

        //string _schedule_chat = "I would like to schedule a private session to chat with the Counselor online. Below is my availability:"
        //        + "\\n\\n[Please provide here dates and times that work best for you.]";

        // (The Counselor will get in touch with you as soon as possible to confirm an appointment. Instructions of how to use the chat website will be sent to you by email.)

        string _student = "<TABLE width='250px' cellpadding='0px' cellspacing='0px' style='background-color:#FFFFFF;font-size:12px'>"
            + "<TR height='20px'><TD style='background-color:rgb(69, 69, 69)'></TD></TR>"
            + "<TR><TD style='background-color:#D3D3D3;padding-top:10px;padding-left:15px;padding-bottom:4px;font-weight:bold'>Tools</TD></TR><TR height='10px'><TD></TD></TR>"
            + "<TR height='5px'><TD></TD></TR>"
            + "<TR><TD" + _style + " onclick=\"window.location.href='password.aspx?p=" + ParticipantId + "'\"><B style='color:red'>Change Password</B></TD></TR>"
            + "<TR height='10px'><TD></TD></TR><TR><TD" + _style + "><A href='../survey/receipt.aspx?p=" + ParticipantId + "&p1=0' target='_blank' style='color:blue'>My Personalized Feedback</A></TD></TR>"
            + "<TR height='5px'><TD></TD></TR>"
            + "<TR><TD" + _style + ">"
            + Helper.GetChatUrl(ParticipantId, Utility.GetStatus(ParticipantId, "COUNSELOR"))
            + "</TD></TR>"
            + "<TR height='5px'><TD></TD></TR>"
            + Helper.ChatTimes("style='padding: 3px 15px; background-color: white;'", ParticipantId);

        /*
        string[][] _s = Db.GetRecords("SELECT datetime_start, unique_id FROM schedule WHERE participant_id = '" + ParticipantId + "' AND datetime_start > #" + DateTime.Now.AddHours(-1) + "# ORDER BY datetime_start");

        if (_s != null)
        {
            _student += "<TR height='20px'><TD></TD></TR><TR><TD style='padding-left:15px;padding-right:15px'>Your Chat Appointment" + (_s.Length > 1 ? "s" : "") + ":<P>";

            for (int i = 0; i < _s.Length; i++)
            {
                DateTime _d = DateTime.Parse(_s[i][0]);
                _student += "&nbsp;&nbsp;&nbsp;&#149;&nbsp;&nbsp;" + _d.DayOfWeek + ", " + _d.ToShortDateString() + ", " + _d.ToShortTimeString() + "<BR>";
            }

            _student += "</TD></TR>";
        }
        else
        {
            _student += "<TR height='20px'><TD></TD></TR><TR><TD style='padding-left:15px;padding-right:15px'>Your Chat Appointment:<P>&nbsp;&nbsp;&nbsp;&#149;&nbsp;&nbsp;none scheduled</TD></TR>";
        }
        */

        //+ "<TR height='3px'><TD></TD></TR><TR><TR><TD" + _style + "><A href='action.aspx?p=faq&p1=" + ParticipantId + "' target='_blank' style='color:blue'>Frequently Asked Questions (FAQs)</A></TD></TR>"
        //+ "<TR height='3px'><TD></TD></TR><TR><TR><TD" + _style + "><SPAN onclick=\"window.Lightview.show({ href: '../image/screenshot.png', rel: 'image', title: 'Tutorial: How to use <I>e</I>Bridge', options: {" + "autosize: true" + "}});\" target='_blank' style='color:blue;text-decoration:underline'>Tutorial</SPAN></TD></TR>"
        //_student += "<TR height='25px'><TD></TD></TR>"
        //    + "<TR><TD style='padding-left:15px;padding-right:15px;padding-bottom:10px'><SPAN style='color:red'>Your goal(s) are:</SPAN> " + GOAL + "</TD></TR>"
        //    + "<TR><TD style='padding-left:15px;padding-right:15px'><SPAN style='color:red'>Your value(s) are:</SPAN> " + VALUE + "</TD></TR>";

        string contact_data=Helper.ContactDataOther(ParticipantId);
        
        _student += "<TR height='40px'><TD></TD></TR>"
            + "<TR><TD style='background-color:rgb(69, 69, 69);color:#FFFFFF;padding-left:10px;padding-top:40px;font-size:11px'>Campus Mental Health Services:"
            + contact_data
            + "</TD></TR>"
            + "</TABLE>";

        string _counselor = "<TABLE width='260px' cellpadding='0px' cellspacing='0px' style='background-color:#FFFFFF'><TR><TD style='background-color:#D3D3D3;padding-top:10px;padding-left:15px;padding-bottom:4px;font-weight:bold'>Tools</TD></TR>"
            + "<TR height='15px'><TD></TD></TR>"
            + "<TR><TD" + _style + " onclick=\"document.getElementById('MSG').value='_MI_SLIDER_|" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_text\\readiness.txt") + "'\">Build Readiness Slider</TD></TR>"
            + "<TR><TD" + _style + " onclick=\"document.getElementById('MSG').value='_MI_SLIDER_|" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_text\\importance.txt") + "'\">Build Importance Slider</TD></TR>"
            + "<TR><TD" + _style + " onclick=\"document.getElementById('MSG').value='_MI_SLIDER_|" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_text\\confidence.txt") + "'\">Build Confidence Slider</TD></TR>"
            + "<TR><TD" + _style + " onclick=\"document.getElementById('MSG').value='_MI_SLIDER_|" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_text\\commitment.txt") + "'\">Build Commitment Slider</TD></TR>"
            + "<TR><TD" + _style + " onclick=\"document.getElementById('MSG').value='" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_text\\slider_response.txt") + "'\">Build Slider Response</TD></TR>"
            + "<TR height='10px'><TD></TD></TR><TR><TD" + _style + "><A href='./transcript.aspx' target='_blank' style='color:blue'>Generate Transcript</A></TD></TR>"
            + "<TR><TD" + _style + "><A href='./email.aspx?p=" + ParticipantId + "' target='_blank' style='color:blue'>Send Email</A></TD></TR>"
            + "<TR><TD" + _style + ">"
            + "<A href='list.aspx' style='color:blue'>Participant List</A>"
            + "</TD></TR>"
            + "<TR><TD" + _style + ">"
            + "<A href='change_hours.aspx' style='color:blue'>Change Hours</A>"
            + "</TD></TR>"
            + "<TR><TD" + _style + ">"
            + "<A href='action.aspx?p=counselor_logout' style='color:blue'>Log Out</A>"
            + "</TD></TR>"
            + "<TR height='30px'><TD></TD></TR><TR height='20px'><TD style='background-color:rgb(69, 69, 69)'></TD></TR><TR><TD style='padding-top:20px;padding-bottom:20px'>" + PrintValue(ParticipantId) + "</TD></TR><TR height='50px'><TD style='background-color:rgb(69, 69, 69)'></TD></TR></TABLE>";
        if (View == "Student") _o += _student; else _o += _counselor;

        _o += "</TD><TD valign='top' style='padding-left:40px;padding-right:40px;padding-bottom:30px'>";

        return _o;
    }

    public static string ThreadPageFooter()
    {
        return "</TD></TR>"

            + "</TABLE></TD></TR></TABLE></BODY></HTML>" + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_ebridge.txt");
    }

    public static string FormateDateTime(string Input)
    {
        return Input.Split(':')[0] + ":" + Input.Split(':')[1] + " " + Input.Split(':')[2].Split(' ')[1].ToLower();
    }

    public static string DisplayThread(string[][] Messages, string View)
    {
        string _o = "";

        if (Messages == null)
        {
            Messages = new string[1][];
            Messages[0] = new string[6]; for (int i = 0; i < Messages[0].Length; i++) Messages[0][i] = "";
            Messages[0][3] = "The dialog is empty."; Messages[0][2] = "Counselor"; Messages[0][4] = DateTime.Now.ToString(); Messages[0][5] = "NULL";
        }

        for (int i = 0; i < Messages.Length; i++)
        {
            // counselor
            if (Messages[i][2] != "NULL")
            {
                _o += "<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>"
                    + "<TR><TD width='120px'>&nbsp;</TD><TD align='right' width='380px'>"
                    + FormatMessage(Messages[i], "Counselor", View) + "<SPAN style='padding-right:5px;font-size:10px;color:#888888'>" + PageElements.FormateDateTime(Messages[i][4]) + "</SPAN>"
                    + "</TD><TD valign='bottom' style='padding-left:5px;padding-bottom:6px'><IMG src='message_image/lightbulb.png'></TD></TR>"
                    + "<TR height='12px'><TD></TD></TR>"
                    + "</TABLE>";
            }
            // student
            else
            {
                _o += "<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>"
                   + "<TR><TD align='left' width='380px'>"
                   + FormatMessage(Messages[i], "Student", View) + "<SPAN style='padding-left:5px;font-size:10px;color:#888888'>" + PageElements.FormateDateTime(Messages[i][4]) + "</SPAN>"
                   + "</TD><TD width='120px'>&nbsp;</TD></TR>"
                   + "<TR height='12px'><TD></TD></TR>"
                   + "</TABLE>";
            }
        }

        return _o;
    }

    public static string FormatMessage(string[] Message, string Type, string View)
    {
        string _color = "blue"; if (Type == "Student") _color = "green";

        string MessageBody = HttpUtility.HtmlDecode(Message[3]).Replace("\r\n", "<BR>").Replace("[CHAT] ", "<SPAN style='color:green;font-style:italic'>chat</SPAN>&nbsp;&nbsp;&nbsp;");
        string MessageId = Message[0];
        string ParticipantId = Message[2];
        string RespondingTo = Message[5];

        MessageBody = GetMITool(MessageId, MessageBody, ParticipantId, View);

        return "<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>"
            + "<TR><TD><IMG src='message_image/" + _color + "_t_l.png'></TD><TD style='background:url(message_image/" + _color + "_t.png)'></TD><TD><IMG src='message_image/" + _color + "_t_r.png'></TD></TR>"
            + "<TR><TD style='background:url(message_image/" + _color + "_l.png)'></TD>"
            + "<TD style='background:url(message_image/" + _color + "_bk.png);padding-left:5px;padding-right:7px;padding-top:1px;cursor:default'>" + (RespondingTo != "NULL" ? "Responding to a posted question: <B>" + MessageBody + "</B>" : MessageBody) + "</TD><TD style='background:url(message_image/" + _color + "_r.png)'></TD></TR>"
            + "<TR><TD><IMG src='message_image/" + _color + "_b_l.png'></TD><TD style='background:url(message_image/" + _color + "_b.png)'></TD><TD><IMG src='message_image/" + _color + "_b_r.png'></TD></TR>"
            + "</TABLE>";
    }

    public static string GetMITool(string MessageId, string Message, string ParticipantId, string View)
    {
        string _o = "";

        if (Message.IndexOf('|') == -1) return Message;

        try
        {
            if (Message.Split('|')[0] == "_MI_SLIDER_")
            {
                string[] Label = Message.Split('|')[2].Split(',');
                string[] _response = Db.GetRecord("SELECT message_body FROM message WHERE responding_to = " + MessageId);

                _o = "<TABLE width='282px' cellpadding='0px' cellspacing='0px'>"
                    + "<TR height='12px'><TD><FORM id='" + MessageId + "' method='post' action='action.aspx?p=student_post'><INPUT name='PID_SLIDER' type='hidden' value='" + ParticipantId + "'></TD></TR>"
                    + "<TR><TD colspan='2' style='padding-bottom:12px;font-weight:bold'>" + Message.Split('|')[1] + "</TD></TR>"
                    + "<TR><TD width='209px'><DIV id='slider-bg' class='yui-h-slider' tabindex='-1' title='Slider'><DIV id='slider-thumb' class='yui-slider-thumb'><IMG src='slider/thumb-n.gif'></DIV></DIV><SPAN id='slider-value' style='display:none'>0</SPAN></TD><TD><INPUT name='V' " + (View == "Counselor" || _response != null ? "disabled " : "") + "id='slider-converted-value' type='text' value='0' size='4' maxlength='4' style='text-align:center'></TD></TR>"
                    + "<TR><TD>"
                    + "<TABLE width='214px' cellpadding='0px' cellspacing='0px' style='font-size:11px'><TD width='33%' style='padding-left:3px'>" + Label[0] + "</TD><TD width='33%' align='center'>" + Label[1] + "</TD><TD width='33%' align='right'>" + Label[2] + "</TD></TR></TABLE></TD><TD></TD></TR>"
                    + "<TR><TD align='right' colspan='2' style='padding-top:10px'><INPUT name='RID' type='hidden' value='" + MessageId + "'>";

                if (_response == null) _o += "<TEXTAREA name='RESPONSE' style='width:280px;height:50px;font-family:verdana;font-size:11px'" + (View == "Counselor" ? " disabled" : "") + "></TEXTAREA>";

                _o += "</TD></TR>";

                if (View == "Counselor")
                {
                    if (_response != null) _o += "<TR><TD colspan='2' style='padding-top:10px;font-size:11px;color:red'>Student response: " + _response[0];
                    else _o += "<TR><TD align='right' colspan='2' style='padding-top:2px'><INPUT disabled type='button' value='Post' onclick=\"document.getElementById('" + MessageId + "').submit();\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'>";
                }
                else
                {
                    if (_response != null) _o += "<TR><TD colspan='2' style='padding-top:10px;font-size:11px;color:red'>You already responded to this question.";
                    else _o += "<TR><TD align='right' colspan='2' style='padding-top:2px'><INPUT type='button' value='Post' onclick=\"document.getElementById('" + MessageId + "').submit();\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'>";
                }

                _o += "</TD></TR></TD></TR><TR height='8px'><TD></FORM></TD></TR></TABLE>";
            }

            return _o;
        }
        catch
        {
            return "MI Tool Corrupted.";
        }
    }

    public static string PrintDebug(string ParticipantId, string Types)
    {
        string _o = "";

        string[] _type = Types.Split(','); string[] _value = null;

        for (int i = 0; i < _type.Length; i++)
        {
            _value = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + _type[i] + "'")[0].Split('|');

            if (_type[i] == "DEM") _o += "<P>Age&mdash;" + Logic.CheckCode("AGE", _value[0]) + " (" + _value[0] + ")"
                                       + "<BR>Gender&mdash;" + Logic.CheckCode("GENDER", _value[1]) + " (" + _value[1] + ")"
                                       + "<BR>Race&mdash;" + _value[2].Replace(",", ", ").Replace(".", "n/a").ToLower()
                                       + "<BR>Race2&mdash;" + _value[3].Replace(",", ", ").Replace(".", "n/a").ToLower()
                                       + "<BR>Ethnicity&mdash;" + _value[4].ToLower();

            if (_type[i] == "ALC3") _o += "<P>AUDIT1&mdash;" + Logic.CheckCode("ALC1", _value[0]) + " (" + _value[0] + ")<BR>AUDIT2&mdash;" + Logic.CheckCode("ALC2", _value[1]) + " (" + _value[1] + ")<BR>AUDIT3&mdash;" + Logic.CheckCode("ALC3-8", _value[2]) + " (" + _value[2] + ")";

            if (_type[i] == "ALC7") _o += "<P>AUDIT1&mdash;" + Logic.CheckCode("ALC2", _value[0]) + " (" + _value[0] + ")<BR>AUDIT2&mdash;" + Logic.CheckCode("ALC2", _value[1]) + " (" + _value[1] + ")<BR>AUDIT3&mdash;" + Logic.CheckCode("ALC3-8", _value[2]) + " (" + _value[2] + ")";

            if (_type[i] == "DRG5")
            {
                _o += "<P>DRG1: " + Logic.CheckCode("DRG", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>DRG2: " + Logic.CheckCode("DRG", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>DRG3: " + Logic.CheckCode("DRG", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>DRG4: " + Logic.CheckCode("DRG", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>DRG5: " + Logic.CheckCode("DRG", _value[4]) + " (" + _value[4] + ")";
            }

            if (_type[i] == "PHQ2_SUI")
            {
                _o += "<P>PHQ1: " + Logic.CheckCode("PHQ", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>PHQ2: " + Logic.CheckCode("PHQ", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>PHQ9: " + Logic.CheckCode("PHQ", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>SUICIDE1: " + Logic.CheckCode("SUICIDE", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>SUICIDE2: " + Logic.CheckCode("SUICIDE", _value[4]) + " (" + _value[4] + ")"
                    + "<BR>SUICIDE3: " + Logic.CheckCode("SUICIDE", _value[5]) + " (" + _value[5] + ")"
                    + "<BR>SUICIDE4: " + Logic.CheckCode("SUICIDE", _value[6]) + " (" + _value[6] + ")"
                    + "<BR>SUICIDE5: " + Logic.CheckCode("SUICIDE", _value[7]) + " (" + _value[7] + ")"
                    + "<BR>SUICIDE6: " + Logic.CheckCode("SUICIDE", _value[8]) + " (" + _value[8] + ")"
                    + "<BR>SUICIDE7: " + _value[9];
            }


            /*if (_type[i] == "PHQ9_HM")
            {
                _o += "<P>PHQ3: " + Logic.CheckCode("PHQ", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>PHQ4: " + Logic.CheckCode("PHQ", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>PHQ5: " + Logic.CheckCode("PHQ", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>PHQ6: " + Logic.CheckCode("PHQ", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>PHQ7: " + Logic.CheckCode("PHQ", _value[4]) + " (" + _value[4] + ")"
                    + "<BR>PHQ8: " + Logic.CheckCode("PHQ", _value[5]) + " (" + _value[5] + ")";
                    //+ "<BR>HM1: " + Logic.CheckCode("HM", _value[6]) + " (" + _value[6] + ")"
                    //+ "<BR>HM2: " + Logic.CheckCode("HM", _value[7]) + " (" + _value[7] + ")";
            }*/
            if (_type[i] == "REA") _o += "<P>REA1: " + _value[0] + "<BR>REA2: " + _value[1] + "<BR>REA3: " + _value[2] + "<BR>REA4: " + _value[3] + "<BR>REA5: " + _value[4] + "<BR>REA6: " + _value[5];

            if (_type[i] == "ALC7") _o += "<P>AUDIT4: " + Logic.CheckCode("ALC3-8", _value[0]) + " (" + _value[0] + ")<BR>AUDIT5: " + Logic.CheckCode("ALC3-8", _value[1]) + " (" + _value[1] + ")<BR>AUDIT6: " + Logic.CheckCode("ALC3-8", _value[2]) + " (" + _value[2] + ")<BR>AUDIT7: " + Logic.CheckCode("ALC3-8", _value[3]) + " (" + _value[3] + ")<BR>AUDIT8: " + Logic.CheckCode("ALC3-8", _value[4]) + " (" + _value[4] + ")<BR>AUDIT9: " + Logic.CheckCode("ALC9-10", _value[5]) + " (" + _value[5] + ")<BR>AUDIT10: " + Logic.CheckCode("ALC9-10", _value[6]) + " (" + _value[6] + ")<P>AUDIT Score: " + Logic.ALCScore(ParticipantId);

            if (_type[i] == "ILL") _o += "<P>Illegal drugs&mdash;" + _value[0].Replace(",", ", ").Replace(".", "n/a");

            if (_type[i] == "SER")
            {
                _value[0] = _value[0].Trim().Trim(new char[] { ',' });
                _value[1] = _value[1].Trim().Trim(new char[] { ',' });
                //Split set of drugs
                string drug_list1 = "<P>Past 12 Months Medications: " + Logic.CheckCode("SER_LIST1", _value[0]);
                string drug_list2 = "<BR>Current medications: " + Logic.CheckCode("SER_LIST2", _value[1]);
                /*string[] values1 = _value[0].Split(',');
                for (int j = 0; j < values1.Length; j++)
                {
                    if (values1[j] != "")
                    {
                        drug_list1 += Logic.CheckCode("SER_LIST", values1[j] + ",");
                    }
                }
                string drug_list2 = "<BR>Current Medications: ";
                string[] values2 = _value[1].Split(',');
                for (int j = 0; j < values2.Length; j++)
                {
                    if (values2[j] != "")
                    {
                        drug_list1 += Logic.CheckCode("SER_LIST", values2[j] + ",");
                    }
                }
                */
                _o += drug_list1 + drug_list2;
                _o += "<BR>Counseling or therapy in past 12 months: " + Logic.CheckCode("SER2", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>Current use of counseling or therapy: " + Logic.CheckCode("SER2A", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>Hospitalized in past 12 months: " + Logic.CheckCode("SER3", _value[4]) + " (" + _value[4] + ")";
                    // wonder if we still have the one below, if sessions in past 12 months has been removed
                    //+ "<BR>Sessions in past 12 months: " + Logic.CheckCode("SER2B", _value[4]) + " (" + _value[4] + ")"
            }
            if (_type[i] == "PHQ9_HM")
            {
                _o += "<P>Trouble falling asleep, sleeping too much: " + Logic.CheckCode("PHQ", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>Feeling tired: " + Logic.CheckCode("PHQ", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>Poor Appetite or overeating: " + Logic.CheckCode("PHQ", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>Feeling bad about yourself: " + Logic.CheckCode("PHQ", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>Trouble concentrating: " + Logic.CheckCode("PHQ", _value[4]) + " (" + _value[4] + ")"
                    + "<BR>Moving or speaking slowly: " + Logic.CheckCode("PHQ", _value[5]) + " (" + _value[5] + ")";
            }

            if (_type[i] == "PAI")
            {
                _o += "<P>Any pain in last year: " + Logic.CheckCode("PAI1", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>Type of pain: " + Logic.CheckCode("PAI1A", _value[1]) + " (" + _value[1] + ")";
            }

            if (_type[i] == "PNS")
            {
                _o += "<P>Joyful: " + Logic.CheckCode("PNS", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>Cheerful: " + Logic.CheckCode("PNS", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>Happy: " + Logic.CheckCode("PNS", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>Lively: " + Logic.CheckCode("PNS", _value[3]) + " (" + _value[3] + ")"
                    + "<BR>Proud: " + Logic.CheckCode("PNS", _value[4]) + " (" + _value[4] + ")";
            }

            if (_type[i] == "IBS")
            {
                _o += "<P>When I feel rejected, I will often say things that I wish I hadn't: " + Logic.CheckCode("IBS", _value[0]) + " (" + _value[0] + ")"
                    + "<BR>It's hard for me not to act on my feelings: " + Logic.CheckCode("IBS", _value[1]) + " (" + _value[1] + ")"
                    + "<BR>I often make matters worse because...: " + Logic.CheckCode("IBS", _value[2]) + " (" + _value[2] + ")"
                    + "<BR>Sometimes I do impulsive things...: " + Logic.CheckCode("IBS", _value[3]) + " (" + _value[3] + ")";
            }

            if (_type[i] == "PHQ10")
            {
                _o += "<P>How difficult has this made it for you...: " + Logic.CheckCode("PHQ10", _value[0]) + " (" + _value[0] + ")";
            }

            if (_type[i] == "NSSI")
            {
                _o += "<P>Harmed yourself in past 12 months: " + Logic.CheckCode("NSSI1", _value[0]) + " (" + _value[0] + ")";
                _value[1] = _value[1].Trim().Trim(new char[] { ',' });
                string harm_list = "<BR>Types of harm: " + Logic.CheckCode("NSSI2", _value[1]);
                _o += harm_list;
            }

            if (_type[i] == "HMSP")
            {
                _o += "<P>Most people would willingly accept...: " + Logic.CheckCode("HMSP1", _value[0]) + " (" + _value[0] + ")";
                _o += "<BR>Most people feel that receiving...: " + Logic.CheckCode("HMSP1", _value[1]) + " (" + _value[1] + ")";
                _o += "<BR>Most people would think less of...: " + Logic.CheckCode("HMSP1", _value[2]) + " (" + _value[2] + ")";
                _o += "<BR>How helpful do you think therapy...: " + Logic.CheckCode("HMSP2", _value[3]) + " (" + _value[3] + ")";
                _o += "<BR>How helpful do you think medication...: " + Logic.CheckCode("HMSP2", _value[4]) + " (" + _value[4] + ")";
            }
            
            if (_type[i] == "HMSB")
            {  
                _value[0] = _value[0].Trim().Trim(new char[] { ',' });
                string barriers_list = "<P>Barriers: " + Logic.CheckCode("HMSB", _value[1]);
                _o += barriers_list;
            }

            if (_type[i] == "GOA_VAL") _o += "<P>Goals&mdash;" + _value[0].Replace(",", ", ").Replace(".", "n/a") + "<BR>Values&mdash;" + _value[1].Replace(",", ", ").Replace(".", "n/a");
        }

        return "<DIV style='padding-left:15px;padding-bottom:10px'>Debug Info: " + _o + "</DIV>";
    }

    public static string PrintDebug2(string ParticipantId, string Types)
    // for follow-up survey
    {
        string _o = "";

        string[] _type = Types.Split(','); string[] _value = null;

        for (int i = 0; i < _type.Length; i++)
        {
            _value = Db.GetRecord("SELECT response FROM followup_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + _type[i] + "'")[0].Split('|');

            if (_type[i] == "DEM") _o += "<P>Age&mdash;" + Logic.CheckCode("AGE", _value[0]) + " (" + _value[0] + ")";

            if (_type[i] == "SER") _o += "<P>Needed help for emotional or mental health problems: " + Logic.CheckCode("FL_SER1", _value[0]) + " (" + _value[0] + ")<BR>Use of medications in past 2 months: " + Logic.CheckCode("FL_SER2", _value[1]) + " (" + _value[1] + ")"
                + "<BR>Frequeny of medication use: " + Logic.CheckCode("FL_SER2A", _value[2]) + " (" + _value[2] + ")<BR>Use of counseling or therapy in past 2 months: " + Logic.CheckCode("FL_SER3", _value[3]) + " (" + _value[3] + ")"
                + "<BR>Frequency of use of counseling or therapy: " + Logic.CheckCode("FL_SER3A", _value[4]) + " (" + _value[4] + ")<BR>Counseling or therapy history: " + _value[5] + "<BR>Source of helpful information: " + _value[6];

            if (_type[i] == "KB") _o += "<P>Know where to seek professional help: " + Logic.CheckCode("KB1", _value[0]) + " (" + _value[0] + ")<BR>Helpfulness of therapy or counseling: " + Logic.CheckCode("KB2-3", _value[1]) + " (" + _value[1] + ")"
                + "<BR>Helpfulness of medication: " + Logic.CheckCode("KB2-3", _value[2]) + " (" + _value[2] + ")<BR>Most people would accept someone who has received mental health treatment as a close friend: " + Logic.CheckCode("KB4-7", _value[3]) + " (" + _value[3] + ")"
                + "<BR>I would accept someone who has received mental health treatment as a close friend: " + Logic.CheckCode("KB4-7", _value[4]) + " (" + _value[4] + ")<BR>Most people feel that receiving mental health treatment is a sign of personal failure: " + Logic.CheckCode("KB4-7", _value[5]) + " (" + _value[5] + ")<BR>I feel that receiving mental health treatment is a sign of personal failure: " + Logic.CheckCode("KB4-7", _value[6]) + " (" + _value[6] + ")" + "<P>Needed help for emotional or mental health problems: " + Logic.CheckCode("FL_SER1", _value[0]) + " (" + _value[0] + ")<BR>Use of medications in past 2 months: " + Logic.CheckCode("FL_SER2", _value[1]) + " (" + _value[1] + ")"
                + "<BR>Frequeny of medication use: " + Logic.CheckCode("FL_SER2A", _value[2]) + " (" + _value[2] + ")<BR>Use of counseling or therapy in past 2 months: " + Logic.CheckCode("FL_SER3", _value[3]) + " (" + _value[3] + ")"
                + "<BR>Frequency of use of counseling or therapy: " + Logic.CheckCode("FL_SER3A", _value[4]) + " (" + _value[4] + ")<BR>Counseling or therapy history: " + _value[5] + "<BR>Source of helpful information: " + _value[6]; ;

            if (_type[i] == "OQ1") _o += "<P>Study group: <SPAN style='color:red'>" + (Utility.GetStatus(ParticipantId, "INTERVENTION") == "0" ? "Control" : "Intervention") + "</SPAN><P>Open-ended Question #1: " + _value[0];

            if (_type[i] == "OQ2") _o += "<P>Open-ended Question #2: " + _value[0];
        }

        return "<DIV style='padding-left:15px;padding-bottom:10px'>Debug Info: " + _o + "</DIV>";
    }

    public static string EligibilityReasoning(string ParticipantId, string FullSruveyOrIntervention)
    {
        string SurveyStarted = Utility.GetLog(ParticipantId, "SURVEY STARTED");
        string SurveyCompleted = Utility.GetLog(ParticipantId, "SURVEY COMPLETED");

        string _o = "<DIV style='padding-top:10px;padding-bottom:10px'>Debug Info:<P>"
                + "Survey started: " + SurveyStarted + "<BR>"
                + "Survey completed: " + SurveyCompleted + "<BR>"
                + "Duration: " + DateTime.Parse(SurveyCompleted).Subtract(DateTime.Parse(SurveyStarted)) + "<P>";

        _o += "Eligibility for " + FullSruveyOrIntervention.ToLower() + ": <SPAN style='color:red'>" + (FullSruveyOrIntervention == "Intervention" ? Logic.InterventionEligibility(ParticipantId).ToString() : Logic.FullSurveyEligibility(ParticipantId).ToString()) + "</SPAN>&mdash;AUDIT Positive: <SPAN style='color:red'>" + Logic.AlcPositive(ParticipantId) + "</SPAN> (" + Logic.ALCScore(ParticipantId) + "), PHQ2 Positive: <SPAN style='color:red'>" + Logic.PHQ2Positive(ParticipantId) + "</SPAN> (" + Logic.PHQ2Score(ParticipantId) + "), PHQ 9th Question: <SPAN style='color:red'>" + Logic.PHQ9_Suicide(ParticipantId) + "</SPAN>, Suicidal attempt: <SPAN style='color:red'>" + Logic.SuicideLifeTimeAttempt(ParticipantId) + "</SPAN>";

        if (FullSruveyOrIntervention == "Intervention")
        {
            string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER'")[0].Split('|');

            bool CurrentlyTakingPrescribedMedication = false; if (_ser[0] == "0") CurrentlyTakingPrescribedMedication = true;
            bool CurrentlyReceivingCounselingOrTherapy = false; if (_ser[1] == "0") CurrentlyReceivingCounselingOrTherapy = true;

            _o += ", Current use of medication: <SPAN style='color:red'>" + CurrentlyTakingPrescribedMedication + "</SPAN>, Current use of counseling or therapy: <SPAN style='color:red'>" + CurrentlyReceivingCounselingOrTherapy + "</SPAN>)";
        }

        return _o + "</DIV>";
    }

    public static string PrintValue(string ParticipantId)
    {
        //string _style = " style='cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"";

        string _o = "<P>"
            + "<P>Depression risk&mdash;<SPAN style='color:red'>" + Logic.PHQ9Severity(ParticipantId) + "</SPAN> (" + Math.Round(Logic.PHQ9Score(ParticipantId), 1) + ")";

        _o += "<P>Alcohol risk&mdash;<SPAN style='color:red'>" + Logic.ALCSeverity(ParticipantId) + "</SPAN> (" + Math.Round(Logic.ALCScore(ParticipantId), 1) + ")";

        string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER'")[0].Split('|');

        //bool CurrentlyTakingPrescribedMedication = false; if (_ser[0] == "0") CurrentlyTakingPrescribedMedication = true;
        //bool CurrentlyReceivingCounselingOrTherapy = false; if (_ser[1] == "0") CurrentlyReceivingCounselingOrTherapy = true;

        _o += "<P><SPAN " + DrawMessage("Criteria", ParticipantId) + ">Eligibility for intervention</SPAN>: <SPAN style='color:red'>" + Logic.InterventionEligibility(ParticipantId).ToString() + "</SPAN><P>";

        string[] _type = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\q.txt").Split(','); string[] _value = null, _v = null;

        for (int i = 0; i < _type.Length; i++)
        {
            _v = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + _type[i] + "'");

            if (_v != null) 
            {
                _value = _v[0].Split('|');

                switch(_type[i])
                { 
                    case "DEM": 
                        _o += "<P>Age&mdash;" + Logic.CheckCode("AGE", _value[0]) + "<BR>Gender&mdash;" + Logic.CheckCode("GENDER", _value[1]) + "<BR>Race&mdash;" + _value[2].Replace(",", ", ").Replace(".", "n/a").ToLower();
                        break;
                    case "PHQ2_SUI": 
                        _o += "<P><SPAN " + DrawMessage("PHQ1") + ">PHQ1</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[0]) + "<BR><SPAN " + DrawMessage("PHQ2") + ">PHQ2</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[1]) + "<BR><SPAN " + DrawMessage("PHQ9") + ">PHQ9</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[2]) + "<BR>Suicidal attempt&mdash;" + (Logic.SuicideLifeTimeAttempt(ParticipantId) ? "yes" : "no");
                        break;
                    case "PHQ9_HM":
                         _o += "<P><SPAN " + DrawMessage("PHQ3") + ">PHQ3</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[0]) + "<BR><SPAN " + DrawMessage("PHQ4") + ">PHQ4</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[1]) + "<BR><SPAN " + DrawMessage("PHQ5") + ">PHQ5</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[2]) + "<BR><SPAN " + DrawMessage("PHQ6") + ">PHQ6</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[3]) + "<BR><SPAN " + DrawMessage("PHQ7") + ">PHQ7</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[4]) + "<BR><SPAN " + DrawMessage("PHQ8") + ">PHQ8</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[5]) + "<BR>";
                        break;
                    case "ALC3":
                         _o += "<P><SPAN " + DrawMessage("ALC1") + ">AUDIT1</SPAN>&mdash;" + Logic.CheckCode("ALC1", _value[0]) + "<BR><SPAN " + DrawMessage("ALC2") + ">AUDIT2</SPAN>&mdash;" + Logic.CheckCode("ALC2", _value[1]) + "<BR><SPAN " + DrawMessage("ALC3") + ">AUDIT3</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[2]);
                        break;
                    case "ALC7": 
                        _o += "<BR><SPAN " + DrawMessage("ALC4") + ">AUDIT4</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[0]) + "<BR><SPAN " + DrawMessage("ALC5") + ">AUDIT5</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[1]) + "<BR><SPAN " + DrawMessage("ALC6") + ">AUDIT6</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[2]) + "<BR><SPAN " + DrawMessage("ALC7") + ">AUDIT7</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[3]) + "<BR><SPAN " + DrawMessage("ALC8") + ">AUDIT8</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[4]) + "<BR><SPAN " + DrawMessage("ALC9") + ">AUDIT9</SPAN>&mdash;" + Logic.CheckCode("ALC9-10", _value[5]) + "<BR><SPAN " + DrawMessage("ALC10") + ">AUDIT10</SPAN>&mdash;" + Logic.CheckCode("ALC9-10", _value[6]);
                        break;
                    case "REA": 
                        _o += "<P><SPAN " + DrawMessage("REA1") + ">REA1</SPAN>&mdash;" + _value[0].Replace(".", "n/a") + "<BR><SPAN " + DrawMessage("REA2") + ">REA2</SPAN>&mdash;" + _value[1].Replace(".", "n/a") + "<BR><SPAN " + DrawMessage("REA3") + ">REA3</SPAN>&mdash;" + _value[2].Replace(".", "n/a") + "<BR><SPAN " + DrawMessage("REA4") + ">REA4</SPAN>&mdash;" + _value[3].Replace(".", "n/a") + "<BR><SPAN " + DrawMessage("REA5") + ">REA5</SPAN>&mdash;" + _value[4].Replace(".", "n/a") + "<BR><SPAN " + DrawMessage("REA6") + ">REA6</SPAN>&mdash;" + _value[5].Replace(".", "n/a");
                        break;
                    case "SER":
                        //_o += "<P>Use of medication: " + Logic.CheckCode("SER1", _value[0]) + "<BR>Use of counseling or therapy: " + Logic.CheckCode("SER2", _value[1]);
                        break;
                    default: 
                        string QuestionCode=_type[i];
                        // get the question content and id that match each thing in value

                        //TODO~yffu this is a workaround until all questions are populated in the tables
                        string QueryString = "SELECT COUNT(*) FROM QUESTION_REF Q, PAGE_REF P WHERE P.PID =Q.PID AND P.VALUE= '{0}'";
                        QueryString= String.Format(QueryString, QuestionCode);
                        if (Db.GetCount(QueryString)==0) continue;
                        
                        QueryString = "SELECT P.VALUE, CONTENT, Q.QID FROM QUESTION_REF Q, PAGE_REF P WHERE P.PID = Q.PID AND P.VALUE='{0}' ORDER BY QID ASC";
                        QueryString=String.Format(QueryString, QuestionCode);

                        string[][] QuestionInfos = Db.GetRecords(QueryString);
                        // the script for popup help
                        string _popup = " style='cursor:help;text-decoration:underline;text-underline-style:dotted' onmouseover=\"document.getElementById('I').style.display='block';document.getElementById('I').innerHTML='{0}'\" onmouseout=\"document.getElementById('I').style.display='none';\"";
                        string _temp = "<P>";
                        string QuestionHtml = "<SPAN {0}>{1}</SPAN>&mdash;{2}<BR>";
                        for (int j = 0; j <_value.Length; j++){
                            // 0 is the DrawMessage result
                            // 1 is this _value[0].Replace(".", "n/a")
                            if (QuestionInfos.Length<=j) continue;
                            string[] QuestionInfo= QuestionInfos[j];

                            string[] Responses = _value[j].Split(',');
                            string QueryString2="SELECT QID, R.CONTENT, R.VALUE FROM RESPONSE_X_QUESTION RQ, RESPONSE_REF R WHERE RQ.QID = {0} AND RQ.RID = R.RID ORDER BY R.RID ASC";
                            QueryString2= String.Format(QueryString2, QuestionInfos[j][2]);

                            string[][] ResponseRef = Db.GetRecords(QueryString2);
                            string Content = "";
                            
                            for (int k = 0; k < Responses.Length; k++)
                                {
                                if (Responses[k] == ".") { Content += "N/A; "; continue;};
                                bool found = false;
                                foreach (string[] r in ResponseRef) { if (r[2]==Responses[k]) { Content += r[1] + "; </BR> "; found = true;} };
                                if (!found) Content += Responses[k] + "; </BR>";
                                }
                            // format the question name
                            QuestionInfo[0] = QuestionInfo[0] + "_" + (j + 1);    
                            // format the value 
                            // for the popup content, comes from question_ref table
                            // create arguments from above and replace
                            string[] args = {string.Format(_popup, QuestionInfo[1]), QuestionInfo[0], Content };
                            _temp += string.Format(QuestionHtml, args);
                            
                        }
                        _o+=_temp+"</P>";                        
                        break;
                }
            }
        }

        return "<DIV style='padding-left:15px;padding-right:15px'><B>Student Info:</B> " + _o + "</DIV>";
    }

    public static string DrawMessage(string Type) { return DrawMessage(Type, ""); }

    public static string DrawMessage(string Type, string ParticipantId)
    {
        string Message = "";

        switch (Type)
        {
            case "ALC1": Message = "AUDIT1. During the past two months, how often have you had a drink containing alcohol?"; break;
            case "ALC2": Message = "AUDIT2. During the past two months, how many standard drinks containing alcohol have you had on a typical day when drinking?"; break;
            case "ALC3": Message = "AUDIT3. During the past two months, how often have you had six or more drinks on one occasion?"; break;

            case "ALC4": Message = "AUDIT4. During the past two months, how often have you found that you were not able to stop <BR>drinking once you had started?"; break;
            case "ALC5": Message = "AUDIT5. During the past two months, how often have you failed to do what was normally expected of you because of drinking?"; break;
            case "ALC6": Message = "AUDIT6. During the past two months, how often have you needed a first drink in the morning to get yourself going after a heavy drinking session?"; break;
            case "ALC7": Message = "AUDIT7. During the past two months, how often have you had a feeling of guilt or remorse after drinking?"; break;
            case "ALC8": Message = "AUDIT8. During the past two months, how often have you been unable to remember what happened the night before because you had been drinking?"; break;
            case "ALC9": Message = "AUDIT9. Have you or someone else been injured because of your drinking?"; break;
            case "ALC10": Message = "AUDIT10. Has a relative or friend or a doctor or another health worker been concerned about your drinking or suggested that you cut down?"; break;

            case "PHQ1": Message = "PHQ1. Little interest or pleasure in doing things."; break;
            case "PHQ2": Message = "PHQ2. Feeling down, depressed or hopeless."; break;
            case "PHQ9": Message = "PHQ9. Thoughts that you would be better off dead or of hurting yourself in some way."; break;
            case "PHQ3": Message = "PHQ3. Trouble falling or staying asleep, or sleeping too much."; break;
            case "PHQ4": Message = "PHQ4. Feeling tired or having little energy."; break;
            case "PHQ5": Message = "PHQ5. Poor appetite or overeating."; break;
            case "PHQ6": Message = "PHQ6. Feeling bad about yourself, or that you are a failure or have let yourself or your family down."; break;
            case "PHQ7": Message = "PHQ7. Trouble concentrating on things, such as reading the newspaper or watching television."; break;
            case "PHQ8": Message = "PHQ8. Moving or speaking so slowly that other people could have noticed? Or the opposite: being so fidgety or restless that you have been moving around a lot more than usual."; break;

            case "HM1": Message = "HM1. How often have you felt that emotional or mental difficulties have hurt your academic performance?"; break;
            case "HM2": Message = "HM2. How often have you felt that emotional or mental difficulties have interfered with your social activities or relationships?"; break;

            case "REA1": Message = "REA1. Seek information (from websites, pamphlets, or other sources) about available mental health or other support services for any concerns you might have (0: screw it, 10: already did)"; break;
            case "REA2": Message = "REA2. Talk to a family member about the possibility of seeking help from a mental health professional (such as a counselor, psychologist, social worker, psychiatrist) for any concerns you might have (0: screw it, 10: already did)"; break;
            case "REA3": Message = "REA3. Talk to a friend or other non family member (e.g., teacher, coach, spiritual leader) about the possibility of seeking help from a mental health professional (such as a counselor, psychologist, social worker, psychiatrist) for any concerns you might have (0: screw it, 10: already did)"; break;
            case "REA4": Message = "REA4. Seek help from a mental health professional (such as a counselor, psychologist, social worker, psychiatrist) for any concerns you might have (0: screw it, 10: already did)"; break;
            case "REA5": Message = "REA5. Access some type of self-help or support group (e.g., alcohol/drug use, sexual assault, identity, self-esteem) for any concerns you might have (0: screw it, 10: already did)"; break;
            case "REA6": Message = "REA6. Access academic support services (e.g., study skills, academic skills, tutoring) for any concerns you might have (0: screw it, 10: already did)"; break;

            case "Criteria":

                if (ParticipantId != "")
                {
                    string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER'")[0].Split('|');
                    bool CurrentlyTakingPrescribedMedication = false; if (_ser[0] == "0") CurrentlyTakingPrescribedMedication = true;
                    bool CurrentlyReceivingCounselingOrTherapy = false; if (_ser[1] == "0") CurrentlyReceivingCounselingOrTherapy = true;

                    Message = "AUDIT Positive: " + Logic.AlcPositive(ParticipantId).ToString().ToLower() + ", PHQ2 Positive: " + Logic.PHQ2Positive(ParticipantId).ToString().ToLower() + ", PHQ9: " + Logic.PHQ9_Suicide(ParticipantId).ToString().ToLower() + ", Suicidal attempt: " + Logic.SuicideLifeTimeAttempt(ParticipantId).ToString().ToLower() + ", Current use of medication: " + CurrentlyTakingPrescribedMedication.ToString().ToLower() + ", Current use of counseling or therapy: " + CurrentlyReceivingCounselingOrTherapy.ToString().ToLower() + ".";
                }
                break;


            case "Post": Message
                    = "The Counselor will be notified that your message has been posted and will respond within 24 hours. You will receive notification by "
                    + "email when the Counselor has responded.  The email will include a link and a password to a secure eBridge site for you to respond to "
                    + "the Counselor. As a reminder, all communication with the Counselor is private."; break;

            case "AvailableTimes": Message
                    = "The Counselor is available to chat with you one-on-one during daytime and evening hours. If you select this option, you will be asked "
                    + "to provide some times that you are available to chat privately. You will receive a message confirming the time of your private chat "
                    + "and a link to be used at the time of your scheduled time."; break;

            case "Chat": Message = "You can return to the <I>e</I>Bridge site at any time to review your feedback or communicate with the Counselor. "
                + "The link to the website has been sent to you by email. We hope to hear from you soon."; break;

            case "Private": Message
                    = "The counselor will never see your name or any other identifying information (such as email address). The emails you receive during "
                    + "your communications with the counselor are sent by an automated process. Similarly, the online chat feature does not reveal any "
                    + "identifying information about you to the counselor."; break;

            case "Counselor": Message
                    = "The counselor is a licensed mental health professional at the University of Michigan who is knowledgeable about mental health concerns that "
                    + "students often face. This individual is prepared to help you consider options for services and support that may be helpful.<P>"
                    + "<B>What do I have to gain from corresponding with the counselor?</B><P>"
                    + "Our goal is to help you consider services or other types of support that may be beneficial to your well-being. The counselor can help you think "
                    + "through your current situation and weigh your options. Regardless of whether you decide to try any services, you may find this thought process to be helpful."; break;
        }

        return " style='cursor:help;text-decoration:underline;text-underline-style:dotted' onmouseover=\"document.getElementById('I').style.display='block';document.getElementById('I').innerHTML='" + Message + "'\" onmouseout=\"document.getElementById('I').style.display='none';\"";
    }

    public static string DrawMessageJHeader(int XOffset, int YOffset, int Width)
    {
        return "<SCRIPT type='text/javascript'>"
                  + "var divName = 'I';"
                  + "var offX = " + XOffset.ToString() + ";"
                  + "var offY = " + YOffset.ToString() + ";"
                  + "var showI = false;"
                  + "function mouseX(evt) {if (!evt) evt = window.event; if (evt.pageX) return evt.pageX; else if (evt.clientX)return evt.clientX + (document.documentElement.scrollLeft ?  document.documentElement.scrollLeft : document.body.scrollLeft); else return 0;}"
                  + "function mouseY(evt) {if (!evt) evt = window.event; if (evt.pageY) return evt.pageY; else if (evt.clientY)return evt.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop); else return 0;}"
                  + "function follow(evt) {if (document.getElementById) {"
                  + "document.getElementById(divName).style.left = (parseInt(mouseX(evt))+offX) + 'px';"
                  + "document.getElementById(divName).style.top = (parseInt(mouseY(evt))+offY) + 'px';}}"
                  + "document.onmousemove = follow;"
                  + "</SCRIPT><DIV id='I' align='left' style='z-index:999;width:" + Width.ToString() + "px;position:absolute;background-color:yellow;padding:10px;font-family:arial;font-size:12px;border:1px solid black;display:none'></DIV>";
    }
}
