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

public partial class admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(PageElements.DialogPanelHeader().Replace("700px", "450px"));

        Response.Write("<FORM method='post' action='action.aspx?p=login'>");
        Response.Write("<TABLE align='center' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");
        Response.Write("<TR height='30px'><TD></TD></TR>");
        Response.Write("<TR><TD colspan='2' align='center' style='color:red;font-weight:bold'>Admin Login</TD></TR>");
        Response.Write("<TR height='20px'><TD></TD></TR>");
        Response.Write("<TR><TD id='P_L' style='padding-right:20px;padding-bottom:2px'>Password:</TD><TD style='padding-right:40px'><INPUT id='P' name='PASSWORD' type='password'></TD></TR>");
        Response.Write("<TR><TD></TD><TD style='padding-top:5px'><INPUT type='submit' value='Login' onclick=\"if (!IsValid()) {return false;}\" style='font-size:12px;font-family:arial;padding-top:1px;padding-bottom:1px'></TD></TR>");
        Response.Write("<TR height='10px'><TD></TD></TR>");
        Response.Write("</TABLE>");
        Response.Write("</FORM>");

        Response.Write(PageElements.PageFooter(null));

        Response.Write("<SCRIPT type='text/javascript'>"
                 + "document.getElementById('P').focus();"
                 + "function IsValid() {"
                 + "if (document.getElementById('P').value=='') {alert('Please enter your password.');document.getElementById('P').focus();document.getElementById('P_L').style.color='red';return false;} "
                 + "return true;}"
                 + "</SCRIPT>");
    }
}
