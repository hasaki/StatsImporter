﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using StatsImporter.Importers;

namespace StatsImporter
{
	public static class Program
	{
		public static int Main(string[] args)
		{
			if (args.Length < 3)
			{
				WriteHelp();
				return -1;
			}

			var sportText = args[0]; // NBA (for now)
			var config = args[1]; // Last10, Last15, All, or Playoffs
			var season = args[2]; // 2014-15, 2015-16, 2016-17, etc..

			AllowedSports sport;
			if (!Enum.TryParse(sportText, true, out sport))
			{
				Console.WriteLine($"Invalid sport name '{sportText}'");
				WriteHelp();
				return -1;
			}

			var import = ImporterFactory.GetImporterForSport(sport);
			if (!import.SetConfiguration(config, season))
			{
				Console.WriteLine($"Invalid configuration '{config}'");
				WriteHelp();
				return -1;
			}

			var dirPath = GetOutputDirPath();
			Directory.CreateDirectory(dirPath);
			var filePath = Path.Combine(dirPath, "data.tsv");

			try
			{
				var data = import.Import() ?? GetErrorMessage("Data returned from website in unexpected format");
				var text = TsvEncoder.Encode(data);
				File.WriteAllText(filePath, text);
			}
			catch (Exception e)
			{
				File.WriteAllText(filePath, e.ToString());
				return 1;
			}

			return 0;
		}

		private static IList<Dictionary<string, object>> GetErrorMessage(string errMsg)
		{
			return new List<Dictionary<string, object>> { new Dictionary<string, object> { { "ErrorMessage", errMsg } } };
		}

		private static void WriteHelp()
		{
			var text = @"
StatsImporter

Command line usage:

StatsImporter <sport> <configName> <season>

<sport>: the sport to import statistics for, such as NBA
<configName>: 'pre', 'last10', 'last15', or 'post'
<season>: the season string for the given sport, nba for example uses '2014-15' as its season name";

			Console.WriteLine(text);
		}

		private static string GetOutputDirPath()
		{
			var settings = ConfigurationManager.AppSettings;

			var baseFolder = settings["baseSaveLocation"];
			Environment.SpecialFolder folder;
			
			if(!Enum.TryParse(baseFolder, true, out folder))
				folder = Environment.SpecialFolder.LocalApplicationData;

			baseFolder = Environment.GetFolderPath(folder);

			return Path.Combine(baseFolder, settings["saveFolder"]);
		}
	}
}
