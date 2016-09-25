using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.App_Data
{
	public class ReadDataStore : IReadDataStore
	{
		private IList<Product> cache;
		private string[] categories;
		private readonly ConfigurationSettings _settings;
		private readonly IAzureLogger _logger;
		private static readonly string path = AppDomain.CurrentDomain.BaseDirectory;

		public ReadDataStore(ConfigurationSettings settings, IAzureLogger logger)
		{
			_settings = settings;
			_logger = logger;

			Initialize();
		}

		public string[] Categories => categories;
		public IList<Product> Cache => cache;
		public int Seed => Cache.Count;

		public void Initialize()
		{
			string data;

			try
			{
				data = File.ReadAllText(path + _settings.DatastoreFilename);
			}
			catch(Exception ex)
			{
				_logger.LogError(GetType(), "Data store file invalid", ex);
				throw new DataStoreException(DataStoreException.ProblemType.BadFile);
			}

			try
			{
				var json = JObject.Parse(data);

				cache = json
					.SelectToken("$.products")
					.Select(p => new Product
					{
						ProductID = (int) p["ProductID"],
						Category = (string) p["Category"],
						ProductName = (string) p["ProductName"]
					}).ToList();

				categories = json
					.SelectTokens("$.products[*].Category")
					.Select(c => (string)c)
					.ToArray();
			}
			catch (Exception ex)
			{
				_logger.LogError(GetType(), "Data store conent invalid", ex);
				throw new DataStoreException(DataStoreException.ProblemType.BadJson);
			}

			if (categories.Length == 0)
			{
				_logger.LogWarning(GetType(), "Data store has no categories");
				throw new DataStoreException(DataStoreException.ProblemType.NoCategories);
			}
		}
		
		public void Dispose()
		{
			categories = null;
			cache = null;
		}
	}

	public class DataStoreException : Exception
	{
		public enum ProblemType
		{
			BadFile,
			BadJson,
			NoCategories
		}

		public DataStoreException(ProblemType problem) : base($"Problem is '{problem}'")
		{
			Problem = problem;
		}

		public ProblemType Problem { get; }
	}
}