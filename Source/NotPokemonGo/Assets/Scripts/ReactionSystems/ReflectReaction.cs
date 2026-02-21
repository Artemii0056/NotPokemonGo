using Armaments;
using Spawners.Spawner;
using Statuses;

namespace ReactionSystems
{
    public class ReflectReaction : IReaction
    {
        private readonly IArmamentSpawner _spawner;

        public ReflectReaction(IArmamentSpawner spawner) => 
            _spawner = spawner;

        public bool CanReact(ReactionContext context) => 
            context.Target.HasStatus(StatusType.Bubble);

        public void React(ReactionContext context)
        {
            context.WasReflected = true;

            var newContext = new ArmamentContext(
                context.Target,
                context.Source,
                context.Armament.Setup,
                context.Armament.FlyingType,
                context.Armament.transform);

            var mover = _spawner.Create(newContext);

            context.SpawnShot?.Invoke(mover);
        }
    }
}