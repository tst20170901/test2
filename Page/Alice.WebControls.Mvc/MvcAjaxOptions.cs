using System;
using System.Web.Mvc.Ajax;
namespace Alice.WebControls.Mvc
{
	public class MvcAjaxOptions : AjaxOptions
	{
		public bool EnablePartialLoading
		{
			get;
			set;
		}
		public string DataFormId
		{
			get;
			set;
		}
	}
}
