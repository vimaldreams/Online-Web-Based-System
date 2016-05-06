using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;

namespace CollateralCreator.Business
{
    public class DocumentManager
    {
        public static Document GetDocument(int iDocumentID)
        {
            return DocumentProvider.GetDocument(iDocumentID);
        }
        
        public static Document GetDocumentFromTemplate(int iTemplateID, bool bPartnerBranded)
        {
            return DocumentProvider.GetDocumentFromTemplate(iTemplateID, bPartnerBranded);
        }

        public static Document CreateDocument(int iTemplateID, bool bPartnerBranded, string sLanguage, long iChannelPartnerID, string sChannelPartnerLoginID, string sChannelPartnerEmail)
        {
            return DocumentProvider.CreateDocument(iTemplateID, bPartnerBranded, sLanguage, iChannelPartnerID, sChannelPartnerLoginID, sChannelPartnerEmail);
        }

        public static Document CreateEktronDocument(int iTemplateID, bool bPartnerBranded, string sLanguageCode, long iChannelPartnerID, string sChannelPartnerLoginID, string sChannelPartnerEmail)
        {
            return DocumentProvider.CreateEktronDocument(iTemplateID, bPartnerBranded, sLanguageCode, iChannelPartnerID, sChannelPartnerLoginID, sChannelPartnerEmail);
        }
    }
}
