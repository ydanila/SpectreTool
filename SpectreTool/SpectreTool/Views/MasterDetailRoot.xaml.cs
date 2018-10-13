using System;
using System.Linq;
using SpectreTool.ViewModel;
using SpectreTool.ViewModel.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpectreTool.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterDetailRoot : MasterDetailPage
	{
		public MasterDetailRoot()
		{
			InitializeComponent();
			MessagingCenter.Subscribe<ShowViewMessage>(this, MessagesKeys.ShowView, ShowViewHandler);
		}

		private void ShowViewHandler(ShowViewMessage message)
		{
			var masterViewModel = (MasterViewModel) Master.BindingContext;
			var items = masterViewModel.MenuItems;
			masterViewModel.SelectedItem = (from i in items
				where i.View == message.View
				select i).FirstOrDefault();

			ShowMenuItem(masterViewModel.SelectedItem);
		}

		internal void ShowMenuItem(MasterDetailPageMenuItem item)
		{
			if (item == null)
			{
#if DEBUG
				throw new Exception("Menu item not set");
#else
				return;
#endif
			}

			var page = (Page) Activator.CreateInstance(item.TargetType);
			page.Title = item.Title;

			Detail = new NavigationPage(page);
			//IsPresented = false;
		}
	}
}