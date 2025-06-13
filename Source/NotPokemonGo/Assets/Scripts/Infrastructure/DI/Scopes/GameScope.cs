using UnityEngine;

namespace Infrastructure.DI.Scopes
{
	public class GameScope : CustomScope
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(this);
		}
	}
}