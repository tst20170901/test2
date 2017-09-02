using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class WebWorkerPageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public WebWorkerContext WebWorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            WebWorkerAreaBaseController ww = this.ViewContext.Controller as WebWorkerAreaBaseController;
            if (ww != null) WebWorkContext = ww.WebWorkContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class WebWorkerPageBase : WebWorkerPageBase<dynamic> { }
}