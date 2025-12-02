using Characters.Configs;
using UnityEngine;

namespace DodgeSystem.Configs
{
	[CreateAssetMenu(fileName = nameof(DodgeConfig), menuName = "StaticData/" + nameof(DodgeConfig))]
	public class DodgeConfig : ScriptableObject
	{
		public UnitType UnitType;
		public AnimationClip AnimationClip;
	}
}