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
        if (String.IsNullOrEmpty(ParticipantId)) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

        Dictionary<string, string> SchoolInfo = Helper.FindSchool(ParticipantId);
        string consent_text = Helper.ConsentText(ParticipantId);
        ConsentLabel.Text = consent_text;

        HeadPlace.Text=PageElements.PageHeader("ConsentPage", "", SchoolInfo["code"]);
        // Need to find some way to add school specific information into the page
        FootPlace.Text=PageElements.PageFooter("ConsentPage", SchoolInfo["code"]);
    }
}
