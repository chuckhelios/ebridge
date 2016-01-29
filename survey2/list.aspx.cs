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

public partial class survey2_list : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] FOLLOWUP_SURVEY_COMPLETED = Db.GetRecords("SELECT DISTINCT id FROM activity_log WHERE activity_code = 'FOLLOWUP SURVEY COMPLETED'");

        for (int i = 0; i < FOLLOWUP_SURVEY_COMPLETED.Length; i++)
        {
            Response.Write((i + 1).ToString() + ", " + FOLLOWUP_SURVEY_COMPLETED[i][0] + ", " + Db.GetRecord("SELECT email FROM participant WHERE id = '" + FOLLOWUP_SURVEY_COMPLETED[i][0] + "'")[0] + "<BR>");
            Response.Flush();
        }
    }
}
