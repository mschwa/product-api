using System.Collections.Generic;
using ProductsApi.Models;

namespace ProductsApi.Services
{
	public interface IProductService
	{
		Product Get(int id);
		IEnumerable<Product> GetByCategory(string category);
	}
}