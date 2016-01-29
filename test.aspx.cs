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

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString.Count == 0) { Response.Write("Provide Participant ID."); return; }
        string IDs_TO_REMOVE = "'"+Request.QueryString[0]+"'";


        if ((bool)Application["DEBUG"]) Utility.SendPHPMail("kzheng@umich.edu", "ebridgeteam@umich.edu", "The eBridge Team", "eBridge: Clearing ParticipantId: " + IDs_TO_REMOVE, "Test");
        else {Response.Write("Invalid Request."); return;}

            Db.Execute("DELETE FROM activity_log WHERE id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM followup_response WHERE participant_id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM screening_response WHERE participant_id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM message WHERE from_id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM message WHERE to_id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM online_status WHERE id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM status WHERE id IN (" + IDs_TO_REMOVE + ")");
            Db.Execute("DELETE FROM schedule WHERE participant_id IN (" + IDs_TO_REMOVE + ")");

            //Db.Execute("DELETE FROM participant WHERE id IN (" + IDs_TO_REMOVE + ")");

            Response.Write("Cleaned.");
    }
}