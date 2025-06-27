using System;
using System.Collections.Generic;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using Statuses.Services;
using Units;

public class Battlefield
{
    private readonly IStatusManager _statusManager;
    private readonly IBattleStateMachine _battleStateMachine;
    private readonly BattleUnitContainer _battleUnitContainer;

    public Battlefield(
        Platoon enemyPlatoon, 
        Platoon platoon2, 
        IStatusManager statusManager,
        IBattleStateMachine battleStateMachine)
    {
        _statusManager = statusManager;
        _battleStateMachine = battleStateMachine;
        EnemyPlatoon = enemyPlatoon;
        Platoon2 = platoon2;
        _battleUnitContainer = new BattleUnitContainer();
    }

    public Platoon EnemyPlatoon { get; private set; }
    public Platoon Platoon2 { get; private set; }

    public void Enable()
    {
        EnemyPlatoon.Enable();
        Platoon2.Enable();
        EnemyPlatoon.UnitPrepared += OnUnitPrepared;
        Platoon2.UnitPrepared += OnUnitPrepared;
    }

    public void Disable()
    {
        EnemyPlatoon.Disable();
        Platoon2.Disable();
        EnemyPlatoon.UnitPrepared -= OnUnitPrepared;
        Platoon2.UnitPrepared -= OnUnitPrepared;
    }

    private void OnUnitPrepared(Unit unit)
    {
        _battleUnitContainer.Add(unit);

        Unit sourceUnit = _battleUnitContainer.Give() ?? throw new Exception("BattleUnitContainer is empty");
        _battleStateMachine.Enter<UnitActionState, Unit>(sourceUnit);
    }

    public void Tick(float deltaTime)
    {
        EnemyPlatoon.Tick(deltaTime);
        Platoon2.Tick(deltaTime);

        _statusManager.Update(deltaTime);
        _statusManager.RemoveInactive();
    }
}

class BattleUnitContainer
{
    public List<Unit> Units = new List<Unit>();

    public void Add(Unit unit) => 
        Units.Add(unit);

    public Unit Give()
    {
        if (Units.Count > 0)
        {
            Unit unit = Units[0];
            Units.Remove(unit);
            return unit;
        }

        return null;
    }
}