using DefaultNamespace;
using Statuses;

public class Battlefield
{
    private readonly Platoon _platoon1;
    private readonly Platoon _platoon2;
    private readonly IStatusManager _statusManager;

    public Battlefield(Platoon platoon1, Platoon platoon2, IStatusManager statusManager)
    {
        _statusManager = statusManager;
        _platoon1 = platoon1;
        _platoon2 = platoon2;
    }

    public void Tick(float deltaTime)
    {
        _platoon1.Tick(deltaTime);
        _platoon2.Tick(deltaTime);

        _statusManager.Update(deltaTime);
        _statusManager.RemoveInactive();
    }
}