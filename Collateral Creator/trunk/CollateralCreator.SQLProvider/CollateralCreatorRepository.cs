using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Common.Data;
using CollateralCreator.Data;

namespace CollateralCreator.SQLProvider
{
    public class CollateralCreatorRepository
    {
        protected static string databaseOwner = "dbo";	// overwrite in web.config

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetSQLConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }

        //public static List<MenuTree> GetMenuTree(string language)
        //{
        //    List<MenuTree> menuitems = new List<MenuTree>();
        //    string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_MenuTree_GetTreeNodes");

        //    using (SqlConnection sqlConn = GetSQLConnection())
        //    {
        //        SqlParameter[] sqlParams = new SqlParameter[1];
        //        sqlParams[0] = SqlHelper.CreateVarCharParam("@Language", 10, ParameterDirection.Input, language);

        //        try
        //        {
        //            sqlConn.Open();

        //            using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
        //            {
        //                while (dr.Read())
        //                {
        //                    MenuTree item = new MenuTree(dr);
        //                    menuitems.Add(item);
        //                }
        //            }
        //        }
        //        catch (SqlException ex)
        //        {
        //            // Write error to debug window
        //            System.Diagnostics.Debug.Write(ex.Message);
        //        }
        //        finally
        //        {
        //            sqlConn.Close();
        //        }
        //    }

        //    return menuitems;
        //}

        public static List<MenuTree> GetMenuTreeData(string language)
        {
            List<MenuTree> menuitems = new List<MenuTree>();
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_MenuTree_GetData");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateVarCharParam("@Language", 10, ParameterDirection.Input, language);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            MenuTree item = new MenuTree(dr);
                            menuitems.Add(item);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return menuitems;
        }

        public static List<MenuTree> GetMenuTreeProductCodes()
        {
            List<MenuTree> menuproductitems = new List<MenuTree>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_MenuTree_ProductCodes");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[0];
                
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            MenuTree item = new MenuTree(dr);
                            menuproductitems.Add(item);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return menuproductitems;
        }

        public static List<Template> GetCustomTemplates_MenuTreeNode(int nodeID, string language) //~VP 09/10/2013
        {
            List<Template> customtemplates = new List<Template>();
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Template_TreeNode");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateIntParam("@NodeID", ParameterDirection.Input, nodeID);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@Language", 10, ParameterDirection.Input, language);
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Template template = new Template(dr);
                            customtemplates.Add(template);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return customtemplates;
        }

        public static Document GetDocumentByID(int iDocumentID)
        {
            Document doc = null;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetByID");

            using (SqlConnection sqlConn = GetSQLConnection())
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
                    return null;
                }
                finally
                {
                    sqlConn.Close();
                }
            }
            return doc;
        }

        public static List<Document> GetAllDocuments(long lChannelPartnerID, string sCountry)
        {
            List<Document> latestdocs = new List<Document>();
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetLatest");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateBigIntParam("@ChannelPartnerID", ParameterDirection.Input, lChannelPartnerID);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@Country", 50, ParameterDirection.Input, sCountry);
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Document doc = new Document(dr);
                            latestdocs.Add(doc);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return latestdocs;
        }

        public static List<Document> GetAllPrintHouseHistoryDocuments(long lChannelPartnerID, string sCountry)
        {
            List<Document> printhousedocs = new List<Document>();
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_DocumentHistory_GetLatest");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateBigIntParam("@ChannelPartnerID", ParameterDirection.Input, lChannelPartnerID);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@Country", 50, ParameterDirection.Input, sCountry);
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Document doc = new Document(dr);
                            printhousedocs.Add(doc);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return printhousedocs;
        }

        public static void DeleteDocumentByID(int iDocumentId)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_DeleteByID");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void ChangeDocumentStatusByID(int iDocumentId, string sDocumentStatus, int iQuantity, string sDocumentCarrier)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_ChangeStatusByID");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@DocumentStateName", 50, ParameterDirection.Input, sDocumentStatus);
                sqlParams[2] = SqlHelper.CreateIntParam("@Quantity", ParameterDirection.Input, iQuantity);
                sqlParams[3] = SqlHelper.CreateNVarCharParam("@Carrier", 255, ParameterDirection.Input, sDocumentCarrier);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

        }

        public static void CreateDocumentHistory(int iDocumentId, string sDocumentStatus)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_CreateHistory");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@DocumentStateName", 50, ParameterDirection.Input, sDocumentStatus);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void DocumentUpdateTextArea(int iAreaID, int iTextAreaID, string iText)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_UpdateTextArea");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = SqlHelper.CreateIntParam("@AreaID", ParameterDirection.Input, iAreaID);
                sqlParams[1] = SqlHelper.CreateIntParam("@TextAreaID", ParameterDirection.Input, iTextAreaID);
                sqlParams[2] = SqlHelper.CreateNTextParam("@Text", ParameterDirection.Input, iText);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void DocumentUploadImage(string sImageAreaID, byte[] bImage)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_CustomizableArea_UploadImage");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateNTextParam("@ImageAreaID", ParameterDirection.Input, sImageAreaID);
                sqlParams[1] = SqlHelper.CreateImageParam("@Image", bImage.Length, bImage);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

        }

        public static byte[] DocumentDownloadImage(string sImageAreaID)
        {
            byte[] byteData = null;
            
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_CustomizableArea_DownloadImage");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateNTextParam("@ImageAreaID", ParameterDirection.Input, sImageAreaID);
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            byteData = SqlHelper.GetType<byte[]>(dr["Image"]);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
            return byteData;
        }

        public static void Log_DocumentDownload(int iDocumentId)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_DownloadLog");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static string GetDocumentTemplateName(int iDocumentId, int iTemplateId, bool bPartnerBradnded)
        {
            string sTemplateName = string.Empty;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetTemplateName");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateId);
                sqlParams[2] = SqlHelper.CreateBitParam("@PartnerBrand", ParameterDirection.Input, bPartnerBradnded);
                sqlParams[3] = SqlHelper.CreateNVarCharParam("@TemplateName", 255, ParameterDirection.InputOutput, sTemplateName);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);

                    sTemplateName = sqlParams[3].Value.ToString(); 

                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return sTemplateName;
        }

        public static void DocumentUpdateAddressDetails(int iDocumentId, int iTemplateId, bool bPartnerBradnded, string sAttention,
                                                          string sCompanyName, string sCompanyID, string sAddressLine1, string sAddressLine2, string sCity, string sState,
                                                          string sPostCode, string sFirstName, string sLastName, string sPhone, string sEmail, string sChannelPartnerPhone, string sCountry)
        {
            string sTemplateName = string.Empty;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_UpdateAddressContactDetails");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[17];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateId);
                sqlParams[2] = SqlHelper.CreateBitParam("@PartnerBranded", ParameterDirection.Input, bPartnerBradnded);
                sqlParams[3] = SqlHelper.CreateNVarCharParam("@Attention", 255, ParameterDirection.Input, sAttention);
                sqlParams[4] = SqlHelper.CreateNVarCharParam("@CompanyName", 255, ParameterDirection.Input, sCompanyName);
                sqlParams[5] = SqlHelper.CreateNVarCharParam("@AddressLine1", 255, ParameterDirection.Input, sAddressLine1);
                sqlParams[6] = SqlHelper.CreateNVarCharParam("@AddressLine2", 255, ParameterDirection.Input, sAddressLine2);
                sqlParams[7] = SqlHelper.CreateNVarCharParam("@City", 255, ParameterDirection.Input, sCity);
                sqlParams[8] = SqlHelper.CreateNVarCharParam("@State", 255, ParameterDirection.Input, sState);
                sqlParams[9] = SqlHelper.CreateNVarCharParam("@PostCode", 255, ParameterDirection.Input, sPostCode);
                sqlParams[10] = SqlHelper.CreateNVarCharParam("@FirstName", 255, ParameterDirection.Input, sFirstName);
                sqlParams[11] = SqlHelper.CreateNVarCharParam("@LastName", 255, ParameterDirection.Input, sLastName);
                sqlParams[12] = SqlHelper.CreateNVarCharParam("@Phone", 255, ParameterDirection.Input, sPhone);
                sqlParams[13] = SqlHelper.CreateNVarCharParam("@Email", 255, ParameterDirection.Input, sEmail);
                sqlParams[14] = SqlHelper.CreateNVarCharParam("@ChannelPartnerPhone", 255, ParameterDirection.Input, sChannelPartnerPhone);
                sqlParams[15] = SqlHelper.CreateNVarCharParam("@Country", 255, ParameterDirection.Input, sCountry);
                sqlParams[16] = SqlHelper.CreateNVarCharParam("@CompanyID", 50, ParameterDirection.Input, sCompanyID);


                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void UpdateDocumentName(int iDocumentId, string sDocumentName)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_UpdateDocumentName");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateNVarCharParam("@DocumentName", 255, ParameterDirection.Input, sDocumentName);
                try
                {
                    sqlConn.Open();
                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static List<Document> GetDocumentPrintQueue()
        {
            List<Document> ldocuments = new List<Document>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetAdminDocuments");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[0];
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Document doc = new Document(dr);
                            ldocuments.Add(doc);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return ldocuments;
        }

        public static void AdminDocument_UpdateStatus(int iDocumentId, string sDocumentStatus, string sTrackingNumber, string sDocumentCarrier)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_DocumentAdmin_UpdateStatusByID");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@DocumentStateName", 50, ParameterDirection.Input, sDocumentStatus);
                sqlParams[2] = SqlHelper.CreateNVarCharParam("@TrackingNumber", 255, ParameterDirection.Input, sTrackingNumber);
                sqlParams[3] = SqlHelper.CreateNVarCharParam("@Carrier", 255, ParameterDirection.Input, sDocumentCarrier);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

        }

        public static void GetDocumentTemplateInfo(int iDocumentId, ref int iTemplateID, ref bool bPartnerBradnded)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetTemplateInfo");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = SqlHelper.CreateIntParam("@DocumentID", ParameterDirection.Input, iDocumentId);
                sqlParams[1] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.InputOutput, iTemplateID);
                sqlParams[2] = SqlHelper.CreateBitParam("@PartnerBrand", ParameterDirection.InputOutput, bPartnerBradnded);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);

                    iTemplateID = Convert.ToInt16(sqlParams[1].Value);
                    bPartnerBradnded = (bool)sqlParams[2].Value;

                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }

        public static void AdminDocument_UpdateBatchStatus(string sDocumentID, string sDocumentStatus, string sTrackingNumber, string sDocumentCarrier)
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_DocumentAdmin_UpdateBatchStatus");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = SqlHelper.CreateNTextParam("@DocumentID", ParameterDirection.Input, sDocumentID);
                sqlParams[1] = SqlHelper.CreateVarCharParam("@DocumentStateName", 50, ParameterDirection.Input, sDocumentStatus);
                sqlParams[2] = SqlHelper.CreateNTextParam("@TrackingNumber", ParameterDirection.Input, sTrackingNumber);
                sqlParams[3] = SqlHelper.CreateNTextParam("@DocumentCarrier", ParameterDirection.Input, sDocumentCarrier);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);

                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

        }

        public static List<Document> GetDocumentByIDs(string sDocumentID)
        {
            List<Document> documents = new List<Document>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetByIDs");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateNTextParam("@DocumentID", ParameterDirection.Input, sDocumentID);

                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Document doc = new Document(dr);
                            documents.Add(doc);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return documents;
        }

        public static int UpdateTemplateButton(int iTemplateID, string sChannelPartnerLoginID, bool bCustomize, bool bPartnerBrand)
        {
            int iNodeID = 0;

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Template_CreateUpdateTemplateButton");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[5];
                sqlParams[0] = SqlHelper.CreateIntParam("@TemplateID", ParameterDirection.Input, iTemplateID);
                sqlParams[1] = SqlHelper.CreateNVarCharParam("@ChannelPartnerLoginID", 255, ParameterDirection.Input, sChannelPartnerLoginID);
                sqlParams[2] = SqlHelper.CreateBitParam("@IsCustomized", ParameterDirection.Input, bCustomize);
                sqlParams[3] = SqlHelper.CreateBitParam("@IsPartnerBrand", ParameterDirection.Input, bPartnerBrand);
                sqlParams[4] = SqlHelper.CreateIntParam("@NodeID", ParameterDirection.InputOutput, iNodeID);

                try
                {
                    sqlConn.Open();
                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                    iNodeID = Convert.ToInt16(sqlParams[4].Value); 
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return iNodeID;
        }

        public static List<Document> GetDocumentArchiveHistory()
        {
            List<Document> ldocuments = new List<Document>();

            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_Document_GetArchiveAdminDocuments");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[0];
                try
                {
                    sqlConn.Open();

                    using (SqlDataReader dr = SqlHelper.ExecuteReader(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams))
                    {
                        while (dr.Read())
                        {
                            Document doc = new Document(dr);
                            ldocuments.Add(doc);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }

            return ldocuments;
        }

        public static void Log_SmartCentreQueryString(string sQueryString) 
        {
            string storedProcedureName = string.Format("{0}.{1}", "dbo", "xcc_SmartCentre_QueryStringLog");

            using (SqlConnection sqlConn = GetSQLConnection())
            {
                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = SqlHelper.CreateNTextMaxParam("@QueryString", ParameterDirection.Input, sQueryString);
                try
                {
                    sqlConn.Open();

                    SqlHelper.ExecuteNonQuery(sqlConn, CommandType.StoredProcedure, storedProcedureName, sqlParams);
                }
                catch (SqlException ex)
                {
                    // Write error to debug window
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                finally
                {
                    sqlConn.Close();
                }
            }
        }
    }
}
