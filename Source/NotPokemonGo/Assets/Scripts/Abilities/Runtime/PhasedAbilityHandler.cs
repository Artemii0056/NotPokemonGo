using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Runtime
{
    public abstract class PhasedAbilityHandler : IAbilityHandler //DELETE
    {
        protected readonly ICoroutineRunner Runner;
        protected readonly List<AbilityPart> Parts;

        protected Unit Source;
        protected Unit Target;

        protected UnitAnimatorTrigger AnimatorTrigger;
        protected AnimatorController AnimatorController;

        private Coroutine _routine;

        private bool _waitingAnim;
        private Action _onAnimFinished;

        public Interruptibility Interruptibility { get; }
        public event Action<IAbilityHandler> Finished;

        protected PhasedAbilityHandler(ICoroutineRunner runner, AbilityModel model)
        {
            Runner = runner;
            Parts = model.Parts;
            Interruptibility = model.Interruptibility;
        }

        public void Play(Unit source, Unit target)
        {
            Source = source;
            Target = target;

            AnimatorTrigger = source.AnimatorTrigger;
            AnimatorController = source.AnimatorController;

            OnPlayStarted();

            _routine = Runner.StartCoroutine(RunAllParts());
        }

        public void Stop()
        {
            if (_routine != null)
                Runner.StopCoroutine(_routine);

            // if (_waitingAnim && _onAnimFinished != null && AnimatorController != null)
            //     AnimatorController.Finished -= _onAnimFinished;

            _waitingAnim = false;
            _onAnimFinished = null;

            OnStopped();
        }

        protected virtual void OnPlayStarted() { }
        protected virtual void OnStopped() { }

        private IEnumerator RunAllParts()
        {
            foreach (var part in Parts)
            {
                foreach (var phase in part.AbilityPhases)
                    yield return ExecutePhase(phase);
            }

            FinishInternal();
        }

        protected abstract IEnumerator ExecutePhase(AbilityPhase phase);

        protected void PlayPhaseAnimation(AbilityPhase phase)
        {
            AnimatorTrigger.SetTarget(Target);
            AnimatorTrigger.SetPhase(phase);
            AnimatorController.Play(phase.AnimationCashName);
        }

        protected IEnumerator WaitAnimatorFinished()
        {
            _waitingAnim = true;
            _onAnimFinished = () => _waitingAnim = false;

            // AnimatorController.Finished += _onAnimFinished;
            // AnimatorController.Finished -= _onAnimFinished;

            _onAnimFinished = null;
            _waitingAnim = false;
            
            yield return new WaitWhile(() => _waitingAnim);
            
        }

        protected void FinishInternal()
        {
            OnFinished();
            Finished?.Invoke(this);
        }

        protected virtual void OnFinished()
        {
            AnimatorController.Play(Constants.BaseAnimations.Idle);
        }
    }
}
