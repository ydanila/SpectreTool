using System;
using Autofac;
using Autofac.Builder;
using SpectreTool.Model.Interfaces;

namespace SpectreTool.Model
{
	internal class DependencyServiceProvider : IDependencyProvider
	{
		private ContainerBuilder containerBuilder;
		private IContainer _container;

		public DependencyServiceProvider()
		{
			Init();
		}

		public void Clear()
		{
			Init();
		}

		public void Add(Type implementor)
		{
			if(implementor.BaseType == null)
			{
				throw new ArgumentException("Implementor must have base type", nameof(implementor));
			}

			if (!implementor.BaseType.IsInterface)
			{
				throw new ArgumentException("Implementor base type must be interface", nameof(implementor));
			}

			var newBuilder = new ContainerBuilder();
			InstanceConfiguration(newBuilder.RegisterType(implementor), DependencyLocatorTarget.GlobalInstance);
			newBuilder.Update(_container);
		}

		public bool Contains(Type implementor)
		{
			return _container.IsRegistered(implementor);
		}

		public T Get<T>() where T : class
		{
			return _container.Resolve<T>();
		}

		public void Register<T>(DependencyLocatorTarget target) where T : class
		{
			var newBuilder = new ContainerBuilder();
			InstanceConfiguration(newBuilder.RegisterType<T>(), target);
			newBuilder.Update(_container);
		}

		public void Register<T1, T2>(DependencyLocatorTarget target)
			where T1 : class
			where T2 : class, T1
		{
			var newBuilder = new ContainerBuilder();
			InstanceConfiguration(newBuilder.RegisterType<T2>().As<T1>(), target);
			newBuilder.Update(_container);
		}

		private void Init()
		{
			containerBuilder = new ContainerBuilder();
			_container = containerBuilder.Build();
		}

		private IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> InstanceConfiguration<T>(IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder, DependencyLocatorTarget target) where T : class
		{
			switch(target)
			{
				case DependencyLocatorTarget.GlobalInstance:
					return registrationBuilder.SingleInstance();

				case DependencyLocatorTarget.NewInstance:
					return registrationBuilder;

				default:
					throw new NotSupportedException();
			}
		}
	}
}
