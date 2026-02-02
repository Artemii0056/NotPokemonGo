using Armaments;
using Armaments.Movers;
using Spawners.Spawner;
using Stats;

namespace ReactionSystems
{
    public class ReflectFireballReaction : IReaction
    {
        private readonly IArmamentSpawner _spawner;

        public ReflectFireballReaction(IArmamentSpawner spawner) => 
            _spawner = spawner;

        public bool CanReact(ReactionContext context) =>
            context.Target.GetStat(StatType.DodgeFlag) > 0;

        public void React(ReactionContext context)
        {
            ArmamentContext armamentContext =
                new ArmamentContext(context.Target, context.Source, context.Armament.Setup, ArmamentFlyingType.Direct);
            
            IArmamentMover mover = _spawner.Create(armamentContext);
            mover.Move();
        }
    }
}