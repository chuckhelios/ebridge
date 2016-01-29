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

public partial class dialog_thread : System.Web.UI.Page
{
    string ParticipantId;

    protected void Page_Load(object sender, EventArgs e)
    {
        ParticipantId = Request.QueryString[0];

        string IniMessage = "";
        if (Session["IniMessage"] != null) IniMessage = Session["IniMessage"].ToString();

        // log activity
        Utility.LogActivity(ParticipantId, "STUDENT: MSG THREAD READ", Request.UserHostAddress + "|" + Request.UserAgent);

        string[][] Messages = Db.GetRecords("SELECT * FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "' ORDER BY ID DESC");

        //if (Messages == null) FirstTimeVisitor();

        Response.Write(PageElements.ThreadPageHeader(ParticipantId, "Student"));

        /*Response.Write("<SCRIPT type='text/javascript' src='./include/prototype.js'></SCRIPT>"
            + "<SCRIPT type='text/javascript' src='./include/scriptaculous-js-1.9.0/src/scriptaculous.js'></SCRIPT>"
            + "<SCRIPT type='text/javascript' src='./include/lightview2.7.1/js/lightview.js'></SCRIPT>"
            + "<LINK rel='stylesheet' type='text/css' href='./include/lightview2.7.1/css/lightview.css' />");

        Response.Write("<SCRIPT type='text/javascript'>"
            + "var http_request = false;"
            + "function postRequest(_url, _parameters) { http_request = false; if (window.XMLHttpRequest) { http_request = new XMLHttpRequest(); if (http_request.overrideMimeType) { http_request.overrideMimeType('text/html'); }} "
            + "else if (window.ActiveXObject) { try { http_request = new ActiveXObject('Msxml2.XMLHTTP');} catch (e) {try {http_request = new ActiveXObject('Microsoft.XMLHTTP');} catch (e) {}}}"
            + "http_request.open('POST', _url, true); http_request.setRequestHeader('Content-type','application/x-www-form-urlencoded'); http_request.setRequestHeader('Content-length',_parameters.length);"
            + "http_request.setRequestHeader('Connection', 'close'); http_request.send(_parameters);}"
            + "</SCRIPT>");*/

        Response.Write("<FORM name='MAIN' method='post' action='action.aspx?p=student_post'>");
        Response.Write("<INPUT name='PID' type='hidden' value='" + ParticipantId + "'>");
        Response.Write("<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='30px'><TD></TD></TR>");

        Response.Flush();

        //Response.Write("<TR><TD style='padding:0px;background-color:#FFFFFF'></TD></TR>");
        //Response.Write("<TR><TD style='padding:8px;background-color:#99C68E'>Please note that the eBridge website is not monitored 24/7. <span onclick=\"alert('hi');window.Lightview.show({ href: '../image/screenshot.png', rel: 'image', title: 'Tutorial: How to use <I>e</I>Bridge', options: {" + "autosize: true" + "}});\">However</span>, the professional counselor will respond to your message within 24 hours.</TD></TR>");

        /*int StudentPost = Db.GetCount("SELECT COUNT(*) FROM message WHERE from_id = '" + ParticipantId + "'");

        if (StudentPost < 2)
        {
            Response.Write("<TR><TD><INPUT type='button' onclick=\"postRequest('action.aspx?p=default0&p1=" + ParticipantId + "','');document.getElementById('MSG').innerHTML='Looking at my personalized feedback, I am wondering about  '\" value='Looking at my personalized feedback, I am wondering about ...' style='font-size:12px;font-family:arial'></TD></TR>");
            Response.Write("<TR><TD style='padding-top:5px'><INPUT type='button' onclick=\"postRequest('action.aspx?p=default1&p1=" + ParticipantId + "','');document.getElementById('MSG').innerHTML='I am wondering how you could help with '\" value='I am wondering how you could help with ...' style='font-size:12px;font-family:arial'></TD></TR>");
            Response.Write("<TR><TD style='padding-top:5px'><INPUT type='button' onclick=\"postRequest('action.aspx?p=default2&p1=" + ParticipantId + "','');document.getElementById('MSG').innerHTML='I want to post a message to you but will plan to do so later.'\" value='I want to post a message to you but will plan to do so later' style='font-size:12px;font-family:arial'></TD></TR>");
            Response.Write("<TR height='20px'><TD></TD></TR>");
        }*/

        Response.Write("<TR><TD><DIV style='font-size:12px;padding-bottom:5px;font-weight:bold'><SPAN style='color:red'>Send a Message to the Counselor:</SPAN></DIV>"
            + "<TEXTAREA name='MSG' id='MSG' style='width:520px;height:160px;font-family:verdana;font-size:11px' onfocus='this.value = this.value'>");


        if (IniMessage == "") Response.Write("I would like to talk to you about ...");

        if (IniMessage == "ADDITIONAL_POST")
        {
            Response.Write("If you have additional thoughts to post to the Counselor:\n\n");
        }

        if (IniMessage == "SCHEDULE_CHAT")
        {
            Response.Write("I would like to schedule a private session to chat with the Counselor online. Below is my availability:"
                + "\n\n[Please provide here dates and times that work best for you.]");
        }

        Response.Write("</TEXTAREA></TD></TR>");// maxlength='500'
        Response.Write("<TR><TD style='padding-top:2px;padding-bottom:20px' align='left'>");
        Response.Write("<INPUT type='submit' id='POST_IT' value=' " + (IniMessage == "SCHEDULE_CHAT" ? "Notify the Counselor" : "Post") + " ' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial'></FORM>");
        Response.Write("</TD></TR>");
        Response.Write("<TR height='30px'><TD></TD></TR>");

        //TODO~yffu the CKEditor is not working, can't figure out how to get it in without the aspx page controls. 
        // wanted to use cke_input.aspx as a frame source, but it's not working (can't nest frames?, dunno)
        // also note to self, ckeditor folder has to be in the root directory, otherwise can't find it.
        //Response.Write("<TR><TD><FRAME NAME='CKEDITOR' SRC='cke_input.aspx?p=" + ParticipantId + "'></TD></TR>");
        Response.Write("</TABLE>");

        Response.Flush();

        Messages = Db.GetRecords("SELECT * FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "' ORDER BY ID DESC");
        Response.Write(PageElements.DisplayThread(Messages, "Student"));
        Response.Flush();
        Response.Write(PageElements.ThreadPageFooter());

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "function IsValid() {"
                 + "if (document.getElementById('MSG').value=='') {alert('Your message cannot be empty.');return false;} "
                 + "return true;}"
                 + "</SCRIPT>");
    }

    /*
    private void FirstTimeVisitor()
    {
        string DialogBody = "_FIRST_VISIT_HIDDEN_";

        Db.Execute("INSERT INTO message (to_id, message_body, date_time) VALUES ('" + ParticipantId + "','" + DialogBody.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");

        string[][] CounselorEmail = Db.GetRecords("SELECT email FROM counselor");

        for (int i = 0; i < CounselorEmail.Length; i++)
        {
            if (CounselorEmail[i][0].Substring(0, 1) != "#")
            {
                //Utility.SendPHPMail(CounselorEmail[i][0], "ebridgeteam@umich.edu", "The eBridge Team", "eBridge: Student " + ParticipantId + " visited the dialog site for the first time.", "To follow up, click " + File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\base_url.txt") + "/dialog/thread_counselor.aspx?p=" + ParticipantId);
            }
        }
    }*/
}
