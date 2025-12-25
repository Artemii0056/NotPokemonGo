namespace Armaments.Spawner
{
    public interface IArmamentLifecycle
    {
        IArmamentMover Register(Armament armament);
    }
}