using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
	public class ProductsController : ApiController
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		// GET api/values
		public IEnumerable<string> Get(string category)
		{
			var products = InvokeRequest(_productService.GetByCategory, category);

			var responseBody = products
				.Select(p => p.AsJSON())
				.ToArray();

			return responseBody;
		}

		// GET api/values/5
		public string Get(int id)
		{
			var product = InvokeRequest(_productService.Get, id);

			return product.AsJSON();
		}

		private static T2 InvokeRequest<T1, T2>(Func<T1, T2> function, T1 input)
		{
			T2 results;

			try
			{
				results = function.Invoke(input);
			}
			catch
			{
				var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
				{
					Content = new StringContent("System Unavailable")
				};

				throw new HttpResponseException(resp);
			}

			if (results == null)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
				{
					Content = new StringContent($"No products found with param = {input}"),
					ReasonPhrase = "Products Not Found"
				};

				throw new HttpResponseException(resp);
			}

			return results;
		}
	}
}