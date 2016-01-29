using System;
using System.Collections;
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

public partial class survey_index : System.Web.UI.Page
{
    
    private List<string> TestParticipantId;
    private Dictionary<string, string> SchoolInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.Count == 0) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }
        string ParticipantId = Request.QueryString[0]; string[] Participant = Db.GetRecord("SELECT name_first, email FROM participant WHERE id = '" + ParticipantId + "'");
        if (Participant == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the invitation email to access the site.</SPAN>"); return; }

        List<string> TestParticipantId = new List<string>() { "361B0EC2i", "F903ECB8i", "32939EF8m", "01CE605Ei", "046C552Bi", "0554F3DAi", "0624B9CAi", "G6tE9FJ9gi", "aiBcAmVnQs", "D67477F8s", "D6E2A31Bs", "D93E3745s" };

        if (!TestParticipantId.Contains(ParticipantId) &&  Utility.GetLog(ParticipantId, "SURVEY COMPLETED") != "NULL") { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>You have already completed the survey.</SPAN>"); return; }

        //log activity
        Utility.LogActivity(ParticipantId, "SURVEY STARTED", Request.UserHostAddress + "|" + Request.UserAgent);

        Session["PARTICIPANT_LOGIN"]= "Y";
        Session["PARTICIPANT_ID"] = ParticipantId;
        Session["EMAIL"]= Participant[1];
        SchoolInfo = Helper.FindSchool(ParticipantId);
        Session["SCHOOL"] = SchoolInfo;

        Response.Write(PageElements.PageHeader("InitialPage", "", SchoolInfo["code"]));
        //Response.Write("<CENTER style='padding-bottom:20px'><IMG src='../image/start.png' border='0' style='cursor:hand;cursor:pointer' onclick=\"window.location.href='question/consent.aspx'\"></CENTER>");
        Response.Write(PageElements.PageFooter("InitialPage", SchoolInfo["code"]));
    }
}