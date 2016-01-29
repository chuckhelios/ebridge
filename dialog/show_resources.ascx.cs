using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dialog_show_resources : System.Web.UI.UserControl
{
    private int displayColumns;

    public int DisplayColumns
    {
        get { return displayColumns; }
        set { displayColumns = value; }
    }

    protected Dictionary<string, string> SchoolInfo;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SchoolInfo = Session["SCHOOL"] as Dictionary<string, string>;
            if (SchoolInfo == null) { Response.Redirect("login.aspx?id=aiBcAmVnQs"); return; }

            string SchoolCode = SchoolInfo["code"];
            ebridgeEntities _db = new ebridgeEntities();
            IQueryable<RESOURCE_LINK> rs_lk = _db.RESOURCE_LINK.Where(rl => rl.ACTIVE == "True" && rl.SITE_ID == SchoolCode).OrderBy(rl1 => rl1.RANK);
            if (displayColumns != null && displayColumns == 1)
            {
                Repeater1.DataSource= new List<List<RESOURCE_LINK>>{rs_lk.ToList()};
                Repeater1.DataBind();
                return;
            }
            int rs_length = rs_lk.Count();
            List<RESOURCE_LINK> rs_lk1 = rs_lk.Take(rs_length / 2).ToList();
            List<RESOURCE_LINK> rs_lk2 = rs_lk.ToList();
            rs_lk2.RemoveRange(0, rs_length / 2);
            Repeater1.DataSource = new List<List<RESOURCE_LINK>>{ rs_lk1, rs_lk2 };
            Repeater1.DataBind();
        }
    }

    protected void columnDataSource(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem item = e.Item;
            List<RESOURCE_LINK> rs_lk = item.DataItem as List<RESOURCE_LINK>;
            Repeater Repeater2 = (Repeater)item.FindControl("Repeater2");
            Repeater2.DataSource = rs_lk;
            Repeater2.DataBind();
        }
    }
}