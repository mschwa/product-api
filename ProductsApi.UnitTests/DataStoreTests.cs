using Moq;
using NUnit.Framework;
using ProductsApi.App_Data;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.UnitTests
{
	[TestFixture]
	public class DataStoreTests
	{
		[Test]
		public void StoreInitializesSuccesfully()
		{
			ReadDataStore _store = null;
			var settings  = new ConfigurationSettings {DatastoreFilename = "/TestFiles/products.json" };
			var logger = new Mock<IAzureLogger>().Object;

			Assert.DoesNotThrow(() => { _store = new ReadDataStore(settings, logger);});
			Assert.NotNull(_store.Categories);
			Assert.NotNull(_store.Cache);
		}

		[Test]
		public void StoreInitializesWithNoCategories()
		{
			var settings = new ConfigurationSettings { DatastoreFilename = "/TestFiles/nocategories.json" };
			var logger = new Mock<IAzureLogger>().Object;
			
			var ex = Assert.Throws<DataStoreException>(() => { new ReadDataStore(settings, logger); });
			Assert.AreEqual(DataStoreException.ProblemType.NoCategories, ex.Problem);
		}

		[Test]
		public void StoreInitializesWithMissingFile()
		{
			var settings = new ConfigurationSettings { DatastoreFilename = "/TestFiles/missing.json" };
			var logger = new Mock<IAzureLogger>().Object;

			var ex = Assert.Throws<DataStoreException>(() => { new ReadDataStore(settings, logger); });
			Assert.AreEqual(DataStoreException.ProblemType.BadFile, ex.Problem);
		}

		[Test]
		public void StoreInitializesWithBadJSON()
		{
			var settings = new ConfigurationSettings { DatastoreFilename = "/TestFiles/badjson.json" };
			var logger = new Mock<IAzureLogger>().Object;

			var ex = Assert.Throws<DataStoreException>(() => { new ReadDataStore(settings, logger); });
			Assert.AreEqual(DataStoreException.ProblemType.BadJson, ex.Problem);
		}
	}
}