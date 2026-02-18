using System.Collections.Generic;
using Statuses;
using Units;

namespace Effects
{
	public interface IEffectsApplier
	{
		void ApplyEffectsOnTarget(Unit target, List<Status> statuses, List<EffectInfo> effects);
	}
}