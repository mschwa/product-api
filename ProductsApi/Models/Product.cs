using Newtonsoft.Json;

namespace ProductsApi.Models
{
	public class Product
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public string Category { get; set; }

		public string AsJSON()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}