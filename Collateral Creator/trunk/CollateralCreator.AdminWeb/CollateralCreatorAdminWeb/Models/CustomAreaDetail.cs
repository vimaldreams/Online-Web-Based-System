using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollateralCreatorAdminWeb.Models
{
    public class CustomAreaDetail
    {
        public int PageNumber { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Rotation { get; set; }
        public string CustomAreaName { get; set; }
        public string Text { get; set; }
        public bool FullyEditable { get; set; }
        public int CharsPerLine { get; set; }
        public int Lines { get; set; }
        public int Align { get; set; }
        public int LineSpacing { get; set; }
        public bool PartnerBranded { get; set; }
        public int FontTypeId { get; set; }
        public int FontSize { get; set; }
        public int FontColour { get; set; }
        public string FontName { get; set; }
        public List<KeyValuePair<int, string>> FontColours = new List<KeyValuePair<int, string>>();
        public List<string> FontNames = new List<string>();
        public List<FontType> FontTypes = new List<FontType>();

    }
}