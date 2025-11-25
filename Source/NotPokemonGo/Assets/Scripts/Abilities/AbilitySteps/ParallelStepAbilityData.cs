using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Abilities.AbilitySteps
{
	public class ParallelStepAbilityData : AbilityStepData
	{
		[SerializeReference]
		public List<AbilityStepData> Steps = new();
	}
}