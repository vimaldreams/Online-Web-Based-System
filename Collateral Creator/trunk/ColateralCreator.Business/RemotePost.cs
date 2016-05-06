using System.Collections.Specialized;
using System.Web;

namespace CollateralCreator.Business
{
    /// <summary>
    /// class dynamically generates a form for POSTing and redirecting the user
    /// for use within a custom method
    /// </summary>
    /// 
    public class RemotePost
    {
        private readonly NameValueCollection _inputs = new NameValueCollection();
        private string _url = "";
        private string _method = "post";
        private string _formName = "form1";

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }
        public string FormName
        {
            get { return _formName; }
            set { _formName = value; }
        }

        public RemotePost()
        {

        }

        public RemotePost(string targetUrl)
        {
            _url = targetUrl;
        }

        public void Add(string name, string value)
        {
            _inputs.Add(name, value);
        }

        public void Post()
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("<html><head></head>");
            HttpContext.Current.Response.Write(string.Format("<body onload=\"document.{0}.submit()\">", FormName));
            HttpContext.Current.Response.Write(string.Format("<form id=\"{0}\" name=\"{0}\" action=\"{1}\" method=\"{2}\" >", FormName, Url, Method));

            for (var i = 0; i < _inputs.Keys.Count; i++)
            {
                HttpContext.Current.Response.Write(string.Format("<INPUT type=\"hidden\" name=\"{0}\" value=\"{1}\">", _inputs.Keys[i], _inputs[_inputs.Keys[i]]));
            }

            HttpContext.Current.Response.Write("<INPUT TYPE=\"submit\" VALUE=\"submit\" style=\"display:none;\"></form></body></html>");
            HttpContext.Current.Response.End();

            //var p = (Page)HttpContext.Current.CurrentHandler;
        }

    }
}
