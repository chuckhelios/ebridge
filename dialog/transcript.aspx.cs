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

public partial class dialog_transcript : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CounselorSchool = Session["SCHOOL"] as string;
        if (string.IsNullOrEmpty(CounselorSchool)) Response.Redirect("list.aspx");
        
        string QueryString = @"SELECT * FROM (SELECT DISTINCT from_id FROM message 
        WHERE from_id <> '' UNION SELECT DISTINCT to_id FROM message WHERE to_id <> '') ids
        WHERE RIGHT(IDS.FROM_ID, 1)='{0}'";
        string[][] _pid = Db.GetRecords(String.Format(QueryString, CounselorSchool));

        /* select * from (SELECT DISTINCT from_id FROM message 
        WHERE from_id <> '' UNION SELECT DISTINCT to_id FROM message WHERE to_id <> '') ids
        where right(ids.from_id,1)='m';*/
        string[][] _msg; string Sender; string Message; string DateAndTime;

        Response.Write(PageElements.DialogPanelHeader());

        Response.Write("<TABLE width='700px' align='center' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px'>");

        for (int i = 0; i < _pid.Length; i++)
        {
            Response.Write("<TR height='30px'><TD></TD></TR>");
            Response.Write("<TR><TD><B>" + _pid[i][0] + "</B></TD></TR>");
            Response.Write("<TR height='20px'><TD></TD></TR>");

            _msg = Db.GetRecords("SELECT to_id, message_body, date_time FROM message WHERE from_id = '" + _pid[i][0] + "' OR to_id = '" + _pid[i][0] + "' ORDER BY id");

            for (int j = 0; j < _msg.Length; j++)
            {
                if (_msg[j][0] == "NULL") Sender = "student"; else Sender = "counselor"; Message = _msg[j][1]; DateAndTime = _msg[j][2];

                //Response.Write(_pid[i][0] + "|" + Sender + "|" + Message + "|" + DateAndTime + "<BR>");

                Response.Write("<TR" + (j % 2 == 1 ? "" : " style='background-color:#D3D3D3'") + ">"
                    + "<TD valign='top'>" + Sender + "</TD><TD valign='top' style='padding-left:20px;padding-right:20px'>" + Message.Replace("\n", "<P>") 
                    + " <SPAN style='font-size:10px;color:#808080'>" + "</SPAN></TD><TD valign='top' width='80px'>" + DateAndTime + "</TD></TR>");
                Response.Write("<TR height='10px'><TD></TD></TR>");
            }

            
            Response.Flush();
        }

        Response.Write("</TABLE>");
        Response.Write("<BR>&nbsp;<BR>");
        Response.Write(PageElements.PageFooter(null));
    }
}
