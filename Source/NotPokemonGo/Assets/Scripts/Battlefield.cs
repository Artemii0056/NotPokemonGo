using System.Collections.Generic;
using Platoons;
using Statuses.Services;
using Units;

public class Battlefield
{
    private readonly IStatusManager _statusManager;

    public readonly List<Unit> Units = new List<Unit>();

    public Battlefield(
        Platoon enemyPlatoon,
        Platoon heroes,
        IStatusManager statusManager)
    {
        _statusManager = statusManager;
        EnemyPlatoon = enemyPlatoon;
        Heroes = heroes;
    }

    public Platoon EnemyPlatoon { get; private set; }
    public Platoon Heroes { get; private set; }

    public void Enable()
    {
        EnemyPlatoon.Enable();
        Heroes.Enable();
        Heroes.UnitPrepared += OnUnitPrepared;
        EnemyPlatoon.UnitPrepared += OnUnitPrepared;
    }

    public void Disable()
    {
        EnemyPlatoon.Disable();
        Heroes.Disable();
        Heroes.UnitPrepared -= OnUnitPrepared;
        EnemyPlatoon.UnitPrepared -= OnUnitPrepared;
    }

    private void OnUnitPrepared(Unit obj)
    {
        Units.Add(obj);
    }

    public void Tick()
    {
        _statusManager.Tick();
        _statusManager.RemoveInactive();
        
        EnemyPlatoon.Tick();
        Heroes.Tick();
    }
}