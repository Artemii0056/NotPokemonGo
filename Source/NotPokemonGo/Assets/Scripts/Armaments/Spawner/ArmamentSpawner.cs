namespace Armaments.Spawner
{
    public class ArmamentSpawner : IArmamentSpawner
    {
        private readonly IArmamentViewFactory _viewFactory;
        private readonly IArmamentLifecycle _lifecycle;

        public ArmamentSpawner(IArmamentViewFactory viewFactory, IArmamentLifecycle lifecycle)
        {
            _viewFactory = viewFactory;
            _lifecycle = lifecycle;
        }

        public ArmamentMover Create(ArmamentContext context)
        {
            Armament armament = _viewFactory.Create(context);
            ArmamentMover mover = new ArmamentMover(armament);
            _lifecycle.Register(mover);

            return mover;
        }
    }
}