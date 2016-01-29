using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class dialog_change_hours : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.LogActivity("counselor", "CHANGE HOURS PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        string CounselorSite = Session["SCHOOL"] as string;
        string CounselorId = Session["COUNSELOR_ID"] as string;

        if (string.IsNullOrEmpty(CounselorSite) || string.IsNullOrEmpty(CounselorId))
        {
            Session.Abandon(); Response.Redirect("login_counselor.aspx?p=list"); return;
        }

        string QueryString = @"select hours from site s, (select max(date_time) as max_date 
                            from site where id='{0}') as r where s.id='{0}' and s.date_time = 
                            r.max_date;";

        string _hours = Db.GetRecord(string.Format(QueryString, CounselorSite))[0];

        BindListView(_hours);
    }

    private void BindListView(string _hours)
    {
        using (DataTable dt = new DataTable())
        {
            dt.Columns.Add("Weekday", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            string[] _days = _hours.Split('|');
            string[] Weekdays = new string[6] { "Title", "Mon", "Tue", "Wed", "Thur", "Fri" };

            for (int i = 0; i < Weekdays.Length; i++)
            {
                //'Mon|10-12|2-5$Tue|10-12|2-5$Wed|10-12|2-5$Thur|10-12|2-5$Fri|10-12|2-5'
                dt.Rows.Add(Weekdays[i], _days[i]);
            }
            DataView dv = new DataView(dt);
            ListView1.DataSource = dv;
            ListView1.DataBind();
        }

    }
}