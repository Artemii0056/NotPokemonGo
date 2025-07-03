using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.MV;
using Animations;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Characters
{
    public class UnitStep
    {
        private readonly UnitAnimatorTrigger _unitAnimatorTrigger;
        private readonly UnitAnimatorController _unitAnimatorController;
        private readonly Animator _animator;
        private readonly ICoroutineRunner _coroutineRunner;

        private Vector3 _startPosition;

        private List<AbilityPhase> _phases;
        private int _currentPhaseIndex;
        private Unit _source;
        private Unit _target;

        public UnitStep(
            UnitAnimatorTrigger unitAnimatorTrigger,
            UnitAnimatorController unitAnimatorController,
            Animator animator,
            ICoroutineRunner coroutineRunner)
        {
            _unitAnimatorTrigger = unitAnimatorTrigger;
            _unitAnimatorController = unitAnimatorController;
            _animator = animator;
            _coroutineRunner = coroutineRunner;
        }

        public event Action ActionEnded;

        public void SetAbilityModel(AbilityModel abilityModel, Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _phases = abilityModel.Phases;
            _currentPhaseIndex = 0;

            _startPosition = source.transform.position;

            _coroutineRunner.StartCoroutine(HandlePhase(_phases));
        }

        private IEnumerator HandlePhase(List<AbilityPhase> phases)
        {
           foreach (AbilityPhase abilityPhase in phases)
           {
               switch (abilityPhase.PhaseType)
               {
                   case PhaseType.IsMelee:
                       _unitAnimatorTrigger.SetPhase(abilityPhase);
                       _unitAnimatorController.PlayString( abilityPhase.AnimationClip.name);
                       yield return new WaitForSeconds(GetAnimationLength(abilityPhase.AnimationClip.name));
                       break;
                   
                   case PhaseType.IsMovementPhase:
                       _unitAnimatorTrigger.SetPhase(abilityPhase);
                       _unitAnimatorController.PlayString( abilityPhase.AnimationClip.name);
                       yield return MoveUnit(_source, _target.transform.position,  abilityPhase.AnimationClip,1f);
                       break;
                   
                   case PhaseType.IsReturnPhase:
                       _unitAnimatorTrigger.SetPhase(abilityPhase);
                       _unitAnimatorController.PlayString( abilityPhase.AnimationClip.name);
                       yield return MoveUnit(_source, _startPosition,  abilityPhase.AnimationClip);
                       break;
                   
                   case PhaseType.Default:
                       _unitAnimatorTrigger.SetPhase(abilityPhase);
                       _unitAnimatorController.PlayString( abilityPhase.AnimationClip.name);
                       yield return new WaitForSeconds(GetAnimationLength(abilityPhase.AnimationClip.name));
                       break;
               }
           }
           
           ActionEnded?.Invoke();
        }

        private IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, AnimationClip animationClip, float offset = 0)
        {
            const float Speed = 2f;

            while (Vector3.Distance(unit.transform.position, targetPosition) > offset)
            {
                unit.transform.position = Vector3.MoveTowards(
                    unit.transform.position,
                    targetPosition,
                    Speed * Time.deltaTime);

                yield return null;
            }
        }

        private float GetAnimationLength(string animationName)
        {
            AnimationClip clip =
                _animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == animationName);

            if (clip == null)
                throw new Exception($"Animator not contains animation {"animationName"}");

            return clip.length;
        }
    }
}