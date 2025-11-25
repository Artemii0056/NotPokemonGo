using Abilities.AbilitySteps;
using Units;

public interface IAbilityStepVisitor
{
	void Visit(CastamentStepData step, Unit source, Unit target);
	void Visit(ArmamentStepData step, Unit source, Unit target);
	void Visit(PlayAnimationStepData step, Unit source, Unit target);
	void Visit(QteStepData step, Unit source, Unit target);
	void Visit(MeleeAttackStepData step, Unit source, Unit target);
}