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

public partial class dialog_slider_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // log activity
        Utility.LogActivity("counselor", "SLIDER TESTER PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        string _js = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\dialog\\slider_ebridge_header.txt");

        Response.Write("<HTML><HEAD><TITLE>eBridge - A Journey for Your Mental Health</TITLE>" + _js
                    + "</HEAD><BODY class='yui-skin-sam'>"
                    + "<TABLE width='100%' height='100%' cellpadding='0px' cellspacing='0px'><TR><TD style='padding-top:20px;padding-bottom:40px;background-color:#454545' align='center'>"
                    + "<TABLE width='460px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#FFFFFF'>"

                    + "<TR><TD align='center' style='padding-top:30px;padding-bottom:10px;background-color:#D3D3D3;font-weight:bold'><I>e</I><SPAN style='font-size:1px'> </SPAN>Bridge: A Journey for Your Mental Health</TD></TR>"
                    + "<TR><TD style='padding-left:40px;padding-right:40px;padding-bottom:30px'>");

        Response.Write("<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='30px'><TD></TD></TR>");
        Response.Write("<TR><TD style='padding-bottom:25px;color:red;font-weight:bold' align='center'>Slider Question Preview</TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("<TR><TD>");

        if (Request.QueryString.Count != 0)
        {
            string[][] Messages = new string[1][];
            Messages[0] = new string[6]; for (int i = 0; i < Messages[0].Length; i++) Messages[0][i] = "";
            Messages[0][3] = Request.QueryString[0]; Messages[0][2] = "Counselor"; Messages[0][4] = DateTime.Now.ToString(); Messages[0][5] = "NULL";

            Response.Write("<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>"
                        + "<TR><TD width='120px'>&nbsp;</TD><TD align='right' width='380px'>"
                        + FormatMessage(Messages[0], "Counselor", "Student") + "<SPAN style='padding-right:5px;font-size:10px;color:#888888'>" + PageElements.FormateDateTime(Messages[0][4]) + "</SPAN>"
                        + "</TD><TD valign='bottom' style='padding-left:5px;padding-bottom:6px'><IMG src='message_image/lightbulb.png'></TD></TR>"
                        + "<TR height='12px'><TD></TD></TR>"
                        + "</TABLE>");

            Response.Write("</TD></TR>");
            Response.Write("</TABLE>");
        }

        Response.Write(PageElements.ThreadPageFooter());

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "function IsValid() {"
                 + "if (document.getElementById('MSG').value=='') {alert('Your message cannot be empty.');return false;} "
                 + "return true;}"
                 + "</SCRIPT>");
    }

    private string FormatMessage(string[] Message, string Type, string View)
    {
        string _color = "blue"; if (Type == "Student") _color = "green";

        string MessageBody = Message[3].Replace("\r\n", "<BR>");
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

    private string GetMITool(string MessageId, string Message, string ParticipantId, string View)
    {
        string _o = "";

        if (Message.IndexOf('|') == -1) return Message;

        try
        {
            if (Message.Split('|')[0] == "_MI_SLIDER_")
            {
                string[] Label = Message.Split('|')[2].Split(',');

                _o = "<TABLE width='282px' cellpadding='0px' cellspacing='0px'>"
                    + "<TR height='12px'><TD></TD></TR>"
                    + "<TR><TD colspan='2' style='padding-bottom:12px;font-weight:bold'>" + Message.Split('|')[1] + "</TD></TR>"
                    + "<TR><TD width='209px'><DIV id='slider-bg' class='yui-h-slider' tabindex='-1' title='Slider'><DIV id='slider-thumb' class='yui-slider-thumb'><IMG src='slider/thumb-n.gif'></DIV></DIV><SPAN id='slider-value' style='display:none'>0</SPAN></TD><TD><INPUT name='V' disabled id='slider-converted-value' type='text' value='0' size='4' maxlength='4' style='text-align:center'></TD></TR>"
                    + "<TR><TD>"
                    + "<TABLE width='214px' cellpadding='0px' cellspacing='0px' style='font-size:11px'><TD width='33%' style='padding-left:3px'>" + Label[0] + "</TD><TD width='33%' align='center'>" + Label[1] + "</TD><TD width='33%' align='right'>" + Label[2] + "</TD></TR></TABLE></TD><TD></TD></TR>"
                    + "<TR><TD align='right' colspan='2' style='padding-top:10px'><INPUT name='RID' type='hidden' value='" + MessageId + "'>";

                _o += "<TEXTAREA disabled style='width:280px;height:50px;font-family:verdana;font-size:11px'></TEXTAREA>";
                _o += "</TD></TR>";
                _o += "<TR><TD align='right' colspan='2' style='padding-top:2px'><INPUT type='button' disabled value='Post' style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'>";
                _o += "</TD></TR></TD></TR><TR height='8px'><TD></TD></TR></TABLE>";
            }

            return _o;
        }
        catch
        {
            return "MI Tool Corrupted. Check your input.";
        }
    }
}
