using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections;


public partial class email_dispatch : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string[][] contents = Db.GetRecords("SELECT id, name_first, email FROM TEMP_UNR_REMINDER_3");
		ArrayList id_list = new ArrayList();
		Response.Write("Sending");
		for(int i = 0; i < contents.GetLength(0); i++)
		{
			string id = contents[i][0];
			string name = contents[i][1];
			string email = contents[i][2];
			string subject = "Final Reminder: Students e-Bridge to Mental Health";

			string mail_test = "Dear " + name + ",\r\n\r\n";
			mail_test += "This is a final reminder that you are invited to participate in a confidential online research study, being conducted at UNR (and also University of Michigan, Stanford University, and Iowa University), about mental health and general well-being among college students.   ";
			mail_test += "Regardless of whether you participate, you will be automatically entered into a random drawing for online gift cards (ten $100 online gift cards).\r\n\r\n";
			mail_test += "The initial survey takes approximately 2-5 minutes. Some students will receive personalized feedback and be invited to participate in a second phase of the study.  \r\n\r\n";
			mail_test += "Participation in this study includes three steps:\r\n";
			mail_test += "\t1. Complete a brief online survey\r\n\t2. View your survey results\r\n\t3. Leave your brief feedback about these results or your experience in this study\r\n\r\n";
			mail_test += "Click the link below to participate: \r\n\r\n";
			mail_test += "http://ebridge.unr.edu/survey?id=" + id + "\r\n\r\n";
			mail_test += "To opt out of the prize drawing, please contact the research program coordinator at reblin@umich.edu.\r\n\r\n";
			mail_test += "Thank you for considering participation in our study!\r\n\r\n";
			mail_test += "Sincerely,\r\n\r\nJacqueline Pistorello, Ph.D. \r\nUNR Counseling Services\r\n(775) 846-554\r\n(775) 846-5540\r\n\r\n\r\n\r\n";
			mail_test += "University of Nevada, Reno IRB Number 659028 and University of Michigan IRB Number HUM00091681";
			try
			{
				if (!(id_list.Contains(id)))
				{
					Utility.SendPHPMail(email, "ebridgeteam@unr.edu", "The eBridge Team", subject, mail_test);
					Db.Execute("INSERT INTO INVITATION_LOG (EMAIL, TYPE) VALUES ('" + email + "', 'REMINDER_3')");
					Response.Write("Email sent to " + email + "<br>");
					id_list.Add(id);
					System.Threading.Thread.Sleep(1500);
				}
			}
			catch
			{
				Db.Execute("INSERT INTO INVITATION_LOG (EMAIL, TYPE) VALUES ('" + email + "', 'REMINDER_3_ERROR')");
				continue;
			}
		}
		

	}
}