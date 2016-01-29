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

public partial class survey_consent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        string ParticipantId = Session["PARTICIPANT_ID"] as string;
        Dictionary<string, string> SchoolInfo = Helper.FindSchool(ParticipantId);
        if (String.IsNullOrEmpty(ParticipantId)) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        Response.Write(PageElements.PageHeader("Logo Only"));
        Response.Write(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\question\\CONSENT.htm"));
        Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
    }
}
