using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace AbilitiesTestFeature
{
	[CreateAssetMenu(menuName = "Config/AbilitiesTestFeatureConfig", fileName = "AbilitiesTestFeatureConfig")]
	public class AbilitiesTestFeatureConfig : ScriptableObject
	{
		public List<UnitConfig> HeroConfigs;
		public  List<UnitConfig> EnemyConfigs;
	}
}