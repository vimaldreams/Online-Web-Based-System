using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;

namespace CollateralCreatorAdminWeb.ViewModels
{
    /// <summary>
    /// Public class view that encapsulates data for a full template creation action
    /// </summary>
    public class CreateTemplateViewModel
    {
        #region Member variables
        
     
        private int createTemplateViewModelId;
        private Template template;
      
        private List<FontType> fontTypes;
        private List<FontUsage> fontUsages;
        private List<Font> fonts;
        private List<string> paperSizes;
        private List<string> letterSize;
        private List<Language> languages;
        private List<KeyValuePair<int, string>> fontColours;
        private List<string> fontNames;
        private PageDetail pageDetail;
        private CustomAreaDetail customAreaDetailSection1;
        private CustomAreaDetail customAreaDetailSection2;
        private CustomAreaDetail customAreaDetailSection3;
        private CustomAreaDetail customAreaDetailSection4;
        private CustomAreaDetail customAreaDetailSection5;
        private CustomAreaDetail customAreaDetailSection6;
        private CustomAreaDetail customAreaDetailSection7;
        private CustomAreaDetail customAreaDetailSection8;
        private CustomAreaDetail customAreaDetailSection9;
        private CustomAreaDetail customAreaDetailSection10;
        private CustomAreaDetail customAreaDetailSection11;
        private CustomAreaDetail customAreaDetailSection12;
        private CustomAreaDetail customAreaDetailSection13;
        private CustomAreaDetail customAreaDetailSection14;
        private CustomAreaDetail customAreaDetailSection15;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreateTemplateViewModel()
        {
            Template template = new Template();
            List<Page> pages = new List<Page>();
            List<CustomizableArea> customizableAreas = new List<CustomizableArea>();
            List<TextArea> textAreas = new List<TextArea>();
            List<ImageArea> imageAreas = new List<ImageArea>();
            List<FontType> fontTypes = new List<FontType>();
            List<FontUsage> fontUsages = new List<FontUsage>();
            List<Font> fonts = new List<Font>();
            List<string> paperSizes = new List<string>();
            List<string> letterSize = new List<string>();
            List<Language> languages = new List<Language>();
            List<KeyValuePair<int, string>> fontColours = new List<KeyValuePair<int, string>>();
            List<string> fontNames = new List<string>();
            pageDetail = new PageDetail();
            customAreaDetailSection1 = new CustomAreaDetail();
            customAreaDetailSection2 = new CustomAreaDetail();
            customAreaDetailSection3 = new CustomAreaDetail();
            customAreaDetailSection4 = new CustomAreaDetail();
            customAreaDetailSection5 = new CustomAreaDetail();
            customAreaDetailSection6 = new CustomAreaDetail();
            customAreaDetailSection7 = new CustomAreaDetail();
            customAreaDetailSection8 = new CustomAreaDetail();
            customAreaDetailSection9 = new CustomAreaDetail();
            customAreaDetailSection10 = new CustomAreaDetail();
            customAreaDetailSection11 = new CustomAreaDetail();
            customAreaDetailSection12 = new CustomAreaDetail();
            customAreaDetailSection13 = new CustomAreaDetail();
            customAreaDetailSection14 = new CustomAreaDetail();
            customAreaDetailSection15 = new CustomAreaDetail();
           
        }

        #endregion

        #region Properties

        public int CreateTemplateViewModelId
        {
            get{return createTemplateViewModelId;}
            set{createTemplateViewModelId=value;}
        }

        public Template Template
        {
            get { return template; }
            set { template = value; }
        }

        public PageDetail DefinedPageDetail
        {
            get { return pageDetail; }
            set { pageDetail = value; }
        }

        
        public List<FontType> FontTypes
        {
            get { return fontTypes; }
            set { fontTypes = value; }
        }

        public List<FontUsage> FontUsages
        {
            get { return fontUsages; }
            set { fontUsages = value; }
        }

        public List<Font> Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }

        public List<string> PaperSizes
        {
            get { return paperSizes; }
            set { paperSizes = value; }

        }

        public List<string> PageSize
        {
            get { return letterSize; }
            set { letterSize = value; }
        }

        public List<Language> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public List<KeyValuePair<int, string>> FontColours
        {
            get { return fontColours; }
            set { fontColours = value; }
        }

        public List<string> FontNames
        {
            get { return fontNames; }
            set { fontNames = value; }
        }

        public CustomAreaDetail CustomAreaDetail1
        {
            get { return customAreaDetailSection1; }
            set { customAreaDetailSection1 = value; }
        }

        public CustomAreaDetail CustomAreaDetail2
        {
            get { return customAreaDetailSection2; }
            set { customAreaDetailSection2 = value; }
        }

        public CustomAreaDetail CustomAreaDetail3
        {
            get { return customAreaDetailSection3; }
            set { customAreaDetailSection3 = value; }
        }

        public CustomAreaDetail CustomAreaDetail4
        {
            get { return customAreaDetailSection4; }
            set { customAreaDetailSection4 = value; }
        }

        public CustomAreaDetail CustomAreaDetail5
        {
            get { return customAreaDetailSection5; }
            set { customAreaDetailSection5 = value; }
        }

        public CustomAreaDetail CustomAreaDetail6
        {
            get { return customAreaDetailSection6; }
            set { customAreaDetailSection6 = value; }
        }

        public CustomAreaDetail CustomAreaDetail7
        {
            get { return customAreaDetailSection7; }
            set { customAreaDetailSection7 = value; }
        }

        public CustomAreaDetail CustomAreaDetail8
        {
            get { return customAreaDetailSection8; }
            set { customAreaDetailSection8 = value; }
        }

        public CustomAreaDetail CustomAreaDetail9
        {
            get { return customAreaDetailSection9; }
            set { customAreaDetailSection9 = value; }
        }

        public CustomAreaDetail CustomAreaDetail10
        {
            get { return customAreaDetailSection10; }
            set { customAreaDetailSection10 = value; }
        }

        public CustomAreaDetail CustomAreaDetail11
        {
            get { return customAreaDetailSection11; }
            set { customAreaDetailSection11 = value; }
        }

        public CustomAreaDetail CustomAreaDetail12
        {
            get { return customAreaDetailSection12; }
            set { customAreaDetailSection12 = value; }
        }

        public CustomAreaDetail CustomAreaDetail13
        {
            get { return customAreaDetailSection13; }
            set { customAreaDetailSection13 = value; }
        }

        public CustomAreaDetail CustomAreaDetail14
        {
            get { return customAreaDetailSection14; }
            set { customAreaDetailSection14 = value; }
        }

        public CustomAreaDetail CustomAreaDetail15
        {
            get { return customAreaDetailSection15; }
            set { customAreaDetailSection15 = value; }
        }

        #endregion


    }
}