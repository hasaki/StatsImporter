using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StatsImporter.Importers
{
	public class NbaImporter : Importer
	{
		public enum NbaGames
		{
			Last10,
			Last15,
			All,
			Playoffs
		}

		private NbaGames _gamesPlayed;

		public override bool SetConfiguration(string config)
		{
			if (string.IsNullOrWhiteSpace(config))
				return false;

			int val;
			if (int.TryParse(config, out val))
				return false; // Don't allow plain numbers to mascarade as enum values

			return Enum.TryParse(config, true, out _gamesPlayed);
		}

		public override IList<Dictionary<string, object>> Import()
		{
			var url = GetUrl();

			var jsonString = GetFileFromWeb(url);

			var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

			var resultSets = jsonDictionary["resultSets"] as JArray;
			var resultSet = resultSets[0] as JObject;

			var headers = resultSet["headers"] as JArray;
			var rows = resultSet["rowSet"] as JArray;

			var result = new List<Dictionary<string, object>>();

			var row = new Dictionary<string, object>();
			foreach (var header in headers)
			{
				row[header.ToString()] = header.ToString();
			}
			result.Add(row);

			foreach (JArray jRow in rows)
			{
				row = new Dictionary<string, object>();
				var index = 0;
				foreach (var header in headers)
				{
					row[header.ToString()] = jRow[index];
					index++;
				}

				result.Add(row);
			}

			return result;
		}

		private string GetUrl()
		{
			var builder = new NbaUrlBuilder();

			switch (_gamesPlayed)
			{
				case NbaGames.Last10:
					builder.LastNGames = "10";
					break;
				case NbaGames.Last15:
					builder.LastNGames = "15";
					break;
				case NbaGames.All:
					builder.LastNGames = "0";
					break;
				case NbaGames.Playoffs:
					builder.LastNGames = "0";
					builder.SeasonType = "Playoffs";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return builder.GetUrl();
		}
	}
}
