using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XCTUploadToolWeb
{
    public partial class TxtBlock : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int BlockID
        {
            set { LblBlockID.Text = value.ToString(); }
        }

        public int XCoord
        {
            get { return Convert.ToInt32(TxtX.Text); }
        }

        public int YCoord
        {
            get { return Convert.ToInt32(TxtY.Text); }
        }

        public int Height
        {
            get { return Convert.ToInt32(TxtHeight.Text); }
        }

        public int Width
        {
            get { return Convert.ToInt32(TxtWidth.Text); }
        }

        public string FontName
        {
            get { return DrpFont.SelectedValue; }
        }

        public int FontSize
        {
            get { return Convert.ToInt32(TxtFontSize.Text); }
        }

        public int FontColour
        {
            get 
            {
                if (ChkUseRGB.Checked)
                {
                    int rcolour = Convert.ToInt32(TxtR.Text);
                    int gcolour = Convert.ToInt32(TxtG.Text);
                    int bcolour = Convert.ToInt32(TxtB.Text);
                    int rcalc = (rcolour * 65536) + (gcolour * 256) + bcolour;
                    return rcalc;
                }
                else
                {
                    return Convert.ToInt32(DrpFontColour.SelectedValue);
                }
            }
        }

        public int Rotation
        {
            get { return Convert.ToInt32(TxtRotation.Text); }
        }

         public int PageNo
        {
            get { return Convert.ToInt32(TxtPageNo.Text); }
        }

        
        
    }
}