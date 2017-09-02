using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Units
{
    public abstract class BigUserPageBase<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public UserWorkContext WorkContext;

        public sealed override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);
            Html.EnableUnobtrusiveJavaScript(true);
            BigUserBaseController bc = this.ViewContext.Controller as BigUserBaseController;
            if (bc != null) WorkContext = bc.WorkContext;
        }
        public sealed override void Write(object value)
        {
            Output.Write(value);
        }
    }
    public abstract class BigUserPageBase : BigUserPageBase<dynamic> { }
}