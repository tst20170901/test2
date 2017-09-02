using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class WebAdminPageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public WebAdminContext WebAdminContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            WebAdminAreaBaseController ww = this.ViewContext.Controller as WebAdminAreaBaseController;
            if (ww != null) WebAdminContext = ww.WebAdminContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class WebAdminPageBase : WebAdminPageBase<dynamic> { }
}