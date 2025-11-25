using System;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Factories;
using Abilities.MV;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Units;

namespace Abilities
{
	public class AbilityService : IAbilityService
	{
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly IAbilityHandlerFactory _abilityHandlerFactory;

		private Battlefield _battlefield;

		private List<IAbilityHandler> _activeAbilityHandlers;

		private Counterattack _counterattack;

		private IAbilityHandler _abilityHandler;

		public event Action Finished;

		public AbilityService(IBattleStateMachine battleStateMachine, IAbilityHandlerFactory abilityHandlerFactory)
		{
			_battleStateMachine = battleStateMachine;
			_abilityHandlerFactory = abilityHandlerFactory;
			_activeAbilityHandlers = new List<IAbilityHandler>();
		}

		public void SetBattlefield(Battlefield battlefield)
		{
			_battlefield = battlefield;
		}

		public void Handle(Unit source, Unit target, AbilityModel abilityModel)
		{
			IAbilityHandler handler = _abilityHandlerFactory.Create(abilityModel);

			RegisterHandler(handler);
			handler.Play(source, target);
		}
		
		public void HandleCounterAttack(Unit source, Unit target, AbilityModel abilityModel)
		{
			StopAllHandlers();

			IAbilityHandler handler = _abilityHandlerFactory.Create(abilityModel);
			RegisterHandler(handler);
			handler.Play(source, target);
		}
		
		private void RegisterHandler(IAbilityHandler handler)
		{
			_activeAbilityHandlers.Add(handler);
			handler.Finished += OnHandlerFinished;
		}

		private void OnHandlerFinished(IAbilityHandler handler)
		{
			handler.Finished -= OnHandlerFinished;
			_activeAbilityHandlers.Remove(handler);

			_battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);

			Finished?.Invoke();
		}

		private void StopAllHandlers()
		{
			foreach (var handler in _activeAbilityHandlers)
			{
				handler.Finished -= OnHandlerFinished;
				handler.Stop();
			}

			_activeAbilityHandlers.Clear();
		}
	}
}