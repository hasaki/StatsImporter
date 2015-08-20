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

		protected string BaseUrl { get; set; }
		protected Dictionary<string, string> Filters { get; private set; }

		protected string GetQueryStringFromFilters()
		{
			return string.Join("&", Filters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		}

		public string GetUrl()
		{
			return BaseUrl + "?" + GetQueryStringFromFilters();
		}
	}

	public class NbaUrlBuilder : UrlBuilder
	{
		public NbaUrlBuilder()
		{
			BaseUrl = "http://stats.nba.com/stats/leaguedashteamstats";

			Filters.Add("Conference", "");
			Filters.Add("DateFrom", "");
			Filters.Add("DateTo", "");
			Filters.Add("Division", "");
			Filters.Add("GameScope", "");
			Filters.Add("GameSegment", "");
			Filters.Add("LastNGames", "0");
			Filters.Add("LeagueID", "00");
			Filters.Add("Location", "");
			Filters.Add("MeasureType", "Base");
			Filters.Add("Month", "0");
			Filters.Add("OpponentTeamID", "0");
			Filters.Add("Outcome", "");
			Filters.Add("PORound", "0");
			Filters.Add("PaceAdjust", "N");
			Filters.Add("PerMode", "PerGame");
			Filters.Add("Period", "0");
			Filters.Add("PlayerExperience", "");
			Filters.Add("PlayerPosition", "");
			Filters.Add("PlusMinus", "N");
			Filters.Add("Rank", "N");
			Filters.Add("Season", "2014-15");
			Filters.Add("SeasonSegment", "");
			Filters.Add("SeasonType", "Regular+Season");
			Filters.Add("ShotClockRange", "");
			Filters.Add("StarterBench", "");
			Filters.Add("TeamID", "0");
			Filters.Add("VsConference", "");
			Filters.Add("VsDivision", "");
		}

		public string LastNGames
		{
			get { return Filters["LastNGames"]; }
			set { Filters["LastNGames"] = value; }
		}

		public string SeasonType
		{
			get { return Filters["SeasonType"]; }
			set { Filters["SeasonType"] = value; }
		}

		public string Season
		{
			get { return Filters["Season"]; }
			set { Filters["Season"] = value; }
		}
	}
}
