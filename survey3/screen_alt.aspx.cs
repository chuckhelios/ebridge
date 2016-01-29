using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Net;

public partial class survey3_screen_alt : System.Web.UI.Page 
{

    private int Index;

    private string[] QuestionCodes;
    private string ParticipantId;
    private Dictionary<string, string> SchoolInfo;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            Index = int.Parse(Request.QueryString[0]);
        }
        catch
        {
            Response.Write("<SPAN style='font-family:verdana;font-size:10px;color:red'>Invalid web page request. Please use the web address you received in the invitation email to access the site.</SPAN>"); return;
            
        }
        
        QuestionCodes = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey3\\q3.txt").Split(',');

        if (Index >= QuestionCodes.Length) 
        {
            Utility.LogActivity(ParticipantId, "6MONTH SURVEY COMPLETED", "all questions completed");
    
        
            Response.Redirect("receipt.aspx?p=" + ParticipantId);

            return; 
        }
        //Style s = new Style();
        //s.BackColor = System.Drawing.Color.Red;
        //Image1.MergeStyle(s);
        
        // get number of values from query
        int qCnt= QuestionCodes.Length;

        Hidden1.Value = Index.ToString(); 

        Image1.ImageUrl= "../image/mental.png";
        Image1.CssClass= "displayed";

        Image2.ImageUrl= string.Format("images/{0}.jpg", QuestionCodes[Index]);
        Image2.CssClass= "displayed";

        AttributeCollection ProgressAttr= progress_bar.Attributes;

        ProgressAttr.Add("aria-valuemax", qCnt.ToString());
        ProgressAttr.Add("aria-valuenow", Index.ToString());
        ProgressAttr.Add("style", string.Format("width: {0}%;", ((float)Index/qCnt * 100)).ToString());
        progress_bar.InnerHtml= Index.ToString() + "/" + qCnt.ToString();

        string QueryString = @"SELECT QID, CONTENT, P.VALUE FROM PAGE_REF P, QUESTION_REF Q WHERE P.PID=Q.PID AND P.VALUE = 'PHQ9_HM' ORDER BY QID ASC;";

        string[][] qContents = Db.GetRecords(QueryString);

        BindListView(qContents);

    }

    private void BindListView(string[][] _qs)
    {
        using (DataTable dt = new DataTable())
        {
            dt.Columns.Add("Order", typeof(string));
            dt.Columns.Add("Content", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Scale", typeof(string));
            
            string Querystring = @"SELECT COUNT(R.RID) FROM RESPONSE_X_QUESTION RQ, RESPONSE_REF R WHERE RQ.QID={0} AND RQ.RID=R.RID";
            for (int i = 0; i < _qs.Length; i++) 
                { 
                    string[] _q = _qs[i];
                    int _r = Db.GetCount(string.Format(Querystring, _q[0]));
                    dt.Rows.Add(i, _q[1], _q[2], _r);
                }

            DataView dv = new DataView(dt);
            ListView1.GroupItemCount = _qs.Length;
            ListView1.DataSource = dv;
            ListView1.DataBind();
        }

    }
}