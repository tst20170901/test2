using System;
namespace Alice.WebControls.Mvc
{
	public class PagerOptions
	{
		private string _containerTagName;
		public string FirstPageRouteName
		{
			get;
			set;
		}
		public bool AutoHide
		{
			get;
			set;
		}
		public string PageIndexOutOfRangeErrorMessage
		{
			get;
			set;
		}
		public string InvalidPageIndexErrorMessage
		{
			get;
			set;
		}
		public string PageIndexParameterName
		{
			get;
			set;
		}
		public bool ShowPageIndexBox
		{
			get;
			set;
		}
		public PageIndexBoxType PageIndexBoxType
		{
			get;
			set;
		}
		public int MaximumPageIndexItems
		{
			get;
			set;
		}
		public bool ShowGoButton
		{
			get;
			set;
		}
		public string GoButtonText
		{
			get;
			set;
		}
		public string PageNumberFormatString
		{
			get;
			set;
		}
		public string CurrentPageNumberFormatString
		{
			get;
			set;
		}
		public string ContainerTagName
		{
			get
			{
				return this._containerTagName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException("ContainerTagName不能为null或空字符串", "ContainerTagName");
				}
				this._containerTagName = value;
			}
		}
		public string PagerItemWrapperFormatString
		{
			get;
			set;
		}
		public string NumericPagerItemWrapperFormatString
		{
			get;
			set;
		}
		public string CurrentPagerItemWrapperFormatString
		{
			get;
			set;
		}
		public string NavigationPagerItemWrapperFormatString
		{
			get;
			set;
		}
		public string MorePagerItemWrapperFormatString
		{
			get;
			set;
		}
		public string PageIndexBoxWrapperFormatString
		{
			get;
			set;
		}
		public string GoToPageSectionWrapperFormatString
		{
			get;
			set;
		}
		public bool AlwaysShowFirstLastPageNumber
		{
			get;
			set;
		}
		public int NumericPagerItemCount
		{
			get;
			set;
		}
		public bool ShowPrevNext
		{
			get;
			set;
		}
		public string PrevPageText
		{
			get;
			set;
		}
		public string NextPageText
		{
			get;
			set;
		}
		public bool ShowNumericPagerItems
		{
			get;
			set;
		}
		public bool ShowFirstLast
		{
			get;
			set;
		}
		public string FirstPageText
		{
			get;
			set;
		}
		public string LastPageText
		{
			get;
			set;
		}
		public bool ShowMorePagerItems
		{
			get;
			set;
		}
		public string MorePageText
		{
			get;
			set;
		}
		public string Id
		{
			get;
			set;
		}
		public string HorizontalAlign
		{
			get;
			set;
		}
		public string CssClass
		{
			get;
			set;
		}
		public bool ShowDisabledPagerItems
		{
			get;
			set;
		}
		public string PagerItemsSeperator
		{
			get;
			set;
		}
		public int MaxPageIndex
		{
			get;
			set;
		}
		public PagerOptions()
		{
			this.AutoHide = true;
			this.PageIndexParameterName = "pageindex";
			this.NumericPagerItemCount = 10;
			this.AlwaysShowFirstLastPageNumber = false;
			this.ShowPrevNext = true;
			this.PrevPageText = "上一页";
			this.NextPageText = "下一页";
			this.ShowNumericPagerItems = true;
			this.ShowFirstLast = true;
			this.FirstPageText = "首页";
			this.LastPageText = "尾页";
			this.ShowMorePagerItems = true;
			this.MorePageText = "...";
			this.ShowDisabledPagerItems = true;
			this.PagerItemsSeperator = "&nbsp;&nbsp;";
			this.ShowPageIndexBox = false;
			this.ShowGoButton = true;
			this.PageIndexBoxType = PageIndexBoxType.TextBox;
			this.MaximumPageIndexItems = 80;
			this.GoButtonText = "跳转";
			this.ContainerTagName = "div";
			this.InvalidPageIndexErrorMessage = "页索引无效";
			this.PageIndexOutOfRangeErrorMessage = "页索引超出范围";
			this.MaxPageIndex = 0;
			this.FirstPageRouteName = null;
		}
	}
}
