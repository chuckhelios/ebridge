using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;
using System.Text;
using System.Configuration;

public class Db {
    
    public static ConnectionStringSettings CONN_STR = ConfigurationManager.ConnectionStrings["ebridge_3"];

    public static int GetCount(string CommandText) { return OleDb.GetCount(CommandText); }
    public static string[] GetRecord(string CommandText) { return OleDb.GetRecord(CommandText); }
    public static string[][] GetRecords(string CommandText) { return OleDb.GetRecords(CommandText); }
    public static void Execute(string CommandText) { OleDb.Execute(CommandText); }

    public static string PrintTable(string[][] Data, string TableCaption)
    {
        string _o = "<TABLE cellspacing='0px' cellpadding='0px' style='font-family:arial;font-size:12px;border:1px solid black;padding:10px'>";

        _o += "<TR><TD colspan='" + Data.Length + "' style='font-weight:bold;padding-bottom:5px'>" + TableCaption + "</TD></TR>";

        for (int i = 0; i < Data.Length; i++)
        {
            _o += "<TR height='18px' " + (i % 2 == 0 ? " style='background-color:#DCDCDC'" : "") + ">";
            for (int j = 0; j < Data[0].Length; j++) _o += "<TD style='padding-left:5px;padding-right:5px'>" + Data[i][j].Replace("NULL", "--") + "</TD>";
            _o += "</TR>";
        }

        return _o + "</TABLE>";
    }

    public static string PrintTable(double[][] Data, string TableCaption)
    {
        string[][] _r = new string[Data.Length][];

        for (int i = 0; i < _r.Length; i++)
        {
            _r[i] = new string[Data[0].Length]; for (int j = 0; j < _r[i].Length; j++) _r[i][j] = Data[i][j].ToString();
        }
        return PrintTable(_r, TableCaption);
    }
}

public class OleDb
{
    private static ConnectionStringSettings CONN_STR = Db.CONN_STR;

    public static int GetCount(string CommandText)
    {
        return (int.Parse(GetRecord(CommandText)[0]));
    }

    public static string[] GetRecord(string CommandText)
    {
        SqlConnection _conn = new SqlConnection(CONN_STR.ConnectionString); _conn.Open();
        SqlCommand _cmd = new SqlCommand(CommandText, _conn); 
        SqlDataReader _dr = _cmd.ExecuteReader(CommandBehavior.CloseConnection);

        string[] _return = null;

        if (_dr.Read())
        {
            _return = new string[_dr.FieldCount];
            for (int i = 0; i < _dr.FieldCount; i++) { if (Convert.IsDBNull(_dr[i])) _return[i] = "NULL"; else _return[i] = _dr[i].ToString().Trim(); }
        }

        _dr.Close(); _conn.Dispose();

        if (_return != null)
        {
            if (CommandText.ToUpper().IndexOf("MAX") != -1 || CommandText.ToUpper().IndexOf("MIN") != -1)
            {
                foreach (string _s in _return) { if (_s != "NULL") return _return; }
                return null;
            }
        }

        return _return;
    }

    public static string[][] GetRecords(string CommandText)
    {
        SqlConnection _conn = new SqlConnection(CONN_STR.ConnectionString); _conn.Open();
        SqlCommand _cmd = new SqlCommand(CommandText, _conn); SqlDataReader _dr = _cmd.ExecuteReader(CommandBehavior.CloseConnection);

        ArrayList _al = new ArrayList(); string[] _row;

        for (int i = 0; _dr.Read(); i++)
        {
            _row = new string[_dr.FieldCount];
            for (int j = 0; j < _dr.FieldCount; j++) { if (Convert.IsDBNull(_dr[j])) _row[j] = "NULL"; else _row[j] = _dr[j].ToString().Trim(); }
            _al.Add(_row);
        }

        string[][] _return = new string[_al.Count][]; for (int i = 0; i < _al.Count; i++) _return[i] = (string[])_al[i];

        _dr.Close(); _conn.Dispose();

        if (_return.Length == 0) return null; else return _return;
    }

    public static void Execute(string CommandText)
    {
        SqlConnection _conn = new SqlConnection(CONN_STR.ConnectionString); _conn.Open();
        SqlCommand _cmd = new SqlCommand(CommandText, _conn); _cmd.ExecuteNonQuery(); _conn.Close(); _conn.Dispose();
    }
}

public class Utility
{
    public static string GetStatus(string ParticipantId, string StatusCode)
    {
        string[] _return = Db.GetRecord("SELECT status_value FROM status WHERE id = '" + ParticipantId + "' AND status_code = '" + StatusCode + "'");// AND unique_id = (SELECT MAX(unique_id) FROM status WHERE id = '" + ParticipantId + "' AND status_code = '" + StatusCode + "')");

        if (_return != null) return _return[0]; else return "NULL";
    }

    public static void UpdateStatus(string ParticipantId, string StatusCode, string Value)
    {
        Db.Execute("DELETE FROM status WHERE id = '" + ParticipantId + "' AND status_code = '" + StatusCode + "'");
        Db.Execute("INSERT INTO status (id, status_code, status_value, date_time) VALUES ('" + ParticipantId + "','" + StatusCode + "','" + Value + "','" + DateTime.Now + "')");
    }

    public static string GetLog(string Id, string Code)
    {
        string[] _return = Db.GetRecord("SELECT date_time FROM activity_log WHERE id = '" + Id + "' AND activity_code = '" + Code + "' AND unique_id = (SELECT MAX(unique_id) FROM activity_log WHERE id = '" + Id + "' AND activity_code = '" + Code + "')");

        if (_return != null) return _return[0]; else return "NULL";
    }

    public static void LogActivity(string ParticipantId, string ActivityCode, string Remark)
    {
        Db.Execute("INSERT INTO activity_log (id, activity_code, date_time, remark) VALUES ('" + ParticipantId + "','" + ActivityCode + "','" + DateTime.Now + "','" + Remark + "')");
    }

    public static void SendGMail(string ReceiverEmail, string ReceiverName, string SenderEmail, string SenderName, string Subject, string Body)
    {
        MailAddress from = new MailAddress(SenderEmail, SenderName);
        MailAddress to = new MailAddress(ReceiverEmail, ReceiverName);
        MailMessage _msg = new MailMessage(from, to);
        _msg.Subject = Subject; 
        _msg.SubjectEncoding = System.Text.Encoding.UTF8;
        _msg.Body = Body; 
        _msg.BodyEncoding = System.Text.Encoding.UTF8;
        //_msg.IsBodyHtml = true;
        //"10.16.6.146", 25
        SmtpClient _client = new SmtpClient();
        _client.Host = "10.16.6.146";
        _client.Port = 25;

        //_client.Credentials = new NetworkCredential("default email", "default password");

        _client.Send(_msg);
    }

    public static void SendPHPMail(string ReceiverEmail, string SenderEmail, string SenderName, string Subject, string Body)
    {
        string ServiceURL = "http://141.211.11.1/utility/email.php";

        byte[] _data = Encoding.UTF8.GetBytes("TO_ADDRESS=" + ReceiverEmail + "&SUBJECT=" + Subject + "&BODY=" + Body + "&FROM=" + SenderName + " <" + SenderEmail + ">");
        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);
        myRequest.Method = "POST";
        myRequest.ContentType = "application/x-www-form-urlencoded";
        myRequest.ContentLength = _data.Length;
        Stream newStream = myRequest.GetRequestStream();
        newStream.Write(_data, 0, _data.Length);
        newStream.Close();
    }

    public static void SendMail(string ReceiverEmail, string ReceiverName, string SenderEmail, string SenderName, string Subject, string Body)
    {
        MailMessage _msg = new MailMessage(new MailAddress(SenderEmail, SenderName), new MailAddress(ReceiverEmail, ReceiverName));
        _msg.Subject = Subject; _msg.SubjectEncoding = System.Text.Encoding.UTF8;
        _msg.Body = Body; _msg.BodyEncoding = System.Text.Encoding.UTF8;
        _msg.IsBodyHtml = true;

        SmtpClient _client = new SmtpClient("68.42.76.41", 2525);

        _client.Send(_msg);
    }
}