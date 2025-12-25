namespace Armaments.Spawner
{
    public interface IArmamentSpawner
    {
        public IArmamentMover Spawn(ArmamentContext context, out Armament armament);
    }
}