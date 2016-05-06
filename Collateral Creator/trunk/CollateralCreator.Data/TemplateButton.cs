using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Data;

namespace CollateralCreator.Data
{
    public class TemplateButton
    {
        public int TemplateID { get; set; }

        public string ChannelPartnerLoginID { get; set; }

        public DateTime DateCustomized { get; set; }

        public bool IsCustomized { get; set; }

        public bool IsPartnerBrand { get; set; }

        public TemplateButton(IDataReader dr)
        {
             if (DataHelper.ColumnExists(ref dr, "TemplateID"))
             {
                 this.TemplateID = SqlHelper.DbSafeType<int>(dr["TemplateID"], 0);
             }

             if (DataHelper.ColumnExists(ref dr, "ChannelPartnerLoginID"))
             {
                 this.ChannelPartnerLoginID = SqlHelper.DbSafeType<string>(dr["ChannelPartnerLoginID"], string.Empty);
             }

             if (DataHelper.ColumnExists(ref dr, "DateCustomized"))
             {
                 this.DateCustomized = SqlHelper.DbSafeType<DateTime>(dr["DateCustomized"], DateTime.MinValue);
             }

             if (DataHelper.ColumnExists(ref dr, "IsCustomized"))
             {
                 this.IsCustomized = SqlHelper.DbSafeType<bool>(dr["IsCustomized"], false);
             }

             if (DataHelper.ColumnExists(ref dr, "IsPartnerBrand"))
             {
                 this.IsPartnerBrand = SqlHelper.DbSafeType<bool>(dr["IsPartnerBrand"], false);
             }

        }
    }
}
