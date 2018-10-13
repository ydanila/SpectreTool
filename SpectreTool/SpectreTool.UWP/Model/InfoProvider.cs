using System.Threading.Tasks;
using SpectreTool.Model.Interfaces;
using SpectreTool.ServiceDTO.Data;

namespace SpectreTool.UWP.Model
{
	public class InfoProvider : IInfoProvider
	{
		public Task<CheckResult> Check()
		{
			return ((App)App.Current).CheckAsync();
		}
	}
}
