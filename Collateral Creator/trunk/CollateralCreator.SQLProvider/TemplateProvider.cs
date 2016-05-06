using System;
using System.Collections.Generic;
using System.Text;
using Common.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using CollateralCreator.Data;

namespace CollateralCreator.SQLProvider
{
    public class TemplateProvider
    {
        public static Template GetTemplate(int iTemplateID)
        {
            Template temp = GetTemplateByID(iTemplateID);
            if (temp == null) return null;
            temp.Pages = GetTemplatePages(iTemplateID);
            foreach (Page p in temp.Pages)
            {
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                }
            }
            return temp;
        }
        
        private static ImageArea GetImageArea(int iCustomizableAreaID)
        {
            ImageArea ia = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_CustomizableArea_GetImageArea");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@CustomizableAreaID", ParameterDirection.Input, iCustomizableAreaID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            ia = new ImageArea(dr);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return ia;
        }

        private static TextArea GetTextArea(int iCustomizableAreaID)
        {
            TextArea ta = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_CustomizableArea_GetTextArea");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@CustomizableAreaID", ParameterDirection.Input, iCustomizableAreaID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            ta = new TextArea(dr);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return ta;
        }

        private static List<CustomizableArea> GetPageCustomizableAreas(int iPageID)
        {
            List<CustomizableArea> lca = new List<CustomizableArea>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Page_GetCustomizableAreas");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@PageID", ParameterDirection.Input, iPageID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            CustomizableArea ca = new CustomizableArea(dr);
                            lca.Add(ca);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return lca;
        }
        
        private static List<Page> GetTemplatePages(int iTemplateID)
        {
            List<Page> ltp = new List<Page>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Template_GetPages");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Page tp = new Page(dr);
                            ltp.Add(tp);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return ltp;
        }

        private static Template GetTemplateByID(int iTemplateID)
        {
            Template doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Template_GetByID");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            doc = new Template(dr);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return doc;
        }

        public static List<TemplateButton> GetTemplateButton(string sChannelPartnerLoginID)
        {
            List<TemplateButton> ltemplates = new List<TemplateButton>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Template_GetTemplateButton");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateNVarCharParam("@ChannelPartnerLoginID", 255, ParameterDirection.Input, sChannelPartnerLoginID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            TemplateButton temp = new TemplateButton(dr);
                            ltemplates.Add(temp);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return ltemplates;
        }
    }
}
