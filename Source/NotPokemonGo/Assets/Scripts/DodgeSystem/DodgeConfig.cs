using UnityEngine;

namespace DodgeSystem
{
	[CreateAssetMenu(fileName = nameof(DodgeConfig), menuName = "StaticData/" + nameof(DodgeConfig))]
	public class DodgeConfig : ScriptableObject
	{
		public AnimationClip AnimationClip;
	}
}