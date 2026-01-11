using UI.BaseUI.Interfaces;
using UnityEngine;

namespace UI.BaseUI.Implemenation
{
	public abstract class View : MonoBehaviour, IView
	{
		public virtual void Activate() => 
			gameObject.SetActive(true);

		public virtual void Deactivate() => 
			gameObject.SetActive(false);
	}
}