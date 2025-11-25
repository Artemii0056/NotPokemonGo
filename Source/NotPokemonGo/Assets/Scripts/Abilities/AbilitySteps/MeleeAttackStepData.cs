using Units;

namespace Abilities.AbilitySteps
{
	public class MeleeAttackStepData : AbilityStepData
	{
		public override void Accept(IAbilityStepVisitor visitor, Unit source, Unit target)
			=> visitor.Visit(this, source, target);
	}
}