using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class PageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public AdminWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            AreaBaseController bc = this.ViewContext.Controller as AreaBaseController;
            if (bc != null) WorkContext = bc.WorkContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class PageBase : PageBase<dynamic> { }
}