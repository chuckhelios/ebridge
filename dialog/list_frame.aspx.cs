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

public partial class dialog_list_frame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["COUNSELOR_LOGIN"] == null) { Response.Redirect("login_counselor.aspx?p=list"); return; }

        Response.Write("<HTML>"
            + "<HEAD><TITLE>eBridge - A Journey for Your Mental Health</TITLE></HEAD>"
            + "<FRAMESET id='F' frameborder='0' framespacing='0' cols='%30,%30'>"
            + "<FRAME name='MAIN' SRC='list.aspx'>"
            + "<FRAME name='CHAT' SRC='../chat/?id=001&c=c&p=F903ECB8i' noresize scrolling='no' marginwidth='0' marginheight='0'>"
            + "</FRAMESET>"
            + "</HTML>");
    }
}
