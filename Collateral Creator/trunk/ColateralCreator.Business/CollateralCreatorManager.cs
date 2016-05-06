using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CollateralCreator.Data;
using CollateralCreator.SQLProvider;

namespace CollateralCreator.Business
{
    public class CollateralCreatorManager
    {
        //public static List<MenuTree> GetMenuTree(string language)
        //{
        //    return CollateralCreatorRepository.GetMenuTree(language);
        //}

        public static List<Template> GetCustomTemplates_MenuTreeNode(int nodeID, string language)
        {
            return CollateralCreatorRepository.GetCustomTemplates_MenuTreeNode(nodeID, language);
        }

        public static Document GetDocument(int iDocumentID)
        {
            return CollateralCreatorRepository.GetDocumentByID(iDocumentID);
        }

        public static List<Document> GetAllDocuments(long lChannelPartnerID, string sCountry)
        {
            return CollateralCreatorRepository.GetAllDocuments(lChannelPartnerID, sCountry);
        }

        public static List<Document> GetAllPrintHouseHistoryDocuments(long lChannelPartnerID, string sCountry)
        {
            return CollateralCreatorRepository.GetAllPrintHouseHistoryDocuments(lChannelPartnerID, sCountry);
        }

        public static List<Document> GetDocumentPrintQueue()
        {
            return CollateralCreatorRepository.GetDocumentPrintQueue();
        }

        public static List<Document> GetDocumentArchiveHistory()
        {
            return CollateralCreatorRepository.GetDocumentArchiveHistory();
        }

        public static List<MenuTree> GetMenuTreeProductCodes()
        {
            return CollateralCreatorRepository.GetMenuTreeProductCodes();
        }

        public static List<MenuTree> GetMenuTreeData(string language)
        {
            return CollateralCreatorRepository.GetMenuTreeData(language);
        }
    }
}
