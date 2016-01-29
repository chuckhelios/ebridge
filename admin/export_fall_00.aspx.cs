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

public partial class admin_export_fall_00 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[][] _d = Db.GetRecords("SELECT * FROM followup_response WHERE question_code = 'OQ1' OR question_code = 'OQ2'");

        for (int i = 0; i < _d.Length; i++)
        {
            Response.Write(_d[i][0] + ", " + _d[i][1] + ": " + _d[i][2] + "<P>");
        }
    }
}