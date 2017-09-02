using System;
using System.Collections.Generic;
using System.Linq;
namespace Alice.WebControls.Mvc
{
	public static class PageLinqExtensions
	{
		public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
		{
			if (pageIndex < 1)
			{
				pageIndex = 1;
			}
			int num = (pageIndex - 1) * pageSize;
			int num2 = allItems.Count<T>();
			while (num2 <= num && pageIndex > 1)
			{
				num = (--pageIndex - 1) * pageSize;
			}
			IQueryable<T> currentPageItems = allItems.Skip(num).Take(pageSize);
			return new PagedList<T>(currentPageItems, pageIndex, pageSize, num2);
		}
		public static PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
		{
			return allItems.AsQueryable<T>().ToPagedList(pageIndex, pageSize);
		}
	}
}
