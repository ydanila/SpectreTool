using System;
using SpectreTool.Model.Interfaces;

namespace SpectreTool.Model
{
	public static class DependencyLocator
	{
		private static IDependencyProvider m_provider = new DependencyServiceProvider();
		
		internal static void SetProvider(IDependencyProvider dependencyProvider)
		{
			m_provider = dependencyProvider ?? throw new ArgumentNullException("dependencyProvider");
		}

		public static T Get<T>() where T : class
		{
			return m_provider.Get<T>();
		}

		internal static void Clear()
		{
			m_provider.Clear();
		}

		public static void Register<T>(DependencyLocatorTarget target = DependencyLocatorTarget.GlobalInstance) where T : class
		{
			m_provider.Register<T>(target);
		}

		public static void Register<T1, T2>(DependencyLocatorTarget target = DependencyLocatorTarget.GlobalInstance) where T2 : class, T1 where T1 : class
		{
			m_provider.Register<T1, T2>(target);
		}
	}
}
