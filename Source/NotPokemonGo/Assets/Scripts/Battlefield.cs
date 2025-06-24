using Platoons;
using Statuses;
using Statuses.Services;

public class Battlefield
{
    private readonly Platoon _enemyPlatoon;
    private readonly Platoon _platoon2;
    private readonly IStatusManager _statusManager;

    public Battlefield(Platoon enemyPlatoon, Platoon platoon2, IStatusManager statusManager)
    {
        _statusManager = statusManager;
        _enemyPlatoon = enemyPlatoon;
        _platoon2 = platoon2;
    }

    public void Tick(float deltaTime)
    {
        _enemyPlatoon.Tick(deltaTime);
        _platoon2.Tick(deltaTime);
        
        _enemyPlatoon.Attack(_platoon2.Units);
        //_platoon2.Attack(_platoon1.Units);
        
        _statusManager.Update(deltaTime);
        _statusManager.RemoveInactive();
    }
}