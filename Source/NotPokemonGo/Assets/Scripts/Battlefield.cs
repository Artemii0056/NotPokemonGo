using Platoons;
using Statuses.Services;

public class Battlefield
{
    private readonly IStatusManager _statusManager;

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

    public void Tick()
    {
        _statusManager.Tick();
        _statusManager.RemoveInactive();

        EnemyPlatoon.Tick();
        HeroesPlatoon.Tick();
    }
}