using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ebridge_dev
{
    public partial class Global : System.Web.HttpApplication
    {    
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["DEBUG"] = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {   // can use this later to check if student or counselor, and direct/redirect based on permissions.

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

            // session is not available in this scope - the request is authenticated before the session start

        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // session variable is available here
            //String[] CounselorPages = { "send_email", "counselor_post", "counselor_logout" };

            //if (Request.QueryString != null && Request.QueryString.Count > 0) {String PageTitle = Request.QueryString.ToString();}
            // accessing the session variable when the key doesn't exist will cause an exception to be thrown.
            string TimeoutResponse =
            @"<SPAN style='font-family:verdana;font-size:10px;color:red'>Session Timed Out: 
            Please login or use the web address you received in the invitation email to access the site. 
            </A></SPAN>";
            try 
            {  
                if (Session == null ) return;
                string UpdateString = "UPDATE ONLINE_STATUS SET DATE_TIME = '{0}' WHERE ID='{1}'";

                // if a counselor with valid session makes request, then update the online status time to current time
                // run a job on database to remove those with current - lastactivity greater than timeout span
                string LoginType = (string)Session["PARTICIPANT_LOGIN"];
                // if the session variable does not have the key, a null will be returned, no exceptions thrown.
                //string ParticipantId = (string)Session["PARTICIPANT_ID"];
                if (LoginType == "Y") 
                {
                    string ParticipantId = (string)Session["PARTICIPANT_ID"];
                    Db.Execute(string.Format(UpdateString, new string[2] {DateTime.Now.ToString(), ParticipantId})); return;
                }
                else if (LoginType == "N")
                {
                    string CounselorId = (string)Session["COUNSELOR_ID"];
                    string CounselorSite = (string)Session["SCHOOL"];
                    Db.Execute(string.Format(UpdateString+" and SITE='{2}'", new string[3] {DateTime.Now.ToString(), CounselorId, CounselorSite}));
                }
                else
                {
                    // do nothing
                }
            }
            catch (HttpException exp)
            {
                Utility.LogActivity("unknown", "SESSION_REFRESH EXCEPTION", exp.Message);
                Response.Write(TimeoutResponse); return;  
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            string TimeoutResponse =
            @"<SPAN style='font-family:verdana;font-size:10px;color:red'>Session Timed Out: 
            Please login or use the web address you received in the invitation email to access the site. 
            </A></SPAN>";
            try
            {
                if (Session == null) return;
                string UpdateString = "DELETE FROM ONLINE_STATUS WHERE ID='{0}'";
                string LoginType = (string)Session["PARTICIPANT_LOGIN"];
                if (LoginType == "Y")
                {
                    string ParticipantId = (string)Session["PARTICIPANT_ID"];
                    Db.Execute(string.Format(UpdateString, ParticipantId));
                    Utility.LogActivity(ParticipantId, "PARTICIPANT TIMEOUT", Request.UserHostAddress + "|" + Request.UserAgent);
                    return;
                }
                else if (LoginType == "N")
                {
                    string CounselorId = (string)Session["COUNSELOR_ID"];
                    string CounselorSite = (string)Session["SCHOOL"];
                    Db.Execute(string.Format(UpdateString + " and SITE='{1}'", new string[2] { CounselorId, CounselorSite }));
                    Utility.LogActivity(CounselorId + "_" + CounselorSite, "COUNSELOR TIMEOUT", Request.UserHostAddress + "|" + Request.UserAgent);
                }
                else
                {
                    // do nothing
                }
            }
            catch (HttpException exp)
            {
                Utility.LogActivity("unknown", "SESSION_END EXCEPTION", exp.Message);
                Response.Write(TimeoutResponse); 
                return;
            }

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}