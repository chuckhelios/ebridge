using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class dialog_login_alt : WebformGeneric
{
    protected string fromUrl;
    ebridgeEntities _db;

    protected struct LoginUser
    {
        private HtmlGenericControl username;
        private HtmlGenericControl checkbox;

        public HtmlGenericControl UserName
        {
            get { return username;}
            set { username = value;}
        }
        public HtmlGenericControl CheckBox
        {
            get { return checkbox;}
            set { checkbox = value; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        validateUrlParams();
        fromUrl = Session["CurrentUrl"] as string;

        if (!IsPostBack)
        {

            if (!validateUrlParams())
            {
                Label Label1 = new Label();
                Label1.Text = "invalid request: please use url from email to access page";
                PlaceHolder1.Controls.AddAt(0,Label1);
                return;
            }
            LoginUser _lu = new LoginUser();

            HtmlGenericControl _usr = new HtmlGenericControl("input");
            _usr.Attributes["class"]="form-control";
            _usr.Attributes["id"]= "uLogin";
            _usr.Attributes["placeholder"]="Login";

            // usertype either counselor or participant, as set in webformgeneric base class
            if (UserType == "counselor") { 
                HtmlGenericControl _chk = new HtmlGenericControl("div");
                _chk.Attributes["class"] = "checkbox";
                _chk.Controls.Add(new LiteralControl("<label><input id='unnoticed' type='checkbox'>Log in Unnoticed</label>"));
                _lu.CheckBox=_chk;
                _lu.UserName=_usr;
            }
            else if (UserType == "participant")
            {
                //disable username field
                _usr.Attributes["value"]=ParticipantId;
                _usr.Attributes["disabled"]="true";
                _lu.UserName=_usr;
            }
            else
            {
                return;
            }
            ButtonClose.Attributes["data-dismiss"] = "modal";
            Repeater1.DataSource=new List<LoginUser>(){_lu};
            Repeater1.DataBind();
            getNavigation(RepeaterNav, "anon");
        }
    }

    protected void bindLogins(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        {
            LoginUser _di = (LoginUser)item.DataItem;
        }
    }

    protected void Login(object sender, EventArgs e)
    {
        // how to get the form values from repeater generated controls? control id is not set
    }
}