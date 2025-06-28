using System;
using System.Collections.Generic;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using Statuses.Services;
using Units;

public class Battlefield
{
    private readonly IStatusManager _statusManager;

    public List<Unit> Units = new List<Unit>();
    public Battlefield(
        Platoon enemyPlatoon, 
        Platoon platoon2, 
        IStatusManager statusManager)
    {
        _statusManager = statusManager;
        EnemyPlatoon = enemyPlatoon;
        Platoon2 = platoon2;
    }

    public Platoon EnemyPlatoon { get; private set; }
    public Platoon Platoon2 { get; private set; }

    public void Enable()
    {
        EnemyPlatoon.Enable();
        Platoon2.Enable();
        Platoon2.UnitPrepared += OnUnitPrepared;
        EnemyPlatoon.UnitPrepared += OnUnitPrepared;
    }

    public void Disable()
    {
        EnemyPlatoon.Disable();
        Platoon2.Disable();
        Platoon2.UnitPrepared -= OnUnitPrepared;
        EnemyPlatoon.UnitPrepared -= OnUnitPrepared;
    }

    private void OnUnitPrepared(Unit obj)
    {
        Units.Add(obj);
    }

    public void Tick(float deltaTime)
    {
        EnemyPlatoon.Tick(deltaTime);
        Platoon2.Tick(deltaTime);

        _statusManager.Update(deltaTime);
        _statusManager.RemoveInactive();
    }
}