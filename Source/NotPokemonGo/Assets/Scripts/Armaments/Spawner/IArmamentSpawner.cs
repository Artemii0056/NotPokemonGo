namespace Armaments.Spawner
{
    public interface IArmamentSpawner
    {
        IArmamentMover Create(ArmamentContext context);
    }
}