using System.Threading.Tasks;
using SpectreTool.ServiceDTO.Data;

namespace SpectreTool.Model.Interfaces
{
	public interface IInfoProvider
	{
		Task<CheckResult> Check();
	}
}
