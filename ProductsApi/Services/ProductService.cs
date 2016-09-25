using System;
using System.Collections.Generic;
using System.Linq;
using ProductsApi.App_Data;
using ProductsApi.Models;

namespace ProductsApi.Services
{
	public class ProductService : IProductService
	{
		private readonly IReadDataStore _dataStore;
		private readonly IAzureLogger _logger;

		public ProductService(IReadDataStore dataStore, IAzureLogger logger )
		{
			_dataStore = dataStore;
			_logger = logger;
		}

		public Product Get(int id)
		{
			if (id < 1 || id > _dataStore.Seed)
			{
				_logger.LogWarning(GetType(), $"Invalid input id = '{id}'");
				return null;
			}

			return _dataStore.Cache.SingleOrDefault(p => p.ProductID == id);
		}

		public IEnumerable<Product> GetByCategory(string category)
		{
			if (!_dataStore.Categories.Contains(category))
			{
				_logger.LogWarning(GetType(), $"Invalid input category = '{category}'");
				return null;
			}

			return _dataStore
				.Cache
				.Where(p => p.Category == category)
				.ToList();
		}
	}
}