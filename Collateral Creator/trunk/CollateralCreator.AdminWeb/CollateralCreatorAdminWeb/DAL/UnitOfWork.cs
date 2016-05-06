/***********************************************************************************************************/
/* unit of work - one way to ensure all the repositories use the same database context                     */
/***********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CollateralCreatorAdminWeb.Models;

namespace CollateralCreatorAdminWeb.DAL
{
    public class UnitOfWork : IDisposable
    {
        private XeroxCCToolEntities context = new XeroxCCToolEntities();

        private GenericRepository<Product_MenuTree> productMenuTreeRepository;
        private GenericRepository<MenuTree> menuTreeRepository;
        private GenericRepository<MenuTreeRecursive> menuTreeRecursiveRepository;
        private GenericRepository<Template> templateRepository;
        private GenericRepository<ExternalCollateral> externalCollateralRepository;
        private GenericRepository<TemplatePartnerBranded> templateBrandRepository;
        private GenericRepository<TemplateButton> templateButtonRepository;     
        private GenericRepository<Page> pageRepository;
        private GenericRepository<CustomizableArea> customAreaRepository;
        private GenericRepository<TextArea> textAreaRepository;
        private GenericRepository<ImageArea> imageAreaRepository;
        private GenericRepository<Font> fontRepository;
        private GenericRepository<FontUsage> fontUsageRepository;
                
        public GenericRepository<Product_MenuTree> ProductMenuTreeRepository
        {
            get
            {
                if (this.productMenuTreeRepository == null)
                {
                    this.productMenuTreeRepository = new GenericRepository<Product_MenuTree>(context);
                }
                return productMenuTreeRepository;
            }
        }

        public GenericRepository<MenuTree> MenuTreeRepository
        {
            get
            {
                if (this.menuTreeRepository == null)
                {
                    this.menuTreeRepository = new GenericRepository<MenuTree>(context);
                }
                return menuTreeRepository;
            }
        }

        public GenericRepository<MenuTreeRecursive> MenuTreeRecursiveRepository
        {
            get
            {
                if (this.menuTreeRecursiveRepository == null)
                {
                    this.menuTreeRecursiveRepository = new GenericRepository<MenuTreeRecursive>(context);
                }
                return menuTreeRecursiveRepository;
            }
        }

        public GenericRepository<Template> TemplateRepository
        {
            get
            {
                if (this.templateRepository == null)
                {
                    this.templateRepository = new GenericRepository<Template>(context);
                }
                return templateRepository;
            }
        }

        public GenericRepository<ExternalCollateral> ExternalCollateralRepository
        {
            get
            {
                if (this.externalCollateralRepository == null)
                {
                    this.externalCollateralRepository = new GenericRepository<ExternalCollateral>(context);
                }
                return externalCollateralRepository;
            }
        }        

        public GenericRepository<TemplatePartnerBranded> TemplateBrandRepository
        {
            get
            {
                if (this.templateBrandRepository == null)
                {
                    this.templateBrandRepository = new GenericRepository<TemplatePartnerBranded>(context);
                }
                return templateBrandRepository;
            }
        }

        public GenericRepository<TemplateButton> TemplateButtonRepository
        {
            get
            {
                if (this.templateButtonRepository == null)
                {
                    this.templateButtonRepository = new GenericRepository<TemplateButton>(context);
                }
                return templateButtonRepository;
            }
        }
                        
        public GenericRepository<Page> PageRepository
        {
            get
            {
                if (this.pageRepository == null)
                {
                    this.pageRepository = new GenericRepository<Page>(context);
                }
                return pageRepository;
            }
        }

        public GenericRepository<CustomizableArea> CustomAreaRepository
        {
            get
            {
                if (this.customAreaRepository == null)
                {
                    this.customAreaRepository = new GenericRepository<CustomizableArea>(context);
                }
                return customAreaRepository;
            }
        }

        public GenericRepository<TextArea> TextAreaRepository
        {
            get
            {
                if (this.textAreaRepository == null)
                {
                    this.textAreaRepository = new GenericRepository<TextArea>(context);
                }
                return textAreaRepository;
            }
        }

        public GenericRepository<ImageArea> ImageAreaRepository
        {
            get
            {
                if (this.imageAreaRepository == null)
                {
                    this.imageAreaRepository = new GenericRepository<ImageArea>(context);
                }
                return imageAreaRepository;
            }
        }

        public GenericRepository<Font> FontRepository
        {
            get
            {
                if (this.fontRepository == null)
                {
                    this.fontRepository = new GenericRepository<Font>(context);
                }
                return fontRepository;
            }
        }

        public GenericRepository<FontUsage> FontUsageRepository
        {
            get
            {
                if (this.fontUsageRepository == null)
                {
                    this.fontUsageRepository = new GenericRepository<FontUsage>(context);
                }
                return fontUsageRepository;
            }
        }
               
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}