using SpectreTool.ServiceDTO.Interfaces;

namespace SpectreTool.ServiceDTO.Command
{
	public abstract class BaseCommand : IJsonSerializable
    {
		public string ToJson()
		{
			return SerializationUtils.SerializeObject(this);
		}

		public static BaseCommand DeserializeCommand(string json)
		{
			return SerializationUtils.DeserializeObject<BaseCommand>(json);
		}
	}
}
