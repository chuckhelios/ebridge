using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dialog_list_alt : WebformGeneric
{

    ebridgeEntities _db;

    public class SummaryStat
    {
        public string Description { get; set; }
        public string Value { get; set; }

        public SummaryStat(string qName, string qString)
        {
            Description = qName;
            Value = Db.GetCount(qString).ToString();
        }
    }

    public class DialogSummary
    {
        public string Id { get; set; }
        public string AssignedTo { get; set; }
        public bool NeedAttn { get; set; }
        public DateTime SurveyCompleteDate { get; set; }

        public DateTime LastPost { get; set; }
        public string LastPostContent { get; set; }
        public int StudentPostCnt { get; set; }

        public DateTime LastCounselorPost { get; set; }
        public string LastCounselorPostContent { get; set; }
        public int CounselorPostCnt { get; set; }

        public DialogSummary(string ParticipantId, string CounselorSite)
        {
            Id = ParticipantId;

            AssignedTo=Utility.GetStatus(ParticipantId, "COUNSELOR");

            string QueryString = "SELECT username FROM counselor WHERE id = '{0}' and site='{1}'";
            AssignedTo = Db.GetRecord(String.Format(QueryString, new string[2] { AssignedTo, CounselorSite }))[0];

            string CompleteTime = Utility.GetLog(ParticipantId, "SURVEY COMPLETED");
            if (CompleteTime == "NULL") CompleteTime = DateTime.Now.ToString();


            SurveyCompleteDate = DateTime.Parse(CompleteTime);

            string[] _lpp = Db.GetRecord("SELECT date_time, message_body FROM message_alt WHERE id = (SELECT MAX(id) FROM message_alt WHERE from_id = '" + ParticipantId + "')");
            if (_lpp != null) 
            { 
                LastPost = DateTime.Parse(_lpp[0]); 
                LastPostContent = _lpp[1]; 
                if(LastPostContent.Length > 250) LastPostContent = LastPostContent.Substring(0, 250) + " ...";
            }

            string[] _lpc = Db.GetRecord("SELECT date_time, message_body FROM message_alt WHERE id = (SELECT MAX(id) FROM message_alt WHERE to_id = '" + ParticipantId + "')");
            if (_lpc != null)
            {
                 LastCounselorPost = DateTime.Parse(_lpc[0]);
                 LastCounselorPostContent = _lpc[1];
                if (LastCounselorPostContent.Length > 150) LastCounselorPostContent = LastCounselorPostContent.Substring(0, 150) + " ...";
                if (LastCounselorPostContent == "_FIRST_VISIT_HIDDEN_") LastCounselorPostContent = "<SPAN style='color:red'>student awaiting for greeting</SPAN>";
            }

            string[] _mpt = Db.GetRecord("SELECT from_id, to_id FROM message_alt WHERE id = (SELECT MAX(id) FROM message_alt WHERE from_id = '" + ParticipantId + "' OR to_id = '" + ParticipantId + "')");
            if (_mpt == null || _mpt[0] != "NULL") NeedAttn= true;
            else NeedAttn = false;

            StudentPostCnt=  Db.GetCount("SELECT COUNT(*) FROM message_alt WHERE from_id = '" + ParticipantId + "'");

            CounselorPostCnt=  Db.GetCount("SELECT COUNT(*) FROM message_alt WHERE to_id = '" + ParticipantId + "'");

        }
    }

    protected string abbrString(string Msg)
    {
        Msg = HttpUtility.HtmlDecode(Msg);
        if (Msg.Length>20) return Msg.Substring(0, 20) + "...";
        else return Msg;
    }

    protected void getStats(string CounselorSite)
    {
        List<SummaryStat> Stats = new List<SummaryStat>();
        Dictionary<string, string> QueryStrings = Helper.ListQueryStrings(CounselorSite);
        foreach (KeyValuePair<string, string> query in QueryStrings)
        {
            Stats.Add(new SummaryStat(query.Key, query.Value));
            //Stats = Stats.OrderBy( x => x.Description).ToList();
        }
        Repeater1.DataSource= Stats;
        Repeater1.DataBind();

    }

    protected void getDialogs(string CounselorSite)
    {
        List<DialogSummary> Dialogs = new List<DialogSummary>();

        string QueryString = @"SELECT p.id FROM participant p, status s WHERE p.id = s.id AND s.status_code = 'INTERVENTION'
        AND s.status_value = '1' and right(p.id,1)='{0}'";

        string[][] Participants = Db.GetRecords(string.Format(QueryString, CounselorSite));

        if (Participants == null)
        {
            Page.Controls.Remove(Repeater2);
            Label1.Text = "No students are eligible for the intervention. Check back later.";
            return;
        }
        
        Label1.Text="Participating Students";
        
        for (int i = 0; i < Participants.Length; i++)
        {
            string ParticipantId= Participants[i][0];
            Dialogs.Add(new DialogSummary(ParticipantId, CounselorSite));
        }
        Repeater2.DataSource=Dialogs;
        Repeater2.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.LogActivity("counselor", "PARTICIPANT LIST PAGE VISITED", Request.UserHostAddress + "|" + Request.UserAgent);

        SchoolInfo = Session["SCHOOL"] as Dictionary<string, string>;
        CounselorId = Session["COUNSELOR_ID"] as string;

        _db = new ebridgeEntities();

        if (SchoolInfo == null || string.IsNullOrEmpty(CounselorId))
        {
            Session.Clear(); Response.Redirect(String.Format("login_alt.aspx?p=counselor")); return;
        }
        UserType="counselor";

        getNavigation(RepeaterNav, "counselor");

        getStats(SchoolInfo["code"]);

        getDialogs(SchoolInfo["code"]);

        RepeaterHours.DataSource= getHours(ref _db, ref SchoolInfo);
        
        RepeaterHours.DataBind();

    }
}