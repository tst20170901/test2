using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class BusinessPageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public BusinessWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            BusinessBaseController bc = this.ViewContext.Controller as BusinessBaseController;
            if (bc != null) WorkContext = bc.WorkContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class BusinessPageBase : BusinessPageBase<dynamic> { }
}