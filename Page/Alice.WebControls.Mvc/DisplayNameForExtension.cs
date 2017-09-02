using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
namespace Alice.WebControls.Mvc
{
	public static class DisplayNameForExtension
	{
		public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
		{
			return DisplayNameForExtension.GetDisplayName<TModel, TValue>(expression);
		}
		public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<PagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
		{
			return DisplayNameForExtension.GetDisplayName<TModel, TValue>(expression);
		}
		private static MvcHtmlString GetDisplayName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
		{
			ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, new ViewDataDictionary<TModel>());
			string expressionText = ExpressionHelper.GetExpressionText(expression);
			string arg_3F_0;
			if ((arg_3F_0 = modelMetadata.DisplayName) == null && (arg_3F_0 = modelMetadata.PropertyName) == null)
			{
				arg_3F_0 = expressionText.Split(new char[]
				{
					'.'
				}).Last<string>();
			}
			string s = arg_3F_0;
			return new MvcHtmlString(HttpUtility.HtmlEncode(s));
		}
	}
}
