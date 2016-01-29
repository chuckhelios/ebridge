using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class dialog_survey_status : System.Web.UI.Page
{
    public static string PrintValue(string ParticipantId)
    {
        //string _style = " style='cursor:hand;cursor:pointer' onmouseover=\"this.style.backgroundColor='yellow'\" onmouseout=\"this.style.backgroundColor='white'\"";

        string _o = "<P>"
            + "<P>Depression risk&mdash;<SPAN style='color:red'>" + Logic.PHQ9Severity(ParticipantId) + "</SPAN> (" + Math.Round(Logic.PHQ9Score(ParticipantId), 1) + ")";

        _o += "<P>Alcohol risk&mdash;<SPAN style='color:red'>" + Logic.ALCSeverity(ParticipantId) + "</SPAN> (" + Math.Round(Logic.ALCScore(ParticipantId), 1) + ")";

        string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER'")[0].Split('|');

        //bool CurrentlyTakingPrescribedMedication = false; if (_ser[0] == "0") CurrentlyTakingPrescribedMedication = true;
        //bool CurrentlyReceivingCounselingOrTherapy = false; if (_ser[1] == "0") CurrentlyReceivingCounselingOrTherapy = true;

        _o += "<P><SPAN " + PageElements.DrawMessage("Criteria", ParticipantId) + ">Eligibility for intervention</SPAN>: <SPAN style='color:red'>" + Logic.InterventionEligibility(ParticipantId).ToString() + "</SPAN><P>";

        string[] _type = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\survey\\q.txt").Split(','); string[] _value = null, _v = null;

        for (int i = 0; i < _type.Length; i++)
        {
            _v = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='" + _type[i] + "'");

            if (_v != null) _value = _v[0].Split('|');

            if (_type[i] == "DEM" && _v != null) _o += "<P>Age&mdash;" + Logic.CheckCode("AGE", _value[0]) + "<BR>Gender&mdash;" + Logic.CheckCode("GENDER", _value[1]) + "<BR>Race&mdash;" + _value[2].Replace(",", ", ").Replace("--", "n/a").ToLower();

            if (_type[i] == "PHQ2_SUI" && _v != null) _o += "<P><SPAN " + PageElements.DrawMessage("PHQ1") + ">PHQ1</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[0]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ2") + ">PHQ2</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[1]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ9") + ">PHQ9</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[2]) + "<BR>Suicidal attempt&mdash;" + (Logic.SuicideLifeTimeAttempt(ParticipantId) ? "yes" : "no");

            //if (_type[i] == "PHQ9_HM" && _v != null) _o += "<P><SPAN " +PageElements.DrawMessage("PHQ3") + ">PHQ3</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[0]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ4") + ">PHQ4</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[1]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ5") + ">PHQ5</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[2]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ6") + ">PHQ6</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[3]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ7") + ">PHQ7</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[4]) + "<BR><SPAN " +PageElements.DrawMessage("PHQ8") + ">PHQ8</SPAN>&mdash;" + Logic.CheckCode("PHQ", _value[5]) + "<BR><SPAN " +PageElements.DrawMessage("HM1") + ">HM1</SPAN>&mdash;" + Logic.CheckCode("HM", _value[6]) + "<BR><SPAN " +PageElements.DrawMessage("HM2") + ">HM2</SPAN>&mdash;" + Logic.CheckCode("HM", _value[7]);

            if (_type[i] == "ALC3" && _v != null) _o += "<P><SPAN " + PageElements.DrawMessage("ALC1") + ">AUDIT1</SPAN>&mdash;" + Logic.CheckCode("ALC1", _value[0]) + "<BR><SPAN " +PageElements.DrawMessage("ALC2") + ">AUDIT2</SPAN>&mdash;" + Logic.CheckCode("ALC2", _value[1]) + "<BR><SPAN " +PageElements.DrawMessage("ALC3") + ">AUDIT3</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[2]);

            if (_type[i] == "ALC7" && _v != null) _o += "<BR><SPAN " + PageElements.DrawMessage("ALC4") + ">AUDIT4</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[0]) + "<BR><SPAN " +PageElements.DrawMessage("ALC5") + ">AUDIT5</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[1]) + "<BR><SPAN " +PageElements.DrawMessage("ALC6") + ">AUDIT6</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[2]) + "<BR><SPAN " +PageElements.DrawMessage("ALC7") + ">AUDIT7</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[3]) + "<BR><SPAN " +PageElements.DrawMessage("ALC8") + ">AUDIT8</SPAN>&mdash;" + Logic.CheckCode("ALC3-8", _value[4]) + "<BR><SPAN " +PageElements.DrawMessage("ALC9") + ">AUDIT9</SPAN>&mdash;" + Logic.CheckCode("ALC9-10", _value[5]) + "<BR><SPAN " +PageElements.DrawMessage("ALC10") + ">AUDIT10</SPAN>&mdash;" + Logic.CheckCode("ALC9-10", _value[6]);

            if (_type[i] == "ILL" && _v != null) _o += "<P>Illegal drugs&mdash;" + _value[0].Replace(",", ", ").Replace("--", "n/a").Replace(", None", "None");

            if (_type[i] == "REA" && _v != null) _o += "<P><SPAN " +PageElements.DrawMessage("REA1") + ">REA1</SPAN>&mdash;" + _value[0].Replace("--", "n/a") + "<BR><SPAN " +PageElements.DrawMessage("REA2") + ">REA2</SPAN>&mdash;" + _value[1].Replace("--", "n/a") + "<BR><SPAN " +PageElements.DrawMessage("REA3") + ">REA3</SPAN>&mdash;" + _value[2].Replace("--", "n/a") + "<BR><SPAN " +PageElements.DrawMessage("REA4") + ">REA4</SPAN>&mdash;" + _value[3].Replace("--", "n/a") + "<BR><SPAN " +PageElements.DrawMessage("REA5") + ">REA5</SPAN>&mdash;" + _value[4].Replace("--", "n/a") + "<BR><SPAN " +PageElements.DrawMessage("REA6") + ">REA6</SPAN>&mdash;" + _value[5].Replace("--", "n/a");

            //if (_type[i] == "SER" && _v != null) _o += "<P>Use of medication: " + Logic.CheckCode("SER1", _value[0]) + "<BR>Use of counseling or therapy: " + Logic.CheckCode("SER2", _value[1]);

            if (_type[i] == "GOA_VAL" && _v != null) _o += "<P><SPAN style='color:red'>Goals</SPAN>&mdash;" + _value[0].Replace(",", ", ").Replace("--", "n/a") + "<P><SPAN style='color:red'>Values</SPAN>&mdash;" + _value[1].Replace(",", ", ").Replace("--", "n/a");
        }

        return "<DIV style='padding-left:15px;padding-right:15px'><B>Student Info:</B> " + _o + "</DIV>";
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        
        string ParticipantId = Request.QueryString[0] as string;
        if (string.IsNullOrEmpty(ParticipantId)) return;

        // need to add javascript to the header to get popup information 
        string _o = "<HTML><HEAD><TITLE>eBridge - A Journey for Your Well Being</TITLE>"+PageElements.DrawMessageJHeader(50, 50, 200)+"</HEAD><BODY style='margin:0px;padding:0px'>"
                    + "<TABLE width='100%' height='100%' cellpadding='0px' cellspacing='0px'><TR><TD style='padding-top:20px;padding-bottom:40px;background-color:#454545' align='center' valign='middle'>"
                    + "<TABLE width='700px' cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;background-color:#FFFFFF'>";

        string ParticipantStatus = "<TR height='30px'><TD></TD></TR><TR height='20px'><TD style='background-color:rgb(69, 69, 69)'></TD></TR><TR><TD style='padding-top:20px;padding-bottom:20px'>{0}{1}</TD></TR><TR height='50px'><TD style='background-color:rgb(69, 69, 69)'></TD></TR></TABLE>";

        ParticipantStatus= String.Format(ParticipantStatus, PrintValue(ParticipantId));

        _o += ParticipantStatus;

        _o += "</BODY></HTML>";

        Response.Write(_o);

    }
}