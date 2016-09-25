using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ProductsApi.App_Data;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi
{
	public class AutofacConfig
	{
		public static IContainer RegisterAutoFac()
		{
			var builder = new ContainerBuilder();

			AddMvcRegistrations(builder);
			AddRegisterations(builder);

			var container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

			return container;
		}

		private static void AddMvcRegistrations(ContainerBuilder builder)
		{
			//mvc
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
			builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
			builder.RegisterModelBinderProvider();

			//web api
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
			builder.RegisterModule<AutofacWebTypesModule>();
		}

		private static void AddRegisterations(ContainerBuilder builder)
		{
			builder
				.RegisterInstance(new ConfigurationSettings {DatastoreFilename = "products.json"})
				.AsSelf();

			builder.RegisterType<ProductService>().As<IProductService>().InstancePerDependency();
			builder.RegisterType<AzureLogger>().As<IAzureLogger>().InstancePerDependency();
			builder.RegisterType<ReadDataStore>().As<IReadDataStore>().InstancePerDependency();
		}
	}
}