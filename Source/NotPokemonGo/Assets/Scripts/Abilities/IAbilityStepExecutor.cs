using System.Collections;
using System.Threading;
using Abilities.AbilitySteps;
using Units;

namespace Abilities
{
	public interface IAbilityStepExecutor
	{
		IEnumerator ExecuteStep(
			AbilityStepData step,
			Unit source,
			Unit target,
			ExecutionContext ctx);
	}
}