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

public partial class admin_enrollment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] Participants = Db.GetRecords("SELECT p.id, p.gender FROM participant p, status s WHERE p.id = s.id AND s.status_code = 'INTERVENTION ELIGIBILITY' AND s.status_value = 'Y' AND p.id <> 'G6tE9FJ9gl'");

        Response.Write("<TABLE border=1>");

        for (int i = 0; i < Participants.Length; i++)
        {
            string[] Demo = Db.GetRecord("SELECT response from screening_response WHERE participant_id = '" + Participants[i][0] + "' AND question_code = 'DEM'")[0].Split('|');


            Response.Write("<TR><TD>" + (i + 1).ToString() + "</TD><TD>" + Participants[i][0] + "</TD><TD>" + Participants[i][1] + "</TD><TD>" + Demo[2] + "</TD><TD>" + Demo[3] + "</TD></TR>");

            Response.Flush();
        }

        Response.Write("</TABLE>");
    }
}
