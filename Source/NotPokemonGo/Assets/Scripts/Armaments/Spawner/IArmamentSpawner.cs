namespace Armaments.Spawner
{
    public interface IArmamentSpawner
    {
        ArmamentMover Create(ArmamentContext context);
    }
}