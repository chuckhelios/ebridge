using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for WebformGeneric
/// </summary>
public class WebformGeneric : System.Web.UI.Page
{
    protected bool Debug;
    protected string UserType = "";
    protected string ParticipantId = "";
    protected string CounselorId;
    protected Dictionary<string, string> SchoolInfo;
    protected string DesignatedCounselor;
    
    public WebformGeneric()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private void Page_Load(object sender, EventArgs e)
    {

    }

    protected bool validateUrlParams()
    {
        Session["CurrentUrl"]= Request.Url.AbsolutePath;
        UserType = Request.QueryString["p"] as string;
        ParticipantId = Request.QueryString["id"] as string;
        return !(string.IsNullOrEmpty(UserType) || !(UserType.Equals("counselor") || UserType.Equals("participant")));
    }

    protected bool setParticipantSession(string PID)
    {
        PID = Session["PARTICIPANT_ID"] as string;
        SchoolInfo = Session["SCHOOL"] as Dictionary<string, string>;
        DesignatedCounselor = Session["DESIGNATED_COUNSELOR"] as string;
        if (string.IsNullOrEmpty(PID) || SchoolInfo == null || string.IsNullOrEmpty(DesignatedCounselor) || !PID.Equals(ParticipantId))
        {
            redirectLogin();
            return false;
        }
        return true;
    }

    protected bool setCounselorSession()
    {
        CounselorId = Session["COUNSELOR_ID"] as string;
        SchoolInfo = Session["SCHOOL"] as Dictionary<string, string>;
        if (string.IsNullOrEmpty(CounselorId) || SchoolInfo == null)
        {
            redirectLogin();
            return false;
        }
        return true;
    }

    protected void redirectLogin(){
        Response.Redirect(string.Format("./login_alt.aspx?p={0}&id={1}", new string[] { UserType, ParticipantId}));
    }

    protected bool validateUser(string UserType)
    {
        string PID = "";
        if (Session == null)
        {
            redirectLogin();
            return false;
        }
        if (UserType == "counselor") 
        {
            return setCounselorSession();
        }
        else if (UserType == "participant") 
        {
            return setParticipantSession(PID);
        }
        else
        {
            redirectLogin();
            return false;
        }
        
    }

    protected void getNavigation(Repeater RepeaterNav, string Role)
    {
        List<SiteMapNode> VerticalNav = new List<SiteMapNode>();
        SiteMapNodeCollection ChildNodes;
        if (SiteMap.CurrentNode == null) return;
        if (SiteMap.CurrentNode.HasChildNodes)
        {
            ChildNodes = SiteMap.CurrentNode.ChildNodes;
        }
        else if (SiteMap.CurrentNode.ParentNode.HasChildNodes)
        {
            ChildNodes = SiteMap.CurrentNode.ParentNode.ChildNodes;
        }
        else return;

        foreach (SiteMapNode n in ChildNodes)
        {
            if (n.Roles.Contains(Role)) VerticalNav.Add(n);
        }

        RepeaterNav.DataSource = VerticalNav;
        RepeaterNav.DataBind();
    }

    // this modifies the links on the vertical nav, which changes options based on user type and location of current page
    protected void getLinkAttr(object sender, RepeaterItemEventArgs e)
    {
        SiteMapNode node = e.Item.DataItem as SiteMapNode;
        HyperLink hp = e.Item.FindControl("HyperLink1") as HyperLink;

        var span = new HtmlGenericControl("span");
        span.Attributes["class"]="glyphicon glyphicon-chevron-right";
        hp.Controls.Add(span);
        hp.Controls.Add(new LiteralControl( "  " + node.Title));
        //hp.Text+=node.Title;

        // these are both jquery/ajax methods that call webmethods, so default urls are suppressed.
        if (node.Title == "Change Hours")
        {
            hp.Attributes.Add("data-toggle","modal");
            hp.Attributes.Add("data-target","#modal1");
            hp.Attributes.Add("id", "hours-toggle");
            hp.NavigateUrl="#";
        }
        // these are both jquery/ajax methods that call webmethods, so default urls are suppressed.
        else if (node.Title == "Send Email")
        {
            hp.Attributes.Add("data-toggle", "modal");
            hp.Attributes.Add("data-target", "#modal2");
            hp.Attributes.Add("id", "email-toggle");
            hp.NavigateUrl="#";
            if (SiteMap.CurrentNode.Title == "Dialog List") { hp.Attributes.Add("class", "hide part-depend"); }
        }
        else if (node.Title == "Change Resource Links")
        {
            hp.NavigateUrl=node.Url;
        }
        else 
        {
            if (SiteMap.CurrentNode.Title == "Dialog List") { hp.Attributes.Add("class", "hide part-depend"); }
            hp.NavigateUrl=node.Url + "?p=" + UserType + "&id=" + ParticipantId;
        }
    }

    public struct WeekdayHours
    {
        private string _hour;
        private string _weekday;
        private int _rank;

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public string Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }
        public string Weekday
        {
            get { return _weekday; }
            set { _weekday = value; }
        }
    }

    protected List<WeekdayHours> getHours(ref ebridgeEntities _db, ref Dictionary<string, string> SchoolInfo)
    {
        if (_db == null && SchoolInfo == null) return null;

        _db = new ebridgeEntities();

        string[] DayKey = new string[6] { "Title", "Mon", "Tue", "Wed", "Thur", "Fri" };

        string _code = SchoolInfo["code"] as string;

        SITE _site = _db.SITE.Where(s => s.ID == _code).OrderByDescending(s1 => s1.SID).First();

        string[] _hours = _db.SITE.First(s => s.SID == _site.SID).HOURS.Split('|');

        List<WeekdayHours> _list = new List<WeekdayHours>();

        for (int i = 0; i < _hours.Length; i++)
        {
            _list.Add(new WeekdayHours { Hour = _hours[i], Weekday = DayKey[i], Rank = i });
        }

        return _list;
    }

    public string getCounselorID()
    {
        if (CounselorId != null ) return CounselorId;
        else 
        { 
            redirectLogin();
            return null;
        }
    }

    public string getSchoolCode()
    {
        if (SchoolInfo != null) return SchoolInfo["code"];
        else 
        { 
            redirectLogin();
            return null;
        }
    }

    public string RenderControlToHtml(Control ControlToRender)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.IO.StringWriter stWriter = new System.IO.StringWriter(sb);
        System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stWriter);
        if (ControlToRender == null ) return "";
        ControlToRender.RenderControl(htmlWriter);
        return sb.ToString();
    }
}
