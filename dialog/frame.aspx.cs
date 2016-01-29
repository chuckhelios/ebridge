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

public partial class dialog_frame : System.Web.UI.Page
{
    string ParticipantId, View;

    protected void Page_Load(object sender, EventArgs e)
    {
        // find if there is a valid participant id associated with the query url
        if (Request.QueryString.Count == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }
        // get the participant id and see if it is found in the database.
        ParticipantId = Request.QueryString[1]; 
        string[] Participant = Db.GetRecord("SELECT name_first, email FROM participant WHERE id = '" + ParticipantId + "'");
        if (Participant == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        View = Request.QueryString[0];
        
        // use the first query url to see the mode of display. 
        if (View == "counselor")
        {
            if (Session["COUNSELOR_ID"] == null) { Response.Redirect("login_counselor.aspx?p=" + ParticipantId ); return; }
       
            Response.Write("<HTML>"
                + "<HEAD><TITLE>eBridge - A Journey for Your Mental Health</TITLE></HEAD>"
                + "<FRAMESET id='F' frameborder='0' framespacing='0' rows='*,0'>"
                + "<FRAME name='MAIN' SRC='thread_counselor.aspx?p=" + ParticipantId + "'>"
                //+ "<FRAME name='CHAT' SRC='chat.aspx?p=counselor&p1=" + ParticipantId + "' noresize scrolling='no' marginwidth='0' marginheight='0'>"
                + "</FRAMESET>"
                + "</HTML>");
        }
        // if the first query url is not counselor, then the participant view is generated. for example, p=participant.
        else
        {
            if (Session["PARTICIPANT_LOGIN"] == null) { Response.Redirect("login.aspx?p=" + ParticipantId); return; }

            //string InitialMsg = ""; if (Request.QueryString.Count == 3) InitialMsg = Request.QueryString[2];
            // the participant is redirected by js from the receipt page to here. you need to set the

            // set the DESIGNATED_COUNSELOR;
            string _counselor = Utility.GetStatus(ParticipantId, "COUNSELOR");
            if (_counselor == null) { Response.Redirect("login.aspx?p=" + ParticipantId); return; }
            Session["DESIGNATED_COUNSELOR"]= _counselor;            

            Response.Write("<HTML>"
                + "<HEAD><TITLE>eBridge - A Journey for Your Mental Health</TITLE></HEAD>"
                + "<FRAMESET id='F' frameborder='0' framespacing='0' rows='*,0'>"
                + "<FRAME name='MAIN' SRC='thread.aspx?p=" + ParticipantId + "'>"
                //+ "<FRAME name='CHAT' SRC='chat.aspx?p=participant&p1=" + ParticipantId + "' noresize scrolling='no' marginwidth='0' marginheight='0'>"
                + "</FRAMESET>"
                + "</HTML>");
        }
    }
}
