using Armaments;
using Armaments.Spawner;
using Stats;

namespace ReactionSystems
{
    public class ReflectFireballReaction : IReaction
    {
        private readonly IArmamentSpawner _armamentSpawner;

        public ReflectFireballReaction(IArmamentSpawner armamentSpawner) =>
            _armamentSpawner = armamentSpawner;

        public bool CanReact(ReactionContext context) =>
            context.Target.GetStat(StatType.DodgeFlag) > 0;

        public void React(ReactionContext context)
        {
            ArmamentContext armamentContext =
                new ArmamentContext(context.Target, context.Source, context.Armament.Setup);

            _armamentSpawner.Spawn(armamentContext, out Armament armament);
        }
    }
}