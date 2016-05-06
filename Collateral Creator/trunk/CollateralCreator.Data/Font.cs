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
    public class Font
    {
        public int FontID { get; set; }
        
        public int FontTypeID { get; set; }

        public float Size { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }

        public bool Underline { get; set; }

        public bool Strikethrough { get; set; }
        
        public long Color { get; set; }
        
        public string Name { get; set; }

        public Font(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "FontID"))
            {
                this.FontID = SqlHelper.DbSafeType<int>(dr["FontID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "FontTypeID"))
            {
                this.FontTypeID = SqlHelper.DbSafeType<int>(dr["FontTypeID"], 0);
            }
            
            if (DataHelper.ColumnExists(ref dr, "Size"))
            {
                this.Size = SqlHelper.DbSafeType<float>(dr["Size"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Bold"))
            {
                this.Bold = SqlHelper.DbSafeType<bool>(dr["Bold"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Italic"))
            {
                this.Italic = SqlHelper.DbSafeType<bool>(dr["Italic"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Underline"))
            {
                this.Underline = SqlHelper.DbSafeType<bool>(dr["Underline"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Strikethrough"))
            {
                this.Strikethrough = SqlHelper.DbSafeType<bool>(dr["Strikethrough"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Color"))
            {
                this.Color = SqlHelper.DbSafeType<long>(dr["Color"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Name"))
            {
                this.Name = SqlHelper.DbSafeType<string>(dr["Name"], string.Empty);
            }
        }
    }
}
