using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Scopes
{
	public class CustomScope : LifetimeScope
	{
		[SerializeField] private List<MonoInstaller> _injectableServicesInstallers;
		[SerializeField] private List<MonoInstaller> _defaultServicesInstallers;
		protected override void Awake()
		{
			base.Awake();
			Debug.Log("CustomScope.Awake");
			DontDestroyOnLoad(this);
		}

		[Inject]
		private void Construct(IObjectResolver objectResolver)
		{
			Debug.Log("CustomScope.Construct");
			_injectableServicesInstallers.ForEach(objectResolver.Inject);
		}
		
		protected override void Configure(IContainerBuilder builder)
		{
			Debug.Log("CustomScope.Configure");
			_injectableServicesInstallers.ForEach(service => service.Install(builder));
			_defaultServicesInstallers.ForEach(service => service.Install(builder));
		}
	}
}