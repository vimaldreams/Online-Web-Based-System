using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    [Serializable]
    public class Page
    {
        public int PageID { get; set; }

        public int TemplateID { get; set; }

        public bool PartnerBranded { get; set; }

        public int DocumentID { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Rotation { get; set; }

        public int PageNumber { get; set; }

        public List<Font> Fonts { get; set; }

        public List<CustomizableArea> CustomizableAreas { get; set; }
        
        public Page(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "PageID"))
            {
                this.PageID = SqlHelper.DbSafeType<int>(dr["PageID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "TemplateID"))
            {
                this.TemplateID = SqlHelper.DbSafeType<int>(dr["TemplateID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "PartnerBranded"))
            {
                this.PartnerBranded = SqlHelper.DbSafeType<bool>(dr["PartnerBranded"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "DocumentID"))
            {
                this.DocumentID = SqlHelper.DbSafeType<int>(dr["DocumentID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Height"))
            {
                this.Height = SqlHelper.DbSafeType<float>(dr["Height"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Width"))
            {
                this.Width = SqlHelper.DbSafeType<float>(dr["Width"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Rotation"))
            {
                this.Rotation = SqlHelper.DbSafeType<float>(dr["Rotation"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "PageNumber"))
            {
                this.PageNumber = SqlHelper.DbSafeType<int>(dr["PageNumber"], 0);
            }
        }
    }
}
