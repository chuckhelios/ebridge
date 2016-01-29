using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class admin_giftcard_sendcard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["giftcard"] == null) { Response.Redirect("index.aspx"); return; }

        Response.Write("<HTML><BODY style='padding:10px'>");
        Response.Write("<FORM method='post' action='sendcard_action.aspx'>");
        Response.Write("<TABLE cellpadding='0' cellspacing='0' style='font-family:arial;font-size:12px;width=600px'>");

        Response.Write("<TR><TD>Study ID(s)</TD><TD style='padding-left:10px'>AMAZON Code(s)</TD></TR>");
        Response.Write("<TR><TD><TEXTAREA name='ID' style='height:200px;width:300px'></TEXTAREA></TD>");
        Response.Write("<TD style='padding-left:10px'><TEXTAREA name='AMAZON' style='height:200px;width:300px'></TEXTAREA></TD></TR>");

        Response.Write("<TR><TD style='padding-top:10px'>Email Subject</TD><TD></TD></TR>");
        Response.Write("<TR><TD colspan='2'><INPUT name='SUBJECT' style='width:610px' value='the $10 Amazon.com gift card'></TD></TR>");
        Response.Write("<TR><TD style='padding-top:10px'>Email Body</TD><TD></TD></TR>");
        Response.Write("<TR><TD colspan='2'><TEXTAREA name='BODY' style='height:200px;width:610px'>" + DefaultMessage() + "</TEXTAREA></TD></TR>");

        Response.Write("<TR><TD style='padding-top:10px'><INPUT type='submit' value=' Send '></TD><TD></TD></TR>");
        Response.Write("</TABLE>");
        Response.Write("</FORM>");
        Response.Write("</BODY></HTML>");
    }

    protected string DefaultMessage()
    {
        return @"Dear _NAME_FIRST_,

We are attaching a $10 Amazon.com gift certificate code to thank you for completing the questionnaire for e-Bridge to well being.

Your Amazon.com gift card is: _AMAZON_GIFT_CODE_.

Thank you again for your participation in our study.

Sincerely,

Cheryl King, Ph.D.
Professor of Psychiatry and Psychology
Director of the Institute for Human Adjustment
University of Michigan";
    }
}