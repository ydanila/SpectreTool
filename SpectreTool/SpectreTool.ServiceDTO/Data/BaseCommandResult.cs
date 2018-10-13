using SpectreTool.ServiceDTO.Interfaces;

namespace SpectreTool.ServiceDTO.Data
{
	public abstract class BaseCommandResult : IJsonSerializable
	{
		public virtual bool IsError { get; set; }

		public bool IsSuccess
		{
			get { return !IsError; }
		}

		public string ToJson()
		{
			return SerializationUtils.SerializeObject(this);
		}

		public static TResult DeserializeCommand<TResult>(string json) where TResult : BaseCommandResult
		{
			return SerializationUtils.DeserializeObject<TResult>(json);
		}
	}
}
