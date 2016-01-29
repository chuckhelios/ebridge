using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dialog_survey_status : System.Web.UI.UserControl
{

    private string participantId;

    public string ParticipantId
    {
        get { return participantId; }
        set { participantId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ebridgeEntities _db = new ebridgeEntities();
            IQueryable<SCREENING_RESPONSE> resp = _db.SCREENING_RESPONSE.Where(sr => sr.PARTICIPANT_ID == ParticipantId);
            var pg = resp.Join(_db.PAGE_REF, r => r.QUESTION_CODE, p => p.value, (r, p) => new { Response = r, Page = p }).ToList();
            List<PAGE_REF> survey_pages = new List<PAGE_REF>();
            foreach (var p in pg)
            {
                //should be as many questions as there are responses separated by pipes
                string[] responses = p.Response.RESPONSE.Split('|');
                PAGE_REF page_ref = p.Page;
                IOrderedEnumerable<QUESTION_REF> questions = page_ref.QUESTION_REF.OrderBy(q => q.qid);
                int cnt = 0;
                foreach (QUESTION_REF quest in questions)
                {
                    string response = "";
                    if (cnt < responses.Length) response = responses[cnt];
                    quest.setChosenResponses(response, ref _db);
                    cnt++;
                }
                page_ref.OrderedQuestions = questions;
                survey_pages.Add(page_ref);
            }
            Repeater1.DataSource = survey_pages;
            Repeater1.DataBind();
        }
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        PAGE_REF page = (PAGE_REF)e.Item.DataItem;
        IEnumerable<QUESTION_REF> orderedquestions = page.OrderedQuestions;
        Repeater SubRepeater1 =(Repeater)item.FindControl("SubRepeater1");
        SubRepeater1.DataSource=orderedquestions.ToList();
        SubRepeater1.DataBind();
    }
}