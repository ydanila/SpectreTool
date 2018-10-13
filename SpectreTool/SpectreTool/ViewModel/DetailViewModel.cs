using System.Windows.Input;
using SpectreTool.ViewModel.Messages;
using SpectreTool.Views;
using Xamarin.Forms;

namespace SpectreTool.ViewModel
{
	public class DetailViewModel : BaseViewModel
	{
		public ICommand ShowCheck { get; }

		public DetailViewModel()
		{
			ShowCheck = new Command(() =>
			{
				MessagingCenter.Send<ShowViewMessage>(new ShowViewMessage { View = ViewType.Check }, MessagesKeys.ShowView);
			});
		}
	}
}
