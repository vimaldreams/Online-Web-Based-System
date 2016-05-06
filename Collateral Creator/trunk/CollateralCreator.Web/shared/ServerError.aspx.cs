
namespace CollateralCreator.Web
{
    using System;
    using CollateralCreator.Business;

    public partial class ServerError : XeroxWebPage
    {
        protected override void OnInit(EventArgs e)
        {
            if (Form != null)
            {
                //Form.Action = "/" + ThemeName + "/" + EventName + "/feedback/" + this.FeedbackGuid;
            }

            // needed to grab event website details in base event
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindControls();
            }
        }

        /// <summary>
        /// Binds data to page controls
        /// </summary>
        private void DataBindControls()
        {
            SetThemeGraphics();
        }

        /// <summary>
        /// Changes graphics and buttons to use localised versions
        /// using the language of the user.
        /// </summary>
        private void SetThemeGraphics()
        {
           // var sLangCode = SiteManager.GetLanguageCodeForLanguage(DisplayLanguageId);      
        }
    }
}
