using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CollateralCreator.SQLProvider.Helpers
{
    public static class SqlHelper
    {
        private static SqlConnection GetSQLConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        public static T DbSafeType<T>(object o, T defaultValue)
        {
            if (o == null)
                return defaultValue;

            if (!IsDbNull(o))
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType(o, typeof(T));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        public static int DbSafeInt32(object obj)
        {
            return DbSafeInt32(obj, default(int));
        }

        public static int DbSafeInt32(object obj, int defaultValue)
        {
            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToInt32(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        public static long DbSafeInt64(object obj)
        {
            return DbSafeInt64(obj, default(long));
        }

        public static long DbSafeInt64(object obj, long defaultValue)
        {
            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToInt64(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double DbSafeDouble(object obj, double defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToDouble(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DbSafeString(object obj)
        {
            return DbSafeString(obj, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string DbSafeString(object obj, string defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (!IsDbNull(obj))
            {
                // Big assumption that Convert.ToString cannot throw 
                // an exception
                defaultValue = Convert.ToString(obj);
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal DbSafeDecimal(object obj)
        {
            return DbSafeDecimal(obj, default(decimal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal DbSafeDecimal(object obj, decimal defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToDecimal(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime DbSafeDate(object obj, DateTime defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToDateTime(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool DbSafeBool(object obj)
        {
            return DbSafeBool(obj, default(bool));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool DbSafeBool(object obj, bool defaultValue)
        {
            if (obj == null)
                return defaultValue;

            if (!IsDbNull(obj))
            {
                try
                {
                    defaultValue = Convert.ToBoolean(obj);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Checks the result set to see if a particular column exists, useful if
        /// you are extending this class to include your own attributes
        /// </summary>
        /// <param name="objDtRw">Recordset to check</param>
        /// <param name="columnName">Column name to look for</param>
        /// <returns>True if the column exists</returns>
        public static bool ColumnExists(ref DataSet objDs, string columnName)
        {
            // Preconditions
            if (objDs == null)
            {
                throw new ArgumentNullException("objDtRw");
            }
            if (string.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException("columnName");
            }

            // Loop through all fields and return whether field exists.
            for (var i = 0; i < 7; i++)
            {
                if (objDs.Tables[0].Columns.Contains(columnName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Return whether the specifield field is set to null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDbNull(object value)
        {
            return value == DBNull.Value;
        }

        /// <summary>
        /// Method return the value of the object casted to the type
        /// T if the value read is not DBNull.Value, else returns the default
        /// value specified in the input parameter.
        /// </summary>
        /// <typeparam name="T">The type to cast the database value to.</typeparam>
        /// <param name="value">The database value being read.</param>
        /// <returns>The type specified.</returns>
        public static T GetType<T>(object value)
        {
            return GetType(value, default(T));
        }

        /// <summary>
        /// Method return the value of the object casted to the type
        /// T if the value read is not DBNull.Value, else returns the default
        /// value specified in the input parameter.
        /// </summary>
        /// <typeparam name="T">The type to cast the database value to.</typeparam>
        /// <param name="value">The database value being read.</param>
        /// <param name="defaultValue">The value to assign when the value being read from the database is DBNull.Value.</param>
        /// <returns>The type specified.</returns>
        public static T GetType<T>(object value, T defaultValue)
        {
            if (value != null && !IsDbNull(value))
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch (FormatException ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
                catch (InvalidCastException ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
            return defaultValue;
        }
    }
}
