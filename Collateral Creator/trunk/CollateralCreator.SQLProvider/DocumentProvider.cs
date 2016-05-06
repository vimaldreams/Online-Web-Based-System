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
    public class DocumentProvider
    {
        public static Document CreateDocument(int iTemplateID, bool bPartnerBradnded, string sLanguage, long iChannelPartnerID, string sChannelPartnerLoginID, string sChannelPartnerEmail)
        {
            Document doc = CreateDocumentFromTemplate(iTemplateID, bPartnerBradnded, sLanguage, iChannelPartnerID, sChannelPartnerLoginID, sChannelPartnerEmail);
            if (doc == null) return null;
            doc.Pages = GetDocumentPages(doc.DocumentID);
            foreach (Page p in doc.Pages)
            {
                p.Fonts = GetPageFonts(p.PageID);
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                    if (ca.TextArea != null)
                        ca.TextArea.Fonts = GetTextAreaFonts(ca.TextArea.TextAreaID);
                }
            }
            doc.Fonts = GetDocumentFonts(doc.DocumentID);
            return doc;
        }

        public static Document CreateEktronDocument(int iTemplateID, bool bPartnerBradnded, string sLanguage, long iChannelPartnerID, string sChannelPartnerLoginID, string sChannelPartnerEmail)
        {
            Document doc = CreateEktronDocumentFromTemplate(iTemplateID, bPartnerBradnded, sLanguage, iChannelPartnerID, sChannelPartnerLoginID, sChannelPartnerEmail);
            if (doc == null) return null;
            doc.Pages = GetDocumentPages(doc.DocumentID);
            foreach (Page p in doc.Pages)
            {
                p.Fonts = GetPageFonts(p.PageID);
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                    if (ca.TextArea != null)
                        ca.TextArea.Fonts = GetTextAreaFonts(ca.TextArea.TextAreaID);
                }
            }
            doc.Fonts = GetDocumentFonts(doc.DocumentID);
            return doc;
        }
        
        public static Document GetDocument(int iDocumentID)
        {
            Document doc = GetDocumentByID(iDocumentID);
            if (doc == null) return null;
            doc.Pages = GetDocumentPages(iDocumentID);
            foreach (Page p in doc.Pages)
            {
                p.Fonts = GetPageFonts(p.PageID);
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                    if (ca.TextArea != null) ca.TextArea.Fonts = GetTextAreaFonts(ca.TextArea.TextAreaID);
                }
            }
            doc.Fonts = GetDocumentFonts(iDocumentID);
            return doc;
        }

        public void GetFullDocument(Document doc)
        {
            if (doc == null) return;
            doc.Pages = GetDocumentPages(doc.DocumentID);
            foreach (Page p in doc.Pages)
            {
                p.Fonts = GetPageFonts(p.PageID);
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                    if (ca.TextArea != null) ca.TextArea.Fonts = GetTextAreaFonts(ca.TextArea.TextAreaID);
                }
            }
            doc.Fonts = GetDocumentFonts(doc.DocumentID);
        }

        public static Document GetCloneDocument(int iDocumentID)
        {
            Document doc = CloneDocument(iDocumentID);
            if (doc == null) return null;
            doc.Pages = GetDocumentPages(doc.DocumentID);
            foreach (Page p in doc.Pages)
            {
                p.Fonts = GetPageFonts(p.PageID);
                p.CustomizableAreas = GetPageCustomizableAreas(p.PageID);
                foreach (CustomizableArea ca in p.CustomizableAreas)
                {
                    ca.TextArea = GetTextArea(ca.AreaID);
                    ca.ImageArea = GetImageArea(ca.AreaID);
                    if (ca.TextArea != null) ca.TextArea.Fonts = GetTextAreaFonts(ca.TextArea.TextAreaID);
                }
            }
            doc.Fonts = GetDocumentFonts(doc.DocumentID);
            return doc;
        }
        
        private static Document CreateDocumentFromTemplate(int iTemplateID, bool bPartnerBradnded, string sLanguage, long iChannelPartnerID, string sChannelPartnerLoginID, string sChannelPartnerEmail)
        {
            Document doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_CreateFromTemplate");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[6];
                sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);
                sqlParams[1] = SqlHelper.CreateBitParam("@PartnerBranded", ParameterDirection.Input, bPartnerBradnded);
                sqlParams[2] = SqlHelper.CreateVarCharParam("@Language", 10, ParameterDirection.Input, sLanguage);
                sqlParams[3] = SqlHelper.CreateBigIntParam("@ChannelPartnerID", ParameterDirection.Input, iChannelPartnerID);
                sqlParams[4] = SqlHelper.CreateNVarCharParam("@ChannelPartnerLoginID", 50, ParameterDirection.Input, sChannelPartnerLoginID);
                sqlParams[5] = SqlHelper.CreateNVarCharParam("@ChannelPartnerEmail", 50, ParameterDirection.Input, sChannelPartnerEmail);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            doc = new Document(dr);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="partnerBranded"></param>
        /// <param name="language"></param>
        /// <param name="channelPartnerId"></param>
        /// <param name="loginId"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private static Document CreateEktronDocumentFromTemplate(int templateId, bool partnerBranded, string language, long channelPartnerId, string loginId, string emailAddress)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);        
            Document doc = new Document();
            int documentId = -1;
            DataSet ds = new DataSet();
            try
            {
            

                 //string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_CreateFromLanguageTemplate");

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.xcc_Document_CreateFromLanguageTemplate";
                        
                    sqlConnection.Open();
                    command.Connection = sqlConnection;

                    SqlParameter sqlParamID = new SqlParameter();
                    sqlParamID.Direction = ParameterDirection.Input;
                    sqlParamID.DbType = DbType.Int32;
                    sqlParamID.Value = templateId;
                    sqlParamID.ParameterName = "@TemplateID";
                    command.Parameters.Add(sqlParamID);

                    SqlParameter sqlParamLang = new SqlParameter();
                    sqlParamLang.Direction = ParameterDirection.Input;
                    sqlParamLang.DbType = DbType.AnsiString;
                    sqlParamLang.Size = 10;
                    sqlParamLang.Value = language;
                    sqlParamLang.ParameterName = "@Language";
                    command.Parameters.Add(sqlParamLang);

                    SqlParameter sqlParamBrand = new SqlParameter();
                    sqlParamBrand.Direction = ParameterDirection.Input;
                    sqlParamBrand.DbType = DbType.Boolean;
                    sqlParamBrand.Value = partnerBranded;
                    sqlParamBrand.ParameterName = "@PartnerBranded";
                    command.Parameters.Add(sqlParamBrand);

                    SqlParameter sqlParamPartnerId = new SqlParameter();
                    sqlParamPartnerId.Direction = ParameterDirection.Input;
                    sqlParamPartnerId.DbType = DbType.Int64;
                    sqlParamPartnerId.Value = channelPartnerId;
                    sqlParamPartnerId.ParameterName = "@ChannelPartnerID";
                    command.Parameters.Add(sqlParamPartnerId);

                    SqlParameter sqlParamLoginId = new SqlParameter();
                    sqlParamLoginId.Direction = ParameterDirection.Input;
                    sqlParamLoginId.DbType = DbType.AnsiString;
                    sqlParamLoginId.Size = 50;
                    sqlParamLoginId.Value = loginId;
                    sqlParamLoginId.ParameterName = "@ChannelPartnerLoginID";
                    command.Parameters.Add(sqlParamLoginId);

                    SqlParameter sqlParamEmail = new SqlParameter();
                    sqlParamEmail.Direction = ParameterDirection.Input;
                    sqlParamEmail.DbType = DbType.AnsiString;
                    sqlParamEmail.Size = 50;
                    sqlParamEmail.Value = emailAddress;
                    sqlParamEmail.ParameterName = "@ChannelPartnerEmail";
                    command.Parameters.Add(sqlParamEmail);

                    SqlParameter sqlDocId = new SqlParameter();
                    sqlDocId.Direction = ParameterDirection.Output;
                    sqlDocId.DbType = DbType.Int32;
                    sqlDocId.ParameterName = "@OutputDocumentId";
                    command.Parameters.Add(sqlDocId);

                    command.ExecuteNonQuery();


                    documentId = Convert.ToInt32(command.Parameters["@OutputDocumentId"].Value);

                    //get new document
                    using (SqlCommand commandDoc = new SqlCommand())
                    {
                        
                        commandDoc.CommandType = CommandType.StoredProcedure;
                        commandDoc.CommandText = "dbo.xcc_Document_GetByID";

                        commandDoc.Connection = sqlConnection;

                        SqlParameter sqlParam = new SqlParameter();
                        sqlParam.Direction = ParameterDirection.Input;
                        sqlParam.DbType = DbType.Int32;
                        sqlParam.Size = 4;
                        sqlParam.Value = documentId;
                        sqlParam.ParameterName = "@DocumentID";

                        commandDoc.Parameters.Add(sqlParam);
                        SqlDataAdapter adapt = new SqlDataAdapter(commandDoc);
                        adapt.Fill(ds, "Document");
                        //populate document object
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            doc.DocumentID = (int)dr["DocumentID"];
                            doc.TemplateID = (int)dr["TemplateID"];
                            doc.PartnerBranded = (bool)dr["PartnerBranded"];
                            doc.ChannelPartnerID = (int)dr["ChannelPartnerID"];
                            doc.ChannelPartnerLoginID = (string)dr["ChannelPartnerLoginID"];
                            doc.ChannelPartnerEmail = (string)dr["ChannelPartnerEmail"];
                            doc.CreatedDate = (DateTime)dr["CreatedDate"];
                            doc.ModifiedDate = (DateTime)dr["ModifiedDate"];
                            doc.DocumentStateID = (int)dr["DocumentStateID"];
                            doc.Name = (string)dr["Name"];
                            doc.Quantity = (int)dr["Quantity"];
                            doc.RecycledPaper = (bool)dr["RecycledPaper"];
                            doc.Duplex = (bool)dr["Duplex"];
                            doc.PaperOption = (string)dr["PaperOption"];
                            doc.PaperSize = (string)dr["PaperSize"];
                            doc.ColorCorrection = (string)dr["ColorCorrection"];
                            doc.PrintMode = (string)dr["PrintMode"];
                            doc.PageSize = (string)dr["PageSize"];
                            doc.PartNumber = (string)dr["PartNumber"];
                        }

                    }

                    return doc;


                }

            }
            catch(Exception error)
            {
                System.Diagnostics.Debug.Write(error.Message);
                return null;
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
               
            }


            //using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            //{
            //    //SqlParameter[] sqlParams = new SqlParameter[6];

        
            //    //sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);
            //    //sqlParams[1] = SqlHelper.CreateBitParam("@PartnerBranded", ParameterDirection.Input, bPartnerBradnded);
            //    //sqlParams[2] = SqlHelper.CreateVarCharParam("@Language", 10, ParameterDirection.Input, sLanguage);
            //    //sqlParams[3] = SqlHelper.CreateBigIntParam("@ChannelPartnerID", ParameterDirection.Input, iChannelPartnerID);
            //    sqlParams[4] = SqlHelper.CreateNVarCharParam("@ChannelPartnerLoginID", 50, ParameterDirection.Input, sChannelPartnerLoginID);
            //    sqlParams[5] = SqlHelper.CreateNVarCharParam("@ChannelPartnerEmail", 50, ParameterDirection.Input, sChannelPartnerEmail);
               

            //    try
            //    {
            //        sqlConn.Open();

            //        using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
            //        {
            //            while (dr.Read())
            //            {
            //                doc = new Document(dr);
            //            }
            //        }
            //    }
            //    catch (SqlException ex)
            //    {
            //        // Write error to debug window
            //        System.Diagnostics.Debug.Write(ex.Message);
            //    }
            //}
            //return doc;
        }

        private static Document GetDocumentByID(int iDocumentID)
        {
            Document doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetByID");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            doc = new Document(dr);
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

        private static Document CloneDocument(int iDocumentID)
        {
            Document doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_Clone");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            doc = new Document(dr);
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

        public static Document GetDocumentFromTemplate(int iTemplateID, bool bPartnerBranded)
        {
            Document doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetFromTemplate");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);
                sqlParams[1] = SqlHelper.CreateBitParam("@PartnerBranded", ParameterDirection.Input, bPartnerBranded);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            doc = new Document(dr);
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

        private static List<Page> GetDocumentPages(int iDocumentID)
        {
            List<Page> ltp = new List<Page>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetPages");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentID);

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
        
        private static List<Font> GetDocumentFonts(int iDocumentID)
        {
            List<Font> lf = new List<Font>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetFonts");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Font f = new Font(dr);
                            lf.Add(f);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return lf;
        }

        private static List<Font> GetPageFonts(int iPageID)
        {
            List<Font> lf = new List<Font>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Page_GetFonts");

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
                            Font f = new Font(dr);
                            lf.Add(f);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return lf;
        }

        private static List<Font> GetTextAreaFonts(int iTextAreaID)
        {
            List<Font> lf = new List<Font>();
            
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_TextArea_GetFonts");

            using (SqlConnection sqlConn = DataHelper.GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@TextAreaID", ParameterDirection.Input, iTextAreaID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Font f = new Font(dr);
                            lf.Add(f);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            }
            return lf;
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
    }
}
