namespace Armaments.Spawner
{
    public interface IArmamentLifecycle
    {
        void Register(IArmamentMover armamentMover);
    }
}