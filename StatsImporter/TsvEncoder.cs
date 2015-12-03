using System.Collections.Generic;
using System.Text;

namespace StatsImporter
{
	public class TsvEncoder
	{
		public static string Encode(IEnumerable<Dictionary<string, object>> data)
		{
			var sb = new StringBuilder();

			var firstRow = true;
			foreach (var row in data)
			{
				if (!firstRow)
					sb.AppendLine();

				firstRow = false;

				var firstItem = true;
				foreach (var kvp in row)
				{
					if (!firstItem)
						sb.Append("\t");

					sb.Append($"{kvp.Value}");

					firstItem = false;
				}
			}

			return sb.ToString();
		}
	}
}
