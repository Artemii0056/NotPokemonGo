using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.AbilitySteps;
using Abilities.MV;
using Infrastructure;
using Services;
using Units;
using UnityEngine;

namespace Abilities.Bennet
{
	public class GenericAbilityHandler  : IAbilityHandler
	{
		private readonly ICoroutineRunner _runner;
		private readonly IAbilityStepExecutor _executor;
		private readonly List<AbilityStepData> _steps;
		private readonly ExecutionContext _context = new();

		private Coroutine _routine;
		private Unit _source;
		private Unit _target;

		public event Action<IAbilityHandler> Finished;

		public GenericAbilityHandler(
			ICoroutineRunner runner,
			AbilityModel model,
			IAbilityStepExecutor executor)
		{
			_runner = runner;
			_executor = executor;
			_steps = model.Steps;
		}

		public void Play(Unit source, Unit target)
		{
			_source = source;
			_target = target;

			_routine = _runner.StartCoroutine(Run());
		}

		public void Stop()
		{
			if (_routine != null)
				_runner.StopCoroutine(_routine);
		}

		private IEnumerator Run()
		{
			for (int i = 0; i < _steps.Count; i++)
			{
				AbilityStepData step = _steps[i];
				yield return _executor.ExecuteStep(step, _source, _target, _context);
			}

			_source.UnitAnimatorController.Play(Constants.BaseAnimations.Idle);
			Finished?.Invoke(this);
		}
	}
}