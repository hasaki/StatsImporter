using System.Collections.Generic;
using System.Net;

namespace StatsImporter.Importers
{
	public abstract class Importer
	{
		protected Importer() { }

		public abstract bool SetConfiguration(string config);

		public abstract IList<Dictionary<string, object>> Import();

		protected string GetFileFromWeb(string url)
		{
			using (var client = new WebClient())
				return client.DownloadString(url);
		}
	}
}
