using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Scopes
{
	public abstract class MonoInstaller : MonoBehaviour, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}