using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollateralCreatorAdminWeb.Models
{
    public class PageDetail
    {
        public int NumberOfPages { get; set; }
        public bool PartnerBranded { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Rotation { get; set; }

    }
}