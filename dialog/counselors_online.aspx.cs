using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class chat_counselors_online : System.Web.UI.Page
{

    Dictionary<string, string> SchoolInfo;

    // chat for students view
    // http://localhost:53422/chat/?p=1&p1=1&p2=G6tE9FJ9gi&p3=3

    // schedule for students view
    // http://localhost:53422/chat/schedule.aspx?p=C001


    protected void Page_Load(object sender, EventArgs e)
    {
        string ParticipantId = Session["PARTICIPANT_ID"] as string;
        string DesignatedCounselor = Session["DESIGNATED_COUNSELOR"] as string;
        SchoolInfo= Session["SCHOOL"] as Dictionary<string, string> ;

        // if session is expired, then don't give them the chat url. (maybe should redirect to login - js?)
        if (string.IsNullOrEmpty(ParticipantId) || string.IsNullOrEmpty(DesignatedCounselor)) return;

        // get count of counselors online
        // bad practice to have site info in chat, where is should be only found in counselor
        // but this eliminates need to join, may be better for speed. 
        
        Response.Buffer=false;
        string ChatUrl= Helper.GetChatUrl(ParticipantId, DesignatedCounselor);        
        Response.Write(ChatUrl);
        Response.Flush();

        //WE'RE NO LONGER USING THE SCHEDULER, USE THE AVAILABLE TIMES INSTEAD

        // find the id of the most available counselor with least appointments

        //string ScheduleQuery="select couselor_id from schedule group by couselor_id having count(*) " 
        //+"=(select max(appt.apptcnts) from (select couselor_id, count(*) as apptcnts "
        //+"from schedule group by couselor_id) as appt)";

        //string[] CounselorInfo=Db.GetRecord(ScheduleQuery);

        //string ScheduleUrl="<A href='../chat/schedule.aspx?cid={0}' target='_blank' style='color:blue'>";

        //ScheduleUrl=String.Format(ScheduleUrl,CounselorInfo);

        //Response.Write(ScheduleUrl);
        //Response.Write("<SPAN id='counselors' style='color:blue;text-decoration:underline'><B>No Counselors Available: Schedule Chat</B></SPAN>");
        //Response.Write("</A>");
        //Response.Flush();        
        

    }
}