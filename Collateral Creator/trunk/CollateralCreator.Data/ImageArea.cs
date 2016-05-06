using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    [Serializable]
    public class ImageArea
    {
        public int ImageAreaID { get; set; }

        public int AreaID { get; set; }

        public byte[] Image { get; set; }

        public float Height { get; set; }

        public float Width { get; set; }

        public bool PartnerBranded { get; set; }

        public ImageArea(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "ImageAreaID"))
            {
                this.ImageAreaID = SqlHelper.DbSafeType<int>(dr["ImageAreaID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "AreaID"))
            {
                this.AreaID = SqlHelper.DbSafeType<int>(dr["AreaID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Image"))
            {
                this.Image = SqlHelper.DbSafeType<byte[]>(dr["Image"], null);
            }

            if (DataHelper.ColumnExists(ref dr, "Height"))
            {
                this.Height = SqlHelper.DbSafeType<float>(dr["Height"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "Width"))
            {
                this.Width = SqlHelper.DbSafeType<float>(dr["Width"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "PartnerBranded"))
            {
                this.PartnerBranded = SqlHelper.DbSafeType<bool>(dr["PartnerBranded"], false);
            }
        }

        public ImageArea()
        {

        }

    }
}
