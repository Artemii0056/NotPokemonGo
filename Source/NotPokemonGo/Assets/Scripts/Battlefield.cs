using Platoons;
using Statuses;
using Statuses.Services;

public class Battlefield
{
    private readonly IStatusManager _statusManager;

    public Battlefield(Platoon enemyPlatoon, Platoon platoon2, IStatusManager statusManager)
    {
        _statusManager = statusManager;
        EnemyPlatoon = enemyPlatoon;
        Platoon2 = platoon2;
    }
    
    public Platoon EnemyPlatoon { get; private set; }
    public Platoon Platoon2 { get; private set; }

    public void Tick(float deltaTime)
    {
        EnemyPlatoon.Tick(deltaTime);
        Platoon2.Tick(deltaTime);
        
        //_enemyPlatoon.Attack(_platoon2.Units);
        //_platoon2.Attack(_platoon1.Units);
        
        _statusManager.Update(deltaTime);
        _statusManager.RemoveInactive();
    }
}