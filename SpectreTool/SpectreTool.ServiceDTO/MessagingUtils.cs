using System;
using System.Collections.Generic;
using SpectreTool.ServiceDTO.Command;
using SpectreTool.ServiceDTO.Data;
using SpectreTool.ServiceDTO.Interfaces;

namespace SpectreTool.ServiceDTO
{
	public static class MessagingUtils
	{
		public const string COMMAND = "command";

		public const string RESULT = "result";

		public static TC PackCommand<TC>(BaseCommand command) where TC : IDictionary<string, object>
		{
			return PackMessage<TC>(COMMAND, command);
		}

		public static TCR PackResult<TCR>(BaseCommandResult result) where TCR : IDictionary<string, object>
		{
			return PackMessage<TCR>(RESULT, result);
		}

		public static TRes PackMessage<TRes>(string key, IJsonSerializable obj) where TRes : IDictionary<string, object>
		{
			var result = Activator.CreateInstance<TRes>();
			result.Add(key, obj.ToJson());

			return result;
		}

		public static TC UnpackCommand<TC>(IDictionary<string, object> message) where TC : BaseCommand
		{
			return UnpackMessage<TC>(COMMAND, message);
		}

		public static TCR UnpackResult<TCR>(IDictionary<string, object> message) where TCR : BaseCommandResult
		{
			return UnpackMessage<TCR>(RESULT, message);
		}

		public static TRes UnpackMessage<TRes>(string key, IDictionary<string, object> message) where TRes : class, IJsonSerializable
		{
			return SerializationUtils.DeserializeObject<TRes>(message[key] as string);
		}
	}
}
