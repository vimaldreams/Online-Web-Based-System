using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using Common.Data;

namespace CollateralCreator.Data
{
    [Serializable]
    public class Document
    {
        public int DocumentID { get; set; }

        public int TemplateID { get; set; }
        
        public bool PartnerBranded { get; set; }

        public int ChannelPartnerID { get; set; }

        public string ChannelPartnerLoginID { get; set; }

        public string CompanyID { get; set; }

        public string ChannelPartnerEmail { get; set; }

        public string ChannelPartnerPhone { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public int DocumentStateID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public bool RecycledPaper { get; set; }

        public bool Duplex { get; set; }

        public string PaperOption { get; set; }

        public string PaperSize { get; set; }

        public string ColorCorrection { get; set; }

        public string PrintMode { get; set; }

        public string PageSize { get; set; }

        public string TrackingNumber { get; set; }

        public string PartNumber { get; set; }

        public string Carrier { get; set; }

        //Contact info for Ordering the Document
        public string Attention { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        //print house information 
        public DateTime datestamp { get; set; }
        public string DocumentStateName { get; set; }

        public List<CollateralCreator.Data.Page> Pages { get; set; }
        public List<Font> Fonts { get; set; }

        /// <summary>
        ///  Data reader constructor
        /// </summary>
        /// <param name="dr"></param>
        public Document(IDataReader dr)
        {
            #region Code
            if (DataHelper.ColumnExists(ref dr, "DocumentID"))
            {
                this.DocumentID = SqlHelper.DbSafeType<int>(dr["DocumentID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "TemplateID"))
            {
                this.TemplateID = SqlHelper.DbSafeType<int>(dr["TemplateID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "PartnerBranded"))
            {
                this.PartnerBranded = SqlHelper.DbSafeType<bool>(dr["PartnerBranded"], false);
            }
            
            if (DataHelper.ColumnExists(ref dr, "ChannelPartnerID"))
            {
                this.ChannelPartnerID = SqlHelper.DbSafeType<int>(dr["ChannelPartnerID"], 0);
            }

            if (DataHelper.ColumnExists(ref dr, "ChannelPartnerLoginID"))
            {
                this.ChannelPartnerLoginID = SqlHelper.DbSafeType<string>(dr["ChannelPartnerLoginID"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "CompanyID"))
            {
                this.CompanyID = SqlHelper.DbSafeType<string>(dr["CompanyID"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "ChannelPartnerEmail"))
            {
                this.ChannelPartnerEmail = SqlHelper.DbSafeType<string>(dr["ChannelPartnerEmail"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "ChannelPartnerPhone"))
            {
                this.ChannelPartnerPhone = SqlHelper.DbSafeType<string>(dr["ChannelPartnerPhone"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "CreatedDate"))
            {
                this.CreatedDate = SqlHelper.DbSafeType<DateTime>(dr["CreatedDate"], DateTime.MinValue);
            }

            if (DataHelper.ColumnExists(ref dr, "ModifiedDate"))
            {
                this.ModifiedDate = SqlHelper.DbSafeType<DateTime>(dr["ModifiedDate"], DateTime.MinValue);
            }

            if (DataHelper.ColumnExists(ref dr, "IsDeleted"))
            {
                this.IsDeleted = SqlHelper.DbSafeType<bool>(dr["IsDeleted"], false);
            }

            if (DataHelper.ColumnExists(ref dr, "DocumentStateID"))
            {
                this.DocumentStateID = SqlHelper.DbSafeType<int>(dr["DocumentStateID"], 0);
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

            if (DataHelper.ColumnExists(ref dr, "PageSize"))
            {
                this.PageSize = SqlHelper.DbSafeType<string>(dr["PageSize"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "TrackingNumber"))
            {
                this.TrackingNumber = SqlHelper.DbSafeType<string>(dr["TrackingNumber"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "PartNumber"))
            {
                this.PartNumber = SqlHelper.DbSafeType<string>(dr["PartNumber"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "Carrier"))
            {
                this.Carrier = SqlHelper.DbSafeType<string>(dr["Carrier"], string.Empty);
            }
            //Contact info for Ordering the Document

            if (DataHelper.ColumnExists(ref dr, "Attention"))
            {
                this.Attention = SqlHelper.DbSafeType<string>(dr["Attention"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "CompanyName"))
            {
                this.CompanyName = SqlHelper.DbSafeType<string>(dr["CompanyName"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "AddressLine1"))
            {
                this.AddressLine1 = SqlHelper.DbSafeType<string>(dr["AddressLine1"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "AddressLine2"))
            {
                this.AddressLine2 = SqlHelper.DbSafeType<string>(dr["AddressLine2"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "City"))
            {
                this.City = SqlHelper.DbSafeType<string>(dr["City"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "State"))
            {
                this.State = SqlHelper.DbSafeType<string>(dr["State"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "PostCode"))
            {
                this.PostCode = SqlHelper.DbSafeType<string>(dr["PostCode"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "Country"))
            {
                this.Country = SqlHelper.DbSafeType<string>(dr["Country"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "FirstName"))
            {
                this.FirstName = SqlHelper.DbSafeType<string>(dr["FirstName"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "LastName"))
            {
                this.LastName = SqlHelper.DbSafeType<string>(dr["LastName"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "Phone"))
            {
                this.Phone = SqlHelper.DbSafeType<string>(dr["Phone"], string.Empty);
            }
            if (DataHelper.ColumnExists(ref dr, "Email"))
            {
                this.Email = SqlHelper.DbSafeType<string>(dr["Email"], string.Empty);
            }

            if (DataHelper.ColumnExists(ref dr, "datestamp"))
            {
                this.datestamp = SqlHelper.DbSafeType<DateTime>(dr["datestamp"], DateTime.MinValue);
            }
            if (DataHelper.ColumnExists(ref dr, "DocumentStateName"))
            {
                this.DocumentStateName = SqlHelper.DbSafeType<string>(dr["DocumentStateName"], string.Empty);
            }
            #endregion
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Document()
        {
            // TODO: Complete member initialization
            this.IsDeleted = false;
            this.Phone = "";
            this.TrackingNumber = "0";
            this.Carrier = "";
            this.Attention = "";
            this.CompanyName = "";
            this.AddressLine1 = "";
            this.AddressLine2 = "";
            this.City = "";
            this.State = "";
            this.PostCode = "";
            this.Country = "";
            this.FirstName = "";
            this.LastName = "";
            this.Phone = "";
            this.Email = "";
            this.datestamp = DateTime.Now;

        }
    }
}

