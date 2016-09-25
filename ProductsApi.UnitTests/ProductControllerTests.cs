using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Moq;
using NUnit.Framework;
using ProductsApi.Controllers;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.UnitTests
{
	[TestFixture]
	public class ProductControllerTests
	{
		[Test]
		public void GetSucceeds()
		{
			var service = new Mock<IProductService>();
			service.Setup(s => s.Get(1)).Returns(MockProduct());

			var controller = new ProductsController(service.Object);

			string response = null;

			Assert.DoesNotThrow(() => response = controller.Get(1));
			Assert.NotNull(response);
			Assert.AreEqual(MockProduct().AsJSON(), response);
		}

		[Test]
		public void GetWithNegativeInputReturnsCode()
		{
			var service = new Mock<IProductService>();

			var controller = new ProductsController(service.Object);

			var ex = Assert.Throws<HttpResponseException>(() => controller.Get(-1));
			Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
		}

		[Test]
		public void GetWithNonExistIdReturnsCode()
		{
			var service = new Mock<IProductService>();

			var controller = new ProductsController(service.Object);

			var ex = Assert.Throws<HttpResponseException>(() => controller.Get(6));
			Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
		}

		[Test]
		public void GetFailsReturnsCode()
		{
			var service = new Mock<IProductService>();
			service.Setup(s => s.Get(It.IsAny<int>())).Throws<Exception>();

			var controller = new ProductsController(service.Object);

			var ex = Assert.Throws<HttpResponseException>(() => controller.Get(6));
			Assert.AreEqual(HttpStatusCode.InternalServerError, ex.Response.StatusCode);
		}

		[Test]
		public void GetByCategorySucceeds()
		{
			var testData = MockProducts().GetRange(1, 2);

			var service = new Mock<IProductService>();
			service.Setup(s => s.GetByCategory("Cat 2")).Returns(MockProducts().GetRange(1,2));

			var controller = new ProductsController(service.Object);

			IEnumerable<string> response = null;

			Assert.DoesNotThrow(() => response = controller.Get("Cat 2"));
			Assert.NotNull(response);

			var products = response.ToList();

			for (var i = 0; i < testData.Count; i++)
			{
				Assert.AreEqual(testData[i].AsJSON(), products[i]);
			}
		}

		[Test]
		public void GetByNonExistCategoryReturnsCode()
		{
			var service = new Mock<IProductService>();
			service.Setup(s => s.GetByCategory(It.IsAny<string>())).Returns(default(IEnumerable<Product>));

			var controller = new ProductsController(service.Object);

			var ex = Assert.Throws<HttpResponseException>(() => controller.Get("Test"));
			Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
		}

		[Test]
		public void GetByCategoryFailsReturnsCode()
		{
			var service = new Mock<IProductService>();
			service.Setup(s => s.GetByCategory(It.IsAny<string>())).Throws<Exception>();

			var controller = new ProductsController(service.Object);

			var ex = Assert.Throws<HttpResponseException>(() => controller.Get("Test"));
			Assert.AreEqual(HttpStatusCode.InternalServerError, ex.Response.StatusCode);
		}

		private Product MockProduct()
		{
			return new Product {ProductID = 1, ProductName = "Product 1", Category = "Cat 1"};
		}

		private List<Product> MockProducts()
		{
			return new List<Product>
			{
				new Product {ProductID = 1, ProductName = "Product 1", Category = "Cat 1"},
				new Product {ProductID = 2, ProductName = "Product 2", Category = "Cat 2"},
				new Product {ProductID = 3, ProductName = "Product 3", Category = "Cat 2"},
			};
		}

		private string[] MockCategories()
		{
			return new[] { "Cat 1", "Cat 2" };
		}
	}
}