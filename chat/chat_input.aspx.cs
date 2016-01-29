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

public partial class chat_chat_input : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["ChatDisplayMode"] = Request.QueryString[0];
        ViewState["ParticipantId"] = Request.QueryString[1];

        if (!Page.IsPostBack)
        {
            BindMessagesView();
            Repeater1.EnableDynamicData(typeof(DataTable));
        }
        //this.Form.Action="action.aspx?p=chat_input&p1=" + ChatApptId + "&p2=" + ChatDisplayMode + "&p3=" + ParticipantId;
    }

    private void FillDataTable(string[][] _chat, ref DataTable dtMessage)
    {
        string ParticipantLink = "<A href='../dialog/frame.aspx?p=counselor&p1={0} target='_blank'>{0}</A>";
        if (_chat != null)
        {
            foreach (string[] _c in _chat)
            {
                DataRow dtRow = dtMessage.NewRow();

                if (_c[0] == "NULL") { dtRow["Name"] = "Counselor"; dtRow["Position"] = "left"; dtRow["Role"] = "counselor"; }
                else { dtRow["Name"] = ((string)ViewState["ChatDisplayMode"] == "c" ? string.Format(ParticipantLink, ViewState["ParticipantId"]) : "Me"); dtRow["Position"] = "right"; dtRow["Role"] = "participant"; }

                dtRow["Message"] = HttpUtility.HtmlDecode(_c[1]).Replace("[CHAT]", "");
                dtRow["Time"] = DateTime.Parse(_c[2]);
                dtRow["MsgID"] = _c[3];
                dtMessage.Rows.Add(dtRow);
            }
            if (ViewState["MessageLast"] != null)
            {
                //sort the datatable
                DataView dvMessage = dtMessage.DefaultView;
                dvMessage.Sort = "Time desc";
                dtMessage = dvMessage.ToTable();
            }
            ViewState["MessageLast"] = _chat[0];
            Repeater1.DataSource = dtMessage;
            Repeater1.DataBind();
        }
    }

    private DataTable CreateDataTable()
    {
        DataTable dtMessage = new DataTable("MessageTable");

        dtMessage.Columns.Add(new DataColumn("Name", typeof(string)));
        dtMessage.Columns.Add(new DataColumn("Time", typeof(DateTime)));
        dtMessage.Columns.Add(new DataColumn("Message", typeof(string)));
        dtMessage.Columns.Add(new DataColumn("Position", typeof(string)));
        dtMessage.Columns.Add(new DataColumn("Role", typeof(string)));
        dtMessage.Columns.Add(new DataColumn("MsgID", typeof(int)));
        
        return dtMessage;
    }

    private void BindMessagesView()
    {
        DataTable dtMessage = null;
        string QueryString;

        QueryString = @"SELECT FROM_ID, MESSAGE_BODY, DATE_TIME, ID FROM MESSAGE_ALT WHERE FROM_ID = '{0}' AND REMARK='1' UNION 
                            SELECT FROM_ID, MESSAGE_BODY, DATE_TIME, ID FROM MESSAGE_ALT WHERE TO_ID = '{0}' AND REMARK='1' ORDER BY ID DESC";

        string[][] _chat = Db.GetRecords(string.Format(QueryString, ViewState["ParticipantId"]));

        if (ViewState["MessageTable"] == null)
        {
            dtMessage= CreateDataTable();
            ViewState["MessageTable"] = dtMessage;
        }
        else
        {
            dtMessage = (DataTable)ViewState["MessageTable"];
        }
        FillDataTable(_chat, ref dtMessage);
        // works with empty datatable.
    }

    private void UpdateMessagesView()
    {
        // Precondition: ViewState["MessageLast"] exists, and dtMessages is not empty
        DataTable dtMessage = null;
        string QueryString;

        // if the view was empty prior then bind messages
        dtMessage = (DataTable)ViewState["MessageTable"];
        string[] dtRow = (string[])ViewState["MessageLast"];

        QueryString = @"SELECT FROM_ID, MESSAGE_BODY, DATE_TIME, ID FROM MESSAGE_ALT WHERE FROM_ID = '{0}' AND DATE_TIME >= '{1}' AND ID != '{2}' AND REMARK='1' UNION 
                            SELECT FROM_ID, MESSAGE_BODY, DATE_TIME, ID FROM MESSAGE_ALT WHERE TO_ID = '{0}' AND DATE_TIME >= '{1}' AND ID != '{2}' AND REMARK='1' ORDER BY ID ASC";
        string[][] _chat = Db.GetRecords(string.Format(QueryString, new string[3] { (string)ViewState["ParticipantId"], dtRow[2], dtRow[3] }));

        FillDataTable(_chat, ref dtMessage);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string DialogBody = CKEditor1.Text.Trim();
        if (String.IsNullOrEmpty(DialogBody)) return;

        DataTable dtMessage = (DataTable)ViewState["MessageTable"];
        string QueryString = "INSERT INTO MESSAGE_ALT ({0}, MESSAGE_BODY, DATE_TIME, REMARK) VALUES ('{1}', '{2}', '{3}', '{4}')";

        if ((string)ViewState["ChatDisplayMode"] != "c")
        { Db.Execute(string.Format(QueryString, new string[5] { "FROM_ID", (string)ViewState["ParticipantId"], "[CHAT] " + DialogBody, DateTime.Now.ToString(), (string)ViewState["ChatAppId"] })); }
        else Db.Execute(string.Format(QueryString, new string[5] { "TO_ID", (string)ViewState["ParticipantId"], "[CHAT] " + DialogBody, DateTime.Now.ToString(), (string)ViewState["ChatAppId"] }));

        if (ViewState["MessageLast"] == null) { BindMessagesView(); }
        else { UpdateMessagesView(); }
    }
    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        DateTimeLabel1.Text = DateTime.Now.ToString();
        if (ViewState["MessageLast"] == null) { BindMessagesView(); }
        else { UpdateMessagesView(); }
    }
    protected void UpdateTimer_Tick(object sender, EventArgs e)
    {
        DateTimeLabel1.Text = DateTime.Now.ToString();
        if (ViewState["MessageLast"] == null) { BindMessagesView(); }
        else { UpdateMessagesView(); }

    }
}
