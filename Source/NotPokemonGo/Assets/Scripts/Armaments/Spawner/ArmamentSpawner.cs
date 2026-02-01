namespace Armaments.Spawner
{
    public class ArmamentSpawner : IArmamentSpawner
    {
        private readonly IArmamentViewFactory _viewFactory;

        public ArmamentSpawner(IArmamentViewFactory viewFactory) => 
            _viewFactory = viewFactory;

        public IArmamentMover Create(ArmamentContext context)
        {
            Armament armament = _viewFactory.Create(context);
            
            IArmamentMover mover = new ArmamentMover(armament);

            return mover;
        }
    }
}