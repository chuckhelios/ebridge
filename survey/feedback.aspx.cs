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

public partial class survey_feedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool DEBUG = (bool)Application["DEBUG"];
        string ParticipantId = Request.QueryString[0];

        Response.Write("<HTML><BODY style='font-family:arial;font-size:12px;padding-left:4px;padding-top:10px'>");

        if (Request.QueryString[1] == "distress")
        {
            //log activity
            Utility.LogActivity(ParticipantId, "DISTRESS FEEDBACK VIEWED", Request.UserHostAddress + "|" + Request.UserAgent);

            double PHQ9_SCORE = Logic.PHQ9Score(ParticipantId);
            double PHQ2_SCORE = Logic.PHQ2Score(ParticipantId);
            string PHQ9_PERCENTILE = Logic.PHQ9Percentile(PHQ9_SCORE).ToString();

            string[] PHQ7_HM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ9_HM'")[0].Split('|');
            
            //commented out due to exclusion of hm from phq9 page
            //string[] HM = (PHQ7_HM[6] + "|" + PHQ7_HM[7]).Split('|');

            Response.Write("Here's how your distress compares to other students: Your score is higher than (" + PHQ9_PERCENTILE + "%) of college students.");
            if (DEBUG) Response.Write(" <SPAN style='color:red'>(" + PHQ9_SCORE.ToString() + ")</SPAN>");

            Response.Write("<CENTER><IMG width='450px' src='./graph/phq/dep_" + Math.Floor(PHQ9_SCORE).ToString() + ".png'></CENTER>");

            if (PHQ2_SCORE >= 3)
            {
                Response.Write("You reported emotional distress that may be creating a problem for you. The kinds of things you reported being most bothered by include "
                    + "<U>feeling down</U>, <U>depressed</U> or <U>hopeless</U>.<P>"
                    + "It's not uncommon for students reporting similar levels of distress to wonder what they can do to manage how they are feeling.<P>");
            }
            else
            {
                Response.Write("You did not report a high level of distress.<P>"
                    + "The good news is that your distress level is within a typical range. However, everyone has times now and then when they experience distress."
                    + "Something to consider is figuring out what might be helpful to manage this occasional distress.<P>");
            }

            //Response.Write("<B>Has your level of distress affected your academics and/or social life?</B><P>");

            //commented out due to exclusion of hm from phq9 page
            
            /*
            if (DEBUG) Response.Write(" <SPAN style='color:red'>(HM0: " + HM[0] + ", HM1: " + HM[1] + ")</SPAN><P>");

            if (HM[0] == "--") HM[0] = "0"; int HM_0 = int.Parse(HM[0]);
            if (HM[1] == "--") HM[1] = "0"; int HM_1 = int.Parse(HM[1]);

            //If response = 0 for both:
            if (HM_0 == 0 && HM_1 == 0)
            {
                Response.Write("Your report suggests your level of distress may not be having a negative impact on your academic performance and social life right now.<P>");
            }

            //If response = 1 or higher on either or both
            if (HM_0 >= 1 || HM_1 >= 1)
            {
                string _sb = "";

                if (HM_0 >= 1 && HM_1 >= 1) _sb = "social and academic";
                if (HM_0 < 1) _sb = "academic";
                if (HM_1 < 1) _sb = "social";

                Response.Write("Your level of distress has been getting in the way of your " + _sb + " life. "
                    + "It's common for students who are experiencing similar levels of distress to struggle from time to time.<P>"
                    + "It may be worth figuring out ways to reduce your distress and its impact on things that are important to you.");
            }
            */
        }
        

        if (Request.QueryString[1] == "alcohol")
        {
            //log activity
            Utility.LogActivity(ParticipantId, "ALC FEEDBACK VIEWED", Request.UserHostAddress + "|" + Request.UserAgent);

            double ALC_SCORE = Logic.ALCScore(ParticipantId);

            Response.Write("Here's information about the level of alcohol use your reported:");
            if (DEBUG) Response.Write(" <SPAN style='color:red'>(" + ALC_SCORE.ToString() + ")</SPAN>");

            Response.Write("<BR><SPAN style='font-size:20px'>&nbsp;</SPAN><BR><CENTER><IMG width='450px' src='./graph/alc/alc_" + Math.Floor(ALC_SCORE).ToString() + ".png'></CENTER><BR>&nbsp;<BR>");

            if (ALC_SCORE >= 8)
            {
                Response.Write("Everyone is different, but students who drink at the level you reported are more likely to experience some of the following:<P>"
                    + "<UL><LI>Embarrassing myself or others</LI>"
                    + "<LI>Vomiting or blacking out from the amount of alcohol drank</LI>"
                    + "<LI>Neglecting or letting down my family or friends</LI>"
                    + "<LI>Missing class or work</LI>"
                    + "<LI>Having to drink or use more to get the desired effect</LI>"
                    + "<LI>Engaging in unprotected/unwanted sex or getting STDs</LI></UL>"
                    + "Something to think about is whether drinking is leading to negative consequences for you and whether or not you think it may be worth making any changes "
                    + "(e.g. cutting back, drinking less often, drinking less when you drink, quitting altogether).<P>"
                    + "Or perhaps you may not have experienced any negative consequences and this may be something to look out for and prevent in the future.");
            }
            else
            {
                Response.Write("Based on your responses it appears your alcohol use is within the minimal risk range. Students who report similar levels of drinking typically "
                    + "don't experience severe negative consequences related to alcohol use.");
            }

        }

        if (Request.QueryString[1] == "gv")
        {
            //log activity
            Utility.LogActivity(ParticipantId, "GV FEEDBACK VIEWED", Request.UserHostAddress + "|" + Request.UserAgent);

            string[] Goal_Value = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='GOA_VAL'")[0].Split('|');
            if (Goal_Value[0] == "--") Goal_Value[0] = "Not provided."; if (Goal_Value[1] == "--") Goal_Value[1] = "Not provided.";

            string GOALS = Goal_Value[0].ToLower().Replace(",", ", ");
            string VALUES = Goal_Value[1].ToLower().Replace(",", ", ");

            Response.Write("<B>Your goals are: <SPAN style='color:red'>" + GOALS + "</SPAN><P>");
            Response.Write("Your values are: <SPAN style='color:red'>" + VALUES + "</SPAN></B><P>");

            Response.Write("Thanks for taking the time to think about your top goals and values. Some people find it helpful to be clear about what’s truly important to them.<P>"
                + "Like others you may find this can help you in deciding whether you want to make any changes in your life or whether you are ok with the way things are going for you now.");
        }

        Response.Write("</BODY></HTML>");
    }

    
}
