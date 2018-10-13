using System;

namespace SpectreTool.Model.Interfaces
{
	public enum DependencyLocatorTarget
	{
		GlobalInstance,
		NewInstance
	}

	interface IDependencyProvider
	{
		T Get<T>() where T : class;

		void Add(Type implementor);

		bool Contains(Type implementor);

		void Register<T>(DependencyLocatorTarget target) where T : class;

		void Register<T1, T2>(DependencyLocatorTarget target)
			where T1 : class
			where T2 : class, T1;

		void Clear();
	}
}
