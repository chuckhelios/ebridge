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

public partial class admin_email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(PageElements.DialogPanelHeader().Replace("700px", "550px"));

        Response.Write("<FORM method='post' action='action.aspx?p=send_email'>");
        Response.Write("<TABLE align='center' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='40px'><TD></TD></TR>");
        Response.Write("<TR><TD id='A_L' style='padding-right:30px;padding-bottom:2px'>To:</TD><TD><INPUT id='A' name='ADDRESS' style='width:220px;height:22px;font-family:verdana;font-size:11px'></TD></TR>");
        Response.Write("<TR><TD id='S_L' style='padding-right:30px;padding-bottom:2px'>Subject:</TD><TD><INPUT id='S' name='SUBJECT' style='width:320px;height:22px;font-family:verdana;font-size:11px'></TD></TR>");
        Response.Write("<TR><TD id='B_L' valign='top' style='padding-right:30px;padding-bottom:2px'>Body:</TD><TD><TEXTAREA id='B' name='BODY' style='width:320px;height:160px;font-family:verdana;font-size:11px'></TEXTAREA></TD></TR>");
        Response.Write("<TR><TD></TD><TD style='padding-top:5px'><INPUT type='submit' value='Send' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'></TD></TR>");
        Response.Write("</TABLE>");
        Response.Write("</FORM>");

        Response.Write(PageElements.PageFooter(null));

        Response.Write("<SCRIPT type='text/javascript'>"
          + "document.getElementById('A').focus();"
          + "document.getElementById('A_L').style.color='#000000';document.getElementById('S_L').style.color='#000000';document.getElementById('B_L').style.color='#000000';"
          + "function IsValid() {"
          + "if (document.getElementById('A').value=='') {alert('Please specify at least one recipient.');document.getElementById('A').focus();document.getElementById('A_L').style.color='red';return false;} "
          + "if (document.getElementById('S').value=='') {alert('Please provide an subject for the email.');document.getElementById('S').focus();document.getElementById('S_L').style.color='red';return false;} "
          + "if (document.getElementById('B').value=='') {alert('Please enter the body of the email.');document.getElementById('B').focus();document.getElementById('B_L').style.color='red';return false;}"
          + "if (document.getElementById('A').value.indexOf('.') == -1 || document.getElementById('A').value.indexOf('@') == -1) {alert('Please enter a valid email address.');document.getElementById('A').focus();document.getElementById('A_L').style.color='red';return false;}"
          + "return true;}"
          + "</SCRIPT>");
    }
}
