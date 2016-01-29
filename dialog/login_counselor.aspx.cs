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

public partial class dialog_login_counselor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }
        
        if (Session["COUNSELOR_ID"] != null) Session.Abandon();
        // log activity
        Utility.LogActivity("counselor", "DIALOG LOGIN PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        Response.Write(PageElements.DialogPanelHeader().Replace("700px", "450px"));

        Response.Write("<FORM method='post' action='action.aspx?p=counselor_login'>");
        Response.Write("<TABLE align='center' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='30px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='2' align='center' style='color:red;font-weight:bold'>Counselor Login</TD></TR>");
        Response.Write("<TR height='20px'><TD></TD></TR>");
        Response.Write("<INPUT name='PR' type='hidden' value='" + Request.QueryString[0] + "'>");
        Response.Write("<TR><TD id='U_L' style='padding-right:20px;padding-bottom:2px'>Username:</TD><TD style='padding-right:40px'><INPUT id='U' name='USERNAME' type='text'></TD></TR>");
        Response.Write("<TR><TD id='P_L' style='padding-right:20px;padding-bottom:2px'>Password:</TD><TD style='padding-right:40px'><INPUT id='P' name='PASSWORD' type='password'></TD></TR>");
        Response.Write("<TR><TD></TD><TD style='padding-right:40px'><INPUT name='H' type='checkbox' checked> Login Unnoticed</TD></TR>");
        Response.Write("<TR><TD></TD><TD style='padding-top:5px'><INPUT type='submit' value=' Login ' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'></TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>"); 
        Response.Write("</TABLE>");
        Response.Write("</FORM>");

        Response.Write(PageElements.PageFooter(null));

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "document.getElementById('U').focus();"
                 + "function IsValid() {"
                 + "if (document.getElementById('U').value=='') {alert('Please enter your username.');document.getElementById('U').focus();document.getElementById('U_L').style.color='red';return false;} "
                 + "if (document.getElementById('P').value=='') {alert('Please enter your password.');document.getElementById('P').focus();document.getElementById('P_L').style.color='red';return false;} "
                 + "return true;}"
                 + "</SCRIPT>");
    }
}
