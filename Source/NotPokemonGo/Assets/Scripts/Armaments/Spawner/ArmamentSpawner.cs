namespace Armaments.Spawner
{
    public class ArmamentSpawner : IArmamentSpawner
    {
        private readonly IArmamentLifecycle _lifecycle;
        private readonly IArmamentViewFactory _viewFactory;

        public ArmamentSpawner(
            IArmamentLifecycle lifecycle,
            IArmamentViewFactory viewFactory)
        {
            _lifecycle = lifecycle;
            _viewFactory = viewFactory;
        }

        public IArmamentMover Spawn(ArmamentContext context, out Armament armament)
        {
            armament = _viewFactory.Create(context);

            IArmamentMover armamentMover = _lifecycle.Register(armament);

            return armamentMover;
        }
    }
}