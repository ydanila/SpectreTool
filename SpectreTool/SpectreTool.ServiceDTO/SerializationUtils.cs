using Newtonsoft.Json;

namespace SpectreTool.ServiceDTO
{
	public static class SerializationUtils
    {
		public static string SerializeObject(object obj)
		{
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			return JsonConvert.SerializeObject(obj, settings);
		}

		public static TObj DeserializeObject<TObj>(string json) where TObj : class
		{
			var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			return JsonConvert.DeserializeObject<TObj>(json, settings);
		}
	}
}
