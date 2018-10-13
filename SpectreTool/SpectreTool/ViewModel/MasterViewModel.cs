using System.Collections.Generic;
using System.Windows.Input;
using SpectreTool.ViewModel.Messages;
using SpectreTool.Views;
using Xamarin.Forms;

namespace SpectreTool.ViewModel
{
	public class MasterViewModel : BaseViewModel
	{
		private MasterDetailPageMenuItem m_selectedItem;

		public List<MasterDetailPageMenuItem> MenuItems { get; private set; }

		public MasterDetailPageMenuItem SelectedItem
		{
			get => m_selectedItem;
			set
			{
				m_selectedItem = value;
				OnPropertyChanged();
			}
		}

		public ICommand ShowPage { get; }
		
		public MasterViewModel()
		{
			MenuItems = new List<MasterDetailPageMenuItem>(new[]
			{
					new MasterDetailPageMenuItem { View = ViewType.Main, Title = Localization.UI.MainPage, TargetType = typeof(MainPageDetail) },
					new MasterDetailPageMenuItem { View = ViewType.Details, Title = Localization.UI.Details, TargetType = typeof(DetailsPageDetail) },
					new MasterDetailPageMenuItem { View = ViewType.Check, Title = Localization.UI.Check, TargetType = typeof(CheckPageDetail) },
					new MasterDetailPageMenuItem { View = ViewType.About, Title = Localization.UI.About, TargetType = typeof(AboutPageDetail) }
			});

			ShowPage = new Command(() =>
			{
				if(SelectedItem == null)
				{
					return;
				}

				MessagingCenter.Send<ShowViewMessage>(new ShowViewMessage { View = SelectedItem.View }, MessagesKeys.ShowView);
			});
		}
	}
}
