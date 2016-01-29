using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class survey_question_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<string, string> school_data = (Dictionary<string, string>)Session["SCHOOL"];
        SchoolName1.Text = school_data["name"];
        SchoolName2.Text = school_data["name"];
        SchoolName3.Text = school_data["name"];

        SchoolDept1.Text = school_data["dept"];
        DeptAddress1.Text = school_data["addr"];


    }
}