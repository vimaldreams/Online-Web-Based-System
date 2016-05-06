using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    [Serializable]
    public class CustomizableArea
    {
        protected static string databaseOwner = "dbo";	// overwrite in web.config
        
        public int AreaID { get; set; }

        public int PageID { get; set; }

        public float XPos { get; set; }

        public float YPos { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public float Rotation { get; set; }

        public string Name { get; set; }

        public TextArea TextArea { get; set; }
        public ImageArea ImageArea { get; set; }
      
        public CustomizableArea(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "AreaID"))
            {
                this.AreaID = SqlHelper.DbSafeType(dr["AreaID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "PageID"))
            {
                this.PageID = SqlHelper.DbSafeType(dr["PageID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "XPos"))
            {
                this.XPos = SqlHelper.DbSafeType<float>(dr["XPos"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "YPos"))
            {
                this.YPos = SqlHelper.DbSafeType<float>(dr["YPos"], 0);
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

            if (DataHelper.ColumnExists(ref dr, "Name"))
            {
                this.Name = SqlHelper.DbSafeType(dr["Name"], String.Empty);
            }
        }

    }
}
