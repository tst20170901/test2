using System;
using System.Web.Mvc;
using System.Web.UI;
namespace Alice.WebControls.Mvc
{
    public static class ScriptResourceExtensions
    {
        public static void RegisterMvcPagerScriptResource(this HtmlHelper html)
        {
            html.ViewContext.Writer.Write("<script type=\"text/javascript\" src=\"/Scripts/MvcPager.min.js\"></script>");
        }
    }
}
