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

public partial class survey2_receipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ParticipantId = Session["PARTICIPANT_ID"] as string;
        Dictionary<string, string> SchoolInfo = Helper.FindSchool(ParticipantId);
        if (String.IsNullOrEmpty(ParticipantId)) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivitation email to access the site.</SPAN>"); return; }

        Response.Write(PageElements.PageHeader("Logo Only"));
        //__debug();

        Response.Write("<DIV style='padding-top:20px;padding-bottom:40px;font-size:18px' align='center'><B>Thank you for participating in the survey.</B><BR>&nbsp;<BR>&nbsp;<BR>Your participation will help us learn more about how to improve the mental health and well-being of college students. We will be sending the final 6-month follow up survey in April 2015, and hope that you continue to take part in this study.</DIV>");

        Response.Write(PageElements.PageFooter(ParticipantId, SchoolInfo["code"]));
    }

    private void __debug()
    {
        string[][] _data = Db.GetRecords("SELECT * FROM followup_response WHERE participant_id = '" + Session["PARTICIPANT_ID"].ToString() + "'");

        for (int i = 0; i < _data.Length; i++)
        {
            Response.Write(_data[i][1] + ": " + _data[i][2] + "<BR>");
        }
        //Response.Write(PageElements.PrintDebug2(Session["PARTICIPANT_ID"].ToString(), File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey2\\q.txt")));
    }
}
