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
        private AbilityModel _abilityModel;
        private UnitAnimatorTrigger _unitAnimatorTrigger;
        private UnitAnimatorController _unitAnimatorController;
        private readonly ICoroutineRunner _coroutineRunner;
        private IAnimationProcessingService _animationProcessingService;
        private Animator _animator;

        private Vector3 _startPosition;

        private List<AbilityPhase> _phases;
        private int _currentPhaseIndex;
        private Unit _source;
        private Unit _target;

        public UnitStep(UnitAnimatorTrigger unitAnimatorTrigger,
            ICoroutineRunner coroutineRunner,
            UnitAnimatorController unitAnimatorController,
            IAnimationProcessingService animationProcessingService,
            Animator animator)
        {
            _unitAnimatorTrigger = unitAnimatorTrigger;
            _coroutineRunner = coroutineRunner;
            _unitAnimatorController = unitAnimatorController;
            _animationProcessingService = animationProcessingService;
            _animator = animator;
        }

        const string AttackAnimationName = "DoubleAttack";

        public event Action<AbilityModel> OnAbility;

        public event Action ActionEnded;

        public void SetAbilityModel(AbilityModel abilityModel, Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _phases = abilityModel.Phases;
            _currentPhaseIndex = 0;

            _startPosition = source.transform.position;
            _abilityModel = abilityModel;

            _unitAnimatorController.Finished += OnAnimationFinished;
            ProcessNextPhase();
        }

        private void ProcessNextPhase()
        {
            Debug.Log(_currentPhaseIndex + " Processing phase");

            if (_currentPhaseIndex >= _phases.Count)
            {
                EndAction();
                return;
            }

            AbilityPhase phase = _phases[_currentPhaseIndex++];

            _unitAnimatorTrigger.SetPhase(phase);
            _coroutineRunner.StartCoroutine(HandlePhase(phase));
        }

        private IEnumerator HandlePhase(AbilityPhase phase)
        {
            _unitAnimatorController.PlayString( phase.AnimationClip.name);
           // _animationProcessingService.PlayAnimation(_source, phase.AnimationClip.name);

            if (phase.IsMovementPhase)
            {
                //_animationProcessingService.PlayAnimation(_source, phase.AnimationClip.name); //TODO тут будет анимация фазы 
                yield return MoveUnit(_source, _target.transform.position,  phase.AnimationClip,1f);
                ProcessNextPhase();
                yield break;
            }

            if (phase.IsMelee)
            {
                //_animationProcessingService.PlayAnimation(_source, _abilityModel.AbilityType);
                yield return new WaitForSeconds(GetAnimationLength(phase.AnimationClip.name));
                ProcessNextPhase();
                yield break;
            }

            if (phase.IsReturnPhase)
            {
                //_animationProcessingService.PlayAnimation(_source, "RunBack");
                yield return MoveUnit(_source, _startPosition,  phase.AnimationClip);
                ProcessNextPhase();
                yield  break;
            }

            // ProcessNextPhase();
            // yield break;

            _unitAnimatorController.PlayString( phase.AnimationClip.name);
           // _animationProcessingService.PlayAnimation(_source, _abilityModel.AbilityType);
        }

        private void OnAnimationFinished()
        {
            ProcessNextPhase();
        }

        private void EndAction()
        {
            _currentPhaseIndex = 0;
            _unitAnimatorController.Finished -= OnAnimationFinished;
            ActionEnded?.Invoke();
            Debug.Log("Ending action");
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

            _unitAnimatorController.PlayString(animationClip.name);
           // _animationProcessingService.PlayAnimation(_source, "Idle"); //Тут не должен вызываться Idle
        }

        // private IEnumerator PlayAbilityAnimation()
        // {
        //     yield return new WaitForSeconds(GetAnimationLength() + 0.1f);
        // }

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