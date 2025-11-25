using System;
using Abilities;
using Abilities.AbilitySteps;
using Infrastructure.ReactionSystem;
using ReactionSystems;
using Units;

namespace Services.AbilityServices
{
    public class AbilityPhaseService
    {
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;

        public AbilityPhaseService(
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
        }

        public void OnNext(AbilityStepData phase, Unit source, Unit target)
        {
        }
        
        private void HandleArmamentSetup(ArmamentStepData armamentStepData,  Unit source, Unit target)
        {
            var setup = armamentStepData.Armament;

            if (setup.HasSetupData == false)
                return;

            var targets = _targetSelector.GetTargets(armamentStepData.TargetMode, target).ToArray();

            _abilityApplicatorService.Apply(setup, source, targets);
        }

        private void HandleCastamentSetup(CastamentStepData castamentStepData, Unit source, Unit target)
        {
            var setup = castamentStepData.Castament;

            if (setup.HasSetupData == false)
                return;

            var targets = _targetSelector.GetTargets(castamentStepData.TargetMode, target).ToArray();

            var context = new ReactionContext(source, target, setup.EffectsSetup[0]);

            if (_reactionService.TryReact(context))
                return;

            _abilityApplicatorService.Apply(setup, source, targets);
        }

        public void Visit(CastamentStepData step, Unit source, Unit target)
        {
            throw new NotImplementedException();
        }

        public void Visit(ArmamentStepData step, Unit source, Unit target)
        {
            throw new NotImplementedException();
        }

        public void Visit(PlayAnimationStepData step, Unit source, Unit target)
        {
            throw new NotImplementedException();
        }

        public void Visit(QteStepData step, Unit source, Unit target)
        {
            throw new NotImplementedException();
        }

        public void Visit(MeleeAttackStepData step, Unit source, Unit target)
        {
            throw new NotImplementedException();
        }
    }
}