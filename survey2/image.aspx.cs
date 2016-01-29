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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

public partial class survey2_image : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string _s1 = Request.QueryString[0];
        //string _s2 = Request.QueryString[1];

        System.Drawing.Image _img = null; Graphics _gra = null; _img = new Bitmap(410, 60);

        _gra = Graphics.FromImage(_img);
        _gra.Clear(Color.White);
        _gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        _gra.TextRenderingHint = TextRenderingHint.AntiAlias;

        //_gra.RotateTransform(-3);
        _gra.DrawString(_s1, new Font("Freestyle Script", 25, FontStyle.Underline | FontStyle.Bold), Brushes.Black, 6, 11);
        //_gra.DrawString(_s2, new Font("Freestyle Script", 18, FontStyle.Bold), Brushes.Black, 510, 10);

        _gra.RotateTransform(3);
        //_gra.DrawString("Basic Demographics", new Font("Tw Cen MT Condensed", 17), Brushes.Olive, 300, 40);

        EncoderParameters _ens = new EncoderParameters(1);
        _ens.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

        _img.Save(Response.OutputStream, ImageCodecInfo.GetImageEncoders()[1], _ens);
        _gra.Dispose(); _img.Dispose();
    }
}
