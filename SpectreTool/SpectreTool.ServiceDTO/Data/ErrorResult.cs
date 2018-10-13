namespace SpectreTool.ServiceDTO.Data
{
	public class ErrorResult : BaseCommandResult
	{
		public override bool IsError
		{
			get { return true; }
			set { }
		}
	}
}
