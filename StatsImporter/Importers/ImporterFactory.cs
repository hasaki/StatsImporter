using System;

namespace StatsImporter.Importers
{
	public static class ImporterFactory
	{
		public static Importer GetImporterForSport(AllowedSports sport)
		{
			switch (sport)
			{
				case AllowedSports.Nba:
					return new NbaImporter();

				default:
					throw new ArgumentOutOfRangeException(nameof(sport), sport, null);
			}
		}
	}
}
