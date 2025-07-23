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
        Platoon heroesPlatoon,
        IStatusManager statusManager)
    {
        _statusManager = statusManager;
        EnemyPlatoon = enemyPlatoon;
        HeroesPlatoon = heroesPlatoon;
    }

    public Platoon EnemyPlatoon { get; private set; }
    public Platoon HeroesPlatoon { get; private set; }

    public void Enable()
    {
        EnemyPlatoon.Enable();
        HeroesPlatoon.Enable();

        HeroesPlatoon.UnitPrepared += OnUnitPrepared;
        EnemyPlatoon.UnitPrepared += OnUnitPrepared;
    }

    public void Disable()
    {
        EnemyPlatoon.Disable();
        HeroesPlatoon.Disable();

        HeroesPlatoon.UnitPrepared -= OnUnitPrepared;
        EnemyPlatoon.UnitPrepared -= OnUnitPrepared;
    }

    private void OnUnitPrepared(Unit unit)
    {
        Units.Add(unit);
    }

    public void Tick()
    {
        _statusManager.Tick();
        _statusManager.RemoveInactive();

        EnemyPlatoon.Tick();
        HeroesPlatoon.Tick();
    }
}