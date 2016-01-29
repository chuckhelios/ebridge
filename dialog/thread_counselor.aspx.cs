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

public partial class dialog_thread_counselor : System.Web.UI.Page
{
    private string EMAIL;
    private string NAME_FIRST;
    string ParticipantId;
    string QueryString;
    string CounselorId;
    string CounselorSite;

    protected void Page_Load(object sender, EventArgs e)
    {
        CounselorId = Session["COUNSELOR_ID"] as string;  
        CounselorSite = Session["SCHOOL"] as string;
        ParticipantId = Request.QueryString[0];

        if (string.IsNullOrEmpty(CounselorId) || string.IsNullOrEmpty(CounselorSite))
        {
            Session.Abandon();
            Response.Redirect("login_counselor.aspx?u=thread_counselor.aspx?p="+ParticipantId);
            return;
        }

        QueryString="SELECT name_first, email FROM participant WHERE id = '{0}'";
        
        string[] Participant = Db.GetRecord(string.Format(QueryString, ParticipantId));
        
        // if participant id is bogus
        if (Participant == null)
        {
            Response.Write("<SCRIPT type='text/javascript'>alert('Participant Id Not Found.');</SCRIPT>");
            Response.Write("<SCRIPT type='text/javascript'>parent.location='list.aspx';</SCRIPT>");
            Response.Redirect("list.aspx");
            return;
        }

        // log activity
        Utility.LogActivity(ParticipantId, "COUNSELOR: MSG THREAD READ", Request.UserHostAddress + "|" + Request.UserAgent);

        string[][] Messages = Db.GetRecords("SELECT * FROM message WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "' ORDER BY ID DESC");
        //if (Messages == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>The student has not access the dialog page yet.</SPAN>"); return; }

        NAME_FIRST = Participant[0]; EMAIL = Participant[1];

        Response.Write(PageElements.ThreadPageHeader(ParticipantId, "Counselor"));
        Response.Write("<FORM method='post' action='action.aspx?p=counselor_post'>");
        Response.Write("<INPUT name='PID' type='hidden' value='" + ParticipantId + "'><INPUT name='PNAME' type='hidden' value='" + NAME_FIRST + "'><INPUT name='PEMAIL' type='hidden' value='" + EMAIL + "'>");
        Response.Write("<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='30px'><TD></TD></TR>");
        //Response.Write("<TR><TD style='padding-bottom:25px;color:red;font-weight:bold' align='center'>Counselor View (With: " + ParticipantId + ")</TD></TR>");
        Response.Write("<TR><TD><DIV style='font-size:12px;padding-bottom:5px;font-weight:bold'>Post a Message to the Student:</DIV><TEXTAREA name='MSG' id='MSG' style='width:520px;height:80px;font-family:verdana;font-size:11px'></TEXTAREA></TD></TR>");// maxlength='500'
        Response.Write("<TR><TD style='padding-top:2px;padding-bottom:20px'>");
        Response.Write("<TABLE width='100%' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'><TR><TD><SPAN style='color:blue;text-decoration:underline;cursor:pointer;cursor:hand' onmouseover=\"this.style.color='red'\" onmouseout=\"this.style.color='blue'\" onclick=\"if (IsValid2()) window.open('slider_test.aspx?' + escape(document.getElementById('MSG').value));\">Preview Slider Question</SPAN></TD><TD align='right'><INPUT type='submit' value=' Post ' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'></TD></TR></TABLE>");
        Response.Write("</TD></TR>");
        Response.Write("<TR height='20px'><TD></TD></TR>");
        Response.Write("</TABLE>");

        Response.Write(PageElements.DisplayThread(Messages, "Counselor"));
        Response.Write("</FORM>");
        Response.Write(PageElements.ThreadPageFooter());

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "function IsValid() {"
                 + "if (document.getElementById('MSG').value=='') {alert('What have you been smoking?');return false;} "
                 + "return true;}"
                 + "function IsValid2() {"
                 + "if (document.getElementById('MSG').value=='') {alert('What have you been smoking?');return false;} "
                 + "return true;}"
                 + "</SCRIPT>");

        Response.Write(PageElements.DrawMessageJHeader(5, 5, 300));
    }

    private void __debug()
    {
        Response.Write("<SPAN style='font-family:arial;font-size:10px;color:gray'>");
        string[][] _rs = Db.GetRecords("SELECT question_code, response FROM screening_response WHERE participant_id = '" + ParticipantId + "' ORDER BY date_time");
        for (int i = 0; i < _rs.Length; i++) Response.Write(_rs[i][0] + ": " + _rs[i][1] + "<BR>");
        Response.Write("</SPAN><P>");
    }
}
