using System.Collections.Generic;
using Effects;
using Statuses;
using Units;

namespace Armaments
{
	public interface IEffectsApplier
	{
		void ApplyEffectsOnTarget(Unit source, Unit target, List<Status> statuses, List<EffectInfo> effects);
	}
}