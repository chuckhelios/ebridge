using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_giftcard_getlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["giftcard"] == null) { Response.Redirect("index.aspx"); return; }

        string Institution = Request.QueryString[0];
        string QueryType = Request.QueryString[1];

        string[][] _participant = Db.GetRecords(Query(Institution, QueryType));

        Response.Write("<SPAN style='font-family:arial;font-size:12px'>");

        foreach (string[] _s in _participant)
        {
            Response.Write(_s[0] + "<BR>");
            Response.Flush();
        }

        Response.Write("</SPAN>");
    }

    protected string Query(string Institution, string QueryType)
    {
        string _q = "SELECT id FROM participant WHERE password <> 'test' AND SUBSTRING(id,10,1) = '" + Institution + "'";
        if (QueryType == "screening") _q += " AND id IN (SELECT id FROM status WHERE status_code = 'INTERVENTION')";
        if (QueryType == "4-week") _q += " AND id IN (SELECT id FROM status WHERE status_code = '4-WEEK FOLLOWUP')"; // confirm
        if (QueryType == "6-month") _q += " AND id IN (SELECT id FROM status WHERE status_code = '6-MONTH FOLLOWUP')"; // confirm
        return _q;
    }
}