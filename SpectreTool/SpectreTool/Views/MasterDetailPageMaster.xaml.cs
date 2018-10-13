using System.Collections.Generic;
using System.Linq;
using SpectreTool.ViewModel.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterDetailPageMaster : ContentPage
	{
		public MasterDetailPageMaster()
		{
			InitializeComponent();
			MessagingCenter.Subscribe<ShowViewMessage>(this, MessagesKeys.ShowView, ShowViewHandler);
		}

		private void ShowViewHandler(ShowViewMessage message)
		{
			//	force selected item because changing SelectedItem for ViewModel has no effect
			var items = (List<MasterDetailPageMenuItem>)MenuItemsListView.ItemsSource;
			MenuItemsListView.SelectedItem = (from i in items where i.View == message.View select i).FirstOrDefault();
		}
	}
}