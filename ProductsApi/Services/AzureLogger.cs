using System;

namespace ProductsApi.Services
{
	public class AzureLogger : IAzureLogger
	{
		public void LogInfo(string message)
		{
			System
				.Diagnostics
				.Trace
				.TraceWarning(message);
		}

		public void LogWarning(Type type, string message)
		{
			System
				.Diagnostics
				.Trace
				.TraceWarning("{0} has a warning. {1}.", type, message);
		}

		public void LogError(Type type, string message, Exception ex)
		{
			System
				.Diagnostics
				.Trace
				.TraceError("{0} threw an exception. {1}. Stack Trace:{2}", type, message, ex.StackTrace);
		}
	}
}