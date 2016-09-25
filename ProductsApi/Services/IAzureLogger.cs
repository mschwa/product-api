using System;

namespace ProductsApi.Services
{
	public interface IAzureLogger
	{
		void LogInfo(string message);
		void LogWarning(Type type, string message);
		void LogError(Type type, string message, Exception ex);
	}
}