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
using System.Collections.Generic;

public partial class Rd_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Defines redirect urls with query string
        Dictionary<string, string> url_hash = new Dictionary<string, string>();
        url_hash.Add("i1", "http://studenthealth.uiowa.edu/");
        url_hash.Add("i2", "http://counseling.studentlife.uiowa.edu/");
        url_hash.Add("i3", "http://www.uihealthcare.org/Psychiatry/");
        url_hash.Add("s1", "http://www.stanford.edu/group/SUDPS/contact.shtml");
        url_hash.Add("s5", "http://vaden.stanford.edu/special-topics/sexual-assault");
        url_hash.Add("s2", "http://vaden.stanford.edu/caps/urgent");
        url_hash.Add("s3", "http://wellness.stanford.edu/deans-call");
        url_hash.Add("s7", "http://caps.stanford.edu");
        url_hash.Add("s4", "http://studentaffairs.stanford.edu/resed/about/contact/rds");
        url_hash.Add("n1", "http://www.unr.edu/counseling");
        url_hash.Add("n2", "http://www.unr.edu/shc/");
        url_hash.Add("m1", "http://www.umich.edu/~caps");
        url_hash.Add("m2", "http://www.umich.edu/~psychcln/");
        url_hash.Add("m3", "http://www.uhs.umich.edu");

        string ppt_id = (string)Session["PARTICIPANT_ID"];
        string site = ppt_id.Substring(ppt_id.Length - 1);
        string action = Request.QueryString[0];
        if (action == "resources")
        {
            Utility.LogActivity(ppt_id, "CLICKED RESOURCE LINK", "Clicked mental health resource link");
            string rd_url = Request.QueryString[1];
            Response.Redirect(url_hash[rd_url]);
        }
        if (action == "pf")
        {
            Utility.LogActivity(ppt_id, "CLICKED RESOURCE LINK", "Clicked mental health resource link");
            Response.Redirect(String.Format("../survey/receipt.aspx?p={0}&p1=0", ppt_id));
        }
        

    }
}
