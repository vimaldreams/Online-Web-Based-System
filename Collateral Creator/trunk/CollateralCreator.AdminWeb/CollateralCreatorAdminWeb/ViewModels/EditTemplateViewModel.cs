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
    /// Public class view that encapsulates data for a full template edit action
    /// </summary>
    public class EditTemplateViewModel
    {
        #region Member variables


        private int createTemplateViewModelId;
        private Template template;
        private List<Page> pages;
        private List<CustomizableArea> customizableAreas;
        private List<TextArea> textAreas;
        private List<ImageArea> imageAreas;
        private List<FontType> fontTypes;
        private List<FontUsage> fontUsages;
        private List<Font> fonts;

        #endregion

         #region Constructors

        public EditTemplateViewModel()
        {
            Template template = new Template();
            List<Page> pages = new List<Page>();
            List<CustomizableArea> customizableAreas = new List<CustomizableArea>();
            List<TextArea> textAreas = new List<TextArea>();
            List<ImageArea> imageAreas = new List<ImageArea>();
            List<FontType> fontTypes = new List<FontType>();
            List<FontUsage> fontUsages = new List<FontUsage>();
            List<Font> fonts = new List<Font>();
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


        public List<Page> Pages
        {
            get { return pages; }
            set { pages = value; }
        }

        public List<CustomizableArea> CustomizableAreas
        {
            get { return customizableAreas; }
            set { customizableAreas = value; }
        }

        public List<TextArea> TextAreas
        {
            get { return textAreas; }
            set { textAreas = value; }
        }

        public List<ImageArea> ImageAreas
        {
            get { return imageAreas; }
            set { imageAreas = value; }
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

        #endregion
    }
}