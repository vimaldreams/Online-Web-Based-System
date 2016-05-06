using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    public class MenuTree
    {
        public int ParentNodeID { get; set; }

        public int NodeID { get; set; }

        public string FamilyCode { get; set; }

        public string MenuText { get; set; } //~VP 09/10/2013

        //public string Product { get; set; }

        public bool IsRoot { get; set; } //~VP 09/10/2013

        public MenuTree(IDataReader dr)
        {
            if (DataHelper.ColumnExists(ref dr, "ParentNodeID"))
            {
                this.ParentNodeID = SqlHelper.DbSafeType<int>(dr["ParentNodeID"], 0);
            }
            if (DataHelper.ColumnExists(ref dr, "NodeID"))
            {
                this.NodeID = SqlHelper.DbSafeType<int>(dr["NodeID"], 0);
            }
            if (DataHelper.ColumnExists(ref dr, "FamilyCode"))
            {
                this.FamilyCode = SqlHelper.DbSafeType<string>(dr["FamilyCode"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "MenuText"))
            {
                this.MenuText = SqlHelper.DbSafeType<string>(dr["MenuText"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "IsRoot"))
            {
                this.IsRoot = SqlHelper.DbSafeType<bool>(dr["IsRoot"], false);
            }
        }
    }
}
