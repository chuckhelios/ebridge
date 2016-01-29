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

public partial class dialog_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }
        string ParticipantId = Request.QueryString[0]; if (Db.GetCount("SELECT COUNT(*) FROM participant WHERE id = '" + ParticipantId + "'") == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        // log activity
        Utility.LogActivity(ParticipantId, "PASSWORD MANAGEMENT PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        Response.Write(PageElements.DialogPanelHeader().Replace("700px","500px"));

        Response.Write("<FORM method='post' action='action.aspx?p=change_pwd'>");
        Response.Write("<TABLE align='center' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='40px'><TD></TD></TR>"); 
        Response.Write("<INPUT name='PID' type='hidden' value='" + ParticipantId + "'>");
        Response.Write("<TR><TD id='P_L' style='padding-right:20px;padding-bottom:2px'>New Password:</TD></TD><TD style='padding-right:40px'><INPUT id='P' name='PASSWORD' type='password'></TD></TR>");
        Response.Write("<TR><TD id='P1_L' style='padding-right:20px;padding-bottom:2px'>Confirm Password:</TD></TD><TD style='padding-right:40px'><INPUT id='P1' type='password'></TD></TR>");
        Response.Write("<TR><TD></TD><TD style='padding-top:5px;padding-bottom:10px'><INPUT type='submit' value='Login' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'></TD></TR>");
        Response.Write("</TABLE>");
        Response.Write("</FORM>");

        Response.Write(PageElements.PageFooter(null));

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "document.getElementById('P').focus();"
                 + "function IsValid() {"
                 + "document.getElementById('P_L').style.color='black';"
                 + "document.getElementById('P1_L').style.color='black';"
                 + "if (document.getElementById('P').value=='') {alert('Please enter new password.');document.getElementById('P').focus();document.getElementById('P_L').style.color='red';return false;} "
                 + "if (document.getElementById('P1').value=='') {alert('Please confirm new password.');document.getElementById('P1').focus();document.getElementById('P1_L').style.color='red';return false;} "
                 + "if (document.getElementById('P').value!=document.getElementById('P1').value) {alert('Passwords do not match.');document.getElementById('P1').focus();document.getElementById('P1').select();document.getElementById('P1_L').style.color='red';return false;} "
                + "return true;}"
                 + "</SCRIPT>");
    }
}
