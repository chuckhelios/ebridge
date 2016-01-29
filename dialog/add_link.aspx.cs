using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dialog_add_link : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void fvLink_ItemInserted(object sender, EventArgs e)
    {
        Response.Redirect("resource_links.aspx");
    }
}