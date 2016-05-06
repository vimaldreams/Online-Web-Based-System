using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    public class Template
    {
        public int TemplateID { get; set; }

        public int LanguageID { get; set; }

        public bool IsPartnerBranded { get; set; }

        public bool IsXeroxBranded { get; set; } 
        
        public bool IsDeleted { get; set; }
        
        public string Name { get; set; }

        public int Quantity { get; set; }

        public bool RecycledPaper { get; set; }

        public bool Duplex { get; set; }

        public string PaperOption { get; set; }

        public string PaperSize { get; set; }

        public string ColorCorrection { get; set; }

        public string PrintMode { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public string Flag { get; set; }

        public List<CollateralCreator.Data.Page> Pages { get; set; }
        public List<Font> Fonts { get; set; }

        public Template(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "TemplateID"))
            {
                this.TemplateID = SqlHelper.DbSafeType<int>(dr["TemplateID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "LanguageID"))
            {
                this.LanguageID = SqlHelper.DbSafeType<int>(dr["LanguageID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "IsPartnerBranded"))
            {
                this.IsPartnerBranded = SqlHelper.DbSafeType<bool>(dr["IsPartnerBranded"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "IsXeroxBranded"))
            {
                this.IsXeroxBranded = SqlHelper.DbSafeType<bool>(dr["IsXeroxBranded"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "IsDeleted"))
            {
                this.IsDeleted = SqlHelper.DbSafeType<bool>(dr["IsDeleted"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Name"))
            {
                this.Name = SqlHelper.DbSafeType<string>(dr["Name"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "Quantity"))
            {
                this.Quantity = SqlHelper.DbSafeType<int>(dr["Quantity"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "RecycledPaper"))
            {
                this.RecycledPaper = SqlHelper.DbSafeType<bool>(dr["RecycledPaper"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "Duplex"))
            {
                this.Duplex = SqlHelper.DbSafeType<bool>(dr["Duplex"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "PaperOption"))
            {
                this.PaperOption = SqlHelper.DbSafeType<string>(dr["PaperOption"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "PaperSize"))
            {
                this.PaperSize = SqlHelper.DbSafeType<string>(dr["PaperSize"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "ColorCorrection"))
            {
                this.ColorCorrection = SqlHelper.DbSafeType<string>(dr["ColorCorrection"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "PrintMode"))
            {
                this.PrintMode = SqlHelper.DbSafeType<string>(dr["PrintMode"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "Description"))
            {
                this.Description = SqlHelper.DbSafeType<string>(dr["Description"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "Detail"))
            {
                this.Detail = SqlHelper.DbSafeType<string>(dr["Detail"], string.Empty);
            }
        }
    }
}
