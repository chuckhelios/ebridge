using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class survey2_index : System.Web.UI.Page
{
    private List<string> TestParticipantId;
    private Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }
        string ParticipantId = Request.QueryString[0]; string [] Participant = Db.GetRecord("SELECT name_first, email FROM participant WHERE id = '" + ParticipantId + "'");
        if (Participant==null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        List<string> TestParticipantId = new List<string>(){"361B0EC2i", "F903ECB8i", "32939EF8m", "01CE605Ei", "046C552Bi", "0554F3DAi", "0624B9CAi"};

        if (!TestParticipantId.Contains(ParticipantId) && Utility.GetLog(ParticipantId, "FOLLOWUP SURVEY COMPLETED") != "NULL") { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>You have already completed the survey.</SPAN>"); return; }

        //log activity
        Utility.LogActivity(ParticipantId, "FOLLOWUP SURVEY VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        Session["PARTICIPANT_ID"] = ParticipantId;
        Session["NAME_FIRST"] = Participant[0];
        Session["EMAIL"] = Participant[1];
        SchoolInfo = Helper.FindSchool(ParticipantId);
        Session["SCHOOL"] = SchoolInfo;

        Response.Redirect("screen.aspx?p=0");
    }
}