using System;
namespace Alice.WebControls.Mvc
{
	internal class PagerItem
	{
		internal string Text
		{
			get;
			set;
		}
		internal int PageIndex
		{
			get;
			set;
		}
		internal bool Disabled
		{
			get;
			set;
		}
		internal PagerItemType Type
		{
			get;
			set;
		}
		public PagerItem(string text, int pageIndex, bool disabled, PagerItemType type)
		{
			this.Text = text;
			this.PageIndex = pageIndex;
			this.Disabled = disabled;
			this.Type = type;
		}
	}
}
