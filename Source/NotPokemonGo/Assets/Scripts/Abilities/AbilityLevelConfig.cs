using System;
using System.Collections.Generic;
using Abilities.AbilitySteps;
using UnityEngine;

namespace Abilities
{
	[CreateAssetMenu(fileName = nameof(AbilityLevelConfig), menuName = "StaticData/" + nameof(AbilityLevelConfig))]
	public class AbilityLevelConfig : ScriptableObject
	{
		public int Level;

		[SerializeReference]
		public List<AbilityStepData> Steps;

		public List<AbilityStatSetup> Stats;
	}
}