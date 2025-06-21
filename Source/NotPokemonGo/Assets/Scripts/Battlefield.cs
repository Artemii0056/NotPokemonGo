using DefaultNamespace;

public class Battlefield
{
    private Platoon _platoon1;
    private Platoon _platoon2;

    public Battlefield(Platoon platoon1, Platoon platoon2)
    {
        _platoon1 = platoon1;
        _platoon2 = platoon2;
    }

    public void Tick(float deltaTime)
    {
        _platoon1.Tick(deltaTime);
        _platoon2.Tick(deltaTime);
    }
}