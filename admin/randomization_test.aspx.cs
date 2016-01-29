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

public partial class admin_randomization_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write("Please wait ...<P>"); for (int i = 0; i < 1000; i++) Response.Write(" ");

        Response.Flush();

        string[][] _user = Db.GetRecords("SELECT id FROM participant");

        Random _r = new Random();
        Db.Execute("DELETE FROM screening_response");
        Db.Execute("DELETE FROM status");

        for (int i = 0; i < _user.Length; i++)
        {
            Db.Execute("INSERT INTO screening_response VALUES ('" + _user[i][0] + "','PHQ2_SUI','0|0|0|" + _r.Next(0, 2) + "','')");
        }

        Random _random_first = new Random();

        for (int i = 0; i < _user.Length; i++)
        {
            string ParticipantId = _user[i][0];
            string[] _profile = Db.GetRecord("SELECT gender, year FROM participant WHERE id = '" + ParticipantId + "'");
            string _gender = _profile[0];
            string _year = _profile[1];
            string _sui = Db.GetRecord("SELECT response FROM screening_response WHERE participant_id = '" + ParticipantId + "' AND question_code = 'PHQ2_SUI'")[0].Split('|')[3];

            int _intervention1 = Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + _sui + ") AND p.gender = '" + _gender + "' AND p.year = " + _year);

            int _intervention2 = Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + _sui + ") AND p.gender = '" + _gender + "' AND p.year = " + _year);

            if (_intervention1 == 0 && _intervention2 == 0)
            {
                string _group_first = _random_first.Next(0, 2).ToString();
                Utility.UpdateStatus(ParticipantId, "INTERVENTION", _group_first);

                Response.Write("GENDER: " + _gender + ", YEAR: " + _year + ", SUICIDE: " + _sui + ", # OF INT: " + _intervention1 + ", # OF CONTROL: " + _intervention2 + ", PLACE INTO: " + _group_first + "<BR>");
            }
            else
            {
                if (_intervention1 >= _intervention2) Utility.UpdateStatus(ParticipantId, "INTERVENTION", "1");
                else Utility.UpdateStatus(ParticipantId, "INTERVENTION", "0");

                Response.Write("GENDER: " + _gender + ", YEAR: " + _year + ", SUICIDE: " + _sui + ", # OF INT: " + _intervention1 + ", # OF CONTROL: " + _intervention2 + ", PLACE INTO: " + (_intervention1 >= _intervention2 ? "1" : "0") + "<BR>");
            }

            
            Response.Flush();
        }

        Response.Write("<P>");

        Response.Write("F, Year 1, Suicide 0, # of Control: " 
            + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "F" + "' AND p.year = " + "1") 
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "F" + "' AND p.year = " + "1") + "<BR>");

        Response.Write("M, Year 1, Suicide 0, # of Control: "
             + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "M" + "' AND p.year = " + "1")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "M" + "' AND p.year = " + "1") + "<BR>");

        Response.Write("F, Year 2, Suicide 0, # of Control: "
              + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "F" + "' AND p.year = " + "2")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "F" + "' AND p.year = " + "2") + "<BR>");

        Response.Write("M, Year 2, Suicide 0, # of Control: "
            + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "M" + "' AND p.year = " + "2")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "0" + ") AND p.gender = '" + "M" + "' AND p.year = " + "2") + "<BR>");
        
        Response.Write("F, Year 1, Suicide 1, # of Control: "
            + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "F" + "' AND p.year = " + "1")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "F" + "' AND p.year = " + "1") + "<BR>");


        Response.Write("M, Year 1, Suicide 1, # of Control: "
            + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "M" + "' AND p.year = " + "1")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "M" + "' AND p.year = " + "1") + "<BR>");  

        Response.Write("F, Year 2, Suicide 1, # of Control: "
             + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "F" + "' AND p.year = " + "2")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "F" + "' AND p.year = " + "2") + "<BR>");  

        Response.Write("M, Year 2, Suicide 1, # of Control: "
              + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '0' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "M" + "' AND p.year = " + "2")
            + ", # of Intervention: " + Db.GetCount("SELECT COUNT(*) FROM participant AS p, status AS s, screening_response AS r WHERE p.id = s.id AND p.id = r.participant_id AND s.status_code = 'INTERVENTION' AND s.status_value = '1' AND r.question_code = 'PHQ2_SUI' AND RIGHT(r.response," + "1" + ") AND p.gender = '" + "M" + "' AND p.year = " + "2") + "<BR>");  
    }
}
