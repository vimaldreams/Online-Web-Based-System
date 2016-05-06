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
    public class TextArea
    {
        public int TextAreaID { get; set; }

        public int AreaID { get; set; }

        public string Text { get; set; }

        public bool FullyEditable { get; set; }

        public int CharsPerLine { get; set; }

        public int Lines { get; set; }

        public string Align { get; set; }

        public int LineSpacing { get; set; }

        public List<Font> Fonts { get; set; }

        public TextArea(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "TextAreaID"))
            {
                this.TextAreaID = SqlHelper.DbSafeType<int>(dr["TextAreaID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "AreaID"))
            {
                this.AreaID = SqlHelper.DbSafeType<int>(dr["AreaID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Text"))
            {
                this.Text = SqlHelper.DbSafeType<string>(dr["Text"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "FullyEditable"))
            {
                this.FullyEditable = SqlHelper.DbSafeType<bool>(dr["FullyEditable"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "CharsPerLine"))
            {
                this.CharsPerLine = SqlHelper.DbSafeType<int>(dr["CharsPerLine"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Lines"))
            {
                this.Lines = SqlHelper.DbSafeType<int>(dr["Lines"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Align"))
            {
                this.Align = SqlHelper.DbSafeType<string>(dr["Align"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "LineSpacing"))
            {
                this.LineSpacing = SqlHelper.DbSafeType<int>(dr["LineSpacing"], 0);
            }
        }
    }
}
