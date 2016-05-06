using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CollateralCreator.Data
{
    /// <summary>
    /// Static helper class for dealing with data manipulation
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Method which tests if a column exists within an IDataReader
        /// </summary>
        /// <param name="dr">Reference to an IDataReader object</param>
        /// <param name="columnName">The column name to test exists</param>
        /// <returns>True if column is found, false otherwise.</returns>
        public static bool ColumnExists(ref IDataReader dr, string columnName)
        {
            if (dr != null)
            {
                // Filter view to only have the named column
                dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";

                // If count is 0 then column does not exist
                return dr.GetSchemaTable().DefaultView.Count > 0;
            }
            else
            {
                return false;
            }
        }

        public static SqlConnection GetSQLConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }
    }
}
