using System.Windows.Input;
using SpectreTool.ViewModel.Messages;
using SpectreTool.Views;
using Xamarin.Forms;

namespace SpectreTool.ViewModel
{
	public class MainViewModel : BaseViewModel
	{
		public ICommand ShowMoreDetails { get; }

		public ICommand ShowCheck { get; }

		public MainViewModel()
		{
			ShowMoreDetails = new Command(() =>
			{
				MessagingCenter.Send<ShowViewMessage>(new ShowViewMessage { View = ViewType.Details }, MessagesKeys.ShowView);
			});

			ShowCheck = new Command(() =>
			{
				MessagingCenter.Send<ShowViewMessage>(new ShowViewMessage { View = ViewType.Check }, MessagesKeys.ShowView);
			});
		}
	}
}
