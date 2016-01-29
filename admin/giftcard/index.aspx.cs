using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_giftcard_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btGo_Click(object sender, EventArgs e)
    {
        if (tbPasscode.Text == "king")
        {
            Session["giftcard"] = "Y";
            lbMessage.Text = "<A href='sendcard.aspx'>Send Email</A><P>";
            lbMessage.Text += __drawInstitution("m") + "<P>" + __drawInstitution("n");
        }
        else
        {
            lbMessage.Text = "Passcode incorrect";
        }
    }

    protected string __drawInstitution(string Institution)
    {
        string _o = "";

        if (Institution == "m") _o += "University of Michigan";
        if (Institution == "n") _o += "University of Nevada Reno";

        _o += "<P>Everybody invited (" + Db.GetCount(Query(Institution, "all")).ToString("N0") + ") <A href='getlist.aspx?p1=" + Institution + "&p2=all'>Get List</A><BR>"
           + "Participants who completed full screening survey (" + Db.GetCount(Query(Institution, "screening")).ToString("N0") + ") <a href='getlist.aspx?p1=" + Institution + "&p2=screening'>Get List</a><BR>"
           + "Participants who completed 4-week follow-up survey (--)<BR>"
           + "Participants who completed 6-month follow-up survey (--)<P>";

        return _o;
    }

    protected string Query(string Institution, string QueryType)
    {
        string _q = "SELECT count(*) FROM participant WHERE password <> 'test' AND SUBSTRING(id,10,1) = '" + Institution + "'";
        if (QueryType == "screening") _q += " AND id IN (SELECT id FROM status WHERE status_code = 'INTERVENTION')";
        if (QueryType == "4-week") _q += " AND id IN (SELECT id FROM status WHERE status_code = '4-WEEK FOLLOWUP')"; // confirm
        if (QueryType == "6-month") _q += " AND id IN (SELECT id FROM status WHERE status_code = '6-MONTH FOLLOWUP')"; // confirm
        return _q;
    }
}