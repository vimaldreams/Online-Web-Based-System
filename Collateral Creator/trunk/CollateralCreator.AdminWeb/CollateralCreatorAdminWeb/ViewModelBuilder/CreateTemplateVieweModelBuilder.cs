using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CollateralCreatorAdminWeb.Models;
using CollateralCreatorAdminWeb.Service;
using CollateralCreatorAdminWeb.Extensions;
using CollateralCreatorAdminWeb.ViewModels;

namespace CollateralCreatorAdminWeb.ViewModelBuilder
{
    /// <summary>
    /// Creates a set of data to use with the create view of the template object
    /// </summary>
    public class CreateTemplateVieweModelBuilder
    {
        #region Member variables

        XeroxCCToolEntities db = new XeroxCCToolEntities();

        #endregion

        #region Constructors

        public CreateTemplateVieweModelBuilder()
        {
          
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds a set of reference data for the view
        /// </summary>
        public CreateTemplateViewModel Build()
        {
            #region Code

            CreateTemplateViewModel templateModel = new CreateTemplateViewModel();

            templateModel.Template = new Template();

          
            templateModel.FontTypes = new List<FontType>();
            templateModel.FontUsages = new List<FontUsage>();
            templateModel.Fonts = new List<Font>();
            templateModel.PaperSizes = new List<string>();
            templateModel.PageSize = new List<string>();
            templateModel.Languages = new List<Language>();
            templateModel.FontColours = new List<KeyValuePair<int, string>>();
            templateModel.FontNames = new List<string>();
            templateModel.DefinedPageDetail = new PageDetail();

            templateModel.Template.PaperOption = "100# Elite Gloss Text";
            templateModel.Template.CollorCorrection = "Commercial";
            templateModel.Template.PrintMode = "High-Quality";
            templateModel.Template.RecycledPaper = false;
            templateModel.Template.Duplex = false;
            templateModel.Template.Quantity = 0;
            templateModel.Template.IsDeleted = false;

            templateModel.PaperSizes.Add("A-size (11*8.5)");
            templateModel.PaperSizes.Add("A-size (8.5*11)");
            templateModel.PaperSizes.Add("LETTER-size (8.26*11.69)");

            templateModel.PageSize.Add("a4");
            templateModel.PageSize.Add("a4landscape");
            templateModel.PageSize.Add("letter");
            templateModel.PageSize.Add("letterlandscape");

            //populate languages
            var languages = db.Languages.OrderBy(l => l.LanguageName);
            foreach (Language slang in languages)
            {
                templateModel.Languages.Add(slang);
            }
            

            templateModel.FontColours.Add(new KeyValuePair<int,string>(0, "Black"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(8224125, "Gray"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(14230065, "Red"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(7188285, "Green"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(2659797, "Blue"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(15103488, "Orange"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(3456186, "Turquosie"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(10167683, "Violet"));
            templateModel.FontColours.Add(new KeyValuePair<int,string>(16777215, "White"));

            templateModel.FontNames.Add("XeroxSans");
            templateModel.FontNames.Add("XeroxSans-Light");
            templateModel.FontNames.Add("XeroxSans-Bold");
            templateModel.FontNames.Add("HelveticaNeue");
            templateModel.FontNames.Add("HelveticaNeue-Bold");
            templateModel.FontNames.Add("HelveticaNeue-Light");
            templateModel.FontNames.Add("ArialMT");
            templateModel.FontNames.Add("Arial");
                 
            var fontTypes = db.FontTypes.OrderBy(t => t.Name);
            foreach (FontType fType in fontTypes)
            {
                templateModel.FontTypes.Add(fType);
            }


            templateModel.CustomAreaDetail1 = new CustomAreaDetail();
            templateModel.CustomAreaDetail1.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail1.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail2 = new CustomAreaDetail();
            templateModel.CustomAreaDetail2.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail2.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail3 = new CustomAreaDetail();
            templateModel.CustomAreaDetail3.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail3.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail4 = new CustomAreaDetail();
            templateModel.CustomAreaDetail4.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail4.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail5 = new CustomAreaDetail();
            templateModel.CustomAreaDetail5.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail5.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail6 = new CustomAreaDetail();
            templateModel.CustomAreaDetail6.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail6.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail7 = new CustomAreaDetail();
            templateModel.CustomAreaDetail7.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail7.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail8 = new CustomAreaDetail();
            templateModel.CustomAreaDetail8.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail8.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail9 = new CustomAreaDetail();
            templateModel.CustomAreaDetail9.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail9.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail10 = new CustomAreaDetail();
            templateModel.CustomAreaDetail10.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail10.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail11 = new CustomAreaDetail();
            templateModel.CustomAreaDetail11.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail11.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail12 = new CustomAreaDetail();
            templateModel.CustomAreaDetail12.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail12.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail13 = new CustomAreaDetail();
            templateModel.CustomAreaDetail13.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail13.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail14 = new CustomAreaDetail();
            templateModel.CustomAreaDetail14.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail14.FontNames = templateModel.FontNames;

            templateModel.CustomAreaDetail15 = new CustomAreaDetail();
            templateModel.CustomAreaDetail15.FontColours = templateModel.FontColours;
            templateModel.CustomAreaDetail15.FontNames = templateModel.FontNames;


            return templateModel;
            #endregion
        }

        #endregion
    }
}