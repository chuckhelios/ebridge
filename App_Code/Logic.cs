using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public class Logic
{
    public static string CheckCode(string Type, string Value)
    {
        if (Value != ".")
        {
            //checks if a comma in the response
            if (Value.IndexOf(',') == -1)
            {
                String ret = Db.GetRecord("SELECT codes FROM codebook WHERE type = '" + Type + "'")[0].Split('|')[int.Parse(Value)].ToLower();
                return ret;
            }
            else
            {
                string codes = Db.GetRecord("SELECT codes FROM codebook WHERE type = '" + Type + "'")[0];
                string[] codes_array = codes.Split('|');
                string[] values = Value.Split(',');
                string values_string = "";
                foreach (string v in values)
                {
                    int to_int;
                    if (int.TryParse(v, out to_int))
                    {
                        values_string += codes_array[to_int] + ",";
                    }
                    else
                    {
                        values_string += v + ",";
                    }
                }
                return values_string;
                //return Db.GetRecord("SELECT codes FROM codebook WHERE type = '" + Type + "'")[0].Split('|')[int.Parse(Value.Split(',')[0])].ToLower() + "&mdash;" + Value.Split(',')[1];
            }
        }
        else return "n/a";
    }

    public static bool AlcPositive(string ParticipantId)
    {
        /*string Gender = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='DEM'")[0].Split('|')[1].Split(',')[0];
        string[] ALC3 = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC3'")[0].Split('|');

        int alc_score = int.Parse(ALC3[0]) + int.Parse(ALC3[1]) + int.Parse(ALC3[2]);
        if (int.Parse(Gender) !=0 && alc_score >= 4) return true;// For men "1" or other "2" a total score 4+ is positive
        else if (int.Parse(Gender) == 0 && alc_score >= 3) return true;// For women "0" a total score 3+ s positive
        else return false;*/

        return (ALCScore(ParticipantId) >= 8);
    }


    public static int PHQ2Score(string ParticipantId)
    {
        string[] PHQ2 = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0].Split('|');

        return (int.Parse(PHQ2[0]) + int.Parse(PHQ2[1]));
    }


    public static bool PHQ2Positive(string ParticipantId)
    {
        if (PHQ2Score(ParticipantId) >= 3) return true; else return false;
    }
    
    // how is this affected by the deletion of the hm?
    public static double PHQ9Score(string ParticipantId)
    {
        double PHQ9_SCORE;

        try
        {
            string _phq2 = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0];
            // how are the new phq2_sui questions being used in this context to calculate the phq9 scores?
            string[] PHQ2 = (_phq2.Split('|')[0] + "|" + _phq2.Split('|')[1] + "|" + _phq2.Split('|')[2]).Split('|');
            // first 3 questions are still the originals -> thoughts about suicide, 4 levels max 3;
            string[] PHQ7_HM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ9_HM'")[0].Split('|');
            // reduced from 8 questions in PHQ7_HM to 6 questions only. 4 levels, max 3
            string[] PHQ9 = (PHQ2[0] + "|" + PHQ2[1] + "|" + PHQ2[2] + '|' + PHQ7_HM[0] + "|" + PHQ7_HM[1] + "|" + PHQ7_HM[2] + "|" + PHQ7_HM[3] + "|" + PHQ7_HM[4] + "|" + PHQ7_HM[5]).Split('|');

            double _counter = 0; double _sum = 0; for (int i = 0; i < PHQ9.Length; i++) { if (PHQ9[i] != ".") { _counter++; _sum += int.Parse(PHQ9[i]); } } PHQ9_SCORE = (_sum / _counter) * 9; //max of 27
        }
        catch
        {
            PHQ9_SCORE = -1;
        }

        return PHQ9_SCORE;
    }

    public static string PHQ9Severity(string ParticipantId)
    {
        string PHQ9_SEVERITY = "";
        double PHQ9_SCORE = PHQ9Score(ParticipantId);

        //minimal (0-4); mild (5-9); moderate (10-14): moderately severe (15-19): severe (20-27)
        if (PHQ9_SCORE <0) PHQ9_SEVERITY = "N/A";
        else if (PHQ9_SCORE >= 0 && PHQ9_SCORE < 5) PHQ9_SEVERITY = "Minimal";
        else if (PHQ9_SCORE >= 5 && PHQ9_SCORE < 10) PHQ9_SEVERITY = "Mild";
        else if (PHQ9_SCORE >= 10 && PHQ9_SCORE < 15) PHQ9_SEVERITY = "Moderate";
        else if (PHQ9_SCORE >= 15 && PHQ9_SCORE < 20) PHQ9_SEVERITY = "Moderately Severe";
        else PHQ9_SEVERITY = "Severe";

        return PHQ9_SEVERITY;
    }

    public static string[] PHQ9Assessment(string ParticipantId)
    {
        string[] PHQ_ASSESSMENT;

        string _phq2 = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0];
        string[] PHQ2 = (_phq2.Split('|')[0] + "|" + _phq2.Split('|')[1]).Split('|');
        string[] PHQ7_HM = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ9_HM'")[0].Split('|');
        // modify below, since PHQ9_HM the new version will not have the bottom 2 questions for HM.
        //string[] PHQ9 = (PHQ2[0] + "|" + PHQ2[1] + '|' + PHQ7_HM[0] + "|" + PHQ7_HM[1] + "|" + PHQ7_HM[2] + "|" + PHQ7_HM[3] + "|" + PHQ7_HM[4] + "|" + PHQ7_HM[5] + "|" + PHQ7_HM[6]).Split('|');
        string[] PHQ9 = (PHQ2[0] + "|" + PHQ2[1] + '|' + PHQ7_HM[0] + "|" + PHQ7_HM[1] + "|" + PHQ7_HM[2] + "|" + PHQ7_HM[3] + "|" + PHQ7_HM[4] + "|" + PHQ7_HM[5]).Split('|');
        // total of 8, up to index of 7. doesn't actually affect the calculations. 
        string _phq = "";
        if (PHQ9[2] != "0" && PHQ9[2] != ".") _phq += "sleep|";
        if (PHQ9[3] != "0" && PHQ9[3] != ".") _phq += "fatigue and energy level|";
        if (PHQ9[4] != "0" && PHQ9[4] != ".") _phq += "appetite or overeating|";
        if (PHQ9[6] != "0" && PHQ9[6] != ".") _phq += "concentration|";
        if (PHQ9[7] != "0" && PHQ9[7] != ".") _phq += "feeling either slowed down or restless|";
        if (_phq.Length != 0) PHQ_ASSESSMENT = _phq.Substring(0, _phq.Length - 1).Split('|');

        else PHQ_ASSESSMENT = null;

        return PHQ_ASSESSMENT;
    }

    public static double ALCScore(string ParticipantId)
    {
        double ALC_SCORE;
        
        
        string[] ALC = (Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC3'")[0] + "|" + Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC7'")[0]).Split('|');

        double _counter = 0; 
        double _sum = 0; 
        for (int i = 0; i < ALC.Length; i++) 
        { 
            if (ALC[i] != ".") 
            { 
                _counter++; 
                _sum += int.Parse(ALC[i]); 
            } 
        } 
        ALC_SCORE = (_sum / _counter) * 10;
        
        return ALC_SCORE;
    }

    public static string ALCSeverity(string ParticipantId)
    {
        string ALC_SEVERITY = "";

        double ALC_SCORE = ALCScore(ParticipantId);

        //low risk of dependence and harmful effects (0-7); medium risk of dependence and harmful effects (8-15);
        //moderately high risk of dependence and harmful effects (16-19); high risk of dependence and harmful effects (20-40)
        if (ALC_SCORE >= 0 && ALC_SCORE < 8) ALC_SEVERITY = "Low";//risk of dependence and harmful effects";
        else if (ALC_SCORE >= 8 && ALC_SCORE < 16) ALC_SEVERITY = "Medium";// risk of dependence and harmful effects";
        else if (ALC_SCORE >= 16 && ALC_SCORE < 20) ALC_SEVERITY = "Moderately high";// risk of dependence and harmful effects";
        else ALC_SEVERITY = "High";// risk of dependence and harmful effects";

        return ALC_SEVERITY;
    }

    public static string[] ALCAssessment(string ParticipantId)
    {

        string[] ALC_ASSESSMENT;

        string[] ALC = (Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC3'")[0] + "|" + Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='ALC7'")[0]).Split('|');

        string _alc = "";
        if (ALC[4] != "0" && ALC[4] != ".") _alc += "You failed to do what was normally expected of you|";
        if (ALC[5] != "0" && ALC[5] != ".") _alc += "You needed a first drink in the morning to get yourself going|";
        if (ALC[6] != "0" && ALC[6] != ".") _alc += "You had a feeling of guilt or remorse after drinking|";
        if (ALC[7] != "0" && ALC[7] != ".") _alc += "You were unable to remember what happened the night before because you had been drinking|";
        if (ALC[8] != "0" && ALC[8] != ".") _alc += "You or someone else was injured because of your drinking|";
        if (_alc.Length != 0) ALC_ASSESSMENT = _alc.Substring(0, _alc.Length - 1).Split('|');
        else ALC_ASSESSMENT = null;
        

        return ALC_ASSESSMENT;
    }

    public static bool SuicideLifeTimeAttempt(string ParticipantId)
    {
        // if any responses regarding suicide is non negative, then return true;
        
        
        /*string[] PHQ2_SUI = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0].Split('|');
        */
            //for (int i = 3; i < PHQ2_SUI.Length; i++) {
            //    if (PHQ2_SUI[i] != "0") return true;
            //}
            // it's like this 3|3|3|1|1|0|0|0|0|0 zero means yes in this case

        /*if (int.Parse(PHQ2_SUI[7]) == 0) return true;*/
        
        return false;
    }

    public static bool PHQ9_Suicide(string ParticipantId)
    {
        
        /*string[] PHQ2_SUI = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='PHQ2_SUI'")[0].Split('|');
        // PHQ9 question 9 >0, or Q4 = yes or Q5 = yes 
        // it's like this 3|3|3|1|1|0|0|0|0|0
        if (int.Parse(PHQ2_SUI[2]) > 0) return true;
        if (int.Parse(PHQ2_SUI[3]) == 0) return true;
        if (int.Parse(PHQ2_SUI[5]) == 0) return true;*/
        return false;
    }

    public static bool FullSurveyEligibility(string ParticipantId)
    {

        // need to be modified
        int _rf = 0;
        if (AlcPositive(ParticipantId)) _rf++;
        if (PHQ2Positive(ParticipantId)) _rf++;
        if (PHQ9_Suicide(ParticipantId)) _rf++;
        if (SuicideLifeTimeAttempt(ParticipantId)) _rf++;

        if (_rf >= 2) return true; else return false;
    }

    public static bool InterventionEligibility(string ParticipantId)
    {
        string[] _ser = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='SER'")[0].Split('|');
        // ,7|,7|1|1|0|1
        bool CurrentlyTakingPrescribedMedication = false; 
        if (_ser[1] != ",7") CurrentlyTakingPrescribedMedication = true;

        bool CurrentlyReceivingCounselingOrTherapy = false; 
        if (_ser[3] == "0") CurrentlyReceivingCounselingOrTherapy = true;

        //bool HosptializedInPast12Months = false;
        //if (_ser[5] == "0") HosptializedInPast12Months = true;
        return FullSurveyEligibility(ParticipantId) && !CurrentlyTakingPrescribedMedication && !CurrentlyReceivingCounselingOrTherapy; //&& !HosptializedInPast12Months;
        //return FullSurveyEligibility(ParticipantId);
    }

    public static string ReadGoalsAndValues(string ParticipantId, string Type)
    {
        string[] Goal_Value = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code ='GOA_VAL'")[0].Split('|');
        string GOALS = Goal_Value[0].ToLower().Replace(",", ", "); string VALUES = Goal_Value[1].ToLower().Replace(",", ", ");
        string GOALS_AND_VALUES = ""; string PFLink = "";
        string TextStyle = "color:#8A4117";

        if (Type == "Student")
        {
            PFLink = " Click <A href='../survey/receipt.aspx?p=" + ParticipantId + "&p1=0' target='new'>here</A> if you would like to review your personalized feedback.";

            if (Goal_Value[0] != "--" && Goal_Value[1] != "--") GOALS_AND_VALUES = "The Goals you set for yourself are <SPAN style='" + TextStyle + "'>" + GOALS + "</SPAN>; and you mentioned that you appreciate the following Values: <SPAN style='" + TextStyle + "'>" + VALUES + "</SPAN>." + PFLink;
            else if (Goal_Value[1] != "--" && Goal_Value[1] == "--") GOALS_AND_VALUES = "The Goals you set for yourself are <SPAN style='" + TextStyle + "'>" + GOALS + "</SPAN>." + PFLink;
            else if (Goal_Value[1] == "--" && Goal_Value[1] != "--") GOALS_AND_VALUES = "You mentioned that you appreciate the following Values: <SPAN style='" + TextStyle + "'>" + VALUES + "</SPAN>." + PFLink;
            else { }// do nothing for now
        }
        else
        {
            PFLink = " Click <A href='../survey/receipt.aspx?p=" + ParticipantId + "&p1=0' target='new'>here</A> to review the student's personalized feedback.";

            if (Goal_Value[0] != "--" && Goal_Value[1] != "--") GOALS_AND_VALUES = "Goals: <SPAN style='" + TextStyle + "'>" + GOALS + "</SPAN>; Values: <SPAN style='" + TextStyle + "'>" + VALUES + "</SPAN>." + PFLink;
            else if (Goal_Value[1] != "--" && Goal_Value[1] == "--") GOALS_AND_VALUES = "Goals: <SPAN style='" + TextStyle + "'>" + GOALS + "</SPAN>." + PFLink;
            else if (Goal_Value[1] == "--" && Goal_Value[1] != "--") GOALS_AND_VALUES = "Values: <SPAN style='" + TextStyle + "'>" + VALUES + "</SPAN>." + PFLink;
            else { }// do nothing for now
        }

        return GOALS_AND_VALUES;
    }

    public static double PHQ9Percentile(double PHQ9_SCORE)
    {
        switch ((int)Math.Round(PHQ9_SCORE, 0))
        {
            case 0: return 0.0;
            case 1: return 6.6;
            case 2: return 13.5;
            case 3: return 24.0;
            case 4: return 34.6;
            case 5: return 44.4;
            case 6: return 52.8;
            case 7: return 60.4;
            case 8: return 66.9;
            case 9: return 72.7;
            case 10: return 77.9;
            case 11: return 81.9;
            case 12: return 85.2;
            case 13: return 88.0;
            case 14: return 90.3;
            case 15: return 92.4;
            case 16: return 94.1;
            case 17: return 95.3;
            case 18: return 96.2;
            case 19: return 97.1;
            case 20: return 97.8;
            case 21: return 98.3;
            case 22: return 98.8;
            case 23: return 99.1;
            case 24: return 99.4;
            case 25: return 99.7;
            case 26: return 99.8;
            case 27: return 99.8;
            default: return 99.9;
        }
    }

    public static double ALCPercentile(double ALC_SCORE)
    {
        switch ((int)Math.Round(ALC_SCORE, 0))
        {
            case 0: return 0;
            case 1: return 14;
            case 2: return 22;
            case 3: return 31;
            case 4: return 32;
            case 5: return 37;
            case 6: return 45;
            case 7: return 50;
            case 8: return 61;
            case 9: return 66;
            case 10: return 75;
            case 11: return 80;
            case 12: return 81;
            case 13: return 84;
            case 14: return 87;
            case 15: return 89;
            case 16: return 92;
            case 17: return 93;
            case 18: return 96;
            case 19: return 96;
            case 20: return 96;
            case 21: return 97;
            case 22: return 97;
            case 23: return 98;
            case 24: return 98;
            default: return 99;
        }
    }
}
