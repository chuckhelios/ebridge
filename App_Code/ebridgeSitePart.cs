using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ebridgeSitePart
/// </summary>
/// 
public partial class RESOURCE_LINK
{
    public List<RESOURCE_LINK> getAllLinks()
    {
        ebridgeEntities _db = new ebridgeEntities();
        return _db.RESOURCE_LINK.ToList<RESOURCE_LINK>();
    }
    public void createLink(string URL_TEXT, string URL, string Addtl_Text, int Rank )
    {
        ebridgeEntities _db = new ebridgeEntities();
        _db.RESOURCE_LINK.Add( new RESOURCE_LINK { URL_TEXT=URL_TEXT, URL=URL, ADDTL_TEXT=ADDTL_TEXT, RANK=Rank, DATE_TIME=DateTime.Now, SITE_ID="s", CREATED_BY="C001", ACTIVE="True"});
        _db.SaveChanges();
    }

    public void updateLink(int RID, string URL_TEXT, string URL, string ADDTL_TEXT, string ACTIVE, int Rank)
    {
        ebridgeEntities _db = new ebridgeEntities();
        RESOURCE_LINK r = _db.RESOURCE_LINK.SingleOrDefault(link => link.RID == RID);
        if (r != null)
        {
            r.URL_TEXT=URL_TEXT;
            r.ADDTL_TEXT=ADDTL_TEXT;
            r.URL=URL;
            r.RANK=Rank;
            r.DATE_TIME=DateTime.Now;
            _db.SaveChanges();
        }
        else
        {
            throw new ApplicationException("Can't find the link");
        }
    }

    public void deleteLink(int RID)
    {
        ebridgeEntities _db = new ebridgeEntities();
        RESOURCE_LINK r = _db.RESOURCE_LINK.SingleOrDefault(link => link.RID==RID);
        if (r != null)
        {
            _db.RESOURCE_LINK.Remove(r);
            _db.SaveChanges();
        }
        else
        {
            throw new ApplicationException("Can't find the link");
        }
    }
}

public partial class MESSAGE_ALT
{
    public string Position { get; set; }

    public void setPosition ()
    {
        if (TO_ID == null)
        {
            Position="left";
        }
        else if (FROM_ID == null)
        {
            Position= "right";
        }
        else
        {
            Position = "right"; 
        }
    }
}

public partial class PAGE_REF
{
    public IOrderedEnumerable<QUESTION_REF> OrderedQuestions;
}

public partial class QUESTION_REF
{
    public List<RESPONSE_REF> ChosenResponses;
    public string DisplayResponses;

    public bool responseHasReference(ref List<string> responses, ref RESPONSE_REF response_ref)
    {
        string val = response_ref.value;
        if (string.IsNullOrEmpty(val))
        {
            responses.Remove(val);
        }
        else if ( responses.Contains(val))
        {
            responses.Remove(val);
            return true;
        }
        return false;

    }

    public void setChosenResponses(string rstring, ref ebridgeEntities _db)
    {
        string page_title = this.PAGE_REF.value;
        IQueryable<RESPONSE_REF> r_ref = _db.RESPONSE_REF;
        List<string> responses = rstring.Split(',').ToList();
        IEnumerable<RESPONSE_REF> Chosen = this.RESPONSE_X_QUESTION.Join(r_ref, rxq=>rxq.rid, r=>r.rid, (rxq, r) => r).Where( r=> responseHasReference(ref responses, ref r));
        this.ChosenResponses=Chosen.ToList();
        string _dsp = "";
        if (ChosenResponses != null)
        {
            foreach (RESPONSE_REF c in ChosenResponses)
            {
                _dsp += string.Format("<li>{0} : {1} </li>", new string[2]{c.value, c.content} );
            }
        }
        if (responses.Count != 0)
        {
            foreach (string r in responses)
            {
                if (r == ".") _dsp += string.Format("<li>{0} : {1} </li>", new string[2]{r, "No Response"} );
                else _dsp += string.Format("<li>{0} : {1} </li>", new string[2]{"Other", r} );
            }
        }

        if (string.IsNullOrEmpty(_dsp)) this.DisplayResponses="";
        else this.DisplayResponses = string.Format("<ul>{0}</ul>" , _dsp);
    }
}