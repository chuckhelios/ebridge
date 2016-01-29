using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dialog_dialog_p : WebformGeneric
{

    List<MESSAGE_ALT> MsgQueue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack || MsgQueue == null)
        {
            //Page.Title= SiteMap.CurrentNode.Title;

            Debug = (bool)Application.Get("DEBUG");
            //either login as a counselor or logged in as a student. 
            if (!validateUrlParams())
            {
                Label Label1 = new Label();
                Label1.Text = "invalid request: please use url from email to access page";
                PlaceHolder1.Controls.AddAt(0,Label1);
            }

            if (!Debug && !validateUser(UserType)) return;

            getNavigation(RepeaterNav, UserType);

        }

        if (UserType == "counselor") 
        {
            Utility.LogActivity(ParticipantId, "COUNSELOR: MSG THREAD READ", Request.UserHostAddress + "|" + Request.UserAgent); 
        }
        else if (UserType == "participant")
        {
            Utility.LogActivity(ParticipantId, "STUDENT: MSG THREAD READ", Request.UserHostAddress + "|" + Request.UserAgent);
        }
        getThreadMessages();
    }

    protected string convertUser(string fromId, string toId)
    {
        if (fromId == null)
        {
            return "counselor";
        }
        else if (toId == null)
        {
            return "user";
        }
        else return "user";
    }

    protected string getParticipantId(string fromId, string toId)
    {
        if (fromId != null)
        {
            return "Counselor";
        }
        else if (toId != null)
        {
            return toId;
        }
        else return "";
    }

    protected void getThreadMessages()
    {
        
        if (ParticipantId == null) return;
             
        ebridgeEntities _db = new ebridgeEntities();
        PARTICIPANT Part = _db.PARTICIPANT.SingleOrDefault<PARTICIPANT>(p=>p.ID==ParticipantId);
        IQueryable<MESSAGE_ALT> Msgs = _db.MESSAGE_ALT.Where<MESSAGE_ALT>(m => m.FROM_ID == ParticipantId || m.TO_ID == ParticipantId).OrderByDescending(m => m.DATE_TIME);

        if (Msgs == null || Part == null) return;

        foreach (MESSAGE_ALT m in Msgs) m.setPosition();

        ShowResources1.DisplayColumns = 1;

        FormView1.DataSource=new List<PARTICIPANT>{Part};
        FormView1.DataBind();

        Repeater2.DataSource = Msgs.ToList<MESSAGE_ALT>();
        Repeater2.DataBind();

    }


    protected void SendMessage(object sender, EventArgs e)
    {
        string DialogBody = CKEditor1.Text.Trim();
        if (String.IsNullOrEmpty(DialogBody)) return;

        ebridgeEntities _db = new ebridgeEntities();

        MESSAGE_ALT newMessage = new MESSAGE_ALT();

        newMessage.MESSAGE_BODY = HttpUtility.HtmlEncode(DialogBody);
        newMessage.DATE_TIME = DateTime.Now;

        if (UserType == "participant")
        {
            newMessage.FROM_ID = ParticipantId;
        }
        else if (UserType == "counselor")
        {
            newMessage.TO_ID = ParticipantId;
        }
        _db.MESSAGE_ALT.Add(newMessage);
        _db.SaveChanges();
        getThreadMessages();
    }

}