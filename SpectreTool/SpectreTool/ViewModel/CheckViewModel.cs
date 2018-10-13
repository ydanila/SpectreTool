using System.Threading.Tasks;
using System.Windows.Input;
using SpectreTool.Model;
using SpectreTool.Model.Interfaces;
using SpectreTool.ServiceDTO.Data;
using SpectreTool.ViewModel.Messages;
using Xamarin.Forms;

namespace SpectreTool.ViewModel
{
	internal class CheckViewModel : BaseViewModel
	{
		private bool m_isBusy;
		private bool m_isChecked;
		private bool m_isLoaded;
		private CheckResult m_result;

		public CheckViewModel()
		{
			MessagingCenter.Subscribe<CheckDeviceMessage>(this, MessagesKeys.CheckDevice,
				async message => await OnCheckDeviceMessageAsync(message).ConfigureAwait(false));

			RefreshCommand = new Command(async () =>
			{
				if (IsBusy) return;

				m_isChecked = false;
				IsLoaded = false;
				await RefreshResult();
			});
		}

		public CheckResult Result
		{
			get => m_result;
			set
			{
				m_result = value;
				OnPropertyChanged();
			}
		}

		public bool IsBusy
		{
			get => m_isBusy;
			set
			{
				m_isBusy = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsNotBusy));
			}
		}

		public bool IsNotBusy => !IsBusy;

		public bool IsLoaded
		{
			get => m_isLoaded;
			set
			{
				m_isLoaded = value;
				OnPropertyChanged();
			}
		}

		public ICommand RefreshCommand { get; }

		private async Task OnCheckDeviceMessageAsync(CheckDeviceMessage message)
		{
			await RefreshResult();
		}

		private async Task RefreshResult()
		{
			if (IsBusy || m_isChecked) return;

			try
			{
				IsBusy = true;
				var infoProvider = DependencyLocator.Get<IInfoProvider>();
				Result = await infoProvider.Check().ConfigureAwait(false);
				IsLoaded = true;
			}
			finally
			{
				m_isChecked = true;
				IsBusy = false;
			}
		}
	}
}