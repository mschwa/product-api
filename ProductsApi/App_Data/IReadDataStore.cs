using System;
using System.Collections.Generic;
using ProductsApi.Models;

namespace ProductsApi.App_Data
{
	public interface IReadDataStore : IDisposable
	{
		int Seed { get; }
		string[] Categories { get; }
		IList<Product> Cache { get; }
	}
}