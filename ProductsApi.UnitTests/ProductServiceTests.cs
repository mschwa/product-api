using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using ProductsApi.App_Data;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.UnitTests
{
	[TestFixture]
	public class ProductServiceTests
	{
		[Test]
		public void GetRetrievesProduct()
		{
			var logger = new Mock<IAzureLogger>().Object;
			var store = new Mock<IReadDataStore>();
			store.SetupGet(s => s.Cache).Returns(MockProducts());
			store.SetupGet(s => s.Seed).Returns(3);

			var service = new ProductService(store.Object, logger);

			Product product = null;

			Assert.DoesNotThrow(() => product = service.Get(1));
			Assert.NotNull(product);
			Assert.AreEqual(1, product.ProductID);
		}

		[Test]
		public void GetWithNonExistIdReturnsNull()
		{
			var logger = new Mock<IAzureLogger>().Object;
			var store = new Mock<IReadDataStore>();
			store.SetupGet(s => s.Cache).Returns(MockProducts());
			store.SetupGet(s => s.Seed).Returns(6);

			var service = new ProductService(store.Object, logger);

			Product product = null;

			Assert.DoesNotThrow(() => product = service.Get(5));
			Assert.Null(product);
		}

		[Test]
		public void GetOutofBoundsSkipsExecution()
		{
			var logger = new Mock<IAzureLogger>().Object;
			var store = new Mock<IReadDataStore>();
			store.SetupGet(s => s.Cache).Returns(MockProducts());
			store.SetupGet(s => s.Seed).Returns(3);
			var service = new ProductService(store.Object, logger);

			Product product = null;

			Assert.DoesNotThrow(() => product = service.Get(5));
			Assert.Null(product);

			store.VerifyGet(s => s.Cache, Times.Never);

			Assert.DoesNotThrow(() => product = service.Get(-1));
			Assert.Null(product);

			store.VerifyGet(s => s.Cache, Times.Never);
		}

		[Test]
		public void GetByCategorySucceeds()
		{
			var logger = new Mock<IAzureLogger>().Object;
			var store = new Mock<IReadDataStore>();
			store.SetupGet(s => s.Cache).Returns(MockProducts());
			store.SetupGet(s => s.Categories).Returns(MockCategories);

			var service = new ProductService(store.Object, logger);

			IEnumerable<Product> products = null;

			Assert.DoesNotThrow(() => products = service.GetByCategory("Cat 2"));
			Assert.NotNull(products);
			Assert.True(products.All(p => p.Category == "Cat 2"));
		}

		[Test]
		public void GetByCategoryThrowsWithBadCategory()
		{
			var logger = new Mock<IAzureLogger>().Object;
			var store = new Mock<IReadDataStore>();
			store.SetupGet(s => s.Cache).Returns(MockProducts());
			store.SetupGet(s => s.Categories).Returns(MockCategories);

			var service = new ProductService(store.Object, logger);

			Assert.Throws<ArgumentException>(() => service.GetByCategory("!@$^%&"));
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
			return new[] {"Cat 1", "Cat 2"};
		} 
	}
}