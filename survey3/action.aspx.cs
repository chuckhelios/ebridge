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
using System.IO;

public partial class survey3_action : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString[0] == "data")
        {
            if (Session["PARTICIPANT_ID"] == null) { Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the inivation email to access the site.</SPAN>"); return; }

            string ParticipantId = (string)Session["PARTICIPANT_ID"];
            int Index = int.Parse(Request.Form[0].Split(',')[0]);
            string QuestionCode = Request.Form[0].Split(',')[1];

            string SurveyResponses = ""; string _temp = "";
            for (int i = 1; i < Request.Form.Count; i++) // skip 0, index info
            {
                _temp = Request.Form[i];

                if (_temp.Length != 0)
                {
                    _temp = (_temp.Substring(_temp.Length - 1) == "," ? _temp.Substring(0, _temp.Length - 1) : _temp); // remove trailing ,
                }
                else
                {
                    _temp = ".";
                }

                SurveyResponses += _temp + "|";
            }
            

            if (_temp.Length != 0)
            {
                SurveyResponses = SurveyResponses.Substring(0, SurveyResponses.Length - 1); // remove trailing pipe
            }
            else
            {
                SurveyResponses = ".";
            }

            Db.Execute("DELETE FROM FOLLOWUP_RESPONSE_B WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + QuestionCode + "'");
            Db.Execute("INSERT INTO FOLLOWUP_RESPONSE_B VALUES ('" + ParticipantId + "','" + QuestionCode + "','" + SurveyResponses.Replace("'", "''") + "','" + DateTime.Now.ToString() + "')");


            if (QuestionCode == "ALC3")
            {
                if (SurveyResponses == "0|0|0")
                {
                    Db.Execute("DELETE FROM FOLLOWUP_RESPONSE_B WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC7'");
                    Db.Execute("INSERT INTO FOLLOWUP_RESPONSE_B VALUES ('" + ParticipantId + "','ALC7','0|0|0|0|0|0|0','" + DateTime.Now.ToString() + "')");
                    Index++;
                }
            }

            if (QuestionCode == "SER")
            {
                _temp = Request.Form["SER3"];
                if (!string.IsNullOrEmpty(_temp) && _temp != ".") {
                    Db.Execute("DELETE FROM FOLLOWUP_RESPONSE_B WHERE participant_id = '" + ParticipantId + "' AND question_code ='MHS'");
                    Db.Execute("INSERT INTO FOLLOWUP_RESPONSE_B VALUES ('" + ParticipantId + "','MHS','"+_temp+"','" + DateTime.Now.ToString() + "')");
                }
            }            

            if (QuestionCode == "QUAL")
            {
                if (Db.GetCount("SELECT COUNT(*) FROM FOLLOWUP_RESPONSE_B WHERE PARTICIPANT_ID = '" + ParticipantId + "' AND QUESTION_CODE = 'MHS'") != 0)
                {
                    _temp = Db.GetRecord("SELECT RESPONSE FROM FOLLOWUP_RESPONSE_B WHERE PARTICIPANT_ID = '" + ParticipantId + "' AND QUESTION_CODE = 'MHS'")[0];
                    if (_temp != "0")  {
                        Index++;
                    }
                }
            }

            Index++;

            Response.Redirect("screen.aspx?p=" + Index.ToString());
        }
    }
}