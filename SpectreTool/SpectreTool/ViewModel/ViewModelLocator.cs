using SpectreTool.Model;
using SpectreTool.Model.Interfaces;

namespace SpectreTool.ViewModel
{
	class ViewModelLocator
    {
		public MasterViewModel Master
		{
			get { return DependencyLocator.Get<MasterViewModel>(); }
		}

		public MainViewModel Main
		{
			get { return DependencyLocator.Get<MainViewModel>(); }
		}

	    public OldMainViewModel OldMain
	    {
		    get { return DependencyLocator.Get<OldMainViewModel>(); }
	    }

		public DetailViewModel Detail
		{
			get { return DependencyLocator.Get<DetailViewModel>(); }
		}

		public CheckViewModel Check
		{
			get { return DependencyLocator.Get<CheckViewModel>(); }
		}

		public static void RegisterViews()
		{
			DependencyLocator.Register<MasterViewModel>(DependencyLocatorTarget.GlobalInstance);
			DependencyLocator.Register<MainViewModel>(DependencyLocatorTarget.GlobalInstance);
			DependencyLocator.Register<OldMainViewModel>(DependencyLocatorTarget.GlobalInstance);
			DependencyLocator.Register<DetailViewModel>(DependencyLocatorTarget.GlobalInstance);
			DependencyLocator.Register<CheckViewModel>(DependencyLocatorTarget.GlobalInstance);
		}
	}
}
