using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using StatsImporter.Importers;

namespace StatsImporter
{
	public enum AllowedSports
	{
		Nba
	}

	public class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 2)
			{
				WriteHelp();
				return -1;
			}

			AllowedSports sport;

			var sportText = args[0];
			if (!Enum.TryParse(sportText, true, out sport))
			{
				Console.WriteLine($"Invalid sport name '{sportText}'");
				WriteHelp();
				return -1;
			}

			var config = args[1];

			var import = ImporterFactory.GetImporterForSport(sport);
			if (!import.SetConfiguration(config))
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

				var encoder = new TsvEncoder();
				var outputText = encoder.Encode(data);

				File.WriteAllText(filePath, outputText);
			}
			catch (Exception e)
			{
				File.WriteAllText(filePath, e.ToString());
				return 1;
			}

			return 0;
		}

		static IList<Dictionary<string, object>> GetErrorMessage(string errMsg)
		{
			return new List<Dictionary<string, object>> { new Dictionary<string, object> { { "ErrorMessage", errMsg } } };
		} 

		static void WriteHelp()
		{
			var text = @"
StatsImporter

Command line usage:

StatsImporter <sport> <configName>

-<sport>: the sport to import statistics for, such as NBA
-<configName>: 'pre', 'last10', 'last15', or 'post'";

			Console.WriteLine(text);
		}

		static string GetOutputDirPath()
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
