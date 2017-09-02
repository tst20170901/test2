using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Alice.WebControls.Mvc
{
    internal class PagerBuilder
    {
        private const string CopyrightText = "";
        private readonly HtmlHelper _html;
        private readonly AjaxHelper _ajax;
        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly int _totalPageCount = 1;
        private readonly int _pageIndex;
        private readonly PagerOptions _pagerOptions;
        private readonly RouteValueDictionary _routeValues;
        private readonly string _routeName;
        private readonly int _startPageIndex = 1;
        private readonly int _endPageIndex = 1;
        private readonly bool _ajaxPagingEnabled;
        private readonly MvcAjaxOptions _ajaxOptions;
        private IDictionary<string, object> _htmlAttributes;
        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagerOptions == null)
            {
                pagerOptions = new PagerOptions();
            }
            this._html = html;
            this._ajax = ajax;
            this._pagerOptions = pagerOptions;
            this._htmlAttributes = htmlAttributes;
        }
        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, string actionName, string controllerName, int totalPageCount, int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, MvcAjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            this._ajaxPagingEnabled = (ajax != null);
            if (pagerOptions == null)
            {
                pagerOptions = new PagerOptions();
            }
            this._html = html;
            this._ajax = ajax;
            this._actionName = actionName;
            this._controllerName = controllerName;
            if (pagerOptions.MaxPageIndex == 0 || pagerOptions.MaxPageIndex > totalPageCount)
            {
                this._totalPageCount = totalPageCount;
            }
            else
            {
                this._totalPageCount = pagerOptions.MaxPageIndex;
            }
            this._pageIndex = pageIndex;
            this._pagerOptions = pagerOptions;
            this._routeName = routeName;
            this._routeValues = routeValues;
            this._ajaxOptions = ajaxOptions;
            this._htmlAttributes = htmlAttributes;
            this._startPageIndex = pageIndex - pagerOptions.NumericPagerItemCount / 2;
            if (this._startPageIndex + pagerOptions.NumericPagerItemCount > this._totalPageCount)
            {
                this._startPageIndex = this._totalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            }
            if (this._startPageIndex < 1)
            {
                this._startPageIndex = 1;
            }
            this._endPageIndex = this._startPageIndex + this._pagerOptions.NumericPagerItemCount - 1;
            if (this._endPageIndex > this._totalPageCount)
            {
                this._endPageIndex = this._totalPageCount;
            }
        }
        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
            : this(helper, null, actionName, controllerName, totalPageCount, pageIndex, pagerOptions, routeName, routeValues, null, htmlAttributes)
        {
        }
        internal PagerBuilder(AjaxHelper helper, string actionName, string controllerName, int totalPageCount, int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, MvcAjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
            : this(null, helper, actionName, controllerName, totalPageCount, pageIndex, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes)
        {
        }
        private void AddPrevious(ICollection<PagerItem> results)
        {
            PagerItem pagerItem = new PagerItem(this._pagerOptions.PrevPageText, this._pageIndex - 1, this._pageIndex == 1, PagerItemType.PrevPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && this._pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(pagerItem);
            }
        }
        private void AddFirst(ICollection<PagerItem> results)
        {
            PagerItem pagerItem = new PagerItem(this._pagerOptions.FirstPageText, 1, this._pageIndex == 1, PagerItemType.FirstPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && this._pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(pagerItem);
            }
        }
        private void AddMoreBefore(ICollection<PagerItem> results)
        {
            if (this._startPageIndex > 1 && this._pagerOptions.ShowMorePagerItems)
            {
                int num = this._startPageIndex - 1;
                if (num < 1)
                {
                    num = 1;
                }
                PagerItem item = new PagerItem(this._pagerOptions.MorePageText, num, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }
        private void AddPageNumbers(ICollection<PagerItem> results)
        {
            for (int i = this._startPageIndex; i <= this._endPageIndex; i++)
            {
                string text = i.ToString(CultureInfo.InvariantCulture);
                if (i == this._pageIndex && !string.IsNullOrEmpty(this._pagerOptions.CurrentPageNumberFormatString))
                {
                    text = string.Format(this._pagerOptions.CurrentPageNumberFormatString, text);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this._pagerOptions.PageNumberFormatString))
                    {
                        text = string.Format(this._pagerOptions.PageNumberFormatString, text);
                    }
                }
                PagerItem item = new PagerItem(text, i, false, PagerItemType.NumericPage);
                results.Add(item);
            }
        }
        private void AddMoreAfter(ICollection<PagerItem> results)
        {
            if (this._endPageIndex < this._totalPageCount)
            {
                int num = this._startPageIndex + this._pagerOptions.NumericPagerItemCount;
                if (num > this._totalPageCount)
                {
                    num = this._totalPageCount;
                }
                PagerItem item = new PagerItem(this._pagerOptions.MorePageText, num, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }
        private void AddNext(ICollection<PagerItem> results)
        {
            PagerItem pagerItem = new PagerItem(this._pagerOptions.NextPageText, this._pageIndex + 1, this._pageIndex >= this._totalPageCount, PagerItemType.NextPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && this._pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(pagerItem);
            }
        }
        private void AddLast(ICollection<PagerItem> results)
        {
            PagerItem pagerItem = new PagerItem(this._pagerOptions.LastPageText, this._totalPageCount, this._pageIndex >= this._totalPageCount, PagerItemType.LastPage);
            if (!pagerItem.Disabled || (pagerItem.Disabled && this._pagerOptions.ShowDisabledPagerItems))
            {
                results.Add(pagerItem);
            }
        }
        private string GenerateUrl(int pageIndex)
        {
            ViewContext viewContext = (this._ajax == null) ? this._html.ViewContext : this._ajax.ViewContext;
            if (pageIndex > this._totalPageCount || pageIndex == this._pageIndex)
            {
                return null;
            }
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(viewContext.RouteData.Values);
            this.AddQueryStringToRouteValues(routeValueDictionary, viewContext);
            if (this._routeValues != null && this._routeValues.Count > 0)
            {
                foreach (KeyValuePair<string, object> current in this._routeValues)
                {
                    if (!routeValueDictionary.ContainsKey(current.Key))
                    {
                        routeValueDictionary.Add(current.Key, current.Value);
                    }
                    else
                    {
                        routeValueDictionary[current.Key] = current.Value;
                    }
                }
            }
            object obj = viewContext.RouteData.Values[this._pagerOptions.PageIndexParameterName];
            string text = this._routeName;
            if (pageIndex == 0)
            {
                routeValueDictionary[this._pagerOptions.PageIndexParameterName] = "__" + this._pagerOptions.PageIndexParameterName + "__";
            }
            else
            {
                if (pageIndex == 1)
                {
                    if (!string.IsNullOrWhiteSpace(this._pagerOptions.FirstPageRouteName))
                    {
                        text = this._pagerOptions.FirstPageRouteName;
                        routeValueDictionary.Remove(this._pagerOptions.PageIndexParameterName);
                        viewContext.RouteData.Values.Remove(this._pagerOptions.PageIndexParameterName);
                    }
                    else
                    {
                        Route route = viewContext.RouteData.Route as Route;
                        if (route != null && (route.Defaults[this._pagerOptions.PageIndexParameterName] == UrlParameter.Optional || !route.Url.Contains("{" + this._pagerOptions.PageIndexParameterName + "}")))
                        {
                            routeValueDictionary.Remove(this._pagerOptions.PageIndexParameterName);
                            viewContext.RouteData.Values.Remove(this._pagerOptions.PageIndexParameterName);
                        }
                        else
                        {
                            routeValueDictionary[this._pagerOptions.PageIndexParameterName] = pageIndex;
                        }
                    }
                }
                else
                {
                    routeValueDictionary[this._pagerOptions.PageIndexParameterName] = pageIndex;
                }
            }
            RouteCollection routeCollection = (this._ajax == null) ? this._html.RouteCollection : this._ajax.RouteCollection;
            string result;
            if (!string.IsNullOrEmpty(text))
            {
                result = UrlHelper.GenerateUrl(text, this._actionName, this._controllerName, routeValueDictionary, routeCollection, viewContext.RequestContext, false);
            }
            else
            {
                result = UrlHelper.GenerateUrl(null, this._actionName, this._controllerName, routeValueDictionary, routeCollection, viewContext.RequestContext, false);
            }
            if (obj != null)
            {
                viewContext.RouteData.Values[this._pagerOptions.PageIndexParameterName] = obj;
            }
            return result;
        }
        internal MvcHtmlString RenderPager()
        {
            if (this._totalPageCount <= 1 && this._pagerOptions.AutoHide)
            {
                return MvcHtmlString.Create("");
            }
            if ((this._pageIndex > this._totalPageCount && this._totalPageCount > 0) || this._pageIndex < 1)
            {
                return MvcHtmlString.Create(string.Format("{0}<div style=\"color:red;font-weight:bold\">{1}</div>{0}", "", this._pagerOptions.PageIndexOutOfRangeErrorMessage));
            }
            List<PagerItem> list = new List<PagerItem>();
            if (this._pagerOptions.ShowFirstLast)
            {
                this.AddFirst(list);
            }
            if (this._pagerOptions.ShowPrevNext)
            {
                this.AddPrevious(list);
            }
            if (this._pagerOptions.ShowNumericPagerItems)
            {
                if (this._pagerOptions.AlwaysShowFirstLastPageNumber && this._startPageIndex > 1)
                {
                    list.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));
                }
                if (this._pagerOptions.ShowMorePagerItems && ((!this._pagerOptions.AlwaysShowFirstLastPageNumber && this._startPageIndex > 1) || (this._pagerOptions.AlwaysShowFirstLastPageNumber && this._startPageIndex > 2)))
                {
                    this.AddMoreBefore(list);
                }
                this.AddPageNumbers(list);
                if (this._pagerOptions.ShowMorePagerItems && ((!this._pagerOptions.AlwaysShowFirstLastPageNumber && this._endPageIndex < this._totalPageCount) || (this._pagerOptions.AlwaysShowFirstLastPageNumber && this._totalPageCount > this._endPageIndex + 1)))
                {
                    this.AddMoreAfter(list);
                }
                if (this._pagerOptions.AlwaysShowFirstLastPageNumber && this._endPageIndex < this._totalPageCount)
                {
                    list.Add(new PagerItem(this._totalPageCount.ToString(CultureInfo.InvariantCulture), this._totalPageCount, false, PagerItemType.NumericPage));
                }
            }
            if (this._pagerOptions.ShowPrevNext)
            {
                this.AddNext(list);
            }
            if (this._pagerOptions.ShowFirstLast)
            {
                this.AddLast(list);
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (this._ajaxPagingEnabled)
            {
                using (List<PagerItem>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        PagerItem current = enumerator.Current;
                        stringBuilder.Append(this.GenerateAjaxPagerElement(current));
                    }
                    goto IL_245;
                }
            }
            foreach (PagerItem current2 in list)
            {
                stringBuilder.Append(this.GeneratePagerElement(current2));
            }
        IL_245:
            TagBuilder tagBuilder = new TagBuilder(this._pagerOptions.ContainerTagName);
            if (!string.IsNullOrEmpty(this._pagerOptions.Id))
            {
                tagBuilder.GenerateId(this._pagerOptions.Id);
            }
            if (!string.IsNullOrEmpty(this._pagerOptions.CssClass))
            {
                tagBuilder.AddCssClass(this._pagerOptions.CssClass);
            }
            if (!string.IsNullOrEmpty(this._pagerOptions.HorizontalAlign))
            {
                string text = "text-align:" + this._pagerOptions.HorizontalAlign.ToLower();
                if (this._htmlAttributes == null)
                {
                    this._htmlAttributes = new RouteValueDictionary
					{

						{
							"style",
							text
						}
					};
                }
                else
                {
                    if (this._htmlAttributes.Keys.Contains("style"))
                    {
                        IDictionary<string, object> htmlAttributes;
                        (htmlAttributes = this._htmlAttributes)["style"] = htmlAttributes["style"] + ";" + text;
                    }
                }
            }
            tagBuilder.MergeAttributes<string, object>(this._htmlAttributes, true);
            if (this._ajaxPagingEnabled)
            {
                IDictionary<string, object> dictionary = this._ajaxOptions.ToUnobtrusiveHtmlAttributes();
                dictionary.Remove("data-ajax-url");
                dictionary.Remove("data-ajax-mode");
                if (this._ajaxOptions.EnablePartialLoading)
                {
                    dictionary.Add("data-ajax-partialloading", "true");
                }
                if (this._pageIndex > 1)
                {
                    dictionary.Add("data-ajax-currentpage", this._pageIndex);
                }
                if (!string.IsNullOrWhiteSpace(this._ajaxOptions.DataFormId))
                {
                    dictionary.Add("data-ajax-dataformid", "#" + this._ajaxOptions.DataFormId);
                }
                this.AddDataAttributes(dictionary);
                tagBuilder.MergeAttributes<string, object>(dictionary, true);
            }
            if (this._pagerOptions.ShowPageIndexBox)
            {
                if (!this._ajaxPagingEnabled)
                {
                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                    this.AddDataAttributes(dictionary2);
                    tagBuilder.MergeAttributes<string, object>(dictionary2, true);
                }
                stringBuilder.Append(this.BuildGoToPageSection());
            }
            else
            {
                stringBuilder.Length -= this._pagerOptions.PagerItemsSeperator.Length;
            }
            tagBuilder.InnerHtml = stringBuilder.ToString();
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
        private void AddDataAttributes(IDictionary<string, object> attrs)
        {
            attrs.Add("data-urlformat", this.GenerateUrl(0));
            attrs.Add("data-mvcpager", "true");
            if (this._pageIndex > 1)
            {
                attrs.Add("data-firstpage", this.GenerateUrl(1));
            }
            attrs.Add("data-pageparameter", this._pagerOptions.PageIndexParameterName);
            attrs.Add("data-maxpages", this._totalPageCount);
            if (this._pagerOptions.ShowPageIndexBox && this._pagerOptions.PageIndexBoxType == PageIndexBoxType.TextBox)
            {
                attrs.Add("data-outrangeerrmsg", this._pagerOptions.PageIndexOutOfRangeErrorMessage);
                attrs.Add("data-invalidpageerrmsg", this._pagerOptions.InvalidPageIndexErrorMessage);
            }
        }
        private string BuildGoToPageSection()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this._pagerOptions.PageIndexBoxType == PageIndexBoxType.DropDownList)
            {
                int num = this._pageIndex - this._pagerOptions.MaximumPageIndexItems / 2;
                if (num + this._pagerOptions.MaximumPageIndexItems > this._totalPageCount)
                {
                    num = this._totalPageCount + 1 - this._pagerOptions.MaximumPageIndexItems;
                }
                if (num < 1)
                {
                    num = 1;
                }
                int num2 = num + this._pagerOptions.MaximumPageIndexItems - 1;
                if (num2 > this._totalPageCount)
                {
                    num2 = this._totalPageCount;
                }
                stringBuilder.AppendFormat("<select data-pageindexbox=\"true\"{0}>", this._pagerOptions.ShowGoButton ? "" : " data-autosubmit=\"true\"");
                for (int i = num; i <= num2; i++)
                {
                    stringBuilder.AppendFormat("<option value=\"{0}\"", i);
                    if (i == this._pageIndex)
                    {
                        stringBuilder.Append(" selected=\"selected\"");
                    }
                    stringBuilder.AppendFormat(">{0}</option>", i);
                }
                stringBuilder.Append("</select>");
            }
            else
            {
                stringBuilder.AppendFormat("<input type=\"text\" value=\"{0}\" data-pageindexbox=\"true\"{1}/>", this._pageIndex, this._pagerOptions.ShowGoButton ? "" : " data-autosubmit=\"true\"");
            }
            string text;
            if (!string.IsNullOrEmpty(this._pagerOptions.PageIndexBoxWrapperFormatString))
            {
                text = string.Format(this._pagerOptions.PageIndexBoxWrapperFormatString, stringBuilder);
                stringBuilder = new StringBuilder(text);
            }
            if (this._pagerOptions.ShowGoButton)
            {
                stringBuilder.AppendFormat("<input type=\"button\" data-submitbutton=\"true\" value=\"{0}\"/>", this._pagerOptions.GoButtonText);
            }
            if (!string.IsNullOrEmpty(this._pagerOptions.GoToPageSectionWrapperFormatString) || !string.IsNullOrEmpty(this._pagerOptions.PagerItemWrapperFormatString))
            {
                text = string.Format(this._pagerOptions.GoToPageSectionWrapperFormatString ?? this._pagerOptions.PagerItemWrapperFormatString, stringBuilder);
            }
            else
            {
                text = stringBuilder.ToString();
            }
            return text;
        }
        private string GenerateAjaxAnchor(PagerItem item)
        {
            string value = this.GenerateUrl(item.PageIndex);
            if (string.IsNullOrWhiteSpace(value))
            {
                return HttpUtility.HtmlEncode(item.Text);
            }
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = item.Text
            };
            tagBuilder.MergeAttribute("href", value);
            tagBuilder.MergeAttribute("data-pageindex", item.PageIndex.ToString(CultureInfo.InvariantCulture));
            return tagBuilder.ToString(TagRenderMode.Normal);
        }
        private MvcHtmlString GeneratePagerElement(PagerItem item)
        {
            string text = this.GenerateUrl(item.PageIndex);
            if (item.Disabled)
            {
                return this.CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            }
            return this.CreateWrappedPagerElement(item, string.IsNullOrEmpty(text) ? HttpUtility.HtmlEncode(item.Text) : string.Format("<a href=\"{0}\">{1}</a>", text, item.Text));
        }
        private MvcHtmlString GenerateAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
            {
                return this.CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            }
            return this.CreateWrappedPagerElement(item, this.GenerateAjaxAnchor(item));
        }
        private MvcHtmlString CreateWrappedPagerElement(PagerItem item, string el)
        {
            string str = el;
            switch (item.Type)
            {
                case PagerItemType.FirstPage:
                case PagerItemType.NextPage:
                case PagerItemType.PrevPage:
                case PagerItemType.LastPage:
                    if (!string.IsNullOrEmpty(this._pagerOptions.NavigationPagerItemWrapperFormatString) || !string.IsNullOrEmpty(this._pagerOptions.PagerItemWrapperFormatString))
                    {
                        str = string.Format(this._pagerOptions.NavigationPagerItemWrapperFormatString ?? this._pagerOptions.PagerItemWrapperFormatString, el);
                    }
                    break;
                case PagerItemType.MorePage:
                    if (!string.IsNullOrEmpty(this._pagerOptions.MorePagerItemWrapperFormatString) || !string.IsNullOrEmpty(this._pagerOptions.PagerItemWrapperFormatString))
                    {
                        str = string.Format(this._pagerOptions.MorePagerItemWrapperFormatString ?? this._pagerOptions.PagerItemWrapperFormatString, el);
                    }
                    break;
                case PagerItemType.NumericPage:
                    if (item.PageIndex == this._pageIndex && (!string.IsNullOrEmpty(this._pagerOptions.CurrentPagerItemWrapperFormatString) || !string.IsNullOrEmpty(this._pagerOptions.PagerItemWrapperFormatString)))
                    {
                        str = string.Format(this._pagerOptions.CurrentPagerItemWrapperFormatString ?? this._pagerOptions.PagerItemWrapperFormatString, el);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this._pagerOptions.NumericPagerItemWrapperFormatString) || !string.IsNullOrEmpty(this._pagerOptions.PagerItemWrapperFormatString))
                        {
                            str = string.Format(this._pagerOptions.NumericPagerItemWrapperFormatString ?? this._pagerOptions.PagerItemWrapperFormatString, el);
                        }
                    }
                    break;
            }
            return MvcHtmlString.Create(str + this._pagerOptions.PagerItemsSeperator);
        }
        private void AddQueryStringToRouteValues(RouteValueDictionary routeValues, ViewContext viewContext)
        {
            if (routeValues == null)
            {
                routeValues = new RouteValueDictionary();
            }
            NameValueCollection queryString = viewContext.HttpContext.Request.QueryString;
            if (queryString != null && queryString.Count > 0)
            {
                string[] array = new string[]
				{
					"x-requested-with",
					"xmlhttprequest",
					this._pagerOptions.PageIndexParameterName.ToLower()
				};
                foreach (string text in queryString.Keys)
                {
                    if (!string.IsNullOrEmpty(text) && Array.IndexOf<string>(array, text.ToLower()) < 0)
                    {
                        routeValues[text] = queryString[text];
                    }
                }
            }
        }
    }
}
