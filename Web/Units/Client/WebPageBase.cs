using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class WebPageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public UserWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            UserBaseController bc = this.ViewContext.Controller as UserBaseController;
            if (bc != null) WorkContext = bc.WorkContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class WebPageBase : PageBase<dynamic> { }
}