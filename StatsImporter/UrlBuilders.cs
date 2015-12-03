using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsImporter
{
	public abstract class UrlBuilder
	{
		protected UrlBuilder()
		{
			Filters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		}

		public string GetUrl()
		{
			return BaseUrl + "?" + GetQueryStringFromFilters();
		}

		protected string BaseUrl { get; set; }

		protected Dictionary<string, string> Filters { get; private set; }

		private string GetQueryStringFromFilters()
		{
			return string.Join("&", Filters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		}
	}
}
